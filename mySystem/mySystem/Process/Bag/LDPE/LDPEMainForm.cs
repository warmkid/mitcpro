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
        }

        private void A1Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_materialrecord material = new LDPEBag_materialrecord(mainform);
            material.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LDPEBag_batchproduction batch = new LDPEBag_batchproduction(mainform);
            batch.ShowDialog();
        }

        private void A2Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_innerpackaging inner = new LDPEBag_innerpackaging(mainform);
            inner.ShowDialog();
        }

        private void B4Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_cleanrance cleanrance = new LDPEBag_cleanrance(mainform);
            cleanrance.ShowDialog();
        }

        private void B1Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_productioninstruction pro_ins = new LDPEBag_productioninstruction(mainform);
            pro_ins.ShowDialog();
        }

        private void B2Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_checklist check = new LDPEBag_checklist(mainform);
            check.ShowDialog();
        }

        private void B3Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_runningrecord run = new LDPEBag_runningrecord(mainform);
            run.ShowDialog();
        }

        private void A3Btn_Click(object sender, EventArgs e)
        {
            LDPEBag_dailyreport daily = new LDPEBag_dailyreport();
            daily.ShowDialog();
        }

        private void Btn热合强度_Click(object sender, EventArgs e)
        {
            mySystem.Process.Bag.LDPE.产品热合强度检验记录 rhform = new 产品热合强度检验记录(mainform);
            rhform.ShowDialog();
        }

        private void Btn外观及检验_Click(object sender, EventArgs e)
        {
            mySystem.Process.Bag.LDPE.产品外观和尺寸检验记录 wgform = new 产品外观和尺寸检验记录(mainform);
            wgform.ShowDialog();
        }


    }
}