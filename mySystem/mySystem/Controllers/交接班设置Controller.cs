using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace mySystem.Controllers
{
    class 交接班设置Controller:BaseController
    {
        String _tblName;
        String SQL_READ = @"select * from {0}";
        public 交接班设置Controller(String tblname, SqlConnection conn)
            : base(conn)
        {
            _tblName = tblname;
        }

        public DataTable read()
        {
            SqlDataAdapter da = new SqlDataAdapter(String.Format(SQL_READ,_tblName),base._conn);
            DataTable ret = new DataTable();
            da.Fill(ret);
            return ret;
        }

    }
}
