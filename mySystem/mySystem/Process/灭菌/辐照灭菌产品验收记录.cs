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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace mySystem.Process.灭菌
{
    public partial class 辐照灭菌产品验收记录 : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_out;
        private OleDbDataAdapter da_out;
        private BindingSource bs_out;
        private OleDbCommandBuilder cb_out;
        private string person_操作员;
        private string person_审核员;
        private List<string> list_操作员;
        private List<string> list_审核员;


        //private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        //private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        // 需要保存的状态
        /// <summary>
        /// 1:操作员，2：审核员，4：管理员
        /// </summary>
        Parameter.UserState _userState;
        /// <summary>
        /// -1:无数据，0：未保存，1：待审核，2：审核通过，3：审核未通过
        /// </summary>
        Parameter.FormState _formState;

        private string code;//灭菌委托单编号
        private int instruID;

        public 辐照灭菌产品验收记录(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            getPeople();
            setUserState();
            getOtherData();

            foreach (Control c in this.Controls)
                c.Enabled = false;
            cb委托单号.Enabled = true;
            bt插入查询.Enabled = true;
            cb委托单号.SelectedItem = mySystem.Parameter.miejunInstruction;
            code = cb委托单号.Text;
            instruID = mySystem.Parameter.miejunInstruID;
        }

        public 辐照灭菌产品验收记录(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();

            getPeople();
            setUserState();
            getOtherData();

            string asql = "select * from 辐照灭菌产品验收记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            code = tempdt.Rows[0]["灭菌委托单编号"].ToString();

            

            if (!cb委托单号.Items.Contains(code))
            {
                cb委托单号.Items.Add(code);
            }

            instruID = id;

            readOuterData(code);
            outerBind();

            //??
            tb辐照批号.DataBindings.Clear();
            tb辐照批号.Text = dt_out.Rows[0]["辐照批号"].ToString();
            tb报告编号.DataBindings.Clear();
            tb报告编号.Text = dt_out.Rows[0]["报告编号"].ToString();
            tb验收人.DataBindings.Clear();
            tb验收人.Text = dt_out.Rows[0]["验收人"].ToString();

            setFormState();
            setEnableReadOnly();

            cb委托单号.Enabled = false;
            bt插入查询.Enabled = false;

            
        }

        void getPeople()
        {
            list_操作员 = new List<string>();
            list_审核员 = new List<string>();

            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='辐照灭菌产品验收记录'", mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
                string[] s = Regex.Split(person_操作员, ",|，");
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != "")
                        list_操作员.Add(s[i]);
                }
                string[] s1 = Regex.Split(person_审核员, ",|，");
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] != "")
                        list_审核员.Add(s1[i]);
                }
            }

        }

        void setUserState()
        {
            //if (mySystem.Parameter.userName == person_操作员)
            //    stat_user = 0;
            //else if (mySystem.Parameter.userName == person_审核员)
            //    stat_user = 1;
            //else
            //    stat_user = 2;

            _userState = Parameter.UserState.NoBody;
            if (list_操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (list_审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
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

        private void getOtherData()
        {
            //获取设置中运输商
            OleDbDataAdapter tda = new OleDbDataAdapter("select * from 设置运输商", mySystem.Parameter.connOle);
            DataTable tdt = new DataTable("运输商");
            tda.Fill(tdt);
            foreach (DataRow tdr in tdt.Rows)
            {
                cb运输商.Items.Add(tdr["运输商"].ToString());
                cb运输商内.Items.Add(tdr["运输商"].ToString());
            }
            //获取设置中辐照商
            OleDbDataAdapter tda2 = new OleDbDataAdapter("select * from 设置辐照单位", mySystem.Parameter.connOle);
            DataTable tdt2 = new DataTable("辐照单位");
            tda2.Fill(tdt2);
            foreach (DataRow tdr in tdt2.Rows)
            {
                cb辐照商.Items.Add(tdr["辐照单位"].ToString());
            }
            //添加打印机
            fill_printer();

            //检验结果下拉框
            cb检查结果1.Items.Add("合格");
            cb检查结果1.Items.Add("不合格");
            cb检查结果2.Items.Add("合格");
            cb检查结果2.Items.Add("不合格");
            cb检查结果3.Items.Add("合格");
            cb检查结果3.Items.Add("不合格");
            cb检查结果4.Items.Add("合格");
            cb检查结果4.Items.Add("不合格");
            cb检查结果5.Items.Add("合格");
            cb检查结果5.Items.Add("不合格");
            cb检查结果6.Items.Add("合格");
            cb检查结果6.Items.Add("不合格");

            //添加委托单号
            OleDbDataAdapter tda3 = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单 where 状态 = 2", mySystem.Parameter.connOle);
            DataTable tdt3 = new DataTable("Gamma射线辐射灭菌委托单");
            tda3.Fill(tdt3);
            foreach (DataRow tdr in tdt3.Rows)
            {
                cb委托单号.Items.Add(tdr["委托单号"].ToString());
            }
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            bool rt = save();

            //控件可见性
            if (rt && _userState == Parameter.UserState.操作员)
                bt发送审核.Enabled = true;
        }

        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
            readOuterData(code);
            outerBind();
            return true;
        }

        bool input_Judge()
        {
            //判断合法性
            //辐照商
            if (cb辐照商.Text == "")
            {
                MessageBox.Show("辐照商不能为空");
                return false;
            }

            if (cb运输商内.Text == "")
            {
                MessageBox.Show("合格运输商不能为空");
                return false;
            }
            //if (cb运输商.Text == "")
            //{
            //    MessageBox.Show("运输商不能为空");
            //    return false;
            //}
            //if (mySystem.Parameter.NametoID(tb操作人.Text) <= 0)
            //{
            //    MessageBox.Show("操作人ID不存在");
            //    return false;
            //}
            //验收人
            if (mySystem.Parameter.NametoID(tb验收人.Text) <= 0)
            {
                MessageBox.Show("验收人ID不存在");
                return false;
            }

            return true;


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
        private void bt打印_Click(object sender, EventArgs e)
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

        //查找打印的表序号
        private int find_indexofprint()
        {
            List<int> list_id = new List<int>();
            string asql = "select * from 辐照灭菌产品验收记录 where 灭菌委托单ID = " + instruID;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);

            for (int i = 0; i < tempdt.Rows.Count; i++)
                list_id.Add((int)tempdt.Rows[i]["ID"]);
            return list_id.IndexOf((int)dt_out.Rows[0]["ID"]) + 1;
        }

        public void print(bool b)
        {
            int label_打印成功 = 1;

            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/miejun/SOP-MFG-106-R02B 辐照灭菌产品验收记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[1];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            //"生产指令-步骤序号- 表序号 /&P"
            my.PageSetup.RightFooter = code + "-" + find_indexofprint().ToString("D3") + " &P/" + wb.ActiveSheet.PageSetup.Pages.Count;  // &P 是页码

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
                { label_打印成功 = 0; }
                finally
                {
                    if (1 == label_打印成功)
                    {
                        string log = "\n=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 完成打印\n";
                        dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;
                        bs_out.EndEdit();
                        da_out.Update((DataTable)bs_out.DataSource);
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
        //填充excel
        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[3, 4].Value = "灭菌委托单编号：" + dt_out.Rows[0]["灭菌委托单编号"].ToString();
            my.Cells[4, 2].Value = Convert.ToDateTime(dt_out.Rows[0]["辐照产品运回日期"]).ToString("yyyy年MM月dd日");
            my.Cells[6, 3].Value = "应是合格辐照商。\n辐照商：" + dt_out.Rows[0]["辐照商"].ToString();
            my.Cells[6, 5].Value = dt_out.Rows[0]["辐照供应商检查结果"].ToString();
            my.Cells[7, 3].Value = "应是合格运输商。\n运输商：" + dt_out.Rows[0]["检查运输商"].ToString();
            my.Cells[7, 5].Value = dt_out.Rows[0]["运输商检查结果"].ToString();
            my.Cells[8, 1].Value = String.Format("3. 辐照产品数量:{0}箱\n              计{1}托", dt_out.Rows[0]["辐照产品数量箱"].ToString(), dt_out.Rows[0]["辐照产品数量托"].ToString());
            my.Cells[8, 5].Value = dt_out.Rows[0]["辐照产品数量检查结果"].ToString();
            my.Cells[9, 5].Value = dt_out.Rows[0]["外包装检查结果"].ToString();
            my.Cells[10, 5].Value = dt_out.Rows[0]["标签检查结果"].ToString();
            my.Cells[11, 3].Value = String.Format("每批照射产品均应有照射报告，且报告中辐照批号与辐照标签上的批号一致。\n\n报告编号：{0}\n\n辐照批号：{1}", dt_out.Rows[0]["报告编号"].ToString(), dt_out.Rows[0]["辐照批号"].ToString());
            my.Cells[11, 5].Value = dt_out.Rows[0]["辐照检查结果"].ToString();
            my.Cells[12, 5].Value = "取样时间：" + Convert.ToDateTime(dt_out.Rows[0]["取样时间"]).ToString("yyyy年MM月dd日"); 
            my.Cells[13, 1].Value = "说明：" + dt_out.Rows[0]["说明"].ToString();
            if (Convert.ToBoolean(dt_out.Rows[0]["符合要求"]))
            {
                my.Cells[15, 1].Value = "结论：  辐照产品符合要求，正常入库☑\n不符合要求，按不合格品处理□";
            }
            else { my.Cells[15, 1].Value = "结论：  辐照产品符合要求，正常入库□\n不符合要求，按不合格品处理☑"; }
            my.Cells[17, 1].Value = String.Format("验收人：{0}    {1}        复核人：{2}     {3}",
                dt_out.Rows[0]["验收人"].ToString(), Convert.ToDateTime(dt_out.Rows[0]["验收日期"]).ToString("yyyy年MM月dd日"),
                dt_out.Rows[0]["审核人"].ToString(), Convert.ToDateTime(dt_out.Rows[0]["审核日期"]).ToString("yyyy年MM月dd日"));
            my.Cells[18, 1].Value = String.Format("运输商：{0}            操作人：{1}      {2}",
                dt_out.Rows[0]["运输商"].ToString(), dt_out.Rows[0]["操作人"].ToString(), Convert.ToDateTime(dt_out.Rows[0]["操作日期"]).ToString("yyyy年MM月dd日"));    
        }

        private void bt插入查询_Click(object sender, EventArgs e)
        {
            //查看是否存在对应的委托单ID
            if (id_findby_code(cb委托单号.Text) <= 0)
            {
                MessageBox.Show("该灭菌委托单ID不存在");
                return;
            }
            code = cb委托单号.Text;
            instruID = id_findby_code(cb委托单号.Text);

            readOuterData(code);
            outerBind();
            if (dt_out.Rows.Count <= 0 && _userState != Parameter.UserState.操作员)
            {
                MessageBox.Show("只有操作员可以新建记录");
                return;
            }
            if (dt_out.Rows.Count <= 0)
            {
                DataRow dr = dt_out.NewRow();
                dr = writeOuterDefault(dr);
                dt_out.Rows.Add(dr);
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(code);
                outerBind();
            }
            

            setFormState();
            setEnableReadOnly();

            cb委托单号.Enabled = false;
            bt插入查询.Enabled = false;
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string paracode)
        {
            dt_out = new DataTable("辐照灭菌产品验收记录");
            bs_out = new BindingSource();
            da_out = new OleDbDataAdapter(@"select * from 辐照灭菌产品验收记录 where 灭菌委托单编号='" + paracode + "'", mySystem.Parameter.connOle);
            cb_out = new OleDbCommandBuilder(da_out);
            da_out.Fill(dt_out);
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单 where 委托单号='"+code+"'",mySystem.Parameter.connOle);
            DataTable dt = new DataTable("射线辐射灭菌委托单");
            da.Fill(dt);
            dr["灭菌委托单编号"] = cb委托单号.Text;
            dr["灭菌委托单ID"] = id_findby_code(cb委托单号.Text);
            dr["辐照产品运回日期"] = DateTime.Now;
            dr["辐照商"] = dt.Rows[0]["辐照单位"].ToString();
            dr["检查运输商"] = dt.Rows[0]["运输商"].ToString();
            dr["辐照产品数量箱"] = Convert.ToInt32(dt.Rows[0]["合计箱数"]);
            dr["辐照产品数量托"] = Convert.ToInt32(dt.Rows[0]["合计托数"]);
            dr["辐照供应商检查结果"] = "合格";
            dr["运输商检查结果"] = "合格";
            
            dr["辐照产品数量检查结果"] = "合格";
            dr["外包装检查结果"] = "合格";
            dr["标签检查结果"] = "合格";
            dr["辐照检查结果"] = "合格";
            dr["取样时间"] = DateTime.Now;
            dr["符合要求"] = true;
            dr["不符合要求"] = false;

            dr["验收日期"] = DateTime.Now;
            dr["审核人"] = "";
            dr["审核日期"] = DateTime.Now;
            dr["审核是否通过"] = false;
            dr["验收人"] = mySystem.Parameter.userName;
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = DateTime.Now;
            dr["说明"] = "无";
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + ":" + mySystem.Parameter.userName + " 新建记录\n";
            log += "灭菌委托单编号：" + cb委托单号.Text + "\n";
            dr["日志"] = log;
            return dr;

        }

        void outerBind()
        {
            //解除之前的绑定
            cb委托单号.DataBindings.Clear();
            dtp运回日期.DataBindings.Clear();
            cb辐照商.DataBindings.Clear();
            cb检查结果1.DataBindings.Clear();
            cb运输商内.DataBindings.Clear();
            cb检查结果2.DataBindings.Clear();
            tb箱.DataBindings.Clear();
            tb托.DataBindings.Clear();
            cb检查结果3.DataBindings.Clear();
            cb检查结果4.DataBindings.Clear();
            cb检查结果5.DataBindings.Clear();
            cb检查结果6.DataBindings.Clear();
            tb报告编号.DataBindings.Clear();
            tb辐照批号.DataBindings.Clear();
            dtp取样时间.DataBindings.Clear();
            tb说明.DataBindings.Clear();
            ckb符合要求.DataBindings.Clear();
            ckb不符合要求.DataBindings.Clear();
            tb验收人.DataBindings.Clear();
            dtp验收日期.DataBindings.Clear();
            tb操作人.DataBindings.Clear();
            cb运输商.DataBindings.Clear();
            tb审核人.DataBindings.Clear();
            dtp审核日期.DataBindings.Clear();
            dtp操作日期.DataBindings.Clear();

            bs_out.DataSource = dt_out;

            cb委托单号.DataBindings.Add("SelectedItem", bs_out.DataSource, "灭菌委托单编号");
            dtp运回日期.DataBindings.Add("Value", bs_out.DataSource, "辐照产品运回日期");
            cb辐照商.DataBindings.Add("Text", bs_out.DataSource, "辐照商");
            cb检查结果1.DataBindings.Add("Text", bs_out.DataSource, "辐照供应商检查结果");
            cb运输商内.DataBindings.Add("Text", bs_out.DataSource, "检查运输商");
            cb检查结果2.DataBindings.Add("Text", bs_out.DataSource, "运输商检查结果");
            tb箱.DataBindings.Add("Text", bs_out.DataSource, "辐照产品数量箱");
            tb托.DataBindings.Add("Text", bs_out.DataSource, "辐照产品数量托");
            cb检查结果3.DataBindings.Add("Text", bs_out.DataSource, "辐照产品数量检查结果");
            cb检查结果4.DataBindings.Add("Text", bs_out.DataSource, "外包装检查结果");
            cb检查结果5.DataBindings.Add("Text", bs_out.DataSource, "标签检查结果");
            cb检查结果6.DataBindings.Add("Text", bs_out.DataSource, "辐照检查结果");
            tb报告编号.DataBindings.Add("Text", bs_out.DataSource, "报告编号");
            tb辐照批号.DataBindings.Add("Text", bs_out.DataSource, "辐照批号");
            dtp取样时间.DataBindings.Add("Value", bs_out.DataSource, "取样时间");
            tb说明.DataBindings.Add("Text", bs_out.DataSource, "说明");
            ckb符合要求.DataBindings.Add("Checked", bs_out.DataSource, "符合要求");
            ckb不符合要求.DataBindings.Add("Checked", bs_out.DataSource, "不符合要求");
            tb验收人.DataBindings.Add("Text", bs_out.DataSource, "验收人");
            dtp验收日期.DataBindings.Add("Value", bs_out.DataSource, "验收日期");
            tb操作人.DataBindings.Add("Text", bs_out.DataSource, "操作人");
            cb运输商.DataBindings.Add("Text", bs_out.DataSource, "运输商");
            tb审核人.DataBindings.Add("Text", bs_out.DataSource, "审核人");
            dtp审核日期.DataBindings.Add("Value", bs_out.DataSource, "审核日期");
            dtp操作日期.DataBindings.Add("Value", bs_out.DataSource, "操作日期");

        }

        void setFormState()
        {
            string s = dt_out.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dt_out.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }
        }

        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
            bt日志.Enabled = true;
            bt打印.Enabled = true;
            cb打印机.Enabled = true;
        }
        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
                c.Enabled = true;
            bt发送审核.Enabled = false;
            bt审核.Enabled = false;
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
                    bt审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (Parameter.UserState.操作员 == _userState)
            {
                if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }
        }

        //通过委托单编号查找对应ID
        private int id_findby_code(string code)
        {
            DataTable dt = new DataTable("Gamma射线辐射灭菌委托单");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from Gamma射线辐射灭菌委托单 where 委托单号='"+code+"'", mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                return (int)dt.Rows[0]["ID"];
            }
            else
                return -1;
        }

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='辐照灭菌产品验收记录' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "辐照灭菌产品验收记录";
                dr["对应ID"] = (int)dt_out.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

            dt_out.Rows[0]["审核人"] = "__待审核";
            dt_out.Rows[0]["审核日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();

            // 添加台账,先读委托单中的内表,修改状态为2
            da_temp = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单 where 委托单号='" + cb委托单号.Text + "'", mySystem.Parameter.connOle);
            cb_temp = new OleDbCommandBuilder(da_temp);
            dt_temp = new DataTable();
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0]["状态"] = 2;
            da_temp.Update(dt_temp);
            DateTime 委托日期 = Convert.ToDateTime(dt_temp.Rows[0]["委托日期"]);
            da_temp = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单详细信息 where TGamma射线辐射灭菌委托单详细信息ID="
                + id_findby_code(cb委托单号.Text), mySystem.Parameter.connOle);
            dt_temp = new DataTable();
            da_temp.Fill(dt_temp);
            OleDbDataAdapter da_台账 = new OleDbDataAdapter("select * from 辐照灭菌台帐详细信息 where 0=1",mySystem.Parameter.connOle);
            DataTable dt_台账 = new DataTable("辐照灭菌台帐详细信息");
            OleDbCommandBuilder cb_台账 = new OleDbCommandBuilder(da_台账);
            BindingSource bs_台账 = new BindingSource();
            da_台账.Fill(dt_台账);
            foreach (DataRow drtemp in dt_temp.Rows)
            {
                DataRow dr台账 = dt_台账.NewRow();
                dr台账["委托单号"] = cb委托单号.Text;
                dr台账["委托日期"] = 委托日期;
                dr台账["产品代码"] = drtemp["产品代码"].ToString();
                dr台账["产品数量只"] = Convert.ToInt32(drtemp["数量只"]);
                dr台账["产品数量箱"] = Convert.ToInt32(drtemp["数量箱"]);
                dr台账["登记员"] = dt_out.Rows[0]["操作人"].ToString();
                dt_台账.Rows.Add(dr台账);
            }
            da_台账.Update(dt_台账);
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            (new mySystem.Other.LogForm()).setLog(dt_out.Rows[0]["日志"].ToString()).Show();
        }

        public override void CheckResult()
        {
            //获得审核信息
            dt_out.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dt_out.Rows[0]["审核日期"] = checkform.time;
            dt_out.Rows[0]["审核意见"] = checkform.opinion;
            dt_out.Rows[0]["审核是否通过"] = checkform.ischeckOk;

            //状态
            setControlFalse();

            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='辐照灭菌产品验收记录' and 对应ID=" + (int)dt_out.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "\n=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt_out.Rows[0]["日志"] = dt_out.Rows[0]["日志"].ToString() + log;

            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);

            base.CheckResult();
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();  
        }

        private void ckb符合要求_CheckedChanged(object sender, EventArgs e)
        {           
            bool ckout = ckb符合要求.Checked;
            dt_out.Rows[0]["符合要求"] = ckout;
            dt_out.Rows[0]["不符合要求"] = !ckout;
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
        }

        private void ckb不符合要求_CheckedChanged(object sender, EventArgs e)
        {
            bool ckout = ckb不符合要求.Checked;
            dt_out.Rows[0]["不符合要求"] = ckout;
            dt_out.Rows[0]["符合要求"] = !ckout;
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
        }
    }
}
