namespace SH_OBD {
    partial class ReportForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.ReportPage1 = new SH_OBD.DiagnosticReportControl();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Location = new System.Drawing.Point(476, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPreview.Location = new System.Drawing.Point(275, 12);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 24);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "预览(&v)";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPrint.Location = new System.Drawing.Point(375, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(90, 24);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Controls.Add(this.ReportPage1);
            this.panel.Location = new System.Drawing.Point(12, 42);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(842, 480);
            this.panel.TabIndex = 4;
            // 
            // ReportPage1
            // 
            this.ReportPage1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ReportPage1.BackColor = System.Drawing.Color.White;
            this.ReportPage1.BorderColor = System.Drawing.Color.Blue;
            this.ReportPage1.CalculatedLoad = 0D;
            this.ReportPage1.CatalystMonitorCompleted = false;
            this.ReportPage1.CatalystMonitorSupported = false;
            this.ReportPage1.ClientAddress1 = " ";
            this.ReportPage1.ClientAddress2 = " ";
            this.ReportPage1.ClientName = " ";
            this.ReportPage1.ClientTelephone = " ";
            this.ReportPage1.ComprehensiveMonitorCompleted = false;
            this.ReportPage1.ComprehensiveMonitorSupported = false;
            this.ReportPage1.DTCDefinitionList = null;
            this.ReportPage1.DTCList = null;
            this.ReportPage1.EGRSystemMonitorCompleted = false;
            this.ReportPage1.EGRSystemMonitorSupported = false;
            this.ReportPage1.EngineCoolantTemp = 0D;
            this.ReportPage1.EngineRPM = 0D;
            this.ReportPage1.EvapSystemMonitorCompleted = false;
            this.ReportPage1.EvapSystemMonitorSupported = false;
            this.ReportPage1.FreezeFrameDTC = "P0000";
            this.ReportPage1.FuelSystem1Status = "0";
            this.ReportPage1.FuelSystem2Status = "0";
            this.ReportPage1.FuelSystemMonitorCompleted = false;
            this.ReportPage1.FuelSystemMonitorSupported = false;
            this.ReportPage1.GenerationDate = " ";
            this.ReportPage1.HeatedCatalystMonitorCompleted = false;
            this.ReportPage1.HeatedCatalystMonitorSupported = false;
            this.ReportPage1.IntakePressure = 0D;
            this.ReportPage1.Location = new System.Drawing.Point(38, 3);
            this.ReportPage1.Logo = null;
            this.ReportPage1.LTFT1 = 0D;
            this.ReportPage1.LTFT2 = 0D;
            this.ReportPage1.LTFT3 = 0D;
            this.ReportPage1.LTFT4 = 0D;
            this.ReportPage1.MilOffImage = ((System.Drawing.Image)(resources.GetObject("ReportPage1.MilOffImage")));
            this.ReportPage1.MilOnImage = ((System.Drawing.Image)(resources.GetObject("ReportPage1.MilOnImage")));
            this.ReportPage1.MilStatus = false;
            this.ReportPage1.MisfireMonitorCompleted = false;
            this.ReportPage1.MisfireMonitorSupported = false;
            this.ReportPage1.Name = "ReportPage1";
            this.ReportPage1.OxygenSensorHeaterMonitorCompleted = false;
            this.ReportPage1.OxygenSensorHeaterMonitorSupported = false;
            this.ReportPage1.OxygenSensorMonitorCompleted = false;
            this.ReportPage1.OxygenSensorMonitorSupported = false;
            this.ReportPage1.PendingDefinitionList = null;
            this.ReportPage1.PendingList = null;
            this.ReportPage1.RefrigerantMonitorCompleted = false;
            this.ReportPage1.RefrigerantMonitorSupported = false;
            this.ReportPage1.SecondaryAirMonitorCompleted = false;
            this.ReportPage1.SecondaryAirMonitorSupported = false;
            this.ReportPage1.ShopAddress1 = " ";
            this.ReportPage1.ShopAddress2 = " ";
            this.ReportPage1.ShopName = " ";
            this.ReportPage1.ShopTelephone = " ";
            this.ReportPage1.ShowCalculatedLoad = false;
            this.ReportPage1.ShowEngineCoolantTemp = false;
            this.ReportPage1.ShowEngineRPM = false;
            this.ReportPage1.ShowFuelSystemStatus = false;
            this.ReportPage1.ShowIntakePressure = false;
            this.ReportPage1.ShowLTFT13 = false;
            this.ReportPage1.ShowLTFT24 = false;
            this.ReportPage1.ShowSparkAdvance = false;
            this.ReportPage1.ShowSTFT13 = false;
            this.ReportPage1.ShowSTFT24 = false;
            this.ReportPage1.ShowVehicleSpeed = false;
            this.ReportPage1.Size = new System.Drawing.Size(750, 1000);
            this.ReportPage1.SparkAdvance = 0D;
            this.ReportPage1.STFT1 = 0D;
            this.ReportPage1.STFT2 = 0D;
            this.ReportPage1.STFT3 = 0D;
            this.ReportPage1.STFT4 = 0D;
            this.ReportPage1.TabIndex = 0;
            this.ReportPage1.TotalCodes = 0;
            this.ReportPage1.Vehicle = " ";
            this.ReportPage1.VehicleSpeed = 0D;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 534);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panel);
            this.Name = "ReportForm";
            this.Text = "ReportForm";
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel;
        public DiagnosticReportControl ReportPage1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}