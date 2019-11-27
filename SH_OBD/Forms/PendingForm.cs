using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class PendingForm : Form {
        private System.Timers.Timer m_timer;
        private readonly string m_strInfo;
        private int m_second;
        private bool m_stop;
        readonly OBDParameter m_param;
        readonly OBDParser m_parser;
        readonly OBDCommELM m_commELM;

        public PendingForm(OBDParameter param, OBDParser parser, OBDCommELM commELM) {
            this.CenterToScreen();
            InitializeComponent();
            m_strInfo = @"ECU忙，正等待其返回数据
可能会持续1分钟以上
点击“中断OBD检测”按钮或关闭本窗口
可以中断当前车辆检测进程";
            m_param = param;
            m_parser = parser;
            m_commELM = commELM;
            m_timer = new System.Timers.Timer(1000);
        }

        private void OnTimeout(object source, System.Timers.ElapsedEventArgs e) {
            if (m_stop) {
                m_timer.Enabled = false;
            }
            ++m_second;
            this.Invoke((EventHandler)delegate {
                if (this.progressBar1 != null) {
                    if (this.progressBar1.Value >= this.progressBar1.Maximum) {
                        this.progressBar1.Value = 0;
                    }
                    this.progressBar1.PerformStep();
                }
                if (this.txtBoxInfo != null) {
                    this.txtBoxInfo.Text = m_strInfo + Environment.NewLine + "已等待: " + m_second.ToString() + "秒";
                }
            });
            OBDResponseList orl = null;
            if (m_second % 5 == 0) {
                if (m_commELM.Online) {
                    orl = m_parser.Parse(m_param, m_commELM.GetResponse(m_param.OBDRequest));
                    if (!orl.ErrorDetected && orl.RawResponse != "PENDING" && orl.RawResponse != "TPPR") {
                        m_timer.Enabled = false;
                        m_stop = true;
                        this.Tag = orl;
                        this.Invoke((EventHandler)delegate {
                            this.Close();
                        });
                    }
                }
            }
            if (m_second > 100) {
                m_timer.Enabled = false;
                m_stop = true;
                this.Tag = orl;
                this.Invoke((EventHandler)delegate {
                    this.Close();
                });
            }
        }

        private void TimeoutForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (m_timer != null) {
                m_timer.Enabled = false;
                m_timer.Dispose();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            m_stop = true;
            this.Close();
        }

        private void TimeoutForm_Load(object sender, EventArgs e) {
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimeout);
            m_timer.AutoReset = true;
            m_timer.Enabled = true;
            m_second = 0;
            this.txtBoxInfo.Text = m_strInfo + Environment.NewLine + "已等待: " + m_second.ToString() + "秒";
        }
    }
}
