using OfficeOpenXml;
using OfficeOpenXml.Style;
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
    public partial class CheckForm : Form {
        readonly DataTable m_dtContent;
        readonly OBDTest m_obdTest;

        public CheckForm(OBDTest obdTest) {
            InitializeComponent();
            m_dtContent = new DataTable();
            m_obdTest = obdTest;
        }

        private void CheckForm_Resize(object sender, EventArgs e) {
            int margin = this.grpBoxECUID.Location.X - (this.grpBoxType.Location.X + this.grpBoxType.Width);
            this.grpBoxType.Width = (this.btnModify.Location.X - this.grpBoxType.Location.X) / 4 - margin;
            this.grpBoxECUID.Location = new Point(this.grpBoxType.Location.X + this.grpBoxType.Width + margin, this.grpBoxType.Location.Y);
            this.grpBoxECUID.Width = this.grpBoxType.Width;
            this.grpBoxCALID.Location = new Point(this.grpBoxECUID.Location.X + this.grpBoxECUID.Width + margin, this.grpBoxType.Location.Y);
            this.grpBoxCALID.Width = this.grpBoxType.Width;
            this.grpBoxCVN.Location = new Point(this.grpBoxCALID.Location.X + this.grpBoxCALID.Width + margin, this.grpBoxType.Location.Y);
            this.grpBoxCVN.Width = this.grpBoxType.Width;
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
            string[,] results = m_obdTest.m_db.GetRecords("VehicleType", null);
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
            string[] columns = m_obdTest.m_db.GetTableColumns("VehicleType");
            SetDataTableColumns<string>(m_dtContent, columns);
            if (this.GridContent.Columns.Count > 0) {
                SetGridViewColumnsSortMode(this.GridContent, DataGridViewColumnSortMode.Programmatic);
            }
            SetDataTableRow(m_dtContent);
        }

        private bool ArrangeRecords(string[,] records, int[] order) {
            bool bRet = true;
            OrderArray.Orderby(records, order);
            string[] row = new string[records.GetLength(1) - 1];
            List<string[]> arranged = new List<string[]>();
            for (int j = 0; j < records.GetLength(1) - 1; j++) {
                row[j] = records[0, j + 1];
            }
            arranged.Add(row);
            for (int i = 1; i < records.GetLength(0); i++) {
                bool equal = true;
                row = new string[records.GetLength(1) - 1];
                for (int j = 0; j < records.GetLength(1) - 1; j++) {
                    row[j] = records[i, j + 1];
                    equal = equal && row[j] == records[i - 1, j + 1];
                }
                if (!equal) {
                    arranged.Add(row);
                }
            }
            m_obdTest.m_db.DeleteVehicleType(null);
            m_obdTest.m_db.ResetTableID("VehicleType");
            int count = 0;
            foreach (string[] item in arranged) {
                count += m_obdTest.m_db.InsertVehicleType(item[0], item[1], item[2], item[3]);
            }
            if (arranged.Count != count) {
                bRet = false;
                m_obdTest.m_db.m_log.TraceError("Arrange error, arranged and inserted count: " + count.ToString() + ", arranged count: " + arranged.Count.ToString());
                MessageBox.Show("整理数据出错，已整理行数：" + count.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return bRet;
        }

        private void CheckForm_Load(object sender, EventArgs e) {
            this.GridContent.DataSource = m_dtContent;
            SetDataTableContent();
        }

        private void GridContent_Click(object sender, EventArgs e) {
            int index = -1;
            if (this.GridContent.CurrentRow != null) {
                index = this.GridContent.CurrentRow.Index;
            }
            if (index >= 0 && index < m_dtContent.Rows.Count) {
                this.txtBoxType.Text = m_dtContent.Rows[index]["Type"].ToString();
                this.txtBoxECUID.Text = m_dtContent.Rows[index]["ECU_ID"].ToString();
                this.txtBoxCALID.Text = m_dtContent.Rows[index]["CAL_ID"].ToString();
                this.txtBoxCVN.Text = m_dtContent.Rows[index]["CVN"].ToString();
            }
        }

        private void BtnModify_Click(object sender, EventArgs e) {
            if (txtBoxType.Text.Length > 0 && txtBoxECUID.Text.Length > 0 && txtBoxCALID.Text.Length > 0 && txtBoxCVN.Text.Length > 0) {
                int index = this.GridContent.CurrentRow.Index;
                string strID = m_dtContent.Rows[index]["ID"].ToString();
                int iResult = m_obdTest.m_db.UpdateVehicleType(txtBoxType.Text, txtBoxECUID.Text, txtBoxCALID.Text, txtBoxCVN.Text, strID);
                if (iResult > 0) {
                    SetDataTableContent();
                    this.GridContent.Rows[index].Selected = true;
                    this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
                } else {
                    m_obdTest.m_db.m_log.TraceError("Modify error, modified count: " + iResult.ToString());
                    MessageBox.Show("修改数据出错，修改行数：" + iResult.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnInsert_Click(object sender, EventArgs e) {
            if (txtBoxType.Text.Length > 0 && txtBoxECUID.Text.Length > 0 && txtBoxCALID.Text.Length > 0 && txtBoxCVN.Text.Length > 0) {
                int index = this.GridContent.Rows.Count;
                int iResult = m_obdTest.m_db.InsertVehicleType(txtBoxType.Text, txtBoxECUID.Text, txtBoxCALID.Text, txtBoxCVN.Text);
                if (iResult > 0) {
                    SetDataTableContent();
                    this.GridContent.Rows[index].Selected = true;
                    this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
                } else {
                    m_obdTest.m_db.m_log.TraceError("Insert error, inserted count: " + iResult.ToString());
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
                        deletedCount += m_obdTest.m_db.DeleteVehicleType(rowView.Row["ID"].ToString());
                    }
                }
                SetDataTableContent();
                if (deletedCount != selectedCount) {
                    m_obdTest.m_db.m_log.TraceError("Remove error, removed count: " + deletedCount.ToString() + ", selected item count: " + selectedCount.ToString());
                    MessageBox.Show("删除数据出错，删除行数：" + deletedCount.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnArrange_Click(object sender, EventArgs e) {
            string[,] temp = new string[m_dtContent.Rows.Count, m_dtContent.Columns.Count];
            for (int i = 0; i < m_dtContent.Rows.Count; i++) {
                for (int j = 0; j < m_dtContent.Columns.Count; j++) {
                    temp[i, j] = m_dtContent.Rows[i][j].ToString();
                }
            }
            ArrangeRecords(temp, new int[] { 1, 2, 3, 4 });
            SetDataTableContent();
        }

        private void MenuItemImport_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Title = "打开 Excel 导入文件",
                Filter = "Excel 2007 及以上 (*.xlsx)|*.xlsx",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            DialogResult result = openFileDialog.ShowDialog();
            try {
                if (result == DialogResult.OK && openFileDialog.FileName.Length > 0) {
                    FileInfo xlFile = new FileInfo(openFileDialog.FileName);
                    using (ExcelPackage package = new ExcelPackage(xlFile, true)) {
                        ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                        for (int i = 2; i < worksheet1.Cells.Rows; i++) {
                            if (worksheet1.Cells[i, 1].Value == null || worksheet1.Cells[i, 1].Value.ToString().Length == 0) {
                                break;
                            }
                            string type = worksheet1.Cells[i, 2].Value.ToString();
                            string ECUID = worksheet1.Cells[i, 3].Value.ToString();
                            string CALID = worksheet1.Cells[i, 4].Value.ToString();
                            string CVN = worksheet1.Cells[i, 5].Value.ToString();
                            m_obdTest.m_db.InsertVehicleType(type, ECUID, CALID, CVN);
                        }
                    }
                }
                string[,] results = m_obdTest.m_db.GetRecords("VehicleType", null);
                if (results != null) {
                    ArrangeRecords(results, new int[] { 1, 2, 3, 4 });
                    SetDataTableContent();
                    MessageBox.Show("导入Excel数据完成", "导入数据");
                }
            } finally {
                openFileDialog.Dispose();
            }
        }

        private void MenuItemExport_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "保存 Excel 导出文件",
                Filter = "Excel 2007 及以上 (*.xlsx)|*.xlsx",
                FilterIndex = 0,
                RestoreDirectory = true,
                OverwritePrompt = true,
                FileName = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd") + "_Export"
            };
            DialogResult result = saveFileDialog.ShowDialog();
            try {
                if (result == DialogResult.OK) {
                    using (ExcelPackage package = new ExcelPackage()) {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("车型校验文件");
                        // 标题
                        for (int i = 0; i < m_dtContent.Columns.Count; i++) {
                            worksheet.Cells[1, i + 1].Value = m_dtContent.Columns[i].ColumnName;
                            // 边框
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        }
                        // 格式化标题
                        using (var range = worksheet.Cells[1, 1, 1, m_dtContent.Columns.Count]) {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        // 记录
                        for (int iRow = 0; iRow < m_dtContent.Rows.Count; iRow++) {
                            for (int iCol = 0; iCol < m_dtContent.Columns.Count; iCol++) {
                                worksheet.Cells[iRow + 2, iCol + 1].Value = m_dtContent.Rows[iRow][iCol].ToString();
                                // 边框
                                worksheet.Cells[iRow + 2, iCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                            }
                        }
                        // 格式化记录
                        using (var range = worksheet.Cells[2, 1, m_dtContent.Rows.Count + 1, m_dtContent.Columns.Count]) {
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                        // 自适应列宽
                        worksheet.Cells.AutoFitColumns(0);
                        // 保存文件
                        FileInfo xlFile = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(xlFile);
                    }
                    MessageBox.Show("导出Excel数据完成", "导出数据");
                }
            } finally {
                saveFileDialog.Dispose();
            }
        }

        private void MenuItemRefresh_Click(object sender, EventArgs e) {
            int index = this.GridContent.CurrentRow.Index;
            SetDataTableContent();
            if (this.GridContent.Rows.Count > index) {
                this.GridContent.Rows[index].Selected = true;
                this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
            }
        }
    }
}
