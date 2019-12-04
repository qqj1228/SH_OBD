namespace SH_OBD {
    partial class CheckForm {
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
            this.GridContent = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.txtBoxType = new System.Windows.Forms.TextBox();
            this.grpBoxType = new System.Windows.Forms.GroupBox();
            this.grpBoxECUID = new System.Windows.Forms.GroupBox();
            this.txtBoxECUID = new System.Windows.Forms.TextBox();
            this.grpBoxCALID = new System.Windows.Forms.GroupBox();
            this.txtBoxCALID = new System.Windows.Forms.TextBox();
            this.grpBoxCVN = new System.Windows.Forms.GroupBox();
            this.txtBoxCVN = new System.Windows.Forms.TextBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.MenuItemArrange = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBoxProject = new System.Windows.Forms.GroupBox();
            this.txtBoxProject = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.grpBoxType.SuspendLayout();
            this.grpBoxECUID.SuspendLayout();
            this.grpBoxCALID.SuspendLayout();
            this.grpBoxCVN.SuspendLayout();
            this.grpBoxProject.SuspendLayout();
            this.SuspendLayout();
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
            this.GridContent.ContextMenuStrip = this.contextMenuStrip1;
            this.GridContent.Location = new System.Drawing.Point(12, 64);
            this.GridContent.Name = "GridContent";
            this.GridContent.ReadOnly = true;
            this.GridContent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.GridContent.RowTemplate.Height = 23;
            this.GridContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridContent.Size = new System.Drawing.Size(776, 374);
            this.GridContent.TabIndex = 8;
            this.GridContent.Click += new System.EventHandler(this.GridContent_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemImport,
            this.MenuItemExport,
            this.toolStripSeparator1,
            this.MenuItemArrange,
            this.MenuItemRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 98);
            // 
            // MenuItemImport
            // 
            this.MenuItemImport.Name = "MenuItemImport";
            this.MenuItemImport.Size = new System.Drawing.Size(177, 22);
            this.MenuItemImport.Text = "导入Excel文件...(&I)";
            this.MenuItemImport.Click += new System.EventHandler(this.MenuItemImport_Click);
            // 
            // MenuItemExport
            // 
            this.MenuItemExport.Name = "MenuItemExport";
            this.MenuItemExport.Size = new System.Drawing.Size(177, 22);
            this.MenuItemExport.Text = "导出Excel文件...(E)";
            this.MenuItemExport.Click += new System.EventHandler(this.MenuItemExport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // MenuItemRefresh
            // 
            this.MenuItemRefresh.Name = "MenuItemRefresh";
            this.MenuItemRefresh.Size = new System.Drawing.Size(177, 22);
            this.MenuItemRefresh.Text = "刷新(&R)";
            this.MenuItemRefresh.Click += new System.EventHandler(this.MenuItemRefresh_Click);
            // 
            // txtBoxType
            // 
            this.txtBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxType.Location = new System.Drawing.Point(3, 17);
            this.txtBoxType.Name = "txtBoxType";
            this.txtBoxType.Size = new System.Drawing.Size(94, 21);
            this.txtBoxType.TabIndex = 1;
            // 
            // grpBoxType
            // 
            this.grpBoxType.Controls.Add(this.txtBoxType);
            this.grpBoxType.Location = new System.Drawing.Point(118, 13);
            this.grpBoxType.Name = "grpBoxType";
            this.grpBoxType.Size = new System.Drawing.Size(100, 45);
            this.grpBoxType.TabIndex = 1;
            this.grpBoxType.TabStop = false;
            this.grpBoxType.Text = "车型";
            // 
            // grpBoxECUID
            // 
            this.grpBoxECUID.Controls.Add(this.txtBoxECUID);
            this.grpBoxECUID.Location = new System.Drawing.Point(224, 13);
            this.grpBoxECUID.Name = "grpBoxECUID";
            this.grpBoxECUID.Size = new System.Drawing.Size(100, 45);
            this.grpBoxECUID.TabIndex = 2;
            this.grpBoxECUID.TabStop = false;
            this.grpBoxECUID.Text = "模块ID";
            // 
            // txtBoxECUID
            // 
            this.txtBoxECUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxECUID.Location = new System.Drawing.Point(3, 17);
            this.txtBoxECUID.Name = "txtBoxECUID";
            this.txtBoxECUID.Size = new System.Drawing.Size(94, 21);
            this.txtBoxECUID.TabIndex = 1;
            // 
            // grpBoxCALID
            // 
            this.grpBoxCALID.Controls.Add(this.txtBoxCALID);
            this.grpBoxCALID.Location = new System.Drawing.Point(330, 13);
            this.grpBoxCALID.Name = "grpBoxCALID";
            this.grpBoxCALID.Size = new System.Drawing.Size(100, 45);
            this.grpBoxCALID.TabIndex = 3;
            this.grpBoxCALID.TabStop = false;
            this.grpBoxCALID.Text = "CAL_ID";
            // 
            // txtBoxCALID
            // 
            this.txtBoxCALID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCALID.Location = new System.Drawing.Point(3, 17);
            this.txtBoxCALID.Name = "txtBoxCALID";
            this.txtBoxCALID.Size = new System.Drawing.Size(94, 21);
            this.txtBoxCALID.TabIndex = 1;
            // 
            // grpBoxCVN
            // 
            this.grpBoxCVN.Controls.Add(this.txtBoxCVN);
            this.grpBoxCVN.Location = new System.Drawing.Point(436, 12);
            this.grpBoxCVN.Name = "grpBoxCVN";
            this.grpBoxCVN.Size = new System.Drawing.Size(100, 45);
            this.grpBoxCVN.TabIndex = 4;
            this.grpBoxCVN.TabStop = false;
            this.grpBoxCVN.Text = "CVN";
            // 
            // txtBoxCVN
            // 
            this.txtBoxCVN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCVN.Location = new System.Drawing.Point(3, 17);
            this.txtBoxCVN.Name = "txtBoxCVN";
            this.txtBoxCVN.Size = new System.Drawing.Size(94, 21);
            this.txtBoxCVN.TabIndex = 1;
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(536, 27);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(80, 23);
            this.btnModify.TabIndex = 5;
            this.btnModify.Text = "修改当前行";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(622, 27);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(80, 23);
            this.btnInsert.TabIndex = 6;
            this.btnInsert.Text = "新增一行";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(708, 27);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(80, 23);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "删除选中行";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // MenuItemArrange
            // 
            this.MenuItemArrange.Name = "MenuItemArrange";
            this.MenuItemArrange.Size = new System.Drawing.Size(177, 22);
            this.MenuItemArrange.Text = "整理数据(A)";
            this.MenuItemArrange.Click += new System.EventHandler(this.MenuItemArrange_Click);
            // 
            // grpBoxProject
            // 
            this.grpBoxProject.Controls.Add(this.txtBoxProject);
            this.grpBoxProject.Location = new System.Drawing.Point(12, 13);
            this.grpBoxProject.Name = "grpBoxProject";
            this.grpBoxProject.Size = new System.Drawing.Size(100, 45);
            this.grpBoxProject.TabIndex = 0;
            this.grpBoxProject.TabStop = false;
            this.grpBoxProject.Text = "项目";
            // 
            // txtBoxProject
            // 
            this.txtBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxProject.Location = new System.Drawing.Point(3, 17);
            this.txtBoxProject.Name = "txtBoxProject";
            this.txtBoxProject.Size = new System.Drawing.Size(94, 21);
            this.txtBoxProject.TabIndex = 1;
            // 
            // CheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpBoxProject);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.grpBoxCVN);
            this.Controls.Add(this.grpBoxCALID);
            this.Controls.Add(this.grpBoxECUID);
            this.Controls.Add(this.grpBoxType);
            this.Controls.Add(this.GridContent);
            this.Name = "CheckForm";
            this.Text = "ShowResultForm";
            this.Load += new System.EventHandler(this.CheckForm_Load);
            this.Resize += new System.EventHandler(this.CheckForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.grpBoxType.ResumeLayout(false);
            this.grpBoxType.PerformLayout();
            this.grpBoxECUID.ResumeLayout(false);
            this.grpBoxECUID.PerformLayout();
            this.grpBoxCALID.ResumeLayout(false);
            this.grpBoxCALID.PerformLayout();
            this.grpBoxCVN.ResumeLayout(false);
            this.grpBoxCVN.PerformLayout();
            this.grpBoxProject.ResumeLayout(false);
            this.grpBoxProject.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridContent;
        private System.Windows.Forms.TextBox txtBoxType;
        private System.Windows.Forms.GroupBox grpBoxType;
        private System.Windows.Forms.GroupBox grpBoxECUID;
        private System.Windows.Forms.TextBox txtBoxECUID;
        private System.Windows.Forms.GroupBox grpBoxCALID;
        private System.Windows.Forms.TextBox txtBoxCALID;
        private System.Windows.Forms.GroupBox grpBoxCVN;
        private System.Windows.Forms.TextBox txtBoxCVN;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemImport;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRefresh;
        private System.Windows.Forms.ToolStripMenuItem MenuItemArrange;
        private System.Windows.Forms.GroupBox grpBoxProject;
        private System.Windows.Forms.TextBox txtBoxProject;
    }
}