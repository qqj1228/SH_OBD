using System;
using System.Collections.Generic;

namespace SH_OBD {
    internal class DataList {
        private DataNode pHead;
        private DataNode pTail;
        private int iTotalNodes;

        public DataList() {
            pHead = null;
            pTail = null;
            iTotalNodes = 0;
        }

        public void Insert(double value, long iTicks) {
            DataNode dataNode = new DataNode {
                pData = new DataItem(value, iTicks)
            };
            if (pHead == null) {
                pHead = dataNode;
                dataNode.pPrev = null;
            } else {
                pTail.pNext = dataNode;
                dataNode.pPrev = pTail;
            }
            pTail = dataNode;
            dataNode.pNext = null;
            ++iTotalNodes;
        }

        public void Insert(double value) {
            DataNode dataNode = new DataNode {
                pData = new DataItem(value)
            };
            if (pHead == null) {
                pHead = dataNode;
                dataNode.pPrev = null;
            } else {
                pTail.pNext = dataNode;
                dataNode.pPrev = pTail;
            }
            pTail = dataNode;
            dataNode.pNext = null;
            ++iTotalNodes;
        }

        public DataItem GetItem(int index) {
            if (index >= iTotalNodes) {
                return null;
            }
            DataNode dataNode = pHead;
            for (uint i = (uint)index; i > 0U; i--) {
                dataNode = dataNode.pNext;
            }
            return dataNode.pData;
        }

        public double GetValue(int index) {
            if (index >= iTotalNodes) {
                return -1.0;
            }
            DataNode dataNode = pHead;
            for (uint i = (uint)index; i > 0U; i--) {
                dataNode = dataNode.pNext;
            }
            return dataNode.pData.Value;
        }

        public long GetTicks(int index) {
            if (index >= iTotalNodes) {
                return -1L;
            }
            DataNode dataNode = pHead;
            for (uint i = (uint)index; i > 0U; i--) {
                dataNode = dataNode.pNext;
            }
            return dataNode.pData.Ticks;
        }

        public double GetSeconds(int index) {
            if (index >= iTotalNodes) {
                return -1.0;
            }
            DataNode dataNode = pHead;
            for (uint i = (uint)index; i > 0U; i--) {
                dataNode = dataNode.pNext;
            }
            return (dataNode.pData.Ticks - pHead.pData.Ticks) * 1E-07;
        }

        public int TotalItems() {
            return iTotalNodes;
        }

        public void Reset() {
            while (pTail != null) {
                pTail = pTail.pPrev;
            }
            pHead = (DataNode)null;
            pTail = (DataNode)null;
            iTotalNodes = 0;
        }
    }

    internal class DataItem {
        public double Value;
        public long Ticks;

        public DataItem(double value, long iTicks) {
            Value = value;
            Ticks = iTicks;
        }

        public DataItem(double value) {
            Value = value;
            Ticks = DateTime.Now.Ticks;
        }
    }

    internal class DataNode {
        public DataNode pNext;
        public DataNode pPrev;
        public DataItem pData;
    }

    [Serializable]
    public class DatedValue {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public DatedValue() {
        }

        public DatedValue(double dValue) {
            Value = dValue;
            Date = DateTime.Now;
        }
    }

    [Serializable]
    public class DynoRecord {
        public double Weight { get; set; }
        public string Label { get; set; }
        public double DriveRatio { get; set; }
        public List<DatedValue> RpmList { get; set; }
        public List<DatedValue> KphList { get; set; }
    }

    [Serializable]
    public class SensorLogItem {
        public DateTime Time { get; } = DateTime.Now;
        public string MetricUnits { get; }
        public string MetricDisplay { get; }
        public string EnglishUnits { get; }
        public string EnglishDisplay { get; }
        public string Name { get; }

        public SensorLogItem(string strName, string strEnglishDisplay, string strEnglishUnits, string strMetricDisplay, string strMetricUnits) {
            Name = strName;
            EnglishDisplay = strEnglishDisplay;
            EnglishUnits = strEnglishUnits;
            MetricDisplay = strMetricDisplay;
            MetricUnits = strMetricUnits;
        }
    }

    [Serializable]
    public class SensorValue {
        public string MetricDisplay { get; set; }
        public double MetricValue { get; set; }
        public string EnglishDisplay { get; set; }
        public double EnglishValue { get; set; }

        public SensorValue() { }

        public SensorValue(double dEnglishValue, string strEnglishDisplay, double dMetricValue, string strMetricDisplay) {
            EnglishValue = dEnglishValue;
            EnglishDisplay = strEnglishDisplay;
            MetricValue = dMetricValue;
            MetricDisplay = strMetricDisplay;
        }
    }

    public class Sensor {
        public double MetricMaxValue { get; set; }
        public double MetricMinValue { get; set; }
        public string MetricUnits { get; set; } = "";
        public double MetricValue { get; set; }
        public string MetricDisplay { get; set; } = "";
        public double EnglishMaxValue { get; set; }
        public double EnglishMinValue { get; set; }
        public string EnglishUnits { get; set; } = "";
        public double EnglishValue { get; set; }
        public string EnglishDisplay { get; set; } = "";
        public bool IsFor1D { get; set; }
        public bool IsO2Dependant { get; set; }
        public bool IsPlottable { get; set; }
        public bool IsSensor { get; set; }
        public int SubPID { get; set; }
        public int PID { get; set; }
        public int Service { get; set; }
        public string Name { get; set; }

        public Sensor() {
        }

        public Sensor(int iService, int iPID, int iSubPID, string strName, double dEnglishMinValue, double dEnglishMaxValue, string strEnglishUnits, double dMetricMinValue, double dMetricMaxValue, string strMetricUnits, bool bIsSensor, bool bIsPlottable, bool bIsO2Dependant, bool bIsFor1D) {
            Service = iService;
            PID = iPID;
            SubPID = iSubPID;
            Name = strName;
            EnglishMinValue = dEnglishMinValue;
            EnglishMaxValue = dEnglishMaxValue;
            EnglishUnits = strEnglishUnits;
            MetricMinValue = dMetricMinValue;
            MetricMaxValue = dMetricMaxValue;
            MetricUnits = strMetricUnits;
            IsSensor = bIsSensor;
            IsPlottable = bIsPlottable;
            IsO2Dependant = bIsO2Dependant;
            IsFor1D = bIsFor1D;
        }

        public override string ToString() {
            return Name;
        }
    }

    [Serializable]
    public class Timeslip {
        public double QuarterMileSpeed { get; set; }
        public double QuarterMileTime { get; set; }
        public double ThousandFootTime { get; set; }
        public double EighthMileSpeed { get; set; }
        public double EighthMileTime { get; set; }
        public double SixtyMphTime { get; set; }
        public double SixtyFootTime { get; set; }
        public string Vehicle { get; set; }
        public DateTime Date { get; set; }

        public Timeslip(DateTime dtDate, string strVehicle, double dSixtyFootTime, double dSixtyMphTime, double dEighthMileTime, double dEighthMileSpeed, double dThousandFootTime, double dQuarterMileTime, double dQuarterMileSpeed) {
            Date = dtDate;
            Vehicle = strVehicle;
            SixtyFootTime = dSixtyFootTime;
            SixtyMphTime = dSixtyMphTime;
            EighthMileTime = dEighthMileTime;
            EighthMileSpeed = dEighthMileSpeed;
            ThousandFootTime = dThousandFootTime;
            QuarterMileTime = dQuarterMileTime;
            QuarterMileSpeed = dQuarterMileSpeed;
        }

        public Timeslip() {
            Date = DateTime.Now;
        }

        public string GetStats() {
            string str = "60 英尺 时间 ..." + (" " + SixtyFootTime.ToString("000.000") + " sec\r\n").PadLeft(25, '.');
            str += "0 至 60 MPH ...." + (" " + SixtyMphTime.ToString("000.000") + " sec\r\n").PadLeft(25, '.');
            str += "1/8 英里 时间 .." + (" " + EighthMileTime.ToString("000.000") + " sec\r\n").PadLeft(25, '.');
            str += "1/8 英里 速度 .." + (" " + EighthMileSpeed.ToString("000.000") + " mph\r\n").PadLeft(25, '.');
            str += "1000 英尺 时间 ." + (" " + ThousandFootTime.ToString("000.000") + " sec\r\n").PadLeft(25, '.');
            str += "1/4 英里 时间 .." + (" " + QuarterMileTime.ToString("000.000") + " sec\r\n").PadLeft(25, '.');
            str += "1/4 英里 速度 " + (" " + QuarterMileSpeed.ToString("000.000") + " mph").PadLeft(25, '.');
            return str;
        }
    }

    public class DTC {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public DTC() {
        }

        public DTC(string strDTC, string strCategory, string strDesc) {
            Name = strDTC;
            Category = strCategory;
            Description = strDesc;
        }

        public override string ToString() {
            return Name;
        }
    }
}
