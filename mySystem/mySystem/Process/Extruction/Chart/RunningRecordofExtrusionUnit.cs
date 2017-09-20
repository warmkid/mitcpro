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

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜机组运行记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);
            String str操作员 = dt.Rows[0]["操作员"].ToString();
            String str审核员 = dt.Rows[0]["审核员"].ToString();
            String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
            MessageBox.Show(str人员信息);
        }
        
       
    }
}
