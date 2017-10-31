using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace mySystem.Process.CleanCut
{
    public partial class DailyRecord : mySystem.BaseForm
    {
        OleDbConnection conOle;
        string tablename1 = "清洁分切日报表";
        DataTable dtOuter;
        OleDbDataAdapter daOuter;
        BindingSource bsOuter;
        OleDbCommandBuilder cbOuter;

        string tablename2 = "清洁分切日报表详细信息";
        DataTable dtInner;
        OleDbDataAdapter daInner;
        BindingSource bsInner;
        OleDbCommandBuilder cbInner;

        List<string> productCode;
        List<string> productCodeLst;
        List<String> matCodes = new List<string>(new String[] {"全部", "T", "PEF", "UP1", "XP1" });
        List<string> wasteReason = new List<string>();
        List<string> flight = new List<string>(new string[] { "白班", "夜班" });
        List<string> usrList = new List<string>();
        List<string> ls操作员;// = new List<string>(new string[] { dtUser.Rows[0]["操作员"].ToString() });
        List<string> ls审核员;//= new List<string>(new string[] { dtUser.Rows[0]["审核员"].ToString() });
        string __待审核 = "__待审核";
        string __生产指令;
        int __生产指令ID;
        private CheckForm check = null;
        int outerId;
        int searchId;

        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>

        Parameter.UserState _userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        Parameter.FormState _formState;
        //public DailyRecord()
        //    : base()
        //{
           
        //    //getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()

        //}

        void cmb物料种类_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbDataAdapter da;
            DataTable dt;
            // 看生产指令状态
            da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where ID="+__生产指令ID,mySystem.Parameter.connOle);
            dt = new DataTable();
            da.Fill(dt);
            int statue = Convert.ToInt32(dt.Rows[0]["状态"]);
            if (4 == statue)
            {
                readOuterData(__生产指令ID, cmb物料种类.SelectedItem.ToString());
                outerBind();
                if (0 == dtOuter.Rows.Count)
                {
                    MessageBox.Show("物料代码： " + cmb物料种类.SelectedItem.ToString() + " 下没有生产记录");
                    return;
                }
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }
            else
            {
                

                // 先看外表有没有，没有就新建
                readOuterData(__生产指令ID, cmb物料种类.SelectedItem.ToString());
                outerBind();
                if (dtOuter.Rows.Count == 0)
                {
                    DataRow dr = dtOuter.NewRow();
                    dr = writeOuterDefault(dr);
                    dtOuter.Rows.Add(dr);
                    daOuter.Update((DataTable)bsOuter.DataSource);
                    readOuterData(__生产指令ID, cmb物料种类.SelectedItem.ToString());
                    outerBind();
                }
                

                // 读取内表，删除所有记录
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
                foreach (DataRow dr in dtInner.Rows)
                {
                    dr.Delete();
                }
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();

                // 读取生产记录

                da = new OleDbDataAdapter("select * from 清洁分切生产记录 where 生产指令ID=" + __生产指令ID, mySystem.Parameter.connOle);
                dt = new DataTable();
                da.Fill(dt);
                OleDbDataAdapter dain;
                int xuhao = 1;
                foreach (DataRow r in dt.Rows)
                {
                    int id = Convert.ToInt32(r["ID"]);
                    string sql = "select * from 清洁分切生产记录详细信息 where T清洁分切生产记录ID={0} and 物料代码 like '%{1}%'";
                    dain = new OleDbDataAdapter(string.Format(sql, id, cmb物料种类.SelectedItem.ToString()), mySystem.Parameter.connOle);
                    DataTable tmp = new DataTable();
                    dain.Fill(tmp);
                    foreach (DataRow dr in tmp.Rows)
                    {
                        DataRow ndr = dtInner.NewRow();
                        ndr["T清洁分切日报表ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
                        ndr["序号"] = xuhao++;
                        ndr["生产指令"] = __生产指令;
                        ndr["生产日期"] = r["生产日期"];
                        ndr["使用物料代码"] = dr["物料代码"];
                        // TODO 能自动填吗？
                        string temp = ndr["使用物料代码"].ToString().TrimStart('X').Split('X')[0];
                        string[] tmps = temp.Split('-');
                        temp = tmps[tmps.Length - 1];
                        ndr["规格a1"] = Convert.ToInt32(temp);
                        ndr["用量b1"] = dr["长度A"];
                        ndr["使用量"] = Convert.ToDouble(ndr["规格a1"]) * Convert.ToDouble(ndr["用量b1"])/1000;
                        ndr["清洁分切后代码"] = dr["清洁分切后代码"];
                        temp = ndr["清洁分切后代码"].ToString().TrimStart('X').Split('X')[0];
                        tmps = temp.Split('-');
                        temp = tmps[tmps.Length - 1];
                        ndr["规格a2"] = Convert.ToInt32(temp.TrimEnd('C')); ;
                        ndr["数量b2"] = dr["长度B"];
                        ndr["数量"] = Convert.ToDouble(ndr["规格a2"]) * Convert.ToDouble(ndr["数量b2"]) / 1000;
                        ndr["工时"] = Convert.ToDouble(dr["工时"]);
                        ndr["操作人"] = mySystem.Parameter.userName;
                        ndr["审核人"] = "";
                        dtInner.Rows.Add(ndr);
                    }

                }
                daInner.Update((DataTable)bsInner.DataSource);
                readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                innerBind();
            }

            computerOuterData();
        }

        public DailyRecord(mySystem.MainForm mainform)
            : base(mainform)
        {
            
            InitializeComponent();
            fill_printer();
            conOle = mySystem.Parameter.connOle;
            getPeople();
            setUserState();
            __生产指令 = mySystem.Parameter.cleancutInstruction;
            __生产指令ID = mySystem.Parameter.cleancutInstruID;
            foreach (string m in matCodes)
            {
                cmb物料种类.Items.Add(m);
            }
            // cmb物料种类.SelectedIndexChanged += new EventHandler(cmb物料种类_SelectedIndexChanged);
            addOtherEventHandler();
            //getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()
            //setEnableReadOnly();
            datetimepicker结束时间.Value = DateTime.Now;
            //dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker1_ValueChanged);
            cmb物料种类.SelectedItem = "全部";

            if (_userState == Parameter.UserState.管理员 || _userState == Parameter.UserState.审核员)
            {
                btn打印.Enabled = true;
            }
            else
            {
                btn打印.Enabled = false;
            }

            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE 0=1", conOle);
            //cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable(tablename2);
            bsInner = new BindingSource();
            daInner.Fill(dtInner);

        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //public DailyRecord(mySystem.MainForm mainform, int Id)
        //    : base(mainform)
        //{
        //    string matcode;
        //    InitializeComponent();
        //    fill_printer();
        //    conOle = mySystem.Parameter.connOle;
        //    getPeople();
        //    setUserState();
        //    OleDbDataAdapter da = new OleDbDataAdapter("select * from 清洁分切日报表 where ID="+Id,mySystem.Parameter.connOle);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    matcode = dt.Rows[0]["物料种类"].ToString();
        //    __生产指令ID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
        //    da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where ID="+__生产指令ID, mySystem.Parameter.connOle);
        //    dt.Clear();
        //    da.Fill(dt);
        //    __生产指令 = dt.Rows[0]["生产指令编号"].ToString();
        //    foreach (string m in matCodes)
        //    {
        //        cmb物料种类.Items.Add(m);
        //    }
        //    cmb物料种类.SelectedIndexChanged += new EventHandler(cmb物料种类_SelectedIndexChanged);
        //    cmb物料种类.SelectedItem = matcode;
        //    addOtherEventHandler();
        //    setEnableReadOnly();
        //}


        void getOtherData()
        {
        }

        private void readOuterData(int pid, string matcode)
        {
            dtOuter = new DataTable(tablename1);
            string sql = "select * from 清洁分切日报表 where 生产指令ID={0} and 物料种类='{1}'";
            daOuter = new OleDbDataAdapter(string.Format(sql, pid, matcode), conOle);
            bsOuter = new BindingSource();
            cbOuter = new OleDbCommandBuilder(daOuter);
            daOuter.Fill(dtOuter);
        }

        private void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "全部" + "';", conOle);
            BindingSource bsUser = new BindingSource();
            OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
            daUser.Fill(dtUser);
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



        //for different user, the form will open different controls
        private void setEnableReadOnly()
        {
            
            switch (_userState)
            {
                case Parameter.UserState.操作员: //0--操作员
                   
                    setControlFalse();
                    cmb物料种类.Enabled = true;
                    bt保存.Enabled = true;
                    break;
                case Parameter.UserState.审核员: //1--审核员
                    
                    setControlFalse();
                    cmb物料种类.Enabled = true;
                    cb打印机.Enabled = true;
                    btn打印.Enabled = true;
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
            // 保证这两个按钮一直是false
            //txb审核员.ReadOnly = true;
            //btn审核.Enabled = false;
            //btn提交审核.Enabled = false;
        }

        //this guarantees the controls are uneditable

        private void setControlFalse()
        //this guarantees the controls are uneditable
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
            btn查看日志.Enabled = true;
            datetimepicker结束时间.Enabled = true;
            //btn打印.Enabled = true;
        }
        // “审核”和“提交审核”按钮特殊，在以上两个函数中要设为false。
        // 当登陆人是审核员时，在外面设置它为true
        // 以上两个函数的写法见示例


        // 如果有需要单行审核的表，在DataGridView下加一个“提交数据审核”按钮和“数据审核”，点击该按钮后，DataGridView中无“审核员”的行都填入：__待审核，同时设为ReadOnly
        // 下面这个函数完成功能：遍历DataGridView的行：只要审核员不为空，则该行ReadOnly
        // 该函数需要在DataGridView的DataBindingComplete事件中和“提交数据审核”点击事件中调用
        //void setDataGridViewColumnReadOnly();
        // 注意：删除按钮点击是要判断：如果该行有审核员信息，则无法删除
        /// 控件状态类 ================================================

        /// 需要单行审核的审核事件 ================================================
        // 下面函数当碰到审核员时，将审核员不为空也不为“__待审核”的设为只读（也就是有了审核结果的）
        //void setDataGridViewColumnReadOnly();
        // “数据审核按钮”点击事件遍历整个DataGridView，找到“审核员”为“__待审核”的行，修改“审核员”为自己
        // 然后调用setDataGridViewColumnReadOnly();

        public override void CheckResult()
        {
            //if (Parameter.userName == dtOuter.Rows[0]["记录员"].ToString())
            //{
            //    MessageBox.Show("记录员,审核员重复");
            //    return;
            //}
            if (check.ischeckOk)
            {
                //to update the Waste record
                base.CheckResult();

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

                //readOuterData(searchId);

                removeOuterBind();
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

                //readOuterData(searchId);

                removeOuterBind();
                outerBind();
                setFormState();
                setEnableReadOnly();
            }
        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            check = new CheckForm(this);
            check.Show();
        }
        //// 给外表的一行写入默认值，包括操作人，时间，班次等
        //DataRow writeOuterDefault(DataRow);
        //// 给内表的一行写入默认值，包括操作人，时间，Y/N等
        //DataRow writeInnerDefault(DataRow);
        //// 根据条件从数据库中读取一行外表的数据
        //void readOuterData(能唯一确定一行外表数据的参数，一般是生产指令ID或生产指令编号)；
        //// 根据条件从数据库中读取多行内表数据
        //void readInnerData(int 外表行ID);
        //// 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        //void removeOuterBinding();
        //// 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        //void removeInner(Binding);
        //// 外表和控件的绑定
        //void outerBind();
        //// 内表和控件的绑定
        //void innerBind();
        //// 设置DataGridView中各列的格
        //void setDataGridViewColumns();
        //// 刷新DataGridView中的列：序号
        //void setDataGridViewRowNums();
        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("SELECT * FROM " + tablename1 + " WHERE 生产指令='" + name + "';", conOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable(tablename1);
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

        private DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = __生产指令ID;
            dr["物料种类"] = cmb物料种类.SelectedItem.ToString();
            dr["清洁前合计A1"] = 0;
            dr["清洁前合计A2"] = 0;
            dr["清洁后合计B1"] = 0;
            dr["清洁后合计B2"] = 0;
            dr["清洁后合计C"] = 0;
            dr["月汇总"] = 0;
            dr["成品率"] = 0;
            dr["工时效率"] = 0;
            dr["工时"] = 0;
            dr["操作人"] = mySystem.Parameter.userName;
            dr["操作日期"] = DateTime.Now;
            dr["审核日期"] = DateTime.Now;
            string log = DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 新建记录\n";
            log += "生产指令编码：" + __生产指令 + "\n";
            dr["日志"] = log;
            return dr;
        }
        

        private void getProductCode()
        {
            DataTable DtproductCode = new DataTable("设置清洁分切产品");
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM 设置清洁分切产品", conOle);
            da.Fill(DtproductCode);
            productCode = new List<string>();
            foreach (DataRow dr in DtproductCode.Rows)
            {
                productCode.Add(dr["产品名称"].ToString());
            }
        }
        

       
        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("txb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    (c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    (c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    (c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        private void removeOuterBind()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("txb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    //(c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("lbl"))
                {
                    (c as Label).DataBindings.Clear();
                    //(c as Label).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("cmb"))
                {
                    (c as ComboBox).DataBindings.Clear();
                    //(c as ComboBox).DataBindings.Add("SelectedItem", bsOuter.DataSource, c.Name.Substring(3));
                }
                else if (c.Name.StartsWith("dtp"))
                {
                    (c as DateTimePicker).DataBindings.Clear();
                    //(c as DateTimePicker).DataBindings.Add("Value", bsOuter.DataSource, c.Name.Substring(3));
                }
            }
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("SELECT * FROM " + tablename2 + " WHERE T清洁分切日报表ID=" + id, conOle);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable(tablename2);
            bsInner = new BindingSource();
            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
        }

        private void removeInnerBind()
        {
            dataGridView1.DataBindings.Clear();
        }
       

        //check the operator, make sure the operator exists in userlist
        private void btn保存_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dtInner.Rows.Count; i++)
            {
                if (usrList.IndexOf(dtInner.Rows[i]["记录员"].ToString().Trim()) < 0)
                {
                    MessageBox.Show("用户不存在");
                    return;
                }
            }

            // 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            //readOuterData(lbl生产指令.Text);
            removeOuterBind();
            outerBind();
            if (Parameter.UserState.操作员 == _userState)
            {
                btn提交审核.Enabled = true;
            }
        }

       

        /// <summary>
        /// for the checked record set rows readonly
        /// </summary>
        private void setDataGridViewColumnReadOnly()
        {
            //for (int i = 0; i < dtInner.Rows.Count; i++)
            //{
            //    if ((Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() != "") && (Convert.ToString(dtInner.Rows[i]["审核员"]).ToString().Trim() != __待审核))
            //    {
            //        dataGridView1.Rows[i].ReadOnly = true;
            //    }
            //    continue;
            //}
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToString(dataGridView1.Rows[i].Cells["审核员"].Value).ToString().Trim() != "") && (Convert.ToString(dataGridView1.Rows[i].Cells["审核员"].Value).ToString().Trim() != __待审核))
                {
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                continue;
            }
            dataGridView1.Columns[10].ReadOnly = true;
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


       

        void outerDataSync(String name, String val)
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name == name)
                {
                    c.Text = val;
                    c.DataBindings[0].WriteValue();
                }
            }
        }

       
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            if (name == "ID")
            {
                return;
            }
            MessageBox.Show(name + "填写错误");
        }

        

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            //setDataGridViewColumnReadOnly();

        }



        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        //添加打印机
        private void fill_printer()
        {

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cb打印机.Items.Add(sPrint);
            }
            cb打印机.SelectedItem = print.PrinterSettings.PrinterName;
        }

        //void fill_printer()
        //{

        //    System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
        //    foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
        //    {
        //        combobox打印机选择.Items.Add(sPrint);
        //    }
        //}

        void computerOuterData()
        {
            double a1 = 0, a2 = 0, b1 = 0, b2 = 0, c = 0;
            foreach (DataRow dr in dtInner.Rows)
            {
                a1 += Convert.ToDouble(dr["用量b1"]);
                a2 += Convert.ToDouble(dr["使用量"]);
                b1 += Convert.ToDouble(dr["数量b2"]);
                b2 += Convert.ToDouble(dr["数量"]);
                c += Convert.ToDouble(dr["工时"]); ;
            }
            //dtOuter.Rows[0]["清洁前合计A1"] = a1;
            //dtOuter.Rows[0]["清洁前合计A2"] = a2;
            //dtOuter.Rows[0]["清洁后合计B1"] = b1;
            //dtOuter.Rows[0]["清洁后合计B2"] = b2;
            //dtOuter.Rows[0]["清洁后合计C"] = c;
            txb清洁前合计A1.Text = a1.ToString();
            txb清洁前合计A2.Text = a2.ToString();
            txb清洁后合计B1.Text = b1.ToString();
            txb清洁后合计B2.Text = b2.ToString();
            txb清洁后合计C.Text = c.ToString();
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(__生产指令ID, cmb物料种类.SelectedItem.ToString());
            outerBind();

            //内表保存
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
        }

        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataBindingComplete+=new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            
        }

        private void btn打印_Click(object sender, EventArgs e)
        {
            if (cb打印机.SelectedItem.ToString() == "")
            {
                MessageBox.Show("请先选择一台打印机");
                return;
            }
            SetDefaultPrinter(cb打印机.Text);
            print(false);
            GC.Collect();
        }

        //打印功能
        public void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\cleancut\6 SOP-MFG-302-R06A 清洁分切日报表.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[2];
            // 修改Sheet中某行某列的值
            my = printValue(my, wb);

            if (isShow)
            {
                //true->预览
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
            }
            else
            {
                bool isPrint = true;
                //false->打印
                try
                {
                    // 设置该进程是否可见
                    //oXL.Visible = false; // oXL.Visible=false 就会直接打印该Sheet
                    // 直接用默认打印机打印该Sheet
                    my.PrintOut();
                }
                catch
                { isPrint = false; }
                finally
                {
                    if (isPrint)
                    {
                        //写日志
                        //string log = "=====================================\n";
                        //log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        //dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                        //bsOuter.EndEdit();
                        //daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    // 关闭文件，false表示不保存
                    wb.Close(false);
                    // 关闭Excel进程
                    oXL.Quit();
                    // 释放COM资源
                    Marshal.ReleaseComObject(wb);
                    Marshal.ReleaseComObject(oXL);
                    wb = null;
                    oXL = null;
                }
            }
        }

        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            if (cmb物料种类.Text == "LDPE")
                mysheet.Cells[3, 1].Value = "物料种类：  LDPE ☑  TY □   TK8 □    UP1 □";
            else if (cmb物料种类.Text == "TY")
                mysheet.Cells[3, 1].Value = "物料种类：  LDPE □  TY ☑   TK8 □    UP1 □";
            else if (cmb物料种类.Text == "TK8")
                mysheet.Cells[3, 1].Value = "物料种类：  LDPE □  TY □   TK8 ☑    UP1 □";
            else if (cmb物料种类.Text == "UP1")
                mysheet.Cells[3, 1].Value = "物料种类：  LDPE □  TY □   TK8 □    UP1 ☑";
            else
                mysheet.Cells[3, 1].Value = "物料种类：  LDPE □  TY □   TK8 □    UP1 □" + cmb物料种类.Text + " ☑ ";
            
            //内表信息
            int i内表行数 = dtInner.Rows.Count;
            int i超出行数 = 0;
            //行数不超过模板行数的部分
            for (int i = 0; i < (i内表行数 > 8 ? 8 : i内表行数); i++)
            {
                mysheet.Cells[5 + i, 1].Value = i + 1;
                mysheet.Cells[5 + i, 2].Value = dtInner.Rows[i]["生产指令"].ToString();
                mysheet.Cells[5 + i, 3].Value = dtInner.Rows[i]["生产日期"].ToString();
                mysheet.Cells[5 + i, 4].Value = dtInner.Rows[i]["使用物料代码"].ToString();
                mysheet.Cells[5 + i, 5].Value = dtInner.Rows[i]["规格a1"].ToString();
                mysheet.Cells[5 + i, 6].Value = dtInner.Rows[i]["用量b1"].ToString();
                mysheet.Cells[5 + i, 7].Value = dtInner.Rows[i]["使用量"].ToString();
                mysheet.Cells[5 + i, 8].Value = dtInner.Rows[i]["清洁分切后代码"].ToString();
                mysheet.Cells[5 + i, 9].Value = dtInner.Rows[i]["规格a2"].ToString();
                mysheet.Cells[5 + i, 10].Value = dtInner.Rows[i]["数量b2"].ToString();
                mysheet.Cells[5 + i, 11].Value = dtInner.Rows[i]["数量"].ToString();
                mysheet.Cells[5 + i, 12].Value = dtInner.Rows[i]["工时"].ToString();
                mysheet.Cells[5 + i, 13].Value = dtInner.Rows[i]["操作人"].ToString();
                mysheet.Cells[5 + i, 14].Value = dtInner.Rows[i]["审核人"].ToString();
            }
            //超过模板行数的部分，插入
            if (i内表行数 > 8)
            {
                i超出行数 = i内表行数 - 8;
                for (int i = 8; i < i内表行数; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[5 + i, Type.Missing];

                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                        Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                    mysheet.Cells[5 + i, 1].Value = i + 1;
                    mysheet.Cells[5 + i, 2].Value = dtInner.Rows[i]["生产指令"].ToString();
                    mysheet.Cells[5 + i, 3].Value = dtInner.Rows[i]["生产日期"].ToString();
                    mysheet.Cells[5 + i, 4].Value = dtInner.Rows[i]["使用物料代码"].ToString();
                    mysheet.Cells[5 + i, 5].Value = dtInner.Rows[i]["规格a1"].ToString();
                    mysheet.Cells[5 + i, 6].Value = dtInner.Rows[i]["用量b1"].ToString();
                    mysheet.Cells[5 + i, 7].Value = dtInner.Rows[i]["使用量"].ToString();
                    mysheet.Cells[5 + i, 8].Value = dtInner.Rows[i]["清洁分切后代码"].ToString();
                    mysheet.Cells[5 + i, 9].Value = dtInner.Rows[i]["规格a2"].ToString();
                    mysheet.Cells[5 + i, 10].Value = dtInner.Rows[i]["数量b2"].ToString();
                    mysheet.Cells[5 + i, 11].Value = dtInner.Rows[i]["数量"].ToString();
                    mysheet.Cells[5 + i, 12].Value = dtInner.Rows[i]["工时"].ToString();
                    mysheet.Cells[5 + i, 13].Value = dtInner.Rows[i]["操作人"].ToString();
                    mysheet.Cells[5 + i, 14].Value = dtInner.Rows[i]["审核人"].ToString();
                }
            }
            mysheet.Cells[13 + i超出行数, 6].Value = txb清洁前合计A1.Text;
            mysheet.Cells[13 + i超出行数, 7].Value = txb清洁前合计A2.Text;
            mysheet.Cells[13 + i超出行数, 10].Value = txb清洁后合计B1.Text;
            mysheet.Cells[13 + i超出行数, 11].Value = txb清洁后合计B2.Text;
            if(Convert.ToDouble(txb清洁前合计A2.Text)!=0)
                mysheet.Cells[14 + i超出行数, 5].Value =Convert.ToDouble( Convert.ToDouble(txb清洁后合计B2.Text) / Convert.ToDouble(txb清洁前合计A2.Text)).ToString("P");
            else
                mysheet.Cells[14 + i超出行数, 5].Value =0;
            if (Convert.ToDouble(txb清洁后合计C.Text)!=0)
                mysheet.Cells[14 + i超出行数, 9].Value = Convert.ToDouble(txb清洁前合计A1.Text) / Convert.ToDouble(txb清洁后合计C.Text);
            else
                mysheet.Cells[14 + i超出行数, 9].Value = 0;
            mysheet.Cells[15 + i超出行数, 11].Value = txb清洁后合计C.Text;
            mysheet.Cells[15 + i超出行数, 2].Value = txb备注.Text;
            mysheet.Cells[16 + i超出行数, 1].Value = String.Format("操作人/日期：{0}      {1}", txb操作员.Text, dtp操作日期.Value.ToString("yyyy年MM月dd日"));
            mysheet.Cells[16 + i超出行数, 8].Value = String.Format("复核人/日期：{0}      {1}", txb审核人.Text, dtp审核日期.Value.ToString("yyyy年MM月dd日"));

            //加页脚
            int sheetnum=0;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from 清洁分切日报表 where 生产指令ID=" + __生产指令ID.ToString(), conOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
          //  sheetnum = sheetList.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
            //读取ID对应的生产指令编码
            OleDbCommand comm生产指令编码 = new OleDbCommand();
            comm生产指令编码.Connection = mySystem.Parameter.connOle;
            comm生产指令编码.CommandText = "select * from 清洁分切工序生产指令 where ID= @name";
            comm生产指令编码.Parameters.AddWithValue("@name", __生产指令ID);

            OleDbDataReader myReader生产指令编码 = comm生产指令编码.ExecuteReader();
            while (myReader生产指令编码.Read())
            {
                __生产指令 = myReader生产指令编码["生产指令编号"].ToString();
                //List<String> list班次 = new List<string>();
                //list班次.Add(myReader班次["班次"].ToString());
            }

            myReader生产指令编码.Close();
            comm生产指令编码.Dispose();  
            mysheet.PageSetup.RightFooter = __生产指令 + "-05-"  + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }

        //// TODO 打印
        //void print(bool preview)
        //{
        //    MessageBox.Show("打印功能正在完善");
        //}

        private void btn查询_Click(object sender, EventArgs e)
        {
            dtInner.Clear();
            if (cmb物料种类.SelectedItem == null || cmb物料种类.SelectedItem.ToString() == "全部")
            {
                query(datetimepicker开始时间.Value, datetimepicker结束时间.Value.Date, "");
            }
            else
            {
                query(datetimepicker开始时间.Value, datetimepicker结束时间.Value.Date, cmb物料种类.SelectedItem.ToString());
            }
            innerBind();
            computerOuterData();
        }

        void query(DateTime start,DateTime end, String matcode)
        {
            OleDbDataAdapter da;
            DataTable tmp;
            string sql;
            // 拿到这个日期下的所有ID
            sql = "select * from 清洁分切生产记录 where 生产日期 between #{0}# and #{1}#";
            da = new OleDbDataAdapter(string.Format(sql, start.Date,end.Date), mySystem.Parameter.connOle);
            tmp = new DataTable();
            da.Fill(tmp);
            List<int> ids = new List<int>();
            Hashtable ht生产指令 = new Hashtable();
            foreach (DataRow dr in tmp.Rows)
            {
                ids.Add(Convert.ToInt32(dr["ID"]));
                ht生产指令.Add(Convert.ToInt32(dr["ID"]),dr["生产指令编号"].ToString());
            }
            
            // 从这些ID中读取符合条件的记录
            //ret = new DataTable("日报表");
            tmp = new DataTable();
            foreach (int id in ids)
            {
                sql = "select * from 清洁分切生产记录详细信息 where T清洁分切生产记录ID={0} and 物料代码 like '%{1}%'";
                da = new OleDbDataAdapter(string.Format(sql, id, matcode), mySystem.Parameter.connOle);
                da.Fill(tmp);
            }
            int xuhao = 1;
            //string sql = "select * from 清洁分切生产记录详细信息 where ";
            //OleDbDataAdapter da = new OleDbDataAdapter(string.Format(sql, id, cmb物料种类.SelectedItem.ToString()), mySystem.Parameter.connOle);
            //DataTable tmp = new DataTable();
            //dain.Fill(tmp);
            foreach (DataRow dr in tmp.Rows)
            {
                DataRow ndr = dtInner.NewRow();
                //ndr["T清洁分切日报表ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
                ndr["序号"] = xuhao++;
                ndr["生产指令"] = ht生产指令[Convert.ToInt32(dr["T清洁分切生产记录ID"])];
                ndr["生产日期"] = datetimepicker结束时间.Value;
                ndr["使用物料代码"] = dr["物料代码"];
                // TODO 能自动填吗？
                string temp = ndr["使用物料代码"].ToString().TrimStart('X').Split('X')[0];
                string[] tmps = temp.Split('-');
                temp = tmps[tmps.Length - 1];
                ndr["规格a1"] = Convert.ToInt32(temp);
                ndr["用量b1"] = dr["长度A"];
                ndr["使用量"] = Convert.ToDouble(ndr["规格a1"]) * Convert.ToDouble(ndr["用量b1"]) / 1000;
                ndr["清洁分切后代码"] = dr["清洁分切后代码"];
                temp = ndr["清洁分切后代码"].ToString().TrimStart('X').Split('X')[0];
                tmps = temp.Split('-');
                temp = tmps[tmps.Length - 1];
                ndr["规格a2"] = Convert.ToInt32(temp.TrimEnd('C')); ;
                ndr["数量b2"] = dr["长度B"];
                ndr["数量"] = Convert.ToDouble(ndr["规格a2"]) * Convert.ToDouble(ndr["数量b2"]) / 1000;
                ndr["工时"] = Convert.ToDouble(dr["工时"]);
                ndr["操作人"] = mySystem.Parameter.userName;
                ndr["审核人"] = "";
                dtInner.Rows.Add(ndr);
            }

        }

 
    }
}
