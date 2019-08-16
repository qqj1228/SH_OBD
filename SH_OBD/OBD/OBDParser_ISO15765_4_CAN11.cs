using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public class OBDParser_ISO15765_4_CAN11 : OBDParser {
        protected const int HEADER_LENGTH = 3;

        public override OBDResponseList Parse(OBDParameter param, string response) {
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
            if (lines[0].Length < OBDParser_ISO15765_4_CAN11.HEADER_LENGTH) {
                responseList.ErrorDetected = true;
                return responseList;
            }

            string header = lines[0].Substring(0, OBDParser_ISO15765_4_CAN11.HEADER_LENGTH);
            for (int i = 1; i < lines.Count; i++) {
                if (lines[i].Length >= OBDParser_ISO15765_4_CAN11.HEADER_LENGTH) {
                    if (lines[i].Substring(0, OBDParser_ISO15765_4_CAN11.HEADER_LENGTH).CompareTo(header) == 0)
                        group.Add(lines[i]);
                    else {
                        group = new List<string> { lines[i] };
                        groups.Add(group);
                        header = lines[i].Substring(0, OBDParser_ISO15765_4_CAN11.HEADER_LENGTH);
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
                int dataStartIndex1 = GetDataStartIndex(param, bIsMultiline, false);
                int length1 = groups[i][0].Length - dataStartIndex1;
                obd_response.Header = groups[i][0].Substring(0, OBDParser_ISO15765_4_CAN11.HEADER_LENGTH);
                obd_response.Data = length1 > 0 ? groups[i][0].Substring(dataStartIndex1, length1) : "";
                int dataStartIndex2 = GetDataStartIndex(param, bIsMultiline, true);
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
                    return !bIsMultiline ? 9 : 11;
                case 4:
                    return 7;
                case 5:
                    return 11;
                case 9:
                    if (param.Parameter == 0x06) {
                        return 11;
                    } else {
                        return 13;
                    }
                default:
                    return 9;
            }
        }
    }
}
