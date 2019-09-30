namespace SH_OBD {
    partial class DynoForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynoForm));
            this.numToRPM = new System.Windows.Forms.NumericUpDown();
            this.numFromRPM = new System.Windows.Forms.NumericUpDown();
            this.lblFromRPM = new System.Windows.Forms.Label();
            this.groupSetup = new System.Windows.Forms.GroupBox();
            this.lblToRPM = new System.Windows.Forms.Label();
            this.btnExportJPEG = new System.Windows.Forms.Button();
            this.groupExport = new System.Windows.Forms.GroupBox();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.groupGraph = new System.Windows.Forms.GroupBox();
            this.dyno = new DGChart.DynoControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.groupChart = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupControl = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numToRPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromRPM)).BeginInit();
            this.groupSetup.SuspendLayout();
            this.groupExport.SuspendLayout();
            this.groupGraph.SuspendLayout();
            this.groupChart.SuspendLayout();
            this.groupControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // numToRPM
            // 
            this.numToRPM.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numToRPM.Location = new System.Drawing.Point(128, 60);
            this.numToRPM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numToRPM.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.numToRPM.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numToRPM.Name = "numToRPM";
            this.numToRPM.ReadOnly = true;
            this.numToRPM.Size = new System.Drawing.Size(96, 25);
            this.numToRPM.TabIndex = 7;
            this.numToRPM.Value = new decimal(new int[] {
            6500,
            0,
            0,
            0});
            this.numToRPM.ValueChanged += new System.EventHandler(this.numToRPM_ValueChanged);
            // 
            // numFromRPM
            // 
            this.numFromRPM.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numFromRPM.Location = new System.Drawing.Point(128, 26);
            this.numFromRPM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numFromRPM.Maximum = new decimal(new int[] {
            5500,
            0,
            0,
            0});
            this.numFromRPM.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numFromRPM.Name = "numFromRPM";
            this.numFromRPM.ReadOnly = true;
            this.numFromRPM.Size = new System.Drawing.Size(96, 25);
            this.numFromRPM.TabIndex = 6;
            this.numFromRPM.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numFromRPM.ValueChanged += new System.EventHandler(this.numFromRPM_ValueChanged);
            // 
            // lblFromRPM
            // 
            this.lblFromRPM.Location = new System.Drawing.Point(16, 26);
            this.lblFromRPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFromRPM.Name = "lblFromRPM";
            this.lblFromRPM.Size = new System.Drawing.Size(112, 28);
            this.lblFromRPM.TabIndex = 2;
            this.lblFromRPM.Text = "自(&F) RPM:";
            this.lblFromRPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupSetup
            // 
            this.groupSetup.Controls.Add(this.numToRPM);
            this.groupSetup.Controls.Add(this.numFromRPM);
            this.groupSetup.Controls.Add(this.lblToRPM);
            this.groupSetup.Controls.Add(this.lblFromRPM);
            this.groupSetup.Location = new System.Drawing.Point(16, 15);
            this.groupSetup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupSetup.Name = "groupSetup";
            this.groupSetup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupSetup.Size = new System.Drawing.Size(240, 109);
            this.groupSetup.TabIndex = 11;
            this.groupSetup.TabStop = false;
            this.groupSetup.Text = "设置";
            // 
            // lblToRPM
            // 
            this.lblToRPM.Location = new System.Drawing.Point(16, 60);
            this.lblToRPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToRPM.Name = "lblToRPM";
            this.lblToRPM.Size = new System.Drawing.Size(112, 26);
            this.lblToRPM.TabIndex = 4;
            this.lblToRPM.Text = "至(&T) RPM:";
            this.lblToRPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExportJPEG
            // 
            this.btnExportJPEG.Location = new System.Drawing.Point(16, 26);
            this.btnExportJPEG.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportJPEG.Name = "btnExportJPEG";
            this.btnExportJPEG.Size = new System.Drawing.Size(208, 35);
            this.btnExportJPEG.TabIndex = 6;
            this.btnExportJPEG.Text = "&JPEG 图片";
            this.btnExportJPEG.Click += new System.EventHandler(this.btnExportJPEG_Click);
            // 
            // groupExport
            // 
            this.groupExport.Controls.Add(this.btnExportJPEG);
            this.groupExport.Location = new System.Drawing.Point(16, 452);
            this.groupExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupExport.Name = "groupExport";
            this.groupExport.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupExport.Size = new System.Drawing.Size(240, 75);
            this.groupExport.TabIndex = 10;
            this.groupExport.TabStop = false;
            this.groupExport.Text = "导出";
            // 
            // pageSetupDialog
            // 
            this.pageSetupDialog.Document = this.printDocument;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog1";
            this.printPreviewDialog.Visible = false;
            // 
            // printDialog
            // 
            this.printDialog.Document = this.printDocument;
            // 
            // groupGraph
            // 
            this.groupGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupGraph.Controls.Add(this.dyno);
            this.groupGraph.Location = new System.Drawing.Point(272, 15);
            this.groupGraph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupGraph.Name = "groupGraph";
            this.groupGraph.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupGraph.Size = new System.Drawing.Size(716, 566);
            this.groupGraph.TabIndex = 9;
            this.groupGraph.TabStop = false;
            // 
            // dyno
            // 
            this.dyno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dyno.BorderRight = 20;
            this.dyno.BorderTop = 20;
            this.dyno.ColorAxis = System.Drawing.Color.Black;
            this.dyno.ColorBg = System.Drawing.Color.White;
            this.dyno.ColorGrid = System.Drawing.Color.LightGray;
            this.dyno.ColorSet1 = System.Drawing.Color.DarkBlue;
            this.dyno.ColorSet2 = System.Drawing.Color.Red;
            this.dyno.ColorSet3 = System.Drawing.Color.Lime;
            this.dyno.ColorSet4 = System.Drawing.Color.Gold;
            this.dyno.ColorSet5 = System.Drawing.Color.Magenta;
            this.dyno.DrawMode = DGChart.DynoControl.DrawModeType.Line;
            this.dyno.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.dyno.Label = "0";
            this.dyno.Location = new System.Drawing.Point(8, 18);
            this.dyno.Logo = null;
            this.dyno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dyno.Name = "dyno";
            this.dyno.ShowData1 = true;
            this.dyno.ShowData2 = false;
            this.dyno.ShowData3 = false;
            this.dyno.ShowData4 = false;
            this.dyno.ShowData5 = false;
            this.dyno.Size = new System.Drawing.Size(700, 541);
            this.dyno.TabIndex = 0;
            this.dyno.XData1 = null;
            this.dyno.XData2 = null;
            this.dyno.XData3 = null;
            this.dyno.XData4 = null;
            this.dyno.XData5 = null;
            this.dyno.XGrid = 0.5D;
            this.dyno.XRangeEnd = 6.5D;
            this.dyno.XRangeStart = 0.5D;
            this.dyno.YData1 = null;
            this.dyno.YData2 = null;
            this.dyno.YData3 = null;
            this.dyno.YData4 = null;
            this.dyno.YData5 = null;
            this.dyno.YGrid = 50D;
            this.dyno.YRangeEnd = 200D;
            this.dyno.YRangeStart = 0D;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 26);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(208, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(16, 74);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(208, 34);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "打开(&O)";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(16, 121);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(208, 34);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupChart
            // 
            this.groupChart.Controls.Add(this.btnSave);
            this.groupChart.Controls.Add(this.btnOpen);
            this.groupChart.Controls.Add(this.btnPrint);
            this.groupChart.Location = new System.Drawing.Point(16, 271);
            this.groupChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupChart.Name = "groupChart";
            this.groupChart.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupChart.Size = new System.Drawing.Size(240, 169);
            this.groupChart.TabIndex = 8;
            this.groupChart.TabStop = false;
            this.groupChart.Text = "图表";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(16, 74);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(208, 34);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "重置(&R)";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(16, 26);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(208, 35);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始功率测试(&B)";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.btnReset);
            this.groupControl.Controls.Add(this.btnStart);
            this.groupControl.Location = new System.Drawing.Point(16, 136);
            this.groupControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl.Name = "groupControl";
            this.groupControl.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl.Size = new System.Drawing.Size(240, 121);
            this.groupControl.TabIndex = 7;
            this.groupControl.TabStop = false;
            this.groupControl.Text = "控制";
            // 
            // DynoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 596);
            this.Controls.Add(this.groupSetup);
            this.Controls.Add(this.groupExport);
            this.Controls.Add(this.groupGraph);
            this.Controls.Add(this.groupChart);
            this.Controls.Add(this.groupControl);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DynoForm";
            this.Text = "DynoForm";
            this.VisibleChanged += new System.EventHandler(this.DynoForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numToRPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromRPM)).EndInit();
            this.groupSetup.ResumeLayout(false);
            this.groupExport.ResumeLayout(false);
            this.groupGraph.ResumeLayout(false);
            this.groupChart.ResumeLayout(false);
            this.groupControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numToRPM;
        private System.Windows.Forms.NumericUpDown numFromRPM;
        private System.Windows.Forms.Label lblFromRPM;
        private System.Windows.Forms.GroupBox groupSetup;
        private System.Windows.Forms.Label lblToRPM;
        private System.Windows.Forms.Button btnExportJPEG;
        private System.Windows.Forms.GroupBox groupExport;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.PrintDialog printDialog;
        private DGChart.DynoControl dyno;
        private System.Windows.Forms.GroupBox groupGraph;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox groupChart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupControl;
    }
}