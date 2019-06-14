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
        private FuelEconomyForm m_FuelEconomyForm;
        private OBDInterface m_obdInterface;
        private double m_TotalFuelConsumption;
        private double m_TotalDistance;
        public bool m_RunThread;
        public bool IsWorking;
        private DateTime m_StartTime;
        private DateTime m_PrevTime;

        public FuelEconomyForm(OBDInterface obd) {
            m_FuelEconomyForm = this;
            InitializeComponent();
            m_obdInterface = obd;
            m_RunThread = true;
            IsWorking = false;

            sensorInstantFuelConsumption.SetDisplayMode(1);
            sensorAvgFuelConsumption.SetDisplayMode(1);
            sensorAvgFuelEconomy.SetDisplayMode(1);
            sensorInstantFuelEconomy.SetDisplayMode(1);
            sensorTotalConsumed.SetDisplayMode(1);
            sensorDistance.SetDisplayMode(1);
            sensorTotalCost.SetDisplayMode(1);
            sensorCostPerMile.SetDisplayMode(1);
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
                        double hours_start = now.Subtract(m_FuelEconomyForm.m_StartTime).TotalSeconds * 0.00027777777777777778;
                        double hours_prev = now.Subtract(m_FuelEconomyForm.m_PrevTime).TotalSeconds * 0.00027777777777777778;
                        m_FuelEconomyForm.m_PrevTime = now;

                        if (m_FuelEconomyForm.radioEnglishUnits.Checked) {
                            m_FuelEconomyForm.sensorInstantFuelConsumption.EnglishDisplay = fuel_gallons_hour.ToString("0.000") + " gallons/hour";
                            m_FuelEconomyForm.m_TotalFuelConsumption += hours_prev * fuel_gallons_hour;
                            double total = m_FuelEconomyForm.m_TotalFuelConsumption;
                            sensorTotalConsumed.EnglishDisplay = total.ToString("0.00") + " gallons";
                            m_FuelEconomyForm.sensorAvgFuelConsumption.EnglishDisplay = ((m_FuelEconomyForm.m_TotalFuelConsumption / hours_start)).ToString("0.00") + " gallons/hour";
                            m_FuelEconomyForm.m_TotalDistance += hours_prev * speed_miles;
                            m_FuelEconomyForm.sensorDistance.EnglishDisplay = m_TotalDistance.ToString("0.00") + " miles";
                            m_FuelEconomyForm.sensorInstantFuelEconomy.EnglishDisplay = (((1.0 / fuel_gallons_hour) * speed_miles)).ToString("0.00") + " miles/gallon";
                            double miles_gallon = 0.0;
                            if (m_FuelEconomyForm.m_TotalDistance > 0.0)
                                miles_gallon = m_FuelEconomyForm.m_TotalDistance / m_FuelEconomyForm.m_TotalFuelConsumption;

                            m_FuelEconomyForm.sensorAvgFuelEconomy.EnglishDisplay = miles_gallon.ToString("0.00") + " miles/gallon";
                            double cost_per_mile = 0.0;
                            if (miles_gallon > 0.0)
                                cost_per_mile = ((double)numericFuelCost.Value) * (1.0 / miles_gallon);

                            m_FuelEconomyForm.sensorCostPerMile.Title = "Average Cost Per Mile";
                            m_FuelEconomyForm.sensorCostPerMile.EnglishDisplay = "$" + cost_per_mile.ToString("0.00");
                            m_FuelEconomyForm.sensorTotalCost.EnglishDisplay = "$" + ((((double)numericFuelCost.Value) * m_TotalFuelConsumption)).ToString("0.00");
                        } else {
                            m_FuelEconomyForm.sensorInstantFuelConsumption.EnglishDisplay = fuel_liters_hour.ToString("0.000") + " liters/hour";
                            m_FuelEconomyForm.m_TotalFuelConsumption += hours_prev * fuel_liters_hour;
                            double total = m_FuelEconomyForm.m_TotalFuelConsumption;
                            sensorTotalConsumed.EnglishDisplay = total.ToString("0.00") + " liters";
                            m_FuelEconomyForm.sensorAvgFuelConsumption.EnglishDisplay = ((m_FuelEconomyForm.m_TotalFuelConsumption / hours_start)).ToString("0.00") + " liters/hour";
                            m_FuelEconomyForm.m_TotalDistance += hours_prev * sae_vss_double;
                            m_FuelEconomyForm.sensorDistance.EnglishDisplay = m_TotalDistance.ToString("0.00") + " kilometers";
                            m_FuelEconomyForm.sensorInstantFuelEconomy.EnglishDisplay = (((1.0 / fuel_liters_hour) * sae_vss_double)).ToString("0.00") + " kilometers/liter";
                            double kilometers_liter = 0.0;
                            if (m_FuelEconomyForm.m_TotalDistance > 0.0)
                                kilometers_liter = m_FuelEconomyForm.m_TotalDistance / m_FuelEconomyForm.m_TotalFuelConsumption;

                            m_FuelEconomyForm.sensorAvgFuelEconomy.EnglishDisplay = kilometers_liter.ToString("0.00") + " kilometers/liter";
                            double cost_per_kilometer = 0.0;
                            if (kilometers_liter > 0.0)
                                cost_per_kilometer = ((double)numericFuelCost.Value) * (1.0 / kilometers_liter);

                            m_FuelEconomyForm.sensorCostPerMile.Title = "Average Cost Per Kilometer";
                            m_FuelEconomyForm.sensorCostPerMile.EnglishDisplay = cost_per_kilometer.ToString("0.00");
                            m_FuelEconomyForm.sensorTotalCost.EnglishDisplay = (((double)numericFuelCost.Value) * m_TotalFuelConsumption).ToString("0.00");
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
