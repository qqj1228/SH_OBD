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
        private OBDInterface m_obd2Interface;

        public TestForm(OBDInterface obd) {
            InitializeComponent();
            m_obd2Interface = obd;
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
            gridConTests.Columns[0].HeaderText = "名称";
            gridConTests.Columns[1].HeaderText = "完成状态";
            gridConTests.Columns[2].HeaderText = "支持ID";
            gridConTests.Columns[3].HeaderText = "状态ID";
            gridNonConTests.Columns[0].HeaderText = "名称";
            gridNonConTests.Columns[1].HeaderText = "完成状态";
            gridNonConTests.Columns[2].HeaderText = "支持ID";
            gridNonConTests.Columns[3].HeaderText = "状态ID";
        }

        private List<TestStatus> GetContinuousTestList() {
            List<TestStatus> list = new List<TestStatus> {
                new TestStatus("失火", "", 2, 5),
                new TestStatus("燃油系统", "", 3, 6),
                new TestStatus("综合组件", "", 4, 7)
            };
            return list;
        }

        private List<TestStatus> GetNonContinuousTestList() {
            List<TestStatus> list = new List<TestStatus> {
                new TestStatus("催化器", "", 8, 0x10),
                new TestStatus("加热催化器", "", 9, 0x11),
                new TestStatus("蒸发系统", "", 10, 0x12),
                new TestStatus("二次空气系统", "", 11, 0x13),
                new TestStatus("A/C 系统制冷剂", "", 12, 20),
                new TestStatus("氧气传感器", "", 13, 0x15),
                new TestStatus("加热氧气传感器", "", 14, 0x16),
                new TestStatus("EGR 系统", "", 15, 0x17)
            };
            return list;
        }

        public void CheckConnection() {
            if (!m_obd2Interface.ConnectedStatus) {
                return;
            }
            UpdateTests();
        }

        public void UpdateTests() {
            progressBar.Minimum = 0;
            progressBar.Maximum = 35;
            OBDParameterValue value;
            value = m_obd2Interface.GetValue("SAE.MISFIRE_SUPPORT", true);
            progressBar.Value = 1;
            TestStatus status = m_ListConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.MISFIRE_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.FUEL_SUPPORT", true);
            progressBar.Value = 2;
            status = m_ListConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.FUEL_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.CCM_SUPPORT", true);
            progressBar.Value = 3;
            status = m_ListConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.CCM_STATUS", true);
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

            value = m_obd2Interface.GetValue("SAE.CAT_SUPPORT", true);
            progressBar.Value = 4;
            status = m_ListNonConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.CAT_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.HCAT_SUPPORT", true);
            progressBar.Value = 5;
            status = m_ListNonConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.HCAT_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.EVAP_SUPPORT", true);
            progressBar.Value = 6;
            status = m_ListNonConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.EVAP_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.AIR_SUPPORT", true);
            progressBar.Value = 7;
            status = m_ListNonConTests[3];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                } else {
                    value = m_obd2Interface.GetValue("SAE.AIR_STATUS", true);
                    if (!value.ErrorDetected)
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    else
                        status.Status = "出错";
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.AC_SUPPORT", true);
            progressBar.Value = 8;
            status = m_ListNonConTests[4];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                }
                else {
                    value = m_obd2Interface.GetValue("SAE.AC_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.O2_SUPPORT", true);
            progressBar.Value = 9;
            status = m_ListNonConTests[5];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                }
                else {
                    value = m_obd2Interface.GetValue("SAE.O2_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.O2HTR_SUPPORT", true);
            progressBar.Value = 10;
            status = m_ListNonConTests[6];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                }
                else {
                    value = m_obd2Interface.GetValue("SAE.O2HTR_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "完成" : "未完成";
                    } else {
                        status.Status = "出错";
                    }
                }
            } else {
                status.Status = "出错";
            }

            value = m_obd2Interface.GetValue("SAE.EGR_SUPPORT", true);
            progressBar.Value = 11;
            status = m_ListNonConTests[7];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "不适用";
                }
                else {
                    value = m_obd2Interface.GetValue("SAE.EGR_STATUS", true);
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

            if (m_obd2Interface.IsParameterSupported("SAE.FUEL1_STATUS")) {
                value = m_obd2Interface.GetValue("SAE.FUEL1_STATUS", true);
                progressBar.Value++;
                lblFuel1.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblFuel1.Text = "不适用";
            }

            if (m_obd2Interface.IsParameterSupported("SAE.FUEL2_STATUS")) {
                value = m_obd2Interface.GetValue("SAE.FUEL2_STATUS", true);
                progressBar.Value++;
                lblFuel2.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblFuel2.Text = "不适用";
            }

            if (m_obd2Interface.IsParameterSupported("SAE.PTO_STATUS")) {
                value = m_obd2Interface.GetValue("SAE.PTO_STATUS", true);
                progressBar.Value++;
                lblPTO.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblPTO.Text = "不适用";
            }

            if (m_obd2Interface.IsParameterSupported("SAE.SECAIR_STATUS")) {
                value = m_obd2Interface.GetValue("SAE.SECAIR_STATUS", true);
                progressBar.Value++;
                lblAir.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblAir.Text = "不适用";
            }

            if (m_obd2Interface.IsParameterSupported("SAE.OBD_TYPE")) {
                value = m_obd2Interface.GetValue("SAE.OBD_TYPE", true);
                progressBar.Value++;
                lblOBD.Text = value.ErrorDetected ? "出错" : value.StringValue;
            } else {
                lblOBD.Text = "不适用";
            }

            string strContent = "";
            string strHeader = "";
            int counter = 0;
            if (m_obd2Interface.IsParameterSupported("SAE.O2B1S1A_PRESENT")) {
                strHeader = "PID $13 Bank 1: ";
                strContent += strHeader;
                counter = 0;
                value = m_obd2Interface.GetValue("SAE.O2B1S1A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S2A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S3A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 3, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S4A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 4, ";
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
                value = m_obd2Interface.GetValue("SAE.O2B2S1A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S2A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S3A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 3, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S4A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 4, ";
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
                value = m_obd2Interface.GetValue("SAE.O2B1S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B1S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
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
                value = m_obd2Interface.GetValue("SAE.O2B2S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B2S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
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
                value = m_obd2Interface.GetValue("SAE.O2B3S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B3S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
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
                value = m_obd2Interface.GetValue("SAE.O2B4S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 1, ";
                    ++counter;
                }

                value = m_obd2Interface.GetValue("SAE.O2B4S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    strContent = strContent + "传感器 2, ";
                    ++counter;
                }
                if (counter == 0) {
                    strContent = strContent.Substring(0, strContent.Length - strHeader.Length);
                } else {
                    strContent = strContent.Substring(0, strContent.Length - 2);
                }
            }
            lblOxygen.Text = strContent;
            progressBar.Value++;
            if (m_obd2Interface.GetDevice() == HardwareType.ELM327) {
                value = m_obd2Interface.GetValue("ELM.BATTERY_VOLTAGE", true);
                if (!value.ErrorDetected) {
                    lblBattery.Text = value.DoubleValue.ToString() + " V";
                }
            } else {
                lblBattery.Text = "不适用";
            }
            progressBar.Value = progressBar.Maximum;
        }

        private void btnUpdate_Click(object sender, EventArgs e) {
            if (!m_obd2Interface.ConnectedStatus) {
                m_obd2Interface.TraceError("Test Form, Attempted refresh without vehicle connection.");
                MessageBox.Show("必须首先与车辆进行连接，才能进行后续操作！", "连接请求", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                btnUpdate.Enabled = false;
                UpdateTests();
                btnUpdate.Enabled = true;
            }
        }

    }

    [Serializable]
    public class TestStatus {
        private string m_strName;
        private string m_strStatus;
        private int m_iSupportID;
        private int m_iStatusID;

        public TestStatus(string strName, string strStatus, int iSupportID, int iStatusID) {
            m_strName = strName;
            m_strStatus = strStatus;
            m_iSupportID = iSupportID;
            m_iStatusID = iStatusID;
        }

        public string Name {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public string Status {
            get { return m_strStatus; }
            set { m_strStatus = value; }
        }

        public int SupportID {
            get { return m_iSupportID; }
            set { m_iSupportID = value; }
        }

        public int StatusID {
            get { return m_iStatusID; }
            set { m_iStatusID = value; }
        }

    }

}
