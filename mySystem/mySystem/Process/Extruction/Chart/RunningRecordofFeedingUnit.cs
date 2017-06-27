using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;



//this form is about the 7th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class RunningRecordofFeedingUnit : Form
    {

        private DataTable datatab = new DataTable();
        public int id = 0;
        SqlConnection conn = null;
        public RunningRecordofFeedingUnit(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;

            //this part add items to the cmblist
            this.cmbEngine.Items.Add("是");
            this.cmbValve.Items.Add("是");
            this.cmbMaterial.Items.Add("是");
            this.cmbAlert.Items.Add("否");
            this.cmbSolve.Items.Add("否");
            this.Default();


            datatab.Columns.Add("序号", typeof(String));
            datatab.Columns.Add("生产日期", typeof(String));
            datatab.Columns.Add("白班/夜班", typeof(String));
            datatab.Columns.Add("检查时间", typeof(String));
            datatab.Columns.Add("电机工作正常", typeof(String));
            datatab.Columns.Add("气动阀工作正常", typeof(String));
            datatab.Columns.Add("供料运行正常", typeof(String));
            datatab.Columns.Add("有报警显示", typeof(String));
            datatab.Columns.Add("解除报警", typeof(String));
            this.dataGridView1.DataSource = datatab;

        }

        private void Default()
        {
            this.cmbEngine.Text = "是";
            this.cmbEngine.Enabled = false;
            this.cmbEngine.Visible = false;
            this.lbEngine.Visible = false;
            this.cmbValve.Text = "是";
            this.cmbValve.Enabled = false;
            this.cmbValve.Visible = false;
            this.lbValve.Visible = false;
            this.cmbMaterial.Text = "是";
            this.cmbMaterial.Enabled = false;
            this.cmbMaterial.Visible = false;
            this.lbMaterial.Visible = false;
            this.cmbAlert.Text = "否";
            this.cmbAlert.Enabled = false;
            this.cmbAlert.Visible = false;
            this.lbAlert.Visible = false;
            this.cmbSolve.Text = "否";
            this.cmbSolve.Enabled = false;
            this.cmbSolve.Visible = false;
            this.lbSolve.Visible = false;
            this.ckbDy.Checked = true;
            //this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Format = DateTimePickerFormat.Short;
            //this.dtpDate.ShowUpDown = true;
            this.dtpDate.Value = DateTime.Today;
            //this.dtpHour.CustomFormat = "HH:mm:ss";
            this.dtpHour.Format = DateTimePickerFormat.Time;
            //this.dtpHour.ShowUpDown = true;
            this.dtpHour.Value = DateTime.Now;            
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
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            //LoginForm check = new LoginForm(conn);
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            //check.ShowDialog();
            
        }
        
       

    }
}
