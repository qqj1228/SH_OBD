using System;
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
    public partial class DynoForm : Form {
        private OBDInterface m_obdInterface;
        private VehicleProfile m_profile;
        private double m_HPMax;
        private double m_TQMax;
        private bool m_Capture;
        private List<DatedValue> m_RpmValues;
        private List<DatedValue> m_KphValues;
        private double[] m_HPValue;
        private double[] m_TQValue;
        private double[] m_SampleRPM;
        private double m_dVehicleWeight;
        private DateTime m_dtDynoTime;

        public DynoForm(OBDInterface obd) {
            m_obdInterface = obd;
            m_profile = obd.ActiveProfile;

            InitializeComponent();

            m_Capture = false;
            m_dtDynoTime = DateTime.Now;
            btnStart.Enabled = false;
            btnReset.Enabled = false;
            printDocument.DefaultPageSettings = new PageSettings {
                Margins = new Margins(100, 100, 100, 100),
                Landscape = true
            };
            pageSetupDialog.Document = printDocument;
        }

        private void btnStart_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("必须首先与车辆进行连接，才能进行后续操作！", "连接请求", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                m_dtDynoTime = DateTime.Now;
                m_dVehicleWeight = m_profile.Weight;
                dyno.Label = m_profile.Name + "\r\n" + m_dtDynoTime.ToString("g");
                btnStart.Enabled = false;
                btnReset.Enabled = true;
                btnOpen.Enabled = false;
                m_Capture = true;
                Task.Factory.StartNew(Capture);
            }
        }

        private void DynoForm_Closing(object sender, CancelEventArgs e) {
            m_Capture = false;
        }

        private new void Capture() {
            m_RpmValues = new List<DatedValue>();
            m_KphValues = new List<DatedValue>();
            OBDParameterValue value;
            DatedValue d_value;
            while (m_Capture) {
                value = m_obdInterface.GetValue("SAE.RPM", true);
                if (!value.ErrorDetected) {
                    d_value = new DatedValue(value.DoubleValue) {
                        Date = DateTime.Now
                    };
                    if (Convert.ToDecimal(d_value.Value) >= numFromRPM.Value && Convert.ToDecimal(d_value.Value) <= numToRPM.Value) {
                        m_RpmValues.Add(d_value);
                        value = m_obdInterface.GetValue("SAE.VSS", false);
                        if (!value.ErrorDetected) {
                            d_value = new DatedValue(value.DoubleValue * (double)m_obdInterface.ActiveProfile.SpeedCalibrationFactor) {
                                Date = DateTime.Now
                            };
                            m_KphValues.Add(d_value);
                        }
                        //m_Capture = false;
                    }
                }
            }
            this.Invoke((EventHandler)delegate {
                Calculate();
                btnOpen.Enabled = true;
            });
        }

        private void Calculate() {
            if (m_RpmValues.Count < 1) {
                return;
            }
            m_TQMax = 0.0;
            m_HPMax = 0.0;
            m_HPValue = new double[m_RpmValues.Count - 1];
            m_TQValue = new double[m_RpmValues.Count - 1];
            m_SampleRPM = new double[m_RpmValues.Count - 1];
            int idx = 0;
            if ((m_RpmValues.Count - 1) > 0) {
                do {
                    double rpm_value_0 = m_RpmValues[idx].Value;
                    int next = idx + 1;
                    double rpm_value_1 = m_RpmValues[next].Value;
                    DateTime rpm_date_0 = m_RpmValues[idx].Date;
                    TimeSpan delta_time = m_RpmValues[next].Date.Subtract(rpm_date_0);
                    rpm_date_0.AddSeconds(delta_time.TotalSeconds * 0.5);

                    double kph_value_0 = m_KphValues[idx].Value;
                    double kph_value_1 = m_KphValues[next].Value;
                    DateTime kph_date_0 = m_KphValues[idx].Date;
                    DateTime kph_date_1 = m_KphValues[next].Date;

                    m_SampleRPM[idx] = (rpm_value_1 + rpm_value_0) * 0.5 * 0.001;
                    double num5 = (kph_value_1 + kph_value_0) * 0.5 * 0.621371192 * 0.44704;
                    double kw_value = m_dVehicleWeight * 0.45359237 * num5 * num5 * 0.5 / kph_date_1.Subtract(kph_date_0).TotalSeconds;

                    m_HPValue[idx] = kw_value * 0.00134102209;
                    if (m_HPValue[idx] > m_HPMax) {
                        m_HPMax = m_HPValue[idx];
                    }

                    m_TQValue[idx] = m_HPValue[idx] * 5252.0 / (m_SampleRPM[idx] * 1000.0);

                    if (m_TQValue[idx] > m_TQMax) {
                        m_TQMax = m_TQValue[idx];
                    }
                    idx = next;
                } while (idx < m_RpmValues.Count - 1);
            }
            dyno.XData1 = m_SampleRPM;
            dyno.YData1 = m_HPValue;
            dyno.XData2 = m_SampleRPM;
            dyno.YData2 = m_TQValue;
            if (m_HPMax != 0.0 && m_TQMax != 0.0) {
                dyno.YRangeEnd = m_HPMax < m_TQMax ? m_TQMax : m_HPMax;
                dyno.YRangeEnd = (double)Convert.ToInt32(dyno.YRangeEnd + 0.5);
                if (Convert.ToInt32(dyno.YRangeEnd) % Convert.ToInt32(dyno.YGrid) != 0) {
                    do {
                        dyno.YRangeEnd = dyno.YRangeEnd + 1.0;
                    } while (Convert.ToInt32(dyno.YRangeEnd) % Convert.ToInt32(dyno.YGrid) != 0);
                }
            }
            dyno.ShowData1 = true;
            dyno.ShowData2 = true;
            dyno.Refresh();
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            if (printDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            printDocument.Print();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
            float num1 = (float)e.MarginBounds.Left;
            float num2 = (float)e.MarginBounds.Right;
            float num3 = (float)e.MarginBounds.Top;
            int bottom = e.MarginBounds.Bottom;
            float num4 = num2 - num1;
            e.Graphics.DrawImage(dyno.GetImage(), Convert.ToInt32(num1), Convert.ToInt32(num3), Convert.ToInt32(num4), Convert.ToInt32(num4 * 0.6666667f));
        }

        private Image GetDynoImage() {
            Bitmap bitmap = new Bitmap(dyno.Width, dyno.Height);
            Graphics.FromImage((Image)bitmap).DrawImage(dyno.GetImage(), 0, 0, dyno.Width, dyno.Height);
            return (Image)bitmap;
        }

        private void Print() {
            if (printDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            printDocument.Print();
        }

        private void btnExportJPEG_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "作为JPEG图片输出";
            dialog.Filter = "JPEG文件 (*.jpg)|*.jpg";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            dialog.ShowDialog();
            if (dialog.FileName != "") {
                GetDynoImage().Save(dialog.FileName, ImageFormat.Jpeg);
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {
            m_Capture = false;
            m_HPValue = (double[])null;
            m_TQValue = (double[])null;
            m_SampleRPM = (double[])null;
            dyno.XData1 = m_SampleRPM;
            dyno.YData1 = m_HPValue;
            dyno.XData2 = m_SampleRPM;
            dyno.YData2 = m_TQValue;
            btnStart.Enabled = true;
            btnReset.Enabled = false;
            btnOpen.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (m_RpmValues == null)
                MessageBox.Show("You must first perform a dyno pull or open a previous session.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            else {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save Dyno Session";
                dialog.Filter = "Dyno files (*.dyn)|*.dyn";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;
                dialog.ShowDialog();
                if (dialog.FileName != "") {
                    DynoRecord record = new DynoRecord();
                    record.RpmList = m_RpmValues;
                    record.Weight = m_dVehicleWeight;
                    record.Label = dyno.Label;
                    Type[] typeArray = new Type[] { typeof(List<DatedValue>), typeof(DatedValue) };
                    using (TextWriter writer = new StreamWriter(dialog.FileName)) {
                        new XmlSerializer(typeof(DynoRecord), typeArray).Serialize(writer, record);
                        writer.Close();
                    }
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dyno files (*.dyn)|*.dyn";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(
                typeof(DynoRecord),
                new System.Type[] { typeof(List<DatedValue>), typeof(DatedValue) }
                );
            DynoRecord dynoRecord;
            using (FileStream reader = new FileStream(openFileDialog.FileName, FileMode.Open)) {
                dynoRecord = (DynoRecord)xmlSerializer.Deserialize(reader);
                reader.Close();
            }
            m_dVehicleWeight = dynoRecord.Weight;
            dyno.Label = dynoRecord.Label;
            m_RpmValues = dynoRecord.RpmList;
            Calculate();
        }

        private void numFromRPM_ValueChanged(object sender, EventArgs e) {
            dyno.XRangeStart = Convert.ToDouble(numFromRPM.Value / 1000M);
            numToRPM.Minimum = numFromRPM.Value + 1000M;
        }

        private void numToRPM_ValueChanged(object sender, EventArgs e) {
            dyno.XRangeEnd = Convert.ToDouble(numToRPM.Value / 1000M);
            numFromRPM.Maximum = numToRPM.Value - 1000M;
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                btnStart.Enabled = true;
                btnReset.Enabled = true;
            } else {
                btnStart.Enabled = false;
                btnReset.Enabled = false;
            }
        }

        private void DynoForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }
    }
}
