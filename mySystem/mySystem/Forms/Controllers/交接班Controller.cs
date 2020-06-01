using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace mySystem.Controllers
{
    class 交接班Controller:BaseController
    {
        String _otn; // outer table name
        String _itn; // inner table name
        String SQL_GET_LASTEST_RECORD = @"select top 1 * from {0} where {1}='{2}' order by id desc";
        String SQL_CREATE_NEW_RECORD = @"select * from {0} where {1}={2} and {3}={4} and {5}={6}";
        String SQL_SELECT_INNER = @"select * from {0} where {1}={2}";
        String SQL_SELECT_OUTER = @"select * from {0} where {1}={2}";

        public 交接班Controller(String otbl, String itbl, SqlConnection c)
            : base(c)
        {
            _otn = otbl;
            _itn = itbl;

        }

        public DataTable getLastest(int instructID)
        {
            String sql = String.Format(SQL_GET_LASTEST_RECORD, _otn, "生产指令ID", instructID);
            return base.query(sql);

        }

        public void readById(int id, out DataTable dtouter, out DataTable dtinner)
        {
            String sql;
            SqlDataAdapter da;
            // outer
            sql = String.Format(SQL_SELECT_OUTER, _otn, "ID", id);
            da = new SqlDataAdapter(sql, base._conn);
            dtouter = new DataTable();
            da.Fill(dtouter);
            int outid = Convert.ToInt32(dtouter.Rows[0]["ID"]);

            // inner
            sql = String.Format(SQL_SELECT_INNER, _itn, "T吹膜岗位交接班记录ID", outid);
            da = new SqlDataAdapter(sql, base._conn);
            dtinner = new DataTable();
            da.Fill(dtinner);
        }

        public bool createNewRecord(int iid, string ins, DateTime time, DataTable setting)
        {
            String sql = string.Format(SQL_CREATE_NEW_RECORD, _otn, "生产指令ID",iid,
            "生产指令编号",ins, "生产日期", time);
            SqlDataAdapter da = new SqlDataAdapter(sql, base._conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0) return false;
            DataRow dr = dt.NewRow();
            dr = writeOuterDefault(dr, iid, ins, time);
            dt.Rows.Add(dr);
            da.Update(dt);
            
            da.Fill(dt);
            int outid = Convert.ToInt32( dt.Rows[0]["ID"]);


            // innter 
            sql = String.Format(SQL_SELECT_INNER, _itn, "T吹膜岗位交接班记录ID", outid);
            da = new SqlDataAdapter(sql, base._conn);
            da.Fill(dt);
            foreach (DataRow drs in setting.Rows)
            {
                DataRow ndr = dt.NewRow();
                ndr["T吹膜岗位交接班记录ID"] = outid;
                ndr["确认项目"] = drs["确认项目"];
                ndr["确认结果白班"] = "是";
                ndr["确认结果夜班"] = "是";
                dt.Rows.Add(ndr);
            }
            da.Update(dt);

            return true;
            
        }

        public void save(int id, String[] outer, DataTable inner)
        {
            String sql;
            SqlDataAdapter da;
            SqlCommandBuilder cb;
            // out
            sql = String.Format(SQL_SELECT_OUTER,_otn, "ID",id);
            da = new SqlDataAdapter(sql, base._conn);
            cb = new SqlCommandBuilder(da);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow fdr = dt.Rows[0];
            fdr["生产日期"] = outer[0];
            fdr["白班异常情况处理"] = outer[1];
            fdr["白班交班员"] = outer[2];
            fdr["白班接班员"] = outer[3];
            fdr["白班交接班时间"] = outer[4];
            fdr["夜班异常情况处理"] = outer[5];
            fdr["夜班交班员"] = outer[6];
            fdr["夜班接班员"] = outer[7];
            fdr["夜班交接班时间"] = outer[8];
            da.Update(dt);

            int outid = Convert.ToInt32(fdr["ID"]);

            // inner
            sql = String.Format(SQL_SELECT_INNER, _itn, "T吹膜岗位交接班记录ID", outid);
            da = new SqlDataAdapter(sql, base._conn);
            cb = new SqlCommandBuilder(da);
            da.Update(inner);

        }

        DataRow writeOuterDefault(DataRow dr, int iid, string ins, DateTime time)
        {
            dr["生产指令ID"] = iid;
            dr["生产指令编号"] = ins;
            dr["生产日期"] = time.Date.ToString();
            
            dr["白班异常情况处理"] = "";

            dr["白班交接班时间"] =time.ToString();
            dr["夜班交接班时间"] =time.ToString();
            
            dr["夜班异常情况处理"] = "";

            
           

            //this part to add log 
            //格式： 
            // =================================================
            // yyyy年MM月dd日，操作员：XXX 创建记录
            string log = "=====================================\n";
            log += DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒") + "\n" + mySystem.Parameter.userName + "：" + mySystem.Parameter.userName + " 创建记录\n";
            log += "生产指令编码：" + ins + "\n";
            dr["日志"] = log;
            return dr;
        }
    }
}
