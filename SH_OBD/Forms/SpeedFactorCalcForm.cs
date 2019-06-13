using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SH_OBD {
    public partial class SpeedFactorCalcForm : Form {
        private float m_fFactor;

        public SpeedFactorCalcForm() {
            InitializeComponent();
            CalculateFactor();
        }

        private new void TextChanged(object sender, EventArgs e) {
            CalculateFactor();
        }

        private void CalculateFactor() {
            try {
                float factor = (float)(
                    Convert.ToDouble(txtStockAspectRatio.Text, CultureInfo.InvariantCulture)
                    * 0.00999999977648258
                    * Convert.ToDouble(txtStockTireWidth.Text, CultureInfo.InvariantCulture)
                    * 0.508000016212463
                    ) + Convert.ToSingle(txtStockRimDiameter.Text);
                m_fFactor = ((float)(
                        Convert.ToDouble(txtCurrentAspectRatio.Text, CultureInfo.InvariantCulture)
                        * 0.00999999977648258
                        * Convert.ToDouble(txtCurrentTireWidth.Text, CultureInfo.InvariantCulture)
                        * 0.508000016212463
                        )
                        + Convert.ToSingle(txtCurrentRimDiameter.Text, CultureInfo.InvariantCulture)
                    )
                    / factor;
                txtFactor.Text = m_fFactor.ToString("0.000");
                btnSave.Enabled = true;
            } catch (FormatException) {
                txtFactor.Text = "ERROR";
                btnSave.Enabled = false;
            }
        }

        public float SpeedFactor {
            get { return m_fFactor; }
        }

    }
}
