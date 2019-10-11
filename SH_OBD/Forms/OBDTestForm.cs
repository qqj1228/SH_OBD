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
using Newtonsoft.Json;

namespace SH_OBD {
    public partial class OBDTestForm : Form {
        private readonly OBDInterface m_obdInterface;
        private readonly OBDTest m_obdTest;

        public OBDTestForm(OBDInterface obd, OBDTest obdTest) {
            InitializeComponent();
            m_obdInterface = obd;
            m_obdTest = obdTest;
            btnStartOBDTest.Enabled = false;
        }

        ~OBDTestForm() { }

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
            GridViewInfo.Columns[0].Width = 30;
            GridViewInfo.Columns[1].Width = 150;
            GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
            GridViewIUPR.Columns[0].Width = GridViewInfo.Columns[0].Width;
            GridViewIUPR.Columns[1].Width = 230;
            SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            SetGridViewColumnsSortMode(this.GridViewIUPR, DataGridViewColumnSortMode.Programmatic);
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

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            // 跨UI线程调用UI控件要使用Invoke
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.Text = Encoding.Default.GetString(bits).Trim();
                if (this.txtBoxVIN.Text.Length == 17) {
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
            if (this.GridViewInfo.Columns.Count > 1) {
                GridViewInfo.Columns[0].Width = 30;
                GridViewInfo.Columns[1].Width = 150;
                SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (this.GridViewECUInfo.Columns.Count > 0) {
                GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
                SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            }
            if (this.GridViewIUPR.Columns.Count > 1) {
                GridViewIUPR.Columns[0].Width = GridViewInfo.Columns[0].Width;
                GridViewIUPR.Columns[1].Width = 230;
                SetGridViewColumnsSortMode(this.GridViewIUPR, DataGridViewColumnSortMode.Programmatic);
            }
            this.txtBoxVIN.Text = m_obdTest.StrVIN_IN;
        }

        private void OBDTestForm_Resize(object sender, EventArgs e) {
            int margin = groupInfo.Location.X;
            groupInfo.Width = (Width - margin * 3) / 2;
            groupInfo.Height = (Height - margin * 3 - btnStartOBDTest.Location.Y - btnStartOBDTest.Height) * 2 / 3;
            groupECUInfo.Location = new Point(groupInfo.Location.X, groupInfo.Location.Y + groupInfo.Height + margin);
            groupECUInfo.Width = groupInfo.Width;
            groupECUInfo.Height = groupInfo.Height / 2;
            groupIUPR.Location = new Point(groupInfo.Width + margin * 2, groupInfo.Location.Y);
            groupIUPR.Width = groupInfo.Width;
            groupIUPR.Height = groupInfo.Height * 3 / 2 + margin;
            labelMESInfo.Location = new Point(groupIUPR.Location.X + groupIUPR.Width * 2 / 3, labelInfo.Location.Y);
        }

        private void OBDTestForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }

        private void TxtBoxVIN_TextChanged(object sender, EventArgs e) {
            if (!m_obdInterface.CommSettings.UseSerialScanner && this.txtBoxVIN.Text.Length == 17) {
                this.btnStartOBDTest.PerformClick();
                this.txtBoxVIN.ReadOnly = true;
            }
        }

        private void BtnStartOBDTest_Click(object sender, EventArgs e) {
            if (!m_obdTest.AdvanceMode) {
                return;
            }
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "准备OBD检测";
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "准备上传数据";

            try {
                m_obdTest.StartOBDTest(out string errorMsg);
#if DEBUG
                MessageBox.Show(errorMsg, WSHelper.GetMethodName(0));
#endif
            } catch (Exception ex) {
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
        }
    }

    public class JES_REVICE_EQUIP_DATE {
        public string DATA_ID;
        public string SOURCE;
        public string ZH_TESTDATE;
        public string ZH_ANALYMANUF;
        public string ZH_ANALYNAME;
        public string ZH_TETYPE;
        public string ZH_TEMODEL;
        public string ZH_TEMDATE;
        public string ZH_VIN;
        public string ZH_TESTNO;
        public string ZH_DYNOMANUF;
        public string ZH_DYNOMODEL;
        public string ZH_TESTTYPE;
        public string ZH_APASS;
        public string ZH_OPASS;
        public string ZH_EPASS;
        public string ZH_RESULT;
        public string ZH_JCJLNO;
        public string ZH_JCXTNO;
        public string ZH_JCKSSJ;
        public string ZH_JCRJ;
        public string ZH_DPCGJNO;
        public string ZH_CGJXT;
        public string ZH_PFCSSJ;
        public string ZH_JYLX;
        public string ZH_YRFF;
        public string CS_RH;
        public string CS_ET;
        public string CS_AP;
        public string CS_COND;
        public string CS_HCND;
        public string CS_NOXND;
        public string CS_CO2ND;
        public string CS_YND;
        public string SDS_REAC;
        public string SDS_LEAC;
        public string SDS_LRCO;
        public string SDS_LLCO;
        public string SDS_LRHC;
        public string SDS_LLHC;
        public string SDS_HRCO;
        public string SDS_HLCO;
        public string SDS_HRHC;
        public string SDS_HLHC;
        public string SDS_JYWD;
        public string SDS_FDJZS;
        public string SDS_SDSFJCSJ;
        public string SDS_SDSFGKSJ;
        public string SDS_SSZMHCND;
        public string SDS_SSZMCOND;
        public string SDS_SSZMCO2ND;
        public string SDS_SSZMO2ND;
        public string SDS_SSZMGDS;
        public string WT_ARHC5025;
        public string WT_ALHC5025;
        public string WT_ARCO5025;
        public string WT_ALCO5025;
        public string WT_ARNOX5025;
        public string WT_ALNOX5025;
        public string WT_ARHC2540;
        public string WT_ALHC2540;
        public string WT_ARCO2540;
        public string WT_ALCO2540;
        public string WT_ARNOX2540;
        public string WT_ALNOX2540;
        public string WT_ZJHC5025;
        public string WT_ZJCO5025;
        public string WT_ZJNO5025;
        public string WT_ZGL5025;
        public string WT_FDJZS5025;
        public string WT_CS5025;
        public string WT_ZJHC2540;
        public string WT_ZJCO2540;
        public string WT_ZJNO2540;
        public string WT_ZGL2540;
        public string WT_FDJZS2540;
        public string WT_CS2540;
        public string WT_WTJCSJ;
        public string WT_WTGKSJ;
        public string WT_WTZMCS;
        public string WT_WTZMFDJZS;
        public string WT_WTZMFZ;
        public string WT_WTZMHCND;
        public string WT_WTZMCOND;
        public string WT_WTZMNOND;
        public string WT_WTZMCO2ND;
        public string WT_WTZMO2ND;
        public string WT_WTZMZ;
        public string WT_WTNOSDXS;
        public string WT_WTZMXSDF;
        public string WT_WTZMHCNDXZ;
        public string WT_WTZMCONDXZ;
        public string WT_WTZMNONDXZ;
        public string JY_VRHC;
        public string JY_VLHC;
        public string JY_VRCO;
        public string JY_VLCO;
        public string JY_VRNOX;
        public string JY_VLNOX;
        public string JY_JYCSSJ;
        public string JY_JYGL;
        public string JY_JYXSJL;
        public string JY_JYHCPF;
        public string JY_JYCOPF;
        public string JY_JYNOXPF;
        public string JY_JYPLCS;
        public string JY_JYGK;
        public string JY_JYZMCS;
        public string JY_JYZMZS;
        public string JY_JYZMZH;
        public string JY_JYZMHCND;
        public string JY_JYZMHCNDXZ;
        public string JY_JYZMCOND;
        public string JY_JYZMCONDXZ;
        public string JY_JYZMNOXND;
        public string JY_JYZMNOXNDXZ;
        public string JY_JYZMCO2ND;
        public string JY_JYZMO2ND;
        public string JY_JYXSO2ND;
        public string JY_JYXSLL;
        public string JY_JYXSXS;
        public string JY_JYNOSDXZ;
        public string JY_JYZMZ;
        public string ZY_RATEREV;
        public string ZY_REV;
        public string ZY_SMOKEK1;
        public string ZY_SMOKEK2;
        public string ZY_SMOKEK3;
        public string ZY_SMOKEAVG;
        public string ZY_SMOKEKLIMIT;
        public string ZY_ZYGXSZ;
        public string ZY_ZYJCSSJ;
        public string ZY_ZYGKSJ;
        public string ZY_ZYZS;
        public string ZY_YDJZZC;
        public string ZY_YDJMC;
        public string ZY_ZYCCRQ;
        public string ZY_ZYJDRQ;
        public string ZY_ZYJCJL;
        public string ZY_ZYBDJL;
        public string JZ_RATEREVUP;
        public string JZ_RATEREVDOWN;
        public string JZ_REV100;
        public string JZ_MAXPOWER;
        public string JZ_MAXPOWERLIMIT;
        public string JZ_SMOKE100;
        public string JZ_SMOKE80;
        public string JZ_SMOKELIMIT;
        public string JZ_NOX;
        public string JZ_NOXLIMIT;
        public string JZ_JSGXS100;
        public string JZ_JSGXS80;
        public string JZ_JSLBGL;
        public string JZ_JSFDJZS;
        public string JZ_JSJCSJ;
        public string JZ_JSGKSJ;
        public string JZ_JSZMCS;
        public string JZ_JSZMZS;
        public string JZ_JSZMZH;
        public string JZ_JSZMNJ;
        public string JZ_JSZMGXS;
        public string JZ_JSZMCO2ND;
        public string JZ_JSZMNOND;
        public string OBD_OTESTDATE;
        public string OBD_OBD;
        public string OBD_ODO;
        public string OBD_MODULEID;
        public string OBD_ECALID;
        public string OBD_ECVN;
        public string OBD_ACALID;
        public string OBD_ACVN;
        public string OBD_OCALLID;
        public string OBD_OCVN;
        public string HANDLE_STATUS;
        public string HANDLE_MESSAGE;
        public string RECORD_TIME;
        public string IS_DEL;
        public string DEL_TIME;
        public string UPDATE_TIME;
    }

    public class SAP_RETURN {
        public bool IsSuccess;
        public int Code;
        public string Message;
        public string MethodParameter;
    }
}
