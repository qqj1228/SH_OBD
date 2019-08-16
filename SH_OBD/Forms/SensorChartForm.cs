using DGChart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class SensorChartForm : Form {
        protected OBDInterface m_obdInterface;
        private readonly List<DatedValue>[] m_arraySensorValues;
        private readonly double[] m_dSensorMax;
        private readonly double[] m_dSensorMin;
        private readonly double[][] dSensorValues;
        private readonly double[][] dSensorTimes;
        private readonly bool[] sensorsEnable;
        private readonly int[] sensorsIndex;
        private readonly int[] unitsIndex;
        public bool IsPlotting;

        public SensorChartForm(OBDInterface obd2) {
            m_obdInterface = obd2;
            InitializeComponent();
            m_arraySensorValues = new List<DatedValue>[4];
            for (int i = 0; i < m_arraySensorValues.Length; i++) {
                m_arraySensorValues[i] = new List<DatedValue>();
            }
            m_dSensorMax = new double[4];
            m_dSensorMin = new double[4];
            dSensorValues = new double[4][];
            dSensorTimes = new double[4][];
            sensorsEnable = new bool[4];
            sensorsIndex = new int[4];
            unitsIndex = new int[4];
        }

        private void SensorChartForm_Resize(object sender, EventArgs e) {
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
            if (chkSensor1.Checked) {
                num1 = 1;
            }
            if (chkSensor2.Checked) {
                ++num1;
            }
            if (chkSensor3.Checked) {
                ++num1;
            }
            if (chkSensor4.Checked) {
                ++num1;
            }
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
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("必须首先与车辆进行连接，才能进行后续操作！", "连接请求", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else if (!chkSensor1.Checked) {
                MessageBox.Show("必须至少设置一个用于监测的传感器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                sensorsEnable[0] = chkSensor1.Checked;
                sensorsEnable[1] = chkSensor2.Checked;
                sensorsEnable[2] = chkSensor3.Checked;
                sensorsEnable[3] = chkSensor4.Checked;
                sensorsIndex[0] = comboSensor1.SelectedIndex;
                sensorsIndex[1] = comboSensor2.SelectedIndex;
                sensorsIndex[2] = comboSensor3.SelectedIndex;
                sensorsIndex[3] = comboSensor4.SelectedIndex;
                unitsIndex[0] = comboUnits1.SelectedIndex;
                unitsIndex[1] = comboUnits2.SelectedIndex;
                unitsIndex[2] = comboUnits3.SelectedIndex;
                unitsIndex[3] = comboUnits4.SelectedIndex;
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

        private void SensorChartForm_Load(object sender, EventArgs e) {
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
            comboUnits1.SelectedIndex = 0;
            comboUnits2.SelectedIndex = 0;
            comboUnits3.SelectedIndex = 0;
            comboUnits4.SelectedIndex = 0;
            comboStyle1.SelectedIndex = 0;
            comboStyle2.SelectedIndex = 0;
            comboStyle3.SelectedIndex = 0;
            comboStyle4.SelectedIndex = 0;
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                if (!IsPlotting) {
                    btnStart.Enabled = true;
                }
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
            } else {
                IsPlotting = false;
                btnStart.Enabled = false;
                comboSensor1.Items.Clear();
                comboSensor2.Items.Clear();
                comboSensor3.Items.Clear();
                comboSensor4.Items.Clear();
            }
        }

        public void UpdateThread() {
            if (!IsPlotting) {
                return;
            }
            do {
                if (m_obdInterface.ConnectedStatus) {
                    if (sensorsEnable[0]) {
                        OBDParameterValue obdParameterValue = m_obdInterface.GetValue(comboSensor1.Items[sensorsIndex[0]] as OBDParameter, unitsIndex[0] == 0);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensorValues[0].Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart(0, chart1);
                        }
                    }
                    if (sensorsEnable[1]) {
                        OBDParameterValue obdParameterValue = m_obdInterface.GetValue(comboSensor2.Items[sensorsIndex[1]] as OBDParameter, unitsIndex[1] == 0);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensorValues[1].Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart(1, chart2);
                        }
                    }
                    if (sensorsEnable[2]) {
                        OBDParameterValue obdParameterValue = m_obdInterface.GetValue(comboSensor3.Items[sensorsIndex[2]] as OBDParameter, unitsIndex[2] == 0);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensorValues[2].Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart(2, chart3);
                        }
                    }
                    if (sensorsEnable[3]) {
                        OBDParameterValue obdParameterValue = m_obdInterface.GetValue(comboSensor4.Items[sensorsIndex[3]] as OBDParameter, unitsIndex[3] == 0);
                        if (!obdParameterValue.ErrorDetected) {
                            m_arraySensorValues[3].Add(new DatedValue(obdParameterValue.DoubleValue));
                            UpdateChart(3, chart4);
                        }
                    }
                } else {
                    Thread.Sleep(300);
                }
            } while (IsPlotting);
        }

        public void UpdateChart(int index, DGChartControl chart) {
            if (index < 0 || index > 3) {
                return;
            }
            if (0 < m_arraySensorValues[index].Count) {
                for (int i = 0; i < m_arraySensorValues[index].Count; i++) {
                    if (DateTime.Now.Subtract(m_arraySensorValues[index][i].Date).TotalSeconds > (double)Convert.ToInt32(numHistory.Value)) {
                        m_arraySensorValues[index].RemoveAt(i);
                        i--;
                    }
                }
            }
            if (m_arraySensorValues[index].Count == 0) {
                return;
            }
            double[] numArray1 = new double[m_arraySensorValues[index].Count];
            numArray1.Initialize();
            dSensorValues[index] = numArray1;
            double[] numArray2 = new double[m_arraySensorValues[index].Count];
            numArray2.Initialize();
            dSensorTimes[index] = numArray2;
            m_dSensorMax[index] = m_arraySensorValues[index][0].Value;
            m_dSensorMin[index] = m_arraySensorValues[index][0].Value;
            for (int i = 0; i < m_arraySensorValues[index].Count; i++) {
                DatedValue datedValue = m_arraySensorValues[index][i];
                if (datedValue.Value > m_dSensorMax[index]) {
                    m_dSensorMax[index] = datedValue.Value;
                }
                if (datedValue.Value < m_dSensorMin[index]) {
                    m_dSensorMin[index] = datedValue.Value;
                }
                dSensorValues[index][i] = datedValue.Value;
                TimeSpan timeSpan = DateTime.Now.Subtract(datedValue.Date);
                dSensorTimes[index][i] = timeSpan.TotalSeconds * -1.0;
            }
            if (chart.YRangeEnd < m_dSensorMax[index]) {
                chart.YRangeEnd = Math.Ceiling(m_dSensorMax[index]);
                chart.YGrid = (chart.YRangeEnd - chart.YRangeStart) * 0.1;
            }
            if (chart.YRangeStart > m_dSensorMin[index]) {
                chart.YRangeStart = Math.Floor(m_dSensorMin[index]);
                chart.YGrid = (chart.YRangeEnd - chart.YRangeStart) * 0.1;
            }
            chart.XData1 = dSensorTimes[index];
            chart.YData1 = dSensorValues[index];
        }

        private void numHistory_ValueChanged(object sender, EventArgs e) {
            chart1.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart2.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart3.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
            chart4.XRangeStart = (double)Convert.ToSingle(numHistory.Value) * -1.0;
        }

        private void ComboStyle_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ComboBox comboStyle) {
                if (comboStyle == comboStyle1) {
                    ComboStyle_Update(comboStyle1, chart1);
                } else if (comboStyle == comboStyle2) {
                    ComboStyle_Update(comboStyle2, chart2);
                } else if (comboStyle == comboStyle3) {
                    ComboStyle_Update(comboStyle3, chart3);
                } else if (comboStyle == comboStyle4) {
                    ComboStyle_Update(comboStyle4, chart4);
                }
            }
        }

        private void ComboStyle_Update(ComboBox comboStyle, DGChartControl chart) {
            if (comboStyle.SelectedIndex == 0) {
                chart.DrawMode = DGChartControl.DrawModeType.Line;
            } else if (comboStyle.SelectedIndex == 1) {
                chart.DrawMode = DGChartControl.DrawModeType.Dot;
            } else {
                chart.DrawMode = DGChartControl.DrawModeType.Bar;
            }
        }

        private void ChkSensor_CheckedChanged(object sender, EventArgs e) {
            DrawCharts();
            if (sender is CheckBox chkSensor) {
                if (chkSensor == chkSensor1) {
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
                } else if (chkSensor == chkSensor2) {
                    if (chkSensor2.Checked) {
                        chkSensor3.Enabled = true;
                    } else {
                        chkSensor3.Checked = false;
                        chkSensor4.Checked = false;
                        chkSensor3.Enabled = false;
                        chkSensor4.Enabled = false;
                    }
                } else if (chkSensor == chkSensor3) {
                    if (chkSensor3.Checked) {
                        chkSensor4.Enabled = true;
                    } else {
                        chkSensor4.Checked = false;
                        chkSensor4.Enabled = false;
                    }
                }
            }
        }

        private void ChkSensor_EnabledChanged(object sender, EventArgs e) {
            if (sender is CheckBox chkSensor) {
                if (chkSensor == chkSensor1) {
                    ChkSensor_Update(chkSensor1, comboSensor1, comboUnits1, comboStyle1);
                } else if (chkSensor == chkSensor2) {
                    ChkSensor_Update(chkSensor2, comboSensor2, comboUnits2, comboStyle2);
                } else if (chkSensor == chkSensor3) {
                    ChkSensor_Update(chkSensor3, comboSensor3, comboUnits3, comboStyle3);
                } else if (chkSensor == chkSensor4) {
                    ChkSensor_Update(chkSensor4, comboSensor4, comboUnits4, comboStyle4);
                }
            }
        }

        private void ChkSensor_Update(CheckBox chkSensor, ComboBox comboSensor, ComboBox comboUnits, ComboBox comboStyle) {
            if (chkSensor.Enabled) {
                comboSensor.Enabled = true;
                comboUnits.Enabled = true;
                comboStyle.Enabled = true;
            } else {
                comboSensor.Enabled = false;
                comboUnits.Enabled = false;
                comboStyle.Enabled = false;
            }
        }

        private void ComboSensorOrUnits_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ComboBox comboBox) {
                if (comboBox == this.comboSensor1 || comboBox == this.comboUnits1) {
                    ComboSensorOrUnits_Update(m_arraySensorValues[0], chart1, comboSensor1, comboUnits1);
                } else if (comboBox == this.comboSensor2 || comboBox == this.comboUnits2) {
                    ComboSensorOrUnits_Update(m_arraySensorValues[1], chart2, comboSensor2, comboUnits2);
                } else if (comboBox == this.comboSensor3 || comboBox == this.comboUnits3) {
                    ComboSensorOrUnits_Update(m_arraySensorValues[2], chart3, comboSensor3, comboUnits3);
                } else if (comboBox == this.comboSensor4 || comboBox == this.comboUnits4) {
                    ComboSensorOrUnits_Update(m_arraySensorValues[3], chart4, comboSensor4, comboUnits4);
                }
            }
        }

        private void ComboSensorOrUnits_Update(List<DatedValue> arraySensorValues, DGChartControl chart, ComboBox comboSensor, ComboBox comboUnits) {
            arraySensorValues.Clear();
            chart.XData1 = (double[])null;
            chart.YData1 = (double[])null;
            if (comboSensor.Items.Count == 0) {
                return;
            }
            int selectedIndex = comboSensor.SelectedIndex;
            if (selectedIndex < 0) {
                return;
            }
            OBDParameter obdParameter = (OBDParameter)comboSensor.Items[selectedIndex];
            if (comboUnits.SelectedIndex == 0) {
                chart.Text = obdParameter.Name + " (" + obdParameter.EnglishUnitLabel + ")";
                chart.YRangeStart = Math.Floor(obdParameter.EnglishMinValue);
                chart.YRangeEnd = obdParameter.EnglishMinValue + 1.0;
            } else {
                chart.Text = obdParameter.Name + " (" + obdParameter.MetricUnitLabel + ")";
                chart.YRangeStart = Math.Floor(obdParameter.MetricMinValue);
                chart.YRangeEnd = obdParameter.MetricMinValue + 1.0;
            }
        }

        private void SensorChartForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }
    }
}
