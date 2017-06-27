using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Extruction.Chart
{
    public partial class wasterecord : mySystem.BaseForm
    {
        int idss;
        public DataTable datatab;
        public DataTable datashw;
        public DataTable datasel;
        public DateTimePicker dtp1;
        public DataGridView fill;
        public DataGridView getsel;
        public DataGridView showx;
        public Button btAdd;
        public Button btSel;
        public Button btChk;
        public Label lbTitle;
        public ComboBox functions;
        public int filltop, fillleft, titletop, titleleft, btntop, btnleft;

        public wasterecord(mySystem.MainForm mainform)
            : base(mainform)
        {
            titleleft = 200;
            titletop = 30;
            btnleft = 520;
            btntop = 30;
            addLayout();
            filltop = 80;
            InitializeComponent();

            datatab = new DataTable();
            datashw = new DataTable();

            drawdatab();
            drawsel();
            drawsel();
            drawsel();
            drawshow();
            function_inni();
            //addItem();
            //addItem();
            //addItem();

            functions.SelectedValueChanged += new EventHandler(functions_SelectedValueChanged);
            fill.CellClick += new DataGridViewCellEventHandler(fill_CellClick);
            getsel.CellClick += new DataGridViewCellEventHandler(getsel_CellClick);




        }
        private void function_inni()
        {
            if (functions.SelectedItem.ToString() == "添加")
            {
                fill.Visible = true;
                fill.Enabled = true;
                fill.BringToFront();
                getsel.Visible = false;
                getsel.Enabled = false;
                //btSel.Enabled = false;
                //btAdd.Enabled = true;
            }
            else if (functions.SelectedItem.ToString() == "查询")
            {
                fill.Visible = false;
                fill.Enabled = false;
                getsel.Visible = true;
                getsel.Enabled = true;
                getsel.BringToFront();
                //btAdd.Enabled = false;
                //btSel.Enabled = true;
            }
            else { ;}
        }
        private void functions_SelectedValueChanged(object sender, EventArgs e)
        {
            if (functions.SelectedItem.ToString() == "添加")
            {
                fill.Visible = true;
                fill.Enabled = true;
                fill.BringToFront();
                getsel.Visible = false;
                getsel.Enabled = false;
            }
            else if (functions.SelectedItem.ToString() == "查询")
            {
                fill.Visible = false;
                fill.Enabled = false;
                getsel.Visible = true;
                getsel.Enabled = true;
                getsel.BringToFront();
            }
            else { ;}
        }

        private void drawsel()
        {


            datasel = new DataTable();
            datasel.Columns.Add("创建开始日期", typeof(DateTime));
            datasel.Columns.Add("创建结束日期", typeof(DateTime));
            datasel.Columns.Add("生产指令", typeof(Int32));
            datasel.Columns.Add("复合人员", typeof(Int32));

            //dtp1 = new DateTimePicker();
            DateTime start = Convert.ToDateTime("2017-01-01");
            DateTime stop = DateTime.Now;

            int revid = 12;
            datasel.Rows.Add(start.Date, stop.Date, 0, 0);

            DateTime d = new DateTime();
            d = Convert.ToDateTime(datasel.Rows[0][0].ToString());
            getsel = new DataGridView();
            getsel.Location = new System.Drawing.Point(30, filltop);
            getsel.Name = "dgvGetsel";
            getsel.RowTemplate.Height = 23;
            getsel.Size = new System.Drawing.Size(600, getsel.RowTemplate.Height * 3);
            getsel.TabIndex = 33;
            getsel.DataSource = datasel;
            getsel.AllowUserToAddRows = false;
            getsel.RowHeadersVisible = false;
            this.Controls.Add(getsel);
        }
        private void drawdatab()
        {
            fill = new DataGridView();
            fill.Location = new System.Drawing.Point(30, filltop);
            fill.Name = "dgvFill";
            fill.RowTemplate.Height = 23;
            fill.Size = new System.Drawing.Size(600, fill.RowTemplate.Height * 3);
            fill.TabIndex = 33;
            fill.DataSource = datatab;
            fill.AllowUserToAddRows = false;
            fill.RowHeadersVisible = false;
            this.Controls.Add(fill);

            datatab.Columns.Add("序号", typeof(Int32));
            datatab.Columns.Add("生产指令", typeof(Int32));
            datatab.Columns.Add("创建日期", typeof(DateTime));
            datatab.Columns.Add("修改时间", typeof(DateTime));
            datatab.Columns.Add("开始生产时间", typeof(DateTime));
            datatab.Columns.Add("结束生产时间", typeof(DateTime));
            datatab.Columns.Add("生产日期", typeof(DateTime));
            datatab.Columns.Add("班次", typeof(Int32));
            datatab.Columns.Add("产品序号", typeof(string));
            datatab.Columns.Add("废品数量", typeof(Int32));
            datatab.Columns.Add("废品原因", typeof(string));
            datatab.Columns.Add("检查人员", typeof(Int32));
            datatab.Columns.Add("复合人员", typeof(Int32));

            dtp1 = new DateTimePicker();
            DateTime tm = DateTime.Now;
            //DateTime moditm = DateTime.Now;
            int pdins = 11;
            DateTime pdtm = DateTime.Now;
            int fli = 1;
            string pdcode = "pd code";
            DateTime chktm = DateTime.Now;

            int chkid = 11;
            int revid = 12;
            datatab.Rows.Add(idss, pdins, tm.Date, tm.Date, tm.Date, tm.Date, tm.Date, fli, pdcode, pdins, pdcode, chkid, revid);
        }
        private void drawshow()
        {
            datashw.Columns.Add("序号", typeof(Int32));
            datashw.Columns.Add("生产指令", typeof(Int32));
            datashw.Columns.Add("创建日期", typeof(DateTime));
            datashw.Columns.Add("修改时间", typeof(DateTime));
            datashw.Columns.Add("开始生产时间", typeof(DateTime));
            datashw.Columns.Add("结束生产时间", typeof(DateTime));
            datashw.Columns.Add("生产日期", typeof(DateTime));
            datashw.Columns.Add("班次", typeof(Int32));
            datashw.Columns.Add("产品序号", typeof(string));
            datashw.Columns.Add("废品数量", typeof(Int32));
            datashw.Columns.Add("废品原因", typeof(string));
            datashw.Columns.Add("检查人员", typeof(Int32));
            datashw.Columns.Add("复合人员", typeof(Int32));

            //datashw.Rows.Add(dtp1);


            showx = new DataGridView();
            showx.Location = new System.Drawing.Point(30, 150);
            showx.Name = "dgvShowx";
            showx.RowTemplate.Height = 23;
            showx.Size = new System.Drawing.Size(600, fill.RowTemplate.Height * 5);
            showx.TabIndex = 34;
            showx.AllowUserToAddRows = false;
            showx.RowHeadersVisible = false;
            showx.Columns.Add("id", "序号");
            showx.Columns.Add("production_instruction", "生产指令");
            showx.Columns.Add("createtime", "创建日期");
            showx.Columns.Add("modifytime", "修改时间");
            showx.Columns.Add("start_time_production", "开始生产时间");
            showx.Columns.Add("end_time_production", "结束生产时间");
            showx.Columns.Add("production_date", "生产日期");
            showx.Columns.Add("flight", "班次");
            showx.Columns.Add("product_id", "产品序号");
            showx.Columns.Add("waste_quantity", "废品数量");
            showx.Columns.Add("cause_of_waste", "废品原因");
            showx.Columns.Add("checker_id", "检查人员");
            showx.Columns.Add("reviewer_id", "复合人员");

            //showx.DataSource = datashw;
            this.Controls.Add(showx);
            //showx.Controls.Add(dtp1);

            //fill.Scroll+=new ScrollEventHandler(fill_Scroll);
            // fill.ColumnWidthChanged+=new DataGridViewColumnEventHandler(fill_ColumnWidthChanged);


        }

        private void fill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dtp1.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                if (fill.Columns[e.ColumnIndex].Name == "创建日期")
                {
                    showdtp(e);
                }
                else if (fill.Columns[e.ColumnIndex].Name == "修改时间")
                {
                    showdtp(e);
                }
                else if (fill.Columns[e.ColumnIndex].Name == "生产日期")
                {
                    showdtp(e);
                }
                else if (fill.Columns[e.ColumnIndex].Name == "检查时间")
                {
                    showdtp(e);
                }
                else
                { ; }
            }
        }
        private void getsel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dtp1.Visible = false;
            if (e.ColumnIndex >= 0)
            {
                if ((getsel.Columns[e.ColumnIndex].Name == "创建开始日期") || (getsel.Columns[e.ColumnIndex].Name == "创建结束日期"))
                {
                    showdtp(e);
                }
                else { ;}
            }
        }
        private void showdtp(DataGridViewCellEventArgs e)
        {
            //dtp1.Visible = false;
            fill.AllowUserToResizeColumns = false;
            fill.AllowUserToResizeRows = false;
            getsel.AllowUserToResizeColumns = false;
            getsel.AllowUserToResizeRows = false;

            if (0 == functions.SelectedIndex)
            {
                dtp1.Size = fill.CurrentCell.Size;
                dtp1.Top = fill.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top;
                dtp1.Left = fill.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left;
                dtp1.Visible = true;
                dtp1.BringToFront();
                fill.Controls.Add(dtp1);
            }
            else if (1 == functions.SelectedIndex)
            {
                dtp1.Size = getsel.CurrentCell.Size;
                dtp1.Top = getsel.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top;
                dtp1.Left = getsel.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Left;
                dtp1.Visible = true;
                dtp1.BringToFront();
                getsel.Controls.Add(dtp1);
            }

            dtp1.ValueChanged += new EventHandler(dtp1_ValueChanged);
        }
        /*        private void showdtp(ScrollEventArgs e)
                {
                    //dtp1.Visible = false;
                    if ((fill.Columns[fill.CurrentCell.ColumnIndex].Name == "创建日期") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "修改时间") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "生产日期") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "检查时间"))
                    {
                        dtp1.Size = fill.CurrentCell.Size;
                        dtp1.Top = fill.GetCellDisplayRectangle(fill.CurrentCell.ColumnIndex, fill.CurrentCell.RowIndex, true).Top;
                        dtp1.Left = fill.GetCellDisplayRectangle(fill.CurrentCell.ColumnIndex, fill.CurrentCell.RowIndex, true).Left;
                        dtp1.BringToFront();
                        dtp1.Visible = true;
                        fill.Controls.Add(dtp1);
                        //fill.Scroll += new ScrollEventHandler(fill_Scroll);
                    }
                }
                private void showdtp(DataGridViewColumnEventArgs e)
                {
                    //dtp1.Visible = false;
                    if ((fill.Columns[fill.CurrentCell.ColumnIndex].Name == "创建日期") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "修改时间") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "生产日期") || (fill.Columns[fill.CurrentCell.ColumnIndex].Name == "检查时间"))
                    {
                        dtp1.Size = fill.CurrentCell.Size;
                        dtp1.Top = fill.GetCellDisplayRectangle(fill.CurrentCell.ColumnIndex, fill.CurrentCell.RowIndex, true).Top;
                        dtp1.Left = fill.GetCellDisplayRectangle(fill.CurrentCell.ColumnIndex, fill.CurrentCell.RowIndex, true).Left;
                        dtp1.BringToFront();
                        fill.Controls.Add(dtp1);
                        //fill.ColumnWidthChanged+=new DataGridViewColumnEventHandler(fill_ColumnWidthChanged);
                    }
                }
                private void fill_ColumnWidthChanged(object sender,DataGridViewColumnEventArgs e )
                {
                    dtp1.Visible = false;
                    showdtp(e);
                    fill.ColumnWidthChanged += new DataGridViewColumnEventHandler(fill_ColumnWidthChanged);
                }
                private void fill_Scroll(object sender, ScrollEventArgs e)
                {
                    dtp1.Visible = false;
                    showdtp(e);
                    fill.Scroll += new ScrollEventHandler(fill_Scroll);
                    dtp1.ValueChanged += new EventHandler(dtp1_ValueChanged);
                    //lbTitle.Text = dtp1.Left.ToString();
                }*/
        private void dtp1_ValueChanged(object sender, EventArgs e)
        {
            if (0 == functions.SelectedIndex)
            {
                fill.CurrentCell.Value = dtp1.Value.Date;
                fill.AllowUserToResizeColumns = true;
                fill.AllowUserToResizeRows = true;
                fill.Enabled = true;
            }
            else if (1 == functions.SelectedIndex)
            {
                getsel.CurrentCell.Value = dtp1.Value.Date;
                getsel.AllowUserToResizeColumns = true;
                getsel.AllowUserToResizeRows = true;
                getsel.Enabled = true;
            }
            else { ;}

            dtp1.Visible = false;

        }
        private void addItem()
        {
            /*
                        datatab.Rows[0][0]=idss++;
                        datatab.Rows[0][1] = DateTime.Now.Date;
                        datatab.Rows[0][2] = DateTime.Now.Date;
                        datatab.Rows[0][3] = 11113333;
                        datatab.Rows[0][4] = DateTime.Now.Date;
                        datatab.Rows[0][5] = 1;
                        datatab.Rows[0][6] = DateTime.Now.Date;
                        datatab.Rows[0][7] = 1;
                        datatab.Rows[0][8] = 1;
                        datatab.Rows[0][9] = 1;
                        datatab.Rows[0][10]=1;
                        datatab.Rows[0][11] = 1;
                        datatab.Rows[0][12] = 11;
                        datatab.Rows[0][13] = 12;
                        //datatab.Rows.Add(idss, creatm, moditm, pdins, pdtm, fli, chktm, engine, vaval, material, alrm, slv, chkid, revid);
                        */
            /*if (!base.mainform.isSqlOk)
            {
                OleDbCommand oledcmd = new OleDbCommand();

                oledcmd.Connection = mainform.connOle;
                //connOle.Open();
                //oledcmd.CommandText = "INSERT INTO running_record_of_feeding_unit VALUES (@IDS,@CREATETM,@MODIFYTM,@PDINS,@PDDATE,@FLI,@CKTM,@MOTOR,@VAVAL,@MATER,@ALM,@SOV,@CKID,@RVID)";
                                oledcmd.CommandText = "INSERT INTO running_record_of_feeding_unit ( id, createtime, modifytime, production_instruction_id, porduction_date, flight, check_time, is_motor_working, is_pneumatic_valve_working, is_feeding_working, is_alarm, is_lift_alarm, checker_id, reviewer_id) VALUES (@IDS,@CREATETM,@MODIFYTM,@PDINS,@PDDATE,@FLI,@CKTM,@MOTOR,@VAVAL,@MATER,@ALM,@SOV,@CKID,@RVID)";

                //                oledcmd.CommandText = "SELECT * FROM running_record_of_feeding_unit WHERE id = @IDS";

                /*oledcmd.Parameters.Add(new OleDbParameter("@IDS", System.Data.OleDb.OleDbType.Integer)).Value = idss;
                oledcmd.Parameters.Add(new OleDbParameter("@CREATETM", System.Data.OleDb.OleDbType.Date)).Value = datatab.Rows[0][1];
                oledcmd.Parameters.Add(new OleDbParameter("@MODIFYTM", System.Data.OleDb.OleDbType.Date)).Value = datatab.Rows[0][2];
                oledcmd.Parameters.Add(new OleDbParameter("@PDINS", System.Data.OleDb.OleDbType.VarChar)).Value = datatab.Rows[0][3];
                oledcmd.Parameters.Add(new OleDbParameter("@PDDATE", System.Data.OleDb.OleDbType.Date)).Value = datatab.Rows[0][4];
                oledcmd.Parameters.Add(new OleDbParameter("@FLI", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][5];
                oledcmd.Parameters.Add(new OleDbParameter("@CKTM", System.Data.OleDb.OleDbType.Date)).Value = datatab.Rows[0][6];
                oledcmd.Parameters.Add(new OleDbParameter("@MOTOR", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][7];
                oledcmd.Parameters.Add(new OleDbParameter("@VAVAL", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][8];
                oledcmd.Parameters.Add(new OleDbParameter("@MATER", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][9];
                oledcmd.Parameters.Add(new OleDbParameter("@ALM", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][10];
                oledcmd.Parameters.Add(new OleDbParameter("@SOV", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][11];
                oledcmd.Parameters.Add(new OleDbParameter("@CKID", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][12];
                oledcmd.Parameters.Add(new OleDbParameter("@RVID", System.Data.OleDb.OleDbType.Integer)).Value = datatab.Rows[0][13];
                */

            /*               
                           oledcmd.Parameters["@IDS"].Value = idss;
                           oledcmd.Parameters[1].Value = creatm;
                           oledcmd.Parameters[2].Value = moditm;
                           oledcmd.Parameters[3].Value = pdins;
                           oledcmd.Parameters[4].Value = pdtm;
                           oledcmd.Parameters[5].Value = fli;
                           oledcmd.Parameters[6].Value = chktm;
                           oledcmd.Parameters[7].Value = engine;
                           oledcmd.Parameters[8].Value = vaval;
                           oledcmd.Parameters[9].Value = material;
                           oledcmd.Parameters[10].Value = alrm;
                           oledcmd.Parameters[11].Value = slv;
                           oledcmd.Parameters[12].Value = chkid;
                           oledcmd.Parameters[13].Value = revid;
                           //*/
            //System.Data.OleDb.OleDbType.
            //oledcmd.ExecuteNonQuery();
            //connOle.Close();
            List<List<Object>> rlt = new List<List<object>>();

            string tablename = "waste_record_of_extrusion_process";
            List<String> insertCols = new List<String>(new String[] { "id", "production_instruction_id", "createtime", "modifytime", "start_time_production", "end_time_production", "production_date", "flight", "product_id", "waste_quantity", "cause_of_waste", "recorder_id", "reviewer_id" });
            List<Object> insertVals = new List<Object>(new Object[] { Convert.ToInt32(datatab.Rows[0][0].ToString()), Convert.ToInt32(datatab.Rows[0][1]), Convert.ToDateTime(datatab.Rows[0][2]), Convert.ToDateTime(datatab.Rows[0][3]), Convert.ToDateTime(datatab.Rows[0][4]), Convert.ToDateTime(datatab.Rows[0][5]), Convert.ToDateTime(datatab.Rows[0][6]), Convert.ToInt32(datatab.Rows[0][7]), Convert.ToString(datatab.Rows[0][8]), Convert.ToInt32(datatab.Rows[0][9]), Convert.ToString(datatab.Rows[0][10]), Convert.ToInt32(datatab.Rows[0][11]), Convert.ToInt32(datatab.Rows[0][12]) });
            Boolean get = Utility.insertAccess(mainform.connOle, tablename, insertCols, insertVals);
            if (get) MessageBox.Show("get");
            returnsel();

        }

        private void addLayout()
        {
            btAdd = new Button();
            btAdd.Location = new System.Drawing.Point(btnleft, btntop);
            btAdd.Name = "btnAdd";
            btAdd.Size = new System.Drawing.Size(75, 23);
            btAdd.TabIndex = 28;
            btAdd.Text = "添加";
            btAdd.UseVisualStyleBackColor = true;
            this.Controls.Add(btAdd);
            btAdd.Click += new EventHandler(btAdd_Click);

            btSel = new Button();
            btSel.Location = new System.Drawing.Point(btnleft + btAdd.Width, btntop);
            btSel.Size = new System.Drawing.Size(75, 23);
            btSel.Text = "查询";
            this.Controls.Add(btSel);
            btSel.Click += new EventHandler(btSel_Click);

            btChk = new Button();
            btChk.Location = new System.Drawing.Point(btnleft + 2 * btAdd.Width, btntop);
            btChk.Size = new System.Drawing.Size(75, 23);
            btChk.Text = "审核";
            this.Controls.Add(btChk);
            btChk.Click += new EventHandler(btChk_Click);

            functions = new ComboBox();
            functions.Location = new System.Drawing.Point(50, btntop);
            functions.Items.Add("添加");
            functions.Items.Add("查询");
            functions.SelectedIndex = 1;
            functions.Font = new System.Drawing.Font("宋体", 12);
            functions.Size = new System.Drawing.Size(80, 23);
            this.Controls.Add(functions);


            lbTitle = new Label();
            this.lbTitle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.Location = new System.Drawing.Point(titleleft, titletop);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTitle.Name = "lbTitle";
            //this.lbTitle.Size = new System.Drawing.Size(169, 19);
            this.lbTitle.AutoSize = true;
            this.lbTitle.TabIndex = 20;
            this.lbTitle.Text = "吹膜工序废品记录";
            this.lbTitle.Visible = true;
            this.Controls.Add(lbTitle);

            this.Size = new System.Drawing.Size(800, 400);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            //fill.Rows.Clear();
            //datashw.Rows.Clear();
            addItem();
        }
        private void btSel_Click(object sender, EventArgs e)
        {
            returnsel();
        }
        private void btChk_Click(object sender, EventArgs e)
        {
            CheckForm check = new CheckForm(base.mainform);
            check.ShowDialog();
            datatab.Rows[0][12] = check.userID;
        }
        private void returnsel()
        {

            datashw.Rows.Clear();
            showx.Rows.Clear();
            List<List<Object>> rlt = new List<List<object>>();
            string betweenCol = "createtime";
            string tablename = "waste_record_of_extrusion_process";
            DateTime left = Convert.ToDateTime(datasel.Rows[0][0].ToString());
            DateTime right = Convert.ToDateTime(datasel.Rows[0][1].ToString());
            List<String> queryCols = new List<String>(new String[] { "id", "production_instruction_id", "createtime", "modifytime", "start_time_production", "end_time_production", "production_date", "flight", "product_id", "waste_quantity", "cause_of_waste", "recorder_id", "reviewer_id" });
            List<Object> queryVals = new List<Object>(new Object[] { typeof(Int32), typeof(Int32), typeof(DateTime), typeof(DateTime), typeof(DateTime), typeof(DateTime), typeof(DateTime), typeof(Int32), typeof(System.String), typeof(Int32), typeof(System.String), typeof(Int32), typeof(Int32) });
            List<String> whereCols;
            List<Object> whereVals;
            if (Convert.ToInt32(datasel.Rows[0][2]) == 0 && Convert.ToInt32(datasel.Rows[0][3]) == 0)
            {
                rlt = Utility.selectAccess(mainform.connOle, tablename, queryCols, null, null, null, null, betweenCol, left, right);

            }
            if (Convert.ToInt32(datasel.Rows[0][2]) == 0 && Convert.ToInt32(datasel.Rows[0][3]) != 0)
            {
                whereCols = new List<String>(new String[] { "reviewer_id" });
                whereVals = new List<Object>(new Object[] { Convert.ToInt32(datasel.Rows[0][3]) });
                rlt = Utility.selectAccess(mainform.connOle, tablename, queryCols, whereCols, whereVals, null, null, betweenCol, left, right);
            }
            if (Convert.ToInt32(datasel.Rows[0][2]) != 0 && Convert.ToInt32(datasel.Rows[0][3]) == 0)
            {
                whereCols = new List<String>(new String[] { "production_instruction_id" });
                whereVals = new List<Object>(new Object[] { Convert.ToInt32(datasel.Rows[0][2]) });
                rlt = Utility.selectAccess(mainform.connOle, tablename, queryCols, whereCols, whereVals, null, null, betweenCol, left, right);
            }

            if (Convert.ToInt32(datasel.Rows[0][2]) != 0 && Convert.ToInt32(datasel.Rows[0][3]) != 0)
            {
                whereCols = new List<String>(new String[] { "production_instruction_id", "reviewer_id" });
                whereVals = new List<Object>(new Object[] { Convert.ToInt32(datasel.Rows[0][2]), Convert.ToInt32(datasel.Rows[0][3].ToString()) });
                rlt = Utility.selectAccess(mainform.connOle, tablename, queryCols, whereCols, whereVals, null, null, betweenCol, left, right);
            }


            Utility.fillDataGridView(showx, rlt);
            //datasel.Rows.Add(Convert.ToDateTime("2017-01-01"), DateTime.Now, null, null);


        }

        //exploring how to use select sql
        //public static List<List<Object>> selectAccess(OleDbConnection conn, String tblName, List<String> queryCols, List<String> whereCols, List<Object> whereVals)


    }
}

