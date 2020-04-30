namespace SH_OBD {
    partial class SettingsForm {
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
            this.btnOK = new System.Windows.Forms.Button();
            this.groupDB = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtBoxPort = new System.Windows.Forms.TextBox();
            this.txtBoxIP = new System.Windows.Forms.TextBox();
            this.txtBoxDBName = new System.Windows.Forms.TextBox();
            this.txtBoxPwd = new System.Windows.Forms.TextBox();
            this.txtBoxUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupMES = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.radioBtnWSDL = new System.Windows.Forms.RadioButton();
            this.radioBtnURL = new System.Windows.Forms.RadioButton();
            this.txtBoxWebSvcWSDL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBoxWebSvcMethods = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBoxWebSvcName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBoxWebSvcAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboHardware = new System.Windows.Forms.ComboBox();
            this.comboPorts = new System.Windows.Forms.ComboBox();
            this.checkBoxAutoDetect = new System.Windows.Forms.CheckBox();
            this.groupELM = new System.Windows.Forms.GroupBox();
            this.comboStandard = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboInitialize = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboProtocol = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBaud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupScanner = new System.Windows.Forms.GroupBox();
            this.chkBoxUseSerialScanner = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbBoxScannerBaud = new System.Windows.Forms.ComboBox();
            this.cmbBoxScannerPort = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupDB.SuspendLayout();
            this.groupMES.SuspendLayout();
            this.groupELM.SuspendLayout();
            this.groupScanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 378);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(406, 378);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "保存(&S)";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // groupDB
            // 
            this.groupDB.Controls.Add(this.label9);
            this.groupDB.Controls.Add(this.txtBoxPort);
            this.groupDB.Controls.Add(this.txtBoxIP);
            this.groupDB.Controls.Add(this.txtBoxDBName);
            this.groupDB.Controls.Add(this.txtBoxPwd);
            this.groupDB.Controls.Add(this.txtBoxUser);
            this.groupDB.Controls.Add(this.label7);
            this.groupDB.Controls.Add(this.label6);
            this.groupDB.Controls.Add(this.label5);
            this.groupDB.Controls.Add(this.label4);
            this.groupDB.Location = new System.Drawing.Point(12, 143);
            this.groupDB.Name = "groupDB";
            this.groupDB.Size = new System.Drawing.Size(190, 160);
            this.groupDB.TabIndex = 1;
            this.groupDB.TabStop = false;
            this.groupDB.Text = "数据库设置";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "端口：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxPort
            // 
            this.txtBoxPort.Location = new System.Drawing.Point(82, 128);
            this.txtBoxPort.Name = "txtBoxPort";
            this.txtBoxPort.Size = new System.Drawing.Size(100, 21);
            this.txtBoxPort.TabIndex = 9;
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Location = new System.Drawing.Point(82, 101);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(100, 21);
            this.txtBoxIP.TabIndex = 7;
            // 
            // txtBoxDBName
            // 
            this.txtBoxDBName.Location = new System.Drawing.Point(82, 74);
            this.txtBoxDBName.Name = "txtBoxDBName";
            this.txtBoxDBName.Size = new System.Drawing.Size(100, 21);
            this.txtBoxDBName.TabIndex = 5;
            // 
            // txtBoxPwd
            // 
            this.txtBoxPwd.Location = new System.Drawing.Point(82, 47);
            this.txtBoxPwd.Name = "txtBoxPwd";
            this.txtBoxPwd.Size = new System.Drawing.Size(100, 21);
            this.txtBoxPwd.TabIndex = 3;
            // 
            // txtBoxUser
            // 
            this.txtBoxUser.Location = new System.Drawing.Point(82, 20);
            this.txtBoxUser.Name = "txtBoxUser";
            this.txtBoxUser.Size = new System.Drawing.Size(100, 21);
            this.txtBoxUser.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 22);
            this.label7.TabIndex = 6;
            this.label7.Text = "IP地址：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "数据库名：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "密码：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "用户名：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupMES
            // 
            this.groupMES.Controls.Add(this.label14);
            this.groupMES.Controls.Add(this.radioBtnWSDL);
            this.groupMES.Controls.Add(this.radioBtnURL);
            this.groupMES.Controls.Add(this.txtBoxWebSvcWSDL);
            this.groupMES.Controls.Add(this.label12);
            this.groupMES.Controls.Add(this.txtBoxWebSvcMethods);
            this.groupMES.Controls.Add(this.label11);
            this.groupMES.Controls.Add(this.txtBoxWebSvcName);
            this.groupMES.Controls.Add(this.label10);
            this.groupMES.Controls.Add(this.txtBoxWebSvcAddress);
            this.groupMES.Controls.Add(this.label8);
            this.groupMES.Location = new System.Drawing.Point(209, 143);
            this.groupMES.Name = "groupMES";
            this.groupMES.Size = new System.Drawing.Size(383, 160);
            this.groupMES.TabIndex = 2;
            this.groupMES.TabStop = false;
            this.groupMES.Text = "WebService设置";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(6, 128);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 22);
            this.label14.TabIndex = 8;
            this.label14.Text = "引用方式：";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioBtnWSDL
            // 
            this.radioBtnWSDL.AutoSize = true;
            this.radioBtnWSDL.Location = new System.Drawing.Point(182, 131);
            this.radioBtnWSDL.Name = "radioBtnWSDL";
            this.radioBtnWSDL.Size = new System.Drawing.Size(95, 16);
            this.radioBtnWSDL.TabIndex = 10;
            this.radioBtnWSDL.TabStop = true;
            this.radioBtnWSDL.Text = "使用WSDL文件";
            this.radioBtnWSDL.UseVisualStyleBackColor = true;
            this.radioBtnWSDL.Click += new System.EventHandler(this.RadioBtn_Click);
            // 
            // radioBtnURL
            // 
            this.radioBtnURL.AutoSize = true;
            this.radioBtnURL.Location = new System.Drawing.Point(86, 131);
            this.radioBtnURL.Name = "radioBtnURL";
            this.radioBtnURL.Size = new System.Drawing.Size(89, 16);
            this.radioBtnURL.TabIndex = 9;
            this.radioBtnURL.TabStop = true;
            this.radioBtnURL.Text = "使用URL地址";
            this.radioBtnURL.UseVisualStyleBackColor = true;
            this.radioBtnURL.Click += new System.EventHandler(this.RadioBtn_Click);
            // 
            // txtBoxWebSvcWSDL
            // 
            this.txtBoxWebSvcWSDL.Location = new System.Drawing.Point(87, 47);
            this.txtBoxWebSvcWSDL.Name = "txtBoxWebSvcWSDL";
            this.txtBoxWebSvcWSDL.Size = new System.Drawing.Size(290, 21);
            this.txtBoxWebSvcWSDL.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 22);
            this.label12.TabIndex = 2;
            this.label12.Text = "WSDL文件：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSvcMethods
            // 
            this.txtBoxWebSvcMethods.Location = new System.Drawing.Point(87, 101);
            this.txtBoxWebSvcMethods.Name = "txtBoxWebSvcMethods";
            this.txtBoxWebSvcMethods.Size = new System.Drawing.Size(290, 21);
            this.txtBoxWebSvcMethods.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 22);
            this.label11.TabIndex = 6;
            this.label11.Text = "方法名：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSvcName
            // 
            this.txtBoxWebSvcName.Location = new System.Drawing.Point(87, 74);
            this.txtBoxWebSvcName.Name = "txtBoxWebSvcName";
            this.txtBoxWebSvcName.Size = new System.Drawing.Size(290, 21);
            this.txtBoxWebSvcName.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 22);
            this.label10.TabIndex = 4;
            this.label10.Text = "程序名：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSvcAddress
            // 
            this.txtBoxWebSvcAddress.Location = new System.Drawing.Point(87, 20);
            this.txtBoxWebSvcAddress.Name = "txtBoxWebSvcAddress";
            this.txtBoxWebSvcAddress.Size = new System.Drawing.Size(290, 21);
            this.txtBoxWebSvcAddress.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "URL地址：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboHardware
            // 
            this.comboHardware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHardware.Items.AddRange(new object[] {
            "自动探测",
            "ELM327/SH-VCI-302U",
            "ELM320 (PWM)",
            "ELM322 (VPW)",
            "ELM323 (ISO)",
            "CANtact"});
            this.comboHardware.Location = new System.Drawing.Point(82, 68);
            this.comboHardware.Name = "comboHardware";
            this.comboHardware.Size = new System.Drawing.Size(115, 20);
            this.comboHardware.TabIndex = 4;
            // 
            // comboPorts
            // 
            this.comboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPorts.Location = new System.Drawing.Point(82, 42);
            this.comboPorts.Name = "comboPorts";
            this.comboPorts.Size = new System.Drawing.Size(115, 20);
            this.comboPorts.TabIndex = 2;
            // 
            // checkBoxAutoDetect
            // 
            this.checkBoxAutoDetect.AutoSize = true;
            this.checkBoxAutoDetect.Location = new System.Drawing.Point(9, 20);
            this.checkBoxAutoDetect.Name = "checkBoxAutoDetect";
            this.checkBoxAutoDetect.Size = new System.Drawing.Size(138, 16);
            this.checkBoxAutoDetect.TabIndex = 0;
            this.checkBoxAutoDetect.Text = "自动探测OBD连接设置";
            this.checkBoxAutoDetect.CheckedChanged += new System.EventHandler(this.CheckBoxAutoDetect_CheckedChanged);
            // 
            // groupELM
            // 
            this.groupELM.Controls.Add(this.comboStandard);
            this.groupELM.Controls.Add(this.label18);
            this.groupELM.Controls.Add(this.label15);
            this.groupELM.Controls.Add(this.label13);
            this.groupELM.Controls.Add(this.comboHardware);
            this.groupELM.Controls.Add(this.comboPorts);
            this.groupELM.Controls.Add(this.comboInitialize);
            this.groupELM.Controls.Add(this.label3);
            this.groupELM.Controls.Add(this.checkBoxAutoDetect);
            this.groupELM.Controls.Add(this.comboProtocol);
            this.groupELM.Controls.Add(this.label2);
            this.groupELM.Controls.Add(this.comboBaud);
            this.groupELM.Controls.Add(this.label1);
            this.groupELM.Location = new System.Drawing.Point(12, 12);
            this.groupELM.Name = "groupELM";
            this.groupELM.Size = new System.Drawing.Size(580, 125);
            this.groupELM.TabIndex = 0;
            this.groupELM.TabStop = false;
            this.groupELM.Text = "ELM327 设置(&C)";
            // 
            // comboStandard
            // 
            this.comboStandard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStandard.Location = new System.Drawing.Point(294, 94);
            this.comboStandard.Name = "comboStandard";
            this.comboStandard.Size = new System.Drawing.Size(280, 20);
            this.comboStandard.TabIndex = 12;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(198, 93);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(90, 21);
            this.label18.TabIndex = 11;
            this.label18.Text = "上层协议(&S):";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(6, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 15);
            this.label15.TabIndex = 3;
            this.label15.Text = "VCI设备:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 15);
            this.label13.TabIndex = 1;
            this.label13.Text = "串口:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboInitialize
            // 
            this.comboInitialize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInitialize.Items.AddRange(new object[] {
            "初始化",
            "旁路初始化"});
            this.comboInitialize.Location = new System.Drawing.Point(294, 68);
            this.comboInitialize.Name = "comboInitialize";
            this.comboInitialize.Size = new System.Drawing.Size(280, 20);
            this.comboInitialize.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(213, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "初始化(&I):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboProtocol
            // 
            this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocol.Location = new System.Drawing.Point(294, 42);
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Size = new System.Drawing.Size(280, 20);
            this.comboProtocol.TabIndex = 8;
            this.comboProtocol.SelectedIndexChanged += new System.EventHandler(this.ComboProtocol_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(213, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 22);
            this.label2.TabIndex = 7;
            this.label2.Text = "OBD协议(&r):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBaud
            // 
            this.comboBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBaud.Items.AddRange(new object[] {
            "9600",
            "38400"});
            this.comboBaud.Location = new System.Drawing.Point(82, 94);
            this.comboBaud.Name = "comboBaud";
            this.comboBaud.Size = new System.Drawing.Size(115, 20);
            this.comboBaud.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "波特率(&B):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupScanner
            // 
            this.groupScanner.Controls.Add(this.chkBoxUseSerialScanner);
            this.groupScanner.Controls.Add(this.label16);
            this.groupScanner.Controls.Add(this.cmbBoxScannerBaud);
            this.groupScanner.Controls.Add(this.cmbBoxScannerPort);
            this.groupScanner.Controls.Add(this.label17);
            this.groupScanner.Location = new System.Drawing.Point(12, 309);
            this.groupScanner.Name = "groupScanner";
            this.groupScanner.Size = new System.Drawing.Size(580, 63);
            this.groupScanner.TabIndex = 3;
            this.groupScanner.TabStop = false;
            this.groupScanner.Text = "串口扫码枪设置";
            // 
            // chkBoxUseSerialScanner
            // 
            this.chkBoxUseSerialScanner.AutoSize = true;
            this.chkBoxUseSerialScanner.Location = new System.Drawing.Point(9, 29);
            this.chkBoxUseSerialScanner.Name = "chkBoxUseSerialScanner";
            this.chkBoxUseSerialScanner.Size = new System.Drawing.Size(108, 16);
            this.chkBoxUseSerialScanner.TabIndex = 0;
            this.chkBoxUseSerialScanner.Text = "使用串口扫码枪";
            this.chkBoxUseSerialScanner.UseVisualStyleBackColor = true;
            this.chkBoxUseSerialScanner.CheckedChanged += new System.EventHandler(this.ChkBoxUseSerialScanner_CheckedChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(114, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 15);
            this.label16.TabIndex = 1;
            this.label16.Text = "串口：";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBoxScannerBaud
            // 
            this.cmbBoxScannerBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxScannerBaud.Items.AddRange(new object[] {
            "9600",
            "38400",
            "115200"});
            this.cmbBoxScannerBaud.Location = new System.Drawing.Point(422, 25);
            this.cmbBoxScannerBaud.Name = "cmbBoxScannerBaud";
            this.cmbBoxScannerBaud.Size = new System.Drawing.Size(150, 20);
            this.cmbBoxScannerBaud.TabIndex = 4;
            // 
            // cmbBoxScannerPort
            // 
            this.cmbBoxScannerPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxScannerPort.Location = new System.Drawing.Point(190, 25);
            this.cmbBoxScannerPort.Name = "cmbBoxScannerPort";
            this.cmbBoxScannerPort.Size = new System.Drawing.Size(150, 20);
            this.cmbBoxScannerPort.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(346, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 22);
            this.label17.TabIndex = 3;
            this.label17.Text = "波特率(&B):";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(604, 411);
            this.Controls.Add(this.groupScanner);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupDB);
            this.Controls.Add(this.groupMES);
            this.Controls.Add(this.groupELM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "通讯参数设置";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupDB.ResumeLayout(false);
            this.groupDB.PerformLayout();
            this.groupMES.ResumeLayout(false);
            this.groupMES.PerformLayout();
            this.groupELM.ResumeLayout(false);
            this.groupELM.PerformLayout();
            this.groupScanner.ResumeLayout(false);
            this.groupScanner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupDB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBoxWebSvcAddress;
        private System.Windows.Forms.TextBox txtBoxPort;
        private System.Windows.Forms.TextBox txtBoxIP;
        private System.Windows.Forms.TextBox txtBoxDBName;
        private System.Windows.Forms.TextBox txtBoxPwd;
        private System.Windows.Forms.TextBox txtBoxUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupMES;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBoxWebSvcName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBoxWebSvcMethods;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBoxWebSvcWSDL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radioBtnWSDL;
        private System.Windows.Forms.RadioButton radioBtnURL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboHardware;
        private System.Windows.Forms.ComboBox comboPorts;
        private System.Windows.Forms.CheckBox checkBoxAutoDetect;
        private System.Windows.Forms.GroupBox groupELM;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboInitialize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboProtocol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBaud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupScanner;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmbBoxScannerBaud;
        private System.Windows.Forms.ComboBox cmbBoxScannerPort;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkBoxUseSerialScanner;
        private System.Windows.Forms.ComboBox comboStandard;
        private System.Windows.Forms.Label label18;
    }
}