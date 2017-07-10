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

namespace mySystem.Process.Extruction.C
{
    public partial class Feed : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜供料系统运行记录";
        DataTable dtFeed;
        OleDbDataAdapter daFeed;
        BindingSource bsFeed;
        OleDbCommandBuilder cbFeed;

        string tablename2 = "吹膜供料系统运行记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        private CheckForm check = null;
        int outerId;
        int status;
        public Feed(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            txb生产指令编号.Text = Parameter.proInstruction;
            binding1();
        }

        private void binding1()
        {
            bsFeed = new BindingSource();
            dtFeed = new DataTable(tablename1);
            daFeed = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "';", conOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
            cbFeed = new OleDbCommandBuilder(daFeed);
            daFeed.Fill(dtFeed);
            if (dtFeed.Rows.Count < 1)  //this record never met before, it needs to add new item and generate new clear information
            {
                status = 1;
                DataRow newrow = dtFeed.NewRow();
                dtFeed.Rows.Add(newrow);
                binding2(0, false);
            }
            else 
            {
                status = 2;
                binding2(getId(), true); 
            }
            
            bsFeed.DataSource = dtFeed;
            
            txb生产指令编号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            txb生产指令编号.DataBindings.Add("Text", bsFeed.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsFeed.DataSource, "生产日期");
            
        }

        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "';";
            return (int)comm.ExecuteScalar();           
        }
        private void binding2(int outerId, bool flag)
        {
            bsItem = new BindingSource();
            dtItem = new DataTable(tablename2);
            if (flag)
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where T吹膜供料系统运行记录ID = " + outerId, conOle);// +" where T吹膜岗位交接班记录ID in (select ID from " + tablename1 + " where 生产日期 = '" + dtp生产日期.Value.Date+"');",conOle);
            }
            else
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where 1=2 ", conOle);
            }
            cbItem = new OleDbCommandBuilder(daItem);
            daItem.Fill(dtItem);
            if (dtItem.Rows.Count < 1)
            {
                DataRow newrow = dtItem.NewRow();
                dtItem.Rows.Add(newrow);
                if (flag)
                {
                    dtItem.Rows[0]["T吹膜供料系统运行记录ID"] = outerId;
                }
            }

            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }
        public override void CheckResult()
        {
            base.CheckResult();

            dtFeed.Rows[0]["审核人"] = check.userID.ToString();
            dtFeed.Rows[0]["审核意见"] = check.opinion.ToString();
            dtFeed.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        private void ckb白班_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb白班.Checked==true)
            {
                ckb夜班.Checked = false;
                dtFeed.Rows[0]["班次"] = true;
            }
        }

        private void ckb夜班_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb夜班.Checked == true)
            {
                ckb白班.Checked = false;
                dtFeed.Rows[0]["班次"] = false;
            }
        }
        private void pullData()
        {
            dtFeed.Rows[0]["生产指令编号"] = txb生产指令编号.Text;
            dtFeed.Rows[0]["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            //dtWaste.Rows[0]["生产结束时间"] = Convert.ToDateTime(dtp生产结束时间.Value.ToShortDateString());
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            pullData();
            bsFeed.EndEdit();
            daFeed.Update((DataTable)bsFeed.DataSource);


            if (1 == status)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = conOle;
                comm.CommandText = "select @@identity";
                outerId = (int)comm.ExecuteScalar();
                dtItem.Rows[0]["T吹膜供料系统运行记录ID"] = Convert.ToInt32(outerId);
            }
                for (int i = 1; i < dtItem.Rows.Count; i++)
                {
                    dtItem.Rows[i]["T吹膜供料系统运行记录ID"] = dtItem.Rows[0]["T吹膜供料系统运行记录ID"];
                }
            
            bsItem.EndEdit();
            daItem.Update((DataTable)bsItem.DataSource);
        }
    }
}
