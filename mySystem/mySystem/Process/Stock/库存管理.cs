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
using System.Data.SqlClient;

namespace 订单和库存管理
{
    public partial class 库存管理 : mySystem.BaseForm
    {
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter da;
        SqlCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        public 库存管理(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();

            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            // 绑定控件
            readFromDatabase();
            bindControl();

            //dataGridView1.ReadOnly = true;
            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.RowHeadersVisible = false;
            //dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["物资验收记录详细信息ID"].Visible = false;
            
        }


        private void readFromDatabase()
        {
            da = new SqlDataAdapter("select * from 库存台帐", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable("库存台帐");
            bs = new BindingSource();
            da.Fill(dt);
        }

        private void bindControl()
        {
            bs.DataSource = dt;
            dataGridView1.DataSource = bs.DataSource;
            mySystem.Utility.setDataGridViewAutoSizeMode(dataGridView1);
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

        private void btn出库退库单_Click(object sender, EventArgs e)
        {
            mySystem.Process.Stock.出库退库单 form = new mySystem.Process.Stock.出库退库单(mainform);
            form.Show();
        }

        private void btn浏览_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void btn导入_Click(object sender, EventArgs e)
        {
            //import供应商();
            //import存货档案();
            import库存台账();
        }


        void import库存台账()
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            //            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
            //                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
            //            OleDbConnection conn;
            //            conn = new OleDbConnection(strConnect);
            //            conn.Open();

            string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + mySystem.Parameter.sql_user + ";Pwd=" + mySystem.Parameter.sql_pwd;
            SqlConnection conn;
            conn = new SqlConnection(strConnect);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案", conn);
            DataTable dt存货档案 = new DataTable();
            da.Fill(dt存货档案);
            da = new SqlDataAdapter("select * from 库存台帐 where", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 2; ; ++i)
            {
                string 存货代码 = my.Cells[i, 5].Value != null ? Convert.ToString(my.Cells[i, 5].Value) : "";
                string 批号 = my.Cells[i, 6].Value != null ? Convert.ToString(my.Cells[i, 6].Value) : "";
                DataRow[] drs库存 = dt.Select("产品代码='" + 存货代码 + "' and 产品批号='" + 批号 + "'");


                if (drs库存.Length == 0)
                {
                    string 仓库名称 = my.Cells[i, 1].Value != null ? Convert.ToString(my.Cells[i, 1].Value) : "";
                    string 存货名称 = my.Cells[i, 3].Value != null ? Convert.ToString(my.Cells[i, 3].Value) : "";
                    string 规格型号 = my.Cells[i, 4].Value != null ? Convert.ToString(my.Cells[i, 4].Value) : "";

                    if (my.Cells[i, 5].Value == null) break;

                    double 现存数量 = my.Cells[i, 8].Value != null ? Convert.ToDouble(my.Cells[i, 8].Value) : 0;



                    DataRow dr = dt.NewRow();
                    dr["仓库名称"] = 仓库名称;
                    dr["产品名称"] = 存货名称;
                    dr["产品代码"] = 存货代码;
                    dr["产品规格"] = 规格型号;
                    dr["产品批号"] = 批号;
                    dr["现存数量"] = 现存数量;
                    dr["状态"] = "合格";
                    dr["用途"] = "__自由";
                    dr["冻结状态"] = true;
                    dr["供应商名称"] = "无";
                    DataRow[] drs = dt存货档案.Select("存货代码='" + 存货代码 + "'");
                    if (drs.Length > 0)
                    {
                        dr["换算率"] = drs[0]["换算率"];
                        dr["现存件数"] = Convert.ToDouble(dr["现存数量"]) / Convert.ToDouble(drs[0]["换算率"]);
                        dr["主计量单位"] = drs[0]["主计量单位名称"];
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    double 现存数量 = my.Cells[i, 8].Value != null ? Convert.ToDouble(my.Cells[i, 8].Value) : 0;
                    drs库存[0]["现存数量"] = 现存数量;
                    DataRow[] drs = dt存货档案.Select("存货代码='" + 存货代码 + "'");
                    if (drs.Length > 0)
                    {
                        drs库存[0]["换算率"] = drs[0]["换算率"];
                        drs库存[0]["现存件数"] = Convert.ToDouble(drs库存[0]["现存数量"]) / Convert.ToDouble(drs[0]["换算率"]);
                        drs库存[0]["主计量单位"] = drs[0]["主计量单位名称"];
                    }
                }
            }
            da.Update(dt);
            MessageBox.Show("导入库存台账成功");
        }

        void import供应商()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                // 打开一个Excel进程
                Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
                // 利用这个进程打开一个Excel文件
                //System.IO.Directory.GetCurrentDirectory;
                Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
                // 选择一个Sheet，注意Sheet的序号是从1开始的
                Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
                // 设置该进程是否可见
                //oXL.Visible = true;
                // 修改Sheet中某行某列的值
                List<String> ls = new List<string>();
                for (int i = 3; i <= 66; ++i)
                {
                    ls.Add(my.Cells[i, 2].Value);
                }
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection conn;
                conn = new OleDbConnection(strConnect);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置供应商信息 where 0=1", conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (string gys in ls)
                {
                    DataRow dr = dt.NewRow();
                    dr["供应商代码"] = "";
                    dr["供应商名称"] = gys;
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入供应商成功");
            }
            else
            {
                // 打开一个Excel进程
                Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
                // 利用这个进程打开一个Excel文件
                //System.IO.Directory.GetCurrentDirectory;
                Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);
                // 选择一个Sheet，注意Sheet的序号是从1开始的
                Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
                // 设置该进程是否可见
                //oXL.Visible = true;
                // 修改Sheet中某行某列的值
                List<String> ls = new List<string>();
                for (int i = 3; i <= 66; ++i)
                {
                    ls.Add(my.Cells[i, 2].Value);
                }
                string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + mySystem.Parameter.sql_user + ";Pwd=" + mySystem.Parameter.sql_pwd;
                SqlConnection conn;
                conn = new SqlConnection(strConnect);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置供应商信息 where 0=1", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (string gys in ls)
                {
                    DataRow dr = dt.NewRow();
                    dr["供应商代码"] = "";
                    dr["供应商名称"] = gys;
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入供应商成功");
            }

        }

        void import存货档案()
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(textBox1.Text);


            List<String> ls存货名称 = new List<string>();
            List<String> ls存货代码 = new List<string>();
            List<String> ls规格型号 = new List<string>();
            List<String> ls主计量单位 = new List<string>();
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            for (int i = 2; i <= 832; ++i)
            {
                if (ls存货代码.IndexOf(my.Cells[i, 5].Value) >= 0) continue;
                ls存货代码.Add(my.Cells[i, 5].Value);
                ls存货名称.Add(my.Cells[i, 4].Value);
                ls规格型号.Add(my.Cells[i, 6].Value);
                ls主计量单位.Add(my.Cells[i, 10].Value);
            }
            my = wb.Worksheets[2];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            for (int i = 2; i <= 727; ++i)
            {
                if (ls存货代码.IndexOf(my.Cells[i, 5].Value) >= 0) continue;
                ls存货名称.Add(my.Cells[i, 4].Value);
                ls存货代码.Add(my.Cells[i, 5].Value);
                ls规格型号.Add(my.Cells[i, 6].Value);
                ls主计量单位.Add(my.Cells[i, 10].Value);
            }

            if (!mySystem.Parameter.isSqlOk)
            {
                string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                OleDbConnection conn;
                conn = new OleDbConnection(strConnect);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置存货档案 where 0=1", conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < ls主计量单位.Count; ++i)
                {
                    DataRow dr = dt.NewRow();
                    dr["存货名称"] = ls存货名称[i];
                    dr["存货代码"] = ls存货代码[i];
                    dr["规格型号"] = ls规格型号[i];
                    dr["主计量单位名称"] = ls主计量单位[i];
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入存货档案成功");
            }
            else
            {
                string strConnect = "server=" + mySystem.Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + mySystem.Parameter.sql_user + ";Pwd=" + mySystem.Parameter.sql_pwd;
                SqlConnection conn;
                conn = new SqlConnection(strConnect);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案 where 0=1", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < ls主计量单位.Count; ++i)
                {
                    DataRow dr = dt.NewRow();
                    dr["存货名称"] = ls存货名称[i];
                    dr["存货代码"] = ls存货代码[i];
                    dr["规格型号"] = ls规格型号[i];
                    dr["主计量单位名称"] = ls主计量单位[i];
                    dt.Rows.Add(dr);
                }
                da.Update(dt);
                MessageBox.Show("导入存货档案成功");
            }

        }

    }
}
