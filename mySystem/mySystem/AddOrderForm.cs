using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class AddOrderForm : Form
    {
        public AddOrderForm()
        {
            InitializeComponent();
        }
        private void MerchandiseForm_Load(object sender, EventArgs e)
        {
        
        }

        private void 商品信息DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}