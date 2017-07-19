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

namespace mySystem
{
    public partial class QueryInstruForm : BaseForm
    {
        DateTime date1;//起始时间
        DateTime date2;//结束时间
        string writer;//编制人

        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;
        private OleDbCommandBuilder cb;      
        
        public QueryInstruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            Initdgv();

        }

        //dgv样式初始化
        private void Initdgv()
        {
            bs = new BindingSource();
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToResizeRows = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 12);

        }

        private void Bind()
        {            
            dt = new DataTable("生产指令信息表"); //""中的是表名
            da = new OleDbDataAdapter("select * from 生产指令信息表 where 编制人 like" + "'%" + writer + "%'" + "and 编制时间 between" + "#" + date1 + "#" + " and " + "#" + date2.AddDays(1) + "#", mySystem.Parameter.connOle);
            cb = new OleDbCommandBuilder(da);
            dt.Columns.Add("序号", System.Type.GetType("System.String"));
            da.Fill(dt);
            bs.DataSource = dt;
            this.dgv.DataBindings.Clear();
            this.dgv.DataSource = bs.DataSource; //绑定
            //显示序号
            setDataGridViewRowNums();

            for (int i = 0; i < this.dgv.Columns.Count; i++)
            {
                string colname = this.dgv.Columns[i].Name;
                if (colname == "序号" || colname == "产品名称" || colname == "生产指令编号" || colname == "生产工艺" || colname == "开始生产日期" || colname == "编制人" || colname == "编制时间")
                {
                    this.dgv.Columns[colname].Visible = true;
                }
                else
                {
                    this.dgv.Columns[colname].Visible = false;
                }
            }
            this.dgv.Columns["序号"].MinimumWidth = 40;  //没用？？？？？
            this.dgv.Columns["产品名称"].MinimumWidth = 200;
            this.dgv.Columns["生产指令编号"].MinimumWidth = 150;
            this.dgv.Columns["生产工艺"].MinimumWidth = 150;
            this.dgv.Columns["开始生产日期"].MinimumWidth = 200; 
            this.dgv.Columns["编制时间"].MinimumWidth = 200; 
            
        }

        private void setDataGridViewRowNums()
        {
            int coun = this.dgv.Rows.Count;
            for (int i = 0; i < coun; i++)
            {
                this.dgv.Rows[i].Cells["序号"].Value = (i + 1).ToString();
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.Date;
            date2 = dateTimePicker2.Value.Date;
            writer = textBox1.Text.Trim();
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }

            Bind();           
            
            #region 旧版
            //dgv.Rows.Clear();
            //date1 = dateTimePicker1.Value.Date;
            //date2 = dateTimePicker2.Value.Date;
            //writer = textBox1.Text.Trim();
            //TimeSpan delt = date2 - date1;
            //if (delt.TotalDays < 0)
            //{
            //    MessageBox.Show("起止时间有误，请重新输入");
            //    return;
            //}


            ////模糊查询用户id
            //String usertblName = "user_aoxing";
            //List<String> queryidCols = new List<String>(new String[] { "user_id","user_name" });
            //String likeCol = "user_name";
            //String likeVal = writer;
            //List<List<Object>> idList = Utility.selectAccess(Parameter.connOle, usertblName, queryidCols, null, null, likeCol, likeVal, null, null, null);
            //string name = null;

            ////查询生产指令
            //for (int i = 0; i <= idList.Count / 2; i++)
            //{
            //    String tblName = "production_instruction";
            //    //编制日期？审批日期？接收日期？
            //    List<String> queryCols = new List<String>(new String[] { "product_name", "production_instruction_code", "production_process", "production_start_date", "edit_date", "principal_id" });
            //    List<String> whereCols = new List<String>(new String[] { "principal_id" });
            //    List<Object> whereVals = new List<Object>(new Object[] { idList[i][0] });
            //    String betweenCol = "production_start_date";
            //    List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, betweenCol, date1, date2);

            //    Utility.fillDataGridView(dgv, res);

            //}

            ////填入姓名
            //int rows = dgv.RowCount - 1;
            //for (int i = 0; i < rows; i++)
            //{
            //    for (int k = 0; k <= idList.Count / 2; k++)
            //    {
            //        if (Convert.ToInt32(dgv.Rows[i].Cells[5].Value) == Convert.ToInt32(idList[k][0]))
            //        {
            //            dgv.Rows[i].Cells[5].Value = idList[k][1];
            //            name = dgv.Rows[i].Cells["principal_id"].Value.ToString();
            //            break;
            //        }
            //    }
            //}
            #endregion

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            int selectIndex = this.dgv.CurrentRow.Index;
            int ID = Convert.ToInt32(this.dgv.Rows[selectIndex].Cells["ID"].Value);
            BatchProductRecord.ProcessProductInstru detailform = new BatchProductRecord.ProcessProductInstru(base.mainform, ID);
            detailform.Show();
        }




    }
}
