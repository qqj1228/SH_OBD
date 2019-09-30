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
        private const int maxPID = 0x100;
        private readonly OBDInterface m_obdInterface;
        private readonly DataTable m_dtInfo;
        private readonly DataTable m_dtECUInfo;
        private readonly DataTable m_dtIUPR;
        private readonly DataTable m_dtInstantData;
        private readonly Dictionary<string, bool[]> m_mode01Support;
        private readonly Dictionary<string, bool[]> m_mode09Support;
        private readonly System.Timers.Timer m_timer;
        private bool m_compIgn;
        private bool m_OBDResult;
        private bool m_DTCResult;
        private bool m_ReadinessResult;
        private bool m_VINResult;
        private readonly Model m_db;
        private SerialPortClass m_sp;
        private bool m_bPortOpened;

        public OBDTestForm(OBDInterface obd) {
            InitializeComponent();
            m_obdInterface = obd;
            m_dtInfo = new DataTable();
            m_dtECUInfo = new DataTable();
            m_dtIUPR = new DataTable();
            m_dtInstantData = new DataTable();
            btnStartOBDTest.Enabled = false;
            m_mode01Support = new Dictionary<string, bool[]>();
            m_mode09Support = new Dictionary<string, bool[]>();
            m_timer = new System.Timers.Timer(1000);
            m_timer.Elapsed += OnTimerElapsed;
            m_timer.AutoReset = true;
            m_compIgn = false;
            m_OBDResult = true;
            m_DTCResult = true;
            m_ReadinessResult = true;
            m_VINResult = true;
            m_db = new Model(m_obdInterface.DBandMES, m_obdInterface.m_log);
            m_bPortOpened = false;
            if (m_obdInterface.CommSettings.UseSerialScanner) {
                m_sp = new SerialPortClass(
                    m_obdInterface.CommSettings.ScannerPortName,
                    m_obdInterface.CommSettings.ScannerBaudRate,
                    Parity.None,
                    8,
                    StopBits.One
                );
                m_sp.DataReceived += new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
                try {
                    m_sp.OpenPort();
                    m_bPortOpened = true;
                } catch (Exception ex) {
                    m_obdInterface.m_log.TraceError("打开扫码枪串口出错: " + ex.Message);
                    MessageBox.Show(ex.Message, "打开扫码枪串口出错！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        ~OBDTestForm() {
            m_dtInfo.Dispose();
            m_dtECUInfo.Dispose();
            m_dtIUPR.Dispose();
            m_dtInstantData.Dispose();
            m_timer.Enabled = false;
            m_timer.Dispose();
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            // 跨UI线程调用UI控件要使用Invoke
            this.Invoke((EventHandler)delegate {
                this.txtBoxVIN.Text = Encoding.Default.GetString(bits);
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
                if (m_sp != null && !m_bPortOpened) {
                    m_sp.PortName = m_obdInterface.CommSettings.ScannerPortName;
                    m_sp.BaudRate = m_obdInterface.CommSettings.ScannerBaudRate;
                    try {
                        m_sp.OpenPort();
                        m_bPortOpened = true;
                    } catch (Exception ex) {
                        m_obdInterface.m_log.TraceError("打开扫码枪串口出错: " + ex.Message);
                        MessageBox.Show(ex.Message, "打开扫码枪串口出错！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } else {
                btnStartOBDTest.Enabled = false;
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "等待连接车辆OBD接口";
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "准备上传MES";
            }
        }

        public void SetDataTableColumns<T>(DataTable dt, Dictionary<string, bool[]> ECUSupports) {
            dt.Clear();
            dt.Columns.Clear();
            dt.Columns.Add(new DataColumn("NO", typeof(int)));
            dt.Columns.Add(new DataColumn("Item", typeof(string)));
            foreach (string key in ECUSupports.Keys) {
                dt.Columns.Add(new DataColumn(key, typeof(T)));
            }
        }

        private void SetDataRow(int lineNO, string strItem, DataTable dt, OBDParameter param) {
            Dictionary<string, bool[]> support = new Dictionary<string, bool[]>();
            if (param.Service == 1 || ((param.Parameter >> 8) & 0x00FF) == 0xF4) {
                support = m_mode01Support;
            } else if (param.Service == 9 || ((param.Parameter >> 8) & 0x00FF) == 0xF8) {
                support = m_mode09Support;
            }
            DataRow dr = dt.NewRow();
            dr[0] = lineNO;
            dr[1] = strItem;

            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.ListString) != 0) {
                int maxLine = 0;
                foreach (OBDParameterValue value in valueList) {
                    if (value.ErrorDetected) {
                        for (int i = 2; i < dt.Columns.Count; i++) {
                            dr[i] = "";
                        }
                        break;
                    }
                    if (value.ListStringValue.Count > maxLine) {
                        maxLine = value.ListStringValue.Count;
                    }
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                            if (value.ListStringValue.Count == 0 || value.ListStringValue[0] == "") {
                                dr[i] = "--";
                            } else {
                                dr[i] = value.ListStringValue[0];
                                for (int j = 1; j < value.ListStringValue.Count; j++) {
                                    dr[i] += "\n" + value.ListStringValue[j];
                                }
                            }
                        }
                    }
                }
                if (param.Service == 1 || param.Service == 9 || param.Service == 0x22) {
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (support.ContainsKey(dt.Columns[i].ColumnName) && !support[dt.Columns[i].ColumnName][(param.Parameter & 0x00FF) - 1]) {
                            dr[i] = "不适用";
                        }
                    }
                }
                dt.Rows.Add(dr);
            } else {
                foreach (OBDParameterValue value in valueList) {
                    if (value.ErrorDetected) {
                        for (int i = 2; i < dt.Columns.Count; i++) {
                            dr[i] = "";
                        }
                        break;
                    }
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.Bool) != 0) {
                                if (value.BoolValue) {
                                    dr[i] = "ON";
                                } else {
                                    dr[i] = "OFF";
                                }
                            } else if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.Double) != 0) {
                                dr[i] = value.DoubleValue.ToString();
                            } else if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.String) != 0) {
                                dr[i] = value.StringValue;
                            } else if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.ShortString) != 0) {
                                dr[i] = value.ShortStringValue;
                            }
                        }
                    }
                }
                if (param.Service == 1 || param.Service == 9 || param.Service == 0x22) {
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (support.ContainsKey(dt.Columns[i].ColumnName) && !support[dt.Columns[i].ColumnName][(param.Parameter & 0x00FF) - 1]) {
                            dr[i] = "不适用";
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
        }

        public void SetReadinessDataRow(int lineNO, string strItem, DataTable dt, List<OBDParameterValue> valueList, int bitIndex, int bitOffset, ref int errorCount) {
            DataRow dr = dt.NewRow();
            dr[0] = lineNO;
            dr[1] = strItem;
            foreach (OBDParameterValue value in valueList) {
                if (value.ErrorDetected) {
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        dr[i] = "";
                    }
                    break;
                }
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                        if (value.GetBitFlag(bitIndex)) {
                            if (value.GetBitFlag(bitIndex + bitOffset)) {
                                dr[i] = "未完成";
                            } else {
                                dr[i] = "完成";
                            }
                        } else {
                            dr[i] = "不适用";
                        }
                    }
                }
            }
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && !m_mode01Support[dt.Columns[i].ColumnName][0]) {
                    dr[i] = "不适用";
                }
            }
            dt.Rows.Add(dr);
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[lineNO - 1][i].ToString() == "未完成") {
                    ++errorCount;
                }
            }
        }

        public void SetDataTableInfo() {
            DataTable dt = m_dtInfo;
            int NO = 0;
            OBDParameter param;
            int HByte = 0;
            if (m_obdInterface.UseISO27145) {
                param = new OBDParameter {
                    OBDRequest = "22F401",
                    Service = 0x22,
                    Parameter = 0xF401,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.Bool
                };
                HByte = 0xF400;
            } else {
                param = new OBDParameter {
                    OBDRequest = "0101",
                    Service = 1,
                    Parameter = 1,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.Bool
                };
            }
            SetDataRow(++NO, "MIL状态", dt, param);                                          // 0
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[dt.Rows.Count - 1][i].ToString() == "ON") {
                    m_OBDResult = false;
                }
            }

            param.Parameter = HByte + 0x21;
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            SetDataRow(++NO, "MIL亮后行驶里程（km）", dt, param);                              // 1  

            param.Parameter = HByte + 0x1C;
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ShortString;
            SetDataRow(++NO, "OBD型式检验类型", dt, param);                                    // 2

            param.Parameter = HByte + 0xA6;
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            SetDataRow(++NO, "总累积里程ODO（km）", dt, param);                                // 3

            if (m_obdInterface.UseISO27145) {
                param.OBDRequest = "194233081E";
            } else {
                param.OBDRequest = "03";
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "存储DTC", dt, param);                                           // 4
            for (int i = 2; i < dt.Columns.Count; i++) {
                string DTC = dt.Rows[dt.Rows.Count - 1][i].ToString();
                if (DTC != "--" && DTC != "不适用" && DTC != "") {
                    m_DTCResult = false;
                }
            }

            if (m_obdInterface.UseISO27145) {
                param.OBDRequest = "194233041E";
            } else {
                param.OBDRequest = "07";
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "未决DTC", dt, param);                                           // 5
            for (int i = 2; i < dt.Columns.Count; i++) {
                string DTC = dt.Rows[dt.Rows.Count - 1][i].ToString();
                if (DTC != "--" && DTC != "不适用" && DTC != "") {
                    m_DTCResult = false;
                }
            }

            if (m_obdInterface.UseISO27145) {
                param.OBDRequest = "195533";
            } else {
                param.OBDRequest = "0A";
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "永久DTC", dt, param);                                           // 6
            for (int i = 2; i < dt.Columns.Count; i++) {
                string DTC = dt.Rows[dt.Rows.Count - 1][i].ToString();
                if (DTC != "--" && DTC != "不适用" && DTC != "") {
                    m_DTCResult = false;
                }
            }

            int errorCount = 0;
            if (m_obdInterface.UseISO27145) {
                param.OBDRequest = "22F401";
            } else {
                param.OBDRequest = "0101";
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.BitFlags;
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            SetReadinessDataRow(++NO, "失火监测", dt, valueList, 15, -4, ref errorCount);      // 7
            SetReadinessDataRow(++NO, "燃油系统监测", dt, valueList, 14, -4, ref errorCount);  // 8
            SetReadinessDataRow(++NO, "综合组件监测", dt, valueList, 13, -4, ref errorCount);  // 9

            foreach (OBDParameterValue value in valueList) {
                if (m_mode01Support.ContainsKey(value.ECUResponseID) && m_mode01Support[value.ECUResponseID][(param.Parameter & 0x00FF) - 1]) {
                    m_compIgn = value.GetBitFlag(12);
                    break;
                }
            }
            if (m_compIgn) {
                // 压缩点火
                SetReadinessDataRow(++NO, "NMHC催化剂监测", dt, valueList, 23, 8, ref errorCount);     // 10
                SetReadinessDataRow(++NO, "NOx/SCR后处理监测", dt, valueList, 22, 8, ref errorCount);  // 11
                SetReadinessDataRow(++NO, "增压系统监测", dt, valueList, 20, 8, ref errorCount);       // 12
                SetReadinessDataRow(++NO, "废气传感器监测", dt, valueList, 18, 8, ref errorCount);     // 13
                SetReadinessDataRow(++NO, "PM过滤器监测", dt, valueList, 17, 8, ref errorCount);       // 14
            } else {
                // 火花点火
                SetReadinessDataRow(++NO, "催化剂监测", dt, valueList, 23, 8, ref errorCount);         // 10
                SetReadinessDataRow(++NO, "加热催化剂监测", dt, valueList, 22, 8, ref errorCount);     // 11
                SetReadinessDataRow(++NO, "燃油蒸发系统监测", dt, valueList, 21, 8, ref errorCount);   // 12
                SetReadinessDataRow(++NO, "二次空气系统监测", dt, valueList, 20, 8, ref errorCount);   // 13
                SetReadinessDataRow(++NO, "空调系统制冷剂监测", dt, valueList, 19, 8, ref errorCount); // 14
                SetReadinessDataRow(++NO, "氧气传感器监测", dt, valueList, 18, 8, ref errorCount);     // 15
                SetReadinessDataRow(++NO, "加热氧气传感器监测", dt, valueList, 17, 8, ref errorCount); // 16
            }
            SetReadinessDataRow(++NO, "EGR/VVT系统监测", dt, valueList, 16, 8, ref errorCount);
            if (errorCount > 2) {
                m_ReadinessResult = false;
            }
        }

        public void SetDataTableECUInfo() {
            DataTable dt = m_dtECUInfo;
            int NO = 0;
            OBDParameter param;
            int HByte = 0;
            if (m_obdInterface.UseISO27145) {
                param = new OBDParameter {
                    OBDRequest = "22F802",
                    Service = 0x22,
                    Parameter = 0xF802,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
                HByte = 0xF800;
            } else {
                param = new OBDParameter {
                    OBDRequest = "0902",
                    Service = 9,
                    Parameter = 2,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
            }
            SetDataRow(++NO, "VIN", dt, param);     // 0
            string strVIN = "";
            for (int i = 2; i < dt.Columns.Count; i++) {
                strVIN = dt.Rows[0][i].ToString();
                if (strVIN != "" || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            if (strVIN != this.txtBoxVIN.Text && this.txtBoxVIN.Text != "") {
                m_VINResult = false;
            }
            param.Parameter = HByte + 0x0A;
            SetDataRow(++NO, "ECU名称", dt, param); // 1
            param.Parameter = HByte + 4;
            SetDataRow(++NO, "CAL_ID", dt, param);  // 2
            param.Parameter = HByte + 6;
            SetDataRow(++NO, "CVN", dt, param);     // 3
        }

        private void SetIUPRDataRow(int lineNO, string strItem, int padTotal, int padNum, DataTable dt, List<OBDParameterValue> valueList, int itemIndex, int InfoType) {
            double num = 0;
            double den = 0;
            DataRow dr = dt.NewRow();
            dr[0] = lineNO;
            foreach (OBDParameterValue value in valueList) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                        if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
                            if (dr[1].ToString() == "") {
                                dr[1] = strItem + ": " + "监测完成次数".PadLeft(padTotal - padNum + 6);
                            }
                            if (value.ListStringValue.Count > itemIndex) {
                                num = Utility.Hex2Int(value.ListStringValue[itemIndex]);
                                dr[i] = num.ToString();
                            } else {
                                num = 0;
                                dr[i] = "0";
                            }
                        }
                    }
                }
            }
            if (dr[1].ToString() != "") {
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            foreach (OBDParameterValue value in valueList) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                        if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
                            if (dr[1].ToString() == "") {
                                dr[1] = "符合监测条件次数".PadLeft(padTotal + 8);
                            }
                            if (value.ListStringValue.Count > itemIndex) {
                                den = Utility.Hex2Int(value.ListStringValue[itemIndex + 1]);
                                dr[i] = den.ToString();
                            } else {
                                den = 0;
                                dr[i] = "0";
                            }
                        }
                    }
                }
            }
            if (dr[1].ToString() != "") {
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            foreach (OBDParameterValue value in valueList) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                        if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
                            if (dr[1].ToString() == "") {
                                dr[1] = "IUPR率".PadLeft(padTotal + 5);
                            }
                            if (den == 0) {
                                dr[i] = "7.99527";
                            } else {
                                double r = Math.Round(num / den, 6);
                                if (r > 7.99527) {
                                    dr[i] = "7.99527";
                                } else {
                                    dr[i] = r.ToString();
                                }
                            }
                        }
                    }
                }
            }
            if (dr[1].ToString() != "") {
                dt.Rows.Add(dr);
            }
        }

        public void SetDataTableIUPR() {
            DataTable dt = m_dtIUPR;
            int NO = 0;
            OBDParameter param;
            int HByte = 0;
            if (m_obdInterface.UseISO27145) {
                param = new OBDParameter {
                    OBDRequest = "22F80B",
                    Service = 0x22,
                    Parameter = 0xF80B,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
                HByte = 0xF800;
            } else {
                param = new OBDParameter {
                    OBDRequest = "090B",
                    Service = 9,
                    Parameter = 0x0B,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
            }
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            SetIUPRDataRow(++NO, "NMHC催化器", 18, 12, dt, valueList, 2, param.Parameter);
            SetIUPRDataRow(++NO, "NOx催化器", 18, 11, dt, valueList, 4, param.Parameter);
            SetIUPRDataRow(++NO, "NOx吸附器", 18, 11, dt, valueList, 6, param.Parameter);
            SetIUPRDataRow(++NO, "PM捕集器", 18, 10, dt, valueList, 8, param.Parameter);
            SetIUPRDataRow(++NO, "废气传感器", 18, 12, dt, valueList, 10, param.Parameter);
            SetIUPRDataRow(++NO, "EGR和VVT", 18, 10, dt, valueList, 12, param.Parameter);
            SetIUPRDataRow(++NO, "增压压力", 18, 10, dt, valueList, 14, param.Parameter);

            NO = 0;
            param.Parameter = HByte + 8;
            valueList = m_obdInterface.GetValueList(param);
            SetIUPRDataRow(++NO, "催化器 组1", 18, 12, dt, valueList, 2, param.Parameter);
            SetIUPRDataRow(++NO, "催化器 组2", 18, 12, dt, valueList, 4, param.Parameter);
            SetIUPRDataRow(++NO, "前氧传感器 组1", 18, 16, dt, valueList, 6, param.Parameter);
            SetIUPRDataRow(++NO, "前氧传感器 组2", 18, 16, dt, valueList, 8, param.Parameter);
            SetIUPRDataRow(++NO, "后氧传感器 组1", 18, 16, dt, valueList, 16, param.Parameter);
            SetIUPRDataRow(++NO, "后氧传感器 组2", 18, 16, dt, valueList, 18, param.Parameter);
            SetIUPRDataRow(++NO, "EVAP", 18, 6, dt, valueList, 14, param.Parameter);
            SetIUPRDataRow(++NO, "EGR和VVT", 18, 10, dt, valueList, 10, param.Parameter);
            SetIUPRDataRow(++NO, "GPF 组1", 18, 9, dt, valueList, 24, param.Parameter);
            SetIUPRDataRow(++NO, "GPF 组2", 18, 9, dt, valueList, 26, param.Parameter);
            SetIUPRDataRow(++NO, "二次空气喷射系统", 18, 18, dt, valueList, 12, param.Parameter);
        }

        #region 读取实时数据，OBD检测不需要
        private void SetInstantDataRow(int lineNO, DataTable dt, OBDParameter param) {
            Dictionary<string, bool[]> support = new Dictionary<string, bool[]>();
            if (param.Service == 1) {
                support = m_mode01Support;
            } else if (param.Service == 9) {
                support = m_mode09Support;
            }
            DataRow dr = dt.Rows[lineNO - 1];
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            foreach (OBDParameterValue value in valueList) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                        dr[i] = value.DoubleValue.ToString();
                    }
                }
            }
            if (param.Service == 1 || param.Service == 9) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (support.ContainsKey(dt.Columns[i].ColumnName) && !support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                        dr[i] = "不适用";
                    }
                }
            }
        }

        private string GetOSLocStr(int index, bool PID13or1D) {
            string[] strRet = new string[8];
            if (PID13or1D) {
                strRet[0] = "组1 传感器1";
                strRet[1] = "组1 传感器2";
                strRet[2] = "组1 传感器3";
                strRet[3] = "组1 传感器4";
                strRet[4] = "组2 传感器1";
                strRet[5] = "组2 传感器2";
                strRet[6] = "组2 传感器3";
                strRet[7] = "组2 传感器4";
            } else {
                strRet[0] = "组1 传感器1";
                strRet[1] = "组1 传感器2";
                strRet[2] = "组2 传感器1";
                strRet[3] = "组2 传感器2";
                strRet[4] = "组3 传感器1";
                strRet[5] = "组3 传感器2";
                strRet[6] = "组4 传感器1";
                strRet[7] = "组4 传感器2";
            }
            if (index < strRet.Length) {
                return strRet[index];
            } else {
                return "";
            }
        }

        private string GetEGTStr(int index, bool PID78or79) {
            string[] strRet = new string[4];
            if (PID78or79) {
                strRet[0] = "组1 传感器1";
                strRet[1] = "组1 传感器2";
                strRet[2] = "组1 传感器3";
                strRet[3] = "组1 传感器4";
            } else {
                strRet[0] = "组2 传感器1";
                strRet[1] = "组2 传感器2";
                strRet[2] = "组2 传感器3";
                strRet[3] = "组2 传感器4";
            }
            if (index < strRet.Length) {
                return strRet[index];
            } else {
                return "";
            }
        }

        private void SetDoubleList(OBDParameter param, List<double> doubleList, DataTable dt) {
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                    foreach (OBDParameterValue value in valueList) {
                        if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                            doubleList[i - 2] = value.DoubleValue;
                        }
                    }
                } else {
                    doubleList[i - 2] = -9999;
                }
            }
        }

        public void SetDataTableInstantData(bool bFirst) {
            DataTable dt = m_dtInstantData;
            int NO = 0;
            List<bool> PID13or1DList = new List<bool>(); // true - PID13, false - PID1D
            List<bool[]> OSLocList = new List<bool[]>();
            for (int i = 0; i < dt.Columns.Count - 2; i++) {
                PID13or1DList.Add(false);
                OSLocList.Add(new bool[8]);
            }

            foreach (string key in m_mode01Support.Keys) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (dt.Columns[i].ColumnName == key) {
                        PID13or1DList[i - 2] = m_mode01Support[key][0x13 - 1];
                    }
                }
            }

            OBDParameter param = new OBDParameter {
                OBDRequest = "0113",
                Service = 1,
                Parameter = 0x13,
                ValueTypes = (int)OBDParameter.EnumValueTypes.BitFlags
            };
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (PID13or1DList[i - 2]) {
                    List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
                    foreach (OBDParameterValue value in valueList) {
                        if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                            bool[] OSLoc = new bool[8];
                            for (int j = 0; j < OSLoc.Length; j++) {
                                OSLoc[j] = value.GetBitFlag(j);
                            }
                            OSLocList[i - 2] = OSLoc;
                        }
                    }
                } else {
                    param.Parameter = 0x1D;
                    List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
                    foreach (OBDParameterValue value in valueList) {
                        if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                            bool[] OSLoc = new bool[8];
                            for (int j = 0; j < OSLoc.Length; j++) {
                                OSLoc[j] = value.GetBitFlag(j);
                            }
                            OSLocList[i - 2] = OSLoc;
                        }
                    }
                }
            }

            if (bFirst) {
                param.Parameter = 0x0D;
                param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
                SetDataRow(++NO, "车速（km/h）", dt, param);

                param.Parameter = 0x0C;
                SetDataRow(++NO, "发动机转速（r/min）", dt, param);

                if (m_compIgn) {
                    List<double> RPMList = new List<double>();
                    List<double> TorPerList = new List<double>();
                    List<double> TorRefList = new List<double>();
                    for (int i = 0; i < dt.Columns.Count - 2; i++) {
                        RPMList.Add(0);
                        TorPerList.Add(0);
                        TorRefList.Add(0);
                    }
                    param.Parameter = 0x0C;
                    SetDoubleList(param, RPMList, dt);
                    param.Parameter = 0x62;
                    SetDoubleList(param, TorPerList, dt);
                    param.Parameter = 0x63;
                    SetDoubleList(param, TorRefList, dt);
                    DataRow dr = dt.NewRow();
                    dr[0] = ++NO;
                    dr[1] = "发动机输出功率（kW）";
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (RPMList[i - 2] == -9999 || TorPerList[i - 2] == -9999 || TorRefList[i - 2] == -9999) {
                            dr[i] = "不适用";
                        } else {
                            dr[i] = Math.Round(TorPerList[i - 2] * TorRefList[i - 2] * RPMList[i - 2] / 9550, 2).ToString();
                        }
                    }
                    dt.Rows.Add(dr);

                    param.Parameter = 0x5A;
                    SetDataRow(++NO, "油门开度（%）", dt, param);

                    param.Parameter = 0x10;
                    SetDataRow(++NO, "进气量（g/s）", dt, param);

                    param.Parameter = 0x70;
                    SetDataRow(++NO, "增压压力（kPa）", dt, param);

                    List<double> VSList = new List<double>();
                    List<double> FRList = new List<double>();
                    for (int i = 0; i < dt.Columns.Count - 2; i++) {
                        VSList.Add(0);
                        FRList.Add(0);
                    }
                    param.Parameter = 0x0D;
                    SetDoubleList(param, VSList, dt);
                    param.Parameter = 0x5E;
                    SetDoubleList(param, FRList, dt);
                    dr = dt.NewRow();
                    dr[0] = ++NO;
                    dr[1] = "耗油量（L/100km）";
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (VSList[i - 2] == -9999 || FRList[i - 2] == -9999) {
                            dr[i] = "不适用";
                        } else {
                            dr[i] = (VSList[i - 2] * FRList[i - 2] * 100).ToString();
                        }
                    }
                    dt.Rows.Add(dr);

                    param.Parameter = 0x83;
                    SetDataRow(++NO, "氮氧传感器浓度（ppm）", dt, param);

                    dr = dt.NewRow();
                    dr[0] = ++NO;
                    dr[1] = "尿素喷射量（L/h）";
                    param.Parameter = 0x85;
                    param.SubParameter = 4; // 取平均反应物消耗量，即柴油车即尿素
                    param.ValueTypes = (int)OBDParameter.EnumValueTypes.BitFlags + (int)OBDParameter.EnumValueTypes.Double;
                    List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                            foreach (OBDParameterValue value in valueList) {
                                if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                                    if (value.GetBitFlag(0)) {
                                        dr[i] = value.DoubleValue.ToString();
                                    } else {
                                        dr[i] = "不适用";
                                    }
                                }
                            }
                        } else {
                            dr[i] = "不适用";
                        }
                    }

                    param.Parameter = 0x78;
                    param.SubParameter = 0;
                    param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double + (int)OBDParameter.EnumValueTypes.BitFlags;
                    valueList = m_obdInterface.GetValueList(param);
                    List<bool[]> EGT78s = new List<bool[]>();
                    for (int i = 0; i < dt.Columns.Count - 2; i++) {
                        EGT78s.Add(new bool[4]);
                    }
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        foreach (OBDParameterValue value in valueList) {
                            if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                                bool[] EGT78 = new bool[4];
                                for (int j = 0; j < EGT78.Length; j++) {
                                    EGT78[j] = value.GetBitFlag(j);
                                }
                                EGT78s[i - 2] = EGT78;
                            }
                        }
                    }
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                            for (int j = 0; j < EGT78s[i - 2].Length; j++) {
                                if (EGT78s[i - 2][j]) {
                                    dr = dt.NewRow();
                                    dr[0] = ++NO;
                                    dr[1] = "排气温度 " + GetEGTStr(j, true) + "(℃)";
                                    param.SubParameter = 4 + j;
                                    param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
                                    valueList = m_obdInterface.GetValueList(param);
                                    foreach (OBDParameterValue value in valueList) {
                                        for (int m = 2; m < dt.Columns.Count; m++) {
                                            if (dt.Columns[m].ColumnName == value.ECUResponseID) {
                                                dr[m] = (value.DoubleValue * 1000).ToString();
                                            }
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }



                    param.Parameter = 0x7A;
                    param.SubParameter = 0;
                    param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
                    SetDataRow(++NO, "颗粒捕集器压差（kPa）", dt, param);

                    dr = dt.NewRow();
                    dr[0] = ++NO;
                    dr[1] = "燃油喷射压力（bar）";
                    param.Parameter = 0x23;
                    valueList = m_obdInterface.GetValueList(param);
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                            foreach (OBDParameterValue value in valueList) {
                                if (dt.Columns[i].ColumnName == value.ECUResponseID) {
                                    dr[i] = (value.DoubleValue / 100).ToString();
                                }
                            }
                        } else {
                            dr[i] = "不适用";
                        }
                    }
                } else {
                    param.Parameter = 0x11;
                    SetDataRow(++NO, "节气门绝对开度（%）", dt, param);

                    param.Parameter = 0x04;
                    SetDataRow(++NO, "计算负荷值（%）", dt, param);

                    for (int i = 2; i < dt.Columns.Count; i++) {
                        for (int j = 0; j < OSLocList[i - 2].Length; j++) {
                            if (OSLocList[i - 2][j]) {
                                param.Parameter = 0x14 + j;
                                param.SubParameter = 0; // 取电压
                                DataRow dr = dt.NewRow();
                                dr[0] = ++NO;
                                dr[1] = "前氧传感器信号 " + GetOSLocStr(j, PID13or1DList[i - 2]) + "（mV）";
                                List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
                                foreach (OBDParameterValue value in valueList) {
                                    for (int m = 2; m < dt.Columns.Count; m++) {
                                        if (dt.Columns[m].ColumnName == value.ECUResponseID) {
                                            dr[m] = (value.DoubleValue * 1000).ToString();
                                        }
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }

                    param.Parameter = 0x44;
                    SetDataRow(++NO, "过量空气系数（入）", dt, param);

                    param.Parameter = 0x10;
                    SetDataRow(++NO, "进气量（g/s）", dt, param);

                    param.Parameter = 0x0B;
                    SetDataRow(++NO, "进气压力（kPa）", dt, param);
                }
            } else {
                param.Parameter = 0x0D;
                param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
                SetInstantDataRow(++NO, dt, param);

                param.Parameter = 0x0C;
                SetInstantDataRow(++NO, dt, param);

                if (m_compIgn) {

                } else {
                    param.Parameter = 0x11;
                    SetInstantDataRow(++NO, dt, param);

                    param.Parameter = 0x04;
                    SetInstantDataRow(++NO, dt, param);

                    for (int i = 2; i < dt.Columns.Count; i++) {
                        for (int j = 0; j < OSLocList[i - 2].Length; j++) {
                            if (OSLocList[i - 2][j]) {
                                param.Parameter = 0x14 + j;
                                param.SubParameter = 0; // 取电压
                                DataRow dr = dt.Rows[++NO - 1];
                                List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
                                foreach (OBDParameterValue value in valueList) {
                                    for (int m = 2; m < dt.Columns.Count; m++) {
                                        if (dt.Columns[m].ColumnName == value.ECUResponseID) {
                                            dr[m] = (value.DoubleValue * 1000).ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    param.Parameter = 0x44;
                    SetInstantDataRow(++NO, dt, param);

                    param.Parameter = 0x10;
                    SetInstantDataRow(++NO, dt, param);

                    param.Parameter = 0x0B;
                    SetInstantDataRow(++NO, dt, param);
                }
            }

        }
        #endregion

        void OnTimerElapsed(object sender, EventArgs e) {
            SetDataTableInstantData(false);
        }

        private bool GetSupportStatus(int mode, Dictionary<string, bool[]> supportStatus) {
            List<List<OBDParameterValue>> ECUSupportList = new List<List<OBDParameterValue>>();
            List<bool> ECUSupportNext = new List<bool>();
            OBDParameter param;
            int HByte = 0;
            if (m_obdInterface.UseISO27145) {
                HByte = (mode << 8) & 0xFF00;
                param = new OBDParameter(0x22, HByte, 0) {
                    ValueTypes = 32
                };
            } else {
                param = new OBDParameter(mode, 0, 0) {
                    ValueTypes = 32
                };
            }
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            foreach (OBDParameterValue value in valueList) {
                List<OBDParameterValue> ECUValueList = new List<OBDParameterValue>();
                if (value.ErrorDetected) {
                    return false;
                }
                ECUValueList.Add(value);
                ECUSupportList.Add(ECUValueList);
                ECUSupportNext.Add(value.GetBitFlag(31));
            }
            bool next = false;
            foreach (bool item in ECUSupportNext) {
                next = next || item;
            }
            if (next) {
                for (int i = 1; (i * 0x20) < maxPID; i++) {
                    param.Parameter = HByte + i * 0x20;
                    List<OBDParameterValue> valueList1 = m_obdInterface.GetValueList(param);
                    foreach (OBDParameterValue value in valueList1) {
                        if (value.ErrorDetected) {
                            return false;
                        }
                        for (int j = 0; j < ECUSupportList.Count; j++) {
                            if (ECUSupportList[j][0].ECUResponseID == value.ECUResponseID) {
                                if (ECUSupportNext[j]) {
                                    ECUSupportList[j].Add(value);
                                    ECUSupportNext[j] = value.GetBitFlag(31);
                                }
                            }
                        }
                    }
                    next = false;
                    foreach (bool item in ECUSupportNext) {
                        next = next || item;
                    }
                    if (!next) {
                        break;
                    }
                }
            }

            foreach (List<OBDParameterValue> ECUValueList in ECUSupportList) {
                List<bool> bitFlagList = new List<bool>();
                foreach (OBDParameterValue value in ECUValueList) {
                    for (int j = 0; j < 0x20; j++) {
                        bitFlagList.Add(value.GetBitFlag(j));
                    }
                }
                bool[] bitFlag = new bool[maxPID];
                for (int i = 0; i < bitFlagList.Count && i < bitFlag.Length; i++) {
                    bitFlag[i] = bitFlagList[i];
                }
                supportStatus.Add(ECUValueList[0].ECUResponseID, bitFlag);
            }
            foreach (string key in supportStatus.Keys) {
                string log = "";
                if (m_obdInterface.UseISO27145) {
                    log = "DID " + mode.ToString("X2") + " Support: [" + key + "], [";
                } else {
                    log = "Mode" + mode.ToString("X2") + " Support: [" + key + "], [";
                }
                for (int i = 0; i * 8 < maxPID; i++) {
                    for (int j = 0; j < 8; j++) {
                        log += supportStatus[key][i * 8 + j] ? "1" : "0";
                    }
                    log += " ";
                }
                log = log.TrimEnd();
                log += "]";
                m_obdInterface.m_log.TraceInfo(log);
            }
            return true;
        }

        private void SetGridViewColumnsSortMode(DataGridView gridView, DataGridViewColumnSortMode sortMode) {
            for (int i = 0; i < gridView.Columns.Count; i++) {
                gridView.Columns[i].SortMode = sortMode;
            }
        }

        private void OBDTestForm_Load(object sender, EventArgs e) {
            this.GridViewInfo.DataSource = m_dtInfo;
            this.GridViewECUInfo.DataSource = m_dtECUInfo;
            this.GridViewIUPR.DataSource = m_dtIUPR;
            this.GridViewInstantData.DataSource = m_dtInstantData;
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
            groupInstantData.Location = new Point(groupIUPR.Location.X, groupECUInfo.Location.Y);
            groupInstantData.Width = groupIUPR.Width;
            groupInstantData.Height = groupECUInfo.Height;
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
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "准备OBD检测";
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "准备上传数据";
            //m_timer.Enabled = false;
            m_dtInfo.Clear();
            m_dtInfo.Dispose();
            m_dtECUInfo.Clear();
            m_dtECUInfo.Dispose();
            m_dtIUPR.Clear();
            m_dtIUPR.Dispose();
            m_dtInstantData.Clear();
            m_dtInstantData.Dispose();
            m_mode01Support.Clear();
            m_mode09Support.Clear();
            m_compIgn = false;
            m_OBDResult = true;
            m_DTCResult = true;
            m_ReadinessResult = true;
            m_VINResult = true;

            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "OBD检测中。。。";
            int mode01 = 1;
            int mode09 = 9;
            if (m_obdInterface.UseISO27145) {
                mode01 = 0xF4;
                mode09 = 0xF8;
            }
            int count = 0;
            while (!GetSupportStatus(mode01, m_mode01Support)) {
                if (++count > 3) {
                    this.labelInfo.ForeColor = Color.Red;
                    this.labelInfo.Text = "连接车辆OBD出错！";
                    return;
                }
            }
            while (!GetSupportStatus(mode09, m_mode09Support)) {
                if (++count > 3) {
                    this.labelInfo.ForeColor = Color.Red;
                    this.labelInfo.Text = "连接车辆OBD出错！";
                    return;
                }
            }

            SetDataTableColumns<string>(m_dtInfo, m_mode01Support);
            SetDataTableColumns<string>(m_dtECUInfo, m_mode09Support);
            SetDataTableColumns<string>(m_dtIUPR, m_mode09Support);
            //SetDataTableColumns<string>(m_dtInstantData, m_mode01Support);
            GridViewInfo.Columns[0].Width = 30;
            GridViewInfo.Columns[1].Width = 150;
            GridViewECUInfo.Columns[0].Width = GridViewInfo.Columns[0].Width;
            GridViewIUPR.Columns[0].Width = GridViewInfo.Columns[0].Width;
            GridViewIUPR.Columns[1].Width = 230;
            //GridViewInstantData.Columns[0].Width = GridViewInfo.Columns[0].Width;
            //GridViewInstantData.Columns[1].Width = 200;
            SetGridViewColumnsSortMode(this.GridViewInfo, DataGridViewColumnSortMode.Programmatic);
            SetGridViewColumnsSortMode(this.GridViewECUInfo, DataGridViewColumnSortMode.Programmatic);
            SetGridViewColumnsSortMode(this.GridViewIUPR, DataGridViewColumnSortMode.Programmatic);
            //SetGridViewColumnsSortMode(this.GridViewInstantData, DataGridViewColumnSortMode.Programmatic);

            SetDataTableInfo();
            SetDataTableECUInfo();
            SetDataTableIUPR();
            //SetDataTableInstantData(true);
            //m_timer.Enabled = true;

            m_OBDResult = m_DTCResult && m_ReadinessResult && m_VINResult;
            if (m_OBDResult) {
                this.labelInfo.ForeColor = Color.ForestGreen;
                this.labelInfo.Text = "OBD检测结束，结果：合格";
            } else {
                string strCat = "";
                if (!m_DTCResult) {
                    strCat += "，存在故障码DTC";
                }
                if (!m_ReadinessResult) {
                    strCat += "，就绪状态未完成项超过2项";
                }
                if (!m_VINResult) {
                    strCat += "，VIN号不匹配";
                }
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "OBD检测结束，结果：不合格" + strCat;
            }
            this.txtBoxVIN.ReadOnly = false;
            Task.Factory.StartNew(WriteDBandMES);
        }

        public void WriteDBandMES() {
            this.BeginInvoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "写入数据库中。。。";
                this.txtBoxVIN.ReadOnly = false;
            });
            string strVIN = "";
            for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                strVIN = m_dtECUInfo.Rows[0][i].ToString();
                if (strVIN != "" || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            string strOBDResult = m_OBDResult ? "1" : "0";

            DataTable dt = new DataTable();
            try {
                dt.Columns.Add("VIN", typeof(string));       // 0
                dt.Columns.Add("ECU_ID", typeof(string));    // 1
                dt.Columns.Add("MIL", typeof(string));       // 2
                dt.Columns.Add("MIL_DIST", typeof(string));  // 3
                dt.Columns.Add("OBD_SUP", typeof(string));   // 4
                dt.Columns.Add("ODO", typeof(string));       // 5
                dt.Columns.Add("DTC03", typeof(string));     // 6
                dt.Columns.Add("DTC07", typeof(string));     // 7
                dt.Columns.Add("DTC0A", typeof(string));     // 8
                dt.Columns.Add("MIS_RDY", typeof(string));   // 9
                dt.Columns.Add("FUEL_RDY", typeof(string));  // 10
                dt.Columns.Add("CCM_RDY", typeof(string));   // 11
                dt.Columns.Add("CAT_RDY", typeof(string));   // 12
                dt.Columns.Add("HCAT_RDY", typeof(string));  // 13
                dt.Columns.Add("EVAP_RDY", typeof(string));  // 14
                dt.Columns.Add("AIR_RDY", typeof(string));   // 15
                dt.Columns.Add("ACRF_RDY", typeof(string));  // 16
                dt.Columns.Add("O2S_RDY", typeof(string));   // 17
                dt.Columns.Add("HTR_RDY", typeof(string));   // 18
                dt.Columns.Add("EGR_RDY", typeof(string));   // 19
                dt.Columns.Add("HCCAT_RDY", typeof(string)); // 20
                dt.Columns.Add("NCAT_RDY", typeof(string));  // 21
                dt.Columns.Add("BP_RDY", typeof(string));    // 22
                dt.Columns.Add("EGS_RDY", typeof(string));   // 23
                dt.Columns.Add("PM_RDY", typeof(string));    // 24
                dt.Columns.Add("ECU_NAME", typeof(string));  // 25
                dt.Columns.Add("CAL_ID", typeof(string));    // 26
                dt.Columns.Add("CVN", typeof(string));       // 27
                dt.Columns.Add("Result", typeof(string));    // 28

                for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                    DataRow dr = dt.NewRow();
                    dr[0] = strVIN;                                                     // VIN
                    dr[1] = m_dtECUInfo.Columns[i].ColumnName;                          // ECU_ID
                    for (int j = 2; j < m_dtInfo.Columns.Count; j++) {
                        if (m_dtInfo.Columns[j].ColumnName == m_dtECUInfo.Columns[i].ColumnName) {
                            dr[2] = m_dtInfo.Rows[0][j].ToString();                     // MIL
                            dr[3] = m_dtInfo.Rows[1][j].ToString();                     // MIL_DIST
                            dr[4] = m_dtInfo.Rows[2][j].ToString();                     // OBD_SUP
                            dr[5] = m_dtInfo.Rows[3][j].ToString();                     // ODO
                            dr[6] = m_dtInfo.Rows[4][j].ToString().Replace("\n", ",");  // DTC03
                            dr[7] = m_dtInfo.Rows[5][j].ToString().Replace("\n", ",");  // DTC07
                            dr[8] = m_dtInfo.Rows[6][j].ToString().Replace("\n", ",");  // DTC0A
                            dr[9] = m_dtInfo.Rows[7][j].ToString();                     // MIS_RDY
                            dr[10] = m_dtInfo.Rows[8][j].ToString();                    // FUEL_RDY
                            dr[11] = m_dtInfo.Rows[9][j].ToString();                    // CCM_RDY
                            if (m_compIgn) {
                                dr[12] = "不适用";                                      // CAT_RDY
                                dr[13] = "不适用";                                      // HCAT_RDY
                                dr[14] = "不适用";                                      // EVAP_RDY
                                dr[15] = "不适用";                                      // AIR_RDY
                                dr[16] = "不适用";                                      // ACRF_RDY
                                dr[17] = "不适用";                                      // O2S_RDY
                                dr[18] = "不适用";                                      // HTR_RDY
                                dr[19] = m_dtInfo.Rows[15][j].ToString();               // EGR_RDY
                                dr[20] = m_dtInfo.Rows[10][j].ToString();               // HCCAT_RDY
                                dr[21] = m_dtInfo.Rows[11][j].ToString();               // NCAT_RDY
                                dr[22] = m_dtInfo.Rows[12][j].ToString();               // BP_RDY
                                dr[23] = m_dtInfo.Rows[13][j].ToString();               // EGS_RDY
                                dr[24] = m_dtInfo.Rows[14][j].ToString();               // PM_RDY
                            } else {
                                dr[12] = m_dtInfo.Rows[10][j].ToString();               // CAT_RDY
                                dr[13] = m_dtInfo.Rows[11][j].ToString();               // HCAT_RDY
                                dr[14] = m_dtInfo.Rows[12][j].ToString();               // EVAP_RDY
                                dr[15] = m_dtInfo.Rows[13][j].ToString();               // AIR_RDY
                                dr[16] = m_dtInfo.Rows[14][j].ToString();               // ACRF_RDY
                                dr[17] = m_dtInfo.Rows[15][j].ToString();               // O2S_RDY
                                dr[18] = m_dtInfo.Rows[16][j].ToString();               // HTR_RDY
                                dr[19] = m_dtInfo.Rows[17][j].ToString();               // EGR_RDY
                                dr[20] = "不适用";                                      // HCCAT_RDY
                                dr[21] = "不适用";                                      // NCAT_RDY
                                dr[22] = "不适用";                                      // BP_RDY
                                dr[23] = "不适用";                                      // EGS_RDY
                                dr[24] = "不适用";                                      // PM_RDY
                            }
                            break;
                        }
                    }
                    dr[25] = m_dtECUInfo.Rows[1][i].ToString();                         // ECU_NAME
                    dr[26] = m_dtECUInfo.Rows[2][i].ToString().Replace("\n", ",");      // CAL_ID
                    dr[27] = m_dtECUInfo.Rows[3][i].ToString().Replace("\n", ",");      // CVN
                    dr[28] = m_OBDResult ? "1" : "0";
                    dt.Rows.Add(dr);
                }
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Result DataTable Error: " + ex.Message);
                MessageBox.Show(ex.Message, "Result DataTable Error");
                this.labelMESInfo.ForeColor = Color.Red;
                this.labelMESInfo.Text = "生成 DataTable 出错";
                dt.Dispose();
                return;
            }

            m_db.ModifyDB("OBDData", dt);

            this.BeginInvoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据库写入完成";
            });
            // 用“无条件上传”和“OBD检测结果”判断是否需要直接返回不上传MES
            if (!m_obdInterface.DBandMES.UploadWhenever && !m_OBDResult) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "数据不上传";
                });
                dt.Dispose();
                return;
            }

            // 上传MES接口
            this.BeginInvoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据上传中。。。";
            });
            m_obdInterface.DBandMES.ChangeWebService = false;
            if (!WSHelper.CreateWebService(m_obdInterface.DBandMES, out string error)) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "获取 WebService 接口出错";
                });
                m_obdInterface.m_log.TraceError("CreateWebService Error: " + error);
                MessageBox.Show(error, "CreateWebService Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dt.Dispose();
                return;
            }

            #region 小蓝和青云谱分开调用WebService方法，已不用
            //            if (/*m_obdInterface.DBandMES.WebServiceMethods.Contains("WriteDataToMes")*/true) {
            //                // 小蓝工厂WebService方法

            //                // DataTable必须设置TableName，否则调用方法时会报错“生成 XML 文档时出错”
            //                DataTable dt1MES = new DataTable("db1");
            //                SetDataTable1MES(dt1MES, strVIN, strOBDResult);

            //                DataTable dt2MES = new DataTable("db2");
            //                dt2MES.Columns.Add("obd");
            //                dt2MES.Columns.Add("odo");
            //                dt2MES.Columns.Add("ModuleID");
            //                dt2MES.Columns.Add("CALID");
            //                dt2MES.Columns.Add("CVN");
            //                for (int i = 0; i < dt.Rows.Count; i++) {
            //                    dt2MES.Rows.Add(
            //                        dt.Rows[i][4].ToString(),
            //                        dt.Rows[i][5].ToString(),
            //                        dt.Rows[i][1].ToString(),
            //                        dt.Rows[i][26].ToString(),
            //                        dt.Rows[i][27].ToString()
            //                    );
            //                }
            //                string strMsg = "";
            //                string strRet = "";
            //                try {
            //                    //MES1.WebServiceDemo ws = new MES1.WebServiceDemo();
            //                    //strRet = ws.WriteDataToMes(dt1MES, dt2MES, out strMsg);
            //                    strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
            //                } catch (Exception ex) {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.Red;
            //                        this.labelMESInfo.Text = "上传MES出错";
            //                    });
            //                    m_obdInterface.m_log.TraceError("GetResponseString error: " + ex.Message);
            //                    MessageBox.Show(ex.Message, WSHelper.GetMethodName(0), MessageBoxButtons.OK, MessageBoxIcon.Error);

            //                    dt2MES.Dispose();
            //                    dt1MES.Dispose();
            //                    dt.Dispose();
            //                    return;
            //                }
            //                if (strRet.Contains("NOK")) {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.Red;
            //                        this.labelMESInfo.Text = "上传MES出错";
            //                    });
            //                    m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
            //                    MessageBox.Show(strMsg, WSHelper.GetMethodName(0) + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                } else {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.ForestGreen;
            //                        this.labelMESInfo.Text = "上传MES完成";
            //                    });
            //#if DEBUG
            //                    MessageBox.Show(strMsg, WSHelper.GetMethodName(0));
            //#endif
            //                }
            //                dt2MES.Dispose();
            //                dt1MES.Dispose();
            //            } else {
            //                // 青云谱工厂WebService方法
            //                JES_REVICE_EQUIP_DATE[] SAPUpload = new JES_REVICE_EQUIP_DATE[dt.Rows.Count];
            //                for (int i = 0; i < dt.Rows.Count; i++) {
            //                    JES_REVICE_EQUIP_DATE json = new JES_REVICE_EQUIP_DATE {
            //                        ZH_VIN = strVIN,
            //                        ZH_OPASS = strOBDResult,
            //                        OBD_OTESTDATE = DateTime.Now.ToLocalTime().ToString(),
            //                        OBD_OBD = dt.Rows[i][4].ToString(),
            //                        OBD_ODO = dt.Rows[i][5].ToString(),
            //                        OBD_MODULEID = dt.Rows[i][1].ToString()
            //                    };

            //                    string[] CALIDArray = dt.Rows[i][26].ToString().Split(',');
            //                    string[] CVNArray = dt.Rows[i][27].ToString().Split(',');
            //                    json.OBD_ECALID = CALIDArray[0];
            //                    json.OBD_ECVN = CVNArray[0];
            //                    if (CALIDArray.Length >= 2) {
            //                        json.OBD_ACALID = CALIDArray[1];
            //                    }
            //                    if (CVNArray.Length >= 2) {
            //                        json.OBD_ACVN = CVNArray[1];
            //                    }

            //                    if (CALIDArray.Length >= 3) {
            //                        string CALIDRest = "";
            //                        for (int j = 2; j < CALIDArray.Length; j++) {
            //                            CALIDRest += CALIDArray[j] + ",";
            //                        }
            //                        CALIDRest = CALIDRest.TrimEnd(',');
            //                        json.OBD_OCALLID = CALIDRest;
            //                    }
            //                    if (CVNArray.Length >= 3) {
            //                        string CVNRest = "";
            //                        for (int j = 2; j < CVNArray.Length; j++) {
            //                            CVNRest += CVNArray[j] + ",";
            //                        }
            //                        CVNRest = CVNRest.TrimEnd(',');
            //                        json.OBD_OCVN = CVNRest;
            //                    }

            //                    SAPUpload[i] = json;
            //                }

            //                string strRet = "";
            //                try {
            //                    strRet = WSHelper.GetResponseString(WSHelper.GetMethodName(0), JsonConvert.SerializeObject(SAPUpload));
            //                } catch (Exception ex) {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.Red;
            //                        this.labelMESInfo.Text = "上传SAP出错";
            //                    });
            //                    m_obdInterface.m_log.TraceError("GetResponseString error: " + ex.Message);
            //                    MessageBox.Show(ex.Message, WSHelper.GetMethodName(0), MessageBoxButtons.OK, MessageBoxIcon.Error);

            //                    dt.Dispose();
            //                    return;
            //                }
            //                SAP_RETURN SAPRet = JsonConvert.DeserializeObject<SAP_RETURN>(strRet);
            //                if (SAPRet.IsSuccess) {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.ForestGreen;
            //                        this.labelMESInfo.Text = "上传SAP完成";
            //                    });
            //#if DEBUG
            //                    MessageBox.Show(SAPRet.MethodParameter + ": " + SAPRet.Message, WSHelper.GetMethodName(0));
            //#endif
            //                } else {
            //                    this.BeginInvoke((EventHandler)delegate {
            //                        this.labelMESInfo.ForeColor = Color.Red;
            //                        this.labelMESInfo.Text = "上传SAP出错";
            //                    });
            //                }
            //            }
            #endregion

            // DataTable必须设置TableName，否则调用方法时会报错“生成 XML 文档时出错”
            DataTable dt1MES = new DataTable("dt1");
            SetDataTable1MES(dt1MES, strVIN, strOBDResult);

            DataTable dt2MES = new DataTable("dt2");
            dt2MES.Columns.Add("obd");
            dt2MES.Columns.Add("odo");
            dt2MES.Columns.Add("ModuleID");
            dt2MES.Columns.Add("CALID");
            dt2MES.Columns.Add("CVN");
            for (int i = 0; i < dt.Rows.Count; i++) {
                string[] CALIDArray = dt.Rows[i][26].ToString().Split(',');
                string[] CVNArray = dt.Rows[i][27].ToString().Split(',');
                dt2MES.Rows.Add(
                    dt.Rows[i][4].ToString().Split(',')[0],
                    dt.Rows[i][5].ToString().Replace("不适用", ""),
                    m_obdInterface.DBandMES.UseECUName ? dt.Rows[i][25].ToString().Split('-')[0] : dt.Rows[i][1].ToString(),
                    CALIDArray[0],
                    CVNArray[0]
                );
                for (int j = 1; j < CALIDArray.Length; j++) {
                    if (j == 1) {
                        dt2MES.Rows.Add(
                            dt.Rows[i][4].ToString().Split(',')[0],
                            dt.Rows[i][5].ToString().Replace("不适用", ""),
                            m_obdInterface.DBandMES.UseECUName ? "SCR" : dt.Rows[i][1].ToString(),
                            CALIDArray[j],
                            CVNArray.Length > j ? CVNArray[j] : ""
                        );
                    } else {
                        dt2MES.Rows.Add(
                            dt.Rows[i][4].ToString().Split(',')[0],
                            dt.Rows[i][5].ToString().Replace("不适用", ""),
                            m_obdInterface.DBandMES.UseECUName ? dt.Rows[i][25].ToString().Split('-')[0] : dt.Rows[i][1].ToString(),
                            CALIDArray[j],
                            CVNArray.Length > j ? CVNArray[j] : ""
                        );
                    }
                }
            }
            string strMsg = "";
            string strRet = "";
            try {
                //MES1.WebServiceDemo ws = new MES1.WebServiceDemo();
                //strRet = ws.WriteDataToMes(dt1MES, dt2MES, out strMsg);
                strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
            } catch (Exception ex) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "上传数据出错！";
                });
                m_obdInterface.m_log.TraceError("GetResponseString error: " + ex.Message);
                MessageBox.Show(ex.Message, WSHelper.GetMethodName(0), MessageBoxButtons.OK, MessageBoxIcon.Error);

                dt2MES.Dispose();
                dt1MES.Dispose();
                dt.Dispose();
                return;
            }
            if (strRet.Contains("NOK") || strRet.Contains("false") || strRet.Contains("False")) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "上传数据出错！";
                });
                m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                MessageBox.Show(strMsg, WSHelper.GetMethodName(0) + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.ForestGreen;
                    this.labelMESInfo.Text = "上传数据完成";
                });
#if DEBUG
                MessageBox.Show(strMsg, WSHelper.GetMethodName(0));
#endif
            }
            dt2MES.Dispose();
            dt1MES.Dispose();
            dt.Dispose();
        }

        private void SetDataTable1MES(DataTable dt1MES, string strVIN, string strOBDResult) {
            dt1MES.Columns.Add("TestDate");     // 0
            dt1MES.Columns.Add("SBFLAG");       // 1
            dt1MES.Columns.Add("AnalyManuf");   // 2
            dt1MES.Columns.Add("AnalyName");    // 3
            dt1MES.Columns.Add("TEType");       // 4
            dt1MES.Columns.Add("AnalyModel");   // 5
            dt1MES.Columns.Add("analyDate");    // 6
            dt1MES.Columns.Add("VIN");          // 7
            dt1MES.Columns.Add("TestNo");       // 8
            dt1MES.Columns.Add("DynoManuf");    // 9
            dt1MES.Columns.Add("DynoModel");    // 10
            dt1MES.Columns.Add("TestType");     // 11
            dt1MES.Columns.Add("APASS");        // 12
            dt1MES.Columns.Add("OPASS");        // 13
            dt1MES.Columns.Add("EPASS");        // 14
            dt1MES.Columns.Add("Result");       // 15
            dt1MES.Columns.Add("JCJLNO");       // 16
            dt1MES.Columns.Add("JCXTNO");       // 17
            dt1MES.Columns.Add("JCKSSJ");       // 18
            dt1MES.Columns.Add("JCRJ");         // 19
            dt1MES.Columns.Add("DPCGJNO");      // 20
            dt1MES.Columns.Add("CGJXT");        // 21
            dt1MES.Columns.Add("PFCSSJ");       // 22
            dt1MES.Columns.Add("JYLX");         // 23
            dt1MES.Columns.Add("YRFF");         // 24
            dt1MES.Columns.Add("RH");           // 25
            dt1MES.Columns.Add("ET");           // 26
            dt1MES.Columns.Add("AP");           // 27
            dt1MES.Columns.Add("COND");         // 28
            dt1MES.Columns.Add("HCND");         // 29
            dt1MES.Columns.Add("NOXND");        // 30
            dt1MES.Columns.Add("CO2ND");        // 31
            dt1MES.Columns.Add("YND");          // 32
            dt1MES.Columns.Add("REAC");         // 33
            dt1MES.Columns.Add("LRCO");         // 34
            dt1MES.Columns.Add("LLCO");         // 35
            dt1MES.Columns.Add("LRHC");         // 36
            dt1MES.Columns.Add("LLHC");         // 37
            dt1MES.Columns.Add("HRCO");         // 38
            dt1MES.Columns.Add("HLCO");         // 39
            dt1MES.Columns.Add("HRHC");         // 40
            dt1MES.Columns.Add("HLHC");         // 41
            dt1MES.Columns.Add("JYWD");         // 42
            dt1MES.Columns.Add("FDJZS");        // 43
            dt1MES.Columns.Add("SDSFJCSJ");     // 44
            dt1MES.Columns.Add("SDSFGKSJ");     // 45
            dt1MES.Columns.Add("SSZMHCND");     // 46
            dt1MES.Columns.Add("SSZMCOND");     // 47
            dt1MES.Columns.Add("SSZMCO2ND");    // 48
            dt1MES.Columns.Add("SSZMO2ND");     // 49
            dt1MES.Columns.Add("SSZMGDS");      // 50
            dt1MES.Columns.Add("ARHC5025");     // 51
            dt1MES.Columns.Add("ALHC5025");     // 52
            dt1MES.Columns.Add("ARCO5025");     // 53
            dt1MES.Columns.Add("ALCO5025");     // 54
            dt1MES.Columns.Add("ARNOX5025");    // 55
            dt1MES.Columns.Add("ALNOX5025");    // 56
            dt1MES.Columns.Add("ARHC2540");     // 57
            dt1MES.Columns.Add("ALHC2540");     // 58
            dt1MES.Columns.Add("ARCO2540");     // 59
            dt1MES.Columns.Add("ALCO2540");     // 60
            dt1MES.Columns.Add("ARNOX2540");    // 61
            dt1MES.Columns.Add("ALNOX2540");    // 62
            dt1MES.Columns.Add("ZJHC5025");     // 63
            dt1MES.Columns.Add("ZJCO5025");     // 64
            dt1MES.Columns.Add("ZJNO5025");     // 65
            dt1MES.Columns.Add("ZGL5025");      // 66
            dt1MES.Columns.Add("FDJZS5025");    // 67
            dt1MES.Columns.Add("CS5025");       // 68
            dt1MES.Columns.Add("ZJHC2540");     // 69
            dt1MES.Columns.Add("ZJCO2540");     // 70
            dt1MES.Columns.Add("ZJNO2540");     // 71
            dt1MES.Columns.Add("ZGL2540");      // 72
            dt1MES.Columns.Add("FDJZS2540");    // 73
            dt1MES.Columns.Add("CS2540");       // 74
            dt1MES.Columns.Add("WTJCSJ");       // 75
            dt1MES.Columns.Add("WTGKSJ");       // 76
            dt1MES.Columns.Add("WTZMCS");       // 77
            dt1MES.Columns.Add("WTZMFDJZS");    // 78
            dt1MES.Columns.Add("WTZMFZ");       // 79
            dt1MES.Columns.Add("WTZMHCND");     // 80
            dt1MES.Columns.Add("WTZMCOND");     // 81
            dt1MES.Columns.Add("WTZMNOND");     // 82
            dt1MES.Columns.Add("WTZMCO2ND");    // 83
            dt1MES.Columns.Add("WTZMO2ND");     // 84
            dt1MES.Columns.Add("WTZMZ");        // 85
            dt1MES.Columns.Add("WTNOSDXS");     // 86
            dt1MES.Columns.Add("WTZMXSDF");     // 87
            dt1MES.Columns.Add("WTZMHCNDXZ");   // 88
            dt1MES.Columns.Add("WTZMCONDXZ");   // 89
            dt1MES.Columns.Add("WTZMNONDXZ");   // 90
            dt1MES.Columns.Add("VRHC");         // 91
            dt1MES.Columns.Add("VLHC");         // 92
            dt1MES.Columns.Add("VRCO");         // 93
            dt1MES.Columns.Add("VLCO");         // 94
            dt1MES.Columns.Add("VRNOX");        // 95
            dt1MES.Columns.Add("VLNOX");        // 96
            dt1MES.Columns.Add("VRHCNOX");      // 97
            dt1MES.Columns.Add("VLHCNOX");      // 98
            dt1MES.Columns.Add("JYCSSJ");       // 99
            dt1MES.Columns.Add("JYGL");         // 100
            dt1MES.Columns.Add("JYXSJL");       // 101
            dt1MES.Columns.Add("JYHCPF");       // 102
            dt1MES.Columns.Add("JYCOPF");       // 103
            dt1MES.Columns.Add("JYNOXPF");      // 104
            dt1MES.Columns.Add("JYPLCS");       // 105
            dt1MES.Columns.Add("JYGK");         // 106
            dt1MES.Columns.Add("JYZMCS");       // 107
            dt1MES.Columns.Add("JYZMZS");       // 108
            dt1MES.Columns.Add("JYZMZH");       // 109
            dt1MES.Columns.Add("JYZMHCND");     // 110
            dt1MES.Columns.Add("JYZMHCNDXZ");   // 111
            dt1MES.Columns.Add("JYZMCOND");     // 112
            dt1MES.Columns.Add("JYZMCONDXZ");   // 113
            dt1MES.Columns.Add("JYZMNOXND");    // 114
            dt1MES.Columns.Add("JYZMNOXNDXZ");  // 115
            dt1MES.Columns.Add("JYZMCO2ND");    // 116
            dt1MES.Columns.Add("JYZMO2ND");     // 117
            dt1MES.Columns.Add("JYXSO2ND");     // 118
            dt1MES.Columns.Add("JYXSLL");       // 119
            dt1MES.Columns.Add("JYXSXS");       // 120
            dt1MES.Columns.Add("JYNOSDXZ");     // 121
            dt1MES.Columns.Add("JYZMZ");        // 122
            dt1MES.Columns.Add("RateRev");      // 123
            dt1MES.Columns.Add("Rev");          // 124
            dt1MES.Columns.Add("SmokeK1");      // 125
            dt1MES.Columns.Add("SmokeK2");      // 126
            dt1MES.Columns.Add("SmokeK3");      // 127
            dt1MES.Columns.Add("SmokeAvg");     // 128
            dt1MES.Columns.Add("SmokeKLimit");  // 129
            dt1MES.Columns.Add("ZYGXSZ");       // 130
            dt1MES.Columns.Add("ZYJCSSJ");      // 131
            dt1MES.Columns.Add("ZYGKSJ");       // 132
            dt1MES.Columns.Add("ZYZS");         // 133
            dt1MES.Columns.Add("YDJZZC");       // 134
            dt1MES.Columns.Add("YDJMC");        // 135
            dt1MES.Columns.Add("ZYCCRQ");       // 136
            dt1MES.Columns.Add("ZYJDRQ");       // 137
            dt1MES.Columns.Add("ZYJCJL");       // 138
            dt1MES.Columns.Add("ZYBDJL");       // 139
            dt1MES.Columns.Add("RateRevUp");    // 140
            dt1MES.Columns.Add("RateRevDown");  // 141
            dt1MES.Columns.Add("Rev100");       // 142
            dt1MES.Columns.Add("MaxPower");     // 143
            dt1MES.Columns.Add("MaxPowerLimit");// 144
            dt1MES.Columns.Add("Smoke100");     // 145
            dt1MES.Columns.Add("Smoke80");      // 146
            dt1MES.Columns.Add("SmokeLimit");   // 147
            dt1MES.Columns.Add("Nox");          // 148
            dt1MES.Columns.Add("NoxLimit");     // 149
            dt1MES.Columns.Add("JSGXS100");     // 150
            dt1MES.Columns.Add("JSGXS80");      // 151
            dt1MES.Columns.Add("JSLBGL");       // 152
            dt1MES.Columns.Add("JSFDJZS");      // 153
            dt1MES.Columns.Add("JSJCSJ");       // 154
            dt1MES.Columns.Add("JSGKSJ");       // 155
            dt1MES.Columns.Add("JSZMCS");       // 156
            dt1MES.Columns.Add("JSZMZS");       // 157
            dt1MES.Columns.Add("JSZMZH");       // 158
            dt1MES.Columns.Add("JSZMNJ");       // 159
            dt1MES.Columns.Add("JSZMGXS");      // 160
            dt1MES.Columns.Add("JSZMCO2ND");    // 161
            dt1MES.Columns.Add("JSZMNOND");     // 162
            dt1MES.Columns.Add("otestdate");    // 163
            dt1MES.Columns.Add("leacmax");      // 164
            dt1MES.Columns.Add("leacmin");      // 165

            DataRow dr = dt1MES.NewRow();
            dr[1] = "OBD";
            dr[7] = strVIN;
            dr[13] = strOBDResult;
            dr[163] = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            dt1MES.Rows.Add(dr);
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
