using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Setting
{
    public partial class AddPeopleForm : BaseForm
    {
        string userid;
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

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            userid = IDtextBox.Text.Trim();           
            username = NametextBox.Text.Trim();
            password = PWtextBox.Text.Trim();

            if (username == "" || password == "" || userid == "" || comboBox角色.SelectedIndex == -1)
            {
                MessageBox.Show("员工信息不能为空", "错误");
                return;
            }
            else
            {
                bool idbool = isIDExist();
                if (!idbool)
                {
                    String tblName = "users";
                    //List<String> insertCols = new List<String>(new String[] { "角色ID", "姓名", "密码", "角色", "用户ID", "班次", "班次开始时间", "班次结束时间", "部门", "岗位" });
                    //List<Object> insertVals = new List<Object>(new Object[] { role_id, username, password, role, userid, "", DateTime.Now, DateTime.Now, "", "" });
                    List<String> insertCols = new List<String>(new String[] { "角色ID", "姓名", "密码", "角色", "用户ID" });
                    List<Object> insertVals = new List<Object>(new Object[] { role_id, username, password, role, userid });
                    
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
                else
                {
                    MessageBox.Show("该员工ID已经存在，请重新输入！", "警告");
                    return;
                }
            }
        }

        //判断用户ID是否已经存在
        private Boolean isIDExist()
        {
            bool b;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOleUser;
            comm.CommandText = "SELECT * FROM users WHERE 用户ID = " + "'" + userid + "'";
            OleDbDataReader reader = comm.ExecuteReader();
            b = reader.HasRows ? true : false;

            return b;
        }


        
    }
}
