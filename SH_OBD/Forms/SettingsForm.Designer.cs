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
            this.comboPorts = new System.Windows.Forms.ComboBox();
            this.groupELM = new System.Windows.Forms.GroupBox();
            this.comboInitialize = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboProtocol = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBaud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupHardware = new System.Windows.Forms.GroupBox();
            this.comboHardware = new System.Windows.Forms.ComboBox();
            this.groupComm = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoDetect = new System.Windows.Forms.CheckBox();
            this.groupOBDSetting = new System.Windows.Forms.GroupBox();
            this.groupDBandMES = new System.Windows.Forms.GroupBox();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBoxWebSvcName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBoxWebSvcAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBoxWebSvcMethods = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupELM.SuspendLayout();
            this.groupHardware.SuspendLayout();
            this.groupComm.SuspendLayout();
            this.groupOBDSetting.SuspendLayout();
            this.groupDBandMES.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboPorts
            // 
            this.comboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPorts.Location = new System.Drawing.Point(6, 21);
            this.comboPorts.Name = "comboPorts";
            this.comboPorts.Size = new System.Drawing.Size(151, 20);
            this.comboPorts.TabIndex = 0;
            // 
            // groupELM
            // 
            this.groupELM.Controls.Add(this.comboInitialize);
            this.groupELM.Controls.Add(this.label3);
            this.groupELM.Controls.Add(this.comboProtocol);
            this.groupELM.Controls.Add(this.label2);
            this.groupELM.Controls.Add(this.comboBaud);
            this.groupELM.Controls.Add(this.label1);
            this.groupELM.Location = new System.Drawing.Point(6, 121);
            this.groupELM.Name = "groupELM";
            this.groupELM.Size = new System.Drawing.Size(483, 124);
            this.groupELM.TabIndex = 9;
            this.groupELM.TabStop = false;
            this.groupELM.Text = "ELM327 设置(&C)";
            // 
            // comboInitialize
            // 
            this.comboInitialize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInitialize.Items.AddRange(new object[] {
            "初始化",
            "旁路初始化"});
            this.comboInitialize.Location = new System.Drawing.Point(106, 86);
            this.comboInitialize.Name = "comboInitialize";
            this.comboInitialize.Size = new System.Drawing.Size(370, 20);
            this.comboInitialize.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "初始化方式(&I):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboProtocol
            // 
            this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocol.Location = new System.Drawing.Point(106, 55);
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Size = new System.Drawing.Size(371, 20);
            this.comboProtocol.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "OBD协议(&r):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBaud
            // 
            this.comboBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBaud.Items.AddRange(new object[] {
            "9600",
            "38400"});
            this.comboBaud.Location = new System.Drawing.Point(106, 25);
            this.comboBaud.Name = "comboBaud";
            this.comboBaud.Size = new System.Drawing.Size(370, 20);
            this.comboBaud.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "波特率(&B):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(417, 458);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(321, 458);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "保存(&S)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupHardware
            // 
            this.groupHardware.Controls.Add(this.comboHardware);
            this.groupHardware.Location = new System.Drawing.Point(175, 55);
            this.groupHardware.Name = "groupHardware";
            this.groupHardware.Size = new System.Drawing.Size(314, 60);
            this.groupHardware.TabIndex = 8;
            this.groupHardware.TabStop = false;
            this.groupHardware.Text = "OBD硬件设备(&H)";
            // 
            // comboHardware
            // 
            this.comboHardware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHardware.Items.AddRange(new object[] {
            "自动探测",
            "ELM327 (通用型)",
            "ELM320 (PWM)",
            "ELM322 (VPW)",
            "ELM323 (ISO)",
            "CANtact"});
            this.comboHardware.Location = new System.Drawing.Point(6, 21);
            this.comboHardware.Name = "comboHardware";
            this.comboHardware.Size = new System.Drawing.Size(301, 20);
            this.comboHardware.TabIndex = 0;
            this.comboHardware.SelectedIndexChanged += new System.EventHandler(this.comboHardware_SelectedIndexChanged);
            // 
            // groupComm
            // 
            this.groupComm.Controls.Add(this.comboPorts);
            this.groupComm.Location = new System.Drawing.Point(6, 55);
            this.groupComm.Name = "groupComm";
            this.groupComm.Size = new System.Drawing.Size(163, 60);
            this.groupComm.TabIndex = 7;
            this.groupComm.TabStop = false;
            this.groupComm.Text = "串口(&P)";
            // 
            // checkBoxAutoDetect
            // 
            this.checkBoxAutoDetect.Location = new System.Drawing.Point(6, 24);
            this.checkBoxAutoDetect.Name = "checkBoxAutoDetect";
            this.checkBoxAutoDetect.Size = new System.Drawing.Size(483, 25);
            this.checkBoxAutoDetect.TabIndex = 12;
            this.checkBoxAutoDetect.Text = "使用ELM327设备自动探测OBD连接设置";
            this.checkBoxAutoDetect.CheckedChanged += new System.EventHandler(this.checkBoxAutoDetect_CheckedChanged);
            // 
            // groupOBDSetting
            // 
            this.groupOBDSetting.Controls.Add(this.groupELM);
            this.groupOBDSetting.Controls.Add(this.checkBoxAutoDetect);
            this.groupOBDSetting.Controls.Add(this.groupComm);
            this.groupOBDSetting.Controls.Add(this.groupHardware);
            this.groupOBDSetting.Location = new System.Drawing.Point(12, 12);
            this.groupOBDSetting.Name = "groupOBDSetting";
            this.groupOBDSetting.Size = new System.Drawing.Size(495, 255);
            this.groupOBDSetting.TabIndex = 13;
            this.groupOBDSetting.TabStop = false;
            this.groupOBDSetting.Text = "OBD通讯设置";
            // 
            // groupDBandMES
            // 
            this.groupDBandMES.Controls.Add(this.label9);
            this.groupDBandMES.Controls.Add(this.txtBoxPort);
            this.groupDBandMES.Controls.Add(this.txtBoxIP);
            this.groupDBandMES.Controls.Add(this.txtBoxDBName);
            this.groupDBandMES.Controls.Add(this.txtBoxPwd);
            this.groupDBandMES.Controls.Add(this.txtBoxUser);
            this.groupDBandMES.Controls.Add(this.label7);
            this.groupDBandMES.Controls.Add(this.label6);
            this.groupDBandMES.Controls.Add(this.label5);
            this.groupDBandMES.Controls.Add(this.label4);
            this.groupDBandMES.Controls.Add(this.groupBox1);
            this.groupDBandMES.Location = new System.Drawing.Point(13, 274);
            this.groupDBandMES.Name = "groupDBandMES";
            this.groupDBandMES.Size = new System.Drawing.Size(494, 178);
            this.groupDBandMES.TabIndex = 14;
            this.groupDBandMES.TabStop = false;
            this.groupDBandMES.Text = "数据库及MES设置";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "端口：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxPort
            // 
            this.txtBoxPort.Location = new System.Drawing.Point(82, 145);
            this.txtBoxPort.Name = "txtBoxPort";
            this.txtBoxPort.Size = new System.Drawing.Size(120, 21);
            this.txtBoxPort.TabIndex = 8;
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Location = new System.Drawing.Point(82, 115);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(120, 21);
            this.txtBoxIP.TabIndex = 7;
            // 
            // txtBoxDBName
            // 
            this.txtBoxDBName.Location = new System.Drawing.Point(82, 85);
            this.txtBoxDBName.Name = "txtBoxDBName";
            this.txtBoxDBName.Size = new System.Drawing.Size(120, 21);
            this.txtBoxDBName.TabIndex = 6;
            // 
            // txtBoxPwd
            // 
            this.txtBoxPwd.Location = new System.Drawing.Point(82, 55);
            this.txtBoxPwd.Name = "txtBoxPwd";
            this.txtBoxPwd.Size = new System.Drawing.Size(120, 21);
            this.txtBoxPwd.TabIndex = 5;
            // 
            // txtBoxUser
            // 
            this.txtBoxUser.Location = new System.Drawing.Point(82, 25);
            this.txtBoxUser.Name = "txtBoxUser";
            this.txtBoxUser.Size = new System.Drawing.Size(120, 21);
            this.txtBoxUser.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 22);
            this.label7.TabIndex = 3;
            this.label7.Text = "IP地址：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 18);
            this.label6.TabIndex = 2;
            this.label6.Text = "数据库名：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "密码：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "用户名：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBoxWebSvcMethods);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtBoxWebSvcName);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtBoxWebSvcAddress);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(208, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 146);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WebService";
            // 
            // txtBoxWebSvcName
            // 
            this.txtBoxWebSvcName.Location = new System.Drawing.Point(73, 47);
            this.txtBoxWebSvcName.Name = "txtBoxWebSvcName";
            this.txtBoxWebSvcName.Size = new System.Drawing.Size(200, 21);
            this.txtBoxWebSvcName.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 22);
            this.label10.TabIndex = 12;
            this.label10.Text = "程序名：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSvcAddress
            // 
            this.txtBoxWebSvcAddress.Location = new System.Drawing.Point(73, 20);
            this.txtBoxWebSvcAddress.Name = "txtBoxWebSvcAddress";
            this.txtBoxWebSvcAddress.Size = new System.Drawing.Size(200, 21);
            this.txtBoxWebSvcAddress.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "地址：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSvcMethods
            // 
            this.txtBoxWebSvcMethods.Location = new System.Drawing.Point(73, 74);
            this.txtBoxWebSvcMethods.Name = "txtBoxWebSvcMethods";
            this.txtBoxWebSvcMethods.Size = new System.Drawing.Size(200, 21);
            this.txtBoxWebSvcMethods.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 22);
            this.label11.TabIndex = 14;
            this.label11.Text = "方法名：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(524, 489);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupOBDSetting);
            this.Controls.Add(this.groupDBandMES);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupELM.ResumeLayout(false);
            this.groupHardware.ResumeLayout(false);
            this.groupComm.ResumeLayout(false);
            this.groupOBDSetting.ResumeLayout(false);
            this.groupDBandMES.ResumeLayout(false);
            this.groupDBandMES.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPorts;
        private System.Windows.Forms.GroupBox groupELM;
        private System.Windows.Forms.ComboBox comboInitialize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboProtocol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBaud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupHardware;
        private System.Windows.Forms.ComboBox comboHardware;
        private System.Windows.Forms.GroupBox groupComm;
        private System.Windows.Forms.CheckBox checkBoxAutoDetect;
        private System.Windows.Forms.GroupBox groupOBDSetting;
        private System.Windows.Forms.GroupBox groupDBandMES;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBoxWebSvcName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBoxWebSvcMethods;
        private System.Windows.Forms.Label label11;
    }
}