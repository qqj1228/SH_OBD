using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SH_OBD {
    public static class Utility {
        public static double Text2Double(string text) {
            if (double.TryParse(text, out double value)) {
                return value;
            }
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) {
                return value;
            }
            return 0.0;
        }

        /// <summary>
        /// RangeInByte取值为1、2、3，用于处理数据长度分别为1byte、2byte、3byte的值
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="RangeInByte"></param>
        /// <returns></returns>
        public static int Int2SInt(int Num, int RangeInByte) {
            int iRet;
            uint uNum = (uint)Num;
            switch (RangeInByte) {
            case 1:
                if ((uNum & 0x80) == 0x80) {
                    iRet = (int)(((~uNum) & 0x7F) + 1) * -1;
                } else {
                    iRet = Num;
                }
                break;
            case 2:
                if ((uNum & 0x8000) == 0x8000) {
                    iRet = (int)(((~uNum) & 0x7FFF) + 1) * -1;
                } else {
                    iRet = Num;
                }
                break;
            case 3:
                if ((uNum & 0x800000) == 0x800000) {
                    iRet = (int)(((~uNum) & 0x7FFFFF) + 1) * -1;
                } else {
                    iRet = Num;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException("Wrong RangeInByte");
            }
            return iRet;
        }

        public static int Hex2Int(string strHex) {
            int value = 0;
            foreach (char digit in strHex) {
                value <<= 4;
                value |= Hex2Int(digit);
            }
            return value;
        }

        public static int Hex2Int(char digit) {
            digit = char.ToUpperInvariant(digit);
            if (digit >= 'A' && digit <= 'F') {
                return Convert.ToInt32(digit - 'A' + 0xA);
            }
            if (digit >= '0' && digit <= '9') {
                return Convert.ToInt32(digit - '0');
            }
            return 0;
        }

        public static string Int2Hex2(int value) {
            if (value < 0 || value > (int)byte.MaxValue) {
                return "";
            }
            return (Int2Hex1(value >> 4) + Int2Hex1(value));
        }

        public static string Int2Hex1(int value) {
            value &= 0x0F;
            if (value >= 0x0A) {
                value += ('A' - 0x0A);
            } else {
                value += '0';
            }
            return char.ToString(Convert.ToChar(value));
        }

        public static string HexStrToASCIIStr(string strHex) {
            string str = "";
            if (strHex.Length > 0) {
                for (int i = 0; i < strHex.Length; i += 2) {
                    int num = Hex2Int(strHex.Substring(i, 2));
                    if (num != 0) {
                        str += new string((char)num, 1);
                    }
                }
            }
            return str;
        }

    }

    // 获取文件版本类
    public static class MainFileVersion {
        public static Version AssemblyVersion {
            get { return ((Assembly.GetEntryAssembly()).GetName()).Version; }
        }

        public static Version AssemblyFileVersion {
            get { return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion); }
        }

        public static string AssemblyInformationalVersion {
            get { return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion; }
        }
    }

}
