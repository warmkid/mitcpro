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
    public partial class 清洁分切设置 : Form
    {
        public 清洁分切设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
        }

        private OleDbDataAdapter da开机;
        private DataTable dt开机;
        private BindingSource bs开机;
        private OleDbCommandBuilder cb开机;
        private OleDbDataAdapter da清场;
        private DataTable dt清场;
        private BindingSource bs清场;
        private OleDbCommandBuilder cb清场;
        private OleDbDataAdapter da产品;
        private DataTable dt产品;
        private BindingSource bs产品;
        private OleDbCommandBuilder cb产品;
        private OleDbDataAdapter da产品编码;
        private DataTable dt产品编码;
        private BindingSource bs产品编码;
        private OleDbCommandBuilder cb产品编码;
        private OleDbDataAdapter da物料代码;
        private DataTable dt物料代码;
        private BindingSource bs物料代码;
        private OleDbCommandBuilder cb物料代码;
        private OleDbDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private OleDbCommandBuilder cb人员;
        private OleDbDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private OleDbCommandBuilder cb权限;
        private DataTable dt开机初始;
        private DataTable dt清场初始;
        private DataTable dt产品初始;
        private DataTable dt产品编码初始;
        private DataTable dt物料代码初始;
        private DataTable dt人员初始;
        private DataTable dt权限初始;

        //dgv样式初始化
        private void Initdgv()
        {
            bs开机 = new BindingSource();
            dgv开机.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv开机.AllowUserToAddRows = false;
            dgv开机.ReadOnly = false;
            dgv开机.RowHeadersVisible = false;
            dgv开机.AllowUserToResizeColumns = true;
            dgv开机.AllowUserToResizeRows = false;
            dgv开机.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv开机.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv开机.Font = new Font("宋体", 12);

            bs清场 = new BindingSource();
            dgv清场.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv清场.AllowUserToAddRows = false;
            dgv清场.ReadOnly = false;
            dgv清场.RowHeadersVisible = false;
            dgv清场.AllowUserToResizeColumns = true;
            dgv清场.AllowUserToResizeRows = false;
            dgv清场.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv清场.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv清场.Font = new Font("宋体", 12);

            bs产品 = new BindingSource();
            dgv产品.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品.AllowUserToAddRows = false;
            dgv产品.ReadOnly = false;
            dgv产品.RowHeadersVisible = false;
            dgv产品.AllowUserToResizeColumns = true;
            dgv产品.AllowUserToResizeRows = false;
            dgv产品.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.Font = new Font("宋体", 12);

            bs产品编码 = new BindingSource();
            dgv产品编码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品编码.AllowUserToAddRows = false;
            dgv产品编码.ReadOnly = false;
            dgv产品编码.RowHeadersVisible = false;
            dgv产品编码.AllowUserToResizeColumns = true;
            dgv产品编码.AllowUserToResizeRows = false;
            dgv产品编码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.Font = new Font("宋体", 12);

            bs物料代码 = new BindingSource();
            dgv物料代码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv物料代码.AllowUserToAddRows = false;
            dgv物料代码.ReadOnly = false;
            dgv物料代码.RowHeadersVisible = false;
            dgv物料代码.AllowUserToResizeColumns = true;
            dgv物料代码.AllowUserToResizeRows = false;
            dgv物料代码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.Font = new Font("宋体", 12);

            bs人员 = new BindingSource();
            dgv人员.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv人员.AllowUserToAddRows = false;
            dgv人员.ReadOnly = false;
            dgv人员.RowHeadersVisible = false;
            dgv人员.AllowUserToResizeColumns = true;
            dgv人员.AllowUserToResizeRows = false;
            dgv人员.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv人员.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv人员.Font = new Font("宋体", 12);

            bs权限 = new BindingSource();
            dgv权限.AllowUserToAddRows = false;
            dgv权限.ReadOnly = false;
            dgv权限.RowHeadersVisible = false;
            dgv权限.AllowUserToResizeColumns = true;
            dgv权限.AllowUserToResizeRows = false;
            dgv权限.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv权限.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv权限.Font = new Font("宋体", 12);
        }

        private void Bind()
        {
            //**************************   开机    ***********************************
            dt开机 = new DataTable("设置开机确认项目"); //""中的是表名
            dt开机初始 = new DataTable("设置开机确认项目"); 
            da开机 = new OleDbDataAdapter("select * from 设置开机确认项目", mySystem.Parameter.connOle);
            cb开机 = new OleDbCommandBuilder(da开机);
            dt开机.Columns.Add("序号", System.Type.GetType("System.String"));
            da开机.Fill(dt开机);
            da开机.Fill(dt开机初始);
            bs开机.DataSource = dt开机;
            this.dgv开机.DataSource = bs开机.DataSource;
            //显示序号
            numFresh(this.dgv开机);
            this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv开机.Columns["ID"].Visible = false;

            //**************************   清场    ***********************************
            dt清场 = new DataTable("设置清场项目"); //""中的是表名
            dt清场初始 = new DataTable("设置清场项目");
            da清场 = new OleDbDataAdapter("select * from 设置清场项目", mySystem.Parameter.connOle);
            cb清场 = new OleDbCommandBuilder(da清场);
            dt清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da清场.Fill(dt清场);
            da清场.Fill(dt清场初始);
            bs清场.DataSource = dt清场;
            this.dgv清场.DataSource = bs清场.DataSource;
            //显示序号
            numFresh(this.dgv清场);
            this.dgv清场.Columns["清场项目"].MinimumWidth = 200;
            this.dgv清场.Columns["清场要点"].MinimumWidth = 250;
            this.dgv清场.Columns["清场要点"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv清场.Columns["清场要点"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv清场.Columns["ID"].Visible = false;

            //************************    产品     *******************************************
            dt产品 = new DataTable("设置清洁分切产品"); //""中的是表名
            dt产品初始 = new DataTable("设置清洁分切产品");
            da产品 = new OleDbDataAdapter("select * from 设置清洁分切产品", mySystem.Parameter.connOle);
            cb产品 = new OleDbCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            da产品.Fill(dt产品初始);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            numFresh(this.dgv产品);
            this.dgv产品.Columns["产品名称"].MinimumWidth = 200;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置清洁分切产品编码"); //""中的是表名
            dt产品编码初始 = new DataTable("设置清洁分切产品编码");
            da产品编码 = new OleDbDataAdapter("select * from 设置清洁分切产品编码", mySystem.Parameter.connOle);
            cb产品编码 = new OleDbCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            da产品编码.Fill(dt产品编码初始);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            numFresh(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 200;
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品编码.Columns["ID"].Visible = false;

            //************************    物料代码     *******************************************
            dt物料代码 = new DataTable("设置物料代码"); //""中的是表名
            dt物料代码初始 = new DataTable("设置物料代码");
            da物料代码 = new OleDbDataAdapter("select * from 设置物料代码", mySystem.Parameter.connOle);
            cb物料代码 = new OleDbCommandBuilder(da物料代码);
            dt物料代码.Columns.Add("序号", System.Type.GetType("System.String"));
            da物料代码.Fill(dt物料代码);
            da物料代码.Fill(dt物料代码初始);
            bs物料代码.DataSource = dt物料代码;
            this.dgv物料代码.DataSource = bs物料代码.DataSource;
            //显示序号
            numFresh(this.dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].MinimumWidth = 200;
            this.dgv物料代码.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv物料代码.Columns["物料代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv物料代码.Columns["ID"].Visible = false;

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("用户"); //""中的是表名
            dt人员初始 = new DataTable("用户");
            da人员 = new OleDbDataAdapter("select * from 用户", mySystem.Parameter.connOle);
            cb人员 = new OleDbCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            da人员.Fill(dt人员初始);
            bs人员.DataSource = dt人员;
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            numFresh(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            this.dgv人员.Columns["用户名"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv人员.Columns["ID"].Visible = false;

            //************************    人员权限     *******************************************
            dt权限 = new DataTable("用户权限"); //""中的是表名
            dt权限初始 = new DataTable("用户权限");
            da权限 = new OleDbDataAdapter("select * from 用户权限", mySystem.Parameter.connOle);
            cb权限 = new OleDbCommandBuilder(da权限);
            dt权限.Columns.Add("序号", System.Type.GetType("System.String"));
            da权限.Fill(dt权限);
            da权限.Fill(dt权限初始);
            bs权限.DataSource = dt权限;
            this.dgv权限.DataSource = bs权限.DataSource;
            //显示序号
            numFresh(this.dgv权限);
            this.dgv权限.Columns["步骤"].MinimumWidth = 250;
            this.dgv权限.Columns["操作员"].MinimumWidth = 150;
            this.dgv权限.Columns["审核员"].MinimumWidth = 150;
            this.dgv权限.Columns["步骤"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;

        }

        private void numFresh(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        private void add开机_Click(object sender, EventArgs e)
        {
            DataRow dr = dt开机.NewRow();
            dt开机.Rows.InsertAt(dt开机.NewRow(), dt开机.Rows.Count);
            numFresh(this.dgv开机);
        }

        private void del开机_Click(object sender, EventArgs e)
        {
            int idx = this.dgv开机.CurrentRow.Index;
            dt开机.Rows[idx].Delete();
            numFresh(this.dgv开机);
        }

        private void add清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt清场.NewRow();
            dt清场.Rows.InsertAt(dt清场.NewRow(), dt清场.Rows.Count);
            numFresh(this.dgv清场);
        }

        private void del清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv清场.CurrentRow.Index;
            dt清场.Rows[idx].Delete();
            numFresh(this.dgv清场);
        }

        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            numFresh(this.dgv产品);
        }

        private void del产品_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品.CurrentRow.Index;
            dt产品.Rows[idx].Delete();
            numFresh(this.dgv产品);
        }

        private void add产品编码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品编码.NewRow();
            dt产品编码.Rows.InsertAt(dt产品编码.NewRow(), dt产品编码.Rows.Count);
            numFresh(this.dgv产品编码);
        }

        private void del产品编码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv产品编码.CurrentRow.Index;
            dt产品编码.Rows[idx].Delete();
            numFresh(this.dgv产品编码);
        }

        private void add物料代码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt物料代码.NewRow();
            dt物料代码.Rows.InsertAt(dt物料代码.NewRow(), dt物料代码.Rows.Count);
            numFresh(this.dgv物料代码);
        }

        private void del物料代码_Click(object sender, EventArgs e)
        {
            int idx = this.dgv物料代码.CurrentRow.Index;
            dt物料代码.Rows[idx].Delete();
            numFresh(this.dgv物料代码);
        }       

        //private void CancelBtn_Click(object sender, EventArgs e)
        //{
        //    //复原
        //    dt开机.Clear();
        //    dt开机 = dt开机初始.Copy();
        //    dt清场.Clear();
        //    dt清场 = dt清场初始.Copy();
        //    dt产品.Clear();
        //    dt产品 = dt产品初始.Copy();
        //    dt产品编码.Clear();
        //    dt产品编码 = dt产品编码初始.Copy();
        //    dt物料代码.Clear();
        //    dt物料代码 = dt物料代码初始.Copy();
        //}

        private void Btn保存项目_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da开机.Update((DataTable)bs开机.DataSource);
                    dt开机.Clear();
                    da开机.Fill(dt开机);
                    da开机.Fill(dt开机初始);
                    numFresh(this.dgv开机);

                    da清场.Update((DataTable)bs清场.DataSource);
                    dt清场.Clear();
                    da清场.Fill(dt清场);
                    da清场.Fill(dt清场初始);
                    numFresh(this.dgv清场);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
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
                    da产品.Fill(dt产品初始);
                    numFresh(this.dgv产品);

                    da产品编码.Update((DataTable)bs产品编码.DataSource);
                    dt产品编码.Clear();
                    da产品编码.Fill(dt产品编码);
                    da产品编码.Fill(dt产品编码初始);
                    numFresh(this.dgv产品编码);

                    da物料代码.Update((DataTable)bs物料代码.DataSource);
                    dt物料代码.Clear();
                    da物料代码.Fill(dt物料代码);
                    da物料代码.Fill(dt物料代码初始);
                    numFresh(this.dgv物料代码);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }

        private void add人员_Click(object sender, EventArgs e)
        {
            DataRow dr = dt人员.NewRow();
            dt人员.Rows.InsertAt(dt人员.NewRow(), dt人员.Rows.Count);
            numFresh(this.dgv人员);
        }

        private void del人员_Click(object sender, EventArgs e)
        {
            int idx = this.dgv人员.SelectedCells[0].RowIndex;
            dt人员.Rows[idx].Delete();
            numFresh(this.dgv人员);
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
                        numFresh(this.dgv人员);

                        Boolean c = checkPeopleRight(); //判断用户是否在清洁分切用户表中
                        if (c)
                        {
                            da权限.Update((DataTable)bs权限.DataSource);
                            dt权限.Clear();
                            da权限.Fill(dt权限);
                            numFresh(this.dgv权限);

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

        //检查人员是否在清洁分切人员中
        private Boolean checkPeopleRight()
        {
            Boolean b = true;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            foreach (DataRow dr in dt权限.Rows)
            {
                String name1 = dr["操作员"].ToString();
                comm.CommandText = "select * from 用户 where 用户名 = " + "'" + name1 + "' ";
                OleDbDataReader reader1 = comm.ExecuteReader();
                if (reader1.HasRows)
                {
                    String name2 = dr["审核员"].ToString();
                    comm.CommandText = "select * from 用户 where 用户名 = " + "'" + name2 + "' ";
                    OleDbDataReader reader2 = comm.ExecuteReader();
                    if (!reader2.HasRows)
                    {
                        b = false;
                        MessageBox.Show("员工" + "“" + name2 + "”" + "无操作清洁分切权限！"); 
                    }
                }
                else
                {
                    b = false;
                    MessageBox.Show("员工" + "“" + name1 + "”" + "无操作清洁分切权限！"); 
                }
                reader1.Dispose();
            }

            comm.Dispose();
            return b;
        }

        

        private void Btn取消人员_Click(object sender, EventArgs e)
        {

        }

        private void Btn取消产品_Click(object sender, EventArgs e)
        {

        }

        private void Btn取消项目_Click(object sender, EventArgs e)
        {

        }


    }
}
