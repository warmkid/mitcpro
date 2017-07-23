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
    public partial class 物资请验单 : Form
    {
        List<String> ls操作员 = new List<string>();
        List<String> ls审核员 = new List<string>();

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

        public 物资请验单(int id)
        {
            InitializeComponent();
            conn = new OleDbConnection(strConn);
            conn.Open();
            getPeople();
            setUserState();
            getOtherData();

            readOuterData(id);
            outerBind();

            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();

            addComputerEventHandler();
            setFormState();
            setEnableReadOnly();
            addOtherEvnetHandler();
        }

        // //getPeople()--> setUserState()--> getOtherData()-->
        // 读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->
        //addComputerEventHandler()--> setFormState()--> setEnableReadOnly() --> 
        //addOtherEvnetHandler()

        void getPeople()
        {
            // TODO
        }

        void setUserState()
        {
            userState = 0;
        }

        void getOtherData()
        {

        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 物资请验单 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("物资请验单");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

            tb物资厂家代码.DataBindings.Clear();
            dtp请验时间.DataBindings.Clear();
            tb请验人.DataBindings.Clear();
            tb审核员.DataBindings.Clear();
            dtp审核时间.DataBindings.Clear();

            tb物资厂家代码.DataBindings.Add("Text", bsOuter.DataSource, "物资厂家代码");
            dtp请验时间.DataBindings.Add("Value", bsOuter.DataSource, "请验时间");
            tb请验人.DataBindings.Add("Text", bsOuter.DataSource, "请验人");
            tb审核员.DataBindings.Add("Text", bsOuter.DataSource, "审核员");
            dtp审核时间.DataBindings.Add("Value", bsOuter.DataSource, "审核时间");
           
        }

        void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 物资请验单详细信息 where 物资请验单ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("物资请验单详细信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        void addComputerEventHandler()
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
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].ReadOnly = false;
        }

        void addOtherEvnetHandler()
        {
            dataGridView1.AllowUserToAddRows = false;
        }

        void setControlTrue()
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
        }

        private void btn提交审核_Click(object sender, EventArgs e)
        {
            // 日志
            string log = "=============================\n";
            log += DateTime.Now.ToShortDateString();
            log += "请验人：" + mySystem.Parameter.userName + " 提交审核\n";
            dtOuter.Rows[0]["日志"] = dtOuter.Rows[0]["日志"].ToString() + log;
            setControlFalse();

            // 控件
            btn提交审核.Enabled = false;

            // 入库  TODO

        }

    }
}
