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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCutMainForm : BaseForm
    {
        public CleanCutMainForm(MainForm mainform):base(mainform)
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

        private void A1Btn_Click(object sender, EventArgs e)
        {
            Instru myDlg = new Instru();
            myDlg.Show();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            Record_cleansite_cut myDlg = new Record_cleansite_cut(base.mainform);
            myDlg.Show();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            CleanCut_CheckBeforePower myDlg = new CleanCut_CheckBeforePower();
            myDlg.Show();
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            CleanCut_Productrecord myDlg = new CleanCut_Productrecord();
            myDlg.Show();
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            DailyRecord myDlg = new DailyRecord();
            myDlg.Show();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.mainform.cleancutInstruction = comboBox1.SelectedItem.ToString();
        }




    }
}
