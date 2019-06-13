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
            groupConTests.Width = Width / 2 - 17;
            groupConTests.Height = Height - 410;
            gridConTests.Width = groupConTests.Width - 20;
            gridConTests.Height = groupConTests.Height - 30;
            groupNonConTests.Location = new Point(groupConTests.Width + 20, 45);
            groupNonConTests.Width = groupConTests.Width;
            groupNonConTests.Height = Height - 340;
            gridNonConTests.Width = groupNonConTests.Width - 20;
            gridNonConTests.Height = groupNonConTests.Height - 30;
            groupFuel1.Location = new Point(10, groupConTests.Location.Y + groupConTests.Height + 7);
            groupFuel1.Width = groupConTests.Width;
            lblFuel1.Width = groupFuel1.Width - 20;
            groupFuel2.Location = new Point(10, groupFuel1.Location.Y + groupFuel1.Height + 10);
            groupFuel2.Width = groupConTests.Width;
            lblFuel2.Width = groupFuel2.Width - 20;
            groupPTO.Location = new Point(10, groupFuel2.Location.Y + groupFuel2.Height + 10);
            groupPTO.Width = groupConTests.Width;
            lblPTO.Width = groupPTO.Width - 20;
            groupAir.Location = new Point(10, groupPTO.Location.Y + groupPTO.Height + 10);
            groupAir.Width = groupConTests.Width;
            lblAir.Width = groupAir.Width - 20;
            groupOBD.Location = new Point(groupNonConTests.Location.X, groupNonConTests.Location.Y + groupNonConTests.Height + 7);
            groupOBD.Width = groupNonConTests.Width;
            lblOBD.Width = groupOBD.Width - 20;
            groupOxygen.Location = new Point(groupNonConTests.Location.X, groupOBD.Location.Y + groupOBD.Height + 10);
            groupOxygen.Width = groupNonConTests.Width;
            lblOxygen.Width = groupOxygen.Width - 20;
            groupBattery.Location = new Point(10, groupOxygen.Location.Y + groupOxygen.Height + 7);
            groupBattery.Width = Width - 23;
        }

        private void TestForm_Load(object sender, EventArgs e) {
            m_ListConTests = getContinuousTestList();
            m_ListNonConTests = getNonContinuousTestList();
            gridConTests.DataSource = m_ListConTests;
            gridNonConTests.DataSource = m_ListNonConTests;
            gridConTests.TableStyles.Clear();
            gridConTests.TableStyles.Add(GetTableStyle());
            gridNonConTests.TableStyles.Clear();
            gridNonConTests.TableStyles.Add(GetTableStyle());
        }

        private List<TestStatus> getContinuousTestList() {
            List<TestStatus> list = new List<TestStatus>();
            list.Add(new TestStatus("Misfire", "", 2, 5));
            list.Add(new TestStatus("Fuel System", "", 3, 6));
            list.Add(new TestStatus("Comprehensive Component", "", 4, 7));
            return list;
        }

        private List<TestStatus> getNonContinuousTestList() {
            List<TestStatus> list = new List<TestStatus>();
            list.Add(new TestStatus("Catalyst", "", 8, 0x10));
            list.Add(new TestStatus("Heated Catalyst", "", 9, 0x11));
            list.Add(new TestStatus("Evaporative System", "", 10, 0x12));
            list.Add(new TestStatus("Secondary Air System", "", 11, 0x13));
            list.Add(new TestStatus("A/C System Refrigerant", "", 12, 20));
            list.Add(new TestStatus("Oxygen Sensor", "", 13, 0x15));
            list.Add(new TestStatus("Oxygen Sensor Heater", "", 14, 0x16));
            list.Add(new TestStatus("EGR System", "", 15, 0x17));
            return list;
        }

        public void CheckConnection() {
            if (!m_obd2Interface.ConnectedStatus) {
                return;
            }
            UpdateTests();
        }

        #region GetTableStyle 
        public DataGridTableStyle GetTableStyle() {
            DataGridTableStyle style = new DataGridTableStyle();
            style.MappingName = "List";

            DataGridTextBoxColumn column = new DataGridTextBoxColumn();
            column.MappingName = "Name";
            column.HeaderText = "Name";
            column.Format = "f4";
            column.Width = 150;
            style.GridColumnStyles.Add(column);

            column = new DataGridTextBoxColumn();
            column.MappingName = "Status";
            column.HeaderText = "Completed";
            column.Format = "f4";
            column.Width = 85;
            style.GridColumnStyles.Add(column);

            column = new DataGridTextBoxColumn();
            column.MappingName = "SupportID";
            column.HeaderText = "SupportID";
            column.Format = "d";
            column.Width = 25;
            style.GridColumnStyles.Add(column);

            column = new DataGridTextBoxColumn();
            column.MappingName = "StatusID";
            column.HeaderText = "StatusID";
            column.Format = "d";
            column.Width = 25;
            style.GridColumnStyles.Add(column);

            return style;
        }
        #endregion

        public void UpdateTests() {
            progressBar.Minimum = 0;
            progressBar.Maximum = 25;
            gridConTests.Visible = false;
            gridNonConTests.Visible = false;
            OBDParameterValue value;
            value = m_obd2Interface.getValue("SAE.MISFIRE_SUPPORT", true);
            progressBar.Value = 1;
            TestStatus status = m_ListConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.MISFIRE_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.FUEL_SUPPORT", true);
            progressBar.Value = 2;
            status = m_ListConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.FUEL_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.CCM_SUPPORT", true);
            progressBar.Value = 3;
            status = m_ListConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.CCM_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.CAT_SUPPORT", true);
            progressBar.Value = 4;
            status = m_ListNonConTests[0];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.CAT_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.HCAT_SUPPORT", true);
            progressBar.Value = 5;
            status = m_ListNonConTests[1];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.HCAT_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.EVAP_SUPPORT", true);
            progressBar.Value = 6;
            status = m_ListNonConTests[2];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.EVAP_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.AIR_SUPPORT", true);
            progressBar.Value = 7;
            status = m_ListNonConTests[3];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                } else {
                    value = m_obd2Interface.getValue("SAE.AIR_STATUS", true);
                    if (!value.ErrorDetected)
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    else
                        status.Status = "ERROR";
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.AC_SUPPORT", true);
            progressBar.Value = 8;
            status = m_ListNonConTests[4];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                }
                else {
                    value = m_obd2Interface.getValue("SAE.AC_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.O2_SUPPORT", true);
            progressBar.Value = 9;
            status = m_ListNonConTests[5];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                }
                else {
                    value = m_obd2Interface.getValue("SAE.O2_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.O2HTR_SUPPORT", true);
            progressBar.Value = 10;
            status = m_ListNonConTests[6];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                }
                else {
                    value = m_obd2Interface.getValue("SAE.O2HTR_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            value = m_obd2Interface.getValue("SAE.EGR_SUPPORT", true);
            progressBar.Value = 11;
            status = m_ListNonConTests[7];
            if (!value.ErrorDetected) {
                if (!value.BoolValue) {
                    status.Status = "Not Supported";
                }
                else {
                    value = m_obd2Interface.getValue("SAE.EGR_STATUS", true);
                    if (!value.ErrorDetected) {
                        status.Status = value.BoolValue ? "Complete" : "Incomplete";
                    } else {
                        status.Status = "ERROR";
                    }
                }
            } else {
                status.Status = "ERROR";
            }

            gridConTests.Visible = true;
            gridNonConTests.Visible = true;
            if (m_obd2Interface.isParameterSupported("SAE.FUEL1_STATUS")) {
                value = m_obd2Interface.getValue("SAE.FUEL1_STATUS", true);
                progressBar.Value++;
                lblFuel1.Text = value.ErrorDetected ? "ERROR" : value.StringValue;
            } else {
                lblFuel1.Text = "Not Supported";
            }

            if (m_obd2Interface.isParameterSupported("SAE.FUEL2_STATUS")) {
                value = m_obd2Interface.getValue("SAE.FUEL2_STATUS", true);
                progressBar.Value++;
                lblFuel2.Text = value.ErrorDetected ? "ERROR" : value.StringValue;
            } else {
                lblFuel2.Text = "Not Supported";
            }

            if (m_obd2Interface.isParameterSupported("SAE.PTO_STATUS")) {
                value = m_obd2Interface.getValue("SAE.PTO_STATUS", true);
                progressBar.Value++;
                lblPTO.Text = value.ErrorDetected ? "ERROR" : value.StringValue;
            } else {
                lblPTO.Text = "Not Supported";
            }

            if (m_obd2Interface.isParameterSupported("SAE.SECAIR_STATUS")) {
                value = m_obd2Interface.getValue("SAE.SECAIR_STATUS", true);
                progressBar.Value++;
                lblAir.Text = value.ErrorDetected ? "ERROR" : value.StringValue;
            } else {
                lblAir.Text = "Not Supported";
            }

            if (m_obd2Interface.isParameterSupported("SAE.OBD_TYPE")) {
                value = m_obd2Interface.getValue("SAE.OBD_TYPE", true);
                progressBar.Value++;
                lblOBD.Text = value.ErrorDetected ? "ERROR" : value.StringValue;
            } else {
                lblOBD.Text = "Not Supported";
            }

            string str = "";
            if (m_obd2Interface.isParameterSupported("SAE.O2B1S1A_PRESENT")) {
                value = m_obd2Interface.getValue("SAE.O2B1S1A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B1S2A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 2\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B1S3A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 3\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B1S4A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 4\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S1A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S2A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 2\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S3A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 3\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S4A_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 4\n";
                }
            }

            if (m_obd2Interface.isParameterSupported("SAE.O2B1S1B_PRESENT")) {
                value = m_obd2Interface.getValue("SAE.O2B1S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B1S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 1 Sensor 2\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B2S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 2 Sensor 2\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B3S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 3 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B3S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 3 Sensor 2\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B4S1B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 4 Sensor 1\n";
                }

                value = m_obd2Interface.getValue("SAE.O2B4S2B_PRESENT", true);
                progressBar.Value++;
                if (!value.ErrorDetected && value.BoolValue) {
                    str = str + "Bank 4 Sensor 2\n";
                }
            }
            lblOxygen.Text = str;
            progressBar.Value++;
            if (m_obd2Interface.getDevice() == HardwareType.ELM327) {
                value = m_obd2Interface.getValue("ELM.BATTERY_VOLTAGE", true);
                if (!value.ErrorDetected) {
                    lblBattery.Text = value.DoubleValue.ToString() + " V";
                }
            } else {
                lblBattery.Text = "Not Supported";
            }
            progressBar.Value = progressBar.Maximum;
        }

        private void btnUpdate_Click(object sender, EventArgs e) {
            if (!m_obd2Interface.ConnectedStatus) {
                MessageBox.Show("A vehicle connection must first be established.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_obd2Interface.logItem("Error. Test Form. Attempted refresh without vehicle connection.");
            } else {
                btnUpdate.Enabled = false;
                UpdateTests();
                btnUpdate.Enabled = true;
            }
        }

    }
}
