using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class Setting_CheckBeforePower : BaseForm
    {
        //setting_checkbeforepower
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;

        public Setting_CheckBeforePower(mySystem.MainForm mainform) : base(mainform)
        {
            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            InitializeComponent();
            Init();
            Bind();
        }

        //dgv样式初始化
        private void Init()
        {
            bs = new BindingSource();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Font = new Font("宋体", 12);
                    
            this.dataGridView1.DataError += this.dataGridView1_DataError;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private void Bind()
        {
            dt = new DataTable("设置吹膜机组开机前确认项目"); //""中的是表名
            da = new OleDbDataAdapter("select * from 设置吹膜机组开机前确认项目", connOle);
            cb = new OleDbCommandBuilder(da);

            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            this.dataGridView1.DataSource = bs.DataSource;

            //显示序号
            numFresh();

            this.dataGridView1.Columns["确认项目"].MinimumWidth = 160;
            this.dataGridView1.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.Columns["ID"].Visible = false;
        }

        //在最后增加一个空白行
        private void add_Click(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();            
            dt.Rows.InsertAt(dt.NewRow(), dt.Rows.Count);
            numFresh();
        }

        private void del_Click(object sender, EventArgs e)
        {
            int deletenum = dataGridView1.CurrentRow.Index; 
            this.dataGridView1.Rows.RemoveAt(deletenum);

            numFresh();
        }

        private void numFresh()
        {
            int coun = this.dataGridView1.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString(); 
            }
        }

        public void DataSave()
        {
            if (isSqlOk)
            { }
            else
            {
                da.Update((DataTable)bs.DataSource);
                dt.Clear();
                da.Fill(dt);
                numFresh();
                
            }
            
        }

        

    }
}
