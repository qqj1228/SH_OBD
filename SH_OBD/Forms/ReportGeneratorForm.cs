using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class ReportGeneratorForm : Form {
        private readonly OBDInterface m_obdInterface;
        private ReportForm m_bReportForm;

        public ReportGeneratorForm(OBDInterface obd2) {
            m_obdInterface = obd2;
            InitializeComponent();
        }

        ~ReportGeneratorForm() {
            if (m_bReportForm != null) {
                m_bReportForm.Dispose();
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e) {
            if (!m_obdInterface.ConnectedStatus) {
                MessageBox.Show("必须首先与车辆进行连接，才能进行后续操作！", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                m_bReportForm = new ReportForm();
                btnGenerate.Enabled = false;
                m_bReportForm.ReportPage1.ShopName = txtByName.Text;
                m_bReportForm.ReportPage1.ShopAddress1 = txtByAddress1.Text;
                m_bReportForm.ReportPage1.ShopAddress2 = txtByAddress2.Text;
                m_bReportForm.ReportPage1.ShopTelephone = txtByTelephone.Text;
                m_bReportForm.ReportPage1.ClientName = txtForName.Text;
                m_bReportForm.ReportPage1.ClientAddress1 = txtForAddress1.Text;
                m_bReportForm.ReportPage1.ClientAddress2 = txtForAddress2.Text;
                m_bReportForm.ReportPage1.ClientTelephone = txtForTelephone.Text;
                m_bReportForm.ReportPage1.Vehicle = txtVehicleYear.Text + " " + txtVehicleMake.Text + " " + txtVehicleModel.Text;
                if (m_bReportForm.ReportPage1.Vehicle.Trim().Length == 0) {
                    m_bReportForm.ReportPage1.Vehicle = "vehicle";
                }
                DateTime now1 = DateTime.Now;
                DateTime now2 = DateTime.Now;
                m_bReportForm.ReportPage1.GenerationDate = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                richTextStatus.Text = "";
                progressBar.Value = 0;
                //progressBar.Maximum = 22;
                Task.Factory.StartNew(CollectData);
            }
        }

        private void CollectData() {
            DisplayStatusMessage("请求MIL状态和故障码数量");
            DisplayRequest("0101");
            OBDParameterValue value5 = m_obdInterface.GetValue("SAE.MIL");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value5.ErrorDetected) {
                if (value5.BoolValue) {
                    DisplayDetailMessage("MIL状态: On");
                    m_bReportForm.ReportPage1.MilStatus = true;
                } else {
                    DisplayDetailMessage("MIL状态: Off");
                    m_bReportForm.ReportPage1.MilStatus = false;
                }
            }
            OBDParameterValue value3 = m_obdInterface.GetValue("SAE.DTC_COUNT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value3.ErrorDetected) {
                m_bReportForm.ReportPage1.TotalCodes = (int)value3.DoubleValue;
                DisplayDetailMessage("存储的故障码数量: " + value3.DoubleValue.ToString());
            }
            DisplayStatusMessage("请求存储的故障码列表");
            OBDParameterValue value4 = m_obdInterface.GetValue("SAE.STORED_DTCS");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value4.ErrorDetected) {
                m_bReportForm.ReportPage1.DTCList.Clear();
                foreach (string strVal in value4.ListStringValue) {
                    m_bReportForm.ReportPage1.DTCList.Add(strVal);
                    DisplayDetailMessage("存储的故障码: " + strVal);
                    DTC dtc2 = m_obdInterface.GetDTC(strVal);
                    if (dtc2 != null) {
                        m_bReportForm.ReportPage1.DTCDefinitionList.Add(dtc2.Description);
                    }
                }
            }
            DisplayStatusMessage("请求未决故障码列表");
            OBDParameterValue value2 = m_obdInterface.GetValue("SAE.PENDING_DTCS");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value2.ErrorDetected) {
                m_bReportForm.ReportPage1.PendingList.Clear();
                foreach (string strVal in value2.ListStringValue) {
                    m_bReportForm.ReportPage1.PendingList.Add(strVal);
                    DisplayDetailMessage("未决故障码: " + strVal);
                    DTC dtc = m_obdInterface.GetDTC(strVal);
                    if (dtc != null) {
                        m_bReportForm.ReportPage1.PendingDefinitionList.Add(dtc.Description);
                    }
                }
            }

            DisplayStatusMessage("检查冻结帧数据");
            OBDParameter parameter = m_obdInterface.LookupParameter("SAE.FF_DTC");
            if (parameter != null) {
                OBDParameter freezeFrameCopy = parameter.GetFreezeFrameCopy(0);
                value2 = m_obdInterface.GetValue(freezeFrameCopy);
                this.BeginInvoke((EventHandler)delegate {
                    progressBar.Value += 1;
                });
                if (!value2.ErrorDetected) {
                    m_bReportForm.ReportPage1.FreezeFrameDTC = value2.StringValue;
                    DisplayDetailMessage("找到冻结帧数据 " + value2.StringValue);
                    CollectFreezeFrameData();
                } else {
                    m_bReportForm.ReportPage1.FreezeFrameDTC = "P0000";
                    DisplayDetailMessage("未找到冻结帧数据");
                }
                CollectMonitoringTestData();
                this.BeginInvoke((EventHandler)delegate {
                    progressBar.Value = progressBar.Maximum;
                    btnGenerate.Enabled = true;
                    // ReportForm窗体类里需要调用SaveFileDialog.ShowDialog()
                    // 该方法需要调用COM对象（系统中的打开文件通用对话框），故需要调用者线程具有STA模式（单线程单元模式）
                    // 而Task无法显式设置线程模式为STA，故只能在主UI线程里调用ReportForm窗体类
                    m_bReportForm.ShowDialog();
                });
            }
        }

        private void CollectFreezeFrameData() {
            OBDParameter param = m_obdInterface.LookupParameter("SAE.FUEL1_STATUS");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowFuelSystemStatus = false;
            if (param != null) {
                param = param.GetFreezeFrameCopy(0);
                OBDParameterValue value17 = m_obdInterface.GetValue(param);
                if (!value17.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowFuelSystemStatus = true;
                    DisplayDetailMessage("燃油系统 1: " + value17.StringValue);
                    m_bReportForm.ReportPage1.FuelSystem1Status = value17.StringValue;
                }
            }
            OBDParameter freezeFrameCopy = m_obdInterface.LookupParameter("SAE.FUEL2_STATUS");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowFuelSystemStatus = false;
            if (freezeFrameCopy != null) {
                freezeFrameCopy = freezeFrameCopy.GetFreezeFrameCopy(0);
                OBDParameterValue value3 = m_obdInterface.GetValue(freezeFrameCopy);
                if (!value3.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowFuelSystemStatus = true;
                    DisplayDetailMessage("燃油系统 2: " + value3.StringValue);
                    m_bReportForm.ReportPage1.FuelSystem2Status = value3.StringValue;
                }
            }
            OBDParameter parameter16 = m_obdInterface.LookupParameter("SAE.LOAD_CALC");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowCalculatedLoad = false;
            if (parameter16 != null) {
                OBDParameter parameter17 = parameter16.GetFreezeFrameCopy(0);
                OBDParameterValue value2 = m_obdInterface.GetValue(parameter17);
                if (!value2.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowCalculatedLoad = true;
                    DisplayDetailMessage("计算负荷: " + value2.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.CalculatedLoad = value2.DoubleValue;
                }
            }
            OBDParameter parameter14 = m_obdInterface.LookupParameter("SAE.ECT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowEngineCoolantTemp = false;
            if (parameter14 != null) {
                parameter14 = parameter14.GetFreezeFrameCopy(0);
                OBDParameterValue value16 = m_obdInterface.GetValue(parameter14);
                if (!value16.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowEngineCoolantTemp = true;
                    DisplayDetailMessage("发动机冷却液温度: " + value16.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.EngineCoolantTemp = value16.DoubleValue;
                }
            }
            OBDParameter parameter13 = m_obdInterface.LookupParameter("SAE.STFT1");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowSTFT13 = false;
            if (parameter13 != null) {
                parameter13 = parameter13.GetFreezeFrameCopy(0);
                OBDParameterValue value15 = m_obdInterface.GetValue(parameter13);
                if (!value15.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowSTFT13 = true;
                    DisplayDetailMessage("短时燃油修正 - 组 1: " + value15.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.STFT1 = value15.DoubleValue;
                }
            }
            OBDParameter parameter12 = m_obdInterface.LookupParameter("SAE.STFT3");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowSTFT13 = false;
            if (parameter12 != null) {
                parameter12 = parameter12.GetFreezeFrameCopy(0);
                OBDParameterValue value14 = m_obdInterface.GetValue(parameter12);
                if (!value14.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowSTFT13 = true;
                    DisplayDetailMessage("短时燃油修正 - 组 3: " + value14.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.STFT3 = value14.DoubleValue;
                }
            }
            OBDParameter parameter11 = m_obdInterface.LookupParameter("SAE.LTFT1");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowLTFT13 = false;
            if (parameter11 != null) {
                parameter11 = parameter11.GetFreezeFrameCopy(0);
                OBDParameterValue value13 = m_obdInterface.GetValue(parameter11);
                if (!value13.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowLTFT13 = true;
                    DisplayDetailMessage("长时燃油修正 - 组 1: " + value13.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.LTFT1 = value13.DoubleValue;
                }
            }
            OBDParameter parameter10 = m_obdInterface.LookupParameter("SAE.LTFT3");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowLTFT13 = false;
            if (parameter10 != null) {
                parameter10 = parameter10.GetFreezeFrameCopy(0);
                OBDParameterValue value12 = m_obdInterface.GetValue(parameter10);
                if (!value12.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowLTFT13 = true;
                    DisplayDetailMessage("长时燃油修正 - 组 3: " + value12.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.LTFT3 = value12.DoubleValue;
                }
            }
            OBDParameter parameter9 = m_obdInterface.LookupParameter("SAE.STFT2");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowSTFT24 = false;
            if (parameter9 != null) {
                parameter9 = parameter9.GetFreezeFrameCopy(0);
                OBDParameterValue value11 = m_obdInterface.GetValue(parameter9);
                if (!value11.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowSTFT24 = true;
                    DisplayDetailMessage("短时燃油修正 - 组 2: " + value11.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.STFT2 = value11.DoubleValue;
                }
            }
            OBDParameter parameter8 = m_obdInterface.LookupParameter("SAE.STFT4");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowSTFT24 = false;
            if (parameter8 != null) {
                parameter8 = parameter8.GetFreezeFrameCopy(0);
                OBDParameterValue value10 = m_obdInterface.GetValue(parameter8);
                if (!value10.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowSTFT24 = true;
                    DisplayDetailMessage("短时燃油修正 - 组 4: " + value10.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.STFT4 = value10.DoubleValue;
                }
            }
            OBDParameter parameter7 = m_obdInterface.LookupParameter("SAE.LTFT2");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowLTFT24 = false;
            if (parameter7 != null) {
                parameter7 = parameter7.GetFreezeFrameCopy(0);
                OBDParameterValue value9 = m_obdInterface.GetValue(parameter7);
                if (!value9.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowLTFT24 = true;
                    DisplayDetailMessage("长时燃油修正 - 组 2: " + value9.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.LTFT2 = value9.DoubleValue;
                }
            }
            OBDParameter parameter6 = m_obdInterface.LookupParameter("SAE.LTFT4");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowLTFT24 = false;
            if (parameter6 != null) {
                parameter6 = parameter6.GetFreezeFrameCopy(0);
                OBDParameterValue value8 = m_obdInterface.GetValue(parameter6);
                if (!value8.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowLTFT24 = true;
                    DisplayDetailMessage("长时燃油修正 - 组 4: " + value8.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.LTFT4 = value8.DoubleValue;
                }
            }
            OBDParameter parameter5 = m_obdInterface.LookupParameter("SAE.MAP");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowIntakePressure = false;
            if (parameter5 != null) {
                parameter5 = parameter5.GetFreezeFrameCopy(0);
                OBDParameterValue value7 = m_obdInterface.GetValue(parameter5);
                if (!value7.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowIntakePressure = true;
                    DisplayDetailMessage("进气歧管压力: " + value7.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.IntakePressure = value7.DoubleValue;
                }
            }
            OBDParameter parameter4 = m_obdInterface.LookupParameter("SAE.RPM");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowEngineRPM = false;
            if (parameter4 != null) {
                parameter4 = parameter4.GetFreezeFrameCopy(0);
                OBDParameterValue value6 = m_obdInterface.GetValue(parameter4);
                if (!value6.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowEngineRPM = true;
                    DisplayDetailMessage("发动机转数: " + value6.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.EngineRPM = value6.DoubleValue;
                }
            }
            OBDParameter parameter3 = m_obdInterface.LookupParameter("SAE.VSS");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowVehicleSpeed = false;
            if (parameter3 != null) {
                parameter3 = parameter3.GetFreezeFrameCopy(0);
                OBDParameterValue value5 = m_obdInterface.GetValue(parameter3);
                if (!value5.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowVehicleSpeed = true;
                    DisplayDetailMessage("车辆速度: " + value5.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.VehicleSpeed = value5.DoubleValue;
                }
            }
            OBDParameter parameter2 = m_obdInterface.LookupParameter("SAE.SPARKADV");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            m_bReportForm.ReportPage1.ShowSparkAdvance = false;
            if (parameter2 != null) {
                parameter2 = parameter2.GetFreezeFrameCopy(0);
                OBDParameterValue value4 = m_obdInterface.GetValue(parameter2);
                if (!value4.ErrorDetected) {
                    m_bReportForm.ReportPage1.ShowSparkAdvance = true;
                    DisplayDetailMessage("点火提前角: " + value4.DoubleValue.ToString());
                    m_bReportForm.ReportPage1.SparkAdvance = value4.DoubleValue;
                }
            }
        }

        private void CollectMonitoringTestData() {
            DisplayStatusMessage("读取故障诊断器结果");
            OBDParameterValue value23 = m_obdInterface.GetValue("SAE.MISFIRE_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value23.ErrorDetected) {
                if (value23.BoolValue) {
                    m_bReportForm.ReportPage1.MisfireMonitorSupported = true;
                    OBDParameterValue value22 = m_obdInterface.GetValue("SAE.MISFIRE_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value22.ErrorDetected) {
                        if (value22.BoolValue) {
                            DisplayDetailMessage("失火就绪状态?: 完成");
                            m_bReportForm.ReportPage1.MisfireMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("失火就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.MisfireMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持失火?: 不适用");
                    m_bReportForm.ReportPage1.MisfireMonitorSupported = false;
                }
            }
            OBDParameterValue value21 = m_obdInterface.GetValue("SAE.FUEL_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value21.ErrorDetected) {
                if (value21.BoolValue) {
                    m_bReportForm.ReportPage1.FuelSystemMonitorSupported = true;
                    OBDParameterValue value20 = m_obdInterface.GetValue("SAE.FUEL_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value20.ErrorDetected) {
                        if (value20.BoolValue) {
                            DisplayDetailMessage("燃油系统就绪状态?: 完成");
                            m_bReportForm.ReportPage1.FuelSystemMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("燃油系统就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.FuelSystemMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持燃油系统?: 不适用");
                    m_bReportForm.ReportPage1.FuelSystemMonitorSupported = false;
                }
            }
            OBDParameterValue value19 = m_obdInterface.GetValue("SAE.CCM_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value19.ErrorDetected) {
                if (value19.BoolValue) {
                    m_bReportForm.ReportPage1.ComprehensiveMonitorSupported = true;
                    OBDParameterValue value18 = m_obdInterface.GetValue("SAE.CCM_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value18.ErrorDetected) {
                        if (value18.BoolValue) {
                            DisplayDetailMessage("综合部件就绪状态?: 完成");
                            m_bReportForm.ReportPage1.ComprehensiveMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("综合部件就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.ComprehensiveMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持综合部件?: 不适用");
                    m_bReportForm.ReportPage1.ComprehensiveMonitorSupported = false;
                }
            }
            OBDParameterValue value17 = m_obdInterface.GetValue("SAE.CAT_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value17.ErrorDetected) {
                if (value17.BoolValue) {
                    m_bReportForm.ReportPage1.CatalystMonitorSupported = true;
                    OBDParameterValue value16 = m_obdInterface.GetValue("SAE.CAT_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value16.ErrorDetected) {
                        if (value16.BoolValue) {
                            DisplayDetailMessage("催化器就绪状态?: 完成");
                            m_bReportForm.ReportPage1.CatalystMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("催化器就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.CatalystMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持催化器?: 不适用");
                    m_bReportForm.ReportPage1.CatalystMonitorSupported = false;
                }
            }
            OBDParameterValue value15 = m_obdInterface.GetValue("SAE.HCAT_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value15.ErrorDetected) {
                if (value15.BoolValue) {
                    m_bReportForm.ReportPage1.HeatedCatalystMonitorSupported = true;
                    OBDParameterValue value14 = m_obdInterface.GetValue("SAE.HCAT_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value14.ErrorDetected) {
                        if (value14.BoolValue) {
                            DisplayDetailMessage("加热催化器就绪状态?: 完成");
                            m_bReportForm.ReportPage1.HeatedCatalystMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("加热催化器就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.HeatedCatalystMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持加热催化器?: 不适用");
                    m_bReportForm.ReportPage1.HeatedCatalystMonitorSupported = false;
                }
            }
            OBDParameterValue value13 = m_obdInterface.GetValue("SAE.EVAP_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value13.ErrorDetected) {
                if (value13.BoolValue) {
                    m_bReportForm.ReportPage1.EvapSystemMonitorSupported = true;
                    OBDParameterValue value12 = m_obdInterface.GetValue("SAE.EVAP_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value12.ErrorDetected) {
                        if (value12.BoolValue) {
                            DisplayDetailMessage("蒸发系统就绪状态?: 完成");
                            m_bReportForm.ReportPage1.EvapSystemMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("蒸发系统就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.EvapSystemMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持蒸发系统?: 不适用");
                    m_bReportForm.ReportPage1.EvapSystemMonitorSupported = false;
                }
            }
            OBDParameterValue value11 = m_obdInterface.GetValue("SAE.AIR_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value11.ErrorDetected) {
                if (value11.BoolValue) {
                    m_bReportForm.ReportPage1.SecondaryAirMonitorSupported = true;
                    OBDParameterValue value10 = m_obdInterface.GetValue("SAE.AIR_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value10.ErrorDetected) {
                        if (value10.BoolValue) {
                            DisplayDetailMessage("二次空气系统就绪状态?: 完成");
                            m_bReportForm.ReportPage1.SecondaryAirMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("二次空气系统就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.SecondaryAirMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持二次空气系统?: 不适用");
                    m_bReportForm.ReportPage1.SecondaryAirMonitorSupported = false;
                }
            }
            OBDParameterValue value9 = m_obdInterface.GetValue("SAE.AC_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value9.ErrorDetected) {
                if (value9.BoolValue) {
                    m_bReportForm.ReportPage1.RefrigerantMonitorSupported = true;
                    OBDParameterValue value8 = m_obdInterface.GetValue("SAE.AC_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value8.ErrorDetected) {
                        if (value8.BoolValue) {
                            DisplayDetailMessage("A/C系统制冷剂就绪状态?: 完成");
                            m_bReportForm.ReportPage1.RefrigerantMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("A/C系统制冷剂就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.RefrigerantMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持A/C系统制冷剂?: 不适用");
                    m_bReportForm.ReportPage1.RefrigerantMonitorSupported = false;
                }
            }
            OBDParameterValue value7 = m_obdInterface.GetValue("SAE.O2_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value7.ErrorDetected) {
                if (value7.BoolValue) {
                    m_bReportForm.ReportPage1.OxygenSensorMonitorSupported = true;
                    OBDParameterValue value6 = m_obdInterface.GetValue("SAE.O2_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value6.ErrorDetected) {
                        if (value6.BoolValue) {
                            DisplayDetailMessage("氧气传感器就绪状态?: 完成");
                            m_bReportForm.ReportPage1.OxygenSensorMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("氧气传感器就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.OxygenSensorMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持氧气传感器?: 不适用");
                    m_bReportForm.ReportPage1.OxygenSensorMonitorSupported = false;
                }
            }
            OBDParameterValue value5 = m_obdInterface.GetValue("SAE.O2HTR_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value5.ErrorDetected) {
                if (value5.BoolValue) {
                    m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorSupported = true;
                    OBDParameterValue value4 = m_obdInterface.GetValue("SAE.O2HTR_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value4.ErrorDetected) {
                        if (value4.BoolValue) {
                            DisplayDetailMessage("加热氧气传感器就绪状态?: 完成");
                            m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("加热氧气传感器就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持加热氧气传感器?: 不适用");
                    m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorSupported = false;
                }
            }
            OBDParameterValue value3 = m_obdInterface.GetValue("SAE.EGR_SUPPORT");
            this.BeginInvoke((EventHandler)delegate {
                progressBar.Value += 1;
            });
            if (!value3.ErrorDetected) {
                if (value3.BoolValue) {
                    m_bReportForm.ReportPage1.EGRSystemMonitorSupported = true;
                    OBDParameterValue value2 = m_obdInterface.GetValue("SAE.EGR_STATUS");
                    this.BeginInvoke((EventHandler)delegate {
                        progressBar.Value += 1;
                    });
                    if (!value2.ErrorDetected) {
                        if (value2.BoolValue) {
                            DisplayDetailMessage("EGR系统就绪状态?: 完成");
                            m_bReportForm.ReportPage1.EGRSystemMonitorCompleted = true;
                        } else {
                            DisplayDetailMessage("EGR系统就绪状态?: 未完成");
                            m_bReportForm.ReportPage1.EGRSystemMonitorCompleted = false;
                        }
                    }
                } else {
                    DisplayDetailMessage("支持EGR系统?: 不适用");
                    m_bReportForm.ReportPage1.EGRSystemMonitorSupported = false;
                }
            }
        }

        private void DisplayStatusMessage(string str) {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 12f, FontStyle.Bold);
                richTextStatus.SelectionColor = Color.Blue;
                richTextStatus.AppendText(str + "\r\n");
            });
        }

        private void DisplayRequest(string str) {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 10f, FontStyle.Bold);
                richTextStatus.SelectionColor = Color.Black;
                richTextStatus.AppendText("   发送: ");
                richTextStatus.SelectionColor = Color.Green;
                richTextStatus.AppendText(str);
                richTextStatus.AppendText("\r\n");
            });
        }

        private void DisplayValidResponse(string str) {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 10f, FontStyle.Bold);
                Color black = Color.Black;
                richTextStatus.SelectionColor = black;
                richTextStatus.AppendText("   接收: ");
                Color green = Color.Green;
                richTextStatus.SelectionColor = green;
                richTextStatus.AppendText(str);
                richTextStatus.AppendText("\r\n");
            });
        }

        private void DisplayInvalidResponse(string str) {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 10f, FontStyle.Bold);
                Color black = Color.Black;
                richTextStatus.SelectionColor = black;
                richTextStatus.AppendText("   接收: ");
                Color red = Color.Red;
                richTextStatus.SelectionColor = red;
                richTextStatus.AppendText(str);
                richTextStatus.AppendText("\r\n");
            });
        }

        private void DisplayDetailMessage(string str) {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 10f, FontStyle.Regular);
                Color black = Color.Black;
                richTextStatus.SelectionColor = black;
                richTextStatus.AppendText("      " + str + "\r\n");
            });
        }

        private void DisplayBlankLine() {
            this.BeginInvoke((EventHandler)delegate {
                richTextStatus.SelectionFont = new Font("Times New Roman", 10f, FontStyle.Regular);
                richTextStatus.AppendText("\r\n");
            });
        }

        private void BtnOpen_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "打开 OBD-II 诊断报告",
                Filter = "报告文件 (*.obd)|*.obd",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            try {
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length <= 0) {
                    return;
                }
                FileStream fileStream = File.OpenRead(openFileDialog.FileName);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                m_bReportForm = new ReportForm();
                m_bReportForm.ReportPage1.ShopName = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ShopAddress1 = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ShopAddress2 = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ShopTelephone = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ClientName = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ClientAddress1 = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ClientAddress2 = binaryReader.ReadString();
                m_bReportForm.ReportPage1.ClientTelephone = binaryReader.ReadString();
                m_bReportForm.ReportPage1.Vehicle = binaryReader.ReadString();
                m_bReportForm.ReportPage1.GenerationDate = binaryReader.ReadString();
                m_bReportForm.ReportPage1.MilStatus = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.TotalCodes = binaryReader.ReadInt32();
                m_bReportForm.ReportPage1.FreezeFrameDTC = binaryReader.ReadString();
                List<string> stringList1 = new List<string>();
                for (uint i = 25U; i > 0U; i--) {
                    string str = binaryReader.ReadString();
                    if (str.Length > 0) {
                        stringList1.Add(str);
                    }
                }
                m_bReportForm.ReportPage1.DTCList = stringList1;
                List<string> stringList2 = new List<string>();
                for (uint i = 25U; i > 0U; i--) {
                    string str = binaryReader.ReadString();
                    if (str.Length > 0) {
                        stringList2.Add(str);
                    }
                }
                m_bReportForm.ReportPage1.DTCDefinitionList = stringList2;
                List<string> stringList3 = new List<string>();
                for (uint i = 25U; i > 0U; i--) {
                    string str = binaryReader.ReadString();
                    if (str.Length > 0) {
                        stringList3.Add(str);
                    }
                }
                m_bReportForm.ReportPage1.PendingList = stringList3;
                List<string> stringList4 = new List<string>();
                for (uint i = 25U; i > 0U; i--) {
                    string str = binaryReader.ReadString();
                    if (str.Length > 0) {
                        stringList4.Add(str);
                    }
                }
                m_bReportForm.ReportPage1.PendingDefinitionList = stringList4;
                m_bReportForm.ReportPage1.FuelSystem1Status = binaryReader.ReadString();
                m_bReportForm.ReportPage1.FuelSystem2Status = binaryReader.ReadString();
                m_bReportForm.ReportPage1.CalculatedLoad = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.EngineCoolantTemp = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.STFT1 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.STFT2 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.STFT3 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.STFT4 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.LTFT1 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.LTFT2 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.LTFT3 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.LTFT4 = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.IntakePressure = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.EngineRPM = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.VehicleSpeed = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.SparkAdvance = binaryReader.ReadDouble();
                m_bReportForm.ReportPage1.ShowFuelSystemStatus = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowCalculatedLoad = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowEngineCoolantTemp = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowSTFT13 = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowSTFT24 = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowLTFT13 = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowLTFT24 = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowIntakePressure = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowEngineRPM = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowVehicleSpeed = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ShowSparkAdvance = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.MisfireMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.MisfireMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.FuelSystemMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.FuelSystemMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ComprehensiveMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.ComprehensiveMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.CatalystMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.CatalystMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.HeatedCatalystMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.HeatedCatalystMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.EvapSystemMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.EvapSystemMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.SecondaryAirMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.SecondaryAirMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.RefrigerantMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.RefrigerantMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.OxygenSensorMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.OxygenSensorMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.OxygenSensorHeaterMonitorCompleted = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.EGRSystemMonitorSupported = binaryReader.ReadBoolean();
                m_bReportForm.ReportPage1.EGRSystemMonitorCompleted = binaryReader.ReadBoolean();
                binaryReader.Close();
                fileStream.Close();
                m_bReportForm.ShowDialog();
            } finally {
                openFileDialog.Dispose();
            }
        }

        private void ReportGeneratorForm_Load(object sender, EventArgs e) {
            txtByName.Text = m_obdInterface.UserPreferences.Name;
            txtByAddress1.Text = m_obdInterface.UserPreferences.Address1;
            txtByAddress2.Text = m_obdInterface.UserPreferences.Address2;
            txtByTelephone.Text = m_obdInterface.UserPreferences.Telephone;
        }

        private void ReportGeneratorForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                txtByName.Text = m_obdInterface.UserPreferences.Name;
                txtByAddress1.Text = m_obdInterface.UserPreferences.Address1;
                txtByAddress2.Text = m_obdInterface.UserPreferences.Address2;
                txtByTelephone.Text = m_obdInterface.UserPreferences.Telephone;
            }
        }

    }
}
