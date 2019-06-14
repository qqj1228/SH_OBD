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
    public partial class FreezeFramesForm : Form {
        private OBDInterface m_obdInterface;
        private int m_FrameNumber;
        private bool m_KeepReading;

        public FreezeFramesForm(OBDInterface obd) {
            InitializeComponent();
            m_obdInterface = obd;
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            if (m_obdInterface.ConnectedStatus) {
                btnRefresh.Enabled = false;
                btnCancel.Enabled = true;
                m_KeepReading = true;
                m_FrameNumber = Convert.ToInt32(numFrame.Value);
                Task.Factory.StartNew(ReadFreezeFrameData);
            } else {
                MessageBox.Show(
                    "A vehicle connection must first be established.",
                    "Connection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                    );
                m_obdInterface.LogItem("Error. Freeze Frame Form. Attempted to refresh without vehicle connection.");
            }
        }

        private void ReadFreezeFrameData() {
            while (true) {
                freezeFrame.Reset();
                OBDParameter parameter = m_obdInterface.LookupParameter("SAE.FF_DTC");
                if (parameter == null) {
                    MessageBox.Show(
                        "An error was encountered while requesting SAE.FF_DTC",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                        );
                    break;
                }

                OBDParameterValue value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (value.ErrorDetected) {
                    MessageBox.Show(
                        "An error was encountered while requesting SAE.FF_DTC",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                        );
                    m_obdInterface.LogItem("Error while requesting SAE.FF_DTC");
                    break;
                }

                if (string.Compare(value.StringValue, "P0000") == 0) {
                    MessageBox.Show(
                        string.Format("No freeze frame information found at frame #{0}.", m_FrameNumber),
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk
                        );
                    break;
                }

                progressBar.Value = 0;
                freezeFrame.DTC = value.StringValue;

                parameter = m_obdInterface.LookupParameter("SAE.FUEL1_STATUS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.FuelSystem1Status = value.StringValue;
                }

                parameter = m_obdInterface.LookupParameter("SAE.FUEL2_STATUS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.FuelSystem2Status = value.StringValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LOAD_CALC");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.CalculatedLoad = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.ECT");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.EngineCoolantTemp = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT1");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.STFT1 = value.DoubleValue;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT3");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.STFT3 = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT1");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.LTFT1 = value.DoubleValue;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT3");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.LTFT3 = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT2");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.STFT2 = value.DoubleValue;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT4");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.STFT4 = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;

                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT2");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.LTFT2 = value.DoubleValue;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT4");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.LTFT4 = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.MAP");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.IntakePressure = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.RPM");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.EngineRPM = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.VSS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.VehicleSpeed = value.DoubleValue;
                }

                progressBar.Increment(progressBar.Step);
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.SPARKADV");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    freezeFrame.SparkAdvance = value.DoubleValue;
                }

                break;
            }
            progressBar.Value = progressBar.Maximum;
            btnRefresh.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            m_KeepReading = false;
            btnRefresh.Enabled = true;
            btnCancel.Enabled = false;
        }

    }
}
