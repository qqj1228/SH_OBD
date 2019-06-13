using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class CommForm : Form {
        private List<VehicleProfile> m_profiles;
        private OBDInterface m_OBDInterface;

        public CommForm(OBDInterface obd) {
            InitializeComponent();
            m_OBDInterface = obd;
            m_profiles = m_OBDInterface.VehicleProfiles;
            PopulateProfileCombobox();
        }

        private void btnConnect_Click(object sender, EventArgs e) {
            comboProfile.Enabled = false;
            btnManageProfiles.Enabled = false;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            picCheck1.Image = picBlankBox.Image;
            picCheck2.Image = picBlankBox.Image;
            picCheck3.Image = picBlankBox.Image;
            picCheck4.Image = picBlankBox.Image;

            m_OBDInterface.SaveActiveProfile((VehicleProfile)comboProfile.SelectedItem);

            m_OBDInterface.logItem("ProScan");
            m_OBDInterface.logItem("Connection Procedure Initiated");

            if (m_OBDInterface.CommSettings.AutoDetect) {
                m_OBDInterface.logItem("   Automatic Hardware Detection: ON");
            } else {
                m_OBDInterface.logItem("   Automatic Hardware Detection: OFF");
            }

            m_OBDInterface.logItem(string.Format("   Baud Rate: {0}", m_OBDInterface.CommSettings.BaudRate));
            m_OBDInterface.logItem(string.Format("   Default Port: {0}", m_OBDInterface.CommSettings.ComPortName));

            switch (m_OBDInterface.CommSettings.HardwareIndex) {
                case HardwareType.Automatic:
                    m_OBDInterface.logItem("   Interface: Auto-Detect");
                    break;
                case HardwareType.ELM327:
                    m_OBDInterface.logItem("   Interface: ELM327");
                    break;
                case HardwareType.ELM320:
                    m_OBDInterface.logItem("   Interface: ELM320");
                    break;
                case HardwareType.ELM322:
                    m_OBDInterface.logItem("   Interface: ELM322");
                    break;
                case HardwareType.ELM323:
                    m_OBDInterface.logItem("   Interface: ELM323");
                    break;
                case HardwareType.CANtact:
                    m_OBDInterface.logItem("   Interface: CANtact");
                    break;
                default:
                    throw new Exception("Bad hardware type.");
            }


            m_OBDInterface.logItem(string.Format("   Protocol: {0}", m_OBDInterface.CommSettings.ProtocolName));

            if (m_OBDInterface.CommSettings.DoInitialization) {
                m_OBDInterface.logItem("   Initialize: YES");
            } else {
                m_OBDInterface.logItem("   Initialize: NO");
            }

            Task.Factory.StartNew(ConnectThreadNew);
        }

        private void btnDisconnect_Click(object sender, EventArgs e) {
            ShowDisconnectedLabel();
            picCheck1.Image = picBlankBox.Image;
            picCheck2.Image = picBlankBox.Image;
            picCheck3.Image = picBlankBox.Image;
            picCheck4.Image = picBlankBox.Image;
            m_OBDInterface.Disconnect();
        }

        private void btnManageProfiles_Click(object sender, EventArgs e) {
            new VehicleForm(m_OBDInterface).ShowDialog();
            PopulateProfileCombobox();
        }

        private void comboProfile_SelectedValueChanged(object sender, EventArgs e) {
        }

        private void ConnectThreadNew() {
            ShowConnectingLabel();
            if (m_OBDInterface.CommSettings.AutoDetect) {
                if (m_OBDInterface.initDeviceAuto()) {
                    m_OBDInterface.logItem("Connection Established!");
                    ShowConnectedLabel();
                    OBDParameter param = new OBDParameter();
                    param.OBDRequest = "0902";
                    param.Service = 9;
                    param.Parameter = 2;
                    param.ValueTypes = 4;
                    m_OBDInterface.getValue(param, true);
                } else {
                    MessageBox.Show("ProScan failed to find a compatible OBD-II interface attached to this computer.\r\n\r\nPlease verify that no other application is currently using the required port.", "Auto Detection Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    m_OBDInterface.logItem("Failed to find a compatible OBD-II interface.");
                    ShowDisconnectedLabel();
                }
            } else {
                int baudRate = m_OBDInterface.CommSettings.BaudRate;
                int comPort = m_OBDInterface.CommSettings.ComPort;
                if (m_OBDInterface.initDevice(m_OBDInterface.CommSettings.HardwareIndex, comPort, baudRate, m_OBDInterface.CommSettings.ProtocolIndex)) {
                    m_OBDInterface.logItem("Connection Established!");
                    ShowConnectedLabel();
                } else {
                    MessageBox.Show(
                        string.Format(@"ProScan failed to find a compatible OBD-II interface attached to {0} at baud rate {1} bps.

Please verify that no other application is currently using the required port and that the baud rate is correct.",
                            m_OBDInterface.CommSettings.ComPortName,
                            m_OBDInterface.CommSettings.BaudRate
                            ),
                        "Connection Failure",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                        );
                    m_OBDInterface.logItem("Failed to find a compatible OBD-II interface.");
                    ShowDisconnectedLabel();
                }
            }
        }

        private void Disconnect() {
            m_OBDInterface.Disconnect();
            ShowDisconnectedLabel();
        }

        private void PopulateProfileCombobox() {
            comboProfile.Items.Clear();
            foreach (VehicleProfile vehicle in m_profiles) {
                comboProfile.Items.Add(vehicle);
            }

            if (comboProfile.Items.Count > 0) {
                if (m_OBDInterface.CommSettings.ActiveProfileIndex < comboProfile.Items.Count) {
                    comboProfile.SelectedIndex = m_OBDInterface.CommSettings.ActiveProfileIndex;
                } else {
                    comboProfile.SelectedIndex = 0;
                }
            }
        }

        private void ShowConnectedLabel() {
            this.Invoke((EventHandler)delegate {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Connected";
            });
        }

        private void ShowConnectingLabel() {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { ShowConnectingLabel(); });
            } else {
                lblStatus.ForeColor = Color.Black;
                lblStatus.Text = "Connecting...";
            }
        }

        private void ShowDisconnectedLabel() {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { ShowDisconnectedLabel(); });
            } else {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Disconnected";
                btnConnect.Enabled = true;
                comboProfile.Enabled = true;
                btnManageProfiles.Enabled = true;
                btnDisconnect.Enabled = false;
            }
        }

        private void ShowOBD2InitFailedError() {
            MessageBox.Show("The interface hardware was detected and initialized, but communication with the vehicle could not be established. Make sure that the vehicle's ignition key is turned to the ON position or that the engine is running.\r\n\r\nIf using ELM320 (PWM), ELM322 (VPW), or ELM323 (ISO):\r\n\tVerify that the vehicle's protocol matches the interface.\r\n\r\nIf using ELM327 (VPW, PWM, ISO, and CAN):\r\n\tVerify the interface configuration under Communication Settings.\r\n\tTry manually setting the protocol.\r\n\tTry bypassing initialization.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void UpdateForm() {
            PopulateProfileCombobox();
        }

    }
}
