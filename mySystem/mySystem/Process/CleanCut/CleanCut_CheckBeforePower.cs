using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_CheckBeforePower : BaseForm
    {
        private String table = "清洁分切开机确认";
        private String tableInfo = "清洁分切开机确认详细信息";
        private String tableSet = "设置开机确认项目";

        //private DbConnection conn = null;
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;
        
        private DataTable dtusers, dt设置, dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private string person_操作员;
        private string person_审核员;
        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        //数据库： 班次改为长文本 （不改不影响）
        //待审核的ID应为自增数字，报错：由于其 Required 属性设置为真(True),字段 '待审核.ID' 不能包含 Null 值.
        //判断当登录人是操作员时，写入数据，如果dt记录.Rows.Count <= 0 && stat_user != 0 不填写任何数据，数据库为空
        //班次有必要吗？班次要怎么新建？！！！！！班次可以修改吗？生产日期有必要吗？
        //要不要都添加“查询/插入”按钮？

        public CleanCut_CheckBeforePower(MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            cb白班.Checked = Parameter.userflight == "白班" ? true : false; //生产班次的初始化？？？？？
            cb夜班.Checked = !cb白班.Checked;

            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            dtp生产日期.Enabled = true;
            bt查询新建.Enabled = true;
            cb白班.Enabled = true;
            cb夜班.Enabled = true;
        }

        //******************************初始化******************************//
        
        // 获取操作员和审核员
        private void getPeople()
        {
            dtusers = new DataTable("用户权限");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from 用户权限 where 步骤 = '清洁分切开机确认'" , connOle);
            datemp.Fill(dtusers);
            datemp.Dispose();
            if (dtusers.Rows.Count > 0)
            {
                person_操作员 = dtusers.Rows[0]["操作员"].ToString();
                person_审核员 = dtusers.Rows[0]["审核员"].ToString();
            }
        }

        // 根据登录人，设置stat_user
        private void setUserState()
        {
            if (mySystem.Parameter.userName == person_操作员)
                stat_user = 0;
            else if (mySystem.Parameter.userName == person_审核员)
                stat_user = 1;
            else
                stat_user = 2;
        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setFormState()
        {
            if (dt记录.Rows[0]["审核人"].ToString() == "")
                stat_form = 0;
            else if (dt记录.Rows[0]["审核人"].ToString() == "__待审核")
                stat_form = 1;
            else if ((bool)dt记录.Rows[0]["审核是否通过"])
                stat_form = 2;
            else
                stat_form = 3;
        }

        //读取设置内容  //GetSettingInfo()
        private void getOtherData()        
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();
        }
        
        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (stat_user == 2)//管理员
            {
                //控件都能点
                foreach (Control c in this.Controls)
                    c.Enabled = true;
                dataGridView1.ReadOnly = false;
            }
            else if (stat_user == 1)//审核人
            {
                if (stat_form == 0 || stat_form == 3 || stat_form == 2)  //0未保存||2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    foreach (Control c in this.Controls)
                        c.Enabled = false;
                    dataGridView1.Enabled = true;
                    dataGridView1.ReadOnly = true;
                    bt日志.Enabled = true;
                    bt打印.Enabled = true;
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    foreach (Control c in this.Controls)
                        c.Enabled = true;
                    dataGridView1.ReadOnly = false;
                    bt发送审核.Enabled = false;
                }
            }
            else//操作员
            {
                if (stat_form == 1 || stat_form == 2 || stat_form == 3) //1待审核||2审核通过||3审核未通过
                {
                    //控件都不能点
                    foreach (Control c in this.Controls)
                        c.Enabled = false;
                    dataGridView1.Enabled = true;
                    dataGridView1.ReadOnly = true;
                    bt日志.Enabled = true;
                    bt打印.Enabled = true;
                }
                else //0未保存
                {
                    //发送审核，审核，打印不能点
                    foreach (Control c in this.Controls)
                        c.Enabled = true;
                    dataGridView1.ReadOnly = false;
                    bt发送审核.Enabled = false;
                    bt审核.Enabled = false;
                    bt打印.Enabled = false;
                }
            }
            //查询条件始终可编辑
            dtp生产日期.Enabled = true;
            bt查询新建.Enabled = true;
            cb白班.Enabled = true;
            cb夜班.Enabled = true;
            //生产指令编码不可改
            tb生产指令编号.Enabled = false;
            //包含序号不可编辑
            setDataGridViewFormat(); 
        }

        // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
        private void addOtherEvnetHandler()
        {
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }
        
        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }
        
        // 设置自动计算类事件
        private void addComputerEventHandler() { }

        //******************************显示数据******************************//

        //根据信息查找显示
        private void DataShow(Int32 InstruID, DateTime searchTime, Boolean flight)
        {
            //******************************外表 根据条件绑定******************************// 
            readOuterData(InstruID, searchTime, flight);
            outerBind();

            if (dt记录.Rows.Count <= 0 && stat_user != 0)
            {                
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                //MessageBox.Show("什么也没有");

                //查询条件始终可编辑
                bt查询新建.Enabled = true;
                cb白班.Enabled = true;
                cb夜班.Enabled = true;
                return;
            }

            if (dt记录.Rows.Count <= 0)
            {
                //********* 外表新建、保存、重新绑定 *********//                
                //初始化外表这一行
                DataRow dr1 = dt记录.NewRow();
                dr1 = writeOuterDefault(dr1);
                dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);
                //立马保存这一行
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                //外表重新绑定
                readOuterData(InstruID, searchTime, flight);
                outerBind();

                //********* 内表新建、保存 *********//

                //内表绑定
                readInnerData((int)dt记录.Rows[0]["ID"]);
                innerBind();
                dt记录详情.Rows.Clear();
                dt记录详情 = writeInnerDefault((int)dt记录.Rows[0]["ID"], dt记录详情);
                setDataGridViewRowNums();
                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                //内表重新绑定                
            }
            dataGridView1.Columns.Clear();
            readInnerData((int)dt记录.Rows[0]["ID"]);
            setDataGridViewColumns();
            innerBind();

            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  
            setDataGridViewFormat(); //包含序号不可编辑
        }
        
        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, DateTime searchTime, Boolean flight)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# and 生产班次 = " + flight.ToString(), connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //绑定（先解除绑定）
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            cb白班.DataBindings.Clear();
            cb白班.DataBindings.Add("Checked", bs记录.DataSource, "生产班次");

            tb车间湿度.DataBindings.Clear();
            tb车间湿度.DataBindings.Add("Text", bs记录.DataSource, "车间湿度");
            tb车间温度.DataBindings.Clear();
            tb车间温度.DataBindings.Add("Text", bs记录.DataSource, "车间温度");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");

            tb确认人.DataBindings.Clear();
            tb确认人.DataBindings.Add("Text", bs记录.DataSource, "确认人");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp确认日期.DataBindings.Clear();
            dtp确认日期.DataBindings.Add("Text", bs记录.DataSource, "确认日期");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
        }

        //添加外表默认信息        
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            dr["生产指令编号"] = mySystem.Parameter.cleancutInstruction;
            dr["生产日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["生产班次"] = cb白班.Checked;
            dr["确认人"] = mySystem.Parameter.userName;
            dr["确认日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            //读取记录表里的记录
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T清洁分切开机确认ID = " + ID.ToString(), connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
        }

        //添加行代码，从设置表里读取
        private DataTable writeInnerDefault(Int32 ID, DataTable dt)
        {
            for (int i = 0; i < dt设置.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["T清洁分切开机确认ID"] = ID;
                dr["序号"] = (i + 1);
                dr["确认项目"] = dt设置.Rows[i]["确认项目"];
                dr["确认内容"] = dt设置.Rows[i]["确认内容"];
                dr["确认结果"] = "Yes";
                dt.Rows.InsertAt(dr, dt.Rows.Count);
            }
            return dt;
        }

        //序号刷新
        private void setDataGridViewRowNums()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
        }

        //设置DataGridView中各列的格式
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "确认结果":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        cbc.Items.Add("Yes");
                        cbc.Items.Add("No");
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 120;
                        break;
                    default:
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = 120;
                        break;
                }
            }
            setDataGridViewFormat();
        }

        //设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T清洁分切开机确认ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["确认内容"].ReadOnly = true;
            dataGridView1.Columns["确认项目"].ReadOnly = true;
            dataGridView1.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        //设置datagridview背景颜色
        private void setDataGridViewBackColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["确认结果"].Value.ToString() == "Yes")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                if (dataGridView1.Rows[i].Cells["确认结果"].Value.ToString() == "No")
                {
                    //dataGridView1.Rows[i].Cells["确认结果"].Style.BackColor = Color.Red;
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }
        
        //******************************按钮功能******************************//
        
        //用于显示/新建数据
        private void bt查询新建_Click(object sender, EventArgs e)
        {
            DataShow(mySystem.Parameter.cleancutInstruID, dtp生产日期.Value, cb白班.Checked);
        }

        //保存按钮
        private void bt确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (stat_user == 0 && isSaved == true)
                bt发送审核.Enabled = true;
        }

        //保存功能
        private bool Save()
        {
            if (mySystem.Parameter.NametoID(tb确认人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『确认人』信息", "ERROR");
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*湿度、温度部位数字*/
                return false;
            }
            else
            {
                // 内表保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(mySystem.Parameter.cleancutInstruID, dtp生产日期.Value, cb白班.Checked);
                outerBind();

                setDataGridViewBackColor();
                return true;
            }
        }
        
        //发送审核按钮
        private void bt发送审核_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow gdvr in dataGridView1.Rows)
            {
                if (gdvr.DefaultCellStyle.BackColor == Color.Red)
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切开机确认' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "清洁分切开机确认";
                dr["对应ID"] = (int)dt记录.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            da_temp.Update(dt_temp);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;
            dt记录.Rows[0]["审核人"] = "__待审核";

            Save();
            stat_form = 1;
            setEnableReadOnly();
            setDataGridViewFormat(); //包含序号不可编辑
        }

        //日志按钮
        private void bt日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dt记录.Rows[0]["日志"].ToString());
        }

        //审核按钮
        private void bt审核_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow gdvr in dataGridView1.Rows)
            {
                if (gdvr.DefaultCellStyle.BackColor == Color.Red)
                {
                    MessageBox.Show("有条目待确认");
                    return;
                }
            }
            checkform = new CheckForm(this);
            checkform.Show();
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();

            dt记录.Rows[0]["审核人"] = Parameter.IDtoName(checkform.userID);
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切开机确认' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;

            bs记录.EndEdit();
            da记录.Update((DataTable)bs记录.DataSource);

            //修改状态，设置可控性
            if (checkform.ischeckOk)
            { stat_form = 2; }//审核通过
            else
            { stat_form = 3; }//审核未通过            
            setEnableReadOnly();
            setDataGridViewFormat(); //包含序号不可编辑
        }

        //****************************** 小功能 ******************************//  

        //白班、夜班
        private void cb白班_CheckedChanged(object sender, EventArgs e)
        {
            cb夜班.Checked = !cb白班.Checked;
        }

        //白班、夜班
        private void cb夜班_CheckedChanged(object sender, EventArgs e)
        {
            cb白班.Checked = !cb夜班.Checked;
        }

        //检查温度、湿度内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb车间湿度, tb车间温度 });
            List<String> StringList = new List<String>(new String[] { "车间湿度", "车间温度" });

            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show("『" + StringList[i] + "』应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }
        
        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        //检查是否为合格，下拉框不是Yes/合格/是，则标红，并不准审核
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            setDataGridViewBackColor();
        }

        //数据绑定结束，设置背景颜色
        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //throw new NotImplementedException();
            setDataGridViewBackColor();
            setDataGridViewFormat();
        }

                 
    }
}
