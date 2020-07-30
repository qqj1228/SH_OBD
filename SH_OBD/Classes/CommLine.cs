using System;
using System.IO;
using System.Text;
using System.Threading;

namespace SH_OBD {
    public abstract class CommLine : CommBase {
        private const int BUFF_SIZE = 512;
        private static readonly object locker = new object();
        private int m_RxIndex = 0;
        private string m_RxString = "";
        private readonly ManualResetEvent m_TransFlag = new ManualResetEvent(true);
        private byte[] m_RxBuffer;
        private byte m_RxTerm;
        private byte[] m_TxTerm;
        private byte[] m_RxFilter;
        private string m_RxLine; // 单独接收到的ELM327发来的消息
        private bool m_bReceivedMsg = false; // 表示给ELM327发送命令后是否收到了返回数据
        private bool m_bRxEnd = false; // 表示已把串口返回的一包数据处理完毕
        protected int TransTimeout { get; set; }

        protected CommLine(Logger log) : base(log) { }

        protected void SetRxFilter(byte[] RxFilter) {
            m_RxFilter = RxFilter;
        }

        protected void Send(string data) {
            int len_data = Encoding.ASCII.GetByteCount(data);

            int len_term = 0;
            if (m_TxTerm != null) {
                len_term = m_TxTerm.Length;
            }

            byte[] sending = new byte[len_data + len_term];
            Encoding.ASCII.GetBytes(data).CopyTo(sending, 0);

            if (m_TxTerm != null) {
                m_TxTerm.CopyTo(sending, len_data);
            }
            base.Send(sending);
        }

        protected string Transact(string data) {
            m_RxString = "";
            Send(data);
            m_bReceivedMsg = false;
            m_TransFlag.Reset();
            if (!m_TransFlag.WaitOne(TransTimeout, false)) {
                if (!m_bReceivedMsg) {
                    ThrowException("Timeout");
                }
                while (!m_bRxEnd) {
                    Thread.Sleep(10);
                }
            }
            lock (locker) {
                m_RxLine = "";
                return m_RxString;
            }
        }

        protected void Setup(CommLine.CommLineSettings settings) {
            m_RxBuffer = new byte[settings.RxStringBufferSize];
            m_RxTerm = settings.RxTerminator;
            m_RxFilter = settings.RxFilter;
            TransTimeout = settings.TransactTimeout;
            m_TxTerm = settings.TxTerminator;
        }

        /// <summary>
        /// 获取单独接收到的ELM327发来的消息，函数返回后会将单独接收缓冲区清空
        /// </summary>
        /// <returns></returns>
        public string GetRxLine() {
            string strRet = m_RxLine;
            m_RxLine = "";
            return strRet;
        }

        protected virtual void OnRxLine() {
            m_RxLine += m_RxString;
        }

        protected string StringFilter(string strOld) {
            char[] chars = strOld.ToCharArray();
            int offset = 0;
            char[] result = new char[chars.Length];
            bool bSkip;
            for (int i = 0; i < chars.Length; i++) {
                bSkip = false;
                foreach (byte item in m_RxFilter) {
                    if (chars[i] == item) {
                        bSkip = true;
                        break;
                    }
                }
                if (bSkip) {
                    continue;
                }
                result[offset] = chars[i];
                offset++;
            }
            return new string(result, 0, offset);
        }

        protected override void OnRxString(string strRx) {
            m_bReceivedMsg = true;
            int index;
            int startIndex = 0;
            string strRxFilter = strRx;
            if (m_RxFilter != null) {
                strRxFilter = StringFilter(strRxFilter);
            }
            m_bRxEnd = false;
            while (startIndex < strRxFilter.Length) {
                index = strRxFilter.IndexOf((char)m_RxTerm, startIndex);
                if (index > -1) {
                    lock (locker) {
                        m_RxString += strRxFilter.Substring(startIndex, index);
                    }
                    // 检测是否已经m_TransFlag.Set()了
                    if (m_TransFlag.WaitOne(0, false)) {
                        // 已经m_TransFlag.Set()了，表示是单独接收ELM327发来的消息，即startIndex > 0
                        OnRxLine();
                    } else {
                        // 没有m_TransFlag.Set()，表示是给ELM327发送命令后返回的消息，即startIndex == 0
                        m_TransFlag.Set();
                    }
                    startIndex = index + 1;
                } else {
                    m_RxString += strRxFilter.Substring(startIndex);
                    if (m_RxString.Length > BUFF_SIZE) {
                        m_RxString = m_RxString.Substring(0, BUFF_SIZE);
                        // 检测是否已经m_TransFlag.Set()了
                        if (m_TransFlag.WaitOne(0, false)) {
                            // 已经m_TransFlag.Set()了，表示是单独接收ELM327发来的消息，即startIndex > 0
                            OnRxLine();
                        } else {
                            // 没有m_TransFlag.Set()，表示是给ELM327发送命令后返回的消息，即startIndex == 0
                            m_TransFlag.Set();
                        }
                    }
                    break;
                }
            }
            m_bRxEnd = true;
        }

        protected override void OnRxChar(byte ch) {
            m_bReceivedMsg = true;
            m_bRxEnd = false;
            if (ch == m_RxTerm || m_RxIndex >= m_RxBuffer.Length) {
                lock (locker) {
                    m_RxString = Encoding.ASCII.GetString(m_RxBuffer, 0, m_RxIndex);
                }
                m_RxIndex = 0;
                m_bRxEnd = true;
                // 检测是否已经m_TransFlag.Set()了
                if (m_TransFlag.WaitOne(0, false)) {
                    // 已经m_TransFlag.Set()了，表示是单独接收ELM327发来的消息
                    OnRxLine();
                } else {
                    // 没有m_TransFlag.Set()，表示是给ELM327发送命令后返回的消息
                    m_TransFlag.Set();
                }
                m_TransFlag.Set();
            } else {
                if (m_RxFilter != null) {
                    for (int idx = 0; idx < m_RxFilter.Length; ++idx) {
                        if (m_RxFilter[idx] == ch) {
                            return;
                        }
                    }
                }
                m_RxBuffer[m_RxIndex] = ch;
                ++m_RxIndex;
            }
        }

        protected class CommLineSettings : CommBase.CommBaseSettings {
            public int RxStringBufferSize = 256;
            public byte RxTerminator = 0x0D;
            public int TransactTimeout = 1000;
            public byte[] RxFilter;
            public byte[] TxTerminator;
        }
    }
}
