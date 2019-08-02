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
                m_obdInterface.GetLogger().TraceError("Freeze Frame Form, Attempted to refresh without vehicle connection.");
                MessageBox.Show(
                    "A vehicle connection must first be established.",
                    "Connection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
            }
        }

        private void ReadFreezeFrameData() {
            while (true) {
                this.BeginInvoke(new Action(() => {
                    freezeFrame.Reset();
                }));
                OBDParameter parameter = m_obdInterface.LookupParameter("SAE.FF_DTC");
                if (parameter == null) {
                    MessageBox.Show(
                        "加载SAE.FF_DTC参数时发生错误",
                        "出错",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                    );
                    break;
                }

                OBDParameterValue value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (value.ErrorDetected) {
                    m_obdInterface.GetLogger().TraceError("Error while requesting SAE.FF_DTC");
                    MessageBox.Show(
                        "请求SAE.FF_DTC命令时发生错误",
                        "出错",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                    );
                    //break;
                }

                if (string.Compare(value.StringValue, "P0000") == 0) {
                    MessageBox.Show(
                        string.Format("在 #{0} 帧中，没有发现冻结帧信息", m_FrameNumber),
                        "信息",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk
                    );
                    break;
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Value = 0;
                    freezeFrame.DTC = value.StringValue;
                }));

                parameter = m_obdInterface.LookupParameter("SAE.FUEL1_STATUS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.FuelSystem1Status = value.StringValue;
                    }));
                }

                parameter = m_obdInterface.LookupParameter("SAE.FUEL2_STATUS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.FuelSystem2Status = value.StringValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LOAD_CALC");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.CalculatedLoad = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.ECT");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.EngineCoolantTemp = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT1");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.STFT1 = value.DoubleValue;
                    }));
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT3");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.STFT3 = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT1");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.LTFT1 = value.DoubleValue;
                    }));
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT3");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.LTFT3 = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT2");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.STFT2 = value.DoubleValue;
                    }));
                }

                parameter = m_obdInterface.LookupParameter("SAE.STFT4");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.STFT4 = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;

                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT2");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.LTFT2 = value.DoubleValue;
                    }));
                }

                parameter = m_obdInterface.LookupParameter("SAE.LTFT4");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.LTFT4 = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.MAP");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.IntakePressure = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.RPM");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.EngineRPM = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.VSS");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.VehicleSpeed = value.DoubleValue;
                    }));
                }

                this.BeginInvoke(new Action(() => {
                    progressBar.Increment(progressBar.Step);
                }));
                if (!m_KeepReading) {
                    break;
                }

                parameter = m_obdInterface.LookupParameter("SAE.SPARKADV");
                if (parameter == null) {
                    break;
                }

                value = m_obdInterface.GetValue(parameter.GetFreezeFrameCopy(m_FrameNumber), true);
                if (!value.ErrorDetected) {
                    this.BeginInvoke(new Action(() => {
                        freezeFrame.SparkAdvance = value.DoubleValue;
                    }));
                }

                break;
            }
            this.BeginInvoke(new Action(() => {
                progressBar.Value = progressBar.Maximum;
                btnRefresh.Enabled = true;
                btnCancel.Enabled = false;
            }));
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            m_KeepReading = false;
            btnRefresh.Enabled = true;
            btnCancel.Enabled = false;
        }

    }
}
