using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//this form is about the 10th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class WastingRecordofExtrusionUnit : Form
    {
        public int items = 0;
        public WastingRecordofExtrusionUnit()
        {
            InitializeComponent();
            //public int items =1;
            //items = this.ltbShow.Items.Count+1;
            //items = 1;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            
            this.lbId.Text = Convert.ToString(1);
            this.dtpProductDate.Format = DateTimePickerFormat.Short;
            this.txbWorkTurn.Text = "day";
            this.txbProductCode.Text = "000";
            this.txbWasteWeight.Text = "0";
            this.txbReason.Text = "unknow";
            this.txbRecordMan.Text = "wang";
            this.txbRecheckMan.Text = "li";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //int items;  //why can the items not increase automacally
            //items = this.ltbShow.Items.Count+1;
            items=items+1;
            this.lbId.Update();
            this.lbId.Text = Convert.ToString(items);
            this.lbId.Update();
            this.ltbShow.Items.Add(this.lbId.Text + "\t" + this.dtpProductDate.Value.ToShortDateString() + "\t" + this.txbWorkTurn.Text + "\t" + this.txbProductCode.Text + "\t" + this.txbWasteWeight.Text + "\t" + this.txbReason.Text + "\t" + this.txbRecordMan.Text + "\t" + this.txbRecheckMan.Text);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.ltbShow.Items.Remove(this.ltbShow.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
            check.ShowDialog();
        }

        
    }
}
