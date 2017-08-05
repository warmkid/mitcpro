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
        string processName;//工序名
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

        //根据不同工序连接不同数据库
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            processName = comboBox1.SelectedItem.ToString();
            switch (processName)
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
                case "PE制袋":
                    Parameter.selectCon = 7;
                    Parameter.InitCon();

                    break;
                case "BPV制袋":
                    Parameter.selectCon = 6;
                    Parameter.InitCon();

                    break;
                case "PTV制袋":
                    Parameter.selectCon = 8;
                    Parameter.InitCon();

                    break;

            }
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

            switch (processName)
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
                case "PE制袋":
                    Bind("生产指令", " 操作员", "操作时间");

                    break;
                case "BPV制袋":
                    Bind("生产指令", " 操作员", "操作时间");

                    break;
                case "PTV制袋":

                    Bind("生产指令", " 操作员", "操作时间");

                    break;

            }
 
        }

        //双击弹出界面
        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int selectIndex = this.dgv.CurrentRow.Index;
                int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                switch (processName)
                {
                    case "吹膜":
                        BatchProductRecord.ProcessProductInstru form1 = new BatchProductRecord.ProcessProductInstru(base.mainform, ID);
                        form1.Show();

                        break;
                    case "清洁分切":
                        mySystem.Process.CleanCut.Instru form2 = new Process.CleanCut.Instru(mainform, ID);
                        form2.Show();

                        break;
                    case "CS制袋":
                        mySystem.Process.Bag.CS.CS制袋生产指令 form3 = new Process.Bag.CS.CS制袋生产指令(ID);
                        form3.Show();

                        break;
                    case "PE制袋":
                        //mySystem.Process.Bag.LDPE.LDPEBag_productioninstruction form4 = new Process.Bag.LDPE.LDPEBag_productioninstruction(mainform, ID);
                        //form4.Show();

                        break;
                    case "BPV制袋":
                        //mySystem.Process.Bag.BTV.BTVProInstruction form5 = new Process.Bag.BTV.BTVProInstruction(mainform, ID);
                        //form5.Show();

                        break;
                    case "PTV制袋":
                        //mySystem.Process.Bag.PTV.PTVBag_productioninstruction form6 = new Process.Bag.PTV.PTVBag_productioninstruction(mainform, ID);
                        //form6.Show();

                        break;

                }
            }
            catch
            { }
            
        }
       

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv.Columns["ID"].Visible = false;
            //设置列宽
            for (int i = 0; i < this.dgv.Columns.Count; i++)
            {
                String colName = this.dgv.Columns[i].HeaderText;
                int strlen = colName.Length;
                this.dgv.Columns[i].MinimumWidth = strlen * 25;
            }  

            //待审核标红
            try
            { setDataGridViewBackColor("审核员"); }
            catch
            { }
            try
            { setDataGridViewBackColor("审核人"); }
            catch
            { }
            if (processName == "吹膜")
                setDataGridViewBackColor("审批人");
        }

        //设置datagridview背景颜色，待审核标红
        private void setDataGridViewBackColor(String checker)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[checker].Value.ToString() == "__待审核")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }


    }
}
