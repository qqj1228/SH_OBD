namespace SH_OBD {
    partial class OBDStartForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OBDStartForm));
            this.tblLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.tblLayoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxVIN = new System.Windows.Forms.TextBox();
            this.tblLayoutBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdvanceMode = new System.Windows.Forms.Button();
            this.labelVINError = new System.Windows.Forms.Label();
            this.labelCALIDCVN = new System.Windows.Forms.Label();
            this.label3Space = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tblLayoutMain.SuspendLayout();
            this.tblLayoutTop.SuspendLayout();
            this.tblLayoutBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tblLayoutMain
            // 
            this.tblLayoutMain.ColumnCount = 1;
            this.tblLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutMain.Controls.Add(this.tblLayoutTop, 0, 1);
            this.tblLayoutMain.Controls.Add(this.tblLayoutBottom, 0, 3);
            this.tblLayoutMain.Controls.Add(this.labelResult, 0, 2);
            this.tblLayoutMain.Controls.Add(this.pictureBox1, 0, 0);
            this.tblLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tblLayoutMain.Name = "tblLayoutMain";
            this.tblLayoutMain.RowCount = 4;
            this.tblLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tblLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.tblLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tblLayoutMain.Size = new System.Drawing.Size(784, 561);
            this.tblLayoutMain.TabIndex = 0;
            // 
            // tblLayoutTop
            // 
            this.tblLayoutTop.ColumnCount = 2;
            this.tblLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tblLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77F));
            this.tblLayoutTop.Controls.Add(this.label1, 0, 0);
            this.tblLayoutTop.Controls.Add(this.txtBoxVIN, 1, 0);
            this.tblLayoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutTop.Location = new System.Drawing.Point(3, 87);
            this.tblLayoutTop.Name = "tblLayoutTop";
            this.tblLayoutTop.RowCount = 1;
            this.tblLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutTop.Size = new System.Drawing.Size(778, 89);
            this.tblLayoutTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 50F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 89);
            this.label1.TabIndex = 0;
            this.label1.Text = "VIN:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBoxVIN
            // 
            this.txtBoxVIN.BackColor = System.Drawing.Color.SteelBlue;
            this.txtBoxVIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVIN.Font = new System.Drawing.Font("宋体", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxVIN.Location = new System.Drawing.Point(181, 3);
            this.txtBoxVIN.Name = "txtBoxVIN";
            this.txtBoxVIN.Size = new System.Drawing.Size(594, 84);
            this.txtBoxVIN.TabIndex = 1;
            this.txtBoxVIN.TextChanged += new System.EventHandler(this.TxtBoxVIN_TextChanged);
            this.txtBoxVIN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBoxVIN_KeyPress);
            // 
            // tblLayoutBottom
            // 
            this.tblLayoutBottom.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tblLayoutBottom.ColumnCount = 4;
            this.tblLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLayoutBottom.Controls.Add(this.btnAdvanceMode, 3, 0);
            this.tblLayoutBottom.Controls.Add(this.labelVINError, 0, 0);
            this.tblLayoutBottom.Controls.Add(this.labelCALIDCVN, 1, 0);
            this.tblLayoutBottom.Controls.Add(this.label3Space, 2, 0);
            this.tblLayoutBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutBottom.Location = new System.Drawing.Point(3, 479);
            this.tblLayoutBottom.Name = "tblLayoutBottom";
            this.tblLayoutBottom.RowCount = 1;
            this.tblLayoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutBottom.Size = new System.Drawing.Size(778, 79);
            this.tblLayoutBottom.TabIndex = 1;
            // 
            // btnAdvanceMode
            // 
            this.btnAdvanceMode.BackColor = System.Drawing.Color.SkyBlue;
            this.btnAdvanceMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdvanceMode.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdvanceMode.Location = new System.Drawing.Point(585, 6);
            this.btnAdvanceMode.Name = "btnAdvanceMode";
            this.btnAdvanceMode.Size = new System.Drawing.Size(187, 67);
            this.btnAdvanceMode.TabIndex = 0;
            this.btnAdvanceMode.Text = "高级模式(&A)";
            this.btnAdvanceMode.UseVisualStyleBackColor = false;
            this.btnAdvanceMode.Click += new System.EventHandler(this.BtnAdvanceMode_Click);
            // 
            // labelVINError
            // 
            this.labelVINError.AutoSize = true;
            this.labelVINError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVINError.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.labelVINError.Location = new System.Drawing.Point(6, 3);
            this.labelVINError.Name = "labelVINError";
            this.labelVINError.Size = new System.Drawing.Size(184, 73);
            this.labelVINError.TabIndex = 3;
            this.labelVINError.Text = "VIN号不匹配";
            this.labelVINError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCALIDCVN
            // 
            this.labelCALIDCVN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCALIDCVN.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCALIDCVN.Location = new System.Drawing.Point(199, 3);
            this.labelCALIDCVN.Name = "labelCALIDCVN";
            this.labelCALIDCVN.Size = new System.Drawing.Size(184, 73);
            this.labelCALIDCVN.TabIndex = 4;
            this.labelCALIDCVN.Text = "CALID和CVN数据不完整";
            this.labelCALIDCVN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3Space
            // 
            this.label3Space.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3Space.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3Space.Location = new System.Drawing.Point(392, 3);
            this.label3Space.Name = "label3Space";
            this.label3Space.Size = new System.Drawing.Size(184, 73);
            this.label3Space.TabIndex = 5;
            this.label3Space.Text = "OBD型式不适用或异常";
            this.label3Space.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelResult
            // 
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Font = new System.Drawing.Font("宋体", 50F, System.Drawing.FontStyle.Bold);
            this.labelResult.Location = new System.Drawing.Point(3, 179);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(778, 297);
            this.labelResult.TabIndex = 2;
            this.labelResult.Text = "OBD检测结果";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(778, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // OBDStartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tblLayoutMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "OBDStartForm";
            this.Text = "江铃OBD检测";
            this.Activated += new System.EventHandler(this.OBDStartForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OBDStartForm_FormClosing);
            this.Load += new System.EventHandler(this.OBDStartForm_Load);
            this.Resize += new System.EventHandler(this.OBDStartForm_Resize);
            this.tblLayoutMain.ResumeLayout(false);
            this.tblLayoutTop.ResumeLayout(false);
            this.tblLayoutTop.PerformLayout();
            this.tblLayoutBottom.ResumeLayout(false);
            this.tblLayoutBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblLayoutMain;
        private System.Windows.Forms.TableLayoutPanel tblLayoutTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tblLayoutBottom;
        private System.Windows.Forms.Button btnAdvanceMode;
        private System.Windows.Forms.TextBox txtBoxVIN;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelVINError;
        private System.Windows.Forms.Label labelCALIDCVN;
        private System.Windows.Forms.Label label3Space;
    }
}