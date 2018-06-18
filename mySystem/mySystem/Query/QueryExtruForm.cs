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
using System.Data.SqlClient;
using mySystem.Process.Extruction;

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
        private SqlDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private SqlCommandBuilder cb;  

        public QueryExtruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            comboInit(); //从数据库中读取生产指令
            Initdgv();

            textBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox1_PreviewKeyDown);
            comboBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox1_PreviewKeyDown);
            comboBox2.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox2_PreviewKeyDown);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            dgv.DataError += dgv_DataError;
            if (Parameter.c查询_吹膜_表单 != "")
            {
                comboBox2.Text = Parameter.c查询_吹膜_表单;
            }
            if (Parameter.c查询_吹膜_生产指令 != "")
            {
                comboBox1.Text = Parameter.c查询_吹膜_生产指令;
            }
            if (3 == mySystem.Parameter.userRole)
            {
                // 3是管理员
                btn删除.Visible = true;
                btn退回审核.Visible = true;
            }
        }

        void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            System.Console.WriteLine(dgv.CurrentCell.OwningColumn.Name);
            System.Console.WriteLine(dgv.CurrentCell.OwningRow.Index);
            System.Console.WriteLine("*******");
        }

        void comboBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        //下拉框获取生产指令
        public void comboInit()
        {
            if (!Parameter.isSqlOk)
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = mySystem.Parameter.conn;
                comm.CommandText = "select * from 生产指令信息表 where 状态<>3";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
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
                comm.CommandText = "select * from 生产指令信息表 where 状态<>3";
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
            Parameter.c查询_吹膜_生产指令 = comboBox1.Text;
            Instruction = comboBox1.SelectedItem.ToString();
            SqlCommand comm = new SqlCommand();
            comm.Connection = mySystem.Parameter.conn;
            comm.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + Instruction + "'";
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                InstruID = Convert.ToInt32(reader["ID"]);
            }
            comm.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameter.c查询_吹膜_表单 = comboBox2.Text;
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
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "批生产记录表", "汇总人", "开始生产时间", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "批生产记录表", "汇总人", "开始生产时间", null); }
                        break;
                    case "03 吹膜机组清洁记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜机组清洁记录表", "审核人", "清洁日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜机组清洁记录表", "审核人", "清洁日期", null); }
                        break;
                    case "14 吹膜岗位交接班记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜岗位交接班记录", null, "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜岗位交接班记录", null, "生产日期", null); }
                        break;
                    case "11 吹膜工序清场记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜工序清场记录", "清场人", "清场日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序清场记录", "清场人", "清场日期", null); }                       
                        break;
                    case "06 吹膜供料记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜供料记录", "审核人", "供料日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜供料记录", "审核人", "供料日期", null); }
                        break;
                    case "10 吹膜工序废品记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜工序废品记录", null, null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序废品记录", null, null, null); }
                        break;
                    case "13 吹膜工序领料退料记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜工序领料退料记录", "退料操作人", "领料日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序领料退料记录", "退料操作人", "领料日期", null); }
                        break;
                    case "02 吹膜生产日报表":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜生产日报表", null, null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜生产日报表", null, null, null); }
                        break;
                    case "09 吹膜工序生产和检验记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜工序生产和检验记录", "审核人", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序生产和检验记录", "审核人", "生产日期", null); }
                        break;
                    case "12 吹膜工序物料平衡记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜工序物料平衡记录", "记录员", "记录日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜工序物料平衡记录", "记录员", "记录日期", null); }
                        break;
                    case "15 产品内包装记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "产品内包装记录表", "操作人", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品内包装记录表", "操作人", "生产日期", null); }
                        break;
                    case "16 产品外包装记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "产品外包装记录表", "操作人", "操作日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "产品外包装记录表", "操作人", "操作日期", null); }
                        break;
                    case "04 吹膜机组开机前确认表":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜机组开机前确认表", "确认人", "确认日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜机组开机前确认表", "确认人", "确认日期", null); }
                        break;
                    case "05 吹膜机组预热参数记录表":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜机组预热参数记录表", "操作员", "日期", "生产指令id"); }
                        else
                        { EachBind(this.dgv, "吹膜机组预热参数记录表", "操作员", "日期", null); }
                        break;
                    case "07 吹膜供料系统运行记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "吹膜供料系统运行记录", "审核员", "生产日期", "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "吹膜供料系统运行记录", "审核员", "生产日期", null); }
                        break;
                    case "08 吹膜机组运行记录":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
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
                    case "生产领料申请单":
                        if (comboBox1.SelectedIndex != -1 || comboBox1.Text.Trim() != "")
                        { EachBind(this.dgv, "生产领料申请单表", "审核员", null, "生产指令ID"); }
                        else
                        { EachBind(this.dgv, "生产领料申请单表", "审核员", null, null); }
                        break;

                    default:
                        break;
                }
            }

            catch (Exception ee)
            {
                MessageBox.Show("输入有误，请重新输入" + ee.Message + "\n" + ee.StackTrace, "错误");
                return;
            }
            Utility.setDataGridViewAutoSizeMode(dgv);
        }

        // 各表查询
        private void EachBind(DataGridView dgv, String tblName, String person, String startDate, String instruID)
        {
            dt = new DataTable(tblName); //""中的是表名
            if (person != null && startDate != null && instruID == null) // 人 + 日期
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'", mySystem.Parameter.conn);
            else if (person == null && startDate != null && instruID != null) // 日期 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person != null && startDate == null && instruID != null) // 人 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person != null && startDate == null && instruID == null) // 人 
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'", mySystem.Parameter.conn);
            else if (person == null && startDate != null && instruID == null) // 日期
                da = new SqlDataAdapter("select * from " + tblName + " where " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'", mySystem.Parameter.conn);
            else if (person == null && startDate == null && instruID != null) // 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + instruID + " = " + InstruID, mySystem.Parameter.conn);
            else if (person == null && startDate == null && instruID == null) // 只有表名
                da = new SqlDataAdapter("select * from " + tblName, mySystem.Parameter.conn);
            else if (person != null && startDate != null && instruID != null) // 人 + 日期 + 生产指令
                da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + startDate + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "'" + " and " + instruID + " = " + InstruID, mySystem.Parameter.conn);

            cb = new SqlCommandBuilder(da);
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
            // TODO 517: 根据选择的表读取列宽；加一个dgv_column_wdth_change时间，只要有变化就保存列宽
            search();
            string tbl = get表名fromCombobox(comboBox2.Text);
            readDGVWidthFromSettingAndSet(dgv, tbl);
        }

        public void search()
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
            if (dgv.Rows.Count > 0)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
 
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    int selectIndex = this.dgv.CurrentRow.Index;
                    int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                    bool b;
                    switch (tableName)
                    {

                        case "00 批生产记录（吹膜）":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "批生产记录表");
                            if (b)
                            {
                                BatchProductRecord.BatchProductRecord form1 = new BatchProductRecord.BatchProductRecord(mainform,ID);
                                form1.Owner = this;
                                form1.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "03 吹膜机组清洁记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜机组清洁记录表");
                           if (b)
                           {
                               Record_extrusClean form3 = new Record_extrusClean(mainform, ID);
                               form3.Owner = this;
                               form3.ShowDialog();
                           }
                           else
                           {
                               MessageBox.Show("您无权查看该页面！");
                               return;
                           }
                            break;
                        case "14 吹膜岗位交接班记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜岗位交接班记录");
                            if (b)
                            {
                                mySystem.Process.Extruction.A.HandOver form5 = new mySystem.Process.Extruction.A.HandOver(mainform, ID);
                                form5.Owner = this;
                                form5.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                           break;
                        case "11 吹膜工序清场记录":
                           b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜工序清场记录");
                            if (b)
                            {
                                Record_extrusSiteClean form4 = new Record_extrusSiteClean(mainform, ID);
                                form4.Owner = this;
                                form4.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "06 吹膜供料记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜供料记录");
                           if (b)
                           {
                               Record_extrusSupply form6 = new Record_extrusSupply(mainform, ID);
                               form6.Owner = this;
                               form6.ShowDialog();
                           }
                           else
                           {
                               MessageBox.Show("您无权查看该页面！");
                               return;
                           }
                            break;
                        case "10 吹膜工序废品记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜工序废品记录");
                            if (b)
                            {
                                mySystem.Process.Extruction.B.Waste form7 = new mySystem.Process.Extruction.B.Waste(mainform, ID);
                                form7.Owner = this;
                                form7.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "13 吹膜工序领料退料记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜工序领料退料记录");
                            if (b)
                            {
                                Record_material_reqanddisg form8 = new Record_material_reqanddisg(mainform, ID);
                                form8.Owner = this;
                                form8.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "02 吹膜生产日报表":
                            ProdctDaily_extrus detailform8 = new ProdctDaily_extrus(base.mainform, ID);
                            detailform8.Owner = this;
                            detailform8.ShowDialog();
                            break;
                        case "09 吹膜工序生产和检验记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜生产和检验记录表");
                            if (b)
                            {
                                ExtructionpRoductionAndRestRecordStep6 form10 = new ExtructionpRoductionAndRestRecordStep6(mainform, ID);
                                form10.Owner = this;
                                form10.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "12 吹膜工序物料平衡记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜工序物料平衡记录");
                            if (b)
                            {
                                MaterialBalenceofExtrusionProcess form11 = new MaterialBalenceofExtrusionProcess(mainform, ID);
                                form11.Owner = this;
                                form11.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "15 产品内包装记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜产品内包装记录表");
                            if (b)
                            {
                                ProductInnerPackagingRecord form12 = new ProductInnerPackagingRecord(mainform, ID);
                                form12.Owner = this;
                                form12.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "16 产品外包装记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜产品外包装记录表");
                           if (b)
                           {
                               Extruction.Chart.outerpack form13 = new Extruction.Chart.outerpack(mainform, ID);
                               form13.Owner = this;
                               form13.ShowDialog();
                           }
                           else
                           {
                               MessageBox.Show("您无权查看该页面！");
                               return;
                           }
                            break;
                        case "04 吹膜机组开机前确认表":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜开机前确认表");
                            if (b)
                            {
                                ExtructionCheckBeforePowerStep2 form14 = new ExtructionCheckBeforePowerStep2(mainform, ID);
                                form14.Owner = this;
                                form14.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "05 吹膜机组预热参数记录表":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜预热参数记录表");
                            if (b)
                            {
                                ExtructionPreheatParameterRecordStep3 form15 = new ExtructionPreheatParameterRecordStep3(mainform, ID);
                                form15.Owner = this;
                                form15.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "07 吹膜供料系统运行记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜供料系统运行记录");
                            if (b)
                            {
                                mySystem.Process.Extruction.C.Feed form16 = new mySystem.Process.Extruction.C.Feed(mainform, ID);
                                form16.Owner = this;
                                form16.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }
                            break;
                        case "08 吹膜机组运行记录":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "吹膜机组运行记录");
                           if (b)
                           {
                               mySystem.Process.Extruction.B.Running form17 = new mySystem.Process.Extruction.B.Running(mainform, ID);
                               form17.Owner = this;
                               form17.ShowDialog();
                           }
                           else
                           {
                               MessageBox.Show("您无权查看该页面！");
                               return;
                           }
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
                        case "生产领料申请单":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "生产领料申请单表");
                            if (b)
                            {
                                吹膜生产领料申请单 form生产领料申请单 = new 吹膜生产领料申请单(mainform, ID, this);
                                form生产领料申请单.Owner = this;
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ee)
                { MessageBox.Show(ee.Message + "\n" + ee.StackTrace); }
            }
        }


        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //设置列宽
            //for (int i = 0; i < dgv.Columns.Count; i++)
            //{
            //    String colName = dgv.Columns[i].HeaderText;
            //    int strlen = colName.Length;
            //    dgv.Columns[i].MinimumWidth = strlen * 25;
            //}

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

        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count <= 0) return;
            if (DialogResult.OK == MessageBox.Show("确认删除吗？此操作不可撤销", "提示", MessageBoxButtons.OKCancel))
            {
                // get id of current selected row
                int id = Convert.ToInt32(dgv.SelectedCells[0].OwningRow.Cells["ID"].Value);
                // delete 
                dt.Select("ID=" + id)[0].Delete();
                da.Update(dt);
                SearchBtn.PerformClick();
            }
            
        }

        private void btn退回审核_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count <= 0) return;
            if (DialogResult.OK == MessageBox.Show("确认撤销审核吗？", "提示", MessageBoxButtons.OKCancel))
            {
                // get id of current selected row
                int id = Convert.ToInt32(dgv.SelectedCells[0].OwningRow.Cells["ID"].Value);
                string colname = "";
                foreach (DataGridViewColumn dgvc in dgv.Columns)
                {
                    if (dgvc.Name == "审核员" || dgvc.Name == "审核人")
                    {
                        colname = dgvc.Name;
                        break;
                    }
                }
                string 审核员;
                try
                {
                    审核员 = dgv.SelectedCells[0].OwningRow.Cells[colname].Value.ToString();
                    if (审核员 == "") return;
                    else
                    {
                        dt.Select("ID=" + id)[0][colname] = "";
                        da.Update(dt);
                        SearchBtn.PerformClick();


                        string 表名 = "";
                        表名 = get表名fromCombobox(comboBox2.Text);
                        if (表名 == "吹膜工序物料平衡记录" || 表名 == "吹膜生产日报表") return;

                        string sql = @"select * from 待审核 where 表名='{0}' and 对应ID={1}";
                        DataTable dt_temp = new DataTable(); ;
                        SqlDataAdapter da_temp = new SqlDataAdapter(String.Format(sql,表名,id), mySystem.Parameter.conn);
                        SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                        da_temp.Fill(dt_temp);

                        if (dt_temp.Rows.Count == 0) return;
                        dt_temp.Rows[0].Delete();
                        da_temp.Update(dt_temp);


                    }
                }
                catch
                {
                    return;
                }
                

               
            }
        }


        string get表名fromCombobox(String comboText)
        {
            string 表名 = "";
            switch (comboText)
            {
                case "00 批生产记录（吹膜）":
                    表名 = "批生产记录表";
                    break;
                case "02 吹膜生产日报表":
                    return "吹膜生产日报表";
                case "03 吹膜机组清洁记录":
                    表名 = "吹膜机组清洁记录表";
                    break;
                case "04 吹膜机组开机前确认表":
                    表名 = "吹膜开机前确认表";
                    break;
                case "05 吹膜机组预热参数记录表":
                    表名 = "吹膜预热参数记录表";
                    break;
                case "06 吹膜供料记录":
                    表名 = "吹膜供料记录";
                    break;
                case "07 吹膜供料系统运行记录":
                    表名 = "吹膜供料系统运行记录";
                    break;
                case "08 吹膜机组运行记录":
                    表名 = "吹膜机组运行记录";
                    break;
                case "09 吹膜工序生产和检验记录":
                    表名 = "吹膜生产和检验记录表";
                    break;
                case "10 吹膜工序废品记录":
                    表名 = "吹膜工序废品记录";
                    break;
                case "11 吹膜工序清场记录":
                    表名 = "吹膜工序清场记录";
                    break;
                case "12 吹膜工序物料平衡记录":
                    return "吹膜工序物料平衡记录";
                case "13 吹膜工序领料退料记录":
                    表名 = "吹膜工序领料退料记录";
                    break;
                case "14 吹膜岗位交接班记录":
                    表名 = "吹膜岗位交接班记录";
                    break;
                case "15 产品内包装记录":
                    表名 = "吹膜产品内包装记录表";
                    break;
                case "16 产品外包装记录":
                    表名 = "吹膜产品外包装记录表";
                    break;
                case "生产领料申请单":
                    表名 = "生产领料申请单表";
                    break;

            }
            return 表名;
        }

        private void dgv_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            string tbl = get表名fromCombobox(comboBox2.Text);
            writeDGVWidthToSetting(dgv, tbl);
        }

    }
}
