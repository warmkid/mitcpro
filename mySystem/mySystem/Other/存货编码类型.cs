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
    public partial class 存货编码类型 : Form
    {
        string _data;

        public 存货编码类型()
        {
            InitializeComponent();
            initData();
        }

        void initData()
        {
            int index;

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "组件";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "半成品";

            index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = false;
            this.dataGridView1.Rows[index].Cells[1].Value = "成品";

           
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
            存货编码类型 form = new 存货编码类型();
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
