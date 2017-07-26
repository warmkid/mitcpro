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
using mySystem.Process.Bag.CS;

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
                comm.CommandText = "select * from 生产指令 where 状态 = 2";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select * from 生产指令 where 状态 = 2";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["生产指令编号"]);
                    }
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.csbagInstruction = instruction;
            String tblName = "生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
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
            //form5 = new Bagprocess_prod_instru();           
            //form5.ShowDialog();
            CS.CS制袋生产指令 form = new CS.CS制袋生产指令();
            form.ShowDialog();
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
            CS.清场记录 myform = new CS.清场记录();
            myform.ShowDialog();
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            form9 = new Record_batch_bag(mainform);           
            form9.ShowDialog();
        }

        private void Btn外观及检验_Click(object sender, EventArgs e)
        {
            CS.产品外观和尺寸检验记录 myform = new 产品外观和尺寸检验记录();
            myform.ShowDialog();
        }

    }
}
