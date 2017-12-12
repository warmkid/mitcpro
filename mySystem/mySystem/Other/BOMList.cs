using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Other
{
    public partial class BOMList : Form
    {
        JArray _data;
        List<string> ls存货代码, ls存货名称, ls规格型号;
        List<int> lsID;

//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;

        public BOMList()
        {
            //conn = new OleDbConnection(strConnect);
            InitializeComponent();
            getOtherData();
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        public BOMList(JArray ja)
        {
            //conn = new OleDbConnection(strConnect);
            InitializeComponent();
            getOtherData();
            fillFromJA(ja);
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        void fillFromJA(JArray ja)
        {
            foreach (JToken jt in ja)
            {
                int id = Convert.ToInt32(jt["ID"]);
                double 数量 = Convert.ToDouble(jt["数量"]);
                int idx = dataGridView1.Rows.Add();
                int idLoc = lsID.IndexOf(id);
                if (idLoc < 0) continue;
                dataGridView1["存货代码", idx].Value = ls存货代码[idLoc];
                dataGridView1["存货名称", idx].Value = ls存货名称[idLoc];
                dataGridView1["规格型号", idx].Value = ls规格型号[idLoc];
                dataGridView1["数量", idx].Value = 数量;
            }

        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string curStr;
            int idx;
            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "存货代码":
                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Value == null) curStr = "";
                    else curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货代码.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
                case "存货名称":
                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Value == null) curStr = "";
                    else curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货名称.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
                case "规格型号":
                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Value == null) curStr = "";
                    else curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls规格型号.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = ls存货代码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货代码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
            }
        }

        void getOtherData(){
            ls存货代码 = new List<string>();
            ls存货名称 = new List<string>();
            ls规格型号 = new List<string>();
            lsID = new List<Int32>();
            SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案", mySystem.Parameter.conn);
            //sq da = new OleDbDataAdapter("select * from 设置存货档案", mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls存货代码.Add(dr["存货代码"].ToString());
                ls存货名称.Add(dr["存货名称"].ToString());
                ls规格型号.Add(dr["规格型号"].ToString());
                lsID.Add(Convert.ToInt32(dr["ID"]));
            }
        }
         

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (e.Control as TextBox);
            tb.AutoCompleteCustomSource = null;
            AutoCompleteStringCollection acsc;
            if (tb == null) return;
            switch (dataGridView1.CurrentCell.OwningColumn.Name)
            {
                case "存货代码":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls存货代码.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    break;
                case "存货名称":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls存货名称.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    break;
                case "规格型号":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls规格型号.ToArray());
                    tb.AutoCompleteCustomSource = acsc;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    break;
            }
        }

        public static String getData()
        {
            BOMList form = new BOMList();
            if (DialogResult.OK == form.ShowDialog())
            {
                string ret = form._data.ToString().Replace("\r\n", "");
                if (ret == "[]") return "空";
                else return form._data.ToString().Replace("\r\n", "");
            }
            else
            {
                return null;
            }

        }

        public static String getData(JArray ja)
        {
            BOMList form = new BOMList(ja);
            if (DialogResult.OK == form.ShowDialog())
            {
                string ret = form._data.ToString().Replace("\r\n", "");
                if (ret == "[]") return "空";
                else return form._data.ToString().Replace("\r\n", "");
            }
            else
            {
                return null;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            if (dataGridView1.RowCount > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            _data = JArray.Parse("[]");
            //OleDbDataAdapter da;
            SqlDataAdapter da;
            DataTable dt;
            string 存货代码, 存货名称, 规格型号, sql;
            double 数量;
            int id;
            try
            {
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    存货代码 = dgvr.Cells["存货代码"].Value.ToString();
                    存货名称 = dgvr.Cells["存货名称"].Value.ToString();
                    规格型号 = dgvr.Cells["规格型号"].Value.ToString();
                    数量 = Convert.ToDouble(dgvr.Cells["数量"].Value);
                    sql = "select * from 设置存货档案 where 存货代码='{0}'";
                    //da = new OleDbDataAdapter(string.Format(sql, 存货代码, 存货名称, 规格型号), mySystem.Parameter.connOle);
                    da = new SqlDataAdapter(string.Format(sql, 存货代码, 存货名称, 规格型号), mySystem.Parameter.conn);

                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) throw new Exception();
                    id = Convert.ToInt32(dt.Rows[0]["ID"]);
                    JObject jo = JObject.Parse("{}");
                    jo.Add("ID", id);
                    jo.Add("数量", 数量);
                    _data.Add(jo);
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("填写错误，请重新检查");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int ridx = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(ridx);
            }
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
            if (textBox1.Text.Trim() == "") return;


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
//            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//            OleDbConnection conn;
            //conn = new OleDbConnection(strConnect);
            //conn.Open();

            //OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置存货档案", mySystem.Parameter.connOle);
            SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案", mySystem.Parameter.conn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 1; ; i++)
            {
                String daima =  Convert.ToString( my.Cells[i,2].Value);
                
                if (daima == "CODE") break;
                if (daima == null)
                {
                    MessageBox.Show("第" + i + "行产品代码缺失！");
                    break;
                }
                if (daima.Trim() == "") break;
                DataRow[] drs = dt.Select("存货代码='" + daima + "'");
                if (drs.Length == 0)
                {
                    MessageBox.Show(daima + " 不存在！");
                    break;
                }
                string guige="";
                string mingc="";
                double shulaing=0;
                try
                {
                     guige = drs[0]["规格型号"].ToString();
                     mingc = drs[0]["存货名称"].ToString();
                    
                    if (my.Cells[i, 5].Value == null) shulaing = 0;
                    else shulaing = Convert.ToDouble(my.Cells[i, 5].Value);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("读取第"+i+"行数据失败，请检查！");
                }
                
                
                int idx = dataGridView1.Rows.Add();
                dataGridView1["存货代码", idx].Value = daima;
                dataGridView1["存货名称", idx].Value = mingc;
                dataGridView1["规格型号", idx].Value = guige;
                dataGridView1["数量", idx].Value = shulaing;
            }
           
            MessageBox.Show("导入BOM列表成功");

        }


    }
}
