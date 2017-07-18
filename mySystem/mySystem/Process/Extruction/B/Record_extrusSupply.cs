using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusSupply : mySystem.BaseForm
    {
        SqlConnection conn = null;//连接sql
        OleDbConnection connOle = null;//连接access
        bool isSqlOk;//使用sql还是access
        mySystem.CheckForm checkform;

        private int label_prodcode;//0代表往里填列表项
        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        private Dictionary<string, string> dict_procode_batch;
        private Dictionary<string, string> dict_inoutmatcode_batch;
        private Dictionary<string, string> dict_midmatcode_batch;

        private void init()
        {
            dict_inoutmatcode_batch = new Dictionary<string, string>();
            dict_midmatcode_batch = new Dictionary<string, string>();
            dict_procode_batch = new Dictionary<string, string>();

            dt_prodinstr = new System.Data.DataTable();
            bs_prodinstr = new System.Windows.Forms.BindingSource();
            da_prodinstr = new OleDbDataAdapter();
            cb_prodinstr = new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist = new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();


            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

        }
        //表格错误处理
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        public Record_extrusSupply(mySystem.MainForm mainform):base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            init();
            label_prodcode = 0;
            addprodcode(mySystem.Parameter.proInstruID);
            addmatcode(mySystem.Parameter.proInstruID);
            label_prodcode = 1;
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            cb产品代码.Enabled = true;
            dtp供料日期.Enabled = true;
        }

        public Record_extrusSupply(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            conn = mainform.conn;
            connOle = mainform.connOle;
            isSqlOk = mainform.isSqlOk;

            InitializeComponent();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;

            init();
            label_prodcode = 0;
            //addprodcode(mySystem.Parameter.proInstruID);
            //addmatcode(mySystem.Parameter.proInstruID);
            label_prodcode = 1;

            string asql = "select * from 吹膜供料记录 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            int instrid = int.Parse(tempdt.Rows[0]["生产指令ID"].ToString());
            string prodcode = tempdt.Rows[0]["产品代码"].ToString();
            DateTime time = (DateTime)tempdt.Rows[0]["供料日期"];
            bool flight = (bool)tempdt.Rows[0]["班次"];

            readOuterData(instrid, prodcode, time, flight);
            removeOuterBinding();
            outerBind();

            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

        }

        //根据id填表
        public void show(int paraid)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 吹膜供料记录 where ID=" + paraid ;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return;
            }
            else
            {
                da.Dispose();
                //填表
                cb产品代码.Text = (string)tempdt.Rows[0][2];
                tb产品批号.Text = (string)tempdt.Rows[0][3];
                tb生产指令.Text = (string)tempdt.Rows[0][4];
                cb原料代码ab1c.Text = (string)tempdt.Rows[0][5];
                cb原料代码b2.Text = (string)tempdt.Rows[0][6];
                tb原料批号ab1c.Text = (string)tempdt.Rows[0][7];
                tb原料批号b2.Text = (string)tempdt.Rows[0][8];
                tb用料ab1c.Text = tempdt.Rows[0][9].ToString();
                tb余料ab1c.Text = tempdt.Rows[0][10].ToString();
                tb用料b2.Text = tempdt.Rows[0][11].ToString();
                tb余料b2.Text = tempdt.Rows[0][12].ToString();

                //填写dategridview
                OleDbCommand comm2 = new OleDbCommand();
                comm2.Connection = mySystem.Parameter.connOle;
                comm2.CommandText = "select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + paraid;

                OleDbDataAdapter da2 = new OleDbDataAdapter(comm);
                DataTable tempdt2 = new DataTable();
                da2.Fill(tempdt2);
                if (tempdt2.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    while (dataGridView1.Rows.Count != 0)
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
                    for (int i = 0; i < tempdt2.Rows.Count; i++)
                    {
                        DataGridViewRow dr = new DataGridViewRow();
                        foreach (DataGridViewColumn c in dataGridView1.Columns)
                        {
                            dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                        }
                        dr.Cells[0].Value = (DateTime)tempdt2.Rows[i][2];
                        dr.Cells[1].Value = (float)tempdt2.Rows[i][3];
                        dr.Cells[2].Value = (float)tempdt2.Rows[i][4];
                        dr.Cells[3].Value = (float)tempdt2.Rows[i][5];
                        dr.Cells[4].Value = (bool)tempdt2.Rows[i][6];
                        dr.Cells[5].Value = (string)tempdt2.Rows[i][7];
                        dataGridView1.Rows.Add(dr);
                    }
                }
            }
            
        }

        //添加供料
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            dr = writeInnerDefault(dr);
            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        //删除一条供料记录
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Index < 0)
                    return;
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
            //刷新合计
            float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
                    sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
                    sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
                    sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        //审核信息
        public override void CheckResult()
        {
            base.CheckResult();
            dt_prodinstr.Rows[0]["审核人"] = checkform.userName;//审核人
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            if (checkform.ischeckOk)//审核通过
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                dataGridView1.Enabled = true;
                dataGridView1.ReadOnly = true;
                bt打印.Enabled = true;
                cb产品代码.Enabled = true;
            }
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

        }

        //审核按钮
        private void button4_Click(object sender, EventArgs e)
        {
            checkform = new mySystem.CheckForm(this);
            checkform.Show();
        }

        //读取该生产指令下所有的产品代码，加入 生产代码的 items
        private void addprodcode(int instrid)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select 产品编码,产品批号 from 生产指令产品列表 where 生产指令ID=" + instrid ;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                if (tempdt.Rows[i]["产品编码"] != null && tempdt.Rows[i]["产品批号"] != null)
                {
                    cb产品代码.Items.Add((string)tempdt.Rows[i]["产品编码"]);
                    dict_procode_batch.Add((string)tempdt.Rows[i]["产品编码"], (string)tempdt.Rows[i]["产品批号"]);
                }
            }
            da.Dispose();
            tempdt.Dispose();
            comm.Dispose();
        }

        //读取生产指令下物料代码，并保存
        private void addmatcode(int instrid)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select 内外层物料代码,内外层物料批号,中层物料代码,中层物料批号 from 生产指令信息表 where ID=" + instrid;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            for (int i = 0; i < tempdt.Rows.Count; i++)
            {
                if (tempdt.Rows[i]["内外层物料代码"] != null && tempdt.Rows[i]["内外层物料批号"] != null)
                {
                    cb原料代码ab1c.Items.Add(Convert.ToString(tempdt.Rows[i]["内外层物料代码"]));
                    dict_inoutmatcode_batch.Add(Convert.ToString(tempdt.Rows[i]["内外层物料代码"]), Convert.ToString(tempdt.Rows[i]["内外层物料批号"]));
                }
                if (tempdt.Rows[i]["中层物料代码"] != null && tempdt.Rows[i]["中层物料批号"] != null)
                {
                    cb原料代码b2.Items.Add(Convert.ToString(tempdt.Rows[i]["中层物料代码"]));
                    dict_midmatcode_batch.Add(Convert.ToString(tempdt.Rows[i]["中层物料代码"]), Convert.ToString(tempdt.Rows[i]["中层物料批号"])); 
 
                }
                       
            }
            da.Dispose();
            tempdt.Dispose();
            comm.Dispose();
        }

        //AB1C用料输入
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (label_prodcode == 1)
            {
                if (tb用料ab1c.Text == "")
                    return;
                float a;
                if (!float.TryParse(tb用料ab1c.Text, out a))
                {
                    MessageBox.Show("用料量必须为数字");
                    return;
                }
                dt_prodinstr.Rows[0]["外中内层原料用量"] = a;
                dt_prodinstr.Rows[0]["外中内层原料余量"] = float.Parse(dt_prodinstr.Rows[0]["外层供料量合计a"].ToString()) + float.Parse(dt_prodinstr.Rows[0]["中内层供料量合计b"].ToString()) - a;
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }

        }

        //B2用料输入
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (label_prodcode == 1)
            {
                if (tb用料b2.Text == "")
                    return;
                float a;
                if (!float.TryParse(tb用料b2.Text, out a))
                {
                    MessageBox.Show("用料量必须为数字");
                    return;
                }
                dt_prodinstr.Rows[0]["中层原料用量"] = a;
                dt_prodinstr.Rows[0]["中层原料余量"] = float.Parse(dt_prodinstr.Rows[0]["中层供料量合计c"].ToString()) - a;
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            }
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["产品代码"] = cb产品代码.Text;
            dr["产品批号"] = dict_procode_batch[cb产品代码.Text];
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;
            dr["外中内层原料代码"] = cb原料代码ab1c.Text;
            dr["中层原料代码"] = cb原料代码b2.Text;
            dr["外中内层原料批号"] = tb原料批号ab1c.Text;
            dr["中层原料批号"] = tb原料批号b2.Text;
            dr["外中内层原料用量"] = 0;
            dr["外中内层原料余量"] = 0;
            dr["中层原料用量"] = 0;
            dr["中层原料余量"] = 0;
            dr["外层供料量合计a"] = 0;
            dr["中内层供料量合计b"] = 0;
            dr["中层供料量合计c"] = 0;
            dr["供料日期"] = DateTime.Parse(dtp供料日期.Value.ToShortDateString());
            dr["班次"] = mySystem.Parameter.userflight == "白班";
            ckb白班.Checked = (bool)dr["班次"] ;
            ckb夜班.Checked = !ckb白班.Checked;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T吹膜供料记录ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["供料时间"] = DateTime.Now;
            dr["外层供料量"] = 0;
            dr["中内层供料量"] = 0;
            dr["中层供料量"] = 0;
            dr["原料抽查结果"] = "合格";
            dr["供料人"] =mySystem.Parameter.userName;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid, string prodcode,DateTime time,bool flight)
        {
            dt_prodinstr = new DataTable("吹膜供料记录");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 吹膜供料记录 where 生产指令ID=" + instrid + " and 产品代码='" + prodcode + "' and 供料日期=#"+time+"# and 班次="+flight, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("吹膜供料记录详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            dtp供料日期.DataBindings.Clear();
            //ckb白班.DataBindings.Clear();
            cb产品代码.DataBindings.Clear();
            tb产品批号.DataBindings.Clear();
            tb生产指令.DataBindings.Clear();
            cb原料代码ab1c.DataBindings.Clear();
            cb原料代码b2.DataBindings.Clear();
            tb原料批号ab1c.DataBindings.Clear();
            tb原料批号b2.DataBindings.Clear();
            tb外层合计.DataBindings.Clear();
            //tb中内层合计.DataBindings.Clear();
            tb中内层合计.DataBindings.Clear();
            tb用料ab1c.DataBindings.Clear();
            tb余料ab1c.DataBindings.Clear();
            tb用料b2.DataBindings.Clear();
            tb余料b2.DataBindings.Clear();
            tb复核人.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            dtp供料日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "供料日期");
            //ckb白班.DataBindings.Add("Checked", bs_prodinstr.DataSource, "班次");
            cb产品代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品代码");
            tb产品批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品批号");
            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            cb原料代码ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料代码");
            cb原料代码b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料代码");
            tb原料批号ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料批号");
            tb原料批号b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料批号");
            tb用料ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料用量");
            tb余料ab1c.DataBindings.Add("Text", bs_prodinstr.DataSource, "外中内层原料余量");
            tb用料b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料用量");
            tb余料b2.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层原料余量");
            tb外层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "外层供料量合计a");
            tb中内层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "中内层供料量合计b");
            //tb内层合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层供料量合计c");
            tb复核人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审核人");

        }
        // 内表和控件的绑定
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);

            setDataGridViewCombox();
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
        }

        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
            {
                switch (dc.ColumnName)
                {
                    //case "外层供料量":
                    //    DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                    //    c3.DataPropertyName = dc.ColumnName;
                    //    c3.HeaderText = dc.ColumnName;
                    //    c3.Name = cb原料代码ab1c.Text;
                    //    c3.SortMode = DataGridViewColumnSortMode.Automatic;
                    //    c3.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(c3);
                    //    break;

                    //case "中内层供料量":
                    //    DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                    //    c4.DataPropertyName = dc.ColumnName;
                    //    c4.HeaderText =cb原料代码b2.Text;
                    //    c4.Name =  dc.ColumnName;
                    //    c4.SortMode = DataGridViewColumnSortMode.Automatic;
                    //    c4.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(c4);
                    //    break;
                    case "原料抽查结果":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = dc.ColumnName;
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.Automatic;
                        c1.ValueType = dc.DataType;

                        c1.Items.Add("合格");
                        c1.Items.Add("不合格");
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }

        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;//ID
            dataGridView1.Columns[1].Visible = false;//T吹膜供料记录ID
            dataGridView1.Columns[5].Visible = false;//中内层供料量
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性
            //ab1c原料批号和b2原料批号
            string s = tb原料批号ab1c.Text;
            string[] s1 = s.Split(',');     
            for (int i = 0; i < s1.Length; i++)
            {
                if(!dict_inoutmatcode_batch.ContainsValue(s1[i]))
                {
                    MessageBox.Show(cb原料代码ab1c.Text+"中没有对应的批号:"+s1[i]);
                    return false;
                }
            }
            s = tb原料批号b2.Text;
            s1 = s.Split(',');
            for (int i = 0; i < s1.Length; i++)
            {
                if (!dict_midmatcode_batch.ContainsValue(s1[i]))
                {
                    MessageBox.Show(cb原料代码b2.Text+"中没有对应的批号:"+s1[i]);
                    return false;
                }
            }
            return true;
        }

        private void cb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_prodcode = 0;
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            bt审核.Enabled = false;
            bt打印.Enabled = false;
            ckb白班.Enabled = false;
            ckb夜班.Enabled = false;

            readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text,DateTime.Parse(dtp供料日期.Value.ToShortDateString()),mySystem.Parameter.userflight=="白班");
            removeOuterBinding();
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, DateTime.Parse(dtp供料日期.Value.ToShortDateString()), mySystem.Parameter.userflight == "白班");
                removeOuterBinding();
                outerBind();
            }
            ckb白班.Checked = (bool)dt_prodinstr.Rows[0]["班次"];
            ckb夜班.Checked = !ckb白班.Checked;

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                cb产品代码.Enabled = true;
                bt打印.Enabled = true;
            }

            
            label_prodcode = 1;

        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            //判断合法性
            if (!input_Judge())
                return;

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(mySystem.Parameter.proInstruID, cb产品代码.Text, DateTime.Parse(dtp供料日期.Value.ToShortDateString()), mySystem.Parameter.userflight == "白班");
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            bt审核.Enabled = true;
        }

        private void cb原料代码ab1c_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt_prodinstr.Rows[0]["外中内层原料代码"] = cb原料代码ab1c.Text;
            //dt_prodinstr.Rows[0]["外中内层原料批号"] = dict_inoutmatcode_batch[cb原料代码ab1c.Text];
            if (cb原料代码ab1c.Text != "")
            {
                cb原料代码ab1c.Enabled = false;
                label9.Text = cb原料代码ab1c.Text;
                label4.Text = cb原料代码ab1c.Text;
            }
        }

        private void cb原料代码b2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt_prodinstr.Rows[0]["中层原料代码"] = cb原料代码b2.Text;
            //dt_prodinstr.Rows[0]["中层原料批号"] = dict_midmatcode_batch[cb原料代码b2.Text];
            if (cb原料代码b2.Text != "")
            {
                cb原料代码b2.Enabled = false;
                label11.Text = cb原料代码b2.Text;
                label5.Text = cb原料代码b2.Text;
            }
        }

        //datagridview单元格编辑结束
        private void dataGridView1_Endedit(object sender, DataGridViewCellEventArgs e)
        {
            //计算合计
            float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
                    sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
                    sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
                    sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;
            //供料人是否合法
            if (e.ColumnIndex == 7)
            {
                if(queryid(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString())==-1)
                {
                    MessageBox.Show("供料人id不存在");
                    dataGridView1.Rows[e.RowIndex].Cells[7].Value = "";
                }
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            
        }

        //检查输入人是否合法
        private int queryid(string s)
        {
            string asql = "select ID from users where 姓名=" + "'" + s + "'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOleUser);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
                return -1;
            else
                return (int)tempdt.Rows[0][0];

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index - 1].Selected = true;

            ////计算合计
            //float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
            //        sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
            //        sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
            //        sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            //}
            //dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            //dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            //dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;


        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count <= 0)
                return;
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dt_prodlist.Rows[index];
            DataRow desRow = dt_prodlist.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_prodlist.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_prodlist.Rows[i];
                DataRow tdesRow = dt_prodlist.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_prodlist.Rows.Add(tdesRow);
            }
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            dt_prodlist.Clear();
            da_prodlist.Fill(dt_prodlist);
            dataGridView1.ClearSelection();
            dataGridView1.Rows[index + 1].Selected = true;

            ////计算合计
            //float sum_out = 0, sum_inmid = 0, sum_mid = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")//外层
            //        sum_out += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")//中内层
            //        sum_inmid += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            //    if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")//内层
            //        sum_mid += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            //}
            //dt_prodinstr.Rows[0]["外层供料量合计a"] = sum_out;
            //dt_prodinstr.Rows[0]["中内层供料量合计b"] = sum_inmid;
            //dt_prodinstr.Rows[0]["中层供料量合计c"] = sum_mid;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Record_extrusSupply s=new Record_extrusSupply(mainform, 5);           
            s.Show();
        }

        private void bt打印_Click(object sender, EventArgs e)
        {
            print(true);
        }

        public void print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/B/SOP-MFG-301-R06 吹膜供料记录.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            fill_excel(my);

            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                // 直接用默认打印机打印该Sheet
                my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                // 关闭文件，false表示不保存
                wb.Close(false);
                // 关闭Excel进程
                oXL.Quit();
                // 释放COM资源
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(oXL);
            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[3, 1].Value = "产品代码: "+cb产品代码.Text;
            my.Cells[3, 5].Value = "产品批号: " + tb产品批号.Text ;
            my.Cells[3, 7].Value = "生产指令编号: " + tb生产指令.Text ;
            my.Cells[5, 6].Value = cb原料代码ab1c.Text;
            my.Cells[5, 8].Value = tb原料批号ab1c.Text;
            my.Cells[6, 6].Value = cb原料代码b2.Text;
            my.Cells[6, 8].Value = tb原料批号b2.Text;

            my.Cells[7, 1].Value = "供料日期时间: "+dtp供料日期.Value.ToLongDateString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime tempdt=DateTime.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                string time = string.Format("{0}:{1}:{2}", tempdt.Hour.ToString(), tempdt.Minute.ToString(), tempdt.Second.ToString());
                my.Cells[9 + i, 1] = time;
                my.Cells[9 + i, 2] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                my.Cells[9 + i, 3] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                my.Cells[9 + i, 5] = dataGridView1.Rows[i].Cells[6].Value.ToString();
                my.Cells[9 + i, 6] = dataGridView1.Rows[i].Cells[7].Value.ToString();
            }

            my.Cells[9, 8].Value = tb用料ab1c.Text;
            my.Cells[9, 9].Value = tb余料ab1c.Text;

            my.Cells[10, 8].Value = tb用料b2.Text;
            my.Cells[10, 9].Value = tb余料b2.Text;
            my.Cells[9, 10].Value = tb复核人.Text;
            my.Cells[13, 2].Value = tb外层合计.Text ;
            my.Cells[13, 3].Value = tb中内层合计.Text;
        }

    }
}
