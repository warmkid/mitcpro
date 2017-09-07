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

namespace mySystem.Process.Stock
{
    public partial class 产品退货记录 : BaseForm
    {

        string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
        OleDbConnection conn;
        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOute;
        DataTable dtOuter;
        BindingSource bsOuter;

        List<String> ls操作员, ls审核员;
        List<string> ls产品名称, ls产品代码, ls销售订单编号;

        Parameter.UserState _userState;
        Parameter.FormState _formState;

        string _code;
        int _id;

        public 产品退货记录(MainForm mainform, string code):base(mainform)
        {
            conn = new OleDbConnection(strConnect);
            conn.Open();
            InitializeComponent();
            _code = code;
            fillPrinter();
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

        void getPeople()
        {
            OleDbDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new OleDbDataAdapter("select * from 库存用户权限 where 步骤='产品退货记录'", conn);
            dt = new DataTable("temp");
            da.Fill(dt);

            ls操作员 = dt.Rows[0]["操作员"].ToString().Split(',').ToList<String>();

            ls审核员 = dt.Rows[0]["审核员"].ToString().Split(',').ToList<String>();
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

        void setFormState(bool newForm = false)
        {
            _formState = Parameter.FormState.未保存;
        }

        void setEnableReadOnly()
        {
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                setControlFalse();
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
            daOuter = new OleDbDataAdapter("select * from 产品退货记录 where 退货申请单编号='" + code + "'", conn);
            dtOuter = new DataTable("产品退货记录");
            cbOute = new OleDbCommandBuilder(daOuter);
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

        private void btn确认_Click(object sender, EventArgs e)
        {
            save();
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (combox打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(combox打印机选择.Text);
            print(false);
            GC.Collect();
        }

        public void print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/tuihuo/表8产品退货记录表.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);

            ////"生产指令-步骤序号- 表序号 /&P"
            //int sheetnum;
            //OleDbDataAdapter da = new OleDbDataAdapter("select ID from 清场记录" + " where 生产指令ID=" + ID.ToString(), mySystem.Parameter.connOle);
            //DataTable dt = new DataTable("temp");
            //da.Fill(dt);
            //List<Int32> sheetList = new List<Int32>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            //sheetnum = sheetList.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
            //my.PageSetup.RightFooter = CODE + "-" + sheetnum.ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码
            my.PageSetup.RightFooter = dtOuter.Rows[0]["退货申请单编号"] + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码


            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                {
                    label_打印成功 = 0;
                }
                finally
                {
                    if (1 == label_打印成功)
                    {
                        //string str角色;
                        //if (_userState == Parameter.UserState.操作员)
                        //    str角色 = "操作员";
                        //else if (_userState == Parameter.UserState.审核员)
                        //    str角色 = "审核员";
                        //else
                        //    str角色 = "管理员";
                        //string log = "\n=====================================\n";
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + str角色 + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
                        bsOuter.EndEdit();
                        daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }

            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[2, 2].Value = dtOuter.Rows[0]["退货申请单编号"].ToString();
            my.Cells[3, 2].Value = dtOuter.Rows[0]["申请人"].ToString();
            my.Cells[3, 4].Value = Convert.ToDateTime(dtOuter.Rows[0]["申请日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["申请日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["申请日期"].ToString()).Day.ToString() + "日";
            my.Cells[4, 2].Value = dtOuter.Rows[0]["拟退货产品销售订单编号"].ToString();
            my.Cells[4, 4].Value = dtOuter.Rows[0]["客户名称"].ToString();
            my.Cells[5, 2].Value = dtOuter.Rows[0]["产品名称"].ToString();
            my.Cells[5, 4].Value = dtOuter.Rows[0]["产品代码"].ToString();
            my.Cells[6, 2].Value = dtOuter.Rows[0]["产品批号"].ToString();
            my.Cells[6, 4].Value = dtOuter.Rows[0]["拟退货数量"].ToString();
            my.Cells[6, 5].Value = dtOuter.Rows[0]["辅计量单位"].ToString();
            my.Cells[7, 2].Value = dtOuter.Rows[0]["退货理由"].ToString();
            my.Cells[9, 2].Value = dtOuter.Rows[0]["销售总监审批意见"].ToString();
            my.Cells[11, 2].Value = dtOuter.Rows[0]["PA质量部审批意见"].ToString();
            my.Cells[14, 2].Value = dtOuter.Rows[0]["接收人"].ToString();
            my.Cells[14, 4].Value = Convert.ToDateTime(dtOuter.Rows[0]["接收日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["接收日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["接收日期"].ToString()).Day.ToString() + "日";
            my.Cells[15, 2].Value = dtOuter.Rows[0]["外包装单据验收情况"].ToString();
            my.Cells[16, 2].Value = dtOuter.Rows[0]["检验报告单编号"].ToString();
            my.Cells[17, 2].Value = dtOuter.Rows[0]["检验结果"].ToString();
            my.Cells[18, 2].Value = dtOuter.Rows[0]["PA质量部处理建议"].ToString();
            my.Cells[21, 2].Value = dtOuter.Rows[0]["PA生产部处理记录"].ToString();
            my.Cells[24, 2].Value = dtOuter.Rows[0]["PA质量部验证结论"].ToString();
            my.Cells[26, 3].Value = dtOuter.Rows[0]["验证人"].ToString();
            my.Cells[26, 5].Value = Convert.ToDateTime(dtOuter.Rows[0]["验证日期"].ToString()).Year.ToString() + "年 " + Convert.ToDateTime(dtOuter.Rows[0]["验证日期"].ToString()).Month.ToString() + "月 " + Convert.ToDateTime(dtOuter.Rows[0]["验证日期"].ToString()).Day.ToString() + "日";
           
        }

        
       
    }
}
