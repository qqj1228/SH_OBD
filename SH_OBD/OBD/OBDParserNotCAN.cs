using System;
using System.Collections;
using System.Collections.Generic;

namespace SH_OBD {
    public abstract class OBDParserNotCAN : OBDParser {
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
                    // 过滤数据帧总长小于帧头长度的错误数据
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
                int dataStartIndex = GetDataStartIndex(headLen, param, bIsMultiline);
                int length1 = groups[i][0].Length - dataStartIndex - 2;
                obd_response.Header = groups[i][0].Substring(0, headLen);
                obd_response.Data = length1 > 0 ? groups[i][0].Substring(dataStartIndex, length1) : "";
                for (int j = 1; j < groups[i].Count; j++) {
                    int length2 = groups[i][j].Length - dataStartIndex - 2;
                    obd_response.Data += (length2 > 0 ? groups[i][j].Substring(dataStartIndex, length2) : "");
                }
                responseList.AddOBDResponse(obd_response);
            }
            return responseList;
        }

        protected int GetDataStartIndex(int headLen, OBDParameter param, bool bIsMultiline) {
            int iRet;
            switch (param.Service) {
            case 1:
                iRet = headLen + 4;
                break;
            case 2:
                iRet = headLen + 6;
                break;
            case 3:
            case 4:
            case 7:
            case 0x0A:
                iRet = headLen + 2;
                break;
            case 5:
                iRet = headLen + 6;
                break;
            case 9:
                return param.Parameter % 2 == 0 ? headLen + 6 : headLen + 4;
            default:
                iRet = headLen + 4;
                break;
            }
            return bIsMultiline ? iRet + 2 : iRet;
        }
    }

    public class OBDParser_ISO14230_4_KWP : OBDParserNotCAN {
        protected const int HEADER_LENGTH = 6;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

    public class OBDParser_ISO9141_2 : OBDParserNotCAN {
        protected const int HEADER_LENGTH = 6;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

    public class OBDParser_J1850_PWM : OBDParserNotCAN {
        protected const int HEADER_LENGTH = 6;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

    public class OBDParser_J1850_VPW : OBDParserNotCAN {
        protected const int HEADER_LENGTH = 6;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }
}
