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
        private Model m_db;

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
            m_db = new Model(m_obdInterface.DBandMES, m_obdInterface.m_log);
        }

        ~OBDTestForm() {
            m_dtInfo.Dispose();
            m_dtECUInfo.Dispose();
            m_dtIUPR.Dispose();
            m_dtInstantData.Dispose();
            m_timer.Enabled = false;
            m_timer.Dispose();
        }

        public void CheckConnection() {
            if (m_obdInterface.ConnectedStatus) {
                btnStartOBDTest.Enabled = true;
                this.labelInfo.ForeColor = Color.Black;
                this.labelInfo.Text = "准备OBD检测";
            } else {
                btnStartOBDTest.Enabled = false;
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "等待连接车辆OBD接口";
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
            if (param.Service == 1) {
                support = m_mode01Support;
            } else if (param.Service == 9) {
                support = m_mode09Support;
            }
            DataRow dr = dt.NewRow();
            dr[0] = lineNO;
            dr[1] = strItem;

            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.ListString) != 0) {
                int maxLine = 0;
                foreach (OBDParameterValue value in valueList) {
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
                if (param.Service == 1 || param.Service == 9) {
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (!support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
                            dr[i] = "不适用";
                        }
                    }
                }
                dt.Rows.Add(dr);
            } else {
                foreach (OBDParameterValue value in valueList) {
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
                                if (value.StringValue == "") {
                                    dr[i] = "--";
                                } else {
                                    dr[i] = value.StringValue;
                                }
                            }
                        }
                    }
                }
                if (param.Service == 1 || param.Service == 9) {
                    for (int i = 2; i < dt.Columns.Count; i++) {
                        if (!support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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
                if (!m_mode01Support[dt.Columns[i].ColumnName][0]) {
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
            OBDParameter param = new OBDParameter {
                OBDRequest = "0101",
                Service = 1,
                Parameter = 1,
                SubParameter = 0,
                ValueTypes = (int)OBDParameter.EnumValueTypes.Bool
            };
            SetDataRow(++NO, "MIL状态", dt, param);                         // 0
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[dt.Rows.Count - 1][i].ToString() == "ON") {
                    m_OBDResult = false;
                }
            }

            param.OBDRequest = "0121";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            SetDataRow(++NO, "MIL亮后行驶里程（km）", dt, param);            // 1  

            param.OBDRequest = "011C";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.String;
            SetDataRow(++NO, "OBD型式检验类型", dt, param);                  // 2

            param.OBDRequest = "01A6";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            SetDataRow(++NO, "总累积里程ODO（km）", dt, param);              // 3

            param.OBDRequest = "03";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "存储DTC", dt, param);                         // 4
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[dt.Rows.Count - 1][i].ToString() != "--") {
                    m_OBDResult = false;
                }
            }

            param.OBDRequest = "07";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "未决DTC", dt, param);                         // 5
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[dt.Rows.Count - 1][i].ToString() != "--") {
                    m_OBDResult = false;
                }
            }

            param.OBDRequest = "0A";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "永久DTC", dt, param);                         // 6
            for (int i = 2; i < dt.Columns.Count; i++) {
                if (dt.Rows[dt.Rows.Count - 1][i].ToString() != "--") {
                    m_OBDResult = false;
                }
            }

            int errorCount = 0;
            param.OBDRequest = "0101";
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.BitFlags;
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            SetReadinessDataRow(++NO, "失火监测", dt, valueList, 15, -4, ref errorCount);      // 7
            SetReadinessDataRow(++NO, "燃油系统监测", dt, valueList, 14, -4, ref errorCount);  // 8
            SetReadinessDataRow(++NO, "综合组件监测", dt, valueList, 13, -4, ref errorCount);  // 9

            foreach (OBDParameterValue value in valueList) {
                if (m_mode01Support[value.ECUResponseID][param.Parameter - 1]) {
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
                m_OBDResult = false;
            }
        }

        public void SetDataTableECUInfo() {
            DataTable dt = m_dtECUInfo;
            int NO = 0;

            OBDParameter param = new OBDParameter {
                OBDRequest = "0902",
                Service = 9,
                Parameter = 2,
                ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
            };
            SetDataRow(++NO, "VIN", dt, param);     // 0
            param.Parameter = 0x0A;
            SetDataRow(++NO, "ECU名称", dt, param); // 1
            param.Parameter = 4;
            SetDataRow(++NO, "CAL_ID", dt, param);  // 2
            param.Parameter = 6;
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
                        if (m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
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
                        if (m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
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
                        if (m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
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

            OBDParameter param = new OBDParameter {
                OBDRequest = "090B",
                Service = 9,
                Parameter = 0x0B,
                ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
            };
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            SetIUPRDataRow(++NO, "NMHC催化器", 18, 12, dt, valueList, 2, param.Parameter);
            SetIUPRDataRow(++NO, "NOx催化器", 18, 11, dt, valueList, 4, param.Parameter);
            SetIUPRDataRow(++NO, "NOx吸附器", 18, 11, dt, valueList, 6, param.Parameter);
            SetIUPRDataRow(++NO, "PM捕集器", 18, 10, dt, valueList, 8, param.Parameter);
            SetIUPRDataRow(++NO, "废气传感器", 18, 12, dt, valueList, 10, param.Parameter);
            SetIUPRDataRow(++NO, "EGR和VVT", 18, 10, dt, valueList, 12, param.Parameter);
            SetIUPRDataRow(++NO, "增压压力", 18, 10, dt, valueList, 14, param.Parameter);

            NO = 0;
            param.Parameter = 8;
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
                    if (!support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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
                if (m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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
                        if (m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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
                        if (m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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
                        if (m_mode01Support[dt.Columns[i].ColumnName][param.Parameter - 1]) {
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

        void OnTimerElapsed(object sender, EventArgs e) {
            SetDataTableInstantData(false);
        }

        private bool GetSupportStatus(int mode, Dictionary<string, bool[]> supportStatus) {
            List<List<OBDParameterValue>> ECUSupportList = new List<List<OBDParameterValue>>();
            List<bool> ECUSupportNext = new List<bool>();
            OBDParameter param = new OBDParameter(mode, 0, 0) {
                ValueTypes = 32
            };
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
                    param.Parameter = i * 0x20;
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
            foreach (var key in supportStatus.Keys) {
                string log = "Mode" + mode.ToString("X2") + " Support: [" + key + "], [";
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
            labelMESInfo.Location = new Point(groupIUPR.Location.X, labelInfo.Location.Y);
        }

        private void OBDTestForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                CheckConnection();
            }
        }

        private void BtnStartOBDTest_Click(object sender, EventArgs e) {
            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "准备OBD检测";
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "准备上传MES";
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

            this.labelInfo.ForeColor = Color.Black;
            this.labelInfo.Text = "OBD检测中。。。";
            int count = 0;
            while (!GetSupportStatus(1, m_mode01Support)) {
                if (++count > 3) {
                    this.labelInfo.ForeColor = Color.Red;
                    this.labelInfo.Text = "连接车辆OBD出错！";
                    return;
                }
            }
            while (!GetSupportStatus(9, m_mode09Support)) {
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
            GridViewIUPR.Columns[1].Width = 220;
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
            if (m_OBDResult) {
                this.labelInfo.ForeColor = Color.ForestGreen;
                this.labelInfo.Text = "OBD检测结束，结果：合格";
            } else {
                this.labelInfo.ForeColor = Color.Red;
                this.labelInfo.Text = "OBD检测结束，结果：不合格";
            }
            this.labelMESInfo.ForeColor = Color.Black;
            this.labelMESInfo.Text = "写入数据库中。。。";
            Task.Factory.StartNew(WriteDBandMES);
        }

        public void WriteDBandMES() {
            string strVIN = "";
            for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                strVIN = m_dtECUInfo.Rows[0][i].ToString();
                if (strVIN != "" || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            string strOBDResult = m_OBDResult ? "1" : "0";

            DataTable dt = new DataTable();
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

            for (int i = 2; i < m_dtInfo.Columns.Count; i++) {
                DataRow dr = dt.NewRow();
                dr[0] = strVIN;                                                // VIN
                dr[25] = m_dtECUInfo.Rows[1][i].ToString();                    // ECU_NAME
                dr[26] = m_dtECUInfo.Rows[2][i].ToString().Replace("\n", ","); // CAL_ID
                dr[27] = m_dtECUInfo.Rows[3][i].ToString().Replace("\n", ","); // CVN

                dr[1] = m_dtInfo.Columns[i].ColumnName;       // ECU_ID
                dr[2] = m_dtInfo.Rows[0][i].ToString();       // MIL
                dr[3] = m_dtInfo.Rows[1][i].ToString();       // MIL_DIST
                dr[4] = m_dtInfo.Rows[2][i].ToString();       // OBD_SUP
                dr[5] = m_dtInfo.Rows[3][i].ToString();       // ODO
                dr[6] = m_dtInfo.Rows[4][i].ToString();       // DTC03
                dr[7] = m_dtInfo.Rows[5][i].ToString();       // DTC07
                dr[8] = m_dtInfo.Rows[6][i].ToString();       // DTC0A
                dr[9] = m_dtInfo.Rows[7][i].ToString();       // MIS_RDY
                dr[10] = m_dtInfo.Rows[8][i].ToString();      // FUEL_RDY
                dr[11] = m_dtInfo.Rows[9][i].ToString();      // CCM_RDY
                if (m_compIgn) {
                    dr[12] = "不适用"; // CAT_RDY
                    dr[13] = "不适用"; // HCAT_RDY
                    dr[14] = "不适用"; // EVAP_RDY
                    dr[15] = "不适用"; // AIR_RDY
                    dr[16] = "不适用"; // ACRF_RDY
                    dr[17] = "不适用"; // O2S_RDY
                    dr[18] = "不适用"; // HTR_RDY

                    dr[19] = m_dtInfo.Rows[15][i].ToString(); // EGR_RDY
                    dr[20] = m_dtInfo.Rows[10][i].ToString(); // HCCAT_RDY
                    dr[21] = m_dtInfo.Rows[11][i].ToString(); // NCAT_RDY
                    dr[22] = m_dtInfo.Rows[12][i].ToString(); // BP_RDY
                    dr[23] = m_dtInfo.Rows[13][i].ToString(); // EGS_RDY
                    dr[24] = m_dtInfo.Rows[14][i].ToString(); // PM_RDY
                } else {
                    dr[12] = m_dtInfo.Rows[10][i].ToString(); // CAT_RDY
                    dr[13] = m_dtInfo.Rows[11][i].ToString(); // HCAT_RDY
                    dr[14] = m_dtInfo.Rows[12][i].ToString(); // EVAP_RDY
                    dr[15] = m_dtInfo.Rows[13][i].ToString(); // AIR_RDY
                    dr[16] = m_dtInfo.Rows[14][i].ToString(); // ACRF_RDY
                    dr[17] = m_dtInfo.Rows[15][i].ToString(); // O2S_RDY
                    dr[18] = m_dtInfo.Rows[16][i].ToString(); // HTR_RDY
                    dr[19] = m_dtInfo.Rows[17][i].ToString(); // EGR_RDY

                    dr[20] = "不适用"; // HCCAT_RDY
                    dr[21] = "不适用"; // NCAT_RDY
                    dr[22] = "不适用"; // BP_RDY
                    dr[23] = "不适用"; // EGS_RDY
                    dr[24] = "不适用"; // PM_RDY
                }
                dr[28] = m_OBDResult ? "1" : "0";
                dt.Rows.Add(dr);
            }

            m_db.ModifyDB("OBDData", dt);

            this.BeginInvoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据库写入完成";
            });
            if (!m_OBDResult) {
                // OBD检测不合格直接返回，不上传MES
                dt.Dispose();
                return;
            }

            // 上传MES接口
            this.BeginInvoke((EventHandler)delegate {
                this.labelMESInfo.ForeColor = Color.Black;
                this.labelMESInfo.Text = "数据上传MES中。。。";
            });

            if (!WSHelper.CreateWebService(m_obdInterface.DBandMES, out string error)) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "获取MES接口出错";
                });
                m_obdInterface.m_log.TraceError("CreateWebService Error: " + error);
                MessageBox.Show(error, "CreateWebService Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dt.Dispose();
                return;
            }

            // DataTable必须设置TableName，否则调用方法时会报错“生成 XML 文档时出错”
            DataTable dt1MES = new DataTable("db1");
            dt1MES.Columns.Add("VIN");
            dt1MES.Columns.Add("OPASS");
            dt1MES.Columns.Add("otestdate");
            dt1MES.Rows.Add(strVIN, strOBDResult, DateTime.Now.ToLocalTime().ToString());

            DataTable dt2MES = new DataTable("db2");
            dt2MES.Columns.Add("obd");
            dt2MES.Columns.Add("odo");
            dt2MES.Columns.Add("ModuleID");
            dt2MES.Columns.Add("CALID");
            dt2MES.Columns.Add("CVN");
            for (int i = 0; i < dt.Rows.Count; i++) {
                dt2MES.Rows.Add(
                    dt.Rows[i][4].ToString(),
                    dt.Rows[i][5].ToString(),
                    dt.Rows[i][1].ToString(),
                    dt.Rows[i][26].ToString(),
                    dt.Rows[i][27].ToString()
                );
            }
            string strMsg = "";
            string strRet = "";
            try {
                //MES1.WebServiceDemo ws = new MES1.WebServiceDemo();
                //strRet = ws.WriteDataToMes(dt1MES, dt2MES, out strMsg);
                strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
            } catch (Exception e) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "上传MES出错";
                });
                m_obdInterface.m_log.TraceError("GetResponseString error: " + e.Message);
                MessageBox.Show(e.Message, WSHelper.GetMethodName(0), MessageBoxButtons.OK, MessageBoxIcon.Error);

                dt2MES.Dispose();
                dt1MES.Dispose();
                dt.Dispose();
                return;
            }
            if (strRet.Contains("NOK")) {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.Red;
                    this.labelMESInfo.Text = "上传MES出错";
                });
                m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                MessageBox.Show(strMsg, WSHelper.GetMethodName(0) + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                this.BeginInvoke((EventHandler)delegate {
                    this.labelMESInfo.ForeColor = Color.ForestGreen;
                    this.labelMESInfo.Text = "上传MES完成";
                });
#if DEBUG
                MessageBox.Show(strMsg, WSHelper.GetMethodName(0));
#endif
            }

            dt2MES.Dispose();
            dt1MES.Dispose();
            dt.Dispose();
        }
    }

}
