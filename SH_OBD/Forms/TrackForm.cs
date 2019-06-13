﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SH_OBD {
    public partial class TrackForm : Form {
        private static long m_iClockStartTicks;
        private static double m_dClock;

        private OBDInterface m_obdInterface;
        private Timeslip timeslip;
        private List<DatedValue> m_KphValues;
        private bool m_bCapture;

        public TrackForm(OBDInterface obd) {
            m_obdInterface = obd;
            timeslip = new Timeslip();
            timeslip.Vehicle = m_obdInterface.ActiveProfile.Name;
            InitializeComponent();
            CheckConnection();
        }

        private void TrackForm_Resize(object sender, EventArgs e) {
            UpdateTimeslip();
            Refresh();
        }

        private void UpdateTimeslip() {
            float emSize = richTextSlip.Width * 0.02f;
            if (emSize < 8f)
                emSize = 8f;

            richTextSlip.Text = "";
            richTextSlip.SelectionAlignment = HorizontalAlignment.Center;

            richTextSlip.SelectionFont = new Font("Courier New", emSize + 2f, FontStyle.Bold);
            richTextSlip.SelectionColor = Color.Blue;
            richTextSlip.AppendText("\r\nProScan Drag Strip\r\n\r\n");

            richTextSlip.SelectionFont = new Font("Courier New", emSize, FontStyle.Bold);
            richTextSlip.SelectionColor = Color.Black;
            richTextSlip.AppendText(timeslip.Vehicle + "\r\n");

            richTextSlip.SelectionFont = new Font("Courier New", emSize, FontStyle.Regular);
            richTextSlip.SelectionColor = Color.Black;
            richTextSlip.AppendText(timeslip.Date.ToLongDateString() + "\r\n" + timeslip.Date.ToLongTimeString() + "\r\n\r\n");

            richTextSlip.SelectionFont = new Font("Courier New", emSize, FontStyle.Regular);
            richTextSlip.SelectionColor = Color.Black;
            richTextSlip.AppendText(timeslip.getStats());

        }

        private void TrackForm_Paint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(picTrack.Image, 0, 0, picTrack.Width, picTrack.Height);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            RectangleF rect = new RectangleF(2.5f, 2.5f, Convert.ToSingle(base.Width), 100f);
            e.Graphics.DrawString(m_dClock.ToString("00.000"), new Font("Verdana", 40f), new SolidBrush(Color.Black), rect, format);
            rect = new RectangleF(0f, 0f, Convert.ToSingle(base.Width), 100f);
            e.Graphics.DrawString(m_dClock.ToString("00.000"), new Font("Verdana", 40f), new SolidBrush(Color.White), rect, format);
        }


        private void timerClock_Tick(object sender, EventArgs e) {
            m_dClock = Convert.ToDouble((long)(DateTime.Now.Ticks - m_iClockStartTicks)) * 1E-07;
            Graphics graphics = base.CreateGraphics();
            graphics.DrawImage(picTrack.Image, 0, 0, picTrack.Width, picTrack.Height);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            RectangleF rect = new RectangleF(2.5f, 2.5f, Convert.ToSingle(base.Width), 100f);
            graphics.DrawString(m_dClock.ToString("00.000"), new Font("Verdana", 40f), new SolidBrush(Color.Black), rect, format);
            rect = new RectangleF(0f, 0f, Convert.ToSingle(base.Width), 100f);
            graphics.DrawString(m_dClock.ToString("00.000"), new Font("Verdana", 40f), new SolidBrush(Color.White), rect, format);
        }

        private void StartTimer() {
            m_iClockStartTicks = DateTime.Now.Ticks;
            timerClock1.Enabled = true;
        }

        private void StopTimer() {
            timerClock1.Enabled = false;
            m_dClock = 0.0;
            Invalidate();
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                btnStage.Enabled = true;
                btnReset.Enabled = true;
            } else {
                btnStage.Enabled = false;
                btnReset.Enabled = false;
            }
        }

        private void btnStage_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show(string.Concat((object)"A vehicle connection must first be established."), "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                btnStage.Enabled = false;
                btnReset.Enabled = true;
                btnOpen.Enabled = false;
                m_bCapture = true;
                Task.Factory.StartNew(Capture);
            }
        }

        private new void Capture() {
            m_KphValues = new List<DatedValue>();
            if (m_bCapture) {
                do {
                    OBDParameterValue obdParameterValue = m_obdInterface.getValue("SAE.VSS", false);

                    if (!obdParameterValue.ErrorDetected && obdParameterValue.DoubleValue > 0.0) {
                        if (m_KphValues.Count == 0) {
                            this.BeginInvoke((EventHandler)delegate {
                                StartTimer();
                            });
                        }
                        m_KphValues.Add(new DatedValue(obdParameterValue.DoubleValue * (double)m_obdInterface.ActiveProfile.SpeedCalibrationFactor));
                        this.CalculateTimeslip();
                    }
                } while (m_bCapture);
            }
            this.BeginInvoke((EventHandler)delegate {
                StopTimer();
            });
            btnOpen.Enabled = true;
        }

        private void CalculateTimeslip() {
            double num1 = 0.0;
            double num2 = 0.0;
            bool flag = false;
            timeslip = new Timeslip();
            timeslip.Vehicle = m_obdInterface.ActiveProfile.Name;
            if (m_KphValues == null || m_KphValues.Count == 0) {
                return;
            }
            timeslip.Vehicle = m_obdInterface.ActiveProfile.Name;
            timeslip.Date = m_KphValues[0].Date;
            if (m_KphValues.Count <= 1) {
                return;
            }

            int index = 1;
            do {
                DatedValue datedValue1 = m_KphValues[index - 1];
                DatedValue datedValue2 = m_KphValues[index];
                double num3 = (datedValue2.Value + datedValue1.Value) * 0.5 * (5.0 / 18.0);
                DateTime dateTime = datedValue1.Date;
                double totalSeconds = datedValue2.Date.Subtract(dateTime).TotalSeconds;
                num1 += totalSeconds;
                double num4 = totalSeconds * num3;
                num2 += num4;
                if (num2 >= 18.288 && timeslip.SixtyFootTime == 0.0) {
                    timeslip.SixtyFootTime = num1 - (num2 - 18.288) / num3;
                    flag = true;
                }
                if (datedValue2.Value >= 96.56064 && timeslip.SixtyMphTime == 0.0) {
                    timeslip.SixtyMphTime = num1 - (datedValue2.Value - 96.56064) / ((datedValue2.Value - datedValue1.Value) / totalSeconds);
                    flag = true;
                }
                if (num2 >= 201.168 && timeslip.EighthMileTime == 0.0) {
                    double num5 = num2 - 201.168;
                    timeslip.EighthMileTime = num1 - num5 / num3;
                    timeslip.EighthMileSpeed = ((datedValue2.Value - datedValue1.Value) * ((num4 - num5) / num4) + datedValue1.Value) * 0.621371192;
                    flag = true;
                }
                if (num2 >= 304.8 && timeslip.ThousandFootTime == 0.0) {
                    timeslip.ThousandFootTime = num1 - (num2 - 304.8) / num3;
                    flag = true;
                }
                if (num2 >= 402.336 && timeslip.QuarterMileTime == 0.0) {
                    double num5 = num2 - 402.336;
                    timeslip.QuarterMileTime = num1 - num5 / num3;
                    timeslip.QuarterMileSpeed = ((datedValue2.Value - datedValue1.Value) * ((num4 - num5) / num4) + datedValue1.Value) * 0.621371192;
                    flag = true;
                    m_bCapture = false;
                }
                ++index;
            } while (index < m_KphValues.Count);
            if (!flag) {
                return;
            }
            UpdateTimeslip();
        }

        private void btnReset_Click(object sender, EventArgs e) {
            m_bCapture = false;
            btnStage.Enabled = true;
            StopTimer();
            timeslip = new Timeslip();
            timeslip.Vehicle = m_obdInterface.ActiveProfile.Name;
            UpdateTimeslip();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Timeslip";
            dialog.Filter = "Timeslip files (*.slp)|*.slp";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            dialog.ShowDialog();
            if (dialog.FileName != "") {
                using (TextWriter writer = new StreamWriter(dialog.FileName)) {
                    new XmlSerializer(timeslip.GetType()).Serialize(writer, timeslip);
                    writer.Close();
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Timeslip files (*.slp)|*.slp";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                XmlSerializer xmlSerializer = new XmlSerializer(timeslip.GetType());
                using (FileStream reader = new FileStream(openFileDialog.FileName, FileMode.Open)) {
                    timeslip = (Timeslip)xmlSerializer.Deserialize(reader);
                    reader.Close();
                }
                UpdateTimeslip();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            if (printDialog1.ShowDialog() != DialogResult.OK) {
                return;
            }
            printDocument.Print();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
            float left = (float)e.MarginBounds.Left;
            float right = (float)e.MarginBounds.Right;
            float top = (float)e.MarginBounds.Top;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            RectangleF layoutRectangle1 = new RectangleF();
            float width = right - left;
            layoutRectangle1 = new RectangleF(left, top, width, 20f);
            RectangleF layoutRectangle2 = new RectangleF();
            layoutRectangle2 = new RectangleF(left, top + 40f, width, 17.5f);
            RectangleF layoutRectangle3 = new RectangleF();
            layoutRectangle3 = new RectangleF(left, top + 55f, width, 40f);
            RectangleF layoutRectangle4 = new RectangleF();
            layoutRectangle4 = new RectangleF(left, top + 110f, width, 140f);
            e.Graphics.DrawString("SH_OBD Drag Strip", new Font("Courier New", 14f, FontStyle.Bold), Brushes.Blue, layoutRectangle1, format);
            e.Graphics.DrawString("2000 Camaro SS", new Font("Courier New", 12f, FontStyle.Bold), Brushes.Black, layoutRectangle2, format);

            string s = DateTime.Now.ToLongDateString() + "\r\n" + DateTime.Now.ToLongTimeString() + "\r\n\r\n";
            Font font = new Font("Courier New", 12f, FontStyle.Regular);
            e.Graphics.DrawString(s, font, Brushes.Black, layoutRectangle3, format);
            e.Graphics.DrawString(timeslip.getStats(), font, Brushes.Black, layoutRectangle4, format);
        }

        private void btnExportJPEG_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export as JPEG";
            saveFileDialog.Filter = "JPEG files (*.jpg)|*.jpg";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            int num = (int)saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "") {
                Bitmap bitmap = new Bitmap(500, 300);
                Graphics graphics = Graphics.FromImage((Image)bitmap);
                Font font1 = new Font("Courier New", 14f, FontStyle.Bold);
                Font font2 = new Font("Courier New", 12f, FontStyle.Bold);
                Font font3 = new Font("Courier New", 12f, FontStyle.Regular);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                RectangleF layoutRectangle1 = new RectangleF();
                layoutRectangle1 = new RectangleF(10f, 25f, 480f, 20f);
                RectangleF layoutRectangle2 = new RectangleF();
                layoutRectangle2 = new RectangleF(10f, 65f, 480f, 17.5f);
                RectangleF layoutRectangle3 = new RectangleF();
                layoutRectangle3 = new RectangleF(10f, 80f, 480f, 40f);
                RectangleF layoutRectangle4 = new RectangleF();
                layoutRectangle4 = new RectangleF(10f, 135f, 480f, 140f);

                graphics.FillRectangle((Brush)new SolidBrush(Color.White), 0, 0, 500, 600);
                graphics.DrawString("ProScan Drag Strip", font1, Brushes.Blue, layoutRectangle1, format);
                graphics.DrawString(timeslip.Vehicle, font2, Brushes.Black, layoutRectangle2, format);
                string s = timeslip.Date.ToLongDateString() + "\r\n" + timeslip.Date.ToLongTimeString() + "\r\n\r\n";
                graphics.DrawString(s, font3, Brushes.Black, layoutRectangle3, format);
                graphics.DrawString(timeslip.getStats(), font3, Brushes.Black, layoutRectangle4, format);
                (bitmap as Image).Save(saveFileDialog.FileName, ImageFormat.Jpeg);
            }
        }

        private void TrackForm_Activated(object sender, EventArgs e) {
            CheckConnection();
        }

    }
}
