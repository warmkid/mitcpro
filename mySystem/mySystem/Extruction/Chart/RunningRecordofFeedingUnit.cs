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
        private DataTable datatab = new DataTable();
        public int id = 0;
        public RunningRecordofFeedingUnit()
        {
            InitializeComponent();

            //this part add items to the cmblist
            this.cmbEngine.Items.Add("是");
            this.cmbEngine.Items.Add("原因一");
            this.cmbEngine.Items.Add("原因二");
            this.cmbValve.Items.Add("是");
            this.cmbValve.Items.Add("原因一");
            this.cmbValve.Items.Add("原因二");
            this.cmbMaterial.Items.Add("是");
            this.cmbMaterial.Items.Add("原因一");
            this.cmbMaterial.Items.Add("原因二");
            this.cmbAlert.Items.Add("否");
            this.cmbAlert.Items.Add("是");
            this.cmbSolve.Items.Add("是");
            this.cmbSolve.Items.Add("否");

            datatab.Columns.Add("序号", typeof(String));
            datatab.Columns.Add("生产日期", typeof(String));
            datatab.Columns.Add("班次", typeof(String));
            datatab.Columns.Add("检查时间", typeof(String));
            datatab.Columns.Add("电机工作正常", typeof(String));
            datatab.Columns.Add("气动阀工作正常", typeof(String));
            datatab.Columns.Add("供料运行正常", typeof(String));
            datatab.Columns.Add("有报警显示", typeof(String));
            datatab.Columns.Add("解除报警", typeof(String));
            this.dataGridView1.DataSource = datatab;

        }

        private void brnDefault_Click(object sender, EventArgs e)
        {
            this.cmbEngine.Text = "是";
            this.cmbValve.Text = "是";
            this.cmbMaterial.Text = "是";
            this.cmbAlert.Text = "否";
            this.cmbSolve.Text = "否";
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
            id++;

            datatab.Rows.Add(id.ToString(), dtpDate.Value.ToShortDateString(), ckbDy.Checked == true ? "白班" : "夜班", dtpHour.Value.ToShortTimeString(), cmbEngine.Text, cmbValve.Text, cmbMaterial.Text, cmbAlert.Text, cmbSolve.Text);
            this.dataGridView1.DataSource = datatab;
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

        /*private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
			check.LoginButton.Text = "审核通过";
			check.ExitButton.Text = "取消";
            check.ShowDialog();
        }*/

       

    }
}
