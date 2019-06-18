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

        public static int Hex2Int(string strHex) {
            int value = 0;
            foreach (char digit in strHex) {
                value <<= 4;
                value |= Hex2Int(digit);
            }
            return value;
        }

        public static int Hex2Int(char digit) {
            digit = Char.ToUpperInvariant(digit);
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
            return Char.ToString(Convert.ToChar(value));
        }
    }

    // 获取文件版本类
    public class MainFileVersion {
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
