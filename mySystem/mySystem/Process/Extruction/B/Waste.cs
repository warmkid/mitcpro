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
using System.Collections;

namespace mySystem.Process.Extruction.B
{

    public partial class Waste : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜工序废品记录";
        DataTable dtWaste;
        OleDbDataAdapter daWaste;
        BindingSource bsWaste;
        OleDbCommandBuilder cbWaste;

        string tablename2 = "吹膜工序废品记录详细信息";
        DataTable dtItem;
        OleDbDataAdapter daItem;
        BindingSource bsItem;
        OleDbCommandBuilder cbItem;

        Hashtable productCode;
        List<string> productCodeLst;
        List<string> wasteReason = new List<string>();
        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> usrList = new List<string>();
        private CheckForm check = null;
        int outerId;
        int status;
        public Waste(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            txb生产指令.Text = Parameter.proInstruction;
            getProductCode();
            getStartTime();
            getUsrList();
            getWasteRason();
            dtp生产结束时间.Value = DateTime.Now;
            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();
            if (0 == dtWaste.Rows.Count)
            {
                DataRow newrow = dtWaste.NewRow();
                newrow = writeWasteDefault(newrow);
                dtWaste.Rows.Add(newrow);
                daWaste.Update((DataTable)bsWaste.DataSource);
                readWasteData(txb生产指令.Text);    
                removeWasteBinding();
                WasteBind();
            }
            txb生产指令.Enabled = false;
            dtp生产开始时间.Enabled = false;
            dtp生产结束时间.Enabled = false;
            btn审核.Enabled = false;
            readItemData(Convert.ToInt32( dtWaste.Rows[0]["ID"]));
            setDataGridViewColumns();
            ItemBind();            
            setDataGridViewRowNums();
            //judgeReview();
            forzen();
        }
        public override void CheckResult()
        {
            base.CheckResult();

            dtWaste.Rows[0]["审核人"] = check.userName.ToString();
            dtWaste.Rows[0]["审核意见"] = check.opinion.ToString();
            dtWaste.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
            daWaste.Update((DataTable)bsWaste.DataSource);
            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();

            //fill reviewer in inner table
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if (Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim() =="")
                {
                    dtItem.Rows[i]["审核人"] = check.userName.ToString();
                }
                continue;
            }
            daItem.Update((DataTable)bsItem.DataSource);
            forzen();
            btn保存.Enabled = false;
        }
        private void judgeReview()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["审核人"].ToString() == "")
                {
                    this.dataGridView1.Rows[i].ReadOnly = true;
                }
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        //// 给外表的一行写入默认值，包括操作人，时间，班次等
        //DataRow writeOuterDefault(DataRow);
        //// 给内表的一行写入默认值，包括操作人，时间，Y/N等
        //DataRow writeInnerDefault(DataRow);
        //// 根据条件从数据库中读取一行外表的数据
        //void readOuterData(能唯一确定一行外表数据的参数，一般是生产指令ID或生产指令编号)；
        //// 根据条件从数据库中读取多行内表数据
        //void readInnerData(int 外表行ID);
        //// 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        //void removeOuterBinding();
        //// 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        //void removeInner(Binding);
        //// 外表和控件的绑定
        //void outerBind();
        //// 内表和控件的绑定
        //void innerBind();
        //// 设置DataGridView中各列的格
        //void setDataGridViewColumns();
        //// 刷新DataGridView中的列：序号
        //void setDataGridViewRowNums();
        private void readWasteData(String name)
        {
            daWaste = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令='" + name+"';", conOle);
            cbWaste = new OleDbCommandBuilder(daWaste);
            dtWaste = new DataTable(tablename1);
            bsWaste = new BindingSource();
            daWaste.Fill(dtWaste);
        }

        private DataRow writeWasteDefault(DataRow dr)
        {
            dr["生产指令ID"] = Parameter.proInstruID;
			dr["生产指令"]=txb生产指令.Text;
			dr["生产开始时间"]=Convert.ToDateTime(dtp生产开始时间.Value.ToString());
			dr["生产结束时间"]=Convert.ToDateTime(dtp生产结束时间.Value.ToString());
            dr["合计不良品数量"] = 0;
            return dr;
        }
        private void getStartTime()
        {
            string sqlStr = "SELECT 开始生产日期 FROM 生产指令信息表 WHERE ID = " + Parameter.proInstruID.ToString();
            OleDbCommand sqlCmd = new OleDbCommand(sqlStr, conOle);
            dtp生产开始时间.Value = Convert.ToDateTime(sqlCmd.ExecuteScalar());
        }

        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("生产指令产品列表");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 生产指令产品列表 WHERE 生产指令ID = " + Parameter.proInstruID, conOle);
            da.Fill(DtproductCode);
            productCode = new Hashtable();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品编码"].ToString(), dr["产品批号"].ToString());
            }
            productCodeLst = new List<string>();
            foreach (string s in productCode.Keys.OfType<string>().ToList<string>())
            {
                productCodeLst.Add(s);
            }
        }
        private void getUsrList()
        {
            DataTable DtUsr = new DataTable("users");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT 姓名 FROM users", Parameter.connOleUser);
            da.Fill(DtUsr);
            
            usrList = new List<string>();
            for(int i=0;i<DtUsr.Rows.Count;i++)
            {
                usrList.Add(Convert.ToString( DtUsr.Rows[i]["姓名"]));
            }
        }

        private void getWasteRason()
        {
            DataTable Dt = new DataTable("设置废品产生原因");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT 废品产生原因 FROM 设置废品产生原因", conOle);
            da.Fill(Dt);

            wasteReason = new List<string>();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                wasteReason.Add(Convert.ToString(Dt.Rows[i]["废品产生原因"]));
            }
        }
        private void WasteBind()
        {
            bsWaste.DataSource = dtWaste;
            txb生产指令.DataBindings.Add("Text", bsWaste.DataSource, "生产指令");
            dtp生产开始时间.DataBindings.Add("Value", bsWaste.DataSource, "生产开始时间");
            dtp生产结束时间.DataBindings.Add("Value", bsWaste.DataSource, "生产结束时间");
            txb合计不良品数量.DataBindings.Add("Text", bsWaste.DataSource, "合计不良品数量");
        }

        private void removeWasteBinding()
        {
            txb生产指令.DataBindings.Clear();
            dtp生产开始时间.DataBindings.Clear();
            dtp生产结束时间.DataBindings.Clear();
            txb合计不良品数量.DataBindings.Clear();
        }

        private void readItemData(int id)
        {
            daItem = new OleDbDataAdapter("SELECT * FROM "+tablename2 +" WHERE T吹膜工序废品记录ID=" + id, conOle);
            cbItem = new OleDbCommandBuilder(daItem);
            dtItem = new DataTable(tablename2);
            bsItem = new BindingSource();
            daItem.Fill(dtItem);
        }

        private void ItemBind()
        {
            bsItem.DataSource = dtItem;
            dataGridView1.DataSource = bsItem.DataSource;
        }

        private void removeItemBinding()
        {
            ;
        }
        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtItem.Columns)
            {
                
                switch (dc.ColumnName)
                {
                    case "班次":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in flight)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    case "产品代码":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in productCodeLst)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    //case "不良品数量":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    case "废品产生原因":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        foreach (String s in wasteReason)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    //case "记录人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break;
                    //case "审核人":
                    //    tbc = new DataGridViewTextBoxColumn();
                    //    tbc.DataPropertyName = dc.ColumnName;
                    //    tbc.HeaderText = dc.ColumnName;
                    //    tbc.Name = dc.ColumnName;
                    //    tbc.ValueType = dc.DataType;
                    //    dataGridView1.Columns.Add(tbc);
                    //    tbc.Visible = true;
                    //    break; 
                    
                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.Automatic;
                        c2.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(c2);

                        break;
                }
            }            
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
        }



        private DataRow writeItemDefault(DataRow dr)
        {
            dr["T吹膜工序废品记录ID"] = dtWaste.Rows[0]["ID"];
            dr["序号"]=0;
            dr["生产日期"] = Convert.ToDateTime(DateTime.Now.ToString());
            dr["班次"] = "";
            dr["产品代码"] = "";
            dr["不良品数量"] = 0;
            dr["废品产生原因"] = "";
            dr["记录人"] = Parameter.userName;
            return dr;
        }


        private void btn保存_Click(object sender, EventArgs e)
        {
            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daItem.Update((DataTable)bsItem.DataSource);
            readItemData(Convert.ToInt32(dtWaste.Rows[0]["ID"]));
            ItemBind();
            setDataGridViewRowNums();

            //find the uncheck item in inner list and reset the review information
            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if (Convert.ToString(dtItem.Rows[i]["审核人"]).ToString().Trim() == "")
                {
                    dtWaste.Rows[0]["审核人"] = "";
                    dtWaste.Rows[0]["审核意见"] = "";
                    dtWaste.Rows[0]["审核是否通过"] = false;
                }
                continue;
            }

            bsWaste.EndEdit();
            daWaste.Update((DataTable)bsWaste.DataSource);
            readWasteData(txb生产指令.Text);
            removeWasteBinding();
            WasteBind();
            btn审核.Enabled = true;
        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            // 内表中添加一行
            DataRow dr = dtItem.NewRow();
            dr = writeItemDefault(dr);
            dtItem.Rows.Add(dr);
            setDataGridViewRowNums();
            btn保存.Enabled = true;
            btn审核.Enabled = false;
        }

        private void setDataGridViewRowNums()
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void forzen()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            { 
                if (dtItem.Rows[i]["审核人"].ToString().Trim() == "")
                {
                    continue;
                }
                dataGridView1.Rows[i].ReadOnly = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (6 == e.ColumnIndex)
            {

                double sum = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    sum += Convert.ToDouble(dtItem.Rows[i]["不良品数量"]);
                }
                dtWaste.Rows[0]["合计不良品数量"] = sum;
            }
            if (8 == e.ColumnIndex || 9 == e.ColumnIndex)     //how to check the usr name of this list
            {
                if(usrList.IndexOf(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim())<0)
                {
                    MessageBox.Show("usr doesn't exist");                    
                }
            }
            
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            if (name == "ID")
            {
                return;
            }
            MessageBox.Show(name + "填写错误");
        }
        
   
    }
}
