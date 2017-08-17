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
                comm.CommandText = "select * from 生产指令信息表 ";
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
                comm.CommandText = "select * from 生产指令信息表 ";
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
            dgv.AllowUserToResizeRows = true;
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
                    case "00 批生产记录（吹膜）":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "批生产记录表", "汇总人", "开始生产时间", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "批生产记录表", "汇总人", "开始生产时间", null); }
                        break;
                    case "03 吹膜机组清洁记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜机组清洁记录表", "审核人", "清洁日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜机组清洁记录表", "审核人", "清洁日期", null); }
                        break;
                    case "14 吹膜岗位交接班记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜岗位交接班记录", null, "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜岗位交接班记录", null, "生产日期", null); }
                        break;
                    case "11 吹膜工序清场记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜工序清场记录", "清场人", "清场日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序清场记录", "清场人", "清场日期", null); }                       
                        break;
                    case "06 吹膜供料记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜供料记录", "审核人", "供料日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜供料记录", "审核人", "供料日期", null); }
                        break;
                    case "10 吹膜工序废品记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜工序废品记录", null, null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序废品记录", null, null, null); }
                        break;
                    case "13 吹膜工序领料退料记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜工序领料退料记录", "退料操作人", "领料日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序领料退料记录", "退料操作人", "领料日期", null); }
                        break;
                    case "02 吹膜生产日报表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜生产日报表", null, null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜生产日报表", null, null, null); }
                        break;
                    case "09 吹膜工序生产和检验记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜工序生产和检验记录", "审核人", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序生产和检验记录", "审核人", "生产日期", null); }
                        break;
                    case "12 吹膜工序物料平衡记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜工序物料平衡记录", "记录员", "记录日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序物料平衡记录", "记录员", "记录日期", null); }
                        break;
                    case "15 产品内包装记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品内包装记录表", "操作人", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品内包装记录表", "操作人", "生产日期", null); }
                        break;
                    case "16 产品外包装记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "产品外包装记录表", "操作人", "操作日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品外包装记录表", "操作人", "操作日期", null); }
                        break;
                    case "04 吹膜机组开机前确认表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜机组开机前确认表", "确认人", "确认日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜机组开机前确认表", "确认人", "确认日期", null); }
                        break;
                    case "05 吹膜机组预热参数记录表":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜机组预热参数记录表", "操作员", "日期", "生产指令id"); }
                        else
                        { EachBind(this.dgv, "吹膜机组预热参数记录表", "操作员", "日期", null); }
                        break;
                    case "07 吹膜供料系统运行记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜供料系统运行记录", "审核员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜供料系统运行记录", "审核员", "生产日期", null); }
                        break;
                    case "08 吹膜机组运行记录":
                        if (comboBox1.SelectedIndex != -1)
                        { EachBind(this.dgv, "吹膜机组运行记录", "记录员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜机组运行记录", "记录员", "生产日期", null); }
                        break;
                    case "培训记录表":
                        EachBind(this.dgv, "吹膜机安全培训记录", "培训日期", null, null);
                        
                        break;
                    case "吹膜机更换模头记录及安装检查表":
                        EachBind(this.dgv, "吹膜机组换模头检查表", "检查人", "更换日期", null);
                        
                        break;
                    case "吹膜机更换模芯记录及安装检查表":
                        EachBind(this.dgv, "吹膜机组换模芯检查表", "检查人", "更换日期", null);
                        
                        break;
                    case "吹膜机更换过滤网记录":
                        EachBind(this.dgv, "吹膜机更换过滤网记录表", "更换人", "更换日期", null);
                        
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

        // 各表查询
        private void EachBind(DataGridView dgv, String tblName, String person, String startDate, String instruID)
        {
            dt = new DataTable(tblName); //""中的是表名
            if (person != null && startDate != null && instruID == null) // 人 + 日期
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            else if (person == null && startDate != null && instruID != null) // 日期 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person != null && startDate == null && instruID != null) // 人 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person != null && startDate == null && instruID == null) // 人 
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'", mySystem.Parameter.connOle);
            else if (person == null && startDate != null && instruID == null) // 日期
                da = new OleDbDataAdapter("select * from " + tblName + " where " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            else if (person == null && startDate == null && instruID != null) // 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + instruID + " = " + InstruID, mySystem.Parameter.connOle);
            else if (person == null && startDate == null && instruID == null) // 只有表名
                da = new OleDbDataAdapter("select * from " + tblName, mySystem.Parameter.connOle);
            else if (person != null && startDate != null && instruID != null) // 人 + 日期 + 生产指令
                da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#" + " and " + instruID + " = " + InstruID, mySystem.Parameter.connOle);

            cb = new OleDbCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            dgv.DataBindings.Clear();
            dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();
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
                return;
            }

            Bind();
            dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
 
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int selectIndex = this.dgv.CurrentRow.Index;
                int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                switch (tableName)
                {
                    case "00 批生产记录（吹膜）":
                        //BatchProductRecord.BatchProductRecord detailform1 = new BatchProductRecord.BatchProductRecord(base.mainform, ID);
                        //detailform1.Show();
                        break;
                    case "03 吹膜机组清洁记录":
                        Record_extrusClean detailform2 = new Record_extrusClean(base.mainform, ID);
                        detailform2.Owner = this;
                        detailform2.ShowDialog();
                        break;
                    case "14 吹膜岗位交接班记录":
                        mySystem.Process.Extruction.A.HandOver detailform3 = new Process.Extruction.A.HandOver(base.mainform, ID);
                        detailform3.Owner = this;
                        detailform3.ShowDialog();
                        break;
                    case "11 吹膜工序清场记录":
                        Record_extrusSiteClean detailform4 = new Record_extrusSiteClean(base.mainform, ID);
                        detailform4.Owner = this;
                        detailform4.ShowDialog();
                        break;
                    case "06 吹膜供料记录":
                        Record_extrusSupply detailform5 = new Record_extrusSupply(base.mainform, ID);
                        detailform5.Owner = this;
                        detailform5.ShowDialog();
                        break;
                    case "10 吹膜工序废品记录":
                        mySystem.Process.Extruction.B.Waste detailform6 = new Process.Extruction.B.Waste(base.mainform, ID);
                        detailform6.Owner = this;
                        detailform6.ShowDialog();
                        break;
                    case "13 吹膜工序领料退料记录":
                        Record_material_reqanddisg detailform7 = new Record_material_reqanddisg(base.mainform, ID);
                        detailform7.Owner = this;
                        detailform7.Show();
                        break;
                    case "02 吹膜生产日报表":
                        ProdctDaily_extrus detailform8 = new ProdctDaily_extrus(base.mainform, ID);
                        detailform8.Owner = this;
                        detailform8.ShowDialog();
                        break;
                    case "09 吹膜工序生产和检验记录":
                        ExtructionpRoductionAndRestRecordStep6 detailform9 = new ExtructionpRoductionAndRestRecordStep6(base.mainform, ID);
                        detailform9.Owner = this;
                        detailform9.ShowDialog();
                        break;
                    case "12 吹膜工序物料平衡记录":
                        MaterialBalenceofExtrusionProcess detailform10 = new MaterialBalenceofExtrusionProcess(base.mainform, ID);
                        detailform10.Owner = this;
                        detailform10.ShowDialog();
                        break;
                    case "15 产品内包装记录":
                        ProductInnerPackagingRecord detailform11 = new ProductInnerPackagingRecord(base.mainform, ID);
                        detailform11.Owner = this;
                        detailform11.ShowDialog();
                        break;
                    case "16 产品外包装记录":
                        Extruction.Chart.outerpack detailform12 = new Extruction.Chart.outerpack(base.mainform, ID);
                        detailform12.Owner = this;
                        detailform12.ShowDialog();
                        break;
                    case "04 吹膜机组开机前确认表":
                        ExtructionCheckBeforePowerStep2 detailform13 = new ExtructionCheckBeforePowerStep2(base.mainform, ID);
                        detailform13.Owner = this;
                        detailform13.ShowDialog();
                        break;
                    case "05 吹膜机组预热参数记录表":
                        ExtructionPreheatParameterRecordStep3 detailform14 = new ExtructionPreheatParameterRecordStep3(base.mainform, ID);
                        detailform14.Owner = this;
                        detailform14.ShowDialog();
                        break;
                    case "07 吹膜供料系统运行记录":
                        mySystem.Process.Extruction.C.Feed detailform15 = new Process.Extruction.C.Feed(base.mainform, ID);
                        detailform15.Owner = this;
                        detailform15.ShowDialog();
                        break;
                    case "08 吹膜机组运行记录":
                        mySystem.Process.Extruction.B.Running detailform16 = new Process.Extruction.B.Running(base.mainform, ID);
                        detailform16.Owner = this;
                        detailform16.ShowDialog();
                        break;
                    case "培训记录表":
                        //Record_train detailform17 = new Record_train(base.mainform, ID);
                        //detailform17.Show();
                        break;
                    case "吹膜机更换模头记录及安装检查表":
                        ReplaceHeadForm detailform18 = new ReplaceHeadForm(base.mainform, ID);
                        detailform18.Owner = this;
                        detailform18.ShowDialog();
                        break;
                    case "吹膜机更换模芯记录及安装检查表":
                        ExtructionReplaceCore detailform19 = new ExtructionReplaceCore(base.mainform, ID);
                        detailform19.Owner = this;
                        detailform19.ShowDialog();
                        break;
                    case "吹膜机更换过滤网记录":
                        //Process.Extruction.D.NetExchange detailform20 = new Process.Extruction.D.NetExchange(base.mainform, ID);
                        //detailform20.Show();
                        break;

                    default:
                        break;
                }
            }
            catch(Exception ee)
            { MessageBox.Show(ee.Message + "\n" + ee.StackTrace); }
        }


        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //设置列宽
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                String colName = dgv.Columns[i].HeaderText;
                int strlen = colName.Length;
                dgv.Columns[i].MinimumWidth = strlen * 25;
            }

            dgv.Columns["ID"].Visible = false;
            try
            { dgv.Columns["生产指令ID"].Visible = false; }
            catch
            {  }
            try
            { setDataGridViewBackColor("审核人"); }
            catch
            { }
            try
            { setDataGridViewBackColor("审核员"); }
            catch
            { }
            if (tableName == "吹膜岗位交接班记录")
            {
                setDataGridViewBackColor("白班接班员");
                setDataGridViewBackColor("夜班接班员");
            }
        }

        //设置datagridview背景颜色，待审核标红
        private void setDataGridViewBackColor(String checker)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[checker].Value.ToString() == "__待审核")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255,127,0);
                }
                else if (dgv.Rows[i].Cells[checker].Value.ToString() == "")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
            }
        }

        


    }
}
