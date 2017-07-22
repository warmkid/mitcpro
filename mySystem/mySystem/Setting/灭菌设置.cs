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
            this.dgv产品.Columns["产品名称"].MinimumWidth = 200;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置灭菌产品编码"); //""中的是表名
            da产品编码 = new OleDbDataAdapter("select * from 设置灭菌产品编码", mySystem.Parameter.connOle);
            cb产品编码 = new OleDbCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 200;
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品编码.Columns["ID"].Visible = false;
            
            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("用户"); //""中的是表名
            da人员 = new OleDbDataAdapter("select * from 用户", mySystem.Parameter.connOle);
            cb人员 = new OleDbCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            bs人员.DataSource = dt人员;
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            this.dgv人员.Columns["用户名"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv人员.Columns["ID"].Visible = false;

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
            this.dgv权限.Columns["步骤"].MinimumWidth = 250;
            this.dgv权限.Columns["操作员"].MinimumWidth = 150;
            this.dgv权限.Columns["审核员"].MinimumWidth = 150;
            this.dgv权限.Columns["步骤"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;

        }

        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

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
            Boolean b = true;
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            OleDbCommand comm2 = new OleDbCommand();
            comm2.Connection = Parameter.connOle;
            foreach (DataRow dr in dt权限.Rows)
            {
                String name1 = dr["操作员"].ToString();
                comm1.CommandText = "select * from 用户 where 用户名 = " + "'" + name1 + "' ";
                OleDbDataReader reader1 = comm1.ExecuteReader();
                if (reader1.HasRows)
                {
                    String name2 = dr["审核员"].ToString();
                    comm2.CommandText = "select * from 用户 where 用户名 = " + "'" + name2 + "' ";
                    OleDbDataReader reader2 = comm2.ExecuteReader();
                    if (!reader2.HasRows)
                    {
                        b = false;
                        MessageBox.Show("员工" + "“" + name2 + "”" + "无操作灭菌工序权限！");
                    }
                    reader2.Dispose();
                }
                else
                {
                    b = false;
                    MessageBox.Show("员工" + "“" + name1 + "”" + "无操作灭菌工序权限！");
                }
                reader1.Dispose();
            }

            comm1.Dispose();
            comm2.Dispose();
            return b;
        }





    }
}
