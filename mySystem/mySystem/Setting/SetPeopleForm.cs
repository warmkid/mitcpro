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
    public partial class SetPeopleForm : BaseForm
    {
        private int userid = 0;
        private string username = null;
        private string userpw = null;
        private OleDbConnection connuser;
        private OleDbDataAdapter dauser;
        private DataTable dtuser;
        private BindingSource bsuser;
        private OleDbCommandBuilder cbuser;

        public SetPeopleForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            if (!Parameter.isSqlOk)
            { connuser = Parameter.connOleUser; }
            else
            { }
            
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
                    //dgvInit();
                    InitUser();
                    BindUser();
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
            String tblName = "users";
            List<String> queryCols = new List<String>(new String[] { "密码" });
            List<String> whereCols = new List<String>(new String[] { "用户ID" });
            List<Object> whereVals = new List<Object>(new Object[] { userid });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOleUser, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
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
            String tblName = "users";
            List<String> updateCols = new List<String>(new String[] { "姓名" });
            List<Object> updateVals = new List<Object>(new Object[] { username });
            List<String> whereCols = new List<String>(new String[] { "用户ID" });
            List<Object> whereVals = new List<Object>(new Object[] { userid });
            Boolean b = Utility.updateAccess(Parameter.connOleUser, tblName, updateCols, updateVals, whereCols, whereVals);
            
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
                        List<String> updateCols2 = new List<String>(new String[] { "密码" });
                        List<Object> updateVals2 = new List<Object>(new Object[] { pw3 });
                        Boolean b2 = Utility.updateAccess(Parameter.connOleUser, tblName, updateCols2, updateVals2, whereCols, whereVals);
                        pw1TextBox.Text = null;
                        pw2TextBox.Text = null;
                        pw3TextBox.Text = null;
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
            dgvUser.Rows.Clear();
            String usertblName = "users";
            List<String> queryCols = new List<String>(new String[] { "用户ID", "姓名", "密码", "班次", "角色" });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOleUser, usertblName, queryCols, null, null, null, null, null, null, null);
            Utility.fillDataGridView(dgvUser, res);

            //填入班次和角色
            int rows = dgvUser.RowCount - 1;
            for (int i = 0; i < rows; i++)
            {
                if (Convert.ToBoolean(dgvUser.Rows[i].Cells["班次"].Value) == true)
                {
                    dgvUser.Rows[i].Cells["班次"].Value = "白班";              
                }
                else
                {
                    dgvUser.Rows[i].Cells["班次"].Value = "夜班";                    
                }
            }
 
        }

        private void InitUser()
        {
            bsuser = new BindingSource();

            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.RowHeadersVisible = false;
            this.dgvUser.ReadOnly = false;
            this.dgvUser.DataError += this.dgvUser_DataError;
        }

        private void dgvUser_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void BindUser()
        {
            dtuser = new DataTable("users"); //""中的是表名
            dauser = new OleDbDataAdapter("select * from users ", connuser);
            cbuser = new OleDbCommandBuilder(dauser);

            dtuser.Columns.Add("序号", System.Type.GetType("System.String"));
            dauser.Fill(dtuser);
            bsuser.DataSource = dtuser;
            this.dgvUser.DataSource = bsuser.DataSource;

            //显示序号列
            int coun = this.dgvUser.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dtuser.Rows[i]["序号"] = i + 1;
            }

            this.dgvUser.Columns["用户ID"].HeaderText = "员工ID";
            this.dgvUser.Columns["ID"].Visible = false;
            this.dgvUser.Columns["角色ID"].Visible = false;
        }

        //角色和角色ID的统一
        private void changeRoleID()
        {
            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                String role = dgvUser.Rows[i].Cells["角色"].Value.ToString();
                switch (role)
                {
                    case "操作员":
                        dgvUser.Rows[i].Cells["角色ID"].Value = 1;
                        break;
                    case "计划员":
                        dgvUser.Rows[i].Cells["角色ID"].Value = 2;
                        break;
                    case "管理员":
                        dgvUser.Rows[i].Cells["角色ID"].Value = 3;
                        break;
                }
            }

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddPeopleForm addform = new AddPeopleForm(base.mainform, this);
            addform.ShowDialog();
      
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int deleteID = Convert.ToInt32(dgvUser.CurrentRow.Cells["用户ID"].Value);
            String tblName = "users";
            List<String> whereCols = new List<String>(new String[] { "用户ID" });
            List<Object> whereVals = new List<Object>(new Object[] { deleteID });
            Boolean b = Utility.deleteAccess(Parameter.connOleUser, tblName, whereCols, whereVals);
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
            changeRoleID();
            dauser.Update((DataTable)bsuser.DataSource);
            dtuser.Clear();
            dauser.Fill(dtuser);
            int coun = this.dgvUser.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dtuser.Rows[i]["序号"] = i + 1;
            }            
            
        }


    }
}
