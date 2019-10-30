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
        //private readonly DataTable m_dtIUPR;
        private readonly Dictionary<string, bool[]> m_mode01Support;
        private readonly Dictionary<string, bool[]> m_mode09Support;
        private static int m_iSN;
        private bool m_compIgn;
        public readonly Model m_db;
        public readonly ModelOracle m_dbOracle;
        public event Action OBDTestStart;
        public event Action SetupColumnsDone;
        public event Action WriteDbStart;
        public event Action WriteDbDone;
        public event Action UploadDataStart;
        public event Action UploadDataDone;
        public event Action NotUploadData;
        public event EventHandler<SetDataTableColumnsErrorEventArgs> SetDataTableColumnsError;

        public bool AdvanceMode { get; set; }
        public int AccessAdvanceMode { get; set; }
        public bool OBDResult { get; set; }
        public bool DTCResult { get; set; }
        public bool ReadinessResult { get; set; }
        public bool VINResult { get; set; }
        public bool CALIDCVNResult { get; set; }
        public bool SpaceResult { get; set; }
        public string StrVIN_IN { get; set; }

        public OBDTest(OBDInterface obd) {
            m_obdInterface = obd;
            m_dtInfo = new DataTable();
            m_dtECUInfo = new DataTable();
            //m_dtIUPR = new DataTable();
            m_mode01Support = new Dictionary<string, bool[]>();
            m_mode09Support = new Dictionary<string, bool[]>();
            m_compIgn = false;
            AdvanceMode = false;
            AccessAdvanceMode = 0;
            OBDResult = true;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;
            CALIDCVNResult = true;
            SpaceResult = true;
            m_db = new Model(m_obdInterface.DBandMES, m_obdInterface.m_log);
            m_dbOracle = new ModelOracle(m_obdInterface.OracleMESSetting, m_obdInterface.m_log);
        }

        public DataTable GetDataTable(int index) {
            switch (index) {
            case 0:
                return m_dtInfo;
            case 1:
                return m_dtECUInfo;
            //case 2:
            //    return m_dtIUPR;
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
                            if (value.ListStringValue.Count == 0 || value.ListStringValue[0].Length == 0) {
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
                if (m_obdInterface.OBDResultSetting.DTC03 && DTC != "--" && DTC != "不适用" && DTC.Length > 0) {
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
                if (m_obdInterface.OBDResultSetting.DTC07 && DTC != "--" && DTC != "不适用" && DTC.Length > 0) {
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
                if (m_obdInterface.OBDResultSetting.DTC0A && DTC != "--" && DTC != "不适用" && DTC.Length > 0) {
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
                SetReadinessDataRow(++NO, "排气传感器监测", dt, valueList, 18, 8, ref errorCount);     // 13
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
            if (m_obdInterface.OBDResultSetting.Readiness && errorCount > 2) {
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
                if (strVIN.Length > 0 || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            if (m_obdInterface.OBDResultSetting.VINError && StrVIN_IN != null && strVIN != StrVIN_IN && StrVIN_IN.Length > 0) {
                VINResult = false;
            }
            param.Parameter = HByte + 0x0A;
            SetDataRow(++NO, "ECU名称", dt, param); // 1
            param.Parameter = HByte + 4;
            SetDataRow(++NO, "CAL_ID", dt, param);  // 2
            param.Parameter = HByte + 6;
            SetDataRow(++NO, "CVN", dt, param);     // 3

            // 根据配置文件，判断CAL_ID和CVN两个值的合法性
            for (int i = 2; i < dt.Columns.Count; i++) {
                string[] CALIDArray = dt.Rows[2][i].ToString().Split('\n');
                string[] CVNArray = dt.Rows[3][i].ToString().Split('\n');
                int length = Math.Max(CALIDArray.Length, CVNArray.Length);
                for (int j = 0; j < length; j++) {
                    string CALID = CALIDArray.Length > j ? CALIDArray[j] : "";
                    string CVN = CVNArray.Length > j ? CVNArray[j] : "";
                    if (!m_obdInterface.OBDResultSetting.Allow3Space) {
                        if (CALID.Contains("   ") || CVN.Contains("   ")) {
                            SpaceResult = false;
                        }
                    }
                    if (!m_obdInterface.OBDResultSetting.CALIDCVNEmpty) {
                        if (CALID.Length * CVN.Length == 0 && CALID.Length + CVN.Length != 0) {
                            CALIDCVNResult = false;
                        }
                    }
                }
            }

        }

        #region 读取IUPR信息，现已取消
        //private void SetIUPRDataRow(int lineNO, string strItem, int padTotal, int padNum, DataTable dt, List<OBDParameterValue> valueList, int itemIndex, int InfoType) {
        //    double num = 0;
        //    double den = 0;
        //    DataRow dr = dt.NewRow();
        //    dr[0] = lineNO;
        //    foreach (OBDParameterValue value in valueList) {
        //        for (int i = 2; i < dt.Columns.Count; i++) {
        //            if (dt.Columns[i].ColumnName == value.ECUResponseID) {
        //                if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
        //                    if (dr[1].ToString().Length == 0) {
        //                        dr[1] = strItem + ": " + "监测完成次数".PadLeft(padTotal - padNum + 6);
        //                    }
        //                    if (value.ListStringValue.Count > itemIndex) {
        //                        num = Utility.Hex2Int(value.ListStringValue[itemIndex]);
        //                        dr[i] = num.ToString();
        //                    } else {
        //                        num = 0;
        //                        dr[i] = "0";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (dr[1].ToString().Length > 0) {
        //        dt.Rows.Add(dr);
        //    }

        //    dr = dt.NewRow();
        //    foreach (OBDParameterValue value in valueList) {
        //        for (int i = 2; i < dt.Columns.Count; i++) {
        //            if (dt.Columns[i].ColumnName == value.ECUResponseID) {
        //                if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
        //                    if (dr[1].ToString().Length == 0) {
        //                        dr[1] = "符合监测条件次数".PadLeft(padTotal + 8);
        //                    }
        //                    if (value.ListStringValue.Count > itemIndex) {
        //                        den = Utility.Hex2Int(value.ListStringValue[itemIndex + 1]);
        //                        dr[i] = den.ToString();
        //                    } else {
        //                        den = 0;
        //                        dr[i] = "0";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (dr[1].ToString().Length > 0) {
        //        dt.Rows.Add(dr);
        //    }

        //    dr = dt.NewRow();
        //    foreach (OBDParameterValue value in valueList) {
        //        for (int i = 2; i < dt.Columns.Count; i++) {
        //            if (dt.Columns[i].ColumnName == value.ECUResponseID) {
        //                if (m_mode09Support.ContainsKey(dt.Columns[i].ColumnName) && m_mode09Support[dt.Columns[i].ColumnName][InfoType - 1]) {
        //                    if (dr[1].ToString().Length == 0) {
        //                        dr[1] = "IUPR率".PadLeft(padTotal + 5);
        //                    }
        //                    if (den == 0) {
        //                        dr[i] = "7.99527";
        //                    } else {
        //                        double r = Math.Round(num / den, 6);
        //                        if (r > 7.99527) {
        //                            dr[i] = "7.99527";
        //                        } else {
        //                            dr[i] = r.ToString();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (dr[1].ToString().Length > 0) {
        //        dt.Rows.Add(dr);
        //    }
        //}

        //public void SetDataTableIUPR() {
        //    DataTable dt = m_dtIUPR;
        //    int NO = 0;
        //    OBDParameter param;
        //    int HByte = 0;
        //    if (m_obdInterface.UseISO27145) {
        //        param = new OBDParameter {
        //            OBDRequest = "22F80B",
        //            Service = 0x22,
        //            Parameter = 0xF80B,
        //            ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
        //        };
        //        HByte = 0xF800;
        //    } else {
        //        param = new OBDParameter {
        //            OBDRequest = "090B",
        //            Service = 9,
        //            Parameter = 0x0B,
        //            ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
        //        };
        //    }
        //    List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
        //    SetIUPRDataRow(++NO, "NMHC催化器", 18, 12, dt, valueList, 2, param.Parameter);
        //    SetIUPRDataRow(++NO, "NOx催化器", 18, 11, dt, valueList, 4, param.Parameter);
        //    SetIUPRDataRow(++NO, "NOx吸附器", 18, 11, dt, valueList, 6, param.Parameter);
        //    SetIUPRDataRow(++NO, "PM捕集器", 18, 10, dt, valueList, 8, param.Parameter);
        //    SetIUPRDataRow(++NO, "废气传感器", 18, 12, dt, valueList, 10, param.Parameter);
        //    SetIUPRDataRow(++NO, "EGR和VVT", 18, 10, dt, valueList, 12, param.Parameter);
        //    SetIUPRDataRow(++NO, "增压压力", 18, 10, dt, valueList, 14, param.Parameter);

        //    NO = 0;
        //    param.Parameter = HByte + 8;
        //    valueList = m_obdInterface.GetValueList(param);
        //    SetIUPRDataRow(++NO, "催化器 组1", 18, 12, dt, valueList, 2, param.Parameter);
        //    SetIUPRDataRow(++NO, "催化器 组2", 18, 12, dt, valueList, 4, param.Parameter);
        //    SetIUPRDataRow(++NO, "前氧传感器 组1", 18, 16, dt, valueList, 6, param.Parameter);
        //    SetIUPRDataRow(++NO, "前氧传感器 组2", 18, 16, dt, valueList, 8, param.Parameter);
        //    SetIUPRDataRow(++NO, "后氧传感器 组1", 18, 16, dt, valueList, 16, param.Parameter);
        //    SetIUPRDataRow(++NO, "后氧传感器 组2", 18, 16, dt, valueList, 18, param.Parameter);
        //    SetIUPRDataRow(++NO, "EVAP", 18, 6, dt, valueList, 14, param.Parameter);
        //    SetIUPRDataRow(++NO, "EGR和VVT", 18, 10, dt, valueList, 10, param.Parameter);
        //    SetIUPRDataRow(++NO, "GPF 组1", 18, 9, dt, valueList, 24, param.Parameter);
        //    SetIUPRDataRow(++NO, "GPF 组2", 18, 9, dt, valueList, 26, param.Parameter);
        //    SetIUPRDataRow(++NO, "二次空气喷射系统", 18, 18, dt, valueList, 12, param.Parameter);
        //}
        #endregion

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

        /// <summary>
        /// 返回值代表检测数据上传后是否返回成功信息
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>是否返回成功信息</returns>
        public bool StartOBDTest(out string errorMsg) {
            errorMsg = "";
            m_dtInfo.Clear();
            m_dtInfo.Dispose();
            m_dtECUInfo.Clear();
            m_dtECUInfo.Dispose();
            //m_dtIUPR.Clear();
            //m_dtIUPR.Dispose();
            m_mode01Support.Clear();
            m_mode09Support.Clear();
            m_compIgn = false;
            OBDResult = true;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;
            CALIDCVNResult = true;
            SpaceResult = true;

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
                    return false;
                }
            }
            while (!GetSupportStatus(mode09, m_mode09Support)) {
                if (++count > 3) {
                    errorMsg = "连接OBD接口出错！";
                    return false;
                }
            }

            SetDataTableColumns<string>(m_dtInfo, m_mode01Support);
            SetDataTableColumns<string>(m_dtECUInfo, m_mode09Support);
            //SetDataTableColumns<string>(m_dtIUPR, m_mode09Support);
            SetupColumnsDone?.Invoke();
            SetDataTableInfo();
            SetDataTableECUInfo();
            //SetDataTableIUPR();

            OBDResult = DTCResult && ReadinessResult && VINResult && CALIDCVNResult && SpaceResult;

            WriteDbStart?.Invoke();
            string strVIN = "";
            for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                strVIN = m_dtECUInfo.Rows[0][i].ToString();
                if (strVIN.Length > 0 || strVIN != "不适用" || strVIN != "--") {
                    break;
                }
            }
            string strOBDResult = OBDResult ? "1" : "0";

            DataTable dt = new DataTable();
            SetDataTableResultColumns(ref dt);
            try {
                SetDataTableResult(strVIN, strOBDResult, ref dt);
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Result DataTable Error: " + ex.Message);
                dt.Dispose();
                throw new Exception("生成 Result DataTable 出错");
            }

            m_db.ModifyDB("OBDData", dt);
            WriteDbDone?.Invoke();

            try {
                ExportResultFile(dt);
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Exporting OBD test result file failed: " + ex.Message);
                dt.Dispose();
                throw new Exception("生成OBD检测结果文件出错");
            }

            // 用“无条件上传”和“OBD检测结果”判断是否需要直接返回不上传MES
            if (!m_obdInterface.OBDResultSetting.UploadWhenever && !OBDResult) {
                m_obdInterface.m_log.TraceError("Won't upload data because OBD test result is NOK");
                dt.Dispose();
                return true;
            }

            bool bRet;
            try {
                if (m_obdInterface.OracleMESSetting.Enable) {
                    bRet = UploadDataOracle(strVIN, strOBDResult, dt, ref errorMsg);
                } else {
                    bRet = UploadData(strVIN, strOBDResult, dt, ref errorMsg);
                }
            } catch (Exception) {
                dt.Dispose();
                throw;
            }
            dt.Dispose();
            return bRet;
        }

        private void SetDataTable1Oracle(string strVIN, DataTable dt) {
            dt.Columns.Add("ID", typeof(string));                       // 0

            dt.Columns.Add("VEHICLEMODEL", typeof(string));             // 1
            dt.Columns.Add("VIN", typeof(string));                      // 2
            dt.Columns.Add("XXGKBH", typeof(string));                   // 3
            dt.Columns.Add("SB", typeof(string));                       // 4
            dt.Columns.Add("SCCDZ", typeof(string));                    // 5
            dt.Columns.Add("SCDATE", typeof(string));                   // 6
            dt.Columns.Add("FDJH", typeof(string));                     // 7
            dt.Columns.Add("FDJSB", typeof(string));                    // 8
            dt.Columns.Add("FDJSCCDZ", typeof(string));                 // 9
            dt.Columns.Add("SCCMC", typeof(string));                    // 10

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 11
            dt.Columns.Add("CREATOR", typeof(string));                  // 12
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 13
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 14
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 15
            dt.Columns.Add("ISDELETED", typeof(string));                // 16
            dt.Columns.Add("DELETER", typeof(string));                  // 17

            DataRow dr = dt.NewRow();
            dr[2] = strVIN;

            dr[11] = DateTime.Now.ToLocalTime();
            dr[12] = m_obdInterface.UserPreferences.Name;
            dr[16] = "0";
            dt.Rows.Add(dr);
            int iRet = 0;
            try {
                string[] strVals = m_dbOracle.GetValue(dt.TableName, "ID", "VIN", strVIN);
                if (strVals.Length == 0) {
                    iRet = m_dbOracle.InsertRecords(dt.TableName, dt);
                } else {
                    iRet = m_dbOracle.UpdateRecords(dt.TableName, dt, "ID", strVals);
                }
            } catch (Exception) {
                throw;
            }
            if (iRet <= 0) {
                throw new Exception("插入或更新 MES 数据出错，返回的影响行数: " + iRet.ToString());
            }
        }

        private void SetDataTable2Oracle(string strKeyID, string strVIN, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("RH", typeof(string));                       // 2
            dt.Columns.Add("ET", typeof(string));                       // 3
            dt.Columns.Add("AP", typeof(string));                       // 4

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 5
            dt.Columns.Add("CREATOR", typeof(string));                  // 6
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 7
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 8
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 9
            dt.Columns.Add("ISDELETED", typeof(string));                // 10
            dt.Columns.Add("DELETER", typeof(string));                  // 11
        }

        private void SetDataTable3Oracle(string strKeyID, string strOBDResult, DataTable dt) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("TESTTYPE", typeof(string));                 // 2
            dt.Columns.Add("TESTNO", typeof(string));                   // 3
            dt.Columns.Add("TESTDATE", typeof(string));                 // 4
            dt.Columns.Add("APASS", typeof(string));                    // 5
            dt.Columns.Add("OPASS", typeof(string));                    // 6
            dt.Columns.Add("OTESTDATE", typeof(string));                // 7
            dt.Columns.Add("EPASS", typeof(string));                    // 8
            dt.Columns.Add("RESULT", typeof(string));                   // 9

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 10
            dt.Columns.Add("CREATOR", typeof(string));                  // 11
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 12
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 13
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 14
            dt.Columns.Add("ISDELETED", typeof(string));                // 15
            dt.Columns.Add("DELETER", typeof(string));                  // 16

            DataRow dr = dt.NewRow();
            dr[1] = strKeyID;
            dr[2] = "0";
            dr[6] = strOBDResult;
            dr[7] = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            dr[9] = strOBDResult;

            dr[10] = DateTime.Now.ToLocalTime();
            dr[11] = m_obdInterface.UserPreferences.Name;
            dr[15] = "0";
            dt.Rows.Add(dr);
            int iRet;
            try {
                string[] strVals = m_dbOracle.GetValue(dt.TableName, "ID", "WQPF_ID", strKeyID);
                if (strVals.Length == 0) {
                    iRet = m_dbOracle.InsertRecords(dt.TableName, dt);
                } else {
                    iRet = m_dbOracle.UpdateRecords(dt.TableName, dt, "ID", strVals);
                }
            } catch (Exception) {
                throw;
            }
            if (iRet <= 0) {
                throw new Exception("插入或更新 MES 数据出错，返回的影响行数: " + iRet.ToString());
            }
        }

        private void SetDataTable4Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("OBD", typeof(string));                      // 2
            dt.Columns.Add("ODO", typeof(string));                      // 3

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 4
            dt.Columns.Add("CREATOR", typeof(string));                  // 5
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 6
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 7
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 8
            dt.Columns.Add("ISDELETED", typeof(string));                // 9
            dt.Columns.Add("DELETER", typeof(string));                  // 10

            DataRow dr = dt.NewRow();
            dr[1] = strKeyID;
            dr[2] = dtResult.Rows[0]["OBD_SUP"].ToString().Split(',')[0];
            dr[3] = dtResult.Rows[0]["ODO"].ToString().Replace("不适用", "");

            dr[4] = DateTime.Now.ToLocalTime();
            dr[5] = m_obdInterface.UserPreferences.Name;
            dr[9] = "0";
            dt.Rows.Add(dr);
            int iRet;
            try {
                string[] strVals = m_dbOracle.GetValue(dt.TableName, "ID", "WQPF_ID", strKeyID);
                if (strVals.Length == 0) {
                    iRet = m_dbOracle.InsertRecords(dt.TableName, dt);
                } else {
                    iRet = m_dbOracle.UpdateRecords(dt.TableName, dt, "ID", strVals);
                }
            } catch (Exception) {
                throw;
            }
            if (iRet <= 0) {
                throw new Exception("插入或更新 MES 数据出错，返回的影响行数: " + iRet.ToString());
            }
        }

        private void SetDataTable4AOracle(string strKeyID, string strKeyID4, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1
            dt.Columns.Add("WQPF4_ID", typeof(string));                 // 2

            dt.Columns.Add("MODULEID", typeof(string));                 // 3
            dt.Columns.Add("CALID", typeof(string));                    // 4
            dt.Columns.Add("CVN", typeof(string));                      // 5

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 6
            dt.Columns.Add("CREATOR", typeof(string));                  // 7
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 8
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 9
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 10
            dt.Columns.Add("ISDELETED", typeof(string));                // 11
            dt.Columns.Add("DELETER", typeof(string));                  // 12

            string[] CALIDArray = dtResult.Rows[0]["CAL_ID"].ToString().Split(',');
            string[] CVNArray = dtResult.Rows[0]["CVN"].ToString().Split(',');
            for (int i = 0; i < 2; i++) {
                DataRow dr = dt.NewRow();
                dr[1] = strKeyID;
                dr[2] = strKeyID4;
                dr[3] = dtResult.Rows[0]["ECU_ID"];
                dr[4] = CALIDArray.Length > i ? CALIDArray[i] : "-";
                dr[5] = CVNArray.Length > i ? CVNArray[i] : "-";

                dr[6] = DateTime.Now.ToLocalTime();
                dr[7] = m_obdInterface.UserPreferences.Name;
                dr[11] = "0";
                dt.Rows.Add(dr);
            }

            for (int iRow = 1; iRow < dtResult.Rows.Count; iRow++) {
                CALIDArray = dtResult.Rows[iRow]["CAL_ID"].ToString().Split(',');
                CVNArray = dtResult.Rows[iRow]["CVN"].ToString().Split(',');
                for (int i = 0; i < CALIDArray.Length; i++) {
                    DataRow dr = dt.NewRow();
                    dr[1] = strKeyID;
                    dr[2] = strKeyID4;
                    dr[3] = dtResult.Rows[iRow]["ECU_ID"];
                    dr[4] = CALIDArray[i];
                    dr[5] = CVNArray.Length > i ? CVNArray[i] : "";

                    dr[6] = DateTime.Now.ToLocalTime();
                    dr[7] = m_obdInterface.UserPreferences.Name;
                    dr[11] = "0";
                    dt.Rows.Add(dr);
                }
            }
            int iRet;
            try {
                string[] strVals = m_dbOracle.GetValue(dt.TableName, "ID", "WQPF_ID", strKeyID);
                if (strVals.Length == 0) {
                    iRet = m_dbOracle.InsertRecords(dt.TableName, dt);
                } else {
                    iRet = m_dbOracle.UpdateRecords(dt.TableName, dt, "ID", strVals);
                }
            } catch (Exception) {
                throw;
            }
            if (iRet <= 0) {
                throw new Exception("插入或更新 MES 数据出错，返回的影响行数: " + iRet.ToString());
            }
        }

        private void SetDataTable51Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("REAC", typeof(string));                     // 2
            dt.Columns.Add("LEACMAX", typeof(string));                  // 3
            dt.Columns.Add("LEACMIN", typeof(string));                  // 4
            dt.Columns.Add("LRCO", typeof(string));                     // 5
            dt.Columns.Add("LLCO", typeof(string));                     // 6
            dt.Columns.Add("LRHC", typeof(string));                     // 7
            dt.Columns.Add("LLHC", typeof(string));                     // 8
            dt.Columns.Add("HRCO", typeof(string));                     // 9
            dt.Columns.Add("HLCO", typeof(string));                     // 10
            dt.Columns.Add("HRHC", typeof(string));                     // 11
            dt.Columns.Add("HLHC", typeof(string));                     // 12

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 13
            dt.Columns.Add("CREATOR", typeof(string));                  // 14
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 15
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 16
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 17
            dt.Columns.Add("ISDELETED", typeof(string));                // 18
            dt.Columns.Add("DELETER", typeof(string));                  // 19
        }

        private void SetDataTable52Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("ARHC5025", typeof(string));                 // 2
            dt.Columns.Add("ALHC5025", typeof(string));                 // 3
            dt.Columns.Add("ARCO5025", typeof(string));                 // 4
            dt.Columns.Add("ALCO5025", typeof(string));                 // 5
            dt.Columns.Add("ARNOX5025", typeof(string));                // 6
            dt.Columns.Add("ALNOX5025", typeof(string));                // 7
            dt.Columns.Add("ARHC2540", typeof(string));                 // 8
            dt.Columns.Add("ALHC2540", typeof(string));                 // 9
            dt.Columns.Add("ARCO2540", typeof(string));                 // 10
            dt.Columns.Add("ALCO2540", typeof(string));                 // 11
            dt.Columns.Add("ARNOX2540", typeof(string));                // 12
            dt.Columns.Add("ALNOX2540", typeof(string));                // 13

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 14
            dt.Columns.Add("CREATOR", typeof(string));                  // 15
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 16
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 17
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 18
            dt.Columns.Add("ISDELETED", typeof(string));                // 19
            dt.Columns.Add("DELETER", typeof(string));                  // 20
        }

        private void SetDataTable53Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("VRHC", typeof(string));                     // 2
            dt.Columns.Add("VLHC", typeof(string));                     // 3
            dt.Columns.Add("VRCO", typeof(string));                     // 4
            dt.Columns.Add("VLCO", typeof(string));                     // 5
            dt.Columns.Add("VRNOX", typeof(string));                    // 6
            dt.Columns.Add("VLNOX", typeof(string));                    // 7

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 8
            dt.Columns.Add("CREATOR", typeof(string));                  // 9
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 10
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 11
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 12
            dt.Columns.Add("ISDELETED", typeof(string));                // 13
            dt.Columns.Add("DELETER", typeof(string));                  // 14
        }

        private void SetDataTable54Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("RATEREVUP", typeof(string));                // 2
            dt.Columns.Add("RATEREVDOWN", typeof(string));              // 3
            dt.Columns.Add("REV100", typeof(string));                   // 4
            dt.Columns.Add("MAXPOWER", typeof(string));                 // 5
            dt.Columns.Add("MAXPOWERLIMIT", typeof(string));            // 6
            dt.Columns.Add("SMOKE100", typeof(string));                 // 7
            dt.Columns.Add("SMOKE80", typeof(string));                  // 8
            dt.Columns.Add("SMOKELIMIT", typeof(string));               // 9
            dt.Columns.Add("NOX", typeof(string));                      // 10
            dt.Columns.Add("NOXLIMIT", typeof(string));                 // 11

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 12
            dt.Columns.Add("CREATOR", typeof(string));                  // 13
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 14
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 15
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 16
            dt.Columns.Add("ISDELETED", typeof(string));                // 17
            dt.Columns.Add("DELETER", typeof(string));                  // 18
        }

        private void SetDataTable55Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("RATEREV", typeof(string));                  // 2
            dt.Columns.Add("REV", typeof(string));                      // 3
            dt.Columns.Add("SMOKEK1", typeof(string));                  // 4
            dt.Columns.Add("SMOKEK2", typeof(string));                  // 5
            dt.Columns.Add("SMOKEK3", typeof(string));                  // 6
            dt.Columns.Add("SMOKEAVG", typeof(string));                 // 7
            dt.Columns.Add("SMOKEKLIMIT", typeof(string));              // 8

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 9
            dt.Columns.Add("CREATOR", typeof(string));                  // 10
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 11
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 12
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 13
            dt.Columns.Add("ISDELETED", typeof(string));                // 14
            dt.Columns.Add("DELETER", typeof(string));                  // 15
        }

        private void SetDataTable56Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("VRCO", typeof(string));                     // 2
            dt.Columns.Add("VLCO", typeof(string));                     // 3
            dt.Columns.Add("VRHCNOX", typeof(string));                  // 4
            dt.Columns.Add("VLHCNOX", typeof(string));                  // 5

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 6
            dt.Columns.Add("CREATOR", typeof(string));                  // 7
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 8
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 9
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 10
            dt.Columns.Add("ISDELETED", typeof(string));                // 11
            dt.Columns.Add("DELETER", typeof(string));                  // 12
        }

        private void SetDataTable6Oracle(string strKeyID, DataTable dt, DataTable dtResult) {
            dt.Columns.Add("ID", typeof(string));                       // 0
            dt.Columns.Add("WQPF_ID", typeof(string));                  // 1

            dt.Columns.Add("ANALYMANUF", typeof(string));               // 2
            dt.Columns.Add("ANALYNAME", typeof(string));                // 3
            dt.Columns.Add("ANALYMODEL", typeof(string));               // 4
            dt.Columns.Add("ANALYDATE", typeof(string));                // 5
            dt.Columns.Add("DYNOMODEL", typeof(string));                // 6
            dt.Columns.Add("DYNOMANUF", typeof(string));                // 7

            dt.Columns.Add("CREATIONTIME", typeof(DateTime));           // 8
            dt.Columns.Add("CREATOR", typeof(string));                  // 9
            dt.Columns.Add("LASTMODIFICATIONTIME", typeof(DateTime));   // 10
            dt.Columns.Add("LASTMODIFIER", typeof(string));             // 11
            dt.Columns.Add("DELETIONTIME", typeof(DateTime));           // 12
            dt.Columns.Add("ISDELETED", typeof(string));                // 13
            dt.Columns.Add("DELETER", typeof(string));                  // 14
        }

        private bool UploadDataOracle(string strVIN, string strOBDResult, DataTable dtIn, ref string errorMsg) {
            DataTable dt1 = new DataTable("IF_EM_WQPF_1");
            //DataTable dt2 = new DataTable("IF_EM_WQPF_2");
            DataTable dt3 = new DataTable("IF_EM_WQPF_3");
            DataTable dt4 = new DataTable("IF_EM_WQPF_4");
            DataTable dt4A = new DataTable("IF_EM_WQPF_4_A");
            //DataTable dt51 = new DataTable("IF_EM_WQPF_5_1");
            //DataTable dt52 = new DataTable("IF_EM_WQPF_5_2");
            //DataTable dt53 = new DataTable("IF_EM_WQPF_5_3");
            //DataTable dt54 = new DataTable("IF_EM_WQPF_5_4");
            //DataTable dt55 = new DataTable("IF_EM_WQPF_5_5");
            //DataTable dt56 = new DataTable("IF_EM_WQPF_5_6");
            //DataTable dt6 = new DataTable("IF_EM_WQPF_6");
            try {
                UploadDataStart?.Invoke();
                m_obdInterface.DBandMES.ChangeWebService = false;
                SetDataTable1Oracle(strVIN, dt1);
                string strKeyID = m_dbOracle.GetValue(dt1.TableName, "ID", "VIN", strVIN)[0];
                //SetDataTable2Oracle(strKeyID, strVIN, dt2, dtIn);
                SetDataTable3Oracle(strKeyID, strOBDResult, dt3);
                SetDataTable4Oracle(strKeyID, dt4, dtIn);
                string strKeyID4 = m_dbOracle.GetValue(dt4.TableName, "ID", "WQPF_ID", strKeyID)[0];
                SetDataTable4AOracle(strKeyID, strKeyID4, dt4A, dtIn);
                //SetDataTable51Oracle(strKeyID, dt51, dtIn);
                //SetDataTable52Oracle(strKeyID, dt52, dtIn);
                //SetDataTable53Oracle(strKeyID, dt53, dtIn);
                //SetDataTable54Oracle(strKeyID, dt54, dtIn);
                //SetDataTable55Oracle(strKeyID, dt55, dtIn);
                //SetDataTable56Oracle(strKeyID, dt56, dtIn);
                //SetDataTable6Oracle(strKeyID, dt6, dtIn);
                m_db.UpdateUpload(strVIN, "1");
                UploadDataDone?.Invoke();
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("UploadDataOracle Error: " + ex.Message);
                throw;
            } finally {
                dt1.Dispose();
                dt3.Dispose();
                dt4.Dispose();
                dt4A.Dispose();
            }
            return true;
        }

        private bool UploadData(string strVIN, string strOBDResult, DataTable dtIn, ref string errorMsg) {
            // 上传MES接口
            UploadDataStart?.Invoke();
            m_obdInterface.DBandMES.ChangeWebService = false;
            if (!WSHelper.CreateWebService(m_obdInterface.DBandMES, out string error)) {
                m_obdInterface.m_log.TraceError("CreateWebService Error: " + error);
                throw new Exception("获取 WebService 接口出错");
            }

            // DataTable必须设置TableName，否则调用方法时会报错“生成 XML 文档时出错”
            DataTable dt1MES = new DataTable("dt1");
            SetDataTable1MES(dt1MES, strVIN, strOBDResult);

            DataTable dt2MES = new DataTable("dt2");
            SetDataTable2MES(dt2MES, dtIn);

            string strMsg;
            string strRet;
            try {
                strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("WebService GetResponseString error: " + ex.Message);
                dt2MES.Dispose();
                dt1MES.Dispose();
                throw;
            }

            int count = 0;
            while (strRet.Contains("NOK") || strRet.Contains("false") || strRet.Contains("False")) {
                if (++count < 4) {
                    m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                    try {
                        strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
                    } catch (Exception ex) {
                        m_obdInterface.m_log.TraceError("WebService GetResponseString error: " + ex.Message);
                        dt2MES.Dispose();
                        dt1MES.Dispose();
                        throw;
                    }
                } else {
                    m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                    errorMsg = "Upload data function return false: " + strMsg;
                    break;
                }
            }
            dt2MES.Dispose();
            dt1MES.Dispose();
            if (count < 4) {
                // 上传数据接口返回成功信息
                m_db.UpdateUpload(strVIN, "1");
                // 保存m_iSN的值，以免程序非正常退出的话，该值就丢失了
                m_obdInterface.SaveDBandMES(m_obdInterface.DBandMES);
                UploadDataDone?.Invoke();
#if DEBUG
                errorMsg = strMsg;
#endif
                return true;
            } else {
                // 上传数据接口返回失败信息
                m_obdInterface.m_log.TraceError("Upload data function return false, VIN = " + strVIN);
                return false;
            }
        }

        private void SetDataTableResultColumns(ref DataTable dtOut) {
            dtOut.Columns.Add("VIN", typeof(string));       // 0
            dtOut.Columns.Add("ECU_ID", typeof(string));    // 1
            dtOut.Columns.Add("MIL", typeof(string));       // 2
            dtOut.Columns.Add("MIL_DIST", typeof(string));  // 3
            dtOut.Columns.Add("OBD_SUP", typeof(string));   // 4
            dtOut.Columns.Add("ODO", typeof(string));       // 5
            dtOut.Columns.Add("DTC03", typeof(string));     // 6
            dtOut.Columns.Add("DTC07", typeof(string));     // 7
            dtOut.Columns.Add("DTC0A", typeof(string));     // 8
            dtOut.Columns.Add("MIS_RDY", typeof(string));   // 9
            dtOut.Columns.Add("FUEL_RDY", typeof(string));  // 10
            dtOut.Columns.Add("CCM_RDY", typeof(string));   // 11
            dtOut.Columns.Add("CAT_RDY", typeof(string));   // 12
            dtOut.Columns.Add("HCAT_RDY", typeof(string));  // 13
            dtOut.Columns.Add("EVAP_RDY", typeof(string));  // 14
            dtOut.Columns.Add("AIR_RDY", typeof(string));   // 15
            dtOut.Columns.Add("ACRF_RDY", typeof(string));  // 16
            dtOut.Columns.Add("O2S_RDY", typeof(string));   // 17
            dtOut.Columns.Add("HTR_RDY", typeof(string));   // 18
            dtOut.Columns.Add("EGR_RDY", typeof(string));   // 19
            dtOut.Columns.Add("HCCAT_RDY", typeof(string)); // 20
            dtOut.Columns.Add("NCAT_RDY", typeof(string));  // 21
            dtOut.Columns.Add("BP_RDY", typeof(string));    // 22
            dtOut.Columns.Add("EGS_RDY", typeof(string));   // 23
            dtOut.Columns.Add("PM_RDY", typeof(string));    // 24
            dtOut.Columns.Add("ECU_NAME", typeof(string));  // 25
            dtOut.Columns.Add("CAL_ID", typeof(string));    // 26
            dtOut.Columns.Add("CVN", typeof(string));       // 27
            dtOut.Columns.Add("Result", typeof(string));    // 28
            dtOut.Columns.Add("Upload", typeof(string));    // 29
        }

        private void SetDataTableResult(string strVIN, string strOBDResult, ref DataTable dtOut) {
            for (int i = 2; i < m_dtECUInfo.Columns.Count; i++) {
                DataRow dr = dtOut.NewRow();
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
                dr[28] = strOBDResult;
                dr[29] = "0";
                dtOut.Rows.Add(dr);
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

            string strNowDateTime = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            DataRow dr = dt1MES.NewRow();
            dr[1] = "OBD";
            dr[7] = strVIN;
            ++m_iSN;
            dr[8] = "XC0079" + strNowDateTime + m_iSN.ToString("d4");
            m_obdInterface.DBandMES.DateSN = strNowDateTime + "," + m_iSN.ToString();
            dr[13] = strOBDResult;
            dr[15] = strOBDResult;
            dr[163] = strNowDateTime;
            dt1MES.Rows.Add(dr);
        }

        private string GetModuleID(string ECUAcronym, string ECUID) {
            string moduleID = ECUAcronym;
            if (m_obdInterface.OBDResultSetting.UseECUAcronym) {
                if (moduleID.Length == 0 || moduleID == "不适用") {
                    moduleID = ECUID;
                }
            } else {
                moduleID = ECUID;
            }
            return moduleID;
        }

        private void DataTable2MESAddRow(DataTable dt2MES, DataTable dtIn, int iRow, string ECUAcronym, string CALID, string CVN) {
            string moduleID = GetModuleID(ECUAcronym, dtIn.Rows[iRow][1].ToString());
            dt2MES.Rows.Add(
                dtIn.Rows[iRow][4].ToString().Split(',')[0],
                dtIn.Rows[iRow][5].ToString().Replace("不适用", ""),
                moduleID,
                CALID,
                CVN
            );
        }

        private void SetDataTable2MES(DataTable dt2MES, DataTable dtIn) {
            dt2MES.Columns.Add("obd");
            dt2MES.Columns.Add("odo");
            dt2MES.Columns.Add("ModuleID");
            dt2MES.Columns.Add("CALID");
            dt2MES.Columns.Add("CVN");
            for (int i = 0; i < dtIn.Rows.Count; i++) {
                string[] CALIDArray = dtIn.Rows[i][26].ToString().Split(',');
                string[] CVNArray = dtIn.Rows[i][27].ToString().Split(',');
                int length = Math.Max(CALIDArray.Length, CVNArray.Length);
                string ECUAcronym = dtIn.Rows[i][25].ToString().Split('-')[0];
                DataTable2MESAddRow(dt2MES, dtIn, i, ECUAcronym, CALIDArray[0], CVNArray[0]);
                for (int j = 1; j < length; j++) {
                    string CALID = CALIDArray.Length > j ? CALIDArray[j] : "";
                    string CVN = CVNArray.Length > j ? CVNArray[j] : "";
                    // 若同一个ECU下有多个CALID和CVN的上传策略
                    if (m_obdInterface.OBDResultSetting.UseSCRName) {
                        //第二个CALID和CVN使用“SCR”作为ModuleID上传
                        if (j == 1) {
                            DataTable2MESAddRow(dt2MES, dtIn, i, "SCR", CALID, CVN);
                        } else {
                            DataTable2MESAddRow(dt2MES, dtIn, i, ECUAcronym, CALID, CVN);
                        }
                    } else {
                        // 多个CALID和CVN使用同一个ModuleID上传
                        DataTable2MESAddRow(dt2MES, dtIn, i, ECUAcronym, CALID, CVN);
                    }
                }
            }
        }

        private void SetDataTableColumnsFromDB(DataTable dtDisplay, DataTable dtIn) {
            dtDisplay.Clear();
            dtDisplay.Columns.Clear();
            if (dtIn.Rows.Count > 0) {
                dtDisplay.Columns.Add(new DataColumn("NO", typeof(int)));
                dtDisplay.Columns.Add(new DataColumn("Item", typeof(string)));
                for (int i = 0; i < dtIn.Rows.Count; i++) {
                    dtDisplay.Columns.Add(new DataColumn(dtIn.Rows[i][1].ToString(), typeof(string)));
                }
            } else {
                SetDataTableColumnsErrorEventArgs args = new SetDataTableColumnsErrorEventArgs {
                    ErrorMsg = "从数据库中未获取到数据"
                };
                SetDataTableColumnsError?.Invoke(this, args);
            }
        }

        private void SetDataRowInfoFromDB(int lineNO, string strItem, DataTable dtIn) {
            DataRow dr = m_dtInfo.NewRow();
            dr[0] = lineNO;
            dr[1] = strItem;
            for (int i = 0; i < dtIn.Rows.Count; i++) {
                if (GetCompIgn(dtIn) && lineNO > 10) {
                    dr[i + 2] = dtIn.Rows[i][lineNO + (lineNO == 16 ? 3 : 9)];
                } else {
                    dr[i + 2] = dtIn.Rows[i][lineNO + 1];
                }
            }
            m_dtInfo.Rows.Add(dr);
        }

        private void SetDataRowECUInfoFromDB(int lineNO, string strItem, DataTable dtIn) {
            DataRow dr = m_dtECUInfo.NewRow();
            dr[0] = lineNO;
            dr[1] = strItem;
            for (int i = 0; i < dtIn.Rows.Count; i++) {
                if (lineNO == 1) {
                    dr[i + 2] = dtIn.Rows[i][lineNO - 1];
                } else {
                    dr[i + 2] = dtIn.Rows[i][lineNO + 23].ToString().Replace(",", "\n");
                }
            }
            m_dtECUInfo.Rows.Add(dr);
        }

        private void SetDataTableInfoFromDB(DataTable dtIn) {
            if (m_dtInfo.Columns.Count <= 0) {
                return;
            }
            int NO = 0;
            SetDataRowInfoFromDB(++NO, "MIL状态", dtIn);                // 0
            SetDataRowInfoFromDB(++NO, "MIL亮后行驶里程（km）", dtIn);   // 1
            SetDataRowInfoFromDB(++NO, "OBD型式检验类型", dtIn);         // 2
            SetDataRowInfoFromDB(++NO, "总累积里程ODO（km）", dtIn);     // 3
            SetDataRowInfoFromDB(++NO, "存储DTC", dtIn);                // 4
            SetDataRowInfoFromDB(++NO, "未决DTC", dtIn);                // 5
            SetDataRowInfoFromDB(++NO, "永久DTC", dtIn);                // 6
            SetDataRowInfoFromDB(++NO, "失火监测", dtIn);               // 7
            SetDataRowInfoFromDB(++NO, "燃油系统监测", dtIn);           // 8
            SetDataRowInfoFromDB(++NO, "综合组件监测", dtIn);           // 9
            if (GetCompIgn(dtIn)) {
                SetDataRowInfoFromDB(++NO, "NMHC催化剂监测", dtIn);     // 10
                SetDataRowInfoFromDB(++NO, "NOx/SCR后处理监测", dtIn);  // 11
                SetDataRowInfoFromDB(++NO, "增压系统监测", dtIn);       // 12
                SetDataRowInfoFromDB(++NO, "排气传感器监测", dtIn);     // 13
                SetDataRowInfoFromDB(++NO, "PM过滤器监测", dtIn);       // 14
            } else {
                SetDataRowInfoFromDB(++NO, "催化剂监测", dtIn);         // 10
                SetDataRowInfoFromDB(++NO, "加热催化剂监测", dtIn);     // 11
                SetDataRowInfoFromDB(++NO, "燃油蒸发系统监测", dtIn);   // 12
                SetDataRowInfoFromDB(++NO, "二次空气系统监测", dtIn);   // 13
                SetDataRowInfoFromDB(++NO, "空调系统制冷剂监测", dtIn); // 14
                SetDataRowInfoFromDB(++NO, "氧气传感器监测", dtIn);     // 15
                SetDataRowInfoFromDB(++NO, "加热氧气传感器监测", dtIn); // 16
            }
            SetDataRowInfoFromDB(++NO, "EGR/VVT系统监测", dtIn);       // 15 / 17
        }

        private void SetDataTableECUInfoFromDB(DataTable dtIn) {
            if (m_dtInfo.Columns.Count <= 0) {
                return;
            }
            int NO = 0;
            SetDataRowECUInfoFromDB(++NO, "VIN", dtIn);               // 0
            SetDataRowECUInfoFromDB(++NO, "ECU名称", dtIn);           // 1
            SetDataRowECUInfoFromDB(++NO, "CAL_ID", dtIn);            // 2
            SetDataRowECUInfoFromDB(++NO, "CVN", dtIn);               // 3
        }

        public bool UploadDataFromDB(string strVIN, out string errorMsg) {
            errorMsg = "";
            DataTable dt = new DataTable();
            SetDataTableResultColumns(ref dt);

            Dictionary<string, int> ColsDic = m_db.GetTableColumnsDic("OBDData");
            Dictionary<string, string> VINDic = new Dictionary<string, string> { { "VIN", strVIN } };
            string[,] Results = m_db.GetRecords("OBDData", VINDic);

            SetDataTableResultFromDB(ColsDic, Results, dt);
            SetDataTableColumnsFromDB(m_dtInfo, dt);
            SetDataTableColumnsFromDB(m_dtECUInfo, dt);
            SetupColumnsDone?.Invoke();
            SetDataTableInfoFromDB(dt);
            SetDataTableECUInfoFromDB(dt);
            bool bRet = false;
            if (!m_obdInterface.OBDResultSetting.UploadWhenever && dt.Rows[0]["Result"].ToString() != "1") {
                m_obdInterface.m_log.TraceError("Won't upload data from database because OBD test result is NOK");
                NotUploadData?.Invoke();
                dt.Dispose();
                return bRet;
            }
            if (dt.Rows.Count > 0) {
                try {
                    if (m_obdInterface.OracleMESSetting.Enable) {
                        bRet = UploadDataOracle(strVIN, dt.Rows[0][28].ToString(), dt, ref errorMsg);
                    } else {
                        bRet = UploadData(strVIN, dt.Rows[0][28].ToString(), dt, ref errorMsg);
                    }
                } catch (Exception) {
                    m_obdInterface.m_log.TraceError("Manual Upload Data Faiure！");
                    dt.Dispose();
                    throw;
                }
            }
            dt.Dispose();
            return bRet;
        }

        private List<string[,]> SplitResultsPerVIN(Dictionary<string, int> ColsDic, string[,] Results) {
            int iRowCount = Results.GetLength(0);
            int iColCount = Results.GetLength(1);
            string[] row;
            string[,] total;
            List<string[]> rowList = new List<string[]>();
            List<string[,]> totalList = new List<string[,]>();
            string VIN = Results[0, ColsDic["VIN"]];
            for (int iRow = 0; iRow < iRowCount; iRow++) {
                row = new string[iColCount];
                for (int iCol = 0; iCol < iColCount; iCol++) {
                    row[iCol] = Results[iRow, iCol];
                }
                if (Results[iRow, ColsDic["VIN"]] != VIN) {
                    total = new string[rowList.Count, iColCount];
                    for (int m = 0; m < rowList.Count; m++) {
                        for (int n = 0; n < iColCount; n++) {
                            total[m, n] = rowList[m][n];
                        }
                    }
                    totalList.Add(total);
                    rowList.Clear();
                    rowList.Add(row);
                    VIN = Results[iRow, ColsDic["VIN"]];
                } else {
                    rowList.Add(row);
                }
            }
            total = new string[rowList.Count, iColCount];
            for (int m = 0; m < rowList.Count; m++) {
                for (int n = 0; n < iColCount; n++) {
                    total[m, n] = rowList[m][n];
                }
            }
            totalList.Add(total);
            return totalList;
        }

        public bool UploadDataFromDBOnTime(out string errorMsg) {
            errorMsg = "";
            bool bRet = false;
            DataTable dt = new DataTable();
            SetDataTableResultColumns(ref dt);

            Dictionary<string, int> ColsDic = m_db.GetTableColumnsDic("OBDData");
            Dictionary<string, string> dic = new Dictionary<string, string> { { "Upload", "0" } };
            string[,] Results = m_db.GetRecords("OBDData", dic);
            if (Results.GetLength(0) == 0) {
                dt.Dispose();
                return bRet;
            }
            List<string[,]> ResultsList = SplitResultsPerVIN(ColsDic, Results);
            for (int i = 0; i < ResultsList.Count; i++) {
                SetDataTableResultFromDB(ColsDic, ResultsList[i], dt);
                if (!m_obdInterface.OBDResultSetting.UploadWhenever && dt.Rows[0]["Result"].ToString() != "1") {
                    continue;
                }
                try {
                    if (m_obdInterface.OracleMESSetting.Enable) {
                        bRet = UploadDataOracle(dt.Rows[0][0].ToString(), dt.Rows[0][28].ToString(), dt, ref errorMsg);
                    } else {
                        bRet = UploadData(dt.Rows[0][0].ToString(), dt.Rows[0][28].ToString(), dt, ref errorMsg);
                    }
                } catch (Exception) {
                    m_obdInterface.m_log.TraceError("Upload Data OnTime Faiure！");
                    dt.Dispose();
                    throw;
                }
            }
            dt.Dispose();
            return bRet;
        }

        private bool GetCompIgn(DataTable dtIn) {
            bool compIgn = true;
            compIgn = compIgn && dtIn.Rows[0][12].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][13].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][14].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][15].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][16].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][17].ToString() == "不适用";
            compIgn = compIgn && dtIn.Rows[0][18].ToString() == "不适用";
            return compIgn;
        }

        private bool GetCompIgn(Dictionary<string, int> ColsDic, string[,] Results, int iRow) {
            bool compIgn = true;
            compIgn = compIgn && Results[iRow, ColsDic["CAT_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["HCAT_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["EVAP_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["AIR_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["ACRF_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["O2S_RDY"]] == "不适用";
            compIgn = compIgn && Results[iRow, ColsDic["HTR_RDY"]] == "不适用";
            return compIgn;
        }

        private void SetDataTableResultFromDB(Dictionary<string, int> ColsDic, string[,] Results, DataTable dtOut) {
            dtOut.Clear();
            if (ColsDic.Count > 0 && Results != null) {
                int rowCount = Results.GetLength(0);
                for (int i = 0; i < rowCount; i++) {
                    DataRow dr = dtOut.NewRow();
                    dr[0] = Results[i, ColsDic["VIN"]];
                    dr[1] = Results[i, ColsDic["ECU_ID"]];
                    dr[2] = Results[i, ColsDic["MIL"]];
                    dr[3] = Results[i, ColsDic["MIL_DIST"]];
                    dr[4] = Results[i, ColsDic["OBD_SUP"]];
                    dr[5] = Results[i, ColsDic["ODO"]];
                    dr[6] = Results[i, ColsDic["DTC03"]];
                    dr[7] = Results[i, ColsDic["DTC07"]];
                    dr[8] = Results[i, ColsDic["DTC0A"]];
                    dr[9] = Results[i, ColsDic["MIS_RDY"]];
                    dr[10] = Results[i, ColsDic["FUEL_RDY"]];
                    dr[11] = Results[i, ColsDic["CCM_RDY"]];
                    dr[12] = Results[i, ColsDic["CAT_RDY"]];
                    dr[13] = Results[i, ColsDic["HCAT_RDY"]];
                    dr[14] = Results[i, ColsDic["EVAP_RDY"]];
                    dr[15] = Results[i, ColsDic["AIR_RDY"]];
                    dr[16] = Results[i, ColsDic["ACRF_RDY"]];
                    dr[17] = Results[i, ColsDic["O2S_RDY"]];
                    dr[18] = Results[i, ColsDic["HTR_RDY"]];
                    dr[19] = Results[i, ColsDic["EGR_RDY"]];
                    dr[20] = Results[i, ColsDic["HCCAT_RDY"]];
                    dr[21] = Results[i, ColsDic["NCAT_RDY"]];
                    dr[22] = Results[i, ColsDic["BP_RDY"]];
                    dr[23] = Results[i, ColsDic["EGS_RDY"]];
                    dr[24] = Results[i, ColsDic["PM_RDY"]];
                    dr[25] = Results[i, ColsDic["ECU_NAME"]];
                    dr[26] = Results[i, ColsDic["CAL_ID"]];
                    dr[27] = Results[i, ColsDic["CVN"]];
                    dr[28] = Results[i, ColsDic["Result"]];
                    dr[29] = Results[i, ColsDic["Upload"]];

                    //if (GetCompIgn(ColsDic, Results, i)) {
                    //    dr[12] = "不适用";         // CAT_RDY
                    //    dr[13] = "不适用";         // HCAT_RDY
                    //    dr[14] = "不适用";         // EVAP_RDY
                    //    dr[15] = "不适用";         // AIR_RDY
                    //    dr[16] = "不适用";         // ACRF_RDY
                    //    dr[17] = "不适用";         // O2S_RDY
                    //    dr[18] = "不适用";         // HTR_RDY
                    //    dr[19] = Results[i, ColsDic["EGR_DRY"]];
                    //    dr[20] = Results[i, ColsDic["HCCAT_DRY"]];
                    //    dr[21] = Results[i, ColsDic["NCAT_DRY"]];
                    //    dr[22] = Results[i, ColsDic["BP_DRY"]];
                    //    dr[23] = Results[i, ColsDic["EGS_DRY"]];
                    //    dr[24] = Results[i, ColsDic["PM_DRY"]];
                    //} else {
                    //    dr[12] = Results[i, ColsDic["CAT_DRY"]];
                    //    dr[13] = Results[i, ColsDic["HCAT_DRY"]];
                    //    dr[14] = Results[i, ColsDic["EVAP_DRY"]];
                    //    dr[15] = Results[i, ColsDic["AIR_DRY"]];
                    //    dr[16] = Results[i, ColsDic["ACRF_DRY"]];
                    //    dr[17] = Results[i, ColsDic["O2S_DRY"]];
                    //    dr[18] = Results[i, ColsDic["HTR_DRY"]];
                    //    dr[19] = Results[i, ColsDic["EGR_DRY"]];
                    //    dr[20] = "不适用";         // HCCAT_RDY
                    //    dr[21] = "不适用";         // NCAT_RDY
                    //    dr[22] = "不适用";         // BP_RDY
                    //    dr[23] = "不适用";         // EGS_RDY
                    //    dr[24] = "不适用";         // PM_RDY
                    //}

                    dtOut.Rows.Add(dr);
                }
            }
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

                // moduleID
                string moduleID = GetModuleID(dt.Rows[0][25].ToString().Split('-')[0], dt.Rows[0][1].ToString());
                worksheet1.Cells["E3"].Value = moduleID;
                if (worksheet1.Cells["B4"].Value.ToString().Length > 0) {
                    if (m_obdInterface.OBDResultSetting.UseECUAcronym) {
                        if (m_obdInterface.OBDResultSetting.UseSCRName) {
                            worksheet1.Cells["E4"].Value = "SCR";
                        } else {
                            worksheet1.Cells["E4"].Value = moduleID;
                        }
                    } else {
                        worksheet1.Cells["E4"].Value = moduleID;
                    }
                }
                string OtherID = "";
                for (int i = 1; i < dt.Rows.Count; i++) {
                    moduleID = GetModuleID(dt.Rows[i][25].ToString().Split('-')[0], dt.Rows[i][1].ToString());
                    OtherID += "," + moduleID;
                }
                worksheet1.Cells["E5"].Value = OtherID.Trim(',');

                // 与OBD诊断仪通讯情况
                worksheet1.Cells["B7"].Value = "通讯成功";

                #region DTC和就绪状态，已取消
                //for (int i = 0; i < dt.Rows.Count; i++) {
                //    // 故障代码及故障信息
                //    string DTC = dt.Rows[i][6].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                //    if (m_obdInterface.OBDResultSetting.DTC03 && DTC.Length > 0) {
                //        worksheet1.Cells[10, 3 + i].Value = DTC;
                //    }
                //    DTC = dt.Rows[i][7].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                //    if (m_obdInterface.OBDResultSetting.DTC07 && worksheet1.Cells[10, 3 + i].Value != null && DTC.Length > 0) {
                //        worksheet1.Cells[10, 3 + i].Value += "\n";
                //    }
                //    if (m_obdInterface.OBDResultSetting.DTC07 && DTC.Length > 0) {
                //        worksheet1.Cells[10, 3 + i].Value += DTC;
                //    }
                //    DTC = dt.Rows[i][8].ToString().Replace(",", "\n").Replace("--", "").Replace("不适用", "");
                //    if (m_obdInterface.OBDResultSetting.DTC0A && worksheet1.Cells[10, 3 + i].Value != null && DTC.Length > 0) {
                //        worksheet1.Cells[10, 3 + i].Value += "\n";
                //    }
                //    if (m_obdInterface.OBDResultSetting.DTC0A && DTC.Length > 0) {
                //        worksheet1.Cells[10, 3 + i].Value += DTC;
                //    }

                //    // 诊断就绪状态未完成项目
                //    if (m_obdInterface.OBDResultSetting.Readiness) {
                //        string readiness = dt.Rows[i][9].ToString();
                //        if (readiness == "未完成") {
                //            worksheet1.Cells[11, 3 + i].Value = "失火";
                //        }
                //        readiness = dt.Rows[i][10].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "燃油系统";
                //        }
                //        readiness = dt.Rows[i][11].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "综合组件";
                //        }
                //        readiness = dt.Rows[i][12].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "催化剂";
                //        }
                //        readiness = dt.Rows[i][13].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "加热催化剂";
                //        }
                //        readiness = dt.Rows[i][14].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "燃油蒸发系统";
                //        }
                //        readiness = dt.Rows[i][15].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "二次空气系统";
                //        }
                //        readiness = dt.Rows[i][16].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "空调系统制冷剂";
                //        }
                //        readiness = dt.Rows[i][17].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "氧气传感器";
                //        }
                //        readiness = dt.Rows[i][18].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "加热氧气传感器";
                //        }
                //        readiness = dt.Rows[i][19].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "EGR/VVT系统监测";
                //        }
                //        readiness = dt.Rows[i][20].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "NMHC催化剂";
                //        }
                //        readiness = dt.Rows[i][21].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "NOx/SCR后处理";
                //        }
                //        readiness = dt.Rows[i][22].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "增压系统";
                //        }
                //        readiness = dt.Rows[i][23].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "废气传感器";
                //        }
                //        readiness = dt.Rows[i][24].ToString();
                //        if (readiness == "未完成") {
                //            if (worksheet1.Cells[11, 3 + i].Value != null) {
                //                worksheet1.Cells[11, 3 + i].Value += "\n";
                //            }
                //            worksheet1.Cells[11, 3 + i].Value += "PM过滤器";
                //        }
                //    }
                //}
                #endregion

                // 检测结果
                string Result = OBDResult ? "合格" : "不合格";
                //Result += DTCResult ? "" : "\n有DTC";
                //Result += ReadinessResult ? "" : "\n就绪状态未完成项超过2项";
                Result += VINResult ? "" : "\nVIN号不匹配";
                Result += CALIDCVNResult ? "" : "\nCALID和CVN数据不完整";
                Result += SpaceResult ? "" : "\nCALID或CVN有多个空格";
                worksheet1.Cells["B8"].Value = Result;

                // 检验员
                worksheet1.Cells["E9"].Value = m_obdInterface.UserPreferences.Name;

                byte[] bin = package.GetAsByteArray();
                FileInfo exportFileInfo = new FileInfo(ExportPath);
                File.WriteAllBytes(exportFileInfo.FullName, bin);
            }
        }

    }

    public class SetDataTableColumnsErrorEventArgs : EventArgs {
        public string ErrorMsg { get; set; }
    }
}
