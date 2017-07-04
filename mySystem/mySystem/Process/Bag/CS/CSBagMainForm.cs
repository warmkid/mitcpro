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
using mySystem.Process.CleanCut;

namespace mySystem.Process.Bag
{
    public partial class CSBagMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public CSBagMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select instruction_code from production_instruction_bag";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["instruction_code"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select production_instruction_code from production_instruction";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["production_instruction_code"]);
                    }
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.csbagInstruction = instruction;
            String tblName = "production_instruction_bag";
            List<String> queryCols = new List<String>(new String[] { "instruction_id" });
            List<String> whereCols = new List<String>(new String[] { "instruction_code" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.csbagInstruID = instruID;

        }


        //定义各窗体变量
        MaterialRecord form1 = null;
        CSBag_InnerPackaging form2 = null;
        Chart_daily_cs form3 = null;

        Bagprocess_prod_instru form5 = null;
        CSBag_CheckBeforePower form6 = null;
        RunningRecord form7 = null;

        Record_batch_bag form9 = null;

        private void A1Btn_Click(object sender, EventArgs e)
        {
            form1 = new MaterialRecord();            
            form1.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            form2 = new CSBag_InnerPackaging(mainform);            
            form2.ShowDialog();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            form3 = new Chart_daily_cs();           
            form3.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            form5 = new Bagprocess_prod_instru();           
            form5.ShowDialog();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            form6 = new CSBag_CheckBeforePower(mainform);            
            form6.ShowDialog();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            form7 = new RunningRecord();            
            form7.ShowDialog();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {

        }

        private void A4Btn_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            form9 = new Record_batch_bag(mainform);           
            form9.ShowDialog();
        }

    }
}
