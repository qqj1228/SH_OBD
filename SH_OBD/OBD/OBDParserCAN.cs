using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public abstract class OBDParserCAN : OBDParser {
        public OBDResponseList Parse(OBDParameter param, string response, int headLen) {
            if (string.IsNullOrEmpty(response)) {
                response = "";
            }

            OBDResponseList responseList = new OBDResponseList(response);
            response = Strip(response);
            if (ErrorCheck(response)) {
                responseList.ErrorDetected = true;
                return responseList;
            }

            List<string> tempLines = SplitByCR(response);
            List<string> lines = new List<string>();
            foreach (string item in tempLines) {
                if (item.Length > 0 && item.Length < headLen) {
                    // 需要过滤数据帧总长小于帧头长度的数据，这种数据帧有两种可能：
                    // 1、J1939多帧消息的第一条，因为目前使用的盗版ELM327的版本为v1.3a，
                    //    这个版本的ELM327不会返回第一条的整个数据只会返回有效字节数
                    // 2、其他协议中错误的CAN或K线消息
                    continue;
                }
                string strNRC = GetNRC(item, headLen);
                if (strNRC.Length == 0) {
                    lines.Add(item);
                } else if (strNRC == "78") {
                    responseList.Pending = true;
                }
            }
            if (lines.Count == 0) {
                if (responseList.Pending) {
                    responseList.RawResponse = "PENDING";
                    return responseList;
                } else {
                    responseList.ErrorDetected = true;
                    return responseList;
                }
            }

            lines.Sort();
            List<List<string>> groups = new List<List<string>>();
            List<string> group = new List<string> { lines[0] };
            groups.Add(group);
            if (lines[0].Length < headLen) {
                responseList.ErrorDetected = true;
                return responseList;
            }

            string header = lines[0].Substring(0, headLen);
            for (int i = 1; i < lines.Count; i++) {
                if (lines[i].Length >= headLen) {
                    if (lines[i].Substring(0, headLen).CompareTo(header) == 0) {
                        group.Add(lines[i]);
                    } else {
                        group = new List<string> { lines[i] };
                        groups.Add(group);
                        header = lines[i].Substring(0, headLen);
                    }
                } else {
                    responseList.ErrorDetected = true;
                    return responseList;
                }
            }
            for (int i = 0; i < groups.Count; i++) {
                OBDResponse obd_response = new OBDResponse();
                bool bIsMultiline = false;
                if (groups[i].Count > 1) {
                    bIsMultiline = true;
                }
                int dataStartIndex1;
                if (param.Service == 0 && bIsMultiline) {
                    dataStartIndex1 = GetDataStartIndex(headLen, param, bIsMultiline, true);
                } else {
                    dataStartIndex1 = GetDataStartIndex(headLen, param, bIsMultiline, false);
                }
                int length1 = groups[i][0].Length - dataStartIndex1;
                obd_response.Header = groups[i][0].Substring(0, headLen);
                obd_response.Data = length1 > 0 ? groups[i][0].Substring(dataStartIndex1, length1) : "";
                int dataStartIndex2 = GetDataStartIndex(headLen, param, bIsMultiline, true);
                for (int j = 1; j < groups[i].Count; j++) {
                    int length2 = groups[i][j].Length - dataStartIndex2;
                    obd_response.Data += (length2 > 0 ? groups[i][j].Substring(dataStartIndex2, length2) : "");
                }
                responseList.AddOBDResponse(obd_response);
            }
            return responseList;
        }

        protected int GetDataStartIndex(int headLen, OBDParameter param, bool bIsMultiline, bool bConsecutiveLine) {
            int iRet;
            if (bConsecutiveLine) {
                // 连续帧
                return headLen + 2;
            }
            switch (param.Service) {
            case 0:
                iRet = headLen;
                break;
            case 1:
                iRet = headLen + 6;
                break;
            case 2:
                iRet = headLen + 8;
                break;
            case 3:
            case 7:
            case 0x0A:
                iRet = headLen + 6;
                break;
            case 4:
                iRet = headLen + 4;
                break;
            case 5:
                iRet = headLen + 8;
                break;
            case 9:
                iRet = param.Parameter % 2 == 0 && param.Parameter % 0x20 != 0 ? headLen + 8 : headLen + 6;
                break;
            case 0x19:
                // ISO 27145 ReadDTCInformation
                string reportType = param.OBDRequest.Substring(2, 2);
                if (reportType == "42") {
                    iRet = headLen + 14;
                } else if (reportType == "55") {
                    iRet = headLen + 12;
                } else {
                    iRet = headLen + 6;
                }
                break;
            case 0x22:
                // ISO 27145 ReadDataByIdentifer
                iRet = headLen + 8;
                break;
            default:
                iRet = headLen + 6;
                break;
            }
            return bIsMultiline ? iRet + 2 : iRet;
        }

        private bool IsTesterPresentResponse(string strData, int headLen) {
            if (strData.Length > 0) {
                string strActual = strData.Substring(headLen + 2);
                return strActual == "7E00";
            } else {
                return false;
            }
        }
    }

    public class OBDParser_ISO15765_4_CAN11 : OBDParserCAN {
        protected const int HEADER_LENGTH = 3;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

    public class OBDParser_ISO15765_4_CAN29 : OBDParserCAN {
        protected const int HEADER_LENGTH = 8;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

    public class OBDParser_SAE_J1939_CAN29 : OBDParserCAN {
        protected const int HEADER_LENGTH = 8;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

}
