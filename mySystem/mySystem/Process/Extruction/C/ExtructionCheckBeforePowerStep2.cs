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
    public partial class ExtructionCheckBeforePowerStep2 : BaseForm
    {
        private DataTable dt_confirmarea = new DataTable();

        private String table = "吹膜机组开机前确认表";
        private String tableInfo = "吹膜机组开机前确认项目记录";
        private String tableSet = "设置吹膜机组开机前确认项目";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        private int Instructionid;
        private CheckForm check = null;
        
        private Boolean isNewRecord = false;

        private DataTable dt设置, dtCBP, dtCBP详情;
        private OleDbDataAdapter daCBP, daCBP详情;
        private BindingSource bsCBP, bsCBP详情;
        private OleDbCommandBuilder cbCBP, cbCBP详情;
                
        public ExtructionCheckBeforePowerStep2(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            Instructionid = Parameter.proInstruID;

            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            GetSettingInfo();
            DSBinding(Instructionid);
        }

        private void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                //MessageBox.Show("有数据");
                DSBinding(Convert.ToInt32(reader1["生产指令ID"].ToString()));
            }
        }

        //******************************初始化******************************//
        
        //读取设置内容
        private void GetSettingInfo()
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();
        }

        //控件属性（先绑定！！！）
        private void formatInit()
        {
            //表格界面设置
            this.dataGridView1.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["T吹膜机组开机前确认表ID"].Visible = false;
            
            this.dataGridView1.Columns["序号"].MinimumWidth = 80;
            this.dataGridView1.Columns["序号"].ReadOnly = true;
            this.dataGridView1.Columns["确认项目"].MinimumWidth = 160;
            this.dataGridView1.Columns["确认项目"].ReadOnly = true;
            this.dataGridView1.Columns["确认内容"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns["确认内容"].ReadOnly = true;
            this.dataGridView1.Columns["确认结果_是"].MinimumWidth = 100;
            this.dataGridView1.Columns["确认结果_否"].MinimumWidth = 100;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Columns["确认内容"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;  
        }

        //******************************显示数据******************************//
        
        private void DSBinding(int ID)
        {
            //生产指令ID
            //Dispose
            if (daCBP != null)
            {
                bsCBP.Dispose();
                dtCBP.Dispose();
                daCBP.Dispose();
                cbCBP.Dispose();
            }
            if (daCBP详情 != null)
            {
                bsCBP详情.Dispose();
                dtCBP详情.Dispose();
                daCBP详情.Dispose();
                cbCBP详情.Dispose();
            }
            //******************************外部控件******************************//  
            // 新建
            bsCBP = new BindingSource();
            dtCBP = new DataTable(table);
            //连数据库
            daCBP = new OleDbDataAdapter("select * from " + table + " where 生产指令id = " + ID.ToString() , connOle);
            cbCBP = new OleDbCommandBuilder(daCBP);
            daCBP.Fill(dtCBP);
            bsCBP.DataSource = dtCBP;
            // 绑定（先解除绑定）
            tb确认人.DataBindings.Clear();
            tb确认人.DataBindings.Add("Text", bsCBP.DataSource, "确认人");
            dtp确认日期.DataBindings.Clear();
            dtp确认日期.DataBindings.Add("Text", bsCBP.DataSource, "确认日期");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bsCBP.DataSource, "审核人");
            dtp审核日期.DataBindings.Clear();
            dtp审核日期.DataBindings.Add("Text", bsCBP.DataSource, "审核日期");

            //待注释
            //MessageBox.Show("记录数目：" + dtCBP.Rows.Count.ToString());

            //*******************************表格内部******************************//            
            // 新建Binding Source
            bsCBP详情 = new BindingSource();  //表格内部
            // 新建DataTable
            dtCBP详情 = new DataTable(tableInfo);  //表格内部
            // 数据库->DataTable
            if (dtCBP.Rows.Count == 0)
            {
                isNewRecord = true;
                //为记录新建一行
                DataRow dr1 = dtCBP.NewRow();
                dr1["生产指令ID"] = mySystem.Parameter.proInstruID;
                dr1["确认人"] = mySystem.Parameter.userName;
                dr1["确认日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                //防止违反并发性，提前赋值
                dr1["审核人"] = " ";
                dr1["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));//违反并发性
                dr1["审核意见"] = " ";
                dr1["审核是否通过"] = false;
                dtCBP.Rows.InsertAt(dr1, dtCBP.Rows.Count);

                // 数据库->DataTable
                daCBP详情 = new OleDbDataAdapter("select * from " + tableInfo + " where 0 = 1", connOle);
                cbCBP详情 = new OleDbCommandBuilder(daCBP详情);
                daCBP详情.Fill(dtCBP详情);
                // DataTable->BindingSource
                bsCBP详情.DataSource = dtCBP详情;
                // 先clear所有bindings
                dataGridView1.DataBindings.Clear();
                // 控件->BindingSource
                dataGridView1.DataSource = bsCBP详情.DataSource;

                dtCBP详情.Rows.Clear();
                for (int i = 0; i < dt设置.Rows.Count; i++)
                {
                    DataRow dr2 = dtCBP详情.NewRow();
                    dr2["序号"] = (i + 1);
                    dr2["确认项目"] = dt设置.Rows[i]["确认项目"];
                    dr2["确认内容"] = dt设置.Rows[i]["确认内容"];
                    dr2["确认结果_是"] = true;
                    dr2["确认结果_否"] = false;
                    dtCBP详情.Rows.InsertAt(dr2, dtCBP详情.Rows.Count);                    
                }
                
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;
                cons_change();
            }
            else
            {
                isNewRecord = false;
                // 数据库->DataTable
                daCBP详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T吹膜机组开机前确认表ID = " + dtCBP.Rows[0]["ID"].ToString(), connOle);
                cbCBP详情 = new OleDbCommandBuilder(daCBP详情);
                daCBP详情.Fill(dtCBP详情);
                // DataTable->BindingSource
                bsCBP详情.DataSource = dtCBP详情;
                // 先clear所有bindings
                dataGridView1.DataBindings.Clear();
                // 控件->BindingSource
                dataGridView1.DataSource = bsCBP详情.DataSource;

                if (Convert.ToBoolean(dtCBP.Rows[0]["审核是否通过"]) == true)
                {
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                    printBtn.Enabled = true;
                    cons_change_not();
                }
                else
                {
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                    cons_change();
                }
            }

            formatInit();  

        }

        //******************************按钮功能******************************//

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(tb确认人.Text) == 0)
            { MessageBox.Show("请重新填写确认人姓名！"); }
            else
            {
                // 保存非DataGridView中的数据必须先执行EndEdit;
                bsCBP.EndEdit();
                daCBP.Update((DataTable)bsCBP.DataSource);

                // dgv  保存，清空，重读
                if (isNewRecord == true)
                {
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = mySystem.Parameter.connOle;
                    comm.CommandText = "select @@identity";
                    Int32 idd = (Int32)comm.ExecuteScalar();

                    daCBP详情.Dispose();
                    cbCBP详情.Dispose();

                    //重新绑定
                    daCBP详情 = new OleDbDataAdapter("select * from " + tableInfo + " where T吹膜机组开机前确认表ID = " + idd.ToString(), connOle);
                    cbCBP详情 = new OleDbCommandBuilder(daCBP详情);
                    daCBP详情.Fill(dtCBP详情);
                    // DataTable->BindingSource
                    bsCBP详情.DataSource = dtCBP详情;
                    // 先clear所有bindings
                    dataGridView1.DataBindings.Clear();
                    // 控件->BindingSource
                    dataGridView1.DataSource = bsCBP详情.DataSource;

                    for (int i = 0; i < dtCBP详情.Rows.Count; i++)
                    { dtCBP详情.Rows[i]["T吹膜机组开机前确认表ID"] = idd; }

                    isNewRecord = false;
                }
                else
                {
                    for (int i = 0; i < dtCBP详情.Rows.Count; i++)
                    { dtCBP详情.Rows[i]["T吹膜机组开机前确认表ID"] = dtCBP详情.Rows[0]["T吹膜机组开机前确认表ID"]; }
                }
                daCBP详情.Update((DataTable)bsCBP详情.DataSource);
                dtCBP详情.Clear();
                daCBP详情.Fill(dtCBP详情);

                CheckBtn.Enabled = true;
            }
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            dtCBP.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
            dtCBP.Rows[0]["审核意见"] = check.opinion;
            dtCBP.Rows[0]["审核是否通过"] = check.ischeckOk;

            // 保存非DataGridView中的数据必须先执行EndEdit;
            bsCBP.EndEdit();
            daCBP.Update((DataTable)bsCBP.DataSource);

            if (check.ischeckOk == true)
            {
                SaveBtn.Enabled = false;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
                cons_change_not();
            }
            else
            {
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
                cons_change();
            }
        }
        
        //******************************小功能******************************// 
        
        //数据可修改
        private void cons_change()
        {
            dataGridView1.ReadOnly = false;
                        
            tb确认人.ReadOnly = false;
            tb审核人.ReadOnly = false;

            dtp确认日期.Enabled = true;
            dtp审核日期.Enabled = true;   
        }

        //数据不可修改
        private void cons_change_not()
        {
            dataGridView1.ReadOnly = true;

            tb确认人.ReadOnly = true;
            tb审核人.ReadOnly = true;

            dtp确认日期.Enabled = false;
            dtp审核日期.Enabled = false;
        }

        //******************************datagridview******************************//  

        // 处理DataGridView中是、否相反
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtCBP详情.Rows.Count > 0 && dtCBP.Rows.Count > 0 )
            {
                if (Convert.ToBoolean(dtCBP.Rows[0]["审核是否通过"]) == false)
                {
                    //确认结果是否合格
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "确认结果_是")
                    {

                        if (Convert.ToBoolean(dtCBP详情.Rows[e.RowIndex]["确认结果_是"]) == true)
                        {
                            dtCBP详情.Rows[e.RowIndex]["确认结果_是"] = false;
                            dtCBP详情.Rows[e.RowIndex]["确认结果_否"] = true;
                        }
                        else
                        {
                            dtCBP详情.Rows[e.RowIndex]["确认结果_是"] = true;
                            dtCBP详情.Rows[e.RowIndex]["确认结果_否"] = false;
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].Name == "确认结果_否")
                    {
                        if (Convert.ToBoolean(dtCBP详情.Rows[e.RowIndex]["确认结果_否"]) == false)
                        {
                            dtCBP详情.Rows[e.RowIndex]["确认结果_是"] = false;
                            dtCBP详情.Rows[e.RowIndex]["确认结果_否"] = true;
                        }
                        else
                        {
                            dtCBP详情.Rows[e.RowIndex]["确认结果_是"] = true;
                            dtCBP详情.Rows[e.RowIndex]["确认结果_否"] = false;
                        }
                    }                    
                    else
                    { }
                }
            }
        }   
            
    }
}
