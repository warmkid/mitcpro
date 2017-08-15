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

namespace BatchProductRecord
{
    // TODO  审核
    public partial class BatchProductRecord : mySystem.BaseForm
    {

        int _instrctID, _myID;
        string _code;
        List<Int32> noid = null;
        List<string> record=null;
        int totalPage = 4;

        OleDbDataAdapter daOuter, daInner1,daInner2;
        OleDbCommandBuilder cbOuter, cbInner1,cbInner2;
        DataTable dtOuter, dtInner1,dtInner2;
        BindingSource bsOuter, bsInner1,bsInner2;


        public BatchProductRecord(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
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
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(mySystem.Parameter.proInstruction);
                outerBind();
            }
            setKeyInfo(mySystem.Parameter.proInstruID, Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.proInstruction);
            readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            inner2Bind();
            checkInner2Data();
            init();
            initly();

        }

        public BatchProductRecord(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            // TODO
            init();

        }

        void setKeyInfo(int pid, int mid, string code)
        {
            _instrctID = pid;
            _myID = mid;
            _code = code;
        }

        void readOuterData(string code){
            string sql = "select * from 批生产记录表 where 生产指令编号='{0}'";
            daOuter = new OleDbDataAdapter(string.Format(sql, code),mySystem.Parameter.connOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("批生产记录表");

            daOuter.Fill(dtOuter);
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
            OleDbDataAdapter da;
            DataTable dt;
            dr["生产指令ID"] = mySystem.Parameter.proInstruID;
            dr["生产指令编号"] = mySystem.Parameter.proInstruction;

            da = new OleDbDataAdapter("select * from 生产指令信息表 where ID="+mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
            dt  = new DataTable("temp");
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
            daInner2 = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.connOle);
            cbInner2 = new OleDbCommandBuilder(daInner2);
            dtInner2 = new DataTable("批生产记录产品详细信息");
            bsInner2 = new BindingSource();

            daInner2.Fill(dtInner2);

           //daInner2
           // daInner2.Update((DataTable)bsInner2.DataSource);

        }

        void checkInner2Data()
        {
            OleDbDataAdapter da;
            DataTable dt;
            da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where 生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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

        void inner2Bind()
        {
            bsInner2.DataSource = dtInner2;
            dataGridView2.DataSource = bsInner2.DataSource;
        }

        private void init()
        {
            record = new List<string>();
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
            initrecord();
        }


        private void initly()
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
            da = new OleDbDataAdapter("select * from 生产指令信息表 where  生产指令编号='" + mySystem.Parameter.proInstruction + "'", mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
                    tempcout+=tmpDt.Rows.Count;
                }
                // TODO 真的全是1吗
                dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
            }
            // 清洁记录
            idx++;
            da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
                dataGridView1.Rows[idx].Cells[totalPage].Value = 1 ;
            }
            // 清场记录
            idx++;
            da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + mySystem.Parameter.proInstruID, mySystem.Parameter.connOle);
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
            catch(Exception exp)
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

        private void btn打印_Click(object sender, EventArgs e)
        {
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
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令信息表 where ID=" +_instrctID, mySystem.Parameter.connOle);
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
                        }
                        break;
                    case 1: // 日报表
                        da = new OleDbDataAdapter("select * from 吹膜生产日报表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜生产日报表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.ProdctDaily_extrus(mainform, id)).print(false);
                        }
                        
                        break;
                    case 2: // 清洁记录表
                        da = new OleDbDataAdapter("select * from 吹膜机组清洁记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜机组清洁记录表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new WindowsFormsApplication1.Record_extrusClean(mainform, id)).print(false);
                        }
                        
                        break;
                    case 3://吹膜机组开机前确认表
                        da = new OleDbDataAdapter("select * from 吹膜机组开机前确认表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜机组开机前确认表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Process.ExtructionCheckBeforePowerStep2(mainform, id)).print(false);
                        }
                        break;
                    case 4:// 预热参数记录表                                    
                        da = new OleDbDataAdapter("select * from 吹膜机组预热参数记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜机组预热参数记录表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            // TODO
                            //(new mySystem.Extruction.Process.ExtructionPreheatParameterRecordStep3(mainform, id)).print(false);

                        }
                        break;
                    case 5://供料记录
                        da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜供料记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new WindowsFormsApplication1.Record_extrusSupply(mainform, id)).print(false);
                        }
                        break;
                    case 6://供料系统运行记录
                        da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜供料系统运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Extruction.C.Feed(mainform, id)).print(false);

                        }
                        break;
                    case 7:// 吹膜机组运行记录

                        da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜机组运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Extruction.B.Running(mainform, id)).print(false);
                        }
                        break;
                    case 8:// 吹膜工序生产和检验记录
                        da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜工序生产和检验记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Process.ExtructionpRoductionAndRestRecordStep6(mainform, id)).print(false);
                        }
                        break;
                    case 9:// 废品记录

                        da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜工序废品记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Extruction.B.Waste(mainform, id)).print(false);
                        }
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
                        }
                        break;
                    case 11:// 物料平衡记录s
                        da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜工序物料平衡记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Process.MaterialBalenceofExtrusionProcess(mainform, id)).print(false);
                        }
                        break;
                    case 12:// 领料退料记录
                        da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜工序领料退料记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Process.Record_material_reqanddisg(mainform, id)).print(false);
                        }
                        break;
                    case 13: // 岗位交接班
                        da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("吹膜岗位交接班记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.Extruction.A.HandOver(mainform, id)).print(false);
                        }
                        break;
                    case 14:// 内包装
                        da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品内包装记录表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Process.ProductInnerPackagingRecord(mainform, id)).print(false);
                        }
                        break;
                    case 15:// 外包装
                        da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("产品外包装记录表");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Extruction.Chart.outerpack(mainform, id)).print(false);
                        }
                        break;
                }
            }
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_code);
            outerBind();
        }
    }
}
