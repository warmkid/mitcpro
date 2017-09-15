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

namespace mySystem.Process.Bag.LDPE
{
    public partial class LDPEMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public LDPEMainForm()
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
                OleDbDataReader reader = comm.ExecuteReader();
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
            Parameter.ldpebagInstruction = instruction;
            String tblName = "生产指令";
            List<String> queryCols = new List<String>(new String[] { "ID" });
            List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
            List<Object> whereVals = new List<Object>(new Object[] { instruction });
            List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            instruID = Convert.ToInt32(res[0][0]);
            Parameter.ldpebagInstruID = instruID;
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
            Btn日报表.Enabled = b;
            Btn标签.Enabled = b;
            Btn开机前确认.Enabled = b;
            Btn运行记录.Enabled = b;
            Btn清场.Enabled = b;
            Btn批生产记录.Enabled = b;
            Btn热合强度.Enabled = b;
            Btn外观及检验.Enabled = b;
            Btn外包装.Enabled = b;
            Btn洁净.Enabled = b;
            Btn退料.Enabled = b;
            Btn交接班.Enabled = b;
            Btn结束.Enabled = b;
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "LDPE制袋生产领料记录");
            if (b)
            {
                LDPEBag_materialrecord material = new LDPEBag_materialrecord(mainform);
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
                LDPEBag_batchproduction batch = new LDPEBag_batchproduction(mainform);
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
                LDPEBag_innerpackaging inner = new LDPEBag_innerpackaging(mainform);
                inner.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清场记录");
            if (b)
            {
                LDPEBag_cleanrance cleanrance = new LDPEBag_cleanrance(mainform);
                cleanrance.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "LDPE制袋生产指令");
            if (b)
            {
                LDPEBag_productioninstruction pro_ins = new LDPEBag_productioninstruction(mainform);
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
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋机开机前确认表");
            if (b)
            {
                LDPEBag_checklist check = new LDPEBag_checklist(mainform);
                check.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
           
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋机运行记录");
            if (b)
            {
                LDPEBag_runningrecord run = new LDPEBag_runningrecord(mainform);
                run.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_dailyreport daily = new LDPEBag_dailyreport();
            daily.ShowDialog();
            
        }

        private void Btn热合强度_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品热合强度检验记录");
            if (b)
            {
                mySystem.Process.Bag.LDPE.产品热合强度检验记录 rhform = new 产品热合强度检验记录(mainform);
                rhform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn外观及检验_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品外观和尺寸检验记录");
            if (b)
            {
                mySystem.Process.Bag.LDPE.产品外观和尺寸检验记录 wgform = new 产品外观和尺寸检验记录(mainform);
                wgform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn外包装_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品外包装记录表");
            if (b)
            {
                LDPE.LDPE产品外包装记录 myform = new LDPE.LDPE产品外包装记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn退料_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "生产退料记录表");
            if (b)
            {
                LDPE.LDPE生产退料记录 myform = new LDPE.LDPE生产退料记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn洁净_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "洁净区温湿度记录表");
            if (b)
            {
                LDPE.LDPE洁净区温湿度记录 myform = new LDPE.LDPE洁净区温湿度记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn结束_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否确认结束工序？", "提示", MessageBoxButtons.YesNo))
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + mySystem.Parameter.ldpebagInstruID, mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows[0]["状态"] = 4;
                da.Update(dt);
            }
        }

        private void Btn交接班_Click(object sender, EventArgs e)
        {
           Boolean b = checkUser(Parameter.userName, Parameter.userRole, "岗位交接班记录");
            if (b)
            {
                //new窗口
                LDPE.HandOver myform = new LDPE.HandOver(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
        }

        private void Btn标签_Click(object sender, EventArgs e)
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