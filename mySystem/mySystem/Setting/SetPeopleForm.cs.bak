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
                    InitUser();
                    BindUser();
                    break;
                default:
                    break;
            }

        }

        #region 上半部分
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
            List<List<Object>> res;
            if (mySystem.Parameter.isSqlOk)
            {
                res = Utility.selectAccess(Parameter.conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            }
            else
            {
                res = Utility.selectAccess(Parameter.connOleUser, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            }
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
                    {  return;}
                }
                else
                {  return; }
            }
            else
            {
                MessageBox.Show("姓名修改成功", "成功");
            }
            
            
        }
        #endregion

        #region 下半部分
        //*************************下半部分*************************        
        public void InitUser()
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

        public void BindUser()
        {
            this.dgvUser.DataBindings.Clear();
            for (int i = this.dgvUser.Columns.Count; i > 0; i--)
            { this.dgvUser.Columns.RemoveAt(i - 1); }  

            dtuser = new DataTable("users"); //""中的是表名
            dauser = new OleDbDataAdapter("select * from users ", connuser);
            cbuser = new OleDbCommandBuilder(dauser);

            dtuser.Columns.Add("序号", System.Type.GetType("System.String"));
            dauser.Fill(dtuser);
            bsuser.DataSource = dtuser;
           
            changeColView(); //列改为combobox                    
            this.dgvUser.DataSource = bsuser.DataSource;
            setDataGridViewRowNums(); //序号           
            Utility.setDataGridViewAutoSizeMode(dgvUser);
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
            //dgvUser.FirstDisplayedScrollingRowIndex = dgvUser.Rows.Count - 1;
            if (dgvUser.Rows.Count > 0)
                dgvUser.FirstDisplayedScrollingRowIndex = dgvUser.Rows.Count - 1;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int idx = this.dgvUser.CurrentRow.Index;
            dtuser.Rows[idx].Delete();
            dauser.Update((DataTable)bsuser.DataSource);
            dtuser.Clear();
            dauser.Fill(dtuser);
            setDataGridViewRowNums();
            MessageBox.Show("删除成功", "success");                      
        }

        private void SaveEditBtn_Click(object sender, EventArgs e)
        {
            changeRoleID();
            dauser.Update((DataTable)bsuser.DataSource);
            dtuser.Clear();
            dauser.Fill(dtuser);
            setDataGridViewRowNums();           
            
        }

        private void setDataGridViewRowNums()
        {
            int coun = this.dgvUser.Rows.Count;
            for (int i = 0; i < coun; i++)
            {
                this.dgvUser.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }

        //更改列显示形态
        private void changeColView()
        {
            foreach (DataColumn dc in dtuser.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "班次":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;
                       
                        c1.Items.Add("白班");
                        c1.Items.Add("夜班");
                        dgvUser.Columns.Add(c1);

                        break;
                    case "角色":
                        DataGridViewComboBoxColumn c2 = new DataGridViewComboBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        c2.Items.Add("操作员");
                     //   c2.Items.Add("计划员");
                        c2.Items.Add("管理员");
                        dgvUser.Columns.Add(c2);
                        break;
                    //case "岗位":
                    //    DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
                    //    c3.DataPropertyName = dc.ColumnName;
                    //    c3.HeaderText = dc.ColumnName;
                    //    c3.Name = dc.ColumnName;
                    //    c3.SortMode = DataGridViewColumnSortMode.Automatic;
                    //    c3.ValueType = dc.DataType;
                    //    c3.Items.Add("岗位1");
                    //    c3.Items.Add("岗位2");
                    //    c3.Items.Add("岗位3");
                    //    dgvUser.Columns.Add(c3);
                    //    break;
                    default:
                        DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
                        c5.DataPropertyName = dc.ColumnName;
                        c5.HeaderText = dc.ColumnName;
                        c5.Name = dc.ColumnName;
                        c5.SortMode = DataGridViewColumnSortMode.Automatic;
                        c5.ValueType = dc.DataType;
                        dgvUser.Columns.Add(c5);

                        break;
                }


            }
        }

        private void dgvUser_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvUser.Columns["ID"].Visible = false;
            this.dgvUser.Columns["角色ID"].Visible = false;
            this.dgvUser.Columns["班次"].Visible = false;
            this.dgvUser.Columns["部门"].Visible = false;
            this.dgvUser.Columns["岗位"].Visible = false;
            this.dgvUser.Columns["班次开始时间"].Visible = false;
            this.dgvUser.Columns["班次结束时间"].Visible = false;

            this.dgvUser.Columns["用户ID"].MinimumWidth = 150;
            this.dgvUser.Columns["密码"].MinimumWidth = 150;

            ////设置列宽
            //for (int i = 0; i < this.dgvUser.Columns.Count; i++)
            //{
            //    String colName = this.dgvUser.Columns[i].HeaderText;
            //    int strlen = colName.Length;
            //    this.dgvUser.Columns[i].MinimumWidth = strlen * 25;
            //}  

        }

        #endregion

    }
}
