﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class CheckForm : Form {
        DataTable m_dtContent;
        OBDTest m_obdTest;
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

        private void CheckForm_Load(object sender, EventArgs e) {
            this.GridContent.DataSource = m_dtContent;
            SetDataTableContent();
        }

        private void GridContent_Click(object sender, EventArgs e) {
            int index = this.GridContent.CurrentRow.Index;
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
                    MessageBox.Show("删除数据出错，删除行数：" + deletedCount.ToString(), "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e) {
            int index = this.GridContent.CurrentRow.Index;
            SetDataTableContent();
            this.GridContent.Rows[index].Selected = true;
            this.GridContent.CurrentCell = this.GridContent.Rows[index].Cells[0];
        }
    }
}