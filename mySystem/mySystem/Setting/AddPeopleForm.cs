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
        string flight = null; //班次
        string role = null; //角色
        int role_id; //角色id
        string department = null; //部门
        string job; //岗位

        //？？？sql中只存时间有time类型，不用datetime？？？
        DateTime starttime; //岗位开始时间  
        DateTime endtime; //岗位结束时间
        SetPeopleForm myparent = null;
        

        public AddPeopleForm(MainForm mainform, SetPeopleForm parent ):base(mainform)
        {
            InitializeComponent();
            myparent = parent;
            this.dtp班次开始时间.ShowUpDown = true;
            this.dtp班次结束时间.ShowUpDown = true;
        }

        private void comboBox班次_SelectedIndexChanged(object sender, EventArgs e)
        {
            flight = this.comboBox班次.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            role = comboBox角色.SelectedItem.ToString();
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

        private void comboBox部门_SelectedIndexChanged(object sender, EventArgs e)
        {
            department = comboBox部门.SelectedItem.ToString();
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
            starttime = this.dtp班次开始时间.Value;
            endtime = this.dtp班次结束时间.Value;
            job = this.tb岗位.Text.Trim();

            if (username == "" || password == "" || useridstr == "" || comboBox班次.SelectedIndex == -1 || comboBox角色.SelectedIndex == -1 || comboBox部门.SelectedIndex == -1 || job == "")
            {
                MessageBox.Show("员工信息不能为空", "错误");
                return;
            }
            else
            {
                if (Int32.TryParse(useridstr, out userid))
                { }
                else
                {
                    MessageBox.Show("员工ID必须为数字", "错误");
                    return;
                }
                String tblName = "users";
                List<String> insertCols = new List<String>(new String[] { "角色ID", "姓名", "密码", "班次", "角色", "部门", "用户ID", "班次开始时间", "班次结束时间", "岗位" });
                List<Object> insertVals = new List<Object>(new Object[] { role_id, username, password, flight, role, department, userid, starttime, endtime, job });
                Boolean b = Utility.insertAccess(Parameter.connOleUser, tblName, insertCols, insertVals);
                if (b)
                {
                    MessageBox.Show("用户添加成功", "success");
                    myparent.BindUser();
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
