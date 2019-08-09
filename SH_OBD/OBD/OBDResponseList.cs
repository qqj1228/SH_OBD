using System;
using System.Collections;
using System.Collections.Generic;

namespace SH_OBD {
    public class OBDResponseList {
        private readonly List<OBDResponse> m_Responses;
        public bool ErrorDetected { get; set; }
        public string RawResponse { get; set; }

        public int ResponseCount {
            get { return m_Responses.Count; }
        }

        public OBDResponseList(string response) {
            RawResponse = response;
            ErrorDetected = false;
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
