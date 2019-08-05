﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SH_OBD {
    public class OBDInterface {
        private const string m_vehicles_db = ".\\configs\\vehicles.db";
        private const string m_settings_xml = ".\\configs\\settings.xml";
        private const string m_userprefs_xml = ".\\configs\\userprefs.xml";

        public delegate void __Delegate_OnConnect();
        public delegate void __Delegate_OnDisconnect();

        private OBDDevice m_obdDevice;
        private List<DTC> m_listDTC;
        private List<OBDParameter> m_listAllParameters;
        private List<OBDParameter> m_listSupportedParameters;
        private Logger m_log;
        private UserPreferences m_userpreferences;
        private Settings m_settings;
        private List<VehicleProfile> m_VehicleProfiles;
        public bool ConnectedStatus {
            get { return m_obdDevice.GetConnected(); }
        }

        public event OBDInterface.__Delegate_OnDisconnect OnDisconnect;
        public event OBDInterface.__Delegate_OnConnect OnConnect;

        public OBDInterface() {
            m_log = new Logger("./log", EnumLogLevel.LogLevelAll, true, 100);
            m_log.TraceInfo("==================== START Ver: " + MainFileVersion.AssemblyVersion + " ====================");
            m_listAllParameters = new List<OBDParameter>();
            m_listSupportedParameters = new List<OBDParameter>();
            m_userpreferences = LoadUserPreferences();
            m_settings = LoadCommSettings();
            m_VehicleProfiles = LoadVehicleProfiles();
            SetDevice(HardwareType.ELM327);
        }

        public HardwareType GetDevice() {
            return m_settings.HardwareIndex;
        }

        public string GetDeviceDesString() {
            return m_obdDevice.DeviceDesString();
        }

        public string GetDeviceIDString() {
            return m_obdDevice.DeviceIDString();
        }

        public ProtocolType GetProtocol() {
            return m_settings.ProtocolIndex;
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
            if (m_obdDevice.Initialize(m_settings) && InitOBD()) {
                m_settings.ProtocolIndex = m_obdDevice.GetProtocolType();
                m_settings.ComPort = m_obdDevice.GetComPortIndex();
                SaveCommSettings(m_settings);
                m_obdDevice.SetConnected(true);
                OnConnect?.Invoke();
                return true;
            }
            m_obdDevice.SetConnected(false);
            return false;
        }

        public bool InitOBD() {
            OBDParameter param = new OBDParameter(1, 0, 0) {
                ValueTypes = 32
            };
            OBDParameterValue value = GetValue(param, true);
            if (value.ErrorDetected) {
                return false;
            }

            foreach (OBDParameter param2 in m_listAllParameters) {
                if (param2.Parameter > 0 && param2.Parameter <= 32 && value.GetBitFlag(param2.Parameter - 1)) {
                    m_listSupportedParameters.Add(param2);
                }
            }
            if (!value.GetBitFlag(31)) {
                return true;
            }

            param.Parameter = 32;
            value = GetValue(param, true);
            if (value.ErrorDetected) {
                return false;
            }
            foreach (OBDParameter param2 in m_listAllParameters) {
                if (param2.Parameter > 32 && param2.Parameter <= 64 && value.GetBitFlag(param2.Parameter - 33)) {
                    m_listSupportedParameters.Add(param2);
                }
            }
            return true;
        }

        public bool IsParameterSupported(string strPID) {
            foreach (OBDParameter param in m_listSupportedParameters) {
                if (param.PID.CompareTo(strPID) == 0) {
                    return true;
                }
            }
            return false;
        }

        public OBDParameterValue GetValue(string strPID, bool bEnglishUnits) {
            OBDParameter obdParameter = LookupParameter(strPID);
            if (obdParameter != null) {
                return GetValue(obdParameter, bEnglishUnits);
            }

            OBDParameterValue value = new OBDParameterValue {
                ErrorDetected = true
            };
            return value;
        }

        public OBDParameterValue GetValue(OBDParameter param, bool bEnglishUnits) {
            if (param.PID.Length > 0) {
                m_log.TraceInfo("Requesting: " + param.PID);
            } else {
                m_log.TraceInfo("Requesting: " + param.OBDRequest);
            }

            if (param.Service == 0) {
                return SpecialValue(param, bEnglishUnits);
            }

            OBDResponseList responses = m_obdDevice.Query(param);
            string strItem1 = "Responses: ";
            if (responses.ResponseCount > 0) {
                int count = 0;
                do {
                    strItem1 += string.Format("[{0}] ", responses.GetOBDResponse(count).Data);
                    ++count;
                } while (count < responses.ResponseCount);
            }
            m_log.TraceInfo(strItem1);
            OBDParameterValue obdParameterValue = OBDInterpreter.GetValue(param, responses, bEnglishUnits);
            if (obdParameterValue.ErrorDetected) {
                m_log.TraceError("Error Detected in obdParameterValue!");
                return obdParameterValue;
            } else {
                string values = "Values: ";
                if ((param.ValueTypes & 0x01) == 0x01) {
                    values += string.Format("[Double: {0}] ", obdParameterValue.DoubleValue.ToString());
                }
                if ((param.ValueTypes & 0x02) == 0x02) {
                    values += string.Format("[Bool: {0}] ", obdParameterValue.BoolValue.ToString());
                }
                if ((param.ValueTypes & 0x04) == 0x04) {
                    values += string.Format("[String: {0} / {1}] ", obdParameterValue.StringValue, obdParameterValue.ShortStringValue);
                }
                if ((param.ValueTypes & 0x08) == 0x08) {
                    values += "[StringCollection: ";
                    foreach (string strx in obdParameterValue.StringCollectionValue) {
                        values = string.Concat(values, strx + ", ");
                    }
                    values = values.Substring(0, values.Length - 2);
                    values += "]";
                }
                if ((param.ValueTypes & 0x20) == 0x20) {
                    values += "[BitFlags: ";
                    for (int idx = 0; idx < 32; idx++) {
                        values += (obdParameterValue.GetBitFlag(idx) ? "T" : "F");
                    }
                    values += "]";
                }
                m_log.TraceInfo(values);
                return obdParameterValue;
            }
        }

        public OBDParameterValue SpecialValue(OBDParameter param, bool bEnglishUnits) {
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


        public bool ClearCodes() {
            return (m_obdDevice.Query("04").IndexOf("44") >= 0);
        }

        public void Disconnect() {
            m_obdDevice.Disconnect();
            m_obdDevice.SetConnected(false);
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
                            case "ProScan":
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
            m_settings.HardwareIndex = device;
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

        public void SaveCommSettings(Settings settings) {
            m_settings = settings;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(m_settings_xml)) {
                xmlSerializer.Serialize(writer, m_settings);
                writer.Close();
            }
        }

        public void SaveUserPreferences(UserPreferences prefs) {
            m_userpreferences = prefs;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserPreferences));
            using (TextWriter writer = (TextWriter)new StreamWriter(m_userprefs_xml)) {
                xmlSerializer.Serialize(writer, m_userpreferences);
                writer.Close();
            }
        }

        public UserPreferences UserPreferences {
            get { return m_userpreferences; }
        }

        public Settings CommSettings {
            get { return m_settings; }
        }

        public Settings LoadCommSettings() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream reader = new FileStream(m_settings_xml, FileMode.Open)) {
                    m_settings = (Settings)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception e) {
                m_log.TraceError("Using default communication settings because of failed to load them, reason: " + e.Message);
                m_settings = new Settings();
            }
            return m_settings;
        }

        public UserPreferences LoadUserPreferences() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(UserPreferences));
                using (FileStream reader = new FileStream(m_userprefs_xml, FileMode.Open)) {
                    m_userpreferences = (UserPreferences)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception e) {
                m_log.TraceError("Using default user preferences because of failed to load them, reason: " + e.Message);
                m_userpreferences = new UserPreferences();
            }
            return m_userpreferences;
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
                    if (m_settings.ActiveProfileIndex >= 0 && m_settings.ActiveProfileIndex < m_VehicleProfiles.Count) {
                        return m_VehicleProfiles[m_settings.ActiveProfileIndex];
                    }
                    if (m_VehicleProfiles.Count == 0) {
                        m_VehicleProfiles.Add(new VehicleProfile());
                    }
                    m_settings.ActiveProfileIndex = 0;

                    return m_VehicleProfiles[0];
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

        public List<VehicleProfile> VehicleProfiles {
            get { return m_VehicleProfiles; }
        }

        public void SaveVehicleProfiles(List<VehicleProfile> profiles) {
            FileStream file = null;
            m_VehicleProfiles = profiles;
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

        public Logger GetLogger() { return m_log; }
    }
}