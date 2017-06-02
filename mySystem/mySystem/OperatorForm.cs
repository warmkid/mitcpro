using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class OperatorForm : Form
    {
        public OperatorForm()
        {
            InitializeComponent();
        }

        private void 操作用户BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.操作用户BindingSource.EndEdit();
            //this.操作用户TableAdapter.Update(this.mySaleDataSet1.操作用户);

        }

        private void OperatorForm_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“mySaleDataSet1.操作用户”中。您可以根据需要移动或移除它。
            //this.操作用户TableAdapter.Fill(this.mySaleDataSet1.操作用户);

        }

    }
}