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
    public partial class CleanCut_Cover : Form
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

        public CleanCut_Cover()
        {
            InitializeComponent();

        }

        private void bt确认_Click(object sender, EventArgs e)
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
        // 给外表的一行写入默认值 TODO:*******************************s
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;

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

            tb生产指令.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            tb使用物料.DataBindings.Add("Text", bs_prodinstr.DataSource, "使用物料");
            dtp开始时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "开始生产时间");
            dtp结束时间.DataBindings.Add("Text", bs_prodinstr.DataSource, "结束生产时间");
            tb汇总.DataBindings.Add("Text", bs_prodinstr.DataSource, "汇总人");
            dtp汇总时间.DataBindings.Add("Text", bs_prodinstr.DataSource, "汇总时间");
            tb审核.DataBindings.Add("Text", bs_prodinstr.DataSource, "审核人");
            dtp审核时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "审核时间");
            tb批准.DataBindings.Add("Value", bs_prodinstr.DataSource, "批准人");
            dtp批准时间.DataBindings.Add("Value", bs_prodinstr.DataSource, "批准时间");
            tb备注.DataBindings.Add("Value", bs_prodinstr.DataSource, "备注");
        }

        // 根据条件从数据库中读取内表数据,目录
        void readInnerData2(int outid)
        {
            dt_prodlist = new DataTable("批生产记录产品详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }

        // 内表和控件的绑定，目录
        void innerBind2()
        {
            //移除所有列
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewCombox();
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView1.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
        }

        // 根据条件从数据库中读取内表数据,datagridview2
        void readInnerData(int outid)
        {
            dt_prodlist = new DataTable("批生产记录产品详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
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

        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist.Columns)
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
                        dataGridView1.Columns.Add(c1);
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

        // 设置各控件的事件
        void addDateEventHandler()
        { }

        // 打印函数
        void print(bool label)
        { }
        // 获取其他需要的数据，比如产品代码，产生废品原因等
        void getOtherData()
        { }
        // 获取操作员和审核员
        void getPeople()
        { }
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
        { }
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        void setUserState()
        { }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        void setEnableReadOnly()
        { }
    }
}
