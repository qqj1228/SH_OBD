namespace SH_OBD {
    partial class OxygenSensorsForm {
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnRead = new System.Windows.Forms.Button();
            this.comboOxygenSensor = new System.Windows.Forms.ComboBox();
            this.lblOxygenSensor = new System.Windows.Forms.Label();
            this.o2WaveformControl1 = new O2Waveform.O2WaveformControl();
            this.o2TestResultsControl1 = new SH_OBD.O2TestResultsControl();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 418);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(776, 25);
            this.progressBar.TabIndex = 10;
            // 
            // btnRead
            // 
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRead.Location = new System.Drawing.Point(698, 7);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(90, 23);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "读取(&R)";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // comboOxygenSensor
            // 
            this.comboOxygenSensor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboOxygenSensor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOxygenSensor.Location = new System.Drawing.Point(126, 9);
            this.comboOxygenSensor.Name = "comboOxygenSensor";
            this.comboOxygenSensor.Size = new System.Drawing.Size(566, 20);
            this.comboOxygenSensor.TabIndex = 8;
            // 
            // lblOxygenSensor
            // 
            this.lblOxygenSensor.Location = new System.Drawing.Point(12, 9);
            this.lblOxygenSensor.Name = "lblOxygenSensor";
            this.lblOxygenSensor.Size = new System.Drawing.Size(108, 20);
            this.lblOxygenSensor.TabIndex = 7;
            this.lblOxygenSensor.Text = "氧气传感器(&O):";
            this.lblOxygenSensor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // o2WaveformControl1
            // 
            this.o2WaveformControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.o2WaveformControl1.LabelColor = System.Drawing.Color.Yellow;
            this.o2WaveformControl1.LabelFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.o2WaveformControl1.LeanBGColor = System.Drawing.Color.Black;
            this.o2WaveformControl1.LineColor = System.Drawing.Color.White;
            this.o2WaveformControl1.Location = new System.Drawing.Point(12, 273);
            this.o2WaveformControl1.MidBGColor = System.Drawing.Color.Gray;
            this.o2WaveformControl1.Name = "o2WaveformControl1";
            this.o2WaveformControl1.RichBGColor = System.Drawing.Color.Black;
            this.o2WaveformControl1.Size = new System.Drawing.Size(776, 139);
            this.o2WaveformControl1.TabIndex = 6;
            this.o2WaveformControl1.TitleColor = System.Drawing.Color.DodgerBlue;
            this.o2WaveformControl1.TitleFont = new System.Drawing.Font("Arial", 10F);
            this.o2WaveformControl1.WaveColor = System.Drawing.Color.White;
            // 
            // o2TestResultsControl1
            // 
            this.o2TestResultsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.o2TestResultsControl1.Location = new System.Drawing.Point(14, 35);
            this.o2TestResultsControl1.Name = "o2TestResultsControl1";
            this.o2TestResultsControl1.Size = new System.Drawing.Size(774, 232);
            this.o2TestResultsControl1.TabIndex = 11;
            // 
            // OxygenSensorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.o2TestResultsControl1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.comboOxygenSensor);
            this.Controls.Add(this.lblOxygenSensor);
            this.Controls.Add(this.o2WaveformControl1);
            this.Name = "OxygenSensorsForm";
            this.Text = "OxygenSensorsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ComboBox comboOxygenSensor;
        private System.Windows.Forms.Label lblOxygenSensor;
        private O2Waveform.O2WaveformControl o2WaveformControl1;
        private O2TestResultsControl o2TestResultsControl1;
    }
}