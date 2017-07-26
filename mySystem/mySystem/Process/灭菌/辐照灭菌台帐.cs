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

namespace mySystem.Process.灭菌
{
    public partial class 辐照灭菌台帐 : mySystem.BaseForm
    {
        private DataTable dt台帐,dt委托单;
        private BindingSource bs台帐,bs委托单;
        private OleDbDataAdapter da台帐,da委托单;
        private OleDbCommandBuilder cb台帐,cb委托单;
        private List<string> weituodanhao;
        DataGridViewComboBoxColumn c1;

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

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void getPeople()
        {
        
        }
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        private void setUserState()
        { 
        
        }
        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        {
            //weituodanhao = new List<string>();
            OleDbDataAdapter danhao_search = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单",mySystem.Parameter.connOle);
            DataTable dt委托单数据源 = new DataTable("委托单号查询");
            danhao_search.Fill(dt委托单数据源);
            //foreach (DataRow tdr in dt委托单数据源.Rows)
            //{
            //    weituodanhao.Add(tdr["委托单号"].ToString());
            //}
            dt委托单 = dt委托单数据源.DefaultView.ToTable(false,new string[]{"委托单号","委托日期"});
        }
        // 根据条件从数据库中读取一行外表的数据
        private void readOuterData()
        {
           
        }
        // 外表和控件的绑定
        private void outerBind()
        { 
        
        }
        // 根据条件从数据库中读取多行内表数据
        private void readInnerData()
        {
//            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
//            OleDbConnection connOle = new OleDbConnection(strConn);
//            connOle.Open();
            dt台帐 = new DataTable("辐照灭菌台帐");
            bs台帐 = new BindingSource();
            da台帐 = new OleDbDataAdapter(@"select * from 辐照灭菌台帐", mySystem.Parameter.connOle);
            cb台帐 = new OleDbCommandBuilder(da台帐);
            da台帐.Fill(dt台帐);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
           // bs委托单.DataSource = dt委托单;
            
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
            setDataGridViewFormat();
            bs台帐.DataSource = dt台帐;
            dataGridView1.DataSource = bs台帐.DataSource;
            //第一列ID不显示
            dataGridView1.Columns[0].Visible = false;
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
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            int columnindex = ((DataGridView)sender).SelectedCells[0].ColumnIndex;
            String Columnsname = ((DataGridView)sender).Columns[columnindex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的『" + Columnsname + "』填写错误");

            if (Columnsname == "登记人" || Columnsname == "审核人")
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
            string str委托日期 = dr委托单号[0]["委托日期"].ToString();
            MessageBox.Show(string.Format("选中：{0}", str委托日期));
            
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
         foreach (DataColumn dc in dt台帐.Columns)
            {
             switch(dc.ColumnName)
            {
                case "委托单号":
                    c1 = new DataGridViewComboBoxColumn();
                    c1.DataPropertyName = dc.ColumnName;
                    c1.HeaderText = dc.ColumnName;
                    c1.Name = dc.ColumnName;
                    c1.DataPropertyName = dc.ColumnName;
                    c1.SortMode = DataGridViewColumnSortMode.Automatic;
                    c1.ValueType = dc.DataType;
                    OleDbDataAdapter danhao_search = new OleDbDataAdapter("select 委托单号 from Gamma射线辐射灭菌委托单", mySystem.Parameter.connOle);
                    DataTable da_danhao = new DataTable("委托单号查询");
                    danhao_search.Fill(da_danhao);
                    foreach (DataRow tdr in da_danhao.Rows)
                    {
                        c1.Items.Add(tdr["委托单号"]);
                    }
                    dataGridView1.Columns.Add(c1);
                    //dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
                    //dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;              
                    break;
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

        }

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns["送去产品托盘数量个"].HeaderText = "送去产品托盘数量(个)";
            dataGridView1.Columns["拉回产品托盘数量个"].HeaderText = "拉回产品托盘数量(个)";
            dataGridView1.Columns["产品数量只"].HeaderText = "产品数量(只)";
            dataGridView1.Columns["产品数量箱"].HeaderText = "产品数量(箱)";
            dataGridView1.Columns["审核意见"].Visible = false;
            dataGridView1.Columns["审核是否通过"].Visible = false;
        }

        //添加新行
        private void bt添加_Click(object sender, EventArgs e)
        {
            //最后一行是否填满
            //int index = dataGridView1.Rows.Count - 1;
            //DataGridViewRow dgvr最后一行 = dataGridView1.Rows[index];
            //DataRow dr最后一行 = dt台帐.NewRow();
            //dr最后一行 = (dgvr最后一行.DataBoundItem as DataRowView).Row;
            bool is填满 = is_filled();
          
            if (is填满)
            {
                DataRow dr新行 = dt台帐.NewRow();
                dr新行 = writeInnerDefault(dr新行);
                dt台帐.Rows.Add(dr新行);
                setDataGridViewRowNums();
                dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
            }
            else
                MessageBox.Show("未填完");
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
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["产品数量箱"] = 0;
            dr["产品数量只"] = 0;
            dr["送去产品托盘数量个"] = 0;
            dr["拉回产品托盘数量个"] = 0;
            dr["备注"] = "无";
            dr["日志"] = "无";
            return dr;
        }

        //保存数据到数据库
        private void bt保存_Click(object sender, EventArgs e)
        {
            bool is填满 = is_filled();
             bool is合法 = input_Judge();
             if (is填满 && is合法)
             {
                 bs台帐.EndEdit();
                 da台帐.Update((DataTable)bs台帐.DataSource);
                 readInnerData();
                 innerBind();
             }
             else if (!is合法 && is填满)
                 MessageBox.Show("信息填写错误");
             else if (is合法 && !is填满)
                 MessageBox.Show("信息填写不完整");
             else
                 MessageBox.Show("信息填写错误且不完整");
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
                if(i!=0&&i!=11&&i!=12)
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
        private bool input_Judge()
        {
            int index = dataGridView1.Rows.Count - 1;
            string str登记人=dt台帐.Rows[index]["登记人"].ToString();
            string str审核人 = dt台帐.Rows[index]["审核人"].ToString();
            if (mySystem.Parameter.NametoID(str登记人) <= 0 || mySystem.Parameter.NametoID(str审核人)<=0)
            {              
                return false;
            }
            else
                return true;
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R09 吹膜工序生产和检验记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 修改Sheet中某行某列的值
                my = printValue(my);
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                //false->打印
                // 设置该进程是否可见
                oXL.Visible = false;
                // 修改Sheet中某行某列的值
                my = printValue(my);
                // 直接用默认打印机打印该Sheet
                my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
            }
        }

        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet)
        {

            return mysheet;
        }

        private void b打印_Click(object sender, EventArgs e)
        {
            print(true);
        }

    }
}
