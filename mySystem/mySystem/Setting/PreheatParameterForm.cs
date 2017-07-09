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
        string tblName = "设置吹膜机组预热参数记录表";

        public PreheatParameterForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            FillNum();
        }

        private void FillNum()
        {
            List<String> readqueryCols = new List<String>(new String[] { "换网预热参数设定1;", "流道预热参数设定1;", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" ,"温度公差"});
            List<String> whereCols = new List<String>(new String[] { "ID" });
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


        public void DataSave()
        {
            string tblName = "设置吹膜机组预热参数记录表";
            List<String> queryCols = new List<String>(new String[] { "温度公差" , "换网预热参数设定1;", "流道预热参数设定1;", "模颈预热参数设定1", "机头1预热参数设定1", "机头2预热参数设定1", 
                    "口模预热参数设定1", "加热保温时间1", "一区预热参数设定1", "二区预热参数设定1", "三区预热参数设定1", "四区预热参数设定1", "换网预热参数设定2", 
                    "流道预热参数设定2", "模颈预热参数设定2", "机头1预热参数设定2", "机头2预热参数设定2", "口模预热参数设定2", "加热保温时间2", "一区预热参数设定2", "二区预热参数设定2", 
                    "三区预热参数设定2", "四区预热参数设定2", "加热保温时间3" });
            List<Object> queryVals = new List<Object>(new Object[] { Convert.ToDouble(tolerance.Text), Convert.ToDouble(hw1.Text), Convert.ToDouble(ld1.Text), 
                Convert.ToDouble(mj1.Text), Convert.ToDouble(jt11.Text), Convert.ToDouble(jt21.Text), Convert.ToDouble(km1.Text), 
                Convert.ToDouble(duration1.Text), Convert.ToDouble(region11.Text), Convert.ToDouble(region21.Text), 
                Convert.ToDouble(region31.Text), Convert.ToDouble(region41.Text), Convert.ToDouble(hw2.Text), Convert.ToDouble(ld2.Text), 
                Convert.ToDouble(mj2.Text), Convert.ToDouble(jt12.Text), Convert.ToDouble(jt22.Text), Convert.ToDouble(km2.Text), 
                Convert.ToDouble(duration2.Text), Convert.ToDouble(region12.Text), Convert.ToDouble(region22.Text), 
                Convert.ToDouble(region32.Text), Convert.ToDouble(region42.Text), Convert.ToDouble(duration3.Text)});
            List<String> whereCols = new List<String>(new String[] { "ID" });
            List<Object> whereVals = new List<Object>(new Object[] { 1 });
            Boolean b = Utility.updateAccess(Parameter.connOle, tblName, queryCols, queryVals, whereCols, whereVals);
            if (!b)
            { MessageBox.Show("预热参数设置保存失败！","错误");}

        }


    }
}
