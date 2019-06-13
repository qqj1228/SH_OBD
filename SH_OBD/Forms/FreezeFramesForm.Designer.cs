namespace SH_OBD {
    partial class FreezeFramesForm {
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblFrameNumber = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.panel = new System.Windows.Forms.Panel();
            this.freezeFrame = new SH_OBD.FreezeFrameDataControl();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(500, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消(&C)";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 41);
            this.progressBar.Maximum = 12;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(787, 25);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 11;
            // 
            // lblFrameNumber
            // 
            this.lblFrameNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFrameNumber.Location = new System.Drawing.Point(196, 9);
            this.lblFrameNumber.Name = "lblFrameNumber";
            this.lblFrameNumber.Size = new System.Drawing.Size(90, 23);
            this.lblFrameNumber.TabIndex = 9;
            this.lblFrameNumber.Text = "冻结帧编号:";
            this.lblFrameNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.Location = new System.Drawing.Point(404, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "读取(&R)";
            // 
            // numFrame
            // 
            this.numFrame.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numFrame.Location = new System.Drawing.Point(296, 10);
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(96, 21);
            this.numFrame.TabIndex = 8;
            this.numFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.Controls.Add(this.freezeFrame);
            this.panel.Location = new System.Drawing.Point(6, 72);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(787, 377);
            this.panel.TabIndex = 12;
            // 
            // freezeFrame
            // 
            this.freezeFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.freezeFrame.CalculatedLoad = 0D;
            this.freezeFrame.DTC = "-";
            this.freezeFrame.EngineCoolantTemp = 0D;
            this.freezeFrame.EngineRPM = 0D;
            this.freezeFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freezeFrame.FuelSystem1Status = "-";
            this.freezeFrame.FuelSystem2Status = "-";
            this.freezeFrame.IntakePressure = 0D;
            this.freezeFrame.Location = new System.Drawing.Point(0, 0);
            this.freezeFrame.LTFT1 = 0D;
            this.freezeFrame.LTFT2 = 0D;
            this.freezeFrame.LTFT3 = 0D;
            this.freezeFrame.LTFT4 = 0D;
            this.freezeFrame.Name = "freezeFrame";
            this.freezeFrame.Size = new System.Drawing.Size(787, 366);
            this.freezeFrame.SparkAdvance = 0D;
            this.freezeFrame.STFT1 = 0D;
            this.freezeFrame.STFT2 = 0D;
            this.freezeFrame.STFT3 = 0D;
            this.freezeFrame.STFT4 = 0D;
            this.freezeFrame.TabIndex = 0;
            this.freezeFrame.VehicleSpeed = 0D;
            // 
            // FreezeFramesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblFrameNumber);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.numFrame);
            this.Controls.Add(this.panel);
            this.Name = "FreezeFramesForm";
            this.Text = "FreezeFramesForm";
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblFrameNumber;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.NumericUpDown numFrame;
        private System.Windows.Forms.Panel panel;
        private FreezeFrameDataControl freezeFrame;
    }
}