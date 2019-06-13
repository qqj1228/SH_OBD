using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class SettingsForm : Form {
        private Preferences m_preferences;

        public SettingsForm(Preferences prefs) {
            InitializeComponent();
            m_preferences = prefs;
        }

        private void SettingsForm_Load(object sender, EventArgs e) {
            try {
                for (int iComPort = 0; iComPort < 50; ++iComPort) {
                    if (CommBase.isPortAvailable(iComPort)) {
                        comboPorts.Items.Add("COM" + iComPort.ToString());
                    }
                }

                if (CommBase.isPortAvailable(m_preferences.ComPort)) {
                    comboPorts.SelectedItem = m_preferences.ComPortName;
                } else if (comboPorts.Items.Count > 0) {
                    comboPorts.SelectedIndex = 0;
                }

                comboHardware.SelectedIndex = m_preferences.HardwareIndexInt;
                comboBaud.SelectedIndex = m_preferences.BaudRateIndex;

                foreach (string item in Preferences.ProtocolNames) {
                    comboProtocol.Items.Add(item);
                }

                comboProtocol.SelectedIndex = m_preferences.ProtocolIndexInt;
                comboInitialize.SelectedIndex = !m_preferences.DoInitialization ? 1 : 0;
                if (m_preferences.AutoDetect) {
                    checkBoxAutoDetect.Checked = true;
                } else {
                    checkBoxAutoDetect.Checked = false;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            m_preferences.AutoDetect = checkBoxAutoDetect.Checked;
            if (comboPorts.SelectedItem != null && comboPorts.SelectedItem.ToString().Length > 3) {
                m_preferences.ComPort = Convert.ToInt32(comboPorts.SelectedItem.ToString().Remove(0, 3));
            }

            m_preferences.BaudRateIndex = comboBaud.SelectedIndex;
            m_preferences.HardwareIndexInt = comboHardware.SelectedIndex;
            m_preferences.ProtocolIndexInt = comboProtocol.SelectedIndex;
            m_preferences.DoInitialization = (comboInitialize.SelectedIndex == 0);
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
