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
        private string person_操作员;
        private string person_审核员;
        private List<string> prodcode;

        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        public Gamma射线辐射灭菌委托单(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
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
            tb辐照单位.DataBindings.Clear();
            tb箱数.DataBindings.Clear();
            tb托数.DataBindings.Clear();
            tb其他说明.DataBindings.Clear();
            tb委托人.DataBindings.Clear();
            dtp委托日期.DataBindings.Clear();
            tb运输商.DataBindings.Clear();
            tb审批人.DataBindings.Clear();
            dtp审批日期.DataBindings.Clear();
            tb操作人.DataBindings.Clear();
            dtp操作日期.DataBindings.Clear();

            bs_prodinstr.DataSource = dt_prodinstr;

            tb委托单号.DataBindings.Add("Text", bs_prodinstr.DataSource, "委托单号");
            tb辐照单位.DataBindings.Add("Text", bs_prodinstr.DataSource, "辐照单位");

            tb箱数.DataBindings.Add("Text", bs_prodinstr.DataSource, "合计箱数");
            tb托数.DataBindings.Add("Text", bs_prodinstr.DataSource, "合计托数");
            tb其他说明.DataBindings.Add("Text", bs_prodinstr.DataSource, "其他说明");
            tb委托人.DataBindings.Add("Text", bs_prodinstr.DataSource, "委托人");          
            dtp委托日期.DataBindings.Add("Value", bs_prodinstr.DataSource, "委托日期");
            tb运输商.DataBindings.Add("Value", bs_prodinstr.DataSource, "运输商");
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
            da_prodlist = new OleDbDataAdapter("select * from Gamma射线辐射灭菌委托单详细信息 where TGamma射线辐射灭菌委托单详细信息ID" + 外表行ID, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);
        }

        // 给内表的一行写入默认值，包括操作人，时间，Y/N等
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["TGamma射线辐射灭菌委托单详细信息ID"] = dt_prodinstr.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["数量箱"] = 0;
            dr["数量只"] = 0;
            dr["每箱重量"] = 0;
            return dr;
        }
        // 内表和控件的绑定
        void innerBind(){}

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        void setDataGridViewColumns()
        {

        }

        // 刷新DataGridView中的列：序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        // 设置各控件的事件
	    // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDateEventHandler(){}
	        // 设置自动计算类事件
        void addComputerEventHandler(){}
	        // 其他事件，比如按钮的点击，数据有效性判断
        void addOtherEvnetHandler(){}
        // 打印函数
        void print(bool label){}
        // 获取其他需要的数据，比如产品代码，产生废品原因等
        void getOtherData()
        {
 
        }
        // 获取操作员和审核员
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
 
        }


        private void bt保存_Click(object sender, EventArgs e)
        {

        }
    }
}
