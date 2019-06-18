using System;
using System.Runtime.InteropServices;

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
        public abstract bool Initialize();
        public abstract void Disconnect();
        public abstract bool Connected();
        public abstract OBDResponseList Query(OBDParameter param);
        public abstract string Query(string cmd);
    }
}