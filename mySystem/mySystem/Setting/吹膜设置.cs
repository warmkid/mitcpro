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
using System.Collections;

namespace mySystem.Setting
{
    public partial class 吹膜设置 : Form
    {
        public 吹膜设置()
        {
            InitializeComponent();
            Initdgv();
            Bind();
            FillNum();
        }

        int para1 = 0;
        double para2 = 0;
        double para3 = 0;
        double para4 = 0;
        private SqlDataAdapter da清洁;
        private DataTable dt清洁;
        private BindingSource bs清洁;
        private SqlCommandBuilder cb清洁;
        private SqlDataAdapter da供料清场;
        private DataTable dt供料清场;
        private BindingSource bs供料清场;
        private SqlCommandBuilder cb供料清场;
        private SqlDataAdapter da吹膜清场;
        private DataTable dt吹膜清场;
        private BindingSource bs吹膜清场;
        private SqlCommandBuilder cb吹膜清场;
        private SqlDataAdapter da开机;
        private DataTable dt开机;
        private BindingSource bs开机;
        private SqlCommandBuilder cb开机;
        private SqlDataAdapter da交接班;
        private DataTable dt交接班;
        private BindingSource bs交接班;
        private SqlCommandBuilder cb交接班;
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
        private SqlDataAdapter da工艺;
        private DataTable dt工艺;
        private BindingSource bs工艺;
        private SqlCommandBuilder cb工艺;
        private SqlDataAdapter da废品;
        private DataTable dt废品;
        private BindingSource bs废品;
        private SqlCommandBuilder cb废品;
        private SqlDataAdapter da人员;
        private DataTable dt人员;
        private BindingSource bs人员;
        private SqlCommandBuilder cb人员;
        private SqlDataAdapter da权限;
        private DataTable dt权限;
        private BindingSource bs权限;
        private SqlCommandBuilder cb权限;
        private SqlDataAdapter da代码批号;
        private DataTable dt代码批号;
        private BindingSource bs代码批号;
        private SqlCommandBuilder cb代码批号;

        //dgv样式初始化
        private void Initdgv()
        {
            bs清洁 = new BindingSource();
            EachInitdgv(dgv清洁);
            bs供料清场 = new BindingSource();
            EachInitdgv(dgv供料清场);
            bs吹膜清场 = new BindingSource();
            EachInitdgv(dgv吹膜清场);
            bs开机 = new BindingSource();
            EachInitdgv(dgv开机);
            bs交接班 = new BindingSource();
            EachInitdgv(dgv交接班);
            bs产品 = new BindingSource();
            EachInitdgv(dgv产品);
            bs产品编码 = new BindingSource();
            EachInitdgv(dgv产品编码);
            bs物料代码 = new BindingSource();
            EachInitdgv(dgv物料代码);
            bs工艺 = new BindingSource();
            EachInitdgv(dgv工艺);
            bs废品 = new BindingSource();
            EachInitdgv(dgv废品);
            bs人员 = new BindingSource();
            EachInitdgv(dgv人员);
            bs权限 = new BindingSource();
            EachInitdgv(dgv权限);
            bs代码批号 = new BindingSource();
            EachInitdgv(dgv代码批号);
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

        private void Bind()
        {
            //**************************   清洁    ***********************************
            dt清洁 = new DataTable("设置吹膜机组清洁项目"); //""中的是表名
            da清洁 = new SqlDataAdapter("select * from 设置吹膜机组清洁项目", mySystem.Parameter.conn);
            cb清洁 = new SqlCommandBuilder(da清洁);
            dt清洁.Columns.Add("序号", System.Type.GetType("System.String"));
            da清洁.Fill(dt清洁);
            bs清洁.DataSource = dt清洁;
            this.dgv清洁.DataSource = bs清洁.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv清洁);
            this.dgv清洁.Columns["清洁区域"].MinimumWidth = 200;
            this.dgv清洁.Columns["清洁内容"].MinimumWidth = 250;
            this.dgv清洁.Columns["清洁内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv清洁);
            this.dgv清洁.Columns["清洁内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv清洁.Columns["ID"].Visible = false;

            //**************************   供料清场    ***********************************
            dt供料清场 = new DataTable("设置供料工序清场项目"); //""中的是表名
            da供料清场 = new SqlDataAdapter("select * from 设置供料工序清场项目", mySystem.Parameter.conn);
            cb供料清场 = new SqlCommandBuilder(da供料清场);
            dt供料清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da供料清场.Fill(dt供料清场);
            bs供料清场.DataSource = dt供料清场;
            this.dgv供料清场.DataSource = bs供料清场.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv供料清场);
            this.dgv供料清场.Columns["清场内容"].MinimumWidth = 200;
            this.dgv供料清场.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv供料清场);
            this.dgv供料清场.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv供料清场.Columns["ID"].Visible = false;

            //************************    吹膜清场     *******************************************
            dt吹膜清场 = new DataTable("设置吹膜工序清场项目"); //""中的是表名
            da吹膜清场 = new SqlDataAdapter("select * from 设置吹膜工序清场项目", mySystem.Parameter.conn);
            cb吹膜清场 = new SqlCommandBuilder(da吹膜清场);
            dt吹膜清场.Columns.Add("序号", System.Type.GetType("System.String"));
            da吹膜清场.Fill(dt吹膜清场);
            bs吹膜清场.DataSource = dt吹膜清场;
            this.dgv吹膜清场.DataSource = bs吹膜清场.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv吹膜清场);
            this.dgv吹膜清场.Columns["清场内容"].MinimumWidth = 200;
            this.dgv吹膜清场.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv吹膜清场);
            this.dgv吹膜清场.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv吹膜清场.Columns["ID"].Visible = false;

            //**************************   开机    ***********************************
            dt开机 = new DataTable("设置吹膜机组开机前确认项目"); //""中的是表名
            da开机 = new SqlDataAdapter("select * from 设置吹膜机组开机前确认项目", mySystem.Parameter.conn);
            cb开机 = new SqlCommandBuilder(da开机);
            dt开机.Columns.Add("序号", System.Type.GetType("System.String"));
            da开机.Fill(dt开机);
            bs开机.DataSource = dt开机;
            this.dgv开机.DataSource = bs开机.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv开机);
            this.dgv开机.Columns["确认项目"].MinimumWidth = 160;
            this.dgv开机.Columns["确认内容"].MinimumWidth = 200;
            this.dgv开机.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv开机);
            this.dgv开机.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv开机.Columns["ID"].Visible = false;

            //************************    交接班     *******************************************
            dt交接班 = new DataTable("设置岗位交接班项目"); //""中的是表名
            da交接班 = new SqlDataAdapter("select * from 设置岗位交接班项目", mySystem.Parameter.conn);
            cb交接班 = new SqlCommandBuilder(da交接班);
            dt交接班.Columns.Add("序号", System.Type.GetType("System.String"));
            da交接班.Fill(dt交接班);
            bs交接班.DataSource = dt交接班;
            this.dgv交接班.DataSource = bs交接班.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv交接班);
            this.dgv交接班.Columns["确认项目"].MinimumWidth = 250;
            this.dgv交接班.Columns["确认项目"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv交接班);
            this.dgv交接班.Columns["确认项目"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv交接班.Columns["ID"].Visible = false;

            //************************    产品     *******************************************
            dt产品 = new DataTable("设置吹膜产品"); //""中的是表名
            da产品 = new SqlDataAdapter("select * from 设置吹膜产品", mySystem.Parameter.conn);
            cb产品 = new SqlCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品);
            this.dgv产品.Columns["产品名称"].MinimumWidth = 150;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv产品);
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置吹膜产品编码"); //""中的是表名
            da产品编码 = new SqlDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.conn);
            cb产品编码 = new SqlCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 150;
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
            this.dgv物料代码.Columns["物料代码"].MinimumWidth = 150;
            this.dgv物料代码.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv物料代码.Columns["ID"].Visible = false;

            //**************************   工艺    ***********************************
            dt工艺 = new DataTable("设置吹膜工艺"); //""中的是表名
            da工艺 = new SqlDataAdapter("select * from 设置吹膜工艺", mySystem.Parameter.conn);
            cb工艺 = new SqlCommandBuilder(da工艺);
            dt工艺.Columns.Add("序号", System.Type.GetType("System.String"));
            da工艺.Fill(dt工艺);
            bs工艺.DataSource = dt工艺;
            this.dgv工艺.DataSource = bs工艺.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv工艺);
            this.dgv工艺.Columns["工艺名称"].MinimumWidth = 150;
            this.dgv工艺.Columns["工艺名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv工艺);
            this.dgv工艺.Columns["工艺名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv工艺.Columns["ID"].Visible = false;

            //**************************   废品    ***********************************
            dt废品 = new DataTable("设置废品产生原因"); //""中的是表名
            da废品 = new SqlDataAdapter("select * from 设置废品产生原因", mySystem.Parameter.conn);
            cb废品 = new SqlCommandBuilder(da废品);
            dt废品.Columns.Add("序号", System.Type.GetType("System.String"));
            da废品.Fill(dt废品);
            bs废品.DataSource = dt废品;
            this.dgv废品.DataSource = bs废品.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv废品);
            this.dgv废品.Columns["废品产生原因"].MinimumWidth = 250;
            this.dgv废品.Columns["废品产生原因"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Utility.setDataGridViewAutoSizeMode(dgv废品);
            this.dgv废品.Columns["废品产生原因"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv废品.Columns["ID"].Visible = false;

            //**************************   人员设置    ***********************************
            dt人员 = new DataTable("用户"); //""中的是表名
            da人员 = new SqlDataAdapter("select * from 用户", mySystem.Parameter.conn);
            cb人员 = new SqlCommandBuilder(da人员);
            dt人员.Columns.Add("序号", System.Type.GetType("System.String"));
            da人员.Fill(dt人员);
            bs人员.DataSource = dt人员;
            dgv人员下拉框设置();
            this.dgv人员.DataSource = bs人员.DataSource;
            //显示序号
            setDataGridViewRowNums(this.dgv人员);
            this.dgv人员.Columns["用户名"].MinimumWidth = 150;
            this.dgv人员.Columns["班次"].MinimumWidth = 100;
            Utility.setDataGridViewAutoSizeMode(dgv人员);
            this.dgv人员.Columns["用户名"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv人员.Columns["ID"].Visible = false;

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
            Utility.setDataGridViewAutoSizeMode(dgv权限);
            this.dgv权限.Columns["步骤"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv权限.Columns["ID"].Visible = false;
            // ********   代码批号 设置 ***********
            dt代码批号 = new DataTable("设置产品代码和产品批号的对应关系");
            da代码批号 = new SqlDataAdapter("select * from 设置产品代码和产品批号的对应关系", mySystem.Parameter.conn);
            cb代码批号 = new SqlCommandBuilder(da代码批号);
            dt代码批号.Columns.Add("序号", System.Type.GetType("System.String"));
            da代码批号.Fill(dt代码批号);
            bs代码批号.DataSource = dt代码批号;
            this.dgv代码批号.DataSource = bs代码批号.DataSource;
            setDataGridViewRowNums(this.dgv代码批号);
            this.dgv代码批号.Columns["ID"].Visible = false;
            Utility.setDataGridViewAutoSizeMode(dgv代码批号);
        }

        private void setDataGridViewRowNums(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }


        #region 区域设置
        private void add清洁_Click(object sender, EventArgs e)
        {
            DataRow dr = dt清洁.NewRow();
            dt清洁.Rows.InsertAt(dt清洁.NewRow(), dt清洁.Rows.Count);
            setDataGridViewRowNums(this.dgv清洁);
            if (dgv清洁.Rows.Count > 0)
                dgv清洁.FirstDisplayedScrollingRowIndex = dgv清洁.Rows.Count - 1;
        }

        private void del清洁_Click(object sender, EventArgs e)
        {
            int idx = this.dgv清洁.CurrentRow.Index;
            dt清洁.Rows[idx].Delete();
            da清洁.Update((DataTable)bs清洁.DataSource);
            dt清洁.Clear();
            da清洁.Fill(dt清洁);
            setDataGridViewRowNums(this.dgv清洁);
        }

        private void add供料清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt供料清场.NewRow();
            dt供料清场.Rows.InsertAt(dt供料清场.NewRow(), dt供料清场.Rows.Count);
            setDataGridViewRowNums(this.dgv供料清场);
            if (dgv供料清场.Rows.Count > 0)
                dgv供料清场.FirstDisplayedScrollingRowIndex = dgv供料清场.Rows.Count - 1;
        }

        private void del供料清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv供料清场.CurrentRow.Index;
            dt供料清场.Rows[idx].Delete();
            da供料清场.Update((DataTable)bs供料清场.DataSource);
            dt供料清场.Clear();
            da供料清场.Fill(dt供料清场);
            setDataGridViewRowNums(this.dgv供料清场);
        }

        private void add吹膜清场_Click(object sender, EventArgs e)
        {
            DataRow dr = dt吹膜清场.NewRow();
            dt吹膜清场.Rows.InsertAt(dt吹膜清场.NewRow(), dt吹膜清场.Rows.Count);
            setDataGridViewRowNums(this.dgv吹膜清场);
            if (dgv吹膜清场.Rows.Count > 0)
                dgv吹膜清场.FirstDisplayedScrollingRowIndex = dgv吹膜清场.Rows.Count - 1;
        }

        private void del吹膜清场_Click(object sender, EventArgs e)
        {
            int idx = this.dgv吹膜清场.CurrentRow.Index;
            dt吹膜清场.Rows[idx].Delete();
            da吹膜清场.Update((DataTable)bs吹膜清场.DataSource);
            dt吹膜清场.Clear();
            da吹膜清场.Fill(dt吹膜清场);
            setDataGridViewRowNums(this.dgv吹膜清场);
        }

        private void Btn保存区域设置_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                { }
                else
                {
                    da清洁.Update((DataTable)bs清洁.DataSource);
                    dt清洁.Clear();
                    da清洁.Fill(dt清洁);
                    setDataGridViewRowNums(this.dgv清洁);

                    da供料清场.Update((DataTable)bs供料清场.DataSource);
                    dt供料清场.Clear();
                    da供料清场.Fill(dt供料清场);
                    setDataGridViewRowNums(this.dgv供料清场);

                    da吹膜清场.Update((DataTable)bs吹膜清场.DataSource);
                    dt吹膜清场.Clear();
                    da吹膜清场.Fill(dt吹膜清场);
                    setDataGridViewRowNums(this.dgv吹膜清场);

                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }
        #endregion

        #region 项目设置
        private void add开机_Click(object sender, EventArgs e)
        {
            DataRow dr = dt开机.NewRow();
            dt开机.Rows.InsertAt(dt开机.NewRow(), dt开机.Rows.Count);
            setDataGridViewRowNums(this.dgv开机);
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

        private void add交接班_Click(object sender, EventArgs e)
        {
            DataRow dr = dt交接班.NewRow();
            dt交接班.Rows.InsertAt(dt交接班.NewRow(), dt交接班.Rows.Count);
            setDataGridViewRowNums(this.dgv交接班);
            if (dgv交接班.Rows.Count > 0)
                dgv交接班.FirstDisplayedScrollingRowIndex = dgv交接班.Rows.Count - 1;
        }

        private void del交接班_Click(object sender, EventArgs e)
        {
            int idx = this.dgv交接班.CurrentRow.Index;
            dt交接班.Rows[idx].Delete();
            da交接班.Update((DataTable)bs开机.DataSource);
            dt交接班.Clear();
            da交接班.Fill(dt交接班);
            setDataGridViewRowNums(this.dgv交接班);
        }

        private void Btn保存项目设置_Click(object sender, EventArgs e)
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
                    setDataGridViewRowNums(this.dgv开机);

                    da交接班.Update((DataTable)bs交接班.DataSource);
                    dt交接班.Clear();
                    da交接班.Fill(dt交接班);
                    setDataGridViewRowNums(this.dgv交接班);
                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！", "错误"); }
        }
        #endregion

        #region 参数设置

        //初始填预热参数textbox
        private void FillNum()
        {
            string tblName = "设置吹膜机组预热参数记录表";
            List<String> readqueryCols = new List<String>(new String[] { "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" ,"温度公差"});
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList;
            if (mySystem.Parameter.isSqlOk)
            {
                queryValsList = Utility.selectAccess(Parameter.conn, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);
            }
            else
            {
                queryValsList = Utility.selectAccess(mySystem.Parameter.conn, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);
            }

            List<String> data = new List<String> { };
            for (int i = 0; i < queryValsList[0].Count; i++)
            {
                data.Add(queryValsList[0][i].ToString());
            }
            List<Control> textboxes = new List<Control> { hw1, ld1, mj1, jt11, jt21, km1, duration1, region11, region21, region31, region41, hw2, 
                    ld2, mj2, jt12, jt22, km2, duration2, region12, region22, region32, region42, duration3, tolerance};
            Utility.fillControl(textboxes, data);


            //读取数据库并显示
            String tblName2 = "设置生产指令参数";
            List<String> readqueryCols2 = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
            List<String> whereCols2 = new List<String>(new String[] { "ID" });
            List<Object> whereVals2 = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList2;
            if (mySystem.Parameter.isSqlOk)
            {
                queryValsList2 = Utility.selectAccess(Parameter.conn, tblName2, readqueryCols2, whereCols2, whereVals2, null, null, null, null, null);
            }
            else
            {
                queryValsList2 = Utility.selectAccess(mySystem.Parameter.conn, tblName2, readqueryCols2, whereCols2, whereVals2, null, null, null, null, null);
            }

            List<String> data2 = new List<String> { };
            for (int i = 0; i < queryValsList2[0].Count; i++)
            {
                data2.Add(queryValsList2[0][i].ToString());
            }
            List<Control> textboxes2 = new List<Control> { tB面数, tB厚度密度, tB参数1, tB参数2 };
            Utility.fillControl(textboxes2, data2);

        }

        private void Btn保存参数设置_Click(object sender, EventArgs e)
        {
            try
            {
                string tblName = "设置吹膜机组预热参数记录表";
                List<String> queryCols = new List<String>(new String[] { "温度公差" , "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" });
                List<Object> queryVals = new List<Object>(new Object[] { Convert.ToDouble(tolerance.Text), Convert.ToDouble(hw1.Text), Convert.ToDouble(ld1.Text), 
                Convert.ToDouble(mj1.Text), Convert.ToDouble(jt11.Text), Convert.ToDouble(jt21.Text), Convert.ToDouble(km1.Text), 
                Convert.ToDouble(duration1.Text), Convert.ToDouble(region11.Text), Convert.ToDouble(region21.Text), 
                Convert.ToDouble(region31.Text), Convert.ToDouble(region41.Text), Convert.ToDouble(hw2.Text), Convert.ToDouble(ld2.Text), 
                Convert.ToDouble(mj2.Text), Convert.ToDouble(jt12.Text), Convert.ToDouble(jt22.Text), Convert.ToDouble(km2.Text), 
                Convert.ToDouble(duration2.Text), Convert.ToDouble(region12.Text), Convert.ToDouble(region22.Text), 
                Convert.ToDouble(region32.Text), Convert.ToDouble(region42.Text), Convert.ToDouble(duration3.Text)});
                List<String> whereCols = new List<String>(new String[] { "ID" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(mySystem.Parameter.conn, tblName, queryCols, queryVals, whereCols, whereVals);
                if (!b)
                {
                    MessageBox.Show("预热参数设置保存失败！", "错误");
                    return;
                }

                para1 = Convert.ToInt32(tB面数.Text.Trim());
                para2 = Convert.ToDouble(tB厚度密度.Text.Trim());
                para3 = Convert.ToDouble(tB参数1.Text.Trim());
                para4 = Convert.ToDouble(tB参数2.Text.Trim());
                String tblName2 = "设置生产指令参数";
                List<String> updateCols2 = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
                List<Object> updateVals2 = new List<Object>(new Object[] { para1, para2, para3, para4 });
                List<String> whereCols2 = new List<String>(new String[] { "ID" });
                List<Object> whereVals2 = new List<Object>(new Object[] { 1 });
                Boolean b2 = Utility.updateAccess(mySystem.Parameter.conn, tblName2, updateCols2, updateVals2, whereCols2, whereVals2);
                if (!b2)
                {
                    MessageBox.Show("参数保存失败", "错误");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("参数保存失败", "错误");
                return;
            }


            MessageBox.Show("保存成功");
        }

        #endregion

        #region 产品设置
        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            setDataGridViewRowNums(this.dgv产品);
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
            dr["面数"] = 2;
            dt产品编码.Rows.InsertAt(dt产品编码.NewRow(), dt产品编码.Rows.Count);
            setDataGridViewRowNums(this.dgv产品编码);
            ////dgv产品编码.CurrentCell = dgv产品编码.Rows[dgv产品编码.Rows.Count - 1].Cells[1];
            ////dgv产品编码.BeginEdit(true);
            //dgv产品编码.Rows[dgv产品编码.Rows.Count - 1].Cells
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

        private void add工艺_Click(object sender, EventArgs e)
        {
            DataRow dr = dt工艺.NewRow();
            dt工艺.Rows.InsertAt(dt工艺.NewRow(), dt工艺.Rows.Count);
            setDataGridViewRowNums(this.dgv工艺);
            if (dgv工艺.Rows.Count > 0)
                dgv工艺.FirstDisplayedScrollingRowIndex = dgv工艺.Rows.Count - 1;
        }

        private void del工艺_Click(object sender, EventArgs e)
        {
            int idx = this.dgv工艺.CurrentRow.Index;
            dt工艺.Rows[idx].Delete();
            da工艺.Update((DataTable)bs开机.DataSource);
            dt工艺.Clear();
            da工艺.Fill(dt工艺);
            setDataGridViewRowNums(this.dgv工艺);
        }

        private void add废品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt废品.NewRow();
            dt废品.Rows.InsertAt(dt废品.NewRow(), dt废品.Rows.Count);
            setDataGridViewRowNums(this.dgv废品);
            if (dgv废品.Rows.Count > 0)
                dgv废品.FirstDisplayedScrollingRowIndex = dgv废品.Rows.Count - 1;
        }

        private void del废品_Click(object sender, EventArgs e)
        {
            int idx = this.dgv废品.CurrentRow.Index;
            dt废品.Rows[idx].Delete();
            da废品.Update((DataTable)bs废品.DataSource);
            dt废品.Clear();
            da废品.Fill(dt废品);
            setDataGridViewRowNums(this.dgv废品);
        }

        private void Btn保存产品设置_Click(object sender, EventArgs e)
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

                    da物料代码.Update((DataTable)bs物料代码.DataSource);
                    dt物料代码.Clear();
                    da物料代码.Fill(dt物料代码);
                    setDataGridViewRowNums(this.dgv物料代码);

                    da工艺.Update((DataTable)bs工艺.DataSource);
                    dt工艺.Clear();
                    da工艺.Fill(dt工艺);
                    setDataGridViewRowNums(this.dgv工艺);

                    da废品.Update((DataTable)bs废品.DataSource);
                    dt废品.Clear();
                    da废品.Fill(dt废品);
                    setDataGridViewRowNums(this.dgv废品);

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

        //检查人员是否在吹膜人员中
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
                MessageBox.Show("员工" + "“" + name + "”" + "无操作吹膜工序权限，保存失败！");
            }

            reader.Dispose();
            comm.Dispose();
            return b;
        }

        #endregion


        #region DataBindingComplete事件
        private void dgv清洁_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv清洁.Columns["ID"].Visible = false;
        }

        private void dgv供料清场_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv供料清场.Columns["ID"].Visible = false;
        }

        private void dgv吹膜清场_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv吹膜清场.Columns["ID"].Visible = false;
        }

        private void dgv开机_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv开机.Columns["ID"].Visible = false;
        }

        private void dgv交接班_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv交接班.Columns["ID"].Visible = false;
        }

        private void dgv产品_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品.Columns["ID"].Visible = false;
        }

        private void dgv产品编码_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv产品编码.Columns["ID"].Visible = false;
        }

        private void dgv物料代码_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv物料代码.Columns["ID"].Visible = false;
        }

        private void dgv工艺_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv工艺.Columns["ID"].Visible = false;
        }

        private void dgv废品_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv废品.Columns["ID"].Visible = false;
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

        private void button7_Click(object sender, EventArgs e)
        {
            da代码批号.Update((DataTable)bs代码批号.DataSource);
            dt代码批号.Clear();
            da代码批号.Fill(dt代码批号);
            setDataGridViewRowNums(this.dgv代码批号);
        }

        private void add代码批号_Click(object sender, EventArgs e)
        {
            //DataRow dr = dt代码批号.NewRow();
            dt代码批号.Rows.Add(dt代码批号.NewRow());
            setDataGridViewRowNums(this.dgv代码批号);
            if (dgv代码批号.Rows.Count > 0)
                dgv代码批号.FirstDisplayedScrollingRowIndex = dgv代码批号.Rows.Count - 1;
        }

        private void del代码批号_Click(object sender, EventArgs e)
        {
            int idx = this.dgv代码批号.SelectedCells[0].RowIndex;
            dt代码批号.Rows[idx].Delete();
            da代码批号.Update((DataTable)bs代码批号.DataSource);
            dt代码批号.Clear();
            da代码批号.Fill(dt代码批号);
            setDataGridViewRowNums(this.dgv代码批号);
        }

        private void btn吹膜产品编码刷新_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da;
            DataTable dt, dtInSetting;
            SqlCommandBuilder cb;
            Hashtable htOld = new Hashtable(); ;
            da = new SqlDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                if (!htOld.ContainsKey(dr["产品编码"])) htOld[dr["产品编码"]] = dr["面数"];
                dr.Delete();
            }
            da.Update(dt);


            string strConnect = "server=" + Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn;
            conn = new SqlConnection(strConnect);
            conn.Open();
            da = new SqlDataAdapter("select 存货代码 from 设置存货档案 where 类型 like '%成品%' and 属于工序 like '%吹膜%' order by 存货代码", conn);
            dtInSetting = new DataTable();
            da.Fill(dtInSetting);


            da = new SqlDataAdapter("select * from 设置吹膜产品编码 ", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow drSetting in dtInSetting.Rows)
            {
                DataRow ndr = dt.NewRow();
                if (htOld.ContainsKey(drSetting["存货代码"]))
                {
                    ndr["产品编码"] = drSetting["存货代码"];
                    ndr["面数"] = htOld[drSetting["存货代码"]];
                }
                else
                {
                    ndr["产品编码"] = drSetting["存货代码"];
                    ndr["面数"] = 2;
                }
                dt.Rows.Add(ndr);
            }

            da.Update(dt);


            da = new SqlDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            dt产品编码 = dt;

            dgv产品编码.DataSource = dt产品编码;
            //显示序号
            setDataGridViewRowNums(dgv产品编码);
        }

        private void btn吹膜物料代码刷新_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da;
            DataTable dt, dtInSetting;
            SqlCommandBuilder cb;
            da = new SqlDataAdapter("select * from 设置物料代码", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            da.Update(dt);


            string strConnect = "server=" + Parameter.IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            SqlConnection conn;
            conn = new SqlConnection(strConnect);
            conn.Open();
            da = new SqlDataAdapter("select 存货代码 from 设置存货档案 where 类型 like '%组件%' and 属于工序 like '%吹膜%' order by 存货代码", conn);
            dtInSetting = new DataTable();
            da.Fill(dtInSetting);


            da = new SqlDataAdapter("select * from 设置物料代码 ", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow drSetting in dtInSetting.Rows)
            {
                DataRow ndr = dt.NewRow();
                ndr["物料代码"] = drSetting["存货代码"];
                dt.Rows.Add(ndr);
            }

            da.Update(dt);


            da = new SqlDataAdapter("select * from 设置物料代码", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            dt物料代码 = dt;
            dgv物料代码.DataSource = dt物料代码;


            setDataGridViewRowNums(dgv物料代码);
        }
    }
}
