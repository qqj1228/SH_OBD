using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class SettingsForm : Form {
        private readonly Settings m_settings;
        private readonly DBandMES m_dbandMES;

        public SettingsForm(Settings settings, DBandMES dbandMES) {
            InitializeComponent();
            m_settings = settings;
            m_dbandMES = dbandMES;
        }

        private void SettingsForm_Load(object sender, EventArgs e) {
            try {
                string[] serialPorts = SerialPort.GetPortNames();
                foreach (string serialPort in serialPorts) {
                    comboPorts.Items.Add(serialPort);
                }

                if (CommBase.IsPortAvailable(m_settings.ComPort)) {
                    comboPorts.SelectedItem = m_settings.ComPortName;
                } else if (comboPorts.Items.Count > 0) {
                    comboPorts.SelectedIndex = 0;
                }

                comboHardware.SelectedIndex = m_settings.HardwareIndexInt;
                comboBaud.SelectedIndex = m_settings.BaudRateIndex;

                foreach (string item in Settings.ProtocolNames) {
                    comboProtocol.Items.Add(item);
                }

                comboProtocol.SelectedIndex = m_settings.ProtocolIndexInt;
                comboInitialize.SelectedIndex = !m_settings.DoInitialization ? 1 : 0;
                if (m_settings.AutoDetect) {
                    checkBoxAutoDetect.Checked = true;
                } else {
                    checkBoxAutoDetect.Checked = false;
                }

                this.txtBoxUser.Text = m_dbandMES.UserName;
                this.txtBoxPwd.Text = m_dbandMES.PassWord;
                this.txtBoxDBName.Text = m_dbandMES.DBName;
                this.txtBoxIP.Text = m_dbandMES.IP;
                this.txtBoxPort.Text = m_dbandMES.Port;
                this.txtBoxWebSvcAddress.Text = m_dbandMES.WebServiceAddress;
                this.txtBoxWebSvcName.Text = m_dbandMES.WebServiceName;
                this.txtBoxWebSvcMethods.Text = m_dbandMES.WebServiceMethods;

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            m_settings.AutoDetect = checkBoxAutoDetect.Checked;
            if (comboPorts.SelectedItem != null && comboPorts.SelectedItem.ToString().Length > 3) {
                m_settings.ComPort = Convert.ToInt32(comboPorts.SelectedItem.ToString().Remove(0, 3));
            }

            m_settings.BaudRateIndex = comboBaud.SelectedIndex;
            m_settings.HardwareIndexInt = comboHardware.SelectedIndex;
            m_settings.ProtocolIndexInt = comboProtocol.SelectedIndex;
            m_settings.DoInitialization = (comboInitialize.SelectedIndex == 0);

            m_dbandMES.UserName = this.txtBoxUser.Text;
            m_dbandMES.PassWord = this.txtBoxPwd.Text;
            m_dbandMES.DBName = this.txtBoxDBName.Text;
            m_dbandMES.IP = this.txtBoxIP.Text;
            m_dbandMES.Port = this.txtBoxPort.Text;
            m_dbandMES.WebServiceAddress = this.txtBoxWebSvcAddress.Text;
            m_dbandMES.WebServiceName = this.txtBoxWebSvcName.Text;
            m_dbandMES.WebServiceMethods = this.txtBoxWebSvcMethods.Text;

            Close();
        }

        private void comboHardware_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboHardware.SelectedIndex == (int)HardwareType.ELM327) {
                groupELM.Enabled = true;
            } else {
                groupELM.Enabled = false;
            }
        }

        private void checkBoxAutoDetect_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxAutoDetect.Checked) {
                groupComm.Enabled = false;
                groupHardware.Enabled = false;
                groupELM.Enabled = false;
            } else {
                groupComm.Enabled = true;
                groupHardware.Enabled = true;
                groupELM.Enabled = true;
            }
        }

    }
}
