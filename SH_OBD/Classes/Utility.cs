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
                    if (num >= 0x20 && num < 0x7F) {
                        str += new string((char)num, 1);
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 返回一个便于阅读的十六进制字符串
        /// </summary>
        /// <param name="offset">跳过头部offset个字符不做处理</param>
        /// <param name="strHex">原始十六进制字符串</param>
        /// <returns></returns>
        public static string GetReadableHexString(int offset, string strHex) {
            string strRet = "";
            if (offset > 0) {
                strRet += strHex.Substring(0, offset) + " ";
            }
            for (int i = 0; i < strHex.Length - offset; i++) {
                if (i % 2 != 0) {
                    strRet += strHex.Substring(offset + i - 1, 2) + " ";
                }
            }
            return strRet.TrimEnd();
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

    public static class OrderArray {
        /// <summary>
        /// 对二维数组排序
        /// </summary>
        /// <param name="values">排序的二维数组</param>
        /// <param name="orderColumnsIndexs">排序根据的列的索引号数组</param>
        /// <param name="desc">是否采用降序排序</param>
        /// <returns>返回排序后的二维数组</returns>
        public static T[,] Orderby<T>(T[,] values, int[] orderColumnsIndexs, bool desc = false) {
            T[] temp;
            int k;
            int compareResult;
            for (int i = 0; i < values.GetLength(0); i++) {
                for (k = i + 1; k < values.GetLength(0); k++) {
                    for (int h = 0; h < orderColumnsIndexs.Length; h++) {
                        compareResult = Comparer<T>.Default.Compare(GetRowByID(values, k)[orderColumnsIndexs[h]], GetRowByID(values, i)[orderColumnsIndexs[h]]);
                        if (compareResult == (desc ? 1 : -1)) {
                            temp = GetRowByID(values, i);
                            Array.Copy(values, k * values.GetLength(1), values, i * values.GetLength(1), values.GetLength(1));
                            CopyToRow(values, k, temp);
                        }
                        if (compareResult != 0) {
                            break;
                        }
                    }
                }
            }
            return values;
        }

        /// <summary>
        /// 获取二维数组中一行的数据
        /// </summary>
        /// <param name="values">二维数据</param>
        /// <param name="rowID">行ID</param>
        /// <returns>返回一行的数据</returns>
        static T[] GetRowByID<T>(T[,] values, int rowID) {
            if (rowID > (values.GetLength(0) - 1)) {
                throw new Exception("rowID超出最大的行索引号!");
            }

            T[] row = new T[values.GetLength(1)];
            for (int i = 0; i < values.GetLength(1); i++) {
                row[i] = values[rowID, i];
            }
            return row;

        }

        /// <summary>
        /// 复制一行数据到二维数组指定的行上
        /// </summary>
        /// <param name="values"></param>
        /// <param name="rowID"></param>
        /// <param name="row"></param>
        static void CopyToRow<T>(T[,] values, int rowID, T[] row) {
            if (rowID > (values.GetLength(0) - 1)) {
                throw new Exception("rowID超出最大的行索引号!");
            }
            if (row.Length > (values.GetLength(1))) {
                throw new Exception("row行数据列数超过二维数组的列数!");
            }
            for (int i = 0; i < row.Length; i++) {
                values[rowID, i] = row[i];
            }
        }
    }
}
