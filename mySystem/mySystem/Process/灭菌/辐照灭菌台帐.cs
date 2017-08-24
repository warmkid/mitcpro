using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Process.灭菌
{
    public partial class 辐照灭菌台帐 : mySystem.BaseForm
    {
        private DataTable dt台帐,dt委托单,dt台帐外表;
        private BindingSource bs台帐,bs委托单,bs台帐外表;
        private OleDbDataAdapter da台帐, da委托单, da台帐外表;
        private OleDbCommandBuilder cb台帐, cb委托单, cb台帐外表;
        private List<string> weituodanhao;
        DataGridViewComboBoxColumn c1;
        private OleDbConnection connOle =  Parameter.connOle;
        List<String> ls操作员, ls审核员;
        Parameter.UserState _userState;
        Int32 index;//datagridview列数
        Int32 ID委托单号;


        public 辐照灭菌台帐(mySystem.MainForm mainform): base(mainform)
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
            setDataGridViewRowNums();


            initQuery();
        }

        private void initQuery()
        {
            dateTimePicker委托日期结束.Value = DateTime.Now;
            dateTimePicker委托日期开始.Value = DateTime.Now.AddDays(-7).Date;
            
        }



        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        {
            //weituodanhao = new List<string>();
            OleDbDataAdapter da单号查询 = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单",mySystem.Parameter.connOle);
            OleDbCommandBuilder cb单号查询 = new OleDbCommandBuilder(da单号查询);
            DataTable dt委托单数据源 = new DataTable("委托单号查询");
            BindingSource bs单号查询 = new BindingSource();
            da单号查询.Fill(dt委托单数据源);
            //foreach (DataRow tdr in dt委托单数据源.Rows)
            //{
            //    weituodanhao.Add(tdr["委托单号"].ToString());
            //}
            dt委托单 = dt委托单数据源.DefaultView.ToTable(false,new string[]{"委托单号","委托日期"});
            fill_printer();
        }
        // 根据条件从数据库中读取一行外表的数据
        private void readOuterData()
        {
            da台帐外表 = new OleDbDataAdapter("select * from 辐照灭菌台帐",mySystem.Parameter.connOle);
            cb台帐外表 = new OleDbCommandBuilder(da台帐外表);
            dt台帐外表 = new DataTable("辐照灭菌台帐外表");
            bs台帐外表 = new BindingSource();
            da台帐外表.Fill(dt台帐外表);
        }
        // 外表和控件的绑定
        private void outerBind()
        {
            bs台帐外表.DataSource = dt台帐外表;
            
        }
        // 根据条件从数据库中读取多行内表数据
        private void readInnerData()
        {
//            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
//            OleDbConnection connOle = new OleDbConnection(strConn);
//            connOle.Open();
            dt台帐 = new DataTable("辐照灭菌台帐详细信息");
            bs台帐 = new BindingSource();
            da台帐 = new OleDbDataAdapter(@"select * from 辐照灭菌台帐详细信息", mySystem.Parameter.connOle);
            cb台帐 = new OleDbCommandBuilder(da台帐);
            da台帐.Fill(dt台帐);
            index = dt台帐.Rows.Count;
        }
        // 内表和控件的绑定
        private void innerBind()
        {
           // bs委托单.DataSource = dt委托单;

            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
            bs台帐.DataSource = dt台帐;
            dataGridView1.DataSource = bs台帐.DataSource;
            index = dt台帐.Rows.Count;
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
            if (_userState == Parameter.UserState.审核员 || _userState == Parameter.UserState.管理员)
            {
                b打印.Enabled = true;
            }
            else
            {
                b打印.Enabled = false;
            }
        }
        // 其他事件，比如按钮的点击，数据有效性判断
        private void addOtherEventHandler()
        {
           dataGridView1.DataError += dataGridView1_DataError;
            //dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
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
                string str人员 = dt台帐.Rows[columnindex][rowsname].ToString();
                if (mySystem.Parameter.NametoID(str人员) <= 0)
                    MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
            }
            
        }

        void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (dataGridView.CurrentCell.GetType().Name == "DataGridViewComboBoxCell")
            {
                ComboBox comboBox = (ComboBox)e.Control;
                comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            }
        }
        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index=dataGridView1.SelectedCells[0].RowIndex;
            ComboBox comboBox = (ComboBox)sender;
            string str委托单号显示;
            DataRow[] dr委托单号;
            string str查询条件 = "委托单号 = '" + comboBox.Text+"'";
            str委托单号显示 = comboBox.Text;
            dr委托单号 = dt委托单.Select(str查询条件);
            dataGridView1.Rows[index].Cells["委托日期"].Value = dr委托单号[0]["委托日期"].ToString();
            //dt台帐.Rows[index]["委托单号"] = str委托单号显示;
            //dt台帐.Rows[index]["委托日期"] = dr委托单号[0]["委托日期"];          
            //dt台帐.Rows[index]["产品数量箱"] = 0;
            //dt台帐.Rows[index]["产品数量只"] = 0;
            //dt台帐.Rows[index]["送去产品托盘数量个"] = 0;
            //dt台帐.Rows[index]["拉回产品托盘数量个"] = 0;
           // string str委托日期 = dr委托单号[0]["委托日期"].ToString();
           // MessageBox.Show(string.Format("选中：{0}", str委托日期));
            
            //DataRow drtemp = dt台帐.NewRow();
            //drtemp["委托单号"] = str委托单号显示;
            //drtemp["产品数量箱"] = 0;
            //dt台帐.Rows.Add(drtemp);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            //int columnindex = ((DataGridView)sender).SelectedCells[0].ColumnIndex;
            //String Columnsname = ((DataGridView)sender).Columns[columnindex].Name;
            //String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString();
            
            //if (Columnsname == "登记人" || Columnsname == "审核人")
            //{
            //    string str人员减一 = dt台帐.Rows[columnindex - 1][rowsname].ToString();
            //    string str人员 = dt台帐.Rows[columnindex][rowsname].ToString();
            //    if (mySystem.Parameter.NametoID(str人员) <= 0)
            //        MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");
            //}
            
        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        private void setDataGridViewColumns()
        {          
            
            DataGridViewTextBoxColumn c2;
         //foreach (DataColumn dc in dt台帐.Columns)
         //   {
         //    switch(dc.ColumnName)
         //   {
         //       case "委托单号":
         //           c1 = new DataGridViewComboBoxColumn();
         //           c1.DataPropertyName = dc.ColumnName;
         //           c1.HeaderText = dc.ColumnName;
         //           c1.Name = dc.ColumnName;
         //           c1.DataPropertyName = dc.ColumnName;
         //           c1.SortMode = DataGridViewColumnSortMode.Automatic;
         //           c1.ValueType = dc.DataType;
         //           OleDbDataAdapter danhao_search = new OleDbDataAdapter("select 委托单号 from Gamma射线辐射灭菌委托单", mySystem.Parameter.connOle);
         //           DataTable da_danhao = new DataTable("委托单号查询");
         //           danhao_search.Fill(da_danhao);
         //           foreach (DataRow tdr in da_danhao.Rows)
         //           {
         //               c1.Items.Add(tdr["委托单号"]);
         //           }
         //           dataGridView1.Columns.Add(c1);
         //           //dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
         //           //dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;              
         //           break;
         //        default:
         //       c2 = new DataGridViewTextBoxColumn();
         //       c2.DataPropertyName = dc.ColumnName;
         //       c2.HeaderText = dc.ColumnName;
         //       c2.Name = dc.ColumnName;
         //       c2.SortMode = DataGridViewColumnSortMode.Automatic;
         //       c2.ValueType = dc.DataType;
         //       dataGridView1.Columns.Add(c2);
         //       break;
         //   }
         //   }

        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.ColumnHeadersHeight = 40;
            //dataGridView1.Columns["送去产品托盘数量个"].HeaderText = "送去产品托盘数量(个)";
            //dataGridView1.Columns["拉回产品托盘数量个"].HeaderText = "拉回产品托盘数量(个)";
            dataGridView1.Columns["产品数量只"].HeaderText = "产品数量(只)";
            dataGridView1.Columns["产品数量箱"].HeaderText = "产品数量(箱)";
            ////第一列ID不显示
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["T辐照灭菌台帐ID"].Visible = false;
            dataGridView1.Columns["审核员"].Visible = false;
            //dataGridView1.Columns["审核是否通过"].Visible = false;
            //dataGridView1.Columns["日志"].Visible = false;
        }

        //添加新行
        private void bt添加_Click(object sender, EventArgs e)
        {
            //最后一行是否填满
            ////int index = dataGridView1.Rows.Count - 1;
            ////DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
            ////DataRow dr最后一行 = dt台帐.NewRow();
            ////dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;
            //bool is填满 = is_filled();
          
            //if (is填满)
            //{
            //    DataRow dr新行 = dt台帐.NewRow();
            //    dr新行 = writeInnerDefault(dr新行);
            //    dt台帐.Rows.Add(dr新行);
            //    setDataGridViewRowNums();
            //    dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            //}
            //else
            //    MessageBox.Show("未填完");
        }

        //设置序号递增
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //写默认行数据
        //DataRow writeInnerDefault(DataRow dr)
        //{
        //   // dr["T辐照灭菌台帐ID"] = 8;
        //   // dr["委托日期"] = DateTime.Now.ToString("D");
        //   // dr["产品数量箱"] = 0;
        //   // dr["产品数量只"] = 0;
        //   // dr["送去产品托盘数量个"] = 0;
        //   // dr["拉回产品托盘数量个"] = 0;
        //   // dr["备注"] = "无";
        //   //// dr["操作员"] = mySystem.Parameter.userName;
        //   //// dr["日志"] = "无";
        //   // return dr;
        //}

        //保存数据到数据库
        private void bt保存_Click(object sender, EventArgs e)
        {
            //bool is填满 = is_filled();
            // bool is合法 = input_Judge();
            // if (is填满 && is合法)
            // {
            //     bs台帐.EndEdit();
            //     da台帐.Update((DataTable)bs台帐.DataSource);
            //     readInnerData();
            //     innerBind();
            // }
            // else if (!is合法 && is填满)
            //     MessageBox.Show("信息填写错误");
            // else if (is合法 && !is填满)
            //     MessageBox.Show("信息填写不完整");
            // else
            //     MessageBox.Show("信息填写错误且不完整");
            // index = dt台帐.Rows.Count;
        }

        //某行数据是否填满
        private bool is_filled()
        {
            int index = dataGridView1.Rows.Count - 1;
            DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
            DataRow dr最后一行 = dt台帐.NewRow();
            dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;
            
            int sum=0;//空白单元格个数
            for (int i = 0; i < dr最后一行.ItemArray.Length; i++)
            {
                //string suibian = dr[i].ToString();
                //if (dr[i] != dr["审核意见"] && dr[i] != dr["审核是否通过"])
                //if (dr[i].Equals(dr["审核意见"]) || dr[i].Equals(dr["审核是否通过"]))
                if(i!=0&&i!=1)
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
        //输入用户姓名是否合法
        //private bool input_Judge()
        //{
        //    //int index = dataGridView1.Rows.Count - 1;
        //    //string str登记员=dt台帐.Rows[index]["登记员"].ToString();
        //    //string str审核员 = dt台帐.Rows[index]["审核员"].ToString();
        //    //if (mySystem.Parameter.NametoID(str登记员) <= 0 || mySystem.Parameter.NametoID(str审核员)<=0)
        //    //{              
        //    //    return false;
        //    //}
        //    //else
        //    //    return true;
        //}

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\miejun\SOP-MFG-106-R03A 辐照灭菌台帐.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
             

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 修改Sheet中某行某列的值
                my = printValue(my,wb);
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
            int rownum = dt台帐.Rows.Count;
            for (int i = 0; i < rownum; i++)
            {
                mysheet.Cells[i + 4, 2].Value = dt台帐.Rows[i]["委托单号"].ToString();
                mysheet.Cells[i + 4, 3].Value = Convert.ToDateTime( dt台帐.Rows[i]["委托日期"]).ToString("D");//去掉时分秒，且显示为****年**月**日
                mysheet.Cells[i + 4, 4].Value = dt台帐.Rows[i]["产品数量箱"].ToString();
                mysheet.Cells[i + 4, 5].Value = dt台帐.Rows[i]["产品数量只"].ToString();
                //mysheet.Cells[i + 4, 6].Value = dt台帐.Rows[i]["送去产品托盘数量个"].ToString();
                //mysheet.Cells[i + 4, 7].Value = dt台帐.Rows[i]["拉回产品托盘数量个"].ToString();
                mysheet.Cells[i + 4, 8].Value = dt台帐.Rows[i]["备注"].ToString();
                mysheet.Cells[i + 4, 9].Value = dt台帐.Rows[i]["登记员"].ToString();
                mysheet.Cells[i + 4, 10].Value = dt台帐.Rows[i]["审核员"].ToString();
            }
            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 辐照灭菌台帐详细信息", connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dt台帐.Rows[0]["ID"])) + 1;
            // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            mysheet.PageSetup.RightFooter = mySystem.Parameter.proInstruction + " - 09 - " + sheetnum.ToString() + " / &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); 
           //返回
            return mysheet;
        }

        private void b打印_Click(object sender, EventArgs e)
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
            dt台帐外表.Rows[0]["日志"] = dt台帐外表.Rows[0]["日志"].ToString() + log;

            bs台帐外表.EndEdit();
            da台帐外表.Update((DataTable)bs台帐外表.DataSource);
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
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        //填过“审核员”后，该行只读
        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            setDataGridViewFormat();
           
            //for (int i = 0; i < index; i++)
            //{
            //    string str审核员 = dt台帐.Rows[i]["审核员"].ToString();
            //    if (str审核员 != "")
            //    {
            //        dataGridView1.Rows[i].ReadOnly = true;
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Wheat;
            //    }
            //}
        }

        //删除
        private void button1_Click(object sender, EventArgs e)
        {
            ////if (dataGridView1.SelectedCells.Count > 1)
            ////{
            //if (dataGridView1.SelectedCells.Count == 0) return;
            //int deletenum = dataGridView1.CurrentRow.Index;

            //if (deletenum < 0)
            //    return;
            //// dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            //else
            //{
            //    dt台帐.Rows[deletenum].Delete();
            //    da台帐.Update((DataTable)bs台帐.DataSource);
            //    readInnerData();
            //    innerBind();
            //    //刷新序号
            //    setDataGridViewRowNums();
            //    index = dt台帐.Rows.Count;
            //}
            ////}
            
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

        //查看日志
        private void bt查看日志_Click(object sender, EventArgs e)
        {
            (new mySystem.Other.LogForm()).setLog(dt台帐外表.Rows[0]["日志"].ToString()).Show();
        }

        private void btn查询_Click(object sender, EventArgs e)
        {
            string sql = @"select * from 辐照灭菌台帐详细信息 where 委托单号 like '%{0}%' and 产品代码 like '%{1}%' and 委托日期 between #{2}# and #{3}#";
            dt台帐 = new DataTable("辐照灭菌台帐详细信息");
            bs台帐 = new BindingSource();
            da台帐 = new OleDbDataAdapter(string.Format(sql, textBox委托单号.Text, textBox产品代码.Text, dateTimePicker委托日期开始.Value, dateTimePicker委托日期结束.Value), mySystem.Parameter.connOle);
            cb台帐 = new OleDbCommandBuilder(da台帐);
            da台帐.Fill(dt台帐);

            innerBind();
        }




    }
}
