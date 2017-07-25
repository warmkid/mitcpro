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
        DataGridViewComboBoxColumn c1;

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
            setDataGridViewFormat();
            bs_taizhang.DataSource = dt_taizhang;
            dataGridView1.DataSource = bs_taizhang.DataSource;
            //第一列ID不显示
            dataGridView1.Columns[0].Visible = false;
        }
        // 设置自动计算类事件
        private void addComputerEventHandler()
        {
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            { 
                case 3:
                    
                    break;

            }
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
            
            DataGridViewTextBoxColumn c2;
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
                 default:
                c2 = new DataGridViewTextBoxColumn();
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

        //设置DataGridView中各列的格式+设置datagridview基本属性
        private void setDataGridViewFormat()
        {
            //dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;
            //dataGridView1.Columns["ID"].Visible = false;
            //dataGridView1.Columns["T吹膜工序生产和检验记录ID"].Visible = false;
            //dataGridView1.Columns["序号"].ReadOnly = true;
            dataGridView1.Columns["送去产品托盘数量个"].HeaderText = "送去产品托盘数量(个)";
            dataGridView1.Columns["拉回产品托盘数量个"].HeaderText = "拉回产品托盘数量(个)";
            dataGridView1.Columns["产品数量只"].HeaderText = "产品数量(只)";
            dataGridView1.Columns["产品数量箱"].HeaderText = "产品数量(箱)";
        }

        //添加新行
        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr= dt_taizhang.NewRow();
            dr = writeInnerDefault(dr);
            dt_taizhang.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        //设置序号递增
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        //写默认行数据
        DataRow writeInnerDefault(DataRow dr)
        {
            
           // dr["产品名称"] = c1.Text;
           // dr["产品批号"] = dt_taizhang.Rows[c1.FindString(cb_taizhang.Text)]["产品批号"].ToString();
            dr["委托单号"]=dataGridView1[0,0].Value.ToString();
            //dr["序号"] = dt_taizhang.Rows[0]["ID"];
            //dr["序号"] = 0;
            //dr["数量卷"] = 0;
            //dr["数量米"] = 0;
            return dr;
        }

        //保存数据到数据库
        private void bt保存_Click(object sender, EventArgs e)
        {
            bs_taizhang.EndEdit();
            da_taizhang.Update((DataTable)bs_taizhang.DataSource);
            readInnerData();
            innerBind();
        }
    }
}
