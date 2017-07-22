using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.灭菌
{
    public partial class 灭菌mainform : BaseForm
    {

        public 灭菌mainform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "Gamma射线辐射灭菌委托单");
            if (b)
            {
                Gamma射线辐射灭菌委托单 mydlg = new Gamma射线辐射灭菌委托单(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "辐照灭菌产品验收记录");
            if (b)
            {
                辐照灭菌产品验收记录 mydlg = new 辐照灭菌产品验收记录();
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }                   
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "辐照灭菌台帐");
            if (b)
            {
                辐照灭菌台帐 mydlg = new 辐照灭菌台帐(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
                      
        }

        //判断是否能查看
        private Boolean checkUser(String user, int role, String tblName)
        {
            Boolean b = false;
            String name1 = null;
            String name2 = null;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            comm.CommandText = "select * from 用户权限 where 步骤 = " + "'" + tblName + "' ";
            OleDbDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                name1 = reader["操作员"].ToString();
                name2 = reader["审核员"].ToString();
            }

            if (role == 3)
            {
                b = true;
            }
            else
            {
                if (user == name1)
                { b = true; }
                if (user == name2)
                { b = true; }
            }
            return b;
        }
    }
}
