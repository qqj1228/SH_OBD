namespace SH_OBD {
    partial class ReportGeneratorForm {
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupVehicle = new System.Windows.Forms.GroupBox();
            this.txtVehicleModel = new System.Windows.Forms.TextBox();
            this.lblVehicleModel = new System.Windows.Forms.Label();
            this.txtVehicleMake = new System.Windows.Forms.TextBox();
            this.lblVehicleMake = new System.Windows.Forms.Label();
            this.txtVehicleYear = new System.Windows.Forms.TextBox();
            this.lblVehicleYear = new System.Windows.Forms.Label();
            this.txtForTelephone = new System.Windows.Forms.TextBox();
            this.lblForTelephone = new System.Windows.Forms.Label();
            this.txtForAddress2 = new System.Windows.Forms.TextBox();
            this.lblForAddress2 = new System.Windows.Forms.Label();
            this.txtForAddress1 = new System.Windows.Forms.TextBox();
            this.richTextStatus = new System.Windows.Forms.RichTextBox();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupPreparedFor = new System.Windows.Forms.GroupBox();
            this.lblForAddress1 = new System.Windows.Forms.Label();
            this.txtForName = new System.Windows.Forms.TextBox();
            this.lblForName = new System.Windows.Forms.Label();
            this.groupPreparedBy = new System.Windows.Forms.GroupBox();
            this.txtByTelephone = new System.Windows.Forms.TextBox();
            this.lblByTelephone = new System.Windows.Forms.Label();
            this.txtByAddress2 = new System.Windows.Forms.TextBox();
            this.lblByAddress2 = new System.Windows.Forms.Label();
            this.txtByAddress1 = new System.Windows.Forms.TextBox();
            this.lblByAddress1 = new System.Windows.Forms.Label();
            this.txtByName = new System.Windows.Forms.TextBox();
            this.lblByName = new System.Windows.Forms.Label();
            this.groupVehicle.SuspendLayout();
            this.groupStatus.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupPreparedFor.SuspendLayout();
            this.groupPreparedBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 49);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(318, 24);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "打开报表(&O)";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // groupVehicle
            // 
            this.groupVehicle.Controls.Add(this.txtVehicleModel);
            this.groupVehicle.Controls.Add(this.lblVehicleModel);
            this.groupVehicle.Controls.Add(this.txtVehicleMake);
            this.groupVehicle.Controls.Add(this.lblVehicleMake);
            this.groupVehicle.Controls.Add(this.txtVehicleYear);
            this.groupVehicle.Controls.Add(this.lblVehicleYear);
            this.groupVehicle.Location = new System.Drawing.Point(12, 371);
            this.groupVehicle.Name = "groupVehicle";
            this.groupVehicle.Size = new System.Drawing.Size(318, 112);
            this.groupVehicle.TabIndex = 2;
            this.groupVehicle.TabStop = false;
            this.groupVehicle.Text = "车辆信息";
            // 
            // txtVehicleModel
            // 
            this.txtVehicleModel.Location = new System.Drawing.Point(93, 76);
            this.txtVehicleModel.MaxLength = 35;
            this.txtVehicleModel.Name = "txtVehicleModel";
            this.txtVehicleModel.Size = new System.Drawing.Size(213, 21);
            this.txtVehicleModel.TabIndex = 5;
            // 
            // lblVehicleModel
            // 
            this.lblVehicleModel.Location = new System.Drawing.Point(12, 76);
            this.lblVehicleModel.Name = "lblVehicleModel";
            this.lblVehicleModel.Size = new System.Drawing.Size(75, 21);
            this.lblVehicleModel.TabIndex = 4;
            this.lblVehicleModel.Text = "车型:";
            this.lblVehicleModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVehicleMake
            // 
            this.txtVehicleMake.Location = new System.Drawing.Point(93, 49);
            this.txtVehicleMake.MaxLength = 35;
            this.txtVehicleMake.Name = "txtVehicleMake";
            this.txtVehicleMake.Size = new System.Drawing.Size(213, 21);
            this.txtVehicleMake.TabIndex = 3;
            // 
            // lblVehicleMake
            // 
            this.lblVehicleMake.Location = new System.Drawing.Point(12, 49);
            this.lblVehicleMake.Name = "lblVehicleMake";
            this.lblVehicleMake.Size = new System.Drawing.Size(75, 21);
            this.lblVehicleMake.TabIndex = 2;
            this.lblVehicleMake.Text = "制造厂:";
            this.lblVehicleMake.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVehicleYear
            // 
            this.txtVehicleYear.Location = new System.Drawing.Point(93, 21);
            this.txtVehicleYear.MaxLength = 4;
            this.txtVehicleYear.Name = "txtVehicleYear";
            this.txtVehicleYear.Size = new System.Drawing.Size(213, 21);
            this.txtVehicleYear.TabIndex = 1;
            // 
            // lblVehicleYear
            // 
            this.lblVehicleYear.Location = new System.Drawing.Point(12, 21);
            this.lblVehicleYear.Name = "lblVehicleYear";
            this.lblVehicleYear.Size = new System.Drawing.Size(75, 22);
            this.lblVehicleYear.TabIndex = 0;
            this.lblVehicleYear.Text = "出厂年份:";
            this.lblVehicleYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForTelephone
            // 
            this.txtForTelephone.Location = new System.Drawing.Point(93, 103);
            this.txtForTelephone.MaxLength = 35;
            this.txtForTelephone.Name = "txtForTelephone";
            this.txtForTelephone.Size = new System.Drawing.Size(213, 21);
            this.txtForTelephone.TabIndex = 7;
            // 
            // lblForTelephone
            // 
            this.lblForTelephone.Location = new System.Drawing.Point(12, 103);
            this.lblForTelephone.Name = "lblForTelephone";
            this.lblForTelephone.Size = new System.Drawing.Size(75, 21);
            this.lblForTelephone.TabIndex = 6;
            this.lblForTelephone.Text = "电话:";
            this.lblForTelephone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForAddress2
            // 
            this.txtForAddress2.Location = new System.Drawing.Point(93, 76);
            this.txtForAddress2.MaxLength = 35;
            this.txtForAddress2.Name = "txtForAddress2";
            this.txtForAddress2.Size = new System.Drawing.Size(213, 21);
            this.txtForAddress2.TabIndex = 5;
            // 
            // lblForAddress2
            // 
            this.lblForAddress2.Location = new System.Drawing.Point(12, 76);
            this.lblForAddress2.Name = "lblForAddress2";
            this.lblForAddress2.Size = new System.Drawing.Size(75, 21);
            this.lblForAddress2.TabIndex = 4;
            this.lblForAddress2.Text = "地址2:";
            this.lblForAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForAddress1
            // 
            this.txtForAddress1.Location = new System.Drawing.Point(93, 49);
            this.txtForAddress1.MaxLength = 35;
            this.txtForAddress1.Name = "txtForAddress1";
            this.txtForAddress1.Size = new System.Drawing.Size(213, 21);
            this.txtForAddress1.TabIndex = 3;
            // 
            // richTextStatus
            // 
            this.richTextStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextStatus.Location = new System.Drawing.Point(12, 51);
            this.richTextStatus.Name = "richTextStatus";
            this.richTextStatus.ReadOnly = true;
            this.richTextStatus.Size = new System.Drawing.Size(426, 453);
            this.richTextStatus.TabIndex = 1;
            this.richTextStatus.Text = "";
            // 
            // groupStatus
            // 
            this.groupStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupStatus.Controls.Add(this.progressBar);
            this.groupStatus.Controls.Add(this.richTextStatus);
            this.groupStatus.Location = new System.Drawing.Point(341, 11);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(451, 514);
            this.groupStatus.TabIndex = 4;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "状态";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 21);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(426, 26);
            this.progressBar.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.Controls.Add(this.btnGenerate);
            this.panel.Controls.Add(this.btnOpen);
            this.panel.Controls.Add(this.groupVehicle);
            this.panel.Controls.Add(this.groupPreparedFor);
            this.panel.Controls.Add(this.groupPreparedBy);
            this.panel.Controls.Add(this.groupStatus);
            this.panel.Location = new System.Drawing.Point(12, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(806, 527);
            this.panel.TabIndex = 7;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 16);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(318, 25);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "生成新报表(&G)";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupPreparedFor
            // 
            this.groupPreparedFor.Controls.Add(this.txtForTelephone);
            this.groupPreparedFor.Controls.Add(this.lblForTelephone);
            this.groupPreparedFor.Controls.Add(this.txtForAddress2);
            this.groupPreparedFor.Controls.Add(this.lblForAddress2);
            this.groupPreparedFor.Controls.Add(this.txtForAddress1);
            this.groupPreparedFor.Controls.Add(this.lblForAddress1);
            this.groupPreparedFor.Controls.Add(this.txtForName);
            this.groupPreparedFor.Controls.Add(this.lblForName);
            this.groupPreparedFor.Location = new System.Drawing.Point(12, 226);
            this.groupPreparedFor.Name = "groupPreparedFor";
            this.groupPreparedFor.Size = new System.Drawing.Size(318, 135);
            this.groupPreparedFor.TabIndex = 1;
            this.groupPreparedFor.TabStop = false;
            this.groupPreparedFor.Text = "接收服务方";
            // 
            // lblForAddress1
            // 
            this.lblForAddress1.Location = new System.Drawing.Point(12, 49);
            this.lblForAddress1.Name = "lblForAddress1";
            this.lblForAddress1.Size = new System.Drawing.Size(75, 21);
            this.lblForAddress1.TabIndex = 2;
            this.lblForAddress1.Text = "地址1:";
            this.lblForAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForName
            // 
            this.txtForName.Location = new System.Drawing.Point(93, 21);
            this.txtForName.MaxLength = 35;
            this.txtForName.Name = "txtForName";
            this.txtForName.Size = new System.Drawing.Size(213, 21);
            this.txtForName.TabIndex = 1;
            // 
            // lblForName
            // 
            this.lblForName.Location = new System.Drawing.Point(12, 21);
            this.lblForName.Name = "lblForName";
            this.lblForName.Size = new System.Drawing.Size(75, 22);
            this.lblForName.TabIndex = 0;
            this.lblForName.Text = "名称:";
            this.lblForName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupPreparedBy
            // 
            this.groupPreparedBy.Controls.Add(this.txtByTelephone);
            this.groupPreparedBy.Controls.Add(this.lblByTelephone);
            this.groupPreparedBy.Controls.Add(this.txtByAddress2);
            this.groupPreparedBy.Controls.Add(this.lblByAddress2);
            this.groupPreparedBy.Controls.Add(this.txtByAddress1);
            this.groupPreparedBy.Controls.Add(this.lblByAddress1);
            this.groupPreparedBy.Controls.Add(this.txtByName);
            this.groupPreparedBy.Controls.Add(this.lblByName);
            this.groupPreparedBy.Location = new System.Drawing.Point(12, 81);
            this.groupPreparedBy.Name = "groupPreparedBy";
            this.groupPreparedBy.Size = new System.Drawing.Size(318, 135);
            this.groupPreparedBy.TabIndex = 0;
            this.groupPreparedBy.TabStop = false;
            this.groupPreparedBy.Text = "提供服务方";
            // 
            // txtByTelephone
            // 
            this.txtByTelephone.Location = new System.Drawing.Point(93, 103);
            this.txtByTelephone.MaxLength = 35;
            this.txtByTelephone.Name = "txtByTelephone";
            this.txtByTelephone.Size = new System.Drawing.Size(213, 21);
            this.txtByTelephone.TabIndex = 7;
            // 
            // lblByTelephone
            // 
            this.lblByTelephone.Location = new System.Drawing.Point(12, 103);
            this.lblByTelephone.Name = "lblByTelephone";
            this.lblByTelephone.Size = new System.Drawing.Size(75, 21);
            this.lblByTelephone.TabIndex = 6;
            this.lblByTelephone.Text = "电话:";
            this.lblByTelephone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtByAddress2
            // 
            this.txtByAddress2.Location = new System.Drawing.Point(93, 76);
            this.txtByAddress2.MaxLength = 35;
            this.txtByAddress2.Name = "txtByAddress2";
            this.txtByAddress2.Size = new System.Drawing.Size(213, 21);
            this.txtByAddress2.TabIndex = 5;
            // 
            // lblByAddress2
            // 
            this.lblByAddress2.Location = new System.Drawing.Point(12, 76);
            this.lblByAddress2.Name = "lblByAddress2";
            this.lblByAddress2.Size = new System.Drawing.Size(75, 21);
            this.lblByAddress2.TabIndex = 4;
            this.lblByAddress2.Text = "地址2:";
            this.lblByAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtByAddress1
            // 
            this.txtByAddress1.Location = new System.Drawing.Point(93, 49);
            this.txtByAddress1.MaxLength = 35;
            this.txtByAddress1.Name = "txtByAddress1";
            this.txtByAddress1.Size = new System.Drawing.Size(213, 21);
            this.txtByAddress1.TabIndex = 3;
            // 
            // lblByAddress1
            // 
            this.lblByAddress1.Location = new System.Drawing.Point(12, 49);
            this.lblByAddress1.Name = "lblByAddress1";
            this.lblByAddress1.Size = new System.Drawing.Size(75, 21);
            this.lblByAddress1.TabIndex = 2;
            this.lblByAddress1.Text = "地址1:";
            this.lblByAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtByName
            // 
            this.txtByName.Location = new System.Drawing.Point(93, 21);
            this.txtByName.MaxLength = 35;
            this.txtByName.Name = "txtByName";
            this.txtByName.Size = new System.Drawing.Size(213, 21);
            this.txtByName.TabIndex = 1;
            // 
            // lblByName
            // 
            this.lblByName.Location = new System.Drawing.Point(12, 21);
            this.lblByName.Name = "lblByName";
            this.lblByName.Size = new System.Drawing.Size(75, 22);
            this.lblByName.TabIndex = 0;
            this.lblByName.Text = "名称:";
            this.lblByName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReportGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 551);
            this.Controls.Add(this.panel);
            this.Name = "ReportGeneratorForm";
            this.Text = "ReportGeneratorForm";
            this.Load += new System.EventHandler(this.ReportGeneratorForm_Load);
            this.VisibleChanged += new System.EventHandler(this.ReportGeneratorForm_VisibleChanged);
            this.groupVehicle.ResumeLayout(false);
            this.groupVehicle.PerformLayout();
            this.groupStatus.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.groupPreparedFor.ResumeLayout(false);
            this.groupPreparedFor.PerformLayout();
            this.groupPreparedBy.ResumeLayout(false);
            this.groupPreparedBy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.GroupBox groupVehicle;
        private System.Windows.Forms.TextBox txtVehicleModel;
        private System.Windows.Forms.Label lblVehicleModel;
        private System.Windows.Forms.TextBox txtVehicleMake;
        private System.Windows.Forms.Label lblVehicleMake;
        private System.Windows.Forms.TextBox txtVehicleYear;
        private System.Windows.Forms.Label lblVehicleYear;
        private System.Windows.Forms.TextBox txtForTelephone;
        private System.Windows.Forms.Label lblForTelephone;
        private System.Windows.Forms.TextBox txtForAddress2;
        private System.Windows.Forms.Label lblForAddress2;
        private System.Windows.Forms.TextBox txtForAddress1;
        private System.Windows.Forms.RichTextBox richTextStatus;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupPreparedFor;
        private System.Windows.Forms.Label lblForAddress1;
        private System.Windows.Forms.TextBox txtForName;
        private System.Windows.Forms.Label lblForName;
        private System.Windows.Forms.GroupBox groupPreparedBy;
        private System.Windows.Forms.TextBox txtByTelephone;
        private System.Windows.Forms.Label lblByTelephone;
        private System.Windows.Forms.TextBox txtByAddress2;
        private System.Windows.Forms.Label lblByAddress2;
        private System.Windows.Forms.TextBox txtByAddress1;
        private System.Windows.Forms.Label lblByAddress1;
        private System.Windows.Forms.TextBox txtByName;
        private System.Windows.Forms.Label lblByName;
    }
}