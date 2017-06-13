using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Record_extrusSupply : Form
    {
        string product_code;//产品代码
        string product_num;//产品批号
        string product_instrnum;//生产指令编号

        string bunker1;//料仓外中
        string bunker1_code;//
        string bunker1_batch;// 1 批号
        string bunker2;//料仓中层
        string bunker2_code;//
        string bunker2_batch;//2 批号

        string date;//供料日期
        float serve_out;//外层供料量
        float serve_mid;//中层供料量
        float serve_in;//中内层供料量
        string checkout;
        string server;//供料人

        string bunker_use;//物料使用料仓
        float use;//物料使用量
        float left;//余料
        string checker;//复核人

        private void Init()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            product_code = "rw2cws3";
            product_num = "rw2cws3";
            product_instrnum = "rw2cws3";

            bunker1 = "AB1C";
            bunker1_code = "rw2cws3";
            bunker1_batch = "rw2cws3";
            bunker2 = "B2";
            bunker2_code = "rw2cws3";
            bunker2_batch = "rw2cws3";

            //string date;//供料日期

        }

        private void Setup()
        {
            textBox1.Text = product_code;
            textBox2.Text = product_num;
            textBox3.Text = product_instrnum;
            textBox4.Text = bunker1;
            textBox5.Text = bunker2;
            textBox6.Text = bunker1_code;
            textBox7.Text = bunker2_code;
            textBox8.Text = bunker1_batch;
            textBox9.Text = bunker2_batch;

            //textBox1.ReadOnly = true;
        }

        public Record_extrusSupply()
        {
            InitializeComponent();
            Init();
            Setup();
        }

        private void splitContainer11_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
 

        private void button1_Click(object sender, EventArgs e)
        {   
            float temp;
            //if(dateTimePicker1.Text==""||!float.TryParse(Serve_out.Text,out temp)||)

            checkout = Serve_checkresult.Text.ToString();
            server = Serve_person.Text.ToString();
            if (checkout == "" || server == "")
            {
                MessageBox.Show("原料抽查结果、供料人均不能为空");
                return;
            }

            date = dateTimePicker1.Text.ToString();

            if (!float.TryParse(Serve_out.Text, out temp) || !float.TryParse(Serve_in.Text, out temp) || !float.TryParse(Serve_mid.Text, out temp))
            {
                MessageBox.Show("供料量必须为数值类型");
                return;  
            }
            serve_out = (float)Convert.ToSingle(Serve_out.Text.ToString());
            serve_in = (float)Convert.ToSingle(Serve_in.Text.ToString());
            serve_mid = (float)Convert.ToSingle(Serve_mid.Text.ToString());



            //填数据
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = date;
            dr.Cells[1].Value = serve_out;
            dr.Cells[2].Value = serve_in;
            dr.Cells[3].Value = serve_mid;
            dr.Cells[4].Value = checkout;
            dr.Cells[5].Value = server;
            dataGridView1.Rows.Add(dr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            bunker_use = comboBox1.Text.ToString();
            checker = textBox10.Text.ToString();
            if (bunker_use == "" || checker == "")
            {
                MessageBox.Show("料仓、复核人均不能为空");
                return;
            }

            float temp;
            if (!float.TryParse(textBox11.Text, out temp) || !float.TryParse(textBox12.Text, out temp))
            {
                MessageBox.Show("用料、余料必须为数值类型");
                return;
            }
            use = (float)Convert.ToSingle(textBox11.Text.ToString());
            left = (float)Convert.ToSingle(textBox12.Text.ToString());
            

            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView2.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = bunker_use;
            dr.Cells[1].Value = use;
            dr.Cells[2].Value = left;
            dr.Cells[3].Value = checker;

            dataGridView2.Rows.Add(dr);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count==0)
                return;
            if (dataGridView1.SelectedRows[0].Index < dataGridView1.Rows.Count - 1 )
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count== 0)
                return;
            if (dataGridView2.SelectedRows[0].Index < dataGridView2.Rows.Count - 1 )
                dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
        }

        private void Serve_out_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
}
