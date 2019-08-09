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
    public partial class FuelEconomyForm : Form {
        private readonly OBDInterface m_obdInterface;
        private double m_TotalFuelConsumption;
        private double m_TotalDistance;
        public bool m_RunThread;
        public bool IsWorking;
        private DateTime m_StartTime;
        private DateTime m_PrevTime;

        public FuelEconomyForm(OBDInterface obd) {
            InitializeComponent();
            m_obdInterface = obd;
            m_RunThread = true;
            IsWorking = false;

            this.sensorInstantFuelConsumption.SetDisplayMode(1);
            this.sensorAvgFuelConsumption.SetDisplayMode(1);
            this.sensorAvgFuelEconomy.SetDisplayMode(1);
            this.sensorInstantFuelEconomy.SetDisplayMode(1);
            this.sensorTotalConsumed.SetDisplayMode(1);
            this.sensorDistance.SetDisplayMode(1);
            this.sensorTotalCost.SetDisplayMode(1);
            this.sensorCostPerMile.SetDisplayMode(1);
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                groupSetup.Enabled = true;
                groupControl.Enabled = true;
            } else {
                groupSetup.Enabled = false;
                groupControl.Enabled = false;
            }
        }

        public void StartWorking() {
            m_TotalDistance = 0.0;
            m_TotalFuelConsumption = 0.0;
            m_StartTime = DateTime.Now;
            m_PrevTime = DateTime.Now;
            IsWorking = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            groupSetup.Enabled = false;
        }

        public void StopWorking() {
            IsWorking = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            groupSetup.Enabled = true;
        }

        private void radioEnglishUnits_CheckedChanged(object sender, EventArgs e) {
            if (radioEnglishUnits.Checked) {
                labelFuelUnit.Text = "/ 加仑";
            } else {
                labelFuelUnit.Text = "/ 升";
            }
        }

        private void btnStart_Click(object sender, EventArgs e) {
            StartWorking();
        }

        private void UpdateThread() {
            if (!m_RunThread) {
                return;
            }

            do {
                if (m_obdInterface.ConnectedStatus && IsWorking) {
                    OBDParameterValue sae_maf = m_obdInterface.GetValue("SAE.MAF", false);
                    OBDParameterValue sae_vss = m_obdInterface.GetValue("SAE.VSS", false);
                    if (!sae_maf.ErrorDetected && !sae_vss.ErrorDetected) {
                        double sae_vss_double = sae_vss.DoubleValue;
                        double speed_miles = sae_vss_double * 0.621371192;
                        double fuel_liters_hour = ((sae_maf.DoubleValue * 0.068027210884353748) * 3600.0) * 0.0013020833333333333;
                        double fuel_gallons_hour = fuel_liters_hour * 0.264172052;

                        DateTime now = DateTime.Now;
                        double hours_start = now.Subtract(this.m_StartTime).TotalSeconds * 0.00027777777777777778;
                        double hours_prev = now.Subtract(this.m_PrevTime).TotalSeconds * 0.00027777777777777778;
                        this.m_PrevTime = now;

                        if (this.radioEnglishUnits.Checked) {
                            this.sensorInstantFuelConsumption.EnglishDisplay = fuel_gallons_hour.ToString("0.000") + " 加仑 / 小时";
                            this.m_TotalFuelConsumption += hours_prev * fuel_gallons_hour;
                            double total = this.m_TotalFuelConsumption;
                            sensorTotalConsumed.EnglishDisplay = total.ToString("0.00") + " 加仑";
                            this.sensorAvgFuelConsumption.EnglishDisplay = ((this.m_TotalFuelConsumption / hours_start)).ToString("0.00") + " 加仑 / 小时";
                            this.m_TotalDistance += hours_prev * speed_miles;
                            this.sensorDistance.EnglishDisplay = m_TotalDistance.ToString("0.00") + " 英里";
                            this.sensorInstantFuelEconomy.EnglishDisplay = (((1.0 / fuel_gallons_hour) * speed_miles)).ToString("0.00") + " 英里 / 加仑";
                            double miles_gallon = 0.0;
                            if (this.m_TotalDistance > 0.0) {
                                miles_gallon = this.m_TotalDistance / this.m_TotalFuelConsumption;
                            }

                            this.sensorAvgFuelEconomy.EnglishDisplay = miles_gallon.ToString("0.00") + " 英里 / 加仑";
                            double cost_per_mile = 0.0;
                            if (miles_gallon > 0.0) {
                                cost_per_mile = ((double)numericFuelCost.Value) * (1.0 / miles_gallon);
                            }

                            this.sensorCostPerMile.Title = "每英里的平均成本";
                            this.sensorCostPerMile.EnglishDisplay = "$" + cost_per_mile.ToString("0.00");
                            this.sensorTotalCost.EnglishDisplay = "$" + ((((double)numericFuelCost.Value) * m_TotalFuelConsumption)).ToString("0.00");
                        } else {
                            this.sensorInstantFuelConsumption.EnglishDisplay = fuel_liters_hour.ToString("0.000") + " 升 / 小时";
                            this.m_TotalFuelConsumption += hours_prev * fuel_liters_hour;
                            double total = this.m_TotalFuelConsumption;
                            sensorTotalConsumed.EnglishDisplay = total.ToString("0.00") + " 升";
                            this.sensorAvgFuelConsumption.EnglishDisplay = ((this.m_TotalFuelConsumption / hours_start)).ToString("0.00") + " 升 / 小时";
                            this.m_TotalDistance += hours_prev * sae_vss_double;
                            this.sensorDistance.EnglishDisplay = m_TotalDistance.ToString("0.00") + " 千米";
                            this.sensorInstantFuelEconomy.EnglishDisplay = (((1.0 / fuel_liters_hour) * sae_vss_double)).ToString("0.00") + " 千米 / 升";
                            double kilometers_liter = 0.0;
                            if (this.m_TotalDistance > 0.0) {
                                kilometers_liter = this.m_TotalDistance / this.m_TotalFuelConsumption;
                            }

                            this.sensorAvgFuelEconomy.EnglishDisplay = kilometers_liter.ToString("0.00") + " 千米 / 升";
                            double cost_per_kilometer = 0.0;
                            if (kilometers_liter > 0.0) {
                                cost_per_kilometer = ((double)numericFuelCost.Value) * (1.0 / kilometers_liter);
                            }

                            this.sensorCostPerMile.Title = "每千米的平均成本";
                            this.sensorCostPerMile.EnglishDisplay = cost_per_kilometer.ToString("0.00");
                            this.sensorTotalCost.EnglishDisplay = (((double)numericFuelCost.Value) * m_TotalFuelConsumption).ToString("0.00");
                        }
                    }
                } else {
                    Thread.Sleep(300);
                }
            } while (m_RunThread);
        }

        private void FuelEconomyForm_Load(object sender, EventArgs e) {
            CheckConnection();
            Task.Factory.StartNew(UpdateThread);
        }

        private void btnStop_Click(object sender, EventArgs e) {
            StopWorking();
        }

    }
}
