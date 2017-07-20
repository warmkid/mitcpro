using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.灭菌
{
    public partial class 辐照灭菌台帐 : mySystem.BaseForm
    {
        private DataTable dt_taizhang;
        private BindingSource bs_taizhang;
        private OleDbDataAdapter da_taizhang;
        private OleDbCommandBuilder cb_taizhang;

        public 辐照灭菌台帐(mySystem.MainForm mainform): base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData();
            outerBind();
            readInnerData();
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void getPeople()
        {
        
        }
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        private void setUserState()
        { 
        
        }
        // 获取其他需要的数据，比如产品代码，产生废品原因等
        private void getOtherData()
        { 
        
        }
        // 根据条件从数据库中读取一行外表的数据
        private void readOuterData()
        {
           
        }
        // 外表和控件的绑定
        private void outerBind()
        { 
        
        }
        // 根据条件从数据库中读取多行内表数据
        private void readInnerData()
        {
            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
            OleDbConnection connOle = new OleDbConnection(strConn);
            connOle.Open();
            dt_taizhang = new DataTable("辐照灭菌台帐");
            bs_taizhang = new BindingSource();
            da_taizhang = new OleDbDataAdapter(@"select * from 辐照灭菌台账", connOle);
            cb_taizhang = new OleDbCommandBuilder(da_taizhang);
            da_taizhang.Fill(dt_taizhang);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
            bs_taizhang.DataSource = dt_taizhang;
            dataGridView1.DataSource = bs_taizhang.DataSource;
        }
        // 设置自动计算类事件
        private void addComputerEventHandler()
        { 
        
        }
        // 获取当前窗体状态：窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        //         如果审核结果为『通过』，则为『审核通过』
        //         如果审核结果为『不通过』，则为『审核未通过』
        private void setFormState()
        {
        
        }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
        private void setEnableReadOnly()
        { 
        
        }
        // 其他事件，比如按钮的点击，数据有效性判断
        private void addOtherEventHandler()
        { 
        
        }
    }
}
