using System;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SH_OBD {
    public abstract class CommBase : IDisposable {
        private SerialPortClass m_serial = null;
        private readonly Logger m_log;
        private bool m_online = false;
        private bool m_auto = false;
        private int m_writeCount = 0;

        public CommBase(Logger log) {
            m_log = log;
        }

        ~CommBase() {
            Dispose();
        }

        public bool Online {
            get {
                if (m_online) {
                    return CheckOnline();
                } else {
                    return false;
                }
            }
        }

        public static string AltName(string s) {
            s.Trim();
            if (s.StartsWith("\\")) {
                return s;
            } else {
                return "\\\\.\\" + s;
            }
        }

        public static bool IsPortAvailable(int iComPort) {
            return (GetPortAvailable(iComPort) > CommBase.PortStatus.Unavailable);
        }

        public static PortStatus GetPortAvailable(int iComPort) {
            string comPort = "COM" + iComPort.ToString();
            SerialPort sp = new SerialPort(comPort);
            try {
                sp.Open();
            } catch (Exception e) {
                if (e is UnauthorizedAccessException uae) {
                    return PortStatus.Absent;
                } else {
                    return PortStatus.Unavailable;
                }
            } finally {
                sp.Close();
            }
            return PortStatus.Available;
        }

        public bool Open() {
            if (m_online) {
                return false;
            }
            CommBase.CommBaseSettings commBaseSettings = CommSettings();
            m_serial = new SerialPortClass(
                commBaseSettings.Port, commBaseSettings.BaudRate,
                (Parity)commBaseSettings.Parity,
                commBaseSettings.DataBits,
                (StopBits)commBaseSettings.StopBits
            ) {
                WriteTimeout = 5000 // 发送超时设为5s
            };
            m_writeCount = 0;
            m_auto = false;
            m_serial.DataReceived += new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
            try {
                if (m_serial.OpenPort()) {
                    m_online = true;
                    if (AfterOpen()) {
                        m_auto = commBaseSettings.AutoReopen;
                        return true;
                    } else {
                        Close();
                        return false;
                    }
                } else {
                    return false;
                }
            } catch (Exception e) {
                m_log.TraceFatal(string.Format("Can't open {0}! Reason: {1}", commBaseSettings.Port, e.Message));
                return false;
            }
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            foreach (byte item in bits) {
                OnRxChar(item);
            }
        }

        public void Close() {
            if (!m_online) {
                return;
            }
            m_auto = false;
            BeforeClose(false);
            InternalClose();
            m_online = false;
        }

        private void InternalClose() {
            m_serial.ClosePort();
        }

        public void Dispose() {
            Close();
        }

        protected void ThrowException(string reason) {
            if (m_online) {
                BeforeClose(true);
                InternalClose();
            }
            throw new CommPortException(reason);
        }

        protected void Send(byte[] tosend) {
            if (CheckOnline()) {
                m_writeCount = tosend.GetLength(0);
                try {
                    m_serial.SendData(tosend, 0, m_writeCount);
                    m_writeCount = 0;
                } catch (Exception ex) {
                    m_log.TraceError("Send failed: " + ex.Message);
                    ThrowException("Send failed: " + ex.Message);
                }
            }
        }

        protected virtual CommBase.CommBaseSettings CommSettings() {
            return new CommBase.CommBaseSettings();
        }

        protected virtual bool AfterOpen() {
            return true;
        }

        protected virtual void BeforeClose(bool error) {
        }

        protected virtual void OnRxChar(byte ch) {
        }

        protected virtual void OnTxDone() {
        }

        protected virtual void OnBreak() {
        }

        protected virtual void OnRxException(Exception e) {
        }

        private bool CheckOnline() {
            if (m_online) {
                return true;
            } else {
                if (m_auto && Open()) {
                    return true;
                }
                m_log.TraceError("CheckOnline: Offline");
                ThrowException("CheckOnline: Offline");
                return false;
            }
        }

        public class CommBaseSettings {
            public string Port = "COM1";
            public int BaudRate = 2400;
            public int Parity = 0;
            public int DataBits = 8;
            public int StopBits = 1;
            public bool AutoReopen = false;
            public bool CheckAllSends = true;

            public void SetStandard(string port, int baudrate) {
                DataBits = 8;
                StopBits = 1;
                Parity = 0;
                Port = port;
                BaudRate = baudrate;
            }
        }

        public enum PortStatus {
            Absent = -1,
            Unavailable = 0,
            Available = 1,
        }

        public enum ASCII : byte {
            NULL = (byte)0,
            SOH = (byte)1,
            STX = (byte)2,
            ETX = (byte)3,
            EOT = (byte)4,
            ENQ = (byte)5,
            ACK = (byte)6,
            BELL = (byte)7,
            BS = (byte)8,
            HT = (byte)9,
            LF = (byte)10,
            VT = (byte)11,
            FF = (byte)12,
            CR = (byte)13,
            SO = (byte)14,
            SI = (byte)15,
            DC1 = (byte)17,
            DC2 = (byte)18,
            DC3 = (byte)19,
            DC4 = (byte)20,
            NAK = (byte)21,
            SYN = (byte)22,
            ETB = (byte)23,
            CAN = (byte)24,
            EM = (byte)25,
            SUB = (byte)26,
            ESC = (byte)27,
            FS = (byte)28,
            GS = (byte)29,
            RS = (byte)30,
            US = (byte)31,
            SP = (byte)32,
            GT = (byte)62,
            DEL = (byte)127,
        }
    }

    public class CommPortException : ApplicationException {
        public CommPortException(string desc) : base(desc) { }

        public CommPortException(Exception e) : base("Receive Thread Exception", e) { }
    }
}
