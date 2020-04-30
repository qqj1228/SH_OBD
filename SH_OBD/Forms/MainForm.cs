using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class MainForm : Form {
        private Dictionary<string, Form> dicSubForms;
        private OBDTestForm f_OBDTest;
        private ProtocolForm f_Protocol;
        private TestForm f_MonitorTests;
        private DTCForm f_DTC;
        private FreezeFramesForm f_FreezeFrames;
        private OxygenSensorsForm f_OxygenSensors;
        private SensorGridForm f_SensorGrid;
        private SensorChartForm f_SensorChart;
        private TrackForm f_Track;
        private DynoForm f_Dyno;
        private FuelEconomyForm f_FuelEconomy;
        private ReportGeneratorForm f_Report;
        private TerminalForm f_Terminal;
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;
        private readonly Font m_boldFont;
        private readonly Font m_originFont;

        public MainForm(OBDInterface obd, OBDTest obdTest) {
            InitializeComponent();
            m_obdInterface = obd;
            m_obdTest = obdTest;
            m_obdInterface.OnConnect += new OBDInterface.__Delegate_OnConnect(On_OBD_Connect);
            m_obdInterface.OnDisconnect += new OBDInterface.__Delegate_OnDisconnect(On_OBD_Disconnect);

            m_originFont = buttonDefaultFontStyle.Font;
            m_boldFont = new Font(m_originFont, FontStyle.Bold);

            StatusLabelConnStatus.ForeColor = Color.Red;
            StatusLabelConnStatus.Text = "OBD通讯接口未连接";
            StatusLabelDeviceName.Text = "未获取到设备名";
            StatusLabelCommProtocol.Text = m_obdInterface.GetProtocol().ToString();
            StatusLabelDeviceType.Text = m_obdInterface.GetDevice().ToString().Replace("ELM327", "SH-VCI-302U");
            if (m_obdInterface.CommSettings != null) {
                if (m_obdInterface.CommSettings.AutoDetect) {
                    StatusLabelPort.Text = "自动探测";
                    StatusLabelAppProtocol.Text = "自动探测";
                } else {
                    StatusLabelPort.Text = m_obdInterface.CommSettings.ComPortName;
                    StatusLabelAppProtocol.Text = m_obdInterface.CommSettings.StandardName;
                }
            }

            InitSubForm();
            this.Text = "SH_OBD - Ver " + MainFileVersion.AssemblyVersion;
        }

        ~MainForm() { f_OBDTest.Close(); }

        void InitSubForm() {
            dicSubForms = new Dictionary<string, Form>();

            f_OBDTest = new OBDTestForm(m_obdInterface, m_obdTest);
            f_Protocol = new ProtocolForm(m_obdTest);
            f_MonitorTests = new TestForm(m_obdInterface);
            f_DTC = new DTCForm(m_obdInterface);
            f_FreezeFrames = new FreezeFramesForm(m_obdInterface);
            f_OxygenSensors = new OxygenSensorsForm(m_obdInterface);
            f_SensorGrid = new SensorGridForm(m_obdInterface);
            f_SensorChart = new SensorChartForm(m_obdInterface);
            f_Track = new TrackForm(m_obdInterface);
            f_Dyno = new DynoForm(m_obdInterface);
            f_FuelEconomy = new FuelEconomyForm(m_obdInterface);
            f_Report = new ReportGeneratorForm(m_obdInterface);
            f_Terminal = new TerminalForm(m_obdInterface);

            buttonOBDTest.Text = Properties.Resources.buttonName_OBDTest;
            buttonProtocol.Text = Properties.Resources.buttonName_Protocol;
            buttonTests.Text = Properties.Resources.buttonName_Tests;
            buttonDTC.Text = Properties.Resources.buttonName_DTC;
            buttonFF.Text = Properties.Resources.buttonName_FreezeFrames;
            buttonO2.Text = Properties.Resources.buttonName_OxygenSensors;
            buttonSensorGrid.Text = Properties.Resources.buttonName_SensorGrid;
            buttonSensorGraph.Text = Properties.Resources.buttonName_SensorChart;
            buttonTrack.Text = Properties.Resources.buttonName_Track;
            buttonDyno.Text = Properties.Resources.buttonName_Dyno;
            buttonFuel.Text = Properties.Resources.buttonName_FuelEconomy;
            buttonReport.Text = Properties.Resources.buttonName_Report;
            buttonTerminal.Text = Properties.Resources.buttonName_Terminal;

            dicSubForms.Add(Properties.Resources.buttonName_OBDTest, f_OBDTest);
            dicSubForms.Add(Properties.Resources.buttonName_Protocol, f_Protocol);
            dicSubForms.Add(Properties.Resources.buttonName_Tests, f_MonitorTests);
            dicSubForms.Add(Properties.Resources.buttonName_DTC, f_DTC);
            dicSubForms.Add(Properties.Resources.buttonName_FreezeFrames, f_FreezeFrames);
            dicSubForms.Add(Properties.Resources.buttonName_OxygenSensors, f_OxygenSensors);
            dicSubForms.Add(Properties.Resources.buttonName_SensorGrid, f_SensorGrid);
            dicSubForms.Add(Properties.Resources.buttonName_SensorChart, f_SensorChart);
            dicSubForms.Add(Properties.Resources.buttonName_Track, f_Track);
            dicSubForms.Add(Properties.Resources.buttonName_Dyno, f_Dyno);
            dicSubForms.Add(Properties.Resources.buttonName_FuelEconomy, f_FuelEconomy);
            dicSubForms.Add(Properties.Resources.buttonName_Report, f_Report);
            dicSubForms.Add(Properties.Resources.buttonName_Terminal, f_Terminal);
        }

        private void BroadcastConnectionUpdate() {
            foreach (var key in dicSubForms.Keys) {
                if (key == Properties.Resources.buttonName_DTC) {
                    (dicSubForms[key] as DTCForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_SensorGrid) {
                    (dicSubForms[key] as SensorGridForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_SensorChart) {
                    (dicSubForms[key] as SensorChartForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_Track) {
                    (dicSubForms[key] as TrackForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_Dyno) {
                    (dicSubForms[key] as DynoForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_FuelEconomy) {
                    (dicSubForms[key] as FuelEconomyForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_Terminal) {
                    (dicSubForms[key] as TerminalForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_OBDTest) {
                    (dicSubForms[key] as OBDTestForm).CheckConnection();
                }
            }
        }

        private void On_OBD_Connect() {
            if (InvokeRequired) {
                this.Invoke((EventHandler)delegate {
                    On_OBD_Connect();
                });
            } else {
                StatusLabelConnStatus.Text = "OBD通讯接口已连接";
                StatusLabelConnStatus.ForeColor = Color.Green;
                StatusLabelDeviceName.Text = m_obdInterface.GetDeviceIDString();
                StatusLabelCommProtocol.Text = m_obdInterface.GetProtocol().ToString();
                toolStripBtnUserPrefs.Enabled = false;
                toolStripBtnVehicles.Enabled = false;
                toolStripBtnSettings.Enabled = false;
                BroadcastConnectionUpdate();
            }
        }

        private void On_OBD_Disconnect() {
            StatusLabelConnStatus.Text = "OBD通讯接口未连接";
            StatusLabelConnStatus.ForeColor = Color.Red;
            StatusLabelDeviceName.Text = "未获取到设备名";
            toolStripBtnUserPrefs.Enabled = true;
            toolStripBtnVehicles.Enabled = true;
            toolStripBtnSettings.Enabled = true;
            BroadcastConnectionUpdate();
            MessageBox.Show("与OBD设备的连接已断开", "断开OBD设备", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void Button_Click(object sender, EventArgs e) {
            if (sender is Button button) {
                if (panel2.Controls.Count > 0 && panel2.Controls[0] is Form activeForm && activeForm != dicSubForms[button.Text]) {
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_SensorGrid] && f_SensorGrid.IsLogging) {
                        if (DialogResult.Yes == MessageBox.Show("当前记录过程将会中断.\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_SensorGrid.PauseLogging();
                        } else {
                            return;
                        }
                    }
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_SensorChart] && f_SensorChart.IsPlotting) {
                        if (DialogResult.Yes == MessageBox.Show("当前图表显示过程将会中止.\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_SensorChart.StopLogging();
                        } else {
                            return;
                        }
                    }
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_FuelEconomy] && f_FuelEconomy.IsWorking) {
                        if (DialogResult.Yes == MessageBox.Show("当前油耗分析过程将会中断.\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_FuelEconomy.StopWorking();
                        } else {
                            return;
                        }
                    }
                    activeForm.Hide();
                }
                foreach (var item in panel1.Controls) {
                    if (item is Button btn) {
                        btn.Font = m_originFont;
                        btn.ForeColor = Color.Black;
                    }
                }
                button.Font = m_boldFont;
                button.ForeColor = Color.Red;

                Form form = dicSubForms[button.Text];
                if (panel2.Controls.IndexOf(form) < 0) {
                    panel2.Controls.Clear();
                    form.TopLevel = false;
                    panel2.Controls.Add(form);
                    panel2.Resize += new EventHandler(Panel2_Resize);
                    //form.MdiParent = this; // 指定当前窗体为顶级Mdi窗体
                    //form.Parent = this.panel2; // 指定子窗体的父容器为
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Size = this.panel2.Size;
                    form.Show();
                }
            }
        }

        private void Panel2_Resize(object sender, EventArgs e) {
            if (panel2.Controls.Count > 0) {
                if (panel2.Controls[0] is Form form) {
                    if (form != null) {
                        form.Size = panel2.Size;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if (!m_obdInterface.LoadParameters(".\\Configs\\generic.csv")) {
                m_obdInterface.GetLogger().TraceError("Failed to load generic parameter definitions!");
                MessageBox.Show("加载generic.csv配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (m_obdInterface.LoadDTCDefinitions(".\\Configs\\dtc.xml") == 0) {
                m_obdInterface.GetLogger().TraceError("Failed to load DTC definitions!");
                MessageBox.Show("加载dtc.xml配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (m_obdInterface.ConnectedStatus) {
                toolStripBtnConnect.Enabled = false;
                toolStripBtnDisconnect.Enabled = true;
                ShowConnectedLabel();
                StatusLabelDeviceName.Text = m_obdInterface.GetDeviceIDString();
            } else {
                toolStripBtnConnect.Enabled = true;
                toolStripBtnDisconnect.Enabled = false;
                ShowDisconnectedLabel();
                StatusLabelDeviceName.Text = "未获取到设备名";
            }
        }

        private void ToolStripBtnUserPrefs_Click(object sender, EventArgs e) {
            UserPreferences userPreferences = m_obdInterface.UserPreferences;
            UserPreferencesForm userForm = new UserPreferencesForm(userPreferences, m_obdTest);
            userForm.ShowDialog();
            m_obdInterface.SaveUserPreferences(userPreferences);
            userForm.Dispose();
        }

        private void ToolStripBtnVehicles_Click(object sender, EventArgs e) {
            VehicleForm vehicleForm = new VehicleForm(m_obdInterface);
            vehicleForm.ShowDialog();
            vehicleForm.Dispose();
        }

        private void ToolStripBtnSettings_Click(object sender, EventArgs e) {
            Settings commSettings = m_obdInterface.CommSettings;
            DBandMES dbandMES = m_obdInterface.DBandMES;
            SettingsForm settingsForm = new SettingsForm(commSettings, dbandMES);
            settingsForm.ShowDialog();
            m_obdInterface.SaveCommSettings(commSettings);
            m_obdInterface.SaveDBandMES(dbandMES);
            StatusLabelCommProtocol.Text = m_obdInterface.GetProtocol().ToString();
            StatusLabelAppProtocol.Text = m_obdInterface.GetStandard().ToString();
            StatusLabelDeviceType.Text = m_obdInterface.GetDevice().ToString().Replace("ELM327", "SH-VCI-302U");
            if (commSettings.AutoDetect) {
                StatusLabelPort.Text = "自动探测";
            } else {
                StatusLabelPort.Text = commSettings.ComPortName;
            }
            settingsForm.Dispose();
        }

        private void ToolStripBtnConnect_Click(object sender, EventArgs e) {
            toolStripBtnConnect.Enabled = false;
            toolStripBtnDisconnect.Enabled = true;

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

            Task.Factory.StartNew(ConnectThreadNew);
        }

        private void ToolStripBtnDisconnect_Click(object sender, EventArgs e) {
            ShowDisconnectedLabel();
            m_obdInterface.Disconnect();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            f_OBDTest.Close();
            f_MonitorTests.Close();
            f_DTC.Close();
            f_FreezeFrames.Close();
            f_OxygenSensors.Close();
            f_SensorGrid.Close();
            f_SensorChart.Close();
            f_Track.Close();
            f_Dyno.Close();
            f_FuelEconomy.Close();
            f_Report.Close();
            f_Terminal.Close();
            m_boldFont.Dispose();
            m_obdInterface.OnConnect -= new OBDInterface.__Delegate_OnConnect(On_OBD_Connect);
            m_obdInterface.OnDisconnect -= new OBDInterface.__Delegate_OnDisconnect(On_OBD_Disconnect);
            m_obdTest.AdvanceMode = false;
        }

        private void ConnectThreadNew() {
            ShowConnectingLabel();
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
                if (m_obdInterface.InitDeviceAuto()) {
                    m_obdInterface.GetLogger().TraceInfo("Connection Established!");
                    ShowConnectedLabel();
                } else {
                    m_obdInterface.GetLogger().TraceWarning("Failed to find a compatible OBD-II interface.");
                    MessageBox.Show("无法找到与本机相连的兼容的OBD-II硬件设备。\r\n请确认没有其他软件正在使用所需端口。", "自动探测失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ShowDisconnectedLabel();
                    m_obdInterface.Disconnect();
                }
            } else {
                int baudRate = m_obdInterface.CommSettings.BaudRate;
                int comPort = m_obdInterface.CommSettings.ComPort;
                if (m_obdInterface.InitDevice(m_obdInterface.CommSettings.HardwareIndex, comPort, baudRate, m_obdInterface.CommSettings.ProtocolIndex, m_obdInterface.CommSettings.StandardIndex)) {
                    m_obdInterface.GetLogger().TraceInfo("Connection Established!");
                    ShowConnectedLabel();
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
                    ShowDisconnectedLabel();
                    m_obdInterface.Disconnect();
                }
            }
        }

        private void ShowConnectedLabel() {
            this.Invoke((EventHandler)delegate {
                StatusLabelConnStatus.ForeColor = Color.Green;
                StatusLabelConnStatus.Text = "OBD通讯接口已连接";
                switch (m_obdInterface.STDType) {
                case StandardType.Automatic:
                    StatusLabelAppProtocol.Text = "Automatic";
                    break;
                case StandardType.ISO_15031:
                    StatusLabelAppProtocol.Text = "ISO_15031";
                    break;
                case StandardType.ISO_27145:
                    StatusLabelAppProtocol.Text = "ISO_27145";
                    break;
                case StandardType.SAE_J1939:
                    StatusLabelAppProtocol.Text = "SAE_J1939";
                    break;
                default:
                    StatusLabelAppProtocol.Text = "Automatic";
                    break;
                }
            });
        }

        private void ShowConnectingLabel() {
            if (InvokeRequired) {
                BeginInvoke((EventHandler)delegate { ShowConnectingLabel(); });
            } else {
                StatusLabelConnStatus.ForeColor = Color.Black;
                StatusLabelConnStatus.Text = "OBD通讯接口连接中...";
            }
        }

        private void ShowDisconnectedLabel() {
            if (InvokeRequired) {
                BeginInvoke((EventHandler)delegate { ShowDisconnectedLabel(); });
            } else {
                StatusLabelConnStatus.ForeColor = Color.Red;
                StatusLabelConnStatus.Text = "OBD通讯接口未连接";
                StatusLabelAppProtocol.Text = "自动探测";
                toolStripBtnConnect.Enabled = true;
                toolStripBtnDisconnect.Enabled = false;
            }
        }

    }
}
