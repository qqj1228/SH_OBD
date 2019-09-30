using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    /// <summary>
    /// 用于接收OBD的返回值
    /// </summary>
    public class OBDParameterValue {
        public List<string> ListStringValue { get; set; }
        public bool ErrorDetected { get; set; }
        public bool BoolValue { get; set; }
        public double DoubleValue { get; set; }
        public string StringValue { get; set; }
        public string ShortStringValue { get; set; }
        public string ECUResponseID { get; set; }
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

        public void SetDataWithBool(bool bValue) {
            BoolValue = bValue;
            if (bValue) {
                DoubleValue = 1.0;
                StringValue = "YES";
                ShortStringValue = "YES";
            } else {
                DoubleValue = 0.0;
                StringValue = "NO";
                ShortStringValue = "NO";
            }
        }

        public void SetBoolValueWithData(int dataByte, int index, bool reverse = false) {
            if (reverse) {
                if ((dataByte & (1 << index)) == 0) {
                    SetDataWithBool(true);
                } else {
                    SetDataWithBool(false);
                }
            } else {
                if ((dataByte & (1 << index)) == 0) {
                    SetDataWithBool(false);
                } else {
                    SetDataWithBool(true);
                }
            }
        }

        public void SetBitFlagBAT(int num0) {
            SetBitFlag(0, ((num0 >> 7) & 1) == 1 ? true : false);
            SetBitFlag(1, ((num0 >> 6) & 1) == 1 ? true : false);
            SetBitFlag(2, ((num0 >> 5) & 1) == 1 ? true : false);
            SetBitFlag(3, ((num0 >> 4) & 1) == 1 ? true : false);
            SetBitFlag(4, ((num0 >> 3) & 1) == 1 ? true : false);
            SetBitFlag(5, ((num0 >> 2) & 1) == 1 ? true : false);
            SetBitFlag(6, ((num0 >> 1) & 1) == 1 ? true : false);
            SetBitFlag(7, (num0 & 1) == 1 ? true : false);
        }

        public void SetBitFlagBAT(int num0, int num1) {
            SetBitFlagBAT(num0);

            SetBitFlag(8, ((num1 >> 7) & 1) == 1 ? true : false);
            SetBitFlag(9, ((num1 >> 6) & 1) == 1 ? true : false);
            SetBitFlag(10, ((num1 >> 5) & 1) == 1 ? true : false);
            SetBitFlag(11, ((num1 >> 4) & 1) == 1 ? true : false);
            SetBitFlag(12, ((num1 >> 3) & 1) == 1 ? true : false);
            SetBitFlag(13, ((num1 >> 2) & 1) == 1 ? true : false);
            SetBitFlag(14, ((num1 >> 1) & 1) == 1 ? true : false);
            SetBitFlag(15, (num1 & 1) == 1 ? true : false);
        }

        public void SetBitFlagBAT(int num0, int num1, int num2) {
            SetBitFlagBAT(num0, num1);

            SetBitFlag(16, ((num2 >> 7) & 1) == 1 ? true : false);
            SetBitFlag(17, ((num2 >> 6) & 1) == 1 ? true : false);
            SetBitFlag(18, ((num2 >> 5) & 1) == 1 ? true : false);
            SetBitFlag(19, ((num2 >> 4) & 1) == 1 ? true : false);
            SetBitFlag(20, ((num2 >> 3) & 1) == 1 ? true : false);
            SetBitFlag(21, ((num2 >> 2) & 1) == 1 ? true : false);
            SetBitFlag(22, ((num2 >> 1) & 1) == 1 ? true : false);
            SetBitFlag(23, (num2 & 1) == 1 ? true : false);
        }

        public void SetBitFlagBAT(int num0, int num1, int num2, int num3) {
            SetBitFlagBAT(num0, num1, num2);

            SetBitFlag(24, ((num3 >> 7) & 1) == 1 ? true : false);
            SetBitFlag(25, ((num3 >> 6) & 1) == 1 ? true : false);
            SetBitFlag(26, ((num3 >> 5) & 1) == 1 ? true : false);
            SetBitFlag(27, ((num3 >> 4) & 1) == 1 ? true : false);
            SetBitFlag(28, ((num3 >> 3) & 1) == 1 ? true : false);
            SetBitFlag(29, ((num3 >> 2) & 1) == 1 ? true : false);
            SetBitFlag(30, ((num3 >> 1) & 1) == 1 ? true : false);
            SetBitFlag(31, (num3 & 1) == 1 ? true : false);
        }

    }
}
