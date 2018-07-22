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

namespace mySystem.Setting
{
    public partial class 清洁分切设置 : Form
    {
        string[] allpeople;
        public 清洁分切设置()
        {
            InitializeComponent();
            
            Initdgv();
            Bind();
            TabPage tb = tabControl1.TabPages[1];
            tabControl1.TabPages.Remove(tb);
        }

        private SqlDataAdapter da开机;
        private DataTable dt开机;
        private BindingSource bs开机;
        private SqlCommandBuilder cb开机;
        private SqlDataAdapter da清场;
        private DataTable dt清场;
        private BindingSource bs清场;
        private SqlCommandBuilder cb清场;
        private SqlDataAdapter da产品;
        private DataTable dt产品;
        private BindingSource bs产品;
        private SqlCommandBuilder cb产品;
        private SqlDataAdapter da产品编码;
        private DataTable dt产品编码;
        private BindingSource bs产品编码;
        private SqlCommandBuilder cb产品编码;
        private SqlDataAdapter da物料代码;
        private DataTable dt物料代码;
        private BindingSource bs物料代码;
        private SqlCommandBuilder cb物料代码;
        private SqlDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private SqlCommandBuilder cb人员;
        private SqlDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private SqlCommandBuilder cb权限;

        //dgv样式初始化
        private void Initdgv()
        {
            bs开机 = new BindingSource();
            EachInitdgv(dgv开机);

            bs清场 = new BindingSource();
            EachInitdgv(dgv清场);

            bs产品 = new BindingSource();
            EachInitdgv(dgv产品);

            bs产品编码 = new BindingSource();
            EachInitdgv(dgv产品编码);

            bs物料代码 = new BindingSource();
            EachInitdgv(dgv物料代码);

            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);

            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);
        }

        private void Bind()
        {
            //**************************   开机    ***********************************
            dt开机 = new DataTable("设置开机确认项目"); //""中的是表名
            da开机 = new SqlDataAdapter("select * from 设置开机确认项目", mySystem.Parameter.conn);
            cb开机 = new SqlCommandBuilder(da开机);
            dt开机.Columns.Add("序号", System.Type.GetType("System.String"));
            da开机.Fill(dt开机);
            bs开机.DataSource = dt开机;
            this.dgv开机.DataSource = bs开机.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv开机);
            this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv开机);
            this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv开机.Columns["ID"].Visible = false;
           
            //**************************   清场    ***********************************
            dt清场 = new DataTable("设置清场项目"); //""中的是表名
            da清场 = new SqlDataAdapter("select * from 设置清场项目", mySystem.Parameter.conn);
            cb清场 = new SqlCommandBuilder(da清场);
            dt清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da清场.Fill(dt清场);
            bs清场.DataSource = dt清场;
            this.dgv清场.DataSource = bs清场.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv清场);
            //this.dgv清场.Columns["清场项目"].MinimumWidth = 200;
            this.dgv清场.Columns["清场项目"].MinimumWidth = 200;
            this.dgv清场.Columns["清场要点"].MinimumWidth = 250;
            this.dgv清场.Columns["清场要点"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv清场);
            this.dgv清场.Columns["清场要点"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv清场.Columns["ID"].Visible = false;
            

            //************************    产品     *******************************************
            dt产品 = new DataTable("设置清洁分切产品"); //""中的是表名
            da产品 = new SqlDataAdapter("select * from 设置清洁分切产品", mySystem.Parameter.conn);
            cb产品 = new SqlCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品);
            this.dgv产品.Columns["产品名称"].MinimumWidth = 200;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv产品);
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;
            

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置清洁分切产品编码"); //""中的是表名
            da产品编码 = new SqlDataAdapter("select * from 设置清洁分切产品编码", mySystem.Parameter.conn);
            cb产品编码 = new SqlCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 200;
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品编码.Columns["ID"].Visible = false;
            

            //************************    物料代码     *******************************************
            dt物料代码 = new DataTable("设置物料代码"); //""中的是表名
            da物料代码 = new SqlDataAdapter("select * from 设置物料代码", mySystem.Parameter.conn);
            cb物料代码 = new SqlCommandBuilder(da物料代码);
            dt物料代码.Columns.Add("序号", System.Type.GetType("System.String"));
            da物料代码.Fill(dt物料代码);
            bs物料代码.DataSource = dt物料代码;
            this.dgv物料代码.DataSource = bs物料代码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].MinimumWidth = 200;
            this.dgv物料代码.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv物料代码.Columns["ID"].Visible = false;
            

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("用户"); //""中的是表名
            da人员 = new SqlDataAdapter("select * from 用户", mySystem.Parameter.conn);
            cb人员 = new SqlCommandBuilder(da人员);
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
            this.dgv人员.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv人员);

            //************************    人员权限     *******************************************
            dt权限 = new DataTable("用户权限"); //""中的是表名
            da权限 = new SqlDataAdapter("select * from 用户权限", mySystem.Parameter.conn);
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
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv权限);

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

        #region 项目设置
        private void add开机_Click(object sender, EventArgs e)
        {
            DataRow dr = dt开机.NewRow();
            dt开机.Rows.InsertAt(dt开机.NewRow(), dt开机.Rows.Count);
            setDataGridViewRowNums(this.dgv开机);
            //点击“添加”跳转到最后一行
            if (dgv开机.Rows.Count > 0)
                dgv开机.FirstDisplayedScrollingRowIndex = dgv开机.Rows.Count - 1;
        }

        private void del开机_Click(object sender, EventArgs e)
        {
            int idx = this.dgv开机.CurrentRow.Index;
            dt开机.Rows[idx].Delete();
            da开机.Update((DataTable)bs开机.DataSource);
            dt开机.Clear();
            da开机.Fill(dt开机);
            setDataGridViewRowNums(this.dgv开机);
        }

        private void add清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt清场.NewRow();
            dt清场.Rows.InsertAt(dt清场.NewRow(), dt清场.Rows.Count);
            setDataGridViewRowNums(this.dgv清场);
            //点击“添加”跳转到最后一行
            if (dgv清场.Rows.Count > 0)
                dgv清场.FirstDisplayedScrollingRowIndex = dgv清场.Rows.Count - 1;
        }

        private void del清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv清场.CurrentRow.Index;
            dt清场.Rows[idx].Delete();
            da清场.Update((DataTable)bs清场.DataSource);
            dt清场.Clear();
            da清场.Fill(dt清场);
            setDataGridViewRowNums(this.dgv清场);
        }

        private void Btn保存项目_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Parameter.isSqlOk)
                { }
                else
                {
                    da开机.Update((DataTable)bs开机.DataSource);
                    dt开机.Clear();
                    da开机.Fill(dt开机);
                    setDataGridViewRowNums(this.dgv开机);

                    da清场.Update((DataTable)bs清场.DataSource);
                    dt清场.Clear();
                    da清场.Fill(dt清场);
                    setDataGridViewRowNums(this.dgv清场);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        #endregion

        #region 产品设置
        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            setDataGridViewRowNums(this.dgv产品);
            //点击“添加”跳转到最后一行
            if (dgv产品.Rows.Count > 0)
                dgv产品.FirstDisplayedScrollingRowIndex = dgv产品.Rows.Count - 1;
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
            //点击“添加”跳转到最后一行
            if (dgv产品编码.Rows.Count > 0)
                dgv产品编码.FirstDisplayedScrollingRowIndex = dgv产品编码.Rows.Count - 1;
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

        private void add物料代码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt物料代码.NewRow();
            dt物料代码.Rows.InsertAt(dt物料代码.NewRow(), dt物料代码.Rows.Count);
            setDataGridViewRowNums(this.dgv物料代码);
            //点击“添加”跳转到最后一行
            if (dgv物料代码.Rows.Count > 0)
                dgv物料代码.FirstDisplayedScrollingRowIndex = dgv物料代码.Rows.Count - 1;
        }

        private void del物料代码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv物料代码.CurrentRow.Index;
            dt物料代码.Rows[idx].Delete();
            da物料代码.Update((DataTable)bs物料代码.DataSource);
            dt物料代码.Clear();
            da物料代码.Fill(dt物料代码);
            setDataGridViewRowNums(this.dgv物料代码);
        }              

        private void Btn保存产品_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Parameter.isSqlOk)
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

                    da物料代码.Update((DataTable)bs物料代码.DataSource);
                    dt物料代码.Clear();
                    da物料代码.Fill(dt物料代码);
                    setDataGridViewRowNums(this.dgv物料代码);

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
            //点击“添加”跳转到最后一行
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

        //检查人员是否在清洁分切人员中
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
            comm.CommandText = "select * from 用户 where 用户名 = " + "'" + name + "' ";
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            { b = true; }
            else
            {
                b = false;
                MessageBox.Show("员工" + "“" + name + "”" + "无操作清洁分切工序权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
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
                else if (dc.ColumnName == "用户名")
                {
                    allpeople = Utility.getAllPeople();
                    cbc = new DataGridViewComboBoxColumn();
                    cbc.HeaderText = dc.ColumnName;
                    cbc.Name = dc.ColumnName;
                    cbc.ValueType = dc.DataType;
                    cbc.DataPropertyName = dc.ColumnName;
                    cbc.SortMode = DataGridViewColumnSortMode.NotSortable;

                    cbc.Items.AddRange(allpeople);

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

       

    }
}
