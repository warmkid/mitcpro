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
        OleDbConnection connOle = Parameter.connOle;
        private CheckForm check = null;
        public MaterialBalenceofExtrusionProcess(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            fill();
            
        }

        public override void CheckResult()
        {
            base.CheckResult();
            txbRecheckMan.Text = check.userID.ToString();
            
        }

        private void fill()
        {
            float t,u,a,rate,balance;
            txbProductInstruction.Text = Parameter.proInstruction.ToString();
            txbDate.Text = getStart().ToString();
            txbWasteWeight.Text = getWaste().ToString();
            txbIn.Text = "15";
            txbGoodWeight.Text = "15";
            t=Convert.ToSingle(txbGoodWeight.Text);
            u=Convert.ToSingle(txbWasteWeight.Text);
            a=Convert.ToSingle(txbIn.Text);
            rate=t/(t+u)*100;
            balance=(t+u)/a*100;
            txbRate.Text = Convert.ToSingle(rate).ToString("0.00");
            txbBalence.Text = Convert.ToSingle(balance).ToString("0.00");
            txbRecordMan.Text = Parameter.userName.ToString();
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
         private int getWaste()
        {
            int sum=0;
            string tblname = "waste_record_of_extrusion_process";
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "waste_quantity"});
            rlt = Utility.selectAccess(connOle, tblname, selectCols, null, null, null, null, null, null, null);
            for (int i = 0; i < rlt.Count; i++)
            {
                sum += Convert.ToInt32(rlt[i][0]);
            }

            return sum;
           
        }

         private void btnReview_Click(object sender, EventArgs e)
         {
             check = new CheckForm(this);
             check.Show();
         }
       
    }
}
