using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH_OBD {
    public class OBDTest {
        private const int maxPID = 0x100;
        private readonly OBDInterface m_obdInterface;
        private readonly DataTable m_dtInfo;
        private readonly DataTable m_dtECUInfo;
        private readonly DataTable m_dtIUPR;
        private readonly Dictionary<string, bool[]> m_mode01Support;
        private readonly Dictionary<string, bool[]> m_mode09Support;
        private bool m_compIgn;
        public readonly Model m_db;
        public event Action OBDTestStart;
        public event Action SetupColumnsDone;
        public event Action WriteDbStart;
        public event Action WriteDbDone;
        public event Action UploadDataStart;
        public event Action UploadDataDone;
        public bool AdvanceMode { get; set; }
        public int AccessAdvanceMode { get; set; }
        public bool OBDResult { get; set; }
        public bool DTCResult { get; set; }
        public bool ReadinessResult { get; set; }
        public bool VINResult { get; set; }
        public string StrVIN_IN { get; set; }

        public OBDTest(OBDInterface obd) {
            m_obdInterface = obd;
            m_dtInfo = new DataTable();
            m_dtECUInfo = new DataTable();
            m_dtIUPR = new DataTable();
            m_mode01Support = new Dictionary<string, bool[]>();
            m_mode09Support = new Dictionary<string, bool[]>();
            m_compIgn = false;
            AdvanceMode = false;
            AccessAdvanceMode = 0;
            OBDResult = true;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;
            m_db = new Model(m_obdInterface.DBandMES, m_obdInterface.m_log);
        }

        public DataTable GetDataTable(int index) {
            switch (index) {
            case 0:
                return m_dtInfo;
            case 1:
                return m_dtECUInfo;
            case 2:
                return m_dtIUPR;
            default:
                return null;
            }
        }

        private void SetDataTableColumns<T>(DataTable dt, Dictionary<string, bool[]> ECUSupports) {
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

        private void SetReadinessDataRow(int lineNO, string strItem, DataTable dt, List<OBDParameterValue> valueList, int bitIndex, int bitOffset, ref int errorCount) {
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

        private void SetDataTableInfo() {
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
                    OBDResult = false;
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
                    DTCResult = false;
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
                    DTCResult = false;
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
                    DTCResult = false;
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
            SetReadinessDataRow(++NO, "EGR/VVT系统监测", dt, valueList, 16, 8, ref errorCount);        // 15 / 17
            if (errorCount > 2) {
                ReadinessResult = false;
            }
        }

        private void SetDataTableECUInfo() {
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
            if (StrVIN_IN != null && strVIN != StrVIN_IN && StrVIN_IN != "") {
                VINResult = false;
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

        public void StartOBDTest(out string errorMsg) {
            errorMsg = "";
            m_dtInfo.Clear();
            m_dtInfo.Dispose();
            m_dtECUInfo.Clear();
            m_dtECUInfo.Dispose();
            m_dtIUPR.Clear();
            m_dtIUPR.Dispose();
            m_mode01Support.Clear();
            m_mode09Support.Clear();
            m_compIgn = false;
            OBDResult = true;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;

            OBDTestStart?.Invoke();

            int mode01 = 1;
            int mode09 = 9;
            if (m_obdInterface.UseISO27145) {
                mode01 = 0xF4;
                mode09 = 0xF8;
            }
            int count = 0;
            while (!GetSupportStatus(mode01, m_mode01Support)) {
                if (++count > 3) {
                    errorMsg = "连接OBD接口出错！";
                    return;
                }
            }
            while (!GetSupportStatus(mode09, m_mode09Support)) {
                if (++count > 3) {
                    errorMsg = "连接OBD接口出错！";
                    return;
                }
            }

            SetDataTableColumns<string>(m_dtInfo, m_mode01Support);
            SetDataTableColumns<string>(m_dtECUInfo, m_mode09Support);
            SetDataTableColumns<string>(m_dtIUPR, m_mode09Support);
            SetupColumnsDone?.Invoke();
            SetDataTableInfo();
            SetDataTableECUInfo();
            SetDataTableIUPR();

            OBDResult = DTCResult && ReadinessResult && VINResult;

            WriteDbStart?.Invoke();
            string strVIN = "";
            for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                strVIN = m_dtECUInfo.Rows[0][i].ToString();
                if (strVIN != "" || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            string strOBDResult = OBDResult ? "1" : "0";

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
                    dr[28] = OBDResult ? "1" : "0";
                    dt.Rows.Add(dr);
                }
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Result DataTable Error: " + ex.Message);
                dt.Dispose();
                throw new Exception("生成 Result DataTable 出错");
            }

            m_db.ModifyDB("OBDData", dt);
            WriteDbDone?.Invoke();

            try {
                ExportResultFile(dt);
            } finally {
                m_obdInterface.m_log.TraceError("Exporting OBD test result file failed");
                dt.Dispose();
            }

            // 用“无条件上传”和“OBD检测结果”判断是否需要直接返回不上传MES
            if (!m_obdInterface.DBandMES.UploadWhenever && !OBDResult) {
                m_obdInterface.m_log.TraceError("Won't upload data because OBD test result is NOK");
                dt.Dispose();
                return;
            }

            // 上传MES接口
            UploadDataStart?.Invoke();
            m_obdInterface.DBandMES.ChangeWebService = false;
            if (!WSHelper.CreateWebService(m_obdInterface.DBandMES, out string error)) {
                m_obdInterface.m_log.TraceError("CreateWebService Error: " + error);
                dt.Dispose();
                throw new Exception("获取 WebService 接口出错");
            }

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
            string strMsg;
            string strRet;
            try {
                strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("GetResponseString error: " + ex.Message);
                dt2MES.Dispose();
                dt1MES.Dispose();
                dt.Dispose();
                throw new Exception("上传数据出错");
            }

            dt2MES.Dispose();
            dt1MES.Dispose();
            dt.Dispose();
            if (strRet.Contains("NOK") || strRet.Contains("false") || strRet.Contains("False")) {
                m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                throw new Exception("上传数据出错");
            } else {
                UploadDataDone?.Invoke();
#if DEBUG
                errorMsg = strMsg;
#endif
            }
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

        private void ExportResultFile(DataTable dt) {
            string OriPath = ".\\Configs\\OBD_Result.xlsx";
            string ExportPath = ".\\Export\\" + DateTime.Now.ToLocalTime().ToString("yyyy-MM");
            if (!Directory.Exists(ExportPath)) {
                Directory.CreateDirectory(ExportPath);
            }
            ExportPath += "\\" + StrVIN_IN + "_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd-HHmmss") + ".xlsx";
            FileInfo fileInfo = new FileInfo(OriPath);
            using (ExcelPackage package = new ExcelPackage(fileInfo, true)) {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                worksheet1.Cells["B2"].Value = dt.Rows[0][0].ToString(); // VIN

                // CALID, CVN
                string[] CALIDArray = dt.Rows[0][26].ToString().Split(',');
                string[] CVNArray = dt.Rows[0][27].ToString().Split(',');
                for (int i = 0; i < 2; i++) {
                    worksheet1.Cells[3 + i, 2].Value = CALIDArray.Length > i ? CALIDArray[i] : "";
                    worksheet1.Cells[3 + i, 4].Value = CVNArray.Length > i ? CVNArray[i] : "";
                }
                for (int i = 1; i < dt.Rows.Count; i++) {
                    worksheet1.Cells["B5"].Value = dt.Rows[i][26].ToString().Replace(",", "\n");
                    worksheet1.Cells["D5"].Value = dt.Rows[i][27].ToString().Replace(",", "\n");
                }

                worksheet1.Cells["C7"].Value = "合格"; // OBD故障指示器
                worksheet1.Cells["C8"].Value = "通讯成功"; // 与OBD诊断仪通讯情况
                worksheet1.Cells["C9"].Value = dt.Rows[0][2].ToString() == "ON" ? "是" : "否"; // OBD故障指示器被点亮

                for (int i = 0; i < dt.Rows.Count; i++) {
                    // 故障代码及故障信息
                    string DTC = dt.Rows[i][6].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                    if (DTC != "") {
                        worksheet1.Cells[10, 3 + i].Value = DTC;
                    }
                    DTC = dt.Rows[i][7].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                    if (worksheet1.Cells[10, 3 + i].Value != null && DTC != "") {
                        worksheet1.Cells[10, 3 + i].Value += "\n";
                    }
                    if (DTC != "") {
                        worksheet1.Cells[10, 3 + i].Value += DTC;
                    }
                    DTC = dt.Rows[i][8].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                    if (worksheet1.Cells[10, 3 + i].Value != null && DTC != "") {
                        worksheet1.Cells[10, 3 + i].Value += "\n";
                    }
                    if (DTC != "") {
                        worksheet1.Cells[10, 3 + i].Value += DTC;
                    }

                    // 诊断就绪状态未完成项目
                    string readiness = dt.Rows[i][9].ToString();
                    if (readiness == "未完成") {
                        worksheet1.Cells[11, 3 + i].Value = "失火";
                    }
                    readiness = dt.Rows[i][10].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "燃油系统";
                    }
                    readiness = dt.Rows[i][11].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "综合组件";
                    }
                    readiness = dt.Rows[i][12].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "催化剂";
                    }
                    readiness = dt.Rows[i][13].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "加热催化剂";
                    }
                    readiness = dt.Rows[i][14].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "燃油蒸发系统";
                    }
                    readiness = dt.Rows[i][15].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "二次空气系统";
                    }
                    readiness = dt.Rows[i][16].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "空调系统制冷剂";
                    }
                    readiness = dt.Rows[i][17].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "氧气传感器";
                    }
                    readiness = dt.Rows[i][18].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "加热氧气传感器";
                    }
                    readiness = dt.Rows[i][19].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "EGR/VVT系统监测";
                    }
                    readiness = dt.Rows[i][20].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "NMHC催化剂";
                    }
                    readiness = dt.Rows[i][21].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "NOx/SCR后处理";
                    }
                    readiness = dt.Rows[i][22].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "增压系统";
                    }
                    readiness = dt.Rows[i][23].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "废气传感器";
                    }
                    readiness = dt.Rows[i][24].ToString();
                    if (readiness == "未完成") {
                        if (worksheet1.Cells[11, 3 + i].Value != null) {
                            worksheet1.Cells[11, 3 + i].Value += "\n";
                        }
                        worksheet1.Cells[11, 3 + i].Value += "PM过滤器";
                    }
                }

                worksheet1.Cells["C12"].Value = dt.Rows[0][3].ToString(); // MIL灯点亮后行驶里程（km）
                // 检测结果
                string Result = OBDResult ? "合格" : "不合格";
                Result += DTCResult ? "" : ",有DTC";
                Result += ReadinessResult ? "" : ",就绪状态未完成项超过2项";
                Result += VINResult ? "" : ",VIN号不匹配";
                worksheet1.Cells["B13"].Value = Result;

                byte[] bin = package.GetAsByteArray();
                FileInfo exportFileInfo = new FileInfo(ExportPath);
                File.WriteAllBytes(exportFileInfo.FullName, bin);
            }
        }
    }
}
