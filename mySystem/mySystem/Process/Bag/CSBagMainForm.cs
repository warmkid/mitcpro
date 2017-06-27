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
        
        public CSBagMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!base.mainform.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = base.mainform.connOle;
                comm.CommandText = "select production_instruction_code from production_instruction";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["production_instruction_code"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = base.mainform.conn;
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
            base.mainform.csbagInstruction = comboBox1.SelectedItem.ToString();
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            MaterialRecord myDlg = new MaterialRecord();
            myDlg.Show();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            CSBag_InnerPackaging myDlg = new CSBag_InnerPackaging();
            myDlg.Show();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            Chart_daily_cs myDlg = new Chart_daily_cs();
            myDlg.Show();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            Bagprocess_prod_instru myDlg = new Bagprocess_prod_instru();
            myDlg.Show();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            CSBag_CheckBeforePower myDlg = new CSBag_CheckBeforePower();
            myDlg.Show();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            RunningRecord myDlg = new RunningRecord();
            myDlg.Show();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {

        }

        private void A4Btn_Click(object sender, EventArgs e)
        {

        }

    }
}
