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
        string flight = null;
        int flight_id;
        string role = null;
        int role_id;
        SetPeopleForm myfather = null;

        public AddPeopleForm(MainForm mainform, SetPeopleForm father ):base(mainform)
        {
            InitializeComponent();
            myfather = father;
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
            string useridstr = IDtextBox.Text.Trim();           
            username = NametextBox.Text.Trim();
            password = PWtextBox.Text.Trim();
            if (username == "" || password == "" || useridstr == ""|| flight == "" || role == "")
            {
                MessageBox.Show("员工信息不能为空", "错误");
                return;
            }
            else
            {
                userid = Convert.ToInt32(useridstr);
                String tblName = "user_aoxing";
                //查最后一行的id？
                //List<String> idCol = new List<String>(new String[] { "id" });
                //List<List<Object>> idres = Utility.selectAccess(Parameter.connOle, tblName, idCol, null, null, null, null, null, null, null);
                //int idlast = Convert.ToInt32(idres[idres.Count-1][0]);

                List<String> insertCols = new List<String>(new String[] { "createtime", "modifytime", "user_id", 
                    "user_name", "user_password", "last_login_time", "role_id", "department_id", "flight" });          
                DateTime dt = DateTime.Now;
                DateTime insertVal = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                List<Object> insertVals = new List<Object>(new Object[] { insertVal, insertVal, userid, username, password, insertVal, role_id, 1, flight_id });
                Boolean b = Utility.insertAccess(Parameter.connOle, tblName, insertCols, insertVals);
                if (b)
                {
                    MessageBox.Show("用户添加成功", "success");
                    myfather.dgvInit();
                    this.Close();
                    this.Dispose();
                    return;
                }
                else
                {
                    MessageBox.Show("用户添加失败", "错误");
                    return;
                }

            }


        }

        
    }
}
