using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SH_OBD {
    public abstract class OBDDevice {
        protected string m_DeviceDes;
        protected string m_DeviceID;
        protected Logger m_log;
        protected OBDParser m_Parser;
        protected OBDCommELM m_CommELM;

        public OBDDevice(Logger log) {
            m_log = log;
            m_CommELM = new OBDCommELM(log);
        }

        public string DeviceDesString() {
            return m_DeviceDes;
        }

        public string DeviceIDString() {
            return m_DeviceID;
        }

        public abstract bool Initialize(int iPort, int iBaud, ProtocolType iProtocol);
        public abstract bool Initialize(int iPort, int iBaud);
        public abstract bool Initialize(Settings settings);
        public abstract void Disconnect();
        public abstract bool GetConnected();
        public abstract void SetConnected(bool status);
        public abstract OBDResponseList Query(OBDParameter param);
        public abstract string Query(string cmd);
        public abstract ProtocolType GetProtocolType();
        public abstract int GetComPortIndex();
        public abstract int GetBaudRateIndex();
        public abstract void SetTimeout(int iTimeout);
        public abstract StandardType GetStandardType();
    }
}