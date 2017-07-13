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
        DataTable dt = new DataTable();        
        
        public QueryInstruForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            dataGridView1.Font = new Font("宋体", 12);
            InitDataTable();

        }

        private void InitDataTable()
        {           
 
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            date1 = dateTimePicker1.Value.Date;
            date2 = dateTimePicker2.Value.Date;
            writer = textBox1.Text.Trim();
            TimeSpan delt = date2 - date1;
            if (delt.TotalDays < 0)
            {
                MessageBox.Show("起止时间有误，请重新输入");
                return;
            }


            //模糊查询用户id
            String usertblName = "user_aoxing";
            List<String> queryidCols = new List<String>(new String[] { "user_id","user_name" });
            String likeCol = "user_name";
            String likeVal = writer;
            List<List<Object>> idList = Utility.selectAccess(Parameter.connOle, usertblName, queryidCols, null, null, likeCol, likeVal, null, null, null);
            string name = null;

            //查询生产指令
            for (int i = 0; i <= idList.Count / 2; i++)
            {
                String tblName = "production_instruction";
                //编制日期？审批日期？接收日期？
                List<String> queryCols = new List<String>(new String[] { "product_name", "production_instruction_code", "production_process", "production_start_date", "edit_date", "principal_id" });
                List<String> whereCols = new List<String>(new String[] { "principal_id" });
                List<Object> whereVals = new List<Object>(new Object[] { idList[i][0] });
                String betweenCol = "production_start_date";
                List<List<Object>> res = Utility.selectAccess(Parameter.connOle, tblName, queryCols, whereCols, whereVals, null, null, betweenCol, date1, date2);

                Utility.fillDataGridView(dataGridView1, res);

            }

            //填入姓名
            int rows = dataGridView1.RowCount - 1;
            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k <= idList.Count / 2; k++)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value) == Convert.ToInt32(idList[k][0]))
                    {
                        dataGridView1.Rows[i].Cells[5].Value = idList[k][1];
                        name = dataGridView1.Rows[i].Cells["principal_id"].Value.ToString();
                        break;
                    }
                }
            }




        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }




    }
}
