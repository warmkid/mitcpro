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
        private List<string> weituodanhao;

        public 辐照灭菌台帐(mySystem.MainForm mainform): base(mainform)
        {
            InitializeComponent();
           // dataGridView1.Columns[1].Visible = false;
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
            weituodanhao = new List<string>();
            OleDbDataAdapter danhao_search = new OleDbDataAdapter("select 委托单号 from Gamma射线辐射灭菌委托单",mySystem.Parameter.connOle);
            DataTable da_danhao = new DataTable("委托单号查询");
            danhao_search.Fill(da_danhao);
            foreach (DataRow tdr in da_danhao.Rows)
            {
                weituodanhao.Add(tdr["委托单号"].ToString());
            }
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
//            String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
//            OleDbConnection connOle = new OleDbConnection(strConn);
//            connOle.Open();
            dt_taizhang = new DataTable("辐照灭菌台帐");
            bs_taizhang = new BindingSource();
            da_taizhang = new OleDbDataAdapter(@"select * from 辐照灭菌台帐", mySystem.Parameter.connOle);
            cb_taizhang = new OleDbCommandBuilder(da_taizhang);
            da_taizhang.Fill(dt_taizhang);
        }
        // 内表和控件的绑定
        private void innerBind()
        {
            while (dataGridView1.Columns.Count > 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            setDataGridViewColumns();
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
        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        private void setDataGridViewColumns()
        {
            DataGridViewComboBoxColumn c1;
            DataGridViewTextBoxColumn c2,c3,c4,c5,c6;
         foreach (DataColumn dc in dt_taizhang.Columns)
            {
             switch(dc.ColumnName)
            {
                case "委托单号":
                    c1 = new DataGridViewComboBoxColumn();
                    c1.DataPropertyName = dc.ColumnName;
                    c1.HeaderText = dc.ColumnName;
                    c1.Name = dc.ColumnName;
                    c1.DataPropertyName = dc.ColumnName;
                    c1.SortMode = DataGridViewColumnSortMode.Automatic;
                    c1.ValueType = dc.DataType;
                    OleDbDataAdapter danhao_search = new OleDbDataAdapter("select 委托单号 from Gamma射线辐射灭菌委托单", mySystem.Parameter.connOle);
                    DataTable da_danhao = new DataTable("委托单号查询");
                    danhao_search.Fill(da_danhao);
                    foreach (DataRow tdr in da_danhao.Rows)
                    {
                        c1.Items.Add(tdr["委托单号"]);
                    }
                    dataGridView1.Columns.Add(c1);
                    break;
                 case "产品数量箱":
                c2= new DataGridViewTextBoxColumn();
                c2.DataPropertyName = dc.ColumnName;
                c2.HeaderText = "产品数量(箱)";
                c2.Name = dc.ColumnName;
                c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                c2.ValueType = dc.DataType;
                dataGridView1.Columns.Add(c2);
                break;
                 case "产品数量只":
                c3 = new DataGridViewTextBoxColumn();
                c3.DataPropertyName = dc.ColumnName;
                c3.HeaderText = "产品数量(只)";
                c3.Name = dc.ColumnName;
                c3.SortMode = DataGridViewColumnSortMode.NotSortable;
                c3.ValueType = dc.DataType;
                dataGridView1.Columns.Add(c3);
                break;
                 case "送去产品托盘数量个":
                c4 = new DataGridViewTextBoxColumn();
                c4.DataPropertyName = dc.ColumnName;
                c4.HeaderText = "送去产品托盘数量(个)";
                c4.Name = dc.ColumnName;
                c4.SortMode = DataGridViewColumnSortMode.NotSortable;
                c4.ValueType = dc.DataType;
                dataGridView1.Columns.Add(c4);
                break;
                 case "拉回产品托盘数量个":
                c5 = new DataGridViewTextBoxColumn();
                c5.DataPropertyName = dc.ColumnName;
                c5.HeaderText = "拉回产品托盘数量(个)";
                c5.Name = dc.ColumnName;
                c5.SortMode = DataGridViewColumnSortMode.NotSortable;
                c5.ValueType = dc.DataType;
                dataGridView1.Columns.Add(c5);
                break;
                 default:
                c6 = new DataGridViewTextBoxColumn();
                c6.DataPropertyName = dc.ColumnName;
                c6.HeaderText = dc.ColumnName;
                c6.Name = dc.ColumnName;
                c6.SortMode = DataGridViewColumnSortMode.Automatic;
                c6.ValueType = dc.DataType;
                dataGridView1.Columns.Add(c6);
                break;

            }
            }

        }

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr= dt_taizhang.NewRow();
            dt_taizhang.Rows.Add(dr);
        }
    }
}
