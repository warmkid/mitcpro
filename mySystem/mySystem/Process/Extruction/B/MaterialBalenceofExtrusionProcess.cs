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
        BindingSource bsBalance;
        DataTable dtBalance;
        OleDbDataAdapter daBalance;
        OleDbCommandBuilder cbBalance;
        OleDbConnection connOle;
        DateTime 生产日期;
        private CheckForm check = null;

        List<string> list操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> list审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });

        string __待审核 = "__待审核";
        int searchId;
        string instructionStr;
        int instructionInt;
        string tablename1 = "吹膜工序物料平衡记录";
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;
        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            connOle = Parameter.connOle;
            getPeople();
            setUserState();
            instructionStr= Parameter.proInstruction;
            instructionInt = Parameter.proInstruID;
            txb生产指令.Text = instructionStr;
            getOtherData();
            //
            readBalance(instructionStr);
            removeBindBalance();
            BindBalance();
            
            if (0 == dtBalance.Rows.Count)
            {
                DataRow newrow = dtBalance.NewRow();
                newrow = writeBalance(newrow);
                dtBalance.Rows.Add(newrow);
                daBalance.Update((DataTable)bsBalance.DataSource);
                readBalance(instructionStr);
                removeBindBalance();
                BindBalance();
                
            }

            searchId = Convert.ToInt32(dtBalance.Rows[0]["ID"]);
            getInstructionInfo();
            compute();
            daBalance.Update((DataTable)bsBalance.DataSource);
            readBalance(instructionStr);
            removeBindBalance();
            BindBalance();
            searchId = Convert.ToInt32(dtBalance.Rows[0]["ID"]);
           

            setFormState();
            setEnableReadOnly();
        }

        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform,int Id)
            : base(mainform)
        {
            InitializeComponent();
            connOle = Parameter.connOle;
            getPeople();
            setUserState();
            searchId=Id;
            readBalance(searchId);
            removeBindBalance();
            BindBalance();

            //get othertime will use current instruction Id
            getInstructionInfo();
            getOtherData();
           

            compute();
            daBalance.Update((DataTable)bsBalance.DataSource);
            readBalance(searchId);
            removeBindBalance();
            BindBalance();
            setFormState();
            setEnableReadOnly();
        }

        private void getOtherData()
        {
            //current instruction information
            
            //get the instruction start time in table "production instruction"
            getStartTime();

        }
        private void getInstructionInfo()
        {
            instructionInt = Convert.ToInt32(dtBalance.Rows[0]["生产指令ID"]);
            instructionStr = Convert.ToString(dtBalance.Rows[0]["生产指令"]);
        }
        private void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", connOle);
            BindingSource bsUser = new BindingSource();
            OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
            daUser.Fill(dtUser);
            if (dtUser.Rows.Count != 1)
            {
                MessageBox.Show("请确认表单权限信息");
                this.Close();
            }

            //the getPeople and setUserState combine here
            list操作员 = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
            list审核员 = new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });

        }
        private void compute()
        {
            readAgain();
        }
        private void setUserState()
        {
            if (list操作员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 0;
            }
            else if (list审核员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 1;
            }
            else
            {
                userState = 2;
            }
        }
        private void setFormState()
        {
            if ("" == dtBalance.Rows[0]["审核人"].ToString().Trim())
            {
                //this means the record hasn't been saved
                formState = 0;
            }
            else if (__待审核 == dtBalance.Rows[0]["审核人"].ToString().Trim())
            {
                //this means this record should be checked
                formState = 1;
            }
            else if (Convert.ToBoolean(dtBalance.Rows[0]["审核是否通过"]))
            {
                //this means this record has been checked
                formState = 2;
            }
            else
            {
                //this means the record has been checked but need more modification
                formState = 3;
            }
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            { MessageBox.Show(dtBalance.Rows[0]["日志"].ToString()); }
            catch
            { MessageBox.Show(" !"); }
        }
        /// <summary>
        ///  for different user, the form will open different controls
        /// </summary>
        private void setEnableReadOnly()
        {
            switch (userState)
            {
                case 0: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (0 == formState || 3 == formState)
                    {
                        setControlTrue();
                        btnSave.Enabled = true;
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (1 == formState || 2 == formState)
                    {
                        setControlFalse();
                    }
                    break;
                case 1: //1--审核员
                    //the formState is to be checked
                    if (1 == formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the formState do not have to be checked
                    else if (0 == formState || 2 == formState || 3 == formState)
                    {
                        setControlFalse();
                    }
                    break;
                case 2: //2--管理员
                    setControlTrue();
                    break;
                default:
                    break;
                    
            }
        }
        // 为了方便设置控件状态，完成如下两个函数：分别用于设置所有控件可用和所有控件不可用

        //this guarantee the controls are editable
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
            btn打印.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //read from database table and find current record
            string checkName = "待审核";
            DataTable dtCheck = new DataTable(checkName);
            OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", connOle);
            BindingSource bsCheck = new BindingSource();
            OleDbCommandBuilder cbCheck = new OleDbCommandBuilder(daCheck);
            daCheck.Fill(dtCheck);

            //if current hasn't been stored, insert a record in table
            if (0 == dtCheck.Rows.Count)
            {
                DataRow newrow = dtCheck.NewRow();
                newrow["表名"] = tablename1;
                //
                newrow["对应ID"] =searchId;
                dtCheck.Rows.Add(newrow);
            }
            bsCheck.DataSource = dtCheck;
            daCheck.Update((DataTable)bsCheck.DataSource);

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtBalance.Rows[0]["日志"] = dtBalance.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtBalance.Rows[0]["审核人"] = __待审核;
            //update log into table
            bsBalance.EndEdit();
            daBalance.Update((DataTable)bsBalance.DataSource);
            try
            {
                readBalance(searchId);
            }
            catch
            {
                readBalance(instructionStr);
            }
            removeBindBalance();
            BindBalance();
            searchId = Convert.ToInt32(dtBalance.Rows[0]["ID"]);
            formState = 1;
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }



        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();

                dtBalance.Rows[0]["审核人"] = check.userName;
                dtBalance.Rows[0]["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString());
                dtBalance.Rows[0]["审核意见"] = check.opinion.ToString();
                dtBalance.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
                bsBalance.EndEdit();
                daBalance.Update((DataTable)bsBalance.DataSource);
                readBalance(txb生产指令.Text);
                removeBindBalance();
                BindBalance();
                searchId = Convert.ToInt32(dtBalance.Rows[0]["ID"]);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtBalance.Rows[0]["日志"] = dtBalance.Rows[0]["日志"].ToString() + log;


                bsBalance.EndEdit();
                daBalance.Update((DataTable)bsBalance.DataSource);

                readBalance(searchId);

                removeBindBalance();
                BindBalance();

                //to delete the unchecked table
                //read from database table and find current record
                string checkName = "待审核";
                DataTable dtCheck = new DataTable(checkName);
                OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", connOle);
                BindingSource bsCheck = new BindingSource();
                OleDbCommandBuilder cbCheck = new OleDbCommandBuilder(daCheck);
                daCheck.Fill(dtCheck);

                //this part will never be run, for there must be a unchecked recird before this button becomes enable
                if (0 == dtCheck.Rows.Count)
                {
                    DataRow newrow = dtCheck.NewRow();
                    newrow["表名"] = tablename1;
                    newrow["对应ID"] = dtBalance.Rows[0]["ID"];
                    dtCheck.Rows.Add(newrow);
                }
                //remove the record
                dtCheck.Rows[0].Delete();
                bsCheck.DataSource = dtCheck;
                daCheck.Update((DataTable)bsCheck.DataSource);
                formState = 2;
                setEnableReadOnly();
            }
            else
            {
                //check unpassed
                base.CheckResult();
                txb审核人.Text = check.userName.ToString();
                dtBalance.Rows[0]["审核人"] = check.userName.ToString();
                dtBalance.Rows[0]["审核意见"] = check.opinion.ToString();
                dtBalance.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtBalance.Rows[0]["日志"] = dtBalance.Rows[0]["日志"].ToString() + log;


                bsBalance.EndEdit();
                daBalance.Update((DataTable)bsBalance.DataSource);

                readBalance(Convert.ToInt32( searchId));

                removeBindBalance();
                BindBalance();
                formState = 3;
                setEnableReadOnly();
            }

            /////////////

        }

        private void getStartTime()
        {
            string sqlStr = "SELECT 开始生产日期 FROM 生产指令信息表 WHERE ID = " + instructionInt.ToString();
            OleDbCommand sqlCmd = new OleDbCommand(sqlStr, connOle);
            生产日期 = Convert.ToDateTime(sqlCmd.ExecuteScalar());
            dtp生产日期.Value = 生产日期;
            //dtp生产日期.Enabled = false;
        }

         private DataRow writeBalance(DataRow dr)
         {            
             dr["生产指令ID"] = instructionInt;
             dr["生产指令"] = instructionStr;
             dr["生产日期"] = 生产日期;           
             dr["记录人"] = Parameter.userName;
             dr["记录日期"]=Convert.ToDateTime(DateTime.Now.ToString());
             dr["审核日期"] = Convert.ToDateTime(DateTime.MinValue);
             return dr;
         }

         private void readAgain()
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
             string Tsqlcmd = "SELECT " + Tcol + " FROM " + Tname + " WHERE " + where + " = " + instructionInt + ";";
             string Asqlcmd1 = "SELECT " + Acol1 + " FROM " + Aname + " WHERE " + where + " = " + instructionInt + ";";
             string Asqlcmd2 = "SELECT " + Acol2 + " FROM " + Aname + " WHERE " + where + " = " + instructionInt + ";";
             string Asqlcmd3 = "SELECT " + Acol3 + " FROM " + Aname + " WHERE " + where + " = " + instructionInt + ";";
             string Usqlcmd = "SELECT " + Ucol + " FROM " + Uname + " WHERE " + where + " = " + instructionInt + ";";
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
             dtBalance.Rows[0]["成品重量合计"] = t;
             dtBalance.Rows[0]["废品量合计"] = u;
             dtBalance.Rows[0]["领料量"] = a;
             dtBalance.Rows[0]["重量比成品率"] = rate;
             dtBalance.Rows[0]["物料平衡"] = balance;

         }


         private void readBalance(string instruc)
         {
             string tablename1 = "吹膜工序物料平衡记录";
             bsBalance = new BindingSource();
             dtBalance = new DataTable(tablename1);
             daBalance = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令 ='" + instruc + "';", connOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
             cbBalance = new OleDbCommandBuilder(daBalance);
             daBalance.Fill(dtBalance);
         }


         private void readBalance(int searchId)
         {
             string tablename1 = "吹膜工序物料平衡记录";
             bsBalance = new BindingSource();
             dtBalance = new DataTable(tablename1);
             daBalance = new OleDbDataAdapter("select * from " + tablename1 + " where ID =" +searchId + ";", connOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
             cbBalance = new OleDbCommandBuilder(daBalance);
             daBalance.Fill(dtBalance);
         }
         private void BindBalance()
         {
             bsBalance.DataSource = dtBalance;

             txb生产指令.DataBindings.Add("Text", bsBalance.DataSource, "生产指令");
             dtp生产日期.DataBindings.Add("Value", bsBalance.DataSource, "生产日期");
             txb成品重量合计.DataBindings.Add("Text", bsBalance.DataSource, "成品重量合计");
             txb废品量合计.DataBindings.Add("Text", bsBalance.DataSource, "废品量合计");
             txb领料量.DataBindings.Add("Text", bsBalance.DataSource, "领料量");
             txb重量比成品率.DataBindings.Add("Text", bsBalance.DataSource, "重量比成品率");
             txb物料平衡.DataBindings.Add("Text", bsBalance.DataSource, "物料平衡");
             // TODO: BINDING FAILED
             txb记录人.DataBindings.Add("Text", bsBalance.DataSource, "记录人");
             dtp记录日期.DataBindings.Add("Value", bsBalance.DataSource, "记录日期");
             txb审核人.DataBindings.Add("Text", bsBalance.DataSource, "审核人");
             dtp审核日期.DataBindings.Add("Value", bsBalance.DataSource, "审核日期");
             txb备注.DataBindings.Add("Text", bsBalance.DataSource, "备注");
         }

         private void removeBindBalance()
         {
             txb生产指令.DataBindings.Clear();
             dtp生产日期.DataBindings.Clear();
             txb成品重量合计.DataBindings.Clear();
             txb废品量合计.DataBindings.Clear();
             txb领料量.DataBindings.Clear();
             txb重量比成品率.DataBindings.Clear();
             txb物料平衡.DataBindings.Clear();
             txb记录人.DataBindings.Clear();
             dtp记录日期.DataBindings.Clear();
             txb审核人.DataBindings.Clear();
             dtp审核日期.DataBindings.Clear();
             txb备注.DataBindings.Clear();
         }

       

         private void btnSave_Click(object sender, EventArgs e)
         {
             // calculate();
             bsBalance.EndEdit();
             daBalance.Update((DataTable)bsBalance.DataSource);
             readBalance(txb生产指令.Text);
             removeBindBalance();
             BindBalance();
             searchId = Convert.ToInt32(dtBalance.Rows[0]["ID"]);
             btn提交审核.Enabled = true;
             btn打印.Enabled = true;
         }

         private void btn审核_Click(object sender, EventArgs e)
         {
             check = new CheckForm(this);
             check.Show();
         }

         private void btn打印_Click(object sender, EventArgs e)
         {
             print(true);
         }
		 public void print(bool preview)
         {
		 // 打开一个Excel进程
             Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
             // 利用这个进程打开一个Excel文件
             //System.IO.Directory.GetCurrentDirectory;
             Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\B\SOP-MFG-301-R12 吹膜工序物料平衡记录.xlsx");
             // 选择一个Sheet，注意Sheet的序号是从1开始的
             Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[3];
             // 设置该进程是否可见
             oXL.Visible = true;
             // 修改Sheet中某行某列的值

             my.Cells[3, 2].Value = txb生产指令.Text.ToString();
             my.Cells[3, 4].Value = dtp生产日期.Value.ToLongDateString();
             my.Cells[6, 1].Value = txb成品重量合计.Text;
             my.Cells[6, 2].Value = txb废品量合计.Text;
             my.Cells[6, 3].Value = txb领料量.Text;
             my.Cells[6, 4].Value = txb重量比成品率.Text;
             my.Cells[6, 5].Value = txb物料平衡.Text;
             my.Cells[7, 2].Value = txb备注.Text;
             my.Cells[8, 1].Value = "记录人/日期：" + txb记录人.Text+"   "+dtp记录日期.Value.ToLongDateString();
             my.Cells[8, 4].Value =  txb审核人.Text+dtp审核日期.Value.ToLongDateString();

			if(preview)
			{
             // 让这个Sheet为被选中状态
                 my.Select();  
				 oXL.Visible=true; //加上这一行  就相当于预览功能
             
			}
			else
			{
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

        
       
    }
}
