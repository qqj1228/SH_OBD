using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SH_OBD {
    public partial class OBDInterpreter {
        public OBDParameterValue GetPIDSupport(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA, dataB, dataC, dataD);
            return value2;
        }

        /// <summary>
        /// MIL状态，DTC数量，就绪状态
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID01Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA, dataB, dataC, dataD);

            switch (param.SubParameter) {
            case 0:
                // MIL
                if ((dataA & 0x80) == 0) {
                    value2.SetDataWithBool(false);
                } else {
                    value2.SetDataWithBool(true);
                }
                break;
            case 1:
                // DTC数量
                if ((dataA & 0x80) != 0) {
                    dataA -= 0x80;
                }
                value2.DoubleValue = dataA;
                break;
            case 2:
            case 3:
            case 4:
            case 5:
                // 失火 / 燃油系统 / 综合组件 / 压缩点火（0 - 火花点火，1 - 压缩点火），监控支持？
                value2.SetBoolValueWithData(dataB, param.SubParameter - 2);
                break;
            case 6:
            case 7:
            case 8:
                // 失火 / 燃油系统 / 综合组件，监控完成？
                value2.SetBoolValueWithData(dataB, param.SubParameter - 2, true);
                break;
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
                // 火花点火车辆：催化器 / 加热催化器 / 燃油蒸发系统 / 二次空气系统 / 空调系统制冷剂 / 氧气传感器 / 加热氧气传感器 / EGR/VVT系统，监控支持？
                // 压缩点火车辆：NMHC催化器 / NOx/SCR后处理 / ISO/SAE保留 / 增压系统 / ISO/SAE保留 / 废气传感器 / PM过滤器 / EGR/VVT系统，监控支持？
                value2.SetBoolValueWithData(dataC, param.SubParameter - 9);
                break;
            case 17:
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
                // 火花点火车辆：催化器 / 加热催化器 / 燃油蒸发系统 / 二次空气系统 / 空调系统制冷剂 / 氧气传感器 / 加热氧气传感器 / EGR系统，监控完成？
                // 压缩点火车辆：NMHC催化器 / NOx/SCR后处理 / ISO/SAE保留 / 增压系统 / ISO/SAE保留 / 排气传感器 / PM过滤器 / EGR/VVT系统，监控完成？
                value2.SetBoolValueWithData(dataD, param.SubParameter - 17, true);
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        /// <summary>
        /// 燃油系统状态
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID03Value(OBDParameter param, OBDResponse response) {
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
            value2.SetBitFlagBAT(num);

            if ((num & 1) != 0) {
                value2.StringValue = "开环，尚未满足闭环条件";
                value2.ShortStringValue = "OL";
            } else if ((num & 2) != 0) {
                value2.StringValue = "闭环，使用氧传感器作为燃油控制的反馈";
                value2.ShortStringValue = "CL";
            } else if ((num & 4) != 0) {
                value2.StringValue = "开环，由于驾驶条件而开环（例如，功率提升、减速消耗）";
                value2.ShortStringValue = "OL-Drive";
            } else if ((num & 8) != 0) {
                value2.StringValue = "开环，由于检测到的系统故障而开环";
                value2.ShortStringValue = "OL-Fault";
            } else if ((num & 0x10) != 0) {
                value2.StringValue = "闭环，但至少有一个氧气故障传感器 - 可能使用单氧传感器作为燃料控制";
                value2.ShortStringValue = "CL-Fault";
            } else if ((num & 0x20) != 0) {
                value2.StringValue = "开环，尚未满足闭环条件（组2）";
                value2.ShortStringValue = "OL B2";
            } else if ((num & 0x40) != 0) {
                value2.StringValue = "开环，由于驾驶条件而开环（例如，功率提升、减速消耗）（组2）";
                value2.ShortStringValue = "OL Drive B2";
            } else if ((num & 0x80) != 0) {
                value2.StringValue = "开环，由于检测到的系统故障而开环（组2）";
                value2.ShortStringValue = "OL Fault B2";
            }
            return value2;
        }

        /// <summary>
        /// 氧气传感器位置
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID13or1DValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 7) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                value2.ErrorDetected = true;
            }
            return value2;
        }

        /// <summary>
        /// 车辆OBD类型要求
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID1CValue(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            value2.DoubleValue = dataA;
            switch (dataA) {
            case 1:
                value2.StringValue = "OBD II (加利福尼亚 ARB)";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDII CARB";
                break;
            case 2:
                value2.StringValue = "OBD (联邦环保局)";
                value2.ShortStringValue = dataA.ToString("X") + ",OBD (Fed)";
                break;
            case 3:
                value2.StringValue = "OBD 和 OBD II";
                value2.ShortStringValue = dataA.ToString("X") + ",OBD/OBDII";
                break;
            case 4:
                value2.StringValue = "OBD I";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDI";
                break;
            case 5:
                value2.StringValue = "不兼容OBD";
                value2.ShortStringValue = dataA.ToString("X") + ",NO OBD";
                break;
            case 6:
                value2.StringValue = "EOBD（欧洲OBD）";
                value2.ShortStringValue = dataA.ToString("X") + ",EOBD";
                break;
            case 7:
                value2.StringValue = "EOBD 和 OBD II";
                value2.ShortStringValue = dataA.ToString("X") + ",EOBD/OBDII";
                break;
            case 8:
                value2.StringValue = "EOBD 和 OBD";
                value2.ShortStringValue = dataA.ToString("X") + ",EOBD/OBD";
                break;
            case 9:
                value2.StringValue = "EOBD, OBD 和 OBD II";
                value2.ShortStringValue = dataA.ToString("X") + ",EOBD/OBD/OBDII";
                break;
            case 0x0A:
                value2.StringValue = "JOBD（日本OBD）";
                value2.ShortStringValue = dataA.ToString("X") + ",JOBD";
                break;
            case 0x0B:
                value2.StringValue = "JOBD 和 OBD II";
                value2.ShortStringValue = dataA.ToString("X") + ",JOBD/OBDII";
                break;
            case 0x0C:
                value2.StringValue = "JOBD 和 EOBD";
                value2.ShortStringValue = dataA.ToString("X") + ",JOBD/EOBD";
                break;
            case 0x0D:
                value2.StringValue = "JOBD, EOBD, 和 OBD II";
                value2.ShortStringValue = dataA.ToString("X") + ",JOBD/EOBD/OBDII";
                break;
            case 0x0E:
                value2.StringValue = "OBD, EOBD 和 KOBD";
                value2.ShortStringValue = dataA.ToString("X") + ",OBD/EOBD/KOBD";
                break;
            case 0x0F:
                value2.StringValue = "OBD, OBD II, EOBD 和 KOBD";
                value2.ShortStringValue = dataA.ToString("X") + ",OBD/OBD II/EOBD/KOBD";
                break;
            case 0x10:
                value2.StringValue = "ISO/SAE 保留";
                value2.ShortStringValue = dataA.ToString("X") + ",--";
                break;
            case 0x11:
                value2.StringValue = "发动机制造商诊断用 (EMD)";
                value2.ShortStringValue = dataA.ToString("X") + ",EMD";
                break;
            case 0x12:
                value2.StringValue = "发动机制造商诊断用增强型 (EMD+)";
                value2.ShortStringValue = dataA.ToString("X") + ",EMD+";
                break;
            case 0x13:
                value2.StringValue = "重型车辆OBD (子类)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD OBD-C";
                break;
            case 0x14:
                value2.StringValue = "重型车辆OBD";
                value2.ShortStringValue = dataA.ToString("X") + ",HD OBD";
                break;
            case 0x15:
                value2.StringValue = "全球协调OBD (WWH OBD)";
                value2.ShortStringValue = dataA.ToString("X") + ",WWH OBD";
                break;
            case 0x16:
                value2.StringValue = "ISO/SAE 保留";
                value2.ShortStringValue = dataA.ToString("X") + ",--";
                break;
            case 0x17:
                value2.StringValue = "欧洲重型车辆OBD 阶段I，无NOx控制 (HD EOBD-I)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD EOBD-I";
                break;
            case 0x18:
                value2.StringValue = "欧洲重型车辆OBD 阶段I，有NOx控制 (HD EOBD-I N)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD EOBD-I N";
                break;
            case 0x19:
                value2.StringValue = "欧洲重型车辆OBD 阶段II，无NOx控制 (HD EOBD-II)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD EOBD-II";
                break;
            case 0x1A:
                value2.StringValue = "欧洲重型车辆OBD 阶段II，有NOx控制 (HD EOBD-II N)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD EOBD-II N";
                break;
            case 0x1B:
                value2.StringValue = "ISO/SAE 保留";
                value2.ShortStringValue = dataA.ToString("X") + ",--";
                break;
            case 0x1C:
                value2.StringValue = "巴西OBD 阶段1";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDBr-1";
                break;
            case 0x1D:
                value2.StringValue = "巴西OBD 阶段2和2+";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDBr-2";
                break;
            case 0x1E:
                value2.StringValue = "韩国 OBD (KOBD)";
                value2.ShortStringValue = dataA.ToString("X") + ",KOBD";
                break;
            case 0x1F:
                value2.StringValue = "印度 OBD I (IOBD I)";
                value2.ShortStringValue = dataA.ToString("X") + ",IOBD I";
                break;
            case 0x20:
                value2.StringValue = "印度 OBD II (IOBD II)";
                value2.ShortStringValue = dataA.ToString("X") + ",IOBD II";
                break;
            case 0x21:
                value2.StringValue = "欧洲重型车辆OBD 阶段VI (HD EOBD-VI)";
                value2.ShortStringValue = dataA.ToString("X") + ",HD EOBD-VI";
                break;
            case 0x22:
                value2.StringValue = "OBD, OBD II 和 HD OBD";
                value2.ShortStringValue = dataA.ToString("X") + ",OBD/OBD II/HD OBD";
                break;
            case 0x23:
                value2.StringValue = "巴西OBD 阶段3";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDBr-3";
                break;
            case 0x24:
                value2.StringValue = "摩托车，欧洲OBD-I（MC EOBD-I）";
                value2.ShortStringValue = dataA.ToString("X") + ",MC EOBD-I";
                break;
            case 0x25:
                value2.StringValue = "摩托车，欧洲OBD-II（MC EOBD-II）";
                value2.ShortStringValue = dataA.ToString("X") + ",MC EOBD-II";
                break;
            case 0x26:
                value2.StringValue = "摩托车，中国OBD-I（MC COBD-I）";
                value2.ShortStringValue = dataA.ToString("X") + ",MC COBD-I";
                break;
            case 0x27:
                value2.StringValue = "摩托车，台湾OBD-I（MC TOBD-I）";
                value2.ShortStringValue = dataA.ToString("X") + ",MC TOBD-I";
                break;
            case 0x28:
                value2.StringValue = "摩托车，日本OBD-I（MC JOBD-I）";
                value2.ShortStringValue = dataA.ToString("X") + ",MC JOBD-I";
                break;
            case 0x29:
                value2.StringValue = "中国国家范围第六阶段（CN-OBD-6）";
                value2.ShortStringValue = dataA.ToString("X") + ",CN-OBD-6";
                break;
            case 0x2A:
                value2.StringValue = "巴西 OBD 柴油（OBDBr-D）";
                value2.ShortStringValue = dataA.ToString("X") + ",OBDBr-D";
                break;
            case 0x2B:
                value2.StringValue = "中国重型车辆VI（CN-HDOBD-VI）";
                value2.ShortStringValue = dataA.ToString("X") + ",CN-HDOBD-VI";
                break;
            default:
                if (dataA >= 0x2C && dataA <= 0xFA) {
                    value2.StringValue = "ISO/SAE 保留";
                    value2.ShortStringValue = dataA.ToString("X") + ",--";
                } else if (dataA >= 0xFB && dataA <= 0xFF) {
                    value2.StringValue = "ISO/SAE - 不用于分配";
                    value2.ShortStringValue = dataA.ToString("X") + ",SAE J1939 特殊用途";
                }
                break;
            }
            return value2;
        }

        /// <summary>
        /// 当前驾驶循环的监控状态
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID41Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA, dataB, dataC, dataD);

            switch (param.SubParameter) {
            case 0:
            case 1:
            case 2:
            case 3:
                // 失火 / 燃油系统 / 综合组件 / 压缩点火，监控可用？
                value2.SetBoolValueWithData(dataB, param.SubParameter);
                break;
            case 4:
            case 5:
            case 6:
                // 失火 / 燃油系统 / 综合组件，监控完成？
                value2.SetBoolValueWithData(dataB, param.SubParameter, true);
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
                // 火花点火车辆：催化器 / 加热催化器 / 燃油蒸发系统 / 二次空气系统 / 空调系统制冷剂 / 氧气传感器 / 加热氧气传感器 / EGR系统，监控可用？
                // 压缩点火车辆：NMHC催化器 / NOx/SCR后处理 / ISO/SAE保留 / 增压系统 / ISO/SAE保留 / 排气传感器 / PM过滤器 / EGR/VVT系统，监控可用？
                value2.SetBoolValueWithData(dataC, param.SubParameter - 7);
                break;
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
                // 火花点火车辆：催化器 / 加热催化器 / 燃油蒸发系统 / 二次空气系统 / 空调系统制冷剂 / 氧气传感器 / 加热氧气传感器 / EGR系统，监控完成？
                // 压缩点火车辆：NMHC催化器 / NOx/SCR后处理 / ISO/SAE保留 / 增压系统 / ISO/SAE保留 / 排气传感器 / PM过滤器 / EGR/VVT系统，监控完成？
                value2.SetBoolValueWithData(dataD, param.SubParameter - 15, true);
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        /// <summary>
        /// 燃料种类
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID51Value(OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 1) {
                value2.ErrorDetected = true;
                return value2;
            }
            switch (Utility.Hex2Int(response.GetDataByte(0))) {
            case 0:
                value2.StringValue = "不可用";
                value2.ShortStringValue = "NONE";
                break;
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
            case 0x0A:
                value2.StringValue = "使用甲醇的双燃料车辆";
                value2.ShortStringValue = "BI_METH";
                break;
            case 0x0B:
                value2.StringValue = "使用乙醇的双燃料车辆";
                value2.ShortStringValue = "BI_ETH";
                break;
            case 0x0C:
                value2.StringValue = "使用LPG的双燃料车辆";
                value2.ShortStringValue = "BI_LPG";
                break;
            case 0x0D:
                value2.StringValue = "使用CNG的双燃料车辆";
                value2.ShortStringValue = "BI_CNG";
                break;
            case 0x0E:
                value2.StringValue = "使用丙烷的双燃料车辆";
                value2.ShortStringValue = "BI_PROP";
                break;
            case 0x0F:
                value2.StringValue = "使用电池的双燃料车辆";
                value2.ShortStringValue = "BI_ELEC";
                break;
            case 0x10:
                value2.StringValue = "使用电池和燃料的双燃料车辆";
                value2.ShortStringValue = "BI_MIX";
                break;
            case 0x11:
                value2.StringValue = "汽油混合动力";
                value2.ShortStringValue = "HYB_GAS";
                break;
            case 0x12:
                value2.StringValue = "乙醇混合动力";
                value2.ShortStringValue = "HYB_ETH";
                break;
            case 0x13:
                value2.StringValue = "柴油混合动力";
                value2.ShortStringValue = "HYB_DSL";
                break;
            case 0x14:
                value2.StringValue = "电动混合动力";
                value2.ShortStringValue = "HYB_ELEC";
                break;
            case 0x15:
                value2.StringValue = "燃料混合动力";
                value2.ShortStringValue = "HYB_FUEL";
                break;
            case 0x16:
                value2.StringValue = "混合再生动力";
                value2.ShortStringValue = "HYB_REG";
                break;
            case 0x17:
                value2.StringValue = "使用柴油的双燃料车辆/天然气";
                value2.ShortStringValue = "BI_DSL/NG";
                break;
            case 0x18:
                value2.StringValue = "使用天然气的双燃料车辆";
                value2.ShortStringValue = "BI_NG";
                break;
            case 0x19:
                value2.StringValue = "使用柴油的双燃料车辆";
                value2.ShortStringValue = "BI_DSL";
                break;
            case 0x1A:
                value2.StringValue = "天然气";
                value2.ShortStringValue = "NG";
                break;
            case 0x1B:
                value2.StringValue = "柴油和压缩天然气双燃料";
                value2.ShortStringValue = "DSL_CNG";
                break;
            case 0x1C:
                value2.StringValue = "柴油和天然气双燃料";
                value2.ShortStringValue = "DSL_NG";
                break;
            default:
                value2.StringValue = "ISO/SAE 保留";
                value2.ShortStringValue = "——";
                break;
            }
            return value2;
        }

        /// <summary>
        /// 辅助输入输出
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID65Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 2) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            value2.SetBitFlagBAT(dataA, dataB);

            if (param.SubParameter >= 0 && param.SubParameter <= 4) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else if (param.SubParameter >= 5 && param.SubParameter <= 8) {
                value2.SetBoolValueWithData(dataB, param.SubParameter - 5);
            } else if (param.SubParameter == 9) {
                // 当前车辆条件下的推荐变速挡位
                value2.DoubleValue = (dataB >> 4) & 0x0F;
            } else {
                value2.ErrorDetected = true;
            }
            return value2;
        }

        /// <summary>
        /// 质量空气流量传感器
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID66Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));

            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 质量空气流量传感器 A
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 2048.0 / 65535.0;
                    break;
                case 3:
                    // 质量空气流量传感器 B
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 2048.0 / 65535.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.13227735731092655;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 引擎冷却液温度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID67Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 3) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 引擎冷却液温度 1
                    value2.DoubleValue = dataB - 40.0;
                    break;
                case 3:
                    // 引擎冷却液温度 2
                    value2.DoubleValue = dataC - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 进气温度传感器
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID68Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // 进气温度 组1，传感器1
                    value2.DoubleValue = dataB - 40.0;
                    break;
                case 7:
                    // 进气温度 组1，传感器2
                    value2.DoubleValue = dataC - 40.0;
                    break;
                case 8:
                    // 进气温度 组1，传感器3
                    value2.DoubleValue = dataD - 40.0;
                    break;
                case 9:
                    // 进气温度 组2，传感器1
                    value2.DoubleValue = dataE - 40.0;
                    break;
                case 10:
                    // 进气温度 组2，传感器2
                    value2.DoubleValue = dataF - 40.0;
                    break;
                case 11:
                    // 进气温度 组2，传感器3
                    value2.DoubleValue = dataG - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 指令EGR和EGR错误
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID69Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // 指令EGR A 工作周期/位置
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 7:
                    // 实际EGR A 工作周期/位置
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 8:
                    // EGR A 错误
                    value2.DoubleValue = dataD * 100.0 / 128.0 - 100;
                    break;
                case 9:
                    // 指令EGR B 工作周期/位置
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                case 10:
                    // 实际EGR B 工作周期/位置
                    value2.DoubleValue = dataF * 100.0 / 255.0;
                    break;
                case 11:
                    // EGR B 错误
                    value2.DoubleValue = dataG * 100.0 / 128.0 - 100.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 指令柴油进气流量控制和相对进气流量位置
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6AValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 指令进气流量 A 控制
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 5:
                    // 进气流量 A 相对位置
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 6:
                    // 指令进气流量 B 控制
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                case 7:
                    // 进气流量 B 相对位置
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 排气再循环（EGR）温度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6BValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 7) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                int dataB;
                switch (param.SubParameter) {
                case 8:
                    // EGR温度传感器 A（组1，传感器1）
                    dataB = param.SubParameter - 8;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) - 40.0;
                    } else if (value2.GetBitFlag(dataB + 4)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 4.0 - 40.0;
                    }
                    break;
                case 9:
                    // EGR温度传感器 C（组1，传感器2）
                    dataB = param.SubParameter - 8;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) - 40.0;
                    } else if (value2.GetBitFlag(dataB + 4)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 4.0 - 40.0;
                    }
                    break;
                case 10:
                    // EGR温度传感器 B（组2，传感器1）
                    dataB = param.SubParameter - 8;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) - 40.0;
                    } else if (value2.GetBitFlag(dataB + 4)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 4.0 - 40.0;
                    }
                    break;
                case 11:
                    // EGR温度传感器 D（组2，传感器2）
                    dataB = param.SubParameter - 8;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) - 40.0;
                    } else if (value2.GetBitFlag(dataB + 4)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 4.0 - 40.0;
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 指令节气门执行器控制和节气门相对位置
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6CValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 指令节气门执行器 A 控制
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 5:
                    // 节气门 A 相对位置
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 6:
                    // 指令节气门执行器 B 控制
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                case 7:
                    // 节气门 B 相对位置
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 燃油压力控制系统
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6DValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 11) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            int dataK = Utility.Hex2Int(response.GetDataByte(10));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // 指令燃油导轨压力 A
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 10.0;
                    break;
                case 7:
                    // 燃油导轨压力 A
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 10.0;
                    break;
                case 8:
                    // 燃油温度 A
                    value2.DoubleValue = dataF - 40.0;
                    break;
                case 9:
                    // 指令燃油导轨压力 B
                    value2.DoubleValue = ((dataG * 256.0) + dataH) * 10.0;
                    break;
                case 10:
                    // 燃油导轨压力 B
                    value2.DoubleValue = ((dataI * 256.0) + dataJ) * 10.0;
                    break;
                case 11:
                    // 燃油温度 B
                    value2.DoubleValue = dataK - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 燃油喷射压力控制
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6EValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 指令喷射控制压力 A
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 10.0;
                    break;
                case 5:
                    // 喷射控制压力 A
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 10.0;
                    break;
                case 6:
                    // 指令喷射控制压力 B
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 10.0;
                    break;
                case 7:
                    // 喷射控制压力 B
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 10.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 涡轮增压压缩机进气压力
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID6FValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 3) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                int dataB;
                switch (param.SubParameter) {
                case 4:
                    // 涡轮增压压缩机进气压力传感器 A
                    dataB = param.SubParameter - 4;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1));
                    } else if (value2.GetBitFlag(dataB + 2)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 8.0;
                    }
                    break;
                case 5:
                    // 涡轮增压压缩机进气压力传感器 B
                    dataB = param.SubParameter - 4;
                    if (value2.GetBitFlag(dataB)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1));
                    } else if (value2.GetBitFlag(dataB + 2)) {
                        value2.DoubleValue = Utility.Hex2Int(response.GetDataByte(dataB + 1)) * 8.0;
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 增压压力控制
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID70Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 10) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // 指令增压压力 A
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 2048.0 / 65535.0;
                    break;
                case 7:
                    // 增压压力 A
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 2048.0 / 65535.0;
                    break;
                case 8:
                    // 指令增压压力 B
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 2048.0 / 65535.0;
                    break;
                case 9:
                    // 增压压力 B
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 2048.0 / 65535.0;
                    break;
                case 10:
                    // 增压压力 A 控制状态
                    switch (dataJ & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "保留，未定义";
                        value2.ShortStringValue = "——";
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "开环（无错误）";
                        value2.ShortStringValue = "BP_A_OL";
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "闭环（无错误）";
                        value2.ShortStringValue = "BP_A_CL";
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "出现错误（增压数据不正确）";
                        value2.ShortStringValue = "BP_A_FAULT";
                        break;
                    }
                    break;
                case 11:
                    // 增压压力 B 控制状态
                    switch ((dataJ >> 2) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "保留，未定义";
                        value2.ShortStringValue = "——";
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "开环（无错误）";
                        value2.ShortStringValue = "BP_A_OL";
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "闭环（无错误）";
                        value2.ShortStringValue = "BP_A_CL";
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "出现错误（增压数据不正确）";
                        value2.ShortStringValue = "BP_A_FAULT";
                        break;
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 可变几何涡轮增压（VGT）控制
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID71Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 6) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // 指令VGT A 位置
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 7:
                    // VGT A 位置
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 8:
                    // 指令VGT B 位置
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                case 9:
                    // VGT B 位置
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                case 10:
                    // VGT A 控制状态
                    switch (dataF & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "保留，未定义";
                        value2.ShortStringValue = "——";
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "开环（无错误）";
                        value2.ShortStringValue = "VGT_A_OL";
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "闭环（无错误）";
                        value2.ShortStringValue = "VGT_A_CL";
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "出现错误（VGT数据不正确）";
                        value2.ShortStringValue = "VGT_A_FAULT";
                        break;
                    }
                    break;
                case 11:
                    // VGT B 控制状态
                    switch ((dataF >> 2) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "保留，未定义";
                        value2.ShortStringValue = "——";
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "开环（无错误）";
                        value2.ShortStringValue = "VGT_A_OL";
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "闭环（无错误）";
                        value2.ShortStringValue = "VGT_A_CL";
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "出现错误（VGT数据不正确）";
                        value2.ShortStringValue = "VGT_A_FAULT";
                        break;
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 排气门控制
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID72Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 指令排气门 A 位置
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 5:
                    // 排气门 A 位置
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 6:
                    // 指令排气门 B 位置
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                case 7:
                    // 排气门 B 位置
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 排气压力
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID73Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 排气压力传感器 组1
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.01;
                    break;
                case 3:
                    // 排气压力传感器 组2
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.01;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 涡轮增压器转速
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID74Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 涡轮增压器 A 转速
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 10.0;
                    break;
                case 3:
                    // 涡轮增压器 B 转速
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 10.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// 涡轮增压器 A / B 温度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID75or76Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 涡轮增压器 A / B 压缩机进气温度
                    value2.DoubleValue = dataB - 40.0;
                    break;
                case 5:
                    // 涡轮增压器 A / B 压缩机排气温度
                    value2.DoubleValue = dataC - 40.0;
                    break;
                case 6:
                    // 涡轮增压器 A / B 涡轮进气温度
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.1 - 40.0;
                    break;
                case 7:
                    // 涡轮增压器 A / B 涡轮排气温度
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 0.1 - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 中冷器（CACT）温度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID77Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 中冷器（CACT）温度 组1，传感器1
                    value2.DoubleValue = dataB - 40.0;
                    break;
                case 5:
                    // 中冷器（CACT）温度 组1，传感器2
                    value2.DoubleValue = dataC - 40.0;
                    break;
                case 6:
                    // 中冷器（CACT）温度 组2，传感器1
                    value2.DoubleValue = dataD - 40.0;
                    break;
                case 7:
                    // 中冷器（CACT）温度 组2，传感器2
                    value2.DoubleValue = dataE - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 排气（EGT）温度 组 1 / 2
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID78or79Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 排气（EGT）温度 组 1 / 2，传感器1
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.1 - 40.0;
                    break;
                case 5:
                    // 排气（EGT）温度 组 1 / 2，传感器2
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.1 - 40.0;
                    break;
                case 6:
                    // 排气（EGT）温度 组 1 / 2，传感器3
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 0.1 - 40.0;
                    break;
                case 7:
                    // 排气（EGT）温度 组 1 / 2，传感器4
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 0.1 - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 柴油颗粒滤清器（DPF）压力 组 1 / 2
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID7Aor7BValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 2) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 3:
                    // 柴油颗粒滤清器（DPF）压差 组 1 / 2
                    value2.DoubleValue = Utility.Int2SInt((dataB * 256) + dataC, 2) * 0.01;
                    break;
                case 4:
                    // 柴油颗粒滤清器（DPF）入口压力 组 1 / 2
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.01;
                    break;
                case 5:
                    // 柴油颗粒滤清器（DPF）出口压力 组 1 / 2
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 0.01;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 柴油颗粒滤清器（DPF）温度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID7CValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 柴油颗粒滤清器（DPF）入口温度 组1
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.1 - 40.0;
                    break;
                case 5:
                    // 柴油颗粒滤清器（DPF）出口温度 组1
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.1 - 40.0;
                    break;
                case 6:
                    // 柴油颗粒滤清器（DPF）入口温度 组2
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 0.1 - 40.0;
                    break;
                case 7:
                    // 柴油颗粒滤清器（DPF）出口温度 组2
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 0.1 - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 引擎运转时间
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID7FValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 13) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            int dataK = Utility.Hex2Int(response.GetDataByte(10));
            int dataL = Utility.Hex2Int(response.GetDataByte(11));
            int dataM = Utility.Hex2Int(response.GetDataByte(12));
            value2.SetBitFlagBAT(dataA);

            int num = 0;
            if (param.SubParameter >= 0 && param.SubParameter <= 2) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 3:
                    // 引擎运转总时间
                    num = (dataB * 0x1000000) + (dataC * 0x10000) + (dataD * 0x100) + dataE;
                    break;
                case 4:
                    // 引擎怠速总时间
                    num = (dataF * 0x1000000) + (dataG * 0x10000) + (dataH * 0x100) + dataI;
                    break;
                case 5:
                    // PTO激活总运转时间
                    num = (dataJ * 0x1000000) + (dataK * 0x10000) + (dataL * 0x100) + dataM;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = num;
                value2.StringValue = (num / 3600).ToString() + " hrs, ";
                value2.StringValue += ((num % 3600) / 60).ToString() + " min, ";
                value2.StringValue += ((num % 3600) % 60).ToString() + " sec";
                value2.ShortStringValue = value2.StringValue;
            }
            return value2;
        }

        /// <summary>
        /// 辅助排放控制装置（AECD）#1 - #5 / #6 - #10 / #11 - #15 / #16 - #20的引擎运转时间
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID81or82or89or8AValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 41) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int[] dataB = new int[4];
            int offset = 1;
            for (int i = 0; i < dataB.Length; i++) {
                dataB[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataC = new int[4];
            offset += dataB.Length;
            for (int i = 0; i < dataC.Length; i++) {
                dataC[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataD = new int[4];
            offset += dataC.Length;
            for (int i = 0; i < dataD.Length; i++) {
                dataD[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataE = new int[4];
            offset += dataD.Length;
            for (int i = 0; i < dataE.Length; i++) {
                dataE[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataF = new int[4];
            offset += dataE.Length;
            for (int i = 0; i < dataF.Length; i++) {
                dataF[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataG = new int[4];
            offset += dataF.Length;
            for (int i = 0; i < dataG.Length; i++) {
                dataG[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataH = new int[4];
            offset += dataG.Length;
            for (int i = 0; i < dataH.Length; i++) {
                dataH[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataI = new int[4];
            offset += dataH.Length;
            for (int i = 0; i < dataI.Length; i++) {
                dataI[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataJ = new int[4];
            offset += dataI.Length;
            for (int i = 0; i < dataJ.Length; i++) {
                dataJ[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            int[] dataK = new int[4];
            offset += dataJ.Length;
            for (int i = 0; i < dataK.Length; i++) {
                dataK[i] = Utility.Hex2Int(response.GetDataByte(i + offset));
            }
            value2.SetBitFlagBAT(dataA);

            int num = 0;
            if (param.SubParameter >= 0 && param.SubParameter <= 4) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 5:
                    // EI-AECD #1 / #6 / #11 / #16 定时器1 激活时总共运转时间
                    num = (dataB[0] * 0x1000000) + (dataB[1] * 0x10000) + (dataB[2] * 0x100) + dataB[3];
                    break;
                case 6:
                    // EI-AECD #1 / #6 / #11 / #16 定时器2 激活时总共运转时间
                    num = (dataC[0] * 0x1000000) + (dataC[1] * 0x10000) + (dataC[2] * 0x100) + dataC[3];
                    break;
                case 7:
                    // EI-AECD #2 / #7 / #12 / #17 定时器1 激活时总共运转时间
                    num = (dataD[0] * 0x1000000) + (dataD[1] * 0x10000) + (dataD[2] * 0x100) + dataD[3];
                    break;
                case 8:
                    // EI-AECD #2 / #7 / #12 / #17 定时器2 激活时总共运转时间
                    num = (dataE[0] * 0x1000000) + (dataE[1] * 0x10000) + (dataE[2] * 0x100) + dataE[3];
                    break;
                case 9:
                    // EI-AECD #3 / #8 / #13 / #18 定时器1 激活时总共运转时间
                    num = (dataF[0] * 0x1000000) + (dataF[1] * 0x10000) + (dataF[2] * 0x100) + dataF[3];
                    break;
                case 10:
                    // EI-AECD #3 / #8 / #13 / #18 定时器2 激活时总共运转时间
                    num = (dataG[0] * 0x1000000) + (dataG[1] * 0x10000) + (dataG[2] * 0x100) + dataG[3];
                    break;
                case 11:
                    // EI-AECD #4 / #9 / #14 / #19 定时器1 激活时总共运转时间
                    num = (dataH[0] * 0x1000000) + (dataH[1] * 0x10000) + (dataH[2] * 0x100) + dataH[3];
                    break;
                case 12:
                    // EI-AECD #4 / #9 / #14 / #19 定时器2 激活时总共运转时间
                    num = (dataI[0] * 0x1000000) + (dataI[1] * 0x10000) + (dataI[2] * 0x100) + dataI[3];
                    break;
                case 13:
                    // EI-AECD #5 / #10 / #15 / #20 定时器1 激活时总共运转时间
                    num = (dataJ[0] * 0x1000000) + (dataJ[1] * 0x10000) + (dataJ[2] * 0x100) + dataJ[3];
                    break;
                case 14:
                    // EI-AECD #5 / #10 / #15 / #20 定时器2 激活时总共运转时间
                    num = (dataK[0] * 0x1000000) + (dataK[1] * 0x10000) + (dataK[2] * 0x100) + dataK[3];
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = num;
                value2.StringValue = (num / 3600).ToString() + " hrs, ";
                value2.StringValue += ((num % 3600) / 60).ToString() + " min, ";
                value2.StringValue += ((num % 3600) % 60).ToString() + " sec";
                value2.ShortStringValue = value2.StringValue;
            }
            return value2;
        }

        /// <summary>
        /// NOx传感器浓度
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID83orA7Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else if (param.SubParameter >= 4 && param.SubParameter <= 7) {
                // 数据可用性，如果数据可用性位 = 1，传感器数据应报告 $FFF
                value2.SetBoolValueWithData(dataA, param.SubParameter, true);
            } else {
                switch (param.SubParameter) {
                case 8:
                    // NOx传感器浓度 组1 传感器1 / 3
                    value2.DoubleValue = (dataB * 256.0) + dataC;
                    break;
                case 9:
                    // NOx传感器浓度 组1 传感器2 / 4
                    value2.DoubleValue = (dataD * 256.0) + dataE;
                    break;
                case 10:
                    // NOx传感器浓度 组2 传感器1 / 3
                    value2.DoubleValue = (dataF * 256.0) + dataG;
                    break;
                case 11:
                    // NOx传感器浓度 组2 传感器2 / 4
                    value2.DoubleValue = (dataH * 256.0) + dataI;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// NOx控制系统
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID85Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 10) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 平均反应物消耗量
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.005;
                    break;
                case 5:
                    // 平均反应物需求消耗量
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.005;
                    break;
                case 6:
                    // 反应物余量
                    value2.DoubleValue = dataF * 100.0 / 255.0;
                    break;
                case 7:
                    // NOx警告模式激活时引擎总运转时间
                    int num = (dataG * 0x1000000) + (dataH * 0x10000) + (dataI * 0x100) + dataJ;
                    value2.DoubleValue = num;
                    value2.StringValue = (num / 3600).ToString() + " hrs, ";
                    value2.StringValue += ((num % 3600) / 60).ToString() + " min, ";
                    value2.StringValue += ((num % 3600) % 60).ToString() + " sec";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 颗粒物（PM）传感器
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID86Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // PM传感器质量浓度 组1，传感器1
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.0125;
                    break;
                case 3:
                    // PM传感器质量浓度 组2，传感器1
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.0125;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 进气歧管绝对压力
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID87Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 进气歧管绝对压力 A
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 2048.0 / 65535.0;
                    break;
                case 3:
                    // 进气歧管绝对压力 B
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 2048.0 / 65535.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        void SetPID88Bit(ref OBDParameterValue value2, int data, int index) {
            switch (index) {
            case 0:
            case 5:
            case 9:
            case 13:
            case 17:
                if ((data & 1) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "试剂量太低";
                    value2.ShortStringValue = "LEVEL_LOW";
                }
                break;
            case 1:
            case 6:
            case 10:
            case 14:
            case 18:
                if ((data & 2) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "试剂错误";
                    value2.ShortStringValue = "INCORR_REAG";
                }
                break;
            case 2:
            case 7:
            case 11:
            case 15:
            case 19:
                if ((data & 4) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "试剂消耗偏差";
                    value2.ShortStringValue = "CONSUMP_DEVIATION";
                }
                break;
            case 3:
            case 8:
            case 12:
            case 16:
            case 20:
                if ((data & 8) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "NOx排放量太高";
                    value2.ShortStringValue = "NOx_LEVEL";
                }
                break;
            }
            if (index >= 5 && index <= 8) {
                value2.ShortStringValue += "1";
            } else if (index >= 9 && index <= 12) {
                value2.ShortStringValue += "2";
            } else if (index >= 13 && index <= 16) {
                value2.ShortStringValue += "3";
            } else if (index >= 17 && index <= 20) {
                value2.ShortStringValue += "4";
            }
        }

        /// <summary>
        /// SCR诱导系统
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID88Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 13) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            int dataK = Utility.Hex2Int(response.GetDataByte(10));
            int dataL = Utility.Hex2Int(response.GetDataByte(11));
            int dataM = Utility.Hex2Int(response.GetDataByte(12));

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                // SCR诱导系统实际状态
                SetPID88Bit(ref value2, dataA, param.SubParameter);
            } else if (param.SubParameter >= 5 && param.SubParameter <= 8) {
                // SCR诱导系统状态10千公里历史
                SetPID88Bit(ref value2, dataB, param.SubParameter);
            } else if (param.SubParameter >= 9 && param.SubParameter <= 12) {
                // SCR诱导系统状态20千公里历史
                SetPID88Bit(ref value2, dataB >> 4, param.SubParameter);
            } else if (param.SubParameter >= 13 && param.SubParameter <= 16) {
                // SCR诱导系统状态30千公里历史
                SetPID88Bit(ref value2, dataC, param.SubParameter);
            } else if (param.SubParameter >= 17 && param.SubParameter <= 20) {
                // SCR诱导系统状态40千公里历史
                SetPID88Bit(ref value2, dataC >> 4, param.SubParameter);
            } else if (param.SubParameter == 4) {
                if ((dataA & 0x80) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "诱导系统激活";
                    value2.ShortStringValue = "ACTIVE";
                }
            } else {
                switch (param.SubParameter) {
                case 21:
                    value2.DoubleValue = (dataD * 256.0) + dataE;
                    break;
                case 22:
                    value2.DoubleValue = (dataF * 256.0) + dataG;
                    break;
                case 23:
                    value2.DoubleValue = (dataH * 256.0) + dataI;
                    break;
                case 24:
                    value2.DoubleValue = (dataJ * 256.0) + dataK;
                    break;
                case 25:
                    value2.DoubleValue = (dataL * 256.0) + dataM;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.621371192;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 柴油后处理状态
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID8BValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 6) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 7:
                    // 柴油颗粒过滤器（DPF）再生状态
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "DPF再生中";
                        value2.ShortStringValue = "DPF_REGEN_STAT: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "DPF未再生";
                        value2.ShortStringValue = "DPF_REGEN_STAT: NO";
                    }
                    break;
                case 8:
                    // 柴油颗粒过滤器（DPF）再生类型
                    if ((dataB & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "主动DPF再生";
                        value2.ShortStringValue = "DPF_REGEN_TYPE: ACTIVE";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "被动DPF再生";
                        value2.ShortStringValue = "DPF_REGEN_TYPE: PASSIVE";
                    }
                    break;
                case 9:
                    // NOx吸附再生状态
                    if ((dataB & 4) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "NOx脱附（再生）中";
                        value2.ShortStringValue = "NOX_ADS_REGEN: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "NOx吸附（未再生）中";
                        value2.ShortStringValue = "NOX_ADS_REGEN: NO";
                    }
                    break;
                case 10:
                    // NOx吸附脱硫状态
                    if ((dataB & 8) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "NOx脱硫中";
                        value2.ShortStringValue = "NOX_ADS_DESULF: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "NOx未脱硫";
                        value2.ShortStringValue = "NOX_ADS_DESULF: NO";
                    }
                    break;
                case 11:
                    // DPF再生标准化触发器
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 12:
                    // DPF再生间隔平均时间
                    value2.DoubleValue = (dataD * 256.0) + dataE;
                    break;
                case 13:
                    // DPF再生间隔平均距离
                    value2.DoubleValue = (dataF * 256.0) + dataG;
                    if (bEnglishUnits) {
                        value2.DoubleValue *= 0.621371192;
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 氧气传感器（宽量程）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID8Cor9CValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 17) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            int dataK = Utility.Hex2Int(response.GetDataByte(10));
            int dataL = Utility.Hex2Int(response.GetDataByte(11));
            int dataM = Utility.Hex2Int(response.GetDataByte(12));
            int dataN = Utility.Hex2Int(response.GetDataByte(13));
            int dataO = Utility.Hex2Int(response.GetDataByte(14));
            int dataP = Utility.Hex2Int(response.GetDataByte(15));
            int dataQ = Utility.Hex2Int(response.GetDataByte(16));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 7) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 8:
                    // 氧气传感器浓度 组1，传感器1 / 3
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 100.0 / 65535.0;
                    break;
                case 9:
                    // 氧气传感器浓度 组1，传感器2 / 4
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 100.0 / 65535.0;
                    break;
                case 10:
                    // 氧气传感器浓度 组2，传感器1 / 3
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 100.0 / 65535.0;
                    break;
                case 11:
                    // 氧气传感器浓度 组2，传感器2 / 4
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 100.0 / 65535.0;
                    break;
                case 12:
                    // 氧气传感器λ 组1，传感器1 / 3
                    value2.DoubleValue = ((dataJ * 256.0) + dataK) * 8.0 / 65535.0;
                    break;
                case 13:
                    // 氧气传感器λ 组1，传感器2 / 4
                    value2.DoubleValue = ((dataL * 256.0) + dataM) * 8.0 / 65535.0;
                    break;
                case 14:
                    // 氧气传感器λ 组2，传感器1 / 3
                    value2.DoubleValue = ((dataN * 256.0) + dataO) * 8.0 / 65535.0;
                    break;
                case 15:
                    // 氧气传感器λ 组2，传感器2 / 4
                    value2.DoubleValue = ((dataP * 256.0) + dataQ) * 8.0 / 65535.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 颗粒物（PM）传感器输出
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID8FValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 7) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // PM传感器激活状态 组1，传感器1
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "传感器主动测量（是）";
                        value2.ShortStringValue = "PM11_ACTIVE: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "传感器主动测量（否）";
                        value2.ShortStringValue = "PM11_ACTIVE: NO";
                    }
                    break;
                case 5:
                    // PM传感器再生状态 组1，传感器1
                    if ((dataB & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "传感器再生（是）";
                        value2.ShortStringValue = "PM11_REGEN: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "传感器再生（否）";
                        value2.ShortStringValue = "PM11_REGEN: NO";
                    }
                    break;
                case 6:
                    // 氧气传感器浓度 组2，传感器1
                    value2.DoubleValue = Utility.Int2SInt((dataC * 256) + dataD, 2) * 0.01;
                    break;
                case 7:
                    // PM传感器激活状态 组2，传感器1
                    if ((dataE & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "传感器主动测量（是）";
                        value2.ShortStringValue = "PM11_ACTIVE: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "传感器主动测量（否）";
                        value2.ShortStringValue = "PM11_ACTIVE: NO";
                    }
                    break;
                case 8:
                    // PM传感器再生状态 组2，传感器1
                    if ((dataE & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "传感器再生（是）";
                        value2.ShortStringValue = "PM21_REGEN: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "传感器再生（否）";
                        value2.ShortStringValue = "PM21_REGEN: NO";
                    }
                    break;
                case 9:
                    // 氧气传感器浓度 组2，传感器1
                    value2.DoubleValue = Utility.Int2SInt((dataF * 256) + dataG, 2) * 0.01;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// WWH-OBD车辆OBD系统信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID90Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 3) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));

            switch (param.SubParameter) {
            case 0:
                // 有差别/无差别显示策略
                switch (dataA & 0x03) {
                case 0:
                    value2.DoubleValue = 0;
                    value2.StringValue = "所有 ECU 都采用无差别 MI 显示策略";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 1:
                    value2.DoubleValue = 1;
                    value2.StringValue = "所有 ECU 都采用有差别 MI 显示策略";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 2:
                    value2.DoubleValue = 2;
                    value2.StringValue = "保留";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 3:
                    value2.DoubleValue = 3;
                    value2.StringValue = "不适用";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                }
                break;
            case 1:
                // 车辆故障指示灯状态
                switch ((dataA >> 2) & 0x0F) {
                case 0:
                    value2.DoubleValue = 0;
                    value2.StringValue = "MI 激活模式 1（MI 关）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 1:
                    value2.DoubleValue = 1;
                    value2.StringValue = "MI 激活模式 2（需求 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 2:
                    value2.DoubleValue = 2;
                    value2.StringValue = "MI 激活模式 3（短 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 3:
                    value2.DoubleValue = 3;
                    value2.StringValue = "MI 激活模式 2（连续 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 0x0F:
                    value2.DoubleValue = 0x0F;
                    value2.StringValue = "不适用";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                default:
                    value2.DoubleValue = (dataA >> 2) & 0x0F;
                    value2.StringValue = "保留";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                }
                break;
            case 2:
                // 排放系统就绪状态
                if ((dataA & 0x40) != 0) {
                    value2.BoolValue = true;
                    value2.StringValue = "车辆排放系统监控均未完成";
                    value2.ShortStringValue = "VOBD_RDY: YES";
                } else {
                    value2.BoolValue = false;
                    value2.StringValue = "车辆排放系统监控均已完成";
                    value2.ShortStringValue = "VOBD_RDY: NO";
                }
                break;
            case 3:
                // 连续 MI 处于活动状态的发动机工作小时数（连续 MI 计数）
                value2.DoubleValue = (dataB * 256) + dataC;
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        /// <summary>
        /// WWH-OBD ECU OBD系统信息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID91Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 5) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));

            switch (param.SubParameter) {
            case 0:
                // 车辆故障指示灯状态
                switch (dataA & 0x0F) {
                case 0:
                    value2.DoubleValue = 0;
                    value2.StringValue = "MI 激活模式 1（MI 关）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 1:
                    value2.DoubleValue = 1;
                    value2.StringValue = "MI 激活模式 2（需求 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 2:
                    value2.DoubleValue = 2;
                    value2.StringValue = "MI 激活模式 3（短 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 3:
                    value2.DoubleValue = 3;
                    value2.StringValue = "MI 激活模式 2（连续 MI）";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                case 0x0F:
                    value2.DoubleValue = 0x0F;
                    value2.StringValue = "不适用";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                default:
                    value2.DoubleValue = dataA & 0x0F;
                    value2.StringValue = "保留";
                    value2.ShortStringValue = value2.StringValue;
                    break;
                }
                break;
            case 1:
                // 连续 MI 处于活动状态的发动机工作小时数（连续 MI 计数）
                value2.DoubleValue = (dataB * 256) + dataC;
                break;
            case 2:
                // ECU B1 计数最高值
                value2.DoubleValue = (dataD * 256) + dataE;
                break;
            default:
                value2.ErrorDetected = true;
                break;
            }
            return value2;
        }

        /// <summary>
        /// 燃油系统控制状态（压缩点火）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID92Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 2) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 7) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 8:
                    // 燃油压力控制 1 状态
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油压力 1 处于闭环控制中";
                        value2.ShortStringValue = "FP1_CL";
                    }
                    break;
                case 9:
                    // 燃油喷射量控制 1 状态
                    if ((dataB & 2) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油喷射量 1 处于闭环控制中";
                        value2.ShortStringValue = "FIQ1_CL";
                    }
                    break;
                case 10:
                    // 燃油喷射时间控制 1 状态
                    if ((dataB & 4) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油喷射时间 1 处于闭环控制中";
                        value2.ShortStringValue = "FIT1_CL";
                    }
                    break;
                case 11:
                    // 怠速燃油平衡/贡献控制 1 状态
                    if ((dataB & 8) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "怠速燃油平衡/贡献 1 处于闭环控制中";
                        value2.ShortStringValue = "IFB1_CL";
                    }
                    break;
                case 12:
                    // 燃油压力控制 2 状态
                    if ((dataB & 0x10) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油压力 2 处于闭环控制中";
                        value2.ShortStringValue = "FP2_CL";
                    }
                    break;
                case 13:
                    // 燃油喷射量控制 2 状态
                    if ((dataB & 0x20) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油喷射量 2 处于闭环控制中";
                        value2.ShortStringValue = "FIQ2_CL";
                    }
                    break;
                case 14:
                    // 燃油喷射时间控制 2 状态
                    if ((dataB & 0x40) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "燃油喷射时间 2 处于闭环控制中";
                        value2.ShortStringValue = "FIT2_CL";
                    }
                    break;
                case 15:
                    // 怠速燃油平衡/贡献控制 2 状态
                    if ((dataB & 0x80) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "怠速燃油平衡/贡献 2 处于闭环控制中";
                        value2.ShortStringValue = "IFB2_CL";
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// WWH-OBD车辆OBD计数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID93Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 3) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter == 0) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                // 累积连续 MI 计数器
                value2.DoubleValue = (dataB * 256) + dataC;
            }
            return value2;
        }

        /// <summary>
        /// NOx控制 - 驱动诱导系统状态和计数器
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID94Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 12) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            int dataJ = Utility.Hex2Int(response.GetDataByte(9));
            int dataK = Utility.Hex2Int(response.GetDataByte(10));
            int dataL = Utility.Hex2Int(response.GetDataByte(11));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 5) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 6:
                    // NOx警告系统激活状态
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "警告系统激活";
                        value2.ShortStringValue = "NOX_WARN_ACT: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "警告系统未激活";
                        value2.ShortStringValue = "NOX_WARN_ACT: NO";
                    }
                    break;
                case 7:
                    // 一级诱导状态
                    switch ((dataB >> 1) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "一级诱导未激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "一级诱导可用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "一级诱导激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "一级诱导不适用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    }
                    break;
                case 8:
                    // 二级诱导状态
                    switch ((dataB >> 3) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "二级诱导未激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "二级诱导可用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "二级诱导激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "二级诱导不适用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    }
                    break;
                case 9:
                    // 三级诱导状态
                    switch ((dataB >> 3) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "三级诱导未激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "三级诱导可用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "三级诱导激活";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "三级诱导不适用";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    }
                    break;
                case 10:
                    // 试剂质量计数器
                    value2.DoubleValue = (dataC * 256) + dataD;
                    break;
                case 11:
                    // 试剂消耗计数器
                    value2.DoubleValue = (dataE * 256) + dataF;
                    break;
                case 12:
                    // 配比活动计数器
                    value2.DoubleValue = (dataG * 256) + dataH;
                    break;
                case 13:
                    // EGR阀门计数器
                    value2.DoubleValue = (dataI * 256) + dataJ;
                    break;
                case 14:
                    // 监控系统计数器
                    value2.DoubleValue = (dataK * 256) + dataL;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// 排气温度（EGT）组1 / 2
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID98or99Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 排气温度 组1 / 2 传感器5
                    value2.DoubleValue = ((dataB * 256.0) + dataC) * 0.1 - 40.0;
                    break;
                case 5:
                    // 排气温度 组1 / 2 传感器6
                    value2.DoubleValue = ((dataD * 256.0) + dataE) * 0.1 - 40.0;
                    break;
                case 6:
                    // 排气温度 组1 / 2 传感器7
                    value2.DoubleValue = ((dataF * 256.0) + dataG) * 0.1 - 40.0;
                    break;
                case 7:
                    // 排气温度 组1 / 2 传感器8
                    value2.DoubleValue = ((dataH * 256.0) + dataI) * 0.1 - 40.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                }
            }
            return value2;
        }

        /// <summary>
        /// 混动/EV 车辆系统数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID9AValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 6) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // 混动/EV 充电状态
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "充电耗尽模式（CDM）";
                        value2.ShortStringValue = "CDM";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "充电维持模式（CSM）/ 非 PHEV";
                        value2.ShortStringValue = "CSM";
                    }
                    break;
                case 5:
                    // 增强型混动/EV 充电状态
                    switch ((dataB >> 1) & 0x03) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "充电维持模式（CSM）/ 非 PHEV";
                        value2.ShortStringValue = "CSM";
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "充电耗尽模式（CDM）";
                        value2.ShortStringValue = "CDM";
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "充电增长模式（CIM）";
                        value2.ShortStringValue = "CSM";
                        break;
                    case 3:
                        value2.DoubleValue = 3;
                        value2.StringValue = "无 PSA";
                        value2.ShortStringValue = "NO PSA";
                        break;
                    }
                    break;
                case 6:
                    // 混动/EV 电池系统电压
                    value2.DoubleValue = ((dataC * 256.0) + dataD) * 1024.0 / 65535.0;
                    break;
                case 7:
                    // 混动/EV 电池系统电流
                    value2.DoubleValue = Utility.Int2SInt((dataE * 256) + dataF, 2) * 0.1;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 柴油排气流体传感器输出
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID9BValue(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // DEF 类型
                    switch ((dataA >> 4) & 0x0F) {
                    case 0:
                        value2.DoubleValue = 0;
                        value2.StringValue = "尿素浓度过高";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 1:
                        value2.DoubleValue = 1;
                        value2.StringValue = "尿素浓度过低";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 2:
                        value2.DoubleValue = 2;
                        value2.StringValue = "流体为柴油";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    case 13:
                        value2.DoubleValue = 13;
                        value2.StringValue = "无错误，无结果";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    default:
                        value2.DoubleValue = (dataA >> 4) & 0x0F;
                        value2.StringValue = "保留";
                        value2.ShortStringValue = value2.StringValue;
                        break;
                    }
                    break;
                case 5:
                    // DEF 浓度
                    value2.DoubleValue = dataB * 0.25;
                    break;
                case 6:
                    // DEF 温度
                    value2.DoubleValue = dataC - 40.0;
                    if (bEnglishUnits) {
                        value2.DoubleValue = (value2.DoubleValue * 1.8) + 32.0;
                    }
                    break;
                case 7:
                    // DEF 余量
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 燃油系统使用百分比
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPID9FValue(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 7) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 8:
                    // 燃油系统 A 使用百分比 组1
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 9:
                    // 燃油系统 B 使用百分比 组1
                    value2.DoubleValue = dataC * 100.0 / 255.0;
                    break;
                case 10:
                    // 燃油系统 A 使用百分比 组2
                    value2.DoubleValue = dataD * 100.0 / 255.0;
                    break;
                case 11:
                    // 燃油系统 B 使用百分比 组2
                    value2.DoubleValue = dataE * 100.0 / 255.0;
                    break;
                case 12:
                    // 燃油系统 A 使用百分比 组3
                    value2.DoubleValue = dataF * 100.0 / 255.0;
                    break;
                case 13:
                    // 燃油系统 B 使用百分比 组3
                    value2.DoubleValue = dataG * 100.0 / 255.0;
                    break;
                case 14:
                    // 燃油系统 A 使用百分比 组4
                    value2.DoubleValue = dataH * 100.0 / 255.0;
                    break;
                case 15:
                    // 燃油系统 B 使用百分比 组4
                    value2.DoubleValue = dataI * 100.0 / 255.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// NOx 传感器已校正数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA1orA8Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else if (param.SubParameter >= 4 && param.SubParameter <= 7) {
                // 数据可用性，如果数据可用性位 = 1，传感器数据应报告 $FFF
                value2.SetBoolValueWithData(dataA, param.SubParameter, true);
            } else {
                switch (param.SubParameter) {
                case 8:
                    // NOx 传感器已校正浓度 组1 传感器1 / 3
                    value2.DoubleValue = (dataB * 256.0) + dataC;
                    break;
                case 9:
                    // NOx 传感器已校正浓度 组1 传感器2 / 4
                    value2.DoubleValue = (dataD * 256.0) + dataE;
                    break;
                case 10:
                    // NOx 传感器已校正浓度 组2 传感器1 / 3
                    value2.DoubleValue = (dataF * 256.0) + dataG;
                    break;
                case 11:
                    // NOx 传感器已校正浓度 组2 传感器2 / 4
                    value2.DoubleValue = (dataH * 256.0) + dataI;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// EVAP系统燃油蒸发压力
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA3Value(OBDParameter param, OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 9) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            int dataE = Utility.Hex2Int(response.GetDataByte(4));
            int dataF = Utility.Hex2Int(response.GetDataByte(5));
            int dataG = Utility.Hex2Int(response.GetDataByte(6));
            int dataH = Utility.Hex2Int(response.GetDataByte(7));
            int dataI = Utility.Hex2Int(response.GetDataByte(8));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 3) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 4:
                    // EVAP系统燃油蒸发压力 A
                    value2.DoubleValue = Utility.Int2SInt((dataB * 256) + dataC, 2) * 0.25;
                    break;
                case 5:
                    // EVAP系统燃油蒸发压力 A（宽量程）
                    value2.DoubleValue = Utility.Int2SInt((dataD * 256) + dataE, 2) * 2.0;
                    break;
                case 6:
                    // EVAP系统燃油蒸发压力 B
                    value2.DoubleValue = Utility.Int2SInt((dataF * 256) + dataG, 2) * 0.25;
                    break;
                case 7:
                    // EVAP系统燃油蒸发压力 B（宽量程）
                    value2.DoubleValue = Utility.Int2SInt((dataH * 256) + dataI, 2) * 2.0;
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
                if (bEnglishUnits) {
                    value2.DoubleValue *= 0.0145037738;
                }
                value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
            }
            return value2;
        }

        /// <summary>
        /// 实际传动挡位
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA4Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 实际传动挡位
                    value2.DoubleValue = (dataB >> 4) & 0x0F;
                    break;
                case 3:
                    // 实际传动比
                    value2.DoubleValue = ((dataC * 256.0) + dataD) * 0.001;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// 柴油排液增加剂量
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA5Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 1) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 2:
                    // 指令DEF剂量
                    value2.DoubleValue = dataB * 100.0 / 255.0;
                    break;
                case 3:
                    // 当前驾驶循环中的DEF消耗
                    value2.DoubleValue = ((dataC * 256.0) + dataD) * 0.0005;
                    value2.DoubleValue = Math.Round(value2.DoubleValue, 2);
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

        /// <summary>
        /// 车辆总里程ODO
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <param name="bEnglishUnits"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA6Value(OBDResponse response, bool bEnglishUnits) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 4) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            int dataC = Utility.Hex2Int(response.GetDataByte(2));
            int dataD = Utility.Hex2Int(response.GetDataByte(3));

            value2.DoubleValue = ((dataA * 0x1000000) + (dataB * 0x10000) + (dataC * 0x100) + dataD) * 0.1;
            if (bEnglishUnits) {
                value2.DoubleValue *= 0.621371192;
            }
            value2.DoubleValue = Math.Round(value2.DoubleValue, 1);
            return value2;
        }

        /// <summary>
        /// 摩托车输入输出状态
        /// </summary>
        /// <param name="param"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public OBDParameterValue GetPIDA9Value(OBDParameter param, OBDResponse response) {
            OBDParameterValue value2 = new OBDParameterValue();
            if (response.GetDataByteCount() < 2) {
                value2.ErrorDetected = true;
                return value2;
            }
            int dataA = Utility.Hex2Int(response.GetDataByte(0));
            int dataB = Utility.Hex2Int(response.GetDataByte(1));
            value2.SetBitFlagBAT(dataA);

            if (param.SubParameter >= 0 && param.SubParameter <= 0) {
                value2.SetBoolValueWithData(dataA, param.SubParameter);
            } else {
                switch (param.SubParameter) {
                case 1:
                    // ABS禁用状态
                    if ((dataB & 1) != 0) {
                        value2.BoolValue = true;
                        value2.StringValue = "已禁用ABS（是）";
                        value2.ShortStringValue = "ABS_DISABLED: YES";
                    } else {
                        value2.BoolValue = false;
                        value2.StringValue = "未禁用ABS（否）";
                        value2.ShortStringValue = "ABS_DISABLED: NO";
                    }
                    break;
                default:
                    value2.ErrorDetected = true;
                    break;
                }
            }
            return value2;
        }

    }
}
