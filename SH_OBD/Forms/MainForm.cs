﻿using System;
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
        private TestForm f_MonitorTests;
        private DTCForm f_DTC;
        private FreezeFramesForm f_FreezeFrames;
        private OxygenSensorsForm f_OxygenSensors;
        private SensorMonitorForm f_SensorGrid;
        private ScopeForm f_SensorChart;
        private TrackForm f_Track;
        private DynoForm f_Dyno;
        private FuelEconomyForm f_FuelEconomy;
        private ReportGeneratorForm f_Report;
        private TerminalForm f_Terminal;
        private CommLogForm f_CommLog;
        private OBDInterface m_obdInterface;

        private Font m_boldFont, m_originFont;

        public MainForm() {
            InitializeComponent();
            m_obdInterface = new OBDInterface();
            m_obdInterface.OnConnect += new OBDInterface.__Delegate_OnConnect(On_OBD_Connect);
            m_obdInterface.OnDisconnect += new OBDInterface.__Delegate_OnDisconnect(On_OBD_Disconnect);

            m_originFont = buttonDefaultFontStyle.Font;
            m_boldFont = new Font(m_originFont, FontStyle.Bold);

            if (m_obdInterface.ActiveProfile != null) {
                StatusLabelVehicle.Text = m_obdInterface.ActiveProfile.ToString();
            }
            if (m_obdInterface.CommSettings != null) {
                if (m_obdInterface.CommSettings.AutoDetect) {
                    StatusLabelPort.Text = "自动探测";
                } else {
                    StatusLabelPort.Text = m_obdInterface.CommSettings.ComPortName;
                }
            }

            InitSubForm();
            m_obdInterface.EnableLogFile(true);
            StatusLabelConnStatus.ForeColor = Color.Red;
        }

        void InitSubForm() {
            dicSubForms = new Dictionary<string, Form>();

            f_MonitorTests = new TestForm(m_obdInterface);
            f_DTC = new DTCForm(m_obdInterface);
            f_FreezeFrames = new FreezeFramesForm(m_obdInterface);
            f_OxygenSensors = new OxygenSensorsForm(m_obdInterface);
            f_SensorGrid = new SensorMonitorForm(m_obdInterface);
            f_SensorChart = new ScopeForm(m_obdInterface);
            f_Track = new TrackForm(m_obdInterface);
            f_Dyno = new DynoForm(m_obdInterface);
            f_FuelEconomy = new FuelEconomyForm(m_obdInterface);
            f_Report = new ReportGeneratorForm(m_obdInterface);
            f_Terminal = new TerminalForm(m_obdInterface);
            f_CommLog = new CommLogForm();

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
            buttonCommLog.Text = Properties.Resources.buttonName_Log;

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
            dicSubForms.Add(Properties.Resources.buttonName_Log, f_CommLog);
        }

        private void BroadcastConnectionUpdate() {
            foreach (var key in dicSubForms.Keys) {
                if (key == Properties.Resources.buttonName_DTC) {
                    (dicSubForms[key] as DTCForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_SensorGrid) {
                    (dicSubForms[key] as SensorMonitorForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_SensorChart) {
                    (dicSubForms[key] as ScopeForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_Track) {
                    (dicSubForms[key] as TrackForm).CheckConnection();
                } else if (key == Properties.Resources.buttonName_Dyno) {
                    (dicSubForms[key] as DynoForm).CheckConnection();
                }
            }
        }

        private void On_OBD_Connect() {
            if (InvokeRequired) {
                this.Invoke((EventHandler)delegate {
                    StatusLabelDeviceName.Text = m_obdInterface.GetDeviceIDString().Trim();
                    StatusLabelConnStatus.Text = "OBD接口已连接";
                    StatusLabelConnStatus.ForeColor = Color.Green;
                    StatusLabelVehicle.Text = m_obdInterface.ActiveProfile.Name;
                    toolStripBtnUserPrefs.Enabled = false;
                    toolStripBtnVehicles.Enabled = false;
                    toolStripBtnSettings.Enabled = false;
                });
            }
            BroadcastConnectionUpdate();
        }

        private void On_OBD_Disconnect() {
            StatusLabelConnStatus.Text = "OBD接口未连接";
            StatusLabelConnStatus.ForeColor = Color.Red;
            toolStripBtnUserPrefs.Enabled = true;
            toolStripBtnVehicles.Enabled = true;
            toolStripBtnSettings.Enabled = true;
            BroadcastConnectionUpdate();
            MessageBox.Show("与OBD设备的连接已断开", "断开OBD设备", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void Button_Click(object sender, EventArgs e) {
            if (sender is Button button) {
                if (panel2.Controls.Count > 0 && panel2.Controls[0] is Form activeForm) {
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_SensorGrid] && f_SensorGrid.IsLogging) {
                        if (DialogResult.Yes == MessageBox.Show("当前记录过程将会中断.\r\n\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_SensorGrid.PauseLogging();
                        } else {
                            return;
                        }
                    }
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_SensorChart] && f_SensorChart.IsPlotting) {
                        if (DialogResult.Yes == MessageBox.Show("当前图表显示过程将会中止.\r\n\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_SensorChart.StopLogging();
                        } else {
                            return;
                        }
                    }
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_FuelEconomy] && f_FuelEconomy.IsWorking) {
                        if (DialogResult.Yes == MessageBox.Show("当前油耗分析过程将会中断.\r\n\r\n是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                            f_FuelEconomy.StopWorking();
                        } else {
                            return;
                        }
                    }
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
                    panel2.Resize += new EventHandler(panel2_Resize);
                    //form.MdiParent = this;//指定当前窗体为顶级Mdi窗体
                    //form.Parent = this.panel2;//指定子窗体的父容器为
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Size = this.panel2.Size;
                    form.Show();
                }
            }
        }

        private void panel2_Resize(object sender, EventArgs e) {
            if (panel2.Controls.Count > 0) {
                if (panel2.Controls[0] is Form form) {
                    if (form != null) {
                        form.Size = panel2.Size;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if (!m_obdInterface.LoadParameters(".\\configs\\generic.xml")) {
                MessageBox.Show("加载generic.xml配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (m_obdInterface.LoadDTCDefinitions(".\\configs\\dtc.xml") == 0) {
                MessageBox.Show("加载dtc.xml配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripBtnUserPrefs_Click(object sender, EventArgs e) {
            UserPreferences userPreferences = m_obdInterface.UserPreferences;
            int num = (int)new UserPreferencesForm(userPreferences).ShowDialog();
            m_obdInterface.SaveUserPreferences(userPreferences);
        }

        private void toolStripBtnVehicles_Click(object sender, EventArgs e) {
            int num = (int)new VehicleForm(m_obdInterface).ShowDialog();
        }

        private void toolStripBtnSettings_Click(object sender, EventArgs e) {
            Preferences commSettings = m_obdInterface.CommSettings;
            new SettingsForm(commSettings).ShowDialog();
            m_obdInterface.SaveCommSettings(commSettings);
            if (commSettings.AutoDetect) {
                StatusLabelPort.Text = "自动探测";
            } else {
                StatusLabelPort.Text = commSettings.ComPortName;
            }
        }

        private void toolStripBtnConnect_Click(object sender, EventArgs e) {
            toolStripBtnConnect.Enabled = false;
            toolStripBtnDisconnect.Enabled = true;

            m_obdInterface.LogItem("SH_OBD");
            m_obdInterface.LogItem("连接过程初始化");

            if (m_obdInterface.CommSettings.AutoDetect) {
                m_obdInterface.LogItem("   自动探测OBD硬件设备: 打开");
            } else {
                m_obdInterface.LogItem("   自动探测OBD硬件设备: 关闭");
            }

            m_obdInterface.LogItem(string.Format("   波特率: {0}", m_obdInterface.CommSettings.BaudRate));
            m_obdInterface.LogItem(string.Format("   默认端口: {0}", m_obdInterface.CommSettings.ComPortName));

            switch (m_obdInterface.CommSettings.HardwareIndex) {
                case HardwareType.Automatic:
                    m_obdInterface.LogItem("   设备接口: 自动探测");
                    break;
                case HardwareType.ELM327:
                    m_obdInterface.LogItem("   设备接口: ELM327");
                    break;
                case HardwareType.ELM320:
                    m_obdInterface.LogItem("   设备接口: ELM320");
                    break;
                case HardwareType.ELM322:
                    m_obdInterface.LogItem("   设备接口: ELM322");
                    break;
                case HardwareType.ELM323:
                    m_obdInterface.LogItem("   设备接口: ELM323");
                    break;
                case HardwareType.CANtact:
                    m_obdInterface.LogItem("   设备接口: CANtact");
                    break;
                default:
                    throw new Exception("不支持的硬件设备");
            }


            m_obdInterface.LogItem(string.Format("   OBD协议: {0}", m_obdInterface.CommSettings.ProtocolName));

            if (m_obdInterface.CommSettings.DoInitialization) {
                m_obdInterface.LogItem("   初始化: 是");
            } else {
                m_obdInterface.LogItem("   初始化: 否");
            }

            Task.Factory.StartNew(ConnectThreadNew);
        }

        private void toolStripBtnDisconnect_Click(object sender, EventArgs e) {
            ShowDisconnectedLabel();
            m_obdInterface.Disconnect();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Monitor.Enter(m_obdInterface);
            if (m_obdInterface.ConnectedStatus) {
                m_obdInterface.Disconnect();
            }
            Monitor.Exit(m_obdInterface);
        }

        private void ConnectThreadNew() {
            ShowConnectingLabel();
            if (m_obdInterface.CommSettings.AutoDetect) {
                if (m_obdInterface.InitDeviceAuto()) {
                    m_obdInterface.LogItem("OBD连接已建立！");
                    ShowConnectedLabel();
                    OBDParameter param = new OBDParameter {
                        OBDRequest = "0902",
                        Service = 9,
                        Parameter = 2,
                        ValueTypes = 4
                    };
                    m_obdInterface.GetValue(param, true);
                } else {
                    MessageBox.Show("软件无法找到与本机相连的兼容的OBD-II硬件设备。\r\n\r\n请确认没有其他软件正在使用所需端口。", "自动探测失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_obdInterface.LogItem("无法找到兼容的OBD-II协议设备。");
                    ShowDisconnectedLabel();
                }
            } else {
                int baudRate = m_obdInterface.CommSettings.BaudRate;
                int comPort = m_obdInterface.CommSettings.ComPort;
                if (m_obdInterface.InitDevice(m_obdInterface.CommSettings.HardwareIndex, comPort, baudRate, m_obdInterface.CommSettings.ProtocolIndex)) {
                    m_obdInterface.LogItem("连接已建立！");
                    ShowConnectedLabel();
                } else {
                    MessageBox.Show(
                        string.Format("软件无法找到与 {0} 连接的波特率为 {1} bps 的兼容的OBD-II协议设备。\r\n\r\n请确认没有其他软件正在使用所需端口且波特率设置正确。",
                            m_obdInterface.CommSettings.ComPortName,
                            m_obdInterface.CommSettings.BaudRate
                            ),
                        "连接失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                        );
                    m_obdInterface.LogItem("无法找到兼容的OBD-II协议设备。");
                    ShowDisconnectedLabel();
                }
            }
        }

        private void ShowConnectedLabel() {
            this.Invoke((EventHandler)delegate {
                StatusLabelConnStatus.ForeColor = Color.Green;
                StatusLabelConnStatus.Text = "OBD接口已连接";
            });
        }

        private void ShowConnectingLabel() {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { ShowConnectingLabel(); });
            } else {
                StatusLabelConnStatus.ForeColor = Color.Black;
                StatusLabelConnStatus.Text = "OBD接口连接中...";
            }
        }

        private void ShowDisconnectedLabel() {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { ShowDisconnectedLabel(); });
            } else {
                StatusLabelConnStatus.ForeColor = Color.Red;
                StatusLabelConnStatus.Text = "OBD接口已断开";
                toolStripBtnConnect.Enabled = true;
                toolStripBtnDisconnect.Enabled = false;
            }
        }

        private void ShowOBD2InitFailedError() {
            MessageBox.Show("当前OBD硬件设备已探测到并成功初始化，但是无法与车辆进行通讯。请确认车辆点火键处于ON位置或者发动机处于运转中。\r\n\r\n如果使用 ELM320 (PWM)、ELM322 (VPW)、ELM323 (ISO)设备：\r\n\t请确认车辆OBD协议与当前OBD设备匹配。\r\n\r\n如果使用 ELM327 (VPW, PWM, ISO, and CAN)设备：\r\n\t请确认当前OBD设备的通讯设置。\r\n\t尝试手动设置OBD协议。\r\n\t尝试旁路初始化。", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

    }
}
