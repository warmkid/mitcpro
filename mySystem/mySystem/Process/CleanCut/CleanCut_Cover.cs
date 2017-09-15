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

namespace mySystem.Process.CleanCut
{
    public partial class CleanCut_Cover : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;

        private DataTable dtOuter, dt_prodlist, dt_prodlist2;//外表，内表目录，内表汇总
        private OleDbDataAdapter daOuter, da_prodlist, da_prodlist2;
        private BindingSource bsOuter, bs_prodlist, bs_prodlist2;
        private OleDbCommandBuilder cbOuter, cb_prodlist, cb_prodlist2;
        private string person_操作员;
        private string person_审核员;
        List<string> list_记录;
        List<int> list_页数;
        private int stat_user;//登录人状态，0 操作员， 1 审核员， 2管理员
        private int stat_form;//窗口状态  0：未保存；1：待审核；2：审核通过；3：审核未通过

        List<string> ls操作员, ls审核员;

        mySystem.Parameter.UserState _userState;
        mySystem.Parameter.FormState _formState;

        mySystem.CheckForm ckform;

        int totalPage = 4;
        List<int> noid;

        int _instrctID, _myID;
        string _code;

        public CleanCut_Cover(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();

            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(mySystem.Parameter.cleancutInstruID);
                outerBind();
            }

            readInnerData((int)dtOuter.Rows[0]["ID"]);
            innerBind();

            setFormState();
            setEnableReadOnly();

            setKeyInfo(mySystem.Parameter.cleancutInstruID, Convert.ToInt32(dtOuter.Rows[0]["ID"]), mySystem.Parameter.cleancutInstruction);
            //readInner2Data(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            //inner2Bind();
            // TODO 获取生产数据
            //checkInner2Data();
            init();
            initly();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
        }

        void setKeyInfo(int pid, int mid, string code)
        {
            _instrctID = pid;
            _myID = mid;
            _code = code;
        }

        private void init()
        {
            list_记录 = new List<string>();
            list_记录.Add("SOP-MFG-110-R01A 清场记录");
            list_记录.Add("SOP-MFG-302-R01A 清洁分切生产指令");
            list_记录.Add("SOP-MFG-302-R02A 清洁分切机开机确认及运行记录");
            list_记录.Add("SOP-MFG-302-R03A 清洁分切生产记录表");
            list_记录.Add("SOP-MFG-302-R04A 清洁分切日报表");
            
            initrecord();
        }

        private void initrecord()
        {
            for (int i = 0; i < list_记录.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[2].Value = i + 1;
                dr.Cells[3].Value = list_记录[i];
                dataGridView1.Rows.Add(dr);
            }
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
            //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;


            // 清场记录
            int temp;
            int idx = 0;
            int tempid;
            da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _instrctID , mySystem.Parameter.connOle);
            dt = new DataTable("清场记录");
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
            // 清洁分切生产指令
            idx++;
            da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where  生产指令编号='" + _code + "'", mySystem.Parameter.connOle);
            dt = new DataTable("清洁分切工序生产指令");
            da.Fill(dt);
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                //int tempcout = 0;
                //for (int i = 0; i < temp; ++i)
                //{
                //    tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
                //    OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录详细信息 where T吹膜工序生产和检验记录ID=" + tempid, mySystem.Parameter.connOle);
                //    DataTable tmpDt = new DataTable("吹膜工序生产和检验记录详细信息");
                //    tmpDa.Fill(tmpDt);
                //    tempcout += tmpDt.Rows.Count;
                //}
                // TODO 真的全是1吗
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }
            // 清洁分切机开机确认及运行记录   TODO 分开
            idx++;
            da = new OleDbDataAdapter("select * from 清洁分切运行记录 where  生产指令编号='" + _code + "'", mySystem.Parameter.connOle);
            dt = new DataTable("清洁分切运行记录");
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
            //清洁分切生产记录表
            idx++;
            da = new OleDbDataAdapter("select * from 清洁分切生产记录 where  生产指令编号='" + _code + "'", mySystem.Parameter.connOle);
            dt = new DataTable("清洁分切生产记录");
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
            // 清洁分切日报表
            idx++;
            da = new OleDbDataAdapter("select * from 清洁分切日报表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            dt = new DataTable("清洁分切日报表");
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
            //// 供料记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜供料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜供料记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    int tempcout = 0;
            //    for (int i = 0; i < temp; ++i)
            //    {
            //        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
            //        OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜供料记录详细信息 where T吹膜供料记录ID=" + tempid, mySystem.Parameter.connOle);
            //        DataTable tmpDt = new DataTable("吹膜供料记录详细信息");
            //        tmpDa.Fill(tmpDt);
            //        tempcout += tmpDt.Rows.Count;
            //    }
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 供料系统运行记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜供料系统运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜供料系统运行记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 吹膜机组运行记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜机组运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜机组运行记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 吹膜工序生产和检验记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜工序生产和检验记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜工序生产和检验记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 废品记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜工序废品记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜工序废品记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    int tempcout = 0;
            //    for (int i = 0; i < temp; ++i)
            //    {
            //        tempid = Convert.ToInt32(dt.Rows[i]["ID"]);
            //        OleDbDataAdapter tmpDa = new OleDbDataAdapter("select * from 吹膜工序废品记录详细信息 where T吹膜工序废品记录ID=" + tempid, mySystem.Parameter.connOle);
            //        DataTable tmpDt = new DataTable("吹膜工序废品记录详细信息");
            //        tmpDa.Fill(tmpDt);
            //        tempcout += tmpDt.Rows.Count;
            //    }
            //    // TODO
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = 1;
            //}
            //// 清场记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜工序清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜工序清场记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 物料平衡记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜工序物料平衡记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜工序物料平衡记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 领料退料记录
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜工序领料退料记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜工序领料退料记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 岗位交接班
            //idx++;
            //da = new OleDbDataAdapter("select * from 吹膜岗位交接班记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("吹膜岗位交接班记录");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 内包装
            //idx++;
            //da = new OleDbDataAdapter("select * from 产品内包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("产品内包装记录表");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            //// 外包装
            //idx++;
            //da = new OleDbDataAdapter("select * from 产品外包装记录表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("产品外包装记录表");
            //da.Fill(dt);
            //temp = dt.Rows.Count;
            //if (temp <= 0)
            //{
            //    disableRow(idx);
            //}
            //else
            //{
            //    dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            //}
            

        }

        void disableRow(int rowidx)
        {
            dataGridView1.Rows[rowidx].Cells[totalPage].Value = 0;
            dataGridView1.Rows[rowidx].ReadOnly = true;
            noid.Add(rowidx);
            dataGridView1.Rows[rowidx].DefaultCellStyle.BackColor = Color.Gray;
        }

        private void bt确认_Click(object sender, EventArgs e)
        {
            bool rt = save();

            //控件可见性
            if (_userState == mySystem.Parameter.UserState.操作员)
                bt发送审核.Enabled = true;
        }

        private bool save()
        {
            //判断合法性
            if (!input_Judge())
                return false;

            //外表保存
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            //内表保存
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            return true;
        }

        bool input_Judge()
        {
            //判断合法性
            //汇总人
            if (mySystem.Parameter.NametoID(tb汇总.Text) <= 0)
            {
                MessageBox.Show("汇总人ID不存在");
                return false;
            }
            //批准人
            //if (mySystem.Parameter.NametoID(tb批准.Text) <= 0)
            //{
            //    MessageBox.Show("批准人ID不存在");
            //    return false;
            //}
            return true;


        }

        void addDataEventHandler()
        {

        }

        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(int instrid)
        {
            dtOuter = new DataTable("批生产记录表");
            bsOuter = new BindingSource();
            daOuter = new OleDbDataAdapter(@"select * from 批生产记录表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
            cbOuter = new OleDbCommandBuilder(daOuter);
            daOuter.Fill(dtOuter);
        }
        // 给外表的一行写入默认值    TODO:*******************************
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["生产指令ID"] = mySystem.Parameter.cleancutInstruID;
            dr["生产指令编号"] = mySystem.Parameter.cleancutInstruction;

            dr["使用物料"] = "";
            dr["开始生产时间"] = DateTime.Now;
            dr["结束生产时间"] = DateTime.Now;
            dr["汇总时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["批准时间"] = DateTime.Now;
            dr["审核是否通过"] = false;
            dr["汇总人"] = mySystem.Parameter.userName;
            return dr;
        }
        // 先清除绑定，再完成外表和控件的绑定
        void outerBind()
        {
            tb生产指令.DataBindings.Clear();
            tb使用物料.DataBindings.Clear();
            dtp开始时间.DataBindings.Clear();
            dtp结束时间.DataBindings.Clear();
            tb汇总.DataBindings.Clear();
            tb批准.DataBindings.Clear();
            tb审核.DataBindings.Clear();
            dtp汇总时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Clear();
            tb备注.DataBindings.Clear();

            bsOuter.DataSource = dtOuter;

            tb生产指令.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            tb使用物料.DataBindings.Add("Text", bsOuter.DataSource, "使用物料");
            dtp开始时间.DataBindings.Add("Value", bsOuter.DataSource, "开始生产时间");
            dtp结束时间.DataBindings.Add("Text", bsOuter.DataSource, "结束生产时间");
            tb汇总.DataBindings.Add("Text", bsOuter.DataSource, "汇总人");
            dtp汇总时间.DataBindings.Add("Value", bsOuter.DataSource, "汇总时间");
            tb审核.DataBindings.Add("Text", bsOuter.DataSource, "审核人");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");
            tb批准.DataBindings.Add("Text", bsOuter.DataSource, "批准人");
            dtp批准时间.DataBindings.Add("Value", bsOuter.DataSource, "批准时间");
            tb备注.DataBindings.Add("Text", bsOuter.DataSource, "备注");
        }

        // 根据条件从数据库中读取内表数据,目录
        void readInnerData(int outid)
        {
            dt_prodlist = new DataTable("批生产记录产品详细信息");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            ////如果为空，则进行插入
            //if (dt_prodlist.Rows.Count <= 0)
            //{
            //    for (int i = 0; i < list_记录.Count; i++)
            //    {
            //        DataRow dr = dt_prodlist.NewRow();
            //        dr["T批生产记录封面ID"] = dt_prodinstr.Rows[0]["ID"];
            //        dr["序号"] = i + 1;
            //        dr["记录"] = list_记录[i];
            //        dr["页数"] = list_页数[i];
            //        dt_prodlist.Rows.Add(dr);
            //    }
            //}
        }

        // 内表和控件的绑定，目录
        void innerBind()
        {
            //移除所有列
            while (dataGridView2.Columns.Count > 0)
                dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            bs_prodlist.DataSource = dt_prodlist;
            dataGridView2.DataSource = bs_prodlist.DataSource;
            setDataGridViewColumns();
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }

        // 根据条件从数据库中读取内表数据,汇总
        void readInnerData2(int outid)
        {
            dt_prodlist2 = new DataTable("批生产记录产品详细信息");
            bs_prodlist2 = new BindingSource();
            da_prodlist2 = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
            cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
            da_prodlist2.Fill(dt_prodlist2);
        }

        // 内表和控件的绑定
        void innerBind2()
        {
            //移除所有列
            while (dataGridView2.Columns.Count > 0)
                dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            setDataGridViewCombox();
            bs_prodlist2.DataSource = dt_prodlist2;
            dataGridView2.DataSource = bs_prodlist2.DataSource;
            setDataGridViewColumns2();
            Utility.setDataGridViewAutoSizeMode(dataGridView2);
        }

        void setDataGridViewCombox()
        {
            foreach (DataColumn dc in dt_prodlist2.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "生产数量":
                        DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
                        c1.DataPropertyName = dc.ColumnName;
                        c1.HeaderText = "生产数量(米)";
                        c1.Name = dc.ColumnName;
                        c1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c1.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c1);
                        break;

                    default:
                        DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
                        c2.DataPropertyName = dc.ColumnName;
                        c2.HeaderText = dc.ColumnName;
                        c2.Name = dc.ColumnName;
                        c2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        c2.ValueType = dc.DataType;
                        dataGridView2.Columns.Add(c2);
                        break;
                }
            }
        }

        // 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
        void setDataGridViewColumns()
        {
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
        }

        void setDataGridViewColumns2()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        // 设置各控件的事件
        void addDateEventHandler()
        { }

        // 打印函数
        void print(bool label)
        { }
        // 获取其他需要的数据 记录和页数
        void getOtherData()
        {
            //list_记录 = new List<string>();
            //list_记录.Add("SOP-MFG-302-R01A 清洁分切生产指令");
            //list_记录.Add("SOP-MFG-302-R02A 清洁分切机开机确认及运行记录");
            //list_记录.Add("SOP-MFG-302-R03A 清洁分切生产记录表");
            //list_记录.Add("SOP-MFG-302-R04A 清洁分切日报表");
            //list_记录.Add("SOP-MFG-110-R01A 清场记录");

            //list_页数 = new List<int>();
            //list_页数.Add(2);
            //list_页数.Add(3);
            //list_页数.Add(4);
            //list_页数.Add(2);
            //list_页数.Add(5);

            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印.Items.Add(sPrint);
            }
            cmb打印.SelectedItem = print.PrinterSettings.PrinterName;
        }
        // 获取操作员和审核员
        void getPeople()
        {
            string tabName = "用户权限";
            DataTable dtUser = new DataTable(tabName);
            OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "全部" + "';", mySystem.Parameter.connOle);
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
        // 计算，主要用于日报表、物料平衡记录中的计算
        void computer()
        { }
        // 获取当前窗体状态：
        // 如果『审核人』为空，则为未保存
        // 否则，如果『审核人』为『__待审核』，则为『待审核』
        // 否则
        // 如果审核结果为『通过』，则为『审核通过』
        // 如果审核结果为『不通过』，则为『审核未通过』
        // 这个函数可以放在父类中？
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
        // 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
        void setUserState()
        {
            _userState = mySystem.Parameter.UserState.NoBody;
            if (ls操作员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= mySystem.Parameter.UserState.操作员;
            if (ls审核员.IndexOf(mySystem.Parameter.userName) >= 0) _userState |= mySystem.Parameter.UserState.审核员;
            // 如果即不是操作员也不是审核员，则是管理员
            if (mySystem.Parameter.UserState.NoBody == _userState)
            {
                _userState = mySystem.Parameter.UserState.管理员;
                label角色.Text = "管理员";
            }
            // 让用户选择操作员还是审核员，选“是”表示操作员
            if (mySystem.Parameter.UserState.Both == _userState)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要以操作员身份进入", "提示", MessageBoxButtons.YesNo)) _userState = mySystem.Parameter.UserState.操作员;
                else _userState = mySystem.Parameter.UserState.审核员;

            }
            if (mySystem.Parameter.UserState.操作员 == _userState) label角色.Text = "操作员";
            if (mySystem.Parameter.UserState.审核员 == _userState) label角色.Text = "审核员";
        }
        // 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
        // 0：未保存；1：待审核；2：审核通过；3：审核未通过
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
                    setControlTrue();
                    bt审核.Enabled = true;
                }
                else setControlFalse();
            }
            if (mySystem.Parameter.UserState.操作员 == _userState)
            {
                if (mySystem.Parameter.FormState.未保存 == _formState || mySystem.Parameter.FormState.审核未通过 == _formState) setControlTrue();
                else setControlFalse();
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
            bt审核.Enabled = false;
            bt发送审核.Enabled = false;
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
            bt打印.Enabled = true;
            cmb打印.Enabled = true;
        }

        private void bt发送审核_Click(object sender, EventArgs e)
        {
            //写待审核表
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

            //写日志 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 提交审核
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n操作员：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            dtOuter.Rows[0]["审核人"] = "__待审核";
            dtOuter.Rows[0]["审核时间"] = DateTime.Now;

            save();

            //空间都不能点
            setControlFalse();
            

        }

        private void bt日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
        }

        private void bt审核_Click(object sender, EventArgs e)
        {
            checkform = new CheckForm(this);
            checkform.Show();
        }

        public override void CheckResult()
        {
            base.CheckResult();

            //获得审核信息
            //dtp审批日期.Value = checkform.time;
            dtOuter.Rows[0]["审核人"] = checkform.userName;
            dtOuter.Rows[0]["审核时间"] = checkform.time;
            dtOuter.Rows[0]["批准人"] = checkform.userName;
            dtOuter.Rows[0]["批准时间"] = checkform.time;
            dtOuter.Rows[0]["审核意见"] = checkform.opinion;
            dtOuter.Rows[0]["审核是否通过"] = checkform.ischeckOk;
            //状态
            setControlFalse();


            //写待审核表
            DataTable dt_temp = new DataTable("待审核");
            //BindingSource bs_temp = new BindingSource();
            OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
            OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
            da_temp.Fill(dt_temp);
            dt_temp.Rows[0].Delete();
            da_temp.Update(dt_temp);

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
        }

        private void bt打印_Click(object sender, EventArgs e)
        {

        }
    }
}
