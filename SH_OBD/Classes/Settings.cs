using System;
using System.Xml.Serialization;

namespace SH_OBD {
    [Serializable]
    public class Settings {
        private bool m_AutoDetect;
        private int m_BaudRateIndex;
        private int m_ComPortIndex;
        private HardwareType m_HardwareIndex;
        private ProtocolType m_ProtocolIndex;
        private int m_ActiveProfileIndex;
        private bool m_Initialize;

        public Settings() {
            m_AutoDetect = true;
            m_ComPortIndex = 1;
            m_ActiveProfileIndex = 0;
            m_BaudRateIndex = 0;
            m_HardwareIndex = HardwareType.Automatic;
            m_ProtocolIndex = ProtocolType.Automatic;
            m_Initialize = true;
        }

        public int BaudRate {
            get {
                switch (m_BaudRateIndex) {
                    case 0: return 9600;
                    case 1: return 38400;
                    case 2: return 115200;
                    default: return 9600;
                }
            }
        }

        public string ComPortName {
            get { return "COM" + Convert.ToString(m_ComPortIndex); }
        }

        public bool DoInitialization {
            get { return m_Initialize; }
            set { m_Initialize = value; }
        }

        public int ActiveProfileIndex {
            get { return m_ActiveProfileIndex; }
            set { m_ActiveProfileIndex = value; }
        }

        public int BaudRateIndex {
            get { return m_BaudRateIndex; }
            set { m_BaudRateIndex = value; }
        }

        [XmlIgnore]
        public static string[] ProtocolNames = new string[]
        {
            "自动",
            "SAE J1850 PWM (41.6K 波特率)",
            "SAE J1850 VPW (10.4K 波特率)",
            "ISO 9141-2 (5 波特率初始化, 10.4K 波特率)",
            "ISO 14230-4 KWP (5 波特率初始化, 10.4K 波特率)",
            "ISO 14230-4 KWP (快速初始化, 10.4K 波特率)",
            "ISO 15765-4 CAN (11 位 CAN ID, 500K 波特率)",
            "ISO 15765-4 CAN (29 位 CAN ID, 500K 波特率)",
            "ISO 15765-4 CAN (11 位 CAN ID, 250K 波特率)",
            "ISO 15765-4 CAN (29 位 CAN ID, 250K 波特率)"
        };

        [XmlIgnore]
        public string ProtocolName {
            get {
                if (ProtocolIndexInt >= 0 && ProtocolIndexInt < ProtocolNames.Length) {
                    return ProtocolNames[ProtocolIndexInt];
                }
                return "Unknown";
            }
        }

        [XmlIgnore]
        public ProtocolType ProtocolIndex {
            get { return m_ProtocolIndex; }
            set { m_ProtocolIndex = value; }
        }
        [XmlElement("ProtocolIndex")]
        public int ProtocolIndexInt {
            get { return (int)m_ProtocolIndex; }
            set { m_ProtocolIndex = (ProtocolType)value; }
        }

        [XmlIgnore]
        public HardwareType HardwareIndex {
            get { return m_HardwareIndex; }
            set { m_HardwareIndex = value; }
        }
        [XmlElement("HardwareIndex")]
        public int HardwareIndexInt {
            get { return (int)m_HardwareIndex; }
            set { m_HardwareIndex = (HardwareType)value; }
        }

        public int ComPort {
            get { return m_ComPortIndex; }
            set { m_ComPortIndex = value; }
        }

        public bool AutoDetect {
            get { return m_AutoDetect; }
            set { m_AutoDetect = value; }
        }
    }
    public enum ProtocolType : int {
        Unknown = -1,
        Automatic = 0,
        J1850_PWM = 1,
        J1850_VPW = 2,
        ISO9141_2 = 3,
        ISO_14230_4_KWP_5BAUDINIT = 4,
        ISO_14230_4_KWP_FASTINIT = 5,
        ISO_15765_4_CAN_11BIT_500KBAUD = 6,
        ISO_15765_4_CAN_29BIT_500KBAUD = 7,
        ISO_15765_4_CAN_11BIT_250KBAUD = 8,
        ISO_15765_4_CAN_29BIT_250KBAUD = 9
    }

    public enum HardwareType : int {
        Automatic = 0,
        ELM327 = 1,
        ELM320 = 2,
        ELM322 = 3,
        ELM323 = 4,
        CANtact = 5
    }
}
