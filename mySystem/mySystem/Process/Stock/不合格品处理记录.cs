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
    public partial class 不合格品处理记录 : Form
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

        int _id;


        public 不合格品处理记录(int id)
        {
            InitializeComponent();
            _id = id;
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
            addOtherEventHandler();
        }


        void getPeople()
        {

        }

        void setUserState()
        {

        }

        void getOtherData()
        {

        }

        void readOuterData(int id)
        {
            daOuter = new OleDbDataAdapter("select * from 不合格品处理记录 where ID=" + id, conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("不合格品处理记录");
            bsOuter = new BindingSource();

            daOuter.Fill(dtOuter);
        }

        void outerBind()
        {
            bsOuter.DataSource = dtOuter;

           
            // 循环绑定和解绑
            // tb=textbox , lbl=label, cmb=combobox,dtp=datetimepicker
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("tb"))
                {
                    (c as TextBox).DataBindings.Clear();
                    (c as TextBox).DataBindings.Add("Text", bsOuter.DataSource, c.Name.Substring(2));
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

        void readInnerData(int id)
        {

        }

        void innerBind()
        {

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
                    //btn审核.Enabled = true;
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
            //btn审核.Enabled = false;
            //btn提交审核.Enabled = false;
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
            // btn查看日志.Enabled = true;
        }

        void addOtherEventHandler()
        {

        }

        private void btn保存_Click(object sender, EventArgs e)
        {
            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(_id);
            outerBind();
        }
    }
}
