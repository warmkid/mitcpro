using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using Newtonsoft.Json.Linq;

namespace mySystem.Extruction.Process
{
    public partial class ProductInnerPackagingRecord : BaseForm
    {
        private String table = "product_inner_packaging_record";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;

        private int operator_id;
        private string operator_name;
        private DateTime operate_date;
        private int review_id;
        private string reviewer_name;
        private DateTime review_date;

        private CheckForm check = null;
        private bool ischeckOk = false;
        private bool isSaveOk = false;
        JArray jarray1 = JArray.Parse("[]");
        private bool isReadOnly = false;

        //问题：包装人是否要存id？
        public ProductInnerPackagingRecord(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            Instructionid = Parameter.proInstruID;
            
            ChartOpenInit();
            DgvInit();
            InfInit();

            DataShow(productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd")));
        }

        private void ChartOpenInit()
        {
            //产品代码初始化
            if (!isSqlOk)
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = Parameter.connOle;
                comm.CommandText = "select product_code from product_aoxing";
                OleDbDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productcodeBox.Items.Add(reader["product_code"]);  //下拉框获取生产指令
                    }
                }
            }
            else
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = Parameter.conn;
                comm.CommandText = "select product_code from product_aoxing";
                SqlDataReader reader = comm.ExecuteReader();//执行查询
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productcodeBox.Items.Add(reader["product_code"]);
                    }
                }
            }
            productcodeBox.SelectedIndex = 0;
            //生产日期初始化
            productionTimePicker.Value = DateTime.Now;

        }

        private void DgvInit()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView1.Columns[i].MinimumWidth = 70;
            }
            this.dataGridView1.Columns["序号"].MinimumWidth = 60;                
            this.dataGridView1.Columns["序号"].ReadOnly = true;
            this.dataGridView1.Columns["生产时间"].MinimumWidth = 100;
            this.dataGridView1.Columns["生产时间"].ReadOnly = true;
            this.dataGridView1.Columns["内包序号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns["包装人"].MinimumWidth = 100;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //表二
            this.dataGridView2.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView2.Columns.Count; i++)
            {
                this.dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView2.Columns[i].MinimumWidth = 80;
            }
            this.dataGridView2.Columns["批号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView2.Columns["包材"].MinimumWidth = 150;

            this.dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AddRowLine2();
            AddRowLine2();
            dataGridView2.Rows[0].Cells[0].Value = "BZD-";
            dataGridView2.Rows[1].Cells[0].Value = "指示剂";          
        
        }

        private void InfInit()
        {
            //记录人初始化
            operator_id = Parameter.userID;
            operator_name = Parameter.userName;
            this.recorderBox.Text = operator_name;
            operate_date = DateTime.Now.Date;
            recordTimePicker.Value = operate_date;
            //审核人初始化
            review_id = 0;
            reviewer_name = null;
            this.checkerBox.Text = reviewer_name;
            review_date = DateTime.Now.Date;
            checkTimePicker.Value = review_date;
            ischeckOk = false;
            //表格信息1
            dataGridView1.Rows.Clear();
            AddRowLine();
            //表格信息2
            dataGridView2.Rows.Clear();
            AddRowLine2();
            AddRowLine2();
            dataGridView2.Rows[0].Cells[0].Value = "BZD-";
            dataGridView2.Rows[1].Cells[0].Value = "指示剂";
            //其他信息
            batchIdBox.Text = "";
            label_release_quantity_Box.Text = "";
            label_use_quantiy_Box.Text = "";
            label_destroy_quantity_Box.Text = "";
            packing_pattern_quantity_Box.Text = "";
            total_quantiy_Box.Text = "";
            quantity_per_piece_Box.Text = "";
            checkBox1.Checked = true;//中文
            checkBox1.Checked = true;//英文
        }

        //控制各种控件是否可见
        private void ConsChanged(bool canChange)
        {
            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { batchIdBox, label_release_quantity_Box, label_use_quantiy_Box, label_destroy_quantity_Box, 
                packing_pattern_quantity_Box, total_quantiy_Box, quantity_per_piece_Box, recorderBox, checkerBox});
            List<Button> BtnList = new List<Button>(new Button[] { AddLineBtn, DelLineBtn });

            if (canChange)
            {
                for (int i = 0; i < TextBoxList.Count; i++)
                { TextBoxList[i].ReadOnly = false; }
                for (int i = 0; i < BtnList.Count; i++)
                { BtnList[i].Enabled = true; }
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                dataGridView1.ReadOnly = false;
                dataGridView2.ReadOnly = false;
                recordTimePicker.Enabled = true;
                checkTimePicker.Enabled = true;
            }
            else
            {
                for (int i = 0; i < TextBoxList.Count; i++)
                { TextBoxList[i].ReadOnly = true; }
                for (int i = 0; i < BtnList.Count; i++)
                { BtnList[i].Enabled = false; }
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                dataGridView1.ReadOnly = true;
                dataGridView2.ReadOnly = true;
                recordTimePicker.Enabled = false;
                checkTimePicker.Enabled = false;
            }
        }
        
        private void DataShow(String productCode, DateTime searchDate)
        {
            List<String> queryCols = new List<String>(new String[] { "inner_info",  
                "operator_id", "operate_date", "reviewer_id", "review_date", "is_review_qualified",
                "packing_name","packing_batch","packing_quantity","packing_receive_quantity","packing_remain_quantiy","packing_use_quantity","packing_back_quantity",
                "indicator_batch","indicator_quantity","indicator_receive_quantity","indicator_remain_quantity","indicator_use_quantiy","indicator_back_quantiy",
                "product_batch_id", "label_release_quantity","label_use_quantiy","label_destroy_quantity","packing_pattern_quantity","total_quantiy","quantity_per_piece",
                "is_Chinese","is_English"});
            List<String> whereCols = new List<String>(new String[] { "product_code", "production_date" });
            List<Object> whereVals = new List<Object>(new Object[] { productCode, Convert.ToDateTime(searchDate.Date.ToString("yyyy/MM/dd")) });
            List<List<Object>> queryValsList = Utility.selectAccess(connOle, table, queryCols, whereCols, whereVals, null, null, null, null, null);
            if (queryValsList.Count == 0)
            {
                InfInit();
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;
                isSaveOk = false;
                isReadOnly = false;
                ConsChanged(true);
            }
            else
            {
                dataGridView1.Rows.Clear();
                //表格1
                jarray1 = JArray.Parse(queryValsList[0][0].ToString());
                List<String> dgvcols = new List<String>(new String[] { "序号", "生产时间", "内包序号", "产品外观", "包装后外观", 
                    "包装袋热封线", "贴标签", "贴指示剂", "包装人" });
                List<Type> types = new List<Type>(new Type[] { Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.String"), Type.GetType("System.Boolean"), Type.GetType("System.Boolean"), 
                    Type.GetType("System.Boolean"), Type.GetType("System.Boolean"), Type.GetType("System.Boolean"), Type.GetType("System.String") });
                Utility.writeJSONToDataGridView(jarray1, dataGridView1, dgvcols, types);                
                //记录人
                operator_id = Convert.ToInt32(queryValsList[0][1].ToString());
                operator_name = Parameter.IDtoName(operator_id);
                this.recorderBox.Text = operator_name;
                operate_date = Convert.ToDateTime(queryValsList[0][2].ToString());
                recordTimePicker.Value = operate_date;

                //判断是否有复核人
                if (Int32.TryParse(queryValsList[0][3].ToString(), out review_id) == false)
                {
                    reviewer_name = null;
                    this.checkerBox.Text = "";
                    review_date = DateTime.Now;
                    checkTimePicker.Value = review_date;
                    ischeckOk = false;

                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                    isReadOnly = false;
                    ConsChanged(true);
                }
                else
                { 
                    reviewer_name = Parameter.IDtoName(review_id);
                    this.checkerBox.Text = reviewer_name;
                    review_date = Convert.ToDateTime(queryValsList[0][4].ToString());
                    checkTimePicker.Value = review_date;
                    ischeckOk = Convert.ToBoolean(queryValsList[0][5].ToString());
                    //判断是否审核合格
                    if (ischeckOk)
                    {
                        SaveBtn.Enabled = false;
                        CheckBtn.Enabled = false;
                        printBtn.Enabled = true;
                        isReadOnly = true;
                        ConsChanged(false);

                    }
                    else
                    {
                        SaveBtn.Enabled = true;
                        CheckBtn.Enabled = true;
                        printBtn.Enabled = false;
                        isReadOnly = false;
                        ConsChanged(true);
                    }
                }

                //表格2
                dataGridView2.Rows.Clear();
                AddRowLine2();
                AddRowLine2();
                dataGridView2.Rows[0].Cells[0].Value = queryValsList[0][6].ToString();
                dataGridView2.Rows[0].Cells[1].Value = queryValsList[0][7].ToString();
                dataGridView2.Rows[0].Cells[2].Value = queryValsList[0][8].ToString();
                dataGridView2.Rows[0].Cells[3].Value = queryValsList[0][9].ToString();
                dataGridView2.Rows[0].Cells[4].Value = queryValsList[0][10].ToString();
                dataGridView2.Rows[0].Cells[5].Value = queryValsList[0][11].ToString();
                dataGridView2.Rows[0].Cells[6].Value = queryValsList[0][12].ToString();
                dataGridView2.Rows[1].Cells[0].Value = "指示剂";
                dataGridView2.Rows[1].Cells[1].Value = queryValsList[0][13].ToString();
                dataGridView2.Rows[1].Cells[2].Value = queryValsList[0][14].ToString();
                dataGridView2.Rows[1].Cells[3].Value = queryValsList[0][15].ToString();
                dataGridView2.Rows[1].Cells[4].Value = queryValsList[0][16].ToString();
                dataGridView2.Rows[1].Cells[5].Value = queryValsList[0][17].ToString();
                dataGridView2.Rows[1].Cells[6].Value = queryValsList[0][18].ToString();
                //其他信息
                batchIdBox.Text = queryValsList[0][19].ToString();
                label_release_quantity_Box.Text = queryValsList[0][20].ToString();
                label_use_quantiy_Box.Text = queryValsList[0][21].ToString();
                label_destroy_quantity_Box.Text = queryValsList[0][22].ToString(); ;
                packing_pattern_quantity_Box.Text = queryValsList[0][23].ToString();
                total_quantiy_Box.Text = queryValsList[0][24].ToString();
                quantity_per_piece_Box.Text = queryValsList[0][25].ToString();
                checkBox1.Checked = Convert.ToBoolean(queryValsList[0][26].ToString());//中文
                checkBox2.Checked = Convert.ToBoolean(queryValsList[0][27].ToString());//英文

                isSaveOk = true;
            }
        }

        //添加单行模板
        private void AddRowLine()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = (dataGridView1.RowCount+1).ToString(); //DateTime.Now.ToString("yyyy-MM-dd");
            dr.Cells[1].Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = true;
            dr.Cells[4].Value = true;
            dr.Cells[5].Value = true;
            dr.Cells[6].Value = true; //"包装是否完好"
            dr.Cells[7].Value = true; //"是否清洁合格"
            dr.Cells[8].Value = ""; //"包装人
            dataGridView1.Rows.Add(dr);
        }
        
        //添加单行模板
        private void AddRowLine2()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView2.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }

            dr.Cells[0].Value = "";
            dr.Cells[1].Value = "";
            dr.Cells[2].Value = "";
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dr.Cells[5].Value = "";
            dr.Cells[6].Value = ""; 
            dataGridView2.Rows.Add(dr);
        }

        private void AddLineBtn_Click(object sender, EventArgs e)
        {  AddRowLine();  }

        private void DelLineBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1)
            {
                this.dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                numFresh();
            }            
        }

        private void numFresh()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            { dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString(); }
        }

        private void DataSaveSql() { }

        private void DataSaveOle()
        {
            List<String> dgvcols = new List<String>(new String[] { "序号", "生产时间", "内包序号", "产品外观", "包装后外观", "包装袋热封线", "贴标签", "贴指示剂", "包装人"});
            jarray1 = Utility.readJSONFromDataGridView(dataGridView1, dgvcols);

            if (isSaveOk==false)
            {
                List<String> queryCols = new List<String>(new String[] { "inner_info", "operator_id", "operate_date", 
                    "packing_name","packing_batch","packing_quantity","packing_receive_quantity",
                    "packing_remain_quantiy","packing_use_quantity","packing_back_quantity",
                    "indicator_batch","indicator_quantity","indicator_receive_quantity",
                    "indicator_remain_quantity","indicator_use_quantiy","indicator_back_quantiy",
                    "product_batch_id", "label_release_quantity","label_use_quantiy",
                    "label_destroy_quantity","packing_pattern_quantity","total_quantiy","quantity_per_piece",
                    "is_Chinese","is_English","product_code", "production_date"});
                List<Object> queryVals = new List<Object>(new Object[] { jarray1.ToString(), operator_id, Convert.ToDateTime(recordTimePicker.Value.ToString("yyyy/MM/dd")), 
                    dataGridView2.Rows[0].Cells[0].Value.ToString(), dataGridView2.Rows[0].Cells[1].Value.ToString(), Convert.ToInt32(dataGridView2.Rows[0].Cells[2].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[3].Value), 
                    Convert.ToInt32(dataGridView2.Rows[0].Cells[4].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[5].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[6].Value),
                    dataGridView2.Rows[1].Cells[1].Value.ToString(), Convert.ToInt32(dataGridView2.Rows[1].Cells[2].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[3].Value),
                    Convert.ToInt32(dataGridView2.Rows[1].Cells[4].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[5].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[6].Value), 
                    Convert.ToInt32(batchIdBox.Text),Convert.ToInt32(label_release_quantity_Box.Text), Convert.ToInt32(label_use_quantiy_Box.Text), 
                    Convert.ToInt32(label_destroy_quantity_Box.Text), Convert.ToInt32(packing_pattern_quantity_Box.Text), Convert.ToInt32(total_quantiy_Box.Text), Convert.ToInt32(quantity_per_piece_Box.Text),
                    Convert.ToBoolean(checkBox1.Checked), Convert.ToBoolean(checkBox2.Checked), productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd"))});     
                 
                Boolean b = Utility.insertAccess(connOle, table, queryCols, queryVals);
            }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "inner_info", "operator_id", "operate_date", 
                    "packing_name","packing_batch","packing_quantity","packing_receive_quantity",
                    "packing_remain_quantiy","packing_use_quantity","packing_back_quantity",
                    "indicator_batch","indicator_quantity","indicator_receive_quantity",
                    "indicator_remain_quantity","indicator_use_quantiy","indicator_back_quantiy",
                    "product_batch_id", "label_release_quantity","label_use_quantiy",
                    "label_destroy_quantity","packing_pattern_quantity","total_quantiy","quantity_per_piece",
                    "is_Chinese","is_English"});
                List<Object> queryVals = new List<Object>(new Object[] { jarray1.ToString(), operator_id, Convert.ToDateTime(recordTimePicker.Value.ToString("yyyy/MM/dd")), 
                    dataGridView2.Rows[0].Cells[0].Value.ToString(), dataGridView2.Rows[0].Cells[1].Value.ToString(), Convert.ToInt32(dataGridView2.Rows[0].Cells[2].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[3].Value), 
                    Convert.ToInt32(dataGridView2.Rows[0].Cells[4].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[5].Value), Convert.ToInt32(dataGridView2.Rows[0].Cells[6].Value),
                    dataGridView2.Rows[1].Cells[1].Value.ToString(), Convert.ToInt32(dataGridView2.Rows[1].Cells[2].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[3].Value),
                    Convert.ToInt32(dataGridView2.Rows[1].Cells[4].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[5].Value), Convert.ToInt32(dataGridView2.Rows[1].Cells[6].Value), 
                    Convert.ToInt32(batchIdBox.Text),Convert.ToInt32(label_release_quantity_Box.Text), Convert.ToInt32(label_use_quantiy_Box.Text), 
                    Convert.ToInt32(label_destroy_quantity_Box.Text), Convert.ToInt32(packing_pattern_quantity_Box.Text), Convert.ToInt32(total_quantiy_Box.Text), Convert.ToInt32(quantity_per_piece_Box.Text),
                    Convert.ToBoolean(checkBox1.Checked), Convert.ToBoolean(checkBox2.Checked)});    
                
                List<String> whereCols = new List<String>(new String[] { "product_code", "production_date" });
                List<Object> whereVals = new List<Object>(new Object[] { productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd")) });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
        }

        //检查包装人
        private bool CheckPackkingUser()
        {
            bool TypeCheck = true;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int idtemp = Parameter.NametoID(dataGridView1.Rows[i].Cells["包装人"].Value.ToString());
                if (idtemp == 0)
                {
                    MessageBox.Show("第" + (i+1).ToString() + "行包装人框内的包装人不存在，请重新填入!");
                    TypeCheck = false;
                    break;
                }
            }

            return TypeCheck;
        }

        //检查第二个dgv信息
        private bool Checkdgv2()
        {
            bool TypeCheck = true;
            int numtemp = 0;
            string[] string1 = { "包材名为"+dataGridView2.Rows[0].Cells[0].Value.ToString(), "指示剂" };
            string[] string2 = { "批号", "接上班数量", "领取数量", "剩余数量", "使用数量", "退库数量" };
            for (int i = 0; i < 12; i++)
            {
                if (Int32.TryParse(dataGridView2.Rows[i / 6].Cells[(i % 6) + 1].Value.ToString(), out numtemp) == false)
                {
                    MessageBox.Show(string1[i / 6] + " 一行内的 " + string2[i % 6] + " 框内应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }            
            return TypeCheck;
        }

        //检查一堆TextBox
        private bool CheckContros()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { batchIdBox, label_release_quantity_Box, label_use_quantiy_Box, label_destroy_quantity_Box, 
                packing_pattern_quantity_Box, total_quantiy_Box, quantity_per_piece_Box});
            List<String> StringList = new List<String>(new String[] { "产品批号","发放数量","使用数量","销毁数量",
                "包装规格(只/包)","总计包装数（包）","总计（只/片）" });

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

        public override void CheckResult()
        {
            base.CheckResult();
            review_id = check.userID;
            string review_opinion = check.opinion;
            ischeckOk = check.ischeckOk;
            reviewer_name = Parameter.IDtoName(review_id);

            if (isSqlOk)
            { }
            else
            {
                List<String> queryCols = new List<String>(new String[] { "reviewer_id", "review_date", "review_opinion", "is_review_qualified" });
                List<Object> queryVals = new List<Object>(new Object[] { review_id, Convert.ToDateTime(checkTimePicker.Value.ToString("yyyy/MM/dd")), review_opinion, ischeckOk });
                List<String> whereCols = new List<String>(new String[] { "product_code", "production_date" });
                List<Object> whereVals = new List<Object>(new Object[] { productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd")) });
                Boolean b = Utility.updateAccess(connOle, table, queryCols, queryVals, whereCols, whereVals);
            }
            //判断检验合格
            if (ischeckOk)
            {
                isReadOnly = true;
                ConsChanged(false);
                SaveBtn.Enabled = false;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
            }
            else
            {
                isReadOnly = false;
                ConsChanged(true);
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
            }

            checkerBox.Text = reviewer_name;
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();                                  
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            operator_name = recorderBox.Text;
            if (operator_name != Parameter.IDtoName(operator_id))
            {
                operator_id = Parameter.NametoID(operator_name);
            }
            if (CheckPackkingUser() == false)
            { /*包装人有误*/ }
            else if (Checkdgv2() ==false)
            { /*第二个dgv信息有误*/ }
            else if (CheckContros() == false)
            { /*其他的各种TextBox有误*/ }
            else if (operator_id == 0) 
            {
                MessageBox.Show("未找到记录人姓名，请重新输入");
            }
            else
            {
                if (isSqlOk)
                { DataSaveSql(); }
                else
                { DataSaveOle(); }
                isSaveOk = true;
            }
            CheckBtn.Enabled = true;
        }

        private void productIdBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataShow(productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd")));
        }

        private void productionTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DataShow(productcodeBox.Text.ToString(), Convert.ToDateTime(productionTimePicker.Value.ToString("yyyy/MM/dd")));
        }

    }
}
