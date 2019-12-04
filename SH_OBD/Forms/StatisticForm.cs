using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class StatisticForm : Form {
        private readonly DataTable m_dtContent;
        private readonly string[] m_columns;
        private readonly OBDTest m_obdTest;
        private int m_allQty;
        private int m_passedQty;
        private int m_uploadedQty;
        private readonly int m_pageSize;

        public StatisticForm(OBDTest obdTest) {
            InitializeComponent();
            m_dtContent = new DataTable();
            m_obdTest = obdTest;
            m_columns = new string[4];
            m_columns[0] = "WriteTime";
            m_columns[1] = "VIN";
            m_columns[2] = "Result";
            m_columns[3] = "Upload";
            m_allQty = 0;
            m_passedQty = 0;
            m_uploadedQty = 0;
            m_pageSize = 500;
        }

        private void SetGridViewColumnsSortMode(DataGridView gridView, DataGridViewColumnSortMode sortMode) {
            for (int i = 0; i < gridView.Columns.Count; i++) {
                gridView.Columns[i].SortMode = sortMode;
            }
        }

        private void SetDataTableColumns<T>(DataTable dt, string[] columns) {
            dt.Clear();
            dt.Columns.Clear();
            foreach (string col in columns) {
                dt.Columns.Add(new DataColumn(col, typeof(T)));
            }
        }

        private void SetDataTableRow(DataTable dt, string[] columns) {
            Dictionary<string, string> whereDic = new Dictionary<string, string>();
            if (this.cmbBoxResult.SelectedIndex > 0) {
                if (this.cmbBoxResult.SelectedIndex == 1) {
                    whereDic.Add("Result", "1");
                } else if (this.cmbBoxResult.SelectedIndex == 2) {
                    whereDic.Add("Result", "0");
                }
            }
            if (this.cmbBoxUpload.SelectedIndex > 0) {
                if (this.cmbBoxUpload.SelectedIndex == 1) {
                    whereDic.Add("Upload", "1");
                } else if (this.cmbBoxUpload.SelectedIndex == 2) {
                    whereDic.Add("Upload", "0");
                }
            }
            Model.FilterTime time = Model.FilterTime.NoFilter;
            if (this.radioBtnDay.Checked) {
                time = Model.FilterTime.Day;
            } else if (this.radioBtnWeek.Checked) {
                time = Model.FilterTime.Week;
            } else if (this.radioBtnMonth.Checked) {
                time = Model.FilterTime.Month;
            }
            int max = (m_allQty / m_pageSize) + (m_allQty % m_pageSize > 0 ? 1 : 0);
            this.UpDownPage.Maximum = max > 0 ? max : 1;
            this.lblAllPage.Text = "页 / 共 " + this.UpDownPage.Maximum.ToString() + " 页";
            string[,] results = m_obdTest.m_db.GetRecords("OBDData", columns, whereDic, time, decimal.ToInt32(this.UpDownPage.Value), m_pageSize);
            if (results == null) {
                return;
            }
            List<string> distinct = new List<string>();
            for (int iRow = 0; iRow < results.GetLength(0); iRow++) {
                if (distinct.Contains(results[iRow, 1])) {
                    int index = distinct.IndexOf(results[iRow, 1]);
                    for (int iCol = 0; iCol < results.GetLength(1); iCol++) {
                        dt.Rows[index][iCol] = results[iRow, iCol];
                    }
                } else {
                    distinct.Add(results[iRow, 1]);
                    DataRow dr = dt.NewRow();
                    for (int iCol = 0; iCol < results.GetLength(1); iCol++) {
                        dr[iCol] = results[iRow, iCol];
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        private void SetDataTableContent() {
            SetDataTableColumns<string>(m_dtContent, m_columns);
            SetGridViewColumnsSortMode(this.GridContent, DataGridViewColumnSortMode.Programmatic);
            SetDataTableRow(m_dtContent, m_columns);
        }

        private void ShowResult(Label lbl, string[,] results, ref int qty) {
            if (results != null && results.GetLength(0) > 0) {
                int.TryParse(results[0, 0].ToString(), out qty);
                if (qty < 10000) {
                    lbl.Text = results[0, 0];
                } else {
                    lbl.Text = (qty / 10000.0).ToString("F2") + "万";
                }
            } else {
                qty = 0;
                lbl.Text = "0";
            }
        }

        private void GetQty() {
            Model.FilterTime time = Model.FilterTime.Day;
            if (this.radioBtnDay.Checked) {
                time = Model.FilterTime.Day;
            } else if (this.radioBtnWeek.Checked) {
                time = Model.FilterTime.Week;
            } else if (this.radioBtnMonth.Checked) {
                time = Model.FilterTime.Month;
            }
            Dictionary<string, string> whereDic = new Dictionary<string, string>();
            string[] columns = { "VIN" };
            string[,] results = m_obdTest.m_db.GetRecordsCount("OBDData", columns, whereDic, time);
            ShowResult(this.lblAllQty, results, ref m_allQty);

            whereDic = new Dictionary<string, string> { { "Result", "1" } };
            results = m_obdTest.m_db.GetRecordsCount("OBDData", columns, whereDic, time);
            ShowResult(this.lblPassedQty, results, ref m_passedQty);
            this.lblPassedRate.Text = (m_passedQty * 100.0 / (float)m_allQty).ToString("F2") + "%";

            whereDic = new Dictionary<string, string> { { "Upload", "1" } };
            results = m_obdTest.m_db.GetRecordsCount("OBDData", columns, whereDic, time);
            ShowResult(this.lblUploadedQty, results, ref m_uploadedQty);
            this.lblUploadedRate.Text = (m_uploadedQty * 100.0 / (float)m_allQty).ToString("F2") + "%";
        }

        private void StatisticForm_Load(object sender, EventArgs e) {
            this.GridContent.DataSource = m_dtContent;
            this.radioBtnDay.Checked = true;
            this.lblAllQty.Text = m_allQty.ToString();
            this.lblPassedQty.Text = m_passedQty.ToString();
            this.lblPassedRate.Text = "0%";
            this.lblUploadedQty.Text = m_uploadedQty.ToString();
            this.lblUploadedRate.Text = "0%";
            this.cmbBoxResult.SelectedIndex = 1;
            this.cmbBoxUpload.SelectedIndex = 1;
            //Task.Factory.StartNew(SetDataTableContent);
            //SetDataTableContent();
        }

        private void StatisticForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (m_dtContent != null) {
                m_dtContent.Dispose();
            }
        }

        private void Option_Click(object sender, EventArgs e) {
            GetQty();
            SetDataTableContent();
        }
    }
}
