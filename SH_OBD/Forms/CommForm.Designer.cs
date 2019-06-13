namespace SH_OBD {
    partial class CommForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommForm));
            this.comboProfile = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblBuildList = new System.Windows.Forms.Label();
            this.picCheck4 = new System.Windows.Forms.PictureBox();
            this.lblInitInterface = new System.Windows.Forms.Label();
            this.picCheck3 = new System.Windows.Forms.PictureBox();
            this.lblDetectInterface = new System.Windows.Forms.Label();
            this.picCheck2 = new System.Windows.Forms.PictureBox();
            this.lblCheckComPort = new System.Windows.Forms.Label();
            this.picCheck1 = new System.Windows.Forms.PictureBox();
            this.panelVersion = new System.Windows.Forms.Panel();
            this.picBlankBox = new System.Windows.Forms.PictureBox();
            this.picX = new System.Windows.Forms.PictureBox();
            this.picCheckMark = new System.Windows.Forms.PictureBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblInstruction4 = new System.Windows.Forms.Label();
            this.picDiagram = new System.Windows.Forms.PictureBox();
            this.lblInstruction5 = new System.Windows.Forms.Label();
            this.lblInstruction3 = new System.Windows.Forms.Label();
            this.lblActiveProfile = new System.Windows.Forms.Label();
            this.lblInstruction2 = new System.Windows.Forms.Label();
            this.m_timer = new System.Windows.Forms.Timer(this.components);
            this.lblInstruction1 = new System.Windows.Forms.Label();
            this.lblBullet5 = new System.Windows.Forms.Label();
            this.lblBullet4 = new System.Windows.Forms.Label();
            this.lblBullet3 = new System.Windows.Forms.Label();
            this.lblBullet2 = new System.Windows.Forms.Label();
            this.lblBullet1 = new System.Windows.Forms.Label();
            this.btnManageProfiles = new System.Windows.Forms.Button();
            this.panelStatus = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck1)).BeginInit();
            this.panelVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBlankBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiagram)).BeginInit();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboProfile
            // 
            this.comboProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProfile.Location = new System.Drawing.Point(257, 103);
            this.comboProfile.Name = "comboProfile";
            this.comboProfile.Size = new System.Drawing.Size(259, 20);
            this.comboProfile.TabIndex = 12;
            this.comboProfile.SelectedIndexChanged += new System.EventHandler(this.comboProfile_SelectedValueChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(491, 76);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(223, 43);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Disconnected";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblBuildList
            // 
            this.lblBuildList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBuildList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildList.ForeColor = System.Drawing.Color.Blue;
            this.lblBuildList.Location = new System.Drawing.Point(491, 204);
            this.lblBuildList.Name = "lblBuildList";
            this.lblBuildList.Size = new System.Drawing.Size(228, 43);
            this.lblBuildList.TabIndex = 18;
            this.lblBuildList.Text = "Initialize OBD-II";
            this.lblBuildList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblBuildList.Visible = false;
            // 
            // picCheck4
            // 
            this.picCheck4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picCheck4.Image = ((System.Drawing.Image)(resources.GetObject("picCheck4.Image")));
            this.picCheck4.Location = new System.Drawing.Point(431, 204);
            this.picCheck4.Name = "picCheck4";
            this.picCheck4.Size = new System.Drawing.Size(56, 43);
            this.picCheck4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheck4.TabIndex = 17;
            this.picCheck4.TabStop = false;
            this.picCheck4.Visible = false;
            // 
            // lblInitInterface
            // 
            this.lblInitInterface.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInitInterface.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitInterface.ForeColor = System.Drawing.Color.Blue;
            this.lblInitInterface.Location = new System.Drawing.Point(491, 161);
            this.lblInitInterface.Name = "lblInitInterface";
            this.lblInitInterface.Size = new System.Drawing.Size(228, 43);
            this.lblInitInterface.TabIndex = 16;
            this.lblInitInterface.Text = "Initialize Interface";
            this.lblInitInterface.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblInitInterface.Visible = false;
            // 
            // picCheck3
            // 
            this.picCheck3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picCheck3.Image = ((System.Drawing.Image)(resources.GetObject("picCheck3.Image")));
            this.picCheck3.Location = new System.Drawing.Point(431, 161);
            this.picCheck3.Name = "picCheck3";
            this.picCheck3.Size = new System.Drawing.Size(56, 43);
            this.picCheck3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheck3.TabIndex = 15;
            this.picCheck3.TabStop = false;
            this.picCheck3.Visible = false;
            // 
            // lblDetectInterface
            // 
            this.lblDetectInterface.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDetectInterface.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetectInterface.ForeColor = System.Drawing.Color.Blue;
            this.lblDetectInterface.Location = new System.Drawing.Point(491, 119);
            this.lblDetectInterface.Name = "lblDetectInterface";
            this.lblDetectInterface.Size = new System.Drawing.Size(228, 42);
            this.lblDetectInterface.TabIndex = 14;
            this.lblDetectInterface.Text = "Detect Interface";
            this.lblDetectInterface.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblDetectInterface.Visible = false;
            // 
            // picCheck2
            // 
            this.picCheck2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picCheck2.Image = ((System.Drawing.Image)(resources.GetObject("picCheck2.Image")));
            this.picCheck2.Location = new System.Drawing.Point(431, 119);
            this.picCheck2.Name = "picCheck2";
            this.picCheck2.Size = new System.Drawing.Size(56, 42);
            this.picCheck2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheck2.TabIndex = 13;
            this.picCheck2.TabStop = false;
            this.picCheck2.Visible = false;
            // 
            // lblCheckComPort
            // 
            this.lblCheckComPort.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCheckComPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckComPort.ForeColor = System.Drawing.Color.Blue;
            this.lblCheckComPort.Location = new System.Drawing.Point(491, 76);
            this.lblCheckComPort.Name = "lblCheckComPort";
            this.lblCheckComPort.Size = new System.Drawing.Size(228, 43);
            this.lblCheckComPort.TabIndex = 12;
            this.lblCheckComPort.Text = "Open Serial Port";
            this.lblCheckComPort.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblCheckComPort.Visible = false;
            // 
            // picCheck1
            // 
            this.picCheck1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picCheck1.Image = ((System.Drawing.Image)(resources.GetObject("picCheck1.Image")));
            this.picCheck1.Location = new System.Drawing.Point(431, 76);
            this.picCheck1.Name = "picCheck1";
            this.picCheck1.Size = new System.Drawing.Size(56, 43);
            this.picCheck1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheck1.TabIndex = 11;
            this.picCheck1.TabStop = false;
            this.picCheck1.Visible = false;
            // 
            // panelVersion
            // 
            this.panelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelVersion.BackColor = System.Drawing.Color.White;
            this.panelVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVersion.Controls.Add(this.picBlankBox);
            this.panelVersion.Controls.Add(this.picX);
            this.panelVersion.Controls.Add(this.picCheckMark);
            this.panelVersion.Location = new System.Drawing.Point(12, 12);
            this.panelVersion.Name = "panelVersion";
            this.panelVersion.Size = new System.Drawing.Size(776, 82);
            this.panelVersion.TabIndex = 17;
            // 
            // picBlankBox
            // 
            this.picBlankBox.Image = ((System.Drawing.Image)(resources.GetObject("picBlankBox.Image")));
            this.picBlankBox.Location = new System.Drawing.Point(344, 20);
            this.picBlankBox.Name = "picBlankBox";
            this.picBlankBox.Size = new System.Drawing.Size(56, 43);
            this.picBlankBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBlankBox.TabIndex = 17;
            this.picBlankBox.TabStop = false;
            this.picBlankBox.Visible = false;
            // 
            // picX
            // 
            this.picX.Image = ((System.Drawing.Image)(resources.GetObject("picX.Image")));
            this.picX.Location = new System.Drawing.Point(274, 20);
            this.picX.Name = "picX";
            this.picX.Size = new System.Drawing.Size(56, 43);
            this.picX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picX.TabIndex = 16;
            this.picX.TabStop = false;
            this.picX.Visible = false;
            // 
            // picCheckMark
            // 
            this.picCheckMark.Image = ((System.Drawing.Image)(resources.GetObject("picCheckMark.Image")));
            this.picCheckMark.Location = new System.Drawing.Point(205, 20);
            this.picCheckMark.Name = "picCheckMark";
            this.picCheckMark.Size = new System.Drawing.Size(55, 43);
            this.picCheckMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheckMark.TabIndex = 15;
            this.picCheckMark.TabStop = false;
            this.picCheckMark.Visible = false;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(419, 452);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(90, 25);
            this.btnDisconnect.TabIndex = 16;
            this.btnDisconnect.Text = "断开(&D)";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConnect.Location = new System.Drawing.Point(313, 452);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 25);
            this.btnConnect.TabIndex = 15;
            this.btnConnect.Text = "连接(&C)";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblInstruction4
            // 
            this.lblInstruction4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstruction4.Location = new System.Drawing.Point(77, 231);
            this.lblInstruction4.Name = "lblInstruction4";
            this.lblInstruction4.Size = new System.Drawing.Size(318, 33);
            this.lblInstruction4.TabIndex = 10;
            this.lblInstruction4.Text = "Turn the vehicle\'s ignition key to the ON position or start the engine.";
            // 
            // picDiagram
            // 
            this.picDiagram.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picDiagram.Image = ((System.Drawing.Image)(resources.GetObject("picDiagram.Image")));
            this.picDiagram.Location = new System.Drawing.Point(113, 3);
            this.picDiagram.Name = "picDiagram";
            this.picDiagram.Size = new System.Drawing.Size(528, 72);
            this.picDiagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDiagram.TabIndex = 9;
            this.picDiagram.TabStop = false;
            // 
            // lblInstruction5
            // 
            this.lblInstruction5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstruction5.Location = new System.Drawing.Point(77, 274);
            this.lblInstruction5.Name = "lblInstruction5";
            this.lblInstruction5.Size = new System.Drawing.Size(318, 22);
            this.lblInstruction5.TabIndex = 8;
            this.lblInstruction5.Text = "Click the \"Connect\" button.";
            // 
            // lblInstruction3
            // 
            this.lblInstruction3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstruction3.Location = new System.Drawing.Point(77, 189);
            this.lblInstruction3.Name = "lblInstruction3";
            this.lblInstruction3.Size = new System.Drawing.Size(318, 32);
            this.lblInstruction3.TabIndex = 7;
            this.lblInstruction3.Text = "Connect the interface hardware between this computer and the vehicle\'s OBD-II con" +
    "nector.";
            // 
            // lblActiveProfile
            // 
            this.lblActiveProfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblActiveProfile.Location = new System.Drawing.Point(102, 102);
            this.lblActiveProfile.Name = "lblActiveProfile";
            this.lblActiveProfile.Size = new System.Drawing.Size(139, 25);
            this.lblActiveProfile.TabIndex = 14;
            this.lblActiveProfile.Text = "当前车辆配置(&P):";
            this.lblActiveProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInstruction2
            // 
            this.lblInstruction2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstruction2.Location = new System.Drawing.Point(77, 124);
            this.lblInstruction2.Name = "lblInstruction2";
            this.lblInstruction2.Size = new System.Drawing.Size(318, 53);
            this.lblInstruction2.TabIndex = 6;
            this.lblInstruction2.Text = "Verify that the Active Vehicle Profile selected above applies to this vehicle. If" +
    " this is the first time you have used ProScan on this vehicle, click \"Manage Pro" +
    "files\" to create a new profile.";
            // 
            // lblInstruction1
            // 
            this.lblInstruction1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstruction1.Location = new System.Drawing.Point(77, 81);
            this.lblInstruction1.Name = "lblInstruction1";
            this.lblInstruction1.Size = new System.Drawing.Size(318, 32);
            this.lblInstruction1.TabIndex = 5;
            this.lblInstruction1.Text = "Verify that you have the correct hardware and communication settings defined unde" +
    "r preferences.";
            // 
            // lblBullet5
            // 
            this.lblBullet5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBullet5.Image = ((System.Drawing.Image)(resources.GetObject("lblBullet5.Image")));
            this.lblBullet5.Location = new System.Drawing.Point(41, 270);
            this.lblBullet5.Name = "lblBullet5";
            this.lblBullet5.Size = new System.Drawing.Size(22, 20);
            this.lblBullet5.TabIndex = 4;
            // 
            // lblBullet4
            // 
            this.lblBullet4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBullet4.Image = ((System.Drawing.Image)(resources.GetObject("lblBullet4.Image")));
            this.lblBullet4.Location = new System.Drawing.Point(41, 231);
            this.lblBullet4.Name = "lblBullet4";
            this.lblBullet4.Size = new System.Drawing.Size(22, 20);
            this.lblBullet4.TabIndex = 3;
            // 
            // lblBullet3
            // 
            this.lblBullet3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBullet3.Image = ((System.Drawing.Image)(resources.GetObject("lblBullet3.Image")));
            this.lblBullet3.Location = new System.Drawing.Point(41, 189);
            this.lblBullet3.Name = "lblBullet3";
            this.lblBullet3.Size = new System.Drawing.Size(22, 19);
            this.lblBullet3.TabIndex = 2;
            // 
            // lblBullet2
            // 
            this.lblBullet2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBullet2.Image = ((System.Drawing.Image)(resources.GetObject("lblBullet2.Image")));
            this.lblBullet2.Location = new System.Drawing.Point(41, 124);
            this.lblBullet2.Name = "lblBullet2";
            this.lblBullet2.Size = new System.Drawing.Size(22, 19);
            this.lblBullet2.TabIndex = 1;
            // 
            // lblBullet1
            // 
            this.lblBullet1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBullet1.Image = ((System.Drawing.Image)(resources.GetObject("lblBullet1.Image")));
            this.lblBullet1.Location = new System.Drawing.Point(41, 81);
            this.lblBullet1.Name = "lblBullet1";
            this.lblBullet1.Size = new System.Drawing.Size(22, 19);
            this.lblBullet1.TabIndex = 0;
            // 
            // btnManageProfiles
            // 
            this.btnManageProfiles.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnManageProfiles.Location = new System.Drawing.Point(532, 102);
            this.btnManageProfiles.Name = "btnManageProfiles";
            this.btnManageProfiles.Size = new System.Drawing.Size(122, 25);
            this.btnManageProfiles.TabIndex = 13;
            this.btnManageProfiles.Text = "管理车辆配置(&M)";
            this.btnManageProfiles.Click += new System.EventHandler(this.btnManageProfiles_Click);
            // 
            // panelStatus
            // 
            this.panelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStatus.AutoScroll = true;
            this.panelStatus.BackColor = System.Drawing.Color.White;
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Controls.Add(this.lblBuildList);
            this.panelStatus.Controls.Add(this.picCheck4);
            this.panelStatus.Controls.Add(this.lblInitInterface);
            this.panelStatus.Controls.Add(this.picCheck3);
            this.panelStatus.Controls.Add(this.lblDetectInterface);
            this.panelStatus.Controls.Add(this.picCheck2);
            this.panelStatus.Controls.Add(this.lblCheckComPort);
            this.panelStatus.Controls.Add(this.picCheck1);
            this.panelStatus.Controls.Add(this.lblInstruction4);
            this.panelStatus.Controls.Add(this.picDiagram);
            this.panelStatus.Controls.Add(this.lblInstruction5);
            this.panelStatus.Controls.Add(this.lblInstruction3);
            this.panelStatus.Controls.Add(this.lblInstruction2);
            this.panelStatus.Controls.Add(this.lblInstruction1);
            this.panelStatus.Controls.Add(this.lblBullet5);
            this.panelStatus.Controls.Add(this.lblBullet4);
            this.panelStatus.Controls.Add(this.lblBullet3);
            this.panelStatus.Controls.Add(this.lblBullet2);
            this.panelStatus.Controls.Add(this.lblBullet1);
            this.panelStatus.Location = new System.Drawing.Point(12, 133);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(776, 313);
            this.panelStatus.TabIndex = 11;
            // 
            // CommForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 489);
            this.Controls.Add(this.comboProfile);
            this.Controls.Add(this.panelVersion);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblActiveProfile);
            this.Controls.Add(this.btnManageProfiles);
            this.Controls.Add(this.panelStatus);
            this.Name = "CommForm";
            this.Text = "CommForm";
            ((System.ComponentModel.ISupportInitialize)(this.picCheck4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheck1)).EndInit();
            this.panelVersion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBlankBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiagram)).EndInit();
            this.panelStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboProfile;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblBuildList;
        private System.Windows.Forms.PictureBox picCheck4;
        private System.Windows.Forms.Label lblInitInterface;
        private System.Windows.Forms.PictureBox picCheck3;
        private System.Windows.Forms.Label lblDetectInterface;
        private System.Windows.Forms.PictureBox picCheck2;
        private System.Windows.Forms.Label lblCheckComPort;
        private System.Windows.Forms.PictureBox picCheck1;
        private System.Windows.Forms.Panel panelVersion;
        private System.Windows.Forms.PictureBox picBlankBox;
        private System.Windows.Forms.PictureBox picX;
        private System.Windows.Forms.PictureBox picCheckMark;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblInstruction4;
        private System.Windows.Forms.PictureBox picDiagram;
        private System.Windows.Forms.Label lblInstruction5;
        private System.Windows.Forms.Label lblInstruction3;
        private System.Windows.Forms.Label lblActiveProfile;
        private System.Windows.Forms.Label lblInstruction2;
        private System.Windows.Forms.Timer m_timer;
        private System.Windows.Forms.Label lblInstruction1;
        private System.Windows.Forms.Label lblBullet5;
        private System.Windows.Forms.Label lblBullet4;
        private System.Windows.Forms.Label lblBullet3;
        private System.Windows.Forms.Label lblBullet2;
        private System.Windows.Forms.Label lblBullet1;
        private System.Windows.Forms.Button btnManageProfiles;
        private System.Windows.Forms.Panel panelStatus;
    }
}