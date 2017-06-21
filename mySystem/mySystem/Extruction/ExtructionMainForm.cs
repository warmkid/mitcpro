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








    }
}
