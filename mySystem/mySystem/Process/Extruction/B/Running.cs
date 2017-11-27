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
using System.Text.RegularExpressions;

namespace mySystem.Process.Extruction.B
{

    public partial class Running : BaseForm
    {
        bool isSaved = false;
        OleDbConnection conOle;
        string tablename1 = "吹膜机组运行记录";
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        BindingSource bsOuter;
        OleDbCommandBuilder cbOuter;

        SqlDataAdapter daOutersql;
        SqlCommandBuilder cbOutersql;

        List<List<String>> array2 = new List<List<String>>();
        List<List<TextBox>> array1 = new List<List<TextBox>>();
        List<string> colname = new List<string>(new string[] { "产品代码", "产品批号", "生产日期", "记录时间", "记录员", "记录员备注", "审核员", "审核意见", "审核是否通过", "A层一区实际温度", "A层二区实际温度", "A层三区实际温度", "A层四区实际温度", "A层换网实际温度", "A层流道实际温度", "B层一区实际温度", "B层二区实际温度", "B层三区实际温度", "B层四区实际温度", "B层换网实际温度", "B层流道实际温度", "C层一区实际温度", "C层二区实际温度", "C层三区实际温度", "C层四区实际温度", "C层换网实际温度", "C层流道实际温度", "模头模颈实际温度", "模头一区实际温度", "模头二区实际温度", "模头口模实际温度", "模头线速度", "第一牵引设置频率", "第一牵引实际频率", "第一牵引电流", "第二牵引设置频率", "第二牵引实际频率", "第二牵引设定张力", "第二牵引实际张力", "第二牵引电流", "外表面电机设置频率", "外表面电机实际频率", "外表面电机设定张力", "外表面电机实际张力", "外表面电机电流", "外冷进风机设置频率", "外冷进风机实际频率", "外冷进风机电流", "A层下料口温度", "B层下料口温度", "C层下料口温度", "挤出机A层实际频率", "挤出机A层电流", "挤出机A层熔体温度", "挤出机A层前熔体", "挤出机A层后熔压", "挤出机A层螺杆转速", "挤出机B层实际频率", "挤出机B层电流", "挤出机B层熔体温度", "挤出机B层前熔体", "挤出机B层后熔压", "挤出机B层螺杆转速", "挤出机C层实际频率", "挤出机C层电流", "挤出机C层熔体温度", "挤出机C层前熔体", "挤出机C层后熔压", "挤出机C层螺杆转速" });
        List<string> tabcol = new List<string>(new string[] { "实际温度(℃)", "一区", "二区", "三区", "四区", "换网", "流道", "参数记录", "设置频率", "实际频率", "设定张力", "实际张力", "电流", "转矩" });
        List<string> namelsft1 = new List<string>(new string[] { "A 层", "B 层", "C 层", "模头" });
        List<string> namemid = new List<string>(new string[] { "模颈", "一区", "二区", "口模", "线速度" });
        List<string> namemid1 = new List<string>(new string[] { "实际频率", "电流", "熔体温度", "前熔体", "后熔压", "螺杆转速" });
        List <string> weight1 = new List<string>(new string[] { "(Hz)", "(A)", "(℃)", "(Mpa)", "(Mpa)", "（rpm）" });
        List<string> namelsft2 = new List<string>(new string[]{"挤出机","A 层","B 层","C 层"});
        List<string> namecol = new List<string>(new string[] { "第一牵引", "第二牵引", "外表面电机", "外冷进风机", "内冷排风机", "内冷进风机", "外中心电机", "内表面电机", "内中心电机", "下料口温度" });
        List<string> nameright = new List<string>(new string[] { "设置频率", "实际频率", "设定张力", "实际张力", "电流", "转矩" });
        List<string> weightright = new List<string>(new string[] { "(Hz)", "(Hz)", "(kg)", "(kg)", "(A)", "(%)" });
        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string note = "-";
        string __待审核 = "__待审核";
        int searchId;
        DataTable dtSetting;
        Hashtable productCode;
        DateTime _Date,_Time;
        int _生产指令ID;
        string _产品代码;
        private CheckForm check = null;

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>

        Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        
        public Running(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            
            //only function without Id deals with cmb.selectedChange
            this.cmb产品代码.SelectedIndexChanged += new System.EventHandler(this.cmb产品代码_SelectedIndexChanged);
            conOle = Parameter.connOle;
            自动绘制表格();
            getPeople();
            setUserState();
            getOtherData();

            _生产指令ID = Parameter.proInstruID;
            _产品代码 = ""; 
            _Date=Convert.ToDateTime( dtp生产日期.Value.ToString());
            _Time = Convert.ToDateTime(dtp记录时间.Value.ToString());

            addDataEventHandler();
            addOtherEvnetHandler();
            cmb产品代码.Enabled = true;
            //this.FormClosing += new FormClosingEventHandler(Running_FormClosing);
            setFormState(true);
            setEnableReadOnly();
            填写界面上被disable的部分为横线();
        }
        private void addDataEventHandler()
        { }
        void Running_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (cmb产品代码.Text == "")
            //{
            //    return;
            //}
            //if (txb审核员.Text.ToString().Trim() == "")
            //{
            //    MessageBox.Show("请提交审核");
            //    e.Cancel = true;               
            //}

            ////test for print
            //btn打印.Enabled = true;
            //btn查看日志.Enabled = true;
            try
            {

                if (!isSaved)
                {
                    dtOuter.Rows[0].Delete();
                    daOuter.Update((DataTable)bsOuter.DataSource);
                }
                else
                {
                    bsOuter.EndEdit();
                    daOuter.Update((DataTable)bsOuter.DataSource);
                    readOuterData(_生产指令ID, _产品代码, _Date, _Time);
                    removeOuterBinding();
                    outerBind();
                }
            }
            catch (Exception ee)
            {
            }
        }

        public Running(mySystem.MainForm mainform, int Id)
            : base(mainform)
        {
            InitializeComponent();
            //different handler function
            isSaved = true;
            conOle = Parameter.connOle;
            searchId = Id;
            自动绘制表格();
            getPeople();
            setUserState();

            getOtherData();
            addDataEventHandler();
            //btn提交审核.PerformClick();
            dtOuter = new DataTable(tablename1);
            daOuter = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID =" + Id, conOle);
            bsOuter = new BindingSource();
            cbOuter = new OleDbCommandBuilder(daOuter);
            daOuter.Fill(dtOuter);
            removeOuterBinding();


            _生产指令ID =Convert.ToInt32( dtOuter.Rows[0]["生产指令ID"]);
            _产品代码 =Convert.ToString( dtOuter.Rows[0]["产品代码"]);
            _Date = Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]);
            _Time = Convert.ToDateTime(dtOuter.Rows[0]["记录时间"]);
            //add item in cmb
            cmb产品代码.Items.Add(dtOuter.Rows[0]["产品代码"].ToString());
            outerBind();

            
            //btn保存.Visible = false;

            //cmb产品代码.SelectedIndexChanged += new EventHandler(cmb产品代码_SelectedIndexChanged_without_Id);
            填写界面上被disable的部分为横线();
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
            fill_printer();
        }
        private void getSetting()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 设置吹膜机组预热参数记录表", conOle);
                da.Fill(dtSetting);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM 设置吹膜机组预热参数记录表", mySystem.Parameter.conn);
                da.Fill(dtSetting);
            }
            
        }
        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("生产指令产品列表");
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 生产指令产品列表 WHERE 生产指令ID = " + Parameter.proInstruID, conOle);
                da.Fill(DtproductCode);
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM 生产指令产品列表 WHERE 生产指令ID = " + Parameter.proInstruID, mySystem.Parameter.conn);
                da.Fill(DtproductCode);
            }
           
            productCode = new Hashtable();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品编码"].ToString(),dr["产品批号"].ToString());
            }
            
        }
        /// <summary>
        /// this function will automatically draw the textboxs in the form
        /// </summary>
        private void 自动绘制表格()
        {            
            int x = 10, y = 150;
            int wider = 100, slimer = 70,marginy=10,marginx=2,middleMargin=30;
            int  diff = wider-slimer;
            for (int i = 0; i < 14; i++)
            {
                List<TextBox> row=new List<TextBox>();
                List<String> row1 = new List<string>();
                for (int j = 0; j < 12; j++)
                {
                    TextBox tb = new TextBox();
                    string bd = "";
                    tb.Text = "                             ";
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
                    row1.Add(bd);
                }
                array1.Add(row);
                array1[i][0].Text = tabcol[i];
                array1[i][0].Enabled = false;
                array1[i][0].BorderStyle = BorderStyle.None;
                array2.Add(row1);
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
            
            添加行列名称();
            隐藏多余TextBox();
            
        }

        /// <summary>
        /// this function fill some textboxes the rows name anf columns name
        /// </summary>
        private void 添加行列名称()
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
        private void 隐藏多余TextBox()
        {
            for (int i = 0; i < 7; i++)
            {
                this.Controls.Remove(array1[i][6]);
            }
        }
        /// <summary>
        /// this function fills textboxes with lines 
        /// </summary>
        private void 表格横线()
        {
            //for (int i = 0; i < 6; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        array1[8 + i][6 + j].Text = note;
            //        array1[8 + i][6 + j].Enabled = true;
            //    }
            //}
            //for (int j = 0; j < 4; j++)
            //{
            //    array1[13][2 + j].Text = note;
            //    array1[13][2 + j].Enabled = true;
            //}
            //array1[10][2].Text = note;
            //array1[10][2].Enabled = true;
            //array1[11][2].Text = note;
            //array1[11][2].Enabled = true;
            //array1[10][5].Text = note;
            //array1[10][5].Enabled = true;
            //array1[11][5].Text = note;
            //array1[11][5].Enabled = true;
            for (int i = 8; i <= 13; ++i)
            {
                for (int j = 2; j <= 10; ++j)
                {
                    array1[i][j].Text = note;
                }
            }
            for (int i = 1; i <= 6; ++i)
            {
                for (int j = 9; j <= 11; ++j)
                {
                    array1[i][j].Text = note;
                }
            }

            array1[9][11].Text = note;
            array1[11][11].Text = note;
            array1[13][11].Text = note;
            array1[6][4].Text = note;
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            //pullData();
            isSaved = true;
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_生产指令ID,_产品代码,_Date,_Time);
            removeOuterBinding();
            outerBind();
            if (_userState == Parameter.UserState.操作员)
            {
                btn提交审核.Enabled = true;
            }
            try { (this.Owner as ExtructionMainForm).InitBtn(); }
            catch (NullReferenceException exp) { }
            
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
        private void 可填写部分TextBox使能(bool flag)
        {
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    array1[i][j].Enabled = false;
                }
            }

            //
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1 + i][1 + j].Enabled = flag;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                array1[1 + i][5].Enabled = flag;
            }
            array1[6][4].Enabled = flag;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1 + i][9 + j].Enabled = flag;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    array1[8 + i][2 + j].Enabled = flag;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    array1[10 + i][2 + j].Enabled = flag;
                }
            }
            for (int j = 0; j < 9; j++)
            {
                array1[12][2 + j].Enabled = flag;
                array1[13][2 + j].Enabled = flag;
            }
            for (int j = 0; j < 5; j += 2)
            {
                array1[9 + j][11].Enabled = flag;
            }

            for (int j = 6; j <= 8; ++j)
            {
                array1[8][j].Enabled = false;
                array1[9][j].Enabled = false;
                array1[12][j].Enabled = false;
            }
            array1[8][10].Enabled = false;
            array1[9][10].Enabled = false;
            array1[12][10].Enabled = false;

            for (int j = 5; j <= 8; ++j)
            {
                array1[10][j].Enabled = false;
                array1[11][j].Enabled = false;
            }
            array1[10][2].Enabled = false;
            array1[10][10].Enabled = false;
            array1[11][2].Enabled = false;
            array1[11][10].Enabled = false;

            for (int j = 2; j <= 10; ++j)
            {
                array1[13][j].Enabled = false;
            }

            //

            //if (flag)
            //{
            //    for (int i = 0; i < 6; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            array1[1 + i][1 + j].Enabled = true;
            //        }
            //    }
            //    for (int i = 0; i < 4; i++)
            //    {
            //        array1[1 + i][5].Enabled = true;
            //    }
            //    array1[6][4].Enabled = true;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            array1[1 + i][9 + j].Enabled = true;
            //        }
            //    }
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            array1[8 + i][2 + j].Enabled = true;
            //        }
            //    }
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 2; j++)
            //        {
            //            array1[10 + i][3 + j].Enabled = true;
            //        }
            //    }
            //    for (int j = 0; j < 4; j++)
            //    {
            //        array1[12][2 + j].Enabled = true;
            //    }
            //    for (int j = 0; j < 5; j+=2)
            //    {
            //        array1[9+j][11].Enabled = true;
            //    }
            //}
            //else
            //{ 
            //    for (int i = 0; i < 6; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            array1[1 + i][1 + j].Enabled = false;
            //        }
            //    }
            //    for (int i = 0; i < 4; i++)
            //    {
            //        array1[1 + i][5].Enabled = false;
            //    }
            //    array1[6][4].Enabled = false;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            array1[1 + i][9 + j].Enabled = false;
            //        }
            //    }
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            array1[8 + i][2 + j].Enabled = false;
            //        }
            //    }
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 2; j++)
            //        {
            //            array1[10 + i][3 + j].Enabled = false;
            //        }
            //    }
            //    for (int j = 0; j < 4; j++)
            //    {
            //        array1[12][2 + j].Enabled = false;
            //    }
            //    for (int j = 0; j < 5; j += 2)
            //    {
            //        array1[9 + j][11].Enabled = false;
            //    }
            //}
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
            if (0 == _userState)
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

        
        private void cmb产品代码_SelectedIndexChanged(object sender, EventArgs e)
        {
            _产品代码 = cmb产品代码.SelectedItem.ToString();
            txb产品批号.Text = productCode[cmb产品代码.SelectedItem.ToString()].ToString();
            MessageBox.Show("请点击'新建'按钮!");
            btn新建.Enabled = true;
            //btn新建.PerformClick();
        }

        private void cmb产品代码_SelectedIndexChanged_without_Id(object sender, EventArgs e)
        {
            _产品代码 = cmb产品代码.SelectedItem.ToString();
            txb产品批号.Text = productCode[cmb产品代码.SelectedItem.ToString()].ToString();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
                    //((TextBox)sender).Focus();
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
            if (((TextBox)sender).Text.Trim() == note)
            {
                return;
            }
            try
            {
                TextBox tb = (sender as TextBox);
                if (tbchecker(tb))
                {
                    int val = Int32.Parse(((TextBox)sender).Text);
                    String str = val.ToString();
                    ((TextBox)sender).Text = str;
                    ((TextBox)sender).DataBindings[0].WriteValue();
                    //((TextBox)sender).Focus();
                }
                else
                {
                    double val = Double.Parse(((TextBox)sender).Text);
                    String str = val.ToString("f1");
                    ((TextBox)sender).Text = str;
                    ((TextBox)sender).DataBindings[0].WriteValue();
                    //((TextBox)sender).Focus();
                }
               
            }
            catch (ArgumentOutOfRangeException ee)
            {
                //MessageBox.Show("格式错误");
                //((TextBox)sender).Text = note;
                //((TextBox)sender).Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show("格式错误");
                ((TextBox)sender).Text = note;
            }
        }
        
        // 检查当前textbox是不是下面的温度压强那几个
        bool tbchecker(TextBox tb)
        {
            for (int i = 3; i <= 5; ++i)
            {
                for (int j = 9; j <= 11; ++j)
                {
                    if (tb.Equals(array1[i][j]))
                    {
                        return true;
                    }
                }
            }
            //if(tb.Equals(array1[9][11])) return true;
            //if(tb.Equals(array1[11][11])) return true;
            //if(tb.Equals(array1[13][11])) return true;
            return false;
        }

        void readOuterData(int instructionId, string productCode, DateTime date, DateTime time)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dtOuter = new DataTable("吹膜机组运行记录");
                daOuter = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE 生产指令ID =" + instructionId + " AND 产品代码= '" + productCode + "' AND 生产日期=#" + date.ToString() + "# AND 记录时间=#" + time.ToString() + "#;", conOle);
                bsOuter = new BindingSource();
                cbOuter = new OleDbCommandBuilder(daOuter);
                daOuter.Fill(dtOuter);
            }
            else
            {
                dtOuter = new DataTable("吹膜机组运行记录");
                daOutersql = new SqlDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE 生产指令ID =" + instructionId + " AND 产品代码= '" + productCode + "' AND 生产日期='" + date.ToString() + "' AND 记录时间='" + time.ToString() + "';", mySystem.Parameter.conn);
                bsOuter = new BindingSource();
                cbOutersql = new SqlCommandBuilder(daOutersql);
                daOutersql.Fill(dtOuter);
            }
            
        }
        void readOuterData(int ID)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                dtOuter = new DataTable("吹膜机组运行记录");
                daOuter = new OleDbDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID =" + ID + ";", conOle);
                bsOuter = new BindingSource();
                cbOuter = new OleDbCommandBuilder(daOuter);
                daOuter.Fill(dtOuter);
            }
            else
            {
                dtOuter = new DataTable("吹膜机组运行记录");
                daOutersql = new SqlDataAdapter("SELECT * FROM 吹膜机组运行记录 WHERE ID =" + ID + ";", mySystem.Parameter.conn);
                bsOuter = new BindingSource();
                cbOutersql = new SqlCommandBuilder(daOutersql);
                daOutersql.Fill(dtOuter);
            }
            
        }
        DataRow writeOuterDefault(DataRow dr)
        {
            // 读取历史数据
           

            dr["生产指令ID"] = Parameter.proInstruID;
            dr["产品代码"] =  cmb产品代码.SelectedItem.ToString();
            dr["产品批号"] = productCode[cmb产品代码.SelectedItem.ToString()];
            dr["生产日期"] = _Date;
            dr["记录时间"] = _Time;
            dr["记录员"] = Parameter.userName;
            dr["记录员备注"] = "无";
            dr["审核员"] = "";
            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 创建记录
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 创建记录\n";
            log += "生产指令编码：" + mySystem.Parameter.proInstruction + "\n";
            dr["日志"] = log;

            return dr;
        }
        void outerBind()
        {
            if (Parameter.UserState.审核员== _userState)
            {
                cmb产品代码.DataBindings.Add("Text", bsOuter.DataSource, colname[0]);
            }
            else
            {
                cmb产品代码.DataBindings.Add("SelectedItem", bsOuter.DataSource, colname[0]);
            }
             //cmb产品代码.DataBindings.Add("SelectedItem",bsRunning.DataSource,colname[0]);
             txb产品批号.DataBindings.Add("Text", bsOuter.DataSource, colname[1]);
             dtp生产日期.DataBindings.Add("Value", bsOuter.DataSource, colname[2]);
             dtp记录时间.DataBindings.Add("Value", bsOuter.DataSource, colname[3]);
             txb记录员.DataBindings.Add("Text", bsOuter.DataSource, colname[4]);
             txb记录员备注.DataBindings.Add("Text", bsOuter.DataSource, colname[5]);
             txb审核员.DataBindings.Add("Text", bsOuter.DataSource, colname[6]);
            bsOuter.DataSource = dtOuter;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1+i][1+j].DataBindings.Add("Text", bsOuter.DataSource, colname[9 + i + 6 * j]);
                    if (dtOuter.Rows.Count != 0)
                        array2[1 + i][1 + j] = Convert.ToString(dtOuter.Rows[0][colname[9 + i + 6 * j]]);
                }
            }


            for (int i = 0; i < 4; i++)
            {
                array1[1 + i][5].DataBindings.Add("Text", bsOuter.DataSource, colname[27 + i]);
                if (dtOuter.Rows.Count != 0)
                    array2[1 + i][5] = Convert.ToString(dtOuter.Rows[0][colname[27 + i]]);
            }
            array1[6][4].DataBindings.Add("Text", bsOuter.DataSource, colname[31]);
            array1[8][2].DataBindings.Add("Text",bsOuter.DataSource,colname[32]);
            array1[9][2].DataBindings.Add("Text",bsOuter.DataSource,colname[33]);
            array1[12][2].DataBindings.Add("Text",bsOuter.DataSource,colname[34]);
            array1[8][5].DataBindings.Add("Text",bsOuter.DataSource,colname[45]);
            array1[9][5].DataBindings.Add("Text",bsOuter.DataSource,colname[46]);
            array1[12][5].DataBindings.Add("Text",bsOuter.DataSource,colname[47]);

            if (dtOuter.Rows.Count != 0)
            {
                array2[6][4] = Convert.ToString(dtOuter.Rows[0][colname[31]]);
                array2[8][2] = Convert.ToString(dtOuter.Rows[0][colname[32]]);
                array2[9][2] = Convert.ToString(dtOuter.Rows[0][colname[33]]);
                array2[12][2] = Convert.ToString(dtOuter.Rows[0][colname[34]]);
                array2[8][5] = Convert.ToString(dtOuter.Rows[0][colname[45]]);
                array2[9][5] = Convert.ToString(dtOuter.Rows[0][colname[46]]);
                array2[12][5] = Convert.ToString(dtOuter.Rows[0][colname[47]]);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    array1[8+i][3+j].DataBindings.Add("Text", bsOuter.DataSource, colname[ 35+ i + 5 * j]);
                    if (dtOuter.Rows.Count != 0)
                        array2[8 + i][3 + j] = Convert.ToString(dtOuter.Rows[0][colname[35 + i + 5 * j]]);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    array1[1 + i][9 + j].DataBindings.Add("Text", bsOuter.DataSource, colname[51 + i + 6 * j]);
                    if (dtOuter.Rows.Count != 0)
                        array2[1 + i][9 + j] = Convert.ToString(dtOuter.Rows[0][colname[51 + i + 6 * j]]);
                }
            }

            array1[9][11].DataBindings.Clear();
            array1[9][11].DataBindings.Add("Text",bsOuter.DataSource,colname[48]);
            if (dtOuter.Rows.Count != 0)
                array2[9][11] = Convert.ToString(dtOuter.Rows[0][colname[48]]);
            array1[11][11].DataBindings.Clear();
            array1[11][11].DataBindings.Add("Text",bsOuter.DataSource,colname[49]);
            if (dtOuter.Rows.Count != 0)
                array2[11][11] = Convert.ToString(dtOuter.Rows[0][colname[49]]);
            array1[13][11].DataBindings.Clear();
            array1[13][11].DataBindings.Add("Text",bsOuter.DataSource,colname[50]);
            if (dtOuter.Rows.Count != 0)
                array2[13][11] = Convert.ToString(dtOuter.Rows[0][colname[50]]);



            array1[8][9].DataBindings.Clear();
            array1[8][9].DataBindings.Add("Text", bsOuter.DataSource, "内表面电机设置频率");
            if (dtOuter.Rows.Count != 0)
                array2[8][9] = Convert.ToString(dtOuter.Rows[0]["内表面电机设置频率"]);
            array1[9][9].DataBindings.Clear();
            array1[9][9].DataBindings.Add("Text", bsOuter.DataSource, "内表面电机实际频率");
            if (dtOuter.Rows.Count != 0)
                array2[9][9] = Convert.ToString(dtOuter.Rows[0]["内表面电机实际频率"]);
            array1[10][9].DataBindings.Clear();
            array1[10][9].DataBindings.Add("Text", bsOuter.DataSource, "内表面电机设定张力");
            if (dtOuter.Rows.Count != 0)
                array2[10][9] = Convert.ToString(dtOuter.Rows[0]["内表面电机设定张力"]);
            array1[11][9].DataBindings.Clear();
            array1[11][9].DataBindings.Add("Text", bsOuter.DataSource, "内表面电机实际张力");
            if (dtOuter.Rows.Count != 0)
                array2[11][9] = Convert.ToString(dtOuter.Rows[0]["内表面电机实际张力"]);
            array1[12][9].DataBindings.Clear();
            array1[12][9].DataBindings.Add("Text", bsOuter.DataSource, "内表面电机电流");
            if (dtOuter.Rows.Count != 0)
                array2[12][9] = Convert.ToString(dtOuter.Rows[0]["内表面电机电流"]); ;


        }
        void removeOuterBinding()
        {
            cmb产品代码.DataBindings.Clear();
            txb产品批号.DataBindings.Clear();
            dtp生产日期.DataBindings.Clear();
            dtp记录时间.DataBindings.Clear();
            txb记录员.DataBindings.Clear();
            txb记录员备注.DataBindings.Clear();
            txb审核员.DataBindings.Clear();
            bsOuter.DataSource = dtOuter;
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
           
        }
        public override void CheckResult()
        {
            if (Parameter.userName == dtOuter.Rows[0]["记录员"].ToString())
            {
                MessageBox.Show("记录员,审核员重复");
                return;
            }
            if (check.ischeckOk)
            {
                //to update the running record
                base.CheckResult();
                txb审核员.Text = Parameter.userName;
                dtOuter.Rows[0]["审核员"] = Parameter.userName;
                dtOuter.Rows[0]["审核意见"] = check.opinion.ToString();
                dtOuter.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);

                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 审核通过\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;


                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);

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
                    newrow["对应ID"] = dtOuter.Rows[0]["ID"];
                    dtCheck.Rows.Add(newrow);
                }
                //remove the record
                dtCheck.Rows[0].Delete();
                bsCheck.DataSource = dtCheck;
                daCheck.Update((DataTable)bsCheck.DataSource);
                setFormState();
                setEnableReadOnly();
            }
            else
            {
                //check unpassed
                base.CheckResult();
                txb审核员.Text = Parameter.userName;
                dtOuter.Rows[0]["审核员"] = Parameter.userName;
                dtOuter.Rows[0]["审核意见"] = check.opinion.ToString();
                dtOuter.Rows[0]["审核是否通过"] = Convert.ToBoolean(check.ischeckOk);


                //this part to add log 
                //格式： 
                // =================================================
                // yyyy年MM月dd日，操作员：XXX 审核
                string log = "=====================================\n";
                log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 审核不通过\n";
                dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;


                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);

                readOuterData(searchId);

                removeOuterBinding();
                outerBind();
                setFormState();
                setEnableReadOnly();
            }
        }
        private void btn审核_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.userName == dtOuter.Rows[0]["记录员"].ToString())
            {
                MessageBox.Show("操作员和审核员不能是同一个人");
                return;
            }
            check = new CheckForm(this);
            check.Show();
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印机选择.Items.Add(sPrint);
            }
            cmb打印机选择.SelectedItem = print.PrinterSettings.PrinterName;
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cmb打印机选择.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cmb打印机选择.Text);
            print(false);
            GC.Collect();
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
            //oXL.Visible = true;
            // 修改Sheet中某行某列的值z



            my.Cells[3, 1].Value = "产品代码：" + dtOuter.Rows[0]["产品代码"];
            my.Cells[3, 5].Value = "批号：" + dtOuter.Rows[0]["产品批号"];
            my.Cells[3, 7].Value = "生产日期：" + Convert.ToDateTime(dtOuter.Rows[0]["生产日期"]).ToString("yyyy年MM月dd日");
            my.Cells[3, 10].Value = "记录时间：" +  Convert.ToDateTime(dtOuter.Rows[0]["记录时间"]).ToString("yyyy年MM月dd日");
            my.Cells[3, 12].Value = "记录人：" + dtOuter.Rows[0]["记录员"];
            my.Cells[3, 14].Value = "复核人：" + dtOuter.Rows[0]["审核员"];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    my.Cells[6+j, 2+i].Value = array2[i+1][j+1];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                my.Cells[10, 2 + i].Value = array2[i + 1][5];
            }

            my.Cells[9, 7] = array2[6][4];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    my.Cells[14 + j, 2 + i].Value = array2[i + 1][j + 9];
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    my.Cells[7 + j, 10 + i].Value = array2[i + 8][j + 2];
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    my.Cells[8 + j, 12 + i].Value = array2[i + 10][j + 3];
                }
            }

            for (int j = 0; j < 4; j++)
            {
                my.Cells[7 + j, 14].Value = array2[12][j + 2];
            }

            my.Cells[16, 10] =  "A层 "+array2[9][11]+"  (℃)";
            my.Cells[16, 12] = "B层 "+array2[11][11]+"  (℃)";
            my.Cells[16, 14] = "C层 " + array2[13][11] + "  (℃)";
            string instr;
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + _生产指令ID, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            my.PageSetup.RightFooter = dt.Rows[0]["生产指令编号"].ToString() + "-08-" + find_indexofprint().ToString("D3") + "  &P/" + wb.ActiveSheet.PageSetup.Pages.Count; ; // &P 是页码
			if(preview)
			{
            // 让这个Sheet为被选中状态
            my.Select();  
			 oXL.Visible=true; //加上这一行  就相当于预览功能
            }
			else
			{	
			// 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch { }
            // 关闭文件，false表示不保存
            wb.Close(false);
            // 关闭Excel进程
            oXL.Quit();
            // 释放COM资源
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(oXL);
            oXL = null;
            wb = null;
            my = null;
			}

		}

        int find_indexofprint()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where 生产指令ID=" + _生产指令ID, mySystem.Parameter.connOle);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
            }
            return ids.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            if (!inputJudge())
            {
                MessageBox.Show("请填写完整再提交审核!");
                return;
            }
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
                newrow["对应ID"] = dtOuter.Rows[0]["ID"];
                dtCheck.Rows.Add(newrow);
            }
            bsCheck.DataSource = dtCheck;
            daCheck.Update((DataTable)bsCheck.DataSource);

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            //fill reviwer information
            dtOuter.Rows[0]["审核员"] = __待审核;
            //update log into tabl
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            
                readOuterData(_生产指令ID, _产品代码, _Date, _Time);
            
            removeOuterBinding();
            outerBind();

            setFormState();
            setEnableReadOnly();
            btn提交审核.Enabled = false;
        }
       /// <summary>
       ///  for different user, the form will open different controls
       /// </summary>
        private void setEnableReadOnly()
        {
            switch (_userState)
            {
                case Parameter.UserState.操作员: //0--操作员
                    //In this situation,operator could edit all the information and the send button is active
                    if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlTrue();
                    }
                    //Once the record send to the reviewer or the record has passed check, all the controls are forbidden
                    else if (Parameter.FormState.待审核 == _formState || Parameter.FormState.审核通过 == _formState)
                    {
                        setControlFalse();
                    }
                    else if (Parameter.FormState.无数据 == _formState)
                    {
                        setControlFalse();
                        cmb产品代码.Enabled = true;
                    }
                    break;
                case Parameter.UserState.审核员: //1--审核员
                    //the _formState is to be checked
                    if (Parameter.FormState.待审核 == _formState)
                    {
                        setControlTrue();
                        btn审核.Enabled = true;
                        //one more button should be avtive here!
                    }
                    //the _formState do not have to be checked
                    else if (Parameter.FormState.未保存 == _formState || Parameter.FormState.审核通过 == _formState || Parameter.FormState.审核未通过 == _formState)
                    {
                        setControlFalse();
                    }
                   
                    else if (Parameter.FormState.无数据 == _formState)
                    {
                        setControlFalse();
                        //cmb产品代码.Enabled = true;
                    }
                    break;
                case Parameter.UserState.管理员: //2--管理员
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
            可填写部分TextBox使能(true);
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
            btn新建.Enabled = false;
            cmb产品代码.Enabled = false;
            txb产品批号.Enabled = false;
            txb审核员.ReadOnly = true;
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
            可填写部分TextBox使能(false);
            btn查看日志.Enabled = true;
            btn打印.Enabled = true;
            cmb打印机选择.Enabled = true;
            bt查看人员信息.Enabled = true;
        }
        private void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", conOle);
                BindingSource bsUser = new BindingSource();
                OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
                daUser.Fill(dtUser);
            }
            else
            {
                SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + tablename1 + "';", mySystem.Parameter.conn);
                BindingSource bsUser = new BindingSource();
                SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
                daUser.Fill(dtUser);
            }
            
            if (dtUser.Rows.Count != 1)
            {
                MessageBox.Show("请确认表单权限信息");
                this.Close();
            }

            //the getPeople and setUserState combine here
            ls操作员 = new List<string>(Regex.Split(dtUser.Rows[0]["操作员"].ToString(), ",|，"));
            ls审核员 = new List<string>(Regex.Split(dtUser.Rows[0]["审核员"].ToString(), ",|，"));

        }

        private void setUserState()
        {
            _userState = Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (Parameter.UserState.NoBody == _userState)
            {
                _userState = Parameter.UserState.管理员;
                label角色.Text = "管理员";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = Parameter.UserState.操作员;
                else _userState = Parameter.UserState.审核员;

            }
            if (Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (Parameter.UserState.审核员 == _userState) label角色.Text = "审核员";
        }
        private void setFormState(bool newForm = false)
        {
            if (newForm)
            {

                _formState = Parameter.FormState.无数据;
                return;
            }
            string s = dtOuter.Rows[0]["审核员"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = Parameter.FormState.待审核;
            else
            {
                if (b) _formState = Parameter.FormState.审核通过;
                else _formState = Parameter.FormState.审核未通过;
            }

        }


        

        private void btn新建_Click(object sender, EventArgs e)
        {
            //_Date = DateTime.Now.Date;
            //_Time = Convert.ToDateTime(DateTime.Now.ToString());
            
            readOuterData(_生产指令ID, _产品代码, _Date, _Time);
           
            removeOuterBinding();
            outerBind();
            if (0 == dtOuter.Rows.Count)
            {
                DataRow newrow = dtOuter.NewRow();
                newrow = writeOuterDefault(newrow);
                dtOuter.Rows.Add(newrow);
                if (!mySystem.Parameter.isSqlOk)
                {
                    daOuter.Update((DataTable)bsOuter.DataSource);
                }
                else
                {
                    ((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = 0;
                    daOutersql.Update((DataTable)bsOuter.DataSource);
                }
                
                readOuterData(_生产指令ID, _产品代码, _Date, _Time);
                removeOuterBinding();
                outerBind();

                string sql = "SELECT * FROM 吹膜机组运行记录 WHERE 生产指令ID={0} AND 产品代码='{1}' order by ID";
                DataTable dt = new DataTable();
                if (!mySystem.Parameter.isSqlOk)
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, _生产指令ID, _产品代码), conOle);
                    da.Fill(dt);
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter(string.Format(sql, _生产指令ID, _产品代码), mySystem.Parameter.conn);
                    da.Fill(dt);
                }
                
                if (dt.Rows.Count == 1)
                {
                    array1[1][1].Text = dtSetting.Rows[0]["一区预热参数设定2"].ToString();
                    array1[1][2].Text = dtSetting.Rows[0]["一区预热参数设定2"].ToString();
                    array1[1][3].Text = dtSetting.Rows[0]["一区预热参数设定2"].ToString();

                    array1[2][1].Text = dtSetting.Rows[0]["二区预热参数设定2"].ToString();
                    array1[2][2].Text = dtSetting.Rows[0]["二区预热参数设定2"].ToString();
                    array1[2][3].Text = dtSetting.Rows[0]["二区预热参数设定2"].ToString();


                    array1[3][1].Text = dtSetting.Rows[0]["三区预热参数设定2"].ToString();
                    array1[3][2].Text = dtSetting.Rows[0]["三区预热参数设定2"].ToString();
                    array1[3][3].Text = dtSetting.Rows[0]["三区预热参数设定2"].ToString();

                    array1[4][1].Text = dtSetting.Rows[0]["四区预热参数设定2"].ToString();
                    array1[4][2].Text = dtSetting.Rows[0]["四区预热参数设定2"].ToString();
                    array1[4][3].Text = dtSetting.Rows[0]["四区预热参数设定2"].ToString();

                    array1[5][1].Text = dtSetting.Rows[0]["换网预热参数设定2"].ToString();
                    array1[5][2].Text = dtSetting.Rows[0]["换网预热参数设定2"].ToString();
                    array1[5][3].Text = dtSetting.Rows[0]["换网预热参数设定2"].ToString();

                    array1[6][1].Text = dtSetting.Rows[0]["流道预热参数设定2"].ToString();
                    array1[6][2].Text = dtSetting.Rows[0]["流道预热参数设定2"].ToString();
                    array1[6][3].Text = dtSetting.Rows[0]["流道预热参数设定2"].ToString();

                    array1[1][5].Text = dtSetting.Rows[0]["模颈预热参数设定2"].ToString();
                    array1[2][5].Text = dtSetting.Rows[0]["机头1预热参数设定2"].ToString();
                    array1[3][5].Text = dtSetting.Rows[0]["机头2预热参数设定2"].ToString();
                    array1[4][5].Text = dtSetting.Rows[0]["口模预热参数设定2"].ToString();

                    表格横线();

                    foreach (List<TextBox> tb1 in array1)
                    {
                        foreach (TextBox tb2 in tb1)
                        {
                            try
                            {
                                tb2.DataBindings[0].WriteValue();
                            }
                            catch (ArgumentOutOfRangeException ee)
                            {
                            }
                        }
                    }
                }
                else
                {
                    int idx = dt.Rows.Count - 2;
                    dtOuter.Rows[0].ItemArray = dt.Rows[idx].ItemArray.Clone() as object[];
                    dtOuter.Rows[0]["生产日期"] = _Date;
                    dtOuter.Rows[0]["记录时间"] = _Time;
                    dtOuter.Rows[0]["记录员"] = Parameter.userName;
                    dtOuter.Rows[0]["审核员"] = "";
                }

            }
            //setFormState()-->setEnableReadOnly() --> addOtherEvnetHandler()
            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();            
            btn打印.Enabled = true;
            cmb打印机选择.Enabled = true;
            
            
        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            try
            {
                mySystem.Other.LogForm logForm = new Other.LogForm();

                logForm.setLog(dtOuter.Rows[0]["日志"].ToString()).Show();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        private void cmb产品代码_TextChanged(object sender, EventArgs e)
        {
            //_产品代码 = cmb产品代码.SelectedItem.ToString();
            //txb产品批号.Text = productCode[cmb产品代码.SelectedItem.ToString()].ToString();
            //MessageBox.Show("请点击'新建'按钮!");
            //btn新建.Enabled = true;
        }

        bool inputJudge()
        {
            bool isok = true;
            int ii = array1.Count;
            int jj = array1[0].Count;
            for(int i=0;i<ii;++i)
            {
                for(int j=0;j<jj;++j)
                {
                    if (array1[i][j].Text == "")
                    {
                        isok = false;
                        array1[i][j].BackColor = Color.Red;
                    }
                    
                }
            }
            return isok;
        }

        void 填写界面上被disable的部分为横线()
        {
            for (int j = 6; j <= 8; ++j)
            {
                array1[8][j].Text = note;
                array1[9][j].Text = note;
                array1[12][j].Text = note;
            }
            array1[8][10].Text = note;
            array1[9][10].Text = note;
            array1[12][10].Text = note;

            for (int j = 5; j <= 8; ++j)
            {
                array1[10][j].Text = note;
                array1[11][j].Text = note;
            }
            array1[10][2].Text = note;
            array1[10][10].Text = note;
            array1[11][2].Text = note;
            array1[11][10].Text = note;

            for (int j = 2; j <= 10; ++j)
            {
                array1[13][j].Text = note;
            }
        }

        private void c(object sender, EventArgs e)
        {

        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 用户权限 where 步骤='吹膜机组运行记录'", mySystem.Parameter.connOle);
            dt = new DataTable("temp");
            da.Fill(dt);
            String str操作员 = dt.Rows[0]["操作员"].ToString();
            String str审核员 = dt.Rows[0]["审核员"].ToString();
            String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
            MessageBox.Show(str人员信息);
        }



        
    }
}
