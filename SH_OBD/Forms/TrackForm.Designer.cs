namespace SH_OBD {
    partial class TrackForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackForm));
            this.btnExportJPEG = new System.Windows.Forms.Button();
            this.groupTimeslipControls = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.groupExport = new System.Windows.Forms.GroupBox();
            this.groupTimeslip = new System.Windows.Forms.GroupBox();
            this.richTextSlip = new System.Windows.Forms.RichTextBox();
            this.groupControls = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStage = new System.Windows.Forms.Button();
            this.timerClock1 = new System.Windows.Forms.Timer(this.components);
            this.picTrack = new System.Windows.Forms.PictureBox();
            this.groupTimeslipControls.SuspendLayout();
            this.groupExport.SuspendLayout();
            this.groupTimeslip.SuspendLayout();
            this.groupControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTrack)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExportJPEG
            // 
            this.btnExportJPEG.Location = new System.Drawing.Point(12, 21);
            this.btnExportJPEG.Name = "btnExportJPEG";
            this.btnExportJPEG.Size = new System.Drawing.Size(156, 28);
            this.btnExportJPEG.TabIndex = 6;
            this.btnExportJPEG.Text = "&JPEG 图片";
            this.btnExportJPEG.Click += new System.EventHandler(this.btnExportJPEG_Click);
            // 
            // groupTimeslipControls
            // 
            this.groupTimeslipControls.Controls.Add(this.btnSave);
            this.groupTimeslipControls.Controls.Add(this.btnOpen);
            this.groupTimeslipControls.Controls.Add(this.btnPrint);
            this.groupTimeslipControls.Location = new System.Drawing.Point(12, 226);
            this.groupTimeslipControls.Name = "groupTimeslipControls";
            this.groupTimeslipControls.Size = new System.Drawing.Size(180, 135);
            this.groupTimeslipControls.TabIndex = 10;
            this.groupTimeslipControls.TabStop = false;
            this.groupTimeslipControls.Text = "耗时";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 21);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(156, 28);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 59);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(156, 27);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开(&O)";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(12, 97);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(156, 27);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument;
            // 
            // groupExport
            // 
            this.groupExport.Controls.Add(this.btnExportJPEG);
            this.groupExport.Location = new System.Drawing.Point(12, 371);
            this.groupExport.Name = "groupExport";
            this.groupExport.Size = new System.Drawing.Size(180, 60);
            this.groupExport.TabIndex = 11;
            this.groupExport.TabStop = false;
            this.groupExport.Text = "输出";
            // 
            // groupTimeslip
            // 
            this.groupTimeslip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTimeslip.Controls.Add(this.richTextSlip);
            this.groupTimeslip.Location = new System.Drawing.Point(205, 119);
            this.groupTimeslip.Name = "groupTimeslip";
            this.groupTimeslip.Size = new System.Drawing.Size(583, 312);
            this.groupTimeslip.TabIndex = 9;
            this.groupTimeslip.TabStop = false;
            // 
            // richTextSlip
            // 
            this.richTextSlip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextSlip.BackColor = System.Drawing.SystemColors.Control;
            this.richTextSlip.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextSlip.Location = new System.Drawing.Point(12, 20);
            this.richTextSlip.Name = "richTextSlip";
            this.richTextSlip.ReadOnly = true;
            this.richTextSlip.Size = new System.Drawing.Size(558, 280);
            this.richTextSlip.TabIndex = 0;
            this.richTextSlip.Text = "";
            // 
            // groupControls
            // 
            this.groupControls.Controls.Add(this.btnReset);
            this.groupControls.Controls.Add(this.btnStage);
            this.groupControls.Location = new System.Drawing.Point(12, 119);
            this.groupControls.Name = "groupControls";
            this.groupControls.Size = new System.Drawing.Size(180, 97);
            this.groupControls.TabIndex = 8;
            this.groupControls.TabStop = false;
            this.groupControls.Text = "控制";
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(12, 59);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(156, 27);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重启(&R)";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStage
            // 
            this.btnStage.Location = new System.Drawing.Point(12, 21);
            this.btnStage.Name = "btnStage";
            this.btnStage.Size = new System.Drawing.Size(156, 28);
            this.btnStage.TabIndex = 1;
            this.btnStage.Text = "上路(&S)";
            this.btnStage.Click += new System.EventHandler(this.btnStage_Click);
            // 
            // timerClock1
            // 
            this.timerClock1.Interval = 51;
            this.timerClock1.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // picTrack
            // 
            this.picTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picTrack.Image = ((System.Drawing.Image)(resources.GetObject("picTrack.Image")));
            this.picTrack.Location = new System.Drawing.Point(0, 0);
            this.picTrack.Name = "picTrack";
            this.picTrack.Size = new System.Drawing.Size(800, 107);
            this.picTrack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrack.TabIndex = 7;
            this.picTrack.TabStop = false;
            this.picTrack.Visible = false;
            // 
            // TrackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupTimeslipControls);
            this.Controls.Add(this.groupExport);
            this.Controls.Add(this.groupTimeslip);
            this.Controls.Add(this.groupControls);
            this.Controls.Add(this.picTrack);
            this.Name = "TrackForm";
            this.Text = "TrackForm";
            this.Activated += new System.EventHandler(this.TrackForm_Activated);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TrackForm_Paint);
            this.Resize += new System.EventHandler(this.TrackForm_Resize);
            this.groupTimeslipControls.ResumeLayout(false);
            this.groupExport.ResumeLayout(false);
            this.groupTimeslip.ResumeLayout(false);
            this.groupControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTrack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExportJPEG;
        private System.Windows.Forms.GroupBox groupTimeslipControls;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.GroupBox groupExport;
        private System.Windows.Forms.GroupBox groupTimeslip;
        private System.Windows.Forms.RichTextBox richTextSlip;
        private System.Windows.Forms.GroupBox groupControls;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStage;
        private System.Windows.Forms.Timer timerClock1;
        private System.Windows.Forms.PictureBox picTrack;
    }
}