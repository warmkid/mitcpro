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
    public partial class 产品退货评审单1 : BaseForm
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

        CheckForm ckform;

        public 产品退货评审单1(MainForm mainform, string code):base(mainform)
        {
            //conn = new OleDbConnection(strConnect);
            //conn.Open();
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
            SqlDataAdapter da;
            DataTable dt;

            ls操作员 = new List<string>();
            ls审核员 = new List<string>();
            da = new SqlDataAdapter("select * from 库存用户权限 where 步骤='产品退货评审单1'", mySystem.Parameter.conn);
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
            string s = dtOuter.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核结果"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        void setEnableReadOnly()
        {
            if (Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (Parameter.UserState.审核员 == _userState)
            {
                if (Parameter.FormState.待审核 == _formState)
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
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
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;

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
            btn日志.Enabled = true;
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
            daOuter = new SqlDataAdapter("select * from 产品退货评审单1 where 退货申请单编号='" + code + "'", mySystem.Parameter.conn);
            dtOuter = new DataTable("产品退货评审单1");
            cbOute = new SqlCommandBuilder(daOuter);
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }



        //DataRow writeOuterDefault(DataRow dr)
        //{
        //    dr["退货申请单编号"] = _code;
        //    dr["接收人"] = mySystem.Parameter.userName;
        //    dr["接收日期"] = DateTime.Now;
        //    dr["审核日期"] = DateTime.Now;
        //    SqlDataAdapter da = new SqlDataAdapter("select * from 产品退货申请单 where 退货申请单编号='" + _code + "'", conn);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    dr["拟退货产品销售订单编号"] = dt.Rows[0]["拟退货产品销售订单编号"];
        //    dr["客户名称"] = dt.Rows[0]["客户名称"];
        //    dr["产品名称"] = dt.Rows[0]["产品名称"];
        //    dr["产品代码"] = dt.Rows[0]["产品代码"];
        //    dr["产品批号"] = dt.Rows[0]["产品批号"];
        //    dr["拟退货数量"] = dt.Rows[0]["拟退货数量"];
        //    dr["辅计量单位"] = dt.Rows[0]["辅计量单位"];
        //    return dr;
        //}

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

        private void btn确认_Click(object sender, EventArgs e)
        {
            save();
            if (_userState == Parameter.UserState.操作员) btn提交审核.Enabled = true;
        }

        void save()
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_code);
            outerBind();
        }

        private void btn提交审核_Click(object sender, EventArgs e)
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

            da = new SqlDataAdapter("select * from 待审核 where 表名='产品退货评审单1' and 对应ID=" + _id, mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["表名"] = "产品退货评审单1";
            dr["对应ID"] = _id;
            dt.Rows.Add(dr);
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = "__待审核";

            string log = "\n=====================================\n";

            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";

            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;




            save();
            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new CheckForm(this);
            ckform.ShowDialog();
        }

        public override void CheckResult()
        {
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            DataTable dt;

            da = new SqlDataAdapter("select * from 待审核 where 表名='产品退货评审单1' and 对应ID=" + _id, mySystem.Parameter.conn);
            cb = new SqlCommandBuilder(da);

            dt = new DataTable("temp");
            da.Fill(dt);
            dt.Rows[0].Delete();
            da.Update(dt);

            dtOuter.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核结果"] = ckform.ischeckOk;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;
            string log = "\n=====================================\n";
       
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";

            log += "审核结果：" + (ckform.ischeckOk == true ? "通过\n" : "不通过\n");

            log += "审核意见：" + ckform.opinion;

            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            if (ckform.ischeckOk)
            {
                // 自动生产评审单2
                da = new SqlDataAdapter("select * from 产品退货评审单2 where 退货申请单编号='" + _code + "'", mySystem.Parameter.conn);
                cb = new SqlCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    MessageBox.Show("产品退货评审单（2）已存在!");
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["退货申请单编号"] = dtOuter.Rows[0]["退货申请单编号"];
                    dr["PA生产部经理"] = "";
                    dr["评审日期"] = DateTime.Now;
                    dr["审核日期"] = DateTime.Now;
                    dr["审核结果"] = false;//默认值
                    for (int i = 4; i <= 10; ++i)
                    {
                        dr[i] = dtOuter.Rows[0][i];
                    }
                    dr["PA质量部处理建议"] = dtOuter.Rows[0]["PA质量部处理建议"];
                    string log1 = "\n=====================================\n";

                    log1 += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + " 新建记录\n";

                    dr["日志"] = log1;
                    dt.Rows.Add(dr);
                    da.Update(dt);
                    MessageBox.Show("产品退货评审单（2）已生成！");
                }

            }

            //String log = "===================================\n";
            //log += DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            //log += "\n审核员：" + mySystem.Parameter.userName + " 审核完毕\n";
            //log += "审核结果为：" + (ckform.ischeckOk ? "通过" : "不通过") + "\n";
            //log += "审核意见为：" + ckform.opinion + "\n";
            //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            save();
            setFormState();
            setEnableReadOnly();

            btn审核.Enabled = false;
            base.CheckResult();
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

        public int print(bool b)
        {
            int label_打印成功 = 1;
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/库存/表6退货产品评审单.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            my.PageSetup.RightFooter = "&P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码


            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
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

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[2, 2].Value = dtOuter.Rows[0]["退货申请单编号"].ToString();
            my.Cells[3, 2].Value = dtOuter.Rows[0]["PAQA经理"].ToString();
            my.Cells[3, 4].Value = Convert.ToDateTime(dtOuter.Rows[0]["评审日期"]).ToString("yyyy年MM月dd日");
            my.Cells[4, 2].Value = dtOuter.Rows[0]["拟退货产品销售订单编号"].ToString();
            my.Cells[4, 4].Value = dtOuter.Rows[0]["客户名称"].ToString();
            my.Cells[5, 2].Value = dtOuter.Rows[0]["产品名称"].ToString();
            my.Cells[5, 4].Value = dtOuter.Rows[0]["产品代码"].ToString();
            my.Cells[6, 2].Value = dtOuter.Rows[0]["产品批号"].ToString();
            my.Cells[6, 4].Value = dtOuter.Rows[0]["拟退货数量"].ToString();
            my.Cells[6, 5].Value = dtOuter.Rows[0]["辅计量单位"].ToString();
            my.Cells[7, 2].Value = dtOuter.Rows[0]["检验报告单编号"].ToString();
            my.Cells[8, 2].Value = dtOuter.Rows[0]["检验结果"].ToString();           
            my.Cells[9, 2].Value = dtOuter.Rows[0]["PA质量部处理建议"].ToString();

        }

        private void btn日志_Click(object sender, EventArgs e)
        {
            try
            {
                (new mySystem.Other.LogForm()).setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
            }
        }



    }
}
