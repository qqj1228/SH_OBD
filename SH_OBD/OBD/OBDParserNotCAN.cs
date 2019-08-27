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
                int dataStartIndex = GetDataStartIndex(param);
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

        protected int GetDataStartIndex(OBDParameter param) {
            switch (param.Service) {
            case 1:
                return 10;
            case 2:
                return 12;
            case 3:
            case 4:
                return 8;
            case 5:
                return 12;
            case 7:
                return 8;
            case 9:
                return param.Parameter % 2 == 0 ? 12 : 10;
            }
            return 10;
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
