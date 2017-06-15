using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class CleanArea_add : Form
    {
        public CleanArea_add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.ToString();
            string content = textBox2.Text.ToString();
            string strCon = @"server=10.105.223.19,56625;database=wyttest;Uid=sa;Pwd=mitc";
            SqlConnection conn = new SqlConnection(strCon);

            string sql = "insert into CleanArea_table (名称,内容) values("+"'"+name+"'"+","+"'"+content+"'"+");";
            System.Console.WriteLine(sql+"*************************************************************************");
            int result = 0;
            using (SqlCommand comm = new SqlCommand(sql, conn))
            {
                conn.Open();
                result = comm.ExecuteNonQuery();
            }
            this.Close();
            if (result > 0)
            {
                MessageBox.Show("添加成功");
            }
            else { MessageBox.Show("错误"); }
        }
    }
}
