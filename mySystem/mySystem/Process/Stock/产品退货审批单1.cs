using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mySystem.Process.Stock
{
    public partial class 产品退货审批单1 : BaseForm
    {

//        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        SqlDataAdapter daOuter;
        SqlCommandBuilder cbOute;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;
        List<string> ls产品名称, ls产品代码, ls销售订单编号;

        Parameter.UserState _userState;
        Parameter.FormState _formState;

        string _code;
        int _id;



        public 产品退货审批单1(MainForm mainform, string code):base(mainform)
        {
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
            InitializeComponent();
             _code = code;
            fillPrinter();
            getOtherData();
            getPeople();
            setUseState();
             // 读取数据
            
            readOuterData(_code);
            _id = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            outerBind();
            setFormState();
            setEnableReadOnly();

            // 事件部分
            //addComputerEventHandler();
            addOtherEvenHandler();
        }

       private void getOtherData()
        {
            
        }


        void getPeople()
        {
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='产品退货审批单1'", mySystem.Parameter.conn);
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

        void setUseState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
        }

        void setFormState(bool newForm = false)
        {
            string s = dtOuter.Rows[0]["批准人"].ToString();
            if (s == "") _formState = Parameter.FormState.未保存;
            else _formState = Parameter.FormState.审核通过;
        }

        void setEnableReadOnly()
        {
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.审核通过 == _formState) setControlFalse();
                else
                {
                    setControlTrue();
                    btn批准.Enabled = true;
                    btn不批准.Enabled = true;
                }
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }


        }

        /// <summary>
        /// 默认控件可用状态
        /// </summary>
        void setControlTrue()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
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
            // 保证这两个按钮一直是false
            btn不批准.Enabled = false;
            btn批准.Enabled = false;
        }

        /// <summary>
        /// 默认控件不可用状态
        /// </summary>
        void setControlFalse()
        {
            // 查询插入，审核，提交审核，生产指令编码 在这里不用管
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
            btn打印.Enabled = true;
            combox打印机选择.Enabled = true;
        }


        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fillPrinter()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                combox打印机选择.Items.Add(sPrint);
            }
            combox打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        

        void readOuterData(String code)
        {
            daOuter = new SqlDataAdapter("select * from 产品退货审批单1 where 退货申请单编号='" + code + "'", mySystem.Parameter.conn);
            dtOuter = new DataTable("产品退货审批单1");
            cbOute = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        


        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            foreach (Control c in this.Controls)
            {

                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2), false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        void addOtherEvenHandler()
        {
            
        }

       

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_code);
            outerBind();
        }

        private void btn批准_Click(object sender, EventArgs e)
        {
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }

            
            
            
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;


            dtOuter.Rows[0]["状态"] = "已批准";
            dtOuter.Rows[0]["批准人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["批准日期"] = DateTime.Now;
            dtOuter.Rows[0]["批准结果"] = true;
            save();
            setControlFalse();
            // 自动生产审批单2
            da = new SqlDataAdapter("select * from 产品退货审批单2 where 退货申请单编号='" + _code + "'", mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("产品退货审批单(2)已存在！");
            }
            else
            {
                DataRow dr = dt.NewRow();
                for (int i = 1; i <= 11; ++i)
                {
                    dr[i] = dtOuter.Rows[0][i];
                }
                dr["销售总监审批意见"] = dtOuter.Rows[0]["销售总监审批意见"];
                dr["销售总监批准日期"] = dtOuter.Rows[0]["批准日期"];
                dr["销售总监批准人"] = dtOuter.Rows[0]["批准人"];
                dr["状态"] = "审批中";
                dr["产品退货申请单ID"] = dtOuter.Rows[0]["产品退货申请单ID"];
                dr["批准日期"] = DateTime.Now;

                dr["批准结果"] = false;//默认值
                dt.Rows.Add(dr);
                da.Update(dt);
                MessageBox.Show("产品退货审批单(2)已生成！");

            }
        }

        private void btn不批准_Click(object sender, EventArgs e)
        {
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }

            
            
            dtOuter.Rows[0]["状态"] = "已批准";
            dtOuter.Rows[0]["批准人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["批准日期"] = DateTime.Now;
            dtOuter.Rows[0]["批准结果"] = false ;
            save();
            setControlFalse();
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (combox打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(combox打印机选择.Text);
            print(true);
            GC.Collect();
        }

        //打印功能
        public int print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\库存\表2产品退货审批单（1）.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        bsOuter.EndEdit();
                        daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    // 关闭文件，false表示不保存
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
                return pageCount;
            }
        }

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[2, 2].Value = dtOuter.Rows[0]["退货申请单编号"].ToString();
            mysheet.Cells[3, 2].Value = dtOuter.Rows[0]["申请人"].ToString();
            mysheet.Cells[3, 4].Value = Convert.ToDateTime(dtOuter.Rows[0]["申请日期"]).ToString("D");
            mysheet.Cells[4, 2].Value = dtOuter.Rows[0]["拟退货产品销售订单编号"].ToString();
            mysheet.Cells[4, 4].Value = dtOuter.Rows[0]["客户名称"].ToString();
            mysheet.Cells[5, 2].Value = dtOuter.Rows[0]["产品名称"].ToString();
            mysheet.Cells[5, 4].Value = dtOuter.Rows[0]["产品代码"].ToString();
            mysheet.Cells[6, 2].Value = dtOuter.Rows[0]["产品批号"].ToString();
            mysheet.Cells[6, 4].Value = dtOuter.Rows[0]["拟退货数量"].ToString();
            mysheet.Cells[6, 5].Value = dtOuter.Rows[0]["辅计量单位"].ToString();
            mysheet.Cells[7, 2].Value = dtOuter.Rows[0]["退货理由"].ToString();
            mysheet.Cells[8, 2].Value = dtOuter.Rows[0]["销售总监审批意见"].ToString();
            mysheet.Cells[14, 3].Value = dtOuter.Rows[0]["批准结果"].ToString() == "True" ? "批准" : "不予批准";
            mysheet.Cells[14, 5].Value = Convert.ToDateTime(dtOuter.Rows[0]["批准日期"]).ToString("D");
            ///
            /// TODO:加页脚
            ///
            
            //返回
            return mysheet;
        }

        

    }
}
