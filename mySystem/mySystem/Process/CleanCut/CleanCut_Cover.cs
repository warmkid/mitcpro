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

        private SqlDataAdapter daOutersql, da_prodlistsql, da_prodlist2sql;
        private SqlCommandBuilder cbOutersql, cb_prodlistsql, cb_prodlist2sql;

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
            fill_printer();
            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            if (dtOuter.Rows.Count <= 0)
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
                    daOutersql.Update((DataTable)bsOuter.DataSource);
                }
                
                readOuterData(mySystem.Parameter.cleancutInstruID);
                outerBind();
            }

            //readInnerData((int)dtOuter.Rows[0]["ID"]);
            //innerBind();

            //readInnerData2((int)dtOuter.Rows[0]["ID"]);
            //innerBind2();

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

        public CleanCut_Cover(mySystem.MainForm mainform, int id)
            : base(mainform)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            addDataEventHandler();
            fill_printer();
            readOuterData(0, id);
            outerBind();

            if (dtOuter.Rows.Count <= 0)
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
                    daOutersql.Update((DataTable)bsOuter.DataSource);
                }

                readOuterData(mySystem.Parameter.cleancutInstruID);
                outerBind();
            }

            //readInnerData((int)dtOuter.Rows[0]["ID"]);
            //innerBind();

            //readInnerData2((int)dtOuter.Rows[0]["ID"]);
            //innerBind2();

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
            list_记录.Add("SOP-MFG-105-R03A 清洁分切批生产记录");
            list_记录.Add("SOP-MFG-302-R01A 清洁分切生产指令");
            list_记录.Add("SOP-MFG-302-R02A 清洁分切机开机确认记录");
            list_记录.Add("SOP-MFG-302-R03A 清洁分切领料生产记录");
            list_记录.Add("SOP-MFG-302-R04A 清洁分切机运行记录");
            list_记录.Add("SOP-MFG-110-R01A 清场记录");
            list_记录.Add("SOP-MFG-302-R05A 清洁分切日报表");
            list_记录.Add("清洁分切标签");
            
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
            
            DataTable dt;
            noid = new List<int>();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells[0].Value = false;
            }

            dataGridView1.Columns[totalPage].ReadOnly = true;

            //封面
            int temp;
            int idx = 0;
            dataGridView1.Rows[idx].Cells[totalPage].Value = 1;//封面页数默认为1

            //清洁分切生产指令
            idx++;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where  生产指令编号='" + _code + "'", mySystem.Parameter.connOle);
                dt = new DataTable("清洁分切工序生产指令");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 清洁分切工序生产指令 where  生产指令编号='" + _code + "'", mySystem.Parameter.conn);
                dt = new DataTable("清洁分切工序生产指令");
                da.Fill(dt);
            }
            
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //清洁分切机开机确认记录
            idx++;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 清洁分切开机确认 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("清洁分切开机确认");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 清洁分切开机确认 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("清洁分切开机确认");
                da.Fill(dt);
            }
            
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //清洁分切领料生产记录
            idx++;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 生产领料申请单表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("生产领料申请单表");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 生产领料申请单表 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("生产领料申请单表");
                da.Fill(dt);
            }
            
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //清洁分切机运行记录
            idx++;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 清洁分切运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("清洁分切运行记录");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 清洁分切运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("清洁分切运行记录");
                da.Fill(dt);
            }
            
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            // 清场记录
            idx++;
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("清场记录");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("清场记录");
                da.Fill(dt);
            }
            
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
            disableRow(idx);
            //da = new OleDbDataAdapter("select * from 清洁分切日报表 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            //dt = new DataTable("清洁分切日报表");
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

            //清洁分切标签
            idx++;
            //disableRow(idx);
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("标签");
                da.Fill(dt);
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("标签");
                da.Fill(dt);
            }
            
            temp = dt.Rows.Count;
            if (temp <= 0)
            {
                disableRow(idx);
            }
            else
            {
                dataGridView1.Rows[idx].Cells[totalPage].Value = temp;
            }

            //*********************************************************************

            //产品记录
            //读生产记录表
            
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da;
                da = new OleDbDataAdapter("select * from 清洁分切生产记录 where 生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                dt = new DataTable("清洁分切生产记录");
                Dictionary<string, double> dic = new Dictionary<string, double>();
                DataTable dtemp = new DataTable();//存放生产记录id
                DataTable dtemp2 = new DataTable();
                da.Fill(dtemp);
                if (dtemp.Rows.Count > 0)
                {
                    int id;
                    string str_code;
                    double d_leng;
                    for (int i = 0; i < dtemp.Rows.Count; i++)
                    {
                        id = int.Parse(dtemp.Rows[i]["ID"].ToString());
                        da = new OleDbDataAdapter("select * from 清洁分切生产记录详细信息 where T清洁分切生产记录ID=" + id, mySystem.Parameter.connOle);
                        da.Fill(dtemp2);

                        for (int j = 0; j < dtemp2.Rows.Count; j++)
                        {
                            str_code = dtemp2.Rows[j]["清洁分切后代码"].ToString();
                            d_leng = double.Parse(dtemp2.Rows[j]["长度B"].ToString());
                            if (!dic.ContainsKey(str_code))
                                dic.Add(str_code, d_leng);
                            else
                                dic[str_code] += d_leng;
                        }
                        dtemp2 = new DataTable();
                    }

                    if (dic.Count > 0)
                    {
                        foreach (var item in dic)
                        {
                            int index = this.dataGridView2.Rows.Add();
                            this.dataGridView2.Rows[index].Cells[0].Value = item.Key;//产品代码
                            this.dataGridView2.Rows[index].Cells[1].Value = "";//产品代码
                            this.dataGridView2.Rows[index].Cells[2].Value = item.Value; //生产数量
                        }

                    }
                }
            }
            else
            {
                SqlDataAdapter da;
                da = new SqlDataAdapter("select * from 清洁分切生产记录 where 生产指令ID=" + _instrctID, mySystem.Parameter.conn);
                dt = new DataTable("清洁分切生产记录");
                Dictionary<string, double> dic = new Dictionary<string, double>();
                DataTable dtemp = new DataTable();//存放生产记录id
                DataTable dtemp2 = new DataTable();
                da.Fill(dtemp);
                if (dtemp.Rows.Count > 0)
                {
                    int id;
                    string str_code;
                    double d_leng;
                    for (int i = 0; i < dtemp.Rows.Count; i++)
                    {
                        id = int.Parse(dtemp.Rows[i]["ID"].ToString());
                        da = new SqlDataAdapter("select * from 清洁分切生产记录详细信息 where T清洁分切生产记录ID=" + id, mySystem.Parameter.conn);
                        da.Fill(dtemp2);

                        for (int j = 0; j < dtemp2.Rows.Count; j++)
                        {
                            str_code = dtemp2.Rows[j]["清洁分切后代码"].ToString();
                            d_leng = double.Parse(dtemp2.Rows[j]["长度B"].ToString());
                            if (!dic.ContainsKey(str_code))
                                dic.Add(str_code, d_leng);
                            else
                                dic[str_code] += d_leng;
                        }
                        dtemp2 = new DataTable();
                    }

                    if (dic.Count > 0)
                    {
                        foreach (var item in dic)
                        {
                            int index = this.dataGridView2.Rows.Add();
                            this.dataGridView2.Rows[index].Cells[0].Value = item.Key;//产品代码
                            this.dataGridView2.Rows[index].Cells[1].Value = "";//产品代码
                            this.dataGridView2.Rows[index].Cells[2].Value = item.Value; //生产数量
                        }

                    }
                }
            }
            
           
            

            dataGridView2.AllowUserToAddRows = false;
            //dataGridView2.DataSource = dt;
            dataGridView2.ReadOnly = true;

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
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter.Update((DataTable)bsOuter.DataSource);
            }
            else
            {
                daOutersql.Update((DataTable)bsOuter.DataSource);
            }
            
            readOuterData(mySystem.Parameter.cleancutInstruID);
            outerBind();

            //内表保存，目录
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            }
            else
            {
                da_prodlistsql.Update((DataTable)bs_prodlist.DataSource);
            }
            
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();
            
            //内表2保存，记录,不用保存，因为是只读的

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
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter = new OleDbDataAdapter(@"select * from 批生产记录表 where 生产指令ID=" + instrid, mySystem.Parameter.connOle);
                cbOuter = new OleDbCommandBuilder(daOuter);
                daOuter.Fill(dtOuter);
            }
            else
            {
                daOutersql = new SqlDataAdapter(@"select * from 批生产记录表 where 生产指令ID=" + instrid, mySystem.Parameter.conn);
                cbOutersql = new SqlCommandBuilder(daOutersql);
                daOutersql.Fill(dtOuter);
            }
            
        }
        void readOuterData(int instrid, int thisid)
        {
            dtOuter = new DataTable("批生产记录表");
            bsOuter = new BindingSource();
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter = new OleDbDataAdapter(@"select * from 批生产记录表 where ID=" + thisid, mySystem.Parameter.connOle);
                cbOuter = new OleDbCommandBuilder(daOuter);
                daOuter.Fill(dtOuter);
            }
            else
            {
                daOutersql = new SqlDataAdapter(@"select * from 批生产记录表 where ID=" + thisid, mySystem.Parameter.conn);
                cbOutersql = new SqlCommandBuilder(daOutersql);
                daOutersql.Fill(dtOuter);
            }

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
            //tb使用物料.DataBindings.Clear();
            //dtp开始时间.DataBindings.Clear();
            //dtp结束时间.DataBindings.Clear();
            tb汇总.DataBindings.Clear();
            tb批准.DataBindings.Clear();
            tb审核.DataBindings.Clear();
            dtp汇总时间.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();
            dtp批准时间.DataBindings.Clear();
            tb备注.DataBindings.Clear();

            bsOuter.DataSource = dtOuter;

            tb生产指令.DataBindings.Add("Text", bsOuter.DataSource, "生产指令编号");
            //tb使用物料.DataBindings.Add("Text", bsOuter.DataSource, "使用物料");
            //dtp开始时间.DataBindings.Add("Value", bsOuter.DataSource, "开始生产时间");
            //dtp结束时间.DataBindings.Add("Text", bsOuter.DataSource, "结束生产时间");
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
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
                cb_prodlist = new OleDbCommandBuilder(da_prodlist);
                da_prodlist.Fill(dt_prodlist);
            }
            else
            {
                da_prodlistsql = new SqlDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.conn);
                cb_prodlistsql = new SqlCommandBuilder(da_prodlistsql);
                da_prodlistsql.Fill(dt_prodlist);
            }
            

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
            if (!mySystem.Parameter.isSqlOk)
            {
                da_prodlist2 = new OleDbDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.connOle);
                cb_prodlist2 = new OleDbCommandBuilder(da_prodlist2);
                da_prodlist2.Fill(dt_prodlist2);
            }
            else
            {
                da_prodlist2sql = new SqlDataAdapter("select * from 批生产记录产品详细信息 where T批生产记录封面ID=" + outid, mySystem.Parameter.conn);
                cb_prodlist2sql = new SqlCommandBuilder(da_prodlist2sql);
                da_prodlist2sql.Fill(dt_prodlist2);
            }
            
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


        //打印功能
        private Microsoft.Office.Interop.Excel._Worksheet printValue(Microsoft.Office.Interop.Excel._Worksheet mysheet, Microsoft.Office.Interop.Excel._Workbook mybook)
        {
            //外表信息
            mysheet.Cells[4, 5].Value = dtOuter.Rows[0]["生产指令编号"].ToString();

            //计算插入行的数量
            int ind = 0;
            if (dataGridView2.Rows.Count > 10)
            {
                //在第6行插入
                for (int i = 0; i < dataGridView2.Rows.Count - 10; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)mysheet.Rows[15, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                    Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                }
                ind = dataGridView2.Rows.Count - 10;
            }

            //内表2，代码、批号、生产数量
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                mysheet.Cells[6 + i, 4].Value = dataGridView2.Rows[i].Cells[0].Value.ToString();
                mysheet.Cells[6 + i, 5].Value = dataGridView2.Rows[i].Cells[1].Value.ToString();
                mysheet.Cells[6 + i, 6].Value = dataGridView2.Rows[i].Cells[2].Value.ToString();            
            }

            //内表1，目录
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                
                string s;
                if (i == 0)
                    s = "1";
                else
                {
                    if (dataGridView1.Rows[i].Cells[1].Value != null)
                    {

                        //s = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        List<Int32> pages = getIntList(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        s = pages.Count.ToString();
                    }
                    else
                        s = "0";
                }

                mysheet.Cells[5 + i, 1].Value = dataGridView1.Rows[i].Cells[2].Value.ToString();
                mysheet.Cells[5 + i, 2].Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                mysheet.Cells[5 + i, 3].Value = s;
            }

            //备注，汇总人，审核人，批准人
            mysheet.Cells[16 + ind, 1].Value = "备注：\n"+dtOuter.Rows[0]["备注"].ToString();
            mysheet.Cells[16 + ind, 5].Value = dtOuter.Rows[0]["汇总人"].ToString()+"  "+DateTime.Parse(dtOuter.Rows[0]["汇总时间"].ToString()).ToShortDateString();
            mysheet.Cells[18 + ind, 5].Value = dtOuter.Rows[0]["审核人"].ToString() + "  " + DateTime.Parse(dtOuter.Rows[0]["审核时间"].ToString()).ToShortDateString();
            mysheet.Cells[20 + ind, 5].Value = dtOuter.Rows[0]["批准人"].ToString() + "  " + DateTime.Parse(dtOuter.Rows[0]["批准时间"].ToString()).ToShortDateString();

            //加页脚
            int sheetnum;
            OleDbDataAdapter da = new OleDbDataAdapter("select ID from " + "批生产记录表" + " where 生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            List<Int32> sheetList = new List<Int32>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { sheetList.Add(Convert.ToInt32(dt.Rows[i]["ID"].ToString())); }
            sheetnum = sheetList.IndexOf(Convert.ToInt32(dtOuter.Rows[0]["ID"])) + 1;
            mysheet.PageSetup.RightFooter = _code + " &P/" + mybook.ActiveSheet.PageSetup.Pages.Count.ToString(); // "生产指令-步骤序号- 表序号 /&P"; // &P 是页码
            //返回
            return mysheet;
        }

        // 打印函数
        void print(bool isShow)
        {
            // 打开一个Excel进程
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            // 利用这个进程打开一个Excel文件
            Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(System.IO.Directory.GetCurrentDirectory() + @"\..\..\xls\cleancut\0 SOP-MFG-105-R03A 清洁分切批生产记录封面.xlsx");
            // 选择一个Sheet，注意Sheet的序号是从1开始的
            Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];
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
                        string log = "=====================================\n";
                        log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + label角色.Text + "：" + mySystem.Parameter.userName + " 打印文档\n";
                        dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

                        bsOuter.EndEdit();
                        if (!mySystem.Parameter.isSqlOk)
                        {
                            daOuter.Update((DataTable)bsOuter.DataSource);
                        }
                        else
                        {
                            daOutersql.Update((DataTable)bsOuter.DataSource);
                        }
                        
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
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter daUser = new OleDbDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "全部" + "';", mySystem.Parameter.connOle);
                BindingSource bsUser = new BindingSource();
                OleDbCommandBuilder cbUser = new OleDbCommandBuilder(daUser);
                daUser.Fill(dtUser);
            }
            else
            {
                SqlDataAdapter daUser = new SqlDataAdapter("SELECT * FROM " + tabName + " WHERE 步骤 = '" + "全部" + "';", mySystem.Parameter.conn);
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
            if (!mySystem.Parameter.isSqlOk)
            {
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
            }
            else
            {
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
            }
            

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
            if (!mySystem.Parameter.isSqlOk)
            {
                OleDbDataAdapter da_temp = new OleDbDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.connOle);
                OleDbCommandBuilder cb_temp = new OleDbCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
            else
            {
                SqlDataAdapter da_temp = new SqlDataAdapter(@"select * from 待审核 where 表名='批生产记录表' and 对应ID=" + (int)dtOuter.Rows[0]["ID"], mySystem.Parameter.conn);
                SqlCommandBuilder cb_temp = new SqlCommandBuilder(da_temp);
                da_temp.Fill(dt_temp);
                dt_temp.Rows[0].Delete();
                da_temp.Update(dt_temp);
            }
           

            //写日志
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n审核员：" + mySystem.Parameter.userName + " 完成审核\n";
            log += "审核结果：" + (checkform.ischeckOk == true ? "通过\n" : "不通过\n");
            log += "审核意见：" + checkform.opinion;
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;

            bsOuter.EndEdit();
            if (!mySystem.Parameter.isSqlOk)
            {
                daOuter.Update((DataTable)bsOuter.DataSource);
            }
            else
            {
                daOutersql.Update((DataTable)bsOuter.DataSource);
            }
            
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
        private void fill_printer()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cmb打印.Items.Add(sPrint);
            }
            cmb打印.SelectedItem = print.PrinterSettings.PrinterName;
        }
        private void bt打印_Click(object sender, EventArgs e)
        {
            if (cmb打印.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cmb打印.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
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

        public void perform打印本页()
        {
            if (cmb打印.Text == "")
            {
                MessageBox.Show("选择一台打印机");
                return;
            }
            SetDefaultPrinter(cmb打印.Text);
            //true->预览
            //false->打印
            print(false);
            GC.Collect();
        }

        private void btn打印选中项_Click(object sender, EventArgs e)
        {
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
                OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产指令 where ID=" + _instrctID, mySystem.Parameter.connOle);
                DataTable dt = new DataTable("生产指令");
                int id;
                List<Int32> pages = getIntList(dataGridView1.Rows[r].Cells[1].Value.ToString());
                switch (r)
                {
                    case 0:// 清洁分切批生产记录
                        //perform打印本页();
                        break;

                    case 1: // 清洁分切生产指令
                        da = new OleDbDataAdapter("select * from 清洁分切工序生产指令 where  ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("清洁分切工序生产指令");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.CleanCut.Instru(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 2: // 清洁分切机开机确认记录
                        da = new OleDbDataAdapter("select * from 清洁分切开机确认 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("清洁分切开机确认");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.CleanCut.CleanCut_CheckBeforePower(mainform, id)).print(false);
                            GC.Collect();
                        }

                        break;
                    case 3: // 清洁分切领料生产记录
                        da = new OleDbDataAdapter("select * from 清洁分切生产记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("清洁分切生产记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.CleanCut.CleanCut_Productrecord(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;

                    case 4://清洁分切机运行记录
                        da = new OleDbDataAdapter("select * from 清洁分切运行记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("清洁分切运行记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            (new mySystem.Process.CleanCut.CleanCut_RunRecord(mainform, id)).print(false);
                            GC.Collect();
                        }
                        break;
                    case 5:// 清场记录                                    
                        da = new OleDbDataAdapter("select * from 清场记录 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("清场记录");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            // TODO
                            (new mySystem.Process.CleanCut.Record_cleansite_cut(mainform, id)).print(false);
                            GC.Collect();

                        }
                        break;

                    case 6://日报表
                        break;

                    case 7://标签
                        da = new OleDbDataAdapter("select * from 标签 where  生产指令ID=" + _instrctID, mySystem.Parameter.connOle);
                        dt = new DataTable("标签");
                        da.Fill(dt);
                        foreach (int page in pages)
                        {
                            id = Convert.ToInt32(dt.Rows[page - 1]["ID"]);
                            // TODO
                            mySystem.Process.CleanCut.清洁分切标签.printLable(id);
                            GC.Collect();
                        }
                        break;
                        

                }
            }
            perform打印本页();
        }

        private void CleanCut_Cover_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
