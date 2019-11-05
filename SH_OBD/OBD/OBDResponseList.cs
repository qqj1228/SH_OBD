using System;
using System.Collections;
using System.Collections.Generic;

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

    public class OBDResponseList {
        private readonly List<OBDResponse> m_Responses;
        public bool ErrorDetected { get; set; }
        public string RawResponse { get; set; }
        public bool Pending { get; set; }

        public int ResponseCount {
            get { return m_Responses.Count; }
        }

        public OBDResponseList(string response) {
            RawResponse = response;
            ErrorDetected = false;
            Pending = false;
            m_Responses = new List<OBDResponse>();
        }

        public void AddOBDResponse(OBDResponse response) {
            m_Responses.Add(response);
        }

        public OBDResponse GetOBDResponse(int index) {
            if (index < m_Responses.Count) {
                return m_Responses[index];
            }
            return null;
        }
    }
}
