using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    public partial class QueryInstruForm : BaseForm
    {
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        string writer;//编制人

        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;      
        
        public QueryInstruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Initdgv();

        }

        //dgv样式初始化
        private void Initdgv()
        {
            bs = new BindingSource();
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);

        }

        private void Bind(String tblName, String person, String time)
        {            
            dt = new DataTable(tblName); //""中的是表名
            da = new OleDbDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + time + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            this.dgv.DataBindings.Clear();
            this.dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();

            for (int i = 0; i < this.dgv.Columns.Count; i++)
            {
                string colname = this.dgv.Columns[i].Name;
                if (colname == "ID")
                {
                    this.dgv.Columns[colname].Visible = false;
                }
                else
                {
                    this.dgv.Columns[colname].Visible = true;
                }
            }
            this.dgv.Columns["序号"].MinimumWidth = 40;  //没用？？？？？            
            
        }

        private void setDataGridViewRowNums()
        {
            int coun = this.dgv.Rows.Count;
            for (int i = 0; i < coun; i++)
            {
                this.dgv.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请选择工序！");
                return;
            }
            date1 = dateTimePicker1.Value.Date;
            date2 = dateTimePicker2.Value.Date;
            writer = textBox1.Text.Trim();
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            String process = comboBox1.SelectedItem.ToString();
            switch (process)
            {
                case "吹膜":
                    Bind("生产指令信息表", " 编制人", "编制时间");        

                    break;
                case "清洁分切":
                    Bind("清洁分切工序生产指令", " 编制人", "编制时间");        

                    break;
                case "CS制袋":
                    Bind("生产指令", " 操作员", "操作时间");        

                    break;

            }

               
            

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            int selectIndex = this.dgv.CurrentRow.Index;
            int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
            String process = comboBox1.SelectedItem.ToString();
            switch (process)
            {
                case "吹膜":
                    BatchProductRecord.ProcessProductInstru detailform = new BatchProductRecord.ProcessProductInstru(base.mainform, ID);
                    detailform.Show();

                    break;
                case "清洁分切":                   

                    break;
                case "CS制袋":

                    break;

            }

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            String process = comboBox1.SelectedItem.ToString();
            switch (process)
            {
                case "吹膜":
                    Parameter.selectCon = 1;
                    Parameter.InitCon();

                    break;
                case "清洁分切":
                    Parameter.selectCon = 2;
                    Parameter.InitCon();

                    break;
                case "CS制袋":
                    Parameter.selectCon = 3;
                    Parameter.InitCon();

                    break;

            }
        }




    }
}
