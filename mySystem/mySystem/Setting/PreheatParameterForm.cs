using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace mySystem
{
    public partial class PreheatParameterForm : BaseForm
    {
        string tblName = "extrusion_s3_preheat";

        public PreheatParameterForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            FillNum();
        }

        private void FillNum()
        {
            List<String> readqueryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_duration1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_duration2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2", "s3_duration3" ,"s3_temperature_tolerance"});
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            List<List<Object>> queryValsList = Utility.selectAccess(Parameter.connOle, tblName, readqueryCols, whereCols, whereVals, null, null, null, null, null);

            List<String> data = new List<String> { };
            for (int i = 0; i < queryValsList[0].Count; i++)
            {
                data.Add(queryValsList[0][i].ToString());
            }
            List<Control> textboxes = new List<Control> { hw1, ld1, mj1, jt11, jt21, km1, duration1, region11, region21, region31, region41, hw2, 
                    ld2, mj2, jt12, jt22, km2, duration2, region12, region22, region32, region42, duration3, tolerance};
            Utility.fillControl(textboxes, data);


        }


        //保存公差到数据库
        private void SaveTolerance() 
        {            
            List<String> queryCols = new List<String>(new String[] { "s3_temperature_tolerance" });
            List<Object> queryVals = new List<Object>(new Object[] { Convert.ToInt32(tolerance.Text) });
            List<String> whereCols = new List<String>(new String[] { "id" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(Parameter.connOle, tblName, queryCols, queryVals, whereCols, whereVals);
 
        }

        private Boolean CheckNum()
        {
            //List<String> queryCols = new List<String>(new String[] { "s3_temperature_tolerance" });
            //List<String> whereCols = new List<String>(new String[] { "id" });
            //List<Object> whereVals = new List<Object>(new Object[] { 1 });
            //List<List<Object>> tolerancelist = selectAccess(base.mainform.connOle, tblName, queryCols, whereCols, whereVals);

            int tolerance = 0;
            OleDbCommand comm = new OleDbCommand();
            comm.Connection = Parameter.connOle;
            comm.CommandText = "select * from extrusion_s3_preheat where id= 1";
            OleDbDataReader myReader = comm.ExecuteReader();
            while (myReader.Read())
            {
                tolerance = myReader.GetInt32(13); //读取公差
            }
            
            myReader.Close();
            comm.Dispose();

            Boolean[] check=new Boolean[23];

            if (120 - tolerance <= Convert.ToInt32(hw1.Text) && Convert.ToInt32(hw1.Text) <= 120 + tolerance)
            { check[0] = true; }
            else
            {
                check[0] = false;
                MessageBox.Show("换网预热参数超出允许范围，请重新设置");
            }


            if (120 - tolerance <= Convert.ToInt32(ld1.Text) && Convert.ToInt32(ld1.Text) <= 120 + tolerance)
            { check[1] = true; }
            else
            {
                check[1] = false;
                MessageBox.Show("流道预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(mj1.Text) && Convert.ToInt32(mj1.Text) <= 120 + tolerance)
            { check[2] = true; }
            else
            {
                check[2] = false;
                MessageBox.Show("模颈预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(jt11.Text) && Convert.ToInt32(jt11.Text) <= 120 + tolerance)
            { check[3] = true; }
            else
            {
                check[3] = false;
                MessageBox.Show("机头一预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(jt21.Text) && Convert.ToInt32(jt21.Text) <= 120 + tolerance)
            { check[4] = true; }
            else
            {
                check[4] = false;
                MessageBox.Show("机头二预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(km1.Text) && Convert.ToInt32(km1.Text) <= 120 + tolerance)
            { check[5] = true; }
            else
            {
                check[5] = false;
                MessageBox.Show("口模预热参数超出允许范围，请重新设置");
            }

            if (60 - tolerance <= Convert.ToInt32(duration1.Text) && Convert.ToInt32(duration1.Text) <= 60 + tolerance)
            { check[6] = true; }
            else
            {
                check[6] = false;
                MessageBox.Show("加热保温时间超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(region11.Text) && Convert.ToInt32(region11.Text) <= 120 + tolerance)
            { check[7] = true; }
            else
            {
                check[7] = false;
                MessageBox.Show("I区预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(region21.Text) && Convert.ToInt32(region21.Text) <= 120 + tolerance)
            { check[8] = true; }
            else
            {
                check[8] = false;
                MessageBox.Show("II区预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(region31.Text) && Convert.ToInt32(region31.Text) <= 120 + tolerance)
            { check[9] = true; }
            else
            {
                check[9] = false;
                MessageBox.Show("III区预热参数超出允许范围，请重新设置");
            }

            if (120 - tolerance <= Convert.ToInt32(region41.Text) && Convert.ToInt32(region41.Text) <= 120 + tolerance)
            { check[10] = true; }
            else
            {
                check[10] = false;
                MessageBox.Show("IV区预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(hw2.Text) && Convert.ToInt32(hw2.Text) <= 215 + tolerance)
            { check[11] = true; }
            else
            {
                check[11] = false;
                MessageBox.Show("换网预热参数超出允许范围，请重新设置");
            }


            if (215 - tolerance <= Convert.ToInt32(ld2.Text) && Convert.ToInt32(ld2.Text) <= 215 + tolerance)
            { check[12] = true; }
            else
            {
                check[12] = false;
                MessageBox.Show("流道预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(mj2.Text) && Convert.ToInt32(mj2.Text) <= 215 + tolerance)
            { check[13] = true; }
            else
            {
                check[13] = false;
                MessageBox.Show("模颈预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(jt12.Text) && Convert.ToInt32(jt12.Text) <= 215 + tolerance)
            { check[14] = true; }
            else
            {
                check[14] = false;
                MessageBox.Show("机头一预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(jt22.Text) && Convert.ToInt32(jt22.Text) <= 215 + tolerance)
            { check[15] = true; }
            else
            {
                check[15] = false;
                MessageBox.Show("机头二预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(km2.Text) && Convert.ToInt32(km2.Text) <= 215 + tolerance)
            { check[16] = true; }
            else
            {
                check[16] = false;
                MessageBox.Show("口模预热参数超出允许范围，请重新设置");
            }

            if (40 - tolerance <= Convert.ToInt32(duration2.Text) && Convert.ToInt32(duration2.Text) <= 40 + tolerance)
            { check[17] = true; }
            else
            {
                check[17] = false;
                MessageBox.Show("加热保温时间超出允许范围，请重新设置");
            }

            if (180 - tolerance <= Convert.ToInt32(region12.Text) && Convert.ToInt32(region12.Text) <= 180 + tolerance)
            { check[18] = true; }
            else
            {
                check[18] = false;
                MessageBox.Show("I区预热参数超出允许范围，请重新设置");
            }

            if (195 - tolerance <= Convert.ToInt32(region22.Text) && Convert.ToInt32(region22.Text) <= 195 + tolerance)
            { check[19] = true; }
            else
            {
                check[19] = false;
                MessageBox.Show("II区预热参数超出允许范围，请重新设置");
            }

            if (210 - tolerance <= Convert.ToInt32(region32.Text) && Convert.ToInt32(region32.Text) <= 210 + tolerance)
            { check[20] = true; }
            else
            {
                check[20] = false;
                MessageBox.Show("III区预热参数超出允许范围，请重新设置");
            }

            if (215 - tolerance <= Convert.ToInt32(region42.Text) && Convert.ToInt32(region42.Text) <= 215 + tolerance)
            { check[21] = true; }
            else
            {
                check[21] = false;
                MessageBox.Show("IV区预热参数超出允许范围，请重新设置");
            }

            if (40 - tolerance <= Convert.ToInt32(duration3.Text) && Convert.ToInt32(duration3.Text) <= 40 + tolerance)
            { check[22] = true; }
            else
            {
                check[22] = false;
                MessageBox.Show("加热保温时间超出允许范围，请重新设置");
            }


            if (check[0] == check[1] == check[2] == check[3] == check[4] == check[5]
                == check[6] == check[7] ==check[8] == check[9] == check[10] == check[11]
                == check[12] == check[13] == check[14] == check[15] == check[16] == check[17]
                == check[18] == check[19] == check[20] == check[21] == check[22] == true)
            { 
                return true; 
            }
            else
            { 
                return false; 
            }
            
        }


        public void DataSave()
        {
            SaveTolerance(); //保存公差
            //bool isNumOk = CheckNum();  //检验数据
            bool isNumOk = true;
            if (isNumOk)
            {
                string tblName = "extrusion_s3_preheat";
                List<String> queryCols = new List<String>(new String[] { "s3_hw_set1", "s3_ld_set1", "s3_mj_set1", "s3_jt1_set1", "s3_jt2_set1", 
                    "s3_km_set1", "s3_duration1", "s3_region1_set1", "s3_region2_set1", "s3_region3_set1", "s3_region4_set1", "s3_hw_set2", 
                    "s3_ld_set2", "s3_mj_set2", "s3_jt1_set2", "s3_jt2_set2", "s3_km_set2", "s3_duration2", "s3_region1_set2", "s3_region2_set2", 
                    "s3_region3_set2", "s3_region4_set2", "s3_duration3" });
                List<Object> queryVals = new List<Object>(new Object[] { Convert.ToInt32(hw1.Text), Convert.ToInt32(ld1.Text), 
                    Convert.ToInt32(mj1.Text), Convert.ToInt32(jt11.Text), Convert.ToInt32(jt21.Text), Convert.ToInt32(km1.Text), 
                    Convert.ToInt32(duration1.Text), Convert.ToInt32(region11.Text), Convert.ToInt32(region21.Text), 
                    Convert.ToInt32(region31.Text), Convert.ToInt32(region41.Text), Convert.ToInt32(hw2.Text), Convert.ToInt32(ld2.Text), 
                    Convert.ToInt32(mj2.Text), Convert.ToInt32(jt12.Text), Convert.ToInt32(jt22.Text), Convert.ToInt32(km2.Text), 
                    Convert.ToInt32(duration2.Text), Convert.ToInt32(region12.Text), Convert.ToInt32(region22.Text), 
                    Convert.ToInt32(region32.Text), Convert.ToInt32(region42.Text), Convert.ToInt32(duration3.Text)});
                List<String> whereCols = new List<String>(new String[] { "id" });
                List<Object> whereVals = new List<Object>(new Object[] { 1 });
                Boolean b = Utility.updateAccess(Parameter.connOle, tblName, queryCols, queryVals, whereCols, whereVals);
                //MessageBox.Show("预热参数设置保存成功！", "success");
            }

            else
            {
                MessageBox.Show("预热参数设置保存失败！","错误");
                return;
            }

        }


    }
}
