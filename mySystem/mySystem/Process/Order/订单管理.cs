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
using System.Collections;

namespace 订单和库存管理
{
    public partial class 订单管理 : mySystem.BaseForm
    {
//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter da;
        SqlCommandBuilder cb;
        DataTable dt;
        BindingSource bs;

        HashSet<String> hs业务类型, hs销售类型, hs客户简称, hs销售部门, hs币种;

        public 订单管理(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            // 连接数据库
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init销售订单();
                    break;
                case 1:
                    init采购需求单();
                    break;
                case 2:
                    break;
            }
            dgv销售订单.CellDoubleClick += new DataGridViewCellEventHandler(dgv销售订单_CellDoubleClick);
            dgv采购需求单.CellDoubleClick += new DataGridViewCellEventHandler(dgv采购需求单_CellDoubleClick);
            dgv采购批准单.CellDoubleClick += new DataGridViewCellEventHandler(dgv采购批准单_CellDoubleClick);
            dgv采购订单.CellDoubleClick += new DataGridViewCellEventHandler(dgv采购订单_CellDoubleClick);
            dgv出库单.CellDoubleClick += new DataGridViewCellEventHandler(dgv出库单_CellDoubleClick);
        }

        void dgv出库单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv出库单["ID", e.RowIndex].Value);
                mySystem.Process.Order.出库单 form = new mySystem.Process.Order.出库单(mainform, id);
                form.Show();
            }
        }

        void dgv采购订单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv采购订单["ID", e.RowIndex].Value);
                mySystem.Process.Order.采购订单 form = new mySystem.Process.Order.采购订单(mainform, id);
                form.Show();
            }
        }

        void dgv采购批准单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv采购批准单["ID", e.RowIndex].Value);
                mySystem.Process.Order.采购批准单 form = new mySystem.Process.Order.采购批准单(mainform, id);
                form.Show();
            }
        }

        void dgv采购需求单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string 订单号 = dgv采购需求单["用途", e.RowIndex].Value.ToString();
                mySystem.Process.Order.采购需求单 form = new mySystem.Process.Order.采购需求单(mainform, 订单号);
                form.Show();
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // 销售订单
                    init销售订单();
                    break;
                case 1:
                    init采购需求单();
                    break;
                case 2:
                    init采购批准单();
                    break;
                case 3:
                    init采购订单();
                    break;
                case 4:
                    init出库单();
                    break;
            } 
        }

        #region 销售订单
        void dgv销售订单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv销售订单[0, e.RowIndex].Value);
                mySystem.Process.Order.销售订单 form = new mySystem.Process.Order.销售订单(mainform, id);
                form.Show();
            }
        }

        void init销售订单()
        {
            dtp销售订单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp销售订单结束时间.Value = DateTime.Now;
            cmb销售订单审核状态.SelectedItem = "待审核";
            dt = get销售订单(dtp销售订单开始时间.Value, dtp销售订单结束时间.Value, tb销售订单号.Text, cmb销售订单审核状态.Text);
            dgv销售订单.DataSource = dt;
            dgv销售订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv销售订单_DataBindingComplete);
            dgv销售订单.ReadOnly = true;
            //get销售订单OtherData();
        }

        //private void get销售订单OtherData()
        //{
        //    hs币种 = new HashSet<string>();
        //    hs客户简称 = new HashSet<string>();
        //    hs销售部门 = new HashSet<string>();
        //    hs销售类型 = new HashSet<string>();
        //    hs业务类型 = new HashSet<string>();
        //}

        void dgv销售订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV销售订单格式();
            mySystem.Utility.setDataGridViewAutoSizeMode(dgv销售订单);
        }

        private void setDGV销售订单格式()
        {
            dgv销售订单.AllowUserToAddRows = false;
            dgv销售订单.Columns["ID"].Visible = false;
        }

        DataTable get销售订单(DateTime start, DateTime end, string code, string status)
        {
            string sql = "select * from 销售订单 where 订单日期 between '{0}' and '{1}' and 状态 like '%{2}%' and 订单号 like '%{3}%'";
            
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, status, code), mySystem.Parameter.conn);
            DataTable dt = new DataTable("销售订单");
            da.Fill(dt);
            return dt;
        }

        private void btn查询销售订单_Click(object sender, EventArgs e)
        {
            dt = get销售订单(dtp销售订单开始时间.Value, dtp销售订单结束时间.Value, tb销售订单号.Text, cmb销售订单审核状态.Text);
            dgv销售订单.DataSource = dt;
        }

        private void btn添加销售订单_Click(object sender, EventArgs e)
        {
            mySystem.Process.Order.销售订单 form = new mySystem.Process.Order.销售订单(mainform);
            form.Show();
        }
        #endregion

        #region 采购需求单
        void init采购需求单()
        {
            dtp采购需求单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp采购需求单结束时间.Value = DateTime.Now;
            cmb采购需求单审核状态.SelectedItem = "待审核";
            dt = get采购需求单(dtp采购需求单开始时间.Value, dtp采购需求单结束时间.Value, tb用途.Text, cmb采购需求单审核状态.Text);
            dgv采购需求单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购需求单_DataBindingComplete);
            dgv采购需求单.DataSource = dt;
            dgv采购需求单.ReadOnly = true;

        }

        void dgv采购需求单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV采购需求单格式();
            mySystem.Utility.setDataGridViewAutoSizeMode(dgv采购需求单);
        }

        private void setDGV采购需求单格式()
        {
            dgv采购需求单.AllowUserToAddRows = false;
            dgv采购需求单.Columns["ID"].Visible = false;
        }

        

        private DataTable get采购需求单(DateTime start, DateTime end, string yongtu, string status)
        {
            //string sql = "select * from 采购需求单 where 申请日期 between #{0}# and #{1}# and 状态 like '%{2}%' and 用途 like '%{3}%'";
            string sql = "select * from 采购需求单 where 申请日期 between '{0}' and '{1}'";
            //SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, status, yongtu), mySystem.Parameter.conn);
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end), mySystem.Parameter.conn);
            DataTable dt = new DataTable("采购需求单");
            da.Fill(dt);
            string select = "状态 like '%{0}%' and 用途 like '%{1}%'";
            DataRow[] drs = dt.Select(string.Format(select, status, yongtu));
            if (drs.Length > 0)
                return drs.CopyToDataTable();
            else
            {
                dt.Clear();
                return dt;
            }
            //return dt;
        }
        private void btn查询采购需求单_Click(object sender, EventArgs e)
        {
            dt = get采购需求单(dtp采购需求单开始时间.Value, dtp采购需求单结束时间.Value, tb用途.Text, cmb采购需求单审核状态.Text);
            dgv采购需求单.DataSource = dt;
        }

        private void btn添加采购需求单_Click(object sender, EventArgs e)
        {
            // 获取所有审核完成的订单
            SqlDataAdapter da = new SqlDataAdapter("select * from 销售订单 where 状态='审核完成'", mySystem.Parameter.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                String ids = mySystem.Other.InputDataGridView.getIDs("", dt, false);
                // 把id变成订单号
                string 订单号 = dt.Select("ID=" + ids)[0]["订单号"].ToString();
                mySystem.Process.Order.采购需求单 form = new mySystem.Process.Order.采购需求单(mainform, 订单号);
                form.Show();
            }
            catch (Exception ee)
            {
            }
            
        }

        #endregion


        #region 采购批准单
        void init采购批准单()
        {
            dtp采购批准单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp采购批准单结束时间.Value = DateTime.Now;
            cmb采购批准单审核状态.SelectedItem = "待审核";
            dt = get采购批准单(dtp采购批准单开始时间.Value, dtp采购批准单结束时间.Value, /*tb用途.Text,*/ cmb采购批准单审核状态.Text);
            dgv采购批准单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购批准单_DataBindingComplete);
            dgv采购批准单.DataSource = dt;
            dgv采购批准单.ReadOnly = true;

        }

        private DataTable get采购批准单(DateTime start, DateTime end, string status)
        {
            string sql = "select * from 采购批准单 where 申请日期 between '{0}' and '{1}' and 状态 like '%{2}%'";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, status), mySystem.Parameter.conn);
            DataTable dt = new DataTable("采购批准单");
            da.Fill(dt);
            return dt;
        }

        void dgv采购批准单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDGV采购批准单格式();
            mySystem.Utility.setDataGridViewAutoSizeMode(dgv采购批准单);
            
        }

        private void setDGV采购批准单格式()
        {
            dgv采购批准单.AllowUserToAddRows = false;
            dgv采购批准单.Columns["ID"].Visible = false;
            
        }

        private void btn采购批准单查询_Click(object sender, EventArgs e)
        {
            dt = get采购批准单(dtp采购批准单开始时间.Value, dtp采购批准单结束时间.Value,  cmb采购批准单审核状态.Text);
            dgv采购批准单.DataSource = dt;
        }

        private void btn采购批准单添加_Click(object sender, EventArgs e)
        {
            DataTable dt未批准需求单详细信息;
            Hashtable ht未批准需求单号2详细信息条数, ht未批准需求单号ID2详细信息,ht未批准需求单号2ID;
            dt未批准需求单详细信息 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from 采购需求单详细信息 where 批准状态='未批准'", mySystem.Parameter.conn);
            da.Fill(dt未批准需求单详细信息);

            ht未批准需求单号ID2详细信息 = new Hashtable();
            ht未批准需求单号2详细信息条数 = new Hashtable();
            ht未批准需求单号2ID = new Hashtable();
            foreach (DataRow dr in dt未批准需求单详细信息.Rows)
            {
                int id = Convert.ToInt32(dr["采购需求单ID"]);
                da = new SqlDataAdapter("select * from 采购需求单 where ID=" + id, mySystem.Parameter.conn);
                DataTable tmp=new DataTable();
                da.Fill(tmp);
                if (tmp.Rows.Count == 0)
                {
                    MessageBox.Show("采购需求单数据错误，请检查");
                    return;
                }
                String code = tmp.Rows[0]["采购申请单号"].ToString();
                if (!ht未批准需求单号2详细信息条数.ContainsKey(code))
                {
                    ht未批准需求单号2详细信息条数[code] = 0;
                    
                }
                if (!ht未批准需求单号ID2详细信息.ContainsKey(dr["采购需求单ID"]))
                {
                    ht未批准需求单号ID2详细信息[dr["采购需求单ID"]] = new List<Object>();
                }
                if (!ht未批准需求单号2ID.ContainsKey(code))
                {
                    ht未批准需求单号2ID[code] = dr["采购需求单ID"];
                }
                ht未批准需求单号2详细信息条数[code] = Convert.ToInt32(ht未批准需求单号2详细信息条数[code]) + 1;
                ((List<Object>)ht未批准需求单号ID2详细信息[dr["采购需求单ID"]]).Add(dr["存货代码"].ToString() + "," + dr["存货名称"].ToString() + "," + dr["规格型号"].ToString());
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns.Add("采购申请单号", Type.GetType("System.String"));
            dt.Columns.Add("未批准项目数量", Type.GetType("System.Int32"));
            foreach (string code in ht未批准需求单号2详细信息条数.Keys.OfType<string>().ToArray<string>())
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = ht未批准需求单号2ID[code];
                dr["采购申请单号"] = code;
                dr["未批准项目数量"] = ht未批准需求单号2详细信息条数[code];
                dt.Rows.Add(dr);
            }
            try
            {
                string ids = mySystem.Other.InputDataGridView.getIDs("", dt, true, ht未批准需求单号ID2详细信息);
                if (ids == "") return;
                mySystem.Process.Order.采购批准单 form = new mySystem.Process.Order.采购批准单(mainform, ids);
                form.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
            }

        }
        #endregion

        #region 采购订单
        void init采购订单()
        {
            dtp采购订单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp采购订单结束时间.Value = DateTime.Now;
            cmb采购订单审核状态.SelectedItem = "待审核";
            tb采购合同号.Text = "";
            dt = get采购订单(dtp采购订单开始时间.Value, dtp采购订单结束时间.Value, tb采购合同号.Text, cmb采购订单审核状态.Text);
            dgv采购订单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv采购订单_DataBindingComplete);
            dgv采购订单.DataSource = dt;
            dgv采购订单.ReadOnly = true;
        }

        private DataTable get采购订单(DateTime start, DateTime end, string status, string 采购合同号)
        {
            string sql = "select * from 采购订单 where 申请日期 between '{0}' and '{1}' and 状态 like '%{2}%' and 采购合同号 like '%{3}%'";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, status, 采购合同号), mySystem.Parameter.conn);
            DataTable dt = new DataTable("采购批准单");
            da.Fill(dt);
            return dt;
        }

        void dgv采购订单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv采购订单.AllowUserToAddRows = false;
            dgv采购订单.Columns["ID"].Visible = false;
            mySystem.Utility.setDataGridViewAutoSizeMode(dgv采购订单);
        }

        private void btn采购订单添加_Click(object sender, EventArgs e)
        {
            // 获取有未采购信息的供应商名字，和要采购的数量

            
            DataTable dt未采购批准单详细信息, dt未采购借用单详细信息;
            Hashtable ht供应商2详细信息条数, ht供应商2详细信息;
            dt未采购批准单详细信息 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from 采购批准单详细信息 where 状态='未采购'", mySystem.Parameter.conn);
            da.Fill(dt未采购批准单详细信息);

            ht供应商2详细信息 = new Hashtable();
            ht供应商2详细信息条数 = new Hashtable();
            foreach (DataRow dr in dt未采购批准单详细信息.Rows)
            {
                int id = Convert.ToInt32(dr["采购批准单ID"]);
                da = new SqlDataAdapter("select * from 采购批准单 where ID=" + id, mySystem.Parameter.conn);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                if (tmp.Rows.Count == 0)
                {
                    MessageBox.Show("采购批准单数据错误，请检查");
                    return;
                }
                string 供应商 = dr["推荐供应商"].ToString();
                if (!ht供应商2详细信息条数.ContainsKey(供应商))
                {
                    ht供应商2详细信息条数[供应商] = 0;

                }
                if (!ht供应商2详细信息.ContainsKey(供应商))
                {
                    ht供应商2详细信息[供应商] = new HashSet<Object>();
                }
                ht供应商2详细信息条数[供应商] = Convert.ToInt32(ht供应商2详细信息条数[供应商]) + 1;
                ((HashSet<Object>)ht供应商2详细信息[供应商]).Add(dr["存货代码"].ToString() + "," + dr["存货名称"].ToString() + "," + dr["规格型号"].ToString()+","+dr["用途"].ToString());
            }


            dt未采购借用单详细信息 = new DataTable();
            da = new SqlDataAdapter("select * from 采购批准单借用订单详细信息 where 状态='未采购'", mySystem.Parameter.conn);
            da.Fill(dt未采购借用单详细信息);
            foreach (DataRow dr in dt未采购借用单详细信息.Rows)
            {
                int id = Convert.ToInt32(dr["采购批准单ID"]);
                da = new SqlDataAdapter("select * from 采购批准单 where ID=" + id, mySystem.Parameter.conn);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                if (tmp.Rows.Count == 0)
                {
                    MessageBox.Show("采购批准单数据错误，请检查");
                    return;
                }
                string 供应商 = dr["推荐供应商"].ToString();
                if (!ht供应商2详细信息条数.ContainsKey(供应商))
                {
                    ht供应商2详细信息条数[供应商] = 0;

                }
                if (!ht供应商2详细信息.ContainsKey(供应商))
                {
                    ht供应商2详细信息[供应商] = new HashSet<Object>();
                }
                ht供应商2详细信息条数[供应商] = Convert.ToInt32(ht供应商2详细信息条数[供应商]) + 1;
                ((HashSet<Object>)ht供应商2详细信息[供应商]).Add(dr["存货代码"].ToString() + "," + dr["存货名称"].ToString() + "," + dr["规格型号"].ToString() + "," + dr["用途"].ToString());
            }



            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.String"));
            dt.Columns.Add("供应商", Type.GetType("System.String"));
            dt.Columns.Add("采购项目数量", Type.GetType("System.Int32"));
            int cnt = 0;
            Hashtable ht临时供应商ID2详细信息 = new Hashtable();
            foreach (String gys in ht供应商2详细信息条数.Keys.OfType<String>().ToArray<String>())
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = ++cnt;
                dr["供应商"] = gys;
                dr["采购项目数量"] = ht供应商2详细信息条数[gys];
                ht临时供应商ID2详细信息[cnt] = ht供应商2详细信息[gys];
                dt.Rows.Add(dr);
            }
            try
            {
                string ids = mySystem.Other.InputDataGridView.getIDs("", dt, false, ht临时供应商ID2详细信息);
                int iii;
                if (ids == "" || !Int32.TryParse(ids, out iii)) return;
                // 从id里获取供应商信息
                string gys = dt.Select("ID=" + iii)[0]["供应商"].ToString();
                mySystem.Process.Order.采购订单 form = new mySystem.Process.Order.采购订单(mainform, gys);
                form.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message+"\n"+ee.StackTrace);
            }

        }

        private void btn采购订单审核状态_Click(object sender, EventArgs e)
        {
            dt = get采购订单(dtp采购订单开始时间.Value, dtp采购订单结束时间.Value, cmb采购订单审核状态.Text, tb采购合同号.Text);
            dgv采购订单.DataSource = dt;
        }
        #endregion

        #region 出库单
        void init出库单()
        {
            dtp出库单开始时间.Value = DateTime.Now.AddDays(-7).Date;
            dtp出库单结束时间.Value = DateTime.Now;
            cmb出库单审核状态.SelectedItem = "待审核";
            tb出库单销售订单.Text = "";
            dt = get采购出库单(dtp出库单开始时间.Value, dtp出库单结束时间.Value, tb出库单销售订单.Text, cmb出库单审核状态.Text);
            dgv出库单.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv出库单_DataBindingComplete);
            dgv出库单.DataSource = dt;
            dgv出库单.ReadOnly = true;
        }

        void dgv出库单_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv出库单.AllowUserToAddRows = false;
            dgv出库单.Columns["ID"].Visible = false;
            mySystem.Utility.setDataGridViewAutoSizeMode(dgv出库单);
        }

        private DataTable get采购出库单(DateTime start, DateTime end, string 销售订单号, string statue)
        {
            string sql = "select * from 出库单 where 出库日期 between '{0}' and '{1}' and 销售订单号 like '%{2}%' and 状态 like '%{3}%'";
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, start, end, 销售订单号, statue), mySystem.Parameter.conn);
            DataTable dt = new DataTable("出库单");
            da.Fill(dt);
            return dt;
        }


        private void btn出库单添加_Click(object sender, EventArgs e)
        {

            mySystem.Process.Order.出库单 form = new mySystem.Process.Order.出库单(mainform);
            form.Show();
        }

        private void btn出库单查询_Click(object sender, EventArgs e)
        {
            dt = get采购出库单(dtp出库单开始时间.Value, dtp出库单结束时间.Value, tb出库单销售订单.Text, cmb出库单审核状态.Text);
            dgv出库单.DataSource = dt;
        }
        #endregion
    }
}
