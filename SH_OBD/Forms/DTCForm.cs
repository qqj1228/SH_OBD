using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class DTCForm : Form {
        private OBDInterface m_obdInterface;
        private bool m_bMilStatus;
        private int m_iTotalDTC;
        private List<DTC> m_ListDTC;
        private List<DTC> m_ListPending;
        private List<DTC> m_ListPermanent;

        public DTCForm(OBDInterface obd2) {
            InitializeComponent();
            m_obdInterface = obd2;
            CheckConnection();
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                btnRefresh.Enabled = true;
                btnErase.Enabled = true;
            } else {
                btnRefresh.Enabled = false;
                btnErase.Enabled = false;
            }
        }

        public void RefreshDiagnosticData() {
            lblMilStatus.Text = "OFF";
            picMIL.Image = picMilOff.Image;
            lblTotalCodes.Text = "0";
            richTextDTC.Text = "";
            richTextPending.Text = "";
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("A vehicle connection must first be established.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_obdInterface.LogItem("Error. DTC Form. Attempted refresh without vehicle connection.");
            } else {
                ReadCodes();
                RefreshDisplay();
            }
        }

        public void ReadCodes() {
            m_ListDTC.Clear();
            m_ListPending.Clear();
            m_ListPermanent.Clear();
            OBDParameterValue value;

            value = m_obdInterface.GetValue("SAE.MIL", true);
            if (!value.ErrorDetected) {
                SetMilStatus(value.BoolValue);
            }

            value = m_obdInterface.GetValue("SAE.DTC_COUNT", true);
            if (!value.ErrorDetected) {
                SetDTCTotal((int)value.DoubleValue);
            }

            value = m_obdInterface.GetValue("SAE.STORED_DTCS", true);
            if (!value.ErrorDetected) {
                foreach (string dtc in value.StringCollectionValue) {
                    m_ListDTC.Add(m_obdInterface.GetDTC(dtc));
                }
            }

            value = m_obdInterface.GetValue("SAE.PENDING_DTCS", true);
            if (!value.ErrorDetected) {
                foreach (string dtc in value.StringCollectionValue) {
                    m_ListPending.Add(m_obdInterface.GetDTC(dtc));
                }
            }

            value = m_obdInterface.GetValue("SAE.PERMANENT_DTCS", true);
            if (!value.ErrorDetected) {
                foreach (string dtc in value.StringCollectionValue) {
                    m_ListPermanent.Add(m_obdInterface.GetDTC(dtc));
                }
            }
        }

        private void DTCForm_Load(object sender, EventArgs e) {
            m_ListDTC = new List<DTC>();
            m_ListPending = new List<DTC>();
            m_ListPermanent = new List<DTC>();
            SetMilStatus(false);
        }

        private void SetMilStatus(bool bStatus) {
            m_bMilStatus = bStatus;
            if (bStatus) {
                lblMilStatus.Text = "ON";
                picMIL.Image = picMilOn.Image;
            } else {
                lblMilStatus.Text = "OFF";
                picMIL.Image = picMilOff.Image;
            }
        }

        private void SetDTCTotal(int iTotal) {
            m_iTotalDTC = iTotal;
            lblTotalCodes.Text = Convert.ToString(iTotal);
        }

        private void RefreshDisplay() {
            richTextDTC.Text = "";
            richTextPending.Text = "";
            richTextPermanent.Text = "";
            int idx;
            string text;

            if (m_ListDTC.Count > 0) {
                for (idx = 1; idx <= m_ListDTC.Count; idx++) {
                    DTC dtc = m_ListDTC[idx - 1];
                    richTextDTC.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                    richTextDTC.SelectionColor = Color.Red;
                    richTextDTC.AppendText(string.Format("{0}. {1}\r\n", idx, dtc.Name));

                    if (string.IsNullOrEmpty(dtc.Description)) {
                        richTextDTC.SelectionFont = new Font("Courier New", 10f, FontStyle.Italic | FontStyle.Bold);
                        richTextDTC.SelectionColor = Color.Black;
                        text = "    No definition found.\r\n\r\n";
                    } else {
                        richTextDTC.SelectionFont = new Font("Courier New", 10f, FontStyle.Bold);
                        richTextDTC.SelectionColor = Color.Black;
                        text = string.Format("    {0}: {1}\r\n\r\n", dtc.Category, dtc.Description);
                    }
                    richTextDTC.AppendText(text);
                }
            } else {
                richTextDTC.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                richTextDTC.SelectionColor = Color.Green;
                richTextDTC.AppendText("No stored trouble codes found.");
            }

            if (m_ListPending.Count > 0) {
                for (idx = 1; idx <= m_ListPending.Count; idx++) {
                    DTC dtc = m_ListPending[idx - 1];
                    richTextPending.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                    richTextPending.SelectionColor = Color.Red;
                    richTextPending.AppendText(string.Format("{0}. {1}\r\n", idx, dtc.Name));

                    if (string.IsNullOrEmpty(dtc.Description)) {
                        richTextPending.SelectionFont = new Font("Courier New", 10f, FontStyle.Italic | FontStyle.Bold);
                        richTextPending.SelectionColor = Color.Black;
                        text = "    No definition found.\r\n\r\n";
                    } else {
                        richTextPending.SelectionFont = new Font("Courier New", 10f, FontStyle.Bold);
                        richTextPending.SelectionColor = Color.Black;
                        text = string.Format("    {0}: {1}\r\n\r\n", dtc.Category, dtc.Description);
                    }
                    richTextPending.AppendText(text);
                }
            } else {
                richTextPending.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                richTextPending.SelectionColor = Color.Green;
                richTextPending.AppendText("No pending trouble codes found.");
            }

            if (m_ListPermanent.Count > 0) {
                for (idx = 1; idx <= m_ListPermanent.Count; idx++) {
                    DTC dtc = (DTC)m_ListPermanent[idx - 1];
                    richTextPermanent.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                    richTextPermanent.SelectionColor = Color.Red;
                    richTextPermanent.AppendText(string.Format("{0}. {1}\r\n", idx, dtc.Name));

                    if (string.IsNullOrEmpty(dtc.Description)) {
                        richTextPermanent.SelectionFont = new Font("Courier New", 10f, FontStyle.Italic | FontStyle.Bold);
                        richTextPermanent.SelectionColor = Color.Black;
                        text = "    No definition found.\r\n\r\n";
                    } else {
                        richTextPermanent.SelectionFont = new Font("Courier New", 10f, FontStyle.Bold);
                        richTextPermanent.SelectionColor = Color.Black;
                        text = string.Format("    {0}: {1}\r\n\r\n", dtc.Category, dtc.Description);
                    }
                    richTextPermanent.AppendText(text);
                }
            } else {
                richTextPermanent.SelectionFont = new Font("Courier New", 12f, FontStyle.Bold);
                richTextPermanent.SelectionColor = Color.Green;
                richTextPermanent.AppendText("No permanent trouble codes found.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            RefreshDiagnosticData();
        }

        private void btnErase_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("A vehicle connection must first be established.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_obdInterface.LogItem("Error. DTC Form. Attempted to erase codes without vehicle connection.");
            } else if (MessageBox.Show("This will clear all trouble codes from your vehicle.\n\n" + "You should have repaired any problems indicated by these codes.\n\n" + "Also, your vehicle may run poorly for a short time while the system " + "recalibrates itself.\n\nAre you sure you want to reset your codes?", "Clear Trouble Codes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                m_obdInterface.ClearCodes();
                RefreshDiagnosticData();
            }
        }

        private void DTCForm_Resize(object sender, EventArgs e) {
            int margin = groupPermanent.Location.Y;
            groupPermanent.Size = new Size(groupPermanent.Width, (Height - margin * 4) / 3);
            groupCodes.Size = new Size(groupCodes.Width, (Height - margin * 4) / 3);
            groupCodes.Location = new Point(groupCodes.Location.X, groupPermanent.Location.Y + groupPermanent.Height + margin);
            groupPending.Size = new Size(groupPending.Width, (Height - margin * 4) / 3);
            groupPending.Location = new Point(groupPending.Location.X, groupCodes.Location.Y + groupCodes.Height + margin);
        }

        private void DTCForm_Activated(object sender, EventArgs e) {
            CheckConnection();
        }

    }
}
