using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mySystem.Process.CleanCut
{
    public partial class CleanCutMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public CleanCutMainForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Init();
            InitBtn();
        }

        private void Init()
        {
            HashSet<String> hash = new HashSet<String>();
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from 清洁分切工序生产指令 where 状态 = 2";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hash.Add(reader["生产指令编号"].ToString());
                    }
                    foreach (String code in hash)
                    {
                        comboBox1.Items.Add(code);
                    }
                }
                comm.Dispose();
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select * from 清洁分切工序生产指令 where 状态 = 2";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hash.Add(reader["生产指令编号"].ToString());
                    }
                    foreach (String code in hash)
                    {
                        comboBox1.Items.Add(code);
                    }
                }
                comm.Dispose();

            }
            //默认下拉框选最后一个
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            Parameter.cleancutInstruction = comboBox1.SelectedItem.ToString();
            String tblName = "清洁分切工序生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { Parameter.cleancutInstruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            Parameter.cleancutInstruID = Convert.ToInt32(res[0][0]);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.cleancutInstruction = instruction;
            String tblName = "清洁分切工序生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.cleancutInstruID = instruID;
            InitBtn();
        }

        //初始化按钮状态
        public void InitBtn()
        {
            if (comboBox1.SelectedIndex == -1)
            { otherBtnInit(false); }
            else
            { otherBtnInit(true); }
        }

        private void otherBtnInit(bool b)
        {
            Btn生产记录.Enabled = b;
            Btn日报表.Enabled = b;
            Btn标签.Enabled = b;
            Btn开机确认.Enabled = b;
            Btn运行记录.Enabled = b;
            Btn清场.Enabled = b;
            Btn批生产.Enabled = b;
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清洁分切工序生产指令");
            if (b)
            {
                Instru form4 = new Instru(base.mainform);
                form4.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }                    
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清场记录");
            if (b)
            {
                Record_cleansite_cut form6 = new Record_cleansite_cut(base.mainform);
                form6.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }            
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清洁分切开机确认");
            if (b)
            {
                CleanCut_CheckBeforePower form5 = new CleanCut_CheckBeforePower(mainform);
                form5.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }            
        }

        private void A4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清洁分切生产记录表");
            if (b)
            {
                CleanCut_Productrecord form1 = new CleanCut_Productrecord(mainform);
                form1.ShowDialog();
            }
            else
            { 
                MessageBox.Show("您无权查看该页面！");
                return;
            }
        }

        private void A5Btn_Click(object sender, EventArgs e)
        {
            DailyRecord form2 = new DailyRecord(mainform);
            form2.ShowDialog();
            
        }

        private void A6Btn_Click(object sender, EventArgs e)
        {
            清洁分切标签 myform = new 清洁分切标签();
            myform.ShowDialog();
        }

        private void A7Btn_Click(object sender, EventArgs e)
        {
            CleanCut_Cover c= new CleanCut_Cover(mainform);
            c.ShowDialog();
        }

        private void A8Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清洁分切运行记录");
            if (b)
            {
                CleanCut_RunRecord form8 = new CleanCut_RunRecord(mainform);
                form8.ShowDialog();
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
            String[] name操作员 = null;
            String[] name审核员 = null;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            if (tblName != "清洁分切工序生产指令")
            {
                tblName = "全部";
            }
            comm.CommandText = "select * from 用户权限 where 步骤 = " + "'" + tblName + "' ";
            OleDbDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                name操作员 = reader["操作员"].ToString().Split("，,".ToCharArray());
                name审核员 = reader["审核员"].ToString().Split("，,".ToCharArray());
            }

            if (role == 3)
            {
                return b = true;
            }
            else
            {
                foreach (String name in name操作员)
                {
                    if (user == name)
                    { return b = true; }
                }
                foreach (String name in name审核员)
                {
                    if (user == name)
                    { return b = true; }
                }

            }
            return b = false;
        }

        private void btn工序结束_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem==null)
            {
                MessageBox.Show("请先选择一个生产指令");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("确定结束本次工序吗？", "警告", MessageBoxButtons.YesNo))
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where ID="+mySystem.Parameter.cleancutInstruID, mySystem.Parameter.connOle);
                DataTable dt = new DataTable();
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                da.Fill(dt);
                dt.Rows[0]["状态"] = 4;
                da.Update(dt);
            }
        }

    }
}
