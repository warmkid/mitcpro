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
using System.Runtime.InteropServices;

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
        List<string> colname = new List<string>(new string[] { "产品代码", "产品批号", "生产日期", "记录时间", "记录人", "审核人", "审核意见", "审核是否通过", "A层一区实际温度", "A层二区实际温度", "A层三区实际温度", "A层四区实际温度", "A层换网实际温度", "A层流道实际温度", "B层一区实际温度", "B层二区实际温度", "B层三区实际温度", "B层四区实际温度", "B层换网实际温度", "B层流道实际温度", "C层一区实际温度", "C层二区实际温度", "C层三区实际温度", "C层四区实际温度", "C层换网实际温度", "C层流道实际温度", "模头模颈实际温度", "模头一区实际温度", "模头二区实际温度", "模头口模实际温度", "模头线速度", "第一牵引设置频率", "第一牵引实际频率", "第一牵引电流", "第二牵引设置频率", "第二牵引实际频率", "第二牵引设定张力", "第二牵引实际张力", "第二牵引电流", "外表面电机设置频率", "外表面电机实际频率", "外表面电机设定张力", "外表面电机实际张力", "外表面电机电流", "外冷进风机设置频率", "外冷进风机实际频率", "外冷进风机电流", "A层下料口温度", "B层下料口温度", "C层下料口温度", "挤出机A层实际频率", "挤出机A层电流", "挤出机A层熔体温度", "挤出机A层前熔体", "挤出机A层后熔压", "挤出机A层螺杆转速", "挤出机B层实际频率", "挤出机B层电流", "挤出机B层熔体温度", "挤出机B层前熔体", "挤出机B层后熔压", "挤出机B层螺杆转速", "挤出机C层实际频率", "挤出机C层电流", "挤出机C层熔体温度", "挤出机C层前熔体", "挤出机C层后熔压", "挤出机C层螺杆转速" });
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
        DataTable dtSetting;
        Hashtable productCode;
        DateTime _Date,_Time;
        private CheckForm check = null;
       
        
        public Running(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            
            conOle = Parameter.connOle;
            dtSetting = new DataTable("setting");
            getProductCode();
            foreach (string s in productCode.Keys.OfType<string>().ToList<string>())
            {
                cmb产品代码.Items.Add(s);
            }

            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            
            cmb产品代码.Enabled = true;
            init1();
            setAble(false);
            getSetting();
            this.FormClosing += new FormClosingEventHandler(Running_FormClosing);
        }

        void Running_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmb产品代码.Text == "")
            {
                return;
            }
            if (txb审核人.Text.ToString().Trim() == "")
            {
                MessageBox.Show("no reviewer checked");
                e.Cancel = true;
               
            }

            //test for print
            btn打印.Enabled = true;
        }

        public Running(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            init1();
            conOle = Parameter.connOle;

            //getProductCode();
            //foreach (string s in productCode.Keys.OfType<string>().ToList<string>())
            //{
            //    cmb产品代码.Items.Add(s);
            //}
            
            dtRunning = new DataTable(tablename1);
            daRunning = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID ="+Id, conOle);
            bsRunning = new BindingSource();
            cbRunning = new OleDbCommandBuilder(daRunning);
            daRunning.Fill(dtRunning);
            removeOuterBinding();
            outerBind();
            cmb产品代码.Text = dtRunning.Rows[0]["产品代码"].ToString();

            btn保存.Visible = false;
            btn审核.Visible = false;
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            btn打印.Enabled = true;
        }

        private void getSetting()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 设置吹膜机组预热参数记录表",conOle);
            da.Fill(dtSetting);
        }
        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("生产指令产品列表");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 生产指令产品列表 WHERE 生产指令ID = "+Parameter.proInstruID , conOle);
            da.Fill(DtproductCode);
            productCode = new Hashtable();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品编码"].ToString(),dr["产品批号"].ToString());
            }
            
        }
        private void init1()
        {
            
            int x = 10, y = 150;
            int wider = 100, slimer = 70,marginy=10,marginx=2,middleMargin=30;
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
                            tb.Location = new System.Drawing.Point(x + i * tb.Width + diff+i*marginx, y + j * tb.PreferredHeight+j*marginy);
                        }
                        else
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width + i * marginx, y + j * tb.PreferredHeight + j * marginy);
                        }
                    }
                    else
                    {
                        if (i == 7)
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width - 6 * diff + i * marginx+middleMargin, y + j * tb.PreferredHeight + j * marginy);
                        }
                        else
                        {
                            tb.Location = new System.Drawing.Point(x + i * tb.Width + 2 * diff + i * marginx+middleMargin, y + j * tb.PreferredHeight + j * marginy);
                        }
                    }
                    tb.BorderStyle = BorderStyle.Fixed3D;
                    //if (0 == j % 2)
                    //{
                    //    tb.BackColor = Color.LightBlue;
                    //}
                    //else
                    //{
                    //    tb.BackColor = Color.LightSteelBlue;
                    //}
                    row.Add(tb);
                    this.Controls.Add(tb);
                }
                array1.Add(row);
                array1[i][0].Text = tabcol[i];
                array1[i][0].Enabled = false;
                array1[i][0].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < 12; i++)
            {
                array1[0][i].Enabled = false;
                array1[0][i].BorderStyle = BorderStyle.None;
            }
            this.Controls.Remove(array1[0][5]);
            this.Controls.Remove(array1[6][5]);
            this.Controls.Remove(array1[7][1]);
            this.Controls.Remove(array1[0][8]);
            array1[0][4].Height = 2 * array1[0][0].Height;
            array1[6][4].Height = 2 * array1[0][0].Height;
            array1[7][0].Height = 2 * array1[0][0].Height;
            array1[0][7].Height = 2 * array1[0][0].Height;
            
            addname1();
            hide();
            line();
        }
        private void addname1()
        {
            for (int i = 0; i < namelsft1.Count; i++)
            {
                array1[0][1 + i].Text = namelsft1[i];
                array1[0][i + 1].Enabled = false;
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
                array1[1 + i][4].Enabled = false;
                array1[1 + i][4].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < namemid1.Count; i++)
            {
                array1[1 + i][7].Text = namemid1[i];
                array1[1 + i][7].Enabled = false;
                array1[1 + i][7].BorderStyle = BorderStyle.None;
                array1[1 + i][8].Text = weight1[i];
                array1[1 + i][8].Enabled = false;
                array1[1 + i][8].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < namecol.Count; i++)
            {
                array1[7][2 + i].Text = namecol[i];
                array1[7][2 + i].Enabled = false;
                array1[7][2 + i].BorderStyle = BorderStyle.None;
            }
            for (int i = 0; i < nameright.Count; i++)
            {
                array1[8 + i][0].Text = nameright[i];
                array1[8 + i][0].Enabled = false;
                array1[8 + i][0].BorderStyle = BorderStyle.None;
                array1[8 + i][1].Text = weightright[i];
                array1[8 + i][1].Enabled = false;
                array1[8 + i][1].BorderStyle = BorderStyle.None;
            }
            array1[5][5].Text = "(m/min)";
            array1[5][5].Enabled = false;
            array1[5][5].BorderStyle = BorderStyle.None;
            array1[8][11].Text = "A层/(℃)";
            array1[8][11].Enabled = false;
            array1[8][11].BorderStyle = BorderStyle.None;
            array1[10][11].Text = "B层/(℃)";
            array1[10][11].Enabled = false;
            array1[10][11].BorderStyle = BorderStyle.None;
            array1[12][11].Text = "C层/(℃)";
            array1[12][11].Enabled = false;
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
            for (int j = 0; j < 4; j++)
            {
                array1[13][2 + j].Text = note;
                array1[13][2 + j].Enabled = false;
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
        
        private void btn保存_Click(object sender, EventArgs e)
        {
            //pullData();
            bsRunning.EndEdit();
            daRunning.Update((DataTable)bsRunning.DataSource);
            readOuterData(Parameter.proInstruID, cmb产品代码.SelectedItem.ToString(), _Date, _Time);
            removeOuterBinding();
            outerBind();
            btn审核.Enabled = true;
        }

        private void btn插入_Click(object sender, EventArgs e)
        {
            //DataRow newDataRow = dtRunning.NewRow();
            //dtRunning.Rows.Add(newDataRow);
        }
        private void setAble(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        array1[1 + i][1 + j].Enabled = true;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    array1[1 + i][5].Enabled = true;
                }
                array1[6][4].Enabled = true;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        array1[1 + i][9 + j].Enabled = true;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        array1[8 + i][2 + j].Enabled = true;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        array1[10 + i][3 + j].Enabled = true;
                    }
                }
                for (int j = 0; j < 4; j++)
                {
                    array1[12][2 + j].Enabled = true;
                }
                for (int j = 0; j < 5; j+=2)
                {
                    array1[9+j][11].Enabled = true;
                }
            }
            else
            { 
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        array1[1 + i][1 + j].Enabled = false;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    array1[1 + i][5].Enabled = false;
                }
                array1[6][4].Enabled = false;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        array1[1 + i][9 + j].Enabled = false;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        array1[8 + i][2 + j].Enabled = false;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        array1[10 + i][3 + j].Enabled = false;
                    }
                }
                for (int j = 0; j < 4; j++)
                {
                    array1[12][2 + j].Enabled = false;
                }
                for (int j = 0; j < 5; j += 2)
                {
                    array1[9 + j][11].Enabled = false;
                }
            }
        }
        private void cmb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Date = DateTime.Now.Date;
            _Time = Convert.ToDateTime(DateTime.Now.ToString());
            readOuterData(Parameter.proInstruID, cmb产品代码.SelectedItem.ToString(), _Date, _Time);
            removeOuterBinding();
            outerBind();
            if (0 == dtRunning.Rows.Count)
            {
                DataRow newrow = dtRunning.NewRow();
                newrow = writeOuterDefault(newrow);
                dtRunning.Rows.Add(newrow);
                daRunning.Update((DataTable)bsRunning.DataSource);
                readOuterData(Parameter.proInstruID, cmb产品代码.SelectedItem.ToString(), _Date, _Time);
                removeOuterBinding();
                outerBind();
            }
            cmb产品代码.Enabled = false;
            txb产品批号.Enabled = false;
            dtp生产日期.Enabled = false;
            dtp记录时间.Enabled = false;
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    //array1[i][j].TextChanged += new EventHandler(Running_TextChanged);
                    array1[i][j].Leave += new EventHandler(Running_TextChanged);
                }
            }

            array1[1][1].Leave += new EventHandler(Running_Leave1);
            array1[1][2].Leave += new EventHandler(Running_Leave1);
            array1[1][3].Leave += new EventHandler(Running_Leave1);
            array1[2][1].Leave+=new EventHandler(Running_Leave2);
            array1[2][2].Leave += new EventHandler(Running_Leave2);
            array1[2][3].Leave += new EventHandler(Running_Leave2);
            array1[3][1].Leave+=new EventHandler(Running_Leave3);
            array1[3][2].Leave += new EventHandler(Running_Leave3);
            array1[3][3].Leave += new EventHandler(Running_Leave3);
            array1[4][1].Leave+=new EventHandler(Running_Leave4);
            array1[4][2].Leave += new EventHandler(Running_Leave4);
            array1[4][3].Leave += new EventHandler(Running_Leave4);
            array1[5][1].Leave+=new EventHandler(Running_Leave5);
            array1[5][2].Leave += new EventHandler(Running_Leave5);
            array1[5][3].Leave += new EventHandler(Running_Leave5);
            array1[6][1].Leave+=new EventHandler(Running_Leave6);
            array1[6][2].Leave += new EventHandler(Running_Leave6);
            array1[6][3].Leave += new EventHandler(Running_Leave6);
            array1[1][5].Leave+=new EventHandler(Running_Leave7);
            array1[2][5].Leave+=new EventHandler(Running_Leave8);
            array1[3][5].Leave+=new EventHandler(Running_Leave9);
            array1[4][5].Leave+=new EventHandler(Running_Leave10);
            btn保存.Enabled = true;
            setAble(true);
            btn审核.Enabled = false;


            //test for print
            btn打印.Enabled = true;
        }
        void Running_Leave10(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["口模预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave9(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["机头2预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave8(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["机头1预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave7(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["模颈预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave6(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["流道预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave5(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["换网预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave4(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["四区预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave3(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["三区预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave2(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["二区预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }

        }
        void Running_Leave1(object sender, EventArgs e)
        {
            try
            {
                double content = Convert.ToDouble(((TextBox)sender).Text);
                double setting = Convert.ToDouble(dtSetting.Rows[0]["一区预热参数设定2"]);
                double diff = Convert.ToDouble(dtSetting.Rows[0]["温度公差"]);
                if (System.Math.Abs(content - setting) > diff)
                {
                    MessageBox.Show("beyond standard");
                    ((TextBox)sender).Focus();
                }
            }
            catch (FormatException)
            { }
           
        }
        void Running_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Trim() == "")
            {
                return;
            }

            try
            {
                Double.Parse(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("format error");
                ((TextBox)sender).Focus();
            }
        }
        
        void readOuterData(int instructionId, string productCode, DateTime date, DateTime time)
        {
            dtRunning = new DataTable("吹膜机组运行记录");
            daRunning=new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE 生产指令ID ="+instructionId+" AND 产品代码= '"+productCode+"' AND 生产日期=#"+date.ToString()+"# AND 记录时间=#"+time.ToString()+"#;",conOle);
            bsRunning = new BindingSource();
            cbRunning = new OleDbCommandBuilder(daRunning);
            daRunning.Fill(dtRunning);
        }
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = Parameter.proInstruID;
            dr["产品代码"] = cmb产品代码.SelectedItem.ToString();
            dr["产品批号"]=productCode[cmb产品代码.SelectedItem.ToString()];
            dr["生产日期"] = _Date;
            dr["记录时间"] = _Time;
            dr["记录人"] = Parameter.userName;
            dr["审核人"] = "";
            return dr;
        }
        void outerBind()
        { 
             cmb产品代码.DataBindings.Add("SelectedItem",bsRunning.DataSource,colname[0]);
             txb产品批号.DataBindings.Add("Text", bsRunning.DataSource, colname[1]);
             dtp生产日期.DataBindings.Add("Value", bsRunning.DataSource, colname[2]);
             dtp记录时间.DataBindings.Add("Value", bsRunning.DataSource, colname[3]);
             txb记录人.DataBindings.Add("Text", bsRunning.DataSource, colname[4]);
             txb审核人.DataBindings.Add("Text", bsRunning.DataSource, colname[5]);
            bsRunning.DataSource = dtRunning;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1+i][1+j].DataBindings.Add("Text", bsRunning.DataSource, colname[8 + i + 6 * j]);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                array1[1 + i][5].DataBindings.Add("Text", bsRunning.DataSource, colname[26 + i]);
            }
            array1[6][4].DataBindings.Add("Text", bsRunning.DataSource, colname[30]);
            array1[8][2].DataBindings.Add("Text",bsRunning.DataSource,colname[31]);
            array1[9][2].DataBindings.Add("Text",bsRunning.DataSource,colname[32]);
            array1[12][2].DataBindings.Add("Text",bsRunning.DataSource,colname[33]);
            array1[8][5].DataBindings.Add("Text",bsRunning.DataSource,colname[44]);
            array1[9][5].DataBindings.Add("Text",bsRunning.DataSource,colname[45]);
            array1[12][5].DataBindings.Add("Text",bsRunning.DataSource,colname[46]);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    array1[8+i][3+j].DataBindings.Add("Text", bsRunning.DataSource, colname[ 34+ i + 5 * j]);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1 + i][9 + j].DataBindings.Add("Text", bsRunning.DataSource, colname[50 + i + 6 * j]);
                }
            }
            array1[9][11].DataBindings.Add("Text",bsRunning.DataSource,colname[47]);
            array1[11][11].DataBindings.Add("Text",bsRunning.DataSource,colname[48]);
            array1[13][11].DataBindings.Add("Text",bsRunning.DataSource,colname[49]);
        }
        void removeOuterBinding()
        {
            cmb产品代码.DataBindings.Clear();
            txb产品批号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            dtp记录时间.DataBindings.Clear();
            txb记录人.DataBindings.Clear();
            txb审核人.DataBindings.Clear();
            bsRunning.DataSource = dtRunning;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1+i][1+j].DataBindings.Clear();
                }
            }
            for (int i = 0; i < 4; i++)
            {
                array1[1 + i][5].DataBindings.Clear();
            }
            array1[6][4].DataBindings.Clear();
            array1[8][2].DataBindings.Clear();
            array1[9][2].DataBindings.Clear();
            array1[12][2].DataBindings.Clear();
            array1[8][5].DataBindings.Clear();
            array1[9][5].DataBindings.Clear();
            array1[12][5].DataBindings.Clear();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    array1[8 + i][3 + j].DataBindings.Clear();
                }
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1 + i][9 + j].DataBindings.Clear();
                }
            }
            array1[9][11].DataBindings.Clear();
            array1[11][11].DataBindings.Clear();
            array1[13][11].DataBindings.Clear();
        }
        public override void CheckResult()
        {
            if (check.ischeckOk)
            {
                base.CheckResult();
                txb审核人.Text = check.userName.ToString();
                dtRunning.Rows[0]["审核人"] = check.userName.ToString();
                dtRunning.Rows[0]["审核意见"] = check.opinion.ToString();
                dtRunning.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);
                bsRunning.EndEdit();
                daRunning.Update((DataTable)bsRunning.DataSource);
                readOuterData(Parameter.proInstruID, cmb产品代码.SelectedItem.ToString(), _Date, _Time);
                removeOuterBinding();
                outerBind();
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                btn打印.Enabled = true;
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            print(true);
        }
        public void print(bool preview)
		{
			// 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            //System.IO.Directory.GetCurrentDirectory;
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\Extrusion\C\SOP-MFG-301-R08 吹膜机组运行记录.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 设置该进程是否可见
            oXL.Visible = true;
            // 修改Sheet中某行某列的值

            my.Cells[3, 1].Value = "产品代码：" + cmb产品代码.Text.ToString();
            my.Cells[3, 5].Value = "批号：" + txb产品批号.Text;
            my.Cells[3, 7].Value = "生产日期：" + dtp生产日期.Value.ToLongDateString();
            my.Cells[3, 10].Value = "记录时间：" + dtp记录时间.Value.ToShortTimeString();
            my.Cells[3, 12].Value = "记录人：" + txb记录人.Text;
            my.Cells[3, 14].Value = "复核人：" + txb审核人.Text;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    my.Cells[6+j, 2+i].Value = array1[i+1][j+1].Text;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                my.Cells[10, 2 + i].Value = array1[i + 1][5].Text;
            }

            my.Cells[9, 7] = array1[6][4].Text;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    my.Cells[14 + j, 2 + i].Value = array1[i + 1][j + 9].Text;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    my.Cells[7 + j, 10 + i].Value = array1[i + 8][j + 2].Text;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    my.Cells[8 + j, 12 + i].Value = array1[i + 10][j + 3].Text;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                my.Cells[7 + j, 14].Value = array1[12][j + 2].Text;
            }

            my.Cells[16, 10] =  "A层 "+array1[9][11].Text+"  (℃)";
            my.Cells[16, 12] = "B层 "+array1[11][11].Text+"  (℃)";
            my.Cells[16, 14] = "C层 " + array1[13][11].Text + "  (℃)";

			if(preview)
			{
            // 让这个Sheet为被选中状态
            my.Select();  
			 oXL.Visible=true; //加上这一行  就相当于预览功能
            }
			else
			{	
			// 直接用默认打印机打印该Sheet
           // my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
			}
		}

        
    }
}
