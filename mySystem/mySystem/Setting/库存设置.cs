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
    public partial class 库存设置 : Form
    {
        public 库存设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
        }

        private OleDbDataAdapter da供应商;
        private DataTable dt供应商;
        private BindingSource bs供应商;
        private OleDbCommandBuilder cb供应商;
        private OleDbDataAdapter da检验标准;
        private DataTable dt检验标准;
        private BindingSource bs检验标准;
        private OleDbCommandBuilder cb检验标准;
        private OleDbDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private OleDbCommandBuilder cb人员;
        private OleDbDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private OleDbCommandBuilder cb权限;


        private void Initdgv()
        {
            bs供应商 = new BindingSource();
            EachInitdgv(dgv供应商);

            bs检验标准 = new BindingSource();
            EachInitdgv(dgv检验标准);

            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);

            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);

        }


        private void Bind()
        {
            //**************************   供应商    ***********************************
            dt供应商 = new DataTable("设置供应商信息"); //""中的是表名
            da供应商 = new OleDbDataAdapter("select * from 设置供应商信息", mySystem.Parameter.connOle);
            cb供应商 = new OleDbCommandBuilder(da供应商);
            dt供应商.Columns.Add("序号", System.Type.GetType("System.String"));
            da供应商.Fill(dt供应商);
            bs供应商.DataSource = dt供应商;
            this.dgv供应商.DataSource = bs供应商.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv供应商);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv供应商);
            this.dgv供应商.Columns["ID"].Visible = false;

            //**************************   检验标准    ***********************************
            dt检验标准 = new DataTable("设置检验标准"); //""中的是表名
            da检验标准 = new OleDbDataAdapter("select * from 设置检验标准", mySystem.Parameter.connOle);
            cb检验标准 = new OleDbCommandBuilder(da检验标准);
            dt检验标准.Columns.Add("序号", System.Type.GetType("System.String"));
            da检验标准.Fill(dt检验标准);
            bs检验标准.DataSource = dt检验标准;
            this.dgv检验标准.DataSource = bs检验标准.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv检验标准);
            //this.dgv开机.Columns["确认项目"].MinimumWidth = 200;
            //this.dgv开机.Columns["确认内容"].MinimumWidth = 250;
            //this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Utility.setDataGridViewAutoSizeMode(dgv检验标准);
            this.dgv检验标准.Columns["ID"].Visible = false;

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("库存用户"); //""中的是表名
            da人员 = new OleDbDataAdapter("select * from 库存用户", mySystem.Parameter.connOle);
            cb人员 = new OleDbCommandBuilder(da人员);
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
            dt权限 = new DataTable("库存用户权限"); //""中的是表名
            da权限 = new OleDbDataAdapter("select * from 库存用户权限", mySystem.Parameter.connOle);
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
            Utility.setDataGridViewAutoSizeMode(dgv权限);
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

        private void add供应商_Click(object sender, EventArgs e)
        {
            DataRow dr = dt供应商.NewRow();
            dt供应商.Rows.InsertAt(dt供应商.NewRow(), dt供应商.Rows.Count);
            setDataGridViewRowNums(this.dgv供应商);
            if (dgv供应商.Rows.Count > 0)
                dgv供应商.FirstDisplayedScrollingRowIndex = dgv供应商.Rows.Count - 1;
        }

        private void del供应商_Click(object sender, EventArgs e)
        {
            int idx = this.dgv供应商.CurrentRow.Index;
            dt供应商.Rows[idx].Delete();
            da供应商.Update((DataTable)bs供应商.DataSource);
            dt供应商.Clear();
            da供应商.Fill(dt供应商);
            setDataGridViewRowNums(this.dgv供应商);
        }

        private void add检验标准_Click(object sender, EventArgs e)
        {
            DataRow dr = dt检验标准.NewRow();
            dt检验标准.Rows.InsertAt(dt检验标准.NewRow(), dt检验标准.Rows.Count);
            setDataGridViewRowNums(this.dgv检验标准);
            if (dgv检验标准.Rows.Count > 0)
                dgv检验标准.FirstDisplayedScrollingRowIndex = dgv检验标准.Rows.Count - 1;
        }

        private void del检验标准_Click(object sender, EventArgs e)
        {
            int idx = this.dgv检验标准.CurrentRow.Index;
            dt检验标准.Rows[idx].Delete();
            da检验标准.Update((DataTable)bs检验标准.DataSource);
            dt检验标准.Clear();
            da检验标准.Fill(dt检验标准);
            setDataGridViewRowNums(this.dgv检验标准);
        }

        private void Btn保存项目_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da供应商.Update((DataTable)bs供应商.DataSource);
                    dt供应商.Clear();
                    da供应商.Fill(dt供应商);
                    setDataGridViewRowNums(this.dgv供应商);

                    da检验标准.Update((DataTable)bs检验标准.DataSource);
                    dt检验标准.Clear();
                    da检验标准.Fill(dt检验标准);
                    setDataGridViewRowNums(this.dgv检验标准);

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
            catch(Exception ee)
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

        //检查人员是否在订单库存管理人员中
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
            comm.CommandText = "select * from 库存用户 where 用户名 = " + "'" + name + "' ";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            { b = true; }
            else
            {
                b = false;
                MessageBox.Show("员工" + "“" + name + "”" + "无库存管理权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
        }
        


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

        


    }
}
