using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class CleanArea_add : mySystem.BaseForm
    {
        //SqlConnection conn = null;//连接sql
        //OleDbConnection connOle = null;//连接access
        //bool isSqlOk;//使用sql还是access

        public CleanArea_add(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.ToString();
            string content = textBox2.Text.ToString();

            string sql = "insert into cleanarea (清洁区域,清洁内容) values(" + "'" + name + "'" + "," + "'" + content + "'" + ");";
            int result = 0;
            if (mainform.isSqlOk)
            {       
                SqlCommand comm = new SqlCommand(sql, mainform.conn);
                result = comm.ExecuteNonQuery();
            }
            else
            {
                OleDbCommand comm = new OleDbCommand(sql, mainform.connOle);
                result = comm.ExecuteNonQuery();
            }
            if (result > 0)
            {
                MessageBox.Show("添加成功");
            }
            else { MessageBox.Show("错误"); }
            this.Close();
        }
    }
}
