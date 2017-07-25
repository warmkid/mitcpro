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
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace mySystem.Process.灭菌
{
    public partial class Gamma射线辐射灭菌委托单 : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;

        private List<string> list_产品代码;
        private string person_操作员;
        private string person_审核员;

        private string str_委托单;

        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        public Gamma射线辐射灭菌委托单(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            foreach (Control c in this.Controls)
                c.Enabled = false;
            tb委托单号.Enabled = true;
            bt查询插入.Enabled = true;
        }

        public Gamma射线辐射灭菌委托单(mySystem.MainForm mainform,int id)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            string asql = "select * from Gamma射线辐射灭菌委托单 where ID=" + id;
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            str_委托单 = tempdt.Rows[0]["委托单号"].ToString();

            readOuterData(str_委托单);
            outerBind();

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            tb委托单号.Enabled = false;
            bt查询插入.Enabled = false;
        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDataEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;
        }

        //表格填写错误提示
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string str_委托单号)
        {
            dt_prodinstr = new DataTable("Gamma射线辐射灭菌委托单");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter(@"select * from Gamma射线辐射灭菌委托单 where 委托单号='" + str_委托单号 + "'", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["委托单号"] = tb委托单号.Text;
            dr["合计箱数"] = 0;
            dr["合计托数"] = 0;

            dr["操作人"] = mySystem.Parameter.userName;
            dr["委托日期"] = DateTime.Now;
            dr["审批日期"] = DateTime.Now;
            dr["操作日期"] = DateTime.Now;

            dr["审核是否通过"] = false;
            return dr;
        }

        // 先清除绑定，再完成外表和控件的绑定
        void outerBind()
        {
            tb委托单号.DataBindings.Clear();
            cb辐照单位.DataBindings.Clear();
            tb箱数.DataBindings.Clear();
            tb托数.DataBindings.Clear();
            tb其他说明.DataBindings.Clear();
            tb委托人.DataBindings.Clear();
            dtp委托日期.DataBindings.Clear();
            cb运输商.DataBindings.Clear();
            tb审批人.DataBindings.Clear();
            dtp审批日期.DataBindings.Clear();
            tb操作人.DataBindings.Clear();
            dtp操作日期.DataBindings.Clear();

            bs_prodinstr.DataSource = dt_prodinstr;

            tb委托单号.DataBindings.Add("Text", bs_prodinstr.DataSource, "委托单号");
            cb辐照单位.DataBindings.Add("Text", bs_prodinstr.DataSource, "辐照单位");
            tb箱数.DataBindings.Add("Text", bs_prodinstr.DataSource, "合计箱数");
            tb托数.DataBindings.Add("Text", bs_prodinstr.DataSource, "合计托数");
            tb其他说明.DataBindings.Add("Text", bs_prodinstr.DataSource, "其他说明");
            tb委托人.DataBindings.Add("Text", bs_prodinstr.DataSource, "委托人");          
            dtp委托日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "委托日期");
            cb运输商.DataBindings.Add("Text", bs_prodinstr.DataSource, "运输商");
            tb审批人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批");
            dtp审批日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批日期");
            tb操作人.DataBindings.Add("Text", bs_prodinstr.DataSource, "操作人");
            dtp操作日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "操作日期");
            
        }


        // 根据条件从数据库中读取内表数据
        void readInnerData(int 外表行ID)
        {
            dt_prodlist = new DataTable("Gamma射线辐射灭菌委托单详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单详细信息 where TGamma射线辐射灭菌委托单详细信息ID=" + 外表行ID, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }

        // 给内表的一行写入默认值，包括操作人，时间，Y/N等
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["TGamma射线辐射灭菌委托单详细信息ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["数量箱"] = 0;
            dr["数量只"] = 0;
            dr["每箱重量"] = 0;
            return dr;
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
                    case "产品代码":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "产品代码";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        foreach (string sdr in list_产品代码)
                        {
                            c1.Items.Add(sdr);
                        }
                        dataGridView1.Columns.Add(c1);
                        break;

                    case "数量箱":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = "数量(箱)";
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;

                    case "数量只":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = "数量(只)";
                        c4.Name = dc.ColumnName;
                        c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c4.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c4);
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);
                        break;
                }
            }
        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        // 刷新DataGridView中的列：序号
        void setDataGridViewRowNums()
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            //}
        }

        // 设置各控件的事件
	    // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDateEventHandler(){}
	    // 设置自动计算类事件

        // 打印函数
        void print(bool label){}
        // 获取其他需要的数据，比如产品代码，产生废品原因等
        void getOtherData()
        {
            get_辐照单位();
            get_运输商();
            get_产品代码();
        }

        private void get_辐照单位()
        {
            DataTable dt = new DataTable("设置辐照单位");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 设置辐照单位", mySystem.Parameter.connOle);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb辐照单位.Items.Add(dt.Rows[i][1].ToString());
            }
        }
        private void get_运输商()
        {
            DataTable dt = new DataTable("设置运输商");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 设置运输商", mySystem.Parameter.connOle);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb运输商.Items.Add(dt.Rows[i][1].ToString());
            }
        }
        private void get_产品代码()
        {
            list_产品代码 = new List<string>();
            DataTable dt = new DataTable("设置灭菌产品代码");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 设置灭菌产品代码", mySystem.Parameter.connOle);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list_产品代码.Add(dt.Rows[i][1].ToString());
            }
        }
        // 获取操作员和审核员
        void getPeople()
        {
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where 步骤='Gamma射线辐射灭菌委托单'", mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
            }
        }
        // 计算，主要用于日报表、物料平衡记录中的计算
        void computer() { }

        // 获取当前窗体状态：
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        // 这个函数可以放在父类中？
        void setFormState()
        {
            if (dt_prodinstr.Rows[0]["审批"].ToString() == "")//未保存
                stat_form = 0;
            else if (dt_prodinstr.Rows[0]["审批"].ToString() == "__待审核")
                stat_form = 1;
            else if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])//审核通过
                stat_form = 2;
            else//审核未通过
                stat_form = 3;
        }
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        void setUserState()
        {
            if (mySystem.Parameter.userName == person_操作员)
                stat_user = 0;
            else if (mySystem.Parameter.userName == person_审核员)
                stat_user = 1;
            else
                stat_user = 2;
        }

        void setControlsTrue()
        {
            //控件都能点
            foreach (Control c in this.Controls)
                c.Enabled = true;
            dataGridView1.ReadOnly = false;
            bt发送审核.Enabled = false;
            bt审核.Enabled = false;
        }
        void setControlsFalse()
        {
            //空间都不能点
            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt日志.Enabled = true;
            bt打印.Enabled = true;
        }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        void setEnableReadOnly()
        {
            if (stat_user == 2)//管理员
            {
                //控件都能点
                setControlsTrue();
            }
            else if (stat_user == 1)//审核人
            {
                if (stat_form == 0 || stat_form == 3 || stat_form == 2)//未保存,审核不通过，审核通过
                {
                    //空间都不能点
                    setControlsFalse();
                }
                else
                {
                    //发送审核不可点，其他都可点
                    setControlsTrue();
                    bt审核.Enabled = true;
                }

            }
            else//操作员
            {
                if (stat_form == 1 || stat_form == 2)//待审核，审核通过
                {
                    //空间都不能点
                    setControlsFalse();
                }
                else
                {
                    //发送审核，审核不能点
                    setControlsTrue();
                }
            }
        }


        private void bt保存_Click(object sender, EventArgs e)
        {
            bool rt=save();
            //控件可见性
            if (rt && stat_user == 0)
                bt发送审核.Enabled = true;
        }

        //保存内外表数据
        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
            readOuterData(str_委托单);
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            return true;
        }
        private bool input_Judge()
        {
            if (tb委托人.Text == "")
            {
                MessageBox.Show("委托人不能为空");
                return false;
            }
            if (mySystem.Parameter.NametoID(tb操作人.Text) <= 0)
            {
                MessageBox.Show("操作人ID不存在");
                return false;
            }
            return true;
        }

        //发送审核
        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='Gamma射线辐射灭菌委托单' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "Gamma射线辐射灭菌委托单";
                dr["对应ID"] = (int)dt_prodinstr.Rows[0]["ID"];
                dt_temp.Rows.Add(dr);
            }
            bs_temp.DataSource = dt_temp;
            da_temp.Update((DataTable)bs_temp.DataSource);

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            dt_prodinstr.Rows[0]["审批"] = "__待审核";
            dt_prodinstr.Rows[0]["审批日期"] = DateTime.Now;

            save();

            //空间都不能点
            setControlsFalse();

            //可以操作其他委托单情况
            bt查询插入.Enabled = true;
            tb委托单号.Enabled = true;
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
        }

        //审核
        public override void CheckResult()
        {
            base.CheckResult();

            //获得审核信息
            dt_prodinstr.Rows[0]["审批"] = checkform.userName;
            dt_prodinstr.Rows[0]["审批日期"] = checkform.time;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //状态,不论是否通过,都不能再点

            //改变控件状态
            setControlsFalse();

            //可以操作其他委托单情况
            bt查询插入.Enabled = true;
            tb委托单号.Enabled = true;



            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='Gamma射线辐射灭菌委托单' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString() + log;

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
        }

        //审核按钮
        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        private void bt打印_Click(object sender, EventArgs e)
        {

        }

        private void bt查询插入_Click(object sender, EventArgs e)
        {
            str_委托单 = tb委托单号.Text;
            readOuterData(str_委托单);
            outerBind();
            if (dt_prodinstr.Rows.Count <= 0 && stat_user != 0)
            {
                MessageBox.Show("只有操作员可以新建指令");
                return;
            }
            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(str_委托单);
                outerBind();
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            tb委托单号.Enabled = false;
            bt查询插入.Enabled = false;
        }

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_prodlist.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < 0)
                    return;
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }

            //刷新序号
            setDataGridViewRowNums();

            //刷新合计
            sumDataGridView1();
        }

        //中文逗号转英文逗号
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
             if (e.RowIndex < 0)
                return;
             if (e.ColumnIndex == 2)
             {
                 string str=ToDBC(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                 string pattern = "([0-9][,]?)+";//正则表达式
                 if (!Regex.IsMatch(str, pattern))
                 {
                     MessageBox.Show("产品代码格式不符合规定，重新输入，例如 1,2,4");
                     dataGridView1.Rows[e.RowIndex].Cells[2].Value = "";
                     return;   
                 }
             }
             sumDataGridView1();
        }

        //刷新合计
        private void sumDataGridView1()
        {
            List<int> list_托 = new List<int>();
            float sum_箱 = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() != "")//托盘号
                {
                    string[] a = dataGridView1.Rows[i].Cells[2].Value.ToString().Split(',');
                    for (int k = 0; k < a.Length; k++)
                        list_托.Add(int.Parse(a[k]));
                }
                if (dataGridView1.Rows[i].Cells[6].Value.ToString() != "")
                    sum_箱 += float.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());            
            }

            dt_prodinstr.Rows[0]["合计箱数"] = sum_箱;
            if(list_托.Count>0)
                dt_prodinstr.Rows[0]["合计托数"] = list_托.Max();

            bs_prodinstr.EndEdit();
            da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);

        }
    }
}
