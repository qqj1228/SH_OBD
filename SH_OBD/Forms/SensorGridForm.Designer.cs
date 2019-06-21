namespace SH_OBD {
    partial class SensorGridForm {
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
            this.listSensors = new System.Windows.Forms.CheckedListBox();
            this.groupDisplay = new System.Windows.Forms.GroupBox();
            this.radioDisplayBoth = new System.Windows.Forms.RadioButton();
            this.radioDisplayMetric = new System.Windows.Forms.RadioButton();
            this.radioDisplayEnglish = new System.Windows.Forms.RadioButton();
            this.groupSelections = new System.Windows.Forms.GroupBox();
            this.groupLogging = new System.Windows.Forms.GroupBox();
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.scrollTime = new System.Windows.Forms.HScrollBar();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.groupDisplay.SuspendLayout();
            this.groupSelections.SuspendLayout();
            this.groupLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // listSensors
            // 
            this.listSensors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSensors.CheckOnClick = true;
            this.listSensors.Location = new System.Drawing.Point(14, 27);
            this.listSensors.Name = "listSensors";
            this.listSensors.Size = new System.Drawing.Size(376, 68);
            this.listSensors.TabIndex = 0;
            this.listSensors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listSensors_ItemCheck);
            // 
            // groupDisplay
            // 
            this.groupDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDisplay.Controls.Add(this.radioDisplayBoth);
            this.groupDisplay.Controls.Add(this.radioDisplayMetric);
            this.groupDisplay.Controls.Add(this.radioDisplayEnglish);
            this.groupDisplay.Location = new System.Drawing.Point(423, 12);
            this.groupDisplay.Name = "groupDisplay";
            this.groupDisplay.Size = new System.Drawing.Size(100, 113);
            this.groupDisplay.TabIndex = 5;
            this.groupDisplay.TabStop = false;
            this.groupDisplay.Text = "单位(&U)";
            // 
            // radioDisplayBoth
            // 
            this.radioDisplayBoth.Enabled = false;
            this.radioDisplayBoth.Location = new System.Drawing.Point(19, 77);
            this.radioDisplayBoth.Name = "radioDisplayBoth";
            this.radioDisplayBoth.Size = new System.Drawing.Size(71, 22);
            this.radioDisplayBoth.TabIndex = 2;
            this.radioDisplayBoth.Text = "同时(&B)";
            this.radioDisplayBoth.Visible = false;
            this.radioDisplayBoth.Click += new System.EventHandler(this.radioDisplayBoth_Click);
            // 
            // radioDisplayMetric
            // 
            this.radioDisplayMetric.Location = new System.Drawing.Point(19, 51);
            this.radioDisplayMetric.Name = "radioDisplayMetric";
            this.radioDisplayMetric.Size = new System.Drawing.Size(71, 22);
            this.radioDisplayMetric.TabIndex = 1;
            this.radioDisplayMetric.Text = "公制(&M)";
            this.radioDisplayMetric.Click += new System.EventHandler(this.radioDisplayMetric_Click);
            // 
            // radioDisplayEnglish
            // 
            this.radioDisplayEnglish.Checked = true;
            this.radioDisplayEnglish.Location = new System.Drawing.Point(19, 26);
            this.radioDisplayEnglish.Name = "radioDisplayEnglish";
            this.radioDisplayEnglish.Size = new System.Drawing.Size(71, 22);
            this.radioDisplayEnglish.TabIndex = 0;
            this.radioDisplayEnglish.TabStop = true;
            this.radioDisplayEnglish.Text = "英制(&n)";
            this.radioDisplayEnglish.Click += new System.EventHandler(this.radioDisplayEnglish_Click);
            // 
            // groupSelections
            // 
            this.groupSelections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSelections.Controls.Add(this.listSensors);
            this.groupSelections.Location = new System.Drawing.Point(12, 12);
            this.groupSelections.Name = "groupSelections";
            this.groupSelections.Size = new System.Drawing.Size(405, 113);
            this.groupSelections.TabIndex = 4;
            this.groupSelections.TabStop = false;
            this.groupSelections.Text = "传感器数据流(&S)";
            // 
            // groupLogging
            // 
            this.groupLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLogging.Controls.Add(this.lblTimeElapsed);
            this.groupLogging.Controls.Add(this.scrollTime);
            this.groupLogging.Controls.Add(this.btnReset);
            this.groupLogging.Controls.Add(this.btnStart);
            this.groupLogging.Controls.Add(this.btnSave);
            this.groupLogging.Location = new System.Drawing.Point(529, 12);
            this.groupLogging.Name = "groupLogging";
            this.groupLogging.Size = new System.Drawing.Size(259, 113);
            this.groupLogging.TabIndex = 6;
            this.groupLogging.TabStop = false;
            this.groupLogging.Text = "控制(&C)";
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.Location = new System.Drawing.Point(20, 21);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(222, 21);
            this.lblTimeElapsed.TabIndex = 0;
            this.lblTimeElapsed.Text = "00:00:00.00";
            this.lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scrollTime
            // 
            this.scrollTime.Location = new System.Drawing.Point(20, 47);
            this.scrollTime.Name = "scrollTime";
            this.scrollTime.Size = new System.Drawing.Size(225, 17);
            this.scrollTime.TabIndex = 4;
            this.scrollTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollTime_Scroll);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(96, 72);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(72, 25);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重启(&R)";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(19, 72);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 25);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始(&t)";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(173, 72);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelDisplay
            // 
            this.panelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDisplay.AutoScroll = true;
            this.panelDisplay.BackColor = System.Drawing.Color.Black;
            this.panelDisplay.Location = new System.Drawing.Point(12, 135);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(776, 303);
            this.panelDisplay.TabIndex = 7;
            // 
            // SensorMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupDisplay);
            this.Controls.Add(this.groupSelections);
            this.Controls.Add(this.groupLogging);
            this.Controls.Add(this.panelDisplay);
            this.Name = "SensorMonitorForm";
            this.Text = "SensorMonitorForm";
            this.Activated += new System.EventHandler(this.SensorMonitorForm_Activated);
            this.Load += new System.EventHandler(this.SensorMonitorForm_Load);
            this.Resize += new System.EventHandler(this.SensorMonitorForm_Resize);
            this.groupDisplay.ResumeLayout(false);
            this.groupSelections.ResumeLayout(false);
            this.groupLogging.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listSensors;
        private System.Windows.Forms.GroupBox groupDisplay;
        private System.Windows.Forms.RadioButton radioDisplayBoth;
        private System.Windows.Forms.RadioButton radioDisplayMetric;
        private System.Windows.Forms.RadioButton radioDisplayEnglish;
        private System.Windows.Forms.GroupBox groupSelections;
        private System.Windows.Forms.GroupBox groupLogging;
        private System.Windows.Forms.Label lblTimeElapsed;
        private System.Windows.Forms.HScrollBar scrollTime;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelDisplay;
    }
}