using System;

namespace SH_OBD {
    public class OBDResponse {
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
        public string Header { get; set; }
        public bool IsValid { get; set; }

        public OBDResponse() {
            Data = "";
        }

        public string GetDataByte(int index) {
            index *= 2;
            if (index + 2 > Data.Length) {
                return "";
            }
            return Data.Substring(index, 2);
        }

        public int GetDataByteCount() {
            return Data.Length / 2;
        }
    }
}
