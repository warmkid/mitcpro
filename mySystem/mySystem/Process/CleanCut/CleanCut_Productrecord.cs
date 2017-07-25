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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_Productrecord : BaseForm
    {
        private String table = "清洁分切生产记录";
        private String tableInfo = "清洁分切生产记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private CheckForm checkform = null;

        private DataTable dtusers, dt记录, dt记录详情, dt物料代码;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private string person_操作员;
        private string person_审核员;
        /// <summary>
        /// 登录人状态，0 操作员， 1 审核员， 2管理员
        /// </summary>
        private int stat_user;
        /// <summary>
        /// 窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        private int stat_form;

        public CleanCut_Productrecord(MainForm mainform) : base(mainform)
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

            setControlFalse();
            //查询条件可编辑
            dtp生产日期.Enabled = true;
            btn查询新建.Enabled = true;
            //打印、查看日志按钮不可用
            btn打印.Enabled = false;
            btn查看日志.Enabled = false;
        }

        public CleanCut_Productrecord(MainForm mainform, Int32 ID) : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

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
            dtusers = new DataTable("用户权限");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from 用户权限 where 步骤 = '清洁分切生产记录表'", connOle);
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

        //读取设置内容  //dt物料代码, 清洁前产品代码, 清洁前批号  //横向是否居中，判定是否合格，是否印刷状态添加“Yes”、“No”
        private void getOtherData()
        {
            //横向是否居中，判定是否合格，是否印刷状态添加“Yes”、“No”
            cmb横向是否居中.Items.Add("Yes");
            cmb横向是否居中.Items.Add("No");
            cmb判定是否合格.Items.Add("Yes");
            cmb判定是否合格.Items.Add("No");
            cmb是否印刷.Items.Add("Yes");
            cmb是否印刷.Items.Add("No");
            //*********物料代码*********//
            dt物料代码 = new DataTable("物料代码");            
            if (!isSqlOk)
            {
                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 清洁分切工序生产指令 where 生产指令编号 = '" + mySystem.Parameter.cleancutInstruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                 
                if (reader1.Read())
                {
                    //查找该生产ID下的产品编码、产品批号
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select ID, 清洁前产品代码, 清洁前批号 from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + reader1["ID"].ToString();
                    OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                    datemp.Fill(dt物料代码);
                    //if (dt物料代码.Rows.Count == 0)
                    //{
                    //    MessageBox.Show("该生产指令编码下的『清洁分切工序生产指令』尚未生成！");
                    //}
                    datemp.Dispose();
                }
                else
                {
                    //MessageBox.Show("该生产指令编码下的『生产指令信息表』尚未生成！");
                }
                reader1.Dispose();
            }
            else
            {
                //从SQL数据库中读取;                
            }
        }

        //根据状态设置可读写性
        private void setEnableReadOnly()
        {
            if (stat_user == 2)//管理员
            {
                //控件都能点
                setControlTrue();
            }
            else if (stat_user == 1)//审核人
            {
                if (stat_form == 0 || stat_form == 3 || stat_form == 2)  //0未保存||2审核通过||3审核未通过
                {
                    //控件都不能点，只有打印,日志可点
                    setControlFalse();
                }
                else //1待审核
                {
                    //发送审核不可点，其他都可点
                    setControlTrue();
                    btn审核.Enabled = true;
                }
            }
            else//操作员
            {
                if (stat_form == 1 || stat_form == 2) //1待审核||2审核通过
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
            tb审核人.Enabled = false;
            //部分空间防作弊，不可改
            tb生产指令编号.ReadOnly = true;
            cb白班.Enabled = false;
            cb夜班.Enabled = false;
            //查询条件始终不可编辑
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

        //显示根据信息查找
        private void DataShow(Int32 InstruID, Boolean flight, DateTime searchTime)
        {
            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID, flight, searchTime);
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
                readOuterData(InstruID, flight, searchTime);
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
                cb白班.Checked = Convert.ToBoolean(dt1.Rows[0]["生产班次"].ToString());
                cb夜班.Checked = !cb白班.Checked;
                DataShow(Convert.ToInt32(dt1.Rows[0]["生产指令ID"].ToString()), Convert.ToBoolean(dt1.Rows[0]["生产班次"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
            }
        }
        
        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID, Boolean flight, DateTime searchTime)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString() + " and 生产班次 = " + flight.ToString() + " and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            tb生产指令编号.DataBindings.Clear();
            tb生产指令编号.DataBindings.Add("Text", bs记录.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            cb白班.DataBindings.Clear();
            cb白班.DataBindings.Add("Checked", bs记录.DataSource, "生产班次");
            //中间
            tb实测宽度.DataBindings.Clear();
            tb实测宽度.DataBindings.Add("Text", bs记录.DataSource, "实测宽度");
            cmb是否印刷.DataBindings.Clear();
            cmb是否印刷.DataBindings.Add("Text", bs记录.DataSource, "是否印刷");
            cmb横向是否居中.DataBindings.Clear();
            cmb横向是否居中.DataBindings.Add("Text", bs记录.DataSource, "横向是否居中");
            tb纵向尺寸测量实际值.DataBindings.Clear();
            tb纵向尺寸测量实际值.DataBindings.Add("Text", bs记录.DataSource, "纵向尺寸测量实际值");
            cmb判定是否合格.DataBindings.Clear();
            cmb判定是否合格.DataBindings.Add("Text", bs记录.DataSource, "判定是否合格");
            tb废品重量.DataBindings.Clear();
            tb废品重量.DataBindings.Add("Text", bs记录.DataSource, "废品重量");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            //下面
            tb操作人.DataBindings.Clear();
            tb操作人.DataBindings.Add("Text", bs记录.DataSource, "操作人");
            tb操作员备注.DataBindings.Clear();
            tb操作员备注.DataBindings.Add("Text", bs记录.DataSource, "操作员备注");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp操作日期.DataBindings.Clear();
            dtp操作日期.DataBindings.Add("Text", bs记录.DataSource, "操作日期");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
                        
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            dr["生产指令编号"] = mySystem.Parameter.cleancutInstruction;
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["生产班次"] = cb白班.Checked;
            dr["是否印刷"] = "Yes";
            dr["横向是否居中"] = "Yes";
            dr["判定是否合格"] = "Yes";
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = Convert.ToDateTime(dtp操作日期.Value.ToString("yyyy/MM/dd"));
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(dtp审核日期.Value.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;
            return dr;
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T清洁分切生产记录ID = " + ID.ToString(), connOle);
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

        //添加行代码, 要求：dt物料代码.Rows.Count > 0
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            dr["序号"] = 0;
            dr["T清洁分切生产记录ID"] = ID;
            dr["物料代码"] = dt物料代码.Rows[0]["清洁前产品代码"].ToString();
            dr["膜卷批号"] = dt物料代码.Rows[0]["清洁前批号"].ToString();
            dr["清洁分切后代码"] = dr["物料代码"] + "C";
            dr["外观检查"] = "Yes";
            dr["长度A"] = 0;
            dr["长度B"] = 0;
            dr["重量"] = 0;
            dr["收率"] = -1;
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
                    case "外观检查":
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
                        cbc.MinimumWidth = 80;
                        break;
                    case "物料代码":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        for (int i = 0; i < dt物料代码.Rows.Count; i++)
                        { cbc.Items.Add(dt物料代码.Rows[i]["清洁前产品代码"]); }
                        dataGridView1.Columns.Add(cbc);
                        cbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                        cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cbc.MinimumWidth = 80;
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
                        tbc.MinimumWidth = 80;
                        break;
                }
            }
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T清洁分切生产记录ID"].Visible = false;
            dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["膜卷批号"].ReadOnly = true;
            dataGridView1.Columns["长度A"].HeaderText = "长度\r(m)";
            dataGridView1.Columns["重量"].HeaderText = "重量\r(kg)";
            dataGridView1.Columns["长度B"].HeaderText = "长度\r(m)";
            dataGridView1.Columns["收率"].ReadOnly = true;
            dataGridView1.Columns["收率"].HeaderText = "收率\r(%)";
            dataGridView1.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        //******************************按钮功能******************************//

        //用于显示/新建数据
        private void btn查询新建_Click(object sender, EventArgs e)
        {
            if (dt物料代码.Rows.Count > 0)
            { DataShow(mySystem.Parameter.cleancutInstruID, cb白班.Checked, dtp生产日期.Value); }
            else
            { MessageBox.Show("当前生产指令尚未设置完毕！"); }
            
        }

        //添加行按钮
        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            setDataGridViewRowNums();
        }

        //删除行按钮
        private void btn删除_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                //dt记录详情.Rows.RemoveAt(deletenum);
                dt记录详情.Rows[deletenum].Delete();
                // 保存
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                innerBind();
                setDataGridViewRowNums();
            }
        }

        //分切按钮
        private void btn分切_Click(object sender, EventArgs e)
        {
            DataRow dr = dt记录详情.NewRow();
            dr = writeInnerDefault(Convert.ToInt32(dt记录.Rows[0]["ID"]), dr);
            dr["序号"] = 0;
            dr["T清洁分切生产记录ID"] = dt记录.Rows[0]["ID"];
            dr["卷号"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["卷号"].ToString();
            dr["物料代码"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["物料代码"].ToString();
            dr["膜卷批号"] = dt记录详情.Rows[dataGridView1.CurrentRow.Index]["膜卷批号"].ToString();
            dr["清洁分切后代码"] = getCodeAfter(dr["物料代码"].ToString());
            dr["外观检查"] = "Yes";
            dr["长度A"] = 0;
            dr["长度B"] = 0;
            dr["重量"] = 0;
            dr["收率"] = -1;
            dt记录详情.Rows.InsertAt(dr, dataGridView1.CurrentRow.Index+1);
            setDataGridViewRowNums();
        }

        //保存按钮
        private void btn确认_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            //控件可见性
            if (stat_user == 0 && isSaved == true)
                btn提交审核.Enabled = true;
        }

        //保存功能
        private bool Save()
        {
            if (mySystem.Parameter.NametoID(tb操作人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『操作员』信息", "ERROR");
                return false;
            }
            else if (TextBox_check() == false)
            {
                /*宽度、尺寸、质量不合格*/
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
                readOuterData(mySystem.Parameter.cleancutInstruID, cb白班.Checked, dtp生产日期.Value);
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
                return;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切生产记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "清洁分切生产记录";
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
        }

        //查看日志按钮
        private void btn查看日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dt记录.Rows[0]["日志"].ToString());
        }

        //审核功能
        public override void CheckResult()
        {
            //保存
            bool isSaved = Save();
            if (isSaved == false)
                return;

            base.CheckResult();

            dt记录.Rows[0]["审核人"] = Parameter.IDtoName(checkform.userID);
            dt记录.Rows[0]["审核意见"] = checkform.opinion;
            dt记录.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter("select * from 待审核 where 表名='清洁分切生产记录' and 对应ID=" + dt记录.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion + "\n";
            dt记录.Rows[0]["日志"] = dt记录.Rows[0]["日志"].ToString() + log;

            Save();

            //修改状态，设置可控性
            if (checkform.ischeckOk)
            { stat_form = 2; }//审核通过
            else
            { stat_form = 3; }//审核未通过            
            setEnableReadOnly();
        }

        //审核按钮
        private void btn审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        //打印按钮 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!        
        private void btn打印_Click(object sender, EventArgs e)
        {

        }

        //******************************小功能******************************//  
        
        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb实测宽度, tb纵向尺寸测量实际值, tb废品重量 });
            List<String> StringList = new List<String>(new String[] { "实测宽度", "纵向尺寸测量实际值", "废品重量" });
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
            return TypeCheck;
        }

        //正则表达式，检查分切后格式
        private bool CheckCodeFormat(String Code)
        {
            //eg:TY-200*300*3C
            String pattern = @"^[a-zA-Z]+-[0-9]+\*[0-9]+\*[0-9]+C";//正则表达式
            if (!Regex.IsMatch(Code, pattern))
            {
                MessageBox.Show("清洁分切后代码格式不符合规定，重新输入，例如 TY-200*300*3C");
                return false;
            }
            return true;
        } 

        //求出分切后的清洁分切后代码
        private String getCodeAfter(String codeBefore)
        {
            String codeAfter;
            Int32 leng;
            //eg: codeBefore = TY-200*300*3
            //String pattern = @"^[a-zA-Z]+-[0-9]+\*[0-9]+\*[0-9]";//正则表达式
            string[] array1 = codeBefore.Split('-'); //array1[0]=TY array1[1]=200*300*3
            string[] array2 = array1[1].Split('*'); //array2[0]=200 array2[1]=300 array2[2]=3
            leng = Int32.Parse(array2[0]);
            codeAfter = array1[0] + "-0*" + array2[1] + "*" + array2[2]+"C";
            return codeAfter; //TY-0*300*3C
        }

        //实时求收率
        private void getPercent(Int32 Rownum)
        {
            int len1;
            int len2;
            // 膜卷长度求和
            if ((Int32.TryParse(dt记录详情.Rows[Rownum]["长度A"].ToString(), out len1) == true) && (Int32.TryParse(dt记录详情.Rows[Rownum]["长度B"].ToString(), out len2) == true))
            {
                //均为数值类型
                if (len1 == 0)
                { dt记录详情.Rows[Rownum]["收率"] = -1;  }
                else
                { dt记录详情.Rows[Rownum]["收率"] = (Int32)((Double)len2/(Double)len1 * 100); }
            }
            else
            { dt记录详情.Rows[Rownum]["收率"] = -1; }
            
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

        //实时求收率
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "物料代码")
                {
                    //实时根据“物料代码”，显示“膜卷批号”
                    //dt记录详情.Rows[e.RowIndex]["膜卷批号"] = dt物料代码.Rows[];
                    ComboBox cbc = new ComboBox();
                    for (int i = 0; i < dt物料代码.Rows.Count; i++)
                    { cbc.Items.Add(dt物料代码.Rows[i]["清洁前产品代码"]); }
                    Int32 index = cbc.FindString(dt记录详情.Rows[e.RowIndex]["物料代码"].ToString());
                    dt记录详情.Rows[e.RowIndex]["膜卷批号"] = dt物料代码.Rows[index]["清洁前批号"].ToString();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "长度A")
                {
                    //实时求“收率”
                    getPercent(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "长度B")
                {
                    //实时求“收率”
                    getPercent(e.RowIndex);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "清洁分切后代码")
                {
                    //实时检查“清洁分切后代码”格式是否合格
                    if (CheckCodeFormat(dt记录详情.Rows[e.RowIndex]["清洁分切后代码"].ToString()) == false)
                    { dt记录详情.Rows[e.RowIndex]["清洁分切后代码"] = getCodeAfter(dt记录详情.Rows[e.RowIndex]["物料代码"].ToString()); }
                }
                else
                { }
            }
        }

        //数据绑定结束，设置表格格式
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat();
        }

    }
}
