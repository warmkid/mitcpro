using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace mySystem.Setting
{
    public partial class 订单设置 : Form
    {

       
        private SqlDataAdapter da存货档案;
        private DataTable dt存货档案, dt存货档案Show;
        private BindingSource bs存货档案;
        private SqlCommandBuilder cb存货档案;


        private SqlDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private SqlCommandBuilder cb人员;
        private SqlDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private SqlCommandBuilder cb权限;

        private SqlDataAdapter da业务类型;
        private DataTable dt业务类型;
        private BindingSource bs业务类型;
        private SqlCommandBuilder cb业务类型;
        private SqlDataAdapter da销售类型;
        private DataTable dt销售类型;
        private BindingSource bs销售类型;
        private SqlCommandBuilder cb销售类型;
        private SqlDataAdapter da客户简称;
        private DataTable dt客户简称;
        private BindingSource bs客户简称;
        private SqlCommandBuilder cb客户简称;
        private SqlDataAdapter da销售部门;
        private DataTable dt销售部门;
        private BindingSource bs销售部门;
        private SqlCommandBuilder cb销售部门;
        private SqlDataAdapter da币种;
        private DataTable dt币种;
        private BindingSource bs币种;
        private SqlCommandBuilder cb币种;
        private SqlDataAdapter da付款条件;
        private DataTable dt付款条件;
        private BindingSource bs付款条件;
        private SqlCommandBuilder cb付款条件;

        string copied工序 = "", copied类型 = "";

        public 订单设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
            dgv存货档案.CellDoubleClick += new DataGridViewCellEventHandler(dgv存货档案_CellDoubleClick);
            dgv存货档案.CellEndEdit += new DataGridViewCellEventHandler(dgv存货档案_CellEndEdit);
            tb代码q.PreviewKeyDown += new PreviewKeyDownEventHandler(tb代码q_PreviewKeyDown);
            tb名称q.PreviewKeyDown += new PreviewKeyDownEventHandler(tb名称q_PreviewKeyDown);
        }

        void dgv存货档案_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv存货档案.CurrentCell != null)
            {
                if (dgv存货档案.CurrentCell.OwningColumn.Name == "存货代码" && dgv存货档案["ID",dgv存货档案.CurrentCell.RowIndex].Value==DBNull.Value)
                {
                    string currDaima = dgv存货档案.CurrentCell.Value.ToString();
                    SqlDataAdapter da = new SqlDataAdapter("select * from 设置存货档案 where 存货代码='" + currDaima + "'", mySystem.Parameter.conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("存货代码：" + currDaima + " 重复出现，以将其加入剪贴板");
                        Clipboard.SetDataObject(dgv存货档案.CurrentCell.Value.ToString(), false);
                        dgv存货档案.CurrentCell.Value = "";
                    }
                }
            }

        }

        void tb名称q_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询订单设置.PerformClick();
        }

        void tb代码q_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn查询订单设置.PerformClick();
        }

        void dgv存货档案_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgv存货档案.Columns[e.ColumnIndex].Name == "属于工序")
                {
                    string data = mySystem.Other.属于工序.getData();
                    if (data != null)
                    {
                        dgv存货档案[e.ColumnIndex, e.RowIndex].Value = data;
                    }
                }
                if (dgv存货档案.Columns[e.ColumnIndex].Name == "类型")
                {
                    string data = mySystem.Other.存货编码类型.getData();
                    if (data != null)
                    {
                        dgv存货档案[e.ColumnIndex, e.RowIndex].Value = data;
                    }
                }
                if (dgv存货档案.Columns[e.ColumnIndex].Name == "BOM列表")
                {
                    // 弹出对话框，选择组件
                    //MessageBox.Show("dian");
                    //SqlDataAdapter da = new SqlDataAdapter("select ID, 存货编码,存货名称,规格型号 from 设置组件存货档案", mySystem.Parameter.conn);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    try
                    {
                        //String ids = mySystem.Other.InputDataGridView.getIDs(dgv产成品存货档案[e.ColumnIndex, e.RowIndex].Value.ToString(), dt);
                        string d = dgv存货档案[e.ColumnIndex, e.RowIndex].Value.ToString();
                        if (d == "" || d.Trim()=="空")
                        {
                            string data = mySystem.Other.BOMList.getData();
                            if (data != null)
                                dgv存货档案[e.ColumnIndex, e.RowIndex].Value = data;
                        }
                        else
                        {
                            JArray ja = JArray.Parse(d);
                            string data = mySystem.Other.BOMList.getData(ja);
                            if (data != null)
                                dgv存货档案[e.ColumnIndex, e.RowIndex].Value = data;
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
                    }
                }
            }
        }


        private void Initdgv()
        {
            bs存货档案 = new BindingSource();
            EachInitdgv(dgv存货档案);

            

            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);

            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);

            bs业务类型 = new BindingSource();
            EachInitdgv(dgv业务类型);

            bs销售类型 = new BindingSource();
            EachInitdgv(dgv销售类型);

            bs客户简称 = new BindingSource();
            EachInitdgv(dgv客户简称);

            bs销售部门 = new BindingSource();
            EachInitdgv(dgv销售部门);

            bs币种 = new BindingSource();
            EachInitdgv(dgv币种);

            bs付款条件 = new BindingSource();
            EachInitdgv(dgv付款条件);

        }


        private void Bind()
        {
            //**************************   设置存货档案    ***********************************
            dt存货档案 = new DataTable("设置存货档案"); //""中的是表名
            da存货档案 = new SqlDataAdapter("select * from 设置存货档案 where 0=1", mySystem.Parameter.conn);
            cb存货档案 = new SqlCommandBuilder(da存货档案);
            //dt存货档案.Columns.Add("序号", System.Type.GetType("System.String"));
            da存货档案.Fill(dt存货档案);
            //dt存货档案Show = topN(dt存货档案,20);
            bs存货档案.DataSource = dt存货档案;
            this.dgv存货档案.DataSource = bs存货档案.DataSource;
            //显示序号
            //setDataGridViewRowNums(this.dgv存货档案);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv存货档案);
            this.dgv存货档案.Columns["ID"].Visible = false;
            this.dgv存货档案.Columns["属于工序"].ReadOnly = true;
            this.dgv存货档案.Columns["BOM列表"].ReadOnly = true;
            this.dgv存货档案.Columns["类型"].ReadOnly = true;
            //Utility.setDataGridViewAutoSizeMode(dgv存货档案);
            setDGV存货档案Column();
            dgv存货档案.RowHeadersVisible = true;
            foreach (DataGridViewColumn dgvc in dgv存货档案.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
            }



            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("订单用户"); //""中的是表名
            da人员 = new SqlDataAdapter("select * from 订单用户", mySystem.Parameter.conn);
            cb人员 = new SqlCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            bs人员.DataSource = dt人员;
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            //dgv人员下拉框设置();
            setDataGridViewRowNums(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            //this.dgv人员.Columns["班次"].MinimumWidth = 100;
            Utility.setDataGridViewAutoSizeMode(dgv人员);
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv人员.Columns["ID"].Visible = false;

            //************************    人员权限     *******************************************
            dt权限 = new DataTable("订单用户权限"); //""中的是表名
            da权限 = new SqlDataAdapter("select * from 订单用户权限", mySystem.Parameter.conn);
            cb权限 = new SqlCommandBuilder(da权限);
            dt权限.Columns.Add("序号", System.Type.GetType("System.String"));
            da权限.Fill(dt权限);
            bs权限.DataSource = dt权限;
            this.dgv权限.DataSource = bs权限.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv权限);
            this.dgv权限.Columns["步骤"].MinimumWidth = 250;
            this.dgv权限.Columns["操作员"].MinimumWidth = 150;
            this.dgv权限.Columns["审核员"].MinimumWidth = 150;
            this.dgv权限.Columns["步骤"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv权限);
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;


            //**************************   业务类型    ***********************************
            dt业务类型 = new DataTable("设置业务类型"); //""中的是表名
            da业务类型 = new SqlDataAdapter("select * from 设置业务类型", mySystem.Parameter.conn);
            cb业务类型 = new SqlCommandBuilder(da业务类型);
            dt业务类型.Columns.Add("序号", System.Type.GetType("System.String"));
            da业务类型.Fill(dt业务类型);
            bs业务类型.DataSource = dt业务类型;
            this.dgv业务类型.DataSource = bs业务类型.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv业务类型);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv业务类型);
            this.dgv业务类型.Columns["ID"].Visible = false;


            //**************************   销售类型    ***********************************
            dt销售类型 = new DataTable("设置销售类型"); //""中的是表名
            da销售类型 = new SqlDataAdapter("select * from 设置销售类型", mySystem.Parameter.conn);
            cb销售类型 = new SqlCommandBuilder(da销售类型);
            dt销售类型.Columns.Add("序号", System.Type.GetType("System.String"));
            da销售类型.Fill(dt销售类型);
            bs销售类型.DataSource = dt销售类型;
            this.dgv销售类型.DataSource = bs销售类型.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv销售类型);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv销售类型);
            this.dgv销售类型.Columns["ID"].Visible = false;


            //**************************   客户简称    ***********************************
            dt客户简称 = new DataTable("设置客户简称"); //""中的是表名
            da客户简称 = new SqlDataAdapter("select * from 设置客户简称", mySystem.Parameter.conn);
            cb客户简称 = new SqlCommandBuilder(da客户简称);
            dt客户简称.Columns.Add("序号", System.Type.GetType("System.String"));
            da客户简称.Fill(dt客户简称);
            bs客户简称.DataSource = dt客户简称;
            this.dgv客户简称.DataSource = bs客户简称.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv客户简称);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv客户简称);
            this.dgv客户简称.Columns["ID"].Visible = false;


            //**************************   销售部门    ***********************************
            dt销售部门 = new DataTable("设置销售部门"); //""中的是表名
            da销售部门 = new SqlDataAdapter("select * from 设置销售部门", mySystem.Parameter.conn);
            cb销售部门 = new SqlCommandBuilder(da销售部门);
            dt销售部门.Columns.Add("序号", System.Type.GetType("System.String"));
            da销售部门.Fill(dt销售部门);
            bs销售部门.DataSource = dt销售部门;
            this.dgv销售部门.DataSource = bs销售部门.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv销售部门);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv销售部门);
            this.dgv销售部门.Columns["ID"].Visible = false;


            //**************************   币种    ***********************************
            dt币种 = new DataTable("设置币种"); //""中的是表名
            da币种 = new SqlDataAdapter("select * from 设置币种", mySystem.Parameter.conn);
            cb币种 = new SqlCommandBuilder(da币种);
            dt币种.Columns.Add("序号", System.Type.GetType("System.String"));
            da币种.Fill(dt币种);
            bs币种.DataSource = dt币种;
            this.dgv币种.DataSource = bs币种.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv币种);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv币种);
            this.dgv币种.Columns["ID"].Visible = false;


            //**************************   付款条件    ***********************************
            dt付款条件 = new DataTable("设置付款条件"); //""中的是表名
            da付款条件 = new SqlDataAdapter("select * from 设置付款条件", mySystem.Parameter.conn);
            cb付款条件 = new SqlCommandBuilder(da付款条件);
            dt付款条件.Columns.Add("序号", System.Type.GetType("System.String"));
            da付款条件.Fill(dt付款条件);
            bs付款条件.DataSource = dt付款条件;
            this.dgv付款条件.DataSource = bs付款条件.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv付款条件);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv付款条件);
            this.dgv付款条件.Columns["ID"].Visible = false;

        }

        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        private void EachInitdgv(DataGridView dgv)
        {
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);
        }

        private void add存货档案_Click(object sender, EventArgs e)
        {
            //DataRow dr = dt存货档案.NewRow();
            int idx = dt存货档案.Rows.Count;
            dt存货档案.Rows.InsertAt(dt存货档案.NewRow(), dt存货档案.Rows.Count);
            dt存货档案.Rows[idx]["存货代码"] = "";
            dt存货档案.Rows[idx]["存货名称"] = "";
            //setDataGridViewRowNums(this.dgv存货档案);
            if (dgv存货档案.Rows.Count > 0)
                dgv存货档案.FirstDisplayedScrollingRowIndex = dgv存货档案.Rows.Count - 1;
            refresh序号();
        }

        private void del存货档案_Click(object sender, EventArgs e)
        {
            int idx = this.dgv存货档案.CurrentRow.Index;
            dt存货档案.Rows[idx].Delete();
            da存货档案.Update((DataTable)bs存货档案.DataSource);
            dt存货档案.Clear();
            da存货档案.Fill(dt存货档案);
            //setDataGridViewRowNums(this.dgv存货档案);
            refresh序号();
        }

        

        private void Btn保存组件设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Parameter.isSqlOk)
                { }
                else
                {
                    //// 检查是否有重复的
                    //if (isDaimaDup)
                    //{
                    //    MessageBox.Show("存货代码有重复，请检查");
                    //    return;
                    //}

                    // 检查是否有空行
                    
                    foreach (DataRow dr in dt存货档案.Rows)
                    {
                        bool isEmpty = true;
                        object[] items = dr.ItemArray;
                        int l = items.Length;
                        for (int i = 1; i < l; ++i)
                        {
                            string item = items[i].ToString().Trim();
                            if (!(item.Equals("") || item == null))
                            {
                                isEmpty = false;
                                break;
                            }
                        }
                        if (isEmpty)
                        {
                            MessageBox.Show("请勿保存空白数据！");
                            return;
                        }
                    }


                    da存货档案.Update((DataTable)bs存货档案.DataSource);
                    dt存货档案.Clear();
                    da存货档案.Fill(dt存货档案);
                    //setDataGridViewRowNums(this.dgv存货档案);

                    

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void Btn保存人员_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Parameter.isSqlOk)
                { }
                else
                {
                    Boolean b = checkPeopleExist(); //判断用户是否在用户大表中
                    if (b)
                    {
                        da人员.Update((DataTable)bs人员.DataSource);
                        dt人员.Clear();
                        da人员.Fill(dt人员);
                        setDataGridViewRowNums(this.dgv人员);

                        Boolean c = checkPeopleRight(); //判断用户是否在库存管理用户表中
                        if (c)
                        {
                            da权限.Update((DataTable)bs权限.DataSource);
                            dt权限.Clear();
                            da权限.Fill(dt权限);
                            setDataGridViewRowNums(this.dgv权限);

                            MessageBox.Show("保存成功！");
                        }
                    }
                }

            }
            catch (Exception ee)
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void add人员_Click(object sender, EventArgs e)
        {
            DataRow dr = dt人员.NewRow();
            dt人员.Rows.InsertAt(dt人员.NewRow(), dt人员.Rows.Count);
            setDataGridViewRowNums(this.dgv人员);
            if (dgv人员.Rows.Count > 0)
                dgv人员.FirstDisplayedScrollingRowIndex = dgv人员.Rows.Count - 1;
        }

        private void del人员_Click(object sender, EventArgs e)
        {
            int idx = this.dgv人员.SelectedCells[0].RowIndex;
            dt人员.Rows[idx].Delete();
            da人员.Update((DataTable)bs人员.DataSource);
            dt人员.Clear();
            da人员.Fill(dt人员);
            setDataGridViewRowNums(this.dgv人员);
        }

        private Boolean checkPeopleExist()
        {
            Boolean b = true;
            string strCon = "server=" + Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;

            foreach (DataRow dr in dt人员.Rows)
            {
                //判断行状态
                DataRowState state = dr.RowState;
                if (state.Equals(DataRowState.Deleted))
                { continue; }
                else
                {
                    String name = dr["用户名"].ToString();
                    comm.CommandText = "select * from [users] where 姓名 = " + "'" + name + "'";
                    SqlDataReader reader = comm.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        b = false;
                        MessageBox.Show("员工" + "“" + name + "”" + "不存在！");
                    }
                    reader.Dispose();
                }
            }

            comm.Dispose();
            conn.Dispose();
            return b;
        }


        private Boolean checkPeopleRight()
        {
            Boolean b;
            String[] list操作员;
            String[] list审核员;
            foreach (DataRow dr in dt权限.Rows)
            {
                list操作员 = dr["操作员"].ToString().Split(new char[] { '，', ',' });
                foreach (String name in list操作员)
                {
                    Boolean eachbool = EachPeopleRightCheck(name);
                    if (!eachbool)
                    { return b = false; }
                }
                list审核员 = dr["审核员"].ToString().Split(new char[] { '，', ',' });
                foreach (String name in list审核员)
                {
                    Boolean eachbool = EachPeopleRightCheck(name);
                    if (!eachbool)
                    { return b = false; }
                }
            }

            return b = true;
        }


        private Boolean EachPeopleRightCheck(String name)
        {
            Boolean b;
            SqlCommand comm = new SqlCommand();
            comm.Connection = mySystem.Parameter.conn;
            comm.CommandText = "select * from 订单用户 where 用户名 = " + "'" + name + "' ";
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            { b = true; }
            else
            {
                b = false;
                MessageBox.Show("员工" + "“" + name + "”" + "无订单管理权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
        }

        private void add业务类型_Click(object sender, EventArgs e)
        {
            DataRow dr = dt业务类型.NewRow();
            dt业务类型.Rows.InsertAt(dt业务类型.NewRow(), dt业务类型.Rows.Count);
            setDataGridViewRowNums(this.dgv业务类型);
            if (dgv业务类型.Rows.Count > 0)
                dgv业务类型.FirstDisplayedScrollingRowIndex = dgv业务类型.Rows.Count - 1;
        }

        private void add销售类型_Click(object sender, EventArgs e)
        {
            DataRow dr = dt销售类型.NewRow();
            dt销售类型.Rows.InsertAt(dt销售类型.NewRow(), dt销售类型.Rows.Count);
            setDataGridViewRowNums(this.dgv销售类型);
            if (dgv销售类型.Rows.Count > 0)
                dgv销售类型.FirstDisplayedScrollingRowIndex = dgv销售类型.Rows.Count - 1;
        }

        private void add客户简称_Click(object sender, EventArgs e)
        {
            DataRow dr = dt客户简称.NewRow();
            dt客户简称.Rows.InsertAt(dt客户简称.NewRow(), dt客户简称.Rows.Count);
            setDataGridViewRowNums(this.dgv客户简称);
            if (dgv客户简称.Rows.Count > 0)
                dgv客户简称.FirstDisplayedScrollingRowIndex = dgv客户简称.Rows.Count - 1;
        }

        private void add销售部门_Click(object sender, EventArgs e)
        {
            DataRow dr = dt销售部门.NewRow();
            dt销售部门.Rows.InsertAt(dt销售部门.NewRow(), dt销售部门.Rows.Count);
            setDataGridViewRowNums(this.dgv销售部门);
            if (dgv销售部门.Rows.Count > 0)
                dgv销售部门.FirstDisplayedScrollingRowIndex = dgv销售部门.Rows.Count - 1;
        }

        private void add币种_Click(object sender, EventArgs e)
        {
            DataRow dr = dt币种.NewRow();
            dt币种.Rows.InsertAt(dt币种.NewRow(), dt币种.Rows.Count);
            setDataGridViewRowNums(this.dgv币种);
            if (dgv币种.Rows.Count > 0)
                dgv币种.FirstDisplayedScrollingRowIndex = dgv币种.Rows.Count - 1;
        }

        private void add付款条件_Click(object sender, EventArgs e)
        {
            DataRow dr = dt付款条件.NewRow();
            dt付款条件.Rows.InsertAt(dt付款条件.NewRow(), dt付款条件.Rows.Count);
            setDataGridViewRowNums(this.dgv付款条件);
            if (dgv付款条件.Rows.Count > 0)
                dgv付款条件.FirstDisplayedScrollingRowIndex = dgv付款条件.Rows.Count - 1;
        }

        private void del业务类型_Click(object sender, EventArgs e)
        {
            int idx = this.dgv业务类型.SelectedCells[0].RowIndex;
            dt业务类型.Rows[idx].Delete();
            da业务类型.Update((DataTable)bs业务类型.DataSource);
            dt业务类型.Clear();
            da业务类型.Fill(dt业务类型);
            setDataGridViewRowNums(this.dgv业务类型);
        }

        private void del销售类型_Click(object sender, EventArgs e)
        {
            int idx = this.dgv销售类型.SelectedCells[0].RowIndex;
            dt销售类型.Rows[idx].Delete();
            da销售类型.Update((DataTable)bs销售类型.DataSource);
            dt销售类型.Clear();
            da销售类型.Fill(dt销售类型);
            setDataGridViewRowNums(this.dgv销售类型);
        }

        private void del客户简称_Click(object sender, EventArgs e)
        {
            int idx = this.dgv客户简称.SelectedCells[0].RowIndex;
            dt客户简称.Rows[idx].Delete();
            da客户简称.Update((DataTable)bs客户简称.DataSource);
            dt客户简称.Clear();
            da客户简称.Fill(dt客户简称);
            setDataGridViewRowNums(this.dgv客户简称);
        }

        private void del销售部门_Click(object sender, EventArgs e)
        {
            int idx = this.dgv销售部门.SelectedCells[0].RowIndex;
            dt销售部门.Rows[idx].Delete();
            da销售部门.Update((DataTable)bs销售部门.DataSource);
            dt销售部门.Clear();
            da销售部门.Fill(dt销售部门);
            setDataGridViewRowNums(this.dgv销售部门);
        }

        private void del币种_Click(object sender, EventArgs e)
        {
            int idx = this.dgv币种.SelectedCells[0].RowIndex;
            dt币种.Rows[idx].Delete();
            da币种.Update((DataTable)bs币种.DataSource);
            dt币种.Clear();
            da币种.Fill(dt币种);
            setDataGridViewRowNums(this.dgv币种);
        }

       

        private void del付款条件_Click(object sender, EventArgs e)
        {
            int idx = this.dgv付款条件.SelectedCells[0].RowIndex;
            dt付款条件.Rows[idx].Delete();
            da付款条件.Update((DataTable)bs付款条件.DataSource);
            dt付款条件.Clear();
            da付款条件.Fill(dt付款条件);
            setDataGridViewRowNums(this.dgv付款条件);
        }

        private void save基本信息设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Parameter.isSqlOk)
                { }
                else
                {   
                    da业务类型.Update((DataTable)bs业务类型.DataSource);
                    dt业务类型.Clear();
                    da业务类型.Fill(dt业务类型);
                    setDataGridViewRowNums(this.dgv业务类型);

                    da销售类型.Update((DataTable)bs销售类型.DataSource);
                    dt销售类型.Clear();
                    da销售类型.Fill(dt销售类型);
                    setDataGridViewRowNums(this.dgv销售类型);

                    da客户简称.Update((DataTable)bs客户简称.DataSource);
                    dt客户简称.Clear();
                    da客户简称.Fill(dt客户简称);
                    setDataGridViewRowNums(this.dgv客户简称);


                    da销售部门.Update((DataTable)bs销售部门.DataSource);
                    dt销售部门.Clear();
                    da销售部门.Fill(dt销售部门);
                    setDataGridViewRowNums(this.dgv销售部门);

                    da币种.Update((DataTable)bs币种.DataSource);
                    dt币种.Clear();
                    da币种.Fill(dt币种);
                    setDataGridViewRowNums(this.dgv币种);

                    da付款条件.Update((DataTable)bs付款条件.DataSource);
                    dt付款条件.Clear();
                    da付款条件.Fill(dt付款条件);
                    setDataGridViewRowNums(this.dgv付款条件);
                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void btn查询订单设置_Click(object sender, EventArgs e)
        {
            //DataRow[] drs = dt存货档案.Select("存货代码 like'%"+tb代码q.Text+"%' and 存货名称 like '%"+tb名称q.Text+"%'");
            //dt存货档案Show = dt存货档案.Clone();
            //foreach (DataRow dr in drs)
            //{
            //    dt存货档案Show.ImportRow(dr);
            //}
            //bs存货档案.DataSource = dt存货档案Show;
            //dgv存货档案.DataSource = bs存货档案.DataSource;
            //Utility.setDataGridViewAutoSizeMode(dgv存货档案);
            dgv存货档案.Columns.Clear();
            string sql = "select * from 设置存货档案 where  存货代码 like '%{0}%' and 存货名称 like '%{1}%'";
            dt存货档案 = new DataTable("设置存货档案"); //""中的是表名
            da存货档案 = new SqlDataAdapter(string.Format(sql, tb代码q.Text, tb名称q.Text), mySystem.Parameter.conn);
            cb存货档案 = new SqlCommandBuilder(da存货档案);
            //dt存货档案.Columns.Add("序号", System.Type.GetType("System.String"));
            da存货档案.Fill(dt存货档案);
            //dt存货档案Show = topN(dt存货档案,20);
            bs存货档案.DataSource = dt存货档案;
            this.dgv存货档案.DataSource = bs存货档案.DataSource;
            //显示序号
            //setDataGridViewRowNums(this.dgv存货档案);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv存货档案);
            this.dgv存货档案.Columns["ID"].Visible = false;
            this.dgv存货档案.Columns["属于工序"].ReadOnly = true;
            this.dgv存货档案.Columns["BOM列表"].ReadOnly = true;
            this.dgv存货档案.Columns["类型"].ReadOnly = true;
            //Utility.setDataGridViewAutoSizeMode(dgv存货档案);
            setDGV存货档案Column();
            dgv存货档案.RowHeadersVisible = true;
            foreach (DataGridViewColumn dgvc in dgv存货档案.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
            }
           
            dgv存货档案.Columns["存货代码"].Frozen = true;
        }


        DataTable topN(DataTable dt, int N)
        {
            if (dt.Rows.Count < N) return dt;
            DataTable ret = dt.Clone();
            for (int i = 0; i < N; ++i)
            {
                ret.ImportRow(dt.Rows[i]);
            }
            return ret;
        }

        void setDGV存货档案Column()
        {
            //dgv存货档案.Columns["存货代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgv存货档案.Columns["存货名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgv存货档案.Columns["规格型号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dgv存货档案.Columns["BOM列表"].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            //dgv存货档案.Columns["存货代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv存货档案.Columns["存货名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv存货档案.Columns["规格型号"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv存货档案.Columns["BOM列表"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //dgv存货档案.Columns["存货代码"].Width = 100;
            dgv存货档案.Columns["存货名称"].Width = 300;
            dgv存货档案.Columns["规格型号"].Width = 300;
            dgv存货档案.Columns["BOM列表"].Width = 100;
        }

        private void btn复制类型_Click(object sender, EventArgs e)
        {
            if (dgv存货档案.SelectedCells.Count == 0)
            {
                return;
            }
            if (dgv存货档案.SelectedCells.Count > 1)
            {
                MessageBox.Show("复制时请勿选择多行");
                return;
            }
            copied类型 = dgv存货档案.SelectedCells[0].OwningRow.Cells["类型"].Value.ToString();
        }

        private void btn复制工序_Click(object sender, EventArgs e)
        {
            if (dgv存货档案.SelectedCells.Count == 0)
            {
                return;
            }
            if (dgv存货档案.SelectedCells.Count > 1)
            {
                MessageBox.Show("复制时请勿选择多行");
                return;
            }
            copied工序 = dgv存货档案.SelectedCells[0].OwningRow.Cells["属于工序"].Value.ToString();
        }

        private void btn粘贴类型_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell dgvc in dgv存货档案.SelectedCells)
            {
                dgvc.OwningRow.Cells["类型"].Value = copied类型;
            }
        }

        private void btn粘贴工序_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell dgvc in dgv存货档案.SelectedCells)
            {
                dgvc.OwningRow.Cells["属于工序"].Value = copied工序;
            }
            
        }

        private void dgv存货档案_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = String.Format("{0}", e.Row.Index + 1);
        }

        void refresh序号()
        {
            for (int i = 0; i < dgv存货档案.Rows.Count; ++i)
            {
                dgv存货档案.Rows[i].HeaderCell.Value = String.Format("{0}", i + 1);
            }
        }

        private void tabControl1_Leave(object sender, EventArgs e)
        {
            //if (dgv存货档案.ColumnCount > 0)
            //{
            //    writeDGVWidthToSetting(dgv存货档案);
            //}
        }

       

       

       
    }
}
