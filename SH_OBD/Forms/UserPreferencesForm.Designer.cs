namespace SH_OBD {
    partial class UserPreferencesForm {
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
            this.groupCompany = new System.Windows.Forms.GroupBox();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupCompany.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupCompany
            // 
            this.groupCompany.Controls.Add(this.txtTelephone);
            this.groupCompany.Controls.Add(this.txtAddress2);
            this.groupCompany.Controls.Add(this.txtAddress1);
            this.groupCompany.Controls.Add(this.txtName);
            this.groupCompany.Controls.Add(this.label3);
            this.groupCompany.Controls.Add(this.label2);
            this.groupCompany.Controls.Add(this.label1);
            this.groupCompany.Location = new System.Drawing.Point(12, 12);
            this.groupCompany.Name = "groupCompany";
            this.groupCompany.Size = new System.Drawing.Size(324, 144);
            this.groupCompany.TabIndex = 3;
            this.groupCompany.TabStop = false;
            this.groupCompany.Text = "用户情况";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(83, 104);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(217, 21);
            this.txtTelephone.TabIndex = 6;
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(83, 77);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(217, 21);
            this.txtAddress2.TabIndex = 4;
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(83, 51);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(217, 21);
            this.txtAddress1.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(83, 26);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(217, 21);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "电话(&T):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "地址(&A):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称(&N):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(184, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 26);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消(&C)";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(78, 164);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UserPreferencesForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(349, 202);
            this.Controls.Add(this.groupCompany);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserPreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户设置";
            this.Load += new System.EventHandler(this.UserPreferencesForm_Load);
            this.groupCompany.ResumeLayout(false);
            this.groupCompany.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupCompany;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}