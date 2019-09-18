namespace SH_OBD {
    partial class TestForm {
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
            this.lblBattery = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblVehicleInfo = new System.Windows.Forms.Label();
            this.lblOBD = new System.Windows.Forms.Label();
            this.lblAir = new System.Windows.Forms.Label();
            this.lblPTO = new System.Windows.Forms.Label();
            this.groupBattery = new System.Windows.Forms.GroupBox();
            this.lblFuel2 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblFuel1 = new System.Windows.Forms.Label();
            this.groupAir = new System.Windows.Forms.GroupBox();
            this.groupFuel1 = new System.Windows.Forms.GroupBox();
            this.gridNonConTests = new System.Windows.Forms.DataGridView();
            this.groupOxygen = new System.Windows.Forms.GroupBox();
            this.groupOBD = new System.Windows.Forms.GroupBox();
            this.groupNonConTests = new System.Windows.Forms.GroupBox();
            this.gridConTests = new System.Windows.Forms.DataGridView();
            this.groupPTO = new System.Windows.Forms.GroupBox();
            this.groupFuel2 = new System.Windows.Forms.GroupBox();
            this.groupConTests = new System.Windows.Forms.GroupBox();
            this.groupBattery.SuspendLayout();
            this.groupAir.SuspendLayout();
            this.groupFuel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNonConTests)).BeginInit();
            this.groupOxygen.SuspendLayout();
            this.groupOBD.SuspendLayout();
            this.groupNonConTests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConTests)).BeginInit();
            this.groupPTO.SuspendLayout();
            this.groupFuel2.SuspendLayout();
            this.groupConTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBattery
            // 
            this.lblBattery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBattery.Location = new System.Drawing.Point(12, 20);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new System.Drawing.Size(340, 30);
            this.lblBattery.TabIndex = 0;
            this.lblBattery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(108, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(680, 24);
            this.progressBar.TabIndex = 21;
            // 
            // lblVehicleInfo
            // 
            this.lblVehicleInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVehicleInfo.Location = new System.Drawing.Point(12, 20);
            this.lblVehicleInfo.Name = "lblVehicleInfo";
            this.lblVehicleInfo.Size = new System.Drawing.Size(380, 175);
            this.lblVehicleInfo.TabIndex = 0;
            this.lblVehicleInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOBD
            // 
            this.lblOBD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOBD.Location = new System.Drawing.Point(12, 20);
            this.lblOBD.Name = "lblOBD";
            this.lblOBD.Size = new System.Drawing.Size(380, 30);
            this.lblOBD.TabIndex = 0;
            this.lblOBD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAir
            // 
            this.lblAir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAir.Location = new System.Drawing.Point(12, 20);
            this.lblAir.Name = "lblAir";
            this.lblAir.Size = new System.Drawing.Size(340, 37);
            this.lblAir.TabIndex = 0;
            this.lblAir.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPTO
            // 
            this.lblPTO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPTO.Location = new System.Drawing.Point(12, 20);
            this.lblPTO.Name = "lblPTO";
            this.lblPTO.Size = new System.Drawing.Size(340, 36);
            this.lblPTO.TabIndex = 0;
            this.lblPTO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBattery
            // 
            this.groupBattery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBattery.Controls.Add(this.lblBattery);
            this.groupBattery.Location = new System.Drawing.Point(12, 483);
            this.groupBattery.Name = "groupBattery";
            this.groupBattery.Size = new System.Drawing.Size(365, 60);
            this.groupBattery.TabIndex = 22;
            this.groupBattery.TabStop = false;
            this.groupBattery.Text = "电池电压";
            // 
            // lblFuel2
            // 
            this.lblFuel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFuel2.Location = new System.Drawing.Point(12, 20);
            this.lblFuel2.Name = "lblFuel2";
            this.lblFuel2.Size = new System.Drawing.Size(340, 30);
            this.lblFuel2.TabIndex = 0;
            this.lblFuel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 24);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "更新(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblFuel1
            // 
            this.lblFuel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFuel1.Location = new System.Drawing.Point(12, 20);
            this.lblFuel1.Name = "lblFuel1";
            this.lblFuel1.Size = new System.Drawing.Size(340, 30);
            this.lblFuel1.TabIndex = 0;
            this.lblFuel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupAir
            // 
            this.groupAir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupAir.Controls.Add(this.lblAir);
            this.groupAir.Location = new System.Drawing.Point(12, 410);
            this.groupAir.Name = "groupAir";
            this.groupAir.Size = new System.Drawing.Size(365, 67);
            this.groupAir.TabIndex = 17;
            this.groupAir.TabStop = false;
            this.groupAir.Text = "指令的二次空气状态";
            // 
            // groupFuel1
            // 
            this.groupFuel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupFuel1.Controls.Add(this.lblFuel1);
            this.groupFuel1.Location = new System.Drawing.Point(12, 206);
            this.groupFuel1.Name = "groupFuel1";
            this.groupFuel1.Size = new System.Drawing.Size(365, 60);
            this.groupFuel1.TabIndex = 14;
            this.groupFuel1.TabStop = false;
            this.groupFuel1.Text = "燃油系统 #1";
            // 
            // gridNonConTests
            // 
            this.gridNonConTests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridNonConTests.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridNonConTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNonConTests.Location = new System.Drawing.Point(3, 17);
            this.gridNonConTests.Name = "gridNonConTests";
            this.gridNonConTests.ReadOnly = true;
            this.gridNonConTests.RowHeadersVisible = false;
            this.gridNonConTests.Size = new System.Drawing.Size(399, 204);
            this.gridNonConTests.TabIndex = 1;
            // 
            // groupOxygen
            // 
            this.groupOxygen.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupOxygen.Controls.Add(this.lblVehicleInfo);
            this.groupOxygen.Location = new System.Drawing.Point(383, 338);
            this.groupOxygen.Name = "groupOxygen";
            this.groupOxygen.Size = new System.Drawing.Size(405, 205);
            this.groupOxygen.TabIndex = 19;
            this.groupOxygen.TabStop = false;
            this.groupOxygen.Text = "车辆信息";
            // 
            // groupOBD
            // 
            this.groupOBD.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupOBD.Controls.Add(this.lblOBD);
            this.groupOBD.Location = new System.Drawing.Point(383, 272);
            this.groupOBD.Name = "groupOBD";
            this.groupOBD.Size = new System.Drawing.Size(405, 60);
            this.groupOBD.TabIndex = 18;
            this.groupOBD.TabStop = false;
            this.groupOBD.Text = "车辆OBD要求";
            // 
            // groupNonConTests
            // 
            this.groupNonConTests.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupNonConTests.Controls.Add(this.gridNonConTests);
            this.groupNonConTests.Location = new System.Drawing.Point(383, 42);
            this.groupNonConTests.Name = "groupNonConTests";
            this.groupNonConTests.Size = new System.Drawing.Size(405, 224);
            this.groupNonConTests.TabIndex = 13;
            this.groupNonConTests.TabStop = false;
            this.groupNonConTests.Text = "非连续诊断";
            // 
            // gridConTests
            // 
            this.gridConTests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridConTests.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridConTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridConTests.Location = new System.Drawing.Point(3, 17);
            this.gridConTests.Name = "gridConTests";
            this.gridConTests.ReadOnly = true;
            this.gridConTests.RowHeadersVisible = false;
            this.gridConTests.Size = new System.Drawing.Size(359, 136);
            this.gridConTests.TabIndex = 0;
            // 
            // groupPTO
            // 
            this.groupPTO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupPTO.Controls.Add(this.lblPTO);
            this.groupPTO.Location = new System.Drawing.Point(12, 338);
            this.groupPTO.Name = "groupPTO";
            this.groupPTO.Size = new System.Drawing.Size(365, 66);
            this.groupPTO.TabIndex = 16;
            this.groupPTO.TabStop = false;
            this.groupPTO.Text = "动力输出";
            // 
            // groupFuel2
            // 
            this.groupFuel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupFuel2.Controls.Add(this.lblFuel2);
            this.groupFuel2.Location = new System.Drawing.Point(12, 272);
            this.groupFuel2.Name = "groupFuel2";
            this.groupFuel2.Size = new System.Drawing.Size(365, 60);
            this.groupFuel2.TabIndex = 15;
            this.groupFuel2.TabStop = false;
            this.groupFuel2.Text = "燃油系统 #2";
            // 
            // groupConTests
            // 
            this.groupConTests.Controls.Add(this.gridConTests);
            this.groupConTests.Location = new System.Drawing.Point(12, 42);
            this.groupConTests.Name = "groupConTests";
            this.groupConTests.Size = new System.Drawing.Size(365, 156);
            this.groupConTests.TabIndex = 12;
            this.groupConTests.TabStop = false;
            this.groupConTests.Text = "连续诊断";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 555);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBattery);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupAir);
            this.Controls.Add(this.groupFuel1);
            this.Controls.Add(this.groupOxygen);
            this.Controls.Add(this.groupOBD);
            this.Controls.Add(this.groupNonConTests);
            this.Controls.Add(this.groupPTO);
            this.Controls.Add(this.groupFuel2);
            this.Controls.Add(this.groupConTests);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.Resize += new System.EventHandler(this.TestForm_Resize);
            this.groupBattery.ResumeLayout(false);
            this.groupAir.ResumeLayout(false);
            this.groupFuel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNonConTests)).EndInit();
            this.groupOxygen.ResumeLayout(false);
            this.groupOBD.ResumeLayout(false);
            this.groupNonConTests.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridConTests)).EndInit();
            this.groupPTO.ResumeLayout(false);
            this.groupFuel2.ResumeLayout(false);
            this.groupConTests.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBattery;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblVehicleInfo;
        private System.Windows.Forms.Label lblOBD;
        private System.Windows.Forms.Label lblAir;
        private System.Windows.Forms.Label lblPTO;
        private System.Windows.Forms.GroupBox groupBattery;
        private System.Windows.Forms.Label lblFuel2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblFuel1;
        private System.Windows.Forms.GroupBox groupAir;
        private System.Windows.Forms.GroupBox groupFuel1;
        private System.Windows.Forms.DataGridView gridNonConTests;
        private System.Windows.Forms.GroupBox groupOxygen;
        private System.Windows.Forms.GroupBox groupOBD;
        private System.Windows.Forms.GroupBox groupNonConTests;
        private System.Windows.Forms.DataGridView gridConTests;
        private System.Windows.Forms.GroupBox groupPTO;
        private System.Windows.Forms.GroupBox groupFuel2;
        private System.Windows.Forms.GroupBox groupConTests;
    }
}