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

namespace mySystem.Process.Extruction.B
{
    public partial class Running : BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "吹膜机组运行记录";
        DataTable dtRunning;
        OleDbDataAdapter daRunning;
        BindingSource bsRunning;
        OleDbCommandBuilder cbRunning;
        List<string> colname=new List<string>(new string[]{"产品代码","产品批号","生产日期","记录时间","记录人","审核人","审核意见","审核是否通过","A层一区实际温度","A层二区实际温度","A层四区实际温度","A层换网实际温度","A层流道实际温度","B层一区实际温度","B层二区实际温度","B层三区实际温度","B层四区实际温度","B层换网实际温度","B层流道实际温度","C层一区实际温度","C层二区实际温度","C层三区实际温度","C层四区实际温度","C层换网实际温度","C层流道实际温度","模头模颈实际温度","模头一区实际温度","模头二区实际温度","模头口模实际温度","模头线速度","模头流道实际温度","第一牵引设置频率","第一牵引实际频率","第一牵引电流","第二牵引设置频率","第二牵引实际频率","第二牵引设定张力","第二牵引实际张力","第二牵引电流","外表面电机设置频率","外表面电机实际频率","外表面电机设定张力","外表面电机实际张力","外表面电机电流","外冷进风机设置频率","外冷进风机实际频率","外冷进风机电流","A层下料口温度","B层下料口温度","C层下料口温度","挤出机A层实际频率","挤出机A层电流","挤出机A层熔体温度","挤出机A层前熔体","挤出机A层后熔压","挤出机A层螺杆转速","挤出机B层实际频率","挤出机B层电流","挤出机B层熔体温度","挤出机B层前熔体","挤出机B层后熔压","挤出机B层螺杆转速","挤出机C层实际频率","挤出机C层电流","挤出机C层熔体温度","挤出机C层前熔体","挤出机C层后熔压","挤出机C层螺杆转速"});
        public Running(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            InitializeComponent();
            conOle = Parameter.connOle;
            init();
            //binding();
        }

        private void init()
        {
            int x = 10, y = 240;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    TextBox tb = new TextBox();
                    tb.Location = new System.Drawing.Point(x + i * tb.Width, y + j * tb.PreferredHeight);
                    if((0==i&&4==j)||(6==i&&4==j))
                    {
                        tb.Height=42;                 
}
                    if ((0 == i && 5 == j) || (6 == i && 5 == j))
                    {
                        continue;
                    }
                    this.Controls.Add(tb);
                }
            }

            

        }

        private void binding()
        {
            dtRunning = new DataTable(tablename1);
            daRunning = new OleDbDataAdapter("select * from " + tablename1, conOle);
            bsRunning = new BindingSource();
            cbRunning = new OleDbCommandBuilder(daRunning);
            daRunning.Fill(dtRunning);
            bsRunning.DataSource = dtRunning;
            //dataGridView1.DataSource = bsRunning.DataSource;
            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Columns[1].Width = dataGridView1.Width-5;
        }
        private void btn保存_Click(object sender, EventArgs e)
        {
            bsRunning.EndEdit();
            daRunning.Update((DataTable)bsRunning.DataSource);
        }

        private void btn插入_Click(object sender, EventArgs e)
        {
            DataRow newDataRow = dtRunning.NewRow();
            dtRunning.Rows.Add(newDataRow);
        }
    }
}
