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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.GridViewInfo = new System.Windows.Forms.DataGridView();
            this.btnStartOBDTest = new System.Windows.Forms.Button();
            this.groupECUInfo = new System.Windows.Forms.GroupBox();
            this.GridViewECUInfo = new System.Windows.Forms.DataGridView();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelMESInfo = new System.Windows.Forms.Label();
            this.txtBoxVIN = new System.Windows.Forms.TextBox();
            this.chkBoxManualUpload = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.chkBoxShowData = new System.Windows.Forms.CheckBox();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).BeginInit();
            this.groupECUInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.GridViewInfo);
            this.groupInfo.Location = new System.Drawing.Point(10, 63);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(322, 375);
            this.groupInfo.TabIndex = 2;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "车辆信息";
            // 
            // GridViewInfo
            // 
            this.GridViewInfo.AllowUserToAddRows = false;
            this.GridViewInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.GridViewInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewInfo.Location = new System.Drawing.Point(3, 17);
            this.GridViewInfo.Name = "GridViewInfo";
            this.GridViewInfo.ReadOnly = true;
            this.GridViewInfo.RowHeadersVisible = false;
            this.GridViewInfo.RowHeadersWidth = 51;
            this.GridViewInfo.RowTemplate.Height = 23;
            this.GridViewInfo.Size = new System.Drawing.Size(316, 355);
            this.GridViewInfo.TabIndex = 0;
            // 
            // btnStartOBDTest
            // 
            this.btnStartOBDTest.Location = new System.Drawing.Point(10, 33);
            this.btnStartOBDTest.Name = "btnStartOBDTest";
            this.btnStartOBDTest.Size = new System.Drawing.Size(120, 24);
            this.btnStartOBDTest.TabIndex = 0;
            this.btnStartOBDTest.Text = "开始OBD检测(&S)";
            this.btnStartOBDTest.UseVisualStyleBackColor = true;
            this.btnStartOBDTest.Click += new System.EventHandler(this.BtnStartOBDTest_Click);
            // 
            // groupECUInfo
            // 
            this.groupECUInfo.Controls.Add(this.GridViewECUInfo);
            this.groupECUInfo.Location = new System.Drawing.Point(338, 63);
            this.groupECUInfo.Name = "groupECUInfo";
            this.groupECUInfo.Size = new System.Drawing.Size(450, 375);
            this.groupECUInfo.TabIndex = 3;
            this.groupECUInfo.TabStop = false;
            this.groupECUInfo.Text = "ECU信息";
            // 
            // GridViewECUInfo
            // 
            this.GridViewECUInfo.AllowUserToAddRows = false;
            this.GridViewECUInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewECUInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewECUInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewECUInfo.DefaultCellStyle = dataGridViewCellStyle4;
            this.GridViewECUInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewECUInfo.Location = new System.Drawing.Point(3, 17);
            this.GridViewECUInfo.Name = "GridViewECUInfo";
            this.GridViewECUInfo.ReadOnly = true;
            this.GridViewECUInfo.RowHeadersVisible = false;
            this.GridViewECUInfo.RowHeadersWidth = 51;
            this.GridViewECUInfo.RowTemplate.Height = 23;
            this.GridViewECUInfo.Size = new System.Drawing.Size(444, 355);
            this.GridViewECUInfo.TabIndex = 0;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(71, 12);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Text = "准备OBD检测";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMESInfo
            // 
            this.labelMESInfo.AutoSize = true;
            this.labelMESInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMESInfo.Location = new System.Drawing.Point(476, 9);
            this.labelMESInfo.Name = "labelMESInfo";
            this.labelMESInfo.Size = new System.Drawing.Size(77, 12);
            this.labelMESInfo.TabIndex = 8;
            this.labelMESInfo.Text = "准备上传数据";
            this.labelMESInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVIN
            // 
            this.txtBoxVIN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBoxVIN.Location = new System.Drawing.Point(135, 35);
            this.txtBoxVIN.Margin = new System.Windows.Forms.Padding(2);
            this.txtBoxVIN.Name = "txtBoxVIN";
            this.txtBoxVIN.Size = new System.Drawing.Size(121, 21);
            this.txtBoxVIN.TabIndex = 9;
            this.txtBoxVIN.Text = "等待扫描VIN号";
            this.txtBoxVIN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBoxVIN_KeyPress);
            // 
            // chkBoxManualUpload
            // 
            this.chkBoxManualUpload.AutoSize = true;
            this.chkBoxManualUpload.Location = new System.Drawing.Point(261, 38);
            this.chkBoxManualUpload.Name = "chkBoxManualUpload";
            this.chkBoxManualUpload.Size = new System.Drawing.Size(96, 16);
            this.chkBoxManualUpload.TabIndex = 10;
            this.chkBoxManualUpload.Text = "手动上传数据";
            this.chkBoxManualUpload.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(478, 33);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 24);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "导入Excel报表数据";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // chkBoxShowData
            // 
            this.chkBoxShowData.AutoSize = true;
            this.chkBoxShowData.Location = new System.Drawing.Point(364, 38);
            this.chkBoxShowData.Name = "chkBoxShowData";
            this.chkBoxShowData.Size = new System.Drawing.Size(108, 16);
            this.chkBoxShowData.TabIndex = 12;
            this.chkBoxShowData.Text = "仅查看已测数据";
            this.chkBoxShowData.UseVisualStyleBackColor = true;
            // 
            // OBDTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkBoxShowData);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.chkBoxManualUpload);
            this.Controls.Add(this.txtBoxVIN);
            this.Controls.Add(this.labelMESInfo);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.groupECUInfo);
            this.Controls.Add(this.btnStartOBDTest);
            this.Controls.Add(this.groupInfo);
            this.Name = "OBDTestForm";
            this.Text = "OBDTestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OBDTestForm_FormClosing);
            this.Load += new System.EventHandler(this.OBDTestForm_Load);
            this.VisibleChanged += new System.EventHandler(this.OBDTestForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.OBDTestForm_Resize);
            this.groupInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).EndInit();
            this.groupECUInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.DataGridView GridViewInfo;
        private System.Windows.Forms.Button btnStartOBDTest;
        private System.Windows.Forms.GroupBox groupECUInfo;
        private System.Windows.Forms.DataGridView GridViewECUInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelMESInfo;
        private System.Windows.Forms.TextBox txtBoxVIN;
        private System.Windows.Forms.CheckBox chkBoxManualUpload;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.CheckBox chkBoxShowData;
    }
}