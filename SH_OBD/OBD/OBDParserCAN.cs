using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public abstract class OBDParserCAN : OBDParser {
        public OBDResponseList Parse(OBDParameter param, string response, int headLenRaw) {
            int headLen = Math.Abs(headLenRaw);
            bool bJ1939 = headLenRaw < 0;
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
            List<string> legalLines = new List<string>();
            if (bJ1939) {
                legalLines = tempLines;
            } else {
                legalLines = GetLegalLines(param, tempLines, headLen);
            }
            List<string> lines = new List<string>();
            foreach (string item in legalLines) {
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

        /// <summary>
        /// 返回符合标准协议规定的CAN帧
        /// </summary>
        /// <param name="tempLines"></param>
        /// <returns></returns>
        private List<string> GetLegalLines(OBDParameter param, List<string> tempLines, int headLen) {
            List<string> lines = new List<string>();
            // dicFrameType表示找到的正响应的PDU帧类型，key: ECU ID，value: 帧类型
            // value值，-1：未确认类型，0：单帧SF，1：首帧FF，2：连续帧CF，3：流控帧FC（ELM327不会返回FC帧）
            Dictionary<string, int> dicFrameType = new Dictionary<string, int>();

            string positiveResponse = (param.Service + 0x40).ToString("X2") + param.OBDRequest.Substring(2);
            string negativeResponse = "7F" + param.OBDRequest.Substring(0, 2);

            for (int i = 0; i < tempLines.Count; i++) {
                if (tempLines[i].Length < headLen) {
                    continue;
                }
                string ECU_ID = tempLines[i].Substring(0, headLen);
                if (!dicFrameType.Keys.Contains(ECU_ID)) {
                    dicFrameType.Add(ECU_ID, -1);
                }

                if (tempLines[i].Contains(negativeResponse)) {
                    // 响应本命令的负反馈，可能有多个
                    lines.Add(tempLines[i]);
                } else if (tempLines[i].Contains(positiveResponse) && dicFrameType[ECU_ID] < 0) {
                    // 响应本命令的正反馈，每个ECU只会有一个
                    int pos = tempLines[i].IndexOf(positiveResponse);
                    if (pos >= 2) {
                        string strPCI = tempLines[i].Substring(pos - 2, 2);
                        try {
                            int iPCI = Convert.ToInt32(strPCI, 16);
                            if (iPCI <= 7) {
                                // 找到单帧
                                dicFrameType[ECU_ID] = 0;
                                // 处理单帧
                                lines.Add(tempLines[i]);
                            } else {
                                dicFrameType[ECU_ID] = -1;
                            }
                        } catch (Exception) {
                            dicFrameType[ECU_ID] = -1;
                        }
                        if (dicFrameType[ECU_ID] < 0 && pos >= 4) {
                            strPCI = tempLines[i].Substring(pos - 4, 4);
                            if (strPCI[0] == '1') {
                                // 找到首帧
                                dicFrameType[ECU_ID] = 1;
                                int iDataLen = 0;
                                int iCF = 0;
                                try {
                                    iDataLen = Convert.ToInt32(strPCI.Substring(1), 16);
                                } catch (Exception) {
                                    dicFrameType[ECU_ID] = -1;
                                }
                                if (iDataLen > 7) {
                                    // 找到连续帧
                                    iCF = (int)Math.Ceiling((iDataLen - 6) / 7.0);
                                    dicFrameType[ECU_ID] = 2;
                                }
                                if (dicFrameType[ECU_ID] > 0) {
                                    // 处理首帧
                                    lines.Add(tempLines[i]);
                                    if (dicFrameType[ECU_ID] > 1) {
                                        // 处理连续帧
                                        for (int j = 1; j <= iCF; j++) {
                                            lines.Add(tempLines[i + j]);
                                        }
                                        i += iCF;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lines;
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
        protected const int HEADER_LENGTH = -8;

        public override OBDResponseList Parse(OBDParameter param, string response) {
            return Parse(param, response, HEADER_LENGTH);
        }
    }

}
