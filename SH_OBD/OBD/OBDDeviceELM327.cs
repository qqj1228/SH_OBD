using System;

namespace SH_OBD {
    public class OBDDeviceELM327 : OBDDevice {
        private ProtocolType m_iProtocol;

        public OBDDeviceELM327(OBDCommLog log) : base(log) {
            try {
                m_iProtocol = ProtocolType.Unknown;
            } catch (Exception ex) {
                throw ex;
            }
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

                // 原代码为if (m_CommELM.Open())
                if (!m_CommELM.Open()) {
                    return false;
                }

                if (!ConfirmAT("ATWS") || !ConfirmAT("ATE0") || !ConfirmAT("ATL0") || !ConfirmAT("ATH1") /*|| !confirmAT("ATCAF1")*/) {
                    m_CommELM.Close();
                    return false;
                }

                base.m_DeviceID = GetDeviceID();
                if (m_iProtocol != ProtocolType.Unknown) {
                    if (!ConfirmAT("ATSP" + ((int)m_iProtocol).ToString())) {
                        m_CommELM.Close();
                        return false;
                    }
                    //m_CommELM.Close();

                    m_CommELM.SetTimeout(5000);
                    //if (!m_CommELM.Open())
                    //	return false;

                    bool flag = false;
                    if (GetOBDResponse("0100").IndexOf("4100") >= 0) {
                        flag = true;
                        SetProtocol((ProtocolType)int.Parse(GetOBDResponse("ATDPN").Replace("A", "")));
                    }
                    //m_CommELM.Close();

                    m_CommELM.SetTimeout(500);
                    //if (!m_CommELM.Open())
                    //	flag = false;
                    return flag;
                }
                if (!ConfirmAT("ATM0")) {
                    m_CommELM.Close();
                    return false;
                }
                //m_CommELM.Close();

                m_CommELM.SetTimeout(5000);
                //if (!m_CommELM.Open())
                //	return false;

                int[] xattr = new int[] { 6, 7, 2, 3, 1, 8, 9, 4, 5 };
                for (int idx = 0; idx < xattr.Length; idx++) {
                    if (!ConfirmAT("ATTP" + xattr[idx].ToString())) {
                        m_CommELM.Close();
                        return false;
                    }

                    if (GetOBDResponse("0100").IndexOf("4100") >= 0) {
                        SetProtocol((ProtocolType)xattr[idx]);
                        //m_CommELM.Close();
                        m_CommELM.SetTimeout(500);
                        //if (!m_CommELM.Open())
                        //	return false;

                        ConfirmAT("ATM1");
                        return true;
                    }
                }
                if (m_CommELM.Online) {
                    m_CommELM.Close();
                }
            } catch (Exception) {
                if (m_CommELM.Online) {
                    m_CommELM.Close();
                }
            }
            return false;
        }

        public override bool Initialize() {
            try {
                if (m_CommELM.Online) {
                    return true;
                }
                for (int iPort = 0; iPort < 100; ++iPort) {
                    if (CommBase.GetPortAvailable(iPort) == CommBase.PortStatus.Available
                        && (Initialize(iPort, 38400) || Initialize(iPort, 115200) || Initialize(iPort, 9600))) {
                        return true;
                    }
                }
            } catch { }
            return false;
        }

        public override OBDResponseList Query(OBDParameter param) {
            return m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
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

        public override bool Connected() {
            return m_CommELM.Online;
        }

        public void SetProtocol(ProtocolType iProtocol) {
            m_iProtocol = iProtocol;
            base.m_commLog.AddItem(string.Format("Protocol switched to: {0}", Preferences.ProtocolNames[(int)iProtocol]));
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
            while (attempts > 0) {
                string response = m_CommELM.GetResponse(command);
                if (response.IndexOf("OK") >= 0 || response.IndexOf("ELM") >= 0) {
                    return true;
                }
                --attempts;
            }
            return false;
        }

        public string GetOBDResponse(string command) {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse(command);
            }
            return "";
        }

        public string GetDeviceID() {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse("ATI");
            }
            return "";
        }
    }
}
