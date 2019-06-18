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
            this.groupELM.SuspendLayout();
            this.groupHardware.SuspendLayout();
            this.groupComm.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboPorts
            // 
            this.comboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPorts.Location = new System.Drawing.Point(20, 21);
            this.comboPorts.Name = "comboPorts";
            this.comboPorts.Size = new System.Drawing.Size(124, 20);
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
            this.groupELM.Location = new System.Drawing.Point(14, 116);
            this.groupELM.Name = "groupELM";
            this.groupELM.Size = new System.Drawing.Size(476, 124);
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
            this.comboInitialize.Location = new System.Drawing.Point(112, 86);
            this.comboInitialize.Name = "comboInitialize";
            this.comboInitialize.Size = new System.Drawing.Size(200, 20);
            this.comboInitialize.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "初始化方式(&I):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboProtocol
            // 
            this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocol.Location = new System.Drawing.Point(112, 54);
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Size = new System.Drawing.Size(348, 20);
            this.comboProtocol.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 22);
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
            this.comboBaud.Location = new System.Drawing.Point(112, 25);
            this.comboBaud.Name = "comboBaud";
            this.comboBaud.Size = new System.Drawing.Size(129, 20);
            this.comboBaud.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "波特率(&B):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(256, 250);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(160, 250);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "保存(&S)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupHardware
            // 
            this.groupHardware.Controls.Add(this.comboHardware);
            this.groupHardware.Location = new System.Drawing.Point(195, 46);
            this.groupHardware.Name = "groupHardware";
            this.groupHardware.Size = new System.Drawing.Size(295, 60);
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
            this.comboHardware.Location = new System.Drawing.Point(20, 21);
            this.comboHardware.Name = "comboHardware";
            this.comboHardware.Size = new System.Drawing.Size(257, 20);
            this.comboHardware.TabIndex = 0;
            this.comboHardware.SelectedIndexChanged += new System.EventHandler(this.comboHardware_SelectedIndexChanged);
            // 
            // groupComm
            // 
            this.groupComm.Controls.Add(this.comboPorts);
            this.groupComm.Location = new System.Drawing.Point(14, 46);
            this.groupComm.Name = "groupComm";
            this.groupComm.Size = new System.Drawing.Size(163, 60);
            this.groupComm.TabIndex = 7;
            this.groupComm.TabStop = false;
            this.groupComm.Text = "串口(&P)";
            // 
            // checkBoxAutoDetect
            // 
            this.checkBoxAutoDetect.Location = new System.Drawing.Point(12, 12);
            this.checkBoxAutoDetect.Name = "checkBoxAutoDetect";
            this.checkBoxAutoDetect.Size = new System.Drawing.Size(476, 25);
            this.checkBoxAutoDetect.TabIndex = 12;
            this.checkBoxAutoDetect.Text = "自动探测OBD连接设置";
            this.checkBoxAutoDetect.CheckedChanged += new System.EventHandler(this.checkBoxAutoDetect_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(504, 287);
            this.Controls.Add(this.groupELM);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupHardware);
            this.Controls.Add(this.groupComm);
            this.Controls.Add(this.checkBoxAutoDetect);
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
    }
}