using System;
using System.Collections;
using System.Collections.Generic;

namespace SH_OBD {
    public class OBDParser_ISO14230_4_KWP : OBDParser {
        protected const int HEADER_LENGTH = 6;

        static OBDParser_ISO14230_4_KWP() {
        }

        public override OBDResponseList Parse(OBDParameter param, string response) {
            if (string.IsNullOrEmpty(response)) {
                response = "";
            }

            OBDResponseList responseList = new OBDResponseList(response);
            response = Strip(response);
            if (ErrorCheck(response)) {
                responseList.ErrorDetected = true;
                return responseList;
            } else {
                List<string> list1 = SplitByCR(response);
                list1.Sort();
                List<List<string>> list2 = new List<List<string>>();
                List<string> list3 = new List<string> { list1[0] };
                list2.Add(list3);
                if (list1[0].Length < OBDParser_ISO14230_4_KWP.HEADER_LENGTH) {
                    responseList.ErrorDetected = true;
                    return responseList;
                } else {
                    string header = list1[0].Substring(0, OBDParser_ISO14230_4_KWP.HEADER_LENGTH);
                    for (int i = 1; i < list1.Count; i++) {
                        if (list1[i].Length >= OBDParser_ISO14230_4_KWP.HEADER_LENGTH) {
                            if (list1[i].Substring(0, OBDParser_ISO14230_4_KWP.HEADER_LENGTH).CompareTo(header) == 0) {
                                list3.Add(list1[i]);
                            } else {
                                list3 = new List<string> { list1[i] };
                                list2.Add(list3);
                                header = list1[i].Substring(0, OBDParser_ISO14230_4_KWP.HEADER_LENGTH);
                            }
                        } else {
                            responseList.ErrorDetected = true;
                            return responseList;
                        }
                    }
                    for (int i = 0; i < list2.Count; i++) {
                        OBDResponse response1 = new OBDResponse();
                        int dataStartIndex = GetDataStartIndex(param);
                        int length1 = list2[i][0].Length - dataStartIndex - 2;
                        response1.Header = list2[i][0].Substring(0, OBDParser_ISO14230_4_KWP.HEADER_LENGTH);
                        response1.Data = length1 > 0 ? list2[i][0].Substring(dataStartIndex, length1) : "";
                        for (int j = 1; j < list2[j].Count; j++) {
                            int length2 = list2[i][j].Length - dataStartIndex - 2;
                            response1.Data += length2 > 0 ? list2[i][j].Substring(dataStartIndex, length2) : "";
                        }
                        responseList.AddOBDResponse(response1);
                    }
                    return responseList;
                }
            }
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
                    if (param.Parameter == 2) {
                        return 12;
                    } else {
                        break;
                    }
            }
            return 10;
        }
    }
}
