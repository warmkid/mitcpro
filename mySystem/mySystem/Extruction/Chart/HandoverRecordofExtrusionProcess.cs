using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


//this form is about the 13th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class HandoverRecordofExtrusionProcess : Form
    {
        public HandoverRecordofExtrusionProcess()
        {
            InitializeComponent();
            
            //this part to add the confirm items
            this.ltbConformItem.Items.Add("Id\tContent");
            this.ltbConformItem.Items.Add("1\titem1");
            this.ltbConformItem.Items.Add("2\titem2");
            this.ltbConformItem.Items.Add("3\titem3");
            this.ltbConformItem.Items.Add("4\titem4");
            this.ltbConformItem.Items.Add("5\titem5");
            this.ltbConformItem.Items.Add("6\titem6");
            this.ltbConformItem.Items.Add("7\titem7");
            this.ltbConformItem.Items.Add("8\titem8");
            this.ltbConformItem.Items.Add("9\titem9");
            this.ltbConformItem.Items.Add("10\titem10");
            this.ltbConformItem.Items.Add("11\titem11");
            this.ltbConformItem.Items.Add("12\titem12");
            this.ltbConformItem.Items.Add("13\titem13");
            this.ltbConformItem.Items.Add("14\titem14");
            
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            //this part automatically fill the blanks
            this.rdbYes1d.Checked = true;
            this.rdbYes1n.Checked = true;
            this.rdbYes2d.Checked = true;
            this.rdbYes2n.Checked = true;
            this.rdbYes3d.Checked = true;
            this.rdbYes3n.Checked = true;
            this.rdbYes4d.Checked = true;
            this.rdbYes4n.Checked = true;
            this.rdbYes5d.Checked = true;
            this.rdbYes5n.Checked = true;
            this.rdbYes6d.Checked = true;
            this.rdbYes6n.Checked = true;
            this.rdbYes7d.Checked = true;
            this.rdbYes7n.Checked = true;
            this.rdbYes8d.Checked = true;
            this.rdbYes8n.Checked = true;
            this.rdbYes9d.Checked = true;
            this.rdbYes9n.Checked = true;
            this.rdbYes10d.Checked = true;
            this.rdbYes10n.Checked = true;
            this.rdbYes11d.Checked = true;
            this.rdbYes11n.Checked = true;
            this.rdbYes12d.Checked = true;
            this.rdbYes12n.Checked = true;
            this.rdbYes13d.Checked = true;
            this.rdbYes13n.Checked = true;
            this.rdbYes14d.Checked = true;
            this.rdbYes14n.Checked = true;
            this.txbBatchNoD.Text = "111";
            this.txbBatchNoN.Text = "112";
            this.txbAmountsD.Text = "100";
            this.txbAmountsN.Text = "100";
            this.txbCodeD.Text = "10000";
            this.txbCodeN.Text = "10000";
            this.txbAbnormalD.Text = "no";
            this.txbAbnormalN.Text = "no";
            this.txbHandinD.Text = "wang";
            this.txbHandinN.Text = "zhang";
            this.txbTakeinD.Text = "sun";
            this.txbTakeinN.Text = "li";
            this.dtpDay.Format = DateTimePickerFormat.Time;
            this.dtpDay.Value = DateTime.Now;
            this.dtpNight.Format = DateTimePickerFormat.Time;
            this.dtpNight.Value = DateTime.Now;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int[,] status = new int[2, 14] { { Convert.ToInt16(rdbYes1d.Checked), Convert.ToInt16(rdbYes2d.Checked), Convert.ToInt16(rdbYes3d.Checked), Convert.ToInt16(rdbYes4d.Checked), Convert.ToInt16(rdbYes5d.Checked), Convert.ToInt16(rdbYes6d.Checked), Convert.ToInt16(rdbYes7d.Checked), Convert.ToInt16(rdbYes8d.Checked), Convert.ToInt16(rdbYes9d.Checked), Convert.ToInt16(rdbYes10d.Checked), Convert.ToInt16(rdbYes11d.Checked), Convert.ToInt16(rdbYes12d.Checked), Convert.ToInt16(rdbYes13d.Checked), Convert.ToInt16(rdbYes14d.Checked) }, { Convert.ToInt16(rdbYes1n.Checked), Convert.ToInt16(rdbYes2n.Checked), Convert.ToInt16(rdbYes3n.Checked), Convert.ToInt16(rdbYes4n.Checked), Convert.ToInt16(rdbYes5n.Checked), Convert.ToInt16(rdbYes6n.Checked), Convert.ToInt16(rdbYes7n.Checked), Convert.ToInt16(rdbYes8n.Checked), Convert.ToInt16(rdbYes9n.Checked), Convert.ToInt16(rdbYes10n.Checked), Convert.ToInt16(rdbYes11n.Checked), Convert.ToInt16(rdbYes12n.Checked), Convert.ToInt16(rdbYes13n.Checked), Convert.ToInt16(rdbYes14n.Checked) } };
            MessageBox.Show("first 7 stutus\t"+status[0,0]+status[0,1]+status[0,2]+status[0,3]+status[0,4]+status[0,5]+status[0,6]);
        }

    }
}
