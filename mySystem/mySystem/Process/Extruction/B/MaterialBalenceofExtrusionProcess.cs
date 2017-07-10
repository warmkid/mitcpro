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

//this form is about the 12th picture of the extrusion step 
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
            binding();
            calculate();
        }

        public override void CheckResult()
        {
            base.CheckResult();
            txb审核人.Text = check.userID.ToString();
            dtBalance.Rows[0]["审核人"] = check.userID.ToString();
            dtp审核日期.Value = DateTime.Now;
            dtBalance.Rows[0]["审核日期"] = dtp审核日期.Value;
            dtBalance.Rows[0]["审核意见"] = check.opinion.ToString();
            dtBalance.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
        }

        private void binding()
        {
            string tablename1 = "吹膜工序物料平衡记录";
            bsBalance = new BindingSource();
            dtBalance = new DataTable(tablename1);
            daBalance = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令 ='"+txb生产指令.Text+"';", connOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
            cbBalance = new OleDbCommandBuilder(daBalance);
            daBalance.Fill(dtBalance);
            if (dtBalance.Rows.Count < 1)
            {
                DataRow newrow = dtBalance.NewRow();
                dtBalance.Rows.Add(newrow);
            }
            bsBalance.DataSource = dtBalance;
             
            txb生产指令.DataBindings.Add("Text", bsBalance.DataSource, "生产指令");
            dtp生产日期.DataBindings.Add("Value", bsBalance.DataSource, "生产日期");
            txb成品重量合计.DataBindings.Add("Text", bsBalance.DataSource, "成品重量合计");
            txb废品量合计.DataBindings.Add("Text", bsBalance.DataSource, "废品量合计");
            txb领料量.DataBindings.Add("Text", bsBalance.DataSource, "领料量");
            txb重量比成品率.DataBindings.Add("Text", bsBalance.DataSource, "重量比成品率");
            txb物料平衡.DataBindings.Add("Text", bsBalance.DataSource, "物料平衡");
            txb记录人.DataBindings.Add("Text", bsBalance.DataSource, "记录人");
            dtp记录日期.DataBindings.Add("Value", bsBalance.DataSource, "记录日期");
            txb审核人.DataBindings.Add("Text", bsBalance.DataSource, "审核人");
            dtp审核日期.DataBindings.Add("Value", bsBalance.DataSource, "审核日期");
            txb备注.DataBindings.Add("Text", bsBalance.DataSource, "备注");
            getDate();
        }

        private void calculate()
        {
            float t,u,a,rate,balance;
            //txb生产指令.Text = Parameter.proInstruction.ToString();
            //txb生产日期.Text = getStart().ToString();
            //txb废品量合计.Text = "15";
            //txb领料量.Text = "15";
            //txb成品重量合计.Text = "15";
            t=Convert.ToSingle(txb成品重量合计.Text);
            u=Convert.ToSingle(txb废品量合计.Text);
            a=Convert.ToSingle(txb领料量.Text);
            rate=t/(t+u)*100;
            balance=(t+u)/a*100;
            txb重量比成品率.Text = Convert.ToSingle(rate).ToString("0.00");
            txb物料平衡.Text = Convert.ToSingle(balance).ToString("0.00");
            txb记录人.Text = Parameter.userName.ToString();


            dtBalance.Rows[0]["生产指令"] = Convert.ToString(txb生产指令.Text);
            dtBalance.Rows[0]["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dtBalance.Rows[0]["成品重量合计"] = Convert.ToDouble(t);
            dtBalance.Rows[0]["废品量合计"] = Convert.ToDouble(u);
            dtBalance.Rows[0]["领料量"] = Convert.ToDouble(a);
            dtBalance.Rows[0]["重量比成品率"] = Convert.ToDouble(rate);
            dtBalance.Rows[0]["物料平衡"] = Convert.ToDouble(balance);
            dtBalance.Rows[0]["记录人"] = txb记录人.Text;
            dtBalance.Rows[0]["记录日期"] = Convert.ToDateTime(dtp记录日期.Value.ToShortDateString());
        }

        private DateTime getStart()
        {
            DateTime rtn = new DateTime();
            string tblname = "production_instruction";
            string sqlcmd = "SELECT production_start_date FROM " + tblname + " WHERE production_instruction_id = " + Parameter.proInstruID.ToString();
            OleDbCommand cmd = new OleDbCommand(sqlcmd, connOle);
            rtn = Convert.ToDateTime(cmd.ExecuteScalar());
            return rtn;
        }
         private void getDate()
        {
            int sum=0;
            string Tname = "吹膜工序生产和检验记录";
            string Tcol = "累计同规格膜卷重量T";
            
            string Aname = "吹膜供料记录";
             string Acol1="外层供料量合计a";
             string Acol2="中内层供料量合计b";
             string Acol3="中层供料量合计c";
             string where="生产指令ID";

             string Uname = "吹膜工序废品记录";
             string Ucol="合计不良品数量";
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
             double u = Convert.ToDouble(Ucmd.ExecuteScalar());

             txb成品重量合计.Text = t.ToString();
             txb废品量合计.Text = u.ToString();
             txb领料量.Text = (a1 + a2 + a3).ToString();
             

            //return sum;
           
        }

         private void btnReview_Click(object sender, EventArgs e)
         {
             
             check = new CheckForm(this);
             check.Show();
         }

         private void btnSave_Click(object sender, EventArgs e)
         {
             calculate();
             bsBalance.EndEdit();
             daBalance.Update((DataTable)bsBalance.DataSource);
         }

         
       
    }
}
