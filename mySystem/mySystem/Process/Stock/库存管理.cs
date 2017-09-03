using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 订单和库存管理
{
    public partial class 库存管理 : mySystem.BaseForm
    {
        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 库存管理(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();

            conn = new OleDbConnection(strConnect);
            conn.Open();
            // 绑定控件
            readFromDatabase();
            bindControl();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["物资验收记录详细信息ID"].Visible = false;
            
        }


        private void readFromDatabase()
        {
            da = new OleDbDataAdapter("select * from 库存台帐", conn);
            cb = new OleDbCommandBuilder(da);
            dt = new DataTable("库存台帐");
            bs = new BindingSource();
            da.Fill(dt);
        }

        private void bindControl()
        {
            bs.DataSource = dt;
            dataGridView1.DataSource = bs.DataSource;
        }

        private void btn入库_Click(object sender, EventArgs e)
        {
            //入库 form = new 入库();
            //form.Show();
        }

        private void btn出库_Click(object sender, EventArgs e)
        {
            //出库 form = new 出库();
            //form.Show();
        }

        private void btn退货_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.退货管理 form = new mySystem.Process.Stock.退货管理(mainform);
            form.Show();
        }

        private void btn原料入库_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.原料入库管理 form = new mySystem.Process.Stock.原料入库管理(mainform);
            form.ShowDialog();
        }

        private void btn取样记录_Click(object sender, EventArgs e)
        {
            //mySystem.Process.Stock.取样记录 form = new mySystem.Process.Stock.取样记录();
            //form.Show();
        }

        private void btn取样证_Click(object sender, EventArgs e)
        {
            //mySystem.Process.Stock.取样证 form = new mySystem.Process.Stock.取样证();
            //form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btn检验台账_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.检验台账 form = new mySystem.Process.Stock.检验台账();
            form.Show();
        }

        private void btn文件上传_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.文件上传 form = new mySystem.Process.Stock.文件上传();
            form.Show();
        }

        private void btn读取_Click(object sender, EventArgs e)
        {
            readFromDatabase();
            bindControl();
        }


    }
}
