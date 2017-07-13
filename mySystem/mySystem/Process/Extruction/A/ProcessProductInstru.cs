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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using WindowsFormsApplication1;

namespace BatchProductRecord
{
    public partial class ProcessProductInstru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;
        DataTable dt=null;//存储从产品表中读到的产品代码
        string last_code;//最后一次保存数据库时生产指令编码,默认该表内不会重复
        float leng;
        float sumweight;

        //新的版本
        int index = 0;//判断是插入模式还是更新模式，0代表插入，1代表更新
        int id;
        int label = 0;//1代表插入成功
        OleDbConnection connOle;
        private DataTable dt_prodinstr,dt_prodlist;
        private OleDbDataAdapter da_prodinstr,da_prodlist;
        private BindingSource bs_prodinstr,bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr,cb_prodlist;

        //读取产品列表填入产品名称下拉列表
        private void fill_prodname()
        {
            DataTable dt = new System.Data.DataTable();
            OleDbDataAdapter da= new OleDbDataAdapter("select 产品名称 from 生产指令信息表", mySystem.Parameter.connOle);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(dt);
            da.Dispose();
            cb.Dispose();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i][0].ToString());
            }
        }


        public ProcessProductInstru(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            init();

            fill_prodname();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb指令编号.Enabled = true;
            button5.Enabled = true;
          
        }
        private void init()
        {
            connOle = mySystem.Parameter.connOle;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr=new System.Data.DataTable();
            bs_prodinstr=new System.Windows.Forms.BindingSource();
            da_prodinstr=new OleDbDataAdapter();
            cb_prodinstr=new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist=new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();

        }

        private void bind_bs_contr()    
        {
            //textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品名称");
            comboBox1.DataBindings.Add("Text",bs_prodinstr.DataSource,"产品名称");
            tb指令编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            tb工艺.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产工艺");
            tb设备编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产设备编号");
            dtp开始生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "开始生产日期");

            tb内外层物料代码.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料代码");
            tb内外层物料批号.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料批号");
            textBox19.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层包装规格");
            tb内外领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层领料量");
            textBox16.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料代码");
            textBox18.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料批号");
            textBox20.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层包装规格");
            tb中层领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层领料量");
            textBox12.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管");
            textBox13.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管规格");
            tb卷心管领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管领料量");
            textBox9.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装包装规格");
            tb双层包装领料量.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装领料量");
            tb负责人.DataBindings.Add("Text", bs_prodinstr.DataSource, "负责人");

            textBox24.DataBindings.Add("Text", bs_prodinstr.DataSource, "编制人");
            textBox25.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批人");
            textBox26.DataBindings.Add("Text", bs_prodinstr.DataSource, "接收人");
            dateTimePicker2.DataBindings.Add("Value", bs_prodinstr.DataSource, "编制时间");
            dateTimePicker3.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批时间");
            dateTimePicker4.DataBindings.Add("Value", bs_prodinstr.DataSource, "接收时间");

            textBox6.DataBindings.Add("Text", bs_prodinstr.DataSource, "计划产量合计米");
            tb用料重量合计.DataBindings.Add("Text", bs_prodinstr.DataSource, "用料重量合计");
            textBox10.DataBindings.Add("Text", bs_prodinstr.DataSource, "计划产量合计卷");
        }
        
        //根据筛选条件得到指令id,没有返回-1
        private int getid(string instrcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 生产指令信息表 where 生产指令编号='" + instrcode + "'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return (int)tempdt.Rows[0][0];
            }
        }

        //表格错误处理
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        //添加编辑行
        private void addrows()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = dataGridView1.Rows.Count+1;
            dataGridView1.Rows.Add(dr);
            //DataGridViewComboBoxCell combox = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1] as DataGridViewComboBoxCell;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    combox.Items.Add(dt.Rows[i][0]);
            //}
        }

        private void ProcessProductInstru_Load(object sender, EventArgs e)
        {

        }

        //审核按钮点击
        public override void CheckResult()
        {
            base.CheckResult();
            //获得审核信息
            textBox25.Text = checkform.userName;
            dateTimePicker3.Value = checkform.time;
            dt_prodinstr.Rows[0]["审批人"] = checkform.userName;
            dt_prodinstr.Rows[0]["审批时间"] = checkform.time;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //状态
            if (checkform.ischeckOk)
            {
                dt_prodinstr.Rows[0]["状态"] = 1;//待接收
            }
            else
            {
                dt_prodinstr.Rows[0]["状态"] = 0;//未审核，草稿
            }

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            checkform = new mySystem.CheckForm(this);
            checkform.Show();  
            
        }

        //确认按钮
        private void button1_Click_1(object sender, EventArgs e)
        {
            //判断合法性
            if (!input_Judge())
                return;

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(tb指令编号.Text);
            removeOuterBinding();
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();
        }

        //编辑单元格结束后触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 3)
            {
                string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                string pattern = @"^[a-zA-Z]+-[a-zA-Z]+-[0-9]+\*[0-9]";//正则表达式
                if (!Regex.IsMatch(str, pattern))
                {
                    MessageBox.Show("产品代码输入不符合规定，重新输入，例如 PEQ-QE-500*100");
                    //MessageBox.Show("...");
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "";
                    leng = 0;
                    return;
                }
                string[] array = str.Split('*');
                string[] array2 = array[0].Split('-');
                leng = float.Parse(array2[2]);


            }
            //用料重量自己计算
            if (e.ColumnIndex == 4)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * leng / 1000.0 * 2 * 0.093;
            }

            //计算合计
            float sum_mi = 0, sum_juan = 0, sum_weight = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString()!="")
                    sum_mi += float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "")
                    sum_weight += float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "")
                    sum_juan += float.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
            }
            dt_prodinstr.Rows[0]["计划产量合计米"] = sum_mi;
            dt_prodinstr.Rows[0]["用料重量合计"] = sum_weight;
            dt_prodinstr.Rows[0]["计划产量合计卷"] = sum_juan;
        }

        //datagridview 添加行
        private void button4_Click(object sender, System.EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        //根据id填空
        private void fill(int id)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令信息表 where ID=" +id;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                tb工艺.Text = "";
                tb设备编号.Text = "";
                dtp开始生产日期.Value = DateTime.Now;

                tb内外层物料代码.Text = "";
                tb内外层物料批号.Text = "";
                textBox19.Text = "";
                tb内外领料量.Text = "";
                textBox16.Text = "";
                textBox18.Text = "";
                textBox20.Text = "";
                tb中层领料量.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                tb卷心管领料量.Text = "";
                textBox9.Text = "";
                tb双层包装领料量.Text = "";
                tb负责人.Text = "";

                textBox24.Text = mySystem.Parameter.userName;
                dateTimePicker2.Value = DateTime.Now;
                textBox25.Text = "";
                dateTimePicker3.Value = DateTime.Now;
                textBox26.Text = "";
                dateTimePicker4.Value = DateTime.Now;
                textBox23.Text = "";
            }
            if (tempdt.Rows.Count == 1)
            {
                tb指令编号.Text = (string)tempdt.Rows[0][2];
                tb工艺.Text = (string)tempdt.Rows[0][3]; 
                tb设备编号.Text = (string)tempdt.Rows[0][4];
                dtp开始生产日期.Value = (DateTime)tempdt.Rows[0][5];

                tb内外层物料代码.Text = (string)tempdt.Rows[0][6]; 
                tb内外层物料批号.Text = (string)tempdt.Rows[0][7]; 
                textBox19.Text = (string)tempdt.Rows[0][8];
                tb内外领料量.Text = ((double)tempdt.Rows[0][9]).ToString();
                textBox16.Text = (string)tempdt.Rows[0][10]; 
                textBox18.Text = (string)tempdt.Rows[0][11];
                textBox20.Text = (string)tempdt.Rows[0][12];
                tb中层领料量.Text = ((double)tempdt.Rows[0][13]).ToString();
                textBox12.Text = (string)tempdt.Rows[0][14]; 
                textBox13.Text = (string)tempdt.Rows[0][15];
                tb卷心管领料量.Text = ((double)tempdt.Rows[0][16]).ToString();
                textBox9.Text = (string)tempdt.Rows[0][17];
                tb双层包装领料量.Text = ((double)tempdt.Rows[0][18]).ToString();
                tb负责人.Text = (string)tempdt.Rows[0][19];

                textBox24.Text = (string)tempdt.Rows[0][20];
                dateTimePicker2.Value = (DateTime)tempdt.Rows[0][21];
                textBox25.Text = (string)tempdt.Rows[0][22];
                dateTimePicker3.Value = (DateTime)tempdt.Rows[0][23];
                textBox26.Text = (string)tempdt.Rows[0][24];
                dateTimePicker4.Value = (DateTime)tempdt.Rows[0][25];
                textBox23.Text = (string)tempdt.Rows[0][29];

                da.Dispose();
                tempdt.Dispose();

            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            mySystem.Setting.Setting_CleanSite s = new mySystem.Setting.Setting_CleanSite(mainform);
            s.Show();
        }

        private void button7_Click(object sender, System.EventArgs e)
        {
            Setting_CleanArea s = new Setting_CleanArea(mainform);
            s.Show();
        }

        //通过生产指令查找对应id
        private int id_findby_instrcode(string code)
        {
            string acsql = "select ID from 生产指令信息表 where 生产指令编号='" + code + "'";
            OleDbCommand comm = new OleDbCommand(acsql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            comm.Dispose();
            da.Dispose();
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                return -1;
            }
            else
            {
                int ret = (int)dt1.Rows[0][0];
                dt1.Dispose();
                return ret;
            }
            
        }

        //查询/插入按钮
        private void button5_Click_1(object sender, System.EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            tb指令编号.Enabled = false;
            bt审核.Enabled = false;

            
            readOuterData(tb指令编号.Text);
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(tb指令编号.Text);
                removeOuterBinding();
                outerBind();
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();
            if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                bt打印.Enabled = true;
                tb指令编号.Enabled = true;
            }

        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = tb指令编号.Text;
            dr["开始生产日期"]=DateTime.Now;
            dr["内外层领料量"]=0;
            dr["中层领料量"]=0;
            dr["卷心管领料量"]=0;
            dr["双层洁净包装领料量"]=0;
            dr["状态"]=0;
            dr["计划产量合计米"]=0;
            dr["用料重量合计"]=0;
            dr["计划产量合计卷"]=0;
            dr["编制人"]=mySystem.Parameter.userName;
            dr["编制时间"]=DateTime.Now;
            dr["审批时间"]=DateTime.Now;
            dr["接收时间"]=DateTime.Now;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["生产指令ID"]=dt_prodinstr.Rows[0]["ID"];
            dr["计划产量米"]=0;
            dr["用料重量"]=0;
            dr["每卷长度"]=0;
            dr["计划产量卷"]=0;
            dr["卷心管规格"]=0;
            dr["标签"]=0;
            dr["标签领料量"]=0;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string code)
        {
            
            dt_prodinstr = new DataTable("生产指令信息表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 生产指令信息表 where 生产指令编号='"+code+"'", connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("生产指令产品列表");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 生产指令产品列表 where 生产指令ID="+id, connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            comboBox1.DataBindings.Clear();
            tb指令编号.DataBindings.Clear();
            tb工艺.DataBindings.Clear();
            tb设备编号.DataBindings.Clear();
            dtp开始生产日期.DataBindings.Clear();

            tb内外层物料代码.DataBindings.Clear();
            tb内外层物料批号.DataBindings.Clear();
            textBox19.DataBindings.Clear();
            tb内外领料量.DataBindings.Clear();
            textBox16.DataBindings.Clear();
            textBox18.DataBindings.Clear();
            textBox20.DataBindings.Clear();
            tb中层领料量.DataBindings.Clear();
            textBox12.DataBindings.Clear();
            textBox13.DataBindings.Clear();
            tb卷心管领料量.DataBindings.Clear();
            textBox9.DataBindings.Clear();
            tb双层包装领料量.DataBindings.Clear();
            tb负责人.DataBindings.Clear();

            textBox24.DataBindings.Clear();
            textBox25.DataBindings.Clear();
            textBox26.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
            dateTimePicker3.DataBindings.Clear();
            dateTimePicker4.DataBindings.Clear();

            textBox6.DataBindings.Clear();
            tb用料重量合计.DataBindings.Clear();
            textBox10.DataBindings.Clear();
        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind() 
        {
            bs_prodinstr.DataSource = dt_prodinstr;
            bind_bs_contr();
        }
        // 内表和控件的绑定
        void innerBind()
        {
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //判断数据合法性
        bool input_Judge()
        {
            //判断合法性
            int nameid = mySystem.Parameter.NametoID(tb负责人.Text);
            if (nameid <= 0)
            {
                MessageBox.Show("负责人id不存在");
                return false;
            }
            if (float.Parse(tb内外领料量.Text) < float.Parse(tb用料重量合计.Text))
            {
                MessageBox.Show("输入不合法");
                tb内外领料量.Text = "0";
                return false;
            }
            //判断领料量是否是合法
            int tempvalue;
            if (!int.TryParse(tb中层领料量.Text, out tempvalue))
            {
                MessageBox.Show("输入不合法");
                tb中层领料量.Text = "0";
                return false;
            }
            if (!int.TryParse(tb卷心管领料量.Text, out tempvalue))
            {
                MessageBox.Show("输入不合法");
                tb卷心管领料量.Text = "0";
                return false;
            }
            if (!int.TryParse(tb双层包装领料量.Text, out tempvalue))
            {
                MessageBox.Show("输入不合法");
                tb双层包装领料量.Text = "0";
                return false;
            }
            return true;
        }
    }
}
