using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SH_OBD {
    public class OBDInterface {
        private const string m_vehicles_db = ".\\Configs\\vehicles.db";
        private const string m_settings_xml = ".\\Configs\\settings.xml";
        private const string m_userprefs_xml = ".\\Configs\\userprefs.xml";
        private const string m_dbandMES_xml = ".\\Configs\\dbandMES.xml";

        public delegate void __Delegate_OnConnect();
        public delegate void __Delegate_OnDisconnect();
        public event __Delegate_OnDisconnect OnDisconnect;
        public event __Delegate_OnConnect OnConnect;

        private OBDDevice m_obdDevice;
        private readonly OBDInterpreter m_obdInterpreter;
        private List<DTC> m_listDTC;
        private readonly List<OBDParameter> m_listAllParameters;
        private readonly List<OBDParameter> m_listSupportedParameters;
        public readonly SerialPortClass m_sp;
        public readonly Logger m_log;

        public UserPreferences UserPreferences { get; private set; }
        public Settings CommSettings { get; private set; }
        public DBandMES DBandMES { get; private set; }
        public List<VehicleProfile> VehicleProfiles { get; private set; }
        public bool UseISO27145 { get; set; }
        public bool ScannerPortOpened { get; set; }

        public OBDInterface() {
            m_log = new Logger("./log", EnumLogLevel.LogLevelAll, true, 100);
            m_log.TraceInfo("==================================================================");
            m_log.TraceInfo("==================== START Ver: " + MainFileVersion.AssemblyVersion + " ====================");
            m_listAllParameters = new List<OBDParameter>();
            m_listSupportedParameters = new List<OBDParameter>();
            m_obdInterpreter = new OBDInterpreter();
            UserPreferences = LoadUserPreferences();
            CommSettings = LoadCommSettings();
            DBandMES = LoadDBandMES();
            VehicleProfiles = LoadVehicleProfiles();
            SetDevice(HardwareType.ELM327);
            UseISO27145 = false;
            ScannerPortOpened = false;
            if (CommSettings.UseSerialScanner) {
                m_sp = new SerialPortClass(
                    CommSettings.ScannerPortName,
                    CommSettings.ScannerBaudRate,
                    Parity.None,
                    8,
                    StopBits.One
                );
                try {
                    m_sp.OpenPort();
                    ScannerPortOpened = true;
                } catch (Exception ex) {
                    m_log.TraceError("打开扫码枪串口出错: " + ex.Message);
                }
            }
        }

        ~OBDInterface() {
            if (m_sp != null) {
                m_sp.ClosePort();
            }
        }

        public Logger GetLogger() { return m_log; }

        public bool ConnectedStatus {
            get { return m_obdDevice.GetConnected(); }
        }

        public HardwareType GetDevice() {
            return CommSettings.HardwareIndex;
        }

        public string GetDeviceDesString() {
            return m_obdDevice.DeviceDesString();
        }

        public string GetDeviceIDString() {
            return m_obdDevice.DeviceIDString();
        }

        public ProtocolType GetProtocol() {
            return CommSettings.ProtocolIndex;
        }

        public bool InitDevice(HardwareType device, int port, int baud, ProtocolType protocol) {
            m_log.TraceInfo(string.Format("Attempting initialization on port {0}", port.ToString()));

            SetDevice(device);
            if (m_obdDevice.Initialize(port, baud, protocol) && InitOBD()) {
                m_obdDevice.SetConnected(true);
                OnConnect?.Invoke();
                return true;
            }
            m_obdDevice.SetConnected(false);
            return false;
        }

        public bool InitDeviceAuto() {
            m_log.TraceInfo("Beginning AUTO initialization...");
            SetDevice(HardwareType.ELM327);
            if (m_obdDevice.Initialize(CommSettings) && InitOBD()) {
                CommSettings.ProtocolIndex = m_obdDevice.GetProtocolType();
                CommSettings.ComPort = m_obdDevice.GetComPortIndex();
                SaveCommSettings(CommSettings);
                m_obdDevice.SetConnected(true);
                OnConnect?.Invoke();
                return true;
            }
            m_obdDevice.SetConnected(false);
            return false;
        }

        public bool InitOBD() {
            bool bRet = true;
            // 获取ISO15031 Mode01 PID支持情况
            OBDParameter param = new OBDParameter(1, 0, 0) {
                ValueTypes = 32
            };
            m_listSupportedParameters.Clear();

            for (int i = 0; (i * 0x20) < 0x100; i++) {
                param.Parameter = i * 0x20;
                OBDParameterValue value = GetValue(param);
                if (value.ErrorDetected) {
                    bRet = false;
                    break;
                }
                foreach (OBDParameter param2 in m_listAllParameters) {
                    if (param2.Parameter > 2 && param2.Parameter > (i * 0x20) && param2.Parameter < ((i + 1) * 0x20) && value.GetBitFlag(param2.Parameter - param.Parameter - 1)) {
                        m_listSupportedParameters.Add(param2);
                    }
                }
                if (!value.GetBitFlag(31)) {
                    break;
                }
            }

            if (!bRet) {
                // 获取ISO27145 PID支持情况
                bRet = true;
                param = new OBDParameter(0x22, 0, 0) {
                    ValueTypes = 32
                };
                m_listSupportedParameters.Clear();

                for (int i = 0; (i * 0x20) < 0x100; i++) {
                    param.Parameter = 0xF400 + i * 0x20;
                    OBDParameterValue value = GetValue(param);
                    if (value.ErrorDetected) {
                        bRet = false;
                        break;
                    }
                    foreach (OBDParameter param2 in m_listAllParameters) {
                        if (param2.Parameter > 2 && param2.Parameter > (i * 0x20) && param2.Parameter < ((i + 1) * 0x20) && value.GetBitFlag(param2.Parameter - param.Parameter - 1)) {
                            m_listSupportedParameters.Add(param2);
                        }
                    }
                    if (!value.GetBitFlag(31)) {
                        break;
                    }
                }
                UseISO27145 = bRet;
                m_log.TraceInfo("Current vehicle support ISO 27145 only!");
            }
            return bRet;
        }

        public bool IsParameterSupported(string strPID) {
            foreach (OBDParameter param in m_listSupportedParameters) {
                if (param.PID.CompareTo(strPID) == 0) {
                    return true;
                }
            }
            return false;
        }

        public OBDParameterValue GetValue(string strPID, bool bEnglishUnits = false) {
            OBDParameter obdParameter = LookupParameter(strPID);
            if (obdParameter != null) {
                return GetValue(obdParameter, bEnglishUnits);
            }

            OBDParameterValue value = new OBDParameterValue {
                ErrorDetected = true
            };
            return value;
        }

        public OBDParameterValue GetValue(OBDParameter param, bool bEnglishUnits = false) {
            if (param.PID.Length > 0) {
                m_log.TraceInfo("Requesting: " + param.PID);
            } else {
                m_log.TraceInfo("Requesting: " + param.OBDRequest);
            }
            if (param.Service == 0) {
                return SpecialValue(param);
            }

            OBDResponseList responses = m_obdDevice.Query(param);
            string strItem = "Responses: ";
            if (responses.ErrorDetected) {
                strItem += "Error Detected!";
                m_log.TraceInfo(strItem);
                return new OBDParameterValue { ErrorDetected = true };
            } else {
                for (int i = 0; i < responses.ResponseCount; i++) {
                    strItem += string.Format("[{0}] ", responses.GetOBDResponse(i).Data);
                }
            }
            m_log.TraceInfo(strItem);
            OBDParameterValue obdValue = m_obdInterpreter.GetValue(param, responses, bEnglishUnits);
            if (obdValue.ErrorDetected) {
                m_log.TraceError("Error Detected in OBDParameterValue!");
            } else {
                m_log.TraceInfo(GetLogString(param, obdValue));
            }
            return obdValue;
        }

        public List<OBDParameterValue> GetValueList(OBDParameter param, bool bEnglishUnits = false) {
            List<OBDParameterValue> ValueList = new List<OBDParameterValue>();

            if (param.PID.Length > 0) {
                m_log.TraceInfo("Requesting: " + param.PID);
            } else {
                m_log.TraceInfo("Requesting: " + param.OBDRequest);
            }
            OBDResponseList responses = m_obdDevice.Query(param);
            string strItem = "Responses: ";
            if (responses.ErrorDetected) {
                strItem += "Error Detected!";
                OBDParameterValue value = new OBDParameterValue {
                    ErrorDetected = true,
                    StringValue = "Error Detected in OBDResponseList!",
                    ShortStringValue = "ERROR_RESP"
                };
                ValueList.Add(value);
                m_log.TraceInfo(strItem);
                return ValueList;
            } else {
                for (int i = 0; i < responses.ResponseCount; i++) {
                    strItem += string.Format("[{0}] ", responses.GetOBDResponse(i).Data);
                }
                strItem = strItem.TrimEnd();
                m_log.TraceInfo(strItem);
            }

            for (int i = 0; i < responses.ResponseCount; i++) {
                OBDParameterValue obdValue = m_obdInterpreter.GetValue(param, responses.GetOBDResponse(i), bEnglishUnits);
                if (obdValue.ErrorDetected) {
                    m_log.TraceError("Error Detected in OBDParameterValue!");
                } else {
                    m_log.TraceInfo(GetLogString(param, obdValue));
                }
                ValueList.Add(obdValue);
            }
            return ValueList;
        }

        private string GetLogString(OBDParameter param, OBDParameterValue obdValue) {
            string strRet = "Values: ";
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.Double) == (int)OBDParameter.EnumValueTypes.Double) {
                strRet += string.Format("[Double: {0}] ", obdValue.DoubleValue.ToString());
            }
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.Bool) == (int)OBDParameter.EnumValueTypes.Bool) {
                strRet += string.Format("[Bool: {0}] ", obdValue.BoolValue.ToString());
            }
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.String) == (int)OBDParameter.EnumValueTypes.String) {
                strRet += string.Format("[String: {0}] ", obdValue.StringValue, obdValue.ShortStringValue);
            }
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.ListString) == (int)OBDParameter.EnumValueTypes.ListString) {
                strRet += "[ListString: ";
                foreach (string strx in obdValue.ListStringValue) {
                    strRet = string.Concat(strRet, strx + ", ");
                }
                strRet = strRet.Substring(0, strRet.Length - 2);
                strRet += "]";
            }
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.ShortString) == (int)OBDParameter.EnumValueTypes.ShortString) {
                strRet += string.Format("[ShortString: {0}] ", obdValue.ShortStringValue);
            }
            if ((param.ValueTypes & (int)OBDParameter.EnumValueTypes.BitFlags) == (int)OBDParameter.EnumValueTypes.BitFlags) {
                strRet += "[BitFlags: ";
                for (int idx = 0; idx < 32; idx++) {
                    strRet += (obdValue.GetBitFlag(idx) ? "1" : "0");
                }
                strRet += "]";
            }
            return strRet;
        }

        public OBDParameterValue SpecialValue(OBDParameter param) {
            if (param.Parameter != 0) {
                return null;
            }

            OBDParameterValue value = new OBDParameterValue();
            string respopnse = GetRawResponse("ATRV");
            m_log.TraceInfo("Response of \"ATRV\": " + respopnse);
            if (respopnse != null) {
                respopnse = respopnse.Replace("V", "");
                value.DoubleValue = Utility.Text2Double(respopnse);
            }
            return value;
        }

        public string GetRawResponse(string strCmd) {
            return m_obdDevice.Query(strCmd);
        }

        public OBDResponseList GetResponseList(OBDParameter param) {
            OBDResponseList responses = m_obdDevice.Query(param);
            string strItem = "Responses: ";
            for (int i = 0; i < responses.ResponseCount; i++) {
                strItem += string.Format("[{0}] ", responses.GetOBDResponse(i).Data);
            }
            m_log.TraceInfo(strItem);
            return responses;
        }

        public bool ClearCodes() {
            return (m_obdDevice.Query("04").IndexOf("44") >= 0);
        }

        public void Disconnect() {
            m_obdDevice.Disconnect();
            m_obdDevice.SetConnected(false);
            UseISO27145 = false;
            OnDisconnect?.Invoke();
        }

        public bool LoadParameters(string fileName) {
            int lineNo = 0;
            string line;
            OBDParameter param;
            string[] tokens;
            char[] comma = new char[] { ',' };

            try {
                using (StreamReader streamReader = new StreamReader(fileName)) {
                    while ((line = streamReader.ReadLine()) != null) {
                        ++lineNo;
                        line = line.Trim();
                        // Ignore empty and comment lines
                        if (line.Length == 0 || line[0] == '#') {
                            continue;
                        }

                        tokens = line.Split(comma);
                        for (int idx = 0; idx < tokens.Length; idx++) {
                            tokens[idx] = (tokens[idx] ?? "").Trim();
                        }

                        param = new OBDParameter {
                            PID = tokens[0],
                            Name = tokens[1],
                            OBDRequest = tokens[2],
                            Service = int.Parse(tokens[3]),
                            Parameter = int.Parse(tokens[4]),
                            SubParameter = int.Parse(tokens[5])
                        };

                        switch (tokens[6]) {
                        case "Airflow":
                            param.Category = 0; break;
                        case "DTC":
                            param.Category = 1; break;
                        case "Emissions":
                            param.Category = 2; break;
                        case "Fuel":
                            param.Category = 3; break;
                        case "General":
                            param.Category = 4; break;
                        case "O2":
                            param.Category = 5; break;
                        case "Powertrain":
                            param.Category = 6; break;
                        case "Speed":
                            param.Category = 7; break;
                        case "Temperature":
                            param.Category = 8; break;
                        }
                        switch (tokens[7]) {
                        case "Generic":
                            param.Type = 0; break;
                        case "Manufacturer":
                            param.Type = 1; break;
                        case "Scripted":
                            param.Type = 2; break;
                        }

                        switch (tokens[8]) {
                        case "SAE":
                            param.Manufacturer = 0; break;
                        case "GM":
                            param.Manufacturer = 1; break;
                        case "Ford":
                            param.Manufacturer = 2; break;
                        case "SH_OBD":
                            param.Manufacturer = 3; break;
                        }

                        param.Priority = int.Parse(tokens[9]);
                        param.EnglishUnitLabel = tokens[10];
                        param.MetricUnitLabel = tokens[11];

                        try {
                            param.EnglishMinValue = Utility.Text2Double(tokens[12]);
                            param.EnglishMaxValue = Utility.Text2Double(tokens[13]);
                            param.MetricMinValue = Utility.Text2Double(tokens[14]);
                            param.MetricMaxValue = Utility.Text2Double(tokens[15]);
                        } catch (Exception e) {
                            m_log.TraceError("Utility.Text2Double() occur error: " + e.Message);
                        }

                        int valueType = 0x00;
                        if (int.Parse(tokens[16]) > 0) {
                            valueType = 0x01;
                        }
                        if (int.Parse(tokens[17]) > 0) {
                            valueType |= 0x02;
                        }
                        if (int.Parse(tokens[18]) > 0) {
                            valueType |= 0x04;
                        }
                        if (int.Parse(tokens[19]) > 0) {
                            valueType |= 0x08;
                        }

                        param.ValueTypes = valueType;
                        m_listAllParameters.Add(param);
                    }
                }
                m_log.TraceInfo(string.Format("Loaded {0} parameters from {1}", lineNo, fileName));
                return true;
            } catch (Exception e) {
                m_log.TraceError(string.Format("Failed to load parameters from: {0}, reason: {1}", fileName, e.Message));
                return false;
            }
        }

        public DTC GetDTC(string code) {
            foreach (DTC dtc in m_listDTC) {
                if (dtc.Name.CompareTo(code) == 0) {
                    return dtc;
                }
            }
            return new DTC(code, "", "");
        }

        public OBDParameter LookupParameter(string pid) {
            foreach (OBDParameter param in m_listAllParameters) {
                if (param.PID.CompareTo(pid) == 0) {
                    return param;
                }
            }
            return null;
        }

        public List<OBDParameter> SupportedParameterList(int valueTypes) {
            List<OBDParameter> list = new List<OBDParameter>(m_listSupportedParameters.Count);
            foreach (OBDParameter param in m_listSupportedParameters) {
                if ((param.ValueTypes & valueTypes) == valueTypes) {
                    list.Add(param);
                }
            }
            return list;
        }

        private void SetDevice(HardwareType device) {
            CommSettings.HardwareIndex = device;
            switch (device) {
            case HardwareType.ELM327:
                m_log.TraceInfo("Set device to ELM327");
                m_obdDevice = new OBDDeviceELM327(m_log);
                break;
            case HardwareType.ELM320:
                m_log.TraceInfo("Set device to ELM320");
                m_obdDevice = new OBDDeviceELM320(m_log);
                break;
            case HardwareType.ELM322:
                m_log.TraceInfo("Set device to ELM322");
                m_obdDevice = new OBDDeviceELM322(m_log);
                break;
            case HardwareType.ELM323:
                m_log.TraceInfo("Set device to ELM323");
                m_obdDevice = new OBDDeviceELM323(m_log);
                break;
            default:
                m_log.TraceInfo("Set device to ELM327");
                m_obdDevice = new OBDDeviceELM327(m_log);
                break;
            }
        }

        public int LoadDTCDefinitions(string fileName) {
            try {
                if (File.Exists(fileName)) {
                    Type[] extraTypes = new Type[] { typeof(DTC) };
                    m_listDTC = new XmlSerializer(typeof(List<DTC>), extraTypes).Deserialize(new FileStream(fileName, FileMode.Open)) as List<DTC>;
                    return m_listDTC.Count;
                } else {
                    m_log.TraceError("Failed to locate DTC definitions file: " + fileName);
                    return 0;
                }
            } catch (Exception e) {
                m_log.TraceError("Failed to load parameters from: " + fileName + ", reason: " + e.Message);
                return -1;
            }
        }

        public void SaveDBandMES(DBandMES dBandMES) {
            this.DBandMES = dBandMES;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DBandMES));
            using (TextWriter writer = new StreamWriter(m_dbandMES_xml)) {
                xmlSerializer.Serialize(writer, this.DBandMES);
                writer.Close();
            }
        }


        public void SaveCommSettings(Settings settings) {
            CommSettings = settings;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(m_settings_xml)) {
                xmlSerializer.Serialize(writer, CommSettings);
                writer.Close();
            }
        }

        public void SaveUserPreferences(UserPreferences prefs) {
            UserPreferences = prefs;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserPreferences));
            using (TextWriter writer = (TextWriter)new StreamWriter(m_userprefs_xml)) {
                xmlSerializer.Serialize(writer, UserPreferences);
                writer.Close();
            }
        }

        public DBandMES LoadDBandMES() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(DBandMES));
                using (FileStream reader = new FileStream(m_dbandMES_xml, FileMode.Open)) {
                    DBandMES = (DBandMES)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception e) {
                m_log.TraceError("Using default DB and MES settings because of failed to load them, reason: " + e.Message);
                DBandMES = new DBandMES();
            }
            return DBandMES;
        }

        public Settings LoadCommSettings() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream reader = new FileStream(m_settings_xml, FileMode.Open)) {
                    CommSettings = (Settings)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception e) {
                m_log.TraceError("Using default communication settings because of failed to load them, reason: " + e.Message);
                CommSettings = new Settings();
            }
            return CommSettings;
        }

        public UserPreferences LoadUserPreferences() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(UserPreferences));
                using (FileStream reader = new FileStream(m_userprefs_xml, FileMode.Open)) {
                    UserPreferences = (UserPreferences)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception e) {
                m_log.TraceError("Using default user preferences because of failed to load them, reason: " + e.Message);
                UserPreferences = new UserPreferences();
            }
            return UserPreferences;
        }

        public List<VehicleProfile> LoadVehicleProfiles() {
            FileStream file = null;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            List<VehicleProfile> profiles = new List<VehicleProfile>();
            try {
                if (File.Exists(m_vehicles_db)) {
                    file = new FileStream(m_vehicles_db, FileMode.Open, FileAccess.Read);
                } else {
                    VehicleProfile profile = new VehicleProfile();
                    file = new FileStream(m_vehicles_db, FileMode.Create, FileAccess.ReadWrite);
                    binaryFormatter.Serialize(file, profile);
                }
                file.Position = 0L;
                while (true) {
                    VehicleProfile vehicleProfile = binaryFormatter.Deserialize(file) as VehicleProfile;
                    profiles.Add(vehicleProfile);
                }
            } catch (SerializationException) {
                // file读完以后会抛出SerializationException异常，什么都不做继续执行finally块
            } catch (Exception e) {
                // 发生其余异常的话就写log
                m_log.TraceError("Failed to load vehicle profile. Reason: " + e.Message);
            } finally {
                if (file != null) {
                    file.Close();
                }
            }
            return profiles;
        }

        public VehicleProfile ActiveProfile {
            get {
                try {
                    if (CommSettings.ActiveProfileIndex >= 0 && CommSettings.ActiveProfileIndex < VehicleProfiles.Count) {
                        return VehicleProfiles[CommSettings.ActiveProfileIndex];
                    }
                    if (VehicleProfiles.Count == 0) {
                        VehicleProfiles.Add(new VehicleProfile());
                    }
                    CommSettings.ActiveProfileIndex = 0;

                    return VehicleProfiles[0];
                } catch (Exception e) {
                    m_log.TraceError("Failed to get vehicle profile, reason: " + e.Message);
                    return null;
                }
            }
        }

        public void SaveActiveProfile(VehicleProfile profile) {
            Settings settings = CommSettings;
            settings.ActiveProfileIndex = VehicleProfiles.IndexOf(profile);
            SaveCommSettings(settings);
        }

        public void SaveVehicleProfiles(List<VehicleProfile> profiles) {
            FileStream file = null;
            VehicleProfiles = profiles;
            try {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                if (File.Exists(m_vehicles_db)) {
                    File.Delete(m_vehicles_db);
                }
                file = new FileStream(m_vehicles_db, FileMode.Create, FileAccess.ReadWrite);
                foreach (VehicleProfile profile in profiles) {
                    binaryFormatter.Serialize(file, profile);
                }
            } catch (Exception e) {
                m_log.TraceError("Failed to save vehicle profile, reason: " + e.Message);
            } finally {
                if (file != null) {
                    file.Close();
                }
            }
        }
    }
}