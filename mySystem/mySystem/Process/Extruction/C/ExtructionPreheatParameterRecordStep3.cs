 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem.Extruction.Process
{
    public partial class ExtructionPreheatParameterRecordStep3 : BaseForm
    {
        private String table = "吹膜机组预热参数记录表";
        private String tableSet = "设置吹膜机组预热参数记录表";

        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private bool isSqlOk;
        
        private CheckForm check = null;

        //List<Control> SettingConsList = new List<Control> { };//各种温度的控件
        //List<String> SettingValdata = new List<String> { };//设置界面内存储的各种温度

        private DataTable dt设置, dt记录;
        private OleDbDataAdapter da记录;
        private BindingSource bs记录;
        private OleDbCommandBuilder cb记录;
                
        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            //温度控件的初始化

            GetSettingInfo();
            if (dt设置.Rows.Count > 0)
            {
                DataShow(mySystem.Parameter.proInstruID);
            }
            else
            { MessageBox.Show("预热参数设置尚未完成，请先去设置！"); }
            
        }
        
        public ExtructionPreheatParameterRecordStep3(MainForm mainform, Int32 ID)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            //温度控件的初始化
            IDShow(ID);
            foreach (Control c in this.Controls) { c.Enabled = false; }
        }

        //******************************初始化******************************//
        //控件不可用 + DTP格式设置
        private void Init()
        {
            foreach (Control c in this.Controls) { c.Enabled = false; }

            this.tb备注.AutoSize = false;
            this.tb备注.Height = 32;

            //时间控件初始化
            this.dtp预热开始时间.ShowUpDown = true;
            this.dtp预热开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp预热开始时间.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间1.ShowUpDown = true;
            this.dtp保温结束时间1.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温开始时间.ShowUpDown = true;
            this.dtp保温开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp保温开始时间.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间2.ShowUpDown = true;
            this.dtp保温结束时间2.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtp保温结束时间3.ShowUpDown = true;
            this.dtp保温结束时间3.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间3.CustomFormat = "yyyy/MM/dd HH:mm";    
        }

        //可编辑，控件初始化
        private void EnableInit(bool able)
        {
            dtp日期.Enabled = able;
            tb模芯规格参数1.Enabled = able;
            tb模芯规格参数2.Enabled = able;
            tb记录人.Enabled = able;
            groupBox1.Enabled = able;
            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox)
                {
                    c.Enabled = false;
                }
            }
            dtp保温结束时间1.Enabled = able;
            dtp保温结束时间2.Enabled = able;
            dtp保温结束时间3.Enabled = able;
            dtp保温开始时间.Enabled = able;
            dtp预热开始时间.Enabled = able;
            tb备注.Enabled = able;
            SaveBtn.Enabled = able;
        }
        
        //读取设置内容
        private void GetSettingInfo()
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();

            //List<Control> SettingConsList = new List<Control>(new Control[] { tb换网预热参数设定1, tb流道预热参数设定1, tb模颈预热参数设定1, tb机头1预热参数设定1, tb机头2预热参数设定1, tb口模预热参数设定1, 
            //    tb一区预热参数设定1, tb二区预热参数设定1, tb三区预热参数设定1, tb四区预热参数设定1, 
            //    tb换网预热参数设定2, tb流道预热参数设定2, tb模颈预热参数设定2, tb机头1预热参数设定2, tb机头2预热参数设定2, tb口模预热参数设定2, 
            //    tb一区预热参数设定2, tb二区预热参数设定2, tb三区预热参数设定2, tb四区预热参数设定2, 
            //    tb加热保温时间1, tb加热保温时间2, tb加热保温时间3});

            //List<String> queryCols = new List<String>(new String[] { "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", "口模预热参数设定1", 
            //    "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1",
            //    "换网预热参数设定2", "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", 
            //    "一区预热参数设定2", "二区预热参数设定2", "三区预热参数设定2", "四区预热参数设定2", 
            //    "加热保温时间1", "加热保温时间2", "加热保温时间3"});
            //List<List<Object>> queValsList = Utility.selectAccess(connOle, tableSet, queryCols, null, null, null, null, null, null, null);

            //List<String> SettingValdata = new List<String> { };//设置界面内存储的各种温度
            //if (queValsList.Count != 0)
            //{
            //    for (int i = 0; i < queValsList[0].Count; i++)
            //    {
            //        SettingValdata.Add(queValsList[0][i].ToString());
            //        ((TextBox)SettingConsList[i]).ReadOnly = true;
            //    }
            //    Utility.fillControl(SettingConsList, SettingValdata);
            //}
        }
        
        //******************************显示数据******************************//
        
        //显示根据信息查找
        private void DataShow(int InstruID) 
        {
            Init();

            //******************************外表 根据条件绑定******************************//  
            readOuterData(InstruID);
            outerBind();
            //MessageBox.Show("记录数目：" + dt记录.Rows.Count.ToString());

            //*******************************表格内部******************************// 
            Boolean isnew = true;
            if (dt记录.Rows.Count <= 0)
            {
                isnew = true;
                //********* 外表新建、保存、重新绑定 *********//                
                //初始化外表这一行
                DataRow dr1 = dt记录.NewRow();
                dr1 = writeOuterDefault(dr1);
                dt记录.Rows.InsertAt(dr1, dt记录.Rows.Count);
                //立马保存这一行
                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);
                //外表重新绑定
                readOuterData(InstruID);
                outerBind();
            }
            else
            {
                isnew = false;
            }


            //********* 控件可用性 *********//   

            if (Convert.ToBoolean(dt记录.Rows[0]["审核是否通过"].ToString()) == true)
            {
                //审核通过表
                printBtn.Enabled = true;
            }
            else
            {
                if (isnew == true)
                {
                    //新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                }
                else
                {
                    //非新建表
                    EnableInit(true);
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    tb审核人.Enabled = true;
                }
            }

        }

        //根据主键显示
        private void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                readOuterData(Convert.ToInt32(reader1["生产指令ID"].ToString()));
                outerBind();
            }
        }

        //****************************** 嵌套 ******************************//

        //外表读数据，填datatable
        private void readOuterData(Int32 InstruID)
        {
            bs记录 = new BindingSource();
            dt记录 = new DataTable(table);
            da记录 = new OleDbDataAdapter("select * from " + table + " where 生产指令ID = " + InstruID.ToString(), connOle);
            cb记录 = new OleDbCommandBuilder(da记录);
            da记录.Fill(dt记录);
        }
        
        //外表控件绑定
        private void outerBind()
        {
            bs记录.DataSource = dt记录;
            // 绑定（先解除绑定）
            dtp日期.DataBindings.Clear();
            dtp日期.DataBindings.Add("Text", bs记录.DataSource, "日期");
            tb模芯规格参数1.DataBindings.Clear();
            tb模芯规格参数1.DataBindings.Add("Text", bs记录.DataSource, "模芯规格参数1");
            tb模芯规格参数2.DataBindings.Clear();
            tb模芯规格参数2.DataBindings.Add("Text", bs记录.DataSource, "模芯规格参数2");
            tb记录人.DataBindings.Clear();
            tb记录人.DataBindings.Add("Text", bs记录.DataSource, "记录人");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs记录.DataSource, "审核人");
            dtp预热开始时间.DataBindings.Clear();
            dtp预热开始时间.DataBindings.Add("Text", bs记录.DataSource, "预热开始时间");
            dtp保温结束时间1.DataBindings.Clear();
            dtp保温结束时间1.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间1");
            dtp保温开始时间.DataBindings.Clear();
            dtp保温开始时间.DataBindings.Add("Text", bs记录.DataSource, "保温开始时间");
            dtp保温结束时间2.DataBindings.Clear();
            dtp保温结束时间2.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间2");
            dtp保温结束时间3.DataBindings.Clear();
            dtp保温结束时间3.DataBindings.Add("Text", bs记录.DataSource, "保温结束时间3");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs记录.DataSource, "备注");
            //不可用 绑定
            tb换网预热参数设定1.DataBindings.Clear();
            tb换网预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "换网预热参数设定1");
            tb流道预热参数设定1.DataBindings.Clear();
            tb流道预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "流道预热参数设定1");
            tb模颈预热参数设定1.DataBindings.Clear();
            tb模颈预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "模颈预热参数设定1");
            tb机头1预热参数设定1.DataBindings.Clear();
            tb机头1预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "机头1预热参数设定1");
            tb机头2预热参数设定1.DataBindings.Clear();
            tb机头2预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "机头2预热参数设定1");
            tb口模预热参数设定1.DataBindings.Clear();
            tb口模预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "口模预热参数设定1");
            tb一区预热参数设定1.DataBindings.Clear();
            tb一区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "一区预热参数设定1");
            tb二区预热参数设定1.DataBindings.Clear();
            tb二区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "二区预热参数设定1");
            tb三区预热参数设定1.DataBindings.Clear();
            tb三区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "三区预热参数设定1");
            tb四区预热参数设定1.DataBindings.Clear();
            tb四区预热参数设定1.DataBindings.Add("Text", bs记录.DataSource, "四区预热参数设定1");
            tb换网预热参数设定2.DataBindings.Clear();
            tb换网预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "换网预热参数设定2");
            tb流道预热参数设定2.DataBindings.Clear();
            tb流道预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "流道预热参数设定2");
            tb模颈预热参数设定2.DataBindings.Clear();
            tb模颈预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "模颈预热参数设定2");
            tb机头1预热参数设定2.DataBindings.Clear();
            tb机头1预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "机头1预热参数设定2");
            tb机头2预热参数设定2.DataBindings.Clear();
            tb机头2预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "机头2预热参数设定2");
            tb口模预热参数设定2.DataBindings.Clear();
            tb口模预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "口模预热参数设定2");
            tb一区预热参数设定2.DataBindings.Clear();
            tb一区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "一区预热参数设定2");
            tb二区预热参数设定2.DataBindings.Clear();
            tb二区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "二区预热参数设定2");
            tb三区预热参数设定2.DataBindings.Clear();
            tb三区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "三区预热参数设定2");
            tb四区预热参数设定2.DataBindings.Clear();
            tb四区预热参数设定2.DataBindings.Add("Text", bs记录.DataSource, "四区预热参数设定2");
            tb加热保温时间1.DataBindings.Clear();
            tb加热保温时间1.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间1");
            tb加热保温时间2.DataBindings.Clear();
            tb加热保温时间2.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间2");
            tb加热保温时间3.DataBindings.Clear();
            tb加热保温时间3.DataBindings.Add("Text", bs记录.DataSource, "加热保温时间3");
        }

        //添加外表默认信息
        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;
            dr["生产指令id"] = mySystem.Parameter.proInstruID;
            dr["日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["记录人"] = mySystem.Parameter.userName;
            dr["审核人"] = "";
            dr["审核日期"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            dr["审核是否通过"] = false;

            //dr["模芯规格参数1"] = 0;
            //dr["模芯规格参数2"] = 0;

            //dr["预热开始时间"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            //dr["保温结束时间1"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            //dr["保温开始时间"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            //dr["保温结束时间2"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            //dr["保温结束时间3"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

            dr["换网预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["换网预热参数设定1"].ToString());
            dr["流道预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["流道预热参数设定1"].ToString());
            dr["模颈预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["模颈预热参数设定1"].ToString());
            dr["机头1预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["机头1预热参数设定1"].ToString());
            dr["机头2预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["机头2预热参数设定1"].ToString());
            dr["口模预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["口模预热参数设定1"].ToString());

            dr["一区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["一区预热参数设定1"].ToString());
            dr["二区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["二区预热参数设定1"].ToString());
            dr["三区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["三区预热参数设定1"].ToString());
            dr["四区预热参数设定1"] = Convert.ToInt32(dt设置.Rows[0]["四区预热参数设定1"].ToString());

            dr["换网预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["换网预热参数设定2"].ToString());
            dr["流道预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["流道预热参数设定2"].ToString());
            dr["模颈预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["模颈预热参数设定2"].ToString());
            dr["机头1预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["机头1预热参数设定2"].ToString());
            dr["机头2预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["机头2预热参数设定2"].ToString());
            dr["口模预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["口模预热参数设定2"].ToString());

            dr["一区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["一区预热参数设定2"].ToString());
            dr["二区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["二区预热参数设定2"].ToString());
            dr["三区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["三区预热参数设定2"].ToString());
            dr["四区预热参数设定2"] = Convert.ToInt32(dt设置.Rows[0]["四区预热参数设定2"].ToString());

            dr["加热保温时间1"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间1"].ToString());
            dr["加热保温时间2"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间2"].ToString());
            dr["加热保温时间3"] = Convert.ToInt32(dt设置.Rows[0]["加热保温时间3"].ToString());

            return dr;
        }

        //******************************按钮功能******************************//

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(tb记录人.Text.ToString()) == 0)
            {
                /*操作人不合格*/
                MessageBox.Show("请重新输入『记录人』信息", "ERROR");
            }
            else if (TextBox_check() == false)
            { /*模芯规格填入的不是数字*/ }
            else
            {
                if (isSqlOk)
                { }
                else
                {
                    //外表保存
                    bs记录.EndEdit();
                    da记录.Update((DataTable)bs记录.DataSource);
                    readOuterData(mySystem.Parameter.proInstruID);
                    outerBind();
                }
                CheckBtn.Enabled = true;
                tb审核人.Enabled = true;
            }
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            if (check.ischeckOk == true)
            {
                dt记录.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
                dt记录.Rows[0]["审核意见"] = check.opinion;
                dt记录.Rows[0]["审核是否通过"] = check.ischeckOk;

                bs记录.EndEdit();
                da记录.Update((DataTable)bs记录.DataSource);

                Init();
                printBtn.Enabled = true;
            }
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
        }

        //打印按钮
        private void printBtn_Click(object sender, EventArgs e)
        {

        }

        //******************************小功能******************************//
        
        //检查控件内容是否合法
        private bool TextBox_check()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb模芯规格参数1, tb模芯规格参数2 });
            List<String> StringList = new List<String>(new String[] { "模芯规格参数φ", "模芯规格参数Gap" });

            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show("『" + StringList[i] + "』应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }
        

        /*   //TabelPaint
        //private void TabelPaint()
        //{
        //    Graphics g = this.CreateGraphics();
        //    this.Show();
        //    //出来一个画笔,这只笔画出来的颜色是红的  
        //    Pen p = new Pen(Brushes.Red);

        //    //创建两个点  
        //    Point p1 = new Point(0, 0);
        //    Point p2 = new Point(1000, 1000);

        //    //将两个点连起来  
        //    g.DrawLine(p, p1, p2);
        //}
        */
                
    }
}
