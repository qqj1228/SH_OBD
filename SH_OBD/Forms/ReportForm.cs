using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class ReportForm : Form {
        public ReportForm() {
            InitializeComponent();
            ReportPage1.DTCList = new System.Collections.Specialized.StringCollection();
            ReportPage1.DTCDefinitionList = new System.Collections.Specialized.StringCollection();
            ReportPage1.PendingList = new System.Collections.Specialized.StringCollection();
            ReportPage1.PendingDefinitionList = new System.Collections.Specialized.StringCollection();
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            if (printDialog1.ShowDialog() != DialogResult.OK) {
                return;
            }
            printDocument1.Print();
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e) {
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
            Image image = ReportPage1.GetImage();
            e.Graphics.DrawImage(image, 50, 50, 750, 1000);
            e.HasMorePages = false;
        }

        private void PrintPreview() {
            int num = (int)printPreviewDialog1.ShowDialog();
        }

        private void Print() {
            if (printDialog1.ShowDialog() != DialogResult.OK) {
                return;
            }
            printDocument1.Print();
        }

        private void btnPreview_Click(object sender, EventArgs e) {
            PrintPreview();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存 OBD-II 诊断报告";
            saveFileDialog.Filter = "SH_OBD 报告文件 (*.obd)|*.obd";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            int num = (int)saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Length <= 0) {
                return;
            }
            FileStream fileStream = File.Create(saveFileDialog.FileName);
            BinaryWriter binaryWriter = new BinaryWriter((Stream)fileStream);
            binaryWriter.Write(ReportPage1.ShopName);
            binaryWriter.Write(ReportPage1.ShopAddress1);
            binaryWriter.Write(ReportPage1.ShopAddress2);
            binaryWriter.Write(ReportPage1.ShopTelephone);
            binaryWriter.Write(ReportPage1.ClientName);
            binaryWriter.Write(ReportPage1.ClientAddress1);
            binaryWriter.Write(ReportPage1.ClientAddress2);
            binaryWriter.Write(ReportPage1.ClientTelephone);
            binaryWriter.Write(ReportPage1.Vehicle);
            binaryWriter.Write(ReportPage1.GenerationDate);
            binaryWriter.Write(ReportPage1.MilStatus);
            binaryWriter.Write(ReportPage1.TotalCodes);
            binaryWriter.Write(ReportPage1.FreezeFrameDTC);
            int index1 = 0;
            do {
                if (ReportPage1.DTCList != null) {
                    if (index1 < ReportPage1.DTCList.Count) {
                        binaryWriter.Write(ReportPage1.DTCList[index1]);
                    } else {
                        binaryWriter.Write("");
                    }
                } else {
                    binaryWriter.Write("");
                }
                ++index1;
            }
            while (index1 < 25);
            int index2 = 0;
            do {
                if (ReportPage1.DTCDefinitionList != null) {
                    if (index2 < ReportPage1.DTCDefinitionList.Count) {
                        binaryWriter.Write(ReportPage1.DTCDefinitionList[index2]);
                    } else {
                        binaryWriter.Write("");
                    }
                } else {
                    binaryWriter.Write("");
                }
                ++index2;
            }
            while (index2 < 25);
            int index3 = 0;
            do {
                if (ReportPage1.PendingList != null) {
                    if (index3 < ReportPage1.PendingList.Count) {
                        binaryWriter.Write(ReportPage1.PendingList[index3]);
                    } else {
                        binaryWriter.Write("");
                    }
                } else {
                    binaryWriter.Write("");
                }
                ++index3;
            } while (index3 < 25);
            int index4 = 0;
            do {
                if (ReportPage1.PendingDefinitionList != null) {
                    if (index4 < ReportPage1.PendingDefinitionList.Count) {
                        binaryWriter.Write(ReportPage1.PendingDefinitionList[index4]);
                    } else {
                        binaryWriter.Write("");
                    }
                } else {
                    binaryWriter.Write("");
                }
                ++index4;
            }
            while (index4 < 25);
            binaryWriter.Write(ReportPage1.FuelSystem1Status);
            binaryWriter.Write(ReportPage1.FuelSystem2Status);
            binaryWriter.Write(ReportPage1.CalculatedLoad);
            binaryWriter.Write(ReportPage1.EngineCoolantTemp);
            binaryWriter.Write(ReportPage1.STFT1);
            binaryWriter.Write(ReportPage1.STFT2);
            binaryWriter.Write(ReportPage1.STFT3);
            binaryWriter.Write(ReportPage1.STFT4);
            binaryWriter.Write(ReportPage1.LTFT1);
            binaryWriter.Write(ReportPage1.LTFT2);
            binaryWriter.Write(ReportPage1.LTFT3);
            binaryWriter.Write(ReportPage1.LTFT4);
            binaryWriter.Write(ReportPage1.IntakePressure);
            binaryWriter.Write(ReportPage1.EngineRPM);
            binaryWriter.Write(ReportPage1.VehicleSpeed);
            binaryWriter.Write(ReportPage1.SparkAdvance);
            binaryWriter.Write(ReportPage1.ShowFuelSystemStatus);
            binaryWriter.Write(ReportPage1.ShowCalculatedLoad);
            binaryWriter.Write(ReportPage1.ShowEngineCoolantTemp);
            binaryWriter.Write(ReportPage1.ShowSTFT13);
            binaryWriter.Write(ReportPage1.ShowSTFT24);
            binaryWriter.Write(ReportPage1.ShowLTFT13);
            binaryWriter.Write(ReportPage1.ShowLTFT24);
            binaryWriter.Write(ReportPage1.ShowIntakePressure);
            binaryWriter.Write(ReportPage1.ShowEngineRPM);
            binaryWriter.Write(ReportPage1.ShowVehicleSpeed);
            binaryWriter.Write(ReportPage1.ShowSparkAdvance);
            binaryWriter.Write(ReportPage1.MisfireMonitorSupported);
            binaryWriter.Write(ReportPage1.MisfireMonitorCompleted);
            binaryWriter.Write(ReportPage1.FuelSystemMonitorSupported);
            binaryWriter.Write(ReportPage1.FuelSystemMonitorCompleted);
            binaryWriter.Write(ReportPage1.ComprehensiveMonitorSupported);
            binaryWriter.Write(ReportPage1.ComprehensiveMonitorCompleted);
            binaryWriter.Write(ReportPage1.CatalystMonitorSupported);
            binaryWriter.Write(ReportPage1.CatalystMonitorCompleted);
            binaryWriter.Write(ReportPage1.HeatedCatalystMonitorSupported);
            binaryWriter.Write(ReportPage1.HeatedCatalystMonitorCompleted);
            binaryWriter.Write(ReportPage1.EvapSystemMonitorSupported);
            binaryWriter.Write(ReportPage1.EvapSystemMonitorCompleted);
            binaryWriter.Write(ReportPage1.SecondaryAirMonitorSupported);
            binaryWriter.Write(ReportPage1.SecondaryAirMonitorCompleted);
            binaryWriter.Write(ReportPage1.RefrigerantMonitorSupported);
            binaryWriter.Write(ReportPage1.RefrigerantMonitorCompleted);
            binaryWriter.Write(ReportPage1.OxygenSensorMonitorSupported);
            binaryWriter.Write(ReportPage1.OxygenSensorMonitorCompleted);
            binaryWriter.Write(ReportPage1.OxygenSensorHeaterMonitorSupported);
            binaryWriter.Write(ReportPage1.OxygenSensorHeaterMonitorCompleted);
            binaryWriter.Write(ReportPage1.EGRSystemMonitorSupported);
            binaryWriter.Write(ReportPage1.EGRSystemMonitorCompleted);
            binaryWriter.Close();
            fileStream.Close();
        }

    }
}
