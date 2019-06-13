namespace SH_OBD {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabelConnStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabelVehicle = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabelDeviceName = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabelPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonVehicles = new System.Windows.Forms.Button();
            this.buttonUserPrefs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonReport = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonCommLog = new System.Windows.Forms.Button();
            this.buttonTerminal = new System.Windows.Forms.Button();
            this.buttonFuel = new System.Windows.Forms.Button();
            this.buttonDyno = new System.Windows.Forms.Button();
            this.buttonTrack = new System.Windows.Forms.Button();
            this.buttonSensorGraph = new System.Windows.Forms.Button();
            this.buttonSensorGrid = new System.Windows.Forms.Button();
            this.buttonO2 = new System.Windows.Forms.Button();
            this.buttonFF = new System.Windows.Forms.Button();
            this.buttonDTC = new System.Windows.Forms.Button();
            this.buttonTests = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabelConnStatus,
            this.StatusLabelVehicle,
            this.StatusLabelDeviceName,
            this.StatusLabelPort});
            this.statusStrip1.Location = new System.Drawing.Point(0, 611);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(884, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabelConnStatus
            // 
            this.StatusLabelConnStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabelConnStatus.Name = "StatusLabelConnStatus";
            this.StatusLabelConnStatus.Size = new System.Drawing.Size(217, 21);
            this.StatusLabelConnStatus.Spring = true;
            this.StatusLabelConnStatus.Text = "OBD设备未连接";
            // 
            // StatusLabelVehicle
            // 
            this.StatusLabelVehicle.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabelVehicle.Name = "StatusLabelVehicle";
            this.StatusLabelVehicle.Size = new System.Drawing.Size(217, 21);
            this.StatusLabelVehicle.Spring = true;
            this.StatusLabelVehicle.Text = "车辆配置";
            // 
            // StatusLabelDeviceName
            // 
            this.StatusLabelDeviceName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabelDeviceName.Name = "StatusLabelDeviceName";
            this.StatusLabelDeviceName.Size = new System.Drawing.Size(217, 21);
            this.StatusLabelDeviceName.Spring = true;
            this.StatusLabelDeviceName.Text = "设备名称";
            // 
            // StatusLabelPort
            // 
            this.StatusLabelPort.Name = "StatusLabelPort";
            this.StatusLabelPort.Size = new System.Drawing.Size(217, 21);
            this.StatusLabelPort.Spring = true;
            this.StatusLabelPort.Text = "端口";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonVehicles);
            this.panel1.Controls.Add(this.buttonUserPrefs);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonReport);
            this.panel1.Controls.Add(this.buttonSettings);
            this.panel1.Controls.Add(this.buttonCommLog);
            this.panel1.Controls.Add(this.buttonTerminal);
            this.panel1.Controls.Add(this.buttonFuel);
            this.panel1.Controls.Add(this.buttonDyno);
            this.panel1.Controls.Add(this.buttonTrack);
            this.panel1.Controls.Add(this.buttonSensorGraph);
            this.panel1.Controls.Add(this.buttonSensorGrid);
            this.panel1.Controls.Add(this.buttonO2);
            this.panel1.Controls.Add(this.buttonFF);
            this.panel1.Controls.Add(this.buttonDTC);
            this.panel1.Controls.Add(this.buttonTests);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 611);
            this.panel1.TabIndex = 3;
            // 
            // buttonVehicles
            // 
            this.buttonVehicles.Location = new System.Drawing.Point(5, 423);
            this.buttonVehicles.Name = "buttonVehicles";
            this.buttonVehicles.Size = new System.Drawing.Size(110, 23);
            this.buttonVehicles.TabIndex = 16;
            this.buttonVehicles.Text = "buttonVehicles";
            this.buttonVehicles.UseVisualStyleBackColor = true;
            this.buttonVehicles.Click += new System.EventHandler(this.buttonVehicles_Click);
            // 
            // buttonUserPrefs
            // 
            this.buttonUserPrefs.Location = new System.Drawing.Point(5, 394);
            this.buttonUserPrefs.Name = "buttonUserPrefs";
            this.buttonUserPrefs.Size = new System.Drawing.Size(110, 23);
            this.buttonUserPrefs.TabIndex = 15;
            this.buttonUserPrefs.Text = "buttonUserPrefs";
            this.buttonUserPrefs.UseVisualStyleBackColor = true;
            this.buttonUserPrefs.Click += new System.EventHandler(this.buttonUserPrefs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 379);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "------------------";
            // 
            // buttonReport
            // 
            this.buttonReport.Location = new System.Drawing.Point(5, 295);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(110, 23);
            this.buttonReport.TabIndex = 13;
            this.buttonReport.Text = "buttonReport";
            this.buttonReport.UseVisualStyleBackColor = true;
            this.buttonReport.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(5, 452);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(110, 23);
            this.buttonSettings.TabIndex = 12;
            this.buttonSettings.Text = "buttonSettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonCommLog
            // 
            this.buttonCommLog.Location = new System.Drawing.Point(5, 353);
            this.buttonCommLog.Name = "buttonCommLog";
            this.buttonCommLog.Size = new System.Drawing.Size(110, 23);
            this.buttonCommLog.TabIndex = 11;
            this.buttonCommLog.Text = "buttonCommLog";
            this.buttonCommLog.UseVisualStyleBackColor = true;
            this.buttonCommLog.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonTerminal
            // 
            this.buttonTerminal.Location = new System.Drawing.Point(5, 324);
            this.buttonTerminal.Name = "buttonTerminal";
            this.buttonTerminal.Size = new System.Drawing.Size(110, 23);
            this.buttonTerminal.TabIndex = 10;
            this.buttonTerminal.Text = "buttonTerminal";
            this.buttonTerminal.UseVisualStyleBackColor = true;
            this.buttonTerminal.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonFuel
            // 
            this.buttonFuel.Location = new System.Drawing.Point(5, 266);
            this.buttonFuel.Name = "buttonFuel";
            this.buttonFuel.Size = new System.Drawing.Size(110, 23);
            this.buttonFuel.TabIndex = 9;
            this.buttonFuel.Text = "buttonFuel";
            this.buttonFuel.UseVisualStyleBackColor = true;
            this.buttonFuel.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonDyno
            // 
            this.buttonDyno.Location = new System.Drawing.Point(5, 237);
            this.buttonDyno.Name = "buttonDyno";
            this.buttonDyno.Size = new System.Drawing.Size(110, 23);
            this.buttonDyno.TabIndex = 8;
            this.buttonDyno.Text = "buttonDyno";
            this.buttonDyno.UseVisualStyleBackColor = true;
            this.buttonDyno.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonTrack
            // 
            this.buttonTrack.Location = new System.Drawing.Point(5, 208);
            this.buttonTrack.Name = "buttonTrack";
            this.buttonTrack.Size = new System.Drawing.Size(110, 23);
            this.buttonTrack.TabIndex = 7;
            this.buttonTrack.Text = "buttonTrack";
            this.buttonTrack.UseVisualStyleBackColor = true;
            this.buttonTrack.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonSensorGraph
            // 
            this.buttonSensorGraph.Location = new System.Drawing.Point(5, 179);
            this.buttonSensorGraph.Name = "buttonSensorGraph";
            this.buttonSensorGraph.Size = new System.Drawing.Size(110, 23);
            this.buttonSensorGraph.TabIndex = 6;
            this.buttonSensorGraph.Text = "buttonSensorGraph";
            this.buttonSensorGraph.UseVisualStyleBackColor = true;
            this.buttonSensorGraph.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonSensorGrid
            // 
            this.buttonSensorGrid.Location = new System.Drawing.Point(5, 150);
            this.buttonSensorGrid.Name = "buttonSensorGrid";
            this.buttonSensorGrid.Size = new System.Drawing.Size(110, 23);
            this.buttonSensorGrid.TabIndex = 5;
            this.buttonSensorGrid.Text = "buttonSensorGrid";
            this.buttonSensorGrid.UseVisualStyleBackColor = true;
            this.buttonSensorGrid.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonO2
            // 
            this.buttonO2.Location = new System.Drawing.Point(5, 121);
            this.buttonO2.Name = "buttonO2";
            this.buttonO2.Size = new System.Drawing.Size(110, 23);
            this.buttonO2.TabIndex = 4;
            this.buttonO2.Text = "buttonO2";
            this.buttonO2.UseVisualStyleBackColor = true;
            this.buttonO2.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonFF
            // 
            this.buttonFF.Location = new System.Drawing.Point(5, 92);
            this.buttonFF.Name = "buttonFF";
            this.buttonFF.Size = new System.Drawing.Size(110, 23);
            this.buttonFF.TabIndex = 3;
            this.buttonFF.Text = "buttonFF";
            this.buttonFF.UseVisualStyleBackColor = true;
            this.buttonFF.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonDTC
            // 
            this.buttonDTC.Location = new System.Drawing.Point(5, 62);
            this.buttonDTC.Name = "buttonDTC";
            this.buttonDTC.Size = new System.Drawing.Size(110, 23);
            this.buttonDTC.TabIndex = 2;
            this.buttonDTC.Text = "buttonDTC";
            this.buttonDTC.UseVisualStyleBackColor = true;
            this.buttonDTC.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonTests
            // 
            this.buttonTests.Location = new System.Drawing.Point(5, 32);
            this.buttonTests.Name = "buttonTests";
            this.buttonTests.Size = new System.Drawing.Size(110, 23);
            this.buttonTests.TabIndex = 1;
            this.buttonTests.Text = "buttonTests";
            this.buttonTests.UseVisualStyleBackColor = true;
            this.buttonTests.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(5, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(110, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "buttonStart";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.Button_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(120, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(764, 611);
            this.panel2.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 637);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "SH_OBD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelConnStatus;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelDeviceName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonTests;
        private System.Windows.Forms.Button buttonDTC;
        private System.Windows.Forms.Button buttonFF;
        private System.Windows.Forms.Button buttonO2;
        private System.Windows.Forms.Button buttonSensorGrid;
        private System.Windows.Forms.Button buttonSensorGraph;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonCommLog;
        private System.Windows.Forms.Button buttonTerminal;
        private System.Windows.Forms.Button buttonFuel;
        private System.Windows.Forms.Button buttonDyno;
        private System.Windows.Forms.Button buttonTrack;
        private System.Windows.Forms.Button buttonReport;
        private System.Windows.Forms.Button buttonVehicles;
        private System.Windows.Forms.Button buttonUserPrefs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelVehicle;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelPort;
    }
}

