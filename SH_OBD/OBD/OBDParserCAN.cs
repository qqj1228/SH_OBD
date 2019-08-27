using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public abstract class OBDParserCAN : OBDParser {
        public OBDResponseList Parse(OBDParameter param, string response, int headLen, int offset) {
            if (string.IsNullOrEmpty(response)) {
                response = "";
            }

            OBDResponseList responseList = new OBDResponseList(response);
            response = Strip(response);
            if (ErrorCheck(response)) {
                responseList.ErrorDetected = true;
                return responseList;
            }

            List<string> lines = SplitByCR(response);
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
                int dataStartIndex1 = GetDataStartIndex(param, bIsMultiline, false) + offset;
                int length1 = groups[i][0].Length - dataStartIndex1;
                obd_response.Header = groups[i][0].Substring(0, headLen);
                obd_response.Data = length1 > 0 ? groups[i][0].Substring(dataStartIndex1, length1) : "";
                int dataStartIndex2 = GetDataStartIndex(param, bIsMultiline, true) + offset;
                for (int j = 1; j < groups[i].Count; j++) {
                    int length2 = groups[i][j].Length - dataStartIndex2;
                    obd_response.Data += (length2 > 0 ? groups[i][j].Substring(dataStartIndex2, length2) : "");
                }
                responseList.AddOBDResponse(obd_response);
            }
            return responseList;
        }

        protected int GetDataStartIndex(OBDParameter param, bool bIsMultiline, bool bConsecutiveLine) {
            if (bConsecutiveLine) {
                return 5;
            }
            switch (param.Service) {
            case 1:
                return 9;
            case 2:
                return 11;
            case 3:
            case 7:
                return bIsMultiline ? 11 : 9;
            case 4:
                return 7;
            case 5:
                return 11;
            case 9:
                return bIsMultiline ? 11 : 9;
            default:
                return 9;
            }
        }
    }

    public class OBDParser_ISO15765_4_CAN11 : OBDParserCAN {
        protected const int HEADER_LENGTH = 3;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH, 0);
        }
    }

    public class OBDParser_ISO15765_4_CAN29 : OBDParserCAN {
        protected const int HEADER_LENGTH = 8;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH, 5);
        }
    }
}
