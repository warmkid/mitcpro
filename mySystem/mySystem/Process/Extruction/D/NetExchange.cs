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

namespace mySystem.Process.Extruction.D
{
    public partial class NetExchange : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜机更换过滤网记录表";
        DataTable dtNetExchange;
        OleDbDataAdapter daNetExchange;
        BindingSource bsNetExchange;
        OleDbCommandBuilder cbNetExchange;
        bool isFirstBind = true; 

        public NetExchange(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            init();
            binding();
        }

        private void init()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            // 处理DataGridView中数据类型输错的情况
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetExchange_FormClosing);
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);

        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void binding()
        {
            dtNetExchange = new DataTable(tablename1);
            daNetExchange = new OleDbDataAdapter("select * from " + tablename1, conOle);
            bsNetExchange = new BindingSource();
            cbNetExchange = new OleDbCommandBuilder(daNetExchange);
            daNetExchange.Fill(dtNetExchange);
            bsNetExchange.DataSource = dtNetExchange;
            dataGridView1.DataSource = bsNetExchange.DataSource;
            dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Columns[1].Width = dataGridView1.Width-5;
        }
        private void btn保存_Click(object sender, EventArgs e)
        {
            bsNetExchange.EndEdit();
            daNetExchange.Update((DataTable)bsNetExchange.DataSource);
        }

        private void btn插入_Click(object sender, EventArgs e)
        {
            DataRow newDataRow = dtNetExchange.NewRow();
            dtNetExchange.Rows.Add(newDataRow);
        }
        
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind = false;
            }
        }

        private void NetExchange_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            writeDGVWidthToSetting(dataGridView1);
        }

    }
}
