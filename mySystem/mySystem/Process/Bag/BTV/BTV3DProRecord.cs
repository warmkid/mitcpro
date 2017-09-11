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
    public partial class BTV3DProRecord : BaseForm
    { 
        /*
         * BECAUSE THE DATABASE HAS NOT BENN UPDATE, YOU HAVE TO MODIFY THE DATABASE
         * 1. ADD THE "检测时间" COLUMN 
         * 2. FOR BETTER 
        */
        /// <summary>
        /// 3D袋体生产记录
        /// </summary>
        private String table = "3D袋体生产记录";
        /// <summary>
        /// 3D袋体生产记录详细信息
        /// </summary>
        private String tableInfo = "3D袋体生产记录领料记录";
        private String tableInfo2 = "3D袋体生产记录领料记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dt记录, dt记录详情,dt领料记录, dt代码批号, dt膜代码;
        private Dictionary<string, string> dic膜材;//膜材代码和批号对应关系
        private OleDbDataAdapter da记录, da记录详情,da领料记录;
        private BindingSource bs记录, bs记录详情,bs领料记录;
        private OleDbCommandBuilder cb记录, cb记录详情,cb领料记录;
        private int[] sum = { 0, 0 };

        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Parameter.FormState _formState;
        Int32 InstruID;
        String Instruction;
        String MoCode;

        public BTV3DProRecord(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.bpvbagInstruID;
            Instruction = Parameter.bpvbagInstruction;


            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            getOtherData();  //读取设置内容
            addOtherEvnetHandler();  // 其他事件，datagridview：DataError、CellEndEdit、DataBindingComplete
            addDataEventHandler();  // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged

            setControlFalse();
            dtp开始生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
            cb打印机.Enabled = false;
        }

        public BTV3DProRecord(MainForm mainform, int ID)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            fill_printer(); //添加打印机
            getPeople();  // 获取操作员和审核员
            setUserState();  // 根据登录人，设置stat_user
            //getOtherDataLocal();  //读取设置内容
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
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='" + table + "';", connOle);
            dt = new DataTable("temp");
            da.Fill(dt);

            //ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();
            //ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();

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

        //读取设置内容  //GetProductInfo //产品名称、产品批号列表获取+产品工艺、设备、班次填写
        private void getOtherData()
        {
            dt代码批号 = new DataTable("代码批号");
            dic膜材 = new Dictionary<string, string>();

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (!isSqlOk)
            {
                //查找该生产ID下的生产时间
                DateTime 计划生产日期;
                string sqlStr = "SELECT 计划生产日期 FROM 生产指令 WHERE ID = " + InstruID + ";";
                OleDbCommand get生产指令详细信息 = new OleDbCommand(sqlStr, connOle);
                try
                {
                    计划生产日期 = Convert.ToDateTime(get生产指令详细信息.ExecuteScalar());
                    dtp开始生产日期.Value = 计划生产日期;
                }
                catch
                {
                    MessageBox.Show("该生产指令编码下的『计划生产日期』尚未填写");
                    dtp开始生产日期.Value = DateTime.Now;
                }
                //查找该生产ID下的产品编码、产品批号
                OleDbCommand comm2 = new OleDbCommand();
                comm2.Connection = Parameter.connOle;
                comm2.CommandText = "select ID, 产品代码, 产品批号 from 生产指令详细信息 where T生产指令ID = " + InstruID;
                OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                datemp.Fill(dt代码批号);
                if (dt代码批号.Rows.Count == 0)
                {
                    MessageBox.Show("该生产指令编码下的『生产指令产品列表』尚未生成！");
                }
                else
                {
                    tb产品代码.Text = dt代码批号.Rows[0]["产品代码"].ToString();
                    tb产品批号.Text = dt代码批号.Rows[0]["产品批号"].ToString();
                }
                datemp.Dispose();

                //TODO:是否正确？
                //从物料中读取物料代码
                OleDbCommand comm3 = new OleDbCommand();
                comm3.Connection = Parameter.connOle;
                comm3.CommandText = "select * from 设置物料代码";
                OleDbDataAdapter datemp2 = new OleDbDataAdapter(comm3);
                DataTable dtemp = new DataTable();
                datemp2.Fill(dtemp);
                if (dtemp.Rows.Count > 0)
                {
                    //认为物料批号是人工填写的
                    for (int i = 0; i < dtemp.Rows.Count; i++)
                    {
                        dic膜材.Add(dtemp.Rows[i][1].ToString(), dtemp.Rows[i][1].ToString());
                    }
                }
                addMaterialToDt();
                datemp2.Dispose();
            }
            else
            {
                //从SQL数据库中读取;                
            }
        }
        private void addMaterialToDt()
        {
            OleDbDataAdapter daGetMaterial = new OleDbDataAdapter("select * from 生产指令物料 where T生产指令ID =" + InstruID, connOle);
            DataTable dtResult = new DataTable();
            daGetMaterial.Fill(dtResult);
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                
                 //dtResult.Rows[i]["物料名称"];                 
               dic膜材.Add(dtResult.Rows[i]["物料代码"].ToString(),dtResult.Rows[i]["物料批号"].ToString());
            }
            daGetMaterial.Dispose();
        }
        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (_userState == Parameter.UserState.管理员)
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
                    dataGridView2.ReadOnly = false;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView2.Rows[i].ReadOnly = false;
                        else
                            dataGridView2.Rows[i].ReadOnly = true;
                    }
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;
                    btn数据审核.Enabled = true;
                    //遍历datagridview，如果有一行为待审核，则该行可以修改
                    dataGridView2.ReadOnly = false;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells["审核员"].Value.ToString() == "__待审核")
                            dataGridView2.Rows[i].ReadOnly = false;
                        else
                            dataGridView2.Rows[i].ReadOnly = true;
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
                else //0未保存||3审核未通过
                {
                    //发送审核，审核不能点
                    setControlTrue();
                }
            }
            //datagridview格式，包含序号不可编辑
            setDataGridViewFormat();
        }

        /// <summary>
        /// 设置所有控件可用；
        /// btn审核、btn提交审核两个按钮一直是false；
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
            tb复核人.Enabled = false;
            btn提交数据审核.Enabled = false;
            btn数据审核.Enabled = false;

            //部分空间防作弊，不可改
            tb产品批号.ReadOnly = true;
            tb产品代码.ReadOnly = true;

            
            //查询条件始终不可编辑 instruction and 生产日期 both decide

            dtp开始生产日期.Enabled = false;
            btn查询新建.Enabled = false;
        }

        /// <summary>
        /// 设置所有控件不可用；
        /// 查看日志、打印、cb打印机始终可用
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
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);

            dataGridView2.DataError += dataGridView2_DataError;
            dataGridView2.CellEndEdit += dataGridView2_CellEndEdit;
            dataGridView2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView2_DataBindingComplete);


        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex >= 0)
            //{
            //    if (dataGridView1.Columns[e.ColumnIndex].Name == "合格品数量")
            //    {
            //        getTotal();
            //    }

            //    if (dataGridView1.Columns[e.ColumnIndex].Name == "生产时间")
            //    {
            //        getTotal();
            //    }

            //    else if (dataGridView1.Columns[e.ColumnIndex].Name == "不良品数量")
            //    {
            //        getTotal();
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
            //}
        }
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        private void addDataEventHandler() { }

        // 设置自动计算类事件
        private void addComputerEventHandler() { }

        //******************************显示数据******************************//

        //显示根据信息查找
        private void DataShow(Int32 InstruID)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID);
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
                readOuterData(InstruID);
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
            dataGridView2.Columns.Clear();
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();
            if (dt记录详情.Rows.Count > 0)
                getTotal();

            dataGridView1.Columns.Clear();
            readInnerData1(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns1();
            innerBind1();

            addComputerEventHandler();  // 设置自动计算类事件
            setFormState();  // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
            setEnableReadOnly();  //根据状态设置可读写性  

        }


        // ANOTHER EDITION: ADD TIME WHNE SEARCH
        private void DataShow(Int32 InstruID, DateTime _生产日期)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, _生产日期);
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
                readOuterData(InstruID, _生产日期);
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
            dataGridView2.Columns.Clear();
            readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();
            if (dt记录详情.Rows.Count > 0)
                getTotal();

            dataGridView1.Columns.Clear();
            readInnerData1(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            setDataGridViewColumns1();
            innerBind1();


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
                //DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()));
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + ";", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //ANOTHER EDITION
        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID + " and 开始生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            tb产品代码.DataBindings.Clear();
            tb产品代码.DataBindings.Add("Text", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            dtp开始生产日期.DataBindings.Clear();
            dtp开始生产日期.DataBindings.Add("Text", bs记录.DataSource, "开始生产日期");


            tb生产时间.DataBindings.Clear();
            tb生产时间.DataBindings.Add("Text", bs记录.DataSource, "生产时间");

            tb合格品数量.DataBindings.Clear();
            tb合格品数量.DataBindings.Add("Text", bs记录.DataSource, "合格品数量");

            tb不良品数量.DataBindings.Clear();
            tb不良品数量.DataBindings.Add("Text", bs记录.DataSource, "不良品数量");

            tb发料人.DataBindings.Clear();
            tb发料人.DataBindings.Add("Text", bs记录.DataSource, "发料人");
            tb领料人.DataBindings.Clear();
            tb领料人.DataBindings.Add("Text", bs记录.DataSource, "领料人");
            tb复核人.DataBindings.Clear();
            tb复核人.DataBindings.Add("Text", bs记录.DataSource, "复核人");

        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = InstruID;
            dr["产品代码"] = tb产品代码.Text;
            dr["产品批号"] = tb产品批号.Text;
            dr["开始生产日期"] = Convert.ToDateTime(dtp开始生产日期.Value.ToString("yyyy/MM/dd"));

            dr["领料人"] = mySystem.Parameter.userName;
            dr["复核人"] = "";
            dr["审核员"] = "";
            dr["审核是否通过"] = false;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + Instruction + "\n";
            dr["日志"] = log;

            dr["生产时间"] = 0;
            dr["合格品数量"] = 0;
            dr["不良品数量"] = 0;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo2);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo2 + " where T" + table + "ID = " + ID.ToString(), connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }
        private void readInnerData1(Int32 ID)
        {
            bs领料记录 = new BindingSource();
            dt领料记录 = new DataTable(tableInfo);
            da领料记录 = new OleDbDataAdapter("select * from " + tableInfo + " where T" + table + "ID = " + ID.ToString(), connOle);
            cb领料记录 = new OleDbCommandBuilder(da领料记录);
            da领料记录.Fill(dt领料记录);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            //dataGridView1.DataBindings.Clear();
            dataGridView2.DataSource = bs记录详情.DataSource;
        }
        private void innerBind1()
        {
            bs领料记录.DataSource = dt领料记录;
            //dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs领料记录.DataSource;
        }
        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            //TO DO : CONFIRM THAT IS THERE EXIST 不良品数量
            dr["T" + table + "ID"] = ID;
            dr["序号"] = 0;
            dr["生产日期"] = DateTime.Now;
            dr["生产时间"] = 0;
            dr["A较大片"] = 0;
            dr["B较小片"] = 0;
            dr["袋体外观检查有缺陷时填写内容"] = "√";
            dr["袋体尺寸确认"] = "√";
            dr["热封线检查"] = "√";
            
            dr["合格品数量"] = 0;
            dr["不良品数量"] = 0;
            
            dr["操作员"] = mySystem.Parameter.userName;
            dr["操作员备注"] = "";
            dr["审核员"] = "";
            return dr;
        }
        private DataRow writeInnerDefault1(Int32 ID, DataRow dr)
        {
            dr["T3D袋体生产记录ID"] = ID;
            dr["领料数量"] = 0;
            dr["使用数量"] = 0;
            dr["退库数量"] = 0;

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
            DataTable setWidth = new DataTable("setWidth");
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                setWidth.Columns.Add(dc.ColumnName, Type.GetType("System.Int32"));
            }
            DataRow nums = setWidth.NewRow();
            nums["ID"] = 50;
            nums["T" + table + "ID"] = 50;
            nums["序号"] = 50;
            nums["生产日期"] = 150;
            nums["生产时间"] = 80;
            nums["A较大片"] = 70;
            nums["B较小片"] = 65;
            nums["袋体外观检查有缺陷时填写内容"] = 65;
            nums["袋体尺寸确认"] = 65;
            nums["热封线检查"] = 65;
            nums["合格品数量"] = 65;
            nums["不良品数量"] = 65;
            nums["操作员"] = 75;
            nums["操作员备注"] = 100;
            nums["审核员"] = 50;
            setWidth.Rows.Add(nums);
            DataGridViewTextBoxColumn tbc;
            foreach (DataColumn dc in dt记录详情.Columns)
            {
                //if ("设定" == dc.ColumnName)
                //{
                //    continue;
                //}
                switch (dc.ColumnName)
                {
                    default:
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(tbc);
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = Convert.ToInt32(setWidth.Rows[0][tbc.Name]);
                        break;
                }
            }
        }
        private void setDataGridViewColumns1()
        {
            foreach (DataColumn dc in dt领料记录.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "膜材代码":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "膜材代码";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        c1.MinimumWidth = 100;
                        c1.ValueType = dc.DataType;
                        foreach (var sdr in dic膜材)
                        {
                            c1.Items.Add(sdr.Key);
                        }
                        dataGridView1.Columns.Add(c1);
                        break;
                    default:
                        DataGridViewTextBoxColumn tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        tbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        tbc.MinimumWidth = 100;
                        break;
                }
            }
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat1()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T3D袋体生产记录ID"].Visible = false;
            dataGridView1.Columns["发料人"].Visible = false;
            dataGridView1.Columns["领料人"].Visible = false;
            dataGridView1.Columns["审核员"].Visible = false;
            dataGridView1.Columns["日志"].Visible = false;
           
            dataGridView1.Columns["领料数量"].HeaderText = "领料数量/M";
            dataGridView1.Columns["使用数量"].HeaderText = "使用数量/M";
            dataGridView1.Columns["退库数量"].HeaderText = "退库数量/M";
        }
        private void setDataGridViewFormat()
        {
            dataGridView2.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ColumnHeadersHeight = 40;
            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["T3D袋体生产记录ID"].Visible = false;
            dataGridView2.Columns["序号"].ReadOnly = true;
            dataGridView2.Columns["审核员"].ReadOnly = true;

            dataGridView2.Columns["生产时间"].HeaderText = "生产时间(分钟)";
            dataGridView2.Columns["A较大片"].HeaderText = "A-较大片(mm)";
            dataGridView2.Columns["B较小片"].HeaderText = "B-较小片(mm)";
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

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            DataShow(InstruID, dtp开始生产日期.Value.Date);
        }

        //添加行按钮
        private void btn添加记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
        }

        //删除行按钮
        private void btn删除记录_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView2.CurrentRow.Index;
                //dt记录详情.Rows.RemoveAt(deletenum);
                dt记录详情.Rows[deletenum].Delete();
                // 保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();
                //求合计
                getTotal();
                setDataGridViewRowNums();
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
                    dataGridView2.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();

            // ADDTION: WHEN 提交数据审核, WE WANT 提交审核 ID STILL ACTIVE AFTER SET ENABLEREADONLY
            btn提交审核.Enabled = true;
        }

        //内表审核按钮
        private void btn数据审核_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    dt记录详情.Rows[i]["审核员"] = mySystem.Parameter.userName;
                    dataGridView2.Rows[i].ReadOnly = true;
                }
            }
            bs记录详情.DataSource = dt记录详情;
            da记录详情.Update((DataTable)bs记录详情.DataSource);
            innerBind();
            setEnableReadOnly();

            // ADDTION: WHEN 数据审核, WE WANT 审核 ID STILL ACTIVE AFTER SET ENABLEREADONLY
            btn审核.Enabled = true;
        }


        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (_userState == Parameter.UserState.操作员 && isSaved == true)
            {
                btn提交审核.Enabled = true;
                btn提交数据审核.Enabled = true;
            }
        }

        //保存功能
        private bool Save()
        {
            if (Name_check() == false)
            {
                /*操作员不合格*/
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*不良品数量*/
                return false;
            }
            else
            {
                // 内表保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();

                da领料记录.Update((DataTable)bs领料记录.DataSource);
                readInnerData1(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind1();

                //外表保存
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                readOuterData(InstruID, dtp开始生产日期.Value);
                outerBind();

                return true;
            }
        }

        //提交审核按钮
        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
            { return; }
            else if (need提交数据审核())
            {
                MessageBox.Show("需要提交数据审核");
                return;
            }

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='" + table + "' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = table;
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
            dt记录.Rows[0]["复核人"] = "__待审核";

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
        public override void CheckResult()
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            base.CheckResult();

            dt记录.Rows[0]["审核员"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["复核人"] = mySystem.Parameter.userName;
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //dt记录.Rows[0]["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString());
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='" + table + "' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
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

        //审核按钮
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dt记录详情.Rows[0]["操作员"].ToString())
            {
                MessageBox.Show("当前登录的审核员与操作员为同一人，不可进行审核！");
                return;
            }
            else if (need数据审核())
            {
                MessageBox.Show("需要数据审核");
                return;
            }
            checkform = new CheckForm(this);
            checkform.Show();
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
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\BPVBag\SOP-MFG-502-R01A  3D袋体生产记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[4];
            // 设置该进程是否可见
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值
            int rowStartAt = 5;
            my.Cells[3, 1].Value = "产品代码：" + dt记录.Rows[0]["产品代码"];
            my.Cells[3, 6].Value = "产品批号：" + dt记录.Rows[0]["产品批号"];
            my.Cells[3, 9].Value = "开始生产日期：" + Convert.ToDateTime(dt记录.Rows[0]["开始生产日期"]).ToString("yyyy年MM月dd日");
            
            int rowNumPerSheet = 1;
            int rowNumTotal = dt领料记录.Rows.Count;
            for (int i = 0; i < (rowNumTotal > rowNumPerSheet ? rowNumPerSheet : rowNumTotal); i++)
            {
                my.Cells[i + rowStartAt, 1].Value = dt领料记录.Rows[i]["膜材代码"];
                my.Cells[i + rowStartAt, 2].Value = dt领料记录.Rows[i]["物料批号"]; ;
                my.Cells[i + rowStartAt, 3].Value = dt领料记录.Rows[i]["领料数量"];
                my.Cells[i + rowStartAt, 4].Value = dt领料记录.Rows[i]["使用数量"];
                my.Cells[i + rowStartAt, 4].Value = dt领料记录.Rows[i]["退库数量"];
                
            }
            if (rowNumTotal > rowNumPerSheet)
            {
                for (int i = rowNumPerSheet; i < rowNumTotal; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                    my.Cells[i + rowStartAt, 1].Value = dt领料记录.Rows[i]["膜材代码"];
                    my.Cells[i + rowStartAt, 2].Value = dt领料记录.Rows[i]["物料批号"]; ;
                    my.Cells[i + rowStartAt, 3].Value = dt领料记录.Rows[i]["领料数量"];
                    my.Cells[i + rowStartAt, 4].Value = dt领料记录.Rows[i]["使用数量"];
                    my.Cells[i + rowStartAt, 4].Value = dt领料记录.Rows[i]["退库数量"];
                }
            }
            int varOffset = (rowNumTotal > rowNumPerSheet) ? rowNumTotal - rowNumPerSheet - 1 : 0;
            rowStartAt = 9 + varOffset;
            
            rowNumPerSheet = 9;
            rowNumTotal = dt记录详情.Rows.Count;
            for (int i = 0; i < (rowNumTotal > rowNumPerSheet ? rowNumPerSheet : rowNumTotal); i++)
            {
                my.Cells[i + rowStartAt, 1].Value = dt记录详情.Rows[i]["序号"];
                my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产时间"]).ToString("MM/dd HH:mm");
                my.Cells[i + rowStartAt, 2].Font.Size = 11;
                my.Cells[i + rowStartAt, 3].Value = "";
                my.Cells[i + rowStartAt, 4].Value = dt记录详情.Rows[i]["A较大片"];
                my.Cells[i + rowStartAt, 5].Value = dt记录详情.Rows[i]["B较小片"];
                my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["袋体外观检查有缺陷时填写内容"];
                my.Cells[i + rowStartAt, 7].Value = dt记录详情.Rows[i]["袋体尺寸确认"];
                my.Cells[i + rowStartAt, 8].Value = dt记录详情.Rows[i]["热封线检查"];
                my.Cells[i + rowStartAt, 9].Value = dt记录详情.Rows[i]["合格品数量"];
                my.Cells[i + rowStartAt, 10].Value = dt记录详情.Rows[i]["不良品数量"];
                my.Cells[i + rowStartAt, 11].Value = dt记录详情.Rows[i]["操作员"];
                my.Cells[i + rowStartAt, 12].Value = dt记录详情.Rows[i]["审核员"];
                    
            }
            if (rowNumTotal > rowNumPerSheet)
            {
                for (int i = rowNumPerSheet; i < rowNumTotal; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)my.Rows[rowStartAt + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                    my.Cells[i + rowStartAt, 1].Value = dt记录详情.Rows[i]["序号"];
                    my.Cells[i + rowStartAt, 2].Value = Convert.ToDateTime(dt记录详情.Rows[i]["生产时间"]).ToString("MM/dd HH:mm");
                    my.Cells[i + rowStartAt, 2].Font.Size = 11;
                    my.Cells[i + rowStartAt, 3].Value = "";
                    my.Cells[i + rowStartAt, 4].Value = dt记录详情.Rows[i]["A较大片"];
                    my.Cells[i + rowStartAt, 5].Value = dt记录详情.Rows[i]["B较小片"];
                    my.Cells[i + rowStartAt, 6].Value = dt记录详情.Rows[i]["袋体外观检查有缺陷时填写内容"];
                    my.Cells[i + rowStartAt, 7].Value = dt记录详情.Rows[i]["袋体尺寸确认"];
                    my.Cells[i + rowStartAt, 8].Value = dt记录详情.Rows[i]["热封线检查"];
                    my.Cells[i + rowStartAt, 9].Value = dt记录详情.Rows[i]["合格品数量"];
                    my.Cells[i + rowStartAt, 10].Value = dt记录详情.Rows[i]["不良品数量"];
                    my.Cells[i + rowStartAt, 11].Value = dt记录详情.Rows[i]["操作员"];
                    my.Cells[i + rowStartAt, 12].Value = dt记录详情.Rows[i]["审核员"];                   
                }
            }
            int rowNum1, rowNum2, rowNum3, rowNum4, rowNum5;
            rowNum2 = 2;
            if (dt领料记录.Rows.Count < 2)
            {
                rowNum1 = rowNum2;
            }
            else
            {
                rowNum1 = dt领料记录.Rows.Count;
            }
            rowNum4 = 10;
            if (dt记录详情.Rows.Count < 2)
            {
                rowNum3 = rowNum4;
            }
            else
            {
                rowNum3 = dt记录详情.Rows.Count;
            }
            rowNum5 = rowNum1 = rowNum3 + 7;
            my.Cells[rowNum5, 9].Value = dt记录.Rows[0]["合格品数量"];
            my.Cells[rowNum5, 10].Value = dt记录.Rows[0]["不良品数量"];
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
		

        //******************************小功能******************************//  

        //need提交数据审核
        private bool need提交数据审核()
        {
            bool rtn = false;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "")
                {
                    rtn = true;
                }
            }
            return rtn;
        }

        //内表审核按钮
        private bool need数据审核()
        {
            bool rtn = false;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (dt记录详情.Rows[i]["审核员"].ToString() == "__待审核")
                {
                    rtn = true;
                }
            }
            return rtn;
        }

        //求合计
        private void getTotal()
        {
            int numtemp;
            // 膜卷长度求和
            sum[0] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["合格品数量"].ToString(), out numtemp) == true)
                { sum[0] += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            outerDataSync("tb合格品数量", sum[0].ToString());
            // 膜卷重量求和
            sum[1] = 0;

            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["不良品数量"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];
            outerDataSync("tb不良品数量", sum[1].ToString());

            sum[1] = 0;

            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["生产时间"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            //dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];
            outerDataSync("tb生产时间", sum[1].ToString());

        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] {  });
            List<String> StringList = new List<String>(new String[] { });
            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show("『" + StringList[i] + "』框内应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }

            if (mySystem.Parameter.NametoID(tb发料人.Text) == 0)
            {
                MessageBox.Show("发料人ID不存在，请重新填入！");
                TypeCheck = false;
            }
            if (mySystem.Parameter.NametoID(tb领料人.Text) == 0)
            {
                MessageBox.Show("领料人ID不存在，请重新填入！");
                TypeCheck = false;
            }
            return TypeCheck;
        }

        // 检查 操作员的姓名
        private bool Name_check()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["操作员"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["操作员"] = mySystem.Parameter.userName;
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的『操作员』信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
        }

        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
        }

        //实时求合计、检查人名合法性
        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "合格品数量")
                {
                    getTotal();
                }

                if (dataGridView2.Columns[e.ColumnIndex].Name == "生产时间")
                {
                    getTotal();
                }

                else if (dataGridView2.Columns[e.ColumnIndex].Name == "不良品数量")
                {
                    getTotal();
                }

                else if (dataGridView2.Columns[e.ColumnIndex].Name == "操作员")
                {
                    if (mySystem.Parameter.NametoID(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["操作员"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『操作员』信息", "ERROR");
                    }
                }
                else
                { }
            }
        }

        //数据绑定结束，设置表格格式
        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat();
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat1();
        }

        private void btn添加领料记录_Click(object sender, EventArgs e)
        {
            DataRow dr = dt领料记录.NewRow();
            dr = writeInnerDefault1(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt领料记录.Rows.InsertAt(dr, dt领料记录.Rows.Count);
            da领料记录.Update((DataTable)bs领料记录.DataSource);
            readInnerData1(Convert.ToInt32(dt记录.Rows[0]["ID"]));
            innerBind1();
        }

        private void btn删除领料记录_Click(object sender, EventArgs e)
        {
            if (dt领料记录.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //dt记录详情.Rows.RemoveAt(deletenum);
                dt领料记录.Rows[deletenum].Delete();
                // 保存
                da领料记录.Update((DataTable)bs领料记录.DataSource);
                readInnerData1(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind1();
                //求合计
                //getTotal();
            }
        }

        
    }
}
