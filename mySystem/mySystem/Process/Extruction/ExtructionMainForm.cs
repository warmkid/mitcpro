using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using System.Data.OleDb;
using WindowsFormsApplication1;


namespace mySystem
{
    public partial class ExtructionMainForm : BaseForm
    {
        SqlConnection conn = null;
        OleDbConnection connOle = null;
        bool isSqlOk;
        MainForm mform = null;
        
        public ExtructionMainForm(MainForm mainform):base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;
            mform = mainform;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = connOle;
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
                comm.Connection = conn;
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


        private void A3Btn_Click(object sender, EventArgs e)
        {
            Record_extrusClean extrusclean = new Record_extrusClean(mform);
            extrusclean.Show();
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            Record_extrusSiteClean ext = new Record_extrusSiteClean(mform);
            ext.Show();

        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            BatchProductRecord.BatchProductRecord b = new BatchProductRecord.BatchProductRecord(mform);
            b.Show();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            BatchProductRecord.ProcessProductInstru ppi = new BatchProductRecord.ProcessProductInstru(mform);
            ppi.Show();
        }
        private void C1Btn_Click(object sender, EventArgs e)
        {
            ExtructionCheckBeforePowerStep2 stepform = new ExtructionCheckBeforePowerStep2(mform);
            stepform.Show();
        }

        private void C2Btn_Click(object sender, EventArgs e)
        {
            ExtructionPreheatParameterRecordStep3 stepform = new ExtructionPreheatParameterRecordStep3(mform);
            stepform.Show();
        }

        
        private void B1Btn_Click(object sender, EventArgs e)
        {
            ExtructionTransportRecordStep4 stepform = new ExtructionTransportRecordStep4(mform);
            stepform.Show();
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            ExtructionpRoductionAndRestRecordStep6 stepform = new ExtructionpRoductionAndRestRecordStep6(mform);
            stepform.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.mainform.proInstruction = comboBox1.SelectedItem.ToString();
        }

        private void C3Btn_Click(object sender, EventArgs e)
        {
            
            mySystem.Extruction.Chart.feedrecord feedrecord = new Extruction.Chart.feedrecord(mform);
            feedrecord.Show();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            Record_extrusSupply recordsupply = new Record_extrusSupply(mform);
            recordsupply.Show();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            Record_material_reqanddisg mat = new Record_material_reqanddisg(mform);
            mat.Show();
        }

        private void B5Btn_Click(object sender, EventArgs e)
        {
            ProdctDaily_extrus pro = new ProdctDaily_extrus(mform);
            pro.Show();
        }

        private void B2Btn_Click_1(object sender, EventArgs e)
        {
            Record_extrusSupply r = new Record_extrusSupply(mform);
            r.Show();
        }

        private void B4Btn_Click_1(object sender, EventArgs e)
        {
            Record_material_reqanddisg r = new Record_material_reqanddisg(mform);
            r.Show();
        }

        private void B5Btn_Click_1(object sender, EventArgs e)
        {
            ProdctDaily_extrus p = new ProdctDaily_extrus(mform);
            p.Show();
        }
		        private void B3Btn_Click(object sender, EventArgs e)
        {
            mySystem.Extruction.Chart.wasterecord wasterecord = new Extruction.Chart.wasterecord(mform);
            wasterecord.Show();
        }

                private void B2Btn_Click_2(object sender, EventArgs e)
                {
                    Record_extrusSupply r = new Record_extrusSupply(mform);
                    r.Show();
                }

                private void B4Btn_Click_2(object sender, EventArgs e)
                {
                    Record_material_reqanddisg r = new Record_material_reqanddisg(mform);
                    r.Show();

                }

                private void B5Btn_Click_2(object sender, EventArgs e)
                {
                    ProdctDaily_extrus p = new ProdctDaily_extrus(mform);
                    p.Show();
                }

                private void B8Btn_Click(object sender, EventArgs e)
                {
                    ProductInnerPackagingRecord PIPRform = new ProductInnerPackagingRecord(mform);
                    PIPRform.Show();
                }


                private void D1Btn_Click(object sender, EventArgs e)
                {
                    Record_train r = new Record_train(mform);
                    r.Show();
                }
				                private void C4Btn_Click(object sender, EventArgs e)
                {
                    mySystem.Extruction.Chart.beeholetable beeholetable = new Extruction.Chart.beeholetable(mform);
                    beeholetable.Show();
                }

                private void B7Btn_Click(object sender, EventArgs e)
                {
                    MaterialBalenceofExtrusionProcess test = new MaterialBalenceofExtrusionProcess(mform);
                    test.Show();
                }

                private void A4Btn_Click(object sender, EventArgs e)
                {
                    HandoverRecordofExtrusionProcess test = new HandoverRecordofExtrusionProcess(mform);
                    test.Show();
                }

                private void B9Btn_Click(object sender, EventArgs e)
                {
                    mySystem.Extruction.Chart.outerpack test = new Extruction.Chart.outerpack(mform);
                    test.Show();
                }
				 private void D3Btn_Click(object sender, EventArgs e)
                {
                    ExtructionReplaceCore ERCform = new ExtructionReplaceCore(mform);
                    ERCform.Show();
                }

                private void D2Btn_Click(object sender, EventArgs e)
                {
                    ReplaceHeadForm myDlg = new ReplaceHeadForm();
                    myDlg.Show();
                }

                private void D1Btn_Click_1(object sender, EventArgs e)
                {
                    Record_train r = new Record_train(mform);
                    r.Show();
                }

                private void D1Btn_Click_2(object sender, EventArgs e)
                {
                    Record_train r = new Record_train(mform);
                    r.Show();
                }

                private void D4Btn_Click(object sender, EventArgs e)
                {

                }
    }
}



