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
    public partial class VehicleForm : Form {
        private List<VehicleProfile> m_VehicleList;
        private bool bDirtyProfile;
        private OBDInterface m_obdInterface;

        public VehicleForm(OBDInterface obd) {
            InitializeComponent();
            m_obdInterface = obd;
            m_VehicleList = m_obdInterface.VehicleProfiles;
        }

        private void btnNewVehicle_Click(object sender, EventArgs e) {
            VehicleProfile vehicleProfile = new VehicleProfile();
            listVehicles.Items.Add(vehicleProfile);
            m_VehicleList.Add(vehicleProfile);
            listVehicles.SetSelected(listVehicles.Items.Count - 1, true);
        }

        private void btnDeleteVehicle_Click(object sender, EventArgs e) {
            if (m_VehicleList.Count > 1) {
                if (MessageBox.Show("This will permanently delete " + listVehicles.SelectedItem.ToString() + ".\n\n Are you sure?", "Delete " + listVehicles.SelectedItem.ToString() + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                    int selectedIndex = listVehicles.SelectedIndex;
                    m_VehicleList.RemoveAt(selectedIndex);
                    UpdateProfileList(m_VehicleList);
                    if (selectedIndex > 0) {
                        listVehicles.SetSelected(selectedIndex - 1, true);
                    }
                    else if (m_VehicleList.Count > 0) {
                        listVehicles.SetSelected(0, true);
                    }
                }
            } else {
                MessageBox.Show("You must keep at least one vehicle profile.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void EditProfile(VehicleProfile profile) {
            txtName.Text = profile.Name;
            numTimeout.Value = new decimal(profile.ElmTimeout);
            if (profile.AutoTransmission) {
                radioAutomatic.Checked = true;
            } else {
                radioManual.Checked = true;
            }

            txtSpeedoFactor.Text = profile.SpeedCalibrationFactor.ToString("0.000");
            txtWeight.Text = profile.Weight.ToString();
            txtDragCoeff.Text = profile.DragCoefficient.ToString("0.000");
            txtTireWidth.Text = profile.Wheel.Width.ToString();
            txtAspectRatio.Text = profile.Wheel.AspectRatio.ToString();
            txtRimDiameter.Text = profile.Wheel.RimDiameter.ToString();
            txtNotes.Text = profile.Notes;
            MarkProfileDirty(false);
        }

        private void btnExit_Click(object sender, EventArgs e) {
            if (bDirtyProfile) {
                if (MessageBox.Show("This profile has been modified since it was last saved.\n\nDo you wish to exit anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                    base.Close();
                }
            } else {
                 base.Close();
            }
        }

        private void VehicleForm_Load(object sender, EventArgs e) {
            UpdateProfileList(m_VehicleList);
            listVehicles.SetSelected(0, true);
        }

        private void UpdateProfileList(List<VehicleProfile> vehicles) {
            listVehicles.Items.Clear();
            foreach (VehicleProfile vehicle in vehicles) {
                listVehicles.Items.Add(vehicle);
            }
        }

        public static int lastSelectedIndex;

        private void listVehicles_SelectedIndexChanged(object sender, EventArgs e) {
            if (listVehicles.SelectedIndex < 0) {
                return;
            }
            if (bDirtyProfile) {
                if (listVehicles.SelectedIndex == lastSelectedIndex) {
                    return;
                }
                listVehicles.SetSelected(lastSelectedIndex, true);
                MessageBox.Show("Please save or discard your changes before switching profiles.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                EditProfile(m_VehicleList[listVehicles.SelectedIndex] as VehicleProfile);
                lastSelectedIndex = listVehicles.SelectedIndex;
            }
        }

        private void MarkProfileDirty(bool bStatus) {
            bDirtyProfile = bStatus;
            if (bDirtyProfile) {
                btnDiscard.Enabled = true;
                btnSave.Enabled = true;
            } else {
                btnDiscard.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void numTimeout_ValueChanged(object sender, EventArgs e) {
            MarkProfileDirty(true);
        }

        private void ValueChanged(object sender, EventArgs e) {
            MarkProfileDirty(true);
        }

        private void btnDiscard_Click(object sender, EventArgs e) {
            EditProfile(m_VehicleList[listVehicles.SelectedIndex] as VehicleProfile);
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (txtName.Text.Length > 0) {
                VehicleProfile vehicle = new VehicleProfile();
                vehicle.Name = txtName.Text;
                vehicle.ElmTimeout = Convert.ToInt32(numTimeout.Value, CultureInfo.InvariantCulture);
                vehicle.AutoTransmission = radioAutomatic.Checked;

                try {
                    vehicle.SpeedCalibrationFactor = Convert.ToSingle(txtSpeedoFactor.Text, CultureInfo.InvariantCulture);
                    vehicle.Weight = Convert.ToSingle(txtWeight.Text, CultureInfo.InvariantCulture);
                    vehicle.DragCoefficient = Convert.ToSingle(txtDragCoeff.Text, CultureInfo.InvariantCulture);
                    vehicle.Wheel.Width = Convert.ToInt32(txtTireWidth.Text, CultureInfo.InvariantCulture);
                    vehicle.Wheel.AspectRatio = Convert.ToInt32(txtAspectRatio.Text, CultureInfo.InvariantCulture);
                    vehicle.Wheel.RimDiameter = Convert.ToInt32(txtRimDiameter.Text, CultureInfo.InvariantCulture);
                    vehicle.Notes = txtNotes.Text;
                } catch (FormatException) {
                    MessageBox.Show("Make sure that numeric fields contain only numeric data, and make sure that you are not forgetting a required field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                m_VehicleList[listVehicles.SelectedIndex] = vehicle;
                MarkProfileDirty(false);

                int selectedIndex = listVehicles.SelectedIndex;
                UpdateProfileList(m_VehicleList);
                listVehicles.SetSelected(selectedIndex, true);
            } else {
                MessageBox.Show("You must enter a name for your profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void VehicleForm_Closing(object sender, CancelEventArgs e) {
            m_obdInterface.SaveVehicleProfiles(m_VehicleList);
        }

        private void txtTireWidth_Enter(object sender, EventArgs e) {
            lblExampleWidth.Font = new Font(lblExampleWidth.Font.FontFamily, 10f, FontStyle.Bold);
            lblExampleWidth.ForeColor = Color.Red;
        }

        private void txtTireWidth_Leave(object sender, EventArgs e) {
            lblExampleWidth.Font = lblExample.Font;
            lblExampleWidth.ForeColor = lblExample.ForeColor;
        }

        private void txtAspectRatio_Enter(object sender, EventArgs e) {
            lblExampleAspect.Font = new Font(lblExampleWidth.Font.FontFamily, 10f, FontStyle.Bold);
            lblExampleAspect.ForeColor = Color.Red;
        }

        private void txtAspectRatio_Leave(object sender, EventArgs e) {
            lblExampleAspect.Font = lblExample.Font;
            lblExampleAspect.ForeColor = lblExample.ForeColor;
        }

        private void txtRimDiameter_Enter(object sender, EventArgs e) {
            lblExampleDiameter.Font = new Font(lblExampleWidth.Font.FontFamily, 10f, FontStyle.Bold);
            lblExampleDiameter.ForeColor = Color.Red;
        }

        private void txtRimDiameter_Leave(object sender, EventArgs e) {
            lblExampleDiameter.Font = lblExample.Font;
            lblExampleDiameter.ForeColor = lblExample.ForeColor;
        }

        private void btnCalcSpeedo_Click(object sender, EventArgs e) {
            SpeedFactorCalcForm speedFactorCalcForm = new SpeedFactorCalcForm();
            if (speedFactorCalcForm.ShowDialog() == DialogResult.OK) {
                txtSpeedoFactor.Text = speedFactorCalcForm.SpeedFactor.ToString("0.000");
                MarkProfileDirty(true);
            }
        }

    }
}
