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

namespace WindowsFormsApplication1
{
    public partial class Record_extrusClean : Form
    {
        private ExtructionProcess extructionformfather = null;
        public Record_extrusClean(ExtructionProcess winMain)
        {
            InitializeComponent();
            extructionformfather = winMain;
            Init();
            connToServer();
            qury();

        }
        private void Init()
        {
            strCon = @"server=10.105.223.19,56625;database=wyttest;Uid=sa;Pwd=mitc";
            sql = "select * from CleanArea_table";
            isOk = false;
            cleancont = new List<cont>();
            cont_clean = new cont();
        }
        public void connToServer()
        {
            conn = new SqlConnection(strCon);
            conn.Open();
            isOk = true;
        }

        private void qury()
        {
            if (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return;
            }
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);

            DataTable dt = new DataTable();
            da.Fill(dt);
            //dataGridView1.DataSource = dt;

            //DataColumn dc = dt.Columns[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = dt.Rows[i][0].ToString();
                dr.Cells[1].Value = dt.Rows[i][1].ToString();
                dr.Cells[2].Value = true;
                //dr.Cells[2].Value = "是".ToString();
                dataGridView1.Rows.Add(dr);
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        string cleantime;//清洁日期
        string classes;//班次
        string checker;//复核人
        string checktime;//复核日期
        List<cont> cleancont;

        bool isOk;
        string strCon;
        string sql;
        SqlConnection conn;

        public class cont 
        {
            public bool cleanstat;
            public string cleaner;
            public string cleanchecker;
            public cont() { cleanstat = true; cleaner = ""; cleanchecker = ""; }
        }
        cont cont_clean;

        public void DataSave()
        {
            cleantime = dateTimePicker1.Text.ToString();
            //textBox1.Text = cleantime;
            classes = textBox1.Text.ToString();
            checker = textBox2.Text.ToString();
            checktime = dateTimePicker2.Text.ToString();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                cont_clean.cleanstat = dataGridView1.Rows[i].Cells[2].Value.ToString() == "True";
                if (null == dataGridView1.Rows[i].Cells[3].Value)
                    cont_clean.cleaner = "";
                else cont_clean.cleaner = dataGridView1.Rows[i].Cells[3].Value.ToString();
                if (null == dataGridView1.Rows[i].Cells[4].Value)
                    cont_clean.cleanchecker = "";
                else cont_clean.cleanchecker = dataGridView1.Rows[i].Cells[4].Value.ToString();
                cleancont.Add(cont_clean);

                System.Console.WriteLine(cleancont[i].cleanstat.ToString() + cleancont[i].cleaner.ToString() + cleancont[i].cleanchecker.ToString());
            }
        }

    }
}
