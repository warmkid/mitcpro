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

namespace mySystem.Process.CleanCut
{
    public partial class Chart_daily_cs : Form
    {
        private OleDbConnection connOle = Parameter.connOle;
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Int32 ID生产指令 = mySystem.Parameter.csbagInstruID;
        String str生产指令编码 = mySystem.Parameter.csbagInstruction;
        
        //界面显示所需信息
        DateTime date生产日期;
        String str班次;
        String str产品代码;
        String str产品批号;
        String str客户或订单号;
        Int32 i入库量只;
        Double i效率=0.0;
        Double i成品宽 = 0.0;
        Double i成品长 = 0.0;
        Double i成品数量 = 0.0;
        Double i膜材1用量米 = 0.0;
        Double i膜材1用量平方米 = 0.0;
        Double i膜材2用量米 = 0.0;
        Double i膜材2用量平方米 = 0.0;
        Double i制袋收率 = 0.0;

        private DataTable dt日报表详细信息, dt日报表, dt生产指令, dt生产指令详细信息, dt领料, dt领料详细信息, dt内包装,dt产品外观;
        private BindingSource bs日报表详细信息, bs日报表, bs生产指令, bs生产指令详细信息, bs领料, bs领料详细信息, bs内包装, bs产品外观;
        private OleDbDataAdapter da日报表详细信息, da日报表, da生产指令, da生产指令详细信息, da领料, da领料详细信息, da内包装, ds产品外观;
        private OleDbCommandBuilder cb日报表详细信息, cb日报表, cb生产指令, cb生产指令详细信息, cb领料, cb领料详细信息, cb内包装, cb产品外观;
       
        public Chart_daily_cs()
        {
            InitializeComponent();

            bt打印.Enabled = false;

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

            dataGridView1.Columns.Clear();

            setDataGridViewColumns();
        }

        private void Chart_daily_cs_Load(object sender, EventArgs e)
        {

        }

        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        {
            //读取生产指令外表
            da生产指令 = new OleDbDataAdapter("select * from 生产指令 where ID="+ID生产指令, mySystem.Parameter.connOle);
            cb生产指令 = new OleDbCommandBuilder(da生产指令);
            dt生产指令 = new DataTable("生产指令");
            bs生产指令 = new BindingSource();
            da生产指令.Fill(dt生产指令);
            DataTable dt生产指令所需信息 = dt生产指令.DefaultView.ToTable(false, new string[] {"ID", "计划生产日期", "生产指令编号" ,"制袋物料名称1","制袋物料名称2"});
            Int32 i生产指令外表ID = Convert.ToInt32(dt生产指令所需信息.Rows[0]["ID"].ToString());
            //根据生产指令外表中的制袋物料名称，读取领料量中对应物料的使用量C
            String str制袋物料名称1 = dt生产指令所需信息.Rows[0]["制袋物料名称1"].ToString();
            String str制袋物料名称2 = dt生产指令所需信息.Rows[0]["制袋物料名称2"].ToString();

            //读取生产指令内表
            da生产指令详细信息 = new OleDbDataAdapter("select * from 生产指令详细信息 where T生产指令ID=" + i生产指令外表ID, mySystem.Parameter.connOle);
            cb生产指令详细信息 = new OleDbCommandBuilder(da生产指令详细信息);
            dt生产指令详细信息 = new DataTable("生产指令详细信息");
            bs生产指令详细信息 = new BindingSource();
            da生产指令详细信息.Fill(dt生产指令详细信息);
            DataTable dt生产指令所需信息详细 = dt生产指令详细信息.DefaultView.ToTable(false, new string[] { "产品代码", "产品批号","客户或订单号" });
            
            str产品代码 = dt生产指令所需信息详细.Rows[0]["产品代码"].ToString();
            str产品批号 = dt生产指令所需信息详细.Rows[0]["产品批号"].ToString();
            str客户或订单号 = dt生产指令所需信息详细.Rows[0]["客户或订单号"].ToString();

            //读取领料量外表
            da领料 = new OleDbDataAdapter("select * from CS制袋领料记录 where 生产指令ID=" + ID生产指令, mySystem.Parameter.connOle);
            cb领料 = new OleDbCommandBuilder(da领料);
            dt领料 = new DataTable("领料");
            bs领料 = new BindingSource();
            da领料.Fill(dt领料);
            DataTable dt领料所需信息 = dt领料.DefaultView.ToTable(false, new string[] { "ID"});
            Int32 i领料量外表ID = Convert.ToInt32(dt领料所需信息.Rows[0]["ID"].ToString());

            //读取领料量内表
            da领料详细信息 = new OleDbDataAdapter("select * from CS制袋领料记录详细记录 where TCS制袋领料记录ID=" + i领料量外表ID, mySystem.Parameter.connOle);
            cb领料详细信息 = new OleDbCommandBuilder(da领料详细信息);
            dt领料详细信息 = new DataTable("领料详细信息");
            bs领料详细信息 = new BindingSource();
            da领料详细信息.Fill(dt领料详细信息);
            DataTable dt领料所需信息详细 = dt领料详细信息.DefaultView.ToTable(false, new string[] { "物料简称", "使用数量C" });



            //读取内包装
            da内包装 = new OleDbDataAdapter("select * from 产品内包装记录 where 生产指令ID="+ID生产指令, mySystem.Parameter.connOle);
            cb内包装 = new OleDbCommandBuilder(da内包装);
            dt内包装 = new DataTable("内包装");
            bs内包装 = new BindingSource();
            da内包装.Fill(dt内包装);
            DataTable dt内包装所需信息 = dt内包装.DefaultView.ToTable(false, new string[] {"ID", "产品数量只数合计B"});
            Int32 i内包装外表ID = Convert.ToInt32(dt内包装所需信息.Rows[0]["ID"].ToString());

            i入库量只 =Convert.ToInt32(dt内包装所需信息.Rows[0]["产品数量只数合计B"].ToString());

            MessageBox.Show(str生产指令编码);

            //添加打印机
            fill_printer();
        }

        // 读取数据，无参数表示从Paramter中读取数据
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
            dataGridView1.Columns[0].Visible = false;
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
           /// Int32[] rowcount = { 0, 1, 2, 3 };
            foreach (DataRow dr in dt日报表详细信息.Rows)
            {
                if (Convert.ToInt32(dr["工时B"].ToString()) == 0)
                {
                    i效率 = 0;
                    dr["效率"] = i效率;
                }
                else
                    i效率 = Convert.ToInt32(dr["入库量只A"].ToString()) * Convert.ToDouble(dr["系数C"].ToString()) / Convert.ToDouble(dr["工时B"].ToString());


                i成品数量 = Convert.ToInt32(dr["入库量只A"].ToString()) * Convert.ToDouble(dr["成品宽D"].ToString()) * Convert.ToDouble(dr["成品长E"].ToString()) / 1000000 * 2;
                i膜材1用量平方米 = Convert.ToDouble(dr["膜材1规格F"].ToString()) * Convert.ToDouble(dr["膜材1用量G"].ToString()) / 1000;
                i膜材2用量平方米 = Convert.ToDouble(dr["膜材2规格H"].ToString()) * Convert.ToDouble(dr["膜材2用量K"].ToString()) / 1000;
                if ((i膜材1用量平方米 + i膜材2用量平方米) != 0)
                {
                    i制袋收率 = i成品数量 / (i膜材1用量平方米 + i膜材2用量平方米);
                }
                dr["效率"] = i效率;
                dr["成品数量W"] = i成品数量;
                dr["膜材1用量E"] = i膜材1用量平方米;
                dr["膜材2用量R"] = i膜材2用量平方米;
                dr["制袋收率"] = i制袋收率;

                //dt日报表详细信息.Rows[i]["效率"] = i效率;
                //dt日报表详细信息.Rows[i]["成品数量W"] = i成品数量;
                //dt日报表详细信息.Rows[i]["膜材1用量E"] = i膜材1用量;
                //dt日报表详细信息.Rows[i]["膜材2用量R"] = i膜材2用量;
                //dt日报表详细信息.Rows[i]["制袋收率"] = i制袋收率;
                
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           
        }

        //写日报表外表数据
        void writeInnerDefault(DataTable dt)
        { 
            
        }

        //写日报表内表数据
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["TCS制袋日报表ID"] = 1;
            dr["生产日期"] = DateTime.Now;
            dr["班次"] = " ";
            dr["客户或订单号"] = str客户或订单号;
            dr["产品代码"] = str产品代码;
            dr["批号"] = str产品批号;
            dr["入库量只A"] = i入库量只;
            dr["工时B"] = 0.0;
            dr["系数C"] = 0.0;
            dr["效率"] = i效率;
            dr["成品宽D"] = 0.0;
            dr["成品长E"] = 0.0;
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

        //设置序号递增
        void setDataGridViewRowNums() 
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //private void bt添加_Click(object sender, EventArgs e)
        //{
        //    bool is填满 = is_filled();

        //    if (is填满)
        //    {
        //        DataRow dr新行 = dt日报表详细信息.NewRow();
        //        dr新行 = writeInnerDefault(dr新行);
        //        dt日报表详细信息.Rows.Add(dr新行);
        //        setDataGridViewRowNums();
        //    }
        //    else
        //        MessageBox.Show("未填完");
        //}

        //private void bt删除_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView1.SelectedCells.Count > 0)
        //    { 
        //        if (dataGridView1.SelectedCells[0].RowIndex < 0)
        //            return;
        //        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
        //    }

        //    da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
        //    innerBind();
        //    //刷新序号
        //    setDataGridViewRowNums();
        //}

        //private void bt保存_Click(object sender, EventArgs e)
        //{
        //    bool is填满 = is_filled();
        //    if (is填满)
        //    {
        //        bs日报表详细信息.EndEdit();
        //        da日报表详细信息.Update((DataTable)bs日报表详细信息.DataSource);
        //        readInnerData();
        //        innerBind();
        //    }
        //    else
        //        MessageBox.Show("信息填写不完整");
        //}

        //某行数据是否填满
        //private bool is_filled()
        //{
        //    int index = dataGridView1.Rows.Count - 1;
        //    DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
        //    DataRow dr最后一行 = dt日报表详细信息.NewRow();
        //    dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;

        //    int sum = 0;//空白单元格个数
        //    for (int i = 0; i < dr最后一行.ItemArray.Length; i++)
        //    {
        //        //string suibian = dr[i].ToString();
        //        //if (dr[i] != dr["审核意见"] && dr[i] != dr["审核是否通过"])
        //        //if (dr[i].Equals(dr["审核意见"]) || dr[i].Equals(dr["审核是否通过"]))
        //        if (i != 0 && i != 1)
        //        {
        //            if (dr最后一行[i].ToString() == "")
        //                sum += 1;
        //        }
        //        else
        //        {
        //            sum += 0;
        //        }
        //    }
        //    if (sum != 0)
        //        return false;
        //    else
        //        return true;
        //}

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
                readInnerData();
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
