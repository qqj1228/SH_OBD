using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class OBDTestForm : Form {
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;
        private DateTime m_lastTime;

        public OBDTestForm(OBDInterface obd, OBDTest obdTest) {
            InitializeComponent();
            m_obdInterface = obd;
            m_obdTest = obdTest;
            btnStartOBDTest.Enabled = false;
            m_lastTime = DateTime.Now;
        }

        void OnOBDTestStart() {
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "OBD检测中。。。";
            });
        }

        void OnSetupColumnsDone() {
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "正在显示结果。。。";
            });
            if (GridViewInfo.Columns.Count > 0) {
                GridViewInfo.Columns[0].Width = 30;
                GridViewInfo.Columns[1].Width = 150;
                SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (GridViewECUInfo.Columns.Count > 0) {
                GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            }
        }

        void OnWriteDbStart() {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "写入数据库中。。。";
                this.txtBoxVIN.ReadOnly = false;
            });
        }

        void OnWriteDbDone() {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据库写入完成";
            });
        }

        void OnUploadDataStart() {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据上传中。。。";
            });
        }

        void OnUploadDataDone() {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.ForestGreen;
                this.labelMESInfo.Text = "数据上传完成";
            });
        }

        void OnNotUploadData() {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Red;
                this.labelMESInfo.Text = "因OBD检测不合格，故数据不上传";
            });
        }

        void OnSetDataTableColumnsError(object sender, SetDataTableColumnsErrorEventArgs e) {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Red;
                this.labelMESInfo.Text = e.ErrorMsg;
            });
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            // 跨UI线程调用UI控件要使用Invoke
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.Text = Encoding.Default.GetString(bits).Trim().ToUpper();
                if (this.txtBoxVIN.Text.Length == 17 && !this.chkBoxManualUpload.Checked) {
                    m_obdInterface.m_log.TraceInfo("Get VIN: " + this.txtBoxVIN.Text + " by serial port scanner");
                    this.btnStartOBDTest.PerformClick();
                }
            });
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                btnStartOBDTest.Enabled = true;
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "准备OBD检测";
                this.txtBoxVIN.ReadOnly = false;
                this.txtBoxVIN.SelectAll();
                this.txtBoxVIN.Focus();
            } else {
                btnStartOBDTest.Enabled = false;
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "等待连接车辆OBD接口";
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "准备上传数据";
            }
        }

        private void SetGridViewColumnsSortMode(DataGridView gridView, DataGridViewColumnSortMode sortMode) {
            for (int i = 0; i < gridView.Columns.Count; i++) {
                gridView.Columns[i].SortMode = sortMode;
            }
        }

        private void OBDTestForm_Load(object sender, EventArgs e) {
            this.GridViewInfo.DataSource = m_obdTest.GetDataTable(0);
            this.GridViewECUInfo.DataSource = m_obdTest.GetDataTable(1);
            if (m_obdInterface.ScannerPortOpened) {
                m_obdInterface.m_sp.DataReceived += new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
            }
            m_obdTest.OBDTestStart += new Action(OnOBDTestStart);
            m_obdTest.SetupColumnsDone += new Action(OnSetupColumnsDone);
            m_obdTest.WriteDbStart += new Action(OnWriteDbStart);
            m_obdTest.WriteDbDone += new Action(OnWriteDbDone);
            m_obdTest.UploadDataStart += new Action(OnUploadDataStart);
            m_obdTest.UploadDataDone += new Action(OnUploadDataDone);
            m_obdTest.NotUploadData += new Action(OnNotUploadData);
            m_obdTest.SetDataTableColumnsError += OnSetDataTableColumnsError;
            if (this.GridViewInfo.Columns.Count > 1) {
                GridViewInfo.Columns[0].Width = 30;
                GridViewInfo.Columns[1].Width = 150;
                SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (this.GridViewECUInfo.Columns.Count > 0) {
                GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            }
            this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
        }

        private void OBDTestForm_Resize(object sender, EventArgs e) {
            int margin = groupInfo.Location.X;
            groupInfo.Width = (Width - margin * 3) / 2;
            groupInfo.Height = Height - margin * 3 - btnStartOBDTest.Location.Y - btnStartOBDTest.Height;
            groupECUInfo.Location = new Point(groupInfo.Width + margin * 2, groupInfo.Location.Y);
            groupECUInfo.Width = groupInfo.Width;
            groupECUInfo.Height = groupInfo.Height;
            labelMESInfo.Location = new Point(groupECUInfo.Location.X + groupECUInfo.Width / 3, labelInfo.Location.Y);
        }

        private void OBDTestForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }

        private void TxtBoxVIN_TextChanged(object sender, EventArgs e) {
            TimeSpan ts = DateTime.Now.Subtract(m_lastTime);
            int sec = (int)ts.TotalSeconds;
            if (this.chkBoxManualUpload.Checked) {
                if (this.txtBoxVIN.Text.Length == 17) {
                    ManualUpload();
                }
            } else {
                if (!m_obdInterface.CommSettings.UseSerialScanner && this.txtBoxVIN.Text.Length == 17 && sec > 1) {
                    m_lastTime = DateTime.Now;
                    this.txtBoxVIN.Text = this.txtBoxVIN.Text.Trim().ToUpper();
                    m_obdInterface.m_log.TraceInfo("Get VIN: " + this.txtBoxVIN.Text);
                    this.btnStartOBDTest.PerformClick();
                    this.txtBoxVIN.ReadOnly = true;
                }
            }
        }

        private void ManualUpload() {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            m_obdInterface.m_log.TraceInfo("Start ManualUpload");
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "手动读取数据";
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "准备手动上传数据";
            try {
                m_obdTest.UploadDataFromDB(this.txtBoxVIN.Text, out string errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "手动上传数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "结果数据显示完毕";
        }

        private void BtnStartOBDTest_Click(object sender, EventArgs e) {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            m_obdInterface.m_log.TraceInfo("Start OBD test in advance mode");
            m_obdTest.StrVIN_IN = this.txtBoxVIN.Text;
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "准备OBD检测";
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "准备上传数据";
            m_obdInterface.m_log.TraceInfo(">>>>>>>>>> Start to test vehicle of VIN: " + m_obdTest.StrVIN_IN + " <<<<<<<<<<");
            string errorMsg = "";
            try {
                m_obdTest.StartOBDTest(out errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("OBD test occurred error: " + errorMsg + ", " + ex.Message);
                MessageBox.Show(ex.Message, "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (m_obdTest.OBDResult) {
                this.labelInfo.ForeColor = Color.ForestGreen;
                this.labelInfo.Text = "OBD检测结束，结果：合格";
            } else {
                string strCat = "";
                if (!m_obdTest.DTCResult) {
                    strCat += "，存在故障码DTC";
                }
                if (!m_obdTest.ReadinessResult) {
                    strCat += "，就绪状态未完成项超过2项";
                }
                if (!m_obdTest.VINResult) {
                    strCat += "，VIN号不匹配";
                }
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "OBD检测结束，结果：不合格" + strCat;
            }
            this.txtBoxVIN.ReadOnly = false;
            if (m_obdTest.CALIDCVNAllEmpty) {
                MessageBox.Show("CALID和CVN均为空！请检查OBD线缆接头连接是否牢固。", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OBDTestForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (m_obdInterface.ScannerPortOpened) {
                m_obdInterface.m_sp.DataReceived -= new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
            }
            m_obdTest.AdvanceMode = false;
            m_obdTest.OBDTestStart -= new Action(OnOBDTestStart);
            m_obdTest.SetupColumnsDone -= new Action(OnSetupColumnsDone);
            m_obdTest.WriteDbStart -= new Action(OnWriteDbStart);
            m_obdTest.WriteDbDone -= new Action(OnWriteDbDone);
            m_obdTest.UploadDataStart -= new Action(OnUploadDataStart);
            m_obdTest.UploadDataDone -= new Action(OnUploadDataDone);
            m_obdTest.NotUploadData -= new Action(OnNotUploadData);
            m_obdTest.SetDataTableColumnsError -= OnSetDataTableColumnsError;
        }

        private void TxtBoxVIN_KeyPress(object sender, KeyPressEventArgs e) {
            e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }
    }

}
