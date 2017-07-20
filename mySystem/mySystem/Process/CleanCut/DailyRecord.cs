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
    public partial class DailyRecord : mySystem.BaseForm
    {
        // 利用isSQL来判断是用SQLConnection还是OleDBConnection来new这个对象
        // 带 ID 的构造函数
        // 注意，如果当前登录人既不是操作员也不是审核员，则提示，然后不显示界面(管理员例外）
        // 父类的变量
        private DataTable dtOuter,dtInner;
        private OleDbDataAdapter daOuter, daInner;
        private OleDbCommandBuilder cbOuter, cbInner;
        private BindingSource bsOuter, bsInner;

        public DailyRecord(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            //getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()


        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        void setDataGridViewColumns();
        // 刷新DataGridView中的列：序号
        void setDataGridViewRowNums();

        // 设置各控件的事件
	        // 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
        void addDateEventHandler();
	        // 设置自动计算类事件
        void addComputerEventHandler();
	        // 其他事件，比如按钮的点击，数据有效性判断
        void addOtherEvnetHandler();
        // 打印函数
        void print(bool label);
        // 获取其他需要的数据，比如产品代码，产生废品原因等

        void getOtherData();
        // 获取操作员和审核员
        void getPeople();
        // 计算，主要用于日报表、物料平衡记录中的计算
        void computer();
        // 获取当前窗体状态：
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        // 这个函数可以放在父类中？
        void setFormState();
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        void setUserState();
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        void setEnableReadOnly();

        private void bt保存_Click(object sender, EventArgs e)
        {

        }
    }
}
