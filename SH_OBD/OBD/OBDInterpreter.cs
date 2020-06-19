using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SH_OBD {
    public partial class OBDInterpreter {
        public OBDParameterValue GetMode0102Value(OBDParameter param, OBDResponse response, bool bEnglishUnits = false) {
            OBDParameterValue value2 = new OBDParameterValue();
            int num;
            switch (param.Parameter) {
            case 0:
            case 0x20:
            case 0x40:
            case 0x60:
            case 0x80:
            case 0xA0:
                value2 = GetPIDSupport(response);
                break;
            case 1:
                value2 = GetPID01Value(param, response);
                break;
            case 2:
                // 引起冻结帧的DTC
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.StringValue = GetDTCName(response.Data);
                break;
            case 3:
                value2 = GetPID03Value(param, response);
                break;
            case 4:
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                break;
            case 5:
                // 引擎冷却液温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round((value2.DoubleValue * 1.8) + 32.0, 2);
                }
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                // 长/短时燃油修正 组 1/2/3/4
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                if (param.SubParameter == 0) {
                    // 组 1/2
                    num = Utility.Hex2Int(response.GetDataByte(0));
                } else {
                    // 组 3/4
                    num = Utility.Hex2Int(response.GetDataByte(1));
                }
                value2.DoubleValue = Math.Round((num * 0.78125) - 100.0, 2);
                break;
            case 0x0A:
                // 燃油导轨压力（表压）
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 3.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
            case 0x0B:
                // 进气歧管绝对压力
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
            case 0x0C:
                // 引擎转速
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 0x100) + Utility.Hex2Int(response.GetDataByte(1))) * 0.25;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x0D:
                // 车速
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round(value2.DoubleValue * 0.621371192, 2);
                }
                break;
            case 0x0E:
                // #1缸点火正时
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 0.5) - 64.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x0F:
                // 进气温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = Math.Round((value2.DoubleValue * 1.8) + 32.0, 2);
                }
                break;
            case 0x10:
                // 空气质量流量率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.01;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.13227735731092655;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x11:
                // 节气门绝对位置
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                break;
            case 0x12:
                // 指令的二次空气状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                num = Utility.Hex2Int(response.GetDataByte(0));
                value2.SetBitFlagBAT(num);
                if ((num & 1) != 0) {
                    value2.StringValue = "第一催化转化器的上游";
                    value2.ShortStringValue = "UPS";
                } else if ((num & 2) != 0) {
                    value2.StringValue = "第一催化转化器入口的下游";
                    value2.ShortStringValue = "DNS";
                } else if ((num & 4) != 0) {
                    value2.StringValue = "大气 / 关闭";
                    value2.ShortStringValue = "OFF";
                }
                break;
            case 0x13:
                value2 = GetPID13or1DValue(param, response);
                break;
            case 0x14:
            case 0x15:
            case 0x16:
            case 0x17:
            case 0x18:
            case 0x19:
            case 0x1A:
            case 0x1B:
                // 根据PID$13/$1D支持情况不同，定义不同的氧气传感器位置
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                if (param.SubParameter == 0) {
                    // 氧传感器输出电压
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.005;
                } else {
                    // 短时燃油修正
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.78125 - 100.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 3);
                break;
            case 0x1C:
                value2 = GetPID1CValue(response);
                break;
            case 0x1D:
                value2 = GetPID13or1DValue(param, response);
                break;
            case 0x1E:
                // 动力输出PTO状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
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
                break;
            case 0x1F:
                // 引擎点火后运行时间
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x21:
                // MIL亮起后行驶距离
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.621371192;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x22:
                // 燃油压力（相对于歧管真空）
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 5178.0 / 65535.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x23:
                // 燃油导轨压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 10.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
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
                    break;
                }
                if (param.SubParameter == 0) {
                    // 当量比（λ）
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                } else {
                    // 氧气传感器电压
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(2)) * 256.0) + Utility.Hex2Int(response.GetDataByte(3))) * 8.0 / 65535.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 6);
                break;
            case 0x2C:
                // 指令的EGR
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                break;
            case 0x2D:
                // EGR误差
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 0.78125 - 100.0, 2);
                break;
            case 0x2E:
            case 0x2F:
                // 指令的燃油蒸发排放, 燃油量输入
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0, 2);
                break;
            case 0x30:
                // DTC清除后热车次数
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                break;
            case 0x31:
                // DTC清除后行驶距离
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.621371192;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
            case 0x32:
                // 蒸发排放系统燃油蒸汽压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                num = Utility.Int2SInt((Utility.Hex2Int(response.GetDataByte(0)) * 256) + Utility.Hex2Int(response.GetDataByte(1)), 2);
                value2.DoubleValue = num * 0.25;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.000145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x33:
                // 大气压
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
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
                    break;
                }
                if (param.SubParameter == 0) {
                    // 宽量程氧气传感器当量比（λ）
                    value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                } else {
                    // 宽量程氧气传感器电压
                    value2.DoubleValue = (((Utility.Hex2Int(response.GetDataByte(2)) * 256.0) + Utility.Hex2Int(response.GetDataByte(3))) * 128.0 / 32768.0) - 128.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 6);
                break;
            case 0x3C:
            case 0x3D:
            case 0x3E:
            case 0x3F:
                // 催化器温度，组 1/2 传感器 1/2
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.1) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x41:
                value2 = GetPID41Value(param, response);
                break;
            case 0x42:
                // 控制模块电压
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.001;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 3);
                break;
            case 0x43:
                // 绝对负载值
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x44:
                // 指令的当量率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2.0 / 65535.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 3);
                break;
            case 0x45:
                // 节气门相对位置
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x46:
                // 环境空气温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
            case 0x47:
            case 0x48:
            case 0x49:
            case 0x4A:
            case 0x4B:
            case 0x4C:
                // 节气门绝对位置 B/C, 油门踏板位置 D/E/F
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x4D:
            case 0x4E:
                // MIL亮起后引擎运转时间, DTC清除后持续时间
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                break;
            case 0x4F:
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    break;
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
                    // 进气歧管绝压最大值
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(3)) * 10.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                break;
            case 0x50:
                // 空气质量流量最大值
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 10.0;
                break;
            case 0x51:
                value2 = GetPID51Value(response);
                break;
            case 0x52:
                // 酒精燃料百分比
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x53:
                // 燃料蒸发排放系统蒸汽绝压
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.005;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x54:
                // 燃料蒸发排放系统蒸汽压力
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Int2SInt((Utility.Hex2Int(response.GetDataByte(0)) * 256) + Utility.Hex2Int(response.GetDataByte(1)), 2);
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.000145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x55:
            case 0x56:
            case 0x57:
            case 0x58:
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                if (param.SubParameter == 0) {
                    // 长/短时第二氧气传感器燃油修正 组1/2
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.78125 - 100.0;
                } else {
                    // 长/短时第二氧气传感器燃油修正 组3/4
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) * 0.78125 - 100.0;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x59:
                // 燃料导轨压力（绝压）
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 10.0;
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x5A:
            case 0x5B:
                // 相对油门踏板位置, 混动/EV电池组剩余电量
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x5C:
                // 引擎润滑油温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                break;
            case 0x5D:
                // 燃油喷射正时
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) / 128.0 - 210.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x5E:
                // 发动机燃油消耗率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.05;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x5F:
                // 车辆设计的排放要求
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                switch (Utility.Hex2Int(response.GetDataByte(0))) {
                case 0x0E:
                    value2.StringValue = "欧4 B1";
                    value2.ShortStringValue = "EURO IV B1";
                    break;
                case 0x0F:
                    value2.StringValue = "欧5 B2";
                    value2.ShortStringValue = "EURO V B2";
                    break;
                case 0x10:
                    value2.StringValue = "欧C";
                    value2.ShortStringValue = "EURO C";
                    break;
                default:
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = "——";
                    break;
                }
                break;
            case 0x61:
            case 0x62:
                // 驾驶员需求的引擎-扭矩百分比, 实际引擎-扭矩百分比
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 125.0;
                break;
            case 0x63:
                // 引擎参考扭矩
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = (Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1));
                break;
            case 0x64:
                // 引擎百分比扭矩数据
                if (response.GetDataByteCount() < 5) {
                    value2.ErrorDetected = true;
                    break;
                }
                switch (param.SubParameter) {
                case 0:
                    // 怠速时引擎百分比扭矩，点位1
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 125.0;
                    break;
                case 1:
                    // 引擎百分比扭矩，点位2
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(1)) - 125.0;
                    break;
                case 2:
                    // 引擎百分比扭矩，点位3
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(2)) - 125.0;
                    break;
                case 3:
                    // 引擎百分比扭矩，点位4
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(3)) - 125.0;
                    break;
                case 4:
                    // 引擎百分比扭矩，点位5
                    value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(4)) - 125.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                break;
            case 0x65:
                value2 = GetPID65Value(param, response);
                break;
            case 0x66:
                value2 = GetPID66Value(param, response, bEnglishUnits);
                break;
            case 0x67:
                value2 = GetPID67Value(param, response, bEnglishUnits);
                break;
            case 0x68:
                value2 = GetPID68Value(param, response, bEnglishUnits);
                break;
            case 0x69:
                value2 = GetPID69Value(param, response);
                break;
            case 0x6A:
                value2 = GetPID6AValue(param, response);
                break;
            case 0x6B:
                value2 = GetPID6BValue(param, response, bEnglishUnits);
                break;
            case 0x6C:
                value2 = GetPID6CValue(param, response);
                break;
            case 0x6D:
                value2 = GetPID6DValue(param, response, bEnglishUnits);
                break;
            case 0x6E:
                value2 = GetPID6EValue(param, response, bEnglishUnits);
                break;
            case 0x6F:
                value2 = GetPID6FValue(param, response, bEnglishUnits);
                break;
            case 0x70:
                value2 = GetPID70Value(param, response, bEnglishUnits);
                break;
            case 0x71:
                value2 = GetPID71Value(param, response);
                break;
            case 0x72:
                value2 = GetPID72Value(param, response);
                break;
            case 0x73:
                value2 = GetPID73Value(param, response, bEnglishUnits);
                break;
            case 0x74:
                value2 = GetPID74Value(param, response);
                break;
            case 0x75:
            case 0x76:
                value2 = GetPID75or76Value(param, response, bEnglishUnits);
                break;
            case 0x77:
                value2 = GetPID77Value(param, response, bEnglishUnits);
                break;
            case 0x78:
            case 0x79:
                value2 = GetPID78or79Value(param, response, bEnglishUnits);
                break;
            case 0x7A:
            case 0x7B:
                value2 = GetPID7Aor7BValue(param, response, bEnglishUnits);
                break;
            case 0x7C:
                value2 = GetPID7CValue(param, response, bEnglishUnits);
                break;
            case 0x7D:
                // NOx NTE控制区状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                num = Utility.Hex2Int(response.GetDataByte(0));
                value2.SetBitFlagBAT(num);

                switch (param.SubParameter) {
                case 0:
                    if ((num & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在控制区内";
                        value2.ShortStringValue = "NNTE: IN";
                    }
                    break;
                case 1:
                    if ((num & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在控制区外";
                        value2.ShortStringValue = "NNTE: OUT";
                    }
                    break;
                case 2:
                    if ((num & 4) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在制造商指定的NOx NTE切离区内";
                        value2.ShortStringValue = "NNTE: CAA";
                    }
                    break;
                case 3:
                    if ((num & 8) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "NOx激活区NTE亏量";
                        value2.ShortStringValue = "NNTE: DEF";
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                break;
            case 0x7E:
                // PM NTE控制区状态
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                num = Utility.Hex2Int(response.GetDataByte(0));
                value2.SetBitFlagBAT(num);

                switch (param.SubParameter) {
                case 0:
                    if ((num & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在PM控制区内";
                        value2.ShortStringValue = "PNTE: IN";
                    }
                    break;
                case 1:
                    if ((num & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在PM控制区外";
                        value2.ShortStringValue = "PNTE: OUT";
                    }
                    break;
                case 2:
                    if ((num & 4) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "在制造商指定的PM NTE切离区内";
                        value2.ShortStringValue = "PNTE: CAA";
                    }
                    break;
                case 3:
                    if ((num & 8) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "PM激活区NTE亏量";
                        value2.ShortStringValue = "PNTE: DEF";
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                break;
            case 0x7F:
                value2 = GetPID7FValue(param, response);
                break;
            case 0x81:
            case 0x82:
            case 0x89:
            case 0x8A:
                value2 = GetPID81or82or89or8AValue(param, response);
                break;
            case 0x83:
            case 0xA7:
                value2 = GetPID83orA7Value(param, response);
                break;
            case 0x84:
                // 歧管表面温度
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 40.0;
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
                break;
            case 0x85:
                value2 = GetPID85Value(param, response);
                break;
            case 0x86:
                value2 = GetPID86Value(param, response);
                break;
            case 0x87:
                value2 = GetPID87Value(param, response, bEnglishUnits);
                break;
            case 0x88:
                value2 = GetPID88Value(param, response, bEnglishUnits);
                break;
            case 0x8B:
                value2 = GetPID8BValue(param, response, bEnglishUnits);
                break;
            case 0x8C:
            case 0x9C:
                value2 = GetPID8Cor9CValue(param, response);
                break;
            case 0x8D:
                // 节气门绝对位置 G
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 100.0 / 255.0;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x8E:
                // 发动机摩擦力 - 扭矩百分比
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) - 125.0;
                break;
            case 0x8F:
                value2 = GetPID8FValue(param, response);
                break;
            case 0x90:
                value2 = GetPID90Value(param, response);
                break;
            case 0x91:
                value2 = GetPID91Value(param, response);
                break;
            case 0x92:
                value2 = GetPID92Value(param, response);
                break;
            case 0x93:
                value2 = GetPID93Value(param, response);
                break;
            case 0x94:
                value2 = GetPID94Value(param, response);
                break;
            case 0x98:
            case 0x99:
                value2 = GetPID98or99Value(param, response, bEnglishUnits);
                break;
            case 0x9A:
                value2 = GetPID9AValue(param, response);
                break;
            case 0x9B:
                value2 = GetPID9BValue(param, response, bEnglishUnits);
                break;
            case 0x9D:
                if (response.GetDataByteCount() < 4) {
                    value2.ErrorDetected = true;
                    break;
                }
                // 引擎燃油率
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.02;
                // 车辆燃油率
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(2)) * 256.0) + Utility.Hex2Int(response.GetDataByte(3))) * 0.02;
                break;
            case 0x9E:
                // 引擎排气流量率
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 0.2;
                break;
            case 0x9F:
                value2 = GetPID9FValue(param, response);
                break;
            case 0xA1:
            case 0xA8:
                value2 = GetPIDA1orA8Value(param, response);
                break;
            case 0xA2:
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = ((Utility.Hex2Int(response.GetDataByte(0)) * 256.0) + Utility.Hex2Int(response.GetDataByte(1))) * 2048.0 / 65535.0;
                break;
            case 0xA3:
                value2 = GetPIDA3Value(param, response, bEnglishUnits);
                break;
            case 0xA4:
                value2 = GetPIDA4Value(param, response);
                break;
            case 0xA5:
                value2 = GetPIDA5Value(param, response);
                break;
            case 0xA6:
                value2 = GetPIDA6Value(response, bEnglishUnits);
                break;
            case 0xA9:
                value2 = GetPIDA9Value(param, response);
                break;
            default:
                if (param.Parameter >= 0xA0 && param.Parameter <= 0xFF) {
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = "——";
                } else {
                    value2.ErrorDetected = true;
                }
                break;
            }
            return value2;
        }

        public OBDParameterValue GetMode03070AValue(OBDResponse response) {
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

        public OBDParameterValue GetMode05Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            switch (param.Parameter) {
            case 0x00:
                value2 = GetPIDSupport(response);
                break;
            case 0x01:
            case 0x02:
            case 0x03:
            case 0x04:
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 0.005;
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                break;
            case 0x05:
            case 0x06:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    break;
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
                break;
            case 0x07:
            case 0x08:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    break;
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
                break;
            case 0x09:
            case 0x0A:
                if (response.GetDataByteCount() < 3) {
                    value2.ErrorDetected = true;
                    break;
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
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        public OBDParameterValue GetMode09Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            List<string> strings = new List<string>();
            int DataOffset;
            int num;

            switch (param.Parameter) {
            case 0x00:
                value2 = GetPIDSupport(response);
                break;
            case 0x01:
            case 0x03:
            case 0x05:
            case 0x07:
            case 0x09:
            case 0x0C:
            case 0x0E:
                // 获取相关InfoType的消息数量，仅适用于非CAN协议
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0));
                break;
            case 0x02:
            case 0x0D:
            case 0x0F:
                // VIN / ESN / EROTAN
                DataOffset = 17 * 2;
                // 过滤从K线读取的“00”数据，该数据用于补齐K线每帧4个有效字节
                string strTemp = "";
                for (int i = 0; i < response.Data.Length; i += 2) {
                    if (response.Data.Substring(i, 2) != "00") {
                        strTemp += response.Data.Substring(i, 2);
                    }
                }
                response.Data = strTemp;
                value2.ListStringValue = SetMode09ASCII(DataOffset, response);
                break;
            case 0x04:
                // CAL ID
                DataOffset = 16 * 2;
                value2.ListStringValue = SetMode09ASCII(DataOffset, response);
                break;
            case 0x06:
                // CVN
                DataOffset = 4 * 2;
                num = response.Data.Length / DataOffset;
                for (int i = 0; i < num; i++) {
                    strings.Add(response.Data.Substring(i * DataOffset, DataOffset));
                }
                value2.ListStringValue = strings;
                break;
            case 0x08:
            case 0x0B:
                // IPT
                DataOffset = 2 * 2;
                num = response.Data.Length / DataOffset;
                for (int i = 0; i < num; i++) {
                    strings.Add(response.Data.Substring(i * DataOffset, DataOffset));
                }
                value2.ListStringValue = strings;
                if (param.SubParameter >= 0 && param.SubParameter < num) {
                    value2.DoubleValue = Utility.Hex2Int(value2.ListStringValue[param.SubParameter]);
                } else {
                    value2.ErrorDetected = true;
                }
                break;
            case 0x0A:
                // ECU名称
                DataOffset = 20 * 2;
                value2.ListStringValue = SetMode09ASCII(DataOffset, response);
                break;
            case 0x10:
                // ECU协议
                if (response.GetDataByteCount() < 1) {
                    value2.ErrorDetected = true;
                    break;
                }
                num = Utility.Hex2Int(response.GetDataByte(0));
                value2.DoubleValue = num;
                switch (num) {
                case 0:
                    value2.StringValue = "保留";
                    value2.ShortStringValue = "--";
                    break;
                case 1:
                    value2.StringValue = "ISO 27145-4";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                default:
                    value2.StringValue = "保留";
                    value2.ShortStringValue = "--";
                    break;
                }
                break;
            case 0x11:
                // WWH-OBD GTR 编号
                DataOffset = 11 * 2;
                value2.ListStringValue = SetMode09ASCII(DataOffset, response);
                break;
            case 0x12:
            case 0x14:
                // 燃油发动机操作点火循环计数 / 自EVAP监测完成后行驶距离
                if (response.GetDataByteCount() < 2) {
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(0)) * 256.0 + Utility.Hex2Int(response.GetDataByte(1));
                break;
            default:
                if (param.Parameter == 0x13 || (param.Parameter >= 0x15 && param.Parameter <= 0xFF)) {
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = "--";
                } else {
                    value2.ErrorDetected = true;
                }
                break;
            }
            return value2;
        }

        private List<string> SetMode09ASCII(int DataOffset, OBDResponse response) {
            List<string> strings = new List<string>();
            int num = response.Data.Length / DataOffset;
            for (int i = 0; i < num; i++) {
                strings.Add(Utility.HexStrToASCIIStr(response.Data.Substring(i * DataOffset, DataOffset)));
            }
            return strings;
        }

        private OBDParameterValue Get42DTCValue(OBDResponse response) {
            return Get19DTCValue(response, 10);
        }

        private OBDParameterValue Get55DTCValue(OBDResponse response) {
            return Get19DTCValue(response, 8);
        }

        private OBDParameterValue Get19DTCValue(OBDResponse response, int offset, int WholeDTCLenInByte = 4) {
            if (offset - WholeDTCLenInByte * 2 < 0) {
                return null;
            }
            OBDParameterValue value2 = new OBDParameterValue();
            List<string> strings = new List<string>();
            for (int i = 0; i <= response.Data.Length - offset; i += offset) {
                string str = GetDTCName(response.Data.Substring(i + offset - WholeDTCLenInByte * 2, 6));
                if (!str.StartsWith("P0000")) {
                    strings.Add(str);
                }
            }
            value2.ListStringValue = strings;
            return value2;
        }

        private OBDParameterValue GetDM5Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 8) {
                value2.ErrorDetected = true;
                return value2;
            }

            switch (param.SubParameter) {
            case 0:
                // OBD型式
                response.Data = response.GetDataByte(2);
                value2 = GetPID1CValue(response);
                break;
            case 1:
                // 激活的故障代码
                response.Data = response.GetDataByte(0);
                break;
            case 2:
                // 先前激活的诊断故障代码
                response.Data = response.GetDataByte(1);
                break;
            case 3:
                // 持续监视系统支持／状态
                response.Data = response.GetDataByte(3);
                break;
            case 4:
                // 非持续监视系统支持
                response.Data = response.GetDataByte(4) + response.GetDataByte(5);
                break;
            case 5:
                // 非持续监视系统状态
                response.Data = response.GetDataByte(6) + response.GetDataByte(7);
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        private OBDParameterValue GetDM19Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 20) {
                value2.ErrorDetected = true;
                return value2;
            }

            int qty = response.Data.Length / (20 * 2);
            string strData = "";
            OBDParameter param2 = new OBDParameter();
            switch (param.SubParameter) {
            case 0:
                // CVN
                param2.Parameter = 0x06;
                for (int i = 0; i < qty; i++) {
                    strData += response.Data.Substring(i * 20 * 2, 4 * 2);
                }
                response.Data = strData;
                value2 = GetMode09Value(param2, response);
                for (int i = 0; i < value2.ListStringValue.Count; i++) {
                    string strVal = value2.ListStringValue[i];
                    value2.ListStringValue[i] = strVal.Substring(6, 2) + strVal.Substring(4, 2) + strVal.Substring(2, 2) + strVal.Substring(0, 2);
                }
                break;
            case 1:
                // CAL_ID
                param2.Parameter = 0x04;
                for (int i = 0; i < qty; i++) {
                    strData += response.Data.Substring(4 * 2 + i * 20 * 2, 16 * 2);
                }
                response.Data = strData;
                value2 = GetMode09Value(param2, response);
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        private OBDParameterValue GetJ1939Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.Header.Substring(2, 2) == "E8") {
                // J1939的确认消息，非正确的返回值
                value2.ErrorDetected = true;
                return value2;
            }
            switch (param.Parameter) {
            case 0xFECE:
                value2 = GetDM5Value(param, response);
                break;
            case 0xD300:
                value2 = GetDM19Value(param, response);
                break;
            case 0xFEEC:
                // VIN
                value2.ListStringValue = SetMode09ASCII(17 * 2, response);
                break;
            }
            return value2;
        }

        public OBDParameterValue GetValue(OBDParameter param, OBDResponse response, bool bEnglishUnits = false) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response == null) {
                value2.ErrorDetected = true;
                return value2;
            }
            switch (param.Service) {
            case 0:
                // SAE J1939
                value2 = GetJ1939Value(param, response);
                break;
            case 1:
            case 2:
                value2 = GetMode0102Value(param, response, bEnglishUnits);
                break;
            case 3:
            case 7:
            case 0x0A:
                value2 = GetMode03070AValue(response);
                break;
            case 5:
                value2 = GetMode05Value(param, response);
                break;
            case 9:
                value2 = GetMode09Value(param, response);
                break;
            case 0x19:
                // ISO 27145 ReadDTCInformation
                string reportType = param.OBDRequest.Substring(2, 2);
                if (reportType == "42") {
                    value2 = Get42DTCValue(response);
                } else if (reportType == "55") {
                    value2 = Get55DTCValue(response);
                }
                break;
            case 0x22:
                // ISO 27145 ReadDataByIdentifer
                int HByte = (param.Parameter >> 8) & 0xFF;
                int LByte = param.Parameter & 0x00FF;
                param.Parameter = LByte;
                if (HByte == 0xF4) {
                    value2 = GetMode0102Value(param, response, bEnglishUnits);
                } else if (HByte == 0xF8) {
                    value2 = GetMode09Value(param, response);
                }
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            value2.ECUResponseID = response.Header;
            if (value2.ECUResponseID.Length == 6) {
                // 如果是K线协议的话ECUResponseID取最后2个字节
                value2.ECUResponseID = value2.ECUResponseID.Substring(2);
            } else if (value2.ECUResponseID.Length == 8 && param.Service == 0) {
                // 如果是J1939协议的话ECUResponseID取最后1个字节
                value2.ECUResponseID = value2.ECUResponseID.Substring(6);
            }
            return value2;
        }

        public OBDParameterValue GetValue(OBDParameter param, OBDResponseList responses, bool bEnglishUnits = false) {
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

        public string GetDTCName(string strHexDTC) {
            if (strHexDTC.Length < 4) {
                return "P0000";
            } else {
                return GetDTCSystem(strHexDTC.Substring(0, 1)) + strHexDTC.Substring(1);
            }
        }

        private string GetDTCSystem(string strSysId) {
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