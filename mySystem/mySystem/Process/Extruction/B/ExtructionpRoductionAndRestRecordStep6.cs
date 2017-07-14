using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using System.Data.OleDb;
using Newtonsoft.Json.Linq;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionpRoductionAndRestRecordStep6 : BaseForm
    {
        //private DataTable dtInformation = new DataTable();

        private String table = "吹膜工序生产和检验记录";
        private String tableInfo = "吹膜工序生产和检验记录详细信息";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;
        private CheckForm check = null;

        private int[] sum = { 0, 0 };

        private DataTable dt记录, dt记录详情, dt代码批号, dt工艺设备;
        private OleDbDataAdapter da记录, da记录详情;
        private BindingSource bs记录, bs记录详情;
        private OleDbCommandBuilder cb记录, cb记录详情;

        private Int32 KeyID = 1;

        //找不到人，弹窗不太好看
        public ExtructionpRoductionAndRestRecordStep6(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            Instructionid = Parameter.proInstruID;
            
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            Init();
            GetProductInfo();

        }

        //******************************初始化******************************//

        //新建BindingSource等等、控件不可用
        private void Init()
        {     
            foreach(Control c in this.Controls){c.Enabled = false;}
            cb产品名称.Enabled = true;
            dtp生产日期.Enabled = true;
            tb累计同规格膜卷长度R.ReadOnly = true;
            tb累计同规格膜卷重量T.ReadOnly = true;            
        }

        //产品名称、产品批号列表获取+产品工艺、设备、班次填写
        private void GetProductInfo()
        {
            dt工艺设备 = new DataTable("工艺设备");
            dt代码批号 = new DataTable("代码批号");

            //*********产品名称、产品批号、产品工艺、设备 -----> 数据获取*********//
            if (!isSqlOk)
            {
                //工艺、设备编号表格新建                
                dt工艺设备.Columns.Add("依据工艺", typeof(String));   //新建第一列
                dt工艺设备.Columns.Add("生产设备", typeof(String));      //新建第二列

                //从 “生产指令信息表” 中找 “生产指令编号” 下的信息
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 生产指令信息表 where 生产指令编号 = '" + mySystem.Parameter.proInstruction + "' ";//这里应有生产指令编码
                OleDbDataReader reader1 = comm1.ExecuteReader();
                if (reader1.Read())
                {
                    //填入生产工艺、生产设备编号
                    dt工艺设备.Rows.Add(reader1["生产工艺"].ToString(), reader1["生产设备编号"].ToString());

                    //查找该生产ID下的产品编码、产品批号
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
                            cb产品名称.Items.Add(dt代码批号.Rows[i]["产品编码"].ToString());//添加
                        }
                    }
                }
                else
                {
                    //填入生产工艺、生产设备编号
                    dt工艺设备.Rows.Add(" ", " ");

                    //填入产品编码、批号
                    dt代码批号.Columns.Add("产品编码", typeof(String));   //新建第一列
                    dt代码批号.Columns.Add("产品批号", typeof(String));      //新建第二列
                    dt代码批号.Rows.Add(" ", " ");
                }
                reader1.Dispose();
            }
            else
            {
                //从SQL数据库中读取;
                
            }

            //MessageBox.Show(dt代码批号.Rows.Count.ToString());

            //*********数据填写*********//
            cb产品名称.SelectedIndex = 0;
            tb产品批号.Text = dt代码批号.Rows[0]["产品批号"].ToString();
            tb生产设备.Text = dt工艺设备.Rows[0]["生产设备"].ToString();
            tb依据工艺.Text = dt工艺设备.Rows[0]["依据工艺"].ToString();
            //cb白班.Checked = mySystem.Parameter.userflight;
            //cb夜班.Checked = !mySystem.Parameter.userflight;
        }

        //控件格式（先绑定！！！）
        private void formatInit()
        {
            this.dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView1.AllowUserToAddRows = false;
            //设置对齐
            this.dataGridView1.RowHeadersVisible = false;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView1.Columns[i].MinimumWidth = 80;
            }
            this.dataGridView1.Columns[0].MinimumWidth = 40;
            this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersHeight = 40;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["T吹膜工序生产和检验记录ID"].Visible = false;

            dataGridView1.Columns["序号"].ReadOnly = true;

            //白班、夜班
            cb白班.Enabled = false;
            cb夜班.Enabled = false;
        }

        //******************************显示数据*********************dt*********//

        //显示根据信息查找
        private void DataShow(String productName, Boolean flight, DateTime searchTime)
        {
            tb环境湿度.Enabled = true;
            tb环境温度.Enabled = true;
            dtp生产日期.Enabled = true;
            dataGridView1.Enabled = true;
            SaveBtn.Enabled = true;

            //******************************外表 根据条件绑定******************************//  
            readOuterData(productName, flight, searchTime);
            outerBind();

            MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************// 

            Boolean isnew = true;
            if (dt记录.Rows.Count == 0)
            {
                //********* 外表新建、保存、重新绑定 *********//
                isnew = true;
                //初始化外表这一行
                DataRow dr1 = dt记录.NewRow();
                dr1 = writeOuterDefault(dr1);
                dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);
                //立马保存这一行
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);                
                //外表重新绑定
                readOuterData(productName, flight, searchTime);
                outerBind();

                //********* 内表新建、保存、重新绑定 *********//

                //获取外表主键
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = mySystem.Parameter.connOle;
                comm.CommandText = "select @@identity";
                Int32 idd1 = (Int32)comm.ExecuteScalar();
                KeyID = idd1;
                //内表绑定
                readInnerData(KeyID);
                innerBind();
                DataRow dr2 = dt记录详情.NewRow();
                writeInnerDefault(KeyID, dr2);
                dt记录详情.Rows.InsertAt(dr2, dt记录详情.Rows.Count);
                setDataGridViewRowNums();

                //立马保存内表
                da记录详情.Update((DataTable)bs记录详情.DataSource);
                //内表重新绑定
                readInnerData(KeyID);
                innerBind();
            }
            else 
            {
                isnew = false;
                KeyID = Convert.ToInt32(dt记录.Rows[0]["ID"].ToString());
                //内表绑定
                readInnerData(KeyID);
                innerBind();
            }
            
            //********* 控件可用性 *********//
            if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"].ToString()) == true)
            {
                Init();
                printBtn.Enabled = true;
            }
            else
            {
                if (isnew ==true)
                {
                    //新建表
                    dataGridView1.ReadOnly = false;
                    SaveBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = true;
                }
                else
                {
                    //非新建表
                    dataGridView1.ReadOnly = false;
                    for (int i = 0; i < dt记录详情.Rows.Count; i++)
                    {
                        if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["检查人"].ToString()) == 0)
                        { dataGridView1.Rows[i].ReadOnly = true; }                        
                    }
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    AddLineBtn.Enabled = true;
                    DelLineBtn.Enabled = true;
                }                
            }
            formatInit();
        }

        //根据主键显示
        public void IDShow(Int32 ID)
        {
            OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + table + " where ID = " + ID.ToString(), connOle);
            DataTable dt1 = new DataTable(table);
            da1.Fill(dt1);

            DataShow(dt1.Rows[0]["产品名称"].ToString(), Convert.ToBoolean(dt1.Rows[0]["班次"].ToString()), Convert.ToDateTime(dt1.Rows[0]["生产日期"].ToString()));
        }
             
        //****************************** 嵌套 ******************************//
        
        //外表读数据，填datatable
        private void readOuterData(String productName, Boolean flight, DateTime searchTime)
        {            
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 产品名称 = '" + productName + "' and 班次 = " + flight.ToString() + " and 生产日期 = #" + searchTime.ToString("yyyy/MM/dd") + "# ", connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }

        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            //控件绑定（先解除，再绑定）
            cb产品名称.DataBindings.Clear();
            cb产品名称.DataBindings.Add("SelectedItem", bs记录.DataSource, "产品名称");
            tb产品批号.DataBindings.Clear();
            tb产品批号.DataBindings.Add("Text", bs记录.DataSource, "产品批号");
            tb依据工艺.DataBindings.Clear();
            tb依据工艺.DataBindings.Add("Text", bs记录.DataSource, "依据工艺");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            cb白班.DataBindings.Clear();
            cb白班.DataBindings.Add("Checked", bs记录.DataSource, "班次");
            tb环境温度.DataBindings.Clear();
            tb环境温度.DataBindings.Add("Text", bs记录.DataSource, "环境温度");
            tb环境湿度.DataBindings.Clear();
            tb环境湿度.DataBindings.Add("Text", bs记录.DataSource, "环境湿度");
            tb生产设备.DataBindings.Clear();
            tb生产设备.DataBindings.Add("Text", bs记录.DataSource, "生产设备");
            dtp生产日期.DataBindings.Clear();
            dtp生产日期.DataBindings.Add("Text", bs记录.DataSource, "生产日期");
            tb累计同规格膜卷长度R.DataBindings.Clear();
            tb累计同规格膜卷长度R.DataBindings.Add("Text", bs记录.DataSource, "累计同规格膜卷长度R");
            tb累计同规格膜卷重量T.DataBindings.Clear();
            tb累计同规格膜卷重量T.DataBindings.Add("Text", bs记录.DataSource, "累计同规格膜卷重量T");
        }

        //内表读数据，填datatable
        private void readInnerData(Int32 ID)
        {            
            bs记录详情 = new BindingSource();
            dt记录详情 = new DataTable(tableInfo);
            da记录详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T吹膜工序生产和检验记录ID = " + ID.ToString(), connOle);
            cb记录详情 = new OleDbCommandBuilder(da记录详情);
            da记录详情.Fill(dt记录详情);
        }

        //内表控件绑定
        private void innerBind()
        {
            bs记录详情.DataSource = dt记录详情;
            dataGridView1.DataBindings.Clear();
            dataGridView1.DataSource = bs记录详情.DataSource;
        }

        //添加行代码
        private DataRow writeInnerDefault(Int32 ID, DataRow dr)
        {
            //DataRow dr = dt记录详情.NewRow();
            dr["T吹膜工序生产和检验记录ID"] = ID;
            dr["记录人"] = mySystem.Parameter.userName;
            dr["检查人"] = " ";
            dr["外观_是"] = true;
            dr["外观_否"] = false;
            dr["判定_是"] = true;
            dr["判定_否"] = false;
            //dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
            return dr;
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["产品名称"] = cb产品名称.Text;
            dr["产品批号"] = dt代码批号.Rows[cb产品名称.FindString(cb产品名称.Text)]["产品批号"].ToString();
            dr["生产日期"] = Convert.ToDateTime(dtp生产日期.Value.ToString("yyyy/MM/dd"));
            dr["班次"] = cb白班.Checked;
            dr["依据工艺"] = dt工艺设备.Rows[0]["依据工艺"];
            dr["生产设备"] = dt工艺设备.Rows[0]["生产设备"];
            dr["审核是否通过"] = false;
            return dr;
        }
        
        //******************************按钮功能******************************//

        //添加行按钮
        private void AddLineBtn_Click_1(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(dt记录详情.Rows[dt记录详情.Rows.Count-1]["记录人"].ToString()) == 0)
            {
                dt记录详情.Rows[dt记录详情.Rows.Count - 1]["记录人"] = mySystem.Parameter.userName;
                MessageBox.Show("请重新输入最后一行的记录人信息信息", "ERROR");
            }
            else if (mySystem.Parameter.NametoID(dt记录详情.Rows[dt记录详情.Rows.Count - 1]["检查人"].ToString()) == 0)
            {
                dt记录详情.Rows[dt记录详情.Rows.Count - 1]["检查人"] = " ";
                MessageBox.Show("请重新输入最后一行的检查人信息信息", "ERROR");
            }
            else
            {
                DialogResult drReadOnly = MessageBox.Show("确认后将不可再修改以上数据，确认要新建记录吗？", "提示", MessageBoxButtons.OKCancel);
                if (drReadOnly == DialogResult.OK)
                {
                    //用户选择确认的操作
                    //MessageBox.Show("您选择的是【确认】");
                    dataGridView1.Rows[dt记录详情.Rows.Count - 1].ReadOnly = true;
                    DataRow dr = dt记录详情.NewRow();
                    writeInnerDefault(KeyID, dr);
                    dt记录详情.Rows.InsertAt(dr, dt记录详情.Rows.Count);
                    setDataGridViewRowNums();
                }
                else if (drReadOnly == DialogResult.Cancel)
                {
                    //用户选择取消的操作
                    //MessageBox.Show("您选择的是【取消】");
                }
            }
        }
        
        //删除行按钮  只允许删除没有检查人的行
        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (dt记录详情.Rows.Count >= 2)
            {
                int deletenum = dataGridView1.CurrentRow.Index;
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[deletenum]["检查人"].ToString()) == 0)
                { 
                    dt记录详情.Rows.RemoveAt(deletenum);
                    getTotal();
                }
            }
        }

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Name_check1() == false)
            { /*记录人不合格*/}
            if (Name_check2() == false)
            { /*检查人不合格*/}
            else if (TextBox_check() == false)
            {/*环境温度、环境湿度不合格*/ }
            else
            {
                if (isSqlOk)
                { }
                else
                {
                    // 内表保存
                    da记录详情.Update((DataTable)bs记录详情.DataSource);
                    readInnerData(Convert.ToInt32(dt记录.Rows[0]["ID"]));
                    innerBind();

                    //外表保存
                    bs记录.EndEdit();
                    da记录.Update((DataTable)bs记录.DataSource);
                    readOuterData(cb产品名称.Text, cb白班.Checked, dtp生产日期.Value);
                    outerBind();

                }
            }            
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            if (check.ischeckOk == true)
            {
                dt记录.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
                dt记录.Rows[0]["审核意见"] = check.opinion;
                dt记录.Rows[0]["审核是否通过"] = check.ischeckOk;

                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);

                Init();
                printBtn.Enabled = true;
            } 
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        //******************************小功能******************************//  

        //序号刷新
        private void setDataGridViewRowNums()
        {
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            { dt记录详情.Rows[i]["序号"] = (i + 1); }
        }

        //求合计
        private void getTotal()
        {
            int numtemp;
            // 膜卷长度求和
            sum[0] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["膜卷长度"].ToString(), out numtemp) == true)
                { sum[0] += numtemp; }
            }
            dt记录.Rows[0]["累计同规格膜卷长度R"] = sum[0];
            // 膜卷重量求和
            sum[1] = 0;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (Int32.TryParse(dt记录详情.Rows[i]["膜卷重量"].ToString(), out numtemp) == true)
                { sum[1] += numtemp; }
            }
            dt记录.Rows[0]["累计同规格膜卷重量T"] = sum[1];            
        }

        // 检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb环境温度, tb环境湿度});
            List<String> StringList = new List<String>(new String[] { "环境温度", "环境湿度" });
            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show(StringList[i] + "框内应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }

        // 检查 记录人的姓名
        private bool Name_check1()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["记录人"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["记录人"] = mySystem.Parameter.userName;
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的检查人信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
        }

        // 检查 检查人的姓名
        private bool Name_check2()
        {
            bool TypeCheck = true;
            for (int i = 0; i < dt记录详情.Rows.Count; i++)
            {
                if (mySystem.Parameter.NametoID(dt记录详情.Rows[i]["检查人"].ToString()) == 0)
                {
                    dt记录详情.Rows[i]["检查人"] = "";
                    MessageBox.Show("请重新输入" + (i + 1).ToString() + "行的检查人信息", "ERROR");
                    TypeCheck = false;
                }
            }
            return TypeCheck;
        }

        //******************************选择条件******************************//  

        //产品名称改变
        private void cb产品名称_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataShow(cb产品名称.Text, cb白班.Checked, dtp生产日期.Value);
        }

        //生产日期
        private void dtp生产日期_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataShow(cb产品名称.Text, cb白班.Checked, dtp生产日期.Value);
        }
        
        //******************************datagridview******************************//  

        // 处理DataGridView中数据类型输错的函数
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String Columnsname = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            String rowsname = (((DataGridView)sender).SelectedCells[0].RowIndex + 1).ToString(); ;
            MessageBox.Show("第" + rowsname + "行的" + Columnsname + "填写错误");
        }

        //实时求合计、检查人名合法性
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷长度")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "膜卷重量")
                {
                    getTotal();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "记录人")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["记录人"] = mySystem.Parameter.userName;
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『记录人』信息", "ERROR");
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "检查人")
                {
                    if (mySystem.Parameter.NametoID(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
                    {
                        dt记录详情.Rows[e.RowIndex]["检查人"] = " ";
                        MessageBox.Show("请重新输入" + (e.RowIndex + 1).ToString() + "行的『检查人』信息", "ERROR");
                    }
                }
                else
                { }
            }
        }

        //是否勾选
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dt记录详情.Rows.Count > 0 && dt记录.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"]) == false)
                {
                    //外观
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "外观_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["外观_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "外观_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["外观_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["外观_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["外观_否"] = false;
                        }
                    }
                    // 判定
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "判定_是")
                    {

                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["判定_是"]) == true)
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "判定_否")
                    {
                        if (Convert.ToBoolean(dt记录详情.Rows[e.RowIndex]["判定_否"]) == false)
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = false;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = true;
                        }
                        else
                        {
                            dt记录详情.Rows[e.RowIndex]["判定_是"] = true;
                            dt记录详情.Rows[e.RowIndex]["判定_否"] = false;
                        }
                    }
                    else
                    { }
                }
            }
        }   
    
    }
}
