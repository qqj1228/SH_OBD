namespace SH_OBD {
    partial class ScopeForm {
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblHistory = new System.Windows.Forms.Label();
            this.groupControl = new System.Windows.Forms.GroupBox();
            this.numHistory = new System.Windows.Forms.NumericUpDown();
            this.comboStyle4 = new System.Windows.Forms.ComboBox();
            this.comboStyle3 = new System.Windows.Forms.ComboBox();
            this.comboStyle2 = new System.Windows.Forms.ComboBox();
            this.comboStyle1 = new System.Windows.Forms.ComboBox();
            this.comboUnits4 = new System.Windows.Forms.ComboBox();
            this.comboUnits3 = new System.Windows.Forms.ComboBox();
            this.comboUnits2 = new System.Windows.Forms.ComboBox();
            this.comboUnits1 = new System.Windows.Forms.ComboBox();
            this.chkSensor4 = new System.Windows.Forms.CheckBox();
            this.chkSensor3 = new System.Windows.Forms.CheckBox();
            this.chkSensor2 = new System.Windows.Forms.CheckBox();
            this.chkSensor1 = new System.Windows.Forms.CheckBox();
            this.comboSensor4 = new System.Windows.Forms.ComboBox();
            this.comboSensor3 = new System.Windows.Forms.ComboBox();
            this.comboSensor2 = new System.Windows.Forms.ComboBox();
            this.comboSensor1 = new System.Windows.Forms.ComboBox();
            this.groupSetup = new System.Windows.Forms.GroupBox();
            this.chart3 = new DGChart.DGChartControl();
            this.chart2 = new DGChart.DGChartControl();
            this.chart1 = new DGChart.DGChartControl();
            this.chart4 = new DGChart.DGChartControl();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHistory)).BeginInit();
            this.groupSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(14, 92);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(110, 22);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "结束(&t)";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(14, 69);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 22);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "开始(&S)";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblHistory
            // 
            this.lblHistory.Location = new System.Drawing.Point(9, 21);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(120, 22);
            this.lblHistory.TabIndex = 0;
            this.lblHistory.Text = "历史显示(&H) (秒):";
            this.lblHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupControl
            // 
            this.groupControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl.Controls.Add(this.btnStop);
            this.groupControl.Controls.Add(this.btnStart);
            this.groupControl.Controls.Add(this.numHistory);
            this.groupControl.Controls.Add(this.lblHistory);
            this.groupControl.Location = new System.Drawing.Point(636, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(140, 130);
            this.groupControl.TabIndex = 13;
            this.groupControl.TabStop = false;
            this.groupControl.Text = "控制";
            // 
            // numHistory
            // 
            this.numHistory.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numHistory.Location = new System.Drawing.Point(14, 45);
            this.numHistory.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numHistory.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numHistory.Name = "numHistory";
            this.numHistory.ReadOnly = true;
            this.numHistory.Size = new System.Drawing.Size(110, 21);
            this.numHistory.TabIndex = 1;
            this.numHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numHistory.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numHistory.ValueChanged += new System.EventHandler(this.numHistory_ValueChanged);
            // 
            // comboStyle4
            // 
            this.comboStyle4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboStyle4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle4.Enabled = false;
            this.comboStyle4.Items.AddRange(new object[] {
            "Line",
            "Dot",
            "Bar"});
            this.comboStyle4.Location = new System.Drawing.Point(511, 94);
            this.comboStyle4.Name = "comboStyle4";
            this.comboStyle4.Size = new System.Drawing.Size(90, 20);
            this.comboStyle4.TabIndex = 20;
            this.comboStyle4.SelectedIndexChanged += new System.EventHandler(this.comboStyle4_SelectedIndexChanged);
            // 
            // comboStyle3
            // 
            this.comboStyle3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboStyle3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle3.Enabled = false;
            this.comboStyle3.Items.AddRange(new object[] {
            "Line",
            "Dot",
            "Bar"});
            this.comboStyle3.Location = new System.Drawing.Point(511, 70);
            this.comboStyle3.Name = "comboStyle3";
            this.comboStyle3.Size = new System.Drawing.Size(90, 20);
            this.comboStyle3.TabIndex = 19;
            this.comboStyle3.SelectedIndexChanged += new System.EventHandler(this.comboStyle3_SelectedIndexChanged);
            // 
            // comboStyle2
            // 
            this.comboStyle2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboStyle2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle2.Enabled = false;
            this.comboStyle2.Items.AddRange(new object[] {
            "Line",
            "Dot",
            "Bar"});
            this.comboStyle2.Location = new System.Drawing.Point(511, 46);
            this.comboStyle2.Name = "comboStyle2";
            this.comboStyle2.Size = new System.Drawing.Size(90, 20);
            this.comboStyle2.TabIndex = 18;
            this.comboStyle2.SelectedIndexChanged += new System.EventHandler(this.comboStyle2_SelectedIndexChanged);
            // 
            // comboStyle1
            // 
            this.comboStyle1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboStyle1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle1.Enabled = false;
            this.comboStyle1.Items.AddRange(new object[] {
            "Line",
            "Dot",
            "Bar"});
            this.comboStyle1.Location = new System.Drawing.Point(511, 22);
            this.comboStyle1.Name = "comboStyle1";
            this.comboStyle1.Size = new System.Drawing.Size(90, 20);
            this.comboStyle1.TabIndex = 17;
            this.comboStyle1.SelectedIndexChanged += new System.EventHandler(this.comboStyle1_SelectedIndexChanged);
            // 
            // comboUnits4
            // 
            this.comboUnits4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboUnits4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUnits4.Enabled = false;
            this.comboUnits4.Items.AddRange(new object[] {
            "English",
            "Metric"});
            this.comboUnits4.Location = new System.Drawing.Point(385, 94);
            this.comboUnits4.Name = "comboUnits4";
            this.comboUnits4.Size = new System.Drawing.Size(120, 20);
            this.comboUnits4.TabIndex = 16;
            this.comboUnits4.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits4_SelectedIndexChanged);
            // 
            // comboUnits3
            // 
            this.comboUnits3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboUnits3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUnits3.Enabled = false;
            this.comboUnits3.Items.AddRange(new object[] {
            "English",
            "Metric"});
            this.comboUnits3.Location = new System.Drawing.Point(385, 70);
            this.comboUnits3.Name = "comboUnits3";
            this.comboUnits3.Size = new System.Drawing.Size(120, 20);
            this.comboUnits3.TabIndex = 15;
            this.comboUnits3.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits3_SelectedIndexChanged);
            // 
            // comboUnits2
            // 
            this.comboUnits2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboUnits2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUnits2.Enabled = false;
            this.comboUnits2.Items.AddRange(new object[] {
            "English",
            "Metric"});
            this.comboUnits2.Location = new System.Drawing.Point(385, 46);
            this.comboUnits2.Name = "comboUnits2";
            this.comboUnits2.Size = new System.Drawing.Size(120, 20);
            this.comboUnits2.TabIndex = 14;
            this.comboUnits2.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits2_SelectedIndexChanged);
            // 
            // comboUnits1
            // 
            this.comboUnits1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboUnits1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUnits1.Enabled = false;
            this.comboUnits1.Items.AddRange(new object[] {
            "English",
            "Metric"});
            this.comboUnits1.Location = new System.Drawing.Point(385, 22);
            this.comboUnits1.Name = "comboUnits1";
            this.comboUnits1.Size = new System.Drawing.Size(120, 20);
            this.comboUnits1.TabIndex = 13;
            this.comboUnits1.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits1_SelectedIndexChanged);
            // 
            // chkSensor4
            // 
            this.chkSensor4.Enabled = false;
            this.chkSensor4.Location = new System.Drawing.Point(12, 92);
            this.chkSensor4.Name = "chkSensor4";
            this.chkSensor4.Size = new System.Drawing.Size(90, 22);
            this.chkSensor4.TabIndex = 12;
            this.chkSensor4.Text = "传感器 &4:";
            this.chkSensor4.CheckedChanged += new System.EventHandler(this.chkSensor4_CheckedChanged);
            this.chkSensor4.EnabledChanged += new System.EventHandler(this.chkSensor4_EnabledChanged);
            // 
            // chkSensor3
            // 
            this.chkSensor3.Enabled = false;
            this.chkSensor3.Location = new System.Drawing.Point(12, 69);
            this.chkSensor3.Name = "chkSensor3";
            this.chkSensor3.Size = new System.Drawing.Size(90, 22);
            this.chkSensor3.TabIndex = 11;
            this.chkSensor3.Text = "传感器 &3:";
            this.chkSensor3.CheckedChanged += new System.EventHandler(this.chkSensor3_CheckedChanged);
            this.chkSensor3.EnabledChanged += new System.EventHandler(this.chkSensor3_EnabledChanged);
            // 
            // chkSensor2
            // 
            this.chkSensor2.Enabled = false;
            this.chkSensor2.Location = new System.Drawing.Point(12, 45);
            this.chkSensor2.Name = "chkSensor2";
            this.chkSensor2.Size = new System.Drawing.Size(90, 22);
            this.chkSensor2.TabIndex = 10;
            this.chkSensor2.Text = "传感器 &2:";
            this.chkSensor2.CheckedChanged += new System.EventHandler(this.chkSensor2_CheckedChanged);
            this.chkSensor2.EnabledChanged += new System.EventHandler(this.chkSensor2_EnabledChanged);
            // 
            // chkSensor1
            // 
            this.chkSensor1.Location = new System.Drawing.Point(12, 21);
            this.chkSensor1.Name = "chkSensor1";
            this.chkSensor1.Size = new System.Drawing.Size(90, 22);
            this.chkSensor1.TabIndex = 9;
            this.chkSensor1.Text = "传感器 &1:";
            this.chkSensor1.CheckedChanged += new System.EventHandler(this.chkSensor1_CheckedChanged);
            this.chkSensor1.EnabledChanged += new System.EventHandler(this.chkSensor1_EnabledChanged);
            // 
            // comboSensor4
            // 
            this.comboSensor4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSensor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensor4.Enabled = false;
            this.comboSensor4.Location = new System.Drawing.Point(108, 94);
            this.comboSensor4.Name = "comboSensor4";
            this.comboSensor4.Size = new System.Drawing.Size(271, 20);
            this.comboSensor4.TabIndex = 7;
            this.comboSensor4.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits4_SelectedIndexChanged);
            // 
            // comboSensor3
            // 
            this.comboSensor3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSensor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensor3.Enabled = false;
            this.comboSensor3.Location = new System.Drawing.Point(108, 70);
            this.comboSensor3.Name = "comboSensor3";
            this.comboSensor3.Size = new System.Drawing.Size(271, 20);
            this.comboSensor3.TabIndex = 5;
            this.comboSensor3.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits3_SelectedIndexChanged);
            // 
            // comboSensor2
            // 
            this.comboSensor2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSensor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensor2.Enabled = false;
            this.comboSensor2.Location = new System.Drawing.Point(108, 46);
            this.comboSensor2.Name = "comboSensor2";
            this.comboSensor2.Size = new System.Drawing.Size(271, 20);
            this.comboSensor2.TabIndex = 3;
            this.comboSensor2.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits2_SelectedIndexChanged);
            // 
            // comboSensor1
            // 
            this.comboSensor1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSensor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensor1.Enabled = false;
            this.comboSensor1.Location = new System.Drawing.Point(108, 22);
            this.comboSensor1.Name = "comboSensor1";
            this.comboSensor1.Size = new System.Drawing.Size(271, 20);
            this.comboSensor1.TabIndex = 1;
            this.comboSensor1.SelectedIndexChanged += new System.EventHandler(this.comboSensorOrUnits1_SelectedIndexChanged);
            // 
            // groupSetup
            // 
            this.groupSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSetup.Controls.Add(this.comboStyle4);
            this.groupSetup.Controls.Add(this.comboStyle3);
            this.groupSetup.Controls.Add(this.comboStyle2);
            this.groupSetup.Controls.Add(this.comboStyle1);
            this.groupSetup.Controls.Add(this.comboUnits4);
            this.groupSetup.Controls.Add(this.comboUnits3);
            this.groupSetup.Controls.Add(this.comboUnits2);
            this.groupSetup.Controls.Add(this.comboUnits1);
            this.groupSetup.Controls.Add(this.chkSensor4);
            this.groupSetup.Controls.Add(this.chkSensor3);
            this.groupSetup.Controls.Add(this.chkSensor2);
            this.groupSetup.Controls.Add(this.chkSensor1);
            this.groupSetup.Controls.Add(this.comboSensor4);
            this.groupSetup.Controls.Add(this.comboSensor3);
            this.groupSetup.Controls.Add(this.comboSensor2);
            this.groupSetup.Controls.Add(this.comboSensor1);
            this.groupSetup.Location = new System.Drawing.Point(12, 12);
            this.groupSetup.Name = "groupSetup";
            this.groupSetup.Size = new System.Drawing.Size(618, 130);
            this.groupSetup.TabIndex = 12;
            this.groupSetup.TabStop = false;
            this.groupSetup.Text = "设置";
            // 
            // chart3
            // 
            this.chart3.BackColor = System.Drawing.SystemColors.Control;
            this.chart3.BorderBottom = 25;
            this.chart3.BorderTop = 20;
            this.chart3.ColorAxis = System.Drawing.Color.Black;
            this.chart3.ColorBg = System.Drawing.SystemColors.Control;
            this.chart3.ColorGrid = System.Drawing.Color.Gray;
            this.chart3.ColorSet1 = System.Drawing.Color.Lime;
            this.chart3.ColorSet2 = System.Drawing.Color.Red;
            this.chart3.ColorSet3 = System.Drawing.Color.DarkBlue;
            this.chart3.ColorSet4 = System.Drawing.Color.Gold;
            this.chart3.ColorSet5 = System.Drawing.Color.Magenta;
            this.chart3.DrawMode = DGChart.DGChartControl.DrawModeType.Line;
            this.chart3.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.chart3.Location = new System.Drawing.Point(12, 326);
            this.chart3.Name = "chart3";
            this.chart3.ShowData1 = true;
            this.chart3.ShowData2 = false;
            this.chart3.ShowData3 = false;
            this.chart3.ShowData4 = false;
            this.chart3.ShowData5 = false;
            this.chart3.Size = new System.Drawing.Size(379, 173);
            this.chart3.TabIndex = 10;
            this.chart3.XData1 = null;
            this.chart3.XData2 = null;
            this.chart3.XData3 = null;
            this.chart3.XData4 = null;
            this.chart3.XData5 = null;
            this.chart3.XGrid = 10D;
            this.chart3.XLabel = "0";
            this.chart3.XRangeEnd = 30D;
            this.chart3.XRangeStart = 0D;
            this.chart3.YData1 = null;
            this.chart3.YData2 = null;
            this.chart3.YData3 = null;
            this.chart3.YData4 = null;
            this.chart3.YData5 = null;
            this.chart3.YGrid = 20D;
            this.chart3.YLabel = "0";
            this.chart3.YRangeEnd = 100D;
            this.chart3.YRangeStart = 0D;
            // 
            // chart2
            // 
            this.chart2.BackColor = System.Drawing.SystemColors.Control;
            this.chart2.BorderBottom = 25;
            this.chart2.BorderTop = 20;
            this.chart2.ColorAxis = System.Drawing.Color.Black;
            this.chart2.ColorBg = System.Drawing.SystemColors.Control;
            this.chart2.ColorGrid = System.Drawing.Color.Gray;
            this.chart2.ColorSet1 = System.Drawing.Color.Red;
            this.chart2.ColorSet2 = System.Drawing.Color.DarkBlue;
            this.chart2.ColorSet3 = System.Drawing.Color.Lime;
            this.chart2.ColorSet4 = System.Drawing.Color.Gold;
            this.chart2.ColorSet5 = System.Drawing.Color.Magenta;
            this.chart2.DrawMode = DGChart.DGChartControl.DrawModeType.Line;
            this.chart2.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.chart2.Location = new System.Drawing.Point(397, 148);
            this.chart2.Name = "chart2";
            this.chart2.ShowData1 = true;
            this.chart2.ShowData2 = false;
            this.chart2.ShowData3 = false;
            this.chart2.ShowData4 = false;
            this.chart2.ShowData5 = false;
            this.chart2.Size = new System.Drawing.Size(379, 172);
            this.chart2.TabIndex = 9;
            this.chart2.XData1 = null;
            this.chart2.XData2 = null;
            this.chart2.XData3 = null;
            this.chart2.XData4 = null;
            this.chart2.XData5 = null;
            this.chart2.XGrid = 10D;
            this.chart2.XLabel = "0";
            this.chart2.XRangeEnd = 30D;
            this.chart2.XRangeStart = 0D;
            this.chart2.YData1 = null;
            this.chart2.YData2 = null;
            this.chart2.YData3 = null;
            this.chart2.YData4 = null;
            this.chart2.YData5 = null;
            this.chart2.YGrid = 20D;
            this.chart2.YLabel = "0";
            this.chart2.YRangeEnd = 100D;
            this.chart2.YRangeStart = 0D;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.SystemColors.Control;
            this.chart1.BorderBottom = 25;
            this.chart1.BorderTop = 20;
            this.chart1.ColorAxis = System.Drawing.Color.Black;
            this.chart1.ColorBg = System.Drawing.SystemColors.Control;
            this.chart1.ColorGrid = System.Drawing.Color.Gray;
            this.chart1.ColorSet1 = System.Drawing.Color.DarkBlue;
            this.chart1.ColorSet2 = System.Drawing.Color.Red;
            this.chart1.ColorSet3 = System.Drawing.Color.Lime;
            this.chart1.ColorSet4 = System.Drawing.Color.Gold;
            this.chart1.ColorSet5 = System.Drawing.Color.Magenta;
            this.chart1.DrawMode = DGChart.DGChartControl.DrawModeType.Line;
            this.chart1.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.chart1.Location = new System.Drawing.Point(12, 148);
            this.chart1.Name = "chart1";
            this.chart1.ShowData1 = true;
            this.chart1.ShowData2 = false;
            this.chart1.ShowData3 = false;
            this.chart1.ShowData4 = false;
            this.chart1.ShowData5 = false;
            this.chart1.Size = new System.Drawing.Size(379, 172);
            this.chart1.TabIndex = 8;
            this.chart1.XData1 = null;
            this.chart1.XData2 = null;
            this.chart1.XData3 = null;
            this.chart1.XData4 = null;
            this.chart1.XData5 = null;
            this.chart1.XGrid = 10D;
            this.chart1.XLabel = "0";
            this.chart1.XRangeEnd = 30D;
            this.chart1.XRangeStart = 0D;
            this.chart1.YData1 = null;
            this.chart1.YData2 = null;
            this.chart1.YData3 = null;
            this.chart1.YData4 = null;
            this.chart1.YData5 = null;
            this.chart1.YGrid = 20D;
            this.chart1.YLabel = "0";
            this.chart1.YRangeEnd = 100D;
            this.chart1.YRangeStart = 0D;
            // 
            // chart4
            // 
            this.chart4.BackColor = System.Drawing.SystemColors.Control;
            this.chart4.BorderBottom = 25;
            this.chart4.BorderTop = 20;
            this.chart4.ColorAxis = System.Drawing.Color.Black;
            this.chart4.ColorBg = System.Drawing.SystemColors.Control;
            this.chart4.ColorGrid = System.Drawing.Color.Gray;
            this.chart4.ColorSet1 = System.Drawing.Color.Magenta;
            this.chart4.ColorSet2 = System.Drawing.Color.Red;
            this.chart4.ColorSet3 = System.Drawing.Color.Lime;
            this.chart4.ColorSet4 = System.Drawing.Color.Gold;
            this.chart4.ColorSet5 = System.Drawing.Color.DarkBlue;
            this.chart4.DrawMode = DGChart.DGChartControl.DrawModeType.Line;
            this.chart4.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.chart4.Location = new System.Drawing.Point(397, 326);
            this.chart4.Name = "chart4";
            this.chart4.ShowData1 = true;
            this.chart4.ShowData2 = false;
            this.chart4.ShowData3 = false;
            this.chart4.ShowData4 = false;
            this.chart4.ShowData5 = false;
            this.chart4.Size = new System.Drawing.Size(379, 173);
            this.chart4.TabIndex = 11;
            this.chart4.XData1 = null;
            this.chart4.XData2 = null;
            this.chart4.XData3 = null;
            this.chart4.XData4 = null;
            this.chart4.XData5 = null;
            this.chart4.XGrid = 10D;
            this.chart4.XLabel = "0";
            this.chart4.XRangeEnd = 30D;
            this.chart4.XRangeStart = 0D;
            this.chart4.YData1 = null;
            this.chart4.YData2 = null;
            this.chart4.YData3 = null;
            this.chart4.YData4 = null;
            this.chart4.YData5 = null;
            this.chart4.YGrid = 20D;
            this.chart4.YLabel = "0";
            this.chart4.YRangeEnd = 100D;
            this.chart4.YRangeStart = 0D;
            // 
            // ScopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 524);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.chart4);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.groupSetup);
            this.Controls.Add(this.chart1);
            this.Name = "ScopeForm";
            this.Text = "ScopeForm";
            this.Activated += new System.EventHandler(this.ScopeForm_Activated);
            this.Load += new System.EventHandler(this.ScopeForm_Load);
            this.Resize += new System.EventHandler(this.ScopeForm_Resize);
            this.groupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numHistory)).EndInit();
            this.groupSetup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.GroupBox groupControl;
        private System.Windows.Forms.NumericUpDown numHistory;
        private System.Windows.Forms.ComboBox comboStyle4;
        private System.Windows.Forms.ComboBox comboStyle3;
        private System.Windows.Forms.ComboBox comboStyle2;
        private System.Windows.Forms.ComboBox comboStyle1;
        private System.Windows.Forms.ComboBox comboUnits4;
        private System.Windows.Forms.ComboBox comboUnits3;
        private System.Windows.Forms.ComboBox comboUnits2;
        private System.Windows.Forms.ComboBox comboUnits1;
        private System.Windows.Forms.CheckBox chkSensor4;
        private System.Windows.Forms.CheckBox chkSensor3;
        private System.Windows.Forms.CheckBox chkSensor2;
        private System.Windows.Forms.CheckBox chkSensor1;
        private System.Windows.Forms.ComboBox comboSensor4;
        private System.Windows.Forms.ComboBox comboSensor3;
        private System.Windows.Forms.ComboBox comboSensor2;
        private DGChart.DGChartControl chart3;
        private DGChart.DGChartControl chart2;
        private System.Windows.Forms.ComboBox comboSensor1;
        private System.Windows.Forms.GroupBox groupSetup;
        private DGChart.DGChartControl chart1;
        private DGChart.DGChartControl chart4;
    }
}