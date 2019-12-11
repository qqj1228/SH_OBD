using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SH_OBD {
    public enum EnumLogLevel {
        LogLevelAll,    //所有信息都写日志
        LogLevelMid,    //写错误、警告信息
        LogLevelNormal, //只写错误信息
        LogLevelStop    //不写日志
    };

    /// <summary>
    /// 线程安全简易日志类
    /// IsTraceLineNum控制是否由程序自动输出行号等信息，其依赖于是否存在pdb调试信息文件
    /// 没有pdb文件的话，只能手工写在代码里
    /// </summary>
    public class Logger {
        EnumLogLevel LogLevel { get; set; }
        bool IsTraceLineNum { get; set; }
        string StrLogPath { get; set; }
        string StrLogName { get; set; }
        object ObjLock { get; set; }
        Queue<string> FileQueue { get; set; }
        int MaxFileQty { get; set; }

        public Logger() {
            this.LogLevel = EnumLogLevel.LogLevelNormal;
            this.IsTraceLineNum = true;
            this.ObjLock = new object();
            this.StrLogPath = ".";
            this.MaxFileQty = 0;
            FileQueue = new Queue<string>();
            GenFileQueue();
        }

        public Logger(string strLogPath, EnumLogLevel LogLevel, bool IsTraceLineNum, int maxFileQty) {
            this.LogLevel = LogLevel;
            this.IsTraceLineNum = IsTraceLineNum;
            this.StrLogPath = strLogPath;
            this.ObjLock = new object();
            this.MaxFileQty = maxFileQty;
            FileQueue = new Queue<string>();
            CreateLogPath();
            GenFileQueue();
        }

        public void TraceFatal(string strLog) {
            if (strLog.Length == 0) {
                return;
            }
            string strLineHead = GetLineHead("[XFatalX]");
            Trace(strLineHead + strLog);
        }

        public void TraceError(string strLog) {
            if (strLog.Length == 0 || LogLevel >= EnumLogLevel.LogLevelStop) {
                return;
            }
            string strLineHead = GetLineHead("[XErrorX]");
            Trace(strLineHead + strLog);
        }

        public void TraceWarning(string strLog) {
            if (strLog.Length == 0 || LogLevel >= EnumLogLevel.LogLevelNormal) {
                return;
            }
            string strLineHead = GetLineHead("[Warning]");
            Trace(strLineHead + strLog);
        }

        public void TraceInfo(string strLog) {
            if (strLog.Length == 0 || LogLevel >= EnumLogLevel.LogLevelMid) {
                return;
            }
            string strLineHead = GetLineHead("[ Info  ]");
            Trace(strLineHead + strLog);
        }

        /// <summary>
        /// 生成log文件名
        /// </summary>
        /// <returns>
        /// 若生成新的文件名的话返回true，否则没有生成新的文件名的话返回false
        /// </returns>
        bool GenerateLogName() {
            string strTemp = DateTime.Now.Date.ToString("yyyy-MM-dd") + ".log";
            if (StrLogName != strTemp) {
                StrLogName = strTemp;
                return true;
            }
            return false;
        }

        void CreateLogPath() {
            if (!Directory.Exists(StrLogPath)) {
                Directory.CreateDirectory(StrLogPath);
            }
        }

        string GetLineHead(string strInfo) {
            string strLineHead;
            if (IsTraceLineNum) {
                StackTrace st = new StackTrace(2, true);
                StackFrame sf = st.GetFrame(0);
                strLineHead = string.Format(
                    "{0}-{1} {2} {3}({4})<{5}>: ",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToString("HH:mm:ss.fff"),
                    strInfo,
                    Path.GetFileName(sf.GetFileName()),
                    sf.GetFileLineNumber(),
                    sf.GetMethod().Name
                    );
            } else {
                strLineHead = string.Format("{0}-{1} {2}: ", DateTime.Now.ToShortDateString(), DateTime.Now.ToString("HH:mm:ss.fff"), strInfo);
            }
            return strLineHead;
        }

        void Trace(string strLog) {
            if (strLog.Length == 0) {
                return;
            }
            lock (ObjLock) {
                try {
                    if (GenerateLogName()) {
                        UpdateFileQueue();
                        FileQueue.Enqueue(StrLogPath + "\\" + StrLogName);
                    }
                    StreamWriter streamWriter = File.AppendText(StrLogPath + "\\" + StrLogName);
                    streamWriter.WriteLine(strLog);
                    streamWriter.Flush();
                    streamWriter.Close();
                } catch (Exception e) {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }

        void GenFileQueue() {
            DirectoryInfo dirinfo = new DirectoryInfo(StrLogPath);
            FileInfo[] Files = dirinfo.GetFiles();
            // 递增排序
            Array.Sort<FileInfo>(Files, (FileInfo x, FileInfo y) => { return x.LastWriteTime.CompareTo(y.LastWriteTime); });
            // 递减排序
            //Array.Sort<FileInfo>(Files, (FileInfo x, FileInfo y) => { return y.LastWriteTime.CompareTo(x.LastWriteTime); });
            foreach (var item in Files) {
                FileQueue.Enqueue(StrLogPath + "\\" + item.Name);
            }
        }

        void UpdateFileQueue() {
            if (MaxFileQty > 0) {
                int qty = FileQueue.Count - MaxFileQty + 1;
                if (qty > 0) {
                    for (int i = 0; i < qty; i++) {
                        File.Delete(FileQueue.Dequeue());
                    }
                }
            }
        }
    }
}
