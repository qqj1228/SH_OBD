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
        private OBDInterface m_obdInterface;

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

        private void btnSend_Click(object sender, EventArgs e) {
            if (txtCommand.Text.Contains("test")) {
                // 用于测试代码，生产环境中无用处
                int num1 = 0x11;
                int num2 = 0x22;
                int num3 = 0x33;
                int num4 = 0x44;
                int num0 = (num1 * 0x1000000) + (num2 * 0x10000) + (num3 * 0x100) + num4;
                string s = (num0 / 3600).ToString() + " hrs, ";
                s += ((num0 % 3600) / 60).ToString() + " min, ";
                s += ((num0 % 3600) % 60).ToString() + " sec";
                MessageBox.Show(s);
                return;
            }
            richText.SelectionStart = richText.Text.Length;
            richText.Focus();

            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("必须首先与车辆进行连接", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
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
                btnSend_Click(btnSend, null);
            }
        }
    }
}
