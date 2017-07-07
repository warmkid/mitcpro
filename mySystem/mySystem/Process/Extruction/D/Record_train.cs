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
        private DataTable teacher;//讲师
        private OleDbDataAdapter da_teacher;
        private BindingSource bs_teacher;
        private OleDbCommandBuilder cb_teacher;

        List<string> pro=null;
        List<string> cont = null;
        public Record_train(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
            init();
            bind();
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
            //
            connOle = mySystem.Parameter.connOle;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DataError += dataGridView2_DataError;

        }

        //绑定连接数据库的表，相当于更新
        private void bind2(int id)
        {
            teacher.Dispose();
            bs_teacher.Dispose();
            da_teacher.Dispose();
            cb_teacher.Dispose();

            teacher = new DataTable("teacher");
            bs_teacher = new BindingSource();
            da_teacher = new OleDbDataAdapter("select * from safety_training_record where id="+id, connOle);
            cb_teacher = new OleDbCommandBuilder(da_teacher);
            da_teacher.Fill(teacher);

            //DataTable到BindingSource的绑定
            bs_teacher.DataSource = teacher;

            //BindingSource到控件的绑定
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();
            textBox2.DataBindings.Add("Text", bs_teacher.DataSource, "teacher");
            textBox3.DataBindings.Add("Text", bs_teacher.DataSource, "location");
            dateTimePicker1.DataBindings.Add("Value", bs_teacher.DataSource, "training_date");
        }

        //初次绑定连接数据库的表，相当于插入
        private void bind()
        {
            teacher = new DataTable("teacher");
            bs_teacher = new BindingSource();
            da_teacher = new OleDbDataAdapter("select * from safety_training_record where 1=2", connOle);
            cb_teacher = new OleDbCommandBuilder(da_teacher);
            da_teacher.Fill(teacher);


            DataRow dr=teacher.NewRow();
            dr[1] = dr[2] = DateTime.Now;
            dr[3] = "";
            dr[4] = "";
            dr[5] = DateTime.Now;
            dr[6] = 0;
            dr[7] = true;
            teacher.Rows.Add(dr);
           
            //DataTable到BindingSource的绑定
            bs_teacher.DataSource = teacher;

            //BindingSource到控件的绑定
            textBox2.DataBindings.Add("Text",bs_teacher.DataSource,"teacher");
            textBox3.DataBindings.Add("Text", bs_teacher.DataSource, "location");
            dateTimePicker1.DataBindings.Add("Value", bs_teacher.DataSource, "training_date");

        }
        void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        private int getid()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";

            return (int)comm.ExecuteScalar();


        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (index == 0)//第一次点击
            {
                //保存教师
                bs_teacher.EndEdit();
                da_teacher.Update((DataTable)bs_teacher.DataSource);
                MessageBox.Show("添加成功");
                int id = getid();
                if(id!=-1)
                    bind2(id);
                index = 1;
            }
            else
            {
                bs_teacher.EndEdit();
                da_teacher.Update((DataTable)bs_teacher.DataSource);
                MessageBox.Show("更新成功");
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
