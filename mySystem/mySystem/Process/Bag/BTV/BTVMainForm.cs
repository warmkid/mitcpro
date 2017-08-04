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
        }

        private void Btn生产领料_Click(object sender, EventArgs e)
        {
            BTVMaterialRecord mydlg = new BTVMaterialRecord();
            mydlg.ShowDialog();
        }

        private void Btn批生产记录_Click(object sender, EventArgs e)
        {
            BTVBatchProduction mydlg = new BTVBatchProduction();
            mydlg.ShowDialog();
        }

        private void Btn内包装_Click(object sender, EventArgs e)
        {
            BTVInnerPackage mydlg = new BTVInnerPackage();
            mydlg.ShowDialog();
        }

        private void Btn清场记录_Click(object sender, EventArgs e)
        {
            BTVClearanceRecord mydlg = new BTVClearanceRecord();
            mydlg.ShowDialog();
        }

        private void Btn生产指令_Click(object sender, EventArgs e)
        {
            BTVProInstruction mydlg = new BTVProInstruction();
            mydlg.ShowDialog();
        }

        private void Btn生产前确认_Click(object sender, EventArgs e)
        {
            BTVConfirmBefore mydlg = new BTVConfirmBefore();
            mydlg.ShowDialog();
        }

        private void Btn切管记录_Click(object sender, EventArgs e)
        {
            BTVCutPipeRecord mydlg = new BTVCutPipeRecord();
            mydlg.ShowDialog();
        }

        private void Btn装配确认_Click(object sender, EventArgs e)
        {
            BTVAssemblyConfirm mydlg = new BTVAssemblyConfirm();
            mydlg.ShowDialog();
        }

        private void Btn2D袋体生产记录_Click(object sender, EventArgs e)
        {
            BTV2DProRecord mydlg = new BTV2DProRecord();
            mydlg.ShowDialog();
        }

        private void Btn关键尺寸确认_Click(object sender, EventArgs e)
        {
            BTVKeySizeConfirm mydlg = new BTVKeySizeConfirm();
            mydlg.ShowDialog();
        }

        private void Btn原材料分装_Click(object sender, EventArgs e)
        {
            BTVRawMaterialDispensing mydlg = new BTVRawMaterialDispensing();
            mydlg.ShowDialog();
        }

        private void Btn底封机运行记录_Click(object sender, EventArgs e)
        {
            BTVRunningRecordDF mydlg = new BTVRunningRecordDF();
            mydlg.ShowDialog();
        }

        private void Btn泄漏测试记录_Click(object sender, EventArgs e)
        {
            BTVLeakTest mydlg = new BTVLeakTest();
            mydlg.ShowDialog();
        }

        private void Btn2D与船型_Click(object sender, EventArgs e)
        {
            BTV2DShipHeat mydlg = new BTV2DShipHeat();
            mydlg.ShowDialog();
        }

        private void Btn瓶口焊接机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordPK mydlg = new BTVRunningRecordPK(mainform);
            mydlg.ShowDialog();
        }

        private void Btn多功能热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJMulti mydlg = new BTVRunningRecordRHJMulti();
            mydlg.ShowDialog();
        }

        private void Btn3D袋体生产记录_Click(object sender, EventArgs e)
        {
            BTV3DProRecord mydlg = new BTV3DProRecord();
            mydlg.ShowDialog();
        }

        private void Btn单管口热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJsingle mydlg = new BTVRunningRecordRHJsingle();
            mydlg.ShowDialog();
        }

        private void Btn90度热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJ90 mydlg = new BTVRunningRecordRHJ90();
            mydlg.ShowDialog();
        }

        private void Btn封口热合机_Click(object sender, EventArgs e)
        {
            BTVRunningRecordRHJseal mydlg = new BTVRunningRecordRHJseal();
            mydlg.ShowDialog();
        }

        private void Btn打孔及与图纸_Click(object sender, EventArgs e)
        {
            BTVPunchDrawingConfirm mydlg = new BTVPunchDrawingConfirm();
            mydlg.ShowDialog();
        }

        
    }
}
