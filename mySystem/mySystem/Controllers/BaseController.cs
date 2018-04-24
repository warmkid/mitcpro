using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace mySystem.Controllers
{
    class BaseController
    {
        protected SqlConnection _conn;

        public BaseController(SqlConnection c)
        {
            _conn = c;
        }

        public DataTable query(String sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, _conn);
            DataTable ret = new DataTable();
            da.Fill(ret);
            return ret;
        }

        public DataTable queryAndInsert(String sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, _conn);
            DataTable ret = new DataTable();
            da.Fill(ret);
            return ret;
        }

    }
}
