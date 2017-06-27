using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


//this form is about the 8th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class RunningRecordofExtrusionUnit : Form
    {
        SqlConnection conn = null;
        public RunningRecordofExtrusionUnit(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("data has benn collecteed and more operarion is under developmen");
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            //LoginForm check = new LoginForm(conn);
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            //check.ShowDialog();
        }
        
       
    }
}
