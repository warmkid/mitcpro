using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace mySystem.Process.Bag.LDPE
{
    public partial class 查询 : BaseForm
    {
        List<string> ls膜材代码;
        List<String> ls产品代码;
        List<Hashtable> MainInfo;
        
        public 查询()
        {
            InitializeComponent();
            setDefaultValueForConsoles();
            // 给两个代码下拉框增加自动补全
            ls膜材代码 = get膜材代码();
            ls产品代码 = get产品代码();
            set自动补全();
            readDGVWidthFromSettingAndSet(dgv总);
            readDGVWidthFromSettingAndSet(dgv膜);
            readDGVWidthFromSettingAndSet(dgv内);
            readDGVWidthFromSettingAndSet(dgv外);
        }

        void setDefaultValueForConsoles()
        {
            dtpStart.Value = DateTime.Now.Date.AddDays(-7);
            dtpEnd.Value = DateTime.Now.Date;
        }

        void set自动补全()
        {
            set自动补全ForTextBox(tb产品代码, ls产品代码);
            set自动补全ForTextBox(tb膜材代码, ls膜材代码);
        }

        void set自动补全ForTextBox(TextBox tb, List<string> src)
        {
            AutoCompleteStringCollection acsc;
            acsc = new AutoCompleteStringCollection();
            acsc.AddRange(src.ToArray());
            tb.AutoCompleteCustomSource = acsc;
            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        List<string> get膜材代码()
        {
            return Utility.get代码By类型And工序("组件", "PE制袋");
        }

        List<string> get产品代码()
        {
            return Utility.get代码By类型And工序("成品", "PE制袋");
        }

        


        private void btn膜材查询_Click(object sender, EventArgs e)
        {
            constructHashTableForQuery();
            // 清空所有dgv
            clearDGVs();
            displayByMocai();
            


        }

        void displayByMocai()
        {
            // 对应左边显示信息
            Hashtable ht = new Hashtable();
            // {代码&批号:数量}
            List<Int32> 要显示的产品信息索引 = new List<int>();
            for (int i = 0; i < MainInfo.Count; ++i)
            {
                List<Hashtable> ylxx = (List<Hashtable>)MainInfo[i]["用料信息"];
                foreach (Hashtable h in ylxx)
                {
                    if (h["类型"].ToString() == "膜材" &&
                        (h["物料代码"].ToString().Contains(tb膜材代码.Text) || tb膜材代码.Text.Trim() == "") &&
                        (h["物料批号"].ToString().Contains(tb膜材批号.Text) || tb膜材批号.Text.Trim() == ""))
                    {
                        if (!要显示的产品信息索引.Contains(i))
                        {
                            要显示的产品信息索引.Add(i);
                        }
                        string key = h["物料代码"].ToString() + "@" + h["物料批号"].ToString();
                        if (!ht.ContainsKey(key))
                        {
                            ht.Add(key, 0);
                        }
                        ht[key] = Convert.ToDouble(h[key]) + Convert.ToDouble(h["领取数量"]);
                    }
                }
            }

            // 显示
            // 膜材
            foreach (string key in ht.Keys.OfType<String>())
            {
                int idx = dgv膜.Rows.Add(1);
                string[] t = key.Split('@');
                dgv膜.Rows[idx].Cells["膜材物料代码"].Value = t[0];
                dgv膜.Rows[idx].Cells["膜材物料批号"].Value = t[1];
                dgv膜.Rows[idx].Cells["膜材数量"].Value = Convert.ToDouble(ht[key]);
            }
            // 产品
            foreach (int nidx in 要显示的产品信息索引)
            {
                int idx = dgv总.Rows.Add(1);
                string 生产指令 = MainInfo[nidx]["生产指令编号"].ToString();
                string 产品代码 = MainInfo[nidx]["产品代码"].ToString();
                string 产品批号 = MainInfo[nidx]["产品批号"].ToString();
                double 膜材用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "膜材");
                double 内包用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "内包");
                double 外包用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "外包");
                int 生产数量 = Convert.ToInt32(MainInfo[nidx]["产量(只)"].ToString());
                double 膜材平米 = get膜材总面积((List<Hashtable>)MainInfo[nidx]["用料信息"]);
                double 生产面积 = getArea(产品代码, 生产数量, 1);
                dgv总.Rows[idx].Cells["生产指令"].Value = 生产指令;
                dgv总.Rows[idx].Cells["产品代码"].Value = 产品代码;
                dgv总.Rows[idx].Cells["产品批号"].Value = 产品批号;
                dgv总.Rows[idx].Cells["膜材用量米"].Value = 膜材用量;
                dgv总.Rows[idx].Cells["膜材用量平米"].Value = 膜材平米;
                dgv总.Rows[idx].Cells["内包用量"].Value = 内包用量;
                dgv总.Rows[idx].Cells["外包用量"].Value = 外包用量;
                dgv总.Rows[idx].Cells["生产数量包"].Value = 生产数量;
                dgv总.Rows[idx].Cells["生产数量平米"].Value = 生产面积;
                dgv总.Rows[idx].Cells["收率"].Value = (Convert.ToDouble(dgv总.Rows[idx].Cells["生产数量平米"].Value) /
                    Convert.ToDouble(dgv总.Rows[idx].Cells["膜材用量平米"].Value) * 100).ToString("F2") + "%";
            }
            

        }

        
        double get总用料量(List<Hashtable> lht, string type)
        {
            double ret = 0;
            foreach (Hashtable ht in lht)
            {
                if(ht["类型"].ToString()==type){
                    ret += Convert.ToDouble(ht["领取数量"]);
                }   
            }
            return ret;
        }

        double get膜材总面积(List<Hashtable> lht)
        {
            double ret = 0;
            foreach (Hashtable ht in lht)
            {
                if (ht["类型"].ToString() == "膜材")
                {
                    ret += getArea(ht["物料代码"].ToString(), Convert.ToInt32(ht["领取数量"]), 0);
                }
            }
            return ret;
        }


        private void btn产品查询_Click(object sender, EventArgs e)
        {
            // 代码和批号不能为空
            constructHashTableForQuery();
            // 清空所有dgv
            clearDGVs();
            displayByChanpin();
        }

        void displayByChanpin()
        {
            // 对应左边显示信息
            Hashtable htM = new Hashtable();
            Hashtable htN = new Hashtable();
            Hashtable htW = new Hashtable();
            // {代码&批号:数量}
            List<Int32> 要显示的产品信息索引 = new List<int>();
            for (int i = 0; i < MainInfo.Count; ++i)
            {
                if ((MainInfo[i]["产品代码"].ToString().Contains(tb产品代码.Text) || tb产品代码.Text.Trim() == "") &&
                    (MainInfo[i]["产品批号"].ToString().Contains(tb产品批号.Text) || tb产品批号.Text.Trim() == ""))
                {
                    要显示的产品信息索引.Add(i);


                    List<Hashtable> lht = (List<Hashtable>)MainInfo[i]["用料信息"];
                    foreach (Hashtable h in lht)
                    {
                        string key = h["物料代码"].ToString() + "@" + h["物料批号"].ToString();
                        switch (h["类型"].ToString())
                        {
                            case "膜材":
                                if (!htM.ContainsKey(key))
                                {
                                    htM.Add(key, 0);
                                }
                                htM[key] = Convert.ToDouble(h[key]) + Convert.ToDouble(h["领取数量"]);
                                break;
                            case "内包":
                                if (!htN.ContainsKey(key))
                                {
                                    htN.Add(key, 0);
                                }
                                htN[key] = Convert.ToDouble(h[key]) + Convert.ToDouble(h["领取数量"]);
                                break;
                            case "外包":
                                if (!htW.ContainsKey(key))
                                {
                                    htW.Add(key, 0);
                                }
                                htW[key] = Convert.ToDouble(h[key]) + Convert.ToDouble(h["领取数量"]);
                                break;
                        }
                    }

                }
            }


            // 显示
            // 膜材
            foreach (string key in htM.Keys.OfType<String>())
            {
                int idx = dgv膜.Rows.Add(1);
                string[] t = key.Split('@');
                dgv膜.Rows[idx].Cells["膜材物料代码"].Value = t[0];
                dgv膜.Rows[idx].Cells["膜材物料批号"].Value = t[1];
                dgv膜.Rows[idx].Cells["膜材数量"].Value = Convert.ToDouble(htM[key]);
            }
            // 内包
            foreach (string key in htN.Keys.OfType<String>())
            {
                int idx = dgv内.Rows.Add(1);
                string[] t = key.Split('@');
                dgv内.Rows[idx].Cells["内包物料代码"].Value = t[0];
                dgv内.Rows[idx].Cells["内包物料批号"].Value = t[1];
                dgv内.Rows[idx].Cells["内包数量"].Value = Convert.ToDouble(htN[key]);
            }
            // 外
            foreach (string key in htW.Keys.OfType<String>())
            {
                int idx = dgv外.Rows.Add(1);
                string[] t = key.Split('@');
                dgv外.Rows[idx].Cells["外包物料代码"].Value = t[0];
                dgv外.Rows[idx].Cells["外包物料批号"].Value = t[1];
                dgv外.Rows[idx].Cells["外包数量"].Value = Convert.ToDouble(htW[key]);
            }
            // 产品
            foreach (int nidx in 要显示的产品信息索引)
            {
                int idx = dgv总.Rows.Add(1);
                string 生产指令 = MainInfo[nidx]["生产指令编号"].ToString();
                string 产品代码 = MainInfo[nidx]["产品代码"].ToString();
                string 产品批号 = MainInfo[nidx]["产品批号"].ToString();
                double 膜材用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "膜材");
                double 内包用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "内包");
                double 外包用量 = get总用料量((List<Hashtable>)MainInfo[nidx]["用料信息"], "外包");
                int 生产数量 = Convert.ToInt32(MainInfo[nidx]["产量(只)"].ToString());
                double 膜材平米 = get膜材总面积((List<Hashtable>)MainInfo[nidx]["用料信息"]);
                double 生产面积 = getArea(产品代码, 生产数量, 1);
                dgv总.Rows[idx].Cells["生产指令"].Value = 生产指令;
                dgv总.Rows[idx].Cells["产品代码"].Value = 产品代码;
                dgv总.Rows[idx].Cells["产品批号"].Value = 产品批号;
                dgv总.Rows[idx].Cells["膜材用量米"].Value = 膜材用量;
                dgv总.Rows[idx].Cells["膜材用量平米"].Value = 膜材平米;
                dgv总.Rows[idx].Cells["内包用量"].Value = 内包用量;
                dgv总.Rows[idx].Cells["外包用量"].Value = 外包用量;
                dgv总.Rows[idx].Cells["生产数量包"].Value = 生产数量;
                dgv总.Rows[idx].Cells["生产数量平米"].Value = 生产面积;
                dgv总.Rows[idx].Cells["收率"].Value = (Convert.ToDouble(dgv总.Rows[idx].Cells["生产数量平米"].Value) /
                    Convert.ToDouble(dgv总.Rows[idx].Cells["膜材用量平米"].Value) * 100).ToString("F2") + "%";
            }

        }

        void clearDGVs()
        {
            clearDGV(dgv总);
            clearDGV(dgv膜);
            clearDGV(dgv内);
            clearDGV(dgv外);
        }

        void clearDGV(DataGridView dgv)
        {
            if (dgv.DataSource != null)
            {
                DataTable dt = (DataTable)dgv.DataSource;
                dt.Rows.Clear();
                dgv.DataSource = dt;
            }
            else
            {
                dgv.Rows.Clear();
            }
        }




        void constructHashTableForQuery()
        {
            // hashtable 格式：
            //[
            //{
            //    "生产指令id": 1,
            //    "生产指令编号": "B-PEF-2018",
            //    "产品代码": "dm",
            //    "产品批号": "ph",
            //    "内包id": [
            //        1,
            //        2,
            //        3
            //    ],
            //    "外包id": [
            //        1,
            //        2,
            //        3
            //    ],
            //    "领料id": [
            //        1,
            //        2
            //    ],
            //    "用料信息": [
            //        {
            //            "类型": "内包",
            //            "物料代码": "pef-bs",
            //            "物料批号": "123",
            //            "用量": 111，
            //            "平米":膜材类型才有这个
            //        }
            //    ],
            //    "工时": 111,
            //    "产量(只)": 3
            //}
            //]


            // 主要要处理数据为空的情况

            // 1. 根据日期读取相关联的生产指令
            List<Int32> InstrIDs = getInsIds();

            // 2. 构建好Hashtable
            MainInfo = new List<Hashtable>();
            foreach (int id in InstrIDs)
            {
                Hashtable hs = new Hashtable();
                hs.Add("生产指令ID", id);              
                MainInfo.Add(hs);
            }

            
            for (int i = 0; i < MainInfo.Count; ++i)
            {
                // 3. 读取每个指令，记录其中的  生产指令名称， 产品代码 ， 产品批号
                fillByInstrId(i);
                // 4. 读内包记录，填写 内包id, 总产量, 总工时
                fillByInnerBag(i);
                //
                fillByOuterBag(i);
                fillByMaterial(i);
            }

           
            


        }

        List<int> getInsIds()
        {
            string sql;
            SqlDataAdapter da;
            DataTable dt;
            
            sql = "select  distinct(生产指令Id) from 产品内包装记录 where 生产日期>='{0}' and 生产日期<='{1}' order by 生产指令Id ASC";
            da = new SqlDataAdapter(string.Format(sql, dtpStart.Value.ToString("yyyy/MM/dd"), dtpEnd.Value.ToString("yyyy/MM/dd")), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            List<int> ret = new List<int>();
            foreach(DataRow dr in dt.Rows){
                ret.Add(Convert.ToInt32( dr["生产指令Id"].ToString()));
            }
            return ret;
        }

        // 参数是list中的序号
        void fillByInstrId(int idx)
        {
            // 生产指令名称， 产品代码 ， 产品批号
            string sql;
            SqlDataAdapter da;
            DataTable dt;

            int instrid = Convert.ToInt32(MainInfo[idx]["生产指令ID"]);

            sql = "select * from 生产指令 where ID={0}";
            da = new SqlDataAdapter(string.Format(sql, instrid), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count==0)    {
                MessageBox.Show("生产指令信息读取失败:" + instrid);
                return;
            }

            // 生产指令名称
            MainInfo[idx].Add("生产指令编号", dt.Rows[0]["生产指令编号"]);



            sql = "select * from 生产指令详细信息 where T生产指令ID={0}";
            da = new SqlDataAdapter(string.Format(sql, instrid), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("生产指令信息读取失败:" + instrid);
                return;
            }
            // 产品代码 ， 产品批号
            MainInfo[idx].Add("产品代码", dt.Rows[0]["产品代码"]);
            MainInfo[idx].Add("产品批号", dt.Rows[0]["产品批号"]);


        }



        void fillByInnerBag(int idx)
        {
            // 内包id， 总产量， 总工时
            string sql;
            SqlDataAdapter da;
            DataTable dt;

            int instrid = Convert.ToInt32(MainInfo[idx]["生产指令ID"]);
            sql = "select * from 产品内包装记录 where 生产指令ID={0} order by ID ASC";
            da = new SqlDataAdapter(string.Format(sql, instrid), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);

            // 内包id
            List<Int32> 内包ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                内包ids.Add(Convert.ToInt32(dr["ID"]));
            }
            MainInfo[idx].Add("内包id", 内包ids);


            // 总产量,总工时
            int 总产量只 = 0;
            double 总工时 = 0.0;
            foreach (DataRow dr in dt.Rows)
            {
                总产量只 += Convert.ToInt32( dr["产品数量只合计B"]);
                总工时 += Convert.ToDouble(dr["工时"]);
            }
            MainInfo[idx].Add("产量(只)",总产量只);
            MainInfo[idx].Add("工时",总工时);
            
        }


        void fillByOuterBag(int idx)
        {
            // 外包id
            string sql;
            SqlDataAdapter da;
            DataTable dt;

            int instrid = Convert.ToInt32(MainInfo[idx]["生产指令ID"]);
            sql = "select * from 产品外包装记录表 where 生产指令ID={0} order by ID ASC";
            da = new SqlDataAdapter(string.Format(sql, instrid), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);

            // 外包id
            List<Int32> 外包ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                外包ids.Add(Convert.ToInt32(dr["ID"]));
            }
            MainInfo[idx].Add("外包id", 外包ids);
        }

        void fillByMaterial(int idx)
        {
            // 领料ids
            // 领料详细信息
            string sql;
            SqlDataAdapter da;
            DataTable dt;

            int instrid = Convert.ToInt32(MainInfo[idx]["生产指令ID"]);
            sql = "select * from 生产领料使用记录 where 生产指令ID={0} order by ID ASC";
            da = new SqlDataAdapter(string.Format(sql, instrid), mySystem.Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);

            // 领料id
            List<Int32> 领料ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                领料ids.Add(Convert.ToInt32(dr["ID"]));
            }
            MainInfo[idx].Add("领料id", 领料ids);


            // 领料详细信息
            // select SUM(领取数量)-SUM(退库数量) as 领取数量, 物料代码,物料批号 from 生产领料使用记录详细信息 
            // where id=1 or id=2 or id=3
            // group by 物料代码,物料批号
            sql = @"select 生产领料使用记录.类型, SUM(生产领料使用记录详细信息.领取数量)-SUM(生产领料使用记录详细信息.退库数量) as" +
                " 领取数量, 生产领料使用记录详细信息.物料代码 as 物料代码, 生产领料使用记录详细信息.物料批号 as 物料批号" +
                " from 生产领料使用记录,生产领料使用记录详细信息 where ({0}) and 生产领料使用记录.ID=生产领料使用记录详细信息.T生产领料使用记录ID" +
                "  group by 生产领料使用记录详细信息.物料代码,生产领料使用记录详细信息.物料批号,生产领料使用记录.类型 ";
            da = new SqlDataAdapter(string.Format(sql, genOrSqlCommand(领料ids)), Parameter.conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Hashtable> matinfo = new List<Hashtable>();
            foreach (DataRow dr in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("类型", dr["类型"]);
                ht.Add("物料代码", dr["物料代码"]);
                ht.Add("物料批号", dr["物料批号"]);
                ht.Add("领取数量", dr["领取数量"]);
                if (ht["类型"].ToString() == "膜材")
                {

                    ht.Add("平米", getArea(ht["物料代码"].ToString(), Convert.ToInt32(ht["领取数量"]), 0));
                }
                matinfo.Add(ht);
            }
            MainInfo[idx].Add("用料信息", matinfo);
            
        }


        // type=0 膜材，代码和米数
        // type=1 产品，代码和包数
        public static double getArea(string code, int num, int type)
        {
            int[] KandG = mySystem.Process.Bag.LDPE.LDPEBag_dailyreport.getChangAndKuan(code);
            int w = KandG[0];
            int l = KandG[1];
            string[] codes = code.Split('-');
            double area = 0;
            if (type == 0) // 膜材  数量x宽
            {
                if (codes.Length <= 1 || (codes[1].StartsWith("S")))
                {
                    area = num / 1000.0 * w;
                }
                else
                {
                    area = num / 1000.0 * 2 * w;
                }
            }
            else
            {
                if (codes.Length <= 1 || (codes[1].StartsWith("S")))
                {
                    area = num / 1000000.0 * w * l;
                }
                else
                {
                    area = num / 1000000.0 * 2 * w * l;
                }
            }
           
            return area;
        }


        string genOrSqlCommand(List<Int32> ids)
        {
            List<string> tmp = new List<string>();
            foreach (int id in ids)
            {
                string t = " T生产领料使用记录ID=" + id + " ";
                tmp.Add(t);
            }
            return String.Join("or", tmp);
        }

       

        private void writeDGVColumnSettings(object sender, DataGridViewColumnEventArgs e)
        {
            writeDGVWidthToSetting((DataGridView)sender);   
        }

        private void dgv总_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string instr = dgv总.SelectedCells[0].OwningRow.Cells["生产指令"].Value.ToString();
            string[] 物料信息 = get关联的所有物料信息(instr);
            clearDGVColor(dgv膜);
            clearDGVColor(dgv内);
            clearDGVColor(dgv外);
            // 膜材
            foreach (string k in 物料信息[0].Split('#'))
            {
                if (k == "") continue;
                setDGV膜背景By产品And批号( k.Split('@')[0], k.Split('@')[1]);
            }
            // 内包
            foreach (string k in 物料信息[1].Split('#'))
            {
                if (k == "") continue;
                setDGV内背景By产品And批号(k.Split('@')[0], k.Split('@')[1]);
            }
            // 外包
            foreach (string k in 物料信息[2].Split('#'))
            {
                if (k == "") continue;
                setDGV外背景By产品And批号(k.Split('@')[0], k.Split('@')[1]);
            }
        }

        string[] get关联的所有物料信息(string code)
        {
            string[] ret = new string[3];
            foreach (Hashtable ht in MainInfo)
            {
                if (ht["生产指令编号"].ToString() == code)
                {
                    List<Hashtable> lht = (List<Hashtable>)ht["用料信息"];
                    ret[0] = get关联的物料信息(lht, "膜材");
                    ret[1] = get关联的物料信息(lht, "内包");
                    ret[2] = get关联的物料信息(lht, "外包");
                    return ret;
                }
            }
            return ret;
        }

        string get关联的物料信息(List<Hashtable> lht, string type)
        {
            List<string> lret = new List<string>();
            foreach (Hashtable ht in lht)
            {
                if (ht["类型"].ToString() == type)
                {
                    lret.Add(ht["物料代码"].ToString() + "@" + ht["物料批号"].ToString());
                }
            }
            return string.Join("#",lret);
        }
        void clearDGVColor(DataGridView dgv)
        {
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                dgvr.DefaultCellStyle.BackColor = Color.White;
            }
        }
        void setDGV膜背景By产品And批号(string code, string batch)
        {
            foreach (DataGridViewRow dgvr in dgv膜.Rows)
            {
                if (dgvr.Cells["膜材物料代码"].Value.ToString() == code &&
                    dgvr.Cells["膜材物料批号"].Value.ToString() == batch)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
        void setDGV内背景By产品And批号(string code, string batch)
        {
            foreach (DataGridViewRow dgvr in dgv内.Rows)
            {
                if (dgvr.Cells["内包物料代码"].Value.ToString() == code &&
                    dgvr.Cells["内包物料批号"].Value.ToString() == batch)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
        void setDGV外背景By产品And批号(string code, string batch)
        {
            foreach (DataGridViewRow dgvr in dgv外.Rows)
            {
                if (dgvr.Cells["外包物料代码"].Value.ToString() == code &&
                    dgvr.Cells["外包物料批号"].Value.ToString() == batch)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
        void setDGV总背景By产品And批号(string code, string batch)
        {
            foreach (DataGridViewRow dgvr in dgv总.Rows)
            {
                if (dgvr.Cells["产品代码"].Value.ToString() == code &&
                    dgvr.Cells["产品批号"].Value.ToString() == batch)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void dgv膜_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            clearDGVColor(dgv膜);
            clearDGVColor(dgv内);
            clearDGVColor(dgv外);

            string code = dgv膜.SelectedCells[0].OwningRow.Cells["膜材物料代码"].Value.ToString();
            string batch = dgv膜.SelectedCells[0].OwningRow.Cells["膜材物料批号"].Value.ToString();
            foreach (Hashtable ht in MainInfo)
            {
                foreach (Hashtable matht in (List<Hashtable>)ht["用料信息"])
                {
                    if (matht["类型"].ToString() == "膜材" &&
                        matht["物料代码"].ToString() == code &&
                        matht["物料批号"].ToString() == batch)
                    {
                        clearDGVColor(dgv总);
                        setDGV总背景By产品And批号(ht["产品代码"].ToString(), ht["产品批号"].ToString());
                    }
                }
            }
        }

        private void dgv内_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            clearDGVColor(dgv膜);
            clearDGVColor(dgv内);
            clearDGVColor(dgv外);
            string code = dgv内.SelectedCells[0].OwningRow.Cells["内包物料代码"].Value.ToString();
            string batch = dgv内.SelectedCells[0].OwningRow.Cells["内包物料批号"].Value.ToString();
            foreach (Hashtable ht in MainInfo)
            {
                foreach (Hashtable matht in (List<Hashtable>)ht["用料信息"])
                {
                    if (matht["类型"].ToString() == "内包" &&
                        matht["物料代码"].ToString() == code &&
                        matht["物料批号"].ToString() == batch)
                    {
                        clearDGVColor(dgv总);
                        setDGV总背景By产品And批号(ht["产品代码"].ToString(), ht["产品批号"].ToString());
                    }
                }
            }
        }

        private void dgv外_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            clearDGVColor(dgv膜);
            clearDGVColor(dgv内);
            clearDGVColor(dgv外);
            string code = dgv外.SelectedCells[0].OwningRow.Cells["外包物料代码"].Value.ToString();
            string batch = dgv外.SelectedCells[0].OwningRow.Cells["外包物料批号"].Value.ToString();
            foreach (Hashtable ht in MainInfo)
            {
                foreach (Hashtable matht in (List<Hashtable>)ht["用料信息"])
                {
                    if (matht["类型"].ToString() == "外包" &&
                        matht["物料代码"].ToString() == code &&
                        matht["物料批号"].ToString() == batch)
                    {
                        clearDGVColor(dgv总);
                        setDGV总背景By产品And批号(ht["产品代码"].ToString(), ht["产品批号"].ToString());
                    }
                }
            }
        }
    }
}
