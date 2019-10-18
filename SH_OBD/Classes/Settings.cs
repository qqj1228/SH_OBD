using System;
using System.Xml.Serialization;

namespace SH_OBD {
    [Serializable]
    public class Settings {
        public bool DoInitialization { get; set; }
        public int ActiveProfileIndex { get; set; }
        public int BaudRateIndex { get; set; }
        public int ScannerBaudRateIndex { get; set; }
        public int ComPort { get; set; }
        public int ScannerPort { get; set; }
        public bool AutoDetect { get; set; }
        public bool UseSerialScanner { get; set; }

        public Settings() {
            AutoDetect = true;
            ComPort = 1;
            ScannerPort = 2;
            ActiveProfileIndex = 0;
            BaudRateIndex = 0;
            ScannerBaudRateIndex = 0;
            HardwareIndex = HardwareType.Automatic;
            ProtocolIndex = ProtocolType.Automatic;
            DoInitialization = true;
            UseSerialScanner = true;
        }

        public int BaudRate {
            get {
                switch (BaudRateIndex) {
                    case 0: return 9600;
                    case 1: return 38400;
                    case 2: return 115200;
                    default: return 9600;
                }
            }
        }

        public int ScannerBaudRate {
            get {
                switch (ScannerBaudRateIndex) {
                case 0: return 9600;
                case 1: return 38400;
                case 2: return 115200;
                default: return 9600;
                }
            }
        }

        public string ComPortName {
            get { return "COM" + Convert.ToString(ComPort); }
        }

        public string ScannerPortName {
            get { return "COM" + Convert.ToString(ScannerPort); }
        }

        [XmlIgnore]
        public static string[] ProtocolNames = new string[] {
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
        public ProtocolType ProtocolIndex { get; set; }

        [XmlElement("ProtocolIndex")]
        public int ProtocolIndexInt {
            get { return (int)ProtocolIndex; }
            set { ProtocolIndex = (ProtocolType)value; }
        }

        [XmlIgnore]
        public HardwareType HardwareIndex { get; set; }

        [XmlElement("HardwareIndex")]
        public int HardwareIndexInt {
            get { return (int)HardwareIndex; }
            set { HardwareIndex = (HardwareType)value; }
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

    [Serializable]
    public class UserPreferences {
        public string Telephone { get; set; }
        public string Address2 { get; set; }
        public string Address1 { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class VehicleProfile {
        public string Name;
        public bool AutoTransmission;
        public float FirstGearRatio;
        public float SecondGearRatio;
        public float ThirdGearRatio;
        public float FourthGearRatio;
        public float FifthGearRatio;
        public float SixthGearRatio;
        public float DynoDriveRatio;
        public float AxleGearRatio;
        public float Weight;
        public float DragCoefficient;
        public int ElmTimeout;
        public float SpeedCalibrationFactor;
        public WheelStruc Wheel;
        public string Notes;

        public VehicleProfile() {
            Name = "New Vehicle";
            AutoTransmission = false;
            FirstGearRatio = 2.66f;
            SecondGearRatio = 1.78f;
            ThirdGearRatio = 1.3f;
            FourthGearRatio = 1f;
            FifthGearRatio = 0.74f;
            SixthGearRatio = 0.5f;
            AxleGearRatio = 3.73f;
            DynoDriveRatio = 0.022f;
            SpeedCalibrationFactor = 1f;
            Weight = 3500f;
            DragCoefficient = 0.13f;
            Wheel = new WheelStruc {
                Width = 255,
                AspectRatio = 40,
                RimDiameter = 16
            };
            ElmTimeout = 200;
            Notes = "";
        }

        public override string ToString() {
            return Name;
        }

        public bool Equals(VehicleProfile p) {
            return (
                Name.Equals(p.Name) &&
                AutoTransmission == p.AutoTransmission &&
                Weight == p.Weight &&
                DynoDriveRatio == p.DynoDriveRatio &&
                DragCoefficient == p.DragCoefficient &&
                Wheel.Width == p.Wheel.Width &&
                Wheel.AspectRatio == p.Wheel.AspectRatio &&
                Wheel.RimDiameter == p.Wheel.RimDiameter &&
                ElmTimeout == p.ElmTimeout && Notes.Equals(p.Notes)
            );
        }
    }

    [Serializable]
    public class WheelStruc {
        public int Width;
        public int AspectRatio;
        public int RimDiameter;
    }

    [Serializable]
    public class DBandMES {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DBName { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string WebServiceAddress { get; set; }
        public string WebServiceName { get; set; }
        public string WebServiceMethods { get; set; }
        public string WebServiceWSDL { get; set; }
        public bool UseURL { get; set; }
        [XmlIgnore]
        public bool ChangeWebService { get; set; }

        public DBandMES() {
            UserName = "sa";
            PassWord = "sh49";
            DBName = "SH_OBD";
            IP = "127.0.0.1";
            Port = "1433";
            WebServiceAddress = "http://193.28.6.4:1908/";
            WebServiceName = "Wes_DeviceTestData_MES";
            WebServiceMethods = "WriteDataToMes";
            WebServiceWSDL = "";
            UseURL = true;
            ChangeWebService = true;
        }

        public string[] GetMethodArray() {
            return WebServiceMethods.Split(',');
        }
    }

    [Serializable]
    public class OBDResultSetting {
        public bool UploadWhenever { get; set; }
        public bool UseECUAcronym { get; set; }
        public bool UseSCRName { get; set; }
        public bool DTC03 { get; set; }
        public bool DTC07 { get; set; }
        public bool DTC0A { get; set; }
        public bool Readiness { get; set; }
        public bool VINError { get; set; }
        public int UploadTime { get; set; }

        public OBDResultSetting() {
            UploadWhenever = false;
            UseECUAcronym = true;
            UseSCRName = true;
            DTC03 = true;
            DTC07 = true;
            DTC0A = false;
            Readiness = false;
            VINError = true;
            UploadTime = 20;
        }
    }

}
