using System;

namespace SH_OBD {
    public class OBDDeviceELM323 : OBDDevice {
        public OBDDeviceELM323(OBDCommLog log) : base(log) {
            try {
                m_Parser = new OBDParser_ISO9141_2();
            } catch (Exception ex) {
                throw ex;
            }
        }

        public override bool Initialize(int iPort, int iBaud, ProtocolType iProtocol) {
            return Initialize(iPort, iBaud);
        }


        public override bool Initialize(int iPort, int iBaud) {
            try {
                if (m_CommELM.Online) {
                    return true;
                }
                m_CommELM.SetPort(iPort);
                m_CommELM.SetBaudRate(iBaud);
                if (m_CommELM.Open()) {
                    if (ConfirmAT("ATZ") && ConfirmAT("ATE0") && ConfirmAT("ATL0") && ConfirmAT("ATH1")) {
                        m_DeviceID = GetDeviceID();
                        return true;
                    }
                    m_CommELM.Close();
                }
            } catch { }
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
                return false;
            } catch (Exception) {
                return false;
            }
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

        public bool ConfirmAT(string command) {
            return ConfirmAT(command, 3);
        }
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
