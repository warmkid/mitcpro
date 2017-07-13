using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Extruction.Chart
{
    public partial class outerpack :   mySystem.BaseForm
    {

        private String table = "产品外包装记录表";
        private String tableInfo = "产品外包装详细记录";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private Boolean isSqlOk;
        private CheckForm check = null;

        private Boolean isNewRecord = false;
        
        private DataTable dt代码批号 = new DataTable(); //ID, 产品编码, 产品批号

        private DataTable dt记录, dt记录详情;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        public outerpack(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            GetProductInfo();
        }

        public void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from "+ table +" where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                //MessageBox.Show("有数据");
                DSBinding(Convert.ToInt32(reader1["生产指令ID"].ToString()), reader1["产品代码"].ToString(), 0, Convert.ToDateTime(reader1["包装日期"].ToString()));
            }            
        }

        //******************************初始化******************************//

        //产品代码、产品批号初始化
        private void GetProductInfo()
        {          
            if (!isSqlOk)
            {
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + mySystem.Parameter.proInstruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select ID, 产品编码, 产品批号 from 生产指令产品列表 where 生产指令ID = " + reader1["ID"].ToString();

                    OleDbDataAdapter datemp = new OleDbDataAdapter(comm2);
                    datemp.Fill(dt代码批号);
                    if (dt代码批号.Rows.Count == 0)
                    {
                        datemp.Dispose();
                        return;
                    }
                    else
                    {
                        datemp.Dispose();
                        for (int i = 0; i < dt代码批号.Rows.Count; i++)
                        {
                            cb产品代码.Items.Add(dt代码批号.Rows[i][1].ToString());//添加
                        }                        
                    }
                    cb产品代码.SelectedIndex = -1;
                }
                else
                {
                    reader1.Dispose();
                    return;
                }
            }
            else
            {
                
            }

        }
        
        //控件属性（先绑定！！！）
        private void formatInit()
        {
            tb产品批号.ReadOnly = true;
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView1.Columns[i].MinimumWidth = 120;
            }
            this.dataGridView1.Columns["序号"].MinimumWidth = 60;
            this.dataGridView1.Columns["序号"].ReadOnly = true;
            this.dataGridView1.Columns["包装明细"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["T产品外包装记录ID"].Visible = false;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        //******************************显示数据******************************//

        private void DSBinding(Int32 ID, String ProductCode,Int32 ProductCodeIndex, DateTime searchTime)
        {
            //Dispose
            if (da记录 != null)
            {
                bs记录.Dispose();
                dt记录.Dispose();
                da记录.Dispose();
                cb记录.Dispose();
            }
            if (da记录详情 != null)
            {
                bs记录详情.Dispose();
                dt记录详情.Dispose();
                da记录详情.Dispose();
                cb记录详情.Dispose();
            }

            //******************************外部控件******************************//  
            // 新建
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            //连数据库
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + ID.ToString() + " and 产品代码 = '" + ProductCode + "' and 包装日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            cb产品代码.DataBindings.Clear();
            cb产品代码.DataBindings.Add("SelectedItem", bs记录.DataSource, "产品代码");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            dtp包装日期.DataBindings.Clear();
            dtp包装日期.DataBindings.Add("Value", bs记录.DataSource, "包装日期");
            tb包装规格每包只数.DataBindings.Clear();
            tb包装规格每包只数.DataBindings.Add("Text", bs记录.DataSource, "包装规格每包只数");
            tb包装规格每包重量.DataBindings.Clear();
            tb包装规格每包重量.DataBindings.Add("Text", bs记录.DataSource, "包装规格每包重量");
            tb包装规格每箱包数.DataBindings.Clear();
            tb包装规格每箱包数.DataBindings.Add("Text", bs记录.DataSource, "包装规格每箱包数");
            tb包装规格每箱重量.DataBindings.Clear();
            tb包装规格每箱重量.DataBindings.Add("Text", bs记录.DataSource, "包装规格每箱重量");
            tb产品数量箱数.DataBindings.Clear();
            tb产品数量箱数.DataBindings.Add("Text", bs记录.DataSource, "产品数量箱数");
            tb产品数量只数.DataBindings.Clear();
            tb产品数量只数.DataBindings.Add("Text", bs记录.DataSource, "产品数量只数");
            tb包材用量箱数.DataBindings.Clear();
            tb包材用量箱数.DataBindings.Add("Text", bs记录.DataSource, "包材用量箱数");
            tb包材用量标签数量.DataBindings.Clear();
            tb包材用量标签数量.DataBindings.Add("Text", bs记录.DataSource, "包材用量标签数量");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            tb操作人.DataBindings.Clear();
            tb操作人.DataBindings.Add("Text", bs记录.DataSource, "操作人");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp操作日期.DataBindings.Clear();
            dtp操作日期.DataBindings.Add("Text", bs记录.DataSource, "操作日期");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bs记录.DataSource, "审核日期");
            
            //待注释
            //MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************//            
            // 新建Binding Source
            bs记录详情 = new BindingSource();  //表格内部
            // 新建DataTable
            dt记录详情 = new DataTable(table);  //表格内部
            // 数据库->DataTable
            if (dt记录.Rows.Count == 0)
            {
                isNewRecord = true;
                //为记录新建一行
                DataRow dr1 = dt记录.NewRow();
                dr1["生产指令ID"] = mySystem.Parameter.proInstruID;
                dr1["产品代码"] = cb产品代码.Text;
                dr1["产品批号"] = dt代码批号.Rows[ProductCodeIndex][2].ToString();
                dr1["包装日期"] = Convert.ToDateTime(searchTime.ToString("yyyy/MM/dd"));
                dr1["备注"] = " ";
                dr1["操作人"] = mySystem.Parameter.userName;
                dr1["操作日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                //防止违反并发性，提前赋值
                dr1["审核人"] = " ";
                dr1["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")); //违反并发性
                dr1["审核意见"] = " ";
                dr1["审核是否通过"] = false; //违反并发性
                dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);

                // 数据库->DataTable
                da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where 0 = 1", connOle);
                cb记录详情 = new OleDbCommandBuilder(da记录详情);
                da记录详情.Fill(dt记录详情);
                // DataTable->BindingSource
                bs记录详情.DataSource = dt记录详情;
                //控件绑定（先解除，再绑定）
                dataGridView1.DataBindings.Clear();
                dataGridView1.DataSource = bs记录详情.DataSource;

                //初始化新的表格
                if (dt记录详情.Rows.Count == 0)
                {
                    AddLine();
                }

                SaveBtn.Enabled = true;
                AddBtn.Enabled = true;
                DelBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;
                cons_change();
            }
            else
            {
                isNewRecord = false;
                // 数据库->DataTable
                da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T产品外包装记录ID = " + dt记录.Rows[0]["ID"].ToString(), connOle);
                cb记录详情 = new OleDbCommandBuilder(da记录详情);
                da记录详情.Fill(dt记录详情);
                // DataTable->BindingSource
                bs记录详情.DataSource = dt记录详情;
                //控件绑定（先解除，再绑定）
                dataGridView1.DataBindings.Clear();
                dataGridView1.DataSource = bs记录详情.DataSource;               

                if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]) == true)
                {
                    SaveBtn.Enabled = false;
                    AddBtn.Enabled = false;
                    DelBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                    printBtn.Enabled = true;
                    cons_change_not();
                }
                else
                {
                    SaveBtn.Enabled = true;
                    AddBtn.Enabled = true;
                    DelBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                    cons_change();
                }
            }
            formatInit();                              
        }
        
        //******************************按钮功能******************************//

        //添加按钮
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddLine();
        }
        
        //添加行代码
        private void AddLine()
        {
            DataRow dr = dt记录详情.NewRow();
            dr["是否贴标签_是"] = true;
            dr["是否贴标签_否"] = false;
            dr["是否打开包封箱_是"] = true;
            dr["是否打开包封箱_否"] = false;
            dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            num_fresh();
        }

        //删除按钮
        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index >= 0)
            {
                dt记录详情.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                num_fresh();
            }
        }

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (TextBox_check() == false)
            { }
            else if (mySystem.Parameter.NametoID(tb操作人.Text) == 0)
            { MessageBox.Show("请重新填写操作人姓名！"); }
            else
            {
                // 保存非DataGridView中的数据必须先执行EndEdit;
                for (int i = 0; i < dt记录.Rows.Count; i++)
                {
                    dt记录.Rows[i]["包装日期"] = Convert.ToDateTime((Convert.ToDateTime(dt记录.Rows[i]["包装日期"].ToString())).ToString("yyyy/MM/dd"));
                }

                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);                

                //  dgv  保存，清空 重读            
                   if (isNewRecord == true)
                {
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = mySystem.Parameter.connOle;
                    comm.CommandText = "select @@identity";
                    Int32 idd = (Int32)comm.ExecuteScalar();

                    dt记录.Clear();
                    da记录.Fill(dt记录详情);

                    da记录详情.Dispose();
                    cb记录详情.Dispose();

                    //重新绑定
                    da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T产品外包装记录ID = " + idd.ToString(), connOle);
                    cb记录详情 = new OleDbCommandBuilder(da记录详情);
                    da记录详情.Fill(dt记录详情);
                    // DataTable->BindingSource
                    bs记录详情.DataSource = dt记录详情;
                    // 先clear所有bindings
                    dataGridView1.DataBindings.Clear();
                    // 控件->BindingSource
                    dataGridView1.DataSource = bs记录详情.DataSource;

                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    { dt记录详情.Rows[i]["T产品外包装记录ID"] = idd; }
                    isNewRecord = false;
                }
                else
                {
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    { dt记录详情.Rows[i]["T产品外包装记录ID"] = dt记录详情.Rows[0]["T产品外包装记录ID"]; }
                }
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                dt记录详情.Clear();
                da记录详情.Fill(dt记录详情);

                CheckBtn.Enabled = true;
            }
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            dt记录.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
            dt记录.Rows[0]["审核意见"] = check.opinion;
            dt记录.Rows[0]["审核是否通过"] = check.ischeckOk;

            // 保存非DataGridView中的数据必须先执行EndEdit;
            bs记录.EndEdit();
            da记录.Update((DataTable)bs记录.DataSource);

            if (check.ischeckOk == true)
            {
                SaveBtn.Enabled = false;
                AddBtn.Enabled = false;
                DelBtn.Enabled = false;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
                cons_change_not();
            }
            else
            {
                SaveBtn.Enabled = true;
                AddBtn.Enabled = true;
                DelBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
                cons_change();
            }
        }

        //******************************选择条件******************************//  

        // 产品代码value changed
        private void cb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DSBinding(mySystem.Parameter.proInstruID, cb产品代码.Text.ToString(), cb产品代码.SelectedIndex, dtp包装日期.Value); }
           
        }

        // 包装日期value changed
        private void dtp包装日期_ValueChanged(object sender, EventArgs e)
        {
            if (cb产品代码.SelectedIndex >= 0)
            { DSBinding(mySystem.Parameter.proInstruID, cb产品代码.Text.ToString(), cb产品代码.SelectedIndex, dtp包装日期.Value); }
        }

        //******************************小功能******************************//  

        //更新序号
        private void num_fresh()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
        }

        //数据可修改
        private void cons_change()
        {
            dataGridView1.ReadOnly = false;

            tb包装规格每包只数.ReadOnly = false;
            tb包装规格每包重量.ReadOnly = false;
            tb包装规格每箱包数.ReadOnly = false;
            tb包装规格每箱重量.ReadOnly = false;
            tb产品数量箱数.ReadOnly = false;
            tb产品数量只数.ReadOnly = false;
            tb包材用量箱数.ReadOnly = false;
            tb包材用量标签数量.ReadOnly = false;
            tb备注.ReadOnly = false;
            tb操作人.ReadOnly = false;
            tb审核人.ReadOnly = false;

            dtp操作日期.Enabled = true;
            dtp审核日期.Enabled = true;            
        }

        //数据不可修改
        private void cons_change_not()
        {
            dataGridView1.ReadOnly = true;

            tb包装规格每包只数.ReadOnly = true;
            tb包装规格每包重量.ReadOnly = true;
            tb包装规格每箱包数.ReadOnly = true;
            tb包装规格每箱重量.ReadOnly = true;
            tb产品数量箱数.ReadOnly = true;
            tb产品数量只数.ReadOnly = true;
            tb包材用量箱数.ReadOnly = true;
            tb包材用量标签数量.ReadOnly = true;
            tb备注.ReadOnly = true;
            tb操作人.ReadOnly = true;
            tb审核人.ReadOnly = true;

            dtp操作日期.Enabled = false;
            dtp审核日期.Enabled = false; 
        }
        
        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb包装规格每包只数, tb包装规格每包重量, tb包装规格每箱包数, tb包装规格每箱重量, 
                tb产品数量箱数, tb产品数量只数, tb包材用量箱数, tb包材用量标签数量});
            List<String> StringList = new List<String>(new String[] {"包装规格每包只数","包装规格每包重量","包装规格每箱包数","包装规格每箱重量",
                "产品数量箱数","产品数量只数","包材用量箱数","包材用量标签数量"  });

            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show(StringList[i] + " 框内应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }

            return TypeCheck;
        }

        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex+1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的" + Columnsname + "填写错误");
        }
        
        // 处理DataGridView中是、否相反
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dt记录详情.Rows.Count>0 && dt记录.Rows.Count>0)
            {
                if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]) == false)
                {
                    //是否贴标签
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "是否贴标签_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["是否贴标签_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "是否贴标签_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["是否贴标签_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["是否贴标签_否"] = false;
                        }
                    }
                    // 是否打开包封箱
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "是否打开包封箱_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["是否打开包封箱_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "是否打开包封箱_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["是否打开包封箱_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["是否打开包封箱_否"] = false;
                        }
                    }
                    else
                    { }
                }
            }
        }   
    
    }
}
