using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class PassWordForm : Form {
        private readonly OBDTest m_obdTest;

        public PassWordForm(OBDTest obdTest) {
            InitializeComponent();
            m_obdTest = obdTest;
        }

        private void BtnOK_Click(object sender, EventArgs e) {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(Encoding.Default.GetBytes(this.txtBoxPassWord.Text.Trim()));
            string strValue = BitConverter.ToString(output).Replace("-", "");
            if (strValue == m_obdTest.m_db.GetPassWord()) {
                m_obdTest.AccessAdvanceMode = 1;
            } else {
                m_obdTest.AccessAdvanceMode = -1;
            }
            md5.Dispose();
            this.Close();
        }
    }
}
