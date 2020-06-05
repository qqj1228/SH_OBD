using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SH_OBD {
    public partial class OBDTestForm : Form {
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;
        CancellationTokenSource m_ctsOBDTestStart;
        CancellationTokenSource m_ctsSetupColumnsDone;
        CancellationTokenSource m_ctsWriteDbStart;
        CancellationTokenSource m_ctsUploadDataStart;

        public OBDTestForm(OBDInterface obd, OBDTest obdTest) {
            InitializeComponent();
            m_obdInterface = obd;
            m_obdTest = obdTest;
            btnStartOBDTest.Enabled = false;
        }

        void OnOBDTestStart() {
            m_ctsOBDTestStart = UpdateUITask("开始OBD检测");
        }

        void OnSetupColumnsDone() {
            if (m_ctsOBDTestStart != null) {
                m_ctsOBDTestStart.Cancel();
            }
            m_ctsSetupColumnsDone = UpdateUITask("正在读取车辆信息");
            this.Invoke((EventHandler)delegate {
                this.GridViewInfo.DataSource = m_obdTest.GetDataTable(0);
                this.GridViewECUInfo.DataSource = m_obdTest.GetDataTable(1);
                this.GridViewIUPR.DataSource = m_obdTest.GetDataTable(2);
                if (GridViewInfo.Columns.Count > 1) {
                    GridViewInfo.Columns[0].Width = 30;
                    GridViewInfo.Columns[1].Width = GridViewInfo.Columns[0].Width * 5;
                    SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
                }
                if (GridViewECUInfo.Columns.Count > 1) {
                    GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                    GridViewECUInfo.Columns[1].Width = GridViewInfo.Columns[1].Width;
                    SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
                }
                if (GridViewIUPR.Columns.Count > 1) {
                    GridViewIUPR.Columns[0].Width = GridViewInfo.Columns[0].Width;
                    GridViewIUPR.Columns[1].Width = GridViewInfo.Columns[0].Width * 8;
                    SetGridViewColumnsSortMode(this.GridViewIUPR, DataGridViewColumnSortMode.Programmatic);
                }
            });
        }

        void OnWriteDbStart() {
            m_ctsSetupColumnsDone.Cancel();
            m_ctsWriteDbStart = UpdateUITask("正在写入本地数据库");
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.ReadOnly = false;
                this.txtBoxVehicleType.ReadOnly = false;
                this.GridViewInfo.Invalidate();
                this.GridViewECUInfo.Invalidate();
                this.GridViewIUPR.Invalidate();
            });
        }

        void OnWriteDbDone() {
            m_ctsWriteDbStart.Cancel();
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据库写入完成";
            });
        }

        void OnUploadDataStart() {
            if (m_ctsSetupColumnsDone != null) {
                m_ctsSetupColumnsDone.Cancel();
            }
            m_ctsUploadDataStart = UpdateUITask("正在上传数据");
            this.Invoke((EventHandler)delegate {
                this.GridViewInfo.Invalidate();
                this.GridViewECUInfo.Invalidate();
                this.GridViewIUPR.Invalidate();
            });
        }

        void OnUploadDataDone() {
            m_ctsUploadDataStart.Cancel();
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.ForestGreen;
                this.labelMESInfo.Text = "上传数据结束";
            });
        }

        void OnNotUploadData() {
            if (m_ctsSetupColumnsDone != null) {
                m_ctsSetupColumnsDone.Cancel();
            }
            this.Invoke((EventHandler)delegate {
                if (!this.chkBoxShowData.Checked) {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "因OBD检测不合格，故数据不上传";
                }
                this.GridViewInfo.Invalidate();
                this.GridViewECUInfo.Invalidate();
                this.GridViewIUPR.Invalidate();
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
                this.txtBoxVIN.Text = Encoding.Default.GetString(bits).Trim();
                if (this.txtBoxVIN.Text.Length == 17 && !this.chkBoxManualUpload.Checked) {
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
                this.txtBoxVehicleType.ReadOnly = false;
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
            this.GridViewIUPR.DataSource = m_obdTest.GetDataTable(2);
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
                GridViewInfo.Columns[1].Width = GridViewInfo.Columns[0].Width * 5;
                SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (this.GridViewECUInfo.Columns.Count > 1) {
                GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                GridViewECUInfo.Columns[1].Width = GridViewInfo.Columns[1].Width;
                SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (this.GridViewIUPR.Columns.Count > 1) {
                GridViewIUPR.Columns[0].Width = GridViewInfo.Columns[0].Width;
                GridViewIUPR.Columns[1].Width = GridViewInfo.Columns[0].Width * 8;
                SetGridViewColumnsSortMode(this.GridViewIUPR, DataGridViewColumnSortMode.Programmatic);
            }
            this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
            this.txtBoxVehicleType.Text = m_obdTest.StrType_IN;
        }

        private void OBDTestForm_Resize(object sender, EventArgs e) {
            int margin = grpBoxInfo.Location.X;
            grpBoxInfo.Width = (this.ClientSize.Width - margin * 3) / 2;
            grpBoxInfo.Height = (this.ClientSize.Height - (btnStartOBDTest.Location.Y + btnStartOBDTest.Height) - margin * 3) * 2 / 3;
            grpBoxECUInfo.Location = new Point(grpBoxInfo.Location.X, grpBoxInfo.Location.Y + grpBoxInfo.Height + margin);
            grpBoxECUInfo.Width = grpBoxInfo.Width;
            grpBoxECUInfo.Height = grpBoxInfo.Height / 2;
            grpBoxIUPR.Location = new Point(grpBoxInfo.Location.X + grpBoxInfo.Width + margin, grpBoxInfo.Location.Y);
            grpBoxIUPR.Width = grpBoxInfo.Width;
            grpBoxIUPR.Height = (this.ClientSize.Height - (btnStartOBDTest.Location.Y + btnStartOBDTest.Height) - margin * 2);
            labelMESInfo.Location = new Point(grpBoxIUPR.Location.X + grpBoxIUPR.Width / 3, labelInfo.Location.Y);
        }

        private void OBDTestForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }

        private void TxtBox_TextChanged(object sender, EventArgs e) {
        }

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                TextBox tb = sender as TextBox;
                string[] codes = tb.Text.Split('*');
                if (codes != null) {
                    if (codes.Length > 2) {
                        m_obdTest.StrVIN_IN = codes[2];
                        m_obdTest.StrType_IN = codes[0];
                        this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
                        this.txtBoxVehicleType.Text = m_obdTest.StrType_IN;
                    } else {
                        if (tb.Name == "txtBoxVIN") {
                            m_obdTest.StrVIN_IN = codes[0];
                        } else if (tb.Name == "txtBoxVehicleType") {
                            m_obdTest.StrType_IN = codes[0];
                        }
                    }
                }
                if (this.chkBoxManualUpload.Checked || this.chkBoxShowData.Checked) {
                    if (this.txtBoxVIN.Text.Length == 17 && m_obdTest.StrType_IN.Length >= 10) {
                        ManualUpload();
                    }
                } else {
                    if (m_obdTest.StrVIN_IN.Length == 17 && m_obdTest.StrType_IN.Length >= 10) {
                        m_obdInterface.m_log.TraceInfo("Get VIN: " + this.txtBoxVIN.Text);
                        if (btnStartOBDTest.Enabled) {
                            this.txtBoxVIN.ReadOnly = true;
                            this.txtBoxVehicleType.ReadOnly = true;
                            this.btnStartOBDTest.PerformClick();
                        }
                    }
                }
            }
        }

        private void ManualUpload() {
            this.GridViewInfo.DataSource = null;
            this.GridViewECUInfo.DataSource = null;
            this.GridViewIUPR.DataSource = null;
            Task.Factory.StartNew(StartManualUpload);
        }

        private void StartManualUpload() {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            m_obdInterface.m_log.TraceInfo("Start ManualUpload");
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "手动读取数据";
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "准备手动上传数据";
            });
            try {
                m_obdTest.UploadDataFromDB(this.txtBoxVIN.Text, out string errorMsg, this.chkBoxShowData.Checked);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "手动上传数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "结果数据显示完毕";
            });
            if (m_ctsOBDTestStart != null) {
                m_ctsOBDTestStart.Cancel();
            }
            if (m_ctsSetupColumnsDone != null) {
                m_ctsSetupColumnsDone.Cancel();
            }
            if (m_ctsWriteDbStart != null) {
                m_ctsWriteDbStart.Cancel();
            }
            if (m_ctsUploadDataStart != null) {
                m_ctsUploadDataStart.Cancel();
            }
        }

        private void BtnStartOBDTest_Click(object sender, EventArgs e) {
            this.GridViewInfo.DataSource = null;
            this.GridViewECUInfo.DataSource = null;
            this.GridViewIUPR.DataSource = null;
            Task.Factory.StartNew(StartOBDTest);
        }

        private void StartOBDTest() {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "准备OBD检测";
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "准备上传数据";
            });
            string errorMsg = "";
            try {
                m_obdTest.StartOBDTest(out errorMsg);
#if DEBUG
                if (!m_obdInterface.OracleMESSetting.Enable) {
                    MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
                }
#endif
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("OBD test occurred error: " + errorMsg + ", " + ex.Message);
                MessageBox.Show(ex.Message, "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Invoke((EventHandler)delegate {
                if (m_obdTest.OBDResult) {
                    this.labelInfo.ForeColor = Color.ForestGreen;
                    this.labelInfo.Text = "OBD检测结束，结果：合格";
                } else {
                    string strCat = "";
                    if (!m_obdTest.DTCResult) {
                        strCat += "，存在DTC故障码";
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
                this.txtBoxVehicleType.ReadOnly = false;
            });
            if (m_ctsOBDTestStart != null) {
                m_ctsOBDTestStart.Cancel();
            }
            if (m_ctsSetupColumnsDone != null) {
                m_ctsSetupColumnsDone.Cancel();
            }
            if (m_ctsWriteDbStart != null) {
                m_ctsWriteDbStart.Cancel();
            }
            if (m_ctsUploadDataStart != null) {
                m_ctsUploadDataStart.Cancel();
            }
        }

        private CancellationTokenSource UpdateUITask(string strMsg) {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Task.Factory.StartNew(() => {
                int count = 0;
                while (!token.IsCancellationRequested) {
                    try {
                        this.Invoke((EventHandler)delegate {
                            this.labelInfo.ForeColor = Color.Black;
                            if (count == 0) {
                                this.labelInfo.Text = strMsg + "。。。";
                            } else {
                                this.labelInfo.Text = strMsg + "，用时" + count.ToString() + "s";
                            }
                        });
                    } catch (ObjectDisposedException ex) {
                        m_obdInterface.m_log.TraceWarning(ex.Message);
                    }
                    Thread.Sleep(1000);
                    ++count;
                }
            }, token);
            return tokenSource;
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
    }

    public class SAP_RETURN {
        public bool IsSuccess;
        public int Code;
        public string Message;
        public string MethodParameter;
    }
}
