using System;
using System.IO.Ports;

namespace SH_OBD {
    public class OBDDeviceELM327 : OBDDevice {
        private ProtocolType m_iProtocol;
        private int m_iBaudRateIndex;
        private int m_iComPortIndex;
        private bool m_bConnected;

        public OBDDeviceELM327(Logger log) : base(log) {
            try {
                m_iProtocol = ProtocolType.Unknown;
                m_bConnected = false;
            } catch (Exception) {
                throw;
            }
        }

        override public void SetTimeout(int iTimeout = 500) {
            m_CommELM.SetTimeout(iTimeout);
        }

        public override bool Initialize(int iPort, int iBaud, ProtocolType iProtocol) {
            SetProtocol(iProtocol);
            return Initialize(iPort, iBaud);
        }

        public override bool Initialize(int iPort, int iBaud) {
            try {
                if (m_CommELM.Online) {
                    return true;
                }
                m_CommELM.SetPort(iPort);
                m_CommELM.SetBaudRate(iBaud);

                if (!m_CommELM.Open()) {
                    return false;
                }
                if (!ConfirmAT("ATWS") || !ConfirmAT("ATE0") || !ConfirmAT("ATL0") || !ConfirmAT("ATH1") || !ConfirmAT("ATCAF1")) {
                    m_CommELM.Close();
                    return false;
                }

                base.m_DeviceDes = GetDeviceDes().Trim();
                base.m_DeviceID = GetDeviceID().Trim();
                if (m_iProtocol != ProtocolType.Unknown) {
                    if (!ConfirmAT("ATSP" + ((int)m_iProtocol).ToString())) {
                        m_CommELM.Close();
                        return false;
                    }

                    m_CommELM.SetTimeout(5000);
                    bool flag = false;
                    if (GetOBDResponse("0100").Replace(" ", "").IndexOf("4100") >= 0 || GetOBDResponse("22F810").Replace(" ", "").IndexOf("62F810") >= 0) {
                        flag = true;
                        if (m_Parser == null) {
                            SetProtocol((ProtocolType)int.Parse(GetOBDResponse("ATDPN").Replace("A", "")));
                        }
                    }

                    m_CommELM.SetTimeout(500);
                    return flag;
                } else {
                    if (!ConfirmAT("ATM0")) {
                        m_CommELM.Close();
                        return false;
                    }

                    m_CommELM.SetTimeout(5000);
                    int[] xattr = new int[] { 6, 7, 2, 3, 1, 8, 9, 4, 5 };
                    for (int idx = 0; idx < xattr.Length; idx++) {
                        if (!ConfirmAT("ATTP" + xattr[idx].ToString())) {
                            m_CommELM.Close();
                            return false;
                        }
                        if (GetOBDResponse("0100").Replace(" ", "").IndexOf("4100") >= 0 || GetOBDResponse("22F810").Replace(" ", "").IndexOf("62F810") >= 0) {
                            if (m_Parser == null) {
                                SetProtocol((ProtocolType)xattr[idx]);
                            }
                            SetBaudRateIndex(iBaud);
                            m_iComPortIndex = iPort;
                            m_CommELM.SetTimeout(500);
                            ConfirmAT("ATM1");
                            return true;
                        }
                    }
                    // 每个协议都无法连接的话就关闭端口准备退出
                    if (m_CommELM.Online) {
                        m_CommELM.Close();
                    }
                }
            } catch (Exception ex) {
                if (m_CommELM.Online) {
                    m_CommELM.Close();
                }
                m_log.TraceError("Initialize occur error: " + ex.Message);
            }
            return false;
        }

        public override bool Initialize(Settings settings) {
            try {
                if (m_CommELM.Online) {
                    return true;
                }
                if (CommBase.GetPortAvailable(settings.ComPort) == CommBase.PortStatus.Available && Initialize(settings.ComPort, settings.BaudRate)) {
                    return true;
                }
                string[] serials = SerialPort.GetPortNames();
                for (int i = 0; i < serials.Length; i++) {
                    if (int.TryParse(serials[i].Substring(3), out int iPort)) {
                        if (iPort != settings.ComPort) {
                            if (CommBase.GetPortAvailable(iPort) == CommBase.PortStatus.Available
                                && (Initialize(iPort, 38400)/* || Initialize(iPort, 115200) || Initialize(iPort, 9600)*/)) {
                                return true;
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                if (m_CommELM.Online) {
                    m_CommELM.Close();
                }
                m_log.TraceError("Initialize occur error: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 获取OBD响应结果，若有错误的话总共尝试3次
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override OBDResponseList Query(OBDParameter param) {
            OBDResponseList orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
            for (int i = 2; i > 0; i--) {
                if (!orl.ErrorDetected) {
                    break;
                }
                orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
            }
            return orl;
        }

        public override string Query(string command) {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse(command);
            }
            return "";
        }

        public override void Disconnect() {
            if (m_CommELM.Online) {
                m_CommELM.Close();
            }
        }

        public override bool GetConnected() {
            return m_bConnected;
        }

        public override void SetConnected(bool status) {
            m_bConnected = status;
        }

        public void SetProtocol(ProtocolType iProtocol) {
            m_iProtocol = iProtocol;
            m_log.TraceInfo(string.Format("Protocol switched to: {0}", Settings.ProtocolNames[(int)iProtocol]));
            switch (iProtocol) {
            case ProtocolType.J1850_PWM:
                m_Parser = new OBDParser_J1850_PWM();
                break;
            case ProtocolType.J1850_VPW:
                m_Parser = new OBDParser_J1850_VPW();
                break;
            case ProtocolType.ISO9141_2:
                m_Parser = new OBDParser_ISO9141_2();
                break;
            case ProtocolType.ISO_14230_4_KWP_5BAUDINIT:
                m_Parser = new OBDParser_ISO14230_4_KWP();
                break;
            case ProtocolType.ISO_14230_4_KWP_FASTINIT:
                m_Parser = new OBDParser_ISO14230_4_KWP();
                break;
            case ProtocolType.ISO_15765_4_CAN_11BIT_500KBAUD:
                m_Parser = new OBDParser_ISO15765_4_CAN11();
                break;
            case ProtocolType.ISO_15765_4_CAN_29BIT_500KBAUD:
                m_Parser = new OBDParser_ISO15765_4_CAN29();
                break;
            case ProtocolType.ISO_15765_4_CAN_11BIT_250KBAUD:
                m_Parser = new OBDParser_ISO15765_4_CAN11();
                break;
            case ProtocolType.ISO_15765_4_CAN_29BIT_250KBAUD:
                m_Parser = new OBDParser_ISO15765_4_CAN29();
                break;
            }
        }

        /// <summary>
        /// Send command with 3 attempts
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool ConfirmAT(string command) {
            return ConfirmAT(command, 3);
        }

        /// <summary>
        /// Send command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public bool ConfirmAT(string command, int attempts) {
            if (!m_CommELM.Online) {
                return false;
            }
            for (int i = attempts; i > 0; i--) {
                string response = m_CommELM.GetResponse(command);
                if (response.IndexOf("OK") >= 0 || response.IndexOf("ELM") >= 0) {
                    return true;
                }
            }
            m_log.TraceWarning("Current device can't support command \"" + command + "\"!");
            return false;
        }

        public string GetOBDResponse(string command) {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse(command);
            }
            return "";
        }

        public string GetDeviceDes() {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse("AT@1");
            }
            return "";
        }

        public string GetDeviceID() {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse("ATI");
            }
            return "";
        }

        public override ProtocolType GetProtocolType() { return m_iProtocol; }
        public override int GetBaudRateIndex() { return m_iBaudRateIndex; }
        public void SetBaudRateIndex(int iBaud) {
            switch (iBaud) {
            case 9600:
                m_iBaudRateIndex = 0;
                break;
            case 38400:
                m_iBaudRateIndex = 1;
                break;
            case 115200:
                m_iBaudRateIndex = 2;
                break;
            default:
                m_iBaudRateIndex = -1;
                break;
            }
        }
        public override int GetComPortIndex() { return m_iComPortIndex; }

    }
}
