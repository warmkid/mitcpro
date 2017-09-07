using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Bag.PTV
{
    public partial class PTVMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public PTVMainForm()
        {
            InitializeComponent();
            comboInit();
            InitBtn();
        }

        //下拉框获取生产指令
        public void comboInit()
        {
            HashSet<String> hash = new HashSet<String>();
            if (!Parameter.isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select * from 生产指令 where 状态 = 2 ";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    comboBox1.Items.Clear();
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

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            instruction = comboBox1.SelectedItem.ToString();
            Parameter.bpvbagInstruction = instruction;
            String tblName = "生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.bpvbagInstruID = instruID;
            InitBtn();
        }

        private void InitBtn()
        {
            if (comboBox1.SelectedIndex == -1)
                otherBtnInit(false);
            else
                otherBtnInit(true);
        }

        private void otherBtnInit(bool b)
        {
            Btn生产领料.Enabled = b;
            Btn产品内包装.Enabled = b;
            Btn生产日报表.Enabled = b;
            Btn开机前确认.Enabled = b;
            Btn底封机.Enabled = b;
            Btn圆口焊接机.Enabled = b;
            Btn泄露测试.Enabled = b;
            Btn超声波.Enabled = b;
            Btn瓶口焊接机.Enabled = b;
            Btn清场记录.Enabled = b;
            Btn批生产记录.Enabled = b;
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "生产领料使用记录");
            if (b)
            {
                PTVBag_materialrecord material = new PTVBag_materialrecord(mainform);
                material.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋工序批生产记录");
            if (b)
            {
                PTVBag_batchproduction batch = new PTVBag_batchproduction(mainform);
                batch.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品内包装记录");
            if (b)
            {
                PTVBag_innerpackaging inner = new PTVBag_innerpackaging(mainform);
                inner.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }              
        }

        private void B8Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清场记录");
            if (b)
            {
                PTVBag_clearance clearance = new PTVBag_clearance(mainform);
                clearance.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "PTV生产指令");
            if (b)
            {
                PTVBag_productioninstruction pro_ins = new PTVBag_productioninstruction(mainform);
                pro_ins.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }              
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "PTV生产开机确认表");
            if (b)
            {
                PTVBag_checklist check = new PTVBag_checklist(mainform);
                check.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            PTVBag_dailyreport daily = new PTVBag_dailyreport(mainform);
            daily.ShowDialog();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "底封机运行记录");
            if (b)
            {
                PTVBag_runningrecordofdf df = new PTVBag_runningrecordofdf(mainform);
                df.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
           
        }

        private void B6Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "超声波焊接记录");
            if (b)
            {
                PTVBag_weldingrecordofwave wave = new PTVBag_weldingrecordofwave(mainform);
                wave.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "圆口焊接机运行记录");
            if (b)
            {
                PTVBag_runningrecordofyk yk = new PTVBag_runningrecordofyk(mainform);
                yk.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B5Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "泄露测试记录");
            if (b)
            {
                PTVBag_testrecordofdisclose xlDlg = new PTVBag_testrecordofdisclose(mainform);
                xlDlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B7Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "瓶口焊接机运行记录");
            if (b)
            {
                PTVBag_runningrecordofpk pk = new PTVBag_runningrecordofpk(mainform);
                pk.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn外包装_Click(object sender, EventArgs e)
        {
            PTV.PTV产品外包装记录 myform = new PTV.PTV产品外包装记录();
            myform.ShowDialog();
        }

        private void Btn退料_Click(object sender, EventArgs e)
        {
            PTV.PTV生产退料记录 myform = new PTV.PTV生产退料记录();
            myform.ShowDialog();
        }

        private void Btn洁净_Click(object sender, EventArgs e)
        {
            PTV.PTV洁净区温湿度记录 myform = new PTV.PTV洁净区温湿度记录();
            myform.ShowDialog();
        }

        private void Btn交接班_Click(object sender, EventArgs e)
        {

        }

        private void Btn结束_Click(object sender, EventArgs e)
        {

        }



        private void PTVMainForm_Load(object sender, EventArgs e)
        {

        }

        //判断是否能查看
        private Boolean checkUser(String user, int role, String tblName)
        {
            Boolean b = false;
            String[] name操作员 = null;
            String[] name审核员 = null;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
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

       

    }
}
