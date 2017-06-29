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
    public partial class SetPeopleForm : BaseForm
    {
        private int userid = 0;
        private string username = null;
        private string userpw = null;

        public SetPeopleForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            RoleInit();
            Init();
        }

        private void Init()
        {
            userid = Parameter.userID;
            username = Parameter.userName;           
            idLabel.Text = userid.ToString();
            nameTextBox.Text = username;

        }

        private void RoleInit()
        {
            switch (Parameter.userRole)
            {
                case 1:
                    SetPeoplePanelBottom.Visible = false;
                    break;
                case 2:
                    SetPeoplePanelBottom.Visible = false;
                    break;
                case 3:
                    SetPeoplePanelBottom.Visible = true;
                    dgvInit();
                    break;
                default:
                    break;
            }

        }

        //*************************上半部分**************************
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            nameTextBox.Text = Parameter.userName;
            pw1TextBox.Text = null;
            pw2TextBox.Text = null;
            pw3TextBox.Text = null;

        }

        private Boolean CheckPWbefore(string pw)
        {
            String tblName = "user_aoxing";
            List<String> queryCols = new List<String>(new String[] { "user_password" });
            List<String> whereCols = new List<String>(new String[] { "user_id" });
            List<Object> whereVals = new List<Object>(new Object[] { userid });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            userpw = res[0][0].ToString(); //用户原始密码
            if(pw == userpw)
            {
                return true;
            }
            else
            {
                MessageBox.Show("原密码输入错误，密码修改失败", "错误");
                pw1TextBox.Text = null;
                pw2TextBox.Text = null;
                pw3TextBox.Text = null;
                return false;
            }            
        }


        private Boolean ComparePW(string pw1, string pw2)
        {
            if (pw1 == pw2)
            {
                MessageBox.Show("修改成功！", "成功");
                return true;
            }
            else
            {
                MessageBox.Show("新密码两次输入不一致，请重新输入", "错误");
                pw1TextBox.Text = null;
                pw2TextBox.Text = null;
                pw3TextBox.Text = null;
                return false;
 
            }                       
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            //修改姓名
            username = nameTextBox.Text.Trim();
            String tblName = "user_aoxing";
            List<String> updateCols = new List<String>(new String[] { "user_name" });
            List<Object> updateVals = new List<Object>(new Object[] { username });
            List<String> whereCols = new List<String>(new String[] { "user_id" });
            List<Object> whereVals = new List<Object>(new Object[] { userid });
            Boolean b = Utility.updateAccess(Parameter.connOle, tblName, updateCols, updateVals, whereCols, whereVals);
            
            //修改密码
            string pw1 = pw1TextBox.Text.Trim();
            string pw2 = pw2TextBox.Text.Trim();
            string pw3 = pw3TextBox.Text.Trim();
            if (pw1 != "" || pw2 != "" || pw3 != "")
            {
                Boolean check1 = CheckPWbefore(pw1);
                if (check1)
                {
                    Boolean check2 = ComparePW(pw2, pw3);
                    if (check2)
                    {
                        //保存密码至数据库
                        List<String> updateCols2 = new List<String>(new String[] { "user_password" });
                        List<Object> updateVals2 = new List<Object>(new Object[] { pw3 });
                        Boolean b2 = Utility.updateAccess(Parameter.connOle, tblName, updateCols2, updateVals2, whereCols, whereVals);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("姓名修改成功", "成功");
            }
            
            
        }


        //*************************下半部分**************************
        public void dgvInit()
        {
            dataGridView1.Rows.Clear();
            String usertblName = "user_aoxing";
            List<String> queryCols = new List<String>(new String[] { "user_id", "user_name", "user_password", "flight", "role_id" });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, usertblName, queryCols, null, null, null, null, null, null, null);
            Utility.fillDataGridView(dataGridView1, res);

            //填入班次和角色
            int rows = dataGridView1.RowCount - 1;
            for (int i = 0; i < rows; i++)
            {
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["flight"].Value) == 0)
                {
                    dataGridView1.Rows[i].Cells["flight"].Value = "白班";
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells["role_id"].Value) == 3)
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "管理员";
                    }
                    else if (Convert.ToInt32(dataGridView1.Rows[i].Cells["role_id"].Value) == 2)
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "计划员";
                    }
                    else 
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "操作员";
                    }
                }
                else
                {
                    dataGridView1.Rows[i].Cells["flight"].Value = "夜班";
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells["role_id"].Value) == 3)
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "管理员";
                    }
                    else if (Convert.ToInt32(dataGridView1.Rows[i].Cells["role_id"].Value) == 2)
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "计划员";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["role_id"].Value = "操作员";
                    }
                }


            }
 
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddPeopleForm addform = new AddPeopleForm(base.mainform, this);
            addform.Show();
      
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int deleteID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["user_id"].Value);
            String tblName = "user_aoxing";
            List<String> whereCols = new List<String>(new String[] { "user_id" });
            List<Object> whereVals = new List<Object>(new Object[] { deleteID });
            Boolean b = Utility.deleteAccess(Parameter.connOle, tblName, whereCols, whereVals);
            if (b)
            {
                MessageBox.Show("删除成功", "success");
                dgvInit();
            }
            else
            {
                MessageBox.Show("删除失败", "错误");
                return;
            }

            
        }

        private void SaveEditBtn_Click(object sender, EventArgs e)
        {
            int rows = dataGridView1.RowCount - 1;
            //读取dgv中的数据
            List<List<Object>> readdgv = Utility.readFromDataGridView(dataGridView1);
            //将班次和权限换为数字
            for (int i = 0; i < rows; i++)
            {
                if (readdgv[i][3] == "白班")
                {
                    readdgv[i][3] = 0;
                }
                else
                {
                    readdgv[i][3] = 1;
                }
            }
            for (int i = 0; i < rows; i++)
            {
                if (readdgv[i][4].ToString() == "操作员")
                {
                    readdgv[i][4] = 1;
                }
                else if ( readdgv[i][4].ToString() == "计划员")
               {
                    readdgv[i][4] = 2;
                }
                else
                {
                    readdgv[i][4] = 3;
                }
            }


            string tblName = "user_aoxing";
            List<String> queryCols = new List<String>(new String[] { "user_name", "user_password", "flight", "role_id" });
            for (int i = 0; i < rows; i++)
            {
                List<Object> queryVals = new List<Object>(new Object[] { readdgv[i][1], readdgv[i][2], Convert.ToInt32(readdgv[i][3]), Convert.ToInt32(readdgv[i][4])});
                List<String> whereCols = new List<String>(new String[] { "user_id" });
                List<Object> whereVals = new List<Object>(new Object[] { Convert.ToInt32(readdgv[i][0]) });
                Boolean b = Utility.updateAccess(Parameter.connOle, tblName, queryCols, queryVals, whereCols, whereVals);
             
            }
            MessageBox.Show("保存成功！", "success");
            dgvInit();

        }







    }
}
