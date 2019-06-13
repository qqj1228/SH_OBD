using DGChart;
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
    public partial class ScopeForm : Form {
        protected OBDInterface m_obdInterface;
        private List<DatedValue> m_arraySensor1Values;
        private List<DatedValue> m_arraySensor2Values;
        private List<DatedValue> m_arraySensor3Values;
        private List<DatedValue> m_arraySensor4Values;
        private double m_dSensor1Max;
        private double m_dSensor2Max;
        private double m_dSensor3Max;
        private double m_dSensor4Max;
        private double m_dSensor1Min;
        private double m_dSensor2Min;
        private double m_dSensor3Min;
        private double m_dSensor4Min;
        private double[] dSensor1Values;
        private double[] dSensor1Times;
        private double[] dSensor2Values;
        private double[] dSensor2Times;
        private double[] dSensor3Values;
        private double[] dSensor3Times;
        private double[] dSensor4Values;
        private double[] dSensor4Times;
        public bool IsPlotting;

        public ScopeForm(OBDInterface obd2) {
            m_obdInterface = obd2;
            InitializeComponent();
        }

        private void ScopeForm_Resize(object sender, EventArgs e) {
            DrawCharts();
        }

        private void DrawCharts() {
            // DGChatControl定位参数
            const int marginLeft = 10;
            const int marginTop = 150;
            const int marginBottom = 10;

            int width = Width;
            int height = Height;
            int num1 = 0;
            if (chkSensor1.Checked)
                num1 = 1;
            if (chkSensor2.Checked)
                ++num1;
            if (chkSensor3.Checked)
                ++num1;
            if (chkSensor4.Checked)
                ++num1;
            if (num1 == 0) {
                chart1.Visible = false;
                chart2.Visible = false;
                chart3.Visible = false;
                chart4.Visible = false;
            } else if (num1 == 1) {
                chart1.Visible = true;
                chart2.Visible = false;
                chart3.Visible = false;
                chart4.Visible = false;
                chart1.Location = new Point(marginLeft, marginTop);
                chart1.Width = width - 2 * marginLeft;
                chart1.Height = height - marginTop - marginBottom;
            } else if (num1 == 2) {
                chart1.Visible = true;
                chart2.Visible = true;
                chart3.Visible = false;
                chart4.Visible = false;
                chart1.Location = new Point(marginLeft, marginTop);
                chart1.Width = width - 2 * marginLeft;
                int num2 = (height - marginTop - marginBottom) / 2;
                chart1.Height = num2;
                chart2.Location = new Point(marginLeft, chart1.Height + chart1.Location.Y);
                chart2.Width = width - 2 * marginLeft;
                chart2.Height = num2;
            } else if (num1 == 3) {
                chart1.Visible = true;
                chart2.Visible = true;
                chart3.Visible = true;
                chart4.Visible = false;
                chart1.Location = new Point(marginLeft, marginTop);
                int x = (width / 2) - marginLeft;
                chart1.Width = x;
                int num2 = (height - marginTop - marginBottom) / 2;
                chart1.Height = num2;
                chart2.Location = new Point(x + marginLeft, chart1.Location.Y);
                chart2.Width = x;
                chart2.Height = num2;
                chart3.Location = new Point(marginLeft, chart1.Height + chart1.Location.Y);
                chart3.Width = width - 2 * marginLeft;
                chart3.Height = num2;
            } else {
                if (num1 != 4)
                    return;
                chart1.Visible = true;
                chart2.Visible = true;
                chart3.Visible = true;
                chart4.Visible = true;
                chart1.Location = new Point(marginLeft, marginTop);
                int x = (width / 2) - marginLeft;
                chart1.Width = x;
                int num2 = (height - marginTop - marginBottom) / 2;
                chart1.Height = num2;
                chart2.Location = new Point(x + marginLeft, chart1.Location.Y);
                chart2.Width = x;
                chart2.Height = num2;
                chart3.Location = new Point(marginLeft, chart1.Height + chart1.Location.Y);
                chart3.Width = x;
                chart3.Height = num2;
                chart4.Location = new Point(x + marginLeft, chart1.Height + chart1.Location.Y);
                chart4.Width = x;
                chart4.Height = num2;
            }
        }

        private void btnStart_Click(object sender, EventArgs e) {
            if (!chkSensor1.Checked) {
                MessageBox.Show("You must configure at least one sensor to monitor.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else {
                chkSensor1.Enabled = false;
                chkSensor2.Enabled = false;
                chkSensor3.Enabled = false;
                chkSensor4.Enabled = false;
                comboSensor1.Enabled = false;
                comboSensor2.Enabled = false;
                comboSensor3.Enabled = false;
                comboSensor4.Enabled = false;
                comboUnits1.Enabled = false;
                comboUnits2.Enabled = false;
                comboUnits3.Enabled = false;
                comboUnits4.Enabled = false;
                comboStyle1.Enabled = false;
                comboStyle2.Enabled = false;
                comboStyle3.Enabled = false;
                comboStyle4.Enabled = false;
                numHistory.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                IsPlotting = true;
                Task.Factory.StartNew(UpdateThread);
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {
            StopLogging();
        }

        public void StopLogging() {
            numHistory.Enabled = true;
            chkSensor1.Enabled = true;
            if (chkSensor1.Checked) {
                chkSensor1.Enabled = true;
                chkSensor2.Enabled = true;
            }
            if (chkSensor2.Checked) {
                chkSensor2.Enabled = true;
                chkSensor3.Enabled = true;
            }
            if (chkSensor3.Checked) {
                chkSensor3.Enabled = true;
                chkSensor4.Enabled = true;
            }
            IsPlotting = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void ScopeForm_Load(object sender, EventArgs e) {
            IsPlotting = false;
            CheckConnection();
            chart1.XRangeStart = -30.0;
            chart1.XRangeEnd = 0.0;
            chart2.XRangeStart = -30.0;
            chart2.XRangeEnd = 0.0;
            chart3.XRangeStart = -30.0;
            chart3.XRangeEnd = 0.0;
            chart4.XRangeStart = -30.0;
            chart4.XRangeEnd = 0.0;
            chkSensor1.Enabled = true;
            comboSensor1.Enabled = true;
            comboStyle1.Enabled = true;
            comboUnits1.Enabled = true;
            m_arraySensor1Values = new List<DatedValue>();
            m_arraySensor2Values = new List<DatedValue>();
            m_arraySensor3Values = new List<DatedValue>();
            m_arraySensor4Values = new List<DatedValue>();
            comboUnits1.SelectedIndex = 0;
            comboUnits2.SelectedIndex = 0;
            comboUnits3.SelectedIndex = 0;
            comboUnits4.SelectedIndex = 0;
            comboStyle1.SelectedIndex = 0;
            comboStyle2.SelectedIndex = 0;
            comboStyle3.SelectedIndex = 0;
            comboStyle4.SelectedIndex = 0;
        }

        static bool isConnected;
        public bool CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                if (!isConnected) {
                    isConnected = true;
                    if (!IsPlotting)
                        btnStart.Enabled = true;
                    comboSensor1.Items.Clear();
                    comboSensor2.Items.Clear();
                    comboSensor3.Items.Clear();
                    comboSensor4.Items.Clear();
                    foreach (OBDParameter obdParameter in m_obdInterface.SupportedParameterList(1)) {
                        comboSensor1.Items.Add((object)obdParameter);
                        comboSensor2.Items.Add((object)obdParameter);
                        comboSensor3.Items.Add((object)obdParameter);
                        comboSensor4.Items.Add((object)obdParameter);
                    }
                }
                return true;
            } else {
                isConnected = false;
                IsPlotting = false;
                btnStart.Enabled = false;
                comboSensor1.Items.Clear();
                comboSensor2.Items.Clear();
                comboSensor3.Items.Clear();
                comboSensor4.Items.Clear();
                return false;
            }
        }

        public void UpdateThread() {
            if (!IsPlotting) {
                return;
            }
            do {
                if (CheckConnection()) {
                    bool bEnglishUnits = true;
                    if (comboUnits1.SelectedIndex > 0)
                        bEnglishUnits = false;
                    if (chkSensor1.Checked) {
                        OBDParameterValue obdParameterValue = m_obdInterface.getValue(comboSensor1.Items[comboSensor1.SelectedIndex] as OBDParameter, bEnglishUnits);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensor1Values.Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart1();
                        }
                    }
                    if (chkSensor2.Checked) {
                        OBDParameterValue obdParameterValue = m_obdInterface.getValue(comboSensor2.Items[comboSensor2.SelectedIndex] as OBDParameter, bEnglishUnits);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensor2Values.Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart2();
                        }
                    }
                    if (chkSensor3.Checked) {
                        OBDParameterValue obdParameterValue = m_obdInterface.getValue(comboSensor3.Items[comboSensor3.SelectedIndex] as OBDParameter, bEnglishUnits);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensor3Values.Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart3();
                        }
                    }
                    if (chkSensor4.Checked) {
                        OBDParameterValue obdParameterValue = m_obdInterface.getValue(comboSensor4.Items[comboSensor4.SelectedIndex] as OBDParameter, bEnglishUnits);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensor4Values.Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart4();
                        }
                    }
                }
            } while (IsPlotting);
        }

        public void UpdateChart1() {
            int index1 = 0;
            if (0 < m_arraySensor1Values.Count) {
                do {
                    if (DateTime.Now.Subtract(m_arraySensor1Values[index1].Date).TotalSeconds > (double)Convert.ToInt32(numHistory.Value)) {
                        m_arraySensor1Values.RemoveAt(index1);
                        --index1;
                    }
                    ++index1;
                } while (index1 < m_arraySensor1Values.Count);
            }
            if (m_arraySensor1Values.Count == 0) {
                return;
            }
            double[] numArray1 = new double[m_arraySensor1Values.Count];
            numArray1.Initialize();
            dSensor1Values = numArray1;
            double[] numArray2 = new double[m_arraySensor1Values.Count];
            numArray2.Initialize();
            dSensor1Times = numArray2;
            m_dSensor1Max = m_arraySensor1Values[0].Value;
            m_dSensor1Min = m_arraySensor1Values[0].Value;
            int index2 = 0;
            if (0 < m_arraySensor1Values.Count) {
                do {
                    DatedValue datedValue = m_arraySensor1Values[index2];
                    if (datedValue.Value > m_dSensor1Max) {
                        m_dSensor1Max = datedValue.Value;
                    }
                    if (datedValue.Value < m_dSensor1Min) {
                        m_dSensor1Min = datedValue.Value;
                    }
                    dSensor1Values[index2] = datedValue.Value;
                    TimeSpan timeSpan = DateTime.Now.Subtract(datedValue.Date);
                    dSensor1Times[index2] = timeSpan.TotalSeconds * -1.0;
                    ++index2;
                } while (index2 < m_arraySensor1Values.Count);
            }
            if (chart1.YRangeEnd < m_dSensor1Max) {
                chart1.YRangeEnd = Math.Ceiling(m_dSensor1Max);
                chart1.YGrid = (chart1.YRangeEnd - chart1.YRangeStart) * 0.1;
            }
            if (chart1.YRangeStart > m_dSensor1Min) {
                chart1.YRangeStart = Math.Floor(m_dSensor1Min);
                chart1.YGrid = (chart1.YRangeEnd - chart1.YRangeStart) * 0.1;
            }
            chart1.XData1 = dSensor1Times;
            chart1.YData1 = dSensor1Values;
        }

        public void UpdateChart2() {
            int index1 = 0;
            if (0 < m_arraySensor2Values.Count) {
                do {
                    if (DateTime.Now.Subtract(m_arraySensor2Values[index1].Date).TotalSeconds > (double)Convert.ToInt32(numHistory.Value)) {
                        m_arraySensor2Values.RemoveAt(index1);
                        --index1;
                    }
                    ++index1;
                } while (index1 < m_arraySensor2Values.Count);
            }
            if (m_arraySensor2Values.Count == 0) {
                return;
            }
            double[] numArray1 = new double[m_arraySensor2Values.Count];
            numArray1.Initialize();
            dSensor2Values = numArray1;
            double[] numArray2 = new double[m_arraySensor2Values.Count];
            numArray2.Initialize();
            dSensor2Times = numArray2;
            m_dSensor2Max = m_arraySensor2Values[0].Value;
            m_dSensor2Min = m_arraySensor2Values[0].Value;
            int index2 = 0;
            if (0 < m_arraySensor2Values.Count) {
                do {
                    DatedValue datedValue = m_arraySensor2Values[index2];
                    if (datedValue.Value > m_dSensor2Max)
                        m_dSensor2Max = datedValue.Value;
                    if (datedValue.Value < m_dSensor2Min)
                        m_dSensor2Min = datedValue.Value;
                    dSensor2Values[index2] = datedValue.Value;
                    TimeSpan timeSpan = DateTime.Now.Subtract(datedValue.Date);
                    dSensor2Times[index2] = timeSpan.TotalSeconds * -1.0;
                    ++index2;
                } while (index2 < m_arraySensor2Values.Count);
            }
            if (chart2.YRangeEnd < m_dSensor2Max) {
                chart2.YRangeEnd = Math.Ceiling(m_dSensor2Max);
                chart2.YGrid = (chart2.YRangeEnd - chart2.YRangeStart) * 0.1;
            }
            if (chart2.YRangeStart > m_dSensor2Min) {
                chart2.YRangeStart = Math.Floor(m_dSensor2Min);
                chart2.YGrid = (chart2.YRangeEnd - chart2.YRangeStart) * 0.1;
            }
            chart2.XData1 = dSensor2Times;
            chart2.YData1 = dSensor2Values;
        }

        public void UpdateChart3() {
            int index1 = 0;
            if (0 < m_arraySensor3Values.Count) {
                do {
                    if (DateTime.Now.Subtract(m_arraySensor3Values[index1].Date).TotalSeconds > (double)Convert.ToInt32(numHistory.Value)) {
                        m_arraySensor3Values.RemoveAt(index1);
                        --index1;
                    }
                    ++index1;
                } while (index1 < m_arraySensor3Values.Count);
            }
            if (m_arraySensor3Values.Count == 0) {
                return;
            }
            double[] numArray1 = new double[m_arraySensor3Values.Count];
            numArray1.Initialize();
            dSensor3Values = numArray1;
            double[] numArray2 = new double[m_arraySensor3Values.Count];
            numArray2.Initialize();
            dSensor3Times = numArray2;
            m_dSensor3Max = m_arraySensor3Values[0].Value;
            m_dSensor3Min = m_arraySensor3Values[0].Value;
            int index2 = 0;
            if (0 < m_arraySensor3Values.Count) {
                do {
                    DatedValue datedValue = m_arraySensor3Values[index2];
                    if (datedValue.Value > m_dSensor3Max)
                        m_dSensor3Max = datedValue.Value;
                    if (datedValue.Value < m_dSensor3Min)
                        m_dSensor3Min = datedValue.Value;
                    dSensor3Values[index2] = datedValue.Value;
                    TimeSpan timeSpan = DateTime.Now.Subtract(datedValue.Date);
                    dSensor3Times[index2] = timeSpan.TotalSeconds * -1.0;
                    ++index2;
                } while (index2 < m_arraySensor3Values.Count);
            }
            if (chart3.YRangeEnd < m_dSensor3Max) {
                chart3.YRangeEnd = Math.Ceiling(m_dSensor3Max);
                chart3.YGrid = (chart3.YRangeEnd - chart3.YRangeStart) * 0.1;
            }
            if (chart3.YRangeStart > m_dSensor3Min) {
                chart3.YRangeStart = Math.Floor(m_dSensor3Min);
                chart3.YGrid = (chart3.YRangeEnd - chart3.YRangeStart) * 0.1;
            }
            chart3.XData1 = dSensor3Times;
            chart3.YData1 = dSensor3Values;
        }

        public void UpdateChart4() {
            int index1 = 0;
            if (0 < m_arraySensor4Values.Count) {
                do {
                    if (DateTime.Now.Subtract(m_arraySensor4Values[index1].Date).TotalSeconds > (double)Convert.ToInt32(numHistory.Value)) {
                        m_arraySensor4Values.RemoveAt(index1);
                        --index1;
                    }
                    ++index1;
                } while (index1 < m_arraySensor4Values.Count);
            }
            if (m_arraySensor4Values.Count == 0) {
                return;
            }
            double[] numArray1 = new double[m_arraySensor4Values.Count];
            numArray1.Initialize();
            dSensor4Values = numArray1;
            double[] numArray2 = new double[m_arraySensor4Values.Count];
            numArray2.Initialize();
            dSensor4Times = numArray2;
            m_dSensor4Max = m_arraySensor4Values[0].Value;
            m_dSensor4Min = m_arraySensor4Values[0].Value;
            int index2 = 0;
            if (0 < m_arraySensor4Values.Count) {
                do {
                    DatedValue datedValue = m_arraySensor4Values[index2];
                    if (datedValue.Value > m_dSensor4Max)
                        m_dSensor4Max = datedValue.Value;
                    if (datedValue.Value < m_dSensor4Min)
                        m_dSensor4Min = datedValue.Value;
                    dSensor4Values[index2] = datedValue.Value;
                    TimeSpan timeSpan = DateTime.Now.Subtract(datedValue.Date);
                    dSensor4Times[index2] = timeSpan.TotalSeconds * -1.0;
                    ++index2;
                } while (index2 < m_arraySensor4Values.Count);
            }
            if (chart4.YRangeEnd < m_dSensor4Max) {
                chart4.YRangeEnd = Math.Ceiling(m_dSensor4Max);
                chart4.YGrid = (chart4.YRangeEnd - chart4.YRangeStart) * 0.1;
            }
            if (chart4.YRangeStart > m_dSensor4Min) {
                chart4.YRangeStart = Math.Floor(m_dSensor4Min);
                chart4.YGrid = (chart4.YRangeEnd - chart4.YRangeStart) * 0.1;
            }
            chart4.XData1 = dSensor4Times;
            chart4.YData1 = dSensor4Values;
        }

        private void numHistory_ValueChanged(object sender, EventArgs e) {
            chart1.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart2.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart3.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart4.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
        }

        private void comboStyle1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void comboStyle2_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void comboStyle3_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void comboStyle4_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void chkSensor1_CheckedChanged(object sender, EventArgs e) {
            DrawCharts();
            if (chkSensor1.Checked) {
                chkSensor2.Enabled = true;
            } else {
                chkSensor2.Checked = false;
                chkSensor3.Checked = false;
                chkSensor4.Checked = false;
                chkSensor2.Enabled = false;
                chkSensor3.Enabled = false;
                chkSensor4.Enabled = false;
            }
        }

        private void chkSensor2_CheckedChanged(object sender, EventArgs e) {
            DrawCharts();
            if (chkSensor2.Checked) {
                chkSensor3.Enabled = true;
            } else {
                chkSensor3.Checked = false;
                chkSensor4.Checked = false;
                chkSensor3.Enabled = false;
                chkSensor4.Enabled = false;
            }
        }

        private void chkSensor3_CheckedChanged(object sender, EventArgs e) {
            DrawCharts();
            if (chkSensor3.Checked) {
                chkSensor4.Enabled = true;
            } else {
                chkSensor4.Checked = false;
                chkSensor4.Enabled = false;
            }
        }

        private void chkSensor4_CheckedChanged(object sender, EventArgs e) {
            DrawCharts();
        }

        private void chkSensor1_EnabledChanged(object sender, EventArgs e) {
            if (chkSensor1.Enabled) {
                comboSensor1.Enabled = true;
                comboUnits1.Enabled = true;
                comboStyle1.Enabled = true;
            } else {
                comboSensor1.Enabled = false;
                comboUnits1.Enabled = false;
                comboStyle1.Enabled = false;
            }
        }

        private void chkSensor2_EnabledChanged(object sender, EventArgs e) {
            if (chkSensor2.Enabled) {
                comboSensor2.Enabled = true;
                comboUnits2.Enabled = true;
                comboStyle2.Enabled = true;
            } else {
                comboSensor2.Enabled = false;
                comboUnits2.Enabled = false;
                comboStyle2.Enabled = false;
            }
        }

        private void chkSensor3_EnabledChanged(object sender, EventArgs e) {
            if (chkSensor3.Enabled) {
                comboSensor3.Enabled = true;
                comboUnits3.Enabled = true;
                comboStyle3.Enabled = true;
            } else {
                comboSensor3.Enabled = false;
                comboUnits3.Enabled = false;
                comboStyle3.Enabled = false;
            }
        }

        private void chkSensor4_EnabledChanged(object sender, EventArgs e) {
            if (chkSensor4.Enabled) {
                comboSensor4.Enabled = true;
                comboUnits4.Enabled = true;
                comboStyle4.Enabled = true;
            } else {
                comboSensor4.Enabled = false;
                comboUnits4.Enabled = false;
                comboStyle4.Enabled = false;
            }
        }

        private void comboSensorOrUnits1_SelectedIndexChanged(object sender, EventArgs e) {
            m_arraySensor1Values.Clear();
            chart1.XData1 = (double[])null;
            chart1.YData1 = (double[])null;
            if (comboSensor1.Items.Count == 0) {
                return;
            }
            int selectedIndex = comboSensor1.SelectedIndex;
            if (selectedIndex < 0) {
                return;
            }
            OBDParameter obdParameter = (OBDParameter)comboSensor1.Items[selectedIndex];
            if (comboUnits1.SelectedIndex == 0) {
                chart1.Text = obdParameter.Name + " (" + obdParameter.EnglishUnitLabel + ")";
                chart1.YRangeStart = Math.Floor(obdParameter.EnglishMinValue);
                chart1.YRangeEnd = obdParameter.EnglishMinValue + 1.0;
            } else {
                chart1.Text = obdParameter.Name + " (" + obdParameter.MetricUnitLabel + ")";
                chart1.YRangeStart = Math.Floor(obdParameter.MetricMinValue);
                chart1.YRangeEnd = obdParameter.MetricMinValue + 1.0;
            }
        }

        private void comboSensorOrUnits2_SelectedIndexChanged(object sender, EventArgs e) {
            m_arraySensor2Values.Clear();
            chart2.XData1 = (double[])null;
            chart2.YData1 = (double[])null;
            if (comboSensor2.Items.Count == 0) {
                return;
            }
            int selectedIndex = comboSensor2.SelectedIndex;
            if (selectedIndex < 0) {
                return;
            }
            OBDParameter obdParameter = comboSensor2.Items[selectedIndex] as OBDParameter;
            if (comboUnits2.SelectedIndex == 0) {
                chart2.Text = obdParameter.Name + " (" + obdParameter.EnglishUnitLabel + ")";
                chart2.YRangeStart = Math.Floor(obdParameter.EnglishMinValue);
                chart2.YRangeEnd = obdParameter.EnglishMinValue + 1.0;
            } else {
                chart2.Text = obdParameter.Name + " (" + obdParameter.MetricUnitLabel + ")";
                chart2.YRangeStart = Math.Floor(obdParameter.MetricMinValue);
                chart2.YRangeEnd = obdParameter.MetricMinValue + 1.0;
            }
        }

        private void comboSensorOrUnits3_SelectedIndexChanged(object sender, EventArgs e) {
            m_arraySensor3Values.Clear();
            chart3.XData1 = (double[])null;
            chart3.YData1 = (double[])null;
            if (comboSensor3.Items.Count == 0) {
                return;
            }
            int selectedIndex = comboSensor3.SelectedIndex;
            if (selectedIndex < 0) {
                return;
            }
            OBDParameter obdParameter = comboSensor3.Items[selectedIndex] as OBDParameter;
            if (comboUnits3.SelectedIndex == 0) {
                chart3.Text = obdParameter.Name + " (" + obdParameter.EnglishUnitLabel + ")";
                chart3.YRangeStart = Math.Floor(obdParameter.EnglishMinValue);
                chart3.YRangeEnd = obdParameter.EnglishMinValue + 1.0;
            } else {
                chart3.Text = obdParameter.Name + " (" + obdParameter.MetricUnitLabel + ")";
                chart3.YRangeStart = Math.Floor(obdParameter.MetricMinValue);
                chart3.YRangeEnd = obdParameter.MetricMinValue + 1.0;
            }
        }

        private void comboSensorOrUnits4_SelectedIndexChanged(object sender, EventArgs e) {
            m_arraySensor4Values.Clear();
            chart4.XData1 = (double[])null;
            chart4.YData1 = (double[])null;
            if (comboSensor4.Items.Count == 0) {
                return;
            }
            int selectedIndex = comboSensor4.SelectedIndex;
            if (selectedIndex < 0) {
                return;
            }
            OBDParameter obdParameter = comboSensor4.Items[selectedIndex] as OBDParameter;
            if (comboUnits4.SelectedIndex == 0) {
                chart4.Text = obdParameter.Name + " (" + obdParameter.EnglishUnitLabel + ")";
                chart4.YRangeStart = Math.Floor(obdParameter.EnglishMinValue);
                chart4.YRangeEnd = obdParameter.EnglishMinValue + 1.0;
            } else {
                chart4.Text = obdParameter.Name + " (" + obdParameter.MetricUnitLabel + ")";
                chart4.YRangeStart = Math.Floor(obdParameter.MetricMinValue);
                chart4.YRangeEnd = obdParameter.MetricMinValue + 1.0;
            }
        }

        private void ScopeForm_Activated(object sender, EventArgs e) {
            CheckConnection();
        }

    }
}
