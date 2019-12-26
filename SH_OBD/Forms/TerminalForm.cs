using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class TerminalForm : Form {
        private string m_strPmt;
        private readonly OBDInterface m_obdInterface;

        public TerminalForm(OBDInterface obd) {
            m_obdInterface = obd;
            InitializeComponent();
            m_strPmt = "ELM > ";
            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.SelectionColor = Color.Black;
            richText.AppendText(m_strPmt);
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                string strID = m_obdInterface.GetDeviceIDString();
                if (strID.Contains("ELM327")) {
                    m_strPmt = "ELM327 > ";
                } else if (strID.Contains("ELM323")) {
                    m_strPmt = "ELM323 > ";
                } else if (strID.Contains("ELM322")) {
                    m_strPmt = "ELM322 > ";
                } else if (strID.Contains("ELM320")) {
                    m_strPmt = "ELM320 > ";
                }
            } else {
                m_strPmt = "ELM > ";
            }
            lblPrompt.Text = m_strPmt;
            if (m_obdInterface.ConnectedStatus) {
                richText.Text = richText.Text.Replace("ELM > ", m_strPmt);
            };
        }

        private void BtnSend_Click(object sender, EventArgs e) {
            richText.SelectionStart = richText.Text.Length;
            richText.Focus();

            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("必须首先与车辆进行连接", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCommand.Text.Trim() == "ClearDTC") {
                switch (m_obdInterface.STDType) {
                case StandardType.Automatic:
                    return;
                case StandardType.ISO_15031:
                    txtCommand.Text = "04";
                    break;
                case StandardType.ISO_27145:
                    txtCommand.Text = "14FFFF33";
                    break;
                case StandardType.SAE_J1939:
                    return;
                }
            }

            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.SelectionColor = Color.Blue;
            richText.AppendText(txtCommand.Text);
            richText.AppendText("\n");

            richText.SelectionColor = Color.DarkMagenta;
            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
            richText.AppendText(m_obdInterface.GetRawResponse(txtCommand.Text).Trim());

            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.AppendText("\n\n");
            richText.SelectionColor = Color.Black;
            richText.AppendText(m_strPmt);
        }

        private void TerminalForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                lblPrompt.Text = m_strPmt;
            }
        }

        private void TxtCommand_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter && sender is TextBox tb && tb.Name == "txtCommand") {
                BtnSend_Click(btnSend, null);
            }
        }
    }
}
