using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SH_OBD {
    public class DiagnosticReportControl : UserControl {
        public Color m_colorBorder;
        public Image m_imgLogo;
        public Image m_imgMilOn;
        public Image m_imgMilOff;
        public string m_strShopName;
        public string m_strShopAddress1;
        public string m_strShopAddress2;
        public string m_strShopTelephone;
        public string m_strClientName;
        public string m_strClientAddress1;
        public string m_strClientAddress2;
        public string m_strClientTelephone;
        public string m_strVehicle;
        public string m_strDate;
        public bool m_bMilStatus;
        public int m_iTotalCodes;
        public string m_strFreezeFrameDTC;
        public List<string> m_listCodes;
        public List<string> m_listDefinitions;
        public List<string> m_listPending;
        public List<string> m_listPendingDefinitions;
        public string m_strFuelSystem1Status;
        public string m_strFuelSystem2Status;
        public double m_dCalculatedLoad;
        public double m_dEngineCoolantTemp;
        public double m_dSTFT1;
        public double m_dSTFT2;
        public double m_dSTFT3;
        public double m_dSTFT4;
        public double m_dLTFT1;
        public double m_dLTFT2;
        public double m_dLTFT3;
        public double m_dLTFT4;
        public double m_dIntakePressure;
        public double m_dEngineRPM;
        public double m_dVehicleSpeed;
        public double m_dSparkAdvance;
        public bool m_bShowFuelSystemStatus;
        public bool m_bShowCalculatedLoad;
        public bool m_bShowEngineCoolantTemp;
        public bool m_bShowSTFT13;
        public bool m_bShowSTFT24;
        public bool m_bShowLTFT13;
        public bool m_bShowLTFT24;
        public bool m_bShowIntakePressure;
        public bool m_bShowEngineRPM;
        public bool m_bShowVehicleSpeed;
        public bool m_bShowSparkAdvance;
        public bool m_bMisfireMonitorSupported;
        public bool m_bMisfireMonitorCompleted;
        public bool m_bFuelSystemMonitorSupported;
        public bool m_bFuelSystemMonitorCompleted;
        public bool m_bComprehensiveMonitorSupported;
        public bool m_bComprehensiveMonitorCompleted;
        public bool m_bCatalystMonitorSupported;
        public bool m_bCatalystMonitorCompleted;
        public bool m_bHeatedCatalystMonitorSupported;
        public bool m_bHeatedCatalystMonitorCompleted;
        public bool m_bEvapSystemMonitorSupported;
        public bool m_bEvapSystemMonitorCompleted;
        public bool m_bSecondaryAirMonitorSupported;
        public bool m_bSecondaryAirMonitorCompleted;
        public bool m_bRefrigerantMonitorSupported;
        public bool m_bRefrigerantMonitorCompleted;
        public bool m_bOxygenSensorMonitorSupported;
        public bool m_bOxygenSensorMonitorCompleted;
        public bool m_bOxygenSensorHeaterMonitorSupported;
        public bool m_bOxygenSensorHeaterMonitorCompleted;
        public bool m_bEGRSystemMonitorSupported;
        public bool m_bEGRSystemMonitorCompleted;
        public Container components;

        [Category("Report")]
        [Description("Show Spark Advance")]
        public bool ShowSparkAdvance {
            get { return m_bShowSparkAdvance; }
            set { m_bShowSparkAdvance = value; }
        }

        [Category("Report")]
        [Description("Show Vehicle Speed")]
        public bool ShowVehicleSpeed {
            get { return m_bShowVehicleSpeed; }
            set { m_bShowVehicleSpeed = value; }
        }

        [Category("Report")]
        [Description("Show Engine RPM")]
        public bool ShowEngineRPM {
            get { return m_bShowEngineRPM; }
            set { m_bShowEngineRPM = value; }
        }

        [Category("Report")]
        [Description("Show Intake Manifold Pressure")]
        public bool ShowIntakePressure {
            get { return m_bShowIntakePressure; }
            set { m_bShowIntakePressure = value; }
        }

        [Category("Report")]
        [Description("Show LTFT Banks 2 and 4")]
        public bool ShowLTFT24 {
            get { return m_bShowLTFT24; }
            set { m_bShowLTFT24 = value; }
        }

        [Category("Report")]
        [Description("Show LTFT Banks 1 and 3")]
        public bool ShowLTFT13 {
            get { return m_bShowLTFT13; }
            set { m_bShowLTFT13 = value; }
        }

        [Category("Report")]
        [Description("Show STFT Banks 2 and 4")]
        public bool ShowSTFT24 {
            get { return m_bShowSTFT24; }
            set { m_bShowSTFT24 = value; }
        }

        [Category("Report")]
        [Description("Show STFT Banks 1 and 3")]
        public bool ShowSTFT13 {
            get { return m_bShowSTFT13; }
            set { m_bShowSTFT13 = value; }
        }

        [Category("Report")]
        [Description("Show Engine Coolant Temp")]
        public bool ShowEngineCoolantTemp {
            get { return m_bShowEngineCoolantTemp; }
            set { m_bShowEngineCoolantTemp = value; }
        }

        [Category("Report")]
        [Description("Show Calculated Load")]
        public bool ShowCalculatedLoad {
            get { return m_bShowCalculatedLoad; }
            set { m_bShowCalculatedLoad = value; }
        }

        [Category("Report")]
        [Description("Show Fuel System Status")]
        public bool ShowFuelSystemStatus {
            get { return m_bShowFuelSystemStatus; }
            set { m_bShowFuelSystemStatus = value; }
        }

        [Category("Report")]
        [Description("Is EGR System Monitor Completed?")]
        public bool EGRSystemMonitorCompleted {
            get { return m_bEGRSystemMonitorCompleted; }
            set { m_bEGRSystemMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is EGR System Monitor Supported?")]
        public bool EGRSystemMonitorSupported {
            get { return m_bEGRSystemMonitorSupported; }
            set { m_bEGRSystemMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Oxygen Sensor Heater Monitor Completed?")]
        public bool OxygenSensorHeaterMonitorCompleted {
            get { return m_bOxygenSensorHeaterMonitorCompleted; }
            set { m_bOxygenSensorHeaterMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Oxygen Sensor Heater Monitor Supported?")]
        public bool OxygenSensorHeaterMonitorSupported {
            get { return m_bOxygenSensorHeaterMonitorSupported; }
            set { m_bOxygenSensorHeaterMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Oxygen Sensor Monitor Completed?")]
        public bool OxygenSensorMonitorCompleted {
            get { return m_bOxygenSensorMonitorCompleted; }
            set { m_bOxygenSensorMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Oxygen Sensor Monitor Supported?")]
        public bool OxygenSensorMonitorSupported {
            get { return m_bOxygenSensorMonitorSupported; }
            set { m_bOxygenSensorMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is A/C System Refrigerant Monitor Completed?")]
        public bool RefrigerantMonitorCompleted {
            get { return m_bRefrigerantMonitorCompleted; }
            set { m_bRefrigerantMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is A/C System Refrigerant Monitor Supported?")]
        public bool RefrigerantMonitorSupported {
            get { return m_bRefrigerantMonitorSupported; }
            set { m_bRefrigerantMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Secondary Air System Monitor Completed?")]
        public bool SecondaryAirMonitorCompleted {
            get { return m_bSecondaryAirMonitorCompleted; }
            set { m_bSecondaryAirMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Secondary Air System Monitor Supported?")]
        public bool SecondaryAirMonitorSupported {
            get { return m_bSecondaryAirMonitorSupported; }
            set { m_bSecondaryAirMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Evaporative System Monitor Completed?")]
        public bool EvapSystemMonitorCompleted {
            get { return m_bEvapSystemMonitorCompleted; }
            set { m_bEvapSystemMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Evaporative System Monitor Supported?")]
        public bool EvapSystemMonitorSupported {
            get { return m_bEvapSystemMonitorSupported; }
            set { m_bEvapSystemMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Heated Catalyst Monitor Completed?")]
        public bool HeatedCatalystMonitorCompleted {
            get { return m_bHeatedCatalystMonitorCompleted; }
            set { m_bHeatedCatalystMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Heated Catalyst Monitor Supported?")]
        public bool HeatedCatalystMonitorSupported {
            get { return m_bHeatedCatalystMonitorSupported; }
            set { m_bHeatedCatalystMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Catalyst Monitor Completed?")]
        public bool CatalystMonitorCompleted {
            get { return m_bCatalystMonitorCompleted; }
            set { m_bCatalystMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Catalyst Monitor Supported?")]
        public bool CatalystMonitorSupported {
            get { return m_bCatalystMonitorSupported; }
            set { m_bCatalystMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Comprehensive Component Monitor Completed?")]
        public bool ComprehensiveMonitorCompleted {
            get { return m_bComprehensiveMonitorCompleted; }
            set { m_bComprehensiveMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Comprehensive Component Monitor Supported?")]
        public bool ComprehensiveMonitorSupported {
            get { return m_bComprehensiveMonitorSupported; }
            set { m_bComprehensiveMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Fuel System Monitor Completed?")]
        public bool FuelSystemMonitorCompleted {
            get { return m_bFuelSystemMonitorCompleted; }
            set { m_bFuelSystemMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Fuel System Monitor Supported?")]
        public bool FuelSystemMonitorSupported {
            get { return m_bFuelSystemMonitorSupported; }
            set { m_bFuelSystemMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Is Misfire Monitor Completed?")]
        public bool MisfireMonitorCompleted {
            get { return m_bMisfireMonitorCompleted; }
            set { m_bMisfireMonitorCompleted = value; }
        }

        [Category("Report")]
        [Description("Is Misfire Monitor Supported?")]
        public bool MisfireMonitorSupported {
            get { return m_bMisfireMonitorSupported; }
            set { m_bMisfireMonitorSupported = value; }
        }

        [Category("Report")]
        [Description("Ignition Timing Advance (deg)")]
        public double SparkAdvance {
            get { return m_dSparkAdvance; }
            set { m_dSparkAdvance = value; }
        }

        [Category("Report")]
        [Description("Vehicle Speed (mph)")]
        public double VehicleSpeed {
            get { return m_dVehicleSpeed; }
            set { m_dVehicleSpeed = value; }
        }

        [Category("Report")]
        [Description("Engine RPM (rev/min)")]
        public double EngineRPM {
            get { return m_dEngineRPM; }
            set { m_dEngineRPM = value; }
        }

        [Category("Report")]
        [Description("Intake Manifold Pressure (inHg)")]
        public double IntakePressure {
            get { return m_dIntakePressure; }
            set { m_dIntakePressure = value; }
        }

        [Category("Report")]
        [Description("Long Term Fuel Trim - Bank 4 (%)")]
        public double LTFT4 {
            get { return m_dLTFT4; }
            set { m_dLTFT4 = value; }
        }

        [Category("Report")]
        [Description("Long Term Fuel Trim - Bank 3 (%)")]
        public double LTFT3 {
            get { return m_dLTFT3; }
            set { m_dLTFT3 = value; }
        }

        [Category("Report")]
        [Description("Long Term Fuel Trim - Bank 2 (%)")]
        public double LTFT2 {
            get { return m_dLTFT2; }
            set { m_dLTFT2 = value; }
        }

        [Category("Report")]
        [Description("Long Term Fuel Trim - Bank 1 (%)")]
        public double LTFT1 {
            get { return m_dLTFT1; }
            set { m_dLTFT1 = value; }
        }

        [Category("Report")]
        [Description("Short Term Fuel Trim - Bank 4 (%)")]
        public double STFT4 {
            get { return m_dSTFT4; }
            set { m_dSTFT4 = value; }
        }

        [Category("Report")]
        [Description("Short Term Fuel Trim - Bank 3 (%)")]
        public double STFT3 {
            get { return m_dSTFT3; }
            set { m_dSTFT3 = value; }
        }

        [Category("Report")]
        [Description("Short Term Fuel Trim - Bank 2 (%)")]
        public double STFT2 {
            get { return m_dSTFT2; }
            set { m_dSTFT2 = value; }
        }

        [Category("Report")]
        [Description("Short Term Fuel Trim - Bank 1 (%)")]
        public double STFT1 {
            get { return m_dSTFT1; }
            set { m_dSTFT1 = value; }
        }

        [Category("Report")]
        [Description("Engine Coolant Temperature (Fahrenheit)")]
        public double EngineCoolantTemp {
            get { return m_dEngineCoolantTemp; }
            set { m_dEngineCoolantTemp = value; }
        }

        [Category("Report")]
        [Description("Calculated Load Value (%)")]
        public double CalculatedLoad {
            get { return m_dCalculatedLoad; }
            set { m_dCalculatedLoad = value; }
        }

        [Category("Report")]
        [Description("Fuel System 2 Status")]
        public string FuelSystem2Status {
            get { return m_strFuelSystem2Status; }
            set { m_strFuelSystem2Status = value; }
        }

        [Category("Report")]
        [Description("Fuel System 1 Status")]
        public string FuelSystem1Status {
            get { return m_strFuelSystem1Status; }
            set { m_strFuelSystem1Status = value; }
        }

        [Category("Report")]
        [Description("List of pending trouble code definitions.")]
        public List<string> PendingDefinitionList {
            get { return m_listPendingDefinitions; }
            set { m_listPendingDefinitions = value; }
        }

        [Category("Report")]
        [Description("List of pending trouble codes.")]
        public List<string> PendingList {
            get { return m_listPending; }
            set { m_listPending = value; }
        }

        [Category("Report")]
        [Description("List of trouble code definitions.")]
        public List<string> DTCDefinitionList {
            get { return m_listDefinitions; }
            set { m_listDefinitions = value; }
        }

        [Category("Report")]
        [Description("List of trouble codes stored.")]
        public List<string> DTCList {
            get { return m_listCodes; }
            set { m_listCodes = value; }
        }

        [Category("Report")]
        [Description("Number of trouble codes stored.")]
        public int TotalCodes {
            get { return m_iTotalCodes; }
            set { m_iTotalCodes = value; }
        }

        [Category("Report")]
        [Description("Status of the vehicle's MIL.")]
        public bool MilStatus {
            get { return m_bMilStatus; }
            set { m_bMilStatus = value; }
        }

        [Category("Report")]
        [Description("The DTC that triggered Freeze Frame storage.")]
        public string FreezeFrameDTC {
            get { return m_strFreezeFrameDTC; }
            set { m_strFreezeFrameDTC = value; }
        }

        [Category("Report")]
        [Description("The year, make, and model of vehicle.")]
        public string Vehicle {
            get { return m_strVehicle; }
            set { m_strVehicle = value; }
        }

        [Category("Report")]
        [Description("The date the report was generated.")]
        public string GenerationDate {
            get { return m_strDate; }
            set { m_strDate = value; }
        }

        [Category("Report")]
        [Description("The telephone number of the client receiving the service.")]
        public string ClientTelephone {
            get { return m_strClientTelephone; }
            set { m_strClientTelephone = value; }
        }

        [Category("Report")]
        [Description("The second address line of the client receiving the service.")]
        public string ClientAddress2 {
            get { return m_strClientAddress2; }
            set { m_strClientAddress2 = value; }
        }

        [Category("Report")]
        [Description("The first address line of the client receiving the service.")]
        public string ClientAddress1 {
            get { return m_strClientAddress1; }
            set { m_strClientAddress1 = value; }
        }

        [Category("Report")]
        [Description("The name of the client receiving the service.")]
        public string ClientName {
            get { return m_strClientName; }
            set { m_strClientName = value; }
        }

        [Category("Report")]
        [Description("The telephone number of the company providing the service.")]
        public string ShopTelephone {
            get { return m_strShopTelephone; }
            set { m_strShopTelephone = value; }
        }

        [Category("Report")]
        [Description("The second address line of the company providing the service.")]
        public string ShopAddress2 {
            get { return m_strShopAddress2; }
            set { m_strShopAddress2 = value; }
        }

        [Category("Report")]
        [Description("The first address line of the company providing the service.")]
        public string ShopAddress1 {
            get { return m_strShopAddress1; }
            set { m_strShopAddress1 = value; }
        }

        [Category("Report")]
        [Description("The name of the company providing the service.")]
        public string ShopName {
            get { return m_strShopName; }
            set { m_strShopName = value; }
        }

        [Category("Report")]
        [Description("The MIL On indicator graphic.")]
        public Image MilOnImage {
            get { return m_imgMilOn; }
            set { m_imgMilOn = value; }
        }

        [Category("Report")]
        [Description("The MIL Off indicator graphic.")]
        public Image MilOffImage {
            get { return m_imgMilOff; }
            set { m_imgMilOff = value; }
        }

        [Category("Report")]
        [Description("The logo to display in the header of the report.")]
        public Image Logo {
            get { return m_imgLogo; }
            set { m_imgLogo = value; }
        }

        [Category("Report")]
        [Description("The border color for the report.")]
        public Color BorderColor {
            get { return m_colorBorder; }
            set { m_colorBorder = value; }
        }

        public DiagnosticReportControl() {
            m_colorBorder = new Color();
            InitializeComponent();
            BorderColor = Color.Black;
            FreezeFrameDTC = "P0000";
            ShopName = " ";
            ShopAddress1 = " ";
            ShopAddress2 = " ";
            ShopTelephone = " ";
            ClientName = " ";
            ClientAddress1 = " ";
            ClientAddress2 = " ";
            ClientTelephone = " ";
            Vehicle = " ";
            GenerationDate = " ";
            DTCList = new List<string>();
            DTCDefinitionList = new List<string>();
            PendingList = new List<string>();
            PendingDefinitionList = new List<string>();
            m_bShowFuelSystemStatus = false;
            m_bShowCalculatedLoad = false;
            m_bShowEngineCoolantTemp = false;
            m_bShowSTFT13 = false;
            m_bShowSTFT24 = false;
            m_bShowLTFT13 = false;
            m_bShowLTFT24 = false;
            m_bShowIntakePressure = false;
            m_bShowEngineRPM = false;
            m_bShowVehicleSpeed = false;
            m_bShowSparkAdvance = false;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void InitializeComponent() {
            BackColor = Color.White;
            Name = "DiagnosticReportControl";
            Paint += new PaintEventHandler(DiagnosticReportControl_Paint);
        }

        public void SetJunk() {
        }

        private void DiagnosticReportControl_Paint(object sender, PaintEventArgs e) {
            PaintControl(e.Graphics);
        }

        private void PaintControl(Graphics g) {
            Brush brush1 = new SolidBrush(Color.Blue);
            Brush brush2 = new SolidBrush(Color.Green);
            Brush brush3 = new SolidBrush(Color.Red);
            Brush brush4 = new SolidBrush(Color.Black);
            Brush brush5 = new SolidBrush(Color.FromArgb(235, 235, byte.MaxValue));
            Pen pen2 = new Pen(Color.Blue);
            Pen pen3 = new Pen(BorderColor, 1f);
            Font font1 = new Font("Times New Roman", 8f, FontStyle.Bold);
            Font font2 = new Font("Times New Roman", 10f, FontStyle.Bold);
            Font font3 = new Font("Times New Roman", 6f);
            Font font4 = new Font("Times New Roman", 6f, FontStyle.Underline);
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            if (m_imgLogo != null) {
                int width = (int)(m_imgLogo.Width / m_imgLogo.Height * 75.0);
                if (width > 240) {
                    width = 240;
                }
                g.DrawImage(m_imgLogo, 5, 5, width, 75);
            }
            Font font6 = new Font("Arial", 18f);
            RectangleF layoutRectangle1 = new RectangleF(250f, 5f, 500f, font6.Height);
            g.DrawString("OBD-II 诊断报告", font6, brush4, layoutRectangle1, format);
            layoutRectangle1.X -= 1f;
            layoutRectangle1.Y -= 1f;
            g.DrawString("OBD-II 诊断报告", font6, brush1, layoutRectangle1, format);
            Font font7 = new Font("Arial", 10f, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            Font font8 = new Font("Arial", 8f);
            RectangleF layoutRectangle2 = new RectangleF(250f, layoutRectangle1.Height + 5f, 250f, font7.Height);
            g.DrawString("提供服务方:", font7, brush4, layoutRectangle2, format);
            layoutRectangle2.Height = font8.Height;
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y + 5.0f;
            g.DrawString(ShopName, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ShopAddress1, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ShopAddress2, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ShopTelephone, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.X = 500f;
            layoutRectangle2.Y = layoutRectangle1.Height + 5f;
            layoutRectangle2.Height = font7.Height;
            g.DrawString("接收服务方:", font7, brush4, layoutRectangle2, format);
            layoutRectangle2.Height = font8.Height;
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y + 5.0f;
            g.DrawString(ClientName, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ClientAddress1, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ClientAddress2, font8, brush4, layoutRectangle2, format);
            layoutRectangle2.Y = layoutRectangle2.Height + layoutRectangle2.Y;
            g.DrawString(ClientTelephone, font8, brush4, layoutRectangle2, format);
            g.DrawLine(pen2, 5, 110, 745, 110);
            g.DrawLine(pen2, 5, 112, 745, 112);
            string s1 = "我们对您的车辆 " + Vehicle + " ，于 " + GenerationDate + " 进行了分析。";
            RectangleF layoutRectangle3 = new RectangleF(0.0f, 112f, 750f, 20f);
            Font font9 = new Font("Arial", 8f);
            g.DrawString(s1, font9, brush4, layoutRectangle3, format);
            g.DrawLine(pen2, 5, (int)layoutRectangle3.Bottom, 745, (int)layoutRectangle3.Bottom);
            g.DrawLine(pen2, 5, (int)layoutRectangle3.Bottom + 2, 745, (int)layoutRectangle3.Bottom + 2);
            int num1 = (int)layoutRectangle3.Bottom + 2;
            int num2 = num1 + 10;
            RectangleF layoutRectangle4 = new RectangleF(5f, num2, 100f, 20f);
            g.DrawRectangle(pen2, (int)layoutRectangle4.X, (int)layoutRectangle4.Y, (int)layoutRectangle4.Width, (int)layoutRectangle4.Height);
            g.DrawString("MIL 故障指示灯", font1, brush1, layoutRectangle4, format);
            int num3 = font1.Height + num2 + 10;
            if (MilStatus && MilOnImage != null) {
                int x = 55 - MilOnImage.Width / 2;
                g.DrawImage(MilOnImage, x, num3 + 7, MilOnImage.Width, MilOnImage.Height);
                int num4 = MilOnImage.Height + num3 + 5;
                layoutRectangle4.Y = num4 + 7f;
                g.DrawString("ON", font2, brush3, layoutRectangle4, format);
                layoutRectangle4.Y = layoutRectangle4.Height + layoutRectangle4.Y + 17.0f;
                g.DrawRectangle(pen2, (int)layoutRectangle4.X, (int)layoutRectangle4.Y, (int)layoutRectangle4.Width, (int)layoutRectangle4.Height);
                g.DrawString("存储的故障码", font1, brush1, layoutRectangle4, format);
                layoutRectangle4.Y = layoutRectangle4.Height + layoutRectangle4.Y + 10.0f;
                int num5 = TotalCodes;
                g.DrawString(num5.ToString("d"), font2, brush3, layoutRectangle4, format);
            } else if (MilOffImage != null) {
                int x = 55 - MilOffImage.Width / 2;
                g.DrawImage(MilOffImage, x, num3 + 7, MilOffImage.Width, MilOffImage.Height);
                int num4 = MilOffImage.Height + num3 + 5;
                layoutRectangle4.Y = num4 + 7f;
                g.DrawString("OFF", font2, brush2, layoutRectangle4, format);
                layoutRectangle4.Y = layoutRectangle4.Height + layoutRectangle4.Y + 17.0f;
                g.DrawRectangle(pen2, (int)layoutRectangle4.X, (int)layoutRectangle4.Y, (int)layoutRectangle4.Width, (int)layoutRectangle4.Height);
                g.DrawString("存储的故障码", font1, brush1, layoutRectangle4, format);
                layoutRectangle4.Y = layoutRectangle4.Height + layoutRectangle4.Y + 10.0f;
                int num5 = TotalCodes;
                g.DrawString(num5.ToString("d"), font2, brush2, layoutRectangle4, format);
            }
            int y1 = num1 + 10;
            RectangleF layoutRectangle5 = new RectangleF(115f, y1, 630f, 20f);
            g.DrawRectangle(pen2, (int)layoutRectangle5.X, (int)layoutRectangle5.Y, (int)layoutRectangle5.Width, (int)layoutRectangle5.Height);
            g.DrawString("存储的故障码", font1, brush1, layoutRectangle5, format);
            int num6 = y1 + font1.Height + 10;
            layoutRectangle5.Height = font3.Height;
            for (int i = 0; i < 5; i++) {
                layoutRectangle5.X = 115f;
                layoutRectangle5.Y = num6;
                if (i % 2 == 0) {
                    g.FillRectangle(brush5, (int)layoutRectangle5.X, (int)layoutRectangle5.Y, (int)layoutRectangle5.Width, (int)layoutRectangle5.Height);
                }
                layoutRectangle5.X += 5f;
                if (DTCList != null && DTCDefinitionList != null && (i < DTCList.Count && i < DTCDefinitionList.Count)) {
                    string s2 = DTCList[i] + " = " + DTCDefinitionList[i];
                    g.DrawString(s2, font3, brush4, layoutRectangle5);
                }
                num6 = font3.Height + num6;
            }
            int num7 = num6 + 5;
            layoutRectangle5.Height = 20f;
            layoutRectangle5.Y = num7;
            layoutRectangle5.X = 115f;
            g.DrawRectangle(pen2, (int)layoutRectangle5.X, (int)layoutRectangle5.Y, (int)layoutRectangle5.Width, (int)layoutRectangle5.Height);
            g.DrawString("未决故障码", font1, brush1, layoutRectangle5, format);
            int y2 = font1.Height + num7 + 10;
            layoutRectangle5.Height = font3.Height;
            for (int i = 0; i < 5; i++) {
                layoutRectangle5.X = 115f;
                layoutRectangle5.Y = y2;
                if (i % 2 == 0) {
                    g.FillRectangle(brush5, (int)layoutRectangle5.X, (int)layoutRectangle5.Y, (int)layoutRectangle5.Width, (int)layoutRectangle5.Height);
                }
                layoutRectangle5.X += 5f;
                if (PendingList != null && PendingDefinitionList != null && (i < PendingList.Count && i < PendingDefinitionList.Count)) {
                    string s2 = PendingList[i] + " = " + PendingDefinitionList[i];
                    g.DrawString(s2, font3, brush4, layoutRectangle5);
                }
                y2 = font3.Height + y2;
            }
            g.DrawLine(pen2, 110, y1, 110, y2);
            int num8 = y2 + 5;
            g.DrawLine(pen2, 5, num8, 745, num8);
            g.DrawLine(pen2, 5, num8 + 2, 745, num8 + 2);
            int num9 = num8 + 12;
            bool flag = string.Compare(FreezeFrameDTC, "P0000") != 0;
            // new string((sbyte*) &\u003CModule\u003E.\u003F\u003F_C\u0040_05OIJEMFOP\u0040P0000\u003F\u0024AA\u0040)
            RectangleF layoutRectangle6 = new RectangleF(5f, num9, 740f, 20f);
            g.DrawRectangle(pen2, (int)layoutRectangle6.X, (int)layoutRectangle6.Y, (int)layoutRectangle6.Width, (int)layoutRectangle6.Height);
            string s3 = !flag ? "冻结帧数据 (不可用)" : "冻结帧数据 (被: " + FreezeFrameDTC + " 帧触发)";
            g.DrawString(s3, font1, brush1, layoutRectangle6, format);
            int num10 = font1.Height + num9 + 10;
            format.LineAlignment = StringAlignment.Near;
            float y3 = num10;
            RectangleF layoutRectangle7 = new RectangleF(5f, y3, 161f, font3.Height);
            RectangleF layoutRectangle8 = new RectangleF(layoutRectangle7.Right, y3, 66f, font3.Height);
            RectangleF layoutRectangle9 = new RectangleF(layoutRectangle8.Right, y3, 36f, font3.Height);
            RectangleF layoutRectangle10 = new RectangleF(layoutRectangle9.Right, y3, 66f, font3.Height);
            RectangleF layoutRectangle11 = new RectangleF(layoutRectangle10.Right, y3, 36f, font3.Height);
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("描述", font4, brush4, layoutRectangle7, format);
            g.DrawString("英制", font4, brush4, layoutRectangle8, format);
            g.DrawString("单位", font4, brush4, layoutRectangle9, format);
            g.DrawString("公制", font4, brush4, layoutRectangle10, format);
            g.DrawString("单位", font4, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("燃油系统 1 状态", font3, brush4, layoutRectangle7);
            if (flag && m_bShowFuelSystemStatus) {
                g.DrawString(FuelSystem1Status, font3, brush4, layoutRectangle8, format);
                g.DrawString(FuelSystem1Status, font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("计算负荷", font3, brush4, layoutRectangle7);
            if (flag && m_bShowCalculatedLoad) {
                double num4 = CalculatedLoad;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                // new string((sbyte*) &\u003CModule\u003E.\u003F\u003F_C\u0040_06HIJAJLEL\u0040\u003F\u0024CD\u003F\u0024CD0\u003F4\u003F\u0024CD\u003F\u0024CD\u003F\u0024AA\u0040)
                double num5 = CalculatedLoad;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("短时燃油修正 - 组 1", font3, brush4, layoutRectangle7);
            if (flag && m_bShowSTFT13) {
                double num4 = STFT1;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = STFT1;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("短时燃油修正 - 组 2", font3, brush4, layoutRectangle7);
            if (flag && m_bShowSTFT24) {
                double num4 = STFT2;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = STFT2;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("短时燃油修正 - 组 3", font3, brush4, layoutRectangle7);
            if (flag && m_bShowSTFT13) {
                double num4 = STFT3;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = STFT3;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("短时燃油修正 - 组 4", font3, brush4, layoutRectangle7);
            if (flag && m_bShowSTFT24) {
                double num4 = STFT4;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = STFT4;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("进气歧管压力", font3, brush4, layoutRectangle7);
            if (flag && m_bShowIntakePressure) {
                double num4 = IntakePressure;
                double num5 = Math.Round(IntakePressure * 0.2953, 2);
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle8, format);
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("inHg", font3, brush4, layoutRectangle9, format);
            g.DrawString("kPa", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("车辆速度", font3, brush4, layoutRectangle7);
            if (flag && m_bShowVehicleSpeed) {
                double num4 = VehicleSpeed;
                double num5 = Math.Round(VehicleSpeed * 0.6214, 2);
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle8, format);
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("mph", font3, brush4, layoutRectangle9, format);
            g.DrawString("kph", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.X = layoutRectangle11.Right + 10f;
            layoutRectangle7.Y = y3;
            layoutRectangle8.X = layoutRectangle7.Right;
            layoutRectangle8.Y = y3;
            layoutRectangle9.X = layoutRectangle8.Right;
            layoutRectangle9.Y = y3;
            layoutRectangle10.X = layoutRectangle9.Right;
            layoutRectangle10.Y = y3;
            layoutRectangle11.X = layoutRectangle10.Right;
            layoutRectangle11.Y = y3;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("描述", font4, brush4, layoutRectangle7, format);
            g.DrawString("英制", font4, brush4, layoutRectangle8, format);
            g.DrawString("单位", font4, brush4, layoutRectangle9, format);
            g.DrawString("公制", font4, brush4, layoutRectangle10, format);
            g.DrawString("单位", font4, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("燃油系统 2 状态", font3, brush4, layoutRectangle7);
            if (flag && m_bShowFuelSystemStatus) {
                g.DrawString(FuelSystem2Status, font3, brush4, layoutRectangle8, format);
                g.DrawString(FuelSystem2Status, font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("", font3, brush4, layoutRectangle9, format);
            g.DrawString("", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("发动机冷却液温度", font3, brush4, layoutRectangle7);
            if (flag && m_bShowEngineCoolantTemp) {
                double num4 = EngineCoolantTemp;
                double num5 = Math.Round(EngineCoolantTemp * 1.8 + 32, 2);
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle8, format);
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("°F", font3, brush4, layoutRectangle9, format);
            g.DrawString("°C", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("长时燃油修正 - 组 1", font3, brush4, layoutRectangle7);
            if (flag && m_bShowLTFT13) {
                double num4 = LTFT1;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = LTFT1;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("长时燃油修正 - 组 2", font3, brush4, layoutRectangle7);
            if (flag && m_bShowLTFT24) {
                double num4 = LTFT2;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = LTFT2;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("长时燃油修正 - 组 3", font3, brush4, layoutRectangle7);
            if (flag && m_bShowLTFT13) {
                double num4 = LTFT3;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = LTFT3;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("长时燃油修正 - 组 4", font3, brush4, layoutRectangle7);
            if (flag && m_bShowLTFT24) {
                double num4 = LTFT4;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = LTFT4;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("%", font3, brush4, layoutRectangle9, format);
            g.DrawString("%", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.DrawString("发动机转速", font3, brush4, layoutRectangle7);
            if (flag && m_bShowEngineRPM) {
                double num4 = EngineRPM;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = EngineRPM;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("rpm", font3, brush4, layoutRectangle9, format);
            g.DrawString("rpm", font3, brush4, layoutRectangle11, format);
            layoutRectangle7.Y = font3.Height + layoutRectangle7.Y;
            layoutRectangle8.Y = font3.Height + layoutRectangle8.Y;
            layoutRectangle9.Y = font3.Height + layoutRectangle9.Y;
            layoutRectangle10.Y = font3.Height + layoutRectangle10.Y;
            layoutRectangle11.Y = font3.Height + layoutRectangle11.Y;
            g.FillRectangle(brush5, (int)layoutRectangle7.X, (int)layoutRectangle7.Y, (int)layoutRectangle7.Width, (int)layoutRectangle7.Height);
            g.FillRectangle(brush5, (int)layoutRectangle8.X, (int)layoutRectangle8.Y, (int)layoutRectangle8.Width, (int)layoutRectangle8.Height);
            g.FillRectangle(brush5, (int)layoutRectangle9.X, (int)layoutRectangle9.Y, (int)layoutRectangle9.Width, (int)layoutRectangle9.Height);
            g.FillRectangle(brush5, (int)layoutRectangle10.X, (int)layoutRectangle10.Y, (int)layoutRectangle10.Width, (int)layoutRectangle10.Height);
            g.FillRectangle(brush5, (int)layoutRectangle11.X, (int)layoutRectangle11.Y, (int)layoutRectangle11.Width, (int)layoutRectangle11.Height);
            g.DrawString("点火提前角", font3, brush4, layoutRectangle7);
            if (flag && m_bShowSparkAdvance) {
                double num4 = SparkAdvance;
                g.DrawString(num4.ToString("f4"), font3, brush4, layoutRectangle8, format);
                double num5 = SparkAdvance;
                g.DrawString(num5.ToString("f4"), font3, brush4, layoutRectangle10, format);
            } else {
                g.DrawString("-", font3, brush4, layoutRectangle8, format);
                g.DrawString("-", font3, brush4, layoutRectangle10, format);
            }
            g.DrawString("°", font3, brush4, layoutRectangle9, format);
            g.DrawString("°", font3, brush4, layoutRectangle11, format);
            int num11 = (int)(layoutRectangle11.Bottom + 5.0);
            g.DrawLine(pen2, 5, num11, 745, num11);
            g.DrawLine(pen2, 5, num11 + 2, 745, num11 + 2);
            format.LineAlignment = StringAlignment.Center;
            int num12 = num11 + 12;
            RectangleF layoutRectangle12 = new RectangleF(5f, num12, 740f, 20f);
            g.DrawRectangle(pen2, (int)layoutRectangle12.X, (int)layoutRectangle12.Y, (int)layoutRectangle12.Width, (int)layoutRectangle12.Height);
            g.DrawString("连续诊断与非连续诊断", font1, brush1, layoutRectangle12, format);
            int num13 = font1.Height + num12 + 10;
            float y4 = num13;
            RectangleF layoutRectangle13 = new RectangleF(5f, y4, 183f, font3.Height);
            RectangleF layoutRectangle14 = new RectangleF(layoutRectangle13.Right, y4, 91f, font3.Height);
            RectangleF layoutRectangle15 = new RectangleF(layoutRectangle14.Right, y4, 91f, font3.Height);
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("连续诊断", font4, brush4, layoutRectangle13, format);
            g.DrawString("支持?", font4, brush4, layoutRectangle14, format);
            g.DrawString("完成?", font4, brush4, layoutRectangle15, format);
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("失火", font3, brush4, layoutRectangle13);
            if (MisfireMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (MisfireMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("燃油系统", font3, brush4, layoutRectangle13);
            if (FuelSystemMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (FuelSystemMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("综合部件", font3, brush4, layoutRectangle13);
            if (ComprehensiveMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (ComprehensiveMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.X = layoutRectangle15.Right + 7f;
            layoutRectangle13.Y = y4;
            layoutRectangle14.X = layoutRectangle13.Right;
            layoutRectangle14.Y = y4;
            layoutRectangle15.X = layoutRectangle14.Right;
            layoutRectangle15.Y = y4;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("非连续诊断", font4, brush4, layoutRectangle13, format);
            g.DrawString("支持?", font4, brush4, layoutRectangle14, format);
            g.DrawString("完成?", font4, brush4, layoutRectangle15, format);
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("催化器", font3, brush4, layoutRectangle13);
            if (CatalystMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (CatalystMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("加热催化器", font3, brush4, layoutRectangle13);
            if (HeatedCatalystMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (HeatedCatalystMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("蒸发系统", font3, brush4, layoutRectangle13);
            if (EvapSystemMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (EvapSystemMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("二次空气系统", font3, brush4, layoutRectangle13);
            if (SecondaryAirMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (SecondaryAirMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("A/C 系统制冷剂", font3, brush4, layoutRectangle13);
            if (RefrigerantMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (RefrigerantMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("氧气传感器", font3, brush4, layoutRectangle13);
            if (OxygenSensorMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (OxygenSensorMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.DrawString("加热氧气传感器", font3, brush4, layoutRectangle13);
            if (OxygenSensorHeaterMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (OxygenSensorHeaterMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            layoutRectangle13.Y = font3.Height + layoutRectangle13.Y;
            layoutRectangle14.Y = font3.Height + layoutRectangle14.Y;
            layoutRectangle15.Y = font3.Height + layoutRectangle15.Y;
            g.FillRectangle(brush5, (int)layoutRectangle13.X, (int)layoutRectangle13.Y, (int)layoutRectangle13.Width, (int)layoutRectangle13.Height);
            g.FillRectangle(brush5, (int)layoutRectangle14.X, (int)layoutRectangle14.Y, (int)layoutRectangle14.Width, (int)layoutRectangle14.Height);
            g.FillRectangle(brush5, (int)layoutRectangle15.X, (int)layoutRectangle15.Y, (int)layoutRectangle15.Width, (int)layoutRectangle15.Height);
            g.DrawString("EGR 系统", font3, brush4, layoutRectangle13);
            if (EGRSystemMonitorSupported) {
                g.DrawString("支持", font3, brush2, layoutRectangle14, format);
                if (EGRSystemMonitorCompleted) {
                    g.DrawString("完成", font3, brush2, layoutRectangle15, format);
                } else {
                    g.DrawString("未完成", font3, brush3, layoutRectangle15, format);
                }
            } else {
                g.DrawString("不适用", font3, brush4, layoutRectangle14, format);
                g.DrawString("-", font3, brush4, layoutRectangle15, format);
            }
            int num14 = (int)(layoutRectangle13.Bottom + 5.0);
            g.DrawLine(pen2, 5, num14, 745, num14);
            g.DrawLine(pen2, 5, num14 + 2, 745, num14 + 2);
            if (num14 < 900) {
                RectangleF rectangleF = new RectangleF(5f, (num14 + 12), 740f, 20f);
                g.DrawRectangle(pen2, (int)rectangleF.X, (int)rectangleF.Y, (int)rectangleF.Width, (int)rectangleF.Height);
                g.DrawString("备注", font1, brush1, rectangleF, format);
                rectangleF.Y = rectangleF.Bottom;
                rectangleF.Height = 1000.0f - rectangleF.Y - 5.0f;
                g.FillRectangle(brush5, rectangleF);
                g.DrawRectangle(pen2, (int)rectangleF.X, (int)rectangleF.Y, (int)rectangleF.Width, (int)rectangleF.Height);
            }
            g.DrawRectangle(pen3, 0, 0, 749, 999);

            font2.Dispose();
            brush2.Dispose();
            brush3.Dispose();
            brush5.Dispose();
        }

        public Image GetImage() {
            Bitmap bitmap = new Bitmap(Width, Height);
            PaintControl(Graphics.FromImage((Image)bitmap));
            return bitmap as Image;
        }
    }
}