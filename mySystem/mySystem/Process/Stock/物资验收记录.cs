using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Process.Stock
{
    public partial class 物资验收记录 : Form
    {

        List<String> ls验收人;
        List<String> ls审核员;
        List<String> ls物资厂家代码;
        /// <summary>
        /// 0--操作员，1--审核员，2--管理员
        /// </summary>
        int userState;
        /// <summary>
        /// 0：未保存；1：待审核；2：审核通过；3：审核未通过
        /// </summary>
        int formState;

        OleDbDataAdapter daOuter, daInner;
        OleDbCommandBuilder cbOuter, cbInner;
        BindingSource bsOuter, bsInner;
        DataTable dtOuter, dtInner;

        String strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";

        OleDbConnection conn;

        public 物资验收记录()
        {
            // TODO 审核人不走这条路
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();
            
            readOuterData();
            outerBind();
            if (dtOuter.Rows.Count == 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefaultValue(dr);
                dtOuter.Rows.Add(dr);

                daOuter.Update((DataTable)bsOuter.DataSource);
                OleDbCommand comm = new OleDbCommand();
                
                comm.Connection = conn;
                comm.CommandText = "select @@identity";
                Int32 idd1 = (Int32)comm.ExecuteScalar();
                readOuterData(idd1);
                outerBind();
            }

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            addComputerEvnetHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
            
        }


        public 物资验收记录(int id)
        {
            InitializeComponent();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData(id);
            outerBind();
            

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            addComputerEvnetHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEventHandler();
        }
        //getPeople()--> setUserState()--> getOtherData()-->
        // 读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->
        //addComputerEventHandler()--> setFormState()--> setEnableReadOnly() --> 
        //addOtherEvnetHandler()

        void getPeople()
        {
            // TODO
            ls审核员 = new List<string>();
            ls验收人 = new List<string>();
        }

        void setUserState()
        {
            // TODO
            userState = 0;
        }

        void getOtherData()
        {
            // TODO
            ls物资厂家代码 = new List<string>();
            ls物资厂家代码.Add("厂家1");
        }

        void readOuterData(int id = -1)
        {
            conn = new OleDbConnection(strConn);

            conn.Open();
            if (-1 == id)
            {
                daOuter = new OleDbDataAdapter("select * from 物资验收记录 where 验收记录编号='" + lbl验收记录编号.Text + "'", conn);
            }
            else
            {
                daOuter = new OleDbDataAdapter("select * from 物资验收记录 where ID=" + id, conn);
            }

            cbOuter = new OleDbCommandBuilder(daOuter);
            bsOuter = new BindingSource();
            dtOuter = new DataTable("物资验收记录");
            daOuter.Fill(dtOuter);

            
            
        }

        DataRow writeOuterDefaultValue(DataRow dr)
        {
            dr["接收时间"] = DateTime.Now;
            dr["验收人"] = mySystem.Parameter.userName;
            dr["请验时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["验收记录编号"] = create验收记录编号();
            return dr;
        }

        String create验收记录编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 验收记录编号 from 物资验收记录 order by ID DESC", conn);
            DataTable dt = new DataTable("物资验收记录");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count==0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            else
            {
                string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
                string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
                if (Int32.Parse(yearInCode) == yearNow)
                {
                    codeNow = Int32.Parse(NOInCode) + 1;
                }
                else
                {
                    codeNow = 1;
                }
                return yearNow.ToString() + codeNow.ToString("D4");
            }
        }

        void outerBind()
        {

            bsOuter.DataSource = dtOuter;

            cmb物资厂家代码.DataBindings.Clear();
            dtp接收时间.DataBindings.Clear();
            tb验收人.DataBindings.Clear();


            cmb厂家随附检验报告.DataBindings.Clear();

            tb检验报告理由.DataBindings.Clear();

            cmb是否有样品.DataBindings.Clear();

            tb样品理由.DataBindings.Clear();

            tb请验人.DataBindings.Clear();
            dtp请验时间.DataBindings.Clear();

            tb审核员.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            lbl验收记录编号.DataBindings.Clear();
            
            // ----
            cmb物资厂家代码.Items.Clear();
            foreach (String s in ls物资厂家代码)
            {
                cmb物资厂家代码.Items.Add(s);
            }

            cmb物资厂家代码.DataBindings.Add("SelectedItem", bsOuter.DataSource, "物资厂家代码");
            dtp接收时间.DataBindings.Add("Value", bsOuter.DataSource, "接收时间");
            tb验收人.DataBindings.Add("Text", bsOuter.DataSource, "验收人");

            cmb厂家随附检验报告.Items.Clear();
            cmb厂家随附检验报告.Items.Add("无");
            cmb厂家随附检验报告.Items.Add("不齐全");
            cmb厂家随附检验报告.Items.Add("齐全");
            cmb厂家随附检验报告.DataBindings.Add("SelectedItem", bsOuter.DataSource, "厂家随附检验报告");

            tb检验报告理由.DataBindings.Add("Text", bsOuter.DataSource, "无检验报告理由");

            cmb是否有样品.Items.Clear();
            cmb是否有样品.Items.Add("有");
            cmb是否有样品.Items.Add("无");
            cmb是否有样品.DataBindings.Add("SelectedItem", bsOuter.DataSource, "是否有样品");

            tb样品理由.DataBindings.Add("Text", bsOuter.DataSource, "无样品理由");

            tb请验人.DataBindings.Add("Text", bsOuter.DataSource, "请验人");
            dtp请验时间.DataBindings.Add("Value", bsOuter.DataSource, "请验时间");

            tb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");

            lbl验收记录编号.DataBindings.Add("Text", bsOuter.DataSource, "验收记录编号");
            
        }

        void readInnerData(int id)
        {

            daInner = new OleDbDataAdapter("select * from 物资验收记录详细信息 where 物资验收记录ID=" + id, strConn);
            cbInner = new OleDbCommandBuilder(daInner);
            bsInner = new BindingSource();
            dtInner = new DataTable("物资验收记录详细信息");
            daInner.Fill(dtInner);
        }

        DataRow writeInnerDefaultValue(DataRow dr)
        {
            dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            dr["数量"] = 0;
            dr["是否需要检查"] = "否";
            return dr;
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        void addComputerEvnetHandler()
        {

        }

        void setFormState()
        {
            if (dtOuter.Rows[0]["审核员"].ToString() == "")
            {
                formState = 0;
            }
            else if (dtOuter.Rows[0]["审核员"].ToString() == "__待审核")
            {
                formState = 1;
            }
            else
            {
                if (Convert.ToBoolean(dtOuter.Rows[0]["审核意见"]))
                {
                    formState = 2;
                }
                else
                {
                    formState = 3;
                }
            }
            
        }

        void setEnableReadOnly()
        {
            if (2 == userState)
            {
                setControlTrue();
            }
            if (1 == userState)
            {
                if (0 == formState || 2 == formState || 3 == formState)
                {
                    setControlFalse();
                }
                else
                {
                    setControlTrue();
                    btn审核.Enabled = true;
                }
            }
            if (0 == userState)
            {
                if (0 == formState || 3 == formState)
                {
                    setControlTrue();
                }
                if (1 == formState || 2 == formState)
                {
                    setControlFalse();
                }
            }
        }

        void addOtherEventHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
        }

        void setControlTrue()
        {
            // textbox,datagridview
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
            btn审核.Enabled = false;
            btn提交审核.Enabled = false;
        }

        void setControlFalse()
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
        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            outerBind();


            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            btn提交审核.Enabled = true;
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            dtOuter.Rows[0]["审核员"] = "__待审核";
            formState = 1;
            btn提交审核.Enabled = false;

            string log = "=============================\n";
            log += DateTime.Now.ToShortDateString();
            log += " 验收人：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            setControlFalse();

            // TODO 判断，然后决定是新建 请验单 还是  检查记录
            
            bool isAllOK = true;
            List<Int32> RowToCheck = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (dataGridView1.Rows[i].Cells["是否需要检查"].Value.ToString() == "是")
                {
                    isAllOK = false;
                    RowToCheck.Add(i);
                }
            }
            // 开请验单
            if (isAllOK)
            {
                create请验单();
            }
            // 开检查记录
            else
            {
                foreach (int r in RowToCheck)
                {
                    OleDbDataAdapter da = new OleDbDataAdapter("select * from 检查记录 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"]+" and 产品名称='"+dtInner.Rows[r]["产品名称"]+"'", conn);
                    DataTable dt = new DataTable("检查记录");
                    OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                    BindingSource bs = new BindingSource();
                    da.Fill(dt);
                    DataRow dr = dt.NewRow();
                    dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
                    dr["产品名称"] = dtInner.Rows[r]["产品名称"];
                    dr["产品批号"] = dtInner.Rows[r]["本厂批号"];
                    dr["数量"] = dtInner.Rows[r]["数量"];
                    dr["检验日期"] = DateTime.Now;
                    dr["审核日期"] = DateTime.Now;
                    dr["产品代码"] = dtInner.Rows[r]["产品代码"];
                    dr["检验结论"] = "合格";
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
                MessageBox.Show("已自动生产" + RowToCheck.Count + "张检查记录");
            }

          
            
        }

        public void create请验单()
        {
            if (dtOuter.Rows[0]["审核人"].ToString() == "")
            {
                return;
            }
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);
            DataTable dt = new DataTable("物资请验单");
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            BindingSource bs = new BindingSource();
            da.Fill(dt);

            // 填写值
            // Outer
            DataRow dr = dt.NewRow();
            dr["物资厂家代码"] = dtOuter.Rows[0]["物资厂家代码"];
            dr["请验时间"] = DateTime.Now;
            dr["审核时间"] = DateTime.Now;
            dr["请验人"] = mySystem.Parameter.userName;
            dr["请验编号"] = create请验编号();
            dr["物资验收记录ID"] = dtOuter.Rows[0]["ID"];
            dt.Rows.Add(dr);

            da.Update(dt);
            da = new OleDbDataAdapter("select * from 物资请验单 where 物资验收记录ID=" + dtOuter.Rows[0]["ID"], conn);

            dt = new DataTable("物资请验单");
            da.Fill(dt);
            // Inner
            da = new OleDbDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + dt.Rows[0]["ID"], conn);
            DataTable dtMore = new DataTable("物资请验单详细信息");
            cb = new OleDbCommandBuilder(da);
            da.Fill(dtMore);
            for (int i = 0; i < dtInner.Rows.Count; ++i)
            {
                DataRow ndr = dtMore.NewRow();
                // 注意ID的值
                for (int j = 2; j < dtInner.Rows[i].ItemArray.Length - 1; ++j)
                {
                    ndr[j] = dtInner.Rows[i][j];
                }
                ndr[1] = dt.Rows[0]["ID"];
                ndr[ndr.ItemArray.Length - 1] = true;
                dtMore.Rows.Add(ndr);
            }
            da.Update(dtMore);
            MessageBox.Show("已自动生产物资请验单！");
        }

        private string create请验编号()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select top 1 请验编号 from 物资请验单 order by ID DESC", conn);
            DataTable dt = new DataTable("物资请验单");
            da.Fill(dt);
            int yearNow = DateTime.Now.Year;
            int codeNow;
            if (dt.Rows.Count == 0)
            {
                codeNow = 1;
                return yearNow.ToString() + codeNow.ToString("D4");
            }
            string yearInCode = dt.Rows[0][0].ToString().Substring(0, 4);
            string NOInCode = dt.Rows[0][0].ToString().Substring(4, 4);
            if (Int32.Parse(yearInCode) == yearNow)
            {
                codeNow = Int32.Parse(NOInCode) + 1;
            }
            else
            {
                codeNow = 1;
            }
            return yearNow.ToString() + codeNow.ToString("D4");

        }

        private void btn查看日志_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtOuter.Rows[0]["日志"].ToString());
        }

        private void btn审核_Click(object sender, EventArgs e)
        {

        }

        private void btn添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefaultValue(dr);
            dtInner.Rows.Add(dr);
        }

        private void 增加物资验收记录_Load(object sender, EventArgs e)
        {
            
            
        }


    }
}
