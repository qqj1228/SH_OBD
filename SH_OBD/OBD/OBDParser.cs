using System;
using System.Collections;
using System.Collections.Generic;

namespace SH_OBD {
    public abstract class OBDParser {
        public abstract OBDResponseList Parse(OBDParameter param, string response);
        protected abstract List<string> GetLegalLines(OBDParameter param, List<string> tempLines, int headLen);

        protected string Strip(string input) {
            if (input == null) {
                return "";
            } else {
                return input
                    .Replace("BUS", "")
                    .Replace("INIT", "")
                    .Replace("DONE", "")
                    .Replace("SEARCHING", "")
                    .Replace(":", "")
                    .Replace(".", "")
                    .Replace(" ", "")
                    .Trim();
            }
        }

        private static readonly char[] CHAR_CR = new char[] { '\r' };
        protected List<string> SplitByCR(string input) {
            string[] lines = (input ?? string.Empty).Split(CHAR_CR);
            List<string> list = new List<string>(lines.Length);
            list.AddRange(lines);
            return list;
        }

        protected bool ErrorCheck(string input) {
            if (input == null) {
                return false;
            } else {
                return (
                    input.Contains("TIMEOUT") ||
                    input.Contains("?") ||
                    input.Contains("NODATA") ||
                    input.Contains("BUFFERFULL") ||
                    input.Contains("BUSBUSY") ||
                    input.Contains("BUSERROR") ||
                    input.Contains("CANERROR") ||
                    input.Contains("DATAERROR") ||
                    input.Contains("<DATAERROR") ||
                    input.Contains("<RXERROR") ||
                    input.Contains("FBERROR") ||
                    input.Contains("ERR") ||
                    input.Contains("LVRESET") ||
                    input.Contains("STOPPED") ||
                    input.Contains("UNABLETOCONNECT")
                );
            }
        }

        protected string ErrorFilter(string input) {
            if (input == null) {
                return "";
            }
            string output = input
                .Replace("TIMEOUT", "")
                .Replace("?", "")
                .Replace("NODATA", "")
                .Replace("BUFFERFULL", "")
                .Replace("BUSBUSY", "")
                .Replace("BUSERROR", "")
                .Replace("CANERROR", "")
                .Replace("DATAERROR", "")
                .Replace("<DATAERROR", "")
                .Replace("<RXERROR", "")
                .Replace("FBERROR", "")
                .Replace("LVRESET", "")
                .Replace("STOPPED", "")
                .Replace("UNABLETOCONNECT", "");
            int start = 0;
            int pos;
            string strRet = string.Empty;
            // 过滤"ERRxx"
            while (start < output.Length) {
                pos = output.IndexOf("ERR", start);
                if (pos < 0) {
                    // 没有"ERR"，则退出循环
                    strRet += output.Substring(start);
                    break;
                }
                if (output.Substring(pos + 3, 2) == "94") {
                    // 不过滤"ERR94"，因为遇到"ERR94"需要重新初始化ELM327
                    strRet += output.Substring(start, pos - start + 5);
                    start = pos + 5;
                } else {
                    strRet += output.Substring(start, pos - start);
                    start = pos + 5;
                }
            }
            return strRet;
        }

        protected string GetNRC(string strData, int headLen) {
            string strNRC = "";
            if (strData.Length > headLen + 2) {
                bool result = int.TryParse(strData.Substring(headLen, 2), out int len);
                if (result) {
                    string strActual = strData.Substring(headLen + 2);
                    if (strActual.Length == len * 2 && strActual.Substring(0, 2) == "7F") {
                        strNRC = strActual.Substring(strActual.Length - 2, 2);
                    }
                }
            }
            return strNRC;
        }

    }
}