namespace SH_OBD {
    partial class DTCForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DTCForm));
            this.picMilOn = new System.Windows.Forms.PictureBox();
            this.groupMIL = new System.Windows.Forms.GroupBox();
            this.lblMilStatus = new System.Windows.Forms.Label();
            this.picMIL = new System.Windows.Forms.PictureBox();
            this.picMilOff = new System.Windows.Forms.PictureBox();
            this.groupPermanent = new System.Windows.Forms.GroupBox();
            this.richTextPermanent = new System.Windows.Forms.RichTextBox();
            this.groupTotal = new System.Windows.Forms.GroupBox();
            this.lblTotalCodes = new System.Windows.Forms.Label();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupCodes = new System.Windows.Forms.GroupBox();
            this.richTextDTC = new System.Windows.Forms.RichTextBox();
            this.groupPending = new System.Windows.Forms.GroupBox();
            this.richTextPending = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMilOn)).BeginInit();
            this.groupMIL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMIL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMilOff)).BeginInit();
            this.groupPermanent.SuspendLayout();
            this.groupTotal.SuspendLayout();
            this.groupCodes.SuspendLayout();
            this.groupPending.SuspendLayout();
            this.SuspendLayout();
            // 
            // picMilOn
            // 
            this.picMilOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMilOn.Image = ((System.Drawing.Image)(resources.GetObject("picMilOn.Image")));
            this.picMilOn.Location = new System.Drawing.Point(18, 27);
            this.picMilOn.Name = "picMilOn";
            this.picMilOn.Size = new System.Drawing.Size(120, 54);
            this.picMilOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMilOn.TabIndex = 0;
            this.picMilOn.TabStop = false;
            this.picMilOn.Visible = false;
            // 
            // groupMIL
            // 
            this.groupMIL.Controls.Add(this.lblMilStatus);
            this.groupMIL.Controls.Add(this.picMIL);
            this.groupMIL.Controls.Add(this.picMilOff);
            this.groupMIL.Controls.Add(this.picMilOn);
            this.groupMIL.Location = new System.Drawing.Point(12, 12);
            this.groupMIL.Name = "groupMIL";
            this.groupMIL.Size = new System.Drawing.Size(156, 174);
            this.groupMIL.TabIndex = 8;
            this.groupMIL.TabStop = false;
            this.groupMIL.Text = "MIL 故障指示灯";
            // 
            // lblMilStatus
            // 
            this.lblMilStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMilStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMilStatus.Location = new System.Drawing.Point(18, 97);
            this.lblMilStatus.Name = "lblMilStatus";
            this.lblMilStatus.Size = new System.Drawing.Size(120, 54);
            this.lblMilStatus.TabIndex = 1;
            this.lblMilStatus.Text = "ON";
            this.lblMilStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picMIL
            // 
            this.picMIL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMIL.Location = new System.Drawing.Point(18, 27);
            this.picMIL.Name = "picMIL";
            this.picMIL.Size = new System.Drawing.Size(120, 54);
            this.picMIL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMIL.TabIndex = 3;
            this.picMIL.TabStop = false;
            // 
            // picMilOff
            // 
            this.picMilOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMilOff.Image = ((System.Drawing.Image)(resources.GetObject("picMilOff.Image")));
            this.picMilOff.Location = new System.Drawing.Point(18, 27);
            this.picMilOff.Name = "picMilOff";
            this.picMilOff.Size = new System.Drawing.Size(120, 54);
            this.picMilOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMilOff.TabIndex = 2;
            this.picMilOff.TabStop = false;
            this.picMilOff.Visible = false;
            // 
            // groupPermanent
            // 
            this.groupPermanent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPermanent.Controls.Add(this.richTextPermanent);
            this.groupPermanent.Location = new System.Drawing.Point(186, 380);
            this.groupPermanent.Name = "groupPermanent";
            this.groupPermanent.Size = new System.Drawing.Size(538, 174);
            this.groupPermanent.TabIndex = 9;
            this.groupPermanent.TabStop = false;
            this.groupPermanent.Text = "永久故障码（模式0A）";
            // 
            // richTextPermanent
            // 
            this.richTextPermanent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextPermanent.Location = new System.Drawing.Point(19, 26);
            this.richTextPermanent.Name = "richTextPermanent";
            this.richTextPermanent.ReadOnly = true;
            this.richTextPermanent.Size = new System.Drawing.Size(502, 129);
            this.richTextPermanent.TabIndex = 0;
            this.richTextPermanent.Text = "";
            // 
            // groupTotal
            // 
            this.groupTotal.Controls.Add(this.lblTotalCodes);
            this.groupTotal.Location = new System.Drawing.Point(12, 192);
            this.groupTotal.Name = "groupTotal";
            this.groupTotal.Size = new System.Drawing.Size(156, 97);
            this.groupTotal.TabIndex = 12;
            this.groupTotal.TabStop = false;
            this.groupTotal.Text = "故障码数量";
            // 
            // lblTotalCodes
            // 
            this.lblTotalCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCodes.Location = new System.Drawing.Point(18, 27);
            this.lblTotalCodes.Name = "lblTotalCodes";
            this.lblTotalCodes.Size = new System.Drawing.Size(120, 54);
            this.lblTotalCodes.TabIndex = 2;
            this.lblTotalCodes.Text = "0";
            this.lblTotalCodes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnErase
            // 
            this.btnErase.Location = new System.Drawing.Point(12, 341);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(156, 27);
            this.btnErase.TabIndex = 11;
            this.btnErase.Text = "清除故障码(&E)";
            this.btnErase.Visible = false;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 301);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(156, 27);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupCodes
            // 
            this.groupCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCodes.Controls.Add(this.richTextDTC);
            this.groupCodes.Location = new System.Drawing.Point(186, 12);
            this.groupCodes.Name = "groupCodes";
            this.groupCodes.Size = new System.Drawing.Size(538, 173);
            this.groupCodes.TabIndex = 13;
            this.groupCodes.TabStop = false;
            this.groupCodes.Text = "已存储故障码（模式03）";
            // 
            // richTextDTC
            // 
            this.richTextDTC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextDTC.Location = new System.Drawing.Point(20, 23);
            this.richTextDTC.Name = "richTextDTC";
            this.richTextDTC.ReadOnly = true;
            this.richTextDTC.Size = new System.Drawing.Size(501, 133);
            this.richTextDTC.TabIndex = 1;
            this.richTextDTC.Text = "";
            // 
            // groupPending
            // 
            this.groupPending.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPending.Controls.Add(this.richTextPending);
            this.groupPending.Location = new System.Drawing.Point(187, 195);
            this.groupPending.Name = "groupPending";
            this.groupPending.Size = new System.Drawing.Size(537, 174);
            this.groupPending.TabIndex = 14;
            this.groupPending.TabStop = false;
            this.groupPending.Text = "未决故障码（模式07）";
            // 
            // richTextPending
            // 
            this.richTextPending.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextPending.Location = new System.Drawing.Point(20, 23);
            this.richTextPending.Name = "richTextPending";
            this.richTextPending.ReadOnly = true;
            this.richTextPending.Size = new System.Drawing.Size(500, 134);
            this.richTextPending.TabIndex = 1;
            this.richTextPending.Text = "";
            // 
            // DTCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 563);
            this.Controls.Add(this.groupMIL);
            this.Controls.Add(this.groupPermanent);
            this.Controls.Add(this.groupTotal);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupCodes);
            this.Controls.Add(this.groupPending);
            this.Name = "DTCForm";
            this.Text = "DTCForm";
            this.Load += new System.EventHandler(this.DTCForm_Load);
            this.VisibleChanged += new System.EventHandler(this.DTCForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.DTCForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picMilOn)).EndInit();
            this.groupMIL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMIL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMilOff)).EndInit();
            this.groupPermanent.ResumeLayout(false);
            this.groupTotal.ResumeLayout(false);
            this.groupCodes.ResumeLayout(false);
            this.groupPending.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picMilOn;
        private System.Windows.Forms.GroupBox groupMIL;
        private System.Windows.Forms.Label lblMilStatus;
        private System.Windows.Forms.PictureBox picMIL;
        private System.Windows.Forms.PictureBox picMilOff;
        private System.Windows.Forms.GroupBox groupPermanent;
        private System.Windows.Forms.RichTextBox richTextPermanent;
        private System.Windows.Forms.GroupBox groupTotal;
        private System.Windows.Forms.Label lblTotalCodes;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox groupCodes;
        private System.Windows.Forms.RichTextBox richTextDTC;
        private System.Windows.Forms.GroupBox groupPending;
        private System.Windows.Forms.RichTextBox richTextPending;
    }
}