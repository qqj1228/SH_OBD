using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    /// <summary>
    /// 用于接收OBD的返回值
    /// </summary>
    public class OBDParameterValue {
        public bool ErrorDetected { get; set; }
        public List<string> ListStringValue { get; set; }
        public bool BoolValue { get; set; }
        public double DoubleValue { get; set; }
        public string StringValue { get; set; }
        public string ShortStringValue { get; set; }
        private readonly bool[] m_bBitFlags;

        public OBDParameterValue(bool bValue, double dValue, string strValue, string shortValue) {
            BoolValue = bValue;
            DoubleValue = dValue;
            StringValue = strValue;
            ShortStringValue = shortValue;
        }

        public OBDParameterValue() {
            StringValue = "";
            ShortStringValue = "";
            m_bBitFlags = new bool[32];
        }

        public bool GetBitFlag(int index) {
            return m_bBitFlags[index];
        }

        public void SetBitFlag(int index, bool status) {
            m_bBitFlags[index] = status;
        }
    }
}
