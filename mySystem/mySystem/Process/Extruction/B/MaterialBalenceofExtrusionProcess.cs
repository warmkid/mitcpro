using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;




//this form is about the 12th picture of the extrusion step
//READ ME :
//this form need to insult many tables, please make sure that all these tablw below contains certain records 
//1.生产指令信息表 
//2.吹膜工序生产和检验记录
//3.吹膜供料记录
//4.吹膜工序废品记录
//5.吹膜工序物料平衡记录

//the operator can't binding with it's textbox but the operator information can be stored in database.


 
namespace mySystem.Extruction.Process
{


    public partial class MaterialBalenceofExtrusionProcess : mySystem.BaseForm
    {
        BindingSource bsOuter;
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        OleDbCommandBuilder cbOuter;
        SqlDataAdapter daOutersql;
        SqlCommandBuilder cbOutersql;
        OleDbConnection connOle;
        DateTime 生产日期;
        private CheckForm check = null;

        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });

        string __待审核 = "__待审核";
        int searchId;
        string __生产指令;
        int __生产指令ID;
        string __生产开始时间;
        string __生产结束时间;
        string tablename1 = "吹膜工序物料平衡记录";
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>

        Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            connOle = Parameter.connOle;
            fill_printer();
            getPeople();
            setUserState();
            __生产指令 = Parameter.proInstruction;
            __生产指令ID = Parameter.proInstruID;
            lbl生产指令.Text = __生产指令;
            getOtherData();
            
            readOuterData(__生产指令);
            removeOuterBind();
            outerBind();
            
            if (0 == dtOuter.Rows.Count)
            {
                DataRow newrow = dtOuter.NewRow();
                newrow = writeOuterDefault(newrow);
                dtOuter.Rows.Add(newrow);
                if (!mySystem.Parameter.isSqlOk)
                {
                    daOuter.Update((DataTable)bsOuter.DataSource);
                }
                else
                {
                    if (((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] == DBNull.Value)
                    {
                        ((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = 0;
                    }
                    //((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = 0;
                    daOutersql.Update((DataTable)bsOuter.DataSource);
                }
                
                readOuterData(__生产指令);
                removeOuterBind();
                outerBind();
                
            }

            searchId = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
            getInstructionInfo();
            重新读取并计算();
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter.Update((DataTable)bsOuter.DataSource);
            }
            else
            {
                daOutersql.Update((DataTable)bsOuter.DataSource);
            }
            
            readOuterData(__生产指令);
            removeOuterBind();
            outerBind();
            searchId = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
           

            setFormState();
            setEnableReadOnly();
        }

        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform,int Id)
            : base(mainform)
        {
            InitializeComponent();
            connOle = Parameter.connOle;
            fill_printer();
            getPeople();
            setUserState();
            searchId=Id;
            readOuterBind(searchId);
            removeOuterBind();
            outerBind();
       
            //get othertime will use current instruction Id
            getInstructionInfo();
            getOtherData();

            重新读取并计算();
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter.Update((DataTable)bsOuter.DataSource);
            }
            else
            {
                daOutersql.Update((DataTable)bsOuter.DataSource);
            }
            
            readOuterBind(searchId);
            removeOuterBind();
            outerBind();
            setFormState();
            setEnableReadOnly();
        }

        private void getOtherData()
        {
            //current instruction information
            //get the instruction start time in table "production instruction"
            getStartTime();
            getEndTime();

        }
        private void getInstructionInfo()
        {
            __生产指令ID = Convert.ToInt32(dtOuter.Rows[0]["生产指令ID"]);
            __生产指令 = Convert.ToString(dtOuter.Rows[0]["生产指令"]);
        }
        private void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", connOle);
                BindingSource bsUser = new BindingSource();
                OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
                daUser.Fill(dtUser);
            }
            else
            {
                SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", mySystem.Parameter.conn);
                BindingSource bsUser = new BindingSource();
                SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
                daUser.Fill(dtUser);
            }
            
            if (dtUser.Rows.Count != 1)
            {
                MessageBox.Show("请确认表单权限信息");
                this.Close();
            }

            //the getPeople and setUserState combine here
            ls操作员 = new List<string>(Regex.Split(dtUser.Rows[0]["操作员"].ToString(), ",|，"));
            ls审核员 = new List<string>(Regex.Split(dtUser.Rows[0]["审核员"].ToString(), ",|，"));

        }




        private void setUserState()
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
        private void setFormState(bool newForm = false)
        {
            //if (newForm)
            //{

            //    _formState = Parameter.FormState.无数据;
            //    return;
            //}
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }

        }
        private void compute()
        {
            重新读取并计算();
        }


        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();

                logForm.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }
        /// <summary>
        ///  for different user, the form will open different controls
        /// </summary>
        private void setEnableReadOnly()
        {
            if (Parameter.FormState.无数据 == _formState)
            {
                setControlFalse();
               
                //lbl生产指令.ReadOnly = false;
                return;
            }
            setControlTrue();
            switch (_userState)
            {
                    
                case Parameter.UserState.操作员: //0--操作员
                    btn打印.Enabled = false;
                    cmb打印机选择.Enabled = false;

                    break;
                case Parameter.UserState.审核员: //1--审核员
                    //the formState is to be checked
                    
                    btn打印.Enabled = true;
                    cmb打印机选择.Enabled = true;
                    break;
                case Parameter.UserState.管理员: //2--管理员
                    break;
                default:
                    break;
            }
        }
        // 为了方便设置控件状态，完成如下两个函数：分别用于设置所有控件可用和所有控件不可用

        //this guarantee the controls 
        private void setControlTrue()
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
            //some textboxes act as column name and row name, so these shoule be forbidden
            btnSave.Enabled = true;
            txb备注.ReadOnly = false;
            btn查看日志.Enabled = true;                        
        }


        /// <summary>
        /// this guarantees the controls are uneditable
        /// </summary>
        private void setControlFalse()
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
                else if (c is Panel)
                {
                    continue;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            //this act as the same in function upper
            //this.panel1.Enabled = true;
            btn查看日志.Enabled = true;
            //only checkman can print
            btn打印.Enabled = true;
            bt查看人员信息.Enabled = true;
        }




        private void getEndTime()
        {
            // 读数据库，没有的话就是当前时间

            String sql;
            


            try
            {
                string sqlStr = "SELECT top 1 生产日期 FROM 吹膜工序生产和检验记录 WHERE 生产指令ID = " + __生产指令ID.ToString() + "ORDER BY ID DESC";
                if (!mySystem.Parameter.isSqlOk)
                {
                    OleDbCommand sqlCmd = new OleDbCommand(sqlStr, connOle);
                    __生产结束时间 = Convert.ToDateTime(sqlCmd.ExecuteScalar().ToString()).ToString("yyyy年MM月dd日");
                }
                else
                {
                    SqlCommand sqlCmd = new SqlCommand(sqlStr, mySystem.Parameter.conn);
                    __生产结束时间 = Convert.ToDateTime(sqlCmd.ExecuteScalar().ToString()).ToString("yyyy年MM月dd日");
                }

                lbl生产结束时间.Text = __生产结束时间;
            }
            catch (Exception ee)
            {
                __生产结束时间 = DateTime.Now.ToString("yyyy年MM月dd日");
                lbl生产结束时间.Text = __生产结束时间;
            }
            // 关闭时，看生产指令是否已经完成，如果还没有完成，保存时间
        }
        

        

        private void getStartTime()
        {
            string sqlStr = "SELECT 开始生产日期 FROM 生产指令信息表 WHERE ID = " + __生产指令ID.ToString();
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbCommand sqlCmd = new OleDbCommand(sqlStr, connOle);
                __生产开始时间 = Convert.ToDateTime(sqlCmd.ExecuteScalar().ToString()).ToString("yyyy年MM月dd日");
            }
            else
            {
                SqlCommand sqlCmd = new SqlCommand(sqlStr, mySystem.Parameter.conn);
                __生产开始时间 = Convert.ToDateTime(sqlCmd.ExecuteScalar().ToString()).ToString("yyyy年MM月dd日");
            }
            
            lbl生产开始时间.Text = __生产开始时间;
        }

         private DataRow writeOuterDefault(DataRow dr)
         {            
             dr["生产指令ID"] = __生产指令ID;
             dr["生产指令"] = __生产指令;
             dr["生产日期"] = __生产开始时间; ;        
             dr["记录员"] = Parameter.userName;

             dr["记录员备注"] = "无";
             dr["记录日期"]=Convert.ToDateTime(DateTime.Now.ToString());
             dr["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString());
             //this part to add log 
             //格式： 
             // =================================================
             // yyyy年MM月dd日，操作员：XXX 创建记录
             string log = "=====================================\n";
             log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 创建记录\n";
             dr["日志"] = log;
             return dr;
         }

         private void 重新读取并计算()
         {
             double eps = 1e-5;
             try
             {
                 string Tname = "吹膜工序生产和检验记录";
                 string Tcol = "累计同规格膜卷重量T";

                 string Aname = "吹膜供料记录";
                 string Acol1 = "外层供料量合计a";
                 string Acol2 = "中内层供料量合计b";
                 string Acol3 = "中层供料量合计c";
                 string where = "生产指令ID";

                 string Uname = "吹膜工序废品记录";
                 string Ucol = "合计不良品数量";
                 string Tsqlcmd = "SELECT sum(" + Tcol + ") FROM " + Tname + " WHERE " + where + " = " + __生产指令ID + ";";
                 string Asqlcmd1 = "SELECT " + Acol1 + " FROM " + Aname + " WHERE " + where + " = " + __生产指令ID + ";";
                 string Asqlcmd2 = "SELECT " + Acol2 + " FROM " + Aname + " WHERE " + where + " = " + __生产指令ID + ";";
                 string Asqlcmd3 = "SELECT " + Acol3 + " FROM " + Aname + " WHERE " + where + " = " + __生产指令ID + ";";
                 string Usqlcmd = "SELECT " + Ucol + " FROM " + Uname + " WHERE " + where + " = " + __生产指令ID + ";";
                 if (!mySystem.Parameter.isSqlOk)
                 {
                     OleDbCommand Tcmd = new OleDbCommand(Tsqlcmd, connOle);
                     OleDbCommand Acmd1 = new OleDbCommand(Asqlcmd1, connOle);
                     OleDbCommand Acmd2 = new OleDbCommand(Asqlcmd2, connOle);
                     OleDbCommand Acmd3 = new OleDbCommand(Asqlcmd3, connOle);
                     OleDbCommand Ucmd = new OleDbCommand(Usqlcmd, connOle);
                     double t = Convert.ToDouble(Tcmd.ExecuteScalar());
                     double a1 = Convert.ToDouble(Acmd1.ExecuteScalar());
                     double a2 = Convert.ToDouble(Acmd2.ExecuteScalar());
                     double a3 = Convert.ToDouble(Acmd3.ExecuteScalar());
                     double a = a1 + a2 + a3;
                     double u = Convert.ToDouble(Ucmd.ExecuteScalar());
                     double rate = t / (t + u) * 100;
                     rate = Convert.ToDouble(rate.ToString("0.00"));
                     double balance = (t + u) / a * 100;
                     balance = Convert.ToDouble(balance.ToString("0.00"));

                     if (Math.Abs(balance - 100) > 2)
                     {
                         MessageBox.Show("物料平衡超出98%--102%!");
                     }
                     dtOuter.Rows[0]["成品重量合计"] = t;
                     dtOuter.Rows[0]["废品量合计"] = u;
                     dtOuter.Rows[0]["领料量"] = a;
                     dtOuter.Rows[0]["重量比成品率"] = rate;
                     dtOuter.Rows[0]["物料平衡"] = balance;
                 }
                 else
                 {
                     // 生产检验记录不止一张表
                     SqlCommand Tcmd = new SqlCommand(Tsqlcmd, mySystem.Parameter.conn);
                     SqlCommand Acmd1 = new SqlCommand(Asqlcmd1, mySystem.Parameter.conn);
                     SqlCommand Acmd2 = new SqlCommand(Asqlcmd2, mySystem.Parameter.conn);
                     SqlCommand Acmd3 = new SqlCommand(Asqlcmd3, mySystem.Parameter.conn);
                     SqlCommand Ucmd = new SqlCommand(Usqlcmd, mySystem.Parameter.conn);
                     double t = Convert.ToDouble(Tcmd.ExecuteScalar());
                     double a1 = Convert.ToDouble(Acmd1.ExecuteScalar());
                     double a2 = Convert.ToDouble(Acmd2.ExecuteScalar());
                     double a3 = Convert.ToDouble(Acmd3.ExecuteScalar());
                     double a = a1 + a2 + a3;
                     double u = Convert.ToDouble(Ucmd.ExecuteScalar());
                     double rate = t / (t + u+eps) * 100;
                     rate = Convert.ToDouble(rate.ToString("0.00"));
                     double balance = (t + u) / (a+eps) * 100;
                     balance = Convert.ToDouble(balance.ToString("0.00"));

                     if (Math.Abs(balance - 100) > 2)
                     {
                         MessageBox.Show("物料平衡超出98%--102%!");
                     }
                     dtOuter.Rows[0]["成品重量合计"] = t;
                     dtOuter.Rows[0]["废品量合计"] = u;
                     dtOuter.Rows[0]["领料量"] = a;
                     dtOuter.Rows[0]["重量比成品率"] = rate;
                     dtOuter.Rows[0]["物料平衡"] = balance;
                 }


             }
             catch (Exception ee)
             {
                 MessageBox.Show("还没开始生产");
             }

         }


         private void readOuterData(string instruc)
         {
             string tablename1 = "吹膜工序物料平衡记录";
             bsOuter = new BindingSource();
             dtOuter = new DataTable(tablename1);
             if (!mySystem.Parameter.isSqlOk)
             {
                 daOuter = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令 ='" + instruc + "';", connOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
                 cbOuter = new OleDbCommandBuilder(daOuter);
                 daOuter.Fill(dtOuter);
             }
             else
             {
                 daOutersql = new SqlDataAdapter("select * from " + tablename1 + " where 生产指令 ='" + instruc + "';", mySystem.Parameter.conn);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
                 cbOutersql = new SqlCommandBuilder(daOutersql);
                 daOutersql.Fill(dtOuter);
             }
             
         }


         private void readOuterBind(int searchId)
         {
             if (!mySystem.Parameter.isSqlOk)
             {
                 string tablename1 = "吹膜工序物料平衡记录";
                 bsOuter = new BindingSource();
                 dtOuter = new DataTable(tablename1);
                 daOuter = new OleDbDataAdapter("select * from " + tablename1 + " where ID =" + searchId + ";", connOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
                 cbOuter = new OleDbCommandBuilder(daOuter);
                 daOuter.Fill(dtOuter);
             }
             else
             {
                 string tablename1 = "吹膜工序物料平衡记录";
                 bsOuter = new BindingSource();
                 dtOuter = new DataTable(tablename1);
                 daOutersql = new SqlDataAdapter("select * from " + tablename1 + " where ID =" + searchId + ";", mySystem.Parameter.conn);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
                 cbOutersql = new SqlCommandBuilder(daOutersql);
                 daOutersql.Fill(dtOuter);
             }
             
         }
         private void outerBind()
         {
             bsOuter.DataSource = dtOuter;

             lbl生产指令.DataBindings.Add("Text", bsOuter.DataSource, "生产指令");
             //lbl生产开始时间.DataBindings.Add("Text", bsOuter.DataSource, "生产日期");
             txb成品重量合计.DataBindings.Add("Text", bsOuter.DataSource, "成品重量合计");
             txb废品量合计.DataBindings.Add("Text", bsOuter.DataSource, "废品量合计");
             txb领料量.DataBindings.Add("Text", bsOuter.DataSource, "领料量");
             txb重量比成品率.DataBindings.Add("Text", bsOuter.DataSource, "重量比成品率");
             txb物料平衡.DataBindings.Add("Text", bsOuter.DataSource, "物料平衡");
             // TODO: BINDING FAILED
             txb记录员.DataBindings.Add("Text", bsOuter.DataSource, "记录员");
             dtp记录日期.DataBindings.Add("Value", bsOuter.DataSource, "记录日期");
             txb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
             dtp审核日期.DataBindings.Add("Value", bsOuter.DataSource, "审核日期");
             txb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
         }

         private void removeOuterBind()
         {
             lbl生产指令.DataBindings.Clear();
             //lbl生产开始时间.DataBindings.Clear();
             txb成品重量合计.DataBindings.Clear();
             txb废品量合计.DataBindings.Clear();
             txb领料量.DataBindings.Clear();
             txb重量比成品率.DataBindings.Clear();
             txb物料平衡.DataBindings.Clear();
             txb记录员.DataBindings.Clear();
             dtp记录日期.DataBindings.Clear();
             txb审核员.DataBindings.Clear();
             dtp审核日期.DataBindings.Clear();
             txb备注.DataBindings.Clear();
         }

       

         private void btnSave_Click(object sender, EventArgs e)
         {
             // calculate();
             string log = "=====================================\n";
             log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 保存\n";
             dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
             bsOuter.EndEdit();
             if (!mySystem.Parameter.isSqlOk)
             {
                 daOuter.Update((DataTable)bsOuter.DataSource);
             }
             else
             {
                 daOutersql.Update((DataTable)bsOuter.DataSource);
             }
             
             readOuterData(__生产指令);
             removeOuterBind();
             outerBind();
             searchId = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
             //btn打印.Enabled = true;
         }

        

         [DllImport("winspool.drv")]
         public static extern bool SetDefaultPrinter(string Name);
         //添加打印机
         private void fill_printer()
         {

             System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
             foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
             {
                 cmb打印机选择.Items.Add(sPrint);
             }
             cmb打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
         }

         private void btn打印_Click(object sender, EventArgs e)
         {
             if (cmb打印机选择.Text == "")
             {
                 MessageBox.Show("选择一台打印机");
                 return;
             }
             SetDefaultPrinter(cmb打印机选择.Text);
             print(false);
             GC.Collect();
         }
		 public int print(bool preview)
         {
		 // 打开一个Excel进程
             Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
             // 利用这个进程打开一个Excel文件
             //System.IO.Directory.GetCurrentDirectory;
             Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R12 吹膜工序物料平衡记录.xlsx");
             // 选择一个Sheet，注意Sheet的序号是从1开始的
             Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[3];
             // 设置该进程是否可见
             //oXL.Visible = true;
             // 修改Sheet中某行某列的值

             my.Cells[3, 2].Value = dtOuter.Rows[0]["生产指令"]; //lbl生产指令.Text.ToString();
             my.Cells[3, 4].Value = dtOuter.Rows[0]["生产日期"]; //lbl生产开始时间.Text.ToString();
             my.Cells[6, 1].Value = dtOuter.Rows[0]["成品重量合计"]; //txb成品重量合计.Text;
             my.Cells[6, 2].Value = dtOuter.Rows[0]["废品量合计"]; //txb废品量合计.Text;
             my.Cells[6, 3].Value = dtOuter.Rows[0]["领料量"]; //txb领料量.Text;
             my.Cells[6, 4].Value = dtOuter.Rows[0]["重量比成品率"]; //txb重量比成品率.Text;
             my.Cells[6, 5].Value = dtOuter.Rows[0]["物料平衡"]; //txb物料平衡.Text;
             my.Cells[7, 2].Value = dtOuter.Rows[0]["备注"];
             my.Cells[8, 1].Value = "记录员/日期：" + dtOuter.Rows[0]["记录员"] +"   "+Convert.ToDateTime(dtOuter.Rows[0]["记录日期"]).ToString("yyyy年MM月dd日");
             my.Cells[8, 4].Value = dtOuter.Rows[0]["审核员"] + Convert.ToDateTime(dtOuter.Rows[0]["审核日期"]).ToString("yyyy年MM月dd日"); 
             

			if(preview)
			{
             // 让这个Sheet为被选中状态
                 my.Select();  
				 oXL.Visible=true; //加上这一行  就相当于预览功能
                 return 0;
			}
			else
			{
                //footprint
                my.PageSetup.RightFooter = __生产指令 + "-12-001 &P/" + wb.ActiveSheet.PageSetup.Pages.Count;
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch { }
                int pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
                 // 关闭文件，false表示不保存
                 wb.Close(false);
                 // 关闭Excel进程
                 oXL.Quit();
                 // 释放COM资源
                 Marshal.ReleaseComObject(wb);
                 Marshal.ReleaseComObject(oXL);
                 oXL = null;
                 wb = null;
                 my = null;
                 return pageCount;
			}

         }

         private void bt查看人员信息_Click(object sender, EventArgs e)
         {
             if (!mySystem.Parameter.isSqlOk)
             {
                 OleDbDataAdapter da;
                 DataTable dt;
                 da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜工序物料平衡记录'", mySystem.Parameter.connOle);
                 dt = new DataTable("temp");
                 da.Fill(dt);
                 String str操作员 = dt.Rows[0]["操作员"].ToString();
                 String str审核员 = dt.Rows[0]["审核员"].ToString();
                 String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                 MessageBox.Show(str人员信息);
             }
             else
             {
                 SqlDataAdapter da;
                 DataTable dt;
                 da = new SqlDataAdapter("select * from 用户权限 where 步骤='吹膜工序物料平衡记录'", mySystem.Parameter.conn);
                 dt = new DataTable("temp");
                 da.Fill(dt);
                 String str操作员 = dt.Rows[0]["操作员"].ToString();
                 String str审核员 = dt.Rows[0]["审核员"].ToString();
                 String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                 MessageBox.Show(str人员信息);
             }
             
         }

         private void MaterialBalenceofExtrusionProcess_FormClosing(object sender, FormClosingEventArgs e)
         {
             // 如果生产指令已经完成，保存结束时间
             //String sql;
             //SqlDataAdapter da;
             //DataTable dt;
             //sql = "select 状态 from 生产指令信息表 where ID=" + __生产指令ID;
             //da = new SqlDataAdapter(sql, mySystem.Parameter.conn);
             //dt = new DataTable();
             //da.Fill(dt);
             //if (dt.Rows.Count > 0)
             //{
             //    if (Convert.ToInt32(dt.Rows[0]["状态"]) != 4)
             //    {
             //        sql = "update 吹膜工序物料平衡记录 set 生产日期='" + DateTime.Now.ToString("yyyy年MM月dd日") + "' where 生产指令ID=" + __生产指令ID;
             //        SqlCommand comm = new SqlCommand(sql, mySystem.Parameter.conn);
             //        comm.ExecuteNonQuery();
             //    }
             //}

         }

        
       
    }
}
