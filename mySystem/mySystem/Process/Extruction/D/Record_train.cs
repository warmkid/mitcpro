using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace mySystem.Extruction.Process
{
    public partial class Record_train : mySystem.BaseForm
    {
        static int index = 0;

        OleDbConnection connOle;
        private DataTable dt_out,dt_in;
        private OleDbDataAdapter da_out,da_in;
        private BindingSource bs_out,bs_in;
        private OleDbCommandBuilder cb_out,cb_in;

        List<string> pro=null;
        List<string> cont = null;
        public Record_train(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            init();

            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }
            tb讲师.Enabled = true;
            tb培训地点.Enabled = true;
            dtp培训日期.Enabled = true;
            bt查询插入.Enabled = true;
        }
        private void init()
        {
            //填表格
            pro = new List<string>();
            cont = new List<string>();
            pro.Add("急停开关");
            pro.Add("推料安全");
            pro.Add("搬运安全");
            pro.Add("高处操作安全");
            pro.Add("吹膜机组高温区域安全");
            pro.Add("吹膜机组切刀安全");
            pro.Add("吹膜机组卸卷安全");
            pro.Add("吹膜机组引膜安全");
            pro.Add("美工刀的使用安全");
            pro.Add("用电安全");
            pro.Add("叉车使用安全");
            pro.Add("设备安全");

            cont.Add("设备上的急停开关位置确认，使用方法。");
            cont.Add("推料时注意推料速度，防止撞伤他人。注意叉车是否放好，料包是否歪斜，防止歪倒砸伤他人和自己。");
            cont.Add("在搬运膜卷和原料时注意人员之间的配合，防治伤到他人和自己。");
            cont.Add("在高处操作时要扶稳、抓牢防止摔伤，必要时佩戴安全带。");
            cont.Add("高温区域：三个挤出机和机头为高温区域，在换网、锁厚度的时候要做好防护措施，戴线手套或高温手套。");
            cont.Add("在更换刀片或清洁设备时要注意二夹上方的切刀，防止被割伤。");
            cont.Add("卸卷时叉车要放正才可以把膜卷叉起，防止膜卷掉落。开叉车时要注意前后左右，防止碰伤他人。抬气胀轴时要两人配合防止砸伤。");
            cont.Add("引膜时不能戴手套，穿过二夹和收卷时要确认后才能操作，防止夹伤。");
            cont.Add("要注意用刀方法和用完刀及时把刀片收回。主要注意时间有：供料用刀、引膜用刀、测厚用刀及包装用刀。");
            cont.Add("所有电气设备未经允许不得操作，所有插座线板要远离潮湿有水的地方。插销要插牢，防止发虚打火。");
            cont.Add("使用叉车人员必须要经过《叉车使用安全》培训后，才可以进行操作。操作时要注意人员安全、叉车高度、宽度、速度和叉车上的物品是否稳定牢固。");
            cont.Add("所有设备未经允许不得操作，防止发生人员伤害或损坏设备。");

            for (int i = 0; i < pro.Count; i++)
            {
                DataGridViewRow dr = new DataGridViewRow();
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
                }
                dr.Cells[0].Value = pro[i];
                dr.Cells[1].Value = cont[i];
                dataGridView1.Rows.Add(dr);
            }
            
            dt_out = new DataTable();
            da_out = new OleDbDataAdapter();
            bs_out = new BindingSource();
            cb_out = new OleDbCommandBuilder();

            dt_in = new DataTable();
            da_in = new OleDbDataAdapter();
            bs_in = new BindingSource();
            cb_in = new OleDbCommandBuilder();

            connOle = mySystem.Parameter.connOle;
            dataGridView1.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DataError += dataGridView2_DataError;

        }

        void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }     

        // 给外表的一行写入默认值
        DataRow writeOuterDefault(DataRow dr)
        {
            dr["讲师"] = tb讲师.Text;
            dr["培训地点"] = tb培训地点.Text;
            dr["培训日期"] = DateTime.Parse(dtp培训日期.Value.ToShortDateString());
            dr["备注"] = "";
            return dr;

        }
        // 给内表的一行写入默认值
        DataRow writeInnerDefault(DataRow dr)
        {
            dr["T吹膜机安全培训记录ID"] = dt_out.Rows[0]["ID"];
            dr["序号"] = 0;
            dr["是否需要参加"] = true;
            return dr;
        }
        // 根据条件从数据库中读取一行外表的数据
        void readOuterData(string tcher,string loc,DateTime time)//注意日期具体到天
        {

            dt_out = new DataTable("吹膜机安全培训记录");
            bs_out = new BindingSource();
            da_out = new OleDbDataAdapter("select * from 吹膜机安全培训记录 where 讲师='" + tcher + "' and 培训地点='" + loc + "' and 培训日期=#" + time + "#", connOle);
            cb_out = new OleDbCommandBuilder(da_out);
            da_out.Fill(dt_out);
        }
        // 根据条件从数据库中读取多行内表数据
        void readInnerData(int id)
        {
            dt_in = new DataTable("吹膜机安全培训记录人员情况");
            bs_in = new BindingSource();
            da_in = new OleDbDataAdapter("select * from 吹膜机安全培训记录人员情况 where T吹膜机安全培训记录ID=" + id, connOle);
            cb_in = new OleDbCommandBuilder(da_in);
            da_in.Fill(dt_in);
        }
        // 移除外表和控件的绑定，建议使用Control.DataBinds.RemoveAt(0)
        void removeOuterBinding()
        {
            //解除之前的绑定
            tb讲师.DataBindings.Clear();
            tb培训地点.DataBindings.Clear();
            tb备注.DataBindings.Clear();
            dtp培训日期.DataBindings.Clear();

        }
        // 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
        void removeInnerBinding()
        { }
        // 外表和控件的绑定
        void outerBind()
        {
            bs_out.DataSource = dt_out;
            tb讲师.DataBindings.Add("Text",bs_out.DataSource,"讲师");
            tb培训地点.DataBindings.Add("Text", bs_out.DataSource, "培训地点");
            tb备注.DataBindings.Add("Text", bs_out.DataSource, "备注");
            dtp培训日期.DataBindings.Add("Value", bs_out.DataSource, "培训日期");
        }
        // 内表和控件的绑定
        void innerBind()
        {
            ////移除所有列
            //while (dataGridView1.Columns.Count > 0)
            //    dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            //setDataGridViewCombox();
            //bs_out.DataSource = dt_out;
            //dataGridView1.DataSource = bs_out.DataSource;
            //setDataGridViewColumns();

            bs_in.DataSource = dt_in;
            dataGridView2.DataSource = bs_in.DataSource;
            setDataGridViewColumns();
        }
        //设置DataGridView中下拉框
        void setDataGridViewCombox()
        {
            //foreach (DataColumn dc in dt_prodlist.Columns)
            //{
            //    switch (dc.ColumnName)
            //    {
            //        case "产品编码":
            //            DataGridViewComboBoxColumn c1 = new DataGridViewComboBoxColumn();
            //            c1.DataPropertyName = dc.ColumnName;
            //            c1.HeaderText = dc.ColumnName;
            //            c1.Name = dc.ColumnName;
            //            c1.SortMode = DataGridViewColumnSortMode.Automatic;
            //            c1.ValueType = dc.DataType;
            //            // 如果换了名字会报错，把当前值也加上就好了
            //            // 加序号，按序号显示
            //            OleDbDataAdapter tda = new OleDbDataAdapter("select 产品编码 from 设置吹膜产品编码", mySystem.Parameter.connOle);
            //            DataTable tdt = new DataTable("产品编码");
            //            tda.Fill(tdt);
            //            foreach (DataRow tdr in tdt.Rows)
            //            {
            //                c1.Items.Add(tdr["产品编码"]);
            //            }
            //            dataGridView1.Columns.Add(c1);
            //            // 重写cell value changed 事件，自动填写id
            //            break;

            //        default:
            //            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            //            c2.DataPropertyName = dc.ColumnName;
            //            c2.HeaderText = dc.ColumnName;
            //            c2.Name = dc.ColumnName;
            //            c2.SortMode = DataGridViewColumnSortMode.Automatic;
            //            c2.ValueType = dc.DataType;
            //            dataGridView1.Columns.Add(c2);
            //            break;
            //    }
            //}
        }
        // 设置DataGridView中各列的格式
        void setDataGridViewColumns()
        {
            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["T吹膜机安全培训记录ID"].Visible = false;
        }
        //设置datagridview序号
        void setDataGridViewRowNums()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells["序号"].Value = i + 1;
            }
        }

        private void bt保存_Click(object sender, EventArgs e)
        {
            //外表保存
            bs_out.EndEdit();
            da_out.Update((DataTable)bs_out.DataSource);
            readOuterData(tb讲师.Text, tb培训地点.Text, DateTime.Parse(dtp培训日期.Value.ToShortDateString()));
            removeOuterBinding();
            outerBind();

            //内表保存
            da_in.Update((DataTable)bs_in.DataSource);
            readInnerData(Convert.ToInt32(dt_out.Rows[0]["ID"]));
            innerBind();
        }

        private void bt审核_Click(object sender, EventArgs e)
        {

        }

        private void bt打印_Click(object sender, EventArgs e)
        {

        }

        //查询/插入按钮
        private void bt查询插入_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            //tb讲师.Enabled = false;
            //tb培训地点.Enabled = false;
            //dtp培训日期.Enabled = false;
            //bt查询插入.Enabled = false;
            //bt审核.Enabled = false;
            //bt打印.Enabled = false;

            readOuterData(tb讲师.Text,tb培训地点.Text,DateTime.Parse(dtp培训日期.Value.ToShortDateString()));
            removeOuterBinding();
            outerBind();
            if (dt_out.Rows.Count <= 0)
            {
                DataRow dr = dt_out.NewRow();
                dr = writeOuterDefault(dr);
                dt_out.Rows.Add(dr);
                da_out.Update((DataTable)bs_out.DataSource);
                readOuterData(tb讲师.Text, tb培训地点.Text, DateTime.Parse(dtp培训日期.Value.ToShortDateString()));
                removeOuterBinding();
                outerBind();
            }

            readInnerData((int)dt_out.Rows[0]["ID"]);
            innerBind();

        }

        private void bt添加_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_in.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr = writeInnerDefault(dr);

            dt_in.Rows.Add(dr);
            setDataGridViewRowNums();
        }

        private void bt删除_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                if (dataGridView2.SelectedRows[0].Index < 0)
                    return;
                dataGridView2.Rows.Remove(dataGridView2.SelectedRows[0]);
            }
        }

        private void bt上移_Click(object sender, EventArgs e)
        {
            int count = dt_in.Rows.Count;
            if (count == 0)
                return;
            int index = dataGridView2.SelectedCells[0].RowIndex;
            if (0 == index)
            {
                return;
            }
            DataRow currRow = dt_in.Rows[index];
            DataRow desRow = dt_in.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_in.Rows.Add(desRow);

            for (int i = index - 1; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_in.Rows[i];
                DataRow tdesRow = dt_in.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_in.Rows.Add(tdesRow);
            }
            da_in.Update((DataTable)bs_in.DataSource);
            dt_in.Clear();
            da_in.Fill(dt_in);
            dataGridView2.ClearSelection();
            dataGridView2.Rows[index - 1].Selected = true;
        }

        private void bt下移_Click(object sender, EventArgs e)
        {
            int count = dt_in.Rows.Count;
            if (count == 0)
                return;
            int index = dataGridView2.SelectedCells[0].RowIndex;
            if (count - 1 == index)
            {
                return;
            }
            DataRow currRow = dt_in.Rows[index];
            DataRow desRow = dt_in.NewRow();
            desRow.ItemArray = currRow.ItemArray.Clone() as object[];
            currRow.Delete();
            dt_in.Rows.Add(desRow);

            for (int i = index + 2; i < count; ++i)
            {
                if (i == index) { continue; }
                DataRow tcurrRow = dt_in.Rows[i];
                DataRow tdesRow = dt_in.NewRow();
                tdesRow.ItemArray = tcurrRow.ItemArray.Clone() as object[];
                tcurrRow.Delete();
                dt_in.Rows.Add(tdesRow);
            }
            da_in.Update((DataTable)bs_in.DataSource);
            dt_in.Clear();
            da_in.Fill(dt_in);
            dataGridView2.ClearSelection();
            dataGridView2.Rows[index + 1].Selected = true;
        }
    }
}
