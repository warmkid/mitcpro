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
    public partial class CleanCut_Cover : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dt_prodinstr, dt_prodlist, dt_prodlist2;//外表，内表目录，内表汇总
        private OleDbDataAdapter da_prodinstr, da_prodlist, da_prodlist2;
        private BindingSource bs_prodinstr, bs_prodlist, bs_prodlist2;
        private OleDbCommandBuilder cb_prodinstr, cb_prodlist, cb_prodlist2;
        private string person_操作员;
        private string person_审核员;
        List<string> list_记录;
        List<int> list_页数;
        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        public CleanCut_Cover(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            if (dt_prodinstr.Rows.Count <= 0)
            {
                DataRow dr = dt_prodinstr.NewRow();
                dr = writeOuterDefault(dr);
                dt_prodinstr.Rows.Add(dr);
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                readOuterData(mySystem.Parameter.cleancutInstruID);
                outerBind();
            }

            readInnerData((int)dt_prodinstr.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

        }

        private void bt确认_Click(object sender, EventArgs e)
        {
            bool rt = save();

            //控件可见性
            if (rt && stat_user == 0)
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
            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dt_prodinstr.Rows[0]["ID"]));
            innerBind();

            return true;
        }

        bool input_Judge()
        {
            //判断合法性
            //汇总人
            if (mySystem.Parameter.NametoID(tb汇总.Text) <= 0)
            {
                MessageBox.Show("汇总人ID不存在");
                return false;
            }
            //批准人
            if (mySystem.Parameter.NametoID(tb批准.Text) <= 0)
            {
                MessageBox.Show("批准人ID不存在");
                return false;
            }
            return true;


        }

        void addDataEventHandler()
        {

        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dt_prodinstr = new DataTable("批生产记录表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter(@"select * from 批生产记录表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);
        }
        // 给外表的一行写入默认值    TODO:*******************************
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            dr["生产指令编号"] = mySystem.Parameter.cleancutInstruction;

            dr["使用物料"] = "";
            dr["开始生产时间"] = DateTime.Now;
            dr["结束生产时间"] = DateTime.Now;
            dr["汇总时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            dr["审核是否通过"] = false;
            return dr;
        }
        // 先清除绑定，再完成外表和控件的绑定
        void outerBind()
        {
            tb生产指令.DataBindings.Clear();
            tb使用物料.DataBindings.Clear();
            dtp开始时间.DataBindings.Clear();
            dtp结束时间.DataBindings.Clear();
            tb汇总.DataBindings.Clear();
            tb批准.DataBindings.Clear();
            tb审核.DataBindings.Clear();
            dtp汇总时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Clear();
            tb备注.DataBindings.Clear();

            bs_prodinstr.DataSource = dt_prodinstr;

            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            tb使用物料.DataBindings.Add("Text", bs_prodinstr.DataSource, "使用物料");
            dtp开始时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "开始生产时间");
            dtp结束时间.DataBindings.Add("Text", bs_prodinstr.DataSource, "结束生产时间");
            tb汇总.DataBindings.Add("Text", bs_prodinstr.DataSource, "汇总人");
            dtp汇总时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "汇总时间");
            tb审核.DataBindings.Add("Text", bs_prodinstr.DataSource, "审核人");
            dtp审核时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "审核时间");
            tb批准.DataBindings.Add("Text", bs_prodinstr.DataSource, "批准人");
            dtp批准时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "批准时间");
            tb备注.DataBindings.Add("Text", bs_prodinstr.DataSource, "备注");
        }

        // 根据条件从数据库中读取内表数据,目录
        void readInnerData(int outid)
        {
            dt_prodlist = new DataTable("批生产记录目录详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 批生产记录目录详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            //如果为空，则进行插入
            if (dt_prodlist.Rows.Count <= 0)
            {
                for (int i = 0; i < list_记录.Count; i++)
                {
                    DataRow dr = dt_prodlist.NewRow();
                    dr["T批生产记录封面ID"] = dt_prodinstr.Rows[0]["ID"];
                    dr["序号"] = i + 1;
                    dr["记录"] = list_记录[i];
                    dr["页数"] = list_页数[i];
                    dt_prodlist.Rows.Add(dr);
                }
            }
        }

        // 内表和控件的绑定，目录
        void innerBind()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
        }

        // 根据条件从数据库中读取内表数据,汇总
        void readInnerData2(int outid)
        {
            dt_prodlist2 = new DataTable("批生产记录产品详细信息");
            bs_prodlist2 = new BindingSource();
            da_prodlist2 = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
            da_prodlist2.Fill(dt_prodlist2);
        }

        // 内表和控件的绑定
        void innerBind2()
        {
            //移除所有列
            while (dataGridView2.Columns.Count > 0)
                dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            setDataGridViewCombox();
            bs_prodlist2.DataSource = dt_prodlist2;
            dataGridView2.DataSource = bs_prodlist2.DataSource;
            setDataGridViewColumns2();
        }

        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist2.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "生产数量":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "生产数量(米)";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c1);
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c2.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c2);
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

        void setDataGridViewColumns2()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        // 设置各控件的事件
        void addDateEventHandler()
        { }

        // 打印函数
        void print(bool label)
        { }
        // 获取其他需要的数据 记录和页数
        void getOtherData()
        {
            list_记录 = new List<string>();
            list_记录.Add("SOP-MFG-302-R01A 清洁分切生产指令");
            list_记录.Add("SOP-MFG-302-R02A 清洁分切机开机确认及运行记录");
            list_记录.Add("SOP-MFG-302-R03A 清洁分切生产记录表");
            list_记录.Add("SOP-MFG-302-R04A 清洁分切日报表");
            list_记录.Add("SOP-MFG-110-R01A 清场记录");

            list_页数 = new List<int>();
            list_页数.Add(2);
            list_页数.Add(3);
            list_页数.Add(4);
            list_页数.Add(2);
            list_页数.Add(5);
        }
        // 获取操作员和审核员
        void getPeople()
        {
            DataTable dt = new DataTable("用户权限");
            OleDbDataAdapter da = new OleDbDataAdapter(@"select * from 用户权限 where ID=8", mySystem.Parameter.connOle);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                person_操作员 = dt.Rows[0]["操作员"].ToString();
                person_审核员 = dt.Rows[0]["审核员"].ToString();
            }
        }
        // 计算，主要用于日报表、物料平衡记录中的计算
        void computer()
        { }
        // 获取当前窗体状态：
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        // 如果审核结果为『通过』，则为『审核通过』
        // 如果审核结果为『不通过』，则为『审核未通过』
        // 这个函数可以放在父类中？
        void setFormState()
        {
            if (dt_prodinstr.Rows[0]["审核人"].ToString() == "")
                stat_form = 0;
            else if (dt_prodinstr.Rows[0]["审核人"].ToString() == "__待审核")
                stat_form = 1;
            else if ((bool)dt_prodinstr.Rows[0]["审核是否通过"])
                stat_form = 2;
            else
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
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
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

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);

            if (dt_temp.Rows.Count == 0)
            {
                DataRow dr = dt_temp.NewRow();
                dr["表名"] = "批生产记录表";
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

            dt_prodinstr.Rows[0]["审核人"] = "__待审核";
            dt_prodinstr.Rows[0]["审核时间"] = DateTime.Now;

            save();

            //空间都不能点
            foreach (Control c in this.Controls)
                c.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = true;
            bt日志.Enabled = true;
            bt打印.Enabled = true;

        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dt_prodinstr.Rows[0]["日志"].ToString());
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        public override void CheckResult()
        {
            base.CheckResult();

            //获得审核信息
            //dtp审批日期.Value = checkform.time;
            dt_prodinstr.Rows[0]["审核人"] = checkform.userName;
            dt_prodinstr.Rows[0]["审核时间"] = checkform.time;

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
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dt_prodinstr.Rows[0]["ID"], mySystem.Parameter.connOle);
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

        private void bt打印_Click(object sender, EventArgs e)
        {

        }
    }
}
