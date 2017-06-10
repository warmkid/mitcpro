using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



//this form is about the 7th picture of the extrusion step 
namespace mySystem.Extruction.Chart
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

            //this part initialize the listbox
            this.ltbShow.Items.Add("Date\t\t"+"kind\t\t"+"Time\t\t"+"Engine"+"\t\t"+"Valve"+"\t\t"+"Material"+"\t\t"+"Alert"+"\t\t"+"Solve");


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
            this.rdbDay.Checked = true;
            this.rdbNight.Checked = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Format = DateTimePickerFormat.Short;
            this.dtpDate.ShowUpDown = true;
            this.dtpHour.CustomFormat = "HH:mm:ss";
            this.dtpHour.Format = DateTimePickerFormat.Time;
            this.dtpHour.ShowUpDown = true;*/
            
            //this.ltbShow.Items.Add(this.cmbEngine.SelectedItem.ToString());
            int workflag = new int();
            string workflag1 = "";
            if (this.rdbDay.Checked)
            {
                workflag = 0;
                workflag1 = "day work";
            }
            else if (this.rdbNight.Checked)
            {
                workflag = 1;
                workflag1 = "night work";
            }
            this.ltbShow.Items.Add(this.dtpDate.Value.ToShortDateString()+"\t"+workflag1+"\t"+this.dtpHour.Value.ToShortTimeString()+"\t\t"+ this.cmbEngine.SelectedItem.ToString()+"\t\t"+this.cmbValve.SelectedItem.ToString()+"\t\t"+this.cmbMaterial.SelectedItem.ToString()+"\t\t"+this.cmbAlert.SelectedItem.ToString()+"\t\t"+this.cmbSolve.SelectedItem.ToString());

        }

        private void btnrRemoveItem_Click(object sender, EventArgs e)
        {
            this.ltbShow.Items.Remove(this.ltbShow.SelectedItem);
        }
    }
}
