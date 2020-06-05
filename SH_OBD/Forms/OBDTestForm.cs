using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class OBDTestForm : Form {
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;
        private string[] m_fileNames;
        private string m_serialRecvBuf;
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
                if (GridViewInfo.Columns.Count > 0) {
                    GridViewInfo.Columns[0].Width = 30;
                    GridViewInfo.Columns[1].Width = 150;
                    SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
                }
                if (GridViewECUInfo.Columns.Count > 0) {
                    GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                    SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
                }
            });
        }

        void OnWriteDbStart() {
            m_ctsSetupColumnsDone.Cancel();
            m_ctsWriteDbStart = UpdateUITask("正在写入本地数据库");
            this.Invoke((EventHandler)delegate {
                this.GridViewInfo.Invalidate();
                this.GridViewECUInfo.Invalidate();
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
            });
        }

        void OnSetDataTableColumnsError(object sender, SetDataTableColumnsErrorEventArgs e) {
            this.Invoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Red;
                this.labelMESInfo.Text = e.ErrorMsg;
            });
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            // 以回车符作为输入结束标志，处理串口输入的VIN号，串口数据可能会有断包问题需要处理
            Control con = this.ActiveControl;
            if (con is TextBox tb) {
                m_serialRecvBuf += Encoding.Default.GetString(bits);
                if (m_serialRecvBuf.Contains("\n")) {
                    m_serialRecvBuf = m_serialRecvBuf.Trim().ToUpper();
                    if (m_serialRecvBuf.Length >= 17) {
                        this.Invoke((EventHandler)delegate {
                            m_obdTest.StrVIN_IN = m_serialRecvBuf.Substring(m_serialRecvBuf.Length - 17, 17);
                            this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
                            m_serialRecvBuf = "";
                        });
                        if (this.chkBoxManualUpload.Checked || this.chkBoxShowData.Checked) {
                            this.Invoke((EventHandler)delegate {
                                ManualUpload();
                            });
                        } else {
                            m_obdInterface.m_log.TraceInfo("Get scanned VIN: " + m_obdTest.StrVIN_IN + " by serial port scanner in advance mode");
                            this.Invoke((EventHandler)delegate {
                                this.btnStartOBDTest.PerformClick();
                            });
                        }
                    }
                }
            }

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
                m_obdTest.UploadDataFromDB(m_obdTest.StrVIN_IN, out string errorMsg, this.chkBoxShowData.Checked);
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

        private void ManualUpload() {
            this.GridViewInfo.DataSource = null;
            this.GridViewECUInfo.DataSource = null;
            Task.Factory.StartNew(StartManualUpload);
        }

        private void BtnStartOBDTest_Click(object sender, EventArgs e) {
            this.GridViewInfo.DataSource = null;
            this.GridViewECUInfo.DataSource = null;
            Task.Factory.StartNew(StartOBDTest);
        }

        private void StartOBDTest() {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            m_obdInterface.m_log.TraceInfo("Start OBD test in advance mode");
            this.Invoke((EventHandler)delegate {
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "准备OBD检测";
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "准备上传数据";
            });
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

            this.Invoke((EventHandler)delegate {
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
            });
            if (m_obdTest.CALIDCVNAllEmpty) {
                MessageBox.Show("CALID和CVN均为空！请检查OBD线缆接头连接是否牢固。", "OBD检测出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void TxtBoxVIN_KeyPress(object sender, KeyPressEventArgs e) {
            // 以回车符作为输入结束标志，处理USB扫码枪扫描的或者人工输入的VIN号
            if (e.KeyChar == (char)Keys.Enter) {
                string strTxt = this.txtBoxVIN.Text.Trim();
                if (strTxt.Length >= 17) {
                    m_obdTest.StrVIN_IN = strTxt.Substring(strTxt.Length - 17, 17);
                    this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
                    this.txtBoxVIN.SelectAll();
                    if (this.chkBoxManualUpload.Checked || this.chkBoxShowData.Checked) {
                        ManualUpload();
                    } else {
                        m_obdInterface.m_log.TraceInfo("Get scanned VIN: " + this.txtBoxVIN.Text + "in advance mode");
                        this.txtBoxVIN.ReadOnly = true;
                        this.btnStartOBDTest.PerformClick();
                    }
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e) {
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "Excel报表数据导入中。。。";
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "打开 Excel 报表文件",
                Filter = "Excel 2007 及以上 (*.xlsx)|*.xlsx",
                FilterIndex = 0,
                RestoreDirectory = true,
                Multiselect = true
            };
            try {
                openFileDialog.ShowDialog();
                if (openFileDialog.FileNames.Length <= 0) {
                    return;
                }
                m_fileNames = openFileDialog.FileNames;
                Task.Factory.StartNew(ImportExcel);
            } finally {
                openFileDialog.Dispose();
            }
        }

        private void ImportExcel() {
            DataTable dtImport = new DataTable("OBDData");
            dtImport.Columns.Add("VIN");
            dtImport.Columns.Add("ECU_ID");
            dtImport.Columns.Add("OBD_SUP");
            dtImport.Columns.Add("ODO");
            dtImport.Columns.Add("CAL_ID");
            dtImport.Columns.Add("CVN");
            dtImport.Columns.Add("Result");
            try {
                if (m_fileNames == null) {
                    this.Invoke((EventHandler)delegate {
                        this.labelMESInfo.ForeColor = Color.Red;
                        this.labelMESInfo.Text = "无Excel报表文件";
                    });
                    return;
                }
                foreach (string file in m_fileNames) {
                    FileInfo fileInfo = new FileInfo(file);
                    using (ExcelPackage package = new ExcelPackage(fileInfo, true)) {
                        ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                        DataRow dr = dtImport.NewRow();
                        dr["VIN"] = worksheet1.Cells["B2"].Value;
                        dr["ECU_ID"] = worksheet1.Cells["E3"].Value;
                        dr["OBD_SUP"] = worksheet1.Cells["B9"].Value;
                        dr["ODO"] = worksheet1.Cells["B10"].Value;
                        string CALID;
                        string CVN;
                        CALID = worksheet1.Cells["B3"].Value == null ? "" : worksheet1.Cells["B3"].Value.ToString();
                        CVN = worksheet1.Cells["D3"].Value == null ? "" : worksheet1.Cells["D3"].Value.ToString();
                        if (worksheet1.Cells["B4"].Value != null && worksheet1.Cells["B4"].Value.ToString().Length > 0) {
                            CALID += "," + worksheet1.Cells["B4"].Value.ToString();
                        }
                        if (worksheet1.Cells["D4"].Value != null && worksheet1.Cells["D4"].Value.ToString().Length > 0) {
                            CVN += "," + worksheet1.Cells["D4"].Value.ToString();
                        }
                        dr["CAL_ID"] = CALID;
                        dr["CVN"] = CVN;
                        if (worksheet1.Cells["B12"].Value != null && worksheet1.Cells["B12"].Value.ToString().Length > 0) {
                            dr["Result"] = worksheet1.Cells["B12"].Value.ToString().Contains("不合格") ? "0" : "1";
                        }
                        dtImport.Rows.Add(dr);

                        if (worksheet1.Cells["E5"].Value != null) {
                            dr = dtImport.NewRow();
                            dr["VIN"] = worksheet1.Cells["B2"].Value;
                            dr["ECU_ID"] = worksheet1.Cells["E5"].Value;
                            dr["OBD_SUP"] = worksheet1.Cells["B9"].Value;
                            dr["ODO"] = worksheet1.Cells["B10"].Value;
                            dr["CAL_ID"] = worksheet1.Cells["B5"].Value == null ? "" : worksheet1.Cells["B5"].Value.ToString();
                            dr["CVN"] = worksheet1.Cells["D5"].Value == null ? "" : worksheet1.Cells["D5"].Value.ToString();
                            if (worksheet1.Cells["B12"].Value != null && worksheet1.Cells["B12"].Value.ToString().Length > 0) {
                                dr["Result"] = worksheet1.Cells["B12"].Value.ToString().Contains("不合格") ? "0" : "1";
                            }
                            dtImport.Rows.Add(dr);
                        }
                        m_obdTest.m_db.ModifyDB(dtImport);
                    }
                }
                this.Invoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.ForestGreen;
                    this.labelMESInfo.Text = "Excel报表数据导入完成";
                });
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Import excel report file data error: " + ex.Message);
                this.Invoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "Excel报表数据导入失败";
                });
                MessageBox.Show(ex.Message, "导入数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                dtImport.Dispose();
            }
        }
    }
}
