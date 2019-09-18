﻿namespace SH_OBD {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.GridViewInfo = new System.Windows.Forms.DataGridView();
            this.groupIUPR = new System.Windows.Forms.GroupBox();
            this.GridViewIUPR = new System.Windows.Forms.DataGridView();
            this.groupInstantData = new System.Windows.Forms.GroupBox();
            this.GridViewInstantData = new System.Windows.Forms.DataGridView();
            this.btnStartOBDTest = new System.Windows.Forms.Button();
            this.groupECUInfo = new System.Windows.Forms.GroupBox();
            this.GridViewECUInfo = new System.Windows.Forms.DataGridView();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelMESInfo = new System.Windows.Forms.Label();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).BeginInit();
            this.groupIUPR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewIUPR)).BeginInit();
            this.groupInstantData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInstantData)).BeginInit();
            this.groupECUInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.GridViewInfo);
            this.groupInfo.Location = new System.Drawing.Point(10, 41);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(322, 230);
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
            this.GridViewInfo.RowTemplate.Height = 23;
            this.GridViewInfo.Size = new System.Drawing.Size(316, 210);
            this.GridViewInfo.TabIndex = 0;
            // 
            // groupIUPR
            // 
            this.groupIUPR.Controls.Add(this.GridViewIUPR);
            this.groupIUPR.Location = new System.Drawing.Point(338, 41);
            this.groupIUPR.Name = "groupIUPR";
            this.groupIUPR.Size = new System.Drawing.Size(453, 230);
            this.groupIUPR.TabIndex = 4;
            this.groupIUPR.TabStop = false;
            this.groupIUPR.Text = "IUPR";
            // 
            // GridViewIUPR
            // 
            this.GridViewIUPR.AllowUserToAddRows = false;
            this.GridViewIUPR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewIUPR.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewIUPR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewIUPR.DefaultCellStyle = dataGridViewCellStyle2;
            this.GridViewIUPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewIUPR.Location = new System.Drawing.Point(3, 17);
            this.GridViewIUPR.Name = "GridViewIUPR";
            this.GridViewIUPR.ReadOnly = true;
            this.GridViewIUPR.RowHeadersVisible = false;
            this.GridViewIUPR.RowTemplate.Height = 23;
            this.GridViewIUPR.Size = new System.Drawing.Size(447, 210);
            this.GridViewIUPR.TabIndex = 0;
            // 
            // groupInstantData
            // 
            this.groupInstantData.Controls.Add(this.GridViewInstantData);
            this.groupInstantData.Location = new System.Drawing.Point(338, 277);
            this.groupInstantData.Name = "groupInstantData";
            this.groupInstantData.Size = new System.Drawing.Size(453, 160);
            this.groupInstantData.TabIndex = 5;
            this.groupInstantData.TabStop = false;
            this.groupInstantData.Text = "实时数据流";
            this.groupInstantData.Visible = false;
            // 
            // GridViewInstantData
            // 
            this.GridViewInstantData.AllowUserToAddRows = false;
            this.GridViewInstantData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridViewInstantData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridViewInstantData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewInstantData.DefaultCellStyle = dataGridViewCellStyle3;
            this.GridViewInstantData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewInstantData.Location = new System.Drawing.Point(3, 17);
            this.GridViewInstantData.Name = "GridViewInstantData";
            this.GridViewInstantData.ReadOnly = true;
            this.GridViewInstantData.RowHeadersVisible = false;
            this.GridViewInstantData.RowTemplate.Height = 23;
            this.GridViewInstantData.Size = new System.Drawing.Size(447, 140);
            this.GridViewInstantData.TabIndex = 0;
            // 
            // btnStartOBDTest
            // 
            this.btnStartOBDTest.Location = new System.Drawing.Point(10, 10);
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
            this.groupECUInfo.Location = new System.Drawing.Point(10, 277);
            this.groupECUInfo.Name = "groupECUInfo";
            this.groupECUInfo.Size = new System.Drawing.Size(322, 159);
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
            this.GridViewECUInfo.RowTemplate.Height = 23;
            this.GridViewECUInfo.Size = new System.Drawing.Size(316, 139);
            this.GridViewECUInfo.TabIndex = 0;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(136, 16);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(71, 12);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Text = "准备OBD检测";
            // 
            // labelMESInfo
            // 
            this.labelMESInfo.AutoSize = true;
            this.labelMESInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMESInfo.Location = new System.Drawing.Point(336, 16);
            this.labelMESInfo.Name = "labelMESInfo";
            this.labelMESInfo.Size = new System.Drawing.Size(71, 12);
            this.labelMESInfo.TabIndex = 8;
            this.labelMESInfo.Text = "准备上传MES";
            // 
            // OBDTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelMESInfo);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.groupECUInfo);
            this.Controls.Add(this.btnStartOBDTest);
            this.Controls.Add(this.groupInstantData);
            this.Controls.Add(this.groupIUPR);
            this.Controls.Add(this.groupInfo);
            this.Name = "OBDTestForm";
            this.Text = "OBDTestForm";
            this.Load += new System.EventHandler(this.OBDTestForm_Load);
            this.VisibleChanged += new System.EventHandler(this.OBDTestForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.OBDTestForm_Resize);
            this.groupInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInfo)).EndInit();
            this.groupIUPR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewIUPR)).EndInit();
            this.groupInstantData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewInstantData)).EndInit();
            this.groupECUInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewECUInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.GroupBox groupIUPR;
        private System.Windows.Forms.GroupBox groupInstantData;
        private System.Windows.Forms.DataGridView GridViewInfo;
        private System.Windows.Forms.DataGridView GridViewIUPR;
        private System.Windows.Forms.DataGridView GridViewInstantData;
        private System.Windows.Forms.Button btnStartOBDTest;
        private System.Windows.Forms.GroupBox groupECUInfo;
        private System.Windows.Forms.DataGridView GridViewECUInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelMESInfo;
    }
}