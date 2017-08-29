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

namespace mySystem.Other
{
    public partial class BOMList : Form
    {
        JArray _data;
        List<string> ls存货编码, ls存货名称, ls规格型号;
        List<int> lsID;

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;

        public BOMList()
        {
            conn = new OleDbConnection(strConnect);
            InitializeComponent();
            getOtherData();
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        public BOMList(JArray ja)
        {
            conn = new OleDbConnection(strConnect);
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
                dataGridView1["存货编码", idx].Value = ls存货编码[idLoc];
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
                case "存货编码":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货编码.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = ls存货编码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
                case "存货名称":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls存货名称.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = ls存货编码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
                case "规格型号":
                    curStr = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                    idx = ls规格型号.IndexOf(curStr);
                    if (idx >= 0)
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = ls存货编码[idx];
                        dataGridView1["存货名称", e.RowIndex].Value = ls存货名称[idx];
                        dataGridView1["规格型号", e.RowIndex].Value = ls规格型号[idx];
                    }
                    else
                    {
                        dataGridView1["存货编码", e.RowIndex].Value = "";
                        dataGridView1["存货名称", e.RowIndex].Value = "";
                        dataGridView1["规格型号", e.RowIndex].Value = "";
                    }
                    break;
            }
        }

        void getOtherData(){
            ls存货编码 = new List<string>();
            ls存货名称 = new List<string>();
            ls规格型号 = new List<string>();
            lsID = new List<Int32>();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 设置组件存货档案", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ls存货编码.Add(dr["存货编码"].ToString());
                ls存货名称.Add(dr["存货名称"].ToString());
                ls规格型号.Add(dr["规格型号"].ToString());
                lsID.Add(Convert.ToInt32(dr["ID"]));
            }
        }
         

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (e.Control as TextBox);
            AutoCompleteStringCollection acsc;
            if (tb == null) return;
            switch (dataGridView1.CurrentCell.OwningColumn.Name)
            {
                case "存货编码":
                    acsc = new AutoCompleteStringCollection();
                    acsc.AddRange(ls存货编码.ToArray());
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
                return form._data.ToString().Replace("\r\n","");
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
                return form._data.ToString().Replace("\r\n", "");
            }
            else
            {
                return null;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            _data = JArray.Parse("[]");
            OleDbDataAdapter da;
            DataTable dt;
            string 存货编码, 存货名称, 规格型号, sql;
            double 数量;
            int id;
            try
            {
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    存货编码 = dgvr.Cells["存货编码"].Value.ToString();
                    存货名称 = dgvr.Cells["存货名称"].Value.ToString();
                    规格型号 = dgvr.Cells["规格型号"].Value.ToString();
                    数量 = Convert.ToDouble(dgvr.Cells["数量"].Value);
                    sql = "select * from 设置组件存货档案 where 存货编码='{0}' and 存货名称='{1}' and 规格型号='{2}'";
                    da = new OleDbDataAdapter(string.Format(sql, 存货编码, 存货名称, 规格型号), conn);
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
            int ridx = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(ridx);
        }
        
    }
}
