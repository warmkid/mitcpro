using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



//this form is about the 7th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class RunningRecordofFeedingUnit : Form
    {
        public RunningRecordofFeedingUnit()
        {
            InitializeComponent();

            //this part add items to the cmblist
            this.cmbEngine.Items.Add("yes");
            this.cmbEngine.Items.Add("reason 1");
            this.cmbEngine.Items.Add("reasin 2");
            this.cmbValve.Items.Add("yes");
            this.cmbValve.Items.Add("reason 1");
            this.cmbValve.Items.Add("reason 2");
            this.cmbMaterial.Items.Add("yes");
            this.cmbMaterial.Items.Add("reason 1");
            this.cmbMaterial.Items.Add("reason 2");
            this.cmbAlert.Items.Add("no");
            this.cmbAlert.Items.Add("yes");
            this.cmbSolve.Items.Add("yes");
            this.cmbSolve.Items.Add("no");

        }

        private void brnDefault_Click(object sender, EventArgs e)
        {
            this.cmbEngine.Text = "yes";
            this.cmbValve.Text = "yes";
            this.cmbMaterial.Text = "yes";
            this.cmbAlert.Text = "no";
            this.cmbSolve.Text = "no";
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Format = DateTimePickerFormat.Custom;
            this.dtpDate.ShowUpDown = true;
            this.dtpDate.Value = DateTime.Today;
            this.dtpHour.CustomFormat = "HH:mm:ss";
            this.dtpHour.Format = DateTimePickerFormat.Custom;
            this.dtpHour.ShowUpDown = true;
            this.dtpHour.Value = DateTime.Now;

            this.txbCheckman.Text = "wang";
            this.txbRecheckman.Text = "li";
            
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        private void ckbDy_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDy.Checked)
            {
                ckbNt.Checked = false;
            }
        }

        private void ckbNt_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbNt.Checked)
            {
                ckbDy.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
            check.ShowDialog();
        }

       

    }
}
