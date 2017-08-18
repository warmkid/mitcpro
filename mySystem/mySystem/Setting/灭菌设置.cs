using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Setting
{
    public partial class 灭菌设置 : Form
    {
        public 灭菌设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
        }

        private OleDbDataAdapter da产品;
        private DataTable dt产品;
        private BindingSource bs产品;
        private OleDbCommandBuilder cb产品;
        private OleDbDataAdapter da产品编码;
        private DataTable dt产品编码;
        private BindingSource bs产品编码;
        private OleDbCommandBuilder cb产品编码;
        private OleDbDataAdapter da产品规格;
        private DataTable dt产品规格;
        private BindingSource bs产品规格;
        private OleDbCommandBuilder cb产品规格;
        private OleDbDataAdapter da运输商;
        private DataTable dt运输商;
        private BindingSource bs运输商;
        private OleDbCommandBuilder cb运输商;
        private OleDbDataAdapter da辐照单位;
        private DataTable dt辐照单位;
        private BindingSource bs辐照单位;
        private OleDbCommandBuilder cb辐照单位;
        private OleDbDataAdapter da产品名称;
        private DataTable dt产品名称;
        private BindingSource bs产品名称;
        private OleDbCommandBuilder cb产品名称;
        private OleDbDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private OleDbCommandBuilder cb人员;
        private OleDbDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private OleDbCommandBuilder cb权限;

        //dgv样式初始化
        private void Initdgv()
        {           
            bs产品 = new BindingSource();
            EachInitdgv(dgv产品);

            bs产品编码 = new BindingSource();
            EachInitdgv(dgv产品编码);            

            bs产品规格 = new BindingSource();
            EachInitdgv(dgv产品规格);

            bs运输商 = new BindingSource();
            EachInitdgv(dgv运输商);

            bs辐照单位 = new BindingSource();
            EachInitdgv(dgv辐照单位);

            bs产品名称 = new BindingSource();
            EachInitdgv(dgv产品名称);

            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);

            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);
        }

        private void Bind()
        {
            //************************    运输商     *******************************************
            dt运输商 = new DataTable("设置运输商"); //""中的是表名
            da运输商 = new OleDbDataAdapter("select * from 设置运输商", mySystem.Parameter.connOle);
            cb运输商 = new OleDbCommandBuilder(da运输商);
            dt运输商.Columns.Add("序号", System.Type.GetType("System.String"));
            da运输商.Fill(dt运输商);
            bs运输商.DataSource = dt运输商;
            this.dgv运输商.DataSource = bs运输商.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv运输商);
            this.dgv运输商.Columns["运输商"].MinimumWidth = 200;
            this.dgv运输商.Columns["运输商"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv运输商.Columns["运输商"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //************************    产品     *******************************************
            dt产品 = new DataTable("设置灭菌产品"); //""中的是表名
            da产品 = new OleDbDataAdapter("select * from 设置灭菌产品", mySystem.Parameter.connOle);
            cb产品 = new OleDbCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品);
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置灭菌产品代码"); //""中的是表名
            da产品编码 = new OleDbDataAdapter("select * from 设置灭菌产品代码", mySystem.Parameter.connOle);
            cb产品编码 = new OleDbCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //************************    产品规格     *******************************************
            dt产品规格 = new DataTable("设置灭菌产品规格"); //""中的是表名
            da产品规格 = new OleDbDataAdapter("select * from 设置灭菌产品规格", mySystem.Parameter.connOle);
            cb产品规格 = new OleDbCommandBuilder(da产品规格);
            dt产品规格.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品规格.Fill(dt产品规格);
            bs产品规格.DataSource = dt产品规格;
            this.dgv产品规格.DataSource = bs产品规格.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品规格);
            this.dgv产品规格.Columns["产品规格"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品规格.Columns["产品规格"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //************************    辐照单位     *******************************************
            dt辐照单位 = new DataTable("设置辐照单位"); //""中的是表名
            da辐照单位 = new OleDbDataAdapter("select * from 设置辐照单位", mySystem.Parameter.connOle);
            cb辐照单位 = new OleDbCommandBuilder(da辐照单位);
            dt辐照单位.Columns.Add("序号", System.Type.GetType("System.String"));
            da辐照单位.Fill(dt辐照单位);
            bs辐照单位.DataSource = dt辐照单位;
            this.dgv辐照单位.DataSource = bs辐照单位.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv辐照单位);
            this.dgv辐照单位.Columns["辐照单位"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv辐照单位.Columns["辐照单位"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //************************   产品名称     *******************************************
            dt产品名称 = new DataTable("设置产品名称"); //""中的是表名
            da产品名称 = new OleDbDataAdapter("select * from 设置产品名称", mySystem.Parameter.connOle);
            cb产品名称 = new OleDbCommandBuilder(da产品名称);
            dt产品名称.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品名称.Fill(dt产品名称);
            bs产品名称.DataSource = dt产品名称;
            this.dgv产品名称.DataSource = bs产品名称.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品名称);
            this.dgv产品名称.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品名称.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("用户"); //""中的是表名
            da人员 = new OleDbDataAdapter("select * from 用户", mySystem.Parameter.connOle);
            cb人员 = new OleDbCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            bs人员.DataSource = dt人员;
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            dgv人员下拉框设置();
            setDataGridViewRowNums(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            this.dgv人员.Columns["班次"].MinimumWidth = 100;
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            //************************    人员权限     *******************************************
            dt权限 = new DataTable("用户权限"); //""中的是表名
            da权限 = new OleDbDataAdapter("select * from 用户权限", mySystem.Parameter.connOle);
            cb权限 = new OleDbCommandBuilder(da权限);
            dt权限.Columns.Add("序号", System.Type.GetType("System.String"));
            da权限.Fill(dt权限);
            bs权限.DataSource = dt权限;
            this.dgv权限.DataSource = bs权限.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv权限);
            this.dgv权限.Columns["操作员"].MinimumWidth = 150;
            this.dgv权限.Columns["审核员"].MinimumWidth = 150;
            this.dgv权限.Columns["步骤"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
       
        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        #region 产品设置
        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            setDataGridViewRowNums(this.dgv产品);
        }

        private void del产品_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品.CurrentRow.Index;
            dt产品.Rows[idx].Delete();
            da产品.Update((DataTable)bs产品.DataSource);
            dt产品.Clear();
            da产品.Fill(dt产品);
            setDataGridViewRowNums(this.dgv产品);
        }

        private void add产品编码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品编码.NewRow();
            dt产品编码.Rows.InsertAt(dt产品编码.NewRow(), dt产品编码.Rows.Count);
            setDataGridViewRowNums(this.dgv产品编码);
        }

        private void del产品编码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品编码.CurrentRow.Index;
            dt产品编码.Rows[idx].Delete();
            da产品编码.Update((DataTable)bs产品编码.DataSource);
            dt产品编码.Clear();
            da产品编码.Fill(dt产品编码);
            setDataGridViewRowNums(this.dgv产品编码);
        }

        private void add规格_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品规格.NewRow();
            dt产品规格.Rows.InsertAt(dt产品规格.NewRow(), dt产品规格.Rows.Count);
            setDataGridViewRowNums(this.dgv产品规格);
        }

        private void del规格_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品规格.CurrentRow.Index;
            dt产品规格.Rows[idx].Delete();
            da产品规格.Update((DataTable)bs产品规格.DataSource);
            dt产品规格.Clear();
            da产品规格.Fill(dt产品规格);
            setDataGridViewRowNums(this.dgv产品规格);
        }

        private void add运输商_Click(object sender, EventArgs e)
        {
            DataRow dr = dt运输商.NewRow();
            dt运输商.Rows.InsertAt(dt运输商.NewRow(), dt运输商.Rows.Count);
            setDataGridViewRowNums(this.dgv运输商);
        }

        private void del运输商_Click(object sender, EventArgs e)
        {
            int idx = this.dgv运输商.CurrentRow.Index;
            dt运输商.Rows[idx].Delete();
            da运输商.Update((DataTable)bs运输商.DataSource);
            dt运输商.Clear();
            da运输商.Fill(dt运输商);
            setDataGridViewRowNums(this.dgv运输商);
        }

        private void add辐照单位_Click(object sender, EventArgs e)
        {
            DataRow dr = dt辐照单位.NewRow();
            dt辐照单位.Rows.InsertAt(dt辐照单位.NewRow(), dt辐照单位.Rows.Count);
            setDataGridViewRowNums(this.dgv辐照单位);
        }

        private void del辐照单位_Click(object sender, EventArgs e)
        {
            int idx = this.dgv辐照单位.CurrentRow.Index;
            dt辐照单位.Rows[idx].Delete();
            da辐照单位.Update((DataTable)bs辐照单位.DataSource);
            dt辐照单位.Clear();
            da辐照单位.Fill(dt辐照单位);
            setDataGridViewRowNums(this.dgv辐照单位);
        }

        private void Btn保存产品_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da产品.Update((DataTable)bs产品.DataSource);
                    dt产品.Clear();
                    da产品.Fill(dt产品);
                    setDataGridViewRowNums(this.dgv产品);

                    da产品编码.Update((DataTable)bs产品编码.DataSource);
                    dt产品编码.Clear();
                    da产品编码.Fill(dt产品编码);
                    setDataGridViewRowNums(this.dgv产品编码);

                    da产品规格.Update((DataTable)bs产品规格.DataSource);
                    dt产品规格.Clear();
                    da产品规格.Fill(dt产品规格);
                    setDataGridViewRowNums(this.dgv产品规格);

                    da运输商.Update((DataTable)bs运输商.DataSource);
                    dt运输商.Clear();
                    da运输商.Fill(dt运输商);
                    setDataGridViewRowNums(this.dgv运输商);

                    da辐照单位.Update((DataTable)bs辐照单位.DataSource);
                    dt辐照单位.Clear();
                    da辐照单位.Fill(dt辐照单位);
                    setDataGridViewRowNums(this.dgv辐照单位);

                    da产品名称.Update((DataTable)bs产品名称.DataSource);
                    dt产品名称.Clear();
                    da产品名称.Fill(dt产品名称);
                    setDataGridViewRowNums(this.dgv产品名称);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        #endregion

        #region 人员设置
        private void add人员_Click(object sender, EventArgs e)
        {
            DataRow dr = dt人员.NewRow();
            dt人员.Rows.InsertAt(dt人员.NewRow(), dt人员.Rows.Count);
            setDataGridViewRowNums(this.dgv人员);
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

        private void Btn保存人员_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
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

                        Boolean c = checkPeopleRight(); //判断用户是否在清洁分切用户表中
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
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        //检查添加的人员是否在总的用户表中
        private Boolean checkPeopleExist()
        {
            Boolean b = true;
            string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
            OleDbConnection conn = new OleDbConnection(strCon);
            conn.Open();
            OleDbCommand comm = new OleDbCommand();
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
                    OleDbDataReader reader = comm.ExecuteReader();
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

        //检查人员是否在灭菌人员中
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
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            comm.CommandText = "select * from 用户 where 用户名 = " + "'" + name + "' ";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            { b = true; }
            else
            {
                b = false;
                MessageBox.Show("员工" + "“" + name + "”" + "无操作灭菌工序权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
        }

        #endregion


        #region DataBindingComplete事件
        private void dgv产品_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品.Columns["ID"].Visible = false;
        }

        private void dgv产品编码_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品编码.Columns["ID"].Visible = false;
        }

        private void dgv产品规格_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品规格.Columns["ID"].Visible = false;
        }

        private void dgv运输商_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv运输商.Columns["ID"].Visible = false;
        }

        private void dgv辐照单位_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv辐照单位.Columns["ID"].Visible = false;
        }

        private void dgv人员_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv人员.Columns["ID"].Visible = false;
        }

        private void dgv权限_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv权限.Columns["ID"].Visible = false;
        }

        #endregion




        void dgv人员下拉框设置()
        {
            dgv人员.Columns.Clear();
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            DataGridViewCheckBoxColumn ckbc;

            // 先把所有的列都加好，基本属性附上
            foreach (DataColumn dc in dt人员.Columns)
            {
                // 要下拉框的特殊处理
                if (dc.ColumnName == "班次")
                {
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;

                    cbc.Items.Add("白班");
                    cbc.Items.Add("夜班");

                    dgv人员.Columns.Add(cbc);
                    continue;
                }

                // 根据数据类型自动生成列的关键信息
                switch (dc.DataType.ToString())
                {

                    case "System.Int32":
                    case "System.String":
                    case "System.Double":
                    case "System.DateTime":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgv人员.Columns.Add(tbc);
                        break;
                    case "System.Boolean":
                        ckbc = new DataGridViewCheckBoxColumn();
                        ckbc.HeaderText = dc.ColumnName;
                        ckbc.Name = dc.ColumnName;
                        ckbc.ValueType = dc.DataType;
                        ckbc.DataPropertyName = dc.ColumnName;
                        ckbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgv人员.Columns.Add(ckbc);
                        break;
                }
            }
        }

        private void dgv产品名称_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品名称.Columns["ID"].Visible = false;
        }

        private void add产品名称_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品名称.NewRow();
            dt产品名称.Rows.InsertAt(dt产品名称.NewRow(), dt产品名称.Rows.Count);
            setDataGridViewRowNums(this.dgv产品名称);
        }

        private void del产品名称_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品名称.CurrentRow.Index;
            dt产品名称.Rows[idx].Delete();
            da产品名称.Update((DataTable)bs产品名称.DataSource);
            dt产品名称.Clear();
            da产品名称.Fill(dt产品名称);
            setDataGridViewRowNums(this.dgv产品名称);
        }

        

    }
}
