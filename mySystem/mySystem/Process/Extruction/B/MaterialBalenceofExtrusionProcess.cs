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
        OleDbConnection connOle = Parameter.connOle;
        private CheckForm check = null;
        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            txb生产指令.Text = Parameter.proInstruction;
            //txb生产指令.Enabled = false;
            getStartTime();
            readBalance(txb生产指令.Text);
            removeBindBalance();
            BindBalance();
            if (0 == dtBalance.Rows.Count)
            {
                DataRow newrow = dtBalance.NewRow();
                newrow = writeBalance(newrow);
                dtBalance.Rows.Add(newrow);
                daBalance.Update((DataTable)bsBalance.DataSource);
                readBalance(txb生产指令.Text);
                removeBindBalance();
                BindBalance();
            }
            readAgain();
            removeBindBalance();
            BindBalance();
            this.dtp生产日期.Enabled = false;
            btn审核.Enabled = false;
            if (dtBalance.Rows[0]["审核人"].ToString().Trim() != "")
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                }
            }
        }

        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform,int Id)
            : base(mainform)
        {
            InitializeComponent();
            dtBalance = new DataTable("吹膜工序物料平衡记录");
            daBalance = new OleDbDataAdapter("SELECT * FROM 吹膜工序物料平衡记录 WHERE ID =" + Id, connOle);
            bsBalance = new BindingSource();
            cbBalance = new OleDbCommandBuilder(daBalance);
            daBalance.Fill(dtBalance);
            removeBindBalance();
            BindBalance();
            
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            btnSave.Visible = false;
            btn审核.Visible = false;
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
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                }
            }
        }

        private void getStartTime()
        {
            string sqlStr = "SELECT 开始生产日期 FROM 生产指令信息表 WHERE ID = " + Parameter.proInstruID.ToString();
            OleDbCommand sqlCmd = new OleDbCommand(sqlStr, connOle);
            dtp生产日期.Value = Convert.ToDateTime(sqlCmd.ExecuteScalar());
            //dtp生产日期.Enabled = false;
        }

         private DataRow writeBalance(DataRow dr)
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
             string Tsqlcmd = "SELECT " + Tcol + " FROM " + Tname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd1 = "SELECT " + Acol1 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd2 = "SELECT " + Acol2 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd3 = "SELECT " + Acol3 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Usqlcmd = "SELECT " + Ucol + " FROM " + Uname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
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


             dr["生产指令ID"] = Parameter.proInstruID;
             dr["生产指令"] = Parameter.proInstruction;
             dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString());
             dr["成品重量合计"] = t;
             dr["废品量合计"] = u;
             dr["领料量"] = a;
             dr["重量比成品率"] = rate;
             dr["物料平衡"] = balance;
             dr["记录人"] = Parameter.userName;
             dr["记录日期"]=Convert.ToDateTime(DateTime.Now.ToString());
             dr["审核日期"] = Convert.ToDateTime(DateTime.MinValue.ToString());
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
             string Tsqlcmd = "SELECT " + Tcol + " FROM " + Tname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd1 = "SELECT " + Acol1 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd2 = "SELECT " + Acol2 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Asqlcmd3 = "SELECT " + Acol3 + " FROM " + Aname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
             string Usqlcmd = "SELECT " + Ucol + " FROM " + Uname + " WHERE " + where + " = " + Parameter.proInstruID + ";";
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

             dtBalance.Rows[0]["生产指令ID"] = Parameter.proInstruID;
             dtBalance.Rows[0]["生产指令"] = Parameter.proInstruction;
             dtBalance.Rows[0]["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString());
             dtBalance.Rows[0]["成品重量合计"] = t;
             dtBalance.Rows[0]["废品量合计"] = u;
             dtBalance.Rows[0]["领料量"] = a;
             dtBalance.Rows[0]["重量比成品率"] = rate;
             dtBalance.Rows[0]["物料平衡"] = balance;
             dtBalance.Rows[0]["记录人"] = Parameter.userName;
             dtBalance.Rows[0]["记录日期"] = Convert.ToDateTime(DateTime.Now.ToString());
             //dtBalance.Rows[0]["审核日期"] = Convert.ToDateTime(DateTime.MinValue.ToString());            
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
             btn审核.Enabled = true;
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
