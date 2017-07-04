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

namespace mySystem.Process.Extruction.B
{
    public partial class WasteInExtrusion : mySystem.BaseForm
    {
        OleDbConnection connOle = Parameter.connOle;
        DateTime start;
        DateTimePicker dtp;
        private CheckForm check = null;
        string checker;
        string tablename = "waste_record_of_extrusion_process";
        public WasteInExtrusion(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            dtp=new DateTimePicker();
            txbInstruction.Text = Parameter.proInstruction;
            start = getStart();
            dtpDate1.Value = start;
            List<List<Object>> rlt = new List<List<object>>();
            List<List<Object>> rlt1 = new List<List<object>>();
            rlt = searchIns(txbInstruction.Text);
            rlt1 = modiList(rlt);
            Utility.fillDataGridView(dataGridView1, rlt1);
            lockdgv();
            total.Text = "废品重量合计" + getWaste().ToString() + "Kg";
        }

        public override void CheckResult()
        {
            base.CheckResult();
            
            bool get;
            List<List<Object>> data = new List<List<object>>();
            List<List<Object>> datafull = new List<List<object>>();
            data = Utility.readFromDataGridView(dataGridView1);

            int productionId = Parameter.proInstruID;
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "createtime", "modifytime", "start_time_production", "end_time_production", "production_date", "flight", "product_id", "waste_quantity", "cause_of_waste", "recorder_id", "reviewer_id", "review_opinion", "is_review_qualified" });
            //List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });



            for (int i = 0; i < data.Count - 1; i++)// do not need the first column
            {
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id", "production_date" });
                List<Object> whereVals = new List<Object>(new Object[] { productionId, Convert.ToDateTime(data[i][1].ToString()) });
                List<Type> types = new List<Type>(new Type[] { Type.GetType("System.Int32"), Type.GetType("System.DateTime"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.String"), Type.GetType("System.Int32") });
                rlt = Utility.selectAccess(connOle, tablename, selectCols, whereCols, whereVals, null, null, null, null, null);
                //List<Object> u = new List<object>();
               


                if (0 != rlt.Count)
                {
                    if (1 == rlt.Count)
                    {
                        if (Convert.ToBoolean(rlt[0][12]))  //reviewed item
                        {
                            //dataGridView1.Rows[i].ReadOnly = true;
                        }
                        else    //update date from datagrid view
                        {
                            List<Object> u = new List<object>();
                            
                            u.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                            u.Add(Convert.ToInt32(check.userID));
                            u.Add(Convert.ToString(check.opinion));
                            u.Add(Convert.ToBoolean(check.ischeckOk));
                           
                            List<String> updateCols = new List<String>(new String[] { "modifytime", "reviewer_id","review_opinion","is_review_qualified" });
                            get = Utility.updateAccess(connOle, tablename, updateCols, u, whereCols, whereVals);

                        }
                    }
                    continue;
                }
            }

        
        }
        //private
        private DateTime getStart()
        {
            DateTime rtn = new DateTime();
            string tblname = "production_instruction";
            string sqlcmd = "SELECT production_start_date FROM " + tblname + " WHERE production_instruction_id = " + Parameter.proInstruID.ToString();
            OleDbCommand cmd = new OleDbCommand(sqlcmd, connOle);
            rtn = Convert.ToDateTime( cmd.ExecuteScalar());
            return rtn;
        }

        private int getWaste()
        {
            int sum = 0;
            string tblname = "waste_record_of_extrusion_process";
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "waste_quantity" });
            rlt = Utility.selectAccess(connOle, tblname, selectCols, null, null, null, null, null, null, null);
            for (int i = 0; i < rlt.Count; i++)
            {
                sum += Convert.ToInt32(rlt[i][0]);
            }

            return sum;

        }

        private List<List<Object>> searchIns(string Instruction)
        {
            int productionId = Parameter.proInstruID;
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "createtime", "modifytime", "start_time_production", "end_time_production", "production_date","flight","product_id","waste_quantity","cause_of_waste","recorder_id","reviewer_id","review_opinion","is_review_qualified"});
            //List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });
            List<String> whereCols = new List<String>(new String[] { "production_instruction_id" });
            List<Object> whereVals = new List<Object>(new Object[] { productionId });
            rlt = Utility.selectAccess(connOle, tablename, selectCols, whereCols, whereVals, null, null, null, null, null);
            return rlt;
        }
        private List<List<Object>> modiList(List<List<Object>> putin)
        {
            List<List<Object>> rlt = new List<List<object>>();
            List<int> index=new List<int>(new int[]{4,5,6,7,8,9,10});
            for (int i = 0; i < putin.Count; i++)
            {
                List<Object> r=new List<object>();
                r.Add(i + 1);
                for (int j=0;j<index.Count;j++)
                {
                    r.Add(putin[i][index[j]]);
                }
                rlt.Add(r);
            }
            return rlt;
        }

        private void lockdgv()
        {
            bool get;
            List<List<Object>> data=new List<List<object>>();
            List<List<Object>> datafull=new List<List<object>>();
            data=Utility.readFromDataGridView(dataGridView1);

            int productionId = Parameter.proInstruID;
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "createtime", "modifytime", "start_time_production", "end_time_production", "production_date","flight","product_id","waste_quantity","cause_of_waste","recorder_id","reviewer_id","review_opinion","is_review_qualified"});
            //List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });



            for (int i = 0; i < data.Count - 1; i++)
            {
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id", "production_date" });
                List<Object> whereVals = new List<Object>(new Object[] { productionId, Convert.ToDateTime(data[i][1].ToString()) });
                List<Type> types = new List<Type>(new Type[] { Type.GetType("System.Int32"), Type.GetType("System.DateTime"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.String"), Type.GetType("System.Int32") });
                rlt = Utility.selectAccess(connOle, tablename, selectCols, whereCols, whereVals, null, null, null, null, null);
                //List<Object> u = new List<object>();
                if (0 != rlt.Count)
                {
                    if (1 == rlt.Count)
                    {
                        if (Convert.ToBoolean(rlt[0][12]))  //reviewed item
                        {
                            dataGridView1.Rows[i].ReadOnly = true;
                        }
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool get;
            List<List<Object>> data=new List<List<object>>();
            List<List<Object>> datafull=new List<List<object>>();
            data=Utility.readFromDataGridView(dataGridView1);

            int productionId = Parameter.proInstruID;
            List<List<Object>> rlt = new List<List<object>>();
            List<String> selectCols = new List<String>(new String[] { "createtime", "modifytime", "start_time_production", "end_time_production", "production_date","flight","product_id","waste_quantity","cause_of_waste","recorder_id","reviewer_id","review_opinion","is_review_qualified"});
            //List<Object> insertVals = new List<Object>(new Object[] { i, dtpDate.Value.ToShortDateString(), DateTime.Now.TimeOfDay, DateTime.Now.TimeOfDay, Convert.ToInt32(txbInstructionId.Text), jarray.ToString(), Convert.ToInt32(txbCodeD.Text), Convert.ToInt32(txbCodeN.Text), txbBatchNoD.Text, txbBatchNoN.Text, Convert.ToInt32(txbAmountsD.Text), Convert.ToInt32(txbAmountsN.Text), txbAbnormalD.Text, txbHandinD.Text, txbTakeinD.Text, dtpDay.Value.TimeOfDay, txbAbnormalN.Text, txbHandinN.Text, txbTakeinN.Text, dtpNight.Value.TimeOfDay });
            
            

            for(int i=0;i<data.Count-1;i++)// do not need the first column
            {
                List<String> whereCols = new List<String>(new String[] { "production_instruction_id","production_date" });
                List<Object> whereVals = new List<Object>(new Object[] { productionId,Convert.ToDateTime(data[i][1].ToString()) });
                List<Type> types = new List<Type>(new Type[] { Type.GetType("System.Int32"), Type.GetType("System.DateTime"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.Int32"), Type.GetType("System.String"), Type.GetType("System.Int32") });
                rlt = Utility.selectAccess(connOle, tablename, selectCols, whereCols, whereVals, null, null, null, null, null);
                //List<Object> u = new List<object>();
                if (0 != rlt.Count)
                {
                    if (1 == rlt.Count)
                    {
                        if (Convert.ToBoolean(rlt[0][12]))  //reviewed item
                        {
                            dataGridView1.Rows[i].ReadOnly = true;
                        }
                        else    //update date from datagrid view
                        {
                            List<Object> u = new List<object>();
                            u.Add(Parameter.proInstruID);
                            u.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                            u.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                            u.Add(start);
                            u.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                            for (int j = 1; j < data[i].Count - 1; j++)
                            {
                                u.Add(Convert.ChangeType(data[i][j], types[j]));
                            }
                            List<String> updateCols = new List<String>(new String[] { "production_instruction_id", "createtime", "modifytime", "start_time_production", "end_time_production", "production_date", "flight", "product_id", "waste_quantity", "cause_of_waste", "recorder_id" });
                            get = Utility.updateAccess(connOle, tablename, updateCols, u,whereCols,whereVals);
           
                         }
                    }
                    continue; 
                }

                List<Object> r=new List<object>();
                r.Add(Parameter.proInstruID);
                r.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                r.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                r.Add(start);
                r.Add(Convert.ToDateTime(DateTime.Now.ToString()));
                for (int j = 1; j < data[i].Count-1; j++)
                {
                    r.Add(Convert.ChangeType( data[i][j],types[j]));
                }
                datafull.Add(r);
                List<String> insertCols = new List<String>(new String[] { "production_instruction_id", "createtime", "modifytime", "start_time_production", "end_time_production", "production_date", "flight", "product_id", "waste_quantity", "cause_of_waste", "recorder_id" });
                get = Utility.insertAccess(connOle, tablename, insertCols, r);
           
            }

            //from now we get the daafull and insert into the database
            //List<String> insertCols = new List<String>(new String[] {"production_instruction_id", "createtime", "modifytime", "start_time_production", "end_time_production", "production_date","flight","product_id","waste_quantity","cause_of_waste","recorder_id","reviewer_id","review_opinion","is_review_qualified"});
            //List<Object> insertVals = new List<Object>(new Object[] { Parameter.proInstruID, Convert.ToDateTime(DateTime.Now.ToString()), Convert.ToDateTime(DateTime.Now.ToString()), start, Convert.ToDateTime(DateTime.Now.ToString()), });
            //Boolean get = Utility.insertAccess(connOle, tablename, insertCols, insertVals);
           


        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
       

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dtp.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "生产日期")
                {
                    showdtp(e);
                }
            }
            dataGridView1.CellClick+=new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }
        private void showdtp(DataGridViewCellEventArgs e)
        {
            //dtp1.Visible = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;


            dtp.Size = dataGridView1.CurrentCell.Size;
            dtp.Top = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top;
            dtp.Left = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left;
            dtp.Visible = true;
            dtp.BringToFront();
            dataGridView1.Controls.Add(dtp);
           

            dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
        }
        private void dtp_ValueChanged(object sender, EventArgs e)
        {
           
                dataGridView1.CurrentCell.Value = dtp.Value.Date;
                dataGridView1.AllowUserToResizeColumns = true;
                dataGridView1.AllowUserToResizeRows = true;
                dataGridView1.Enabled = true;
                dtp.Visible = false;

        }
    }
}
