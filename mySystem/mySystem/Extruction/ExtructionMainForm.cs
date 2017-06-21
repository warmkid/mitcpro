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
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {

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








    }
}
