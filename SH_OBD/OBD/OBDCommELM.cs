﻿using System;

namespace SH_OBD {
    public class OBDCommELM : CommLine {
        protected string m_Port = "COM1";
        protected int m_BaudRate = 38400;
        protected int m_Timeout = 300;
        protected CommBase.ASCII m_asciiRxTerm = (CommBase.ASCII)62;
        protected Logger m_log;

        public OBDCommELM(Logger log) : base(log) {
            m_log = log;
        }

        public void SetPort(int iPort) {
            m_Port = "COM" + iPort.ToString();
            m_log.TraceInfo(string.Format("Port set to {0}", m_Port));
        }

        public void SetBaudRate(int iBaudRate) {
            m_BaudRate = iBaudRate;
            m_log.TraceInfo(string.Format("Baud rate set to {0}", m_BaudRate.ToString()));
        }

        public int GetBaudRate() {
            return m_BaudRate;
        }

        public void SetTimeout(int iTimeout) {
            m_Timeout = iTimeout;
            SetTransTimeout(iTimeout);
            m_log.TraceInfo(string.Format("Timeout set to {0} ms", m_Timeout.ToString()));
        }

        public void SetRxTerminator(CommBase.ASCII ch) {
            m_asciiRxTerm = ch;
        }

        public string GetResponse(string command) {
            string response;
            try {
                m_log.TraceInfo(string.Format("TX: {0}", command));
                response = Transact(command);
                m_log.TraceInfo(string.Format("RX: {0}", response.Replace("\r", @"\r")));
            } catch (Exception ex) {
                m_log.TraceError(ex.Message);
                if (string.Compare(ex.Message, "Timeout") == 0) {
                    Open();
                }
                m_log.TraceError("RX: COMM TIMED OUT!");
                response = "TIMEOUT";
            }
            return response;
        }

        protected override CommBaseSettings CommSettings() {
            CommLine.CommLineSettings settings = new CommLine.CommLineSettings();
            settings.SetStandard(m_Port, m_BaudRate);
            settings.RxTerminator = m_asciiRxTerm;
            settings.RxFilter = new ASCII[] { ASCII.LF, ASCII.SP, ASCII.GT, ASCII.NULL };
            settings.TxTerminator = new ASCII[] { ASCII.CR };
            settings.TransactTimeout = m_Timeout;
            base.Setup(settings);
            return settings;
        }
    }
}