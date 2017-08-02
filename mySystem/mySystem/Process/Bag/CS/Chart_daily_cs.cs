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

namespace mySystem.Process.CleanCut
{
    public partial class Chart_daily_cs : Form
    {
        private OleDbConnection connOle = Parameter.connOle;
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;

        private DataTable dt日报表详细信息, dt日报表, dt生产指令, dt生产指令详细信息, dt领料, dt内包装;
        private BindingSource bs日报表详细信息, bs日报表, bs生产指令, bs生产指令详细信息, bs领料, bs内包装;
        private OleDbDataAdapter da日报表详细信息, da日报表, da生产指令, da生产指令详细信息, da领料, da内包装;
        private OleDbCommandBuilder cb日报表详细信息, cb日报表, cb生产指令, cb生产指令详细信息, cb领料, cb内包装;
       
        int ID = mySystem.Parameter.csbagInstruID;
        string strID = mySystem.Parameter.csbagInstruction;

        public Chart_daily_cs()
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData();
            outerBind();
            readInnerData();
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
        }

        private void Chart_daily_cs_Load(object sender, EventArgs e)
        {

        }

        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        {
            //读取生产指令
            da生产指令 = new OleDbDataAdapter("select * from 生产指令", mySystem.Parameter.connOle);
            cb生产指令 = new OleDbCommandBuilder(da生产指令);
            dt生产指令 = new DataTable("生产指令");
            bs生产指令 = new BindingSource();
            da生产指令.Fill(dt生产指令);
            DataTable dt生产指令所需信息 = dt生产指令.DefaultView.ToTable(false, new string[] { "计划生产日期", "生产指令编号" });

            //读取生产指令
            da生产指令详细信息 = new OleDbDataAdapter("select * from 生产指令详细信息", mySystem.Parameter.connOle);
            cb生产指令详细信息 = new OleDbCommandBuilder(da生产指令);
            dt生产指令详细信息 = new DataTable("生产指令详细信息");
            bs生产指令详细信息 = new BindingSource();
            da生产指令详细信息.Fill(dt生产指令详细信息);
            DataTable dt生产指令所需信息详细 = dt生产指令详细信息.DefaultView.ToTable(false, new string[] { "产品代码", "产品批号","客户或订单号" });

            //读取领料量
            da领料 = new OleDbDataAdapter("select * from CS制袋领料记录详细记录",mySystem.Parameter.connOle);
            cb领料 = new OleDbCommandBuilder(da领料);
            dt领料 = new DataTable("领料");
            bs领料 = new BindingSource();
            da领料.Fill(dt领料);
            DataTable dt领料所需信息 = dt领料.DefaultView.ToTable(false, new string[] { "TCS制袋领料记录ID","物料简称","使用数量C"});

            //读取内包装
            da内包装 = new OleDbDataAdapter("select * from 产品内包装记录",mySystem.Parameter.connOle);
            cb内包装 = new OleDbCommandBuilder(da内包装);
            dt内包装 = new DataTable("内包装");
            bs内包装 = new BindingSource();
            da内包装.Fill(dt内包装);
            DataTable dt内包装所需信息 = dt内包装.DefaultView.ToTable(false, new string[] {"生产指令ID", "产品数量只数合计B"});

            MessageBox.Show(strID);

            //添加打印机
            fill_printer();
        }
        // 根据条件从数据库中读取一行外表的数据
        private void readOuterData()
        {
            da日报表 = new OleDbDataAdapter("select * from CS制袋日报表", mySystem.Parameter.connOle);
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
        private void readInnerData()
        {
            //            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
            //                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
            //            OleDbConnection connOle = new OleDbConnection(strConn);
            //            connOle.Open();
            dt日报表详细信息 = new DataTable("CS制袋日报表详细信息");
            bs日报表详细信息 = new BindingSource();
            da日报表详细信息 = new OleDbDataAdapter(@"select * from CS制袋日报表详细信息", mySystem.Parameter.connOle);
            cb日报表详细信息 = new OleDbCommandBuilder(da日报表详细信息);
            da日报表详细信息.Fill(dt日报表详细信息);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
            // bs委托单.DataSource = dt委托单;

            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
            setDataGridViewFormat();
            bs日报表详细信息.DataSource = dt日报表详细信息;
            dataGridView1.DataSource = bs日报表详细信息.DataSource;

        }
        // 设置自动计算类事件
        private void addComputerEventHandler()
        {
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
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='辐照灭菌台帐'", connOle);
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

            DataGridViewTextBoxColumn c1;
            foreach (DataColumn dc in dt日报表详细信息.Columns)
            {
                switch (dc.ColumnName)
                {
                      default:
                        c1 = new DataGridViewTextBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c1);
                        break;
                }
            }

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
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["TCS制袋日报表ID"].Visible = false;
        }

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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        { }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        { }

    }
}
