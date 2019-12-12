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
            this.components = new System.ComponentModel.Container();
            this.txtBoxCARCODE = new System.Windows.Forms.TextBox();
            this.cmbBoxProtocol = new System.Windows.Forms.ComboBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.GridContent = new System.Windows.Forms.DataGridView();
            this.grpBoxCARCODE = new System.Windows.Forms.GroupBox();
            this.grpBoxProtocol = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.grpBoxEngine = new System.Windows.Forms.GroupBox();
            this.txtBoxEngine = new System.Windows.Forms.TextBox();
            this.txtBoxStage = new System.Windows.Forms.TextBox();
            this.grpBoxStage = new System.Windows.Forms.GroupBox();
            this.grpBoxFuel = new System.Windows.Forms.GroupBox();
            this.txtBoxFuel = new System.Windows.Forms.TextBox();
            this.grpBoxModel = new System.Windows.Forms.GroupBox();
            this.txtBoxModel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridContent)).BeginInit();
            this.grpBoxCARCODE.SuspendLayout();
            this.grpBoxProtocol.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.grpBoxEngine.SuspendLayout();
            this.grpBoxStage.SuspendLayout();
            this.grpBoxFuel.SuspendLayout();
            this.grpBoxModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBoxCARCODE
            // 
            this.txtBoxCARCODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCARCODE.Location = new System.Drawing.Point(3, 17);
            this.txtBoxCARCODE.Name = "txtBoxCARCODE";
            this.txtBoxCARCODE.Size = new System.Drawing.Size(74, 21);
            this.txtBoxCARCODE.TabIndex = 0;
            // 
            // cmbBoxProtocol
            // 
            this.cmbBoxProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBoxProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxProtocol.FormattingEnabled = true;
            this.cmbBoxProtocol.Location = new System.Drawing.Point(3, 17);
            this.cmbBoxProtocol.Name = "cmbBoxProtocol";
            this.cmbBoxProtocol.Size = new System.Drawing.Size(94, 20);
            this.cmbBoxProtocol.TabIndex = 1;
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(551, 28);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 6;
            this.btnModify.Text = "修改当前行";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(632, 28);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 7;
            this.btnInsert.Text = "新增一行";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.BtnInsert_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(713, 28);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "删除选中行";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // GridContent
            // 
            this.GridContent.AllowUserToAddRows = false;
            this.GridContent.AllowUserToDeleteRows = false;
            this.GridContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridContent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridContent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.GridContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridContent.ContextMenuStrip = this.contextMenuStrip1;
            this.GridContent.Location = new System.Drawing.Point(12, 67);
            this.GridContent.Name = "GridContent";
            this.GridContent.ReadOnly = true;
            this.GridContent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GridContent.RowTemplate.Height = 23;
            this.GridContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridContent.Size = new System.Drawing.Size(776, 371);
            this.GridContent.TabIndex = 9;
            this.GridContent.Click += new System.EventHandler(this.GridContent_Click);
            // 
            // grpBoxCARCODE
            // 
            this.grpBoxCARCODE.Controls.Add(this.txtBoxCARCODE);
            this.grpBoxCARCODE.Location = new System.Drawing.Point(12, 12);
            this.grpBoxCARCODE.Name = "grpBoxCARCODE";
            this.grpBoxCARCODE.Size = new System.Drawing.Size(80, 49);
            this.grpBoxCARCODE.TabIndex = 0;
            this.grpBoxCARCODE.TabStop = false;
            this.grpBoxCARCODE.Text = "车型代码";
            // 
            // grpBoxProtocol
            // 
            this.grpBoxProtocol.Controls.Add(this.cmbBoxProtocol);
            this.grpBoxProtocol.Location = new System.Drawing.Point(442, 13);
            this.grpBoxProtocol.Name = "grpBoxProtocol";
            this.grpBoxProtocol.Size = new System.Drawing.Size(100, 48);
            this.grpBoxProtocol.TabIndex = 5;
            this.grpBoxProtocol.TabStop = false;
            this.grpBoxProtocol.Text = "OBD协议";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemImport,
            this.toolStripSeparator1,
            this.MenuItemRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(221, 54);
            // 
            // MenuItemImport
            // 
            this.MenuItemImport.Name = "MenuItemImport";
            this.MenuItemImport.Size = new System.Drawing.Size(220, 22);
            this.MenuItemImport.Text = "导入车型OBD协议文件...(&I)";
            this.MenuItemImport.Click += new System.EventHandler(this.MenuItemImport_Click);
            // 
            // MenuItemRefresh
            // 
            this.MenuItemRefresh.Name = "MenuItemRefresh";
            this.MenuItemRefresh.Size = new System.Drawing.Size(220, 22);
            this.MenuItemRefresh.Text = "刷新(&R)";
            this.MenuItemRefresh.Click += new System.EventHandler(this.MenuItemRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // grpBoxEngine
            // 
            this.grpBoxEngine.Controls.Add(this.txtBoxEngine);
            this.grpBoxEngine.Location = new System.Drawing.Point(98, 12);
            this.grpBoxEngine.Name = "grpBoxEngine";
            this.grpBoxEngine.Size = new System.Drawing.Size(80, 49);
            this.grpBoxEngine.TabIndex = 1;
            this.grpBoxEngine.TabStop = false;
            this.grpBoxEngine.Text = "发动机";
            // 
            // txtBoxEngine
            // 
            this.txtBoxEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxEngine.Location = new System.Drawing.Point(3, 17);
            this.txtBoxEngine.Name = "txtBoxEngine";
            this.txtBoxEngine.Size = new System.Drawing.Size(74, 21);
            this.txtBoxEngine.TabIndex = 0;
            // 
            // txtBoxStage
            // 
            this.txtBoxStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxStage.Location = new System.Drawing.Point(3, 17);
            this.txtBoxStage.Name = "txtBoxStage";
            this.txtBoxStage.Size = new System.Drawing.Size(74, 21);
            this.txtBoxStage.TabIndex = 0;
            // 
            // grpBoxStage
            // 
            this.grpBoxStage.Controls.Add(this.txtBoxStage);
            this.grpBoxStage.Location = new System.Drawing.Point(184, 12);
            this.grpBoxStage.Name = "grpBoxStage";
            this.grpBoxStage.Size = new System.Drawing.Size(80, 49);
            this.grpBoxStage.TabIndex = 2;
            this.grpBoxStage.TabStop = false;
            this.grpBoxStage.Text = "排放阶段";
            // 
            // grpBoxFuel
            // 
            this.grpBoxFuel.Controls.Add(this.txtBoxFuel);
            this.grpBoxFuel.Location = new System.Drawing.Point(270, 12);
            this.grpBoxFuel.Name = "grpBoxFuel";
            this.grpBoxFuel.Size = new System.Drawing.Size(80, 49);
            this.grpBoxFuel.TabIndex = 3;
            this.grpBoxFuel.TabStop = false;
            this.grpBoxFuel.Text = "燃油方式";
            // 
            // txtBoxFuel
            // 
            this.txtBoxFuel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxFuel.Location = new System.Drawing.Point(3, 17);
            this.txtBoxFuel.Name = "txtBoxFuel";
            this.txtBoxFuel.Size = new System.Drawing.Size(74, 21);
            this.txtBoxFuel.TabIndex = 0;
            // 
            // grpBoxModel
            // 
            this.grpBoxModel.Controls.Add(this.txtBoxModel);
            this.grpBoxModel.Location = new System.Drawing.Point(356, 12);
            this.grpBoxModel.Name = "grpBoxModel";
            this.grpBoxModel.Size = new System.Drawing.Size(80, 49);
            this.grpBoxModel.TabIndex = 4;
            this.grpBoxModel.TabStop = false;
            this.grpBoxModel.Text = "车型";
            // 
            // txtBoxModel
            // 
            this.txtBoxModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxModel.Location = new System.Drawing.Point(3, 17);
            this.txtBoxModel.Name = "txtBoxModel";
            this.txtBoxModel.Size = new System.Drawing.Size(74, 21);
            this.txtBoxModel.TabIndex = 0;
            // 
            // ProtocolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpBoxModel);
            this.Controls.Add(this.grpBoxFuel);
            this.Controls.Add(this.grpBoxStage);
            this.Controls.Add(this.grpBoxEngine);
            this.Controls.Add(this.grpBoxProtocol);
            this.Controls.Add(this.grpBoxCARCODE);
            this.Controls.Add(this.GridContent);
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
            this.contextMenuStrip1.ResumeLayout(false);
            this.grpBoxEngine.ResumeLayout(false);
            this.grpBoxEngine.PerformLayout();
            this.grpBoxStage.ResumeLayout(false);
            this.grpBoxStage.PerformLayout();
            this.grpBoxFuel.ResumeLayout(false);
            this.grpBoxFuel.PerformLayout();
            this.grpBoxModel.ResumeLayout(false);
            this.grpBoxModel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxCARCODE;
        private System.Windows.Forms.ComboBox cmbBoxProtocol;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DataGridView GridContent;
        private System.Windows.Forms.GroupBox grpBoxCARCODE;
        private System.Windows.Forms.GroupBox grpBoxProtocol;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRefresh;
        private System.Windows.Forms.GroupBox grpBoxEngine;
        private System.Windows.Forms.TextBox txtBoxEngine;
        private System.Windows.Forms.TextBox txtBoxStage;
        private System.Windows.Forms.GroupBox grpBoxStage;
        private System.Windows.Forms.GroupBox grpBoxFuel;
        private System.Windows.Forms.TextBox txtBoxFuel;
        private System.Windows.Forms.GroupBox grpBoxModel;
        private System.Windows.Forms.TextBox txtBoxModel;
    }
}