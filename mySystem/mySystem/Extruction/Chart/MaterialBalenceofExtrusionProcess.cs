using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//this form is about the 12th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class MaterialBalenceofExtrusionProcess : Form
    {
        public MaterialBalenceofExtrusionProcess()
        {
            InitializeComponent();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.txbProductInstruction.Text = "11111";
            this.dtpProductDate.Format = DateTimePickerFormat.Short;
            this.dtpProductDate.Value = DateTime.Now;
            this.txbGoodWeight.Text = "100";
            this.txbWasteWeight.Text = "100";
            this.txbMid.Text = "180";
            this.txbInnerOuter.Text = "20";
            this.txbRate.Text = Convert.ToString((Convert.ToDouble(this.txbGoodWeight.Text)) / (Convert.ToDouble(this.txbInnerOuter.Text) + Convert.ToDouble(this.txbMid.Text)));
            this.txbBalence.Text = Convert.ToString((Convert.ToDouble(this.txbGoodWeight.Text) + Convert.ToDouble(this.txbWasteWeight.Text)) / (Convert.ToDouble(this.txbInnerOuter.Text) + Convert.ToDouble(this.txbMid.Text)));
            this.txbNote.Text = "none";
            this.txbRecordMan.Text = "wang";
            this.txbRecheckMan.Text = "li";
            this.dtpRecord.Format = DateTimePickerFormat.Short;
            this.dtpRecord.Value = DateTime.Now;
            this.dtpRecheck.Format = DateTimePickerFormat.Short;
            this.dtpRecheck.Value = DateTime.Now;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("date has benn collected and further operate is under development ");
        }

    }
}
