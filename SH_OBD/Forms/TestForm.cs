using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class TestForm : Form {
        private static List<TestStatus> m_ListConTests;
        private static List<TestStatus> m_ListNonConTests;
        private readonly OBDInterface m_obdInterface;

        public TestForm(OBDInterface obd) {
            InitializeComponent();
            m_obdInterface = obd;
        }

        private void TestForm_Resize(object sender, EventArgs e) {
            const int bottomConTests = 357;
            const int bottomNonConTests = 289;
            int margin = groupConTests.Location.X;
            groupConTests.Width = (Width - margin * 3) / 2;
            groupConTests.Height = Height - bottomConTests - groupConTests.Location.Y;
            groupNonConTests.Location = new Point(groupConTests.Location.X + groupConTests.Width + margin, groupNonConTests.Location.Y);
            groupNonConTests.Width = groupConTests.Width;
            groupNonConTests.Height = Height - bottomNonConTests - groupNonConTests.Location.Y;
            groupFuel1.Width = groupConTests.Width;
            groupFuel2.Width = groupConTests.Width;
            groupPTO.Width = groupConTests.Width;
            groupAir.Width = groupConTests.Width;
            groupOBD.Location = new Point(groupNonConTests.Location.X, groupOBD.Location.Y);
            groupOBD.Width = groupNonConTests.Width;
            groupOxygen.Location = new Point(groupNonConTests.Location.X, groupOxygen.Location.Y);
            groupOxygen.Width = groupNonConTests.Width;
            groupBattery.Width = groupNonConTests.Width;
        }

        private void TestForm_Load(object sender, EventArgs e) {
            m_ListConTests = GetContinuousTestList();
            m_ListNonConTests = GetNonContinuousTestList();
            gridConTests.DataSource = m_ListConTests;
            gridNonConTests.DataSource = m_ListNonConTests;
            gridConTests.Columns[0].HeaderText = "序号";
            gridConTests.Columns[1].HeaderText = "名称";
            gridConTests.Columns[2].HeaderText = "完成状态";
            gridNonConTests.Columns[0].HeaderText = "序号";
            gridNonConTests.Columns[1].HeaderText = "名称";
            gridNonConTests.Columns[2].HeaderText = "完成状态";
        }

        private List<TestStatus> GetContinuousTestList() {
            List<TestStatus> list = new List<TestStatus> {
                new TestStatus(1, "失火", ""),
                new TestStatus(2, "燃油系统", ""),
                new TestStatus(3, "综合组件", "")
            };
            return list;
        }

        private List<TestStatus> GetNonContinuousTestList() {
            List<TestStatus> list = new List<TestStatus> {
                new TestStatus(1, "催化器", ""),
                new TestStatus(2, "加热催化器", ""),
                new TestStatus(3, "蒸发系统", ""),
                new TestStatus(4, "二次空气系统", ""),
                new TestStatus(5, "A/C 系统制冷剂", ""),
                new TestStatus(6, "氧气传感器", ""),
                new TestStatus(7, "加热氧气传感器", ""),
                new TestStatus(8, "EGR 系统", "")
            };
            return list;
        }

        public void CheckConnection() {
            if (!m_obdInterface.ConnectedStatus) {
                return;
            }
            UpdateTests();
        }

        public void UpdateTests() {
            progressBar.Minimum = 0;
            progressBar.Maximum = 35;
            OBDParameterValue value;
            value = m_obdInterface.GetValue("SAE.MISFIRE_SUPPORT");
            progressBar.Value = 1;
            TestStatus status = m_ListConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.MISFIRE_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.FUEL_SUPPORT");
            progressBar.Value = 2;
            status = m_ListConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.FUEL_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.CCM_SUPPORT");
            progressBar.Value = 3;
            status = m_ListConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.CCM_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }
            gridConTests.Refresh();

            value = m_obdInterface.GetValue("SAE.CAT_SUPPORT");
            progressBar.Value = 4;
            status = m_ListNonConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.CAT_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.HCAT_SUPPORT");
            progressBar.Value = 5;
            status = m_ListNonConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.HCAT_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.EVAP_SUPPORT");
            progressBar.Value = 6;
            status = m_ListNonConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.EVAP_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.AIR_SUPPORT");
            progressBar.Value = 7;
            status = m_ListNonConTests[3];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.AIR_STATUS");
                    if (!value.ErrorDetected)
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    else
                        status.Status = "出错";
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.AC_SUPPORT");
            progressBar.Value = 8;
            status = m_ListNonConTests[4];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.AC_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.O2_SUPPORT");
            progressBar.Value = 9;
            status = m_ListNonConTests[5];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.O2_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.O2HTR_SUPPORT");
            progressBar.Value = 10;
            status = m_ListNonConTests[6];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.O2HTR_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obdInterface.GetValue("SAE.EGR_SUPPORT");
            progressBar.Value = 11;
            status = m_ListNonConTests[7];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obdInterface.GetValue("SAE.EGR_STATUS");
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }
            gridNonConTests.Refresh();

            if (m_obdInterface.IsParameterSupported("SAE.FUEL1_STATUS")) {
                value = m_obdInterface.GetValue("SAE.FUEL1_STATUS");
                progressBar.Value++;
                lblFuel1.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblFuel1.Text = "不适用";
            }

            if (m_obdInterface.IsParameterSupported("SAE.FUEL2_STATUS")) {
                value = m_obdInterface.GetValue("SAE.FUEL2_STATUS");
                progressBar.Value++;
                lblFuel2.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblFuel2.Text = "不适用";
            }

            if (m_obdInterface.IsParameterSupported("SAE.PTO_STATUS")) {
                value = m_obdInterface.GetValue("SAE.PTO_STATUS");
                progressBar.Value++;
                lblPTO.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblPTO.Text = "不适用";
            }

            if (m_obdInterface.IsParameterSupported("SAE.SECAIR_STATUS")) {
                value = m_obdInterface.GetValue("SAE.SECAIR_STATUS");
                progressBar.Value++;
                lblAir.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblAir.Text = "不适用";
            }

            if (m_obdInterface.IsParameterSupported("SAE.OBD_TYPE")) {
                value = m_obdInterface.GetValue("SAE.OBD_TYPE");
                progressBar.Value++;
                lblOBD.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblOBD.Text = "不适用";
            }

            string strContent = "";
            OBDParameter param = new OBDParameter {
                OBDRequest = "0902",
                Service = 9,
                Parameter = 2,
                ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
            };
            OBDParameterValue val = m_obdInterface.GetValue(param);
            progressBar.Value++;
            strContent += "VIN:";
            foreach (string item in val.ListStringValue) {
                strContent += "\n" + item;
            }

            param.OBDRequest = "0904";
            val = m_obdInterface.GetValue(param);
            progressBar.Value++;
            strContent += "\n\nCAL ID:";
            foreach (string item in val.ListStringValue) {
                strContent += "\n" + item;
            }

            param.OBDRequest = "0906";
            val = m_obdInterface.GetValue(param);
            progressBar.Value++;
            strContent += "\n\nCVN:";
            foreach (string item in val.ListStringValue) {
                strContent += "\n" + item;
            }

            //param.OBDRequest = "0908";
            //param.SubParameter = 0;
            //val = m_obdInterface.GetValue(param);
            //progressBar.Value++;
            //strContent += "\n\nIPT:";
            //strContent += "\n" + val.DoubleValue;
            ////foreach (string item in val.ListStringValue) {
            ////    strContent += "," + Utility.Hex2Int(item);
            ////}

            param.OBDRequest = "090A";
            val = m_obdInterface.GetValue(param);
            progressBar.Value++;
            strContent += "\n\nECU名称:";
            foreach (string item in val.ListStringValue) {
                strContent += "\n" + item;
            }

            lblVehicleInfo.Text = strContent;
            progressBar.Value++;

            /*string strContent = "";
            string strHeader;
            int counter;

            if (m_obd2Interface.IsParameterSupported("SAE.O2B1S1A_PRESENT")) {
                strHeader = "PID $13 Bank 1: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B1S1A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S2A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S3A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 3, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S4A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 4, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }

                strHeader = "\n\r\nPID $13 Bank 2: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B2S1A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S2A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S3A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 3, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S4A_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 4, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }
            }

            if (m_obd2Interface.IsParameterSupported("SAE.O2B1S1B_PRESENT")) {
                strHeader = "\n\r\nPID $1D Bank 1: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B1S1B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S2B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }

                strHeader = "\n\r\nPID $1D Bank 2: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B2S1B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S2B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }

                strHeader = "\n\r\nPID $1D Bank 3: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B3S1B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B3S2B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }

                strHeader = "\n\r\nPID $1D Bank 4: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B4S1B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B4S2B_PRESENT");
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent += "传感器 2, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }
            }*/
            if (m_obdInterface.GetDevice() == HardwareType.ELM327) {
                value = m_obdInterface.GetValue("ELM.BATTERY_VOLTAGE");
                if (!value.ErrorDetected) {
                    lblBattery.Text = value.DoubleValue.ToString() + " V";
                }
            } else {
                lblBattery.Text = "不适用";
            }
            progressBar.Value = progressBar.Maximum;
        }

        private void btnUpdate_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                m_obdInterface.GetLogger().TraceError("Test Form, Attempted refresh without vehicle connection.");
                MessageBox.Show("必须首先与车辆进行连接，才能进行后续操作！", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                btnUpdate.Enabled = false;
                UpdateTests();
                btnUpdate.Enabled = true;
            }
        }

    }

    public class TestStatus {
        public int NO { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public TestStatus(int NO, string strName, string strStatus) {
            this.NO = NO;
            Name = strName;
            Status = strStatus;
        }
    }

}
