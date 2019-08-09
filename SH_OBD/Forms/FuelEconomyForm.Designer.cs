namespace SH_OBD {
    partial class FuelEconomyForm {
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
            this.groupControl = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupSetup = new System.Windows.Forms.GroupBox();
            this.labelFuelUnit = new System.Windows.Forms.Label();
            this.numericFuelCost = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.radioMetricUnits = new System.Windows.Forms.RadioButton();
            this.radioEnglishUnits = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sensorTotalCost = new SH_OBD.SensorDisplayControl();
            this.sensorTotalConsumed = new SH_OBD.SensorDisplayControl();
            this.sensorCostPerMile = new SH_OBD.SensorDisplayControl();
            this.sensorDistance = new SH_OBD.SensorDisplayControl();
            this.sensorAvgFuelEconomy = new SH_OBD.SensorDisplayControl();
            this.sensorAvgFuelConsumption = new SH_OBD.SensorDisplayControl();
            this.sensorInstantFuelEconomy = new SH_OBD.SensorDisplayControl();
            this.sensorInstantFuelConsumption = new SH_OBD.SensorDisplayControl();
            this.groupControl.SuspendLayout();
            this.groupSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericFuelCost)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl.Controls.Add(this.btnStop);
            this.groupControl.Controls.Add(this.btnStart);
            this.groupControl.Location = new System.Drawing.Point(565, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(223, 89);
            this.groupControl.TabIndex = 20;
            this.groupControl.TabStop = false;
            this.groupControl.Text = "控制";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(66, 51);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(96, 26);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "结束(&t)";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(66, 17);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 25);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开始(&S)";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupSetup
            // 
            this.groupSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSetup.Controls.Add(this.labelFuelUnit);
            this.groupSetup.Controls.Add(this.numericFuelCost);
            this.groupSetup.Controls.Add(this.label1);
            this.groupSetup.Controls.Add(this.radioMetricUnits);
            this.groupSetup.Controls.Add(this.radioEnglishUnits);
            this.groupSetup.Location = new System.Drawing.Point(12, 12);
            this.groupSetup.Name = "groupSetup";
            this.groupSetup.Size = new System.Drawing.Size(545, 89);
            this.groupSetup.TabIndex = 19;
            this.groupSetup.TabStop = false;
            this.groupSetup.Text = "设置";
            // 
            // labelFuelUnit
            // 
            this.labelFuelUnit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFuelUnit.Location = new System.Drawing.Point(368, 48);
            this.labelFuelUnit.Name = "labelFuelUnit";
            this.labelFuelUnit.Size = new System.Drawing.Size(58, 26);
            this.labelFuelUnit.TabIndex = 5;
            this.labelFuelUnit.Text = "/ 加仑";
            this.labelFuelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericFuelCost
            // 
            this.numericFuelCost.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericFuelCost.DecimalPlaces = 2;
            this.numericFuelCost.Location = new System.Drawing.Point(285, 51);
            this.numericFuelCost.Name = "numericFuelCost";
            this.numericFuelCost.Size = new System.Drawing.Size(77, 21);
            this.numericFuelCost.TabIndex = 4;
            this.numericFuelCost.Value = new decimal(new int[] {
            350,
            0,
            0,
            131072});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Location = new System.Drawing.Point(285, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "燃油成本(&C):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioMetricUnits
            // 
            this.radioMetricUnits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioMetricUnits.Location = new System.Drawing.Point(132, 51);
            this.radioMetricUnits.Name = "radioMetricUnits";
            this.radioMetricUnits.Size = new System.Drawing.Size(124, 26);
            this.radioMetricUnits.TabIndex = 2;
            this.radioMetricUnits.Text = "公制单位(&M)";
            this.radioMetricUnits.CheckedChanged += new System.EventHandler(this.radioEnglishUnits_CheckedChanged);
            // 
            // radioEnglishUnits
            // 
            this.radioEnglishUnits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioEnglishUnits.Checked = true;
            this.radioEnglishUnits.Location = new System.Drawing.Point(132, 17);
            this.radioEnglishUnits.Name = "radioEnglishUnits";
            this.radioEnglishUnits.Size = new System.Drawing.Size(124, 26);
            this.radioEnglishUnits.TabIndex = 1;
            this.radioEnglishUnits.TabStop = true;
            this.radioEnglishUnits.Text = "英制单位(&E)";
            this.radioEnglishUnits.CheckedChanged += new System.EventHandler(this.radioEnglishUnits_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.sensorTotalCost);
            this.panel1.Controls.Add(this.sensorTotalConsumed);
            this.panel1.Controls.Add(this.sensorCostPerMile);
            this.panel1.Controls.Add(this.sensorDistance);
            this.panel1.Controls.Add(this.sensorAvgFuelEconomy);
            this.panel1.Controls.Add(this.sensorAvgFuelConsumption);
            this.panel1.Controls.Add(this.sensorInstantFuelEconomy);
            this.panel1.Controls.Add(this.sensorInstantFuelConsumption);
            this.panel1.Location = new System.Drawing.Point(14, 109);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 329);
            this.panel1.TabIndex = 21;
            // 
            // sensorTotalCost
            // 
            this.sensorTotalCost.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorTotalCost.EnglishDisplay = null;
            this.sensorTotalCost.Location = new System.Drawing.Point(389, 242);
            this.sensorTotalCost.MetricDisplay = null;
            this.sensorTotalCost.Name = "sensorTotalCost";
            this.sensorTotalCost.Size = new System.Drawing.Size(370, 70);
            this.sensorTotalCost.TabIndex = 7;
            this.sensorTotalCost.Title = "总行程成本";
            // 
            // sensorTotalConsumed
            // 
            this.sensorTotalConsumed.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorTotalConsumed.EnglishDisplay = null;
            this.sensorTotalConsumed.Location = new System.Drawing.Point(13, 242);
            this.sensorTotalConsumed.MetricDisplay = null;
            this.sensorTotalConsumed.Name = "sensorTotalConsumed";
            this.sensorTotalConsumed.Size = new System.Drawing.Size(370, 70);
            this.sensorTotalConsumed.TabIndex = 6;
            this.sensorTotalConsumed.Title = "总油耗";
            // 
            // sensorCostPerMile
            // 
            this.sensorCostPerMile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorCostPerMile.EnglishDisplay = null;
            this.sensorCostPerMile.Location = new System.Drawing.Point(389, 166);
            this.sensorCostPerMile.MetricDisplay = null;
            this.sensorCostPerMile.Name = "sensorCostPerMile";
            this.sensorCostPerMile.Size = new System.Drawing.Size(370, 70);
            this.sensorCostPerMile.TabIndex = 5;
            this.sensorCostPerMile.Title = "每英里的平均成本";
            // 
            // sensorDistance
            // 
            this.sensorDistance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorDistance.EnglishDisplay = null;
            this.sensorDistance.Location = new System.Drawing.Point(13, 166);
            this.sensorDistance.MetricDisplay = null;
            this.sensorDistance.Name = "sensorDistance";
            this.sensorDistance.Size = new System.Drawing.Size(370, 70);
            this.sensorDistance.TabIndex = 4;
            this.sensorDistance.Title = "行驶距离";
            // 
            // sensorAvgFuelEconomy
            // 
            this.sensorAvgFuelEconomy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorAvgFuelEconomy.EnglishDisplay = null;
            this.sensorAvgFuelEconomy.Location = new System.Drawing.Point(389, 90);
            this.sensorAvgFuelEconomy.MetricDisplay = null;
            this.sensorAvgFuelEconomy.Name = "sensorAvgFuelEconomy";
            this.sensorAvgFuelEconomy.Size = new System.Drawing.Size(370, 70);
            this.sensorAvgFuelEconomy.TabIndex = 3;
            this.sensorAvgFuelEconomy.Title = "平均燃油经济性";
            // 
            // sensorAvgFuelConsumption
            // 
            this.sensorAvgFuelConsumption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorAvgFuelConsumption.EnglishDisplay = null;
            this.sensorAvgFuelConsumption.Location = new System.Drawing.Point(13, 90);
            this.sensorAvgFuelConsumption.MetricDisplay = null;
            this.sensorAvgFuelConsumption.Name = "sensorAvgFuelConsumption";
            this.sensorAvgFuelConsumption.Size = new System.Drawing.Size(370, 70);
            this.sensorAvgFuelConsumption.TabIndex = 2;
            this.sensorAvgFuelConsumption.Title = "平均油耗";
            // 
            // sensorInstantFuelEconomy
            // 
            this.sensorInstantFuelEconomy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorInstantFuelEconomy.EnglishDisplay = null;
            this.sensorInstantFuelEconomy.Location = new System.Drawing.Point(389, 14);
            this.sensorInstantFuelEconomy.MetricDisplay = null;
            this.sensorInstantFuelEconomy.Name = "sensorInstantFuelEconomy";
            this.sensorInstantFuelEconomy.Size = new System.Drawing.Size(370, 70);
            this.sensorInstantFuelEconomy.TabIndex = 1;
            this.sensorInstantFuelEconomy.Title = "瞬时燃油经济性";
            // 
            // sensorInstantFuelConsumption
            // 
            this.sensorInstantFuelConsumption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sensorInstantFuelConsumption.EnglishDisplay = null;
            this.sensorInstantFuelConsumption.Location = new System.Drawing.Point(13, 14);
            this.sensorInstantFuelConsumption.MetricDisplay = null;
            this.sensorInstantFuelConsumption.Name = "sensorInstantFuelConsumption";
            this.sensorInstantFuelConsumption.Size = new System.Drawing.Size(370, 70);
            this.sensorInstantFuelConsumption.TabIndex = 0;
            this.sensorInstantFuelConsumption.Title = "瞬时油耗";
            // 
            // FuelEconomyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.groupSetup);
            this.Controls.Add(this.panel1);
            this.Name = "FuelEconomyForm";
            this.Text = "FuelEconomyForm";
            this.Load += new System.EventHandler(this.FuelEconomyForm_Load);
            this.groupControl.ResumeLayout(false);
            this.groupSetup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericFuelCost)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupControl;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupSetup;
        private System.Windows.Forms.Label labelFuelUnit;
        private System.Windows.Forms.NumericUpDown numericFuelCost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioMetricUnits;
        private System.Windows.Forms.RadioButton radioEnglishUnits;
        private System.Windows.Forms.Panel panel1;
        private SensorDisplayControl sensorTotalCost;
        private SensorDisplayControl sensorTotalConsumed;
        private SensorDisplayControl sensorCostPerMile;
        private SensorDisplayControl sensorDistance;
        private SensorDisplayControl sensorAvgFuelEconomy;
        private SensorDisplayControl sensorAvgFuelConsumption;
        private SensorDisplayControl sensorInstantFuelEconomy;
        private SensorDisplayControl sensorInstantFuelConsumption;
    }
}