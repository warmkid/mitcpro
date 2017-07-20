using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem.Query
{
    public partial class QueryExtruForm : BaseForm
    {
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        String writer;//操作员
        String Instruction = null;//下拉框获取的生产指令
        int InstruID;//下拉框获取的生产指令ID
        String tableName = null;
        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;  

        public QueryExtruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            Initdgv();

        }

        //下拉框获取生产指令
        public void comboInit()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select ID,生产指令编号 from 生产指令信息表 ";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }
                comm.Dispose();
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select ID,生产指令编号 from 生产指令信息表 ";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }
                comm.Dispose();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instruction = comboBox1.SelectedItem.ToString();
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + Instruction + "'";
            OleDbDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                InstruID = Convert.ToInt32(reader["ID"]);
            }
            comm.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = comboBox2.SelectedItem.ToString();
        }

        //dgv样式初始化
        private void Initdgv()
        {
            bs = new BindingSource();
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);

        }

        private void Bind()
        {            
            try
            {
                switch (tableName)
                {
                    #region case
                    case "批生产记录（吹膜）":
                        dt = new DataTable("批生产记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 批生产记录表 where 汇总人 like " + "'%" + writer + "%'" + " and 开始生产时间 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID")
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;  //没用？？？？？
                        this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;
                        this.dgv.Columns["使用物料"].MinimumWidth = 150;
                        this.dgv.Columns["开始生产时间"].MinimumWidth = 170;
                        this.dgv.Columns["结束生产时间"].MinimumWidth = 170;
                        this.dgv.Columns["汇总人"].MinimumWidth = 100;
                        this.dgv.Columns["批准人"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;

                        break;
                    case "吹膜机组清洁记录":
                        dt = new DataTable("吹膜机组清洁记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where 审核人 like " + "'%" + writer + "%'" + " and 清洁日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["清洁日期"].MinimumWidth = 170;
                        this.dgv.Columns["班次"].MinimumWidth = 100;
                        this.dgv.Columns["审核时间"].MinimumWidth = 170;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;
                        break;

                    case "吹膜岗位交接班记录":
                        dt = new DataTable("吹膜岗位交接班记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
                        this.dgv.Columns["生产日期"].MinimumWidth = 150;
                        this.dgv.Columns["白班产品代码批号数量"].MinimumWidth = 200;
                        this.dgv.Columns["白班交班人"].MinimumWidth = 120;
                        this.dgv.Columns["白班接班人"].MinimumWidth = 120;
                        this.dgv.Columns["夜班产品代码批号数量"].MinimumWidth = 200;
                        this.dgv.Columns["夜班交班人"].MinimumWidth = 120;
                        this.dgv.Columns["夜班接班人"].MinimumWidth = 120;
                        this.dgv.Columns["白班交接班时间"].MinimumWidth = 170;
                        this.dgv.Columns["夜班交接班时间"].MinimumWidth = 170;
                        this.dgv.Columns["白班异常情况处理"].MinimumWidth = 200;
                        this.dgv.Columns["夜班异常情况处理"].MinimumWidth = 200;

                        break;
                    case "吹膜工序清场记录":
                        dt = new DataTable("吹膜工序清场记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where 清场人 like " + "'%" + writer + "%'" + " and 清场日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;
                        this.dgv.Columns["清场前产品代码"].MinimumWidth = 150;
                        this.dgv.Columns["清场前产品批号"].MinimumWidth = 150;
                        this.dgv.Columns["清场日期"].MinimumWidth = 150;
                        this.dgv.Columns["检查结果"].MinimumWidth = 120;
                        this.dgv.Columns["清场人"].MinimumWidth = 100;
                        this.dgv.Columns["检查人"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;

                        break;  
                    case "吹膜供料记录":
                        dt = new DataTable("吹膜供料记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜供料记录 where 审核人 like " + "'%" + writer + "%'" + " and 供料日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
                        this.dgv.Columns["产品代码"].MinimumWidth = 120;
                        this.dgv.Columns["产品批号"].MinimumWidth = 120;
                        this.dgv.Columns["外中内层原料代码"].MinimumWidth = 200;
                        this.dgv.Columns["中层原料代码"].MinimumWidth = 150;
                        this.dgv.Columns["外中内层原料批号"].MinimumWidth = 200;
                        this.dgv.Columns["中层原料批号"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 100;

                        break;
                    case "吹膜工序废品记录":
                        dt = new DataTable("吹膜工序废品记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where 生产指令ID= " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;  
                        this.dgv.Columns["生产开始时间"].MinimumWidth = 170;
                        this.dgv.Columns["生产结束时间"].MinimumWidth = 170;
                        this.dgv.Columns["合计不良品数量"].MinimumWidth = 170;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;

                        break;
                    
                    case "吹膜工序领料退料记录":
                        dt = new DataTable("吹膜工序领料退料记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where 退料操作人 like " + "'%" + writer + "%'" + " and 领料日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["物料代码"].MinimumWidth = 120;
                        this.dgv.Columns["领料日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 100;
                        this.dgv.Columns["重量合计"].MinimumWidth = 100;
                        this.dgv.Columns["数量合计"].MinimumWidth = 100;
                        this.dgv.Columns["退料"].MinimumWidth = 100;
                        this.dgv.Columns["退料操作人"].MinimumWidth = 120;
                        this.dgv.Columns["退料审核人"].MinimumWidth = 120;

                        break;
                    
                    case "吹膜生产日报表":
                        dt = new DataTable("吹膜生产日报表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜生产日报表 where 生产指令ID= " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产数量合计"].MinimumWidth = 150;
                        this.dgv.Columns["生产重量合计"].MinimumWidth = 150;
                        this.dgv.Columns["废品重量合计"].MinimumWidth = 150;
                        this.dgv.Columns["加料A合计"].MinimumWidth = 120;
                        this.dgv.Columns["加料B1C合计"].MinimumWidth = 150;
                        this.dgv.Columns["加料B2合计"].MinimumWidth = 120;
                        this.dgv.Columns["工时合计"].MinimumWidth = 100;
                        this.dgv.Columns["中层B2物料占比"].MinimumWidth = 150;
                        this.dgv.Columns["工时效率"].MinimumWidth = 100;

                        break;
                    case "吹膜工序生产和检验记录":
                        dt = new DataTable("吹膜工序生产和检验记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where 审核人 like " + "'%" + writer + "%'" + " and 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();
                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50; 
                        this.dgv.Columns["产品名称"].MinimumWidth = 200;
                        this.dgv.Columns["产品批号"].MinimumWidth = 100;
                        this.dgv.Columns["生产日期"].MinimumWidth = 170;
                        this.dgv.Columns["依据工艺"].MinimumWidth = 150;
                        this.dgv.Columns["生产设备"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;

                        break;
                    case "吹膜工序物料平衡记录":
                        dt = new DataTable("吹膜工序物料平衡记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where 记录人 like " + "'%" + writer + "%'" + " and 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产日期"].MinimumWidth = 170;
                        this.dgv.Columns["成品重量合计"].MinimumWidth = 150;
                        this.dgv.Columns["废品量合计"].MinimumWidth = 120;
                        this.dgv.Columns["领料量"].MinimumWidth = 100;
                        this.dgv.Columns["重量比成品率"].MinimumWidth = 150;
                        this.dgv.Columns["记录人"].MinimumWidth = 100;
                        this.dgv.Columns["记录日期"].MinimumWidth = 170;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 170;

                        break;
                    case "产品内包装记录":
                        dt = new DataTable("产品内包装记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 产品内包装记录表 where 操作人 like " + "'%" + writer + "%'" + " and 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["产品批号"].MinimumWidth = 100;
                        //this.dgv.Columns["生产日期"].MinimumWidth = 150;
                        this.dgv.Columns["标签语言是否英文"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        this.dgv.Columns["包材名称"].MinimumWidth = 100;
                        this.dgv.Columns["包材批号"].MinimumWidth = 100;
                        this.dgv.Columns["指示剂批号"].MinimumWidth = 120;
                        this.dgv.Columns["包装规格"].MinimumWidth = 100;
                        this.dgv.Columns["总计包数"].MinimumWidth = 100;
                        this.dgv.Columns["操作人"].MinimumWidth = 100;
                        this.dgv.Columns["操作日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 150;

                        break;
                    case "产品外包装记录":
                        dt = new DataTable("产品外包装记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 产品外包装记录表 where 操作人 like " + "'%" + writer + "%'" + " and 操作日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["产品代码"].MinimumWidth = 100;
                        this.dgv.Columns["产品批号"].MinimumWidth = 100;
                        this.dgv.Columns["包装日期"].MinimumWidth = 150;
                        this.dgv.Columns["包装规格每包只数"].MinimumWidth = 170;
                        this.dgv.Columns["包装规格每箱包数"].MinimumWidth = 170;
                        this.dgv.Columns["产品数量箱数"].MinimumWidth = 140;
                        this.dgv.Columns["产品数量只数"].MinimumWidth = 140;
                        this.dgv.Columns["操作人"].MinimumWidth = 100;
                        this.dgv.Columns["操作日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 150;

                        break;
                    case "吹膜机组开机前确认表":
                        dt = new DataTable("吹膜机组开机前确认表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where 确认人 like " + "'%" + writer + "%'" + " and 确认日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["确认人"].MinimumWidth = 100;
                        this.dgv.Columns["确认日期"].MinimumWidth = 150;                       
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "吹膜机组预热参数记录表":
                        dt = new DataTable("吹膜机组预热参数记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where 记录人 like " + "'%" + writer + "%'" + " and 日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令id = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID" || colname == "生产指令id") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
                        this.dgv.Columns["模芯规格参数1"].MinimumWidth = 150;
                        this.dgv.Columns["模芯规格参数2"].MinimumWidth = 150;
                        this.dgv.Columns["预热开始时间"].MinimumWidth = 170;
                        this.dgv.Columns["保温开始时间"].MinimumWidth = 170;
                        //this.dgv.Columns["保温结束时间1"].MinimumWidth = 170;
                        this.dgv.Columns["保温结束时间1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        this.dgv.Columns["保温结束时间2"].MinimumWidth = 170;
                        this.dgv.Columns["保温结束时间3"].MinimumWidth = 170;
                        this.dgv.Columns["记录人"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "吹膜供料系统运行记录":
                        dt = new DataTable("吹膜供料系统运行记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where 审核人 like " + "'%" + writer + "%'" + " and 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
                        this.dgv.Columns["生产日期"].MinimumWidth = 150;
                        this.dgv.Columns["班次"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "吹膜机组运行记录":
                        dt = new DataTable("吹膜机组运行记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where 记录人 like " + "'%" + writer + "%'" + " and 生产日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and 生产指令ID = " + InstruID, mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["产品代码"].MinimumWidth = 150;
                        this.dgv.Columns["产品批号"].MinimumWidth = 150;
                        this.dgv.Columns["生产日期"].MinimumWidth = 150;
                        this.dgv.Columns["记录时间"].MinimumWidth = 150;
                        this.dgv.Columns["记录人"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "培训记录表":
                        dt = new DataTable("吹膜机安全培训记录"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机安全培训记录 where 培训日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["讲师"].MinimumWidth = 100;
                        this.dgv.Columns["培训地点"].MinimumWidth = 300;
                        this.dgv.Columns["培训日期"].MinimumWidth = 170;
                        this.dgv.Columns["备注"].MinimumWidth = 200;                        

                        break;
#endregion
                    case "吹膜机更换模头记录及安装检查表":
                        dt = new DataTable("吹膜机组换模头检查表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组换模头检查表 where 检查人 like " + "'%" + writer + "%'" + " and 更换日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["更换原因"].MinimumWidth = 150;
                        this.dgv.Columns["更换前模头型号"].MinimumWidth = 170;
                        //this.dgv.Columns["更换前模头型号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        this.dgv.Columns["更换日期"].MinimumWidth = 150;
                        this.dgv.Columns["更换后模头型号"].MinimumWidth = 170;
                        this.dgv.Columns["检查人"].MinimumWidth = 100;
                        this.dgv.Columns["检查日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "吹膜机更换模芯记录及安装检查表":
                        dt = new DataTable("吹膜机组换模芯检查表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机组换模芯检查表 where 检查人 like " + "'%" + writer + "%'" + " and 更换日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["更换原因"].MinimumWidth = 150;
                        this.dgv.Columns["更换前模芯型号"].MinimumWidth = 170;
                        this.dgv.Columns["更换日期"].MinimumWidth = 150;
                        this.dgv.Columns["更换后模芯型号"].MinimumWidth = 170;
                        this.dgv.Columns["检查人"].MinimumWidth = 100;
                        this.dgv.Columns["检查日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核日期"].MinimumWidth = 150;
                        this.dgv.Columns["审核是否通过"].MinimumWidth = 150;

                        break;
                    case "吹膜机更换过滤网记录":
                        dt = new DataTable("吹膜机更换过滤网记录表"); //""中的是表名
                        da = new OleDbDataAdapter("select * from 吹膜机更换过滤网记录表 where 更换人 like " + "'%" + writer + "%'" + " and 更换日期 between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
                        cb = new OleDbCommandBuilder(da);
                        dt.Columns.Add("序号", System.Type.GetType("System.String"));
                        da.Fill(dt);
                        bs.DataSource = dt;
                        this.dgv.DataBindings.Clear();
                        this.dgv.DataSource = bs.DataSource; //绑定
                        //显示序号
                        setDataGridViewRowNums();

                        for (int i = 0; i < this.dgv.Columns.Count; i++)
                        {
                            string colname = this.dgv.Columns[i].Name;
                            if (colname == "ID" || colname == "生产指令ID") 
                            { this.dgv.Columns[colname].Visible = false; }
                            else
                            {  this.dgv.Columns[colname].Visible = true; }
                        }
                        this.dgv.Columns["序号"].MinimumWidth = 50;
                        this.dgv.Columns["更换日期"].MinimumWidth = 150;
                        this.dgv.Columns["更换原因"].MinimumWidth = 200;
                        this.dgv.Columns["滤网目数层数"].MinimumWidth = 150;
                        this.dgv.Columns["更换人"].MinimumWidth = 100;
                        this.dgv.Columns["审核人"].MinimumWidth = 100;
                        this.dgv.Columns["审核意见"].MinimumWidth = 150;

                        break;

                    default:
                        break;
                }
            }

            catch
            {
                MessageBox.Show("输入有误，请重新输入", "错误");
                return;
            }

        }

        //填序号列的值
        private void setDataGridViewRowNums()
        {
            int coun = this.dgv.Rows.Count;
            for (int i = 0; i < coun; i++)
            {
                this.dgv.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }




        private void SearchBtn_Click(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.Date;
            date2 = dateTimePicker2.Value.Date;
            writer = textBox1.Text.Trim();

            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            if (this.comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择表单！", "提示");
            }

            Bind();

 
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            int selectIndex = this.dgv.CurrentRow.Index;
            int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
            switch (tableName)
            {
                case "批生产记录（吹膜）":
                    //BatchProductRecord.BatchProductRecord detailform1 = new BatchProductRecord.BatchProductRecord(base.mainform, ID);
                    //detailform1.Show();
                    break;
                case "吹膜机组清洁记录":
                    Record_extrusClean detailform2 = new Record_extrusClean(base.mainform, ID);
                    detailform2.Show();
                    break;
                case "吹膜岗位交接班记录":
                    mySystem.Process.Extruction.A.HandOver detailform3 = new Process.Extruction.A.HandOver(base.mainform, ID);
                    detailform3.Show();
                    break;
                case "吹膜工序清场记录":
                    Record_extrusSiteClean detailform4 = new Record_extrusSiteClean(base.mainform, ID);
                    detailform4.Show();
                    break;
                case "吹膜供料记录":
                    Record_extrusSupply detailform5 = new Record_extrusSupply(base.mainform, ID);
                    detailform5.Show();
                    break;
                case "吹膜工序废品记录":
                    mySystem.Process.Extruction.B.Waste detailform6 = new Process.Extruction.B.Waste(base.mainform, ID);
                    detailform6.Show();
                    break;
                case "吹膜工序领料退料记录":
                    Record_material_reqanddisg detailform7 = new Record_material_reqanddisg(base.mainform, ID);
                    detailform7.Show();
                    break;
                case "吹膜生产日报表":
                    ProdctDaily_extrus detailform8 = new ProdctDaily_extrus(base.mainform, ID);
                    detailform8.Show();
                    break;
                case "吹膜工序生产和检验记录":                   
                    ExtructionpRoductionAndRestRecordStep6 detailform9 = new ExtructionpRoductionAndRestRecordStep6(base.mainform, ID);
                    detailform9.Show();
                    break;
                case "吹膜工序物料平衡记录":
                    MaterialBalenceofExtrusionProcess detailform10 = new MaterialBalenceofExtrusionProcess(base.mainform, ID);
                    detailform10.Show();
                    break;
                case "产品内包装记录":
                    ProductInnerPackagingRecord detailform11 = new ProductInnerPackagingRecord(base.mainform, ID);
                    detailform11.Show();
                    break;
                case "产品外包装记录":
                    Extruction.Chart.outerpack detailform12 = new Extruction.Chart.outerpack(base.mainform, ID);
                    detailform12.Show();
                    break;
                case "吹膜机组开机前确认表":
                    ExtructionCheckBeforePowerStep2 detailform13 = new ExtructionCheckBeforePowerStep2(base.mainform, ID);
                    detailform13.Show();
                    break;
                case "吹膜机组预热参数记录表":
                    ExtructionPreheatParameterRecordStep3 detailform14 = new ExtructionPreheatParameterRecordStep3(base.mainform, ID);
                    detailform14.Show();
                    break;
                case "吹膜供料系统运行记录":
                    mySystem.Process.Extruction.C.Feed detailform15 = new Process.Extruction.C.Feed(base.mainform, ID);
                    detailform15.Show();
                    break;
                case "吹膜机组运行记录":
                    mySystem.Process.Extruction.B.Running detailform16 = new Process.Extruction.B.Running(base.mainform, ID);
                    detailform16.Show();
                    break;
                case "培训记录表":
                    //Record_train detailform17 = new Record_train(base.mainform, ID);
                    //detailform17.Show();
                    break;
                case "吹膜机更换模头记录及安装检查表":
                    ReplaceHeadForm detailform18 = new ReplaceHeadForm(base.mainform, ID);
                    detailform18.Show();
                    break;
                case "吹膜机更换模芯记录及安装检查表":
                    ExtructionReplaceCore detailform19 = new ExtructionReplaceCore(base.mainform, ID);
                    detailform19.Show();
                    break;
                case "吹膜机更换过滤网记录":
                    //Process.Extruction.D.NetExchange detailform20 = new Process.Extruction.D.NetExchange(base.mainform, ID);
                    //detailform20.Show();
                    break;

                default:
                    break;


            }
        }

        

        


    }
}
