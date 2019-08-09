namespace SH_OBD {
    partial class VehicleForm {
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
            this.lblTireAspectUnits = new System.Windows.Forms.Label();
            this.lblExampleEnd = new System.Windows.Forms.Label();
            this.lblExampleDiameter = new System.Windows.Forms.Label();
            this.lblExampleAspect = new System.Windows.Forms.Label();
            this.lblExampleSlash = new System.Windows.Forms.Label();
            this.lblExampleDash = new System.Windows.Forms.Label();
            this.groupMisc = new System.Windows.Forms.GroupBox();
            this.txtDragCoeff = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPounds = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblExampleWidth = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();
            this.groupNotes = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblTireWidthUnits = new System.Windows.Forms.Label();
            this.lblExample = new System.Windows.Forms.Label();
            this.lblRimDiameter = new System.Windows.Forms.Label();
            this.groupTimeout = new System.Windows.Forms.GroupBox();
            this.lblTimeoutUnits = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtAspectRatio = new System.Windows.Forms.TextBox();
            this.lblTireAspectRatio = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCalcSpeedo = new System.Windows.Forms.Button();
            this.txtSpeedoFactor = new System.Windows.Forms.TextBox();
            this.txtTireWidth = new System.Windows.Forms.TextBox();
            this.lblTireWidth = new System.Windows.Forms.Label();
            this.radioManual = new System.Windows.Forms.RadioButton();
            this.radioAutomatic = new System.Windows.Forms.RadioButton();
            this.txtRimDiameter = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupWheels = new System.Windows.Forms.GroupBox();
            this.lblRimDiameterUnits = new System.Windows.Forms.Label();
            this.groupVehicles = new System.Windows.Forms.GroupBox();
            this.btnDeleteVehicle = new System.Windows.Forms.Button();
            this.btnNewVehicle = new System.Windows.Forms.Button();
            this.listVehicles = new System.Windows.Forms.ListBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupDrivetrain = new System.Windows.Forms.GroupBox();
            this.groupProfile = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.comboProfile = new System.Windows.Forms.ComboBox();
            this.lblActiveProfile = new System.Windows.Forms.Label();
            this.groupMisc.SuspendLayout();
            this.groupNotes.SuspendLayout();
            this.groupTimeout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupWheels.SuspendLayout();
            this.groupVehicles.SuspendLayout();
            this.groupDrivetrain.SuspendLayout();
            this.groupProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTireAspectUnits
            // 
            this.lblTireAspectUnits.Location = new System.Drawing.Point(174, 72);
            this.lblTireAspectUnits.Name = "lblTireAspectUnits";
            this.lblTireAspectUnits.Size = new System.Drawing.Size(36, 21);
            this.lblTireAspectUnits.TabIndex = 12;
            this.lblTireAspectUnits.Text = "%";
            this.lblTireAspectUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExampleEnd
            // 
            this.lblExampleEnd.Location = new System.Drawing.Point(204, 21);
            this.lblExampleEnd.Name = "lblExampleEnd";
            this.lblExampleEnd.Size = new System.Drawing.Size(6, 22);
            this.lblExampleEnd.TabIndex = 6;
            this.lblExampleEnd.Text = ")";
            this.lblExampleEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExampleDiameter
            // 
            this.lblExampleDiameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExampleDiameter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblExampleDiameter.Location = new System.Drawing.Point(174, 21);
            this.lblExampleDiameter.Name = "lblExampleDiameter";
            this.lblExampleDiameter.Size = new System.Drawing.Size(30, 22);
            this.lblExampleDiameter.TabIndex = 5;
            this.lblExampleDiameter.Text = "17";
            this.lblExampleDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExampleAspect
            // 
            this.lblExampleAspect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExampleAspect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblExampleAspect.Location = new System.Drawing.Point(126, 21);
            this.lblExampleAspect.Name = "lblExampleAspect";
            this.lblExampleAspect.Size = new System.Drawing.Size(36, 22);
            this.lblExampleAspect.TabIndex = 3;
            this.lblExampleAspect.Text = "40";
            this.lblExampleAspect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExampleSlash
            // 
            this.lblExampleSlash.Location = new System.Drawing.Point(114, 21);
            this.lblExampleSlash.Name = "lblExampleSlash";
            this.lblExampleSlash.Size = new System.Drawing.Size(12, 22);
            this.lblExampleSlash.TabIndex = 2;
            this.lblExampleSlash.Text = "/";
            this.lblExampleSlash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExampleDash
            // 
            this.lblExampleDash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExampleDash.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblExampleDash.Location = new System.Drawing.Point(162, 21);
            this.lblExampleDash.Name = "lblExampleDash";
            this.lblExampleDash.Size = new System.Drawing.Size(12, 22);
            this.lblExampleDash.TabIndex = 4;
            this.lblExampleDash.Text = "-";
            this.lblExampleDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupMisc
            // 
            this.groupMisc.Controls.Add(this.txtDragCoeff);
            this.groupMisc.Controls.Add(this.label1);
            this.groupMisc.Controls.Add(this.lblPounds);
            this.groupMisc.Controls.Add(this.txtWeight);
            this.groupMisc.Controls.Add(this.lblWeight);
            this.groupMisc.Location = new System.Drawing.Point(12, 269);
            this.groupMisc.Name = "groupMisc";
            this.groupMisc.Size = new System.Drawing.Size(246, 81);
            this.groupMisc.TabIndex = 5;
            this.groupMisc.TabStop = false;
            this.groupMisc.Text = "其他";
            // 
            // txtDragCoeff
            // 
            this.txtDragCoeff.Location = new System.Drawing.Point(108, 49);
            this.txtDragCoeff.Name = "txtDragCoeff";
            this.txtDragCoeff.Size = new System.Drawing.Size(60, 21);
            this.txtDragCoeff.TabIndex = 4;
            this.txtDragCoeff.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "拖拽系数(&D):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPounds
            // 
            this.lblPounds.Location = new System.Drawing.Point(174, 21);
            this.lblPounds.Name = "lblPounds";
            this.lblPounds.Size = new System.Drawing.Size(60, 22);
            this.lblPounds.TabIndex = 2;
            this.lblPounds.Text = "lbs";
            this.lblPounds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(108, 21);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(60, 21);
            this.txtWeight.TabIndex = 1;
            this.txtWeight.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lblWeight
            // 
            this.lblWeight.Location = new System.Drawing.Point(18, 21);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(84, 22);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "重量(&e):";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExampleWidth
            // 
            this.lblExampleWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExampleWidth.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblExampleWidth.Location = new System.Drawing.Point(78, 21);
            this.lblExampleWidth.Name = "lblExampleWidth";
            this.lblExampleWidth.Size = new System.Drawing.Size(36, 22);
            this.lblExampleWidth.TabIndex = 1;
            this.lblExampleWidth.Text = "275";
            this.lblExampleWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(306, 356);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 27);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Location = new System.Drawing.Point(402, 356);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.Size = new System.Drawing.Size(90, 27);
            this.btnDiscard.TabIndex = 9;
            this.btnDiscard.Text = "放弃(&i)";
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // groupNotes
            // 
            this.groupNotes.Controls.Add(this.txtNotes);
            this.groupNotes.Location = new System.Drawing.Point(270, 190);
            this.groupNotes.Name = "groupNotes";
            this.groupNotes.Size = new System.Drawing.Size(222, 160);
            this.groupNotes.TabIndex = 7;
            this.groupNotes.TabStop = false;
            this.groupNotes.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(12, 21);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(198, 128);
            this.txtNotes.TabIndex = 0;
            this.txtNotes.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lblTireWidthUnits
            // 
            this.lblTireWidthUnits.Location = new System.Drawing.Point(174, 45);
            this.lblTireWidthUnits.Name = "lblTireWidthUnits";
            this.lblTireWidthUnits.Size = new System.Drawing.Size(36, 22);
            this.lblTireWidthUnits.TabIndex = 9;
            this.lblTireWidthUnits.Text = "mm";
            this.lblTireWidthUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExample
            // 
            this.lblExample.Location = new System.Drawing.Point(6, 21);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(66, 22);
            this.lblExample.TabIndex = 0;
            this.lblExample.Text = "( 举例:";
            this.lblExample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRimDiameter
            // 
            this.lblRimDiameter.Location = new System.Drawing.Point(12, 99);
            this.lblRimDiameter.Name = "lblRimDiameter";
            this.lblRimDiameter.Size = new System.Drawing.Size(90, 21);
            this.lblRimDiameter.TabIndex = 13;
            this.lblRimDiameter.Text = "轮圈直径(&d):";
            this.lblRimDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupTimeout
            // 
            this.groupTimeout.Controls.Add(this.lblTimeoutUnits);
            this.groupTimeout.Controls.Add(this.numTimeout);
            this.groupTimeout.Controls.Add(this.lblTimeout);
            this.groupTimeout.Location = new System.Drawing.Point(12, 51);
            this.groupTimeout.Name = "groupTimeout";
            this.groupTimeout.Size = new System.Drawing.Size(246, 63);
            this.groupTimeout.TabIndex = 2;
            this.groupTimeout.TabStop = false;
            this.groupTimeout.Text = "OBD-II 时间参数";
            // 
            // lblTimeoutUnits
            // 
            this.lblTimeoutUnits.Location = new System.Drawing.Point(209, 25);
            this.lblTimeoutUnits.Name = "lblTimeoutUnits";
            this.lblTimeoutUnits.Size = new System.Drawing.Size(21, 22);
            this.lblTimeoutUnits.TabIndex = 2;
            this.lblTimeoutUnits.Text = "ms";
            this.lblTimeoutUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numTimeout
            // 
            this.numTimeout.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numTimeout.Location = new System.Drawing.Point(126, 26);
            this.numTimeout.Maximum = new decimal(new int[] {
            1020,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(73, 21);
            this.numTimeout.TabIndex = 1;
            this.numTimeout.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numTimeout.ValueChanged += new System.EventHandler(this.numTimeout_ValueChanged);
            // 
            // lblTimeout
            // 
            this.lblTimeout.Location = new System.Drawing.Point(18, 26);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(102, 21);
            this.lblTimeout.TabIndex = 0;
            this.lblTimeout.Text = "ELM 超时时间(&T):";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAspectRatio
            // 
            this.txtAspectRatio.Location = new System.Drawing.Point(108, 72);
            this.txtAspectRatio.Name = "txtAspectRatio";
            this.txtAspectRatio.Size = new System.Drawing.Size(60, 21);
            this.txtAspectRatio.TabIndex = 11;
            this.txtAspectRatio.TextChanged += new System.EventHandler(this.ValueChanged);
            this.txtAspectRatio.Enter += new System.EventHandler(this.txtAspectRatio_Enter);
            this.txtAspectRatio.Leave += new System.EventHandler(this.txtAspectRatio_Leave);
            // 
            // lblTireAspectRatio
            // 
            this.lblTireAspectRatio.Location = new System.Drawing.Point(12, 72);
            this.lblTireAspectRatio.Name = "lblTireAspectRatio";
            this.lblTireAspectRatio.Size = new System.Drawing.Size(90, 21);
            this.lblTireAspectRatio.TabIndex = 10;
            this.lblTireAspectRatio.Text = "扁平比(&a):";
            this.lblTireAspectRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCalcSpeedo);
            this.groupBox1.Controls.Add(this.txtSpeedoFactor);
            this.groupBox1.Location = new System.Drawing.Point(13, 190);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 70);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "速度表标定因子";
            // 
            // btnCalcSpeedo
            // 
            this.btnCalcSpeedo.Location = new System.Drawing.Point(126, 29);
            this.btnCalcSpeedo.Name = "btnCalcSpeedo";
            this.btnCalcSpeedo.Size = new System.Drawing.Size(98, 25);
            this.btnCalcSpeedo.TabIndex = 1;
            this.btnCalcSpeedo.Text = "计算(&C)";
            this.btnCalcSpeedo.Click += new System.EventHandler(this.btnCalcSpeedo_Click);
            // 
            // txtSpeedoFactor
            // 
            this.txtSpeedoFactor.Location = new System.Drawing.Point(22, 30);
            this.txtSpeedoFactor.Name = "txtSpeedoFactor";
            this.txtSpeedoFactor.Size = new System.Drawing.Size(86, 21);
            this.txtSpeedoFactor.TabIndex = 0;
            this.txtSpeedoFactor.Text = "1.000";
            this.txtSpeedoFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTireWidth
            // 
            this.txtTireWidth.Location = new System.Drawing.Point(108, 45);
            this.txtTireWidth.Name = "txtTireWidth";
            this.txtTireWidth.Size = new System.Drawing.Size(60, 21);
            this.txtTireWidth.TabIndex = 8;
            this.txtTireWidth.TextChanged += new System.EventHandler(this.ValueChanged);
            this.txtTireWidth.Enter += new System.EventHandler(this.txtTireWidth_Enter);
            this.txtTireWidth.Leave += new System.EventHandler(this.txtTireWidth_Leave);
            // 
            // lblTireWidth
            // 
            this.lblTireWidth.Location = new System.Drawing.Point(12, 45);
            this.lblTireWidth.Name = "lblTireWidth";
            this.lblTireWidth.Size = new System.Drawing.Size(90, 22);
            this.lblTireWidth.TabIndex = 7;
            this.lblTireWidth.Text = "轮胎宽度(&w):";
            this.lblTireWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioManual
            // 
            this.radioManual.Checked = true;
            this.radioManual.Location = new System.Drawing.Point(139, 21);
            this.radioManual.Name = "radioManual";
            this.radioManual.Size = new System.Drawing.Size(90, 22);
            this.radioManual.TabIndex = 1;
            this.radioManual.TabStop = true;
            this.radioManual.Text = "手动(&M)";
            this.radioManual.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // radioAutomatic
            // 
            this.radioAutomatic.Location = new System.Drawing.Point(18, 21);
            this.radioAutomatic.Name = "radioAutomatic";
            this.radioAutomatic.Size = new System.Drawing.Size(90, 22);
            this.radioAutomatic.TabIndex = 0;
            this.radioAutomatic.Text = "自动(&A)";
            this.radioAutomatic.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // txtRimDiameter
            // 
            this.txtRimDiameter.Location = new System.Drawing.Point(108, 99);
            this.txtRimDiameter.Name = "txtRimDiameter";
            this.txtRimDiameter.Size = new System.Drawing.Size(60, 21);
            this.txtRimDiameter.TabIndex = 14;
            this.txtRimDiameter.TextChanged += new System.EventHandler(this.ValueChanged);
            this.txtRimDiameter.Enter += new System.EventHandler(this.txtRimDiameter_Enter);
            this.txtRimDiameter.Leave += new System.EventHandler(this.txtRimDiameter_Leave);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(601, 416);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 28);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "完成(&o)";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupWheels
            // 
            this.groupWheels.Controls.Add(this.lblRimDiameterUnits);
            this.groupWheels.Controls.Add(this.lblTireAspectUnits);
            this.groupWheels.Controls.Add(this.lblTireWidthUnits);
            this.groupWheels.Controls.Add(this.lblExampleEnd);
            this.groupWheels.Controls.Add(this.lblExampleDiameter);
            this.groupWheels.Controls.Add(this.lblExampleAspect);
            this.groupWheels.Controls.Add(this.lblExampleSlash);
            this.groupWheels.Controls.Add(this.lblExampleDash);
            this.groupWheels.Controls.Add(this.lblExampleWidth);
            this.groupWheels.Controls.Add(this.lblExample);
            this.groupWheels.Controls.Add(this.txtRimDiameter);
            this.groupWheels.Controls.Add(this.lblRimDiameter);
            this.groupWheels.Controls.Add(this.txtAspectRatio);
            this.groupWheels.Controls.Add(this.lblTireAspectRatio);
            this.groupWheels.Controls.Add(this.txtTireWidth);
            this.groupWheels.Controls.Add(this.lblTireWidth);
            this.groupWheels.Location = new System.Drawing.Point(270, 51);
            this.groupWheels.Name = "groupWheels";
            this.groupWheels.Size = new System.Drawing.Size(222, 132);
            this.groupWheels.TabIndex = 6;
            this.groupWheels.TabStop = false;
            this.groupWheels.Text = "车轮";
            // 
            // lblRimDiameterUnits
            // 
            this.lblRimDiameterUnits.Location = new System.Drawing.Point(174, 99);
            this.lblRimDiameterUnits.Name = "lblRimDiameterUnits";
            this.lblRimDiameterUnits.Size = new System.Drawing.Size(36, 21);
            this.lblRimDiameterUnits.TabIndex = 15;
            this.lblRimDiameterUnits.Text = "in";
            this.lblRimDiameterUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupVehicles
            // 
            this.groupVehicles.Controls.Add(this.btnDeleteVehicle);
            this.groupVehicles.Controls.Add(this.btnNewVehicle);
            this.groupVehicles.Controls.Add(this.listVehicles);
            this.groupVehicles.Location = new System.Drawing.Point(12, 12);
            this.groupVehicles.Name = "groupVehicles";
            this.groupVehicles.Size = new System.Drawing.Size(180, 392);
            this.groupVehicles.TabIndex = 3;
            this.groupVehicles.TabStop = false;
            this.groupVehicles.Text = "车辆配置";
            // 
            // btnDeleteVehicle
            // 
            this.btnDeleteVehicle.Location = new System.Drawing.Point(92, 356);
            this.btnDeleteVehicle.Name = "btnDeleteVehicle";
            this.btnDeleteVehicle.Size = new System.Drawing.Size(76, 27);
            this.btnDeleteVehicle.TabIndex = 2;
            this.btnDeleteVehicle.Text = "删除(&D)";
            this.btnDeleteVehicle.Click += new System.EventHandler(this.btnDeleteVehicle_Click);
            // 
            // btnNewVehicle
            // 
            this.btnNewVehicle.Location = new System.Drawing.Point(12, 356);
            this.btnNewVehicle.Name = "btnNewVehicle";
            this.btnNewVehicle.Size = new System.Drawing.Size(74, 27);
            this.btnNewVehicle.TabIndex = 1;
            this.btnNewVehicle.Text = "新建(&N)";
            this.btnNewVehicle.Click += new System.EventHandler(this.btnNewVehicle_Click);
            // 
            // listVehicles
            // 
            this.listVehicles.ItemHeight = 12;
            this.listVehicles.Location = new System.Drawing.Point(12, 21);
            this.listVehicles.Name = "listVehicles";
            this.listVehicles.Size = new System.Drawing.Size(156, 328);
            this.listVehicles.TabIndex = 0;
            this.listVehicles.SelectedIndexChanged += new System.EventHandler(this.listVehicles_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(72, 21);
            this.txtName.MaxLength = 20;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(420, 21);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // groupDrivetrain
            // 
            this.groupDrivetrain.Controls.Add(this.radioManual);
            this.groupDrivetrain.Controls.Add(this.radioAutomatic);
            this.groupDrivetrain.Location = new System.Drawing.Point(12, 123);
            this.groupDrivetrain.Name = "groupDrivetrain";
            this.groupDrivetrain.Size = new System.Drawing.Size(246, 60);
            this.groupDrivetrain.TabIndex = 3;
            this.groupDrivetrain.TabStop = false;
            this.groupDrivetrain.Text = "变速箱";
            // 
            // groupProfile
            // 
            this.groupProfile.Controls.Add(this.groupBox1);
            this.groupProfile.Controls.Add(this.groupTimeout);
            this.groupProfile.Controls.Add(this.btnSave);
            this.groupProfile.Controls.Add(this.btnDiscard);
            this.groupProfile.Controls.Add(this.groupNotes);
            this.groupProfile.Controls.Add(this.groupMisc);
            this.groupProfile.Controls.Add(this.groupWheels);
            this.groupProfile.Controls.Add(this.groupDrivetrain);
            this.groupProfile.Controls.Add(this.txtName);
            this.groupProfile.Controls.Add(this.lblName);
            this.groupProfile.Location = new System.Drawing.Point(198, 12);
            this.groupProfile.Name = "groupProfile";
            this.groupProfile.Size = new System.Drawing.Size(509, 392);
            this.groupProfile.TabIndex = 4;
            this.groupProfile.TabStop = false;
            this.groupProfile.Text = "选中的车辆配置";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(6, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 22);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "配置名:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboProfile
            // 
            this.comboProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProfile.Location = new System.Drawing.Point(361, 419);
            this.comboProfile.Name = "comboProfile";
            this.comboProfile.Size = new System.Drawing.Size(226, 20);
            this.comboProfile.TabIndex = 15;
            // 
            // lblActiveProfile
            // 
            this.lblActiveProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblActiveProfile.Location = new System.Drawing.Point(249, 418);
            this.lblActiveProfile.Name = "lblActiveProfile";
            this.lblActiveProfile.Size = new System.Drawing.Size(106, 25);
            this.lblActiveProfile.TabIndex = 16;
            this.lblActiveProfile.Text = "当前车辆配置(&P):";
            this.lblActiveProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VehicleForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(719, 457);
            this.Controls.Add(this.comboProfile);
            this.Controls.Add(this.lblActiveProfile);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupVehicles);
            this.Controls.Add(this.groupProfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VehicleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "车辆参数管理";
            this.Load += new System.EventHandler(this.VehicleForm_Load);
            this.groupMisc.ResumeLayout(false);
            this.groupMisc.PerformLayout();
            this.groupNotes.ResumeLayout(false);
            this.groupNotes.PerformLayout();
            this.groupTimeout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupWheels.ResumeLayout(false);
            this.groupWheels.PerformLayout();
            this.groupVehicles.ResumeLayout(false);
            this.groupDrivetrain.ResumeLayout(false);
            this.groupProfile.ResumeLayout(false);
            this.groupProfile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTireAspectUnits;
        private System.Windows.Forms.Label lblExampleEnd;
        private System.Windows.Forms.Label lblExampleDiameter;
        private System.Windows.Forms.Label lblExampleAspect;
        private System.Windows.Forms.Label lblExampleSlash;
        private System.Windows.Forms.Label lblExampleDash;
        private System.Windows.Forms.GroupBox groupMisc;
        private System.Windows.Forms.TextBox txtDragCoeff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPounds;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblExampleWidth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDiscard;
        private System.Windows.Forms.GroupBox groupNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblTireWidthUnits;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.Label lblRimDiameter;
        private System.Windows.Forms.GroupBox groupTimeout;
        private System.Windows.Forms.Label lblTimeoutUnits;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.TextBox txtAspectRatio;
        private System.Windows.Forms.Label lblTireAspectRatio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCalcSpeedo;
        private System.Windows.Forms.TextBox txtSpeedoFactor;
        private System.Windows.Forms.TextBox txtTireWidth;
        private System.Windows.Forms.Label lblTireWidth;
        private System.Windows.Forms.RadioButton radioManual;
        private System.Windows.Forms.RadioButton radioAutomatic;
        private System.Windows.Forms.TextBox txtRimDiameter;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupWheels;
        private System.Windows.Forms.Label lblRimDiameterUnits;
        private System.Windows.Forms.GroupBox groupVehicles;
        private System.Windows.Forms.Button btnDeleteVehicle;
        private System.Windows.Forms.Button btnNewVehicle;
        private System.Windows.Forms.ListBox listVehicles;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.GroupBox groupDrivetrain;
        private System.Windows.Forms.GroupBox groupProfile;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox comboProfile;
        private System.Windows.Forms.Label lblActiveProfile;
    }
}