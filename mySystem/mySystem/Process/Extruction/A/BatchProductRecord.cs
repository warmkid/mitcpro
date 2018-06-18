using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;
using mySystem;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace BatchProductRecord
{
    
    public partial class BatchProductRecord : mySystem.BaseForm
    {

        int _instrctID, _myID;
        string _code;
        List<Int32> noid = null;
        List<string> record=null;
        int totalPage = 4;

        OleDbDataAdapter daOuter, daInner1,daInner2;
        OleDbCommandBuilder cbOuter, cbInner1,cbInner2;

        SqlDataAdapter daOuter_sql, daInner1_sql, daInner2_sql;
        SqlCommandBuilder cbOuter_sql, cbInner1_sql, cbInner2_sql;

        DataTable dtOuter, dtInner1,dtInner2;
        BindingSource bsOuter, bsInner1,bsInner2;

        List<string> ls操作员, ls审核员;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        mySystem.CheckForm ckform;

        Hashtable htRow2Page = new Hashtable();
        bool isFirstBind1 = true;
        bool isFirstBind2 = true; 

        public BatchProductRecord(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            // 构建外表，写日志，构建内表
            // 读取数据填写两个内表
            readOuterData(mySystem.Parameter.proInstruction);
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                if (!mySystem.Parameter.isSqlOk)
                {
                    daOuter.Update((DataTable)bsOuter.DataSource);
                }
                else
                {
                    if (((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] == DBNull.Value)
                    {
                        ((DataTable)bsOuter.DataSource).Rows[0]["审核是否通过"] = false;
                    }
                    daOuter_sql.Update((DataTable)bsOuter.DataSource);
                }
                
                readOuterData(mySystem.Parameter.proInstruction);
                outerBind();
            }
            setFormState();
            setEnableReadOnly();
            setKeyInfo(mySystem.Parameter.proInstruID, Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.proInstruction);
            readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            inner2Bind();
            checkInner2Data();
            init();
            initly();
            addOtherEventHandler();
        }

        public BatchProductRecord(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                InitializeComponent();
                // TODO
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 批生产记录表 where ID=" + id, mySystem.Parameter.connOle);
                DataTable dt = new DataTable();
                da.Fill(dt);
                setKeyInfo(Convert.ToInt32(dt.Rows[0]["生产指令ID"]), id, dt.Rows[0]["生产指令编号"].ToString());
                getOtherData();
                // 构建外表，写日志，构建内表
                // 读取数据填写两个内表
                readOuterData(_code);
                outerBind();
                if (dtOuter.Rows.Count == 0)
                {
                    DataRow dr = dtOuter.NewRow();
                    dr = writeOuterDefault(dr);
                    dtOuter.Rows.Add(dr);
                    if (!mySystem.Parameter.isSqlOk)
                    {
                        daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    else
                    {
                        daOuter_sql.Update((DataTable)bsOuter.DataSource);
                    }
                    
                    readOuterData(_code);
                    outerBind();
                }
                readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                inner2Bind();
                checkInner2Data();
                init();
                initly();

            }
            else
            {
                InitializeComponent();
                // TODO
                SqlDataAdapter da = new SqlDataAdapter("select * from 批生产记录表 where ID=" + id, mySystem.Parameter.conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                setKeyInfo(Convert.ToInt32(dt.Rows[0]["生产指令ID"]), id, dt.Rows[0]["生产指令编号"].ToString());
                getOtherData();
                // 构建外表，写日志，构建内表
                // 读取数据填写两个内表
                readOuterData(_code);
                outerBind();
                if (dtOuter.Rows.Count == 0)
                {
                    DataRow dr = dtOuter.NewRow();
                    dr = writeOuterDefault(dr);
                    dtOuter.Rows.Add(dr);
                    if (!mySystem.Parameter.isSqlOk)
                    {
                        daOuter.Update((DataTable)bsOuter.DataSource);
                    }
                    else
                    {
                        daOuter_sql.Update((DataTable)bsOuter.DataSource);
                    }
                    
                    readOuterData(_code);
                    outerBind();
                }
                readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                inner2Bind();
                checkInner2Data();
                init();
                initly();

            }
            addOtherEventHandler();
        }

        void setKeyInfo(int pid, int mid, string code)
        {
            _instrctID = pid;
            _myID = mid;
            _code = code;
        }

        void readOuterData(string code){
            if (!mySystem.Parameter.isSqlOk)
            {
                string sql = "select * from 批生产记录表 where 生产指令编号='{0}'";
                daOuter = new OleDbDataAdapter(string.Format(sql, code), mySystem.Parameter.connOle);
                cbOuter = new OleDbCommandBuilder(daOuter);
                bsOuter = new BindingSource();
                dtOuter = new DataTable("批生产记录表");

                daOuter.Fill(dtOuter);
            }
            else
            {
                string sql = "select * from 批生产记录表 where 生产指令编号='{0}'";
                daOuter_sql = new SqlDataAdapter(string.Format(sql, code), mySystem.Parameter.conn);
                cbOuter_sql = new SqlCommandBuilder(daOuter_sql);
                bsOuter = new BindingSource();
                dtOuter = new DataTable("批生产记录表");

                daOuter_sql.Fill(dtOuter);
            }

        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            tb备注.DataBindings.Clear();
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");

            tb生产指令.DataBindings.Clear();
            tb生产指令.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");

            tb使用物料.DataBindings.Clear();
            tb使用物料.DataBindings.Add("Text", bsOuter.DataSource, "使用物料");

            tb开始时间.DataBindings.Clear();
            tb开始时间.DataBindings.Add("Text", bsOuter.DataSource, "开始生产时间");

            tb结束时间.DataBindings.Clear();
            tb结束时间.DataBindings.Add("Text", bsOuter.DataSource, "结束生产时间");

            tb汇总人.DataBindings.Clear();
            tb汇总人.DataBindings.Add("Text", bsOuter.DataSource, "汇总人");

            dtp汇总时间.DataBindings.Clear();
            dtp汇总时间.DataBindings.Add("Value", bsOuter.DataSource, "汇总时间");

            tb审核人.DataBindings.Clear();
            tb审核人.DataBindings.Add("Text", bsOuter.DataSource, "审核人");

            dtp审核时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");

            tb批准人.DataBindings.Clear();
            tb批准人.DataBindings.Add("Text", bsOuter.DataSource, "批准人");

            dtp批准时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Add("Value", bsOuter.DataSource, "批准时间");


        }

        DataRow writeOuterDefault(DataRow dr)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                dr["生产指令ID"] = mySystem.Parameter.proInstruID;
                dr["生产指令编号"] = mySystem.Parameter.proInstruction;

                da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                dr["使用物料"] = dt.Rows[0]["内外层物料代码"].ToString() + "," + dt.Rows[0]["中层物料代码"].ToString();
                dr["开始生产时间"] = dt.Rows[0]["开始生产日期"];
                dr["结束生产时间"] = DateTime.Now;
                dr["汇总人"] = mySystem.Parameter.userName;
                dr["汇总时间"] = DateTime.Now;
                dr["批准时间"] = DateTime.Now;
                dr["审核时间"] = DateTime.Now;
                return dr;
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                dr["生产指令ID"] = mySystem.Parameter.proInstruID;
                dr["生产指令编号"] = mySystem.Parameter.proInstruction;

                da = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                dr["使用物料"] = dt.Rows[0]["内外层物料代码"].ToString() + "," + dt.Rows[0]["中层物料代码"].ToString();
                dr["开始生产时间"] = dt.Rows[0]["开始生产日期"];
                dr["结束生产时间"] = DateTime.Now;
                dr["汇总人"] = mySystem.Parameter.userName;
                dr["汇总时间"] = DateTime.Now;
                dr["批准时间"] = DateTime.Now;
                dr["审核时间"] = DateTime.Now;
                return dr;
            }

        }

        void getOtherData()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印.Items.Add(sPrint);
            }
            cmb打印.SelectedItem = print.PrinterSettings.PrinterName;
        }

        void readInner2Data(int id)
        {
            // 先读取所有的产品代码
            // 然后根据产品代码读取所有数量

            if (!mySystem.Parameter.isSqlOk)
            {
                daInner2 = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.connOle);
                cbInner2 = new OleDbCommandBuilder(daInner2);
                dtInner2 = new DataTable("批生产记录产品详细信息");
                bsInner2 = new BindingSource();

                daInner2.Fill(dtInner2);

                //daInner2
                // daInner2.Update((DataTable)bsInner2.DataSource);
            }
            else
            {
                daInner2_sql = new SqlDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.conn);
                cbInner2_sql = new SqlCommandBuilder(daInner2_sql);
                dtInner2 = new DataTable("批生产记录产品详细信息");
                bsInner2 = new BindingSource();

                daInner2_sql.Fill(dtInner2);
            }

        }

        void checkInner2Data()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where 生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                Hashtable ht产品代码 = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        if (ht产品代码.ContainsKey(dr["产品名称"].ToString()))
                        {
                            double weight = Convert.ToDouble((ht产品代码[dr["产品名称"].ToString()] as List<Object>)[1]) + Convert.ToDouble(dr["累计同规格膜卷长度R"]);
                            (ht产品代码[dr["产品名称"].ToString()] as List<Object>)[1] = weight;
                            //ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), weight }));
                        }
                        else
                        {
                            ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), Convert.ToDouble(dr["累计同规格膜卷长度R"]) }));
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), 0 }));
                    }
                }


                foreach (String k in ht产品代码.Keys)
                {
                    DataRow[] drs = dtInner2.Select("产品代码='" + k + "'");
                    if (drs.Length == 0)
                    {
                        DataRow dr = dtInner2.NewRow();
                        dr["T批生产记录封面ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
                        dr["产品代码"] = k;
                        dr["产品批号"] = (ht产品代码[k] as List<Object>)[0].ToString();
                        dtInner2.Rows.Add(dr);
                        dr["生产数量"] = Convert.ToDouble((ht产品代码[k] as List<Object>)[1]);
                    }
                    else
                    {
                        drs[0]["生产数量"] = Convert.ToDouble((ht产品代码[k] as List<Object>)[1]);
                    }

                }
                daInner2.Update((DataTable)bsInner2.DataSource);
                readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                inner2Bind();
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                da = new SqlDataAdapter("select * from 吹膜工序生产和检验记录 where 生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                Hashtable ht产品代码 = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        if (ht产品代码.ContainsKey(dr["产品名称"].ToString()))
                        {
                            double weight = Convert.ToDouble((ht产品代码[dr["产品名称"].ToString()] as List<Object>)[1]) + Convert.ToDouble(dr["累计同规格膜卷长度R"]);
                            (ht产品代码[dr["产品名称"].ToString()] as List<Object>)[1] = weight;
                            //ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), weight }));
                        }
                        else
                        {
                            ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), Convert.ToDouble(dr["累计同规格膜卷长度R"]) }));
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        ht产品代码.Add(dr["产品名称"].ToString(), new List<Object>(new Object[] { dr["产品批号"].ToString(), 0 }));
                    }
                }


                foreach (String k in ht产品代码.Keys)
                {
                    DataRow[] drs = dtInner2.Select("产品代码='" + k + "'");
                    if (drs.Length == 0)
                    {
                        DataRow dr = dtInner2.NewRow();
                        dr["T批生产记录封面ID"] = Convert.ToInt32(dtOuter.Rows[0]["ID"]);
                        dr["产品代码"] = k;
                        dr["产品批号"] = (ht产品代码[k] as List<Object>)[0].ToString();
                        dtInner2.Rows.Add(dr);
                        dr["生产数量"] = Convert.ToDouble((ht产品代码[k] as List<Object>)[1]);
                    }
                    else
                    {
                        drs[0]["生产数量"] = Convert.ToDouble((ht产品代码[k] as List<Object>)[1]);
                    }

                }
                daInner2_sql.Update((DataTable)bsInner2.DataSource);
                readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
                inner2Bind();
            }

        }

        void inner2Bind()
        {
            bsInner2.DataSource = dtInner2;
            dataGridView2.DataSource = bsInner2.DataSource;
            Utility.setDataGridViewAutoSizeMode(dataGridView1);
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }

        private void init()
        {
            record = new List<string>();
            //record.Add("SOP-MFG-301-R00 吹膜批处理生产记录");
            record.Add("SOP-MFG-301-R01 吹膜工序生产指令");
            record.Add("SOP-MFG-301-R02 吹膜生产日报表");
            record.Add("SOP-MFG-301-R03 吹膜机组清洁记录");
            record.Add("SOP-MFG-301-R04 吹膜机组开机确认表");
            record.Add("SOP-MFG-301-R05 吹膜机组预热参数记录表");
            record.Add("SOP-MFG-301-R06 吹膜供料记录");
            record.Add("SOP-MFG-301-R07 吹膜供料系统运行记录");
            record.Add("SOP-MFG-301-R08 吹膜机组运行记录");
            record.Add("SOP-MFG-301-R09 吹膜工序生产和检验记录");
            record.Add("SOP-MFG-301-R10 吹膜工序废品记录");
            record.Add("SOP-MFG-301-R11 吹膜工序清场记录");
            record.Add("SOP-MFG-301-R12 吹膜工序物料平衡记录");
            record.Add("SOP-MFG-301-R13 吹膜工序领料退料记录");
            record.Add("SOP-MFG-301-R14 吹膜岗位交接班记录");
            record.Add("SOP-MFG-109-R01A 产品内包装记录");
            record.Add("SOP-MFG-111-R01A 产品外包装记录");


            record.Add("吹膜标签");
            initrecord();
        }


        private void initly()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                noid = new List<int>();

                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    dgvr.Cells[0].Value = false;
                }

                dataGridView1.Columns[totalPage].ReadOnly = true;
                //
                //tb生产指令.Text = mySystem.Parameter.proInstruction;
                //tb生产指令.Enabled = false;

                //
                //OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //DataTable dt = new DataTable("生产指令信息表");
                //da.Fill(dt);
                //tb使用物料.Text = dt.Rows[0]["内外层物料代码"] + "," + dt.Rows[0]["中层物料代码"];

                //
                //tb开始时间.Text = dt.Rows[0]["开始生产日期"].ToString();
                //tb结束时间.Text = DateTime.Now.ToString();

                //
                //da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //dt = new DataTable("吹膜工序生产和检验记录");
                //da.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    int tmpid = Convert.ToInt32(dt.Rows[0]["ID"]);
                //    da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录详细信息 where  T吹膜工序生产和检验记录ID=" + tmpid, mySystem.Parameter.connOle);
                //    dt = new DataTable("吹膜工序生产和检验记录详细信息");
                //    da.Fill(dt);
                //    BindingSource bs = new BindingSource();
                //    bs.DataSource = dt;
                //    dataGridView2.DataSource = bs.DataSource;
                //}

                // 
                //dataGridView1.Rows[0].ReadOnly = true;
                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                // 生产指令
                int temp;
                int idx = 0;
                int tempid;
                da = new OleDbDataAdapter("select * from 生产指令信息表 where  ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("生产指令信息表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 生产日报表
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序生产和检验记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录详细信息 where T吹膜工序生产和检验记录ID=" + tempid, mySystem.Parameter.connOle);
                        DataTable tmpDt = new DataTable("吹膜工序生产和检验记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    // TODO 真的全是1吗
                    dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
                }
                // 清洁记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜机组清洁记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                //开机前确认表
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜机组开机前确认表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 预热参数记录表
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜机组预热参数记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 供料记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜供料记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + tempid, mySystem.Parameter.connOle);
                        DataTable tmpDt = new DataTable("吹膜供料记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 供料系统运行记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜供料系统运行记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 吹膜机组运行记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜机组运行记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 吹膜工序生产和检验记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序生产和检验记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 废品记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序废品记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜工序废品记录详细信息 where T吹膜工序废品记录ID=" + tempid, mySystem.Parameter.connOle);
                        DataTable tmpDt = new DataTable("吹膜工序废品记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    // TODO
                    dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
                }
                // 清场记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序清场记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 物料平衡记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序物料平衡记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 领料退料记录
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜工序领料退料记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 岗位交接班
                idx++;
                da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("吹膜岗位交接班记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 内包装
                idx++;
                da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("产品内包装记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 外包装
                idx++;
                da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("产品外包装记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 吹膜标签
                idx++;
                da = new OleDbDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("标签");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 绑定控件和DataGridView
                // TODO，这里应该读取真实的产量
                //da = new OleDbDataAdapter("select 产品编码,产品批号,计划产量米 from 生产指令产品列表 where 生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //dt = new DataTable("生产指令产品列表");
                //da.Fill(dt);
                //dataGridView2.AllowUserToAddRows = false;
                //dataGridView2.Columns.Clear();
                //dataGridView2.DataSource = dt;
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                noid = new List<int>();

                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    dgvr.Cells[0].Value = false;
                }

                dataGridView1.Columns[totalPage].ReadOnly = true;
                //
                //tb生产指令.Text = mySystem.Parameter.proInstruction;
                //tb生产指令.Enabled = false;

                //
                //OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //DataTable dt = new DataTable("生产指令信息表");
                //da.Fill(dt);
                //tb使用物料.Text = dt.Rows[0]["内外层物料代码"] + "," + dt.Rows[0]["中层物料代码"];

                //
                //tb开始时间.Text = dt.Rows[0]["开始生产日期"].ToString();
                //tb结束时间.Text = DateTime.Now.ToString();

                //
                //da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //dt = new DataTable("吹膜工序生产和检验记录");
                //da.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    int tmpid = Convert.ToInt32(dt.Rows[0]["ID"]);
                //    da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录详细信息 where  T吹膜工序生产和检验记录ID=" + tmpid, mySystem.Parameter.connOle);
                //    dt = new DataTable("吹膜工序生产和检验记录详细信息");
                //    da.Fill(dt);
                //    BindingSource bs = new BindingSource();
                //    bs.DataSource = dt;
                //    dataGridView2.DataSource = bs.DataSource;
                //}

                // 
                //dataGridView1.Rows[0].ReadOnly = true;
                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

                //dataGridView1.Rows[0].Cells[totalPage].Value = 1;

                // 生产指令
                int temp;
                int idx = 0;
                int tempid;
                da = new SqlDataAdapter("select * from 生产指令信息表 where  生产指令编号='" + _code + "'", mySystem.Parameter.conn);
                dt = new DataTable("生产指令信息表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 生产日报表
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序生产和检验记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        SqlDataAdapter tmpDa = new SqlDataAdapter("select * from 吹膜工序生产和检验记录详细信息 where T吹膜工序生产和检验记录ID=" + tempid, mySystem.Parameter.conn);
                        DataTable tmpDt = new DataTable("吹膜工序生产和检验记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    // TODO 真的全是1吗
                    dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
                }
                // 清洁记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜机组清洁记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                //开机前确认表
                idx++;
                da = new SqlDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜机组开机前确认表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 预热参数记录表
                idx++;
                da = new SqlDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜机组预热参数记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 供料记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜供料记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        SqlDataAdapter tmpDa = new SqlDataAdapter("select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + tempid, mySystem.Parameter.conn);
                        DataTable tmpDt = new DataTable("吹膜供料记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 供料系统运行记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜供料系统运行记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 吹膜机组运行记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜机组运行记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 吹膜工序生产和检验记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序生产和检验记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 废品记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序废品记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    int tempcout = 0;
                    for (int i = 0; i < temp; ++i)
                    {
                        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                        SqlDataAdapter tmpDa = new SqlDataAdapter("select * from 吹膜工序废品记录详细信息 where T吹膜工序废品记录ID=" + tempid, mySystem.Parameter.conn);
                        DataTable tmpDt = new DataTable("吹膜工序废品记录详细信息");
                        tmpDa.Fill(tmpDt);
                        tempcout += tmpDt.Rows.Count;
                    }
                    // TODO
                    dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
                }
                // 清场记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序清场记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 物料平衡记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序物料平衡记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 领料退料记录
                idx++;
                da = new SqlDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜工序领料退料记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 岗位交接班
                idx++;
                da = new SqlDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("吹膜岗位交接班记录");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 内包装
                idx++;
                da = new SqlDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("产品内包装记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 外包装
                idx++;
                da = new SqlDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("产品外包装记录表");
                da.Fill(dt);
                temp = dt.Rows.Count;
                if (temp <= 0)
                {
                    disableRow(idx);
                }
                else
                {
                    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
                }
                // 绑定控件和DataGridView
                // TODO，这里应该读取真实的产量
                //da = new OleDbDataAdapter("select 产品编码,产品批号,计划产量米 from 生产指令产品列表 where 生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //dt = new DataTable("生产指令产品列表");
                //da.Fill(dt);
                //dataGridView2.AllowUserToAddRows = false;
                //dataGridView2.Columns.Clear();
                //dataGridView2.DataSource = dt;
            }
            
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex == totalPage+1)
                //{
                //    if (noid.IndexOf(e.RowIndex) < 0)
                //    {
                //        int page = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[totalPage + 1].Value);
                //        if (page > Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[totalPage].Value))
                //        {
                //            MessageBox.Show("页码太大！");
                //        }
                //        else
                //        {
                //            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //            DataTable dt = new DataTable("生产指令信息表");
                //            int id;
                //            switch (e.RowIndex)
                //            {
                //                case 0: // 生产指令
                //                    da = new OleDbDataAdapter("select * from 生产指令信息表 where  ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                    dt = new DataTable("吹膜工序生产和检验记录");
                //                    da.Fill(dt);
                //                    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                    (new ProcessProductInstru(mainform, id)).Show();
                //                    break;
                //                case 1: // 日报表
                //                    da = new OleDbDataAdapter("select * from 吹膜生产日报表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                    dt = new DataTable("吹膜生产日报表");
                //                    da.Fill(dt);
                //                    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                    (new mySystem.ProdctDaily_extrus(mainform, id)).Show();
                //                    break;
                //                case 2: // 清洁记录表
                //                    da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                    dt = new DataTable("吹膜机组清洁记录表");
                //                    da.Fill(dt);
                //                    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                    (new WindowsFormsApplication1.Record_extrusClean(mainform, id)).Show();
                //                    break;
                //                case 3://吹膜机组开机前确认表
                //                    da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                    dt = new DataTable("吹膜机组开机前确认表");
                //                    da.Fill(dt);
                //                    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                    (new mySystem.Extruction.Process.ExtructionCheckBeforePowerStep2(mainform, id)).Show();
                //                    break;
                //                case 4:// 预热参数记录表                                    
                //                    da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                    dt = new DataTable("吹膜机组预热参数记录表");
                //                    da.Fill(dt);
                //                    id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                    (new mySystem.Extruction.Process.ExtructionPreheatParameterRecordStep3(mainform, id)).Show();
                //                    break;
                //                case 5://供料记录
                //                     da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜供料记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new WindowsFormsApplication1.Record_extrusSupply(mainform, id)).Show();
                //                     break;
                //                case 6://供料系统运行记录
                //                     da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜供料系统运行记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Process.Extruction.C.Feed(mainform, id)).Show();
                //                     break;
                //                case 7:// 吹膜机组运行记录

                //                     da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜机组运行记录");
                                                                         
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Process.Extruction.B.Running(mainform, id)).Show();
                //                     break;
                //                case 8:// 吹膜工序生产和检验记录

                //                     da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜工序生产和检验记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Process.ExtructionpRoductionAndRestRecordStep6(mainform, id)).Show();
                //                     break;
                //                case 9:// 废品记录

                //                     da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜工序废品记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Process.Extruction.B.Waste(mainform, id)).Show();
                //                     break;
                //                case 10:
                //                     // 清场记录
                //                     da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜工序清场记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Process.Record_extrusSiteClean(mainform, id)).Show();
                //                     break;
                //                case 11:// 物料平衡记录s
                //                     da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜工序物料平衡记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform, id)).Show();
                //                     break;
                //                case 12:// 领料退料记录
                //                     da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜工序领料退料记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Process.Record_material_reqanddisg(mainform, id)).Show();
                //                     break;
                //                case 13: // 岗位交接班
                //                     da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("吹膜岗位交接班记录");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Process.Extruction.A.HandOver(mainform, id)).Show();
                //                     break;
                //                case 14:// 内包装
                //                     da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("产品内包装记录表");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Process.ProductInnerPackagingRecord(mainform, id)).Show();
                //                     break;
                //                case 15:// 外包装
                //                     da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
                //                     dt = new DataTable("产品外包装记录表");
                //                     da.Fill(dt);
                //                     id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                //                     (new mySystem.Extruction.Chart.outerpack(mainform, id)).Show();
                //                     break;
                //            }
                //        }

                //        //switch (e.RowIndex)
                //        //{
                //        // TODO show window
                //        //}
                //    }
                //}
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }


        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            //MessageBox.Show("sss");
        }
        private void initrecord()
        {
            for (int i = 0; i < record.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i+1;
                dr.Cells[3].Value = record[i];
                //if (record[i] == "SOP-MFG-301-R00 吹膜批处理生产记录")
                //{
                //    //dr.Cells["总页数"].Value = 1;
                //}
                dataGridView1.Rows.Add(dr);
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private List<Int32> getIntList(String str)
        {
            try
            {
                List<Int32> ret = new List<int>();
                String[] strs = str.Split('-');
                if (1 == strs.Length)
                {
                    ret.Add(Convert.ToInt32(strs[0]));
                }
                else if (2 == strs.Length)
                {
                    int a = Convert.ToInt32(strs[0]);
                    int b = Convert.ToInt32(strs[1]);
                    if (a > b)
                    {
                        throw new Exception("后面数字比前面的大");
                    }
                    for (int i = a; i <= b; ++i)
                    {
                        ret.Add(i);
                    }
                }
                else
                {
                    throw new Exception("打印页码解析失败！");
                }
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<int>();
            }
            
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        private void btn打印_Click(object sender, EventArgs e)
        {
            htRow2Page = new Hashtable();
            // TODO GC.Collect()
            List<Int32> checkedRows = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                {
                    checkedRows.Add(i);
                }
            }

            foreach (Int32 r in checkedRows)
            {
                try
                {
                    List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                }
                catch 
                {
                    MessageBox.Show("打印页数请填入数字或遵循1-2这种格式");
                    return;
                }
            }

            SetDefaultPrinter(cmb打印.Text);
            #region

            if (!mySystem.Parameter.isSqlOk)
            {
                foreach (Int32 r in checkedRows)
                {
                    
                    OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" + _instrctID, mySystem.Parameter.connOle);
                    DataTable dt = new DataTable("生产指令信息表");
                    int id;
                    List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                    
                    switch (r)
                    {
                        case 0: // 生产指令
                            da = new OleDbDataAdapter("select * from 生产指令信息表 where  ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序生产和检验记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new ProcessProductInstru(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 1: // 日报表
                            da = new OleDbDataAdapter("select * from 吹膜生产日报表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜生产日报表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.ProdctDaily_extrus(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 2: // 清洁记录表
                            da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜机组清洁记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new WindowsFormsApplication1.Record_extrusClean(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 3://吹膜机组开机前确认表
                            da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜机组开机前确认表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ExtructionCheckBeforePowerStep2(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 4:// 预热参数记录表                                    
                            da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜机组预热参数记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                // TODO
                                (new mySystem.Extruction.Process.ExtructionPreheatParameterRecordStep3(mainform, id)).print(false);
                                GC.Collect();

                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 5://供料记录
                            da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜供料记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new WindowsFormsApplication1.Record_extrusSupply(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 6://供料系统运行记录
                            da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜供料系统运行记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.C.Feed(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 7:// 吹膜机组运行记录

                            da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜机组运行记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.B.Running(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 8:// 吹膜工序生产和检验记录
                            da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序生产和检验记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ExtructionpRoductionAndRestRecordStep6(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 9:// 废品记录

                            da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序废品记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.B.Waste(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 10:
                            // 清场记录
                            da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序清场记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.Record_extrusSiteClean(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 11:// 物料平衡记录
                            da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序物料平衡记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 12:// 领料退料记录
                            da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜工序领料退料记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.Record_material_reqanddisg(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 13: // 岗位交接班
                            da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("吹膜岗位交接班记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.A.HandOver(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 14:// 内包装
                            da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("产品内包装记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ProductInnerPackagingRecord(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 15:// 外包装
                            da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("产品外包装记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 16:// 标签
                            da = new OleDbDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                            dt = new DataTable("标签");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                mySystem.Process.Extruction.LabelPrint.printLable(id);
                                //(new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                    }
                }
            }
            else
            {
                foreach (Int32 r in checkedRows)
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + _instrctID, mySystem.Parameter.conn);
                    DataTable dt = new DataTable("生产指令信息表");
                    int id;
                    List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                    switch (r)
                    {
                        //case 0:// 批处理
                        //    printSelf();
                        //    break;
                        case 0: // 生产指令
                            da = new SqlDataAdapter("select * from 生产指令信息表 where  ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序生产和检验记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new ProcessProductInstru(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 1: // 日报表
                            da = new SqlDataAdapter("select * from 吹膜生产日报表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜生产日报表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.ProdctDaily_extrus(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 2: // 清洁记录表
                            da = new SqlDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜机组清洁记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new WindowsFormsApplication1.Record_extrusClean(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 3://吹膜机组开机前确认表
                            da = new SqlDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜机组开机前确认表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ExtructionCheckBeforePowerStep2(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 4:// 预热参数记录表                                    
                            da = new SqlDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜机组预热参数记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                // TODO
                                (new mySystem.Extruction.Process.ExtructionPreheatParameterRecordStep3(mainform, id)).print(false);
                                GC.Collect();

                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 5://供料记录
                            da = new SqlDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜供料记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new WindowsFormsApplication1.Record_extrusSupply(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 6://供料系统运行记录
                            da = new SqlDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜供料系统运行记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.C.Feed(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 7:// 吹膜机组运行记录

                            da = new SqlDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜机组运行记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.B.Running(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 8:// 吹膜工序生产和检验记录
                            da = new SqlDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序生产和检验记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ExtructionpRoductionAndRestRecordStep6(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 9:// 废品记录

                            da = new SqlDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序废品记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.B.Waste(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 10:
                            // 清场记录
                            da = new SqlDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序清场记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.Record_extrusSiteClean(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 11:// 物料平衡记录
                            da = new SqlDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序物料平衡记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 12:// 领料退料记录
                            da = new SqlDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜工序领料退料记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.Record_material_reqanddisg(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 13: // 岗位交接班
                            da = new SqlDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("吹膜岗位交接班记录");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Process.Extruction.A.HandOver(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 14:// 内包装
                            da = new SqlDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("产品内包装记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Process.ProductInnerPackagingRecord(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 15:// 外包装
                            da = new SqlDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("产品外包装记录表");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                (new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                        case 16:// 标签
                            da = new SqlDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                            dt = new DataTable("标签");
                            da.Fill(dt);
                            foreach (int page in pages)
                            {
                                id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                                mySystem.Process.Extruction.LabelPrint.printLable(id);
                                //(new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                                GC.Collect();
                            }
                            htRow2Page[r] = pages.Count;
                            break;
                    }
                }
            }

            printSelf();
            # endregion

        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            

            save();
        }

        void save()
        {
            if (!mySystem.Parameter.isSqlOk)
                daOuter.Update((DataTable)bsOuter.DataSource);
            else
                daOuter_sql.Update((DataTable)bsOuter.DataSource);

            readOuterData(_code);
            outerBind();
            if (_userState == mySystem.Parameter.UserState.操作员)
                btn提交审核.Enabled = true;
        }


        private void getPeople()
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                string tabName = "用户权限";
                DataTable dtUser = new DataTable(tabName);
                OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "批生产记录表" + "';", mySystem.Parameter.connOle);
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
            else
            {
                string tabName = "用户权限";
                DataTable dtUser = new DataTable(tabName);
                SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "批生产记录表" + "';", mySystem.Parameter.conn);
                BindingSource bsUser = new BindingSource();
                SqlCommandBuilder cbUser = new SqlCommandBuilder(daUser);
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

        }

        private void setUserState()
        {
            _userState = mySystem.Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= mySystem.Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= mySystem.Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (mySystem.Parameter.UserState.NoBody == _userState)
            {
                _userState = mySystem.Parameter.UserState.管理员;
                label角色.Text = mySystem.Parameter.userName+"(管理员)";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (mySystem.Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = mySystem.Parameter.UserState.操作员;
                else _userState = mySystem.Parameter.UserState.审核员;

            }
            if (mySystem.Parameter.UserState.操作员 == _userState) label角色.Text = mySystem.Parameter.userName+"(操作员)";
            if (mySystem.Parameter.UserState.审核员 == _userState) label角色.Text = mySystem.Parameter.userName+"(审核员)";
        }

        void setFormState()
        {
            string s = dtOuter.Rows[0]["审核人"].ToString();
            bool b = Convert.ToBoolean(dtOuter.Rows[0]["审核是否通过"]);
            if (s == "") _formState = 0;
            else if (s == "__待审核") _formState = mySystem.Parameter.FormState.待审核;
            else
            {
                if (b) _formState = mySystem.Parameter.FormState.审核通过;
                else _formState = mySystem.Parameter.FormState.审核未通过;
            }
        }

        private void setControlTrue()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = false;
                }
                else
                {
                    c.Enabled = true;
                }
            }
            // 保证这两个按钮一直是false
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
        }

        private void setControlFalse()
        {
            foreach (Control c in this.Controls)
            {
                if (c is DataGridView)
                {
                    (c as DataGridView).ReadOnly = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
            btn打印.Enabled = true;
            cmb打印.Enabled = true;
            bt查看人员信息.Enabled = true;
            splitContainer1.Enabled = true;
            splitContainer1.Panel1.Enabled = true;
            tb备注.ReadOnly = true;
            splitContainer1.Panel2.Enabled = false;
            dataGridView1.ReadOnly = false;
        }


        void setEnableReadOnly()
        {

            if (mySystem.Parameter.UserState.管理员 == _userState)
            {
                setControlTrue();
            }
            if (mySystem.Parameter.UserState.审核员 == _userState)
            {
                if (mySystem.Parameter.FormState.待审核 == _formState)
                {
                    setControlFalse();
                    btn审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (mySystem.Parameter.UserState.操作员 == _userState)
            {
                if (mySystem.Parameter.FormState.未保存 == _formState || mySystem.Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
            }
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            String n;
            if (!checkOuterData(out n))
            {
                MessageBox.Show("请填写完整的信息: " + n, "提示");
                return;
            }

            //if (!checkInnerData(dataGridView1))
            //{
            //    MessageBox.Show("请填写完整的表单信息", "提示");
            //    return;
            //}
            //if (!checkInnerData(dataGridView2))
            //{
            //    MessageBox.Show("请填写完整的表单信息", "提示");
            //    return;
            //}
            if (!mySystem.Parameter.isSqlOk)
            {
                DataTable dt_temp = new DataTable("待审核");
                BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);

                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "批生产记录表";
                    dr["对应ID"] = (int)dtOuter.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);

                dtOuter.Rows[0]["审核人"] = "__待审核";
                dtOuter.Rows[0]["审核时间"] = DateTime.Now;
                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(_code);
                outerBind();

                setControlFalse();
            }
            else
            {
                DataTable dt_temp = new DataTable("待审核");
                BindingSource bs_temp = new BindingSource();
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);

                if (dt_temp.Rows.Count == 0)
                {
                    DataRow dr = dt_temp.NewRow();
                    dr["表名"] = "批生产记录表";
                    dr["对应ID"] = (int)dtOuter.Rows[0]["ID"];
                    dt_temp.Rows.Add(dr);
                }
                bs_temp.DataSource = dt_temp;
                da_temp.Update((DataTable)bs_temp.DataSource);

                dtOuter.Rows[0]["审核人"] = "__待审核";
                dtOuter.Rows[0]["审核时间"] = DateTime.Now;
                bsOuter.EndEdit();
                daOuter_sql.Update((DataTable)bsOuter.DataSource);
                readOuterData(_code);
                outerBind();

                setControlFalse();
            }

        }

        private void btn审核_Click(object sender, EventArgs e)
        {
            ckform = new mySystem.CheckForm(this);
            ckform.ShowDialog();
        }

        public override void CheckResult()
        {
            dtOuter.Rows[0]["审核人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["审核时间"] = ckform.time;
            dtOuter.Rows[0]["批准人"] = mySystem.Parameter.userName;
            dtOuter.Rows[0]["批准时间"] = ckform.time;
            dtOuter.Rows[0]["审核意见"] = ckform.opinion;
            dtOuter.Rows[0]["审核是否通过"] = ckform.ischeckOk;

            setControlFalse();

            if (!mySystem.Parameter.isSqlOk)
            {
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                bsOuter.EndEdit();
                daOuter.Update((DataTable)bsOuter.DataSource);
                base.CheckResult();
            }
            else
            {
                DataTable dt_temp = new DataTable("待审核");
                //BindingSource bs_temp = new BindingSource();
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);

                bsOuter.EndEdit();
                daOuter_sql.Update((DataTable)bsOuter.DataSource);
                base.CheckResult();
            }

            try
            {
                (this.Owner as ExtructionMainForm).InitBtn();
            }
            catch (NullReferenceException exp)
            {

            }
            try
            {
                (this.Owner as mySystem.Query.QueryExtruForm).search();
            }
            catch (NullReferenceException exp) { }

            base.CheckResult();
        }

        private void bt查看人员信息_Click(object sender, EventArgs e)
        {
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                DataTable dt;
                da = new OleDbDataAdapter("select * from 用户权限 where 步骤='批生产记录表'", mySystem.Parameter.connOle);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            else
            {
                SqlDataAdapter da;
                DataTable dt;
                da = new SqlDataAdapter("select * from 用户权限 where 步骤='批生产记录表'", mySystem.Parameter.conn);
                dt = new DataTable("temp");
                da.Fill(dt);
                String str操作员 = dt.Rows[0]["操作员"].ToString();
                String str审核员 = dt.Rows[0]["审核员"].ToString();
                String str人员信息 = "人员信息：\n\n操作员：" + str操作员 + "\n\n审核员：" + str审核员;
                MessageBox.Show(str人员信息);
            }
            
        }

        void addOtherEventHandler()
        {
            dataGridView2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView2_DataBindingComplete);
        }

        void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["T批生产记录封面ID"].Visible = false;
            if (isFirstBind2)
            {
                readDGVWidthFromSettingAndSet(dataGridView2);
                isFirstBind2 = false;
            }
        }


        void printSelf()
        {
            print(false);
            GC.Collect();
        }

        int print(bool b)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir += "./../../xls/Extrusion/A/SOP-MFG-105-R02A 吹膜工序批生产记录封面.xlsx";
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(dir);
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[3];
            // 修改Sheet中某行某列的值
            fill_excel(my);
            // 拿到打印后的页数
            int ccc = wb.ActiveSheet.PageSetup.Pages.Count;
            //
            my.PageSetup.RightFooter = tb生产指令.Text + "-00-001 &P/" + wb.ActiveSheet.PageSetup.Pages.Count;
            if (b)
            {
                // 设置该进程是否可见
                oXL.Visible = true;
                // 让这个Sheet为被选中状态
                my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
                return 0;
            }
            else
            {
                int pageCount = 0;
                // 直接用默认打印机打印该Sheet
                try
                {
                    my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
                }
                catch
                { }
                finally
                {
                    pageCount = wb.ActiveSheet.PageSetup.Pages.Count;
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
                return pageCount;
            }
        }

        private void fill_excel(Microsoft.Office.Interop.Excel._Worksheet my)
        {
            my.Cells[4, 5].Value = dtOuter.Rows[0]["生产指令编号"];
            my.Cells[5, 5].Value = dtOuter.Rows[0]["使用物料"];
            my.Cells[6, 5].Value = Convert.ToDateTime(dtOuter.Rows[0]["开始生产时间"]).ToString("yyy/MM/dd") + "--" + Convert.ToDateTime(dtOuter.Rows[0]["结束生产时间"]).ToString("yyy/MM/dd");
            for (int i = 0; i < dtInner2.Rows.Count; ++i)
            {
                my.Cells[8 + i, 4].Value = dtInner2.Rows[i]["产品代码"];
                my.Cells[8 + i, 5].Value = dtInner2.Rows[i]["产品批号"];
                my.Cells[8 + i, 6].Value = dtInner2.Rows[i]["生产数量"];
            }
            my.Cells[22, 2].Value = dtOuter.Rows[0]["备注"];
            my.Cells[22, 5].Value = dtOuter.Rows[0]["汇总人"].ToString() + "   " + dtOuter.Rows[0]["汇总时间"];
            my.Cells[24, 5].Value = dtOuter.Rows[0]["审核人"].ToString() + "   " + dtOuter.Rows[0]["审核时间"];
            my.Cells[26, 5].Value = dtOuter.Rows[0]["批准人"].ToString() + "   " + dtOuter.Rows[0]["批准时间"];

            //int prePage = 0;
            //int curPage = 0;
            for (int i = 5; i <= 21; ++i)
            {
                my.Cells[i, 3] = 0;
                if (htRow2Page.ContainsKey(i - 5))
                {
                    //curPage += Convert.ToInt32(htRow2Page[i - 5]);
                    my.Cells[i, 3] = htRow2Page[i - 5];
                }
                //else
                //{
                //    my.Cells[i, 3] = "/";
                //}
                
                //prePage += Convert.ToInt32(htRow2Page[i - 5]);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (isFirstBind1)
            {
                readDGVWidthFromSettingAndSet(dataGridView1);
                isFirstBind1 = false;
            }
        }
        
        private void BatchProductRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string width = getDGVWidth(dataGridView1);
            if (dataGridView1.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView1);
            }
            if (dataGridView2.ColumnCount > 0)
            {
                writeDGVWidthToSetting(dataGridView2);
            }
        }
        public static void 生成表单(string intrcode, int instrid)
        {
            SqlDataAdapter dat, datt;
            SqlCommandBuilder cbt;
            DataTable dtt, dttt;
            string sql = "select * from 批生产记录表 where 生产指令编号='{0}'";
            dat = new SqlDataAdapter(string.Format(sql, intrcode), mySystem.Parameter.conn);
            cbt = new SqlCommandBuilder(dat);
            dtt = new DataTable("批生产记录表");

            dat.Fill(dtt);

            if (dtt.Rows.Count == 0)
            {
                DataRow dr = dtt.NewRow();
                dr["生产指令ID"] = instrid;
                dr["生产指令编号"] = intrcode;

                datt = new SqlDataAdapter("select * from 生产指令信息表 where ID=" + instrid, mySystem.Parameter.conn);
                dttt = new DataTable("temp");
                datt.Fill(dttt);
                dr["使用物料"] = dttt.Rows[0]["内外层物料代码"].ToString() + "," + dttt.Rows[0]["中层物料代码"].ToString();
                dr["开始生产时间"] = dttt.Rows[0]["开始生产日期"];
                dr["结束生产时间"] = DateTime.Now;
                dr["汇总人"] = mySystem.Parameter.userName;
                dr["汇总时间"] = DateTime.Now;
                dr["批准时间"] = DateTime.Now;
                dr["审核时间"] = DateTime.Now;

                dr["审核是否通过"] = false;

                dtt.Rows.Add(dr);

                dat.Update(dtt);
            }
        }
    }

}

