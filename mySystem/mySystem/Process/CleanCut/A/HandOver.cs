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

namespace mySystem.Process.Extruction.A
{
    public partial class HandOver : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜岗位交接班记录";
        DataTable dtHandOver;
        OleDbDataAdapter daHandOver;
        BindingSource bsHandOver;
        OleDbCommandBuilder cbHandOver;

        string tablename2 = "吹膜岗位交接班项目记录";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        string tablename3 = "设置岗位交接班项目";
        DataTable dtSettingHandOver;
        OleDbDataAdapter daSettingHandOver;
        BindingSource bsSettingHandOver;
        OleDbCommandBuilder cbSettingHandOver;

        int status;
        int outerId;
        
        public HandOver(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            init();
            txb生产指令编号.Text = Parameter.proInstruction;
            dtp生产日期.Value = Convert.ToDateTime("2017/7/12"); ;
            binding();
            //binding2();
            //binding3();
        }
        private void init()
        {

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            // 处理DataGridView中数据类型输错的情况
            dataGridView1.DataError += dataGridView1_DataError;

        }
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void searchID()
        {
 
        }

        private void binding()
        {
            bsHandOver = new BindingSource();
            dtHandOver = new DataTable(tablename1);
            daHandOver = new OleDbDataAdapter("select * from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "' and 生产日期 = #"+dtp生产日期.Value.ToShortDateString()+"#;", conOle);// + " where 生产日期 = '" + dtp生产日期.Value.Date+"'", conOle);
            cbHandOver = new OleDbCommandBuilder(daHandOver);
            daHandOver.Fill(dtHandOver);

            if (dtHandOver.Rows.Count < 1)
            {
                status = 1;
                DataRow newrow = dtHandOver.NewRow();
                dtHandOver.Rows.Add(newrow);
                binding2(0, false);
            }
            else
            {
                status = 2;
                binding2(getId(), true);
            }
            bsHandOver.DataSource = dtHandOver;
            // 
            txb生产指令编号.DataBindings.Add("Text", bsHandOver.DataSource, "生产指令编号");
            dtp生产日期.DataBindings.Add("Value", bsHandOver.DataSource, "生产日期");
            txb白班产品代码批号数量.DataBindings.Add("Text", bsHandOver.DataSource, "白班产品代码批号数量");
            txb夜班产品代码批号数量.DataBindings.Add("Text", bsHandOver.DataSource, "夜班产品代码批号数量");
            txb白班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "白班异常情况处理");
            txb白班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班交班人");
            txb白班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "白班接班人");
            dtp白班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "白班交接班时间");
            txb夜班异常情况处理.DataBindings.Add("Text", bsHandOver.DataSource, "夜班异常情况处理");
            txb夜班交班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班交班人");
            txb夜班接班人.DataBindings.Add("Text", bsHandOver.DataSource, "夜班接班人");
            dtp夜班交接班时间.DataBindings.Add("Value", bsHandOver.DataSource, "夜班交接班时间");
        }
        private int getId()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = conOle;
            comm.CommandText = "select ID from " + tablename1 + " where 生产指令编号 ='" + txb生产指令编号.Text + "' and 生产日期 = #" + dtp生产日期.Value.ToShortDateString() + "#;";
            return (int)comm.ExecuteScalar();
        }
        private void binding2(int outerId, bool flag)
        {
            bsItem = new BindingSource();
            dtItem = new DataTable(tablename2);
            if (flag)
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where T吹膜岗位交接班记录ID = " + outerId, conOle);// +" where T吹膜岗位交接班记录ID in (select ID from " + tablename1 + " where 生产日期 = '" + dtp生产日期.Value.Date+"');",conOle);
            }
            else
            {
                daItem = new OleDbDataAdapter("select * from " + tablename2 + " where false", conOle);
            }

            cbItem = new OleDbCommandBuilder(daItem);
            daItem.Fill(dtItem);
            if (dtItem.Rows.Count < 1)  //dtItem is NULL
            {
                //DataRow newrow = dtItem.NewRow();
                //dtItem.Rows.Add(newrow);
                binding3();
                if (flag)
                {
                    dtItem.Rows[0]["T吹膜岗位交接班记录ID"] = outerId;
                }
            }
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
            
        }
        private void binding3()
        {
            bsSettingHandOver = new BindingSource();
            dtSettingHandOver = new DataTable(tablename3);
            daSettingHandOver = new OleDbDataAdapter("select * from " + tablename3, conOle);
            cbSettingHandOver = new OleDbCommandBuilder(daSettingHandOver);
            daSettingHandOver.Fill(dtSettingHandOver);
            bsSettingHandOver.DataSource = dtSettingHandOver;

            for (int i = 0; i < dtSettingHandOver.Rows.Count; i++)
            {
                DataRow newrow = dtItem.NewRow();
                newrow["确认项目"] = dtSettingHandOver.Rows[i]["确认项目"];
                newrow["序号"] = Convert.ToInt32(i + 1);
                dtItem.Rows.Add(newrow);
            }
        }

        private void pullData()
        {
            dtHandOver.Rows[0]["生产指令编号"] = txb生产指令编号.Text;
            dtHandOver.Rows[0]["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dtHandOver.Rows[0]["白班产品代码批号数量"] = txb白班产品代码批号数量.Text;
            dtHandOver.Rows[0]["夜班产品代码批号数量"] = txb夜班产品代码批号数量.Text;
            dtHandOver.Rows[0]["白班异常情况处理"] = txb白班异常情况处理.Text;
            dtHandOver.Rows[0]["白班交班人"] = txb白班交班人.Text;
            dtHandOver.Rows[0]["白班接班人"] = txb白班接班人.Text;
            dtHandOver.Rows[0]["白班交接班时间"] = Convert.ToDateTime(dtp白班交接班时间.Value.ToShortTimeString());
            dtHandOver.Rows[0]["夜班异常情况处理"] = txb夜班异常情况处理.Text;
            dtHandOver.Rows[0]["夜班交班人"] = txb夜班交班人.Text;
            dtHandOver.Rows[0]["夜班接班人"] = txb夜班接班人.Text;
            dtHandOver.Rows[0]["夜班交接班时间"] = Convert.ToDateTime(dtp夜班交接班时间.Value.ToShortTimeString());
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            pullData();
            

            bsHandOver.EndEdit();
            daHandOver.Update((DataTable)bsHandOver.DataSource);
            if (1 == status)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = conOle;
                comm.CommandText = "select @@identity";
                outerId = (int)comm.ExecuteScalar();
                dtItem.Rows[0]["T吹膜岗位交接班记录ID"] = Convert.ToInt32(outerId);
            }
            for (int i = 1; i < dtItem.Rows.Count; i++)
            {
                dtItem.Rows[i]["T吹膜岗位交接班记录ID"] = dtItem.Rows[0]["T吹膜岗位交接班记录ID"];
            }
            bsItem.EndEdit();
            daItem.Update((DataTable)bsItem.DataSource);
        }
    }
}
