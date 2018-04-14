using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Other
{
    public partial class 属于工序 : Form
    {
        string _data;
        public 属于工序()
        {
            InitializeComponent();
            initData();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        public 属于工序(String data)
        {
            InitializeComponent();
            initData(data);
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (null != dgv.CurrentCell)
            {
                int rowIdx = dgv.CurrentCell.RowIndex;
                if(0!=rowIdx)
                    dgv[0, rowIdx].Value = !Convert.ToBoolean(dgv[0, rowIdx].Value);
            }
        }


        void initData()
        {
            int index;

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "吹膜";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "清洁分切";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "灭菌";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "CS制袋";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "PE制袋";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "BPV制袋";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "PTV制袋";
        }

        void initData(String data)
        {
            initData();
            string[] gongxu = data.Split(',');
            foreach (String s in gongxu)
            {
                switch (s)
                {
                    case "吹膜":
                        this.dataGridView1.Rows[0].Cells[0].Value = true;
                        break;
                    case "清洁分切":
                        this.dataGridView1.Rows[1].Cells[0].Value = true;
                        break;
                    case "灭菌":
                        this.dataGridView1.Rows[2].Cells[0].Value = true;
                        break;
                    case "CS制袋":
                        this.dataGridView1.Rows[3].Cells[0].Value = true;
                        break;
                    case "PE制袋":
                        this.dataGridView1.Rows[4].Cells[0].Value = true;
                        break;
                    case "BPV制袋":
                        this.dataGridView1.Rows[5].Cells[0].Value = true;
                        break;
                    case "PTV制袋":
                        this.dataGridView1.Rows[6].Cells[0].Value = true;
                        break;
                }
            }
        }

        private void benDone_Click(object sender, EventArgs e)
        {
            List<String> ls = new List<string>();
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(dgvr.Cells[0].Value))
                {
                    ls.Add(dgvr.Cells[1].Value.ToString());
                }
            }
            _data = String.Join(",", ls.ToArray());
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static string getData()
        {
            属于工序 form = new 属于工序();
            if (DialogResult.OK == form.ShowDialog())
            {
                return form._data;
            }
            else
            {
                return null;
            }
        }

        public static string getData(String data)
        {
            属于工序 form = new 属于工序(data);
            if (DialogResult.OK == form.ShowDialog())
            {
                return form._data;
            }
            else
            {
                return null;
            }
        }

    }
}
