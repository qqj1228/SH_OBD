using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class OxygenSensorsForm : Form {
        private OBDInterface m_obdInterface;

        public OxygenSensorsForm(OBDInterface obd2) {
            m_obdInterface = obd2;
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("成功连接车辆OBD接口后才能进行操作", "车辆OBD接口连接请求", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                btnRead.Enabled = false;
                ReadTestResults();
                btnRead.Enabled = true;
            }
        }

        private void ReadTestResults() {
            o2TestResultsControl1.Reset();
            progressBar.Maximum = 12;
            progressBar.Value = 0;
            if (!m_obdInterface.GetValue("SAE.O2_SUPPORT", true).ErrorDetected) {
                OBDParameterValue value;
                value = m_obdInterface.GetValue("SAE.O2_STATUS", true);
                if (!value.ErrorDetected && value.BoolValue) {
                    progressBar.Increment(1);
                    int selectedSensorId = GetSelectedSensorID();
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 1, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue01 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 1, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum01 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 1, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum01 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 2, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue02 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 2, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum02 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 2, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum02 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 3, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue03 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 3, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum03 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 3, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum03 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 4, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue04 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 4, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum04 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 4, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum04 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 5, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue05 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 5, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum05 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 5, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum05 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 6, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue06 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 6, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum06 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 6, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum06 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 7, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue07 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 7, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum07 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 7, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum07 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 8, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue08 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 8, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum08 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 8, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum08 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 9, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue09 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 9, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum09 = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 9, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum09 = value.DoubleValue;
                    }
                    progressBar.Increment(1);

                    value = m_obdInterface.GetValue(new OBDParameter(5, 10, 0, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestValue0A = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 10, 1, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMinimum0A = value.DoubleValue;
                    }
                    value = m_obdInterface.GetValue(new OBDParameter(5, 10, 2, selectedSensorId), true);
                    if (!value.ErrorDetected) {
                        o2TestResultsControl1.TestMaximum0A = value.DoubleValue;
                    }
                    progressBar.Value = progressBar.Maximum;

                    return;
                }
            }
            MessageBox.Show("该辆车不适用氧气传感器监测或者监测未完成", "不适用", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private int GetSelectedSensorID() {
            string item = comboOxygenSensor.SelectedItem as string;
            if (string.Compare(item, "组 1, 传感器 1 (O2B1S1)") == 0) {
                return 1;
            }
            if (string.Compare(item, "组 1, 传感器 2 (O2B1S2)") == 0) {
                return 2;
            }
            if (string.Compare(item, "组 1, 传感器 3 (O2B1S3)") == 0) {
                return 3;
            }
            if (string.Compare(item, "组 1, 传感器 4 (O2B1S4)") == 0) {
                return 4;
            }
            if (string.Compare(item, "组 2, 传感器 1 (O2B2S1)") == 0) {
                return 16;
            }
            if (string.Compare(item, "组 2, 传感器 2 (O2B2S2)") == 0) {
                return 32;
            }
            if (string.Compare(item, "组 2, 传感器 3 (O2B2S3)") == 0) {
                return 7;
            }
            if (string.Compare(item, "组 2, 传感器 4 (O2B2S4)") == 0) {
                return 0;
            }
            return -1;
        }

        private void PopulateO2Locations() {
            comboOxygenSensor.Items.Clear();
            OBDParameterValue value;

            value = m_obdInterface.GetValue("SAE.O2B1S1A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 1 (O2B1S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B1S2A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 2 (O2B1S2)");
            }

            value = m_obdInterface.GetValue("SAE.O2B1S3A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 3 (O2B1S3)");
            }

            value = m_obdInterface.GetValue("SAE.O2B1S4A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 4 (O2B1S4)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S1A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 1 (O2B2S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S2A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 2 (O2B2S2)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S3A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 3 (O2B2S3)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S4A_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 4 (O2B2S4)");
            }

            if (comboOxygenSensor.Items.Count > 0) {
                comboOxygenSensor.SelectedIndex = 0;
            }

            value = m_obdInterface.GetValue("SAE.O2B1S1B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 1 (O2B1S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B1S2B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 1, 传感器 2 (O2B1S2)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S1B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 1 (O2B2S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B2S2B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 2, 传感器 2 (O2B2S2)");
            }

            value = m_obdInterface.GetValue("SAE.O2B3S1B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 3, 传感器 1 (O2B3S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B3S2B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 3, 传感器 2 (O2B3S2)");
            }

            value = m_obdInterface.GetValue("SAE.O2B4S1B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 4, 传感器 1 (O2B4S1)");
            }

            value = m_obdInterface.GetValue("SAE.O2B4S2B_PRESENT", true);
            if (!value.ErrorDetected && value.BoolValue) {
                comboOxygenSensor.Items.Add("组 4, 传感器 2 (O2B4S2)");
            }

            if (comboOxygenSensor.Items.Count > 0) {
                comboOxygenSensor.SelectedIndex = 0;
            }
        }

        private void OxygenSensorsForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                if (!m_obdInterface.ConnectedStatus) {
                    return;
                }
                PopulateO2Locations();
            }
        }
    }
}
