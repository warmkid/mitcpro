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
        private SqlDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private SqlCommandBuilder cb;      
        
        public QueryInstruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Initdgv();

            textBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox1_PreviewKeyDown);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            comboBox1.PreviewKeyDown += new PreviewKeyDownEventHandler(comboBox1_PreviewKeyDown);
        }

        void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
        }

        void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchBtn.PerformClick();
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
            //if (writer != "")
            //{
            da = new SqlDataAdapter("select * from " + tblName + " where " + person + " like " + "'%" + writer + "%'" + " and " + time + " between " + "'" + date1 + "'" + " and " + "'" + date2.AddDays(1) + "' order by ID", mySystem.Parameter.conn);
            //}
            //else
            //{
            //    da = new SqlDataAdapter("select * from " + tblName + " where " + person + " is null and " + time + " between " + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "# order by ID", mySystem.Parameter.conn);
            //}
            cb = new SqlCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            this.dgv.DataBindings.Clear();
            this.dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();
            Utility.setDataGridViewAutoSizeMode(dgv);
        }

        private void Bind吹膜(String person, String time,bool mod)
        {
            String tblName = "生产指令信息表";
            dt = new DataTable(tblName); //""中的是表名
            String sql = "";
            if (mod)
            {
                sql ="select * from {0} where {1} like '%{2}%' and {3} between '{4}' and '{5}' and 状态=4 order by ID";
                
            }
            else
            {
                sql = "select * from {0} where {1} like '%{2}%' and {3} between '{4}' and '{5}' and 状态<>4 order by ID";
            }
            da = new SqlDataAdapter(String.Format(sql, tblName, person, writer, time, date1, date2.AddDays(1)), mySystem.Parameter.conn);
            
            cb = new SqlCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            this.dgv.DataBindings.Clear();
            this.dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();
            Utility.setDataGridViewAutoSizeMode(dgv);
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
            search();
        }

        public void search()
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
                    //Bind("生产指令信息表", " 编制人", "编制时间");        
                    Bind吹膜("编制人", "编制时间", cb吹膜更改.Checked);
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
            if (dgv.Rows.Count > 0)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
        }

        //双击弹出界面
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    int selectIndex = this.dgv.CurrentRow.Index;
                    int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
                    bool b;
                    switch (processName)
                    {
                        case "吹膜":
                            b = mySystem.ExtructionMainForm.checkUser(Parameter.userName, Parameter.userRole, "生产指令信息表");
                            if (b)
                            {
                                BatchProductRecord.ProcessProductInstru form1 = new BatchProductRecord.ProcessProductInstru(base.mainform, ID);
                                form1.Owner = this;
                                form1.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("您无权查看该页面！");
                                return;
                            }

                            
                            break;
                        case "清洁分切":
                            mySystem.Process.CleanCut.Instru form2 = new Process.CleanCut.Instru(mainform, ID);
                            form2.ShowDialog();

                            break;
                        case "CS制袋":
                            mySystem.Process.Bag.CS.CS制袋生产指令 form3 = new Process.Bag.CS.CS制袋生产指令(mainform, ID);
                            form3.ShowDialog();

                            break;
                        case "PE制袋":
                            mySystem.Process.Bag.LDPE.LDPEBag_productioninstruction form4 = new Process.Bag.LDPE.LDPEBag_productioninstruction(mainform, ID);
                            form4.ShowDialog();

                            break;
                        case "BPV制袋":
                            mySystem.Process.Bag.BTV.BPV制袋生产指令 form5 = new Process.Bag.BTV.BPV制袋生产指令(mainform, ID);
                            form5.ShowDialog();
                            break;
                        case "PTV制袋":
                            mySystem.Process.Bag.PTV.PTVBag_productioninstruction form6 = new Process.Bag.PTV.PTVBag_productioninstruction(mainform, ID);
                            form6.ShowDialog();

                            break;

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + "\n" + ee.StackTrace);
                }
            }
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
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 0);
                }
                else if (dgv.Rows[i].Cells[checker].Value.ToString() == "")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
            }
        }

        private void btn复制_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count == 0) return;
            // TODO 复制内表
            String newCode = mySystem.Other.InputTextWindow.getString("新的生产指令编码：");
            if (newCode == "")
            {
                return;
            }


            
            DataRow dr;
            SqlDataAdapter daT;
            DataTable dtT;
            SqlCommandBuilder cbT;
            
           
            
            



            switch (processName)
            {
                case "吹膜":
                    // 检查指令
                    daT = new SqlDataAdapter("select * from 生产指令信息表 where 生产指令编号='" + newCode+"'", mySystem.Parameter.conn);
                    cbT = new SqlCommandBuilder(daT);
                    dtT = new DataTable();
                    daT.Fill(dtT);
                    if (dtT.Rows.Count > 0)
                    {
                        MessageBox.Show("吹膜中已经有了生产指令:" + newCode);
                        return;
                    }
                    // 复制外表
                    int pid = Convert.ToInt32(dgv.SelectedRows[0].Cells["ID"].Value);
                    dr = dt.NewRow();
                    dr.ItemArray = dt.Rows[dgv.SelectedCells[0].RowIndex].ItemArray.Clone() as object[];
                    dr["生产指令编号"] = newCode;
                    dr["审批人"] = "";
                    dr["状态"] = 0;
                    string log = "=====================================\n";
                    log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName +
                        " 复制生产指令：" + dt.Rows[dgv.SelectedCells[0].RowIndex]["生产指令编号"].ToString() + "\n";
                    dr["日志"] = log;
                    dt.Rows.Add(dr);
                    da.Update((DataTable)bs.DataSource);
                    
                    // 更新界面
                    da = new SqlDataAdapter(da.SelectCommand);
                    cb = new SqlCommandBuilder(da);
                    dt = new DataTable(dt.TableName);
                    bs = new BindingSource();
                    dt.Columns.Add("序号", System.Type.GetType("System.String"));
                    da.Fill(dt);
                    bs.DataSource = dt;
                    dgv.DataSource = bs.DataSource;
                    setDataGridViewRowNums();

                    // 获取原内表数据
                    daT = new SqlDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + pid, mySystem.Parameter.conn);
                    cbT = new SqlCommandBuilder(daT);
                    dtT = new DataTable();
                    daT.Fill(dtT);
                    DataTable dtTOrig = dtT.Copy();


                    daT = new SqlDataAdapter("select * from 生产指令信息表 where 生产指令编号='" + newCode+"'", mySystem.Parameter.conn);
                    cbT = new SqlCommandBuilder(daT);
                    dtT = new DataTable();
                    daT.Fill(dtT);
                    int newid = Convert.ToInt32(dtT.Rows[0]["ID"]);
                    daT = new SqlDataAdapter("select * from 生产指令产品列表 where 生产指令ID=" + newid, mySystem.Parameter.conn);
                    cbT = new SqlCommandBuilder(daT);
                    dtT = new DataTable();
                    daT.Fill(dtT);
                    List<DataRow> ndrs = new List<DataRow>();
                    foreach (DataRow tdr in dtTOrig.Rows)
                    {
                        DataRow t = dtT.NewRow();
                        t.ItemArray = tdr.ItemArray.Clone() as object[];
                        t["生产指令ID"] = newid;
                        ndrs.Add(t);
                    }
                    foreach (DataRow tdr in ndrs)
                    {
                        dtT.Rows.Add(tdr);
                    }
                    daT.Update(dtT);
                    break;
                case "清洁分切":
                    

                    break;
                case "CS制袋":
                    

                    break;
                case "PE制袋":
                    

                    break;
                case "BPV制袋":
                    

                    break;
                case "PTV制袋":

                    break;
            }
            
 
        }


    }
}
