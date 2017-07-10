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

namespace mySystem.Setting
{
    public partial class Setting_CleanSite : mySystem.BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private OleDbDataAdapter da1;
        private DataTable dt1;
        private BindingSource bs1;
        private OleDbCommandBuilder cb1;
        private OleDbDataAdapter da2;
        private DataTable dt2;
        private BindingSource bs2;
        private OleDbCommandBuilder cb2;


        public Setting_CleanSite(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            Init();
            Bind();

        }

        //dgv样式初始化
        private void Init()
        {
            bs1 = new BindingSource();
            dgv供料.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv供料.AllowUserToAddRows = false;
            dgv供料.ReadOnly = false;
            dgv供料.RowHeadersVisible = false;
            dgv供料.AllowUserToResizeColumns = true;
            dgv供料.AllowUserToResizeRows = false;
            dgv供料.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv供料.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv供料.Font = new Font("宋体", 12);
            this.dgv供料.DataError += this.dgv供料_DataError;

            bs2 = new BindingSource();
            dgv吹膜.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv吹膜.AllowUserToAddRows = false;
            dgv吹膜.ReadOnly = false;
            dgv吹膜.RowHeadersVisible = false;
            dgv吹膜.AllowUserToResizeColumns = true;
            dgv吹膜.AllowUserToResizeRows = false;
            dgv吹膜.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv吹膜.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv吹膜.Font = new Font("宋体", 12);
            this.dgv吹膜.DataError += this.dgv吹膜_DataError;
        }

        private void dgv供料_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void dgv吹膜_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void Bind()
        {
            //****************供料******************
            dt1 = new DataTable("设置供料工序清场项目"); //""中的是表名
            da1 = new OleDbDataAdapter("select * from 设置供料工序清场项目", connOle);
            cb1 = new OleDbCommandBuilder(da1);

            dt1.Columns.Add("序号", System.Type.GetType("System.String"));
            da1.Fill(dt1);
            bs1.DataSource = dt1;
            this.dgv供料.DataSource = bs1.DataSource;

            //显示序号
            numFresh(this.dgv供料);

            this.dgv供料.Columns["清场内容"].MinimumWidth = 200;
            this.dgv供料.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv供料.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv供料.Columns["ID"].Visible = false;
            this.dgv供料.Columns["序号"].Visible = true;

            //****************吹膜******************
            dt2 = new DataTable("设置吹膜工序清场项目"); //""中的是表名
            da2 = new OleDbDataAdapter("select * from 设置吹膜工序清场项目", connOle);
            cb2 = new OleDbCommandBuilder(da2);

            dt2.Columns.Add("序号", System.Type.GetType("System.String"));
            da2.Fill(dt2);
            bs2.DataSource = dt2;
            this.dgv吹膜.DataSource = bs2.DataSource;

            //显示序号
            numFresh(this.dgv吹膜);

            this.dgv吹膜.Columns["清场内容"].MinimumWidth = 200;
            this.dgv吹膜.Columns["清场内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv吹膜.Columns["清场内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv吹膜.Columns["ID"].Visible = false;
            this.dgv吹膜.Columns["序号"].Visible = true;
        }


        private void numFresh(DataGridView dgv )
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }


        private void add供料_Click(object sender, EventArgs e)
        {
            DataRow dr = dt1.NewRow();
            dt1.Rows.InsertAt(dt1.NewRow(), dt1.Rows.Count);
            numFresh(this.dgv供料);
        }

        private void del供料_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv供料.CurrentRow.Index;
            this.dgv供料.Rows.RemoveAt(deletenum);
            numFresh(this.dgv供料);
        }

        private void add吹膜_Click(object sender, EventArgs e)
        {
            DataRow dr = dt2.NewRow();
            dt2.Rows.InsertAt(dt2.NewRow(), dt2.Rows.Count);
            numFresh(this.dgv吹膜);
        }

        private void del吹膜_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv吹膜.CurrentRow.Index;
            this.dgv吹膜.Rows.RemoveAt(deletenum);
            numFresh(this.dgv吹膜);
        }



        public void DataSave()
        {
            if (isSqlOk)
            { }
            else
            {
                da1.Update((DataTable)bs1.DataSource);
                dt1.Clear();
                da1.Fill(dt1);
                numFresh(this.dgv供料);

                da2.Update((DataTable)bs2.DataSource);
                dt2.Clear();
                da2.Fill(dt2);
                numFresh(this.dgv吹膜);
            }
        }
        
    }
}
