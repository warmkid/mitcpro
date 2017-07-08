using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace BatchProductRecord
{
    public partial class ProcessProductInstru : mySystem.BaseForm
    {
        mySystem.CheckForm checkform;
        DataTable dt=null;//存储从产品表中读到的产品代码
        string last_code;//最后一次保存数据库时生产指令编码,默认该表内不会重复
        float leng;
        float sumweight;

        //新的版本
        int index = 0;//判断是插入模式还是更新模式，0代表插入，1代表更新
        int id;
        int label = 0;
        OleDbConnection connOle;
        private DataTable dt_prodinstr,dt_prodlist;//讲师
        private OleDbDataAdapter da_prodinstr,da_prodlist;
        private BindingSource bs_prodinstr,bs_prodlist;
        private OleDbCommandBuilder cb_prodinstr,cb_prodlist;

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }
        public ProcessProductInstru(mySystem.MainForm mainform):base(mainform)
        {
            InitializeComponent();
            #region 之前
            //init();
            //addrows();
            #endregion

            init();
            bind();
            
        }
        private void init()
        {
            #region 之前
            //textBox4.Text = "AA-EQM-032";
            //textBox24.Text = mySystem.Parameter.userName;
            //button2.Enabled = false;
            //button3.Enabled = false;
            //sumweight = 0;

            ////从产品表中读数据填入产品代码下拉列表中
            //if (mainform.isSqlOk)
            //{
            //}
            //else
            //{
            //    string asql = "select product_code from product_aoxing";
            //    OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            //    OleDbDataAdapter da = new OleDbDataAdapter(comm);

            //    dt = new DataTable();
            //    da.Fill(dt);
            //    da.Dispose();
            //    comm.Dispose();

            //}
            #endregion


            connOle = mySystem.Parameter.connOle;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataError += dataGridView1_DataError;

            dt_prodinstr=new System.Data.DataTable();
            bs_prodinstr=new System.Windows.Forms.BindingSource();
            da_prodinstr=new OleDbDataAdapter();
            cb_prodinstr=new OleDbCommandBuilder();

            dt_prodlist = new System.Data.DataTable();
            bs_prodlist=new System.Windows.Forms.BindingSource();
            da_prodlist = new OleDbDataAdapter();
            cb_prodlist = new OleDbCommandBuilder();


        }
        //产品列表绑定
        private void bind_list(int id)
        {
            dt_prodlist.Dispose();
            bs_prodlist.Dispose();
            da_prodlist.Dispose();
            cb_prodlist.Dispose();

            dt_prodlist = new DataTable("生产指令产品列表");
            bs_prodlist = new BindingSource();
            da_prodlist = new OleDbDataAdapter("select * from 生产指令产品列表 where 生产指令ID="+id, connOle);
            cb_prodlist = new OleDbCommandBuilder(da_prodlist);
            da_prodlist.Fill(dt_prodlist);

            //DataTable到BindingSource的绑定
            bs_prodlist.DataSource = dt_prodlist;

            //BindingSource到控件的绑定
            dataGridView1.DataSource = bs_prodlist.DataSource;
        }

        //初次绑定数据库表
        private void bind()
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("生产指令信息表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 生产指令信息表 where 1=2", connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            DataRow dr = dt_prodinstr.NewRow();
            dr[1] = dr[2] = dr[3] = dr[4] = dr[6] = dr[7] = dr[8] = dr[10] = dr[11] = dr[12] = dr[14] = dr[15] = dr[17] = dr[19] = dr[20] = dr[22] = dr[24] = dr[26] = dr[29] = "";
            dr[5] = dr[21] = dr[23] = dr[25] = DateTime.Now;
            dr[9] = dr[13] = dr[16] = dr[18] = dr[28] = dr[30] = dr[31] = dr[32] = 0;
            dr[27] = false;
            dt_prodinstr.Rows.Add(dr);

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();

            textBox15.DataBindings.Clear();
            textBox17.DataBindings.Clear();
            textBox19.DataBindings.Clear();
            textBox21.DataBindings.Clear();
            textBox16.DataBindings.Clear();
            textBox18.DataBindings.Clear();
            textBox20.DataBindings.Clear();
            textBox22.DataBindings.Clear();
            textBox12.DataBindings.Clear();
            textBox13.DataBindings.Clear();
            textBox14.DataBindings.Clear();
            textBox9.DataBindings.Clear();
            textBox11.DataBindings.Clear();
            textBox5.DataBindings.Clear();

            textBox24.DataBindings.Clear();
            textBox25.DataBindings.Clear();
            textBox26.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
            dateTimePicker3.DataBindings.Clear();
            dateTimePicker4.DataBindings.Clear();

            //BindingSource到控件的绑定
            bind_bs_contr();

            index = 0;

        }

        private void bind_bs_contr_list()
        {
            dataGridView1.DataSource = bs_prodlist.DataSource;
 
        }
        private void bind_bs_contr()    
        {
            textBox1.DataBindings.Add("Text", bs_prodinstr.DataSource, "产品名称");
            textBox2.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产指令编号");
            textBox3.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产工艺");
            textBox4.DataBindings.Add("Text", bs_prodinstr.DataSource, "生产设备编号");
            dateTimePicker1.DataBindings.Add("Value", bs_prodinstr.DataSource, "开始生产日期");

            textBox15.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料代码");
            textBox17.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层物料批号");
            textBox19.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层包装规格");
            textBox21.DataBindings.Add("Text", bs_prodinstr.DataSource, "内外层领料量");
            textBox16.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料代码");
            textBox18.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层物料批号");
            textBox20.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层包装规格");
            textBox22.DataBindings.Add("Text", bs_prodinstr.DataSource, "中层领料量");
            textBox12.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管");
            textBox13.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管规格");
            textBox14.DataBindings.Add("Text", bs_prodinstr.DataSource, "卷心管领料量");
            textBox9.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装包装规格");
            textBox11.DataBindings.Add("Text", bs_prodinstr.DataSource, "双层洁净包装领料量");
            textBox5.DataBindings.Add("Text", bs_prodinstr.DataSource, "负责人");

            textBox24.DataBindings.Add("Text", bs_prodinstr.DataSource, "编制人");
            textBox25.DataBindings.Add("Text", bs_prodinstr.DataSource, "审批人");
            textBox26.DataBindings.Add("Text", bs_prodinstr.DataSource, "接收人");
            dateTimePicker2.DataBindings.Add("Value", bs_prodinstr.DataSource, "编制时间");
            dateTimePicker3.DataBindings.Add("Value", bs_prodinstr.DataSource, "审批时间");
            dateTimePicker4.DataBindings.Add("Value", bs_prodinstr.DataSource, "接收时间");
        }
        
        //得到最新插入的行的id
        private int getid()
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select @@identity";
            return (int)comm.ExecuteScalar();
        }
        //根据筛选条件得到指令id,没有返回-1
        private int getid(string instrcode)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select ID from 生产指令信息表 where 生产指令编号='" + instrcode + "'";

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                da.Dispose();
                tempdt.Dispose();
                return -1;
            }
            else
            {
                da.Dispose();
                return (int)tempdt.Rows[0][0];
            }
        }

        //再次绑定数据库表，相当与更新
        private void bind2(int id)
        {
            dt_prodinstr.Dispose();
            bs_prodinstr.Dispose();
            da_prodinstr.Dispose();
            cb_prodinstr.Dispose();

            dt_prodinstr = new DataTable("生产指令信息表");
            bs_prodinstr = new BindingSource();
            da_prodinstr = new OleDbDataAdapter("select * from 生产指令信息表 where ID="+id, connOle);
            cb_prodinstr = new OleDbCommandBuilder(da_prodinstr);
            da_prodinstr.Fill(dt_prodinstr);

            //DataTable到BindingSource的绑定
            bs_prodinstr.DataSource = dt_prodinstr;

            //解除之前的绑定
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();

            textBox15.DataBindings.Clear();
            textBox17.DataBindings.Clear();
            textBox19.DataBindings.Clear();
            textBox21.DataBindings.Clear();
            textBox16.DataBindings.Clear();
            textBox18.DataBindings.Clear();
            textBox20.DataBindings.Clear();
            textBox22.DataBindings.Clear();
            textBox12.DataBindings.Clear();
            textBox13.DataBindings.Clear();
            textBox14.DataBindings.Clear();
            textBox9.DataBindings.Clear();
            textBox11.DataBindings.Clear();
            textBox5.DataBindings.Clear();

            textBox24.DataBindings.Clear();
            textBox25.DataBindings.Clear();
            textBox26.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
            dateTimePicker3.DataBindings.Clear();
            dateTimePicker4.DataBindings.Clear();

            //BindingSource到控件的绑定
            bind_bs_contr();

            index = 1;
        }

        //表格错误处理
        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 获取选中的列，然后提示
            String name = ((DataGridView)sender).Columns[((DataGridView)sender).SelectedCells[0].ColumnIndex].Name;
            MessageBox.Show(name + "填写错误");
        }

        //添加编辑行
        private void addrows()
        {
            DataGridViewRow dr = new DataGridViewRow();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);//给行添加单元格
            }
            dr.Cells[0].Value = dataGridView1.Rows.Count+1;
            dataGridView1.Rows.Add(dr);
            //DataGridViewComboBoxCell combox = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1] as DataGridViewComboBoxCell;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    combox.Items.Add(dt.Rows[i][0]);
            //}
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProcessProductInstru_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public override void CheckResult()
        {
            base.CheckResult();
            //获得审核信息
            //string opinion = checkform.opinion;
            //bool isok = checkform.ischeckOk;
            textBox25.Text = name_findby_id(checkform.userID);
            dateTimePicker3.Value = checkform.time;

            //选择刚才的表中对应的记录，并更新里面的记录
            string asql = "select production_instruction_id from production_instruction where production_instruction_code='"+last_code+"'";
            OleDbCommand comm = new OleDbCommand(asql, mySystem.Parameter.connOle);
            OleDbDataAdapter da = new OleDbDataAdapter(comm);

            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 0)
            {
                MessageBox.Show("对应的记录未找到");
                return;
            }
            int id = Int32.Parse(tempdt.Rows[0][0].ToString());
            comm.CommandText = "update production_instruction set reviewer_id=@id,review_date=@date,review_opinion=@opinion,is_review_qualified=@isok where production_instruction_id="+id;
            comm.Parameters.Add("@id", System.Data.OleDb.OleDbType.Integer);
            comm.Parameters.Add("@date", System.Data.OleDb.OleDbType.Date);
            comm.Parameters.Add("@opinion", System.Data.OleDb.OleDbType.VarChar);
            comm.Parameters.Add("@isok", System.Data.OleDb.OleDbType.Boolean);

            comm.Parameters["@id"].Value = checkform.userID;
            comm.Parameters["@date"].Value = checkform.time;
            comm.Parameters["@opinion"].Value = checkform.opinion;
            comm.Parameters["@isok"].Value = checkform.ischeckOk;
            int result = comm.ExecuteNonQuery();
            if (result<=0)
            {
                MessageBox.Show("添加错误");
            }

            button3.Enabled = true;
            da.Dispose();
            comm.Dispose();
            tempdt.Dispose();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            checkform = new mySystem.CheckForm(this);
            checkform.Show();  
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            #region 之前
//            float in_amout;
//            float mid_amout;
//            float juan_amout;
//            float extruproc_label_amout;
//            float doubclean_amout;

//            if (!float.TryParse(textBox21.Text, out  in_amout) || !float.TryParse(textBox22.Text, out mid_amout) || !float.TryParse(textBox14.Text, out juan_amout))
//            {
//                MessageBox.Show("领料量必须为数值类型");
//                return;
//            }

//            string prodname = textBox1.Text;
//            string instrcode = textBox2.Text;
//            if (instrcode == "")
//            {
//                MessageBox.Show("生产指令未填写");
//                return;
//            }
//            string art = textBox3.Text;
//            string number = textBox4.Text;
//            DateTime d = dateTimePicker1.Value;
//            string in_matcode = textBox15.Text;//内外层物料代码
//            string in_matbatch = textBox17.Text;
//            string in_format = textBox19.Text;

//            string mid_matcode = textBox16.Text;//中层物料代码
//            string mid_matbatch = textBox18.Text;
//            string mid_format = textBox20.Text;

//            string juan_extr = textBox12.Text;
//            string juan_format = textBox13.Text;
//            string juan_quan = textBox14.Text;

//            //string extruproc_label_format = textBox8.Text;
//            //string extruproc_label_quan = textBox10.Text;
//            string doubclean_format = textBox9.Text;//内包形式包装规格
//            string doubclean_quan = textBox11.Text;//内包形式领料量

//            string chargeman = textBox5.Text;
//            int chargeman_id = id_findby_code(chargeman);
//            if (chargeman_id == -1)
//            {
//                MessageBox.Show("负责人id不存在");
//                return;
//            }

//            string extra = textBox23.Text;
//            string compman = textBox24.Text;//编制人;
//            //int compman_id = id_findby_code(compman);
//            //if (compman_id == -1)
//            //{
//            //    MessageBox.Show("编制人id不存在");
//            //    return;
//            //}

//            DateTime compdate = dateTimePicker2.Value;
//            string checkman = textBox25.Text;//审批人

//            DateTime checkdate = dateTimePicker3.Value;
//            string recman = textBox26.Text;//接收人
//            int recman_id = id_findby_code(recman);
//            if (recman_id == -1)
//            {
//                MessageBox.Show("接受人id不存在");
//                return;
//            }

//            DateTime recdate = dateTimePicker4.Value;

//            //领料量是否合法
//            sumweight = 0;
//            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
//            {
//                sumweight += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
//            }
//            if (in_amout < sumweight * 0.75 / 0.9)
//            {
//                MessageBox.Show("领料量小于供料重量，请重新填写");
//                return;
//            }


//            //jason格式产品代码
//            JArray ret = JArray.Parse("[]");
//            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) //最后一行不添加
//            {
//                string st = "{'";
//                string t = dataGridView1.Rows[i].Cells[1].Value.ToString() + "':";
//                st += t;
//                st += "[" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[5].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[7].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[8].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[9].Value.ToString() + "']}";

//                JObject temp = JObject.Parse(st);
//                ret.Add(temp);
//            }
//            System.Console.WriteLine(ret.ToString());

//            if (mainform.isSqlOk)
//            {
//            }
//            else
//            {
//                int result = 0;
//                OleDbCommand comm = new OleDbCommand();
//                comm.Connection = mySystem.Parameter.connOle;

//                //                comm.CommandText = "insert into production_instruction(product_name,production_instruction_code,production_process,machine,production_start_date,instruction_description,raw_material_id_in_out,raw_material_batch_in_out," +
//                //    "raw_material_id_middle,raw_material_batch_middle,package_specifications_in_out,package_specifications_middle," +
//                //    "receive_quantity_in_out,receive_quantity_middle,core_tube_parameter,core_tube_package_specifications,core_tube_receive_the_quantity_of_raw_material,package_specifications,package_receive_the_quantity_of_raw_material,package_specifications_inner,package_receive_the_quantity_of_raw_material_inner,extr," +
//                //    "principal_id,editor_id,reviewer_id,receiver_id,edit_date,review_date,receive_date)" +
//                //    " values(@name,@instrcode,@prodcess,@machine,@startdate,@desc,@inout_id,@inout_batch,@mid_id,@mid_batch,@inout_pac,@mid_pac," +
//                //"@inout_quan,@mid_quan,@tube_para,@tube_pac,@tube_quan,@pac_label,@quan_label,@pac_inner,@quan_inner,@extr,@princ_id,@editor_id,@rev_id,@rec_id,@editdate,@revdate,@recdate)";

//                comm.CommandText = "insert into production_instruction(product_name,production_instruction_code,production_process,machine,production_start_date,instruction_description,raw_material_id_in_out,raw_material_batch_in_out," +
//"raw_material_id_middle,raw_material_batch_middle,package_specifications_in_out,package_specifications_middle," +
//"receive_quantity_in_out,receive_quantity_middle,core_tube_parameter,core_tube_package_specifications,core_tube_receive_the_quantity_of_raw_material,package_specifications,package_receive_the_quantity_of_raw_material,extr," +
//"principal_id,editor_id,reviewer_id,receiver_id,edit_date,review_date,receive_date)" +
//" values(@name,@instrcode,@prodcess,@machine,@startdate,@desc,@inout_id,@inout_batch,@mid_id,@mid_batch,@inout_pac,@mid_pac," +
//"@inout_quan,@mid_quan,@tube_para,@tube_pac,@tube_quan,@pac_inner,@quan_inner,@extr,@princ_id,@editor_id,@rev_id,@rec_id,@editdate,@revdate,@recdate)";


//                System.Console.WriteLine(comm.CommandText.ToString());
//                comm.Parameters.Add("@name", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@instrcode", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@prodcess", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@machine", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@startdate", System.Data.OleDb.OleDbType.Date);
//                comm.Parameters.Add("@desc", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@inout_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@inout_batch", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@mid_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@mid_batch", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@inout_pac", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@mid_pac", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@inout_quan", System.Data.OleDb.OleDbType.Integer);//
//                comm.Parameters.Add("@mid_quan", System.Data.OleDb.OleDbType.Integer);//
//                comm.Parameters.Add("@tube_para", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@tube_pac", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@tube_quan", System.Data.OleDb.OleDbType.Integer);//
//                //comm.Parameters.Add("@pac_label", System.Data.OleDb.OleDbType.VarChar);
//                //comm.Parameters.Add("@quan_label", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@pac_inner", System.Data.OleDb.OleDbType.VarChar);//内包装 包装规格
//                comm.Parameters.Add("@quan_inner", System.Data.OleDb.OleDbType.VarChar);//内包装 领量
//                comm.Parameters.Add("@extr", System.Data.OleDb.OleDbType.VarChar);
//                comm.Parameters.Add("@princ_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@editor_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@rev_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@rec_id", System.Data.OleDb.OleDbType.Integer);
//                comm.Parameters.Add("@editdate", System.Data.OleDb.OleDbType.Date);
//                comm.Parameters.Add("@revdate", System.Data.OleDb.OleDbType.Date);
//                comm.Parameters.Add("@recdate", System.Data.OleDb.OleDbType.Date);

//                comm.Parameters["@name"].Value = prodname;
//                comm.Parameters["@instrcode"].Value = instrcode;
//                comm.Parameters["@prodcess"].Value = art;
//                comm.Parameters["@machine"].Value = number;
//                comm.Parameters["@startdate"].Value = d;
//                comm.Parameters["@desc"].Value = ret.ToString();
//                comm.Parameters["@inout_id"].Value = id_findby_code(ret.ToString());
//                comm.Parameters["@inout_batch"].Value = in_matbatch;
//                comm.Parameters["@mid_id"].Value = id_findby_code(mid_matcode);
//                comm.Parameters["@mid_batch"].Value = mid_matbatch;
//                comm.Parameters["@inout_pac"].Value = mid_format;
//                comm.Parameters["@mid_pac"].Value = ret.ToString();
//                comm.Parameters["@inout_quan"].Value = Convert.ToInt32(in_amout);//float to int
//                comm.Parameters["@mid_quan"].Value = Convert.ToInt32(mid_amout);//float to int
//                comm.Parameters["@tube_para"].Value = juan_extr;
//                comm.Parameters["@tube_pac"].Value = juan_format;
//                comm.Parameters["@tube_quan"].Value = juan_quan;
//                //comm.Parameters["@pac_label"].Value = extruproc_label_format;
//                //comm.Parameters["@quan_label"].Value = extruproc_label_quan;
//                comm.Parameters["@pac_inner"].Value = doubclean_format;
//                comm.Parameters["@quan_inner"].Value = doubclean_quan;
//                comm.Parameters["@extr"].Value = extra;
//                comm.Parameters["@princ_id"].Value = id_findby_code(chargeman);
//                comm.Parameters["@editor_id"].Value = id_findby_code(compman);
//                comm.Parameters["@rev_id"].Value = id_findby_code(checkman);
//                comm.Parameters["@rec_id"].Value = id_findby_code(recman);
//                comm.Parameters["@editdate"].Value = compdate;
//                comm.Parameters["@revdate"].Value = checkdate;
//                comm.Parameters["@recdate"].Value = recdate;

//                //System.Console.WriteLine(comm.CommandText.ToString());

//                result = comm.ExecuteNonQuery();
//                if (result > 0)
//                {
//                    last_code = instrcode;
//                    MessageBox.Show("添加成功");
//                }
//                else { MessageBox.Show("错误"); }
//            }
//            button2.Enabled = true;
            #endregion
            //判断合法性
            if (textBox21.Text == "" || int.Parse(textBox21.Text) < 20)
            {
                MessageBox.Show("输入不合法");
                textBox21.Text = "";
                return;
            }
            //判断是更新还是插入
            id = getid(textBox2.Text);
            if (id == -1)//进行插入
            {
                // 保存非DataGridView中的数据必须先执行EndEdit;
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                bind();
                id = getid();            
                bind_list(id);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[4].ReadOnly = true;
                fill(id);
                MessageBox.Show("添加成功");
            }
            else//进行更新
            {
                if (index == 0)
                    bind2(id);
                bs_prodinstr.EndEdit();
                da_prodinstr.Update((DataTable)bs_prodinstr.DataSource);
                fill(id);
                MessageBox.Show("更新成功");
            }

        }
        private int id_findby_code(string code)
        {
            if (mainform.isSqlOk)
            {
                return -1;
            }
            else
            {
                string asql = "select user_id from user_aoxing where user_name=" + "'" + code + "'";
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        private int matid_findby_code(string code)
        {
            if (mainform.isSqlOk)
            {
                return -1;
            }
            else
            {
                string asql = "select raw_material_id from raw_material where raw_material_code=" + "'" + code + "'";
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return -1;
                else
                    return Int32.Parse(tempdt.Rows[0][0].ToString());
            }
        }
        private string name_findby_id(int id)
        {
            if (mainform.isSqlOk)
            {
                return "";
            }
            else
            {
                string asql = "select user_name from user_aoxing where user_id=" +id;
                OleDbCommand comm = new OleDbCommand(asql,mySystem.Parameter.connOle);
                OleDbDataAdapter da = new OleDbDataAdapter(comm);

                DataTable tempdt = new DataTable();
                da.Fill(tempdt);
                if (tempdt.Rows.Count == 0)
                    return "";
                else
                    return tempdt.Rows[0][0].ToString();
            }
        }
        //编辑单元格结束后触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 3)
            {
                string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                string pattern = @"^[a-zA-Z]+-[a-zA-Z]+-[0-9]+\*[0-9]";//正则表达式
                if (!Regex.IsMatch(str, pattern))
                {
                    MessageBox.Show("产品代码输入不符合规定，重新输入，例如 PEQ-QE-500*100");
                    //MessageBox.Show("...");
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "";
                    leng = 0;
                    return;
                }
                string[] array = str.Split('*');
                string[] array2 = array[0].Split('-');
                leng = float.Parse(array2[2]);


            }
            //用料重量自己计算
            if (e.ColumnIndex == 7)
            {

                float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = a * leng / 1000.0 * 2 * 0.093;
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
            //if (e.RowIndex == dataGridView1.Rows.Count - 1)
            //    addrows();

            //if (e.ColumnIndex == 3)
            //{
            //    string str = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            //    string pattern = @"^[a-zA-Z]+-[a-zA-Z]+-[0-9]+\*[0-9]";//正则表达式
            //    if (!Regex.IsMatch(str, pattern))
            //    {
            //        MessageBox.Show("产品代码输入不符合规定，重新输入，例如 PEQ-QE-500*100");
            //        //MessageBox.Show("...");
            //        dataGridView1.Rows[e.RowIndex].Cells[3].Value = "";
            //        leng = 0;
            //        return;
            //    }
            //    string[] array = str.Split('*');
            //    string[] array2 = array[0].Split('-');
            //    leng = float.Parse(array2[2]);
            //}
            ////用料重量自己计算
            //if (e.ColumnIndex == 4)
            //{

            //    float a = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            //    dataGridView1.Rows[e.RowIndex].Cells[5].Value = a * leng / 1000.0 * 2 * 0.093;
            //}

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void fill(string prodinstr)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        //datagridview 添加行
        private void button4_Click(object sender, System.EventArgs e)
        {
            DataRow dr = dt_prodlist.NewRow();
            // 如果行有默认值，在这里写代码填上
            dr[1] = id;
            dr[2] = dt_prodlist.Rows.Count+1;
            dt_prodlist.Rows.InsertAt(dr, dt_prodlist.Rows.Count);
        }

        private void textBox21_TextChanged(object sender, System.EventArgs e)
        {

            //if (index == 1)
            //{
            //    if (textBox21.Text == "")
            //        return;
            //    if (int.Parse(textBox21.Text) < 20)
            //    {
            //        MessageBox.Show("输入不合法");
            //        textBox21.Text = "";
            //    }
            //}
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            da_prodlist.Update((DataTable)bs_prodlist.DataSource);
            MessageBox.Show("添加成功");
        }
        //根据id填空
        private void fill(int id)
        {
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = mySystem.Parameter.connOle;
            comm.CommandText = "select * from 生产指令信息表 where ID=" +id;

            OleDbDataAdapter da = new OleDbDataAdapter(comm);
            DataTable tempdt = new DataTable();
            da.Fill(tempdt);
            if (tempdt.Rows.Count == 1)
            {
                textBox1.Text=(string)tempdt.Rows[0][1];
                textBox2.Text = (string)tempdt.Rows[0][2];
                textBox3.Text = (string)tempdt.Rows[0][3]; 
                textBox4.Text = (string)tempdt.Rows[0][4];
                dateTimePicker1.Value = (DateTime)tempdt.Rows[0][5];

                textBox15.Text = (string)tempdt.Rows[0][6]; 
                textBox17.Text = (string)tempdt.Rows[0][7]; 
                textBox19.Text = (string)tempdt.Rows[0][8];
                textBox21.Text = ((double)tempdt.Rows[0][9]).ToString();
                textBox16.Text = (string)tempdt.Rows[0][10]; 
                textBox18.Text = (string)tempdt.Rows[0][11];
                textBox20.Text = (string)tempdt.Rows[0][12];
                textBox22.Text = ((double)tempdt.Rows[0][13]).ToString();
                textBox12.Text = (string)tempdt.Rows[0][14]; 
                textBox13.Text = (string)tempdt.Rows[0][15];
                textBox14.Text = ((double)tempdt.Rows[0][16]).ToString();
                textBox9.Text = (string)tempdt.Rows[0][17];
                textBox11.Text = ((double)tempdt.Rows[0][18]).ToString();
                textBox5.Text = (string)tempdt.Rows[0][19];

                textBox24.Text = (string)tempdt.Rows[0][20];
                dateTimePicker2.Value = (DateTime)tempdt.Rows[0][21];
                textBox25.Text = (string)tempdt.Rows[0][22];
                dateTimePicker3.Value = (DateTime)tempdt.Rows[0][23];
                textBox26.Text = (string)tempdt.Rows[0][24];
                dateTimePicker4.Value = (DateTime)tempdt.Rows[0][25];
                textBox23.Text = (string)tempdt.Rows[0][29];

                da.Dispose();
                tempdt.Dispose();
                //填datagridview,填datatable
                comm.CommandText = "select * from 生产指令产品列表 where 生产指令ID=" + id;
                da = new OleDbDataAdapter(comm);
                da.Fill(dt_prodlist);
                bs_prodlist.DataSource = dt_prodlist;
                da_prodlist.Update((DataTable)bs_prodlist.DataSource);

            }
        }
    }
}
