using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Process.Extruction.B
{

    public partial class Waste : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜工序废品记录";
        DataTable dtWaste;
        OleDbDataAdapter daWaste;
        BindingSource bsWaste;
        OleDbCommandBuilder cbWaste;

        string tablename2 = "吹膜工序废品记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        private CheckForm check = null;
        int outerId;
        int status;
        public Waste(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            txb生产指令.Text = Parameter.proInstruction;
            binding1();
            //binding2(0);
        }
        public override void CheckResult()
        {
            base.CheckResult();

            dtWaste.Rows[0]["审核人"] = check.userID.ToString();
            dtWaste.Rows[0]["审核意见"] = check.opinion.ToString();
            dtWaste.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
        }

        private void binding1()
        {
            bsWaste = new BindingSource();
            dtWaste = new DataTable(tablename1);
            daWaste = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令 ='" + txb生产指令.Text + "';", conOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
            cbWaste = new OleDbCommandBuilder(daWaste);
            daWaste.Fill(dtWaste);
            if (dtWaste.Rows.Count < 1)
            {
                status = 1;
                DataRow newrow = dtWaste.NewRow();
                dtWaste.Rows.Add(newrow);
                binding2(0, false);
            }
            else
            {
                status = 2;
                binding2(getId(), true);
            }
           
            bsWaste.DataSource = dtWaste;
            //
            txb生产指令.DataBindings.Clear();
            dtp生产结束时间.DataBindings.Clear();
            dtp生产开始时间.DataBindings.Clear();
            txb生产指令.DataBindings.Add("Text", bsWaste.DataSource, "生产指令");
            dtp生产开始时间.DataBindings.Add("Value", bsWaste.DataSource, "生产开始时间");
            dtp生产结束时间.DataBindings.Add("Value", bsWaste.DataSource, "生产结束时间");
        }

        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令 ='" + txb生产指令.Text + "';";
            return (int)comm.ExecuteScalar();
        }
        private void binding2(int outerId, bool flag)
        {
            bsItem = new BindingSource();
            dtItem = new DataTable(tablename2);
            if (flag)
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where T吹膜工序废品记录ID = " + outerId, conOle);// +" where T吹膜岗位交接班记录ID in (select ID from " + tablename1 + " where 生产日期 = '" + dtp生产日期.Value.Date+"');",conOle);
            }
            else 
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where false", conOle);
            }
            cbItem = new OleDbCommandBuilder(daItem);
            daItem.Fill(dtItem);
            if (dtItem.Rows.Count < 1)
            {
                DataRow newrow = dtItem.NewRow();
                dtItem.Rows.Add(newrow);
                if (flag)
                {
                    dtItem.Rows[0]["T吹膜工序废品记录ID"] = outerId;
                }
            }
            
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }


        private void pullData()
        {
            dtWaste.Rows[0]["生产指令"] = txb生产指令.Text;
            dtWaste.Rows[0]["生产开始时间"] = Convert.ToDateTime(dtp生产开始时间.Value.ToShortDateString());
            dtWaste.Rows[0]["生产结束时间"] = Convert.ToDateTime(dtp生产结束时间.Value.ToShortDateString());
        }
        private void btn保存_Click(object sender, EventArgs e)
        {
            pullData();
            bsWaste.EndEdit();
            daWaste.Update((DataTable)bsWaste.DataSource);

            if (1 == status)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = conOle;
                comm.CommandText = "select @@identity";
                outerId = (int)comm.ExecuteScalar();
                dtItem.Rows[0]["T吹膜工序废品记录ID"] = Convert.ToInt32(outerId);
            }

           

            for (int i = 1; i < dtItem.Rows.Count; i++)
            {
                dtItem.Rows[i]["T吹膜工序废品记录ID"] = dtItem.Rows[0]["T吹膜工序废品记录ID"];
            }
            bsItem.EndEdit();
            daItem.Update((DataTable)bsItem.DataSource);
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
    }
}
