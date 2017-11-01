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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace mySystem.Process.Bag.BTV
{
    public partial class  BTVCutPipeRecord  : BaseForm
    {

        private String table = "BPV切管记录";
        private String tableInfo = "BPV切管记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        string Prodcode;
        string Prodbatch;
        DateTime date;

        public  BTVCutPipeRecord (MainForm mainform) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.bpvbagInstruID;
            Instruction = Parameter.bpvbagInstruction;
            tb生产指令编号.Text = Instruction;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();

            tb成品代码.ReadOnly = false;
            tb产品批号.ReadOnly = false;
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;

        }

        public  BTVCutPipeRecord (MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            IDShow(ID);
        }
        
        //******************************初始化******************************//

        // 获取操作员和审核员
        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='BPV切管记录'", connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string[] s = Regex.Split(dt.Rows[0]["操作员"].ToString(), ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        ls操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(dt.Rows[0]["审核员"].ToString(), ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        ls审核员.Add(s1[i]);
                }
            }
        }

        // 根据登录人，设置stat_user
        private void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = "管理员";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = "审核员";
        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setFormState(bool newForm = false)
        {
            if (newForm)
            {
                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dt记录.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]);
            if (s == "") _formState = Parameter.FormState.未保存;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        //读取设置内容  //GetProductInfo //产品代码、产品批号初始化
        private void getOtherData()
        {
            //if (!isSqlOk)
            //{
            //    OleDbCommand comm1 = new OleDbCommand();
            //    comm1.Connection = Parameter.connOle;
            //    comm1.CommandText = "select * from 生产指令 where 生产指令编号 = '" + Instruction + "' ";//这里应有生产指令编码
            //    OleDbDataReader reader1 = comm1.ExecuteReader();
            //    if (reader1.Read())
            //    {
            //        OleDbCommand comm2 = new OleDbCommand();
            //        comm2.Connection = Parameter.connOle;
            //        comm2.CommandText = "select ID, 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID = " + reader1["ID"].ToString();

            //        OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
            //        datemp.Fill(dt代码批号);
            //        if (dt代码批号.Rows.Count == 0)
            //        {
            //            MessageBox.Show("该生产指令编码下的『生产指令详细信息』尚未生成！");
            //        }
            //        else
            //        {
            //            for (int i = 0; i < dt代码批号.Rows.Count; i++)
            //            {
            //                cb产品代码.Items.Add(dt代码批号.Rows[i][1].ToString());//添加
            //            }
            //        }
            //        datemp.Dispose();
            //    }
            //    else
            //    {
            //        //dt代码批号为空
            //        MessageBox.Show("该生产指令编码下的『生产指令』尚未生成！");
            //    }
            //    reader1.Dispose();
            //}
            //else
            //{ }

            ////*********数据填写*********//
            //cb产品代码.SelectedIndex = -1;
            //tb生产批号.Text = "";
        }

        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (_userState == Parameter.UserState.管理员)//管理员
            {
                //控件都能点
                setControlTrue();
            }
            else if (_userState == Parameter.UserState.审核员)//审核人
            {
                if (_formState == Parameter.FormState.审核通过 || _formState == Parameter.FormState.审核未通过)  //2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                else if (_formState == Parameter.FormState.未保存)//0未保存
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView1.Rows[i].ReadOnly = false;
                        else
                            dataGridView1.Rows[i].ReadOnly = true;
                    }
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView1.Rows[i].ReadOnly = false;
                        else
                            dataGridView1.Rows[i].ReadOnly = true;
                    }
                }                
            }
            else//操作员
            {
                if (_formState == Parameter.FormState.待审核 || _formState == Parameter.FormState.审核通过) //1待审核||2审核通过
                {
                    //控件都不能点
                    setControlFalse();
                }
                else if (_formState == Parameter.FormState.未保存) //0未保存
                {
                    //发送审核，审核不能点
                    setControlTrue();
                    btn提交数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为未审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                            dataGridView1.Rows[i].ReadOnly = true;
                        else
                            dataGridView1.Rows[i].ReadOnly = false;
                    }
                }
                else //3审核未通过
                {
                    //发送审核，审核不能点
                    setControlTrue();
                    btn提交数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为未审核，则该行可以修改
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["审核员"].Value.ToString() != "")
                            dataGridView1.Rows[i].ReadOnly = true;
                        else
                            dataGridView1.Rows[i].ReadOnly = false;
                    }
                }                
            }
            //datagridview格式，包含序号不可编辑
            setDataGridViewFormat();
        }

        /// <summary>
        /// 设置所有控件可用；
        /// btn审核、btn提交审核两个按钮一直是false；内表审核、提交审核false
        /// 部分控件防作弊，不可改；
        /// 查询条件始终不可编辑
        /// </summary>
        void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = false;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮、审核人姓名框一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            tb审核员.Enabled = false;
            btn数据审核.Enabled = false;
            btn提交数据审核.Enabled = false;
            //部分空间防作弊，不可改
            
            //查询条件始终不可编辑
            tb生产指令编号.Enabled = false;
            tb成品代码.Enabled = false;
            tb产品批号.Enabled = false;
            dtp生产日期.Enabled = false;
            btn查询新建.Enabled = false;
        }

        /// <summary>
        /// 设置所有控件不可用；
        /// 查看日志、打印始终可用
        /// </summary>
        void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = true;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            //查看日志、打印始终可用
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cb打印机.Enabled = true;
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

        // 设置自动计算类事件：TextChanged&Leave
        private void addComputerEventHandler()
        {
        }

        //修改单个控件的值
        private void outerDataSync(String name, String val)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == name)
                {
                    c.Text = val;
                    c.DataBindings[0].WriteValue();
                }
            }
        }

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID, String productCode,String prodbatch,DateTime datetime)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, productCode,prodbatch,datetime);
            outerBind();
            //MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************// 
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
                readOuterData(InstruID, productCode, prodbatch, datetime);
                outerBind();

                //********* 内表新建、保存、重新绑定 *********//

                //内表绑定
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();
                DataRow dr2 = dt记录详情.NewRow();
                dr2 = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr2);
                dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
                setDataGridViewRowNums();
                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
            }
            //内表绑定
            dataGridView1.Columns.Clear();
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();

            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  
        }

        //根据主键显示
        public void IDShow(Int32 ID)
        {
            OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + table + " where ID = " + ID.ToString(), connOle);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                InstruID = Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString());
                Instruction = dt1.Rows[0]["生产指令编号"].ToString();
                Prodcode = dt1.Rows[0]["成品代码"].ToString();
                Prodbatch = dt1.Rows[0]["产品批号"].ToString();
                date=DateTime.Parse(dt1.Rows[0]["生产日期"].ToString());
                DataShow(InstruID, Prodcode, Prodbatch, date);
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, String productCode,String prodbatch,DateTime date)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID + " and 成品代码 = '" + productCode + "' and 产品批号='" + prodbatch + "' and 生产日期=#" + date.ToString("yyyy/MM/dd") + "#", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //解绑->绑定
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            tb成品代码.DataBindings.Clear();
            tb成品代码.DataBindings.Add("Text", bs记录.DataSource, "成品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");

            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            tb审核员.DataBindings.Clear();
            tb审核员.DataBindings.Add("Text", bs记录.DataSource, "审核员");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["生产指令编号"] = Instruction;
            dr["成品代码"] = tb成品代码.Text;
            dr["产品批号"] = tb产品批号.Text;
            dr["生产日期"] = DateTime.Now.ToString("yyyy/MM/dd");
            dr["审核员"] = "";
            dr["审核是否通过"] = false;          
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + Instruction + "\n";
            dr["日志"] = log;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where TBPV切管记录ID = " + ID, connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            dataGridView1.DataSource = bs记录详情.DataSource;
        }

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            dr["序号"] = 0;
            dr["TBPV切管记录ID"] = ID;
            dr["使用管代码"] = "";
            dr["领取管数量米"] = 0;
            dr["图纸要求尺寸毫米"] = 0;
            dr["切管数量个"] = 0;
            dr["质量检查外观"] = "Yes";
            dr["质量检查尺寸"] = "Yes";
            dr["质量检查判定"] = "Yes";
            dr["操作员"] = mySystem.Parameter.userName;
            dr["审核员"] = "";
            return dr;
        }

        //序号刷新
        private void setDataGridViewRowNums()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "质量检查外观":
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
                    case "质量检查尺寸":
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
                    case "质量检查判定":
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
            //隐藏
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["TBPV切管记录ID"].Visible = false;
            //不可用
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["审核员"].ReadOnly = true;
            //HeaderText
            dataGridView1.Columns["领取管数量米"].HeaderText = "领取管数量\r/M";
            dataGridView1.Columns["图纸要求尺寸毫米"].HeaderText = "图纸要求尺寸\r/mm";
            dataGridView1.Columns["切管数量个"].HeaderText = "切管数量\r/个";
            dataGridView1.Columns["质量检查外观"].HeaderText = "外观";
            dataGridView1.Columns["质量检查尺寸"].HeaderText = "尺寸";
            dataGridView1.Columns["质量检查判定"].HeaderText = "判定";
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (tb成品代码.Text == "" || tb产品批号.Text == "")
            {
                MessageBox.Show("成品代码和产品批号均不能为空");
                return;
            }
            Prodcode = tb成品代码.Text;
            Prodbatch = tb产品批号.Text ;
            date = dtp生产日期.Value ;
            DataShow(InstruID, Prodcode, Prodbatch, date);
        }

        //添加按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
            setEnableReadOnly();
        }
        
        //删除按钮
        private void btn删除记录_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //仅当审核人为空时，可删除
                if (dataGridView1.Rows[deletenum].Cells["审核员"].Value.ToString() == "")
                {
                    //dt记录详情.Rows.RemoveAt(deletenum);
                    dt记录详情.Rows[deletenum].Delete();

                    // 保存
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                    readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                    innerBind();

                    setDataGridViewRowNums();
                    setEnableReadOnly();
                }
            }
        }

        //内表移交审核按钮
        private void btn提交数据审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    dt记录详情.Rows[i]["审核员"] = "__待审核";
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();
        }

        //内表审核按钮
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            HashSet<Int32> hi待审核行号 = new HashSet<int>();
            foreach (DataGridViewCell dgvc in dataGridView1.SelectedCells)
            {
                hi待审核行号.Add(dgvc.RowIndex);
            }
            foreach (int i in hi待审核行号)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    dt记录详情.Rows[i]["审核员"] = mySystem.Parameter.userName;
                    dataGridView1.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();
        }

        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
                btn提交审核.Enabled = true;
        }

        //保存功能
        private bool Save()
        {
            // 内表保存
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind();

            //外表保存
            bs记录.EndEdit();
            da记录.Update((DataTable)bs记录.DataSource);
            readOuterData(InstruID, tb成品代码.Text,tb产品批号.Text,dtp生产日期.Value);
            outerBind();

            setEnableReadOnly();

            return true;
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //判断内表是否完全提交审核
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    MessageBox.Show("第" + (i + 1) + "行未提交数据审核");
                    return;
                }
            }

            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='BPV切管记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "BPV切管记录";
                dr["对应ID"] = (int)dt记录.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            da_temp.Update(dt_temp);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 提交审核\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;
            dt记录.Rows[0]["审核员"] = "__待审核";

            Save();
            _formState = Parameter.FormState.待审核;
            setEnableReadOnly();
        }

        //查看日志按钮
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            mySystem.Other.LogForm logform = new mySystem.Other.LogForm();
            logform.setLog(dt记录.Rows[0]["日志"].ToString()).Show();
        }

        //审核功能
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
                return;
            }
            //判断内表是否完全审核
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    MessageBox.Show("数据审核未完成");
                    return;
                }
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    MessageBox.Show("第"+(i+1)+"行未提交数据审核");
                    return;
                }
            }
            checkform = new CheckForm(this);
            checkform.Show();
        }

        //审核功能
        public override void CheckResult()
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            base.CheckResult();

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='BPV切管记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;

            Save();

            //修改状态，设置可控性
            if (checkform.ischeckOk)
            { _formState = Parameter.FormState.审核通过; }//审核通过
            else
            { _formState = Parameter.FormState.审核未通过; }//审核未通过              
            setEnableReadOnly();
        }

        //添加打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        private void fill_printer()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }
        //打印按钮
        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }
        public void print(bool preview)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\16 SOP-MFG-306-R04A  BPV切管记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值

            int rowStartAt = 6;
            my.Cells[3, 10].Value = dt记录.Rows[0]["生产指令编号"];
            my.Cells[3, 2].Value =  dt记录.Rows[0]["成品代码"];
            my.Cells[3, 5].Value =  dt记录.Rows[0]["产品批号"];           
            //my.Cells[3, 9].Value = "生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日");


            //EVERY SHEET CONTAINS 12 RECORDS
            int rowNumPerSheet = 11;
            int rowNumTotal = dt记录详情.Rows.Count;
            for (int i = 0; i < (rowNumTotal > rowNumPerSheet ? rowNumPerSheet : rowNumTotal); i++)
            {

                my.Cells[i + rowStartAt, 1].Value = dt记录详情.Rows[i]["序号"];
                my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).ToString("yyyy/MM/dd HH:mm");
                my.Cells[i + rowStartAt, 3].Value = dt记录详情.Rows[i]["使用管代码"];
                //my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产日期时间"]).ToString("MM/dd HH:mm");
                //my.Cells[i + rowStartAt, 2].Font.Size = 11;
                //my.Cells[i + rowStartAt, 3].Value = dt记录详情.Rows[i]["领取管数量米"];
                my.Cells[i + rowStartAt, 4].Value = dt记录详情.Rows[i]["图纸要求尺寸毫米"];
                my.Cells[i + rowStartAt, 5].Value = dt记录详情.Rows[i]["切管数量个"];
                //my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["产品数量只"];
                my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["质量检查外观"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                my.Cells[i + rowStartAt, 7].Value = dt记录详情.Rows[i]["质量检查尺寸"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                my.Cells[i + rowStartAt, 8].Value = dt记录详情.Rows[i]["质量检查判定"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                my.Cells[i + rowStartAt, 9].Value = dt记录详情.Rows[i]["操作员"];
                my.Cells[i + rowStartAt, 10].Value = dt记录详情.Rows[i]["审核员"];
                my.Cells[i + rowStartAt, 11].Value = dt记录详情.Rows[i]["备注"];
                
            }

            //THIS PART HAVE TO INSERT NOEW BETWEEN THE HEAD AND BOTTM
            if (rowNumTotal > rowNumPerSheet)
            {
                for (int i = rowNumPerSheet; i < rowNumTotal; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);


                    my.Cells[i + rowStartAt, 1].Value = dt记录详情.Rows[i]["序号"];
                    my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录.Rows[0]["生产日期"].ToString()).ToString("yyyy/MM/dd HH:mm");
                    my.Cells[i + rowStartAt, 3].Value = dt记录详情.Rows[i]["使用管代码"];
                    //my.Cells[i + rowStartAt, 3].Value = Convert.ToDateTime(dt记录.Rows[0]["生产日期"]).ToString("MM/dd HH:mm");
                    //my.Cells[i + rowStartAt, 2].Font.Size = 11;
                    //my.Cells[i + rowStartAt, 3].Value = dt记录详情.Rows[i]["领取管数量米"];
                    my.Cells[i + rowStartAt, 4].Value = dt记录详情.Rows[i]["图纸要求尺寸毫米"];
                    my.Cells[i + rowStartAt, 5].Value = dt记录详情.Rows[i]["切管数量个"];
                    //my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["产品数量只"];
                    my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["质量检查外观"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                    my.Cells[i + rowStartAt, 7].Value = dt记录详情.Rows[i]["质量检查尺寸"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                    my.Cells[i + rowStartAt, 8].Value = dt记录详情.Rows[i]["质量检查判定"] == "Yes" ? "Y☑   N□" : "Y□   N☑";
                    my.Cells[i + rowStartAt, 9].Value = dt记录详情.Rows[i]["操作员"];
                    my.Cells[i + rowStartAt, 10].Value = dt记录详情.Rows[i]["审核员"];
                    my.Cells[i + rowStartAt, 11].Value = dt记录详情.Rows[i]["备注"];
                }
            }

            Microsoft.Office.Interop.Excel.Range range1 = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + rowNumTotal, Type.Missing];
            range1.EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);

            //THE BOTTOM HAVE TO CHANGE LOCATE ACCORDING TO THE ROWS NUMBER IN DT.
            int varOffset = (rowNumTotal > rowNumPerSheet) ? rowNumTotal - rowNumPerSheet + 1 : 0;
            //my.Cells[19 + varOffset, 1].Value = "备注：\n" + dt记录.Rows[0]["备注"];
            if (preview)
            {
                my.Select();
                oXL.Visible = true; //加上这一行  就相当于预览功能            
            }
            else
            {
                //add footer
                my.PageSetup.RightFooter = Instruction + "-10-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码

                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch { }
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源

                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
                oXL = null;
                my = null;
                wb = null;
            }
        }


        int find_indexofprint()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from " + table + " where 生产指令ID=" + InstruID, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dt记录.Rows[0]["ID"])) + 1;
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

        //数据绑定结束，设置表格格式
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat();
        }

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
            //    if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量包数")
            //    {
            //        int sumA = 0;
            //        int numtemp;
            //        for (int i = 0; i < dt记录详情.Rows.Count; i++)
            //        {
            //            if (Int32.TryParse(dt记录详情.Rows[i]["产品数量包数"].ToString(), out numtemp) == true)
            //            { sumA += numtemp; }
            //        }
            //        //dt记录.Rows[0]["产品数量包数合计A"] = sum;
            //        outerDataSync("tb产品数量包数合计A", sumA.ToString());
            //    }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "产品数量只数")
            //    {
            //        getPercent();
            //    }
            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "操作员")
            //    {
            //        if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
            //        {
            //            dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
            //            MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
            //        }
            //    }
            //    else
            //    { }
            }
        }

    }
}


