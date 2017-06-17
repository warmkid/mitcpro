using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


//this form is about the 12th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class MaterialBalenceofExtrusionProcess : Form
    {
        DataTable dt;
        DataRow[] dr;
        DateTime produce1, produce2, record1, record2, recheck1, recheck2;
        string produceid, recordman, recheckman;

        public MaterialBalenceofExtrusionProcess()
        {
            InitializeComponent();
            
            
            this.sqlconnec();
            //this.dtpProduce.Value = DateTime.MinValue;
            //this.dtpRecord.Value = DateTime.MinValue;
            //this.dtpRecheck.Value = DateTime.MinValue;

            show();
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
            //MessageBox.Show("date has benn collected and further operate is under development ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm check = new LoginForm();
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            check.ShowDialog();
        }

        private void show()
        {
            
            //this part to edit the inquire
            //string para = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //SqlConnection lcon = new SqlConnection(para);
            //lcon.Open();
            //string condi = "12345";
            //string sqlString = "SELECT " + condi + " FROM balance_test";
            //SqlCommand cmd = new SqlCommand(sqlString, lcon);
            //object o = cmd.ExecuteScalar();
            //MessageBox.Show(o.ToString());
            //Console.WriteLine("list:{0}", o);
            //lcon.Close();

            produce1 = dtpProduce.Value.Date;
            produce2 = dtpProduceTo.Value.Date;
            record1 = dtpRecord.Value.Date;
            record2 = dtpRecodTo.Value.Date;
            recheck1 = dtpRecheck.Value.Date;
            recheck2 = dtpRecheckTo.Value.Date;
            produceid = txbProduce2.Text;
            recordman = txbRecordMan.Text;
            recheckman = txbRecheckMan.Text;
            TimeSpan del_pro = produce2 - produce1;
            TimeSpan del_crd = record2 - record1;
            TimeSpan del_rck = recheck2 - recheck1;
            if (del_crd.TotalDays * del_pro.TotalDays * del_rck.TotalDays < 0)
            {
                MessageBox.Show("error");
            }
            string sql = "生产日期>=" + "'" + produce1 + "'" + " and " + "生产日期<=" + "'" + produce2 + "'";
            sql += " and " + "记录日期>=" + "'" + record1 + "'" + " and " + "记录日期<=" + "'" + record2 + "'";
            sql += " and " + "复核日期>=" + "'" + recheck1 + "'" + " and " + "复核日期<=" + "'" + recheck2 + "'";
            if (produceid != "")
                sql += " and " + "生产指令 like" + "'%" + produceid + "%'";
            
            if (recordman != "(空)")
                sql += " and " + "记录人 like" + "'%" + recordman + "%'";
            if (recheckman != "(空)")
                sql += " and " + "复核人 like" + "'%" + recheckman + "%'";

            dr = dt.Select(sql);
            if (dr.Length == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }

            DataTable temp = dr.CopyToDataTable();
            dataGridView1.DataSource = temp;
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this part to edit the inquire
            //string para = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //SqlConnection lcon = new SqlConnection(para);
            //lcon.Open();
            //string condi = "12345";
            //string sqlString = "SELECT " + condi + " FROM balance_test";
            //SqlCommand cmd = new SqlCommand(sqlString, lcon);
            //object o = cmd.ExecuteScalar();
            //MessageBox.Show(o.ToString());
            //Console.WriteLine("list:{0}", o);
            //lcon.Close();

            produce1 = dtpProduce.Value.Date;
            produce2 = dtpProduceTo.Value.Date;
            record1 = dtpRecord.Value.Date;
            record2 = dtpRecodTo.Value.Date;
            recheck1 = dtpRecheck.Value.Date;
            recheck2 = dtpRecheckTo.Value.Date;
            produceid = txbProduce2.Text;
            recordman = txbRecordMan.Text;
            recheckman = txbRecheckMan.Text;
            TimeSpan del_pro = produce2 - produce1;
            TimeSpan del_crd = record2 - record1;
            TimeSpan del_rck = recheck2 - recheck1;
            if (del_crd.TotalDays * del_pro.TotalDays * del_rck.TotalDays < 0)
            {
                MessageBox.Show("error");
            }
            string sql = "生产日期>=" + "'" + produce1 + "'" + " and " + "生产日期<=" + "'" + produce2 + "'";
            sql += " and " + "记录日期>=" + "'" + record1 + "'" + " and " + "记录日期<=" + "'" + record2 + "'";
            sql += " and " + "复核日期>=" + "'" + recheck1 + "'" + " and " + "复核日期<=" + "'" + recheck2 + "'";
            if (produceid != "")
                sql += " and " + "生产指令 like" + "'%" + produceid + "%'";
            
            if (recordman != "(空)")
                sql += " and " + "记录人 like" + "'%" + recordman + "%'";
            if (recheckman != "(空)")
                sql += " and " + "复核人 like" + "'%" + recheckman + "%'";

            dr = dt.Select(sql);
            if (dr.Length == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }

            DataTable temp = dr.CopyToDataTable();
            dataGridView1.DataSource = temp;
        }

        private void sqlconnec()
        {
            try
            {
                string para = @"server=10.105.223.19,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
                SqlConnection lcon = new SqlConnection(para);
                lcon.Open();
                string sql1 = "select * from balance_test";
                SqlCommand cmd = new SqlCommand(sql1, lcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                DataColumn col = new DataColumn("编号");
                dt.Columns.Add(col);
                da.Fill(dt);
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    dt.Rows[row][0] = (row + 1).ToString();
                }

                lcon.Close();
            }

            catch
            {
                MessageBox.Show("failed");

            }
            
        }

       
    }
}
