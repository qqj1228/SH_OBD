using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public class OBDInterpreter {
        public static string HexStringToASCIIString(string strHex) {
            string str = "";
            if (strHex.Length > 0) {
                for (int i = 0; i < strHex.Length; i += 2) {
                    int num2 = Utility.Hex2Int(strHex.Substring(i, 2));
                    if (num2 != 0) {
                        str += new string((char)num2, 1);
                    }
                }
            }
            return str;
        }

        public static OBDParameterValue GetPIDFlag(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int num8 = Utility.Hex2Int(response.GetDataByte(0));
            int num7 = Utility.Hex2Int(response.GetDataByte(1));
            int num6 = Utility.Hex2Int(response.GetDataByte(2));
            int num5 = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlag(0, ((num8 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(1, ((num8 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(2, ((num8 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(3, ((num8 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(4, ((num8 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(5, ((num8 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(6, ((num8 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(7, (num8 & 1) == 1 ? true : false);
            value2.SetBitFlag(8, ((num7 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(9, ((num7 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(10, ((num7 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(11, ((num7 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(12, ((num7 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(13, ((num7 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(14, ((num7 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(15, (num7 & 1) == 1 ? true : false);
            value2.SetBitFlag(16, ((num6 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(17, ((num6 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(18, ((num6 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(19, ((num6 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(20, ((num6 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(21, ((num6 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(22, ((num6 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(23, (num6 & 1) == 1 ? true : false);
            value2.SetBitFlag(24, ((num5 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(25, ((num5 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(26, ((num5 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(27, ((num5 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(28, ((num5 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(29, ((num5 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(30, ((num5 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(31, (num5 & 1) == 1 ? true : false);
            return value2;
        }

        public static OBDParameterValue GetDTCStatus(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            switch (param.SubParameter) {
            case 0:
                // MIL
                if ((Utility.Hex2Int(response.GetDataByte(0)) & 0x80) == 0) {
                    return new OBDParameterValue(false, 0.0, "OFF", "OFF");
                }
                return new OBDParameterValue(true, 1.0, "ON", "ON");
            case 1:
                // DTC数量
                int num12 = Utility.Hex2Int(response.GetDataByte(0));
                if ((num12 & 0x80) != 0) {
                    num12 -= 0x80;
                }
                value2.DoubleValue = num12;
                return value2;
            case 2:
                // 失火支持？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 1) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 3:
                // 燃油系统支持？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 2) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 4:
                // 综合组件支持？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 4) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 5:
                // 失火完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x10) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 6:
                // 燃油系统完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x20) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 7:
                // 综合组件完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x40) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 8:
                // 催化器支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 1) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 9:
                // 加热催化器支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 2) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 10:
                // 燃油蒸发系统支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 4) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 11:
                // 二次空气系统支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 8) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 12:
                // 空调系统制冷剂支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x10) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 13:
                // 氧气传感器支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x20) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 14:
                // 加热氧气传感器支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x40) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 15:
                // EGR系统支持？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x80) == 0) {
                    return new OBDParameterValue(false, 0.0, "NO", "NO");
                }
                return new OBDParameterValue(true, 1.0, "YES", "YES");
            case 0x10:
                // 催化器完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 1) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x11:
                // 加热催化器完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 2) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x12:
                // 燃油蒸发系统完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 4) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x13:
                // 二次空气系统完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 8) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 20:
                // 空调系统制冷剂完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x10) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x15:
                // 氧气传感器完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x20) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x16:
                // 加热氧气传感器完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x40) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            case 0x17:
                // EGR系统完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x80) == 0) {
                    return new OBDParameterValue(true, 1.0, "YES", "YES");
                }
                return new OBDParameterValue(false, 0.0, "NO", "NO");
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetFuelStatus(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 2) {
                value2.ErrorDetected = true;
                return value2;
            }
            int num;
            if (param.SubParameter == 0) {
                // 燃油系统 1 状态
                num = Utility.Hex2Int(response.GetDataByte(0));
            } else {
                // 燃油系统 2 状态
                num = Utility.Hex2Int(response.GetDataByte(1));
            }
            if ((num & 1) != 0) {
                value2.StringValue = "开环：尚未满足闭环条件";
                value2.ShortStringValue = "OL";
                return value2;
            }
            if ((num & 2) != 0) {
                value2.StringValue = "闭环：使用氧传感器作为燃油控制的反馈";
                value2.ShortStringValue = "CL";
                return value2;
            }
            if ((num & 4) != 0) {
                value2.StringValue = "由于驾驶条件而开环（例如，功率提升、减速消耗）";
                value2.ShortStringValue = "OL-Drive";
                return value2;
            }
            if ((num & 8) != 0) {
                value2.StringValue = "由于检测到的系统故障而开环";
                value2.ShortStringValue = "OL-Fault";
                return value2;
            }
            if ((num & 0x10) != 0) {
                value2.StringValue = "闭环，但至少有一个氧气故障传感器 - 可能使用单氧传感器作为燃料控制";
                value2.ShortStringValue = "CL-Fault";
                return value2;
            }
            value2.StringValue = "不适用";
            value2.ShortStringValue = value2.StringValue;
            return value2;
        }

        /// <summary>
        /// b13Or1D：true - 用于PID 13，false - 用于PID 1D
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="b13Or1D"></param>
        /// <returns></returns>
        public static OBDParameterValue GetO2SLocation(OBDParameter param, OBDResponse response, bool b13Or1D) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            int num = Utility.Hex2Int(response.GetDataByte(0));
            value2.SetBitFlag(0, ((num >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(1, ((num >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(2, ((num >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(3, ((num >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(4, ((num >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(5, ((num >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(6, ((num >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(7, (num & 1) == 1 ? true : false);

            switch (param.SubParameter) {
            case 0:
                if ((num & 1) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "组 1 传感器 1，没出现在该位置";
                    value2.ShortStringValue = "O2B1S1: NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "组 1 传感器 1，出现在该位置";
                    value2.ShortStringValue = "O2B1S1: YES";
                }
                return value2;
            case 1:
                if ((num & 2) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "组 1 传感器 2，没出现在该位置";
                    value2.ShortStringValue = "O2B1S2: NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "组 1 传感器 2，出现在该位置";
                    value2.ShortStringValue = "O2B1S2: YES";
                }
                return value2;
            case 2:
                if ((num & 4) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 4) == 0) {
                        value2.StringValue = "组 1 传感器 3，没出现在该位置";
                        value2.ShortStringValue = "O2B1S3: NO";
                    } else {
                        value2.StringValue = "组 1 传感器 3，出现在该位置";
                        value2.ShortStringValue = "O2B1S3: YES";
                    }
                } else {
                    if ((num & 4) == 0) {
                        value2.StringValue = "组 2 传感器 1，没出现在该位置";
                        value2.ShortStringValue = "O2B2S1: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 1，出现在该位置";
                        value2.ShortStringValue = "O2B2S1: YES";
                    }
                }
                return value2;
            case 3:
                if ((num & 8) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 8) == 0) {
                        value2.StringValue = "组 1 传感器 4，没出现在该位置";
                        value2.ShortStringValue = "O2B1S4: NO";
                    } else {
                        value2.StringValue = "组 1 传感器 4，出现在该位置";
                        value2.ShortStringValue = "O2B1S4: YES";
                    }
                } else {
                    if ((num & 8) == 0) {
                        value2.StringValue = "组 2 传感器 2，没出现在该位置";
                        value2.ShortStringValue = "O2B2S2: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 2，出现在该位置";
                        value2.ShortStringValue = "O2B2S2: YES";
                    }
                }
                return value2;
            case 4:
                if ((num & 0x10) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 0x10) == 0) {
                        value2.StringValue = "组 2 传感器 1，没出现在该位置";
                        value2.ShortStringValue = "O2B2S1: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 1，出现在该位置";
                        value2.ShortStringValue = "O2B2S1: YES";
                    }
                } else {
                    if ((num & 0x10) == 0) {
                        value2.StringValue = "组 3 传感器 1，没出现在该位置";
                        value2.ShortStringValue = "O2B3S1: NO";
                    } else {
                        value2.StringValue = "组 3 传感器 1，出现在该位置";
                        value2.ShortStringValue = "O2B3S1: YES";
                    }
                }
                return value2;
            case 5:
                if ((num & 0x20) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 0x20) == 0) {
                        value2.StringValue = "组 2 传感器 2，没出现在该位置";
                        value2.ShortStringValue = "O2B2S2: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 2，出现在该位置";
                        value2.ShortStringValue = "O2B2S2: YES";
                    }
                } else {
                    if ((num & 0x20) == 0) {
                        value2.StringValue = "组 3 传感器 2，没出现在该位置";
                        value2.ShortStringValue = "O2B3S2: NO";
                    } else {
                        value2.StringValue = "组 3 传感器 2，出现在该位置";
                        value2.ShortStringValue = "O2B3S2: YES";
                    }
                }
                return value2;
            case 6:
                if ((num & 0x40) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 0x40) == 0) {
                        value2.StringValue = "组 2 传感器 3，没出现在该位置";
                        value2.ShortStringValue = "O2B2S3: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 3，出现在该位置";
                        value2.ShortStringValue = "O2B2S3: YES";
                    }
                } else {
                    if ((num & 0x40) == 0) {
                        value2.StringValue = "组 4 传感器 1，没出现在该位置";
                        value2.ShortStringValue = "O2B4S1: NO";
                    } else {
                        value2.StringValue = "组 4 传感器 1，出现在该位置";
                        value2.ShortStringValue = "O2B4S1: YES";
                    }
                }
                return value2;
            case 7:
                if ((num & 0x80) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                }
                if (b13Or1D) {
                    if ((num & 0x40) == 0) {
                        value2.StringValue = "组 2 传感器 4，没出现在该位置";
                        value2.ShortStringValue = "O2B2S4: NO";
                    } else {
                        value2.StringValue = "组 2 传感器 4，出现在该位置";
                        value2.ShortStringValue = "O2B2S4: YES";
                    }
                } else {
                    if ((num & 0x40) == 0) {
                        value2.StringValue = "组 4 传感器 2，没出现在该位置";
                        value2.ShortStringValue = "O2B4S2: NO";
                    } else {
                        value2.StringValue = "组 4 传感器 2，出现在该位置";
                        value2.ShortStringValue = "O2B4S2: YES";
                    }
                }
                return value2;
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetOBDType(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            int num = Utility.Hex2Int(response.GetDataByte(0));
            switch (num) {
            case 1:
                value2.StringValue = "OBD II (加利福尼亚 ARB)";
                value2.ShortStringValue = "OBDII CARB";
                return value2;
            case 2:
                value2.StringValue = "OBD (联邦环保局)";
                value2.ShortStringValue = "OBD (Fed)";
                return value2;
            case 3:
                value2.StringValue = "OBD 和 OBD II";
                value2.ShortStringValue = "OBD/OBDII";
                return value2;
            case 4:
                value2.StringValue = "OBD I";
                value2.ShortStringValue = "OBDI";
                return value2;
            case 5:
                value2.StringValue = "不兼容OBD";
                value2.ShortStringValue = "NO OBD";
                return value2;
            case 6:
                value2.StringValue = "EOBD";
                value2.ShortStringValue = "EOBD";
                return value2;
            case 7:
                value2.StringValue = "EOBD 和 OBD II";
                value2.ShortStringValue = "EOBD/OBDII";
                return value2;
            case 8:
                value2.StringValue = "EOBD 和 OBD";
                value2.ShortStringValue = "EOBD/OBD";
                return value2;
            case 9:
                value2.StringValue = "EOBD, OBD 和 OBD II";
                value2.ShortStringValue = "EOBD/OBD/OBDII";
                return value2;
            case 0x0A:
                value2.StringValue = "JOBD";
                value2.ShortStringValue = "JOBD";
                return value2;
            case 0x0B:
                value2.StringValue = "JOBD 和 OBD II";
                value2.ShortStringValue = "JOBD/OBDII";
                return value2;
            case 0x0C:
                value2.StringValue = "JOBD 和 EOBD";
                value2.ShortStringValue = "JOBD/EOBD";
                return value2;
            case 0x0D:
                value2.StringValue = "JOBD, EOBD, 和 OBD II";
                value2.ShortStringValue = "JOBD/EOBD/OBDII";
                return value2;
            case 0x0E:
                value2.StringValue = "重型车辆 (欧四) B1";
                value2.ShortStringValue = "EURO IV B1";
                return value2;
            case 0x0F:
                value2.StringValue = "重型车辆 (欧五) B2";
                value2.ShortStringValue = "EURO V B2";
                return value2;
            case 0x10:
                value2.StringValue = "重型车辆 (欧共体) C (燃气发动机)";
                value2.ShortStringValue = "EURO C";
                return value2;
            case 0x11:
                value2.StringValue = "发动机制造商诊断用 (EMD)";
                value2.ShortStringValue = "EMD";
                return value2;
            default:
                if (num >= 0x12 && num <= 0xFA) {
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = "---";
                } else if (num >= 0xFB && num <= 0xFF) {
                    value2.StringValue = "ISO/SAE - 不用于分配";
                    value2.ShortStringValue = "SAE J1939 特殊用途";
                }
                return value2;
            }
        }

        public static OBDParameterValue GetCurrentStatus(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int num8 = Utility.Hex2Int(response.GetDataByte(0));
            int num7 = Utility.Hex2Int(response.GetDataByte(1));
            int num6 = Utility.Hex2Int(response.GetDataByte(2));
            int num5 = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlag(0, ((num8 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(1, ((num8 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(2, ((num8 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(3, ((num8 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(4, ((num8 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(5, ((num8 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(6, ((num8 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(7, (num8 & 1) == 1 ? true : false);
            value2.SetBitFlag(8, ((num7 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(9, ((num7 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(10, ((num7 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(11, ((num7 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(12, ((num7 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(13, ((num7 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(14, ((num7 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(15, (num7 & 1) == 1 ? true : false);
            value2.SetBitFlag(16, ((num6 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(17, ((num6 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(18, ((num6 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(19, ((num6 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(20, ((num6 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(21, ((num6 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(22, ((num6 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(23, (num6 & 1) == 1 ? true : false);
            value2.SetBitFlag(24, ((num5 >> 7) & 1) == 1 ? true : false);
            value2.SetBitFlag(25, ((num5 >> 6) & 1) == 1 ? true : false);
            value2.SetBitFlag(26, ((num5 >> 5) & 1) == 1 ? true : false);
            value2.SetBitFlag(27, ((num5 >> 4) & 1) == 1 ? true : false);
            value2.SetBitFlag(28, ((num5 >> 3) & 1) == 1 ? true : false);
            value2.SetBitFlag(29, ((num5 >> 2) & 1) == 1 ? true : false);
            value2.SetBitFlag(30, ((num5 >> 1) & 1) == 1 ? true : false);
            value2.SetBitFlag(31, (num5 & 1) == 1 ? true : false);

            switch (param.SubParameter) {
            case 0:
                // 失火在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 1) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 1:
                // 燃油系统在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 2) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 2:
                // 综合组件在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 4) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 3:
                // 失火在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x10) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 4:
                // 燃油系统在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x20) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 5:
                // 综合组件在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(1)) & 0x40) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 6:
                // 催化器在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 1) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 7:
                // 加热催化器在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 2) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 8:
                // 燃油蒸发系统在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 4) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 9:
                // 二次空气系统在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 8) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 10:
                // 空调系统制冷剂在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x10) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 11:
                // 氧气传感器在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x20) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 12:
                // 加热氧气传感器在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x40) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 13:
                // EGR系统在当前驾驶循环中可用？
                if ((Utility.Hex2Int(response.GetDataByte(2)) & 0x80) == 0) {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                } else {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                }
                return value2;
            case 14:
                // 催化器在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 1) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 15:
                // 加热催化器在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 2) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 0x10:
                // 燃油蒸发系统在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 4) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 0x11:
                // 二次空气系统在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 8) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 0x12:
                // 空调系统制冷剂在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x10) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 0x13:
                // 氧气传感器在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x20) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 20:
                // 加热氧气传感器在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x40) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            case 0x15:
                // EGR系统在当前驾驶循环中完成？
                if ((Utility.Hex2Int(response.GetDataByte(3)) & 0x80) == 0) {
                    value2.BoolValue = true;
                    value2.DoubleValue = 1.0;
                    value2.StringValue = "YES";
                    value2.ShortStringValue = "YES";
                } else {
                    value2.BoolValue = false;
                    value2.DoubleValue = 0.0;
                    value2.StringValue = "NO";
                    value2.ShortStringValue = "NO";
                }
                return value2;
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetFuelType(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            switch (Utility.Hex2Int(response.GetDataByte(0))) {
            case 1:
                value2.StringValue = "汽油/石油";
                value2.ShortStringValue = "GAS";
                break;
            case 2:
                value2.StringValue = "甲醇";
                value2.ShortStringValue = "METH";
                break;
            case 3:
                value2.StringValue = "乙醇";
                value2.ShortStringValue = "ETH";
                break;
            case 4:
                value2.StringValue = "柴油";
                value2.ShortStringValue = "DSL";
                break;
            case 5:
                value2.StringValue = "液化石油气（LPG）";
                value2.ShortStringValue = "LPG";
                break;
            case 6:
                value2.StringValue = "压缩天然气（CNG）";
                value2.ShortStringValue = "CNG";
                break;
            case 7:
                value2.StringValue = "丙烷";
                value2.ShortStringValue = "PROP";
                break;
            case 8:
                value2.StringValue = "电池/电动";
                value2.ShortStringValue = "ELEC";
                break;
            case 9:
                value2.StringValue = "使用汽油的双燃料车辆";
                value2.ShortStringValue = "BI_GAS";
                break;
            case 10:
                value2.StringValue = "使用甲醇的双燃料车辆";
                value2.ShortStringValue = "BI_METH";
                break;
            case 11:
                value2.StringValue = "使用乙醇的双燃料车辆";
                value2.ShortStringValue = "BI_ETH";
                break;
            case 12:
                value2.StringValue = "使用LPG的双燃料车辆";
                value2.ShortStringValue = "BI_LPG";
                break;
            case 13:
                value2.StringValue = "使用CNG的双燃料车辆";
                value2.ShortStringValue = "BI_CNG";
                break;
            case 14:
                value2.StringValue = "使用丙烷的双燃料车辆";
                value2.ShortStringValue = "BI_PROP";
                break;
            case 15:
                value2.StringValue = "使用电池的双燃料车辆";
                value2.ShortStringValue = "BI_ELEC";
                break;
            /* 以下类型不在ISO15031-5标准内 */
            case 16:
                value2.StringValue = "使用汽油/电动混合动力的双燃料车辆";
                value2.ShortStringValue = "GAS_ELEC";
                break;
            case 17:
                value2.StringValue = "汽油混合动力";
                value2.ShortStringValue = "HY_GAS";
                break;
            case 18:
                value2.StringValue = "乙醇混合动力";
                value2.ShortStringValue = "HY_ETH";
                break;
            case 19:
                value2.StringValue = "柴油混合动力";
                value2.ShortStringValue = "HY_DSL";
                break;
            case 20:
                value2.StringValue = "电动混合动力";
                value2.ShortStringValue = "HY_ELEC";
                break;
            case 21:
                value2.StringValue = "燃料混合动力";
                value2.ShortStringValue = "HY_FUEL";
                break;
            case 22:
                value2.StringValue = "混合再生动力";
                value2.ShortStringValue = "HY_REG";
                break;
            default:
                value2.StringValue = "ISO/SAE 保留";
                value2.ShortStringValue = "——";
                break;
            }
            return value2;
        }

        public static OBDParameterValue GetMode0102Value(OBDParameter param, OBDResponse response, bool bEnglishUnits = false) {
            OBDParameterValue value2 = new OBDParameterValue();
            int num;

            switch (param.Parameter) {
            case 0:
            case 0x20:
            case 0x40:
            case 0x60:
            case 0x80:
                return GetPIDFlag(response);
            case 1:
                return GetDTCStatus(param, response);
            case 2:
                // 引起冻结帧的DTC
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.StringValue = GetDTCName(response.Data);
                return value2;
            case 3:
                return GetFuelStatus(param, response);
            case 4:
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                return value2;
            case 5:
                // 引擎冷却液温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round((value2.DoubleValue * 1.8) + 32.0, 2);
                }
                return value2;
            case 6:
            case 7:
            case 8:
            case 9:
                // 长/短时燃油修正 组 1/2/3/4
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 组 1/2
                    num = Utility.Hex2Int(response.GetDataByte(0));
                } else {
                    // 组 3/4
                    num = Utility.Hex2Int(response.GetDataByte(1));
                }
                value2.DoubleValue = Math.Round((num * 0.78125) - 100.0, 2);
                return value2;
            case 0x0A:
                // 燃油导轨压力（表压）
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 3.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x0B:
                // 进气歧管绝对压力
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                return value2;
            case 0x0C:
                // 引擎转速
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 0x100) + Utility.Hex2Int(response.GetDataByte(1))) * 0.25;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x0D:
                // 车速
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round(value2.DoubleValue * 0.621371192, 2);
                }
                return value2;
            case 0x0E:
                // #1缸点火正时
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 0.5) - 64.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x0F:
                // 进气温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round((value2.DoubleValue * 1.8) + 32.0, 2);
                }
                return value2;
            case 0x10:
                // 空气质量流量率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.01;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.13227735731092655;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x11:
                // 节气门绝对位置
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                return value2;
            case 0x12:
                // 指令的二次空气状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                num = Utility.Hex2Int(response.GetDataByte(0));
                if ((num & 1) != 0) {
                    value2.StringValue = "第一催化转化器的上游";
                    value2.ShortStringValue = "UPS";
                    return value2;
                }
                if ((num & 2) != 0) {
                    value2.StringValue = "第一催化转化器入口的下游";
                    value2.ShortStringValue = "DNS";
                    return value2;
                }
                if ((num & 4) != 0) {
                    value2.StringValue = "大气 / 关闭";
                    value2.ShortStringValue = "OFF";
                }
                return value2;
            case 0x13:
                return GetO2SLocation(param, response, true);
            case 0x14:
            case 0x15:
            case 0x16:
            case 0x17:
            case 0x18:
            case 0x19:
            case 0x1A:
            case 0x1B:
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 氧传感器输出电压
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.005;
                } else {
                    // 短时燃油修正
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.78125 - 100.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x1C:
                return GetOBDType(response);
            case 0x1D:
                return GetO2SLocation(param, response, false);
            case 0x1E:
                // 动力输出PTO状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if ((Utility.Hex2Int(response.GetDataByte(0)) & 1) != 0) {
                    value2.DoubleValue = 1.0;
                    value2.BoolValue = true;
                    value2.StringValue = "ON";
                    value2.ShortStringValue = "ON";
                } else {
                    value2.DoubleValue = 0.0;
                    value2.BoolValue = false;
                    value2.StringValue = "OFF";
                    value2.ShortStringValue = "OFF";
                }
                return value2;
            case 0x1F:
                // 引擎点火后运行时间
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x21:
                // MIL亮起后行驶距离
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.621371192;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x22:
                // 燃油导轨压力（相对于歧管真空压力）
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 5178.0 / 65535.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x23:
                // 燃油导轨压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 10.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x24:
            case 0x25:
            case 0x26:
            case 0x27:
            case 0x28:
            case 0x29:
            case 0x2A:
            case 0x2B:
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 当量比（λ）
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                } else {
                    // 氧气传感器电压
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(2)) * 256.0) + Utility.Hex2Int(response.GetDataByte(3))) * 8.0 / 65535.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x2C:
                // 指令的EGR
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                return value2;
            case 0x2D:
                // EGR错误
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 0.78125 - 100.0, 2);
                return value2;
            case 0x2E:
                // 指令的燃油蒸发排放
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                return value2;
            case 0x2F:
                // 燃油量输入
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                return value2;
            case 0x30:
                // DTC清除后热车次数
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                return value2;
            case 0x31:
                // DTC清除后行驶距离
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.621371192;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                return value2;
            case 0x32:
                // 蒸发排放系统燃油蒸汽压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                num = Utility.Int2SInt((Utility.Hex2Int(response.GetDataByte(0)) * 256) + Utility.Hex2Int(response.GetDataByte(1)), 2);
                value2.DoubleValue = num * 0.25;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.000145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x33:
                // 大气压
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                return value2;
            case 0x34:
            case 0x35:
            case 0x36:
            case 0x37:
            case 0x38:
            case 0x39:
            case 0x3A:
            case 0x3B:
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 宽量程氧气传感器当量比（λ）
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                } else {
                    // 宽量程氧气传感器电压
                    value2.DoubleValue = (((Utility.Hex2Int(response.GetDataByte(2)) * 256.0) + Utility.Hex2Int(response.GetDataByte(3))) * 0.00390625) - 128.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x3C:
            case 0x3D:
            case 0x3E:
            case 0x3F:
                // 催化器温度，组 1/2 传感器 1/2
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.1) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x41:
                return GetCurrentStatus(param, response);
            case 0x42:
                // 控制模块电压
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.001;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 3);
                return value2;
            case 0x43:
                // 绝对负载值
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x44:
                // 指令的当量率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x45:
                // 节气门相对位置
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x46:
                // 环境空气温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                return value2;
            case 0x47:
            case 0x48:
                // 节气门绝对位置 B/C
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x49:
            case 0x4A:
            case 0x4B:
            case 0x4C:
                // 油门踏板位置 D/E/F
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x4D:
                // MIL亮起后引擎运转时间
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                return value2;
            case 0x4E:
                // DTC清除后持续时间
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                return value2;
            case 0x4F:
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                switch (param.SubParameter) {
                case 0:
                    // 当量比率最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                    break;
                case 1:
                    // 氧气传感器电压最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1));
                    break;
                case 2:
                    // 氧气传感器电流最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(2));
                    break;
                case 3:
                    // 进气歧管绝对压力最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(3)) * 10.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    return value2;
                }
                return value2;
            case 0x50:
                // 空气质量流量最大值
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 10.0;
                return value2;
            case 0x51:
                return GetFuelType(response);
            case 0x52:
                // 酒精燃料百分比
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x53:
                // 燃料蒸发排放系统蒸汽绝压
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.005;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x54:
                // 燃料蒸发排放系统蒸汽压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Int2SInt((Utility.Hex2Int(response.GetDataByte(0)) * 256) + Utility.Hex2Int(response.GetDataByte(1)), 2);
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.000145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x55:
            case 0x56:
            case 0x57:
            case 0x58:
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 长/短时第二氧气传感器燃油修正 组1/2
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.78125 - 100.0;
                } else {
                    // 长/短时第二氧气传感器燃油修正 组3/4
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.78125 - 100.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x59:
                // 燃料导轨压力（绝压）
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 10.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x5A:
                // 相对油门踏板位置
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            default:
                if (param.Parameter >= 0x5B && param.Parameter <= 0xFF) {
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = "——";
                } else {
                    value2.ErrorDetected = true;
                }
                return value2;
            }
        }

        public static OBDParameterValue GetMode03070AValue(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            List<string> strings = new List<string>();
            for (int i = 0; i <= response.Data.Length - 4; i += 4) {
                string str = GetDTCName(response.Data.Substring(i, 4));
                if (str.CompareTo("P0000") != 0) {
                    strings.Add(str);
                }
            }
            value2.ListStringValue = strings;
            return value2;
        }

        public static OBDParameterValue GetMode05Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            switch (param.Parameter) {
            case 0x00:
                return GetPIDFlag(response);
            case 0x01:
            case 0x02:
            case 0x03:
            case 0x04:
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.005;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x05:
            case 0x06:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 取计算值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.004;
                } else if (param.SubParameter == 1) {
                    // 取最小值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.004;
                } else if (param.SubParameter == 2) {
                    // 取最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(2)) * 0.004;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x07:
            case 0x08:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 取计算值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.005;
                } else if (param.SubParameter == 1) {
                    // 取最小值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.005;
                } else if (param.SubParameter == 2) {
                    // 取最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(2)) * 0.005;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            case 0x09:
            case 0x0A:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                if (param.SubParameter == 0) {
                    // 取计算值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.04;
                } else if (param.SubParameter == 1) {
                    // 取最小值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.04;
                } else if (param.SubParameter == 2) {
                    // 取最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(2)) * 0.04;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                return value2;
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetMode09Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            List<string> strings = new List<string>();
            const int NumOffset = 1 * 2;
            int DataOffset;
            int num;

            switch (param.Parameter) {
            case 0x00:
                return GetPIDFlag(response);
            case 0x01:
            case 0x03:
            case 0x05:
            case 0x07:
            case 0x09:
                // 获取相关InfoType的消息数量，仅适用于非CAN协议
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    return value2;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                return value2;
            case 0x02:
                // VIN
                DataOffset = 17 * 2;
                num = Utility.Hex2Int(response.Data.Substring(0, NumOffset));
                for (int i = 0; i < num; i++) {
                    strings.Add(HexStringToASCIIString(response.Data.Substring(NumOffset + i * DataOffset, DataOffset)));
                }
                value2.ListStringValue = strings;
                return value2;
            case 0x04:
                // CAL ID
                DataOffset = 16 * 2;
                num = Utility.Hex2Int(response.Data.Substring(0, NumOffset));
                for (int i = 0; i < num; i++) {
                    strings.Add(HexStringToASCIIString(response.Data.Substring(NumOffset + i * DataOffset, DataOffset)));
                }
                value2.ListStringValue = strings;
                return value2;
            case 0x06:
                // CVN
                DataOffset = 4 * 2;
                num = Utility.Hex2Int(response.Data.Substring(0, NumOffset));
                for (int i = 0; i < num; i++) {
                    strings.Add(response.Data.Substring(NumOffset + i * DataOffset, DataOffset));
                }
                value2.ListStringValue = strings;
                return value2;
            case 0x08:
                // IPT
                DataOffset = 2 * 2;
                num = Utility.Hex2Int(response.Data.Substring(0, NumOffset));
                for (int i = 0; i < num; i++) {
                    strings.Add(response.Data.Substring(NumOffset + i * DataOffset, DataOffset));
                }
                value2.ListStringValue = strings;
                if (param.SubParameter >= 0 && param.SubParameter < num) {
                    value2.DoubleValue = Utility.Hex2Int(value2.ListStringValue[param.SubParameter]);
                } else {
                    value2.ErrorDetected = true;
                }
                return value2;
            case 0x0A:
                // ECU名称
                DataOffset = 20 * 2;
                num = Utility.Hex2Int(response.Data.Substring(0, NumOffset));
                for (int i = 0; i < num; i++) {
                    strings.Add(HexStringToASCIIString(response.Data.Substring(NumOffset + i * DataOffset, DataOffset)));
                }
                value2.ListStringValue = strings;
                return value2;
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetValue(OBDParameter param, OBDResponse response, bool bEnglishUnits = false) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response == null) {
                value2.ErrorDetected = true;
                return value2;
            }
            switch (param.Service) {
            case 1:
            case 2:
                return GetMode0102Value(param, response, bEnglishUnits);
            case 3:
            case 7:
            case 0x0A:
                return GetMode03070AValue(response);
            case 5:
                return GetMode05Value(param, response);
            case 9:
                return GetMode09Value(param, response);
            default:
                value2.ErrorDetected = true;
                return value2;
            }
        }

        public static OBDParameterValue GetValue(OBDParameter param, OBDResponseList responses, bool bEnglishUnits = false) {
            if (responses.ResponseCount == 1) {
                return GetValue(param, responses.GetOBDResponse(0), bEnglishUnits);
            }
            if ((param.Service == 1) || (param.Service == 2)) {
                if (((param.Parameter == 0) || (param.Parameter == 0x20)) || ((param.Parameter == 0x40) || (param.Parameter == 0x60))) {
                    OBDParameterValue value7 = new OBDParameterValue();
                    for (int i = 0; i < responses.ResponseCount; i++) {
                        OBDParameterValue value4 = GetValue(param, responses.GetOBDResponse(i), bEnglishUnits);
                        if (value4.ErrorDetected) {
                            return value4;
                        }
                        for (int j = 0; j < 0x20; j++) {
                            value7.SetBitFlag(j, value4.GetBitFlag(j));
                        }
                    }
                    return value7;
                }
                if ((param.Parameter == 1) && (param.SubParameter == 0)) {
                    OBDParameterValue value2 = new OBDParameterValue {
                        BoolValue = false
                    };
                    for (int i = 0; i < responses.ResponseCount; i++) {
                        OBDParameterValue value6 = GetValue(param, responses.GetOBDResponse(i), bEnglishUnits);
                        if (value6.ErrorDetected) {
                            return value6;
                        }
                        if (value6.BoolValue) {
                            value2.BoolValue = true;
                        }
                    }
                    if (value2.BoolValue) {
                        value2.DoubleValue = 1.0;
                        value2.StringValue = "ON";
                        value2.ShortStringValue = "ON";
                        return value2;
                    } else {
                        value2.DoubleValue = 0.0;
                        value2.StringValue = "OFF";
                        value2.ShortStringValue = "OFF";
                        return value2;
                    }
                }
                if ((param.Parameter == 1) && (param.SubParameter == 1)) {
                    OBDParameterValue value3 = new OBDParameterValue {
                        DoubleValue = 0.0
                    };
                    for (int i = 0; i < responses.ResponseCount; i++) {
                        OBDParameterValue value5 = GetValue(param, responses.GetOBDResponse(i), bEnglishUnits);
                        if (value5.ErrorDetected) {
                            return value5;
                        }
                        value3.DoubleValue = value5.DoubleValue + value3.DoubleValue;
                    }
                    return value3;
                }
            }
            if ((param.Service != 3) && (param.Service != 7)) {
                return GetValue(param, responses.GetOBDResponse(0), bEnglishUnits);
            }
            OBDParameterValue value8 = new OBDParameterValue();
            List<string> strings = new List<string>();
            for (int i = 0; i < responses.ResponseCount; i++) {
                foreach (string str in GetValue(param, responses.GetOBDResponse(i), bEnglishUnits).ListStringValue) {
                    strings.Add(str);
                }
            }
            value8.ListStringValue = strings;
            return value8;
        }

        public static string GetDTCName(string strHexDTC) {
            if (strHexDTC.Length != 4) {
                return "P0000";
            } else {
                return GetDTCSystem(strHexDTC.Substring(0, 1)) + strHexDTC.Substring(1, 3);
            }
        }

        private static string GetDTCSystem(string strSysId) {
            string strSys;
            switch (strSysId) {
            case "0":
                strSys = "P0";
                break;
            case "1":
                strSys = "P1";
                break;
            case "2":
                strSys = "P2";
                break;
            case "3":
                strSys = "P3";
                break;
            case "4":
                strSys = "C0";
                break;
            case "5":
                strSys = "C1";
                break;
            case "6":
                strSys = "C2";
                break;
            case "7":
                strSys = "C3";
                break;
            case "8":
                strSys = "B0";
                break;
            case "9":
                strSys = "B1";
                break;
            case "A":
                strSys = "B2";
                break;
            case "B":
                strSys = "B3";
                break;
            case "C":
                strSys = "U0";
                break;
            case "D":
                strSys = "U1";
                break;
            case "E":
                strSys = "U2";
                break;
            case "F":
                strSys = "U3";
                break;
            default:
                strSys = "ER";
                break;
            }
            return strSys;
        }
    }
}