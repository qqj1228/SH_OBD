namespace SH_OBD {
    partial class ProtocolForm {
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
            this.txtBoxCARCODE = new System.Windows.Forms.TextBox();
            this.cmbBoxProtocol = new System.Windows.Forms.ComboBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.GridContent = new System.Windows.Forms.DataGridView();
            this.grpBoxCARCODE = new System.Windows.Forms.GroupBox();
            this.grpBoxProtocol = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).BeginInit();
            this.grpBoxCARCODE.SuspendLayout();
            this.grpBoxProtocol.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBoxCARCODE
            // 
            this.txtBoxCARCODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCARCODE.Location = new System.Drawing.Point(3, 17);
            this.txtBoxCARCODE.Name = "txtBoxCARCODE";
            this.txtBoxCARCODE.Size = new System.Drawing.Size(109, 21);
            this.txtBoxCARCODE.TabIndex = 0;
            // 
            // cmbBoxProtocol
            // 
            this.cmbBoxProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBoxProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxProtocol.FormattingEnabled = true;
            this.cmbBoxProtocol.Location = new System.Drawing.Point(3, 17);
            this.cmbBoxProtocol.Name = "cmbBoxProtocol";
            this.cmbBoxProtocol.Size = new System.Drawing.Size(244, 20);
            this.cmbBoxProtocol.TabIndex = 1;
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(470, 28);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "修改当前行";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(551, 28);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.Text = "新增一行";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(632, 28);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "删除选中行";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(713, 28);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // GridContent
            // 
            this.GridContent.AllowUserToAddRows = false;
            this.GridContent.AllowUserToDeleteRows = false;
            this.GridContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridContent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GridContent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridContent.Location = new System.Drawing.Point(12, 67);
            this.GridContent.Name = "GridContent";
            this.GridContent.ReadOnly = true;
            this.GridContent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.GridContent.RowTemplate.Height = 23;
            this.GridContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridContent.Size = new System.Drawing.Size(776, 371);
            this.GridContent.TabIndex = 7;
            this.GridContent.Click += new System.EventHandler(this.GridContent_Click);
            // 
            // grpBoxCARCODE
            // 
            this.grpBoxCARCODE.Controls.Add(this.txtBoxCARCODE);
            this.grpBoxCARCODE.Location = new System.Drawing.Point(12, 12);
            this.grpBoxCARCODE.Name = "grpBoxCARCODE";
            this.grpBoxCARCODE.Size = new System.Drawing.Size(115, 49);
            this.grpBoxCARCODE.TabIndex = 0;
            this.grpBoxCARCODE.TabStop = false;
            this.grpBoxCARCODE.Text = "车型";
            // 
            // grpBoxProtocol
            // 
            this.grpBoxProtocol.Controls.Add(this.cmbBoxProtocol);
            this.grpBoxProtocol.Location = new System.Drawing.Point(133, 13);
            this.grpBoxProtocol.Name = "grpBoxProtocol";
            this.grpBoxProtocol.Size = new System.Drawing.Size(250, 48);
            this.grpBoxProtocol.TabIndex = 1;
            this.grpBoxProtocol.TabStop = false;
            this.grpBoxProtocol.Text = "OBD协议";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(389, 28);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "导入...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // ProtocolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.grpBoxProtocol);
            this.Controls.Add(this.grpBoxCARCODE);
            this.Controls.Add(this.GridContent);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnModify);
            this.Name = "ProtocolForm";
            this.Text = "ShowResultForm";
            this.Load += new System.EventHandler(this.ProtocolForm_Load);
            this.Resize += new System.EventHandler(this.ProtocolForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).EndInit();
            this.grpBoxCARCODE.ResumeLayout(false);
            this.grpBoxCARCODE.PerformLayout();
            this.grpBoxProtocol.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxCARCODE;
        private System.Windows.Forms.ComboBox cmbBoxProtocol;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView GridContent;
        private System.Windows.Forms.GroupBox grpBoxCARCODE;
        private System.Windows.Forms.GroupBox grpBoxProtocol;
        private System.Windows.Forms.Button btnImport;
    }
}