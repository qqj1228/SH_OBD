namespace SH_OBD {
    partial class SpeedFactorCalcForm {
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtFactor = new System.Windows.Forms.TextBox();
            this.lblFactor = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCurrentRimDiameter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurrentAspectRatio = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCurrentTireWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStockTireWidth = new System.Windows.Forms.TextBox();
            this.lblTireWidth = new System.Windows.Forms.Label();
            this.lblTireWidthUnits = new System.Windows.Forms.Label();
            this.txtStockRimDiameter = new System.Windows.Forms.TextBox();
            this.lblRimDiameter = new System.Windows.Forms.Label();
            this.txtStockAspectRatio = new System.Windows.Forms.TextBox();
            this.lblRimDiameterUnits = new System.Windows.Forms.Label();
            this.lblTireAspectUnits = new System.Windows.Forms.Label();
            this.lblTireAspectRatio = new System.Windows.Forms.Label();
            this.groupWheels = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupWheels.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(242, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 26);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(144, 167);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存(&S)";
            // 
            // txtFactor
            // 
            this.txtFactor.Location = new System.Drawing.Point(242, 133);
            this.txtFactor.Name = "txtFactor";
            this.txtFactor.ReadOnly = true;
            this.txtFactor.Size = new System.Drawing.Size(120, 21);
            this.txtFactor.TabIndex = 14;
            this.txtFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFactor
            // 
            this.lblFactor.Location = new System.Drawing.Point(70, 133);
            this.lblFactor.Name = "lblFactor";
            this.lblFactor.Size = new System.Drawing.Size(164, 19);
            this.lblFactor.TabIndex = 13;
            this.lblFactor.Text = "计算所得的速度表标定因子:";
            this.lblFactor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCurrentRimDiameter);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtCurrentAspectRatio);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCurrentTireWidth);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(242, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 112);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主胎尺寸";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(174, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 22);
            this.label1.TabIndex = 15;
            this.label1.Text = "in";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(174, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "%";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(174, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 22);
            this.label3.TabIndex = 9;
            this.label3.Text = "mm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrentRimDiameter
            // 
            this.txtCurrentRimDiameter.Location = new System.Drawing.Point(108, 77);
            this.txtCurrentRimDiameter.Name = "txtCurrentRimDiameter";
            this.txtCurrentRimDiameter.Size = new System.Drawing.Size(60, 21);
            this.txtCurrentRimDiameter.TabIndex = 14;
            this.txtCurrentRimDiameter.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 22);
            this.label4.TabIndex = 13;
            this.label4.Text = "轮圈直径(&d):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCurrentAspectRatio
            // 
            this.txtCurrentAspectRatio.Location = new System.Drawing.Point(108, 51);
            this.txtCurrentAspectRatio.Name = "txtCurrentAspectRatio";
            this.txtCurrentAspectRatio.Size = new System.Drawing.Size(60, 21);
            this.txtCurrentAspectRatio.TabIndex = 11;
            this.txtCurrentAspectRatio.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 22);
            this.label5.TabIndex = 10;
            this.label5.Text = "轮胎比率(&a):";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCurrentTireWidth
            // 
            this.txtCurrentTireWidth.Location = new System.Drawing.Point(108, 26);
            this.txtCurrentTireWidth.Name = "txtCurrentTireWidth";
            this.txtCurrentTireWidth.Size = new System.Drawing.Size(60, 21);
            this.txtCurrentTireWidth.TabIndex = 8;
            this.txtCurrentTireWidth.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 22);
            this.label6.TabIndex = 7;
            this.label6.Text = "轮胎宽度(&w):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStockTireWidth
            // 
            this.txtStockTireWidth.Location = new System.Drawing.Point(108, 26);
            this.txtStockTireWidth.Name = "txtStockTireWidth";
            this.txtStockTireWidth.Size = new System.Drawing.Size(60, 21);
            this.txtStockTireWidth.TabIndex = 8;
            this.txtStockTireWidth.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // lblTireWidth
            // 
            this.lblTireWidth.Location = new System.Drawing.Point(12, 26);
            this.lblTireWidth.Name = "lblTireWidth";
            this.lblTireWidth.Size = new System.Drawing.Size(90, 22);
            this.lblTireWidth.TabIndex = 7;
            this.lblTireWidth.Text = "轮胎宽度(&w):";
            this.lblTireWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTireWidthUnits
            // 
            this.lblTireWidthUnits.Location = new System.Drawing.Point(174, 26);
            this.lblTireWidthUnits.Name = "lblTireWidthUnits";
            this.lblTireWidthUnits.Size = new System.Drawing.Size(36, 22);
            this.lblTireWidthUnits.TabIndex = 9;
            this.lblTireWidthUnits.Text = "mm";
            this.lblTireWidthUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStockRimDiameter
            // 
            this.txtStockRimDiameter.Location = new System.Drawing.Point(108, 77);
            this.txtStockRimDiameter.Name = "txtStockRimDiameter";
            this.txtStockRimDiameter.Size = new System.Drawing.Size(60, 21);
            this.txtStockRimDiameter.TabIndex = 14;
            this.txtStockRimDiameter.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // lblRimDiameter
            // 
            this.lblRimDiameter.Location = new System.Drawing.Point(12, 77);
            this.lblRimDiameter.Name = "lblRimDiameter";
            this.lblRimDiameter.Size = new System.Drawing.Size(90, 22);
            this.lblRimDiameter.TabIndex = 13;
            this.lblRimDiameter.Text = "轮圈直径(&d):";
            this.lblRimDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStockAspectRatio
            // 
            this.txtStockAspectRatio.Location = new System.Drawing.Point(108, 51);
            this.txtStockAspectRatio.Name = "txtStockAspectRatio";
            this.txtStockAspectRatio.Size = new System.Drawing.Size(60, 21);
            this.txtStockAspectRatio.TabIndex = 11;
            this.txtStockAspectRatio.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // lblRimDiameterUnits
            // 
            this.lblRimDiameterUnits.Location = new System.Drawing.Point(174, 77);
            this.lblRimDiameterUnits.Name = "lblRimDiameterUnits";
            this.lblRimDiameterUnits.Size = new System.Drawing.Size(36, 22);
            this.lblRimDiameterUnits.TabIndex = 15;
            this.lblRimDiameterUnits.Text = "in";
            this.lblRimDiameterUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTireAspectUnits
            // 
            this.lblTireAspectUnits.Location = new System.Drawing.Point(174, 51);
            this.lblTireAspectUnits.Name = "lblTireAspectUnits";
            this.lblTireAspectUnits.Size = new System.Drawing.Size(36, 22);
            this.lblTireAspectUnits.TabIndex = 12;
            this.lblTireAspectUnits.Text = "%";
            this.lblTireAspectUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTireAspectRatio
            // 
            this.lblTireAspectRatio.Location = new System.Drawing.Point(12, 51);
            this.lblTireAspectRatio.Name = "lblTireAspectRatio";
            this.lblTireAspectRatio.Size = new System.Drawing.Size(90, 22);
            this.lblTireAspectRatio.TabIndex = 10;
            this.lblTireAspectRatio.Text = "轮胎比率(&a):";
            this.lblTireAspectRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupWheels
            // 
            this.groupWheels.Controls.Add(this.lblRimDiameterUnits);
            this.groupWheels.Controls.Add(this.lblTireAspectUnits);
            this.groupWheels.Controls.Add(this.lblTireWidthUnits);
            this.groupWheels.Controls.Add(this.txtStockRimDiameter);
            this.groupWheels.Controls.Add(this.lblRimDiameter);
            this.groupWheels.Controls.Add(this.txtStockAspectRatio);
            this.groupWheels.Controls.Add(this.lblTireAspectRatio);
            this.groupWheels.Controls.Add(this.txtStockTireWidth);
            this.groupWheels.Controls.Add(this.lblTireWidth);
            this.groupWheels.Location = new System.Drawing.Point(12, 12);
            this.groupWheels.Name = "groupWheels";
            this.groupWheels.Size = new System.Drawing.Size(222, 112);
            this.groupWheels.TabIndex = 11;
            this.groupWheels.TabStop = false;
            this.groupWheels.Text = "备胎尺寸";
            // 
            // SpeedFactorCalcForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(479, 202);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtFactor);
            this.Controls.Add(this.lblFactor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupWheels);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpeedFactorCalcForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "计算速度表标定因子";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupWheels.ResumeLayout(false);
            this.groupWheels.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtFactor;
        private System.Windows.Forms.Label lblFactor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCurrentRimDiameter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurrentAspectRatio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCurrentTireWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStockTireWidth;
        private System.Windows.Forms.Label lblTireWidth;
        private System.Windows.Forms.Label lblTireWidthUnits;
        private System.Windows.Forms.TextBox txtStockRimDiameter;
        private System.Windows.Forms.Label lblRimDiameter;
        private System.Windows.Forms.TextBox txtStockAspectRatio;
        private System.Windows.Forms.Label lblRimDiameterUnits;
        private System.Windows.Forms.Label lblTireAspectUnits;
        private System.Windows.Forms.Label lblTireAspectRatio;
        private System.Windows.Forms.GroupBox groupWheels;
    }
}