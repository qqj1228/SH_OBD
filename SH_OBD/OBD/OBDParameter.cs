using System;

namespace SH_OBD {
    /// <summary>
    /// 用于发送命令的OBD参数
    /// </summary>
    public class OBDParameter {
        private int m_parameter;
        public int Parameter {
            get { return m_parameter; }
            set {
                m_parameter = value;
                if (m_OBDRequest.Length > 4) {
                    m_OBDRequest = m_OBDRequest.Substring(0, 2) + Utility.Int2Hex2(m_parameter) + m_OBDRequest.Substring(4, 2);
                } else if (m_OBDRequest.Length > 2) {
                    m_OBDRequest = m_OBDRequest.Substring(0, 2) + Utility.Int2Hex2(m_parameter);
                }
            }
        }
        private string m_OBDRequest;
        public string OBDRequest {
            get { return m_OBDRequest; }
            set {
                m_OBDRequest = value;
                if (m_OBDRequest.Length >= 4) {
                    bool bRet = int.TryParse(m_OBDRequest.Substring(0, 2), out int result);
                    if (bRet) {
                        Service = result;
                    } else {
                        try {
                            Service = Convert.ToInt32(m_OBDRequest.Substring(0, 2), 16);
                        } catch (Exception) {
                            Service = 0;
                        }
                    }
                    bRet = int.TryParse(m_OBDRequest.Substring(2, 2), out result);
                    if (bRet) {
                        m_parameter = result;
                    } else {
                        try {
                            m_parameter = Convert.ToInt32(m_OBDRequest.Substring(2, 2), 16);
                        } catch (Exception) {
                            m_parameter = 0;
                        }
                    }
                } else if (m_OBDRequest.Length >= 2) {
                    bool bRet = int.TryParse(m_OBDRequest.Substring(0, 2), out int result);
                    if (bRet) {
                        Service = result;
                    } else {
                        try {
                            Service = Convert.ToInt32(m_OBDRequest.Substring(0, 2), 16);
                        } catch (Exception) {
                            Service = 0;
                        }
                    }
                    m_parameter = 0;
                }
            }
        }
        public int Service { get; set; }
        public int SubParameter { get; set; }
        public double MetricMaxValue { get; set; }
        public double MetricMinValue { get; set; }
        public double EnglishMaxValue { get; set; }
        public double EnglishMinValue { get; set; }
        public int ValueTypes { get; set; }
        public int Priority { get; set; }
        public int Manufacturer { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public string MetricUnitLabel { get; set; } = "";
        public string EnglishUnitLabel { get; set; } = "";
        public string Name { get; set; } = "";
        public string PID { get; set; } = "";

        public OBDParameter(int service, int parameter, int subParameter, int frame) {
            m_OBDRequest = Utility.Int2Hex2(service) + Utility.Int2Hex2(parameter) + Utility.Int2Hex2(frame);
            Service = service;
            m_parameter = parameter;
            SubParameter = subParameter;
        }

        public OBDParameter(int service, int parameter, int subParameter) {
            m_OBDRequest = Utility.Int2Hex2(service) + Utility.Int2Hex2(parameter);
            Service = service;
            m_parameter = parameter;
            SubParameter = subParameter;
        }

        public OBDParameter() {
            m_OBDRequest = "";
        }

        public OBDParameter GetCopy() {
            OBDParameter p = new OBDParameter {
                Category = Category,
                ValueTypes = ValueTypes,
                EnglishMaxValue = EnglishMaxValue,
                EnglishMinValue = EnglishMinValue,
                EnglishUnitLabel = EnglishUnitLabel,
                Manufacturer = Manufacturer,
                MetricMaxValue = MetricMaxValue,
                MetricMinValue = MetricMinValue,
                MetricUnitLabel = MetricUnitLabel,
                Name = Name,
                m_OBDRequest = OBDRequest,
                m_parameter = Parameter,
                PID = PID,
                Priority = Priority,
                Service = Service,
                SubParameter = SubParameter,
                Type = Type
            };
            return p;
        }

        public OBDParameter GetFreezeFrameCopy(int iFrame) {
            OBDParameter copy = GetCopy();
            copy.Service = 2;
            copy.OBDRequest = "02" + copy.OBDRequest.Substring(2, 2) + iFrame.ToString("D2");
            return copy;
        }

        public override string ToString() {
            return Name;
        }

        public enum EnumValueTypes {
            Double = 0x01,
            Bool = 0x02,
            String = 0x04,
            ListString = 0x08,
            BitFlags = 0x20
        }

    }
}
