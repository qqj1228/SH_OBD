using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;

namespace SH_OBD {
    public class OBDCommELM : CommLine {
        protected string m_Port = "COM1";
        public int Port {
            get {
                return int.Parse(m_Port.Substring(3));
            }
            set {
                m_Port = "COM" + value.ToString();
                m_log.TraceInfo(string.Format("Port set to {0}", m_Port));
            }
        }

        protected int m_BaudRate = 38400;
        public int BaudRate {
            get { return m_BaudRate; }
            set {
                m_BaudRate = value;
                m_log.TraceInfo(string.Format("Baud rate set to {0}", m_BaudRate.ToString()));
            }
        }

        public int Timeout {
            get { return TransTimeout; }
            set {
                TransTimeout = value;
                m_log.TraceInfo(string.Format("Timeout set to {0} ms", TransTimeout.ToString()));
            }
        }

        protected byte m_asciiRxTerm = (byte)'>';
        public byte RxTerminator {
            get { return m_asciiRxTerm; }
            set { m_asciiRxTerm = value; }
        }

        protected byte[] m_RxFilterWithSpace;
        protected byte[] m_RxFilterNoSpace;

        public OBDCommELM(Logger log) : base(log) {
            m_RxFilterWithSpace = new byte[] { 0x0A, 0x20, 0 };
            m_RxFilterNoSpace = new byte[] { 0x0A, 0 };
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
                m_log.TraceError("Transact() occur exception: " + ex.Message);
                if (string.Compare(ex.Message, "Timeout") == 0) {
                    Open();
                    m_log.TraceError("RX: COMM TIMED OUT!");
                    response = "TIMEOUT";
                }
                response = ex.Message;
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
            settings.TxTerminator = new byte[] { 0x0D };
            base.Setup(settings);
            return settings;
        }
    }
}