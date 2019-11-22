using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class ProtocolForm : Form {
        DataTable m_dtContent;
        OBDTest m_obdTest;

        public ProtocolForm(OBDTest obdTest) {
            InitializeComponent();
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
            }
            SetDataTableRow(m_dtContent);
        }

        private void ProtocolForm_Load(object sender, EventArgs e) {
            this.GridContent.DataSource = m_dtContent;
            SetDataTableContent();
        }

        private void ProtocolForm_Resize(object sender, EventArgs e) {
            int margin = this.grpBoxProtocol.Location.X - (this.grpBoxModel.Location.X + this.grpBoxModel.Width);
            this.grpBoxModel.Width = (this.btnModify.Location.X - this.grpBoxModel.Location.X) / 3 - margin;
            this.grpBoxProtocol.Location = new Point(this.grpBoxModel.Location.X + this.grpBoxModel.Width + margin, this.grpBoxModel.Location.Y);
            this.grpBoxProtocol.Width = this.grpBoxModel.Width * 2 + margin;
        }
    }
}
