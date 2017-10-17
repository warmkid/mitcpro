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

namespace mySystem.Process.Bag.BTV
{
    public partial class BTVMainForm : BaseForm
    {
        string instruction = null;
        int instruID = 0;

        public BTVMainForm()
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
            //默认下拉框选最后一个
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                Parameter.bpvbagInstruction = comboBox1.SelectedItem.ToString();
                String tblName = "生产指令";
                List<String> queryCols = new List<String>(new String[] { "ID" });
                List<String> whereCols = new List<String>(new String[] { "生产指令编号" });
                List<Object> whereVals = new List<Object>(new Object[] { instruction });
                List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                Parameter.bpvbagInstruID = Convert.ToInt32(res[0][0]);
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
            Btn批生产记录.Enabled = b;
            Btn内包装.Enabled = b;
            Btn清场记录.Enabled = b;
            Btn生产前确认.Enabled = b;
            Btn切管记录.Enabled = b;
            Btn装配确认.Enabled = b;
            Btn2D袋体生产记录.Enabled = b;
            Btn关键尺寸确认.Enabled = b;
            Btn原材料分装.Enabled = b;
            Btn底封机运行记录.Enabled = b;
            Btn泄漏测试记录.Enabled = b;
            Btn2D与船型.Enabled = b;
            Btn瓶口焊接机.Enabled = b;
            Btn多功能热合机.Enabled = b;
            Btn3D袋体生产记录.Enabled = b;
            Btn单管口热合机.Enabled = b;
            Btn90度热合机.Enabled = b;
            Btn封口热合机.Enabled = b;
            Btn打孔及与图纸.Enabled = b;
            Btn热合强度.Enabled = b;
            Btn外观尺寸.Enabled = b;
            Btn日报表.Enabled = b;
            Btn外包装.Enabled = b;
            Btn退料.Enabled = b;
            Btn洁净.Enabled = b;
            Btn交接班.Enabled = b;
            Btn结束.Enabled = b;

        }

        private void Btn生产领料_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "生产领料使用记录");
            if (b)
            {
                BTVMaterialRecord mydlg = new BTVMaterialRecord(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn批生产记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "制袋工序批生产记录");
            if (b)
            {
                BTVBatchProduction mydlg = new BTVBatchProduction();
                mydlg.ShowDialog();
                BPV批生产记录 bb = new BPV批生产记录(mainform);
                bb.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn内包装_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品内包装记录");
            if (b)
            {
                BTVInnerPackage mydlg = new BTVInnerPackage(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn清场记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "清场记录");
            if (b)
            {
                BTVClearanceRecord mydlg = new BTVClearanceRecord(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn生产指令_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "BPV制袋生产指令");
            if (b)
            {
                //BTVProInstruction mydlg = new BTVProInstruction();
                BPV制袋生产指令 mydlg = new BPV制袋生产指令(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn生产前确认_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "BPV生产前确认记录");
            if (b)
            {
                BTVConfirmBefore mydlg = new BTVConfirmBefore(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn切管记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "BPV切管记录");
            if (b)
            {
                BTVCutPipeRecord mydlg = new BTVCutPipeRecord(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn装配确认_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "BPV装配确认记录");
            if (b)
            {
                BTVAssemblyConfirm mydlg = new BTVAssemblyConfirm(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn2D袋体生产记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "2D袋体生产记录");
            if (b)
            {
                BTV2DProRecord mydlg = new BTV2DProRecord(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn关键尺寸确认_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "关键尺寸确认记录");
            if (b)
            {
                BTVKeySizeConfirm mydlg = new BTVKeySizeConfirm(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn原材料分装_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "原材料分装记录");
            if (b)
            {
                BTVRawMaterialDispensing mydlg = new BTVRawMaterialDispensing(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn底封机运行记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "底封机运行记录");
            if (b)
            {
                BTVRunningRecordDF mydlg = new BTVRunningRecordDF(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn泄漏测试记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "泄漏测试记录");
            if (b)
            {
                BTVLeakTest mydlg = new BTVLeakTest(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn2D与船型_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "2D袋体与船型接口热合记录");
            if (b)
            {
                BTV2DShipHeat mydlg = new BTV2DShipHeat(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn瓶口焊接机_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "瓶口焊接机运行记录");
            if (b)
            {
                BTVRunningRecordPK mydlg = new BTVRunningRecordPK(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn多功能热合机_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "多功能热合机运行记录");
            if (b)
            {
                BTVRunningRecordRHJMulti mydlg = new BTVRunningRecordRHJMulti(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn3D袋体生产记录_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "3D袋体生产记录");
            if (b)
            {
                BTV3DProRecord mydlg = new BTV3DProRecord(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn单管口热合机_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "单管口热合机运行记录");
            if (b)
            {
                BTVRunningRecordRHJsingle mydlg = new BTVRunningRecordRHJsingle(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn90度热合机_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "90度热合机运行记录");
            if (b)
            {
                BTVRunningRecordRHJ90 mydlg = new BTVRunningRecordRHJ90(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn封口热合机_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "封口热合机运行记录");
            if (b)
            {
                BTVRunningRecordRHJseal mydlg = new BTVRunningRecordRHJseal(mainform);
                mydlg.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            }
            
        }

        private void Btn打孔及与图纸_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "打孔及与图纸确认记录");
            if (b)
            {
                BTVPunchDrawingConfirm mydlg = new BTVPunchDrawingConfirm(mainform);
                mydlg.ShowDialog();
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
                BTV.BPV产品外包装记录 myform = new BTV.BPV产品外包装记录(mainform);
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
                BTV.BPV生产退料记录 myform = new BTV.BPV生产退料记录(mainform);
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
                BTV.BPV洁净区温湿度记录 myform = new BTV.BPV洁净区温湿度记录(mainform);
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
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + mySystem.Parameter.bpvbagInstruID, mySystem.Parameter.connOle);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Rows[0]["状态"] = 4;
                da.Update(dt);
            }
        }

        private void Btn热合强度_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品热合强度检验记录");
            if (b)
            {
                BTV.产品热合强度检验记录 myform = new BTV.产品热合强度检验记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn外观尺寸_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "产品外观和尺寸检验记录");
            if (b)
            {
                BTV.产品外观和尺寸检验记录 myform = new BTV.产品外观和尺寸检验记录(mainform);
                myform.ShowDialog();
            }
            else
            {
                MessageBox.Show("您无权查看该页面！");
                return;
            } 
            
        }

        private void Btn日报表_Click(object sender, EventArgs e)
        {

        }

        private void Btn交接班_Click(object sender, EventArgs e)
        {
            Boolean b = checkUser(Parameter.userName, Parameter.userRole, "岗位交接班记录");
            if (b)
            {
                //new窗口
                BTV.HandOver myform = new BTV.HandOver(mainform);
                myform.ShowDialog();
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