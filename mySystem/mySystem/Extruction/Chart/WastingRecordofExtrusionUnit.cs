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
        public int items = 1;
        private DataTable datatab = new DataTable();
        public WastingRecordofExtrusionUnit()
        {
            InitializeComponent();
            this.Default();
            //int items =1;
            //items = this.ltbShow.Items.Count+1;
            //items = 1;
        }

        private void Default()
        {
            
            this.lbId.Text = Convert.ToString(1);
            this.lbIdtxt.Visible = false;
            this.lbId.Visible = false;
            this.dtpProductDate.Format = DateTimePickerFormat.Short;
            this.txbWorkTurn.Text = "白班";
            this.txbWorkTurn.Visible = false;
            this.lbWorkTurn.Visible = false;
            this.txbProductCode.Text = "000";
            this.txbProductCode.Visible = false;
            this.lbProductCode.Visible = false;
            this.txbWasteWeight.Text = "0";
            this.txbWasteWeight.Visible = false;
            this.lbWasteWeight.Visible = false;
            this.txbReason.Text = "";
            this.txbReason.Visible = false;
            this.lbReason.Visible = false;
            this.txbRecordMan.Text = "";
            this.txbRecordMan.Visible = false;
            this.lbRecordMan.Visible = false;
            this.txbRecheckMan.Text = "";
            this.txbRecheckMan.Visible = false;
            this.lbRecheckMan.Visible = false;
            datatab.Columns.Add("序号", typeof(String));
            datatab.Columns.Add("生产日期", typeof(String));
            datatab.Columns.Add("班次", typeof(String));
            datatab.Columns.Add("生产代码", typeof(String));
            datatab.Columns.Add("废品重量", typeof(String));
            datatab.Columns.Add("原因", typeof(String));
            this.dataGridView1.DataSource = datatab;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.lbId.Text = items.ToString();
            datatab.Rows.Add(lbId.Text, dtpProductDate.Value.ToShortDateString(), txbWorkTurn.Text, txbProductCode.Text, txbWasteWeight.Text, txbReason.Text);
            items++;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //this.ltbShow.Items.Remove(this.ltbShow.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            check.ShowDialog();
        }

        
    }
}
