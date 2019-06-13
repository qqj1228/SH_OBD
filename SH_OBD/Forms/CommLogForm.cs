using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class CommLogForm : Form {
        public CommLogForm() {
            InitializeComponent();
        }

        public new void Update() {
            if (!File.Exists("commlog.txt")) {
                return;
            }
            FileStream fileStream = new FileStream("commlog.txt", FileMode.Open, FileAccess.Read);
            richTextBox.LoadFile((Stream)fileStream, RichTextBoxStreamType.PlainText);
            fileStream.Close();
        }

    }
}
