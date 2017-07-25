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
        List<string> list操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> list审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string note = "--------";
        string __待审核 = "__待审核";
        int searchId;
        DataTable dtSetting;
        Hashtable productCode;
        DateTime _Date,_Time;
        int instructionId;
        string productStr;
        private CheckForm check = null;

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;
        
        public Running(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            
            conOle = Parameter.connOle;
            init1();
            getPeople();
            setUserState();
            
            //getOtherData()--> -->只让部分控件可点击 {当combobox或datetimepicker取到值后：读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->addComputerEventHandler()-->setFormState()-->setEnableReadOnly() --> addOtherEvnetHandler()

            //Parameter.proInstruID, cmb产品代码.SelectedItem.ToString(), _Date, _Time
            instructionId = Parameter.proInstruID;
            productStr=cmb产品代码.SelectedItem.ToString();
            _Date=Convert.ToDateTime( dtp生产日期.Value.ToString());
            _Time = Convert.ToDateTime(dtp记录时间.Value.ToString());
            getOtherData();
            addDataEventHandler();
            addOtherEvnetHandler();
            cmb产品代码.Enabled = true;
            
            
            this.FormClosing += new FormClosingEventHandler(Running_FormClosing);
        }
        private void addDataEventHandler()
        { }
        void Running_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmb产品代码.Text == "")
            {
                return;
            }
            if (txb审核人.Text.ToString().Trim() == "")
            {
                MessageBox.Show("请提交审核");
                e.Cancel = true;               
            }

            //test for print
            btn打印.Enabled = true;
            btn查看日志.Enabled = true;
        }

        public Running(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            conOle = Parameter.connOle;
            searchId = Id;
            init1();
            getPeople();
            setUserState();

            getOtherData();
            addDataEventHandler();
            //btn提交审核.PerformClick();
            dtRunning = new DataTable(tablename1);
            daRunning = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID =" + Id, conOle);
            bsRunning = new BindingSource();
            cbRunning = new OleDbCommandBuilder(daRunning);
            daRunning.Fill(dtRunning);
            removeOuterBinding();


            instructionId =Convert.ToInt32( dtRunning.Rows[0]["生产指令ID"]);
            productStr =Convert.ToString( dtRunning.Rows[0]["产品代码"]);
            _Date = Convert.ToDateTime(dtRunning.Rows[0]["生产日期"]);
            _Time = Convert.ToDateTime(dtRunning.Rows[0]["记录时间"]);
            //add item in cmb
            cmb产品代码.Items.Add(dtRunning.Rows[0]["产品代码"].ToString());
            outerBind();

            setFormState();
            setEnableReadOnly();
            

            //btn保存.Visible = false;
            


        }
        /// <summary>
        /// get settings and so on
        /// </summary>
        private void getOtherData()
        {
            dtSetting = new DataTable("setting");
            getSetting();
            getProductCode();
            foreach (string s in productCode.Keys.OfType<string>().ToList<string>())
            {
                cmb产品代码.Items.Add(s);
            }
            
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
        /// <summary>
        /// this function will automatically draw the textboxs in the form
        /// </summary>
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

        /// <summary>
        /// this function fill some textboxes the rows name anf columns name
        /// </summary>
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

        /// <summary>
        /// this function hide some unused textboxes
        /// </summary>
        private void hide()
        {
            for (int i = 0; i < 7; i++)
            {
                this.Controls.Remove(array1[i][6]);
            }
        }
        /// <summary>
        /// this function fills textboxes with lines 
        /// </summary>
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
            try
            {
                readOuterData(searchId);
            }
            catch
            {
                readOuterData(instructionId,productStr,_Date,_Time);
            }
            removeOuterBinding();
            outerBind();
            btn提交审核.Enabled = true;
        }

        private void btn插入_Click(object sender, EventArgs e)
        {
            //DataRow newDataRow = dtRunning.NewRow();
            //dtRunning.Rows.Add(newDataRow);
        }

        /// <summary>
        /// this function controls the enable and disable status of the textbox array
        /// </summary>
        /// <param name="flag">true means enable and false opposite</param>
        private void setAble(bool flag)
        {
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    array1[i][j].Enabled = false;
                }
            }
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

        /// <summary>
        /// 设置自动计算类事件
        /// </summary>
        private void addComputerEventHandler()
        { }

        /// <summary>
        /// 其他事件，比如按钮的点击，数据有效性判断
        /// </summary>
        private void addOtherEvnetHandler()
        {
            //when operator login,new a record;
            if (0 == userState)
            {
                //this.Shown += new EventHandler(Running_Shown);
            }
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
            array1[2][1].Leave += new EventHandler(Running_Leave2);
            array1[2][2].Leave += new EventHandler(Running_Leave2);
            array1[2][3].Leave += new EventHandler(Running_Leave2);
            array1[3][1].Leave += new EventHandler(Running_Leave3);
            array1[3][2].Leave += new EventHandler(Running_Leave3);
            array1[3][3].Leave += new EventHandler(Running_Leave3);
            array1[4][1].Leave += new EventHandler(Running_Leave4);
            array1[4][2].Leave += new EventHandler(Running_Leave4);
            array1[4][3].Leave += new EventHandler(Running_Leave4);
            array1[5][1].Leave += new EventHandler(Running_Leave5);
            array1[5][2].Leave += new EventHandler(Running_Leave5);
            array1[5][3].Leave += new EventHandler(Running_Leave5);
            array1[6][1].Leave += new EventHandler(Running_Leave6);
            array1[6][2].Leave += new EventHandler(Running_Leave6);
            array1[6][3].Leave += new EventHandler(Running_Leave6);
            array1[1][5].Leave += new EventHandler(Running_Leave7);
            array1[2][5].Leave += new EventHandler(Running_Leave8);
            array1[3][5].Leave += new EventHandler(Running_Leave9);
            array1[4][5].Leave += new EventHandler(Running_Leave10);
        }

        void Running_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("new form!");
            //btn新建.PerformClick();
        }
        private void cmb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                    MessageBox.Show("超出标准");
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
                MessageBox.Show("格式错误");
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
        void readOuterData(int ID)
        {
            dtRunning = new DataTable("吹膜机组运行记录");
            daRunning = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID =" +ID+";", conOle);
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
            if (userState == 1)
            {
                cmb产品代码.DataBindings.Add("Text", bsRunning.DataSource, colname[0]);
            }
            else
            {
                cmb产品代码.DataBindings.Add("SelectedItem", bsRunning.DataSource, colname[0]);
            }
             //cmb产品代码.DataBindings.Add("SelectedItem",bsRunning.DataSource,colname[0]);
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
                //to update the running record
                base.CheckResult();
                txb审核人.Text = check.userName.ToString();
                dtRunning.Rows[0]["审核人"] = check.userName.ToString();
                dtRunning.Rows[0]["审核意见"] = check.opinion.ToString();
                dtRunning.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核通过\n";
                dtRunning.Rows[0]["日志"] = dtRunning.Rows[0]["日志"].ToString() + log;


                bsRunning.EndEdit();
                daRunning.Update((DataTable)bsRunning.DataSource);

                readOuterData(searchId);

                removeOuterBinding();
                outerBind();

                //to delete the unchecked table
                //read from database table and find current record
                string checkName = "待审核";
                DataTable dtCheck = new DataTable(checkName);
                OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", conOle);
                BindingSource bsCheck = new BindingSource();
                OleDbCommandBuilder cbCheck = new OleDbCommandBuilder(daCheck);
                daCheck.Fill(dtCheck);

                //this part will never be run, for there must be a unchecked recird before this button becomes enable
                if (0 == dtCheck.Rows.Count)
                {
                    DataRow newrow = dtCheck.NewRow();
                    newrow["表名"] = tablename1;
                    newrow["对应ID"] = dtRunning.Rows[0]["ID"];
                    dtCheck.Rows.Add(newrow);
                }
                //remove the record
                dtCheck.Rows[0].Delete();
                bsCheck.DataSource = dtCheck;
                daCheck.Update((DataTable)bsCheck.DataSource);
                formState = 2;
                setEnableReadOnly();
            }
            else
            {
                //check unpassed
                base.CheckResult();
                txb审核人.Text = check.userName.ToString();
                dtRunning.Rows[0]["审核人"] = check.userName.ToString();
                dtRunning.Rows[0]["审核意见"] = check.opinion.ToString();
                dtRunning.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtRunning.Rows[0]["日志"] = dtRunning.Rows[0]["日志"].ToString() + log;


                bsRunning.EndEdit();
                daRunning.Update((DataTable)bsRunning.DataSource);

                readOuterData(searchId);

                removeOuterBinding();
                outerBind();
                formState = 3;
                setEnableReadOnly();
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

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            //read from database table and find current record
            string checkName = "待审核";
            DataTable dtCheck = new DataTable(checkName);
            OleDbDataAdapter daCheck = new OleDbDataAdapter("SELECT * FROM " + checkName + " WHERE 表名='" + tablename1 + "' AND 对应ID = " + searchId + ";", conOle);
            BindingSource bsCheck = new BindingSource();
            OleDbCommandBuilder cbCheck= new OleDbCommandBuilder(daCheck);
            daCheck.Fill(dtCheck);

            //if current hasn't been stored, insert a record in table
            if (0 == dtCheck.Rows.Count)
            {
                DataRow newrow = dtCheck.NewRow();
                newrow["表名"] = tablename1;
                newrow["对应ID"] = dtRunning.Rows[0]["ID"];
                dtCheck.Rows.Add(newrow);
            }
            bsCheck.DataSource = dtCheck;
            daCheck.Update((DataTable)bsCheck.DataSource);

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtRunning.Rows[0]["日志"] = dtRunning.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtRunning.Rows[0]["审核人"] = __待审核;
            //update log into table
            bsRunning.EndEdit();
            daRunning.Update((DataTable)bsRunning.DataSource);
            try
            {
                readOuterData(searchId);
            }
            catch
            {
                readOuterData(instructionId, productStr, _Date, _Time);
            }
            removeOuterBinding();
            outerBind();

            formState = 1;
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }
       /// <summary>
       ///  for different user, the form will open different controls
       /// </summary>
        private void setEnableReadOnly()
        {
            switch (userState)
            {
                case 0: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (0 == formState || 3 == formState)
                    {
                        setControlTrue();
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (1 == formState || 2 == formState)
                    {
                        setControlFalse();
                    }
                    break;
                case 1: //1--审核员
                    //the formState is to be checked
                    if (1 == formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the formState do not have to be checked
                    else if (0 == formState || 2 == formState || 3 == formState)
                    {
                        setControlFalse();
                    }
                    break;
                case 2: //2--管理员
                    setControlTrue();
                    break;
                default:
                    break;
            }
        }
        // 为了方便设置控件状态，完成如下两个函数：分别用于设置所有控件可用和所有控件不可用

        //this guarantee the controls are editable
        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = false;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            //some textboxes act as column name and row name, so these shoule be forbidden
            setAble(true);
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
        }

        
        /// <summary>
        /// this guarantees the controls are uneditable
        /// </summary>
        private void setControlFalse()
        
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).ReadOnly = true;
                }
                else if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            //this act as the same in function upper
            setAble(false);
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
        }
        private void getPeople()
        {
            string tabName ="用户权限";
            DataTable dtUser = new DataTable(tabName);
            OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM "+tabName+" WHERE 步骤 = '"+tablename1+"';",conOle);
            BindingSource bsUser = new BindingSource();
            OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
            daUser.Fill(dtUser);
            if (dtUser.Rows.Count !=1)
            {
                MessageBox.Show("请确认表单权限信息");
                this.Close();
            }

            //the getPeople and setUserState combine here
            list操作员 =new List<string>(new string[]{dtUser.Rows[0]["操作员"].ToString()});
            list审核员 =new List<string>(new string[]{dtUser.Rows[0]["审核员"].ToString()});
            
        }
        private void setUserState()
        {
            if (list操作员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 0;
            }
            else if (list审核员.IndexOf(Parameter.userName) >= 0)
            {
                userState = 1;
            }
            else
            {
                userState = 2;
            }
        }
        private void setFormState()
        {
            if ("" == dtRunning.Rows[0]["审核人"].ToString().Trim())
            {
                //this means the record hasn't been saved
                formState = 0;
            }
            else if (__待审核 == dtRunning.Rows[0]["审核人"].ToString().Trim())
            {
                //this means this record should be checked
                formState = 1;
            }
            else if (Convert.ToBoolean(dtRunning.Rows[0]["审核是否通过"]))
            {
                //this means this record has been checked
                formState = 2;
            }
            else
            {
                //this means the record has been checked but need more modification
                formState = 3;
            }
        }

        private void btn新建_Click(object sender, EventArgs e)
        {
            _Date = DateTime.Now.Date;
            _Time = Convert.ToDateTime(DateTime.Now.ToString());
            try
            {
                readOuterData(searchId);
            }
            catch
            {
                readOuterData(instructionId, productStr, _Date, _Time);
            }
            removeOuterBinding();
            outerBind();
            if (0 == dtRunning.Rows.Count)
            {
                DataRow newrow = dtRunning.NewRow();
                newrow = writeOuterDefault(newrow);
                dtRunning.Rows.Add(newrow);
                daRunning.Update((DataTable)bsRunning.DataSource);
                try
                {
                    readOuterData(searchId);
                }
                catch
                {
                    readOuterData(instructionId, productStr, _Date, _Time);
                }
                removeOuterBinding();
                outerBind();
            }
            //setFormState()-->setEnableReadOnly() --> addOtherEvnetHandler()
            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();
            //cmb产品代码.Enabled = false;
            //txb产品批号.Enabled = false;
            //dtp生产日期.Enabled = false;
            //dtp记录时间.Enabled = false;

            //btn保存.Enabled = true;
            //setAble(true);
            //btn审核.Enabled = false;

            //test for print
            btn打印.Enabled = true;
            MessageBox.Show("new!");
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            { MessageBox.Show(dtRunning.Rows[0]["日志"].ToString()); }
            catch
            { MessageBox.Show(" !"); }
        }
    }
}
