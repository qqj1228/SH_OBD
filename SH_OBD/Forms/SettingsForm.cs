﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class SettingsForm : Form {
        private readonly Settings m_settings;
        private readonly DBandMES m_dbandMES;
        private bool m_lastURLStatus;

        public SettingsForm(Settings settings, DBandMES dbandMES) {
            InitializeComponent();
            m_settings = settings;
            m_dbandMES = dbandMES;
        }

        private void SettingsForm_Load(object sender, EventArgs e) {
            try {
                string[] serialPorts = SerialPort.GetPortNames();
                foreach (string serialPort in serialPorts) {
                    comboPorts.Items.Add(serialPort);
                    cmbBoxScannerPort.Items.Add(serialPort);
                }

                if (CommBase.IsPortAvailable(m_settings.ComPort)) {
                    comboPorts.SelectedItem = m_settings.ComPortName;
                } else if (comboPorts.Items.Count > 0) {
                    comboPorts.SelectedIndex = 0;
                }

                if (CommBase.IsPortAvailable(m_settings.ScannerPort)) {
                    cmbBoxScannerPort.SelectedItem = m_settings.ScannerPortName;
                } else {
                    for (int i = 0; i < cmbBoxScannerPort.Items.Count; i++) {
                        if (i != comboPorts.SelectedIndex) {
                            cmbBoxScannerPort.SelectedIndex = i;
                            break;
                            // 自动设置可用串口
                            //if (CommBase.IsPortAvailable(Convert.ToInt32(cmbBoxScannerPort.Items[i].ToString().Remove(0, 3)))) {
                            //    cmbBoxScannerPort.SelectedIndex = i;
                            //    break;
                            //}
                        }
                    }
                }

                comboHardware.SelectedIndex = m_settings.HardwareIndexInt;
                comboBaud.SelectedIndex = m_settings.BaudRateIndex;
                cmbBoxScannerBaud.SelectedIndex = m_settings.ScannerBaudRateIndex;

                this.chkBoxUseSerialScanner.Checked = m_settings.UseSerialScanner;
                this.cmbBoxScannerPort.Enabled = this.chkBoxUseSerialScanner.Checked;
                this.cmbBoxScannerBaud.Enabled = this.chkBoxUseSerialScanner.Checked;

                foreach (string item in Settings.ProtocolNames) {
                    comboProtocol.Items.Add(item);
                }

                comboProtocol.SelectedIndex = m_settings.ProtocolIndexInt;
                comboInitialize.SelectedIndex = !m_settings.DoInitialization ? 1 : 0;
                if (m_settings.AutoDetect) {
                    checkBoxAutoDetect.Checked = true;
                } else {
                    checkBoxAutoDetect.Checked = false;
                }

                this.txtBoxUser.Text = m_dbandMES.UserName;
                this.txtBoxPwd.Text = m_dbandMES.PassWord;
                this.txtBoxDBName.Text = m_dbandMES.DBName;
                this.txtBoxIP.Text = m_dbandMES.IP;
                this.txtBoxPort.Text = m_dbandMES.Port;
                this.txtBoxWebSvcAddress.Text = m_dbandMES.WebServiceAddress;
                this.txtBoxWebSvcName.Text = m_dbandMES.WebServiceName;
                this.txtBoxWebSvcMethods.Text = m_dbandMES.WebServiceMethods;
                this.txtBoxWebSvcWSDL.Text = m_dbandMES.WebServiceWSDL;
                if (m_dbandMES.UseURL) {
                    this.radioBtnURL.Checked = true;
                    m_lastURLStatus = this.radioBtnURL.Checked;
                    this.txtBoxWebSvcAddress.Enabled = true;
                    this.txtBoxWebSvcWSDL.Enabled = false;
                } else {
                    this.radioBtnWSDL.Checked = true;
                    this.txtBoxWebSvcAddress.Enabled = false;
                    this.txtBoxWebSvcWSDL.Enabled = true;
                }
                m_lastURLStatus = this.radioBtnURL.Checked;
                this.radioBtnURL.Enabled = m_dbandMES.ChangeWebService;
                this.radioBtnWSDL.Enabled = m_dbandMES.ChangeWebService;
                this.chkBoxUploadWhenever.Checked = m_dbandMES.UploadWhenever;
                this.chkBoxUseECUName.Checked = m_dbandMES.UseECUName;
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnOK_Click(object sender, EventArgs e) {
            m_settings.AutoDetect = checkBoxAutoDetect.Checked;
            if (comboPorts.SelectedItem != null && comboPorts.SelectedItem.ToString().Length > 3) {
                m_settings.ComPort = Convert.ToInt32(comboPorts.SelectedItem.ToString().Remove(0, 3));
            }
            m_settings.UseSerialScanner = chkBoxUseSerialScanner.Checked;
            if (cmbBoxScannerPort.SelectedItem != null && cmbBoxScannerPort.SelectedItem.ToString().Length > 3) {
                m_settings.ScannerPort = Convert.ToInt32(cmbBoxScannerPort.SelectedItem.ToString().Remove(0, 3));
            }
            m_settings.ScannerBaudRateIndex = cmbBoxScannerBaud.SelectedIndex;
            m_settings.BaudRateIndex = comboBaud.SelectedIndex;
            m_settings.HardwareIndexInt = comboHardware.SelectedIndex;
            m_settings.ProtocolIndexInt = comboProtocol.SelectedIndex;
            m_settings.DoInitialization = (comboInitialize.SelectedIndex == 0);

            m_dbandMES.UserName = this.txtBoxUser.Text;
            m_dbandMES.PassWord = this.txtBoxPwd.Text;
            m_dbandMES.DBName = this.txtBoxDBName.Text;
            m_dbandMES.IP = this.txtBoxIP.Text;
            m_dbandMES.Port = this.txtBoxPort.Text;
            m_dbandMES.WebServiceAddress = this.txtBoxWebSvcAddress.Text;
            m_dbandMES.WebServiceName = this.txtBoxWebSvcName.Text;
            m_dbandMES.WebServiceMethods = this.txtBoxWebSvcMethods.Text;
            m_dbandMES.WebServiceWSDL = this.txtBoxWebSvcWSDL.Text;
            m_dbandMES.UseURL = this.radioBtnURL.Checked;
            m_dbandMES.UploadWhenever = this.chkBoxUploadWhenever.Checked;
            m_dbandMES.UseECUName = this.chkBoxUseECUName.Checked;
            Close();
        }

        private void ComboHardware_SelectedIndexChanged(object sender, EventArgs e) {
            //if (comboHardware.SelectedIndex == (int)HardwareType.ELM327) {
            //    groupELM.Enabled = true;
            //} else {
            //    groupELM.Enabled = false;
            //}
        }

        private void CheckBoxAutoDetect_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxAutoDetect.Checked) {
                this.comboPorts.Enabled = false;
                this.comboHardware.Enabled = false;
                this.comboBaud.Enabled = false;
                this.comboProtocol.Enabled = false;
                this.comboInitialize.Enabled = false;
            } else {
                this.comboPorts.Enabled = true;
                this.comboHardware.Enabled = true;
                this.comboBaud.Enabled = true;
                this.comboProtocol.Enabled = true;
                this.comboInitialize.Enabled = true;
            }
        }

        private void RadioBtn_Click(object sender, EventArgs e) {
            if (this.radioBtnURL.Checked) {
                this.txtBoxWebSvcAddress.Enabled = true;
                this.txtBoxWebSvcWSDL.Enabled = false;
            } else {
                this.txtBoxWebSvcAddress.Enabled = false;
                this.txtBoxWebSvcWSDL.Enabled = true;
            }
            if (m_lastURLStatus != this.radioBtnURL.Checked) {
                m_lastURLStatus = this.radioBtnURL.Checked;
                try {
                    File.Delete(this.txtBoxWebSvcName.Text + ".dll");
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "变更WebService方式出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChkBoxUseSerialScanner_CheckedChanged(object sender, EventArgs e) {
            if (chkBoxUseSerialScanner.Checked) {
                this.cmbBoxScannerPort.Enabled = true;
                this.cmbBoxScannerBaud.Enabled = true;
            } else {
                this.cmbBoxScannerPort.Enabled = false;
                this.cmbBoxScannerBaud.Enabled = false;
            }
        }
    }
}
