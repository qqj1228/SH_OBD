using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class OBDStartForm : Form {
        public static bool m_bCanOBDTest;
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;
        private MainForm f_MainForm;
        private readonly Color m_backColor;
        private float m_lastHeight;
        readonly System.Timers.Timer m_timer;
        CancellationTokenSource m_ctsOBDTestStart;
        CancellationTokenSource m_ctsSetupColumnsDone;
        CancellationTokenSource m_ctsWriteDbStart;
        CancellationTokenSource m_ctsUploadDataStart;

        public OBDStartForm() {
            InitializeComponent();
            this.Text += " Ver: " + MainFileVersion.AssemblyVersion;
            m_bCanOBDTest = true;
            m_lastHeight = this.Height;
            m_obdInterface = new OBDInterface();
            if (m_obdInterface.StrLoadConfigResult.Length > 0) {
                m_obdInterface.StrLoadConfigResult += "是否要以默认配置运行程序？点击\"否\"：将会退出程序。";
                DialogResult result = MessageBox.Show(m_obdInterface.StrLoadConfigResult, "加载配置文件出错", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) {
                    Environment.Exit(0);
                }
            }
            m_obdTest = new OBDTest(m_obdInterface);
            m_backColor = label1.BackColor;
            if (m_obdInterface.ScannerPortOpened) {
                m_obdInterface.m_sp.DataReceived += new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
            }
            m_obdTest.OBDTestStart += new Action(OnOBDTestStart);
            m_obdTest.SetupColumnsDone += new Action(OnSetupColumnsDone);
            m_obdTest.WriteDbStart += new Action(OnWriteDbStart);
            m_obdTest.WriteDbDone += new Action(OnWriteDbDone);
            m_obdTest.UploadDataStart += new Action(OnUploadDataStart);
            m_obdTest.UploadDataDone += new Action(OnUploadDataDone);
#if !DEBUG
            // 删除WebService上传接口缓存dll
            string dllPath = ".\\" + m_obdInterface.DBandMES.WebServiceName + ".dll";
            try {
                if (File.Exists(dllPath)) {
                    File.Delete(dllPath);
                }
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Delete WebService dll file failed: " + ex.Message);
            }
#endif
            Task.Factory.StartNew(TestNativeDatabase);
            // 在OBDData表中新增Upload字段，用于存储上传是否成功的标志
            Task.Factory.StartNew(m_obdTest.m_db.AddUploadField);
            // 在OBDUser表中新增SN字段，用于存储检测报表编号中顺序号的特征字符串
            Task.Factory.StartNew(m_obdTest.m_db.AddSNField);
            // 新增OBDProtocol表，用于存储车型OBD协议数据
            //Task.Factory.StartNew(m_obdTest.m_db.AddOBDProtocol);
            // 定时上传以前上传失败的数据
            m_timer = new System.Timers.Timer(m_obdInterface.OBDResultSetting.UploadInterval * 60 * 1000);
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimeUpload);
            m_timer.AutoReset = true;
            m_timer.Enabled = true;
        }

        private void TestNativeDatabase() {
            try {
                m_obdTest.m_db.ShowDB("OBDUser");
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Access native database failed: " + ex.Message);
                MessageBox.Show("检测到数据库通讯异常，请排查相关故障：\n" + ex.Message, "数据库通讯异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnTimeUpload(object source, System.Timers.ElapsedEventArgs e) {
            m_obdInterface.m_log.TraceInfo("Start UploadDataFromDBOnTime");
            try {
                m_obdTest.UploadDataFromDBOnTime(out string errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("UploadDataFromDBOnTime fialed：" + ex.Message);
            }

        }

        void OnOBDTestStart() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsOBDTestStart = UpdateUITask("开始OBD检测");
            }
        }

        void OnSetupColumnsDone() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsOBDTestStart.Cancel();
                m_ctsSetupColumnsDone = UpdateUITask("正在读取车辆信息");
            }
        }

        void OnWriteDbStart() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsSetupColumnsDone.Cancel();
                m_ctsWriteDbStart = UpdateUITask("正在写入本地数据库");
            }
        }

        void OnWriteDbDone() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsWriteDbStart.Cancel();
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Black;
                    this.labelResult.Text = "写入本地数据库结束";
                });
            }
        }

        void OnUploadDataStart() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsUploadDataStart = UpdateUITask("正在上传数据");
            }
        }

        void OnUploadDataDone() {
            if (!m_obdTest.AdvanceMode) {
                m_ctsUploadDataStart.Cancel();
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Black;
                    this.labelResult.Text = "上传数据结束";
                });
            }
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            // 该处接收串口扫码枪传进来的VIN号代码没有考虑串口数据断包问题
            // 获取VIN号有隐患，需要串口扫码枪在VIN号结尾加个回车表示结束
            // 但是现场使用的是USB接口的扫码枪，VIN号结尾也没有加回车，故暂时不需要处理这个问题
            if (!m_bCanOBDTest) {
                if (Encoding.Default.GetString(bits).Trim().ToUpper().Length == 17) {
                    this.Invoke((EventHandler)delegate {
                        this.txtBoxVIN.SelectAll();
                        MessageBox.Show("上一辆车还未完全结束检测过程，请稍后再试", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    });
                }
                return;
            }
            string strTxt = Encoding.Default.GetString(bits).Trim().ToUpper();
            // 跨UI线程调用UI控件要使用Invoke
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.Text = strTxt;
            });
            if (strTxt.Length == 17) {
                m_bCanOBDTest = false;
                m_obdTest.StrVIN_IN = strTxt;
                m_obdInterface.m_log.TraceInfo("Get VIN: " + m_obdTest.StrVIN_IN + " by serial port scanner");
                if (!m_obdTest.AdvanceMode) {
                    Task.Factory.StartNew(StartOBDTest);
                }
            }
        }

        private void LogCommSettingInfo() {
            m_obdInterface.GetLogger().TraceInfo("Connection Procedure Initiated");

            if (m_obdInterface.CommSettings.AutoDetect) {
                m_obdInterface.GetLogger().TraceInfo("   Automatic Hardware Detection: ON");
            } else {
                m_obdInterface.GetLogger().TraceInfo("   Automatic Hardware Detection: OFF");
            }

            m_obdInterface.GetLogger().TraceInfo(string.Format("   Baud Rate: {0}", m_obdInterface.CommSettings.BaudRate));
            m_obdInterface.GetLogger().TraceInfo(string.Format("   Default Port: {0}", m_obdInterface.CommSettings.ComPortName));

            switch (m_obdInterface.CommSettings.HardwareIndex) {
            case HardwareType.Automatic:
                m_obdInterface.GetLogger().TraceInfo("   Interface: Auto-Detect");
                break;
            case HardwareType.ELM327:
                m_obdInterface.GetLogger().TraceInfo("   Interface: ELM327");
                break;
            case HardwareType.ELM320:
                m_obdInterface.GetLogger().TraceInfo("   Interface: ELM320");
                break;
            case HardwareType.ELM322:
                m_obdInterface.GetLogger().TraceInfo("   Interface: ELM322");
                break;
            case HardwareType.ELM323:
                m_obdInterface.GetLogger().TraceInfo("   Interface: ELM323");
                break;
            case HardwareType.CANtact:
                m_obdInterface.GetLogger().TraceInfo("   Interface: CANtact");
                break;
            default:
                m_obdInterface.GetLogger().TraceInfo("Bad hardware type.");
                throw new Exception("Bad hardware type.");
            }

            m_obdInterface.GetLogger().TraceInfo(string.Format("   Protocol: {0}", m_obdInterface.CommSettings.ProtocolName));
            m_obdInterface.GetLogger().TraceInfo(string.Format("   Application Layer Protocol: {0}", m_obdInterface.CommSettings.StandardName));

            if (m_obdInterface.CommSettings.DoInitialization) {
                m_obdInterface.GetLogger().TraceInfo("   Initialize: YES");
            } else {
                m_obdInterface.GetLogger().TraceInfo("   Initialize: NO");
            }
        }

        private bool ConnectOBD() {
            LogCommSettingInfo();
            if (m_obdInterface.CommSettings.AutoDetect) {
                if (m_obdInterface.OBDResultSetting.SpecifiedProtocol) {
                    try {
                        if (!m_obdInterface.SetXAttrByVIN(m_obdTest.StrVIN_IN)) {
                            MessageBox.Show("从SAP获取OBD协议失败。将会自动探测OBD协议。", "获取OBD协议失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } catch (Exception ex) {
                        m_obdInterface.GetLogger().TraceError("Failed to get protocol by VIN, Reason: " + ex.Message);
                        MessageBox.Show("从SAP获取OBD协议发生异常。将会自动探测OBD协议。\r\n" + ex.Message, "获取OBD协议失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (m_obdInterface.InitDeviceAuto(false)) {
                    m_obdInterface.GetLogger().TraceInfo("Connection Established!");
                } else {
                    m_obdInterface.GetLogger().TraceWarning("Failed to find a compatible OBD-II interface.");
                    MessageBox.Show("无法找到与本机相连的兼容的OBD-II硬件设备。\r\n请确认没有其他软件正在使用所需端口。", "自动探测失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_obdInterface.Disconnect();
                    return false;
                }
            } else {
                int baudRate = m_obdInterface.CommSettings.BaudRate;
                int comPort = m_obdInterface.CommSettings.ComPort;
                if (m_obdInterface.InitDevice(m_obdInterface.CommSettings.HardwareIndex, comPort, baudRate, m_obdInterface.CommSettings.ProtocolIndex, m_obdInterface.CommSettings.StandardIndex, false)) {
                    m_obdInterface.GetLogger().TraceInfo("Connection Established!");
                } else {
                    m_obdInterface.GetLogger().TraceWarning("Failed to find a compatible OBD-II interface.");
                    MessageBox.Show(
                        string.Format("在 \"端口：{0}，波特率：{1}\" 通讯设置下，无法找到兼容的OBD-II协议设备。\r\n请确认没有其他软件正在使用所需端口且波特率设置正确。",
                            m_obdInterface.CommSettings.ComPortName,
                            m_obdInterface.CommSettings.BaudRate
                        ),
                        "连接失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                        );
                    m_obdInterface.Disconnect();
                    return false;
                }
            }
            return true;
        }

        private void StartOBDTest() {
            this.Invoke((EventHandler)delegate {
                this.labelResult.ForeColor = Color.Black;
                this.labelResult.Text = "准备OBD检测";
                this.labelVINError.BackColor = m_backColor;
                this.labelVINError.ForeColor = Color.Gray;
                this.labelCALIDCVN.BackColor = m_backColor;
                this.labelCALIDCVN.ForeColor = Color.Gray;
                this.label3Space.BackColor = m_backColor;
                this.label3Space.ForeColor = Color.Gray;
            });
            m_obdInterface.m_log.TraceInfo(">>>>>>>>>> Start to test vehicle of VIN: " + m_obdTest.StrVIN_IN + " <<<<<<<<<<");
            if (m_obdInterface.ConnectedStatus) {
                m_obdInterface.Disconnect();
            }
            this.Invoke((EventHandler)delegate {
                this.labelResult.ForeColor = Color.Black;
                this.labelResult.Text = "正在连接车辆。。。";
            });
            CancellationTokenSource tokenSource = UpdateUITask("正在连接车辆");
            if (!ConnectOBD()) {
                tokenSource.Cancel();
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Red;
                    this.labelResult.Text = "连接车辆失败！";
                });
                m_bCanOBDTest = true;
                return;
            }
            tokenSource.Cancel();

            string errorMsg = "";
            int VINCount = 0;
            bool bNoTestRecord = false;
            bool bTestException = false;
            try {
                m_obdTest.StartOBDTest(out errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif

                // 江铃股份操作工反应会有少量车辆漏检，故加入二次检查被测车辆是否已经检测过
                Dictionary<string, string> whereDic = new Dictionary<string, string> { { "VIN", m_obdTest.StrVIN_ECU } };
                VINCount = m_obdTest.m_db.GetRecordCount("OBDData", whereDic);
                if (VINCount == 0) {
                    m_obdInterface.m_log.TraceError("No test record of this vehicle: " + m_obdTest.StrVIN_ECU);
                    m_obdTest.OBDResult = false;
                    bNoTestRecord = true;
                }
            } catch (Exception ex) {
                if (m_obdTest.StrVIN_ECU == null || m_obdTest.StrVIN_ECU.Length == 0) {
                    m_obdTest.StrVIN_ECU = m_obdTest.StrVIN_IN;
                }
                m_obdInterface.m_log.TraceError("OBD test occurred error: " + errorMsg + ", " + ex.Message);
                bTestException = true;
                MessageBox.Show(errorMsg + "\n" + ex.Message, "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Invoke((EventHandler)delegate {
                if (m_obdTest.OBDResult) {
                    this.labelResult.ForeColor = Color.GreenYellow;
                    this.labelResult.Text = "被检车辆: " + m_obdTest.StrVIN_ECU + "\nOBD检测结果：合格";
                    this.txtBoxVIN.Text = "";
                } else {
                    if (!m_obdTest.VINResult) {
                        this.labelVINError.BackColor = Color.Red;
                        this.labelVINError.ForeColor = Color.Black;
                    }
                    if (!m_obdTest.CALIDCVNResult || !m_obdTest.CALIDUnmeaningResult) {
                        this.labelCALIDCVN.BackColor = Color.Red;
                        this.labelCALIDCVN.ForeColor = Color.Black;
                    }
                    if (!m_obdTest.OBDSUPResult) {
                        this.label3Space.BackColor = Color.Red;
                        this.label3Space.ForeColor = Color.Black;
                    }

                    this.labelResult.ForeColor = Color.Red;
                    if (bNoTestRecord) {
                        this.labelResult.Text = "被检车辆: " + m_obdTest.StrVIN_ECU + "\n没有本地检测记录";
                    } else if (bTestException) {
                        this.labelResult.Text = "被检车辆: " + m_obdTest.StrVIN_ECU + "\nOBD检测过程发生异常";
                    } else {
                        this.labelResult.Text = "被检车辆: " + m_obdTest.StrVIN_ECU + "\nOBD检测结果：不合格";
                    }
                }
            });
            if (m_obdTest.CALIDCVNAllEmpty) {
                MessageBox.Show("CALID和CVN均为空！请检查OBD线缆接头连接是否牢固。", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            m_obdTest.StrVIN_IN = "";
            m_obdTest.StrVIN_ECU = "";
            if (m_ctsOBDTestStart != null) {
                m_ctsOBDTestStart.Cancel();
            }
            if (m_ctsSetupColumnsDone != null) {
                m_ctsSetupColumnsDone.Cancel();
            }
            if (m_ctsWriteDbStart != null) {
                m_ctsWriteDbStart.Cancel();
            }
            if (m_ctsUploadDataStart != null) {
                m_ctsUploadDataStart.Cancel();
            }
            m_bCanOBDTest = true;
        }

        private CancellationTokenSource UpdateUITask(string strMsg) {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Task.Factory.StartNew(() => {
                int count = 0;
                while (!token.IsCancellationRequested) {
                    try {
                        this.Invoke((EventHandler)delegate {
                            this.labelResult.ForeColor = Color.Black;
                            if (count == 0) {
                                this.labelResult.Text = strMsg + "。。。";
                            } else {
                                this.labelResult.Text = strMsg + "，用时" + count.ToString() + "s";
                            }
                        });
                    } catch (ObjectDisposedException ex) {
                        m_obdInterface.m_log.TraceWarning(ex.Message);
                    }
                    Thread.Sleep(1000);
                    ++count;
                }
            }, token);
            return tokenSource;
        }

        private void ResizeFont(Control control, float scale) {
            control.Font = new Font(control.Font.FontFamily, control.Font.Size * scale, control.Font.Style);
        }

        private void OBDStartForm_Resize(object sender, EventArgs e) {
            if (m_lastHeight == 0) {
                return;
            }
            float scale = this.Height / m_lastHeight;
            ResizeFont(this.txtBoxVIN, scale);
            ResizeFont(this.label1, scale);
            ResizeFont(this.labelResult, scale);
            ResizeFont(this.labelVINError, scale);
            ResizeFont(this.labelCALIDCVN, scale);
            ResizeFont(this.label3Space, scale);
            ResizeFont(this.btnAdvanceMode, scale);
            m_lastHeight = this.Height;
        }

        private void BtnAdvanceMode_Click(object sender, EventArgs e) {
            m_obdTest.AccessAdvanceMode = 0;
            PassWordForm passWordForm = new PassWordForm(m_obdTest);
            passWordForm.ShowDialog();
            if (m_obdTest.AccessAdvanceMode > 0) {
                m_obdTest.AdvanceMode = true;
                f_MainForm = new MainForm(m_obdInterface, m_obdTest);
                f_MainForm.Show();
            } else if (m_obdTest.AccessAdvanceMode < 0) {
                MessageBox.Show("密码错误！", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtBoxVIN.Focus();
            } else {
                this.txtBoxVIN.Focus();
            }
            passWordForm.Dispose();
        }

        private void OBDStartForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (f_MainForm != null) {
                f_MainForm.Close();
            }
            if (m_timer != null) {
                m_timer.Dispose();
            }

            Monitor.Enter(m_obdInterface);
            if (m_obdInterface.ConnectedStatus) {
                m_obdInterface.Disconnect();
            }
            Monitor.Exit(m_obdInterface);
        }

        private void OBDStartForm_Load(object sender, EventArgs e) {
            this.labelResult.ForeColor = Color.Black;
            this.labelResult.Text = "准备OBD检测";
            this.txtBoxVIN.Focus();
            this.labelVINError.ForeColor = Color.Gray;
            this.labelCALIDCVN.ForeColor = Color.Gray;
            this.label3Space.ForeColor = Color.Gray;
        }

        private void TxtBoxVIN_TextChanged(object sender, EventArgs e) {
            if (!m_bCanOBDTest) {
                if (!m_obdInterface.CommSettings.UseSerialScanner && this.txtBoxVIN.Text.Trim().Length == 17) {
                    this.txtBoxVIN.SelectAll();
                    MessageBox.Show("上一辆车还未完全结束检测过程，请稍后再试", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            string strTxt = this.txtBoxVIN.Text.Trim();
            if (!m_obdInterface.CommSettings.UseSerialScanner && strTxt.Length == 17) {
                m_bCanOBDTest = false;
                m_obdTest.StrVIN_IN = strTxt;
                m_obdInterface.m_log.TraceInfo("Get VIN: " + m_obdTest.StrVIN_IN);
                if (!m_obdTest.AdvanceMode) {
                    Task.Factory.StartNew(StartOBDTest);
                }
                this.txtBoxVIN.SelectAll();
            }
        }

        private void OBDStartForm_Activated(object sender, EventArgs e) {
            this.txtBoxVIN.Focus();
        }

        private void MenuItemStat_Click(object sender, EventArgs e) {
            StatisticForm form = new StatisticForm(m_obdTest);
            form.ShowDialog();
            form.Dispose();
        }
    }
}
