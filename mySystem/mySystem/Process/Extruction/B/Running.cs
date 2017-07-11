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
        List<List<TextBox>> array1 = new List<List<TextBox>>();
        List<string> colname=new List<string>(new string[]{"产品代码","产品批号","生产日期","记录时间","记录人","审核人","审核意见","审核是否通过","A层一区实际温度","A层二区实际温度","A层四区实际温度","A层换网实际温度","A层流道实际温度","B层一区实际温度","B层二区实际温度","B层三区实际温度","B层四区实际温度","B层换网实际温度","B层流道实际温度","C层一区实际温度","C层二区实际温度","C层三区实际温度","C层四区实际温度","C层换网实际温度","C层流道实际温度","模头模颈实际温度","模头一区实际温度","模头二区实际温度","模头口模实际温度","模头线速度","模头流道实际温度","第一牵引设置频率","第一牵引实际频率","第一牵引电流","第二牵引设置频率","第二牵引实际频率","第二牵引设定张力","第二牵引实际张力","第二牵引电流","外表面电机设置频率","外表面电机实际频率","外表面电机设定张力","外表面电机实际张力","外表面电机电流","外冷进风机设置频率","外冷进风机实际频率","外冷进风机电流","A层下料口温度","B层下料口温度","C层下料口温度","挤出机A层实际频率","挤出机A层电流","挤出机A层熔体温度","挤出机A层前熔体","挤出机A层后熔压","挤出机A层螺杆转速","挤出机B层实际频率","挤出机B层电流","挤出机B层熔体温度","挤出机B层前熔体","挤出机B层后熔压","挤出机B层螺杆转速","挤出机C层实际频率","挤出机C层电流","挤出机C层熔体温度","挤出机C层前熔体","挤出机C层后熔压","挤出机C层螺杆转速"});
        List<string> tabcol = new List<string>(new string[] { "实际温度(℃)", "一区", "二区", "三区", "四区", "换网", "流道", "参数记录", "设置频率", "实际频率", "设定张力", "实际张力", "电流", "转矩" });
        List<string> namelsft1 = new List<string>(new string[] { "A 层", "B 层", "C 层", "模头" });
        List<string> namemid = new List<string>(new string[] { "模颈", "一区", "二区", "口模", "线速度" });
        List<string> namemid1 = new List<string>(new string[] { "实际频率", "电流", "熔体温度", "前熔体", "后熔压", "螺杆转速" });
        List <string> weight1 = new List<string>(new string[] { "(Hz)", "(A)", "(℃)", "(Mpa)", "(Mpa)", "（rpm）" });
        List<string> namelsft2 = new List<string>(new string[]{"挤出机","A 层","B 层","C 层"});
        List<string> namecol = new List<string>(new string[] { "第一牵引", "第二牵引", "外表面电机", "外冷进风机", "内冷排风机", "内冷进风机", "外中心电机", "内表面电机", "内中心电机", "下料口温度" });
        List<string> nameright = new List<string>(new string[] { "设置频率", "实际频率", "设定张力", "实际张力", "电流", "转矩" });
        List<string> weightright = new List<string>(new string[] { "(Hz)", "(Hz)", "(kg)", "(kg)", "(A)", "(%)" });
        string note = "--------";
        public Running(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            InitializeComponent();
            conOle = Parameter.connOle;
            txb产品代码.Text = "ftfy";
            init1();
            binding();
        }

        private void init1()
        {
            
            int x = 10, y = 200;
            int wider = 100, slimer = 70;
            int  diff = wider-slimer;
            for (int i = 0; i < 14; i++)
            {
                List<TextBox> row=new List<TextBox>();
                for (int j = 0; j < 12; j++)
                {
                    TextBox tb = new TextBox();
                    if (0 == i ||7==i)
                    {
                        tb.Width = wider;
                    }
                    else
                    {
                        tb.Width=slimer;
                    }
                    if (i < 7)
                    {
                        if (i > 0)
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width + diff, y + j * tb.PreferredHeight);
                        }
                        else
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width, y + j * tb.PreferredHeight);
                        }
                    }
                    else
                    {
                        if (i == 7)
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width - 6 * diff, y + j * tb.PreferredHeight);
                        }
                        else
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width + 2*diff, y + j * tb.PreferredHeight);
                        }
                    }
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    //tb.Margin= 
                    row.Add(tb);
                    this.Controls.Add(tb);
                }
                array1.Add(row);
                array1[i][0].Text = tabcol[i];
                array1[i][0].ReadOnly = true;
                array1[i][0].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < 12; i++)
            {
                array1[0][i].ReadOnly = true;
                array1[0][i].BorderStyle = BorderStyle.None;
            }
            this.Controls.Remove(array1[0][5]);
            this.Controls.Remove(array1[6][5]);
            this.Controls.Remove(array1[7][1]);
            this.Controls.Remove(array1[0][8]);
            array1[0][4].Height = 2 * array1[0][0].Height;
            array1[0][4].BackColor = Color.Bisque;
            array1[6][4].Height = 2 * array1[0][0].Height;
            array1[7][0].Height = 2 * array1[0][0].Height;
            array1[0][7].Height = 2 * array1[0][0].Height;
            //array1[0][4].Text = "hello";
            addname1();
            hide();
            line();
        }
        private void addname1()
        {
            for (int i = 0; i < namelsft1.Count; i++)
            {
                array1[0][1 + i].Text = namelsft1[i];
                array1[0][i + 1].ReadOnly = true;
                array1[0][i + 1].BorderStyle = BorderStyle.None;
            }
            array1[0][7].Text=namelsft2[0];
            for (int i=1;i<namelsft2.Count;i++)
            {
                array1[0][8+i].Text=namelsft2[i];
            }
            for (int i = 0; i < namemid.Count; i++)
            {
                array1[1 + i][4].Text = namemid[i];
                array1[1 + i][4].ReadOnly = true;
                array1[1 + i][4].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < namemid1.Count; i++)
            {
                array1[1 + i][7].Text = namemid1[i];
                array1[1 + i][7].ReadOnly = true;
                array1[1 + i][7].BorderStyle = BorderStyle.None;
                array1[1 + i][8].Text = weight1[i];
                array1[1 + i][8].ReadOnly = true;
                array1[1 + i][8].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < namecol.Count; i++)
            {
                array1[7][2 + i].Text = namecol[i];
                array1[7][2 + i].ReadOnly = true;
                array1[7][2 + i].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < nameright.Count; i++)
            {
                array1[8 + i][0].Text = nameright[i];
                array1[8 + i][0].ReadOnly = true;
                array1[8 + i][0].BorderStyle = BorderStyle.None;
                array1[8 + i][1].Text = weightright[i];
                array1[8 + i][1].ReadOnly = true;
                array1[8 + i][1].BorderStyle = BorderStyle.None;
            }
            array1[8][11].Text = "A层/(℃)";
            array1[8][11].ReadOnly = true;
            array1[8][11].BorderStyle = BorderStyle.None;
            array1[10][11].Text = "B层/(℃)";
            array1[10][11].ReadOnly = true;
            array1[10][11].BorderStyle = BorderStyle.None;
            array1[12][11].Text = "C层/(℃)";
            array1[12][11].ReadOnly = true;
            array1[12][11].BorderStyle = BorderStyle.None;
        }
        private void hide()
        {
            for (int i = 0; i < 7; i++)
            {
                this.Controls.Remove(array1[i][6]);
            }
        }
        private void line()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    array1[8 + i][6 + j].Text = note;
                    array1[8 + i][6 + j].Enabled = false;
                }
            }
            array1[10][2].Text = note;
            array1[10][2].Enabled = false;
            array1[11][2].Text = note;
            array1[11][2].Enabled = false;
            array1[10][5].Text = note;
            array1[10][5].Enabled = false;
            array1[11][5].Text = note;
            array1[11][5].Enabled = false;
        }
        private void binding()
        {
            dtRunning = new DataTable(tablename1);
            daRunning = new OleDbDataAdapter("select * from " + tablename1+" where false", conOle);
            bsRunning = new BindingSource();
            cbRunning = new OleDbCommandBuilder(daRunning);
            daRunning.Fill(dtRunning);
            if (dtRunning.Rows.Count < 1)
            {
                //status = 1;
                DataRow newrow = dtRunning.NewRow();
                dtRunning.Rows.Add(newrow);
                //binding2(0, false);
            }
            bsRunning.DataSource = dtRunning;
            txb产品代码.DataBindings.Add("Text", bsRunning.DataSource, colname[0]);
            txb产品批号.DataBindings.Add("Text", bsRunning.DataSource, colname[1]);
            dtp生产日期.DataBindings.Add("Value", bsRunning.DataSource, colname[2]);
            dtp记录时间.DataBindings.Add("Value", bsRunning.DataSource, colname[3]);
        }
        private void pullData()
        {
            dtRunning.Rows[0][colname[0]] = txb产品代码.Text;
            dtRunning.Rows[0][colname[1]] = txb产品批号.Text;
            dtRunning.Rows[0][colname[2]] = Convert.ToDateTime(dtp生产日期.Value.ToShortDateString());
            dtRunning.Rows[0][colname[3]] = Convert.ToDateTime(dtp记录时间.Value.ToShortTimeString());

        }
        private void btn保存_Click(object sender, EventArgs e)
        {
            pullData();
            bsRunning.EndEdit();
            daRunning.Update((DataTable)bsRunning.DataSource);
        }

        private void btn插入_Click(object sender, EventArgs e)
        {
            //DataRow newDataRow = dtRunning.NewRow();
            //dtRunning.Rows.Add(newDataRow);
        }
    }
}
