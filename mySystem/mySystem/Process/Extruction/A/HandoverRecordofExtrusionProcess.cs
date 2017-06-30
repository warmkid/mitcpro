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

//this form is about the 13th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    
    public partial class HandoverRecordofExtrusionProcess : mySystem.BaseForm
    {
        string checker;
        private CheckForm check = null;
        private OleDbConnection connOle = null;
        public HandoverRecordofExtrusionProcess(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            connOle = Parameter.connOle;
            searchTime(DateTime.Now);

             
        }

        public override void CheckResult()
        {
            base.CheckResult();
            checker= check.opinion;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            //the control names queren
            //this.dtpDate.Value = DateTime.Now;
            mySystem.Setting.SettingHandOver test = new Setting.SettingHandOver(mainform);
            test.Show();
            searchTime(Convert.ToDateTime(dtpDate.Value.Date));

        }
        private void searchTime(DateTime dt)
        {
            string dateStr = dt.ToShortDateString();
            string tablename = "handover_record_of_extrusion_process";
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "id", "production_date", "createtime", "modifytime", "production_instruction_id", "confirm_item", "product_id_day", "product_id_night", "product_batch_day", "product_batch_night", "product_quantity_day", "product_quantity_night", "exception_handling_day", "to_attend_day", "successor_day", "successor_time_day", "exception_handling_night", "to_attend_night", "successor_night", "successor_time_night", });
            //List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });
            List<String> whereCols = new List<String>(new String[] {  "production_date"});
            List<Object> whereVals = new List<Object>(new Object[] { dt.ToShortDateString()});
            rlt = Utility.selectAccess(connOle, tablename, selectCols, whereCols, whereVals, null, null, null, null, null);

            if (rlt.Count != 0)
            {
                List<Control> cons = new List<Control> { txbInstructionId, txbBatchNoD, txbBatchNoN, txbAmountsD, txbAmountsN, txbCodeD, txbCodeN, txbAbnormalD, txbAbnormalN, txbHandinD, txbHandinN, txbTakeinD, txbTakeinN, dtpDay, dtpNight };
                List<String> strs = new List<string> { rlt[0][4].ToString(), rlt[0][8].ToString(), rlt[0][9].ToString(), rlt[0][10].ToString(), rlt[0][11].ToString(), rlt[0][6].ToString(), rlt[0][7].ToString(), rlt[0][12].ToString(), rlt[0][16].ToString(), rlt[0][13].ToString(), rlt[0][17].ToString(), rlt[0][14].ToString(), rlt[0][18].ToString(), rlt[0][15].ToString(), rlt[0][19].ToString() };
                Utility.fillControl(cons, strs);
                jason(rlt[0][5].ToString());
                btnDefault.Enabled = false;
                button1.Enabled = true;
                btnAdd.Enabled = false;
            }
            else
            {
                getItem();
                button1.Enabled = false;
                btnAdd.Enabled = true;
                btnDefault.Enabled = true;
            }
        }

        private void jy()
        {
            JArray jarray = new JArray();
            List<String> cols = new List<string>(new string[] { "Column1", "Column2", "Column3" });
            List<Type> types = new List<Type>(new Type[] { Type.GetType("System.String"),Type.GetType("bool"),Type.GetType("bool") });
        }

        private void getItem()
        {
            List<List<Object>> ret = new List<List<Object>>();
            //string tableStr = "set_handover";
            string cmdStr = "SELECT 确认项目 FROM handoveritem;";
            //ret = Utility.selectAccess(mainform.connOle, tableStr, null, null, null, null, null, null, null, null);
            OleDbCommand sqlcmd = new OleDbCommand(cmdStr, connOle);
            //sqlcmd.ExecuteNonQuery();
            
            OleDbDataReader reader = null;
            reader = sqlcmd.ExecuteReader();
            sqlcmd.Dispose();
            while (reader.Read())
            {
                List<Object> row = new List<Object>(new Object[] { reader["确认项目"], true, true });
                //row.Add(row);
               
                ret.Add(row);
            }
            Utility.fillDataGridView(dataGridView1, ret);

        }

        private void jason(String str)
        {

            List<String> cols = new List<String>(new String[] { "Column1", "Column2", "Column3" });
            JArray jarray = JArray.Parse(str);
            List<Type> types = new List<Type>(new Type[] { Type.GetType("System.String"), Type.GetType("System.Boolean"), Type.GetType("System.Boolean") });
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
            Utility.writeJSONToDataGridView(jarray, dataGridView1, cols, types);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //this control names save

            int i = 1;
            List<String> cols = new List<String>(new String[] { "Column1", "Column2", "Column3" });
            JArray jarray = Utility.readJSONFromDataGridView(dataGridView1, cols);
            string tablename = "handover_record_of_extrusion_process";
            List<String> insertCols = new List<String>(new String[] { "id", "production_date" , "createtime", "modifytime", "production_instruction_id", "confirm_item", "product_id_day", "product_id_night", "product_batch_day", "product_batch_night", "product_quantity_day", "product_quantity_night", "exception_handling_day", "to_attend_day", "successor_day", "successor_time_day","exception_handling_night", "to_attend_night","successor_night","successor_time_night",});
            List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });
            Boolean get = Utility.insertAccess(connOle, tablename, insertCols, insertVals);
            MessageBox.Show(Convert.ToString(get));
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            //this control names shen he
            //getItem();
            //LoginForm check = new LoginForm(conn);
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            //check.ShowDialog();
            check = new CheckForm(this);
            check.Show();
            
            //public static Boolean updateAccess(OleDbConnection conn, String tblName, List<String> updateCols, List<Object> updateVals,List<String> whereCols, List<Object> whereVals)

            

            
        }
        private void ud()
        {
            List<String> updateCols = new List<string> { "reviewer_id", "review_opinion", "is_review_qualified" };
            List<Object> updateVals = new List<object> { Convert.ToInt32(check.userID), Convert.ToString(check.opinion), Convert.ToBoolean(check.ischeckOk) };
            List<String> whereCols = new List<string> { "production_date" };
            List<Object> whereVals = new List<object> { dtpDate.Value.ToShortDateString() };
            string tablename = "handover_record_of_extrusion_process";
            bool falg = Utility.updateAccess(connOle, tablename, updateCols, updateVals, whereCols, whereVals);
            MessageBox.Show(falg.ToString());
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            clearCrol();
            searchTime(dtpDate.Value);
        }
        private void clearCrol()
        {
            List<Control> cons = new List<Control> { txbInstructionId, txbBatchNoD, txbBatchNoN, txbAmountsD, txbAmountsN, txbCodeD, txbCodeN, txbAbnormalD, txbAbnormalN, txbHandinD, txbHandinN, txbTakeinD, txbTakeinN, dtpDay, dtpNight };
            List<String> strs = new List<string> { "","","","","","","","","","","","","","","" };
            Utility.fillControl(cons, strs);
            for (int i=dataGridView1.Rows.Count-1;i>=0;i--)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
            
           
        }
        

        

    }
}

