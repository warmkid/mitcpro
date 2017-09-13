using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections;

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVBag_dailyreport : BaseForm
    {
        private OleDbConnection connOle = Parameter.connOle;
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Int32 ID生产指令;
        String str生产指令编码;
        Int32 i日报表行数;
        
        //界面显示所需信息
        DateTime date生产日期=DateTime.Now;
        String str班次="";
        
        
        Int32 i入库量只=0;
        Double i效率=0.0;
        Double i成品宽 = 0.0;
        Double i成品长 = 0.0;
        Double i成品数量 = 0.0;
        Double i膜材1用量米 = 0.0;
        Double i膜材1用量平方米 = 0.0;
        Double i膜材2用量米 = 0.0;
        Double i膜材2用量平方米 = 0.0;
        Double i制袋收率 = 0.0;

        private DataTable dt日报表详细信息, dt日报表, dt生产指令, dt生产指令详细信息, dt领料, dt领料详细信息1, dt领料详细信息2, dt内包装, dt内包装详细信息, dt产品外观, dt产品外观详细信息;
        private BindingSource bs日报表详细信息, bs日报表, bs生产指令, bs生产指令详细信息, bs领料, bs领料详细信息1, bs领料详细信息2, bs内包装, bs内包装详细信息, bs产品外观, bs产品外观详细信息;
        private OleDbDataAdapter da日报表详细信息, da日报表, da生产指令, da生产指令详细信息, da领料, da领料详细信息1, da领料详细信息2, da内包装, da内包装详细信息, da产品外观, da产品外观详细信息;
        private OleDbCommandBuilder cb日报表详细信息, cb日报表, cb生产指令, cb生产指令详细信息, cb领料, cb领料详细信息1, cb领料详细信息2, cb内包装, cb内包装详细信息, cb产品外观, cb产品外观详细信息;

        // ly
        String str产品代码 = "";
        String str产品批号 = "";
        String str客户或订单号 = "";
        String str膜1 = "";
        String str膜2 = "";
        Hashtable ht内包ID数量 = new Hashtable();
        //
       
        public PTVBag_dailyreport(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            ID生产指令 = mySystem.Parameter.ptvbagInstruID;
            str生产指令编码 = mySystem.Parameter.ptvbagInstruction;
            bt打印.Enabled = false;

            getPeople();
            setUserState();
            getOtherData();

            readOuterData(ID生产指令);
            outerBind();

            if (dt日报表.Rows.Count == 0)
            {
                DataRow dr = dt日报表.NewRow();
                dr = writeOuterDefault(dr);
                dt日报表.Rows.Add(dr);
                da日报表.Update((DataTable)bs日报表.DataSource);
                readOuterData(ID生产指令);
                outerBind();
                //DataRow dr内表 = dt日报表详细信息.NewRow();
                //dr内表 = writeInnerDefault(dr内表);
                //dt日报表详细信息.Rows.Add(dr内表);
                //writeInnerData();
                //da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            }

            readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
            innerBind();
            fillAndCheckInner();
            //if (dt日报表详细信息.Rows.Count == 0)
            //{
            //    DataRow drinner = dt日报表详细信息.NewRow();
            //    drinner = writeInnerDefault(drinner);
            //    dt日报表详细信息.Rows.Add(drinner);
            //    da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            //    readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
            //    innerBind();
            //}          
            //if (dt日报表详细信息.Rows.Count == 0)
            //{
            //    //DataRow dr内表 = dt日报表详细信息.NewRow();
            //    //dr内表 = writeInnerDefault(dr内表);
            //    //dt日报表详细信息.Rows.Add(dr内表);
            //    writeInnerData();
            //    da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            //}

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();

            dataGridView1.Columns.Clear();
            setDataGridViewColumns();
            setDataGridViewRowNums();
            
        }

        public PTVBag_dailyreport(MainForm mainform, int id)
            : base(mainform)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from CS制袋日报表 where ID=" + id, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ID生产指令 = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            str生产指令编码 = dt.Rows[0]["生产指令编号"].ToString();
            InitializeComponent();
            //ID生产指令 = mySystem.Parameter.csbagInstruID;
            //str生产指令编码 = mySystem.Parameter.csbagInstruction;
            bt打印.Enabled = false;

            getPeople();
            setUserState();
            getOtherData();

            readOuterData(ID生产指令);
            outerBind();

            if (dt日报表.Rows.Count == 0)
            {
                DataRow dr = dt日报表.NewRow();
                dr = writeOuterDefault(dr);
                dt日报表.Rows.Add(dr);
                da日报表.Update((DataTable)bs日报表.DataSource);
                readOuterData(ID生产指令);
                outerBind();
                //DataRow dr内表 = dt日报表详细信息.NewRow();
                //dr内表 = writeInnerDefault(dr内表);
                //dt日报表详细信息.Rows.Add(dr内表);
                //writeInnerData();
                //da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            }

            readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
            innerBind();
            fillAndCheckInner();
            //if (dt日报表详细信息.Rows.Count == 0)
            //{
            //    DataRow drinner = dt日报表详细信息.NewRow();
            //    drinner = writeInnerDefault(drinner);
            //    dt日报表详细信息.Rows.Add(drinner);
            //    da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            //    readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
            //    innerBind();
            //}          
            //if (dt日报表详细信息.Rows.Count == 0)
            //{
            //    //DataRow dr内表 = dt日报表详细信息.NewRow();
            //    //dr内表 = writeInnerDefault(dr内表);
            //    //dt日报表详细信息.Rows.Add(dr内表);
            //    writeInnerData();
            //    da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            //}

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();

            dataGridView1.Columns.Clear();
            setDataGridViewColumns();
            setDataGridViewRowNums();
        }

        private void fillAndCheckInner()
        {
            // 从内表中得到它已经有的内包装ID
            HashSet<int> innerPackageIDs = new HashSet<int>();
            foreach (DataRow dr in dt日报表详细信息.Rows)
            {
                innerPackageIDs.Add(Convert.ToInt32(dr["T产品内包装记录ID"]));
            }
            // 遍历hashtable，如果ID已经有了，则跳过，否则就填值
            foreach (int id in ht内包ID数量.Keys.OfType<int>().ToList<int>())
            {
                if (innerPackageIDs.Contains(id)) continue;
                DataRow ndr = dt日报表详细信息.NewRow();
                ndr["T产品内包装记录ID"] = id;
                ndr["TCS制袋日报表ID"] = Convert.ToInt32(dt日报表.Rows[0]["ID"]);
                ndr["生产日期"] = Convert.ToDateTime(  ((List<object> )ht内包ID数量[id])[1] );
                ndr["班次"] = str班次;
                ndr["客户或订单号"] = str客户或订单号;
                ndr["产品代码"] = str产品代码;
                ndr["批号"] = str产品批号;
                ndr["入库量只A"] = i入库量只;
                dt日报表详细信息.Rows.Add(ndr);
            }
            da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
            readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
            innerBind();
        }

        private void Chart_daily_cs_Load(object sender, EventArgs e)
        {

        }

        // 获取其他需要的数据，内包装：生产日期，产品数量只
        //生产指令：客户或订单号，产品代码，产品批号，制袋物料1，制袋物料2
        //领料量：膜材1和膜材2领取数量
        //产品外观和尺寸检验记录：检查员班次，成品宽和长
        private void getOtherData()
        {
            //读取生产指令外表
            da生产指令 = new OleDbDataAdapter("select * from 生产指令 where ID="+ID生产指令, mySystem.Parameter.connOle);
            cb生产指令 = new OleDbCommandBuilder(da生产指令);
            dt生产指令 = new DataTable("生产指令");
            bs生产指令 = new BindingSource();
            da生产指令.Fill(dt生产指令);
            //DataTable dt生产指令所需信息 = dt生产指令.DefaultView.ToTable(false, new string[] {"ID", "计划生产日期", "生产指令编号" ,"制袋物料名称1","制袋物料名称2"});
            //Int32 i生产指令外表ID = Convert.ToInt32(dt生产指令所需信息.Rows[0]["ID"].ToString());
            //根据生产指令外表中的制袋物料名称，读取领料量中对应物料的使用量C
            //String str制袋物料名称1 = dt生产指令所需信息.Rows[0]["制袋物料名称1"].ToString();
            //String str制袋物料名称2 = dt生产指令所需信息.Rows[0]["制袋物料名称2"].ToString();
            str膜1 = dt生产指令.Rows[0]["制袋物料名称1"].ToString();
            str膜2 = dt生产指令.Rows[0]["制袋物料名称2"].ToString();

            //date生产日期 =Convert.ToDateTime(dt生产指令所需信息.Rows[0]["计划生产日期"].ToString());

            //读取生产指令内表
            da生产指令详细信息 = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + ID生产指令, mySystem.Parameter.connOle);
            cb生产指令详细信息 = new OleDbCommandBuilder(da生产指令详细信息);
            dt生产指令详细信息 = new DataTable("生产指令详细信息");
            bs生产指令详细信息 = new BindingSource();
            da生产指令详细信息.Fill(dt生产指令详细信息);
            DataTable dt生产指令所需信息详细 = dt生产指令详细信息.DefaultView.ToTable(false, new string[] { "产品代码", "产品批号", "客户或订单号" });

            str产品代码 = dt生产指令详细信息.Rows[0]["产品代码"].ToString();
            str产品批号 = dt生产指令详细信息.Rows[0]["产品批号"].ToString();
            str客户或订单号 = dt生产指令详细信息.Rows[0]["客户或订单号"].ToString();

            //读取领料量外表
            da领料 = new OleDbDataAdapter("select * from CS制袋领料记录 where 生产指令ID=" + ID生产指令, mySystem.Parameter.connOle);
            cb领料 = new OleDbCommandBuilder(da领料);
            dt领料 = new DataTable("领料");
            bs领料 = new BindingSource();
            da领料.Fill(dt领料);
            DataTable dt领料所需信息 = dt领料.DefaultView.ToTable(false, new string[] { "ID" });
            Int32 i领料量外表ID = Convert.ToInt32(dt领料所需信息.Rows[0]["ID"].ToString());

            string str制袋物料名称1 = str膜1;
            string str制袋物料名称2 = str膜2;
            //读取领料量内表
            da领料详细信息1 = new OleDbDataAdapter("select * from CS制袋领料记录详细记录 where TCS制袋领料记录ID=" + i领料量外表ID + "and 物料简称='" + str制袋物料名称1 + "'", mySystem.Parameter.connOle);
            cb领料详细信息1 = new OleDbCommandBuilder(da领料详细信息1);
            dt领料详细信息1 = new DataTable("领料详细信息1");
            bs领料详细信息1 = new BindingSource();
            da领料详细信息1.Fill(dt领料详细信息1);
            DataTable dt领料所需信息详细1 = dt领料详细信息1.DefaultView.ToTable(false, new string[] { "物料简称", "使用数量C" });

            da领料详细信息2 = new OleDbDataAdapter("select * from CS制袋领料记录详细记录 where TCS制袋领料记录ID=" + i领料量外表ID + "and 物料简称='" + str制袋物料名称2 + "'", mySystem.Parameter.connOle);
            cb领料详细信息2 = new OleDbCommandBuilder(da领料详细信息2);
            dt领料详细信息2 = new DataTable("领料详细信息1");
            bs领料详细信息2 = new BindingSource();
            da领料详细信息2.Fill(dt领料详细信息2);
            DataTable dt领料所需信息详细2 = dt领料详细信息2.DefaultView.ToTable(false, new string[] { "物料简称", "使用数量C" });
             
            
            //读取内包装外表
            da内包装 = new OleDbDataAdapter("select * from 产品内包装记录 where 生产指令ID="+ID生产指令, mySystem.Parameter.connOle);
            cb内包装 = new OleDbCommandBuilder(da内包装);
            dt内包装 = new DataTable("内包装");
            bs内包装 = new BindingSource();
            da内包装.Fill(dt内包装);
            DataTable dt内包装所需信息 = dt内包装.DefaultView.ToTable(false, new string[] {"ID", "产品数量只数合计B"});
            Int32 i内包装外表ID = Convert.ToInt32(dt内包装所需信息.Rows[0]["ID"].ToString());
            i日报表行数 = dt内包装所需信息.Rows.Count;

            i入库量只 =Convert.ToInt32(dt内包装所需信息.Rows[0]["产品数量只数合计B"].ToString());
            
            //读取内包装内表
            da内包装详细信息 = new OleDbDataAdapter("select * from 产品内包装详细记录 where T产品内包装记录ID=" + i内包装外表ID, mySystem.Parameter.connOle);
            cb内包装详细信息 = new OleDbCommandBuilder(da内包装详细信息);
            dt内包装详细信息 = new DataTable("内包装详细信息");
            bs内包装详细信息 = new BindingSource();
            da内包装详细信息.Fill(dt内包装详细信息);
            DataTable dt内包装所需信息详细 = dt内包装详细信息.DefaultView.ToTable(false, new string[] { "生产开始时间", "生产结束时间" });
            date生产日期 = Convert.ToDateTime(dt内包装所需信息详细.Rows[0]["生产开始时间"].ToString());

            foreach (DataRow dr in dt内包装.Rows)
            {
                ht内包ID数量.Add(Convert.ToInt32(dr["ID"]), new List<object>(new object[]{ 
                    Convert.ToInt32(dr["产品数量只数合计B"]),date生产日期}
            ));
            }
                        
            //读取产品外观和尺寸检验记录外表
            da产品外观 = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录 where 生产指令ID="+ID生产指令,mySystem.Parameter.connOle);
            cb产品外观 = new OleDbCommandBuilder(da产品外观);
            dt产品外观 = new DataTable("产品外观");
            bs产品外观 = new BindingSource();
            da产品外观.Fill(dt产品外观);
            DataTable dt产品外观所需信息 = dt产品外观.DefaultView.ToTable(false, new string[] {"ID", "操作员", "尺寸规格宽", "尺寸规格长" });
            Int32 i产品外观外表ID = Convert.ToInt32(dt产品外观所需信息.Rows[0]["ID"].ToString());

            i成品宽 = Convert.ToDouble(dt产品外观所需信息.Rows[0]["尺寸规格宽"].ToString());
            i成品长 = Convert.ToDouble(dt产品外观所需信息.Rows[0]["尺寸规格长"].ToString());

            String str检查员 = dt产品外观所需信息.Rows[0]["操作员"].ToString();
            //读取检查员对应的班次
            OleDbCommand comm班次 = new OleDbCommand();
            comm班次.Connection = mySystem.Parameter.connOle;
            comm班次.CommandText = "select * from 用户 where 用户名= @name";
            comm班次.Parameters.AddWithValue("@name", str检查员);

            OleDbDataReader myReader班次 = comm班次.ExecuteReader();
            while (myReader班次.Read())
            {
                str班次 = myReader班次["班次"].ToString();
                //List<String> list班次 = new List<string>();
                //list班次.Add(myReader班次["班次"].ToString());
            }

            myReader班次.Close();
            comm班次.Dispose();  

            //读取产品外观和尺寸检验记录内表
            da产品外观详细信息 = new OleDbDataAdapter("select * from 产品外观和尺寸检验记录详细信息 where T产品外观和尺寸检验记录ID=" + i产品外观外表ID, mySystem.Parameter.connOle);
            cb产品外观详细信息 = new OleDbCommandBuilder(da产品外观详细信息);
            dt产品外观详细信息 = new DataTable("产品外观详细信息");
            bs产品外观详细信息 = new BindingSource();
            da产品外观详细信息.Fill(dt产品外观详细信息);
            DataTable dt产品外观详细信息所需信息 = dt产品外观详细信息.DefaultView.ToTable(false, new string[] {  });

          //  mySystem.Parameter.connOle.Dispose();
           // MessageBox.Show(str生产指令编码);

            //添加打印机
            fill_printer();
        }

        // 读取数据，无参数表示从Paramter中读取数据
        private void readOuterData(int id)
        {
            da日报表 = new OleDbDataAdapter("select * from CS制袋日报表 where 生产指令ID="+id, mySystem.Parameter.connOle);
            cb日报表 = new OleDbCommandBuilder(da日报表);
            dt日报表 = new DataTable("CS制袋日报表");
            bs日报表 = new BindingSource();
            da日报表.Fill(dt日报表);
        }
        // 外表和控件的绑定
        private void outerBind()
        {
            bs日报表.DataSource = dt日报表;

        }
        // 根据条件从数据库中读取多行内表数据
        private void readInnerData(int outerid)
        {
            //            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
            //                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
            //            OleDbConnection connOle = new OleDbConnection(strConn);
            //            connOle.Open();
            dt日报表详细信息 = new DataTable("CS制袋日报表详细信息");
            bs日报表详细信息 = new BindingSource();
            da日报表详细信息 = new OleDbDataAdapter(@"select * from CS制袋日报表详细信息 where TCS制袋日报表ID="+outerid, mySystem.Parameter.connOle);
            cb日报表详细信息 = new OleDbCommandBuilder(da日报表详细信息);
            da日报表详细信息.Fill(dt日报表详细信息);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);

            bs日报表详细信息.DataSource = dt日报表详细信息;
            dataGridView1.DataSource = bs日报表详细信息.DataSource;

        }
        // 设置自动计算类事件
        private void addComputerEventHandler()
        {
          //  DataGridViewsum();
        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        private void setFormState()
        {

        }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setEnableReadOnly()
        {

        }
        // 其他事件，比如按钮的点击，数据有效性判断
        private void addOtherEventHandler()
        {
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        }
        // 获取操作员和审核员
        private void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='CS制袋日报表'", connOle);
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
                bt打印.Enabled = true;
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;
                bt打印.Enabled = true;
            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (Parameter.UserState.审核员 == _userState)
            { 
                label角色.Text = "审核员";
                bt打印.Enabled = true;
            }
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        private void setDataGridViewColumns()
        {          
            DataGridViewTextBoxColumn c2;
            foreach (DataColumn dc in dt日报表详细信息.Columns)
            {
                switch (dc.ColumnName)
                {                  
                      default:
                        c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
            setDataGridViewFormat();
        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["入库量只A"].HeaderText = "入库量(只)";
            dataGridView1.Columns["工时B"].HeaderText = "工时/h";
            dataGridView1.Columns["系数C"].HeaderText = "系数";
            dataGridView1.Columns["成品宽D"].HeaderText = "宽/mm";
            dataGridView1.Columns["成品长E"].HeaderText = "长/mm";
            dataGridView1.Columns["成品数量W"].HeaderText = "成品数量/㎡";
            dataGridView1.Columns["膜材1规格F"].HeaderText = "膜材1规格/mm";
            dataGridView1.Columns["膜材1用量G"].HeaderText = "膜材1用量/mm";
            dataGridView1.Columns["膜材1用量E"].HeaderText = "膜材1用量/㎡";
            dataGridView1.Columns["膜材2规格H"].HeaderText = "膜材2规格/mm";
            dataGridView1.Columns["膜材2用量K"].HeaderText = "膜材2用量/mm";
            dataGridView1.Columns["膜材2用量R"].HeaderText = "膜材2用量/㎡";
            dataGridView1.Columns["制袋收率"].HeaderText = "制袋收率（%）";
            //第一列ID不显示
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["TCS制袋日报表ID"].Visible = false;
        }

        //填写数据类型错误提示
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            int columnindex = ((DataGridView)sender).SelectedCells[0].ColumnIndex;
            String Columnsname = ((DataGridView)sender).Columns[columnindex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString();
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");

            if (Columnsname == "登记员" || Columnsname == "审核员")
            {
                string str人员 = dt日报表详细信息.Rows[columnindex][rowsname].ToString();
                if (mySystem.Parameter.NametoID(str人员) <= 0)
                    MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
            }

        }

        //根据公式计算
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           // Int32[] rowcount = { 0, 1, 2, 3 };
            double 入库量只, 工时B, 系数C, 成品宽D, 成品长E, 膜材1规格F, 膜材1用量G, 膜材2规格H, 膜材2用量K;
            if (!Double.TryParse(dataGridView1[8, e.RowIndex].Value.ToString(), out 入库量只))
            {
                入库量只 = 0;
            }
            if (!Double.TryParse(dataGridView1[9, e.RowIndex].Value.ToString(), out 工时B))
            {
                工时B = 0;
            }
            if (!Double.TryParse(dataGridView1[10, e.RowIndex].Value.ToString(), out 系数C))
            {
                系数C = 0;
            }

            if (!Double.TryParse(dataGridView1[12, e.RowIndex].Value.ToString(), out 成品宽D))
            {
                成品宽D = 0;
            }
            if (!Double.TryParse(dataGridView1[13, e.RowIndex].Value.ToString(), out 成品长E))
            {
                成品长E = 0;
            }

            if (!Double.TryParse(dataGridView1[15, e.RowIndex].Value.ToString(), out 膜材1规格F))
            {
                膜材1规格F = 0;
            }
            if (!Double.TryParse(dataGridView1[16, e.RowIndex].Value.ToString(), out 膜材1用量G))
            {
                膜材1用量G = 0;
            }

            if (!Double.TryParse(dataGridView1[18, e.RowIndex].Value.ToString(), out 膜材2规格H))
            {
                膜材2规格H = 0;
            }
            if (!Double.TryParse(dataGridView1[19, e.RowIndex].Value.ToString(), out 膜材2用量K))
            {
                膜材2用量K = 0;
            }

            try
            {
                double 效率 = 入库量只 * 系数C / 工时B;
                double 数量 = 入库量只 * 成品宽D * 成品长E / 1000000 * 2;
                double 用量1 = 膜材1规格F * 膜材1用量G / 1000;
                double 用量2 = 膜材2规格H * 膜材2用量K / 1000;
                double 收率 = 数量 / (用量1 + 用量2);
                dataGridView1[11, e.RowIndex].Value = 效率;
                dataGridView1[14, e.RowIndex].Value = 数量;
                dataGridView1[17, e.RowIndex].Value = 用量1;
                dataGridView1[20, e.RowIndex].Value = 用量2;
                dataGridView1[21, e.RowIndex].Value = 收率;

            }
            catch (DivideByZeroException ee)
            {
            }
             
            
            //foreach (DataRow dr in dt日报表详细信息.Rows)
            //{
            //    if (Convert.ToInt32(dr["工时B"].ToString()) == 0)
            //    {
            //        i效率 = 0;
            //        dr["效率"] = i效率;
            //    }
            //    else
            //        i效率 = Convert.ToInt32(dr["入库量只A"].ToString()) * Convert.ToDouble(dr["系数C"].ToString()) / Convert.ToDouble(dr["工时B"].ToString());


            //    i成品数量 = Convert.ToInt32(dr["入库量只A"].ToString()) * Convert.ToDouble(dr["成品宽D"].ToString()) * Convert.ToDouble(dr["成品长E"].ToString()) / 1000000 * 2;
            //    i膜材1用量平方米 = Convert.ToDouble(dr["膜材1规格F"].ToString()) * Convert.ToDouble(dr["膜材1用量G"].ToString()) / 1000;
            //    i膜材2用量平方米 = Convert.ToDouble(dr["膜材2规格H"].ToString()) * Convert.ToDouble(dr["膜材2用量K"].ToString()) / 1000;
            //    if ((i膜材1用量平方米 + i膜材2用量平方米) != 0)
            //    {
            //        i制袋收率 = i成品数量 / (i膜材1用量平方米 + i膜材2用量平方米);
            //    }
            //    dr["效率"] = i效率;
            //    dr["成品数量W"] = i成品数量;
            //    dr["膜材1用量E"] = i膜材1用量平方米;
            //    dr["膜材2用量R"] = i膜材2用量平方米;
            //    dr["制袋收率"] = i制袋收率;

            //    //dt日报表详细信息.Rows[i]["效率"] = i效率;
            //    //dt日报表详细信息.Rows[i]["成品数量W"] = i成品数量;
            //    //dt日报表详细信息.Rows[i]["膜材1用量E"] = i膜材1用量;
            //    //dt日报表详细信息.Rows[i]["膜材2用量R"] = i膜材2用量;
            //    //dt日报表详细信息.Rows[i]["制袋收率"] = i制袋收率;
                
            //}
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T产品内包装记录ID"].Visible = false;
        }

        //写日报表外表数据
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = ID生产指令;
            dr["日志"] = "";
            dr["审核员"] = "";
            dr["生产指令编号"] = str生产指令编码;
            return dr;
        }

        //写日报表内表数据
        DataRow writeInnerDefault(DataRow dr)
        {
           
            dr["TCS制袋日报表ID"] = dt日报表.Rows[0]["ID"];
            dr["生产日期"] = DateTime.Now;
            dr["班次"] = str班次;
            dr["客户或订单号"] = str客户或订单号;
            dr["产品代码"] = str产品代码;
            dr["批号"] = str产品批号;
            dr["入库量只A"] = i入库量只;
            dr["工时B"] = 0.0;
            dr["系数C"] = 0.0;
            dr["效率"] = i效率;
            dr["成品宽D"] = i成品长;
            dr["成品长E"] = i成品宽;
            dr["成品数量W"] = i成品数量;
            
            dr["膜材1规格F"] = 0.0;
            dr["膜材1用量G"] = 0.0;
            dr["膜材1用量E"] = i膜材1用量平方米;
            dr["膜材2规格H"] = 0.0;
            dr["膜材2用量K"] = 0.0;
            dr["膜材2用量R"] = i膜材2用量平方米;
            dr["制袋收率"] = i制袋收率;
            
            return dr;
        }

        void writeInnerData()
        {
            bool issameday;
            DataTable dt内包装所需信息详细 = dt内包装详细信息.DefaultView.ToTable(false, new string[] { "生产开始时间", "生产结束时间" });
            DataTable dt领料所需信息详细1 = dt领料详细信息1.DefaultView.ToTable(false, new string[] { "领料日期时间", "物料简称", "使用数量C" });
            DataTable dt领料所需信息详细2 = dt领料详细信息2.DefaultView.ToTable(false, new string[] { "领料日期时间", "物料简称", "使用数量C" });
            for (int i = 0; i < i日报表行数; i++)
            {
                //以内包装为准，显示信息
                DataRow dr内表 = dt日报表详细信息.NewRow();
                dr内表 = writeInnerDefault(dr内表);
                dt日报表详细信息.Rows.Add(dr内表);
                date生产日期 = Convert.ToDateTime(dt内包装所需信息详细.Rows[i]["生产开始时间"].ToString());
                dt日报表详细信息.Rows[i]["生产日期"] = date生产日期;
                dt日报表详细信息.Rows[i]["班次"] = str班次;
                dt日报表详细信息.Rows[i]["客户或订单号"] = str客户或订单号;
                dt日报表详细信息.Rows[i]["产品代码"] = str产品代码;
                dt日报表详细信息.Rows[i]["批号"] = str产品批号;
                dt日报表详细信息.Rows[i]["入库量只A"] = i入库量只;
                dt日报表详细信息.Rows[i]["工时B"] = 0.0;
                dt日报表详细信息.Rows[i]["系数C"] = 0.0;
                dt日报表详细信息.Rows[i]["效率"] = i效率;
                dt日报表详细信息.Rows[i]["成品宽D"] = i成品长;
                dt日报表详细信息.Rows[i]["成品长E"] = i成品宽;
                dt日报表详细信息.Rows[i]["成品数量W"] = i成品数量;
                dt日报表详细信息.Rows[i]["膜材1规格F"] = 0.0;
                dt日报表详细信息.Rows[i]["膜材2规格H"] = 0.0;

                //判断内包装记录日期下，有无领料记录，若有，填写对应信息，若无，填0
                //若内包装行数小于等于领料量行数，循环，判断内包装行数范围内，日期是否相同
                
                int i领料量1行数 = dt领料详细信息1.Rows.Count;
                if (i日报表行数 <= i领料量1行数)
                {
                  //  for (int j = 0; j <= i日报表行数; j++)
                  //  {
                    DateTime date领料1日期 = Convert.ToDateTime(dt领料所需信息详细1.Rows[i]["领料日期时间"].ToString());
                        if (date生产日期.ToString("yyyy-MM-dd") == date领料1日期.ToString("yyyy-MM-dd"))
                            issameday = true;
                        else
                            issameday = false;
                        if (issameday)
                        {
                            i膜材1用量米 = Convert.ToDouble(dt领料详细信息1.Rows[i]["使用数量C"].ToString());
                            // i膜材2用量米 = Convert.ToDouble(dt领料详细信息2.Rows[i]["使用数量C"].ToString());
                            dt日报表详细信息.Rows[i]["膜材1用量G"] = i膜材1用量米;
                            // dt日报表详细信息.Rows[i]["膜材2用量K"] = i膜材2用量米;
                        }
                        else
                        {
                            dt日报表详细信息.Rows[i]["膜材1用量G"] = 0.0;
                            //  dt日报表详细信息.Rows[j]["膜材2用量K"] = 0.0;
                        }
                  //  }
                   
                }
                //若内包装行数大于领料量行数，循环到领料量行数即可，内包装剩余行默认写0
                else 
                {
                    if (i < i领料量1行数)
                    {
                        DateTime date领料1日期 = Convert.ToDateTime(dt领料所需信息详细1.Rows[i]["领料日期时间"].ToString());
                        if (date生产日期.ToString("yyyy-MM-dd") == date领料1日期.ToString("yyyy-MM-dd"))
                            issameday = true;
                        else
                            issameday = false;
                        if (issameday)
                        {
                            i膜材1用量米 = Convert.ToDouble(dt领料详细信息1.Rows[i]["使用数量C"].ToString());
                            dt日报表详细信息.Rows[i]["膜材1用量G"] = i膜材1用量米;
                        }
                        else
                        {
                            dt日报表详细信息.Rows[i]["膜材1用量G"] = 0.0;
                        }
                    }
                    else
                    {
                        dt日报表详细信息.Rows[i]["膜材1用量G"] = 0.0;
                    }
                }

                int i领料量2行数 = dt领料详细信息2.Rows.Count;
                if (i日报表行数 <= i领料量2行数)
                {
                    DateTime date领料2日期 = Convert.ToDateTime(dt领料所需信息详细2.Rows[i]["领料日期时间"].ToString());
                    if (date生产日期.ToString("yyyy-MM-dd") == date领料2日期.ToString("yyyy-MM-dd"))
                        issameday = true;
                    else
                        issameday = false;
                    if (issameday)
                    {
                        i膜材2用量米 = Convert.ToDouble(dt领料详细信息2.Rows[i]["使用数量C"].ToString());
                        dt日报表详细信息.Rows[i]["膜材2用量K"] = i膜材2用量米;
                    }
                    else
                    {
                        dt日报表详细信息.Rows[i]["膜材2用量K"] = 0.0;
                    }

                }
                //若内包装行数大于领料量行数，循环到领料量行数即可，内包装剩余行默认写0
                else
                {
                    if (i < i领料量2行数)
                    {
                        DateTime date领料2日期 = Convert.ToDateTime(dt领料所需信息详细2.Rows[i]["领料日期时间"].ToString());
                        if (date生产日期.ToString("yyyy-MM-dd") == date领料2日期.ToString("yyyy-MM-dd"))
                            issameday = true;
                        else
                            issameday = false;
                        if (issameday)
                        {
                            i膜材2用量米 = Convert.ToDouble(dt领料详细信息2.Rows[i]["使用数量C"].ToString());
                            dt日报表详细信息.Rows[i]["膜材2用量K"] = i膜材2用量米;
                        }
                        else
                        {
                            dt日报表详细信息.Rows[i]["膜材2用量K"] = 0.0;
                        }
                    }
                    else
                    {
                        dt日报表详细信息.Rows[i]["膜材2用量K"] = 0.0;
                    }
                }

                dt日报表详细信息.Rows[i]["膜材1用量E"] = i膜材1用量平方米;
                dt日报表详细信息.Rows[i]["膜材2用量R"] = i膜材2用量平方米;
                dt日报表详细信息.Rows[i]["制袋收率"] = i制袋收率;
            }

        }

        //设置序号递增
        void setDataGridViewRowNums() 
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\CSBag\SOP-MFG-303-R06A  CS制袋日报表.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];


            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 修改Sheet中某行某列的值
                my = printValue(my, wb);
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                //false->打印
                // 设置该进程是否可见
                oXL.Visible = false;
                // 修改Sheet中某行某列的值
                my = printValue(my, wb);
                try
                {
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                { }
                finally
                {
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                }
            }
        }

        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {          
            int rownum = dt日报表详细信息.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[i + 5, 1].Value = Convert.ToDateTime(dt日报表详细信息.Rows[i]["生产日期"]).ToString("D");//去掉时分秒，且显示为****年**月**日
                mysheet.Cells[i + 5, 2].Value = dt日报表详细信息.Rows[i]["班次"].ToString();
                mysheet.Cells[i + 5, 3].Value = dt日报表详细信息.Rows[i]["客户或订单号"].ToString();

                mysheet.Cells[i + 5, 4].Value = dt日报表详细信息.Rows[i]["产品代码"].ToString();
                mysheet.Cells[i + 5, 5].Value = dt日报表详细信息.Rows[i]["批号"].ToString();
                mysheet.Cells[i + 5, 6].Value = dt日报表详细信息.Rows[i]["入库量只A"].ToString();
                mysheet.Cells[i + 5, 7].Value = dt日报表详细信息.Rows[i]["工时B"].ToString();
                mysheet.Cells[i + 5, 8].Value = dt日报表详细信息.Rows[i]["系数C"].ToString();
                mysheet.Cells[i + 5, 9].Value = dt日报表详细信息.Rows[i]["效率"].ToString();
                mysheet.Cells[i + 5, 10].Value = dt日报表详细信息.Rows[i]["成品宽D"].ToString();
                mysheet.Cells[i + 5, 11].Value = dt日报表详细信息.Rows[i]["成品长E"].ToString();
                mysheet.Cells[i + 5, 12].Value = dt日报表详细信息.Rows[i]["成品数量W"].ToString();
                mysheet.Cells[i + 5, 13].Value = dt日报表详细信息.Rows[i]["膜材1规格F"].ToString();
                mysheet.Cells[i + 5, 14].Value = dt日报表详细信息.Rows[i]["膜材1用量G"].ToString();
                mysheet.Cells[i + 5, 15].Value = dt日报表详细信息.Rows[i]["膜材1用量E"].ToString();
                mysheet.Cells[i + 5, 16].Value = dt日报表详细信息.Rows[i]["膜材2规格H"].ToString();
                mysheet.Cells[i + 5, 17].Value = dt日报表详细信息.Rows[i]["膜材2用量K"].ToString();
                mysheet.Cells[i + 5, 18].Value = dt日报表详细信息.Rows[i]["膜材2用量R"].ToString();
                mysheet.Cells[i + 5, 19].Value = dt日报表详细信息.Rows[i]["制袋收率"].ToString();
            }
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from CS制袋日报表详细信息", connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt日报表详细信息.Rows[0]["ID"])) + 1;
            // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            mysheet.PageSetup.RightFooter = mySystem.Parameter.proInstruction + " - 09 - " + sheetnum.ToString() + " / &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString();
            //返回
            return mysheet;
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(true);
            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
            dt日报表.Rows[0]["日志"] = dt日报表.Rows[0]["日志"].ToString() + log;

            bs日报表.EndEdit();
            da日报表.Update((DataTable)bs日报表.DataSource);
        }

        private void bt查看日志_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da日报表日志 = new OleDbDataAdapter("select * from CS制袋日报表 where 生产指令ID=" + ID生产指令, mySystem.Parameter.connOle);
           // OleDbCommandBuilder cb日报表日志 = new OleDbCommandBuilder(da日报表日志);
            DataTable dt日报表日志 = new DataTable("CS制袋日报表");
            //BindingSource bs日报表日志 = new BindingSource();
            da日报表日志.Fill(dt日报表日志);
            String str日志 = dt日报表日志.Rows[0]["日志"].ToString();
            (new mySystem.Other.LogForm()).setLog(dt日报表.Rows[0]["日志"].ToString()).Show();
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            bool is填满 = is_filled();
            
            if (is填满)
            {
             //   DataGridViewsum();
                bs日报表详细信息.EndEdit();
                da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
                readInnerData(Convert.ToInt32(dt日报表.Rows[0]["ID"]));
                innerBind();
            }
            else 
                MessageBox.Show("信息填写不完整");
        }
       
        //判断是否填满
        private bool is_filled()
        {
            int index = dataGridView1.Rows.Count - 1;
            DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
            DataRow dr最后一行 = dt日报表详细信息.NewRow();
            dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;

            int sum = 0;//空白单元格个数
            for (int i = 0; i < dr最后一行.ItemArray.Length; i++)
            {
                if (i != 0 && i != 1)
                {
                    if (dr最后一行[i].ToString() == "")
                        sum += 1;
                }
                else
                {
                    sum += 0;
                }
            }
            if (sum != 0)
                return false;
            else
                return true;
        }

    }
}
