using System;

namespace SH_OBD {
    public class OBDCommELM : CommLine {
        protected string m_Port = "COM1";
        protected int m_BaudRate = 38400;
        protected int m_Timeout = 300;
        protected CommBase.ASCII m_asciiRxTerm = (CommBase.ASCII)62;
        protected CommBase.ASCII[] m_RxFilterWithSpace;
        protected CommBase.ASCII[] m_RxFilterNoSpace;
        protected Logger m_log;

        public OBDCommELM(Logger log) : base(log) {
            m_log = log;
            m_RxFilterWithSpace = new ASCII[] { ASCII.LF, ASCII.SP, ASCII.GT, ASCII.NULL };
            m_RxFilterNoSpace = new ASCII[] { ASCII.LF, ASCII.GT, ASCII.NULL };
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
            bool bRxFilterNoSpace = false;
            if (command.Contains("AT")) {
                // 如果是发送AT命令的话，返回值就不过滤空格
                SetRxFilter(m_RxFilterNoSpace);
                bRxFilterNoSpace = true;
            }
            try {
                m_log.TraceInfo(string.Format("TX: {0}", command));
                response = Transact(command);
                m_log.TraceInfo(string.Format("RX: {0}", response.Replace("\r", @"\r").Replace("\n", @"\n")));
            } catch (Exception ex) {
                m_log.TraceError(ex.Message);
                if (string.Compare(ex.Message, "Timeout") == 0) {
                    Open();
                }
                m_log.TraceError("RX: COMM TIMED OUT!");
                response = "TIMEOUT";
            } finally {
                if (bRxFilterNoSpace) {
                    // 将返回值重设为过滤空格
                    SetRxFilter(m_RxFilterWithSpace);
                }
            }
            return response;
        }

        protected override CommBaseSettings CommSettings() {
            CommLine.CommLineSettings settings = new CommLine.CommLineSettings();
            settings.SetStandard(m_Port, m_BaudRate);
            settings.RxTerminator = m_asciiRxTerm;
            settings.RxFilter = m_RxFilterWithSpace;
            settings.TxTerminator = new ASCII[] { ASCII.CR };
            settings.TransactTimeout = m_Timeout;
            base.Setup(settings);
            return settings;
        }
    }
}