using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class CheckForm : BaseForm
    {
        bool isSqlOk;
        public int userID;
        public string userName;
        public string opinion;
        public bool ischeckOk; //审核是否通过
        public DateTime time;

        BaseForm bs;
        public CheckForm(BaseForm mainform)
        {
            bs = mainform;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {          
            opinion = OpTextBox.Text;
            ischeckOk = true;
            time = DateTime.Now;
            bs.CheckResult();
            this.Dispose();            
        }

        private void NotOKBtn_Click(object sender, EventArgs e)
        {           
            opinion = OpTextBox.Text;
            ischeckOk = false;
            time = DateTime.Now;
            bs.CheckResult();
            this.Dispose();
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
       
        private void OpTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OKBtn.Focus();
            }
        }

    }
}
