using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace mySystem.Other
{
    public partial class InputDataGridView : Form
    {
        DataTable _datatable;
        bool _multiSelect;
        Hashtable _additional;
        public InputDataGridView(String data, DataTable setting, bool multiSelect=true, Hashtable additional=null)
        {
            InitializeComponent();
            _datatable = setting;
            _multiSelect = multiSelect;
            _additional = additional;
            // add column
            setting.Columns.Add("选择", System.Type.GetType("System.Boolean"));
            // check sth
            List<string> ids;
            if (data.Trim() == "")
            {
                ids = new List<string>();
            }
            else
            {
                ids = new List<string>(data.Split(','));
            }
            foreach (DataRow dr in _datatable.Rows)
            {
                dr["选择"] = false;
            }
            foreach (string sid in ids)
            {
                int id = Convert.ToInt32(sid);
                DataRow[] drs = _datatable.Select("ID=" + id);
                if (drs.Length == 0)
                {
                    MessageBox.Show("ID为" + id + "的数据不存在","警告");
                }
                drs[0]["选择"] = true;
            }

            dataGridView1.DataSource = _datatable;
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            //addOtherEventHandler();
        }

        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "选择":
                    if (!_multiSelect)
                    {
                        //dataGridView1.Rows[e.RowIndex].Cells[0].edited
                        if (Convert.ToBoolean(dataGridView1[e.ColumnIndex, e.RowIndex].EditedFormattedValue))
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                            {
                                //if (i == e.RowIndex) continue;
                                dataGridView1["选择", i].Value = false;
                            }
                            dataGridView1[e.ColumnIndex, e.RowIndex].Value = true;
                        }
                    }
                    break;
            }
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
            //throw new NotImplementedException();
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        void addOtherEventHandler()
        {
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["选择"].DisplayIndex = 0;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            if (_additional != null)
            {
                dataGridView1.ShowCellToolTips = true;
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    int id = Convert.ToInt32(dgvr.Cells["ID"].Value);
                    foreach (DataGridViewCell cell in dgvr.Cells)
                    {
                        object[] tmp;
                        try
                        {
                            tmp = ((List<Object>)_additional[id]).ToArray();
                            cell.ToolTipText = String.Join("\n", tmp);
                        }
                        catch (InvalidCastException ee)
                        {
                            tmp = ((HashSet<Object>)_additional[id]).ToArray();
                            cell.ToolTipText = String.Join("\n", tmp);
                        }


                    }
                }
            }
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            addOtherEventHandler();
            dataGridView1.DataBindingComplete -= dataGridView1_DataBindingComplete;
        }

        private void btn完成_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static String getIDs(String data, DataTable setting, bool multiSelect = true, Hashtable additional = null)
        {
            InputDataGridView dlg = new InputDataGridView(data, setting, multiSelect, additional);
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // 获取用户的选择，并返回
                List<int> ids = new List<int>();
                foreach (DataRow dr in dlg._datatable.Rows)
                {
                    if (Convert.ToBoolean(dr["选择"]))
                    {
                        ids.Add(Convert.ToInt32(dr["ID"]));
                    }
                }
                return String.Join(",", ids.ToArray());
            }
            else
            {
                return "";
            }
           
        }
    }
}
