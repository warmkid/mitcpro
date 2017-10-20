using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;

namespace mySystem.Process.Stock
{
    public partial class 出库退库单 : BaseForm
    {

//        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
//                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
//        OleDbConnection conn;
        DataTable dt退库单, dt出库单;

        public 出库退库单(MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            //conn = new OleDbConnection(strConn);
            //conn.Open();


            // TODO 默认不读全部记录，一周内的
            read退库单Data();
            退库单Bind();
            read出库单Date();
            出库单Bind();
            
            addOtherEventHandler();

            setQueryControl();
        }

        private void setQueryControl()
        {
            dateTimePicker开始.Value = DateTime.Now.AddDays(-7);
            dateTimePicker结束.Value = DateTime.Now;
            comboBox审核状态.Items.Add("__待审核");
            comboBox审核状态.SelectedIndex = 0;
        }

        

        

        


        void addOtherEventHandler()
        {
            
            dgv退库单.AllowUserToAddRows = false;
            dgv出库单.AllowUserToAddRows = false;
            
            // TODO  加一个绑定完成事件，把需要审核的行标记

            dgv退库单.CellDoubleClick += new DataGridViewCellEventHandler(dgv退库单_CellDoubleClick);
            dgv出库单.CellDoubleClick += new DataGridViewCellEventHandler(dgv出库单_CellDoubleClick);
            

            // 隐藏ID等列


            dgv退库单.Columns["ID"].Visible = false;
            dgv退库单.Columns["T材料退库单ID"].Visible = false;

            dgv出库单.Columns["ID"].Visible = false;
            dgv出库单.Columns["T材料出库单ID"].Visible = false;
        }



        //show outer information
        void dgv退库单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv退库单.Rows[e.RowIndex].Cells[0].Value);
                
                材料退库出库单 form = new 材料退库出库单(mainform, id, 1);
                form.Show();
            }
        }

        //show inner information
        void dgv出库单_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int id = Convert.ToInt32(dgv出库单.Rows[e.RowIndex].Cells[1].Value);
                材料退库出库单 form = new 材料退库出库单(mainform, id, 2);
                form.Show();
            }
        }

  
        //show history between any two timepoints
        private void button查询_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(tabControl1.SelectedIndex.ToString()+"\n"+comboBox审核状态.Text);
            String shr = comboBox审核状态.Text;
            DateTime startT = dateTimePicker开始.Value.Date;
            DateTime endT = dateTimePicker结束.Value;
            OleDbDataAdapter da;
            String sql;
            switch (tabControl1.SelectedIndex)
            {
                case 0:// 退库单
                     if (shr != "")
                    {
                        sql = @"SELECT * FROM 材料退库单 where ID in (select T材料退库单ID from 材料退库单详细信息 where 审核员 like '%{0}%' and 退库日期时间 between #{1}# and #{2}#)";
                        da = new OleDbDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.connOle);
                    }
                    else
                    {
                        sql = @"SELECT * FROM 材料退库单 where ID in (select T材料退库单ID from 材料退库单详细信息 where 审核员 is null and 退库日期时间 between #{0}# and #{1}#)";
                        da = new OleDbDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.connOle);
                    }
                     dt退库单 = new DataTable("退库单");
                    da.Fill(dt退库单);
                    退库单Bind();
                    break;
                case 1: // 出库单
                    if (shr != "")
                    {
                        sql = @"select * from 材料出库单详细信息 where 审核员 like '%{0}%' and 出库日期时间 between #{1}# and #{2}#";
                        da = new OleDbDataAdapter(string.Format(sql, shr, startT, endT), mySystem.Parameter.connOle);
                    }
                    else
                    {
                        sql = @"select * from 材料出库单详细信息 where 审核员 is null and 出库日期时间 between #{0}# and #{1}#";
                        da = new OleDbDataAdapter(string.Format(sql, startT, endT), mySystem.Parameter.connOle);
                    }
                    dt出库单 = new DataTable("出库单");
                    da.Fill(dt出库单);
                    出库单Bind();
                    break;
                default:
                    break;
            }
        }

        

        
        //read history in recent 7 days defaultly
        void read退库单Data()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 材料退库单详细信息 where 退库日期时间 between #" +
                DateTime.Now.AddDays(-7).Date + "# and #" + DateTime.Now + "# and  审核员='__待审核'", mySystem.Parameter.connOle);
            dt退库单 = new DataTable("材料退库单详细信息");
            da.Fill(dt退库单);
        }

        void 退库单Bind()
        {
            dgv退库单.DataSource = dt退库单;

            Utility.setDataGridViewAutoSizeMode(dgv退库单);
        }

        private void read出库单Date()
        {

            OleDbDataAdapter da = new OleDbDataAdapter("select * from 材料出库单详细信息 where 出库日期时间 between #" +
                DateTime.Now.AddDays(-7).Date + "# and #" + DateTime.Now + "# and  审核员='__待审核'", mySystem.Parameter.connOle);
            dt出库单 = new DataTable("材料出库单详细信息");
            da.Fill(dt出库单);
        }

        private void 出库单Bind()
        {
            dgv出库单.DataSource = dt出库单;

            Utility.setDataGridViewAutoSizeMode(dgv出库单);
        }
               
        private void btn退库单读取_Click(object sender, EventArgs e)
        {
            read退库单Data();
            退库单Bind();
        }

        private void btn出库单读取_Click(object sender, EventArgs e)
        {
            read出库单Date();
            出库单Bind();
        }

        


    }
}
