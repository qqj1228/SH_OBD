﻿using System;
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
        private bool m_bAcceptVIN_TXT;

        public OBDStartForm() {
            InitializeComponent();
            this.Text += " Ver: " + MainFileVersion.AssemblyVersion;
            m_bCanOBDTest = true;
            m_lastHeight = this.Height;
            m_bAcceptVIN_TXT = true;
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
            // 删除WebService上传接口缓存dll
            string dllPath = ".\\" + m_obdInterface.DBandMES.WebServiceName + ".dll";
            try {
                if (File.Exists(dllPath)) {
                    File.Delete(dllPath);
                }
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Delete WebService dll file failed: " + ex.Message);
            }
            // 在OBDData表中新增Upload字段，用于存储上传是否成功的标志
            m_obdTest.m_db.AddUploadField();
            // 在OBDUser表中新增SN字段，用于存储检测报表编号中顺序号的特征字符串
            m_obdTest.m_db.AddSNField();
            // 定时上传以前上传失败的数据
            m_timer = new System.Timers.Timer(m_obdInterface.OBDResultSetting.UploadInterval * 60 * 1000);
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimeUpload);
            m_timer.AutoReset = true;
            m_timer.Enabled = true;

            Task.Factory.StartNew(TestNativeDatabase);
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
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Black;
                    this.labelResult.Text = "OBD检测中。。。";
                });
            }
        }

        void OnSetupColumnsDone() {
            if (!m_obdTest.AdvanceMode) {
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Black;
                    this.labelResult.Text = "正在处理结果。。。";
                });
            }
        }

        void OnWriteDbStart() { }

        void OnWriteDbDone() { }

        void OnUploadDataStart() { }

        void OnUploadDataDone() { }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            if (!m_bCanOBDTest) {
                return;
            }
            m_bCanOBDTest = false;
            // 跨UI线程调用UI控件要使用Invoke
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.Text = Encoding.Default.GetString(bits).Trim().ToUpper();
            });
            if (this.txtBoxVIN.Text.Length == 17) {
                m_obdTest.StrVIN_IN = this.txtBoxVIN.Text;
                m_obdInterface.m_log.TraceInfo("Get VIN: " + m_obdTest.StrVIN_IN + " by serial port scanner");
                if (!m_obdTest.AdvanceMode) {
                    StartOBDTest();
                }
            }
            m_bCanOBDTest = true;
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

            if (m_obdInterface.CommSettings.DoInitialization) {
                m_obdInterface.GetLogger().TraceInfo("   Initialize: YES");
            } else {
                m_obdInterface.GetLogger().TraceInfo("   Initialize: NO");
            }
        }

        private bool ConnectOBD() {
            LogCommSettingInfo();
            if (m_obdInterface.CommSettings.AutoDetect) {
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
                if (m_obdInterface.InitDevice(m_obdInterface.CommSettings.HardwareIndex, comPort, baudRate, m_obdInterface.CommSettings.ProtocolIndex, false)) {
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
            if (!ConnectOBD()) {
                this.Invoke((EventHandler)delegate {
                    this.labelResult.ForeColor = Color.Red;
                    this.labelResult.Text = "连接车辆失败！";
                });
                return;
            }
            string errorMsg = "";
            try {
                m_obdTest.StartOBDTest(out errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("OBD test occurred error: " + errorMsg + ", " + ex.Message);
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
                    this.labelResult.Text = "被检车辆: " + m_obdTest.StrVIN_ECU + "\nOBD检测结果：不合格";
                }
            });
            if (m_obdTest.CALIDCVNAllEmpty) {
                MessageBox.Show("CALID和CVN均为空！请检查OBD线缆接头连接是否牢固。", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                return;
            }
            m_bCanOBDTest = false;
            if (!m_obdInterface.CommSettings.UseSerialScanner && this.txtBoxVIN.Text.Trim().Length == 17 && m_bAcceptVIN_TXT) {
                m_bAcceptVIN_TXT = false;
                this.txtBoxVIN.Text = this.txtBoxVIN.Text.Trim().ToUpper();
                m_obdTest.StrVIN_IN = this.txtBoxVIN.Text;
                m_obdInterface.m_log.TraceInfo("Get VIN: " + m_obdTest.StrVIN_IN);
                if (!m_obdTest.AdvanceMode) {
                    StartOBDTest();
                }
                this.txtBoxVIN.SelectAll();
            }
            m_bAcceptVIN_TXT = true;
            m_bCanOBDTest = true;
        }

        private void OBDStartForm_Activated(object sender, EventArgs e) {
            this.txtBoxVIN.Focus();
        }

    }
}
