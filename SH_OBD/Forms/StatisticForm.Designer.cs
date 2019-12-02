namespace SH_OBD {
    partial class StatisticForm {
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
            this.grpBoxTime = new System.Windows.Forms.GroupBox();
            this.radioBtnMonth = new System.Windows.Forms.RadioButton();
            this.radioBtnWeek = new System.Windows.Forms.RadioButton();
            this.radioBtnDay = new System.Windows.Forms.RadioButton();
            this.GridContent = new System.Windows.Forms.DataGridView();
            this.grpBoxAllQty = new System.Windows.Forms.GroupBox();
            this.lblAllQty = new System.Windows.Forms.Label();
            this.grpBoxPassedQty = new System.Windows.Forms.GroupBox();
            this.lblPassedQty = new System.Windows.Forms.Label();
            this.grpBoxPassedRate = new System.Windows.Forms.GroupBox();
            this.lblPassedRate = new System.Windows.Forms.Label();
            this.grpBoxUploadedQty = new System.Windows.Forms.GroupBox();
            this.lblUploadedQty = new System.Windows.Forms.Label();
            this.grpBoxUploadedRate = new System.Windows.Forms.GroupBox();
            this.lblUploadedRate = new System.Windows.Forms.Label();
            this.grpBoxFilter = new System.Windows.Forms.GroupBox();
            this.chkBoxUploaded = new System.Windows.Forms.CheckBox();
            this.chkBoxPassed = new System.Windows.Forms.CheckBox();
            this.lblAllPage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.UpDownPage = new System.Windows.Forms.NumericUpDown();
            this.grpBoxTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).BeginInit();
            this.grpBoxAllQty.SuspendLayout();
            this.grpBoxPassedQty.SuspendLayout();
            this.grpBoxPassedRate.SuspendLayout();
            this.grpBoxUploadedQty.SuspendLayout();
            this.grpBoxUploadedRate.SuspendLayout();
            this.grpBoxFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownPage)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBoxTime
            // 
            this.grpBoxTime.Controls.Add(this.radioBtnMonth);
            this.grpBoxTime.Controls.Add(this.radioBtnWeek);
            this.grpBoxTime.Controls.Add(this.radioBtnDay);
            this.grpBoxTime.Location = new System.Drawing.Point(13, 13);
            this.grpBoxTime.Name = "grpBoxTime";
            this.grpBoxTime.Size = new System.Drawing.Size(100, 90);
            this.grpBoxTime.TabIndex = 0;
            this.grpBoxTime.TabStop = false;
            this.grpBoxTime.Text = "统计时间";
            // 
            // radioBtnMonth
            // 
            this.radioBtnMonth.AutoSize = true;
            this.radioBtnMonth.Location = new System.Drawing.Point(7, 65);
            this.radioBtnMonth.Name = "radioBtnMonth";
            this.radioBtnMonth.Size = new System.Drawing.Size(59, 16);
            this.radioBtnMonth.TabIndex = 2;
            this.radioBtnMonth.TabStop = true;
            this.radioBtnMonth.Text = "30天内";
            this.radioBtnMonth.UseVisualStyleBackColor = true;
            this.radioBtnMonth.Click += new System.EventHandler(this.Option_Click);
            // 
            // radioBtnWeek
            // 
            this.radioBtnWeek.AutoSize = true;
            this.radioBtnWeek.Location = new System.Drawing.Point(7, 43);
            this.radioBtnWeek.Name = "radioBtnWeek";
            this.radioBtnWeek.Size = new System.Drawing.Size(53, 16);
            this.radioBtnWeek.TabIndex = 1;
            this.radioBtnWeek.TabStop = true;
            this.radioBtnWeek.Text = "7天内";
            this.radioBtnWeek.UseVisualStyleBackColor = true;
            this.radioBtnWeek.Click += new System.EventHandler(this.Option_Click);
            // 
            // radioBtnDay
            // 
            this.radioBtnDay.AutoSize = true;
            this.radioBtnDay.Location = new System.Drawing.Point(7, 21);
            this.radioBtnDay.Name = "radioBtnDay";
            this.radioBtnDay.Size = new System.Drawing.Size(59, 16);
            this.radioBtnDay.TabIndex = 0;
            this.radioBtnDay.TabStop = true;
            this.radioBtnDay.Text = "当天内";
            this.radioBtnDay.UseVisualStyleBackColor = true;
            this.radioBtnDay.Click += new System.EventHandler(this.Option_Click);
            // 
            // GridContent
            // 
            this.GridContent.AllowUserToAddRows = false;
            this.GridContent.AllowUserToDeleteRows = false;
            this.GridContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridContent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.GridContent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.GridContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridContent.Location = new System.Drawing.Point(119, 39);
            this.GridContent.Name = "GridContent";
            this.GridContent.ReadOnly = true;
            this.GridContent.RowHeadersVisible = false;
            this.GridContent.RowTemplate.Height = 23;
            this.GridContent.Size = new System.Drawing.Size(453, 410);
            this.GridContent.TabIndex = 1;
            // 
            // grpBoxAllQty
            // 
            this.grpBoxAllQty.Controls.Add(this.lblAllQty);
            this.grpBoxAllQty.Location = new System.Drawing.Point(12, 185);
            this.grpBoxAllQty.Name = "grpBoxAllQty";
            this.grpBoxAllQty.Size = new System.Drawing.Size(100, 40);
            this.grpBoxAllQty.TabIndex = 2;
            this.grpBoxAllQty.TabStop = false;
            this.grpBoxAllQty.Text = "已测车辆总数";
            // 
            // lblAllQty
            // 
            this.lblAllQty.AutoSize = true;
            this.lblAllQty.Location = new System.Drawing.Point(8, 21);
            this.lblAllQty.Name = "lblAllQty";
            this.lblAllQty.Size = new System.Drawing.Size(77, 12);
            this.lblAllQty.TabIndex = 0;
            this.lblAllQty.Text = "1234567890万";
            // 
            // grpBoxPassedQty
            // 
            this.grpBoxPassedQty.Controls.Add(this.lblPassedQty);
            this.grpBoxPassedQty.Location = new System.Drawing.Point(12, 232);
            this.grpBoxPassedQty.Name = "grpBoxPassedQty";
            this.grpBoxPassedQty.Size = new System.Drawing.Size(100, 40);
            this.grpBoxPassedQty.TabIndex = 3;
            this.grpBoxPassedQty.TabStop = false;
            this.grpBoxPassedQty.Text = "合格数量";
            // 
            // lblPassedQty
            // 
            this.lblPassedQty.AutoSize = true;
            this.lblPassedQty.Location = new System.Drawing.Point(8, 20);
            this.lblPassedQty.Name = "lblPassedQty";
            this.lblPassedQty.Size = new System.Drawing.Size(77, 12);
            this.lblPassedQty.TabIndex = 0;
            this.lblPassedQty.Text = "1234567890万";
            // 
            // grpBoxPassedRate
            // 
            this.grpBoxPassedRate.Controls.Add(this.lblPassedRate);
            this.grpBoxPassedRate.Location = new System.Drawing.Point(12, 279);
            this.grpBoxPassedRate.Name = "grpBoxPassedRate";
            this.grpBoxPassedRate.Size = new System.Drawing.Size(100, 40);
            this.grpBoxPassedRate.TabIndex = 4;
            this.grpBoxPassedRate.TabStop = false;
            this.grpBoxPassedRate.Text = "合格率";
            // 
            // lblPassedRate
            // 
            this.lblPassedRate.AutoSize = true;
            this.lblPassedRate.Location = new System.Drawing.Point(8, 21);
            this.lblPassedRate.Name = "lblPassedRate";
            this.lblPassedRate.Size = new System.Drawing.Size(41, 12);
            this.lblPassedRate.TabIndex = 0;
            this.lblPassedRate.Text = "99.99%";
            // 
            // grpBoxUploadedQty
            // 
            this.grpBoxUploadedQty.Controls.Add(this.lblUploadedQty);
            this.grpBoxUploadedQty.Location = new System.Drawing.Point(12, 326);
            this.grpBoxUploadedQty.Name = "grpBoxUploadedQty";
            this.grpBoxUploadedQty.Size = new System.Drawing.Size(100, 40);
            this.grpBoxUploadedQty.TabIndex = 5;
            this.grpBoxUploadedQty.TabStop = false;
            this.grpBoxUploadedQty.Text = "已上传数量";
            // 
            // lblUploadedQty
            // 
            this.lblUploadedQty.AutoSize = true;
            this.lblUploadedQty.Location = new System.Drawing.Point(10, 21);
            this.lblUploadedQty.Name = "lblUploadedQty";
            this.lblUploadedQty.Size = new System.Drawing.Size(77, 12);
            this.lblUploadedQty.TabIndex = 0;
            this.lblUploadedQty.Text = "1234567890万";
            // 
            // grpBoxUploadedRate
            // 
            this.grpBoxUploadedRate.Controls.Add(this.lblUploadedRate);
            this.grpBoxUploadedRate.Location = new System.Drawing.Point(12, 373);
            this.grpBoxUploadedRate.Name = "grpBoxUploadedRate";
            this.grpBoxUploadedRate.Size = new System.Drawing.Size(100, 40);
            this.grpBoxUploadedRate.TabIndex = 6;
            this.grpBoxUploadedRate.TabStop = false;
            this.grpBoxUploadedRate.Text = "上传成功率";
            // 
            // lblUploadedRate
            // 
            this.lblUploadedRate.AutoSize = true;
            this.lblUploadedRate.Location = new System.Drawing.Point(10, 21);
            this.lblUploadedRate.Name = "lblUploadedRate";
            this.lblUploadedRate.Size = new System.Drawing.Size(41, 12);
            this.lblUploadedRate.TabIndex = 0;
            this.lblUploadedRate.Text = "99.99%";
            // 
            // grpBoxFilter
            // 
            this.grpBoxFilter.Controls.Add(this.chkBoxUploaded);
            this.grpBoxFilter.Controls.Add(this.chkBoxPassed);
            this.grpBoxFilter.Location = new System.Drawing.Point(12, 109);
            this.grpBoxFilter.Name = "grpBoxFilter";
            this.grpBoxFilter.Size = new System.Drawing.Size(100, 70);
            this.grpBoxFilter.TabIndex = 7;
            this.grpBoxFilter.TabStop = false;
            this.grpBoxFilter.Text = "显示车辆";
            // 
            // chkBoxUploaded
            // 
            this.chkBoxUploaded.AutoSize = true;
            this.chkBoxUploaded.Location = new System.Drawing.Point(6, 44);
            this.chkBoxUploaded.Name = "chkBoxUploaded";
            this.chkBoxUploaded.Size = new System.Drawing.Size(72, 16);
            this.chkBoxUploaded.TabIndex = 1;
            this.chkBoxUploaded.Text = "上传失败";
            this.chkBoxUploaded.UseVisualStyleBackColor = true;
            this.chkBoxUploaded.Click += new System.EventHandler(this.Option_Click);
            // 
            // chkBoxPassed
            // 
            this.chkBoxPassed.AutoSize = true;
            this.chkBoxPassed.Location = new System.Drawing.Point(7, 20);
            this.chkBoxPassed.Name = "chkBoxPassed";
            this.chkBoxPassed.Size = new System.Drawing.Size(60, 16);
            this.chkBoxPassed.TabIndex = 0;
            this.chkBoxPassed.Text = "不合格";
            this.chkBoxPassed.UseVisualStyleBackColor = true;
            this.chkBoxPassed.Click += new System.EventHandler(this.Option_Click);
            // 
            // lblAllPage
            // 
            this.lblAllPage.AutoSize = true;
            this.lblAllPage.Location = new System.Drawing.Point(198, 16);
            this.lblAllPage.Name = "lblAllPage";
            this.lblAllPage.Size = new System.Drawing.Size(83, 12);
            this.lblAllPage.TabIndex = 11;
            this.lblAllPage.Text = "页 / 共 99 页";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "第";
            // 
            // UpDownPage
            // 
            this.UpDownPage.Location = new System.Drawing.Point(142, 12);
            this.UpDownPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownPage.Name = "UpDownPage";
            this.UpDownPage.Size = new System.Drawing.Size(50, 21);
            this.UpDownPage.TabIndex = 13;
            this.UpDownPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownPage.ValueChanged += new System.EventHandler(this.Option_Click);
            // 
            // StatisticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.UpDownPage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAllPage);
            this.Controls.Add(this.grpBoxFilter);
            this.Controls.Add(this.grpBoxUploadedRate);
            this.Controls.Add(this.grpBoxUploadedQty);
            this.Controls.Add(this.grpBoxPassedRate);
            this.Controls.Add(this.grpBoxPassedQty);
            this.Controls.Add(this.grpBoxAllQty);
            this.Controls.Add(this.GridContent);
            this.Controls.Add(this.grpBoxTime);
            this.Name = "StatisticForm";
            this.Text = "统计信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticForm_FormClosing);
            this.Load += new System.EventHandler(this.StatisticForm_Load);
            this.grpBoxTime.ResumeLayout(false);
            this.grpBoxTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).EndInit();
            this.grpBoxAllQty.ResumeLayout(false);
            this.grpBoxAllQty.PerformLayout();
            this.grpBoxPassedQty.ResumeLayout(false);
            this.grpBoxPassedQty.PerformLayout();
            this.grpBoxPassedRate.ResumeLayout(false);
            this.grpBoxPassedRate.PerformLayout();
            this.grpBoxUploadedQty.ResumeLayout(false);
            this.grpBoxUploadedQty.PerformLayout();
            this.grpBoxUploadedRate.ResumeLayout(false);
            this.grpBoxUploadedRate.PerformLayout();
            this.grpBoxFilter.ResumeLayout(false);
            this.grpBoxFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownPage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxTime;
        private System.Windows.Forms.RadioButton radioBtnMonth;
        private System.Windows.Forms.RadioButton radioBtnWeek;
        private System.Windows.Forms.RadioButton radioBtnDay;
        private System.Windows.Forms.DataGridView GridContent;
        private System.Windows.Forms.GroupBox grpBoxAllQty;
        private System.Windows.Forms.Label lblAllQty;
        private System.Windows.Forms.GroupBox grpBoxPassedQty;
        private System.Windows.Forms.Label lblPassedQty;
        private System.Windows.Forms.GroupBox grpBoxPassedRate;
        private System.Windows.Forms.Label lblPassedRate;
        private System.Windows.Forms.GroupBox grpBoxUploadedQty;
        private System.Windows.Forms.Label lblUploadedQty;
        private System.Windows.Forms.GroupBox grpBoxUploadedRate;
        private System.Windows.Forms.Label lblUploadedRate;
        private System.Windows.Forms.GroupBox grpBoxFilter;
        private System.Windows.Forms.CheckBox chkBoxUploaded;
        private System.Windows.Forms.CheckBox chkBoxPassed;
        private System.Windows.Forms.Label lblAllPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown UpDownPage;
    }
}