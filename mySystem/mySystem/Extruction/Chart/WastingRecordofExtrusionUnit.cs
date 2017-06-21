using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


//this form is about the 10th picture of the extrusion step 
namespace mySystem.Extruction.Process
{
    public partial class WastingRecordofExtrusionUnit : Form
    {
        public int items = 1;
        private DateTimePicker dtp = new DateTimePicker();
        private DataTable datatab = new DataTable();
        SqlConnection conn = null;

        public WastingRecordofExtrusionUnit(SqlConnection myConnection)
        {
            InitializeComponent();
            conn = myConnection;
            this.Default();
            //int items =1;
            //items = this.ltbShow.Items.Count+1;
            //items = 1;
        }

        private void Default()
        {
            
            this.lbId.Text = Convert.ToString(1);
            this.lbIdtxt.Visible = false;
            this.lbId.Visible = false;
            this.dtpProductDate.Format = DateTimePickerFormat.Short;
            this.txbWorkTurn.Text = "白班";
            this.txbWorkTurn.Visible = false;
            this.lbWorkTurn.Visible = false;
            this.txbProductCode.Text = "000";
            this.txbProductCode.Visible = false;
            this.lbProductCode.Visible = false;
            this.txbWasteWeight.Text = "0";
            this.txbWasteWeight.Visible = false;
            this.lbWasteWeight.Visible = false;
            this.txbReason.Text = "";
            this.txbReason.Visible = false;
            this.lbReason.Visible = false;
            this.txbRecordMan.Text = "";
            this.txbRecordMan.Visible = false;
            this.lbRecordMan.Visible = false;
            this.txbRecheckMan.Text = "";
            this.txbRecheckMan.Visible = false;
            this.lbRecheckMan.Visible = false;
            this.lbProductDate.Visible = false;
            datatab.Columns.Add("序号", typeof(String));
            datatab.Columns.Add("生产日期", typeof(String));
            datatab.Columns.Add("班次", typeof(String));
            datatab.Columns.Add("生产代码", typeof(String));
            datatab.Columns.Add("废品重量", typeof(String));
            datatab.Columns.Add("原因", typeof(String));
            this.dataGridView1.DataSource = datatab;
            dataGridView1.Columns[1].Width=155; 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.lbId.Text = items.ToString();
            datatab.Rows.Add(lbId.Text, dtp.Value.ToShortDateString(), txbWorkTurn.Text, txbProductCode.Text, txbWasteWeight.Text, txbReason.Text);
            items++;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //this.ltbShow.Items.Remove(this.ltbShow.SelectedItem);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //LoginForm check = new LoginForm(conn);
			//check.LoginButton.Text = "审核通过";
			//check.ExitButton.Text = "取消";
            //check.ShowDialog();
            //this.txbRecheckMan.Text = "李四";
            //this.txbRecheckMan.Visible = true;
            //this.lbRecheckMan.Visible = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (true==dtp.Visible ) dtp.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "生产日期")
                {
                    showdtp(e);
                }
            }
        }

        private void showdtp(DataGridViewCellEventArgs e)
        {

            dtp.Size = dataGridView1.CurrentCell.Size;
            //dtp.Top = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top + dataGridView1.Top;
            //dtp.Left = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left + dataGridView1.Left;
            dtp.Top = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top; 
            dtp.Left = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left;
            dtp.BringToFront();
            dtp.Visible = true;
            if (!(object.Equals(Convert.ToString(dataGridView1.CurrentCell.Value), "")))
            {
                dtp.Value = Convert.ToDateTime(dataGridView1.CurrentCell.Value);
            }
            
            dtp.Visible = true;
            this.Controls.Add(dtp);
            dataGridView1.Controls.Add(dtp);
            //dtp.Show();

            //DateTimePicker dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            //dateTimePicker1.Location = new System.Drawing.Point(0,0);
            //dateTimePicker1.Name = "dateTimePicker1";
            //dateTimePicker1.Size = dataGridView1.CurrentCell.Size;
            //dateTimePicker1.TabIndex = 24;
            //dateTimePicker1.BringToFront();
            //this.Controls.Add(dateTimePicker1);

            dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = dtp.Value.ToShortDateString();
        }
        
    }
}
