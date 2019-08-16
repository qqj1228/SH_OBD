namespace SH_OBD {
    partial class TerminalForm {
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
            this.btnSend = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.richText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.AutoSize = true;
            this.btnSend.Location = new System.Drawing.Point(667, 10);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(96, 32);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "发送(&S)";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommand.Location = new System.Drawing.Point(95, 12);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(4);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(563, 25);
            this.txtCommand.TabIndex = 6;
            this.txtCommand.Text = "09 02";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new System.Drawing.Point(16, 12);
            this.lblPrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(71, 28);
            this.lblPrompt.TabIndex = 5;
            this.lblPrompt.Text = "Prompt";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // richText
            // 
            this.richText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richText.Location = new System.Drawing.Point(16, 52);
            this.richText.Margin = new System.Windows.Forms.Padding(4);
            this.richText.Name = "richText";
            this.richText.ReadOnly = true;
            this.richText.Size = new System.Drawing.Size(745, 494);
            this.richText.TabIndex = 4;
            this.richText.Text = "";
            // 
            // TerminalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 562);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.richText);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TerminalForm";
            this.Text = "TerminalForm";
            this.VisibleChanged += new System.EventHandler(this.TerminalForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.RichTextBox richText;
    }
}