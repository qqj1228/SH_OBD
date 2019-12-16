namespace SH_OBD {
    partial class OBDTestForm {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpBoxInfo = new System.Windows.Forms.GroupBox();
            this.GridViewInfo = new System.Windows.Forms.DataGridView();
            this.btnStartOBDTest = new System.Windows.Forms.Button();
            this.grpBoxECUInfo = new System.Windows.Forms.GroupBox();
            this.GridViewECUInfo = new System.Windows.Forms.DataGridView();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelMESInfo = new System.Windows.Forms.Label();
            this.txtBoxVIN = new System.Windows.Forms.TextBox();
            this.chkBoxManualUpload = new System.Windows.Forms.CheckBox();
            this.grpBoxIUPR = new System.Windows.Forms.GroupBox();
            this.GridViewIUPR = new System.Windows.Forms.DataGridView();
            this.grpBoxInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).BeginInit();
            this.grpBoxECUInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).BeginInit();
            this.grpBoxIUPR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewIUPR)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBoxInfo
            // 
            this.grpBoxInfo.Controls.Add(this.GridViewInfo);
            this.grpBoxInfo.Location = new System.Drawing.Point(10, 63);
            this.grpBoxInfo.Name = "grpBoxInfo";
            this.grpBoxInfo.Size = new System.Drawing.Size(322, 262);
            this.grpBoxInfo.TabIndex = 5;
            this.grpBoxInfo.TabStop = false;
            this.grpBoxInfo.Text = "车辆信息";
            // 
            // GridViewInfo
            // 
            this.GridViewInfo.AllowUserToAddRows = false;
            this.GridViewInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.GridViewInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewInfo.Location = new System.Drawing.Point(3, 17);
            this.GridViewInfo.Name = "GridViewInfo";
            this.GridViewInfo.ReadOnly = true;
            this.GridViewInfo.RowHeadersVisible = false;
            this.GridViewInfo.RowHeadersWidth = 51;
            this.GridViewInfo.RowTemplate.Height = 23;
            this.GridViewInfo.Size = new System.Drawing.Size(316, 242);
            this.GridViewInfo.TabIndex = 0;
            // 
            // btnStartOBDTest
            // 
            this.btnStartOBDTest.Location = new System.Drawing.Point(10, 33);
            this.btnStartOBDTest.Name = "btnStartOBDTest";
            this.btnStartOBDTest.Size = new System.Drawing.Size(120, 24);
            this.btnStartOBDTest.TabIndex = 2;
            this.btnStartOBDTest.Text = "开始OBD检测(&S)";
            this.btnStartOBDTest.UseVisualStyleBackColor = true;
            this.btnStartOBDTest.Click += new System.EventHandler(this.BtnStartOBDTest_Click);
            // 
            // grpBoxECUInfo
            // 
            this.grpBoxECUInfo.Controls.Add(this.GridViewECUInfo);
            this.grpBoxECUInfo.Location = new System.Drawing.Point(10, 331);
            this.grpBoxECUInfo.Name = "grpBoxECUInfo";
            this.grpBoxECUInfo.Size = new System.Drawing.Size(322, 107);
            this.grpBoxECUInfo.TabIndex = 6;
            this.grpBoxECUInfo.TabStop = false;
            this.grpBoxECUInfo.Text = "ECU信息";
            // 
            // GridViewECUInfo
            // 
            this.GridViewECUInfo.AllowUserToAddRows = false;
            this.GridViewECUInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewECUInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewECUInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewECUInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.GridViewECUInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewECUInfo.Location = new System.Drawing.Point(3, 17);
            this.GridViewECUInfo.Name = "GridViewECUInfo";
            this.GridViewECUInfo.ReadOnly = true;
            this.GridViewECUInfo.RowHeadersVisible = false;
            this.GridViewECUInfo.RowHeadersWidth = 51;
            this.GridViewECUInfo.RowTemplate.Height = 23;
            this.GridViewECUInfo.Size = new System.Drawing.Size(316, 87);
            this.GridViewECUInfo.TabIndex = 0;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(71, 12);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "准备OBD检测";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMESInfo
            // 
            this.labelMESInfo.AutoSize = true;
            this.labelMESInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMESInfo.Location = new System.Drawing.Point(487, 9);
            this.labelMESInfo.Name = "labelMESInfo";
            this.labelMESInfo.Size = new System.Drawing.Size(77, 12);
            this.labelMESInfo.TabIndex = 1;
            this.labelMESInfo.Text = "准备上传数据";
            this.labelMESInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVIN
            // 
            this.txtBoxVIN.Location = new System.Drawing.Point(135, 35);
            this.txtBoxVIN.Margin = new System.Windows.Forms.Padding(2);
            this.txtBoxVIN.Name = "txtBoxVIN";
            this.txtBoxVIN.Size = new System.Drawing.Size(121, 21);
            this.txtBoxVIN.TabIndex = 3;
            this.txtBoxVIN.Text = "等待扫描VIN号";
            this.txtBoxVIN.TextChanged += new System.EventHandler(this.TxtBoxVIN_TextChanged);
            // 
            // chkBoxManualUpload
            // 
            this.chkBoxManualUpload.AutoSize = true;
            this.chkBoxManualUpload.Location = new System.Drawing.Point(262, 38);
            this.chkBoxManualUpload.Name = "chkBoxManualUpload";
            this.chkBoxManualUpload.Size = new System.Drawing.Size(96, 16);
            this.chkBoxManualUpload.TabIndex = 4;
            this.chkBoxManualUpload.Text = "手动上传数据";
            this.chkBoxManualUpload.UseVisualStyleBackColor = true;
            // 
            // grpBoxIUPR
            // 
            this.grpBoxIUPR.Controls.Add(this.GridViewIUPR);
            this.grpBoxIUPR.Location = new System.Drawing.Point(339, 61);
            this.grpBoxIUPR.Name = "grpBoxIUPR";
            this.grpBoxIUPR.Size = new System.Drawing.Size(449, 377);
            this.grpBoxIUPR.TabIndex = 7;
            this.grpBoxIUPR.TabStop = false;
            this.grpBoxIUPR.Text = "IUPR";
            // 
            // GridViewIUPR
            // 
            this.GridViewIUPR.AllowUserToAddRows = false;
            this.GridViewIUPR.AllowUserToDeleteRows = false;
            this.GridViewIUPR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewIUPR.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewIUPR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewIUPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewIUPR.Location = new System.Drawing.Point(3, 17);
            this.GridViewIUPR.Name = "GridViewIUPR";
            this.GridViewIUPR.ReadOnly = true;
            this.GridViewIUPR.RowHeadersVisible = false;
            this.GridViewIUPR.RowTemplate.Height = 23;
            this.GridViewIUPR.Size = new System.Drawing.Size(443, 357);
            this.GridViewIUPR.TabIndex = 0;
            // 
            // OBDTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpBoxIUPR);
            this.Controls.Add(this.chkBoxManualUpload);
            this.Controls.Add(this.txtBoxVIN);
            this.Controls.Add(this.labelMESInfo);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.grpBoxECUInfo);
            this.Controls.Add(this.btnStartOBDTest);
            this.Controls.Add(this.grpBoxInfo);
            this.Name = "OBDTestForm";
            this.Text = "OBDTestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OBDTestForm_FormClosing);
            this.Load += new System.EventHandler(this.OBDTestForm_Load);
            this.VisibleChanged += new System.EventHandler(this.OBDTestForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.OBDTestForm_Resize);
            this.grpBoxInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).EndInit();
            this.grpBoxECUInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).EndInit();
            this.grpBoxIUPR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewIUPR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxInfo;
        private System.Windows.Forms.DataGridView GridViewInfo;
        private System.Windows.Forms.Button btnStartOBDTest;
        private System.Windows.Forms.GroupBox grpBoxECUInfo;
        private System.Windows.Forms.DataGridView GridViewECUInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelMESInfo;
        private System.Windows.Forms.TextBox txtBoxVIN;
        private System.Windows.Forms.CheckBox chkBoxManualUpload;
        private System.Windows.Forms.GroupBox grpBoxIUPR;
        private System.Windows.Forms.DataGridView GridViewIUPR;
    }
}