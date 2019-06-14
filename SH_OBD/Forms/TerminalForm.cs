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
        private string m_strID;
        private OBDInterface m_obdInterface;

        public TerminalForm(OBDInterface obd) {
            m_obdInterface = obd;
            InitializeComponent();

            m_strID = m_obdInterface.GetDeviceIDString();
            Update();
            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.SelectionColor = Color.Black;
            addPrompt();
        }

        private void addPrompt() {
            if (string.IsNullOrEmpty(m_strID)) {
                richText.AppendText("ELM > ");
            } else {
                richText.AppendText(m_strID + " > ");
            }
        }

        public new void Update() {
            if (string.IsNullOrEmpty(m_strID)) {
                lblPrompt.Text = "ELM >";
            } else {
                lblPrompt.Text = m_strID + " > ";
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            richText.SelectionStart = richText.Text.Length;
            richText.Focus();

            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("A vehicle connection must first be established.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.SelectionColor = Color.Blue;
            richText.AppendText(txtCommand.Text);
            richText.AppendText("\r\n\r\n");

            richText.SelectionColor = Color.DarkMagenta;
            richText.SelectionFont = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold);
            richText.AppendText(m_obdInterface.GetRawResponse(txtCommand.Text));

            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.AppendText("\r\n\r\n");
            richText.SelectionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
            richText.SelectionColor = Color.Black;
            addPrompt();
        }

    }
}
