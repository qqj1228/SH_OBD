using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class UserPreferencesForm : Form {
        private UserPreferences m_userpreferences;

        public UserPreferencesForm(UserPreferences prefs) {
            InitializeComponent();
            m_userpreferences = prefs;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            m_userpreferences.Name = txtName.Text;
            m_userpreferences.Address1 = txtAddress1.Text;
            m_userpreferences.Address2 = txtAddress2.Text;
            m_userpreferences.Telephone = txtTelephone.Text;
            Close();
        }

        private void UserPreferencesForm_Load(object sender, EventArgs e) {
            txtName.Text = m_userpreferences.Name;
            txtAddress1.Text = m_userpreferences.Address1;
            txtAddress2.Text = m_userpreferences.Address2;
            txtTelephone.Text = m_userpreferences.Telephone;
        }

    }
}
