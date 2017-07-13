using mySystem.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1;


namespace mySystem
{
    public partial class SetExtruForm : BaseForm
    {
        Setting_CleanArea setcleanDlg = null;
        Setting_CheckBeforePower bfPowerDlg = null;
        PreheatParameterForm preheatDlg = null;
        Setting_CleanSite setsiteDlg = null;
        SettingHandOver handoverDlg = null;
        private bool isSqlOk;
        int para1 = 0;
        double para2 = 0;
        double para3 = 0;
        double para4 = 0;

        public SetExtruForm(MainForm mainform):base(mainform)
        {
            isSqlOk = Parameter.isSqlOk;
            InitializeComponent();
            Init();
            InitParameter();
            Initdgv();
            Bind();
        }

        //加载panel
        private void Init()
        {
            //清洁区域设置
            setcleanDlg = new Setting_CleanArea(base.mainform);
            setcleanDlg.TopLevel = false;
            setcleanDlg.FormBorderStyle = FormBorderStyle.None;
            cleanPanel.Controls.Add(setcleanDlg);
            setcleanDlg.Show();

            //开机前确认项目设置
            bfPowerDlg = new Setting_CheckBeforePower(base.mainform);
            bfPowerDlg.TopLevel = false;
            bfPowerDlg.FormBorderStyle = FormBorderStyle.None;
            bfStartPanel.Controls.Add(bfPowerDlg);
            bfPowerDlg.Show();

            //预热参数设置
            preheatDlg = new PreheatParameterForm(base.mainform);
            preheatDlg.TopLevel = false;
            preheatDlg.FormBorderStyle = FormBorderStyle.None;
            preHeatPanel.Controls.Add(preheatDlg);
            preheatDlg.Show();

            //清场设置
            setsiteDlg = new Setting_CleanSite(base.mainform);
            setsiteDlg.TopLevel = false;
            setsiteDlg.FormBorderStyle = FormBorderStyle.None;
            procClearPanel.Controls.Add(setsiteDlg);
            setsiteDlg.Show();

            //交接班设置
            handoverDlg = new SettingHandOver(base.mainform);
            handoverDlg.TopLevel = false;
            handoverDlg.FormBorderStyle = FormBorderStyle.None;
            this.handoverPanel.Controls.Add(handoverDlg);
            handoverDlg.Show();

 
        }

        //系数设置部分
        private void InitParameter()
        { 
            //读取数据库并显示
            String tblName = "设置生产指令参数";
            List<String> readqueryCols = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(Parameter.connOle, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);

            List<String> data = new List<String> { };
            for (int i = 0; i < queryValsList[0].Count; i++)
            {
                data.Add(queryValsList[0][i].ToString());
            }
            List<Control> textboxes = new List<Control> { tB面数, tB厚度密度, tB参数1, tB参数2 };
            Utility.fillControl(textboxes, data);

        }

        private void ParaSave()
        {
            para1 = Convert.ToInt32(tB面数.Text.Trim());
            para2 = Convert.ToDouble(tB厚度密度.Text.Trim());
            para3 = Convert.ToDouble(tB参数1.Text.Trim());
            para4 = Convert.ToDouble(tB参数2.Text.Trim());
            String tblName = "设置生产指令参数";
            List<String> updateCols = new List<String>(new String[] { "面", "密度", "系数1", "系数2" });
            List<Object> updateVals = new List<Object>(new Object[] { para1, para2, para3, para4 });
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(Parameter.connOle, tblName, updateCols, updateVals, whereCols, whereVals);
            if (b)
            {
                return;
            }
            else
            {
                MessageBox.Show("参数保存失败", "错误");
                return;
            }

        }


        //********************************四个dgv设置**********************************
        private OleDbDataAdapter da产品;
        private DataTable dt产品;
        private BindingSource bs产品;
        private OleDbCommandBuilder cb产品;
        private OleDbDataAdapter da产品编码;
        private DataTable dt产品编码;
        private BindingSource bs产品编码;
        private OleDbCommandBuilder cb产品编码;
        private OleDbDataAdapter da物料代码;
        private DataTable dt物料代码;
        private BindingSource bs物料代码;
        private OleDbCommandBuilder cb物料代码;
        private OleDbDataAdapter da工艺;
        private DataTable dt工艺;
        private BindingSource bs工艺;
        private OleDbCommandBuilder cb工艺;
        //dgv样式初始化
        private void Initdgv()
        {
            bs产品 = new BindingSource();
            dgv产品.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品.AllowUserToAddRows = false;
            dgv产品.ReadOnly = false;
            dgv产品.RowHeadersVisible = false;
            dgv产品.AllowUserToResizeColumns = true;
            dgv产品.AllowUserToResizeRows = false;
            dgv产品.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品.Font = new Font("宋体", 12);

            bs产品编码 = new BindingSource();
            dgv产品编码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv产品编码.AllowUserToAddRows = false;
            dgv产品编码.ReadOnly = false;
            dgv产品编码.RowHeadersVisible = false;
            dgv产品编码.AllowUserToResizeColumns = true;
            dgv产品编码.AllowUserToResizeRows = false;
            dgv产品编码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv产品编码.Font = new Font("宋体", 12);

            bs物料代码 = new BindingSource();
            dgv物料代码.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv物料代码.AllowUserToAddRows = false;
            dgv物料代码.ReadOnly = false;
            dgv物料代码.RowHeadersVisible = false;
            dgv物料代码.AllowUserToResizeColumns = true;
            dgv物料代码.AllowUserToResizeRows = false;
            dgv物料代码.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv物料代码.Font = new Font("宋体", 12);

            bs工艺 = new BindingSource();
            dgv工艺.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv工艺.AllowUserToAddRows = false;
            dgv工艺.ReadOnly = false;
            dgv工艺.RowHeadersVisible = false;
            dgv工艺.AllowUserToResizeColumns = true;
            dgv工艺.AllowUserToResizeRows = false;
            dgv工艺.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv工艺.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv工艺.Font = new Font("宋体", 12);

        }

        private void Bind()
        {
            //************************    产品     *******************************************
            dt产品 = new DataTable("设置吹膜产品"); //""中的是表名
            da产品 = new OleDbDataAdapter("select * from 设置吹膜产品", mySystem.Parameter.connOle);
            cb产品 = new OleDbCommandBuilder(da产品);
            dt产品.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品.Fill(dt产品);
            bs产品.DataSource = dt产品;
            this.dgv产品.DataSource = bs产品.DataSource;
            //显示序号
            numFresh(this.dgv产品);
            this.dgv产品.Columns["产品名称"].MinimumWidth = 150;
            this.dgv产品.Columns["产品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品.Columns["产品名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品.Columns["ID"].Visible = false;

            //**************************   产品编码    ***********************************
            dt产品编码 = new DataTable("设置吹膜产品编码"); //""中的是表名
            da产品编码 = new OleDbDataAdapter("select * from 设置吹膜产品编码", mySystem.Parameter.connOle);
            cb产品编码 = new OleDbCommandBuilder(da产品编码);
            dt产品编码.Columns.Add("序号", System.Type.GetType("System.String"));
            da产品编码.Fill(dt产品编码);
            bs产品编码.DataSource = dt产品编码;
            this.dgv产品编码.DataSource = bs产品编码.DataSource;
            //显示序号
            numFresh(this.dgv产品编码);
            this.dgv产品编码.Columns["产品编码"].MinimumWidth = 150;
            this.dgv产品编码.Columns["产品编码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv产品编码.Columns["产品编码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv产品编码.Columns["ID"].Visible = false;

            //************************    物料代码     *******************************************
            dt物料代码 = new DataTable("设置物料代码"); //""中的是表名
            da物料代码 = new OleDbDataAdapter("select * from 设置物料代码", mySystem.Parameter.connOle);
            cb物料代码 = new OleDbCommandBuilder(da物料代码);
            dt物料代码.Columns.Add("序号", System.Type.GetType("System.String"));
            da物料代码.Fill(dt物料代码);
            bs物料代码.DataSource = dt物料代码;
            this.dgv物料代码.DataSource = bs物料代码.DataSource;
            //显示序号
            numFresh(this.dgv物料代码);
            this.dgv物料代码.Columns["物料代码"].MinimumWidth = 150;
            this.dgv物料代码.Columns["物料代码"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv物料代码.Columns["物料代码"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv物料代码.Columns["ID"].Visible = false;

            //**************************   工艺    ***********************************
            dt工艺 = new DataTable("设置吹膜工艺"); //""中的是表名
            da工艺 = new OleDbDataAdapter("select * from 设置吹膜工艺", mySystem.Parameter.connOle);
            cb工艺 = new OleDbCommandBuilder(da工艺);
            dt工艺.Columns.Add("序号", System.Type.GetType("System.String"));
            da工艺.Fill(dt工艺);
            bs工艺.DataSource = dt工艺;
            this.dgv工艺.DataSource = bs工艺.DataSource;
            //显示序号
            numFresh(this.dgv工艺);
            this.dgv工艺.Columns["工艺名称"].MinimumWidth = 150;
            this.dgv工艺.Columns["工艺名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgv工艺.Columns["工艺名称"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.dgv工艺.Columns["ID"].Visible = false;

        }

        private void add产品_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品.NewRow();
            dt产品.Rows.InsertAt(dt产品.NewRow(), dt产品.Rows.Count);
            numFresh(this.dgv产品);
        }

        private void del产品_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv产品.CurrentRow.Index;
            this.dgv产品.Rows.RemoveAt(deletenum);
            numFresh(this.dgv产品);
        }

        private void add产品编码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt产品编码.NewRow();
            dt产品编码.Rows.InsertAt(dt产品编码.NewRow(), dt产品编码.Rows.Count);
            numFresh(this.dgv产品编码);
        }

        private void del产品编码_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv产品编码.CurrentRow.Index;
            this.dgv产品编码.Rows.RemoveAt(deletenum);
            numFresh(this.dgv产品编码);
        }

        private void add物料代码_Click(object sender, EventArgs e)
        {
            DataRow dr = dt物料代码.NewRow();
            dt物料代码.Rows.InsertAt(dt物料代码.NewRow(), dt物料代码.Rows.Count);
            numFresh(this.dgv物料代码);
        }

        private void del物料代码_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv物料代码.CurrentRow.Index;
            this.dgv物料代码.Rows.RemoveAt(deletenum);
            numFresh(this.dgv物料代码);
        }

        private void add工艺_Click(object sender, EventArgs e)
        {
            DataRow dr = dt工艺.NewRow();
            dt工艺.Rows.InsertAt(dt工艺.NewRow(), dt工艺.Rows.Count);
            numFresh(this.dgv工艺);
        }

        private void del工艺_Click(object sender, EventArgs e)
        {
            int deletenum = this.dgv工艺.CurrentRow.Index;
            this.dgv工艺.Rows.RemoveAt(deletenum);
            numFresh(this.dgv工艺);
        }


        private void numFresh(DataGridView dgv)
        {
            int coun = dgv.RowCount;
            for (int i = 0; i < coun; i++)
            {
                dgv.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }

        private void dgv4Save()
        {
            if (isSqlOk)
            { }
            else
            {
                da产品.Update((DataTable)bs产品.DataSource);
                dt产品.Clear();
                da产品.Fill(dt产品);
                numFresh(this.dgv产品);

                da产品编码.Update((DataTable)bs产品编码.DataSource);
                dt产品编码.Clear();
                da产品编码.Fill(dt产品编码);
                numFresh(this.dgv产品编码);

                da物料代码.Update((DataTable)bs物料代码.DataSource);
                dt物料代码.Clear();
                da物料代码.Fill(dt物料代码);
                numFresh(this.dgv物料代码);

                da工艺.Update((DataTable)bs工艺.DataSource);
                dt工艺.Clear();
                da工艺.Fill(dt工艺);
                numFresh(this.dgv工艺);
            }
        }


        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parameter.isSqlOk)
                {

                }
                else
                {
                    bfPowerDlg.DataSave();
                    preheatDlg.DataSave();
                    setsiteDlg.DataSave();
                    setcleanDlg.DataSave();
                    handoverDlg.DataSave();
                    ParaSave();
                    dgv4Save();
                }
                MessageBox.Show("保存成功！");
            }
            catch
            { MessageBox.Show("保存失败！"); }
        }
        

    }
}
