﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class ProtocolForm : Form {
        private readonly string[] m_Protocols;
        private readonly DataTable m_dtContent;
        private readonly OBDTest m_obdTest;

        public ProtocolForm(OBDTest obdTest) {
            InitializeComponent();
            m_Protocols = new string[] {
                "ISO 15031 CAN",
                "ISO 27145 CAN",
                "SAE J1939 CAN",
                "ISO 14230-4 KWP",
                "ISO 9141-2 K",
                "SAE J1850"
            };
            m_dtContent = new DataTable();
            m_obdTest = obdTest;
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

        private void SetDataTableRow(DataTable dt) {
            string[,] results = m_obdTest.m_db.GetRecords("OBDProtocol", null);
            if (results == null) {
                return;
            }
            for (int iRow = 0; iRow < results.GetLength(0); iRow++) {
                DataRow dr = dt.NewRow();
                for (int iCol = 0; iCol < results.GetLength(1); iCol++) {
                    dr[iCol] = results[iRow, iCol];
                }
                dt.Rows.Add(dr);
            }
        }

        private void SetDataTableContent() {
            string[] columns = m_obdTest.m_db.GetTableColumns("OBDProtocol");
            SetDataTableColumns<string>(m_dtContent, columns);
            if (this.GridContent.Columns.Count > 0) {
                SetGridViewColumnsSortMode(this.GridContent, DataGridViewColumnSortMode.Programmatic);
                this.GridContent.Columns[0].Width = this.GridContent.ClientSize.Width / 20;
            }
            SetDataTableRow(m_dtContent);
        }

        private void ProtocolForm_Load(object sender, EventArgs e) {
            this.GridContent.DataSource = m_dtContent;
            foreach (string protocol in m_Protocols) {
                this.cmbBoxProtocol.Items.Add(protocol);
            }
            SetDataTableContent();
        }

        private void ProtocolForm_Resize(object sender, EventArgs e) {
            int margin = this.grpBoxProtocol.Location.X - (this.grpBoxModel.Location.X + this.grpBoxModel.Width);
            this.grpBoxCARCODE.Width = (this.btnModify.Location.X - this.grpBoxCARCODE.Location.X - margin * 6) / 6;
            this.grpBoxEngine.Location = new Point(this.grpBoxCARCODE.Location.X + this.grpBoxCARCODE.Width + margin, this.grpBoxCARCODE.Location.Y);
            this.grpBoxEngine.Width = this.grpBoxCARCODE.Width;
            this.grpBoxStage.Location = new Point(this.grpBoxEngine.Location.X + this.grpBoxEngine.Width + margin, this.grpBoxCARCODE.Location.Y);
            this.grpBoxStage.Width = this.grpBoxCARCODE.Width;
            this.grpBoxFuel.Location = new Point(this.grpBoxStage.Location.X + this.grpBoxStage.Width + margin, this.grpBoxCARCODE.Location.Y);
            this.grpBoxFuel.Width = this.grpBoxCARCODE.Width;
            this.grpBoxModel.Location = new Point(this.grpBoxFuel.Location.X + this.grpBoxFuel.Width + margin, this.grpBoxCARCODE.Location.Y);
            this.grpBoxModel.Width = this.grpBoxCARCODE.Width;
            this.grpBoxProtocol.Location = new Point(this.grpBoxModel.Location.X + this.grpBoxModel.Width + margin, this.grpBoxCARCODE.Location.Y);
            this.grpBoxProtocol.Width = this.grpBoxCARCODE.Width;
        }

        private void GridContent_Click(object sender, EventArgs e) {
            if (this.GridContent.CurrentRow == null) {
                return;
            }
            int index = this.GridContent.CurrentRow.Index;
            if (index >= 0 && index < m_dtContent.Rows.Count) {
                this.txtBoxCARCODE.Text = m_dtContent.Rows[index]["CAR_CODE"].ToString();
                this.txtBoxEngine.Text = m_dtContent.Rows[index]["Engine"].ToString();
                this.txtBoxStage.Text = m_dtContent.Rows[index]["Stage"].ToString();
                this.txtBoxFuel.Text = m_dtContent.Rows[index]["Fuel"].ToString();
                this.txtBoxModel.Text = m_dtContent.Rows[index]["Model"].ToString();
                string strProtocol = m_dtContent.Rows[index]["Protocol"].ToString();
                if (strProtocol.Contains("15031")) {
                    this.cmbBoxProtocol.SelectedIndex = 0;
                } else if (strProtocol.Contains("27145")) {
                    this.cmbBoxProtocol.SelectedIndex = 1;
                } else if (strProtocol.Contains("1939")) {
                    this.cmbBoxProtocol.SelectedIndex = 2;
                } else if (strProtocol.Contains("14230")) {
                    this.cmbBoxProtocol.SelectedIndex = 3;
                } else if (strProtocol.Contains("9141")) {
                    this.cmbBoxProtocol.SelectedIndex = 4;
                } else if (strProtocol.Contains("1850")) {
                    this.cmbBoxProtocol.SelectedIndex = 5;
                }
            }
        }

        private void MenuItemImport_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "打开车型 OBD 协议文件",
                Filter = "Excel 2007 及以上 (*.xlsx)|*.xlsx",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            DataTable dtImport = new DataTable("OBDProtocol");
            string[] columns = m_obdTest.m_db.GetTableColumns(dtImport.TableName);
            foreach (string col in columns) {
                if (col != "ID") {
                    dtImport.Columns.Add(new DataColumn(col, typeof(string)));
                }
            }
            try {
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length <= 0) {
                    return;
                }
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                using (ExcelPackage package = new ExcelPackage(fileInfo, true)) {
                    ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                    for (int i = 2; i < worksheet1.Cells.Rows; i++) {
                        if (worksheet1.Cells[i, 1].Value == null || worksheet1.Cells[i, 1].Value.ToString().Length == 0) {
                            break;
                        }
                        if (worksheet1.Cells[i, 6].Value == null || worksheet1.Cells[i, 6].Value.ToString().Length == 0) {
                            continue;
                        }
                        DataRow dr = dtImport.NewRow();
                        dr["CAR_CODE"] = worksheet1.Cells[i, 1].Value.ToString();
                        dr["Engine"] = worksheet1.Cells[i, 2].Value.ToString();
                        dr["Stage"] = worksheet1.Cells[i, 3].Value.ToString();
                        dr["Fuel"] = worksheet1.Cells[i, 4].Value.ToString();
                        dr["Model"] = worksheet1.Cells[i, 5].Value.ToString();
                        if (worksheet1.Cells[i, 6].Value.ToString().Contains("15765")) {
                            dr["Protocol"] = m_Protocols[0];
                        } else if (worksheet1.Cells[i, 6].Value.ToString().Contains("27145")) {
                            dr["Protocol"] = m_Protocols[1];
                        } else if (worksheet1.Cells[i, 6].Value.ToString().Contains("1939")) {
                            dr["Protocol"] = m_Protocols[2];
                        } else if (worksheet1.Cells[i, 6].Value.ToString().Contains("14230")) {
                            dr["Protocol"] = m_Protocols[3];
                        } else if (worksheet1.Cells[i, 6].Value.ToString().Contains("9141")) {
                            dr["Protocol"] = m_Protocols[4];
                        } else if (worksheet1.Cells[i, 6].Value.ToString().Contains("1850")) {
                            dr["Protocol"] = m_Protocols[5];
                        }
                        dtImport.Rows.Add(dr);
                    }
                }
                m_obdTest.m_db.AddProtocol(dtImport);
                MessageBox.Show("导入Excel数据完成", "导入数据");
                this.MenuItemRefresh.PerformClick();
            } finally {
                openFileDialog.Dispose();
                dtImport.Dispose();
            }
        }

        private void BtnModify_Click(object sender, EventArgs e) {
            if (txtBoxCARCODE.Text.Length > 0 && cmbBoxProtocol.Text.Length > 0) {
                int index = this.GridContent.CurrentRow.Index;
                string strID = m_dtContent.Rows[index]["ID"].ToString();
                int iResult = m_obdTest.m_db.UpdateProtocol(txtBoxCARCODE.Text, cmbBoxProtocol.Text, strID);
                if (iResult > 0) {
                    SetDataTableContent();
                    this.GridContent.Rows[index].Selected = true;
                    this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
                } else {
                    MessageBox.Show("修改数据出错，修改行数：" + iResult.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnInsert_Click(object sender, EventArgs e) {
            if (txtBoxCARCODE.Text.Length > 0 && cmbBoxProtocol.Text.Length > 0) {
                int index = this.GridContent.Rows.Count;
                int iResult = m_obdTest.m_db.InsertProtocol(txtBoxCARCODE.Text, cmbBoxProtocol.Text);
                if (iResult > 0) {
                    SetDataTableContent();
                    this.GridContent.Rows[index].Selected = true;
                    this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
                } else {
                    MessageBox.Show("插入数据出错，插入行数：" + iResult.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e) {
            int selectedCount = this.GridContent.SelectedRows.Count;
            if (selectedCount > 0) {
                int deletedCount = 0;
                for (int i = 0; i < selectedCount; i++) {
                    if ((DataRowView)this.GridContent.SelectedRows[i].DataBoundItem is DataRowView rowView) {
                        deletedCount += m_obdTest.m_db.DeleteProtocol(rowView.Row["ID"].ToString());
                    }
                }
                SetDataTableContent();
                if (deletedCount != selectedCount) {
                    MessageBox.Show("删除数据出错，删除行数：" + deletedCount.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuItemRefresh_Click(object sender, EventArgs e) {
            int index = 0;
            if (this.GridContent.CurrentRow != null) {
                index = this.GridContent.CurrentRow.Index;
            }
            SetDataTableContent();
            this.GridContent.Rows[index].Selected = true;
            this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
        }

    }
}
