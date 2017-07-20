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

namespace mySystem.Process.CleanCut
{
    public partial class Instru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist;
        private OleDbDataAdapter da_prodinstr, da_prodlist;
        private BindingSource bs_prodinstr, bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist;
        private string person_操作员;
        private string person_审核员;
        private List<string> prodcode;

        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过


        public Instru(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            getPeople(2);
            setUserState();
            getOtherData();
            addDataEventHandler();

            foreach(Control c in this.Controls)
                c.Enabled=false;
            tb指令编号.Enabled = true;
            bt查询插入.Enabled = true;

        }

        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDataEventHandler()
        {
 
        }

        void setUserState()
        {
            if (mySystem.Parameter.userName == person_操作员)
                stat_user = 0;
            else if (mySystem.Parameter.userName == person_审核员)
                stat_user = 1;
            else
                stat_user = 2;
        }

        //// 获取操作员和审核员
        void getPeople(int id)
        {
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where ID=" + id, mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
            }

        }

        //获取设置中产品代码
        private void getOtherData()
        {
            prodcode = new List<string>();
            OleDbDataAdapter tda = new OleDbDataAdapter("select 产品编码 from 设置清洁分切产品编码", mySystem.Parameter.connOle);
            DataTable tdt = new DataTable("产品编码");
            tda.Fill(tdt);
            foreach (DataRow tdr in tdt.Rows)
            {
                prodcode.Add(tdr["产品编码"].ToString());
            }
        }

        //public Instru()
        //{ }
        private void Init()
        {
           
        }

        //表格填写错误提示
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
        }

        bool input_Judge()
        {
            //判断合法性
            //编制人
            if (mySystem.Parameter.NametoID(tb编制人.Text) <= 0)
            {
                MessageBox.Show("编制人ID不存在");
                return false;
            }
            //接收人
            if (mySystem.Parameter.NametoID(tb接收人.Text) <= 0)
            {
                MessageBox.Show("接受人ID不存在");
                return false;
            }
            return true;


        }

        //确认按钮
        private void button1_Click(object sender, EventArgs e)
        {
            bool rt=save();

            //控件可见性
            if(rt && stat_user==0)
                bt发送审核.Enabled = true;
        }

        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

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

            return true;
        }

        private void bt查询插入_Click(object sender, EventArgs e)
        {
            readOuterData(tb指令编号.Text);
            removeOuterBinding();
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

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();

        }

        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        void setFormState()
        {
            if (dt_prodinstr.Rows[0]["审核人"].ToString() == "")
                stat_form = 0;
            else if(dt_prodinstr.Rows[0]["审核人"].ToString() =="__待审核")
                stat_form = 1;
            else if((bool)dt_prodinstr.Rows[0]["审核是否通过"])
                stat_form = 2;
            else
                stat_form = 3;
        }

        // 其他事件，比如按钮的点击，数据有效性判断
        void addOtherEvnetHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;
        }

        void setEnableReadOnly()
        {
            if (stat_user == 2)//管理员
            {
                //控件都能点
                foreach (Control c in this.Controls)
                    c.Enabled = true;
            }
            else if (stat_user == 1)//审核人
            {
                if (stat_form == 0 || stat_form == 3 || stat_form == 2)//草稿,审核不通过，审核通过
                {
                    //空间都不能点
                    foreach (Control c in this.Controls)
                        c.Enabled = false;
                    dataGridView1.Enabled = true;
                    dataGridView1.ReadOnly = true;
                    bt日志.Enabled = true;
                    bt打印.Enabled = true;

                }
                else//待审核
                {
                    //发送审核不可点，其他都可点
                    foreach (Control c in this.Controls)
                        c.Enabled = true;
                    bt发送审核.Enabled = false;
                }

            }
            else//操作员
            {
                if (stat_form == 1 || stat_form == 2)//待接收，审核通过
                {
                    //空间都不能点
                    foreach (Control c in this.Controls)
                        c.Enabled = false;
                    dataGridView1.Enabled = true;
                    dataGridView1.ReadOnly = true;
                    bt日志.Enabled = true;
                    bt打印.Enabled = true;

                    bt查询插入.Enabled = true;
                    tb指令编号.Enabled = true;

                }
                else//未审核与审核不通过
                {
                    //发送审核，审核不能点
                    foreach (Control c in this.Controls)
                        c.Enabled = true;
                    bt发送审核.Enabled = false;
                    bt审核.Enabled = false;

                }
            }
        }

        // 设置自动计算类事件
        void addComputerEventHandler()
        { }

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = tb指令编号.Text;
            dr["生产设备"] = "分切机AA-EQU-035";
            dr["计划生产日期"] = DateTime.Now;

            dr["编制人"] = mySystem.Parameter.userName;
            dr["编制时间"] = DateTime.Now;
            dr["审批时间"] = DateTime.Now;
            dr["接收时间"] = DateTime.Now;
            dr["备注"] = "";

            dr["状态"] = 0;
            dr["审核是否通过"] = false;
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T生产指令表ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["数量卷"] = 0;
            dr["数量米"] = 0;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string code)
        {
            dt_prodinstr = new DataTable("清洁分切工序生产指令");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter(@"select * from 清洁分切工序生产指令 where 生产指令编号='" + code + "'", mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_prodlist = new DataTable("清洁分切工序生产指令详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 清洁分切工序生产指令详细信息 where T生产指令表ID=" + id, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb指令编号.DataBindings.Clear();
            tb设备编号.DataBindings.Clear();
            dtp计划生产日期.DataBindings.Clear();

            tb备注.DataBindings.Clear();
            tb编制人.DataBindings.Clear();
            tb审批人.DataBindings.Clear();
            tb接收人.DataBindings.Clear();

            dtp编制日期.DataBindings.Clear();
            dtp审批日期.DataBindings.Clear();
            dtp接收日期.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_prodinstr.DataSource = dt_prodinstr;

            tb指令编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            tb设备编号.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产设备");
            dtp计划生产日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "计划生产日期");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
            tb编制人.DataBindings.Add("Text", bs_prodinstr.DataSource, "编制人");
            tb审批人.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批人");
            tb接收人.DataBindings.Add("Text", bs_prodinstr.DataSource, "接收人");
            dtp编制日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "编制时间");
            dtp审批日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批时间");
            dtp接收日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "接收时间");
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
                    case "清洁前产品代码":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "清洁前产品代码(规格型号)";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        // 如果换了名字会报错，把当前值也加上就好了
                        // 加序号，按序号显示
                        OleDbDataAdapter tda = new OleDbDataAdapter("select 产品编码 from 设置清洁分切产品编码", mySystem.Parameter.connOle);
                        DataTable tdt = new DataTable("产品编码");
                        tda.Fill(tdt);
                        foreach (DataRow tdr in tdt.Rows)
                        {
                            c1.Items.Add(tdr["产品编码"]);
                        }
                        dataGridView1.Columns.Add(c1);
                        // 重写cell value changed 事件，自动填写id
                        break;

                    case "数量卷":
                        DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
                        c3.DataPropertyName = dc.ColumnName;
                        c3.HeaderText = "数量(卷)";
                        c3.Name = dc.ColumnName;
                        c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c3.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c3);
                        break;

                    case "数量米":
                        DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
                        c4.DataPropertyName = dc.ColumnName;
                        c4.HeaderText = "数量(米)";
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
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
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
        }

        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
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

            //刷新序号
            setDataGridViewRowNums();
        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_prodlist.Rows.Count;
            if (count == 0)
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

            //刷新序号
            setDataGridViewRowNums();
        }

        public override void CheckResult()
        {
            //获得审核信息
            //dtp审批日期.Value = checkform.time;
            dt_prodinstr.Rows[0]["审批人"] = checkform.userName;
            dt_prodinstr.Rows[0]["审批时间"] = checkform.time;

            dt_prodinstr.Rows[0]["审核人"] = checkform.userName;
            dt_prodinstr.Rows[0]["审核意见"] = checkform.opinion;
            dt_prodinstr.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //状态
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt打印.Enabled = true;
            bt日志.Enabled = true;


            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
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

            base.CheckResult();
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp= new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='清洁分切工序生产指令' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "清洁分切工序生产指令";
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
            dt_prodinstr.Rows[0]["日志"] = dt_prodinstr.Rows[0]["日志"].ToString()+log;

            dt_prodinstr.Rows[0]["审核人"] = "__待审核";
            dt_prodinstr.Rows[0]["审批时间"] = DateTime.Now;
            dt_prodinstr.Rows[0]["审批人"] = "__待审核";
            
            save();

            //空间都不能点
            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt日志.Enabled = true;
            bt打印.Enabled = true;

            bt查询插入.Enabled = true;
            tb指令编号.Enabled = true;
            
        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
