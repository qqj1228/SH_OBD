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
    public partial class UserPreferencesForm : Form {
        private readonly UserPreferences m_userpreferences;
        private readonly OBDTest m_obdTest;

        public UserPreferencesForm(UserPreferences prefs, OBDTest obdTest) {
            InitializeComponent();
            m_userpreferences = prefs;
            m_obdTest = obdTest;
        }

        private void BtnSave_Click(object sender, EventArgs e) {
            m_userpreferences.Name = txtName.Text;
            m_userpreferences.Address1 = txtAddress1.Text;
            m_userpreferences.Address2 = txtAddress2.Text;
            m_userpreferences.Telephone = txtTelephone.Text;
            bool CanClose = true;
            if (this.txtBoxOriPwd.Text.Length > 0 && this.txtBoxNewPwd1.Text.Length > 0 && this.txtBoxNewPwd2.Text.Length > 0) {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(Encoding.Default.GetBytes(this.txtBoxOriPwd.Text.Trim()));
                string strValue = BitConverter.ToString(output).Replace("-", "");
                if (strValue == m_obdTest.m_db.GetPassWord()) {
                    if (this.txtBoxNewPwd1.Text != this.txtBoxNewPwd2.Text) {
                        CanClose = false;
                        MessageBox.Show("两次输入的新密码不一致！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        output = md5.ComputeHash(Encoding.Default.GetBytes(this.txtBoxNewPwd1.Text.Trim()));
                        strValue = BitConverter.ToString(output).Replace("-", "");
                        if (m_obdTest.m_db.SetPassWord(strValue) == 1) {
                            MessageBox.Show("修改管理员密码成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                } else {
                    CanClose = false;
                    MessageBox.Show("原密码不正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                md5.Dispose();
            }
            if (CanClose) {
                Close();
            }
        }

        private void UserPreferencesForm_Load(object sender, EventArgs e) {
            txtName.Text = m_userpreferences.Name;
            txtAddress1.Text = m_userpreferences.Address1;
            txtAddress2.Text = m_userpreferences.Address2;
            txtTelephone.Text = m_userpreferences.Telephone;
        }

    }
}
