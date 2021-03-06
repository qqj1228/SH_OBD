﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SH_OBD {
    public partial class SensorGridForm : Form {
        private readonly OBDInterface m_obdInterface;
        private DateTime m_dtStartTime;
        private const int controlMargin = 5;
        private const int controlHeight = 65;
        private const int controlWidth = 420;
        private static List<OBDParameter> m_ListSensors;
        private static List<SensorLogItem> m_ListLog;
        public bool IsLogging;
        public bool IsRunThread;

        public SensorGridForm(OBDInterface obd2) {
            try {
                InitializeComponent();
                m_obdInterface = obd2;
                IsLogging = false;
                btnStart.Enabled = false;
                btnReset.Enabled = false;
                btnSave.Enabled = false;
                IsRunThread = true;
                m_ListSensors = new List<OBDParameter>();
                m_ListLog = new List<SensorLogItem>();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        public new void Close() {
            IsLogging = false;
            IsRunThread = false;
            base.Close();
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                groupDisplay.Enabled = true;
                groupSelections.Enabled = true;
                groupLogging.Enabled = true;
                if (IsLogging) {
                    btnStart.Enabled = false;
                    btnStart.Text = Properties.Resources.btnStart_Name_Resume;
                    btnReset.Enabled = true;
                    btnSave.Enabled = true;
                } else {
                    btnStart.Enabled = true;
                    btnStart.Text = Properties.Resources.btnStart_Name_Start;
                    listSensors.Enabled = true;
                    btnReset.Enabled = false;
                    btnSave.Enabled = false;
                }
                listSensors.Items.Clear();
                foreach (OBDParameter obdParameter in m_obdInterface.SupportedParameterList(1)) {
                    listSensors.Items.Add(obdParameter);
                }
            } else {
                listSensors.Items.Clear();
                groupDisplay.Enabled = false;
                groupSelections.Enabled = false;
                groupLogging.Enabled = false;
            }
        }

        private void SensorMonitorForm_Resize(object sender, EventArgs e) {
            Control control;
            for (int i = 0; i < panelDisplay.Controls.Count; i++) {
                control = panelDisplay.Controls[i];
                if (panelDisplay.Width > controlWidth) {
                    control.Width = (panelDisplay.Width - 3 * controlMargin) / 2;
                    if (i % 2 == 0) {
                        control.Location = new Point(controlMargin, (control.Height + controlMargin) * (i / 2) + controlMargin);
                    } else {
                        control.Location = new Point(control.Width + controlMargin * 2, (control.Height + controlMargin) * (i / 2) + controlMargin);
                    }
                } else {
                    control.Width = panelDisplay.Width - 2 * controlMargin;
                    control.Location = new Point(controlMargin, (control.Height + controlMargin) * i + controlMargin);
                }
            }
            panelDisplay.Refresh();
        }

        private void SensorMonitorForm_Load(object sender, EventArgs e) {
            CheckConnection();
            Task.Factory.StartNew(UpdateThread);
        }

        private void radioDisplayEnglish_Click(object sender, EventArgs e) {
            RebuildSensorGrid();
        }

        private void radioDisplayMetric_Click(object sender, EventArgs e) {
            RebuildSensorGrid();
        }

        private void radioDisplayBoth_Click(object sender, EventArgs e) {
            RebuildSensorGrid();
        }

        private void listSensors_ItemCheck(object sender, ItemCheckEventArgs e) {
            SensorGridForm.m_ListSensors.Clear();
            for (int i = 0; i < listSensors.CheckedIndices.Count; i++) {
                int index = listSensors.CheckedIndices[i];
                if (index != e.Index) {
                    m_ListSensors.Add((OBDParameter)listSensors.Items[index]);
                }
            }
            if (e.CurrentValue == CheckState.Unchecked) {
                m_ListSensors.Add((OBDParameter)listSensors.Items[e.Index]);
            }
            RebuildSensorGrid();
        }

        private void UpdateThread() {
            while (IsRunThread) {
                if (m_obdInterface.ConnectedStatus && IsLogging) {
                    foreach (SensorDisplayControl control in panelDisplay.Controls) {
                        OBDParameter param = (OBDParameter)control.Tag;
                        OBDParameterValue value = m_obdInterface.GetValue(param, radioDisplayEnglish.Checked);
                        if (!value.ErrorDetected) {
                            string text = param.EnglishUnitLabel;
                            if (!radioDisplayEnglish.Checked) {
                                text = param.MetricUnitLabel;
                            }

                            SensorLogItem sensorLogItem = new SensorLogItem(
                                param.Name,
                                value.DoubleValue.ToString(),
                                text,
                                value.DoubleValue.ToString(),
                                text);
                            m_ListLog.Add(sensorLogItem);
                            this.Invoke((EventHandler)delegate {
                                scrollTime.Maximum = m_ListLog.Count - 1;
                                scrollTime.Value = scrollTime.Maximum;
                            });

                            DateTime dateTime = new DateTime(0L);
                            this.Invoke((EventHandler)delegate {
                                lblTimeElapsed.Text = dateTime.Add(sensorLogItem.Time.Subtract(m_dtStartTime)).ToString("mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
                            });

                            text = value.DoubleValue.ToString();
                            if (radioDisplayEnglish.Checked) {
                                control.EnglishDisplay = text + " " + param.EnglishUnitLabel;
                            } else {
                                control.MetricDisplay = text + " " + param.MetricUnitLabel;
                            }
                        }
                    }
                } else {
                    Thread.Sleep(300);
                }
            }
        }

        private void RebuildSensorGrid() {
            panelDisplay.Controls.Clear();
            Size controlSize = new Size(controlWidth, controlHeight);
            for (int i = 0; i < m_ListSensors.Count; i++) {
                if (panelDisplay.Width > controlWidth) {
                    controlSize.Width = (panelDisplay.Width - 3 * controlMargin) / 2;
                } else {
                    controlSize.Width = panelDisplay.Width - 2 * controlMargin;
                }
                SensorDisplayControl control = new SensorDisplayControl {
                    Title = m_ListSensors[i].Name,
                    Size = controlSize,
                    Tag = m_ListSensors[i]
                };
                if (radioDisplayEnglish.Checked) {
                    control.SetDisplayMode(1);
                } else {
                    control.SetDisplayMode(2);
                }
                control.Refresh();
                if (panelDisplay.Width > controlWidth) {
                    if (i % 2 == 0) {
                        control.Location = new Point(controlMargin, (control.Height + controlMargin) * (i / 2) + controlMargin);
                    } else {
                        control.Location = new Point(control.Width + controlMargin * 2, (control.Height + controlMargin) * (i / 2) + controlMargin);
                    }
                } else {
                    control.Location = new Point(controlMargin, (control.Height + controlMargin) * i + controlMargin);
                }
                panelDisplay.Controls.Add(control);
            }
        }

        private void btnStart_Click(object sender, EventArgs e) {
            IsLogging = !IsLogging;
            if (string.Compare(btnStart.Text, Properties.Resources.btnStart_Name_Start) == 0) {
                m_dtStartTime = DateTime.Now;
                listSensors.Enabled = false;
                groupDisplay.Enabled = false;
            }
            if (IsLogging) {
                scrollTime.Enabled = false;
                btnStart.Text = Properties.Resources.btnStart_Name_Pause;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
            } else {
                btnStart.Text = Properties.Resources.btnStart_Name_Resume;
                scrollTime.Enabled = true;
                btnReset.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (m_ListLog.Count != 0) {
                SaveFileDialog dialog = new SaveFileDialog {
                    Title = "Save Logged Data As",
                    Filter = "Comma-separated values (*.csv)|*.csv|XML (*.xml)|*.xml",
                    FilterIndex = 0,
                    RestoreDirectory = true
                };
                dialog.ShowDialog();
                if (dialog.FileName != "") {
                    if (dialog.FileName.EndsWith(".xml")) {
                        Type[] typeArray = new Type[] { typeof(SensorLogItem), typeof(Sensor) };
                        using (TextWriter writer = new StreamWriter(dialog.FileName)) {
                            new XmlSerializer(typeof(List<SensorLogItem>), typeArray).Serialize(writer, m_ListLog);
                            writer.Close();
                        }
                    } else {
                        List<string> list = new List<string>();
                        for (int i = 0; i < m_ListLog.Count; i++) {
                            string str =
                                m_ListLog[i].Time.ToString("MM-dd-yyyy hh:mm:ss.fff", DateTimeFormatInfo.InvariantInfo) + ", "
                                + m_ListLog[i].Name + ", "
                                + m_ListLog[i].EnglishDisplay + ", "
                                + m_ListLog[i].EnglishUnits + ", "
                                + m_ListLog[i].MetricDisplay + ", "
                                + m_ListLog[i].MetricUnits;
                            list.Add(str);
                        }
                        FileStream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(stream);
                        for (int i = 0; i < list.Count; i++) {
                            writer.WriteLine(list[i]);
                        }
                        writer.Close();
                        stream.Close();
                    }
                }
                dialog.Dispose();
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {
            btnReset.Enabled = false;
            btnSave.Enabled = false;
            btnStart.Enabled = true;
            btnStart.Text = Properties.Resources.btnStart_Name_Start;
            listSensors.Enabled = true;
            groupDisplay.Enabled = true;
            m_ListLog.Clear();
            lblTimeElapsed.Text = "00:00.000";
            scrollTime.Enabled = false;

            for (int i = 0; i < panelDisplay.Controls.Count; i++) {
                SensorDisplayControl control = (SensorDisplayControl)panelDisplay.Controls[i];
                control.EnglishDisplay = "";
                control.MetricDisplay = "";
            }
        }

        private void scrollTime_Scroll(object sender, ScrollEventArgs e) {
            if (scrollTime.Value < 0 || scrollTime.Value >= m_ListLog.Count) {
                return;
            }

            int index = scrollTime.Value;
            SensorLogItem log_item = m_ListLog[index];
            DateTime dateTime = new DateTime(0L);
            TimeSpan timeSpan = log_item.Time.Subtract(m_dtStartTime);
            lblTimeElapsed.Text = dateTime.Add(timeSpan).ToString("mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
            for (int i = 0; i < m_ListSensors.Count && index >= 0; i++, --index) {
                SensorLogItem sensorLogItem2 = m_ListLog[index];
                for (int j = 0; j < panelDisplay.Controls.Count; j++) {
                    SensorDisplayControl sensorDisplayControl = (SensorDisplayControl)panelDisplay.Controls[j];
                    if (string.Compare(sensorDisplayControl.Title, sensorLogItem2.Name) == 0) {
                        sensorDisplayControl.EnglishDisplay = sensorLogItem2.EnglishDisplay + " " + sensorLogItem2.EnglishUnits;
                        sensorDisplayControl.MetricDisplay = sensorLogItem2.MetricDisplay + " " + sensorLogItem2.MetricUnits;
                    }
                }
            }
        }

        public void PauseLogging() {
            IsLogging = false;
            btnStart.Text = Properties.Resources.btnStart_Name_Resume;
            scrollTime.Enabled = true;
            btnReset.Enabled = true;
            btnSave.Enabled = true;
        }

        private void SensorGridForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }
    }
}
