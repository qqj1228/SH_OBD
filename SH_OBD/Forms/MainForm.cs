using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class MainForm : Form {
        private Dictionary<string, Form> dicSubForms;
        private CommForm f_Start;
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
            m_originFont = buttonSettings.Font;
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

            f_Start = new CommForm(m_obdInterface);
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

            buttonStart.Text = Properties.Resources.buttonName_Start;
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

            buttonUserPrefs.Text = Properties.Resources.buttonName_UserPrefs;
            buttonVehicles.Text = Properties.Resources.buttonName_Vehicles;
            buttonSettings.Text = Properties.Resources.buttonName_Settings;

            dicSubForms.Add(Properties.Resources.buttonName_Start, f_Start);
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
            StatusLabelDeviceName.Text = m_obdInterface.getDeviceIDString().Trim();
            StatusLabelConnStatus.Text = "OBD设备已连接";
            StatusLabelConnStatus.ForeColor = Color.Green;
            StatusLabelVehicle.Text = m_obdInterface.ActiveProfile.Name;
            buttonUserPrefs.Enabled = false;
            buttonVehicles.Enabled = false;
            buttonSettings.Enabled = false;
            BroadcastConnectionUpdate();
        }

        private void On_OBD_Disconnect() {
            StatusLabelConnStatus.Text = "OBD设备未连接";
            StatusLabelConnStatus.ForeColor = Color.Red;
            buttonUserPrefs.Enabled = true;
            buttonVehicles.Enabled = true;
            buttonSettings.Enabled = true;
            BroadcastConnectionUpdate();
            MessageBox.Show("与OBD设备的连接已断开", "断开OBD设备", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void Button_Click(object sender, EventArgs e) {
            if (sender is Button button) {
                if (panel2.Controls.Count > 0 && panel2.Controls[0] is Form activeForm) {
                    if (activeForm == dicSubForms[Properties.Resources.buttonName_SensorGrid] && f_SensorGrid.IsLogging ) {
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

        private void buttonUserPrefs_Click(object sender, EventArgs e) {
            UserPreferences userPreferences = m_obdInterface.UserPreferences;
            int num = (int)new UserPreferencesForm(userPreferences).ShowDialog();
            m_obdInterface.SaveUserPreferences(userPreferences);
        }

        private void buttonVehicles_Click(object sender, EventArgs e) {
            int num = (int)new VehicleForm(m_obdInterface).ShowDialog();
            f_Start.UpdateForm();
        }

        private void buttonSettings_Click(object sender, EventArgs e) {
            Preferences commSettings = m_obdInterface.CommSettings;
            new SettingsForm(commSettings).ShowDialog();
            m_obdInterface.SaveCommSettings(commSettings);
            if (commSettings.AutoDetect) {
                StatusLabelPort.Text = "自动探测";
            } else {
                StatusLabelPort.Text = commSettings.ComPortName;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if (!m_obdInterface.LoadParameters("generic.xml")) {
                MessageBox.Show("加载generic.xml配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (m_obdInterface.LoadDTCDefinitions("dtc.xml") == 0) {
                MessageBox.Show("加载dtc.xml配置文件失败!", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Monitor.Enter(m_obdInterface);
            if (m_obdInterface.ConnectedStatus) {
                m_obdInterface.Disconnect();
            }
            Monitor.Exit(m_obdInterface);
        }

    }
}
