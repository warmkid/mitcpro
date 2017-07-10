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
        private bool ischeckOk = false;
        private bool isSaveOk = false;

        List<Control> SettingConsList = new List<Control> { };//各种温度的控件
        List<String> SettingValdata = new List<String> { };//设置界面内存储的各种温度
        private int TemTolerance;//温度公差

        private DataTable dt设置, dt预热;
        private OleDbDataAdapter da预热;
        private BindingSource bs预热;
        private OleDbCommandBuilder cb预热;

        //已完成的预热表，那些数据也是读取设置界面吗？
        public ExtructionPreheatParameterRecordStep3(MainForm mainform): base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            //温度控件的初始化

            GetSettingInfo();
            DSBinding(Parameter.proInstruID); 
        }

        private void IDShow(Int32 ID)
        {
            OleDbCommand comm1 = new OleDbCommand();
            comm1.Connection = Parameter.connOle;
            comm1.CommandText = "select * from " + table + " where ID = " + ID.ToString();
            OleDbDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                //MessageBox.Show("有数据");
                DSBinding(Convert.ToInt32(reader1["生产指令ID"].ToString()));
            }
        }

        //******************************初始化******************************//
        
        //读取设置内容
        private void GetSettingInfo()
        {
            //连数据库
            dt设置 = new DataTable("设置");
            OleDbDataAdapter datemp = new OleDbDataAdapter("select * from " + tableSet, connOle);
            datemp.Fill(dt设置);
            datemp.Dispose();

            List<Control> SettingConsList = new List<Control>(new Control[] { tb换网预热参数设定1, tb流道预热参数设定1, tb模颈预热参数设定1, tb机头1预热参数设定1, tb机头2预热参数设定1, tb口模预热参数设定1, 
                tb一区预热参数设定1, tb二区预热参数设定1, tb三区预热参数设定1, tb四区预热参数设定1, 
                tb换网预热参数设定2, tb流道预热参数设定2, tb模颈预热参数设定2, tb机头1预热参数设定2, tb机头2预热参数设定2, tb口模预热参数设定2, 
                tb一区预热参数设定2, tb二区预热参数设定2, tb三区预热参数设定2, tb四区预热参数设定2, 
                tb加热保温时间1, tb加热保温时间2, tb加热保温时间3});

            List<String> queryCols = new List<String>(new String[] { "换网预热参数设定1", "流道预热参数设定1", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", "口模预热参数设定1", 
                "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1",
                "换网预热参数设定2", "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", 
                "一区预热参数设定2", "二区预热参数设定2", "三区预热参数设定2", "四区预热参数设定2", 
                "加热保温时间1", "加热保温时间2", "加热保温时间3"});
            List<List<Object>> queValsList = Utility.selectAccess(connOle, tableSet, queryCols, null, null, null, null, null, null, null);

            List<String> SettingValdata = new List<String> { };//设置界面内存储的各种温度
            if (queValsList.Count != 0)
            {
                for (int i = 0; i < queValsList[0].Count; i++)
                {
                    SettingValdata.Add(queValsList[0][i].ToString());
                    ((TextBox)SettingConsList[i]).ReadOnly = true;
                }
                Utility.fillControl(SettingConsList, SettingValdata);
            }
        }

        //控件属性（先绑定！！！）
        private void formatInit() 
        {
            this.tb备注.AutoSize = false;
            this.tb备注.Height = 32;

            //时间控件初始化
            this.dtp预热开始时间.ShowUpDown = true;
            this.dtp预热开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp预热开始时间.CustomFormat = "yyyy/MM/dd    HH:mm";
            this.dtp保温结束时间1.ShowUpDown = true;
            this.dtp保温结束时间1.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间1.CustomFormat = "yyyy/MM/dd    HH:mm";
            this.dtp保温开始时间.ShowUpDown = true;
            this.dtp保温开始时间.Format = DateTimePickerFormat.Custom;
            this.dtp保温开始时间.CustomFormat = "yyyy/MM/dd    HH:mm";
            this.dtp保温结束时间2.ShowUpDown = true;
            this.dtp保温结束时间2.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间2.CustomFormat = "yyyy/MM/dd    HH:mm";
            this.dtp保温结束时间3.ShowUpDown = true;
            this.dtp保温结束时间3.Format = DateTimePickerFormat.Custom;
            this.dtp保温结束时间3.CustomFormat = "yyyy/MM/dd    HH:mm";     
        }

        //******************************显示数据******************************//

        private void DSBinding(int InstructionID) 
        {
            //生产指令ID
            //Dispose
            if (da预热 != null)
            {
                bs预热.Dispose();
                dt预热.Dispose();
                da预热.Dispose();
                cb预热.Dispose();
            }

            // 新建
            bs预热 = new BindingSource();
            dt预热 = new DataTable(table);
            //连数据库
            da预热 = new OleDbDataAdapter("select * from " + table + " where 生产指令id = " + InstructionID.ToString(), connOle);
            cb预热 = new OleDbCommandBuilder(da预热);
            da预热.Fill(dt预热);
            bs预热.DataSource = dt预热;
            // 绑定（先解除绑定）
            dtp日期.DataBindings.Clear();
            dtp日期.DataBindings.Add("Text", bs预热.DataSource, "日期");
            tb模芯规格参数1.DataBindings.Clear();
            tb模芯规格参数1.DataBindings.Add("Text", bs预热.DataSource, "模芯规格参数1");
            tb模芯规格参数2.DataBindings.Clear();
            tb模芯规格参数2.DataBindings.Add("Text", bs预热.DataSource, "模芯规格参数2");
            tb记录人.DataBindings.Clear();
            tb记录人.DataBindings.Add("Text", bs预热.DataSource, "记录人");
            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bs预热.DataSource, "审核人");
            dtp预热开始时间.DataBindings.Clear();
            dtp预热开始时间.DataBindings.Add("Text", bs预热.DataSource, "预热开始时间");
            dtp保温结束时间1.DataBindings.Clear();
            dtp保温结束时间1.DataBindings.Add("Text", bs预热.DataSource, "保温结束时间1");
            dtp保温开始时间.DataBindings.Clear();
            dtp保温开始时间.DataBindings.Add("Text", bs预热.DataSource, "保温开始时间");
            dtp保温结束时间2.DataBindings.Clear();
            dtp保温结束时间2.DataBindings.Add("Text", bs预热.DataSource, "保温结束时间2");
            dtp保温结束时间3.DataBindings.Clear();
            dtp保温结束时间3.DataBindings.Add("Text", bs预热.DataSource, "保温结束时间3");
            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bs预热.DataSource, "备注");

            //待注释
            //MessageBox.Show("记录数目：" + dt预热.Rows.Count.ToString());

            if (dt预热.Rows.Count == 0)
            {
                DataRow dr1 = dt预热.NewRow();
                dr1["生产指令编号"] = mySystem.Parameter.proInstruction;
                dr1["生产指令id"] = mySystem.Parameter.proInstruID;
                dr1["记录人"] = mySystem.Parameter.userName;
                //防止违反并发性，提前赋值
                dr1["审核人"] = " ";
                dr1["审核意见"] = " ";
                dr1["审核是否通过"] = false;
                dt预热.Rows.InsertAt(dr1, dt预热.Rows.Count);

                SaveBtn.Enabled = true;
                CheckBtn.Enabled = false;
                printBtn.Enabled = false;
                cons_change();
            }
            else
            {
                if (Convert.ToBoolean(dt预热.Rows[0]["审核是否通过"]) == true)
                {
                    SaveBtn.Enabled = false;
                    CheckBtn.Enabled = false;
                    printBtn.Enabled = true;
                    cons_change_not();
                }
                else
                {
                    SaveBtn.Enabled = true;
                    CheckBtn.Enabled = true;
                    printBtn.Enabled = false;
                    cons_change();
                }
            }

            formatInit();  
        }

        private void ConsBind()
        { }

        //******************************按钮功能******************************//

        //保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (mySystem.Parameter.NametoID(tb记录人.Text) == 0)
            { MessageBox.Show("请重新填写记录人姓名！"); }
            else if (CheckType() == false)
            { /*模芯规格填入的不是数字*/ }
            else
            {
                // 保存非DataGridView中的数据必须先执行EndEdit;
                dt预热.Rows[0]["日期"] = DateTime.Now;

                dt预热.Rows[0]["保温结束时间1"] = DateTime.Now;
                dt预热.Rows[0]["保温开始时间"] = DateTime.Now;
                dt预热.Rows[0]["保温结束时间2"] = DateTime.Now;
                dt预热.Rows[0]["保温结束时间3"] = DateTime.Now;
                dt预热.Rows[0]["预热开始时间"] = DateTime.Now;

                bs预热.EndEdit();
                da预热.Update((DataTable)bs预热.DataSource);

                CheckBtn.Enabled = true;
            }
        }

        //审核功能
        public override void CheckResult()
        {
            base.CheckResult();
            dt预热.Rows[0]["日期"] = DateTime.Now;

            dt预热.Rows[0]["保温结束时间1"] = DateTime.Now;
            dt预热.Rows[0]["保温开始时间"] = DateTime.Now;
            dt预热.Rows[0]["保温结束时间2"] = DateTime.Now;
            dt预热.Rows[0]["保温结束时间3"] = DateTime.Now;
            dt预热.Rows[0]["预热开始时间"] = DateTime.Now;


            dt预热.Rows[0]["审核人"] = Parameter.IDtoName(check.userID);
            dt预热.Rows[0]["审核意见"] = check.opinion;
            dt预热.Rows[0]["审核是否通过"] = check.ischeckOk;

            //保存非DataGridView中的数据必须先执行EndEdit;
            bs预热.EndEdit();
            da预热.Update((DataTable)bs预热.DataSource);

            if (check.ischeckOk == true)
            {
                SaveBtn.Enabled = false;
                CheckBtn.Enabled = false;
                printBtn.Enabled = true;
                cons_change_not();
            }
            else
            {
                SaveBtn.Enabled = true;
                CheckBtn.Enabled = true;
                printBtn.Enabled = false;
                cons_change();
            }
        }

        //审核按钮
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.ShowDialog();
        }

        //******************************小功能******************************//
        
        //数据可修改
        private void cons_change()
        {
            tb模芯规格参数1.ReadOnly = false;
            tb模芯规格参数2.ReadOnly = false;
            tb记录人.ReadOnly = false;
            tb审核人.ReadOnly = false;
            tb备注.ReadOnly = false;
            dtp预热开始时间.Enabled = true;
            dtp保温结束时间1.Enabled = true;
            dtp保温开始时间.Enabled = true;
            dtp保温结束时间2.Enabled = true;
            dtp保温结束时间3.Enabled = true;
            dtp日期.Enabled = true;
        }

        //数据不可修改
        private void cons_change_not()
        {
            tb模芯规格参数1.ReadOnly = true;
            tb模芯规格参数2.ReadOnly = true;
            tb记录人.ReadOnly = true;
            tb审核人.ReadOnly = true;
            tb备注.ReadOnly = true;
            dtp预热开始时间.Enabled = false;
            dtp保温结束时间1.Enabled = false;
            dtp保温开始时间.Enabled = false;
            dtp保温结束时间2.Enabled = false;
            dtp保温结束时间3.Enabled = false;
            dtp日期.Enabled = false;
        }

        //检查控件内容是否合法
        private bool CheckType()
        {
            bool TypeCheck = true;

            List<TextBox> TextBoxList = new List<TextBox>(new TextBox[] { tb模芯规格参数1, tb模芯规格参数2 });
            List<String> StringList = new List<String>(new String[] { "模芯规格参数φ", "模芯规格参数Gap" });

            int numtemp = 0;
            for (int i = 0; i < TextBoxList.Count; i++)
            {
                if (Int32.TryParse(TextBoxList[i].Text.ToString(), out numtemp) == false)
                {
                    MessageBox.Show(StringList[i] + "应填数字，请重新填入！");
                    TypeCheck = false;
                    break;
                }
            }
            return TypeCheck;
        }






        /// <summary>
        /// ////////////
        /// </summary>
     

        

        //private void otherConsChanged(bool canChanged)
        //{

        //    List<TextBox> TextBoxList = new List<TextBox> { };//各种温度的控件
        //    TextBoxList = new List<TextBox> { tb换网预热参数设定1, tb流道预热参数设定1, tb模颈预热参数设定1, tb机头1预热参数设定1, tb机头2预热参数设定1, 
        //            tb口模预热参数设定1, tb一区预热参数设定1, tb二区预热参数设定1, tb三区预热参数设定1, tb四区预热参数设定1, 
        //            tb换网预热参数设定2, tb流道预热参数设定2, tb模颈预热参数设定2, tb机头1预热参数设定2, tb机头2预热参数设定2, tb口模预热参数设定2, 
        //            tb一区预热参数设定2, tb二区预热参数设定2, tb三区预热参数设定2, tb四区预热参数设定2 };

        //    if (canChanged == true)
        //    {
        //        tb模芯规格参数1.ReadOnly = false;
        //        tb模芯规格参数2.ReadOnly = false;
        //        tb记录人.ReadOnly = false;
        //        tb审核人.ReadOnly = false;
        //        tb备注.ReadOnly = false;           
        //        dtp预热开始时间.Enabled = true;
        //        dtp保温结束时间1.Enabled = true;
        //        dtp保温开始时间.Enabled = true;
        //        dtp保温结束时间2.Enabled = true;
        //        dtp保温结束时间3.Enabled = true;
        //        for (int i = 0; i < TextBoxList.Count; i++)
        //        {
        //            TextBoxList[i].ReadOnly = false;
        //        }
        //    }
        //    else
        //    {
        //        tb模芯规格参数1.ReadOnly = true;
        //        tb模芯规格参数2.ReadOnly = true;
        //        tb记录人.ReadOnly = true;
        //        tb审核人.ReadOnly = true;
        //        tb备注.ReadOnly = true;
        //        dtp预热开始时间.Enabled = false;
        //        dtp保温结束时间1.Enabled = false;
        //        dtp保温开始时间.Enabled = false;
        //        dtp保温结束时间2.Enabled = false;
        //        dtp保温结束时间3.Enabled = false;
        //        for (int i = 0; i < TextBoxList.Count; i++)
        //        {
        //            TextBoxList[i].ReadOnly = true;
        //        }
        //    }
        //}


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
