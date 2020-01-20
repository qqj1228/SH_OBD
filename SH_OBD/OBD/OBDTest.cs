using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SH_OBD {
    public class OBDTest {
        private const int maxPID = 0x100;
        private readonly OBDInterface m_obdInterface;
        private readonly DataTable m_dtInfo;
        private readonly DataTable m_dtECUInfo;
        private readonly Dictionary<string, bool[]> m_mode01Support;
        private readonly Dictionary<string, bool[]> m_mode09Support;
        private static int m_iSN;
        private string m_strSN;
        private bool m_compIgn;
        private bool m_CN6;
        public readonly Model m_db;
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
        public bool CALIDCVNAllEmpty { get; set; }
        public bool CALIDUnmeaningResult { get; set; }
        public bool OBDSUPResult { get; set; }
        public string StrVIN_IN { get; set; }
        public string StrVIN_ECU { get; set; }

        public OBDTest(OBDInterface obd) {
            m_obdInterface = obd;
            m_dtInfo = new DataTable();
            m_dtECUInfo = new DataTable();
            m_mode01Support = new Dictionary<string, bool[]>();
            m_mode09Support = new Dictionary<string, bool[]>();
            m_compIgn = false;
            m_CN6 = false;
            AdvanceMode = false;
            AccessAdvanceMode = 0;
            OBDResult = false;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;
            CALIDCVNResult = true;
            CALIDCVNAllEmpty = false;
            CALIDUnmeaningResult = true;
            OBDSUPResult = true;
            m_db = new Model(m_obdInterface.DBandMES, m_obdInterface.m_log);
            // 设置“testNo”字段中的每日顺序号初值
            GetSN(DateTime.Now.ToLocalTime().ToString("yyyyMMdd"));
        }

        private int GetSN(string strNowDate) {
            m_strSN = m_db.GetSN();
            if (m_strSN.Length == 0) {
                m_iSN = m_obdInterface.OBDResultSetting.StartSN;
            } else if (m_strSN.Split(',')[0] != strNowDate) {
                m_iSN = m_obdInterface.OBDResultSetting.StartSN;
            } else {
                bool result = int.TryParse(m_strSN.Split(',')[1], out m_iSN);
                if (result) {
                    m_iSN = (m_iSN % 1000) + m_obdInterface.OBDResultSetting.StartSN;
                } else {
                    m_iSN = m_obdInterface.OBDResultSetting.StartSN;
                }
            }
            return m_iSN;
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

            if (param.Service == 0 && param.Parameter == 0) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (support.ContainsKey(dt.Columns[i].ColumnName)) {
                        dr[i] = "";
                    }
                }
                dt.Rows.Add(dr);
                return;
            }
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
                                dr[i] = "";
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
            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                for (int i = 2; i < dt.Columns.Count; i++) {
                    if (m_mode01Support.ContainsKey(dt.Columns[i].ColumnName) && !m_mode01Support[dt.Columns[i].ColumnName][0]) {
                        dr[i] = "不适用";
                    }
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
            OBDParameter param = new OBDParameter();
            int HByte = 0;
            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param = new OBDParameter {
                    OBDRequest = "22F401",
                    Service = 0x22,
                    Parameter = 0xF401,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.Bool
                };
                HByte = 0xF400;
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param = new OBDParameter {
                    OBDRequest = "0101",
                    Service = 1,
                    Parameter = 1,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.Bool
                };
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter();
            }
            SetDataRow(++NO, "MIL状态", dt, param);                                          // 0

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 0x21;
                param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            } else {
                param = new OBDParameter();
            }
            SetDataRow(++NO, "MIL亮后行驶里程（km）", dt, param);                              // 1  

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 0x1C;
                param.ValueTypes = (int)OBDParameter.EnumValueTypes.ShortString;
            } else {
                param = new OBDParameter {
                    OBDRequest = "00FECE",
                    Service = 0,
                    Parameter = 0xFECE,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ShortString
                };
            }
            SetDataRow(++NO, "OBD型式检验类型", dt, param);                                    // 2
            string OBD_SUP = dt.Rows[dt.Rows.Count - 1][2].ToString().Split(',')[0];
            string[] CN6_OBD_SUP = m_obdInterface.OBDResultSetting.CN6_OBD_SUP.Split(',');
            foreach (string item in CN6_OBD_SUP) {
                if (OBD_SUP == item) {
                    m_CN6 = true;
                    break;
                }
            }
            // 判断ECM的OBD型式是否合法
            if (m_obdInterface.OBDResultSetting.OBD_SUP) {
                if (OBD_SUP.Length == 0 || OBD_SUP.Length > 2 || OBD_SUP == "不适用") {
                    OBDSUPResult = false;
                }
            }

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 0xA6;
                param.ValueTypes = (int)OBDParameter.EnumValueTypes.Double;
            } else {
                param = new OBDParameter();
            }
            SetDataRow(++NO, "总累积里程ODO（km）", dt, param);                                // 3

            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param.OBDRequest = "194233081E";
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param.OBDRequest = "03";
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter();
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "存储DTC", dt, param);                                           // 4
            for (int i = 2; i < dt.Columns.Count; i++) {
                string DTC = dt.Rows[dt.Rows.Count - 1][i].ToString();
                if (m_obdInterface.OBDResultSetting.DTC03 && DTC != "--" && DTC != "不适用" && DTC.Length > 0) {
                    DTCResult = false;
                }
            }

            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param.OBDRequest = "194233041E";
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param.OBDRequest = "07";
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter();
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.ListString;
            SetDataRow(++NO, "未决DTC", dt, param);                                           // 5
            for (int i = 2; i < dt.Columns.Count; i++) {
                string DTC = dt.Rows[dt.Rows.Count - 1][i].ToString();
                if (m_obdInterface.OBDResultSetting.DTC07 && DTC != "--" && DTC != "不适用" && DTC.Length > 0) {
                    DTCResult = false;
                }
            }

            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param.OBDRequest = "195533";
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param.OBDRequest = "0A";
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter();
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
            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param.OBDRequest = "22F401";
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param.OBDRequest = "0101";
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter();
            }
            param.ValueTypes = (int)OBDParameter.EnumValueTypes.BitFlags;
            List<OBDParameterValue> valueList = new List<OBDParameterValue>();
            if (param.Service == 0 && param.Parameter == 0) {
                OBDParameterValue value = new OBDParameterValue {
                    ErrorDetected = true
                };
                valueList.Add(value);
            } else {
                valueList = m_obdInterface.GetValueList(param);
            }
            SetReadinessDataRow(++NO, "失火监测", dt, valueList, 15, -4, ref errorCount);      // 7
            SetReadinessDataRow(++NO, "燃油系统监测", dt, valueList, 14, -4, ref errorCount);  // 8
            SetReadinessDataRow(++NO, "综合组件监测", dt, valueList, 13, -4, ref errorCount);  // 9

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                foreach (OBDParameterValue value in valueList) {
                    if (value.ECUResponseID != null && m_mode01Support.ContainsKey(value.ECUResponseID) && m_mode01Support[value.ECUResponseID][(param.Parameter & 0x00FF) - 1]) {
                        m_compIgn = value.GetBitFlag(12);
                        break;
                    }
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
            OBDParameter param = new OBDParameter();
            int HByte = 0;
            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                param = new OBDParameter {
                    OBDRequest = "22F802",
                    Service = 0x22,
                    Parameter = 0xF802,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
                HByte = 0xF800;
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param = new OBDParameter {
                    OBDRequest = "0902",
                    Service = 9,
                    Parameter = 2,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter {
                    OBDRequest = "00FEEC",
                    Service = 0,
                    Parameter = 0xFEEC,
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
            StrVIN_ECU = strVIN;
            if (m_obdInterface.OBDResultSetting.VINError && StrVIN_IN != null && StrVIN_ECU != StrVIN_IN && StrVIN_IN.Length > 0) {
                m_obdInterface.m_log.TraceWarning("Scan tool VIN[" + StrVIN_IN + "] and ECU VIN[" + StrVIN_ECU + "] are not consistent");
                VINResult = false;
            }

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 0x0A;
            } else {
                param = new OBDParameter();
            }
            SetDataRow(++NO, "ECU名称", dt, param); // 1

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 4;
            } else {
                param = new OBDParameter {
                    OBDRequest = "00D300",
                    Service = 0,
                    Parameter = 0xD300,
                    SubParameter = 1,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
                m_obdInterface.SetTimeout(1000);
            }
            SetDataRow(++NO, "CAL_ID", dt, param);  // 2

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                param.Parameter = HByte + 6;
            } else {
                param = new OBDParameter {
                    OBDRequest = "00D300",
                    Service = 0,
                    Parameter = 0xD300,
                    SubParameter = 0,
                    ValueTypes = (int)OBDParameter.EnumValueTypes.ListString
                };
                m_obdInterface.SetTimeout(1000);
            }
            SetDataRow(++NO, "CVN", dt, param);     // 3
            m_obdInterface.SetTimeout(500);

            // 根据配置文件，判断CAL_ID和CVN两个值的合法性
            for (int i = 2; i < dt.Columns.Count; i++) {
                string[] CALIDArray = dt.Rows[2][i].ToString().Split('\n');
                string[] CVNArray = dt.Rows[3][i].ToString().Split('\n');
                int length = Math.Max(CALIDArray.Length, CVNArray.Length);
                for (int j = 0; j < length; j++) {
                    string CALID = CALIDArray.Length > j ? CALIDArray[j] : "";
                    string CVN = CVNArray.Length > j ? CVNArray[j] : "";
                    if (m_CN6) {
                        if (!m_obdInterface.OBDResultSetting.CALIDCVNEmpty) {
                            if (CALID.Length * CVN.Length == 0) {
                                if (CALID.Length + CVN.Length == 0) {
                                    if (j == 0) {
                                        CALIDCVNResult = false;
                                        CALIDCVNAllEmpty = true;
                                    }
                                } else {
                                    CALIDCVNResult = false;
                                }
                            }
                        }
                        // 国六车型，CALID全部字符均为空格的话也判为乱码
                        if (Utility.IsUnmeaningString(CALID, m_obdInterface.OBDResultSetting.UnmeaningNum, true)) {
                            CALIDUnmeaningResult = false;
                        }
                    } else if (Utility.IsUnmeaningString(CALID, m_obdInterface.OBDResultSetting.UnmeaningNum, false)) {
                        // 国五车型，CALID全部字符均为空格的话不判为乱码
                        CALIDUnmeaningResult = false;
                    }
                }
            }

        }

        private bool GetSupportStatus(int mode, Dictionary<string, bool[]> supportStatus) {
            List<List<OBDParameterValue>> ECUSupportList = new List<List<OBDParameterValue>>();
            List<bool> ECUSupportNext = new List<bool>();
            OBDParameter param = new OBDParameter();
            int HByte = 0;
            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                HByte = (mode << 8) & 0xFF00;
                param = new OBDParameter(0x22, HByte, 0) {
                    ValueTypes = 32
                };
            } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
                param = new OBDParameter(mode, 0, 0) {
                    ValueTypes = 32
                };
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                param = new OBDParameter(0, mode, 0);
            }
            List<OBDParameterValue> valueList = m_obdInterface.GetValueList(param);
            foreach (OBDParameterValue value in valueList) {
                List<OBDParameterValue> ECUValueList = new List<OBDParameterValue>();
                if (value.ErrorDetected) {
                    return false;
                }
                if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                    supportStatus.Add(value.ECUResponseID, null);
                } else {
                    ECUValueList.Add(value);
                    ECUSupportList.Add(ECUValueList);
                    ECUSupportNext.Add(value.GetBitFlag(31));
                }
            }
            if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                return true;
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
                        for (int j = 0; j < ECUSupportNext.Count; j++) {
                            ECUSupportNext[j] = false;
                        }
                        for (int j = 0; j < ECUSupportList.Count; j++) {
                            if (ECUSupportList[j][0].ECUResponseID == value.ECUResponseID) {
                                ECUSupportList[j].Add(value);
                                ECUSupportNext[j] = value.GetBitFlag(31);
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
                if (m_obdInterface.STDType == StandardType.ISO_27145) {
                    log = "DID " + mode.ToString("X2") + " Support: [" + key + "], [";
                } else if (m_obdInterface.STDType == StandardType.ISO_15031) {
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
            m_obdInterface.m_log.TraceInfo(">>>>> Enter StartOBDTest function. Ver: " + MainFileVersion.AssemblyVersion + " <<<<<");
            errorMsg = "";
            m_dtInfo.Clear();
            m_dtInfo.Dispose();
            m_dtECUInfo.Clear();
            m_dtECUInfo.Dispose();
            m_mode01Support.Clear();
            m_mode09Support.Clear();
            m_compIgn = false;
            m_CN6 = false;
            OBDResult = false;
            DTCResult = true;
            ReadinessResult = true;
            VINResult = true;
            CALIDCVNResult = true;
            CALIDCVNAllEmpty = false;
            CALIDUnmeaningResult = true;
            OBDSUPResult = true;

            OBDTestStart?.Invoke();

            int mode01 = 1;
            int mode09 = 9;
            if (m_obdInterface.STDType == StandardType.ISO_27145) {
                mode01 = 0xF4;
                mode09 = 0xF8;
            } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                // J1939只取一个支持状态就够了
                mode01 = 0xFECE;
            }

            if (!GetSupportStatus(mode01, m_mode01Support)) {
                OBDResult = false;
                if (m_obdInterface.STDType == StandardType.ISO_27145) {
                    errorMsg = "获取 DID F4 支持状态出错！";
                    m_obdInterface.m_log.TraceError("Get DID F4 Support Status Error!");
                } else if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                    errorMsg = "获取 DM5 支持状态出错！";
                    m_obdInterface.m_log.TraceError("Get DM5 Support Status Error!");
                } else {
                    errorMsg = "获取 Mode01 支持状态出错！";
                    m_obdInterface.m_log.TraceError("Get Mode01 Support Status Error!");
                }
                SetupColumnsDone?.Invoke();
                throw new Exception(errorMsg);
            }

            if (m_obdInterface.STDType != StandardType.SAE_J1939) {
                // J1939只取一个支持状态就够了
                if (!GetSupportStatus(mode09, m_mode09Support)) {
                    OBDResult = false;
                    if (m_obdInterface.STDType == StandardType.ISO_27145) {
                        errorMsg = "获取 DID F8 支持状态出错！";
                        m_obdInterface.m_log.TraceError("Get DID F8 Support Status Error!");
                    } else {
                        errorMsg = "获取 Mode09 支持状态出错！";
                        m_obdInterface.m_log.TraceError("Get Mode09 Support Status Error!");
                    }
                    SetupColumnsDone?.Invoke();
                    throw new Exception(errorMsg);
                }
            }

            SetDataTableColumns<string>(m_dtInfo, m_mode01Support);
            if (m_obdInterface.STDType == StandardType.SAE_J1939) {
                // J1939用mode01来初始化m_dtECUInfo
                SetDataTableColumns<string>(m_dtECUInfo, m_mode01Support);
            } else {
                SetDataTableColumns<string>(m_dtECUInfo, m_mode09Support);
            }
            SetupColumnsDone?.Invoke();
            SetDataTableInfo();
            SetDataTableECUInfo();

            WriteDbStart?.Invoke();
            OBDResult = DTCResult && ReadinessResult && VINResult && CALIDCVNResult && CALIDUnmeaningResult && OBDSUPResult;
            string strLog = "OBD Test Result: " + OBDResult.ToString() + " [";
            strLog += "DTCResult: " + DTCResult.ToString();
            strLog += ", ReadinessResult: " + ReadinessResult.ToString();
            strLog += ", CALIDUnmeaningResult: " + CALIDUnmeaningResult.ToString();
            strLog += ", OBDSUPResult: " + OBDSUPResult.ToString();
            strLog += ", VINResult: " + VINResult.ToString();
            strLog += ", CALIDCVNResult: " + CALIDCVNResult.ToString() + "]";
            m_obdInterface.m_log.TraceInfo(strLog);

            string strOBDResult = OBDResult ? "1" : "0";

            DataTable dt = new DataTable("OBDData");
            SetDataTableResultColumns(ref dt);
            try {
                SetDataTableResult(StrVIN_ECU, strOBDResult, ref dt);
            } catch (Exception ex) {
                m_obdInterface.m_log.TraceError("Result DataTable Error: " + ex.Message);
                dt.Dispose();
                WriteDbDone?.Invoke();
                throw new Exception("生成 Result DataTable 出错");
            }

            m_db.ModifyDB(dt);
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
                NotUploadData?.Invoke();
                dt.Dispose();
                return true;
            }

            bool bRet;
            try {
                bRet = UploadData(StrVIN_ECU, strOBDResult, dt, ref errorMsg);
            } catch (Exception) {
                dt.Dispose();
                throw;
            }
            dt.Dispose();
            return bRet;
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
                UploadDataDone?.Invoke();
                throw;
            }

            int count = 0;
            while (strRet.Contains("NOK") || strRet.Contains("false") || strRet.Contains("False")) {
                if (++count < 4) {
                    m_obdInterface.m_log.TraceError(WSHelper.GetMethodName(0) + " Error: " + strMsg);
                    try {
                        strRet = WSHelper.GetResponseOutString(WSHelper.GetMethodName(0), out strMsg, dt1MES, dt2MES);
                    } catch (Exception ex) {
                        m_obdInterface.m_log.TraceError("WebService GetResponseString error: " + strMsg + ", " + ex.Message);
                        dt2MES.Dispose();
                        dt1MES.Dispose();
                        UploadDataDone?.Invoke();
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
                m_obdInterface.m_log.TraceInfo("Upload data success, VIN = " + strVIN);
                m_db.UpdateUpload(strVIN, "1");
                UploadDataDone?.Invoke();
#if DEBUG
                errorMsg = strMsg;
#endif
                return true;
            } else {
                // 上传数据接口返回失败信息
                m_obdInterface.m_log.TraceError("Upload data function return false, VIN = " + strVIN);
                UploadDataDone?.Invoke();
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

            DataRow dr = dt1MES.NewRow();
            dr["SBFLAG"] = "OBD";
            dr["VIN"] = strVIN;
            string strNowDateTime = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            GetSN(strNowDateTime);
            ++m_iSN;
            m_iSN %= 10000;
            dr["TestNo"] = "XC0079" + strNowDateTime + m_iSN.ToString("d4");
            m_strSN = strNowDateTime + "," + m_iSN.ToString();
            m_db.SetSN(m_strSN);
            dr["TestType"] = "0";
            dr["APASS"] = "1";
            dr["OPASS"] = strOBDResult;
            dr["Result"] = strOBDResult;
            dr["otestdate"] = strNowDateTime;
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
            // OBD型式和ODO只用第一个ECU（即7E8，ECM）的数据上传
            string OBD_SUP = dtIn.Rows[0][4].ToString().Split(',')[0].Replace("不适用", "");
            string ODO = dtIn.Rows[0][5].ToString().Replace("不适用", "");
            if (OBD_SUP.Length > 2) {
                OBD_SUP = OBD_SUP.Substring(0, 2);
            }
            dt2MES.Rows.Add(OBD_SUP, ODO, moduleID, CALID, CVN);
        }

        private void SetDataTable2MES(DataTable dt2MES, DataTable dtIn) {
            dt2MES.Columns.Add("obd");
            dt2MES.Columns.Add("odo");
            dt2MES.Columns.Add("ModuleID");
            dt2MES.Columns.Add("CALID");
            dt2MES.Columns.Add("CVN");
            for (int i = 0; i < dtIn.Rows.Count; i++) {
                string ECUAcronym = dtIn.Rows[i][25].ToString().Split('-')[0];
                if (m_CN6) {
                    string[] CALIDArray = dtIn.Rows[i][26].ToString().Split(',');
                    string[] CVNArray = dtIn.Rows[i][27].ToString().Split(',');
                    int length = Math.Max(CALIDArray.Length, CVNArray.Length);
                    DataTable2MESAddRow(dt2MES, dtIn, i, ECUAcronym, CALIDArray[0], CVNArray[0]);
                    for (int j = 1; j < length; j++) {
                        string CALID = CALIDArray.Length > j ? CALIDArray[j] : "";
                        string CVN = CVNArray.Length > j ? CVNArray[j] : "";
                        // 若同一个ECU下有多个CALID和CVN的上传策略
                        if (m_obdInterface.STDType == StandardType.SAE_J1939 && m_obdInterface.OBDResultSetting.KMSSpecified) {
                            // 国六、有多组CALID和CVN、J1939、康明斯车型特殊处理选项为true
                            // 以上条件均满足的话，除了第一组CALID和CVN，其余均不上传
                            m_obdInterface.m_log.TraceWarning(string.Format("KMS with CN6 vehicle ignore {0}(nd/rd/th) [CALID: {1}, CVN: {2}], won't upload", j + 1, CALID, CVN));
                            continue;
                        }
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
                } else {
                    string CALID = dtIn.Rows[i][26].ToString().Replace(",", "").Replace("不适用", "");
                    if (CALID.Length > 20) {
                        CALID = CALID.Substring(0, 20);
                    }
                    string CVN = dtIn.Rows[i][27].ToString().Replace(",", "").Replace("不适用", "");
                    if (CVN.Length > 20) {
                        CVN = CVN.Substring(0, 20);
                    }
                    DataTable2MESAddRow(dt2MES, dtIn, i, ECUAcronym, CALID, CVN);
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

        public bool UploadDataFromDB(string strVIN, out string errorMsg, bool bOnlyShowData) {
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
            if (bOnlyShowData) {
                m_obdInterface.m_log.TraceInfo("Only show data from database");
                NotUploadData?.Invoke();
                dt.Dispose();
                return bRet;
            }
            if (!m_obdInterface.OBDResultSetting.UploadWhenever && dt.Rows[0]["Result"].ToString() != "1") {
                m_obdInterface.m_log.TraceWarning("Won't upload data from database because OBD test result is NOK");
                NotUploadData?.Invoke();
                dt.Dispose();
                return bRet;
            }
            if (dt.Rows.Count > 0) {
                try {
                    bRet = UploadData(strVIN, dt.Rows[0][28].ToString(), dt, ref errorMsg);
                } catch (Exception) {
                    string strMsg = "Wrong record: VIN = " + strVIN + ", OBDResult = " + dt.Rows[0][28].ToString() + ", ";
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        strMsg += "ECU_ID = " + dt.Rows[i][1] + ", ";
                        strMsg += "OBD_SUP = " + dt.Rows[i][4] + ", ";
                        strMsg += "ODO = " + dt.Rows[i][5] + ", ";
                        strMsg += "ECU_NAME = " + dt.Rows[i][25] + ", ";
                        strMsg += "CAL_ID = " + dt.Rows[i][26] + ", ";
                        strMsg += "CVN = " + dt.Rows[i][27] + " || ";
                    }
                    strMsg = strMsg.Substring(0, strMsg.Length - 4);
                    m_obdInterface.m_log.TraceError("Manual Upload Data Failed: " + errorMsg);
                    m_obdInterface.m_log.TraceError(strMsg);
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
                    bRet = UploadData(dt.Rows[0][0].ToString(), dt.Rows[0][28].ToString(), dt, ref errorMsg);
                } catch (Exception ex) {
                    string strMsg = "Wrong record: VIN = " + dt.Rows[0][0].ToString() + ", OBDResult = " + dt.Rows[0][28].ToString() + " [";
                    for (int j = 0; j < dt.Rows.Count; j++) {
                        strMsg += "ECU_ID = " + dt.Rows[j][1] + ", ";
                        strMsg += "OBD_SUP = " + dt.Rows[j][4] + ", ";
                        strMsg += "ODO = " + dt.Rows[j][5] + ", ";
                        strMsg += "ECU_NAME = " + dt.Rows[j][25] + ", ";
                        strMsg += "CAL_ID = " + dt.Rows[j][26] + ", ";
                        strMsg += "CVN = " + dt.Rows[j][27] + " || ";
                    }
                    strMsg = strMsg.Substring(0, strMsg.Length - 4) + "]";
                    m_obdInterface.m_log.TraceError("Upload Data OnTime Failed: " + errorMsg + ", " + ex.Message);
                    m_obdInterface.m_log.TraceError(strMsg);
                    continue;
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
                // VIN
                worksheet1.Cells["B2"].Value = dt.Rows[0][0].ToString();

                // CALID, CVN
                if (m_CN6) {
                    string[] CALIDArray = dt.Rows[0][26].ToString().Split(',');
                    string[] CVNArray = dt.Rows[0][27].ToString().Split(',');
                    for (int i = 0; i < 2; i++) {
                        if (m_obdInterface.STDType == StandardType.SAE_J1939 && i > 0 && m_obdInterface.OBDResultSetting.KMSSpecified) {
                            // 国六、有多组CALID和CVN、J1939、康明斯车型特殊处理选项为true
                            // 以上条件均满足的话，除了第一组CALID和CVN，其余均不上传
                            m_obdInterface.m_log.TraceWarning(string.Format("KMS with CN6 vehicle ignore {0}(nd/rd/th) [CALID: {1}, CVN: {2}], won't export into excel file", i + 1, CALIDArray[i], CVNArray[i]));
                            continue;
                        }
                        worksheet1.Cells[3 + i, 2].Value = CALIDArray.Length > i ? CALIDArray[i] : "";
                        worksheet1.Cells[3 + i, 4].Value = CVNArray.Length > i ? CVNArray[i] : "";
                    }
                    for (int i = 1; i < dt.Rows.Count; i++) {
                        worksheet1.Cells["B5"].Value = dt.Rows[i][26].ToString().Replace(",", "\n");
                        worksheet1.Cells["D5"].Value = dt.Rows[i][27].ToString().Replace(",", "\n");
                    }
                } else {
                    string CALID = dt.Rows[0][26].ToString().Replace(",", "");
                    if (CALID.Length > 20) {
                        CALID = CALID.Substring(0, 20);
                    }
                    string CVN = dt.Rows[0][27].ToString().Replace(",", "");
                    if (CVN.Length > 20) {
                        CVN = CVN.Substring(0, 20);
                    }
                    worksheet1.Cells["B3"].Value = CALID;
                    worksheet1.Cells["D3"].Value = CVN;

                    for (int i = 1; i < dt.Rows.Count; i++) {
                        CALID = dt.Rows[i][26].ToString().Replace(",", "");
                        if (CALID.Length > 20) {
                            CALID = CALID.Substring(0, 20);
                        }
                        CVN = dt.Rows[i][27].ToString().Replace(",", "");
                        if (CVN.Length > 20) {
                            CVN = CVN.Substring(0, 20);
                        }
                        worksheet1.Cells[3 + i, 2].Value = CALID;
                        worksheet1.Cells[3 + i, 4].Value = CVN;
                    }
                }

                // moduleID
                string moduleID = GetModuleID(dt.Rows[0][25].ToString().Split('-')[0], dt.Rows[0][1].ToString());
                worksheet1.Cells["E3"].Value = moduleID;
                worksheet1.Cells["B4"].Value += "";
                worksheet1.Cells["D4"].Value += "";

                string OtherID = "";
                if (m_CN6) {
                    if (worksheet1.Cells["B4"].Value.ToString().Length > 0 || worksheet1.Cells["D4"].Value.ToString().Length > 0) {
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
                    for (int i = 1; i < dt.Rows.Count; i++) {
                        moduleID = GetModuleID(dt.Rows[i][25].ToString().Split('-')[0], dt.Rows[i][1].ToString());
                        OtherID += "," + moduleID;
                    }
                    worksheet1.Cells["E5"].Value = OtherID.Trim(',');
                } else {
                    if (dt.Rows.Count > 1) {
                        moduleID = GetModuleID(dt.Rows[1][25].ToString().Split('-')[0], dt.Rows[1][1].ToString());
                        worksheet1.Cells["E4"].Value = moduleID;
                        if (dt.Rows.Count > 2) {
                            for (int i = 2; i < dt.Rows.Count; i++) {
                                moduleID = GetModuleID(dt.Rows[i][25].ToString().Split('-')[0], dt.Rows[i][1].ToString());
                                OtherID += "," + moduleID;
                            }
                            worksheet1.Cells["E5"].Value = OtherID.Trim(',');
                        }
                    }
                }

                // 外观检验结果
                worksheet1.Cells["B7"].Value = "合格";

                // OBD型式检验要求
                worksheet1.Cells["B9"].Value = dt.Rows[0][4].ToString();

                // 总累积里程ODO（km）
                worksheet1.Cells["B10"].Value = dt.Rows[0][5].ToString();

                // 与OBD诊断仪通讯情况
                worksheet1.Cells["B11"].Value = "通讯成功";

                // 检测结果
                string Result = OBDResult ? "合格" : "不合格";
                //Result += DTCResult ? "" : "\n有DTC";
                //Result += ReadinessResult ? "" : "\n就绪状态未完成项超过2项";
                Result += VINResult ? "" : "\nVIN号不匹配";
                Result += CALIDCVNResult ? "" : "\nCALID和CVN数据不完整";
                Result += CALIDUnmeaningResult ? "" : "\nCALID含有乱码";
                Result += OBDSUPResult ? "" : "\nOBD型式不适用或异常";
                worksheet1.Cells["B12"].Value = Result;

                // 检验员
                worksheet1.Cells["E13"].Value = m_obdInterface.UserPreferences.Name;

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
