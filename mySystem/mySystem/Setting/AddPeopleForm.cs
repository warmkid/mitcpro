using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class AddPeopleForm : BaseForm
    {
        int userid;
        string username;
        string password;
        string flight;
        int flight_id;
        string role;
        int role_id;
        
        public AddPeopleForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flight = comboBox1.SelectedItem.ToString();
            if (flight == "白班")
            {
                flight_id = 0;
            }
            else
            {
                flight_id = 1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            role = comboBox2.SelectedItem.ToString();
            if (role == "操作员")
            {
                role_id = 1;
            }
            else if (role == "计划员")
            {
                role_id = 2;
            }
            else
            {
                role_id = 3;
            }
        }


        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            userid = Convert.ToInt32(IDtextBox.Text.Trim());
            username = NametextBox.Text.Trim();
            password = PWtextBox.Text.Trim();
            if (username == "" || password == "" || userid == 0)
            {
                MessageBox.Show("用户信息不能为空", "错误");
                return;
            }
            else
            {
                String tblName = "user_aoxing";
                List<String> insertCols = new List<String>(new String[] { "createtime", "modifytime", "user_id", 
                    "user_name", "user_password", "last_login_time", "role_id", "department_id", "flight" });          
                DateTime dt = DateTime.Now;
                DateTime insertVal = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                List<Object> insertVals = new List<Object>(new Object[] { insertVal, insertVal, userid, username, password, insertVal, role_id, 1, flight_id });
                Boolean b = Utility.insertAccess(base.mainform.connOle, tblName, insertCols, insertVals);
                if (b)
                {
                    MessageBox.Show("用户添加成功", "success");
                }
                else
                {
                    MessageBox.Show("用户添加失败", "错误");
                }

            }


        }

        
    }
}
