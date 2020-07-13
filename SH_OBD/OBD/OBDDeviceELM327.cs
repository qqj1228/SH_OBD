﻿using System;
using System.IO.Ports;
using System.Threading;

namespace SH_OBD {
    public class OBDDeviceELM327 : OBDDevice {
        private ProtocolType m_iProtocol;
        private StandardType m_iStandard;
        private int m_iBaudRateIndex;
        private bool m_bConnected;

        public OBDDeviceELM327(Logger log, int[] xattr) : base(log, xattr) {
            m_iProtocol = ProtocolType.Unknown;
            m_iStandard = StandardType.Automatic;
            m_bConnected = false;
        }

        public override bool Initialize(int iPort, int iBaud, ProtocolType iProtocol, StandardType iStandard) {
            SetProtocol(iProtocol);
            m_iStandard = iStandard;
            return Initialize(iPort, iBaud);
        }

        private void InitELM327Format() {
            ConfirmAT("ATE0");
            ConfirmAT("ATL0");
            ConfirmAT("ATH1");
            ConfirmAT("ATCAF1");
        }

        public override bool Initialize(int iPort, int iBaud) {
            try {
                if (m_CommELM.Online) {
                    return true;
                }
                m_CommELM.Port = iPort;
                m_CommELM.BaudRate = iBaud;

                if (!m_CommELM.Open()) {
                    return false;
                }
                if (!ConfirmAT("ATWS") || !ConfirmAT("ATE0") || !ConfirmAT("ATL0") || !ConfirmAT("ATH1") || !ConfirmAT("ATCAF1")) {
                    m_CommELM.Close();
                    return false;
                }

                GetVoltage();
                base.m_DeviceDes = GetDeviceDes().Trim();
                base.m_DeviceID = GetDeviceID().Trim().Replace("ELM327", "SH-VCI-302U");
                if (m_iProtocol != ProtocolType.Unknown) {
                    if (!ConfirmAT("ATSP" + ((int)m_iProtocol).ToString("X1"))) {
                        m_CommELM.Close();
                        return false;
                    }

                    int originalTimeout = m_CommELM.Timeout;
                    m_CommELM.Timeout = 5000;
                    m_iStandard = SetStandardType(m_iProtocol);
                    if (m_iStandard != StandardType.Automatic) {
                        if (m_Parser == null) {
                            string strDPN = GetOBDResponse("ATDPN").Replace("A", "");
                            if (strDPN.Length == 0) {
                                strDPN = "A";
                            }
                            SetProtocol((ProtocolType)Convert.ToInt32(strDPN, 16));
                        }
                    }

                    m_CommELM.Timeout = originalTimeout;
                    return m_iStandard != StandardType.Automatic;
                } else {
                    if (!ConfirmAT("ATM0")) {
                        m_CommELM.Close();
                        return false;
                    }

                    int originalTimeout = m_CommELM.Timeout;
                    m_CommELM.Timeout = 5000;
                    for (int idx = 0; idx < m_xattr.Length; idx++) {
                        if (!ConfirmAT("ATTP" + m_xattr[idx].ToString("X1"))) {
                            m_CommELM.Close();
                            return false;
                        }
                        m_iStandard = SetStandardType((ProtocolType)m_xattr[idx]);
                        if (m_iStandard != StandardType.Automatic) {
                            if (m_Parser == null) {
                                SetProtocol((ProtocolType)m_xattr[idx]);
                            }
                            SetBaudRateIndex(iBaud);
                            m_CommELM.Timeout = originalTimeout;
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
                m_iStandard = settings.StandardIndex;
                if (CommBase.GetPortAvailable(settings.ComPort) == CommBase.PortStatus.Available && Initialize(settings.ComPort, settings.BaudRate)) {
                    settings.FirstRun = false;
                    return true;
                }
                if (settings.FirstRun) {
                    string[] serials = SerialPort.GetPortNames();
                    for (int i = 0; i < serials.Length; i++) {
                        if (int.TryParse(serials[i].Substring(3), out int iPort)) {
                            if (iPort != settings.ComPort) {
                                if (CommBase.GetPortAvailable(iPort) == CommBase.PortStatus.Available
                                    && (Initialize(iPort, 38400)/* || Initialize(iPort, 115200) || Initialize(iPort, 9600)*/)) {
                                    settings.FirstRun = false;
                                    return true;
                                }
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

        private StandardType SetStandardType(ProtocolType protocol) {
            if (m_iStandard != StandardType.Automatic) {
                return m_iStandard;
            }
            bool bflag = false;
            StandardType standard = StandardType.Automatic;
            switch (protocol) {
            case ProtocolType.J1850_PWM:
            case ProtocolType.J1850_VPW:
            case ProtocolType.ISO9141_2:
            case ProtocolType.ISO_14230_4_KWP_5BAUDINIT:
            case ProtocolType.ISO_14230_4_KWP_FASTINIT:
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("0100").Replace(" ", "").Contains("4100")) {
                        bflag = true;
                        standard = StandardType.ISO_15031;
                    }
                }
                break;
            case ProtocolType.ISO_15765_4_CAN_11BIT_500KBAUD:
            case ProtocolType.ISO_15765_4_CAN_11BIT_250KBAUD:
                ConfirmAT("ATCF7E0");
                ConfirmAT("ATCM7F0");
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("22F810").Replace(" ", "").Contains("62F810")) {
                        bflag = bflag || true;
                        standard = StandardType.ISO_27145;
                    }
                }
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("0100").Replace(" ", "").Contains("4100")) {
                        bflag = true;
                        standard = StandardType.ISO_15031;
                    }
                }
                if (standard == StandardType.Automatic) {
                    ConfirmAT("ATAR");
                }
                break;
            case ProtocolType.ISO_15765_4_CAN_29BIT_500KBAUD:
            case ProtocolType.ISO_15765_4_CAN_29BIT_250KBAUD:
                ConfirmAT("ATCF18DAF100");
                ConfirmAT("ATCM1FFFFF00");
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("22F810").Replace(" ", "").Contains("62F810")) {
                        bflag = bflag || true;
                        standard = StandardType.ISO_27145;
                    }
                }
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("0100").Replace(" ", "").Contains("4100")) {
                        bflag = true;
                        standard = StandardType.ISO_15031;
                    }
                }
                if (standard == StandardType.Automatic) {
                    ConfirmAT("ATAR");
                }
                break;
            case ProtocolType.SAE_J1939_CAN_29BIT_250KBAUD:
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("00FECE").Replace(" ", "").Contains("60FECE")) {
                        bflag = bflag || true;
                        standard = StandardType.SAE_J1939;
                    }
                }
                break;
            default:
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("22F810").Replace(" ", "").Contains("62F810")) {
                        bflag = bflag || true;
                        standard = StandardType.ISO_27145;
                    }
                }
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("0100").Replace(" ", "").Contains("4100")) {
                        bflag = true;
                        standard = StandardType.ISO_15031;
                    }
                }
                for (int i = 3; i > 0 && !bflag; i--) {
                    if (GetOBDResponse("00FECE").Replace(" ", "").Contains("60FECE")) {
                        bflag = bflag || true;
                        standard = StandardType.SAE_J1939;
                    }
                }
                break;
            }
            m_log.TraceInfo("SetStandardType: " + standard.ToString());
            return standard;
        }

        public override OBDResponseList Query(OBDParameter param) {
            OBDResponseList orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
            int errorQty;
            for (errorQty = 0; errorQty < 2; errorQty++) {
                if (!orl.ErrorDetected) {
                    break;
                }
                if (m_bConnected) {
                    Thread.Sleep(500);
                }
                orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
            }
            // 重试3次后还是出错的话，软重启ELM327后再重试3次
            if (errorQty >= 2) {
                ConfirmAT("ATWS");
                InitELM327Format();
                ConfirmAT("ATSP" + ((int)m_iProtocol).ToString("X1"));
                for (errorQty = 0; errorQty < 3; errorQty++) {
                    if (!orl.ErrorDetected) {
                        break;
                    }
                    if (m_bConnected && errorQty != 0) {
                        Thread.Sleep(500);
                    }
                    orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
                }
            }
            if (orl.RawResponse == "PENDING") {
                int waittingTime = 60; // 重试总时间，单位秒
                int interval = 6; // 重试间隔时间，单位秒
                // NRC=78 处理
                m_log.TraceWarning("Receive only NRC78, handle pending message");
                switch (m_iProtocol) {
                case ProtocolType.J1850_PWM:
                case ProtocolType.J1850_VPW:
                    // 间隔30秒重复发送一次请求，直到有正响应消息返回
                    interval = 30;
                    for (int i = waittingTime / interval; i > 0; i--) {
                        Thread.Sleep(interval * 1000);
                        orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
                        if (!(orl.RawResponse == "PENDING" || orl.ErrorDetected)) {
                            break;
                        }
                    }
                    break;
                case ProtocolType.ISO9141_2:
                    // 间隔4秒重复发送一次请求，直到有正响应消息返回
                    interval = 4;
                    for (int i = waittingTime / interval; i > 0; i--) {
                        Thread.Sleep(interval * 1000);
                        orl = m_Parser.Parse(param, GetOBDResponse(param.OBDRequest));
                        if (!(orl.RawResponse == "PENDING" || orl.ErrorDetected)) {
                            break;
                        }
                    }
                    break;
                default:
                    // ISO14230-4 会在50ms内发送NRC78负响应直到发送正响应，v2.1以下的ELM327一般可收到所需要的消息，只需要过滤NRC78的负响应即可
                    // ISO15765-4 会在5s内发送NRC78负响应直到发送正响应，v2.1以下的ELM327无法收到所需要的消息，需要上层应用自己处理
                    // 间隔6s(5s加上传输延时)检测一次是否有正响应消息返回，如果没有则继续
                    for (int i = waittingTime / interval; i > 0; i--) {
                        Thread.Sleep(interval * 1000);
                        string RxLine = m_CommELM.GetRxLine();
                        if (RxLine != null && RxLine.Length > 0) {
                            m_log.TraceInfo("RX: " + RxLine.Replace("\r", "\\r"));
                            orl = m_Parser.Parse(param, RxLine);
                            if (!(orl.RawResponse == "PENDING" || orl.ErrorDetected)) {
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            return orl;
        }

        public override string Query(string command) {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse(command);
            }
            return "";
        }

        override public void SetTimeout(int iTimeout) {
            m_CommELM.Timeout = iTimeout;
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
            case ProtocolType.SAE_J1939_CAN_29BIT_250KBAUD:
                m_Parser = new OBDParser_SAE_J1939_CAN29();
                break;
            }
        }

        public bool ConfirmAT(string command, int attempts = 3) {
            if (!m_CommELM.Online) {
                return false;
            }
            for (int i = attempts; i > 0; i--) {
                string response = m_CommELM.GetResponse(command);
                if (response.Contains("OK") || response.Contains("ELM")) {
                    return true;
                } else if (response.Contains("STOPPED")) {
                    Thread.Sleep(500);
                }
            }
            m_log.TraceWarning("Current device can't support command \"" + command + "\"!");
            return false;
        }

        public string GetOBDResponse(string command) {
            string strRet = "";
            if (m_CommELM.Online) {
                strRet = m_CommELM.GetResponse(command);
            }
            // 返回"ERR94"说明发生CAN网络错误，ELM327会返回出厂设置
            // 如需继续的话，需要重新初始化ELM327
            if (strRet.Contains("ERR94")) {
                InitELM327Format();
            }
            return strRet;
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

        public string GetVoltage() {
            if (m_CommELM.Online) {
                return m_CommELM.GetResponse("ATRV");
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

        public override int GetComPortIndex() { return m_CommELM.Port; }

        public override StandardType GetStandardType() { return m_iStandard; }

    }
}
