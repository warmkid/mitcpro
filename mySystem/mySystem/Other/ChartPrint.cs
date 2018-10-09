using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace mySystem.Other
{
    public partial class ChartPrint : BaseForm
    {

        SqlConnection conn;
        string innerTblName, outerTblName;
        string colDate;


        public ChartPrint()
        {
            InitializeComponent();
            comboboxProcess.SelectedIndex = 0;
            comboBoxChart.SelectedIndex = 0;
            DateTime start = DateTime.Now;
            dtpStart.Value = start.AddDays(-start.Day+1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initConn();
            query();
            readDGVWidthFromSettingAndSet(dataGridView1);
        }

        void initConn()
        {
            string dbname = "";
            switch (comboboxProcess.Text)
            {
                case "PE制袋":
                    dbname = "LDPE";
                    break;
            }
            string strConnect = "server=" + Parameter.IP_port + ";database=" + dbname + ";MultipleActiveResultSets=true;Uid="
                + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
            conn = new SqlConnection(strConnect);
            conn.Open();

        }

        void query()
        {
            colDate = "领料日期时间";
            string sql = @"
select cast(生产领料使用记录详细信息.领料日期时间 as date) as 日期,
生产领料使用记录详细信息.物料代码,
生产领料使用记录详细信息.物料简称,
生产领料使用记录详细信息.物料批号,
SUM(领取数量)-SUM(退库数量) as 数量
from 生产领料使用记录,生产领料使用记录详细信息 
where 
生产领料使用记录.类型='{2}' 
and 生产领料使用记录详细信息.领料日期时间 between '{0}' and '{1}' and
生产领料使用记录详细信息.T生产领料使用记录ID = 生产领料使用记录.ID
GROUP BY 
cast(生产领料使用记录详细信息.领料日期时间 as date),
生产领料使用记录详细信息.物料代码,
生产领料使用记录详细信息.物料批号,
生产领料使用记录详细信息.物料简称";

            DataTable dt = new DataTable();
            
            //comboBoxChart // 0 内包，1 外包
            
            SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, dtpStart.Value, dtpEnd.Value, 
                comboBoxChart.SelectedItem.ToString()), conn);
            da.Fill(dt);

            dt.Columns.Add("主计量单位", Type.GetType("System.String"));
            dt.Columns.Add("辅计量单位", Type.GetType("System.String"));
            foreach (DataRow dr in dt.Rows)
            {
                string code = dr["物料代码"].ToString();
                string dwMai = Utility.find主计量单位by代码(code);
                string dwSec = Utility.find辅计量单位by代码(code);
                dr["主计量单位"] = dwMai;
                dr["辅计量单位"] = dwSec;
            }
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            writeDGVWidthToSetting(dataGridView1);   
        }

       
    }
}
