using System;

namespace SH_OBD {
    public class OBDDeviceELM320 : OBDDevice {
        private int m_iBaudRateIndex;
        private int m_iComPortIndex;
        private bool m_bConnected;

        public OBDDeviceELM320(Logger log) : base(log) {
            try {
                m_Parser = new OBDParser_J1850_PWM();
                m_bConnected = false;
            } catch (Exception) {
                throw;
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
                    SetBaudRateIndex(iBaud);
                    m_iComPortIndex = iPort;
                    m_CommELM.Close();
                }
            } catch { }
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

        public override bool GetConnected() {
            return m_bConnected;
        }

        public override void SetConnected(bool status) {
            m_bConnected = status;
        }

        public bool ConfirmAT(string command) {
            return ConfirmAT(command, 3);
        }
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
            return false;
        }

        public string GetOBDResponse(string strCmd) {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse(strCmd);
            }
            return "";
        }

        public string GetDeviceID() {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse("ATI");
            }
            return "";
        }

        public override ProtocolType GetProtocolType() { return (ProtocolType)1; }
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