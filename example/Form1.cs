using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;

namespace AoxinCommon // TODO, time range and LIKE
{
    public partial class Form1 : Form
    {
        private OleDbConnection conn;
        public Form1()
        {

            InitializeComponent();
            conn = connectToAccess();

            testSelectAccess(conn);
            testInsertAccess(conn);
            testUpdateAccess(conn);
            testDeleteAccess(conn);
            testFillDataGridView(conn);
            
        }

        private void testSelectAccess(OleDbConnection conn)
        {
            String tblName = "table1";
            List<String> queryCols = new List<String>(new String[] { "caozuoren", "shifou", "shijian" });
            List<String> whereCols = new List<String>(new String[] { "num" });
            List<Object> whereVals = new List<Object>(new Object[] { 9 });
            String likeCol = "caozuoren";
            String likeVal = "yifan";
            String betweenCol = "shijian";
            DateTime left = new DateTime(2017, 6, 20);
            DateTime right = new DateTime(2017, 6, 24);
            List<List<Object>> res = selectAccess(conn, tblName, queryCols, null, null, likeCol, likeVal, betweenCol, left, right);
        }

        private void testInsertAccess(OleDbConnection conn)
        {
            String tblName = "table1";
            List<String> insertCols = new List<String>(new String[] { "caozuoren", "shifou", "shijian" });
            // NOTE: DateTime.Now has MilliSecond which is not allowed in Access (WHY??)
            // So we need to construct a new DateTime object without MilliSecond
            DateTime dt = DateTime.Now;
            DateTime insertVal = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            List<Object> insertVals = new List<Object>(new Object[] { "ly", true, insertVal });
            Boolean b = insertAccess(conn, tblName, insertCols, insertVals);
        }

        private void testUpdateAccess(OleDbConnection conn)
        {
            String tblName = "table1";
            List<String> updateCols = new List<String>(new String[] { "num" });
            List<Object> updateVals = new List<Object>(new Object[] { 9 });
            List<String> whereCols = new List<String>(new String[] { "shifou" });
            List<Object> whereVals = new List<Object>(new Object[] { true });
            Boolean b = updateAccess(conn, tblName, updateCols, updateVals, whereCols, whereVals);
        }

        private void testDeleteAccess(OleDbConnection conn)
        {
            String tblName = "table1";
            List<String> whereCols = new List<String>(new String[] { "shifou" });
            List<Object> whereVals = new List<Object>(new Object[] { false });
            deleteAccess(conn, tblName, whereCols, whereVals);
        }

        private void testFillDataGridView(OleDbConnection conn)
        {
            String tblName = "table1";
            List<String> queryCols = new List<String>(new String[] { "caozuoren", "shifou", "shijian" });
            List<String> whereCols = new List<String>(new String[] { "shifou" });
            List<Object> whereVals = new List<Object>(new Object[] { true });


            List<List<Object>> res = selectAccess(conn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
            fillDataGridView(dataGridView1, res);
        }

     
        private OleDbConnection connectToAccess()
        {
            string strConnect = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=D:/ly.mdb;Persist Security Info=False";
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection(strConnect);
                conn.Open();
            }
            catch
            {
                MessageBox.Show("连接数据库失败", "success");
                return null;
            }

            MessageBox.Show("连接数据库成功", "success");
            return conn;
        }

        // conn: connection to Access
        // tblName : Name of table
        // queryCols : List of name of columns to query
        // whereCols : list of name of column in WHERE clause, leave it null if no WHERE clause
        // whereVals : list of value of corresponding column
        // likeCol: name of column to use LIKE in SQL
        // likeVal: String corresponding to likeCol
        // betweenCol: name of column to use BETWEEN in SQL
        // left: smaller value in BETWEEN
        // right: larger value in BETWEEN
        private List<List<Object>> selectAccess(OleDbConnection conn, String tblName, List<String> queryCols, 
            List<String> whereCols, List<Object> whereVals, String likeCol, String likeVal, String betweenCol, 
            Object left, Object right)
        {
            bool isWhere = false;
            String strSQL = String.Format("SELECT * FROM {0}", tblName);
            if (null != whereCols)
            {
                if (!isWhere)
                {
                    strSQL += " WHERE ";
                    isWhere = true;
                }
                List<String> temp = new List<string>();
                foreach (String s in whereCols)
                {
                    temp.Add(s + "=@" + s);
                }
                strSQL += joinList(temp, ",");
            }
            if (null != likeCol)
            {
                if (!isWhere)
                {
                    strSQL += " WHERE ";
                    isWhere = true;
                }
                else
                {
                    strSQL += " AND ";
                }
                strSQL += likeCol + " LIKE @" + likeCol;
            }
            if (null != betweenCol)
            {
                if (!isWhere)
                {
                    strSQL += " WHERE ";
                    isWhere = true;
                }
                else
                {
                    strSQL += " AND ";
                }
                strSQL += betweenCol + " BETWEEN @l" + betweenCol + " AND @r" + betweenCol;
            }


            OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            if (null != whereCols)
            {
                for (int i = 0; i < whereCols.Count; ++i)
                {
                    String c = whereCols[i];
                    Object v = whereVals[i];
                    cmd.Parameters.AddWithValue("@" + c, v);
                }
            }
            if (null != likeCol)
            {
                cmd.Parameters.AddWithValue("@" + likeCol, "%" + likeVal + "%");
            }
            if (null != betweenCol)
            {
                cmd.Parameters.AddWithValue("@l" + betweenCol, left);
                cmd.Parameters.AddWithValue("@r" + betweenCol, right);
            }
            List<List<Object>> ret = new List<List<Object>>();
            OleDbDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<Object> row = new List<Object>();
                foreach (String str in queryCols)
                {
                    row.Add(reader[str]);
                }
                ret.Add(row);
            }
            return ret;
        }


        // conn: connection to Access
        // tblName : Name of table
        // queryCols : List of name of columns to query
        // insertCols : list of name of column to insert
        // insertVals : list of value of corresponding column
        private Boolean insertAccess(OleDbConnection conn, String tblName, List<String> insertCols, List<Object> insertVals)
        {

            String cols = joinList(insertCols, ",");
            List<String> temp = new List<string>();
            foreach (String s in insertCols)
            {
                temp.Add("@" + s);
            }
            String vals = joinList(temp, ",");

            String strSQL = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tblName, cols, vals);
            OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            for (int i = 0; i < insertCols.Count; ++i)
            {
                String c = insertCols[i];
                Object v = insertVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // conn: connection to Access
        // tblName : Name of table
        // updateCols : List of name of columns to update
        // updateVals : list of value of corresponding update column
        // whereCols : list of name of column in WHERE clause
        // whereVals : list of value of corresponding column
        private Boolean updateAccess(OleDbConnection conn, String tblName, List<String> updateCols, List<Object> updateVals, 
            List<String> whereCols, List<Object> whereVals)
        {
            List<String> temp = new List<string>();
            foreach (String s in updateCols)
            {
                temp.Add(s + "=@" + s);
            }
            String updates = joinList(temp, ",");

            temp = new List<string>();
            foreach (String s in whereCols)
            {
                temp.Add(s + "=@" + s);
            }
            String wheres = joinList(temp, ",");

            String strSQL = String.Format("UPDATE {0} SET {1} WHERE {2}", tblName, updates, wheres);
            OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            for (int i = 0; i < updateCols.Count; ++i)
            {
                String c = updateCols[i];
                Object v = updateVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            for (int i = 0; i < whereCols.Count; ++i)
            {
                String c = whereCols[i];
                Object v = whereVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // conn: connection to Access
        // tblName : Name of table
        // whereCols : list of name of column in WHERE clause
        // whereVals : list of value of corresponding column
        private Boolean deleteAccess(OleDbConnection conn, String tblName, List<String> whereCols, List<Object> whereVals)
        {
            List<String> temp = new List<string>();
            foreach (String s in whereCols)
            {
                temp.Add(s + "=@" + s);
            }
            String where = joinList(temp, ",");
            String strSQL = String.Format("DELETE FROM {0} WHERE {1}", tblName, where);
            OleDbCommand cmd = new OleDbCommand(strSQL, conn);
            for (int i = 0; i < whereCols.Count; ++i)
            {
                String c = whereCols[i];
                Object v = whereVals[i];
                cmd.Parameters.AddWithValue("@" + c, v);
            }
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String joinList(List<String> lst, String sep)
        {
            String ret = "";
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst.Count - 1 != i)
                {
                    ret += lst[i] + sep;
                }
                else
                {
                    ret += lst[i] + " ";
                }

            }
            return ret;
        }

        // fill the DataGridView with data
        // Note: please add the column to datagridview first!!
        private void fillDataGridView(DataGridView dgv, List<List<Object>> data)
        {

            for (int i = 0; i < data.Count; ++i)
            {
                dgv.Rows.Add(data[i].ToArray());

            }
        }

        // read data from datagridview
        // Note: the last row maybe empty
        private List<List<Object>> readFromDataGridView(DataGridView dgv)
        {
            List<List<Object>> data = new List<List<object>>();

            foreach (DataGridViewRow r in dgv.Rows)
            {
                List<Object> row = new List<object>();
                foreach (DataGridViewCell c in r.Cells)
                {
                    row.Add(c.Value);
                }
                data.Add(row);
            }
            return data;
        }

        // read all text from controls;
        private List<String> readFromControl(List<Control> cons)
        {
            List<String> ret = new List<string>();
            foreach (Control c in cons)
            {
                ret.Add(c.Text);
            }
            return ret;
        }

        // fill text to controls
        private void fillControl(List<Control> cons, List<String> data)
        {
            for (int i = 0; i < cons.Count; ++i)
            {
                cons[i].Text = data[i];
            }
        }

    }
}
