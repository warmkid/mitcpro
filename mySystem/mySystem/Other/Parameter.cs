﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;

namespace mySystem
{
    class Parameter
    {
        public static int i = 0;
        public static bool isSqlOk = false; //sql or access
        public static int userID; //登录人ID
        public static string userName; //登录用户名
        public static int userRole; //登录用户角色（权限）
        public static bool userflight; //登录人班次

        public static SqlConnection conn;
        public static OleDbConnection connOle;
        public static SqlConnection connUser;
        public static OleDbConnection connOleUser;

        public static string proInstruction; //吹膜生产指令
        public static int proInstruID; //吹膜生产指令编号
        public static string csbagInstruction; //cs制袋生产指令
        public static int csbagInstruID; //cs制袋生产指令编号
        public static string cleancutInstruction; //清洁分切生产指令
        public static int cleancutInstruID; //清洁分切生产指令编号

        public static int selectCon;
        static string strConn;
        static bool isOk = false;

        public static ExtructionMainForm parentExtru; //吹膜mainform
        public static mySystem.Process.CleanCut.CleanCutMainForm parentClean; //清洁分切mainform
        public static mySystem.Process.Bag.CSBagMainForm parentCS; //cs制袋mainform
        public static mySystem.Process.Bag.LDPE.LDPEMainForm parentLDPE; //LDPE制袋mainform
        public static mySystem.Process.Bag.PTV.PTVMainForm parentPTV; //PTV制袋mainform


        //通过id查名字
        public static string IDtoName(int id)
        {
            string name = null;
            string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
            if (!isSqlOk)
            {
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = myConn;
                comm.CommandText = "select * from user_aoxing where user_id= @ID";
                comm.Parameters.AddWithValue("@ID", id);

                OleDbDataReader myReader = comm.ExecuteReader();
                while (myReader.Read())
                {
                    name = myReader.GetString(4);
                }

                myReader.Close();
                comm.Dispose();
                myConn.Dispose();
            }
            return name;
        }

        //通过名字查id
        public static int NametoID(string name)
        {
            int id = 0;
            if (!isSqlOk)
            {
                string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "user_aoxing";
                List<String> queryCols = new List<String>(new String[] { "user_id" });
                List<String> whereCols = new List<String>(new String[] { "user_name" });
                List<Object> whereVals = new List<Object>(new Object[] { name });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);

                if (res.Count > 1)
                {
                    MessageBox.Show("该姓名的员工不唯一，请在设置中更改", "警告");
                    id = Convert.ToInt32(res[0][0]);
                }
                else if (res.Count == 0)
                {
                    MessageBox.Show("未找到结果", "错误");
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt32(res[0][0]);
                }
                myConn.Dispose();
            }
            
            return id;
        }

        //通过id查班次
        public static bool IDtoFlight(int id)
        {
            bool flight = false;
            if (!isSqlOk)
            {
                string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "user_aoxing";
                List<String> queryCols = new List<String>(new String[] { "flight" });
                List<String> whereCols = new List<String>(new String[] { "user_id" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                flight = Convert.ToBoolean(res[0][0]);
                myConn.Dispose();
            }  

            return flight; 
        }

        //通过id查用户角色（权限）
        public static int IDtoRole(int id)
        {
            int role = 0;
            if (!isSqlOk)
            {
                string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "user_aoxing";
                List<String> queryCols = new List<String>(new String[] { "role_id" });
                List<String> whereCols = new List<String>(new String[] { "user_id" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                role = Convert.ToInt32(res[0][0]);
                myConn.Dispose();
            }           
            return role;
        }



        //初始化连接有用户表的数据库
        public static void InitConnUser()
        {
            if (!isSqlOk)
            {
                string strsql = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
                connOleUser = Init(strsql, connOleUser);

            }
            else
            {
                connUser = Init(connUser);
            }
        }

        public static void InitCon()
        {
            if (!isSqlOk)
            {
                switch (selectCon)
                {
                    case 1:  //吹膜
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Extrusion.mdb;Persist Security Info=False";
                        connOle = Init(strConn, connOle);
                        break;
                    case 2:  //清洁分切
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Cleancut.mdb;Persist Security Info=False";
                        connOle = Init(strConn, connOle);
                        break;
                    case 3:  //CS制袋
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/db_Bag.mdb;Persist Security Info=False";
                        connOle = Init(strConn, connOle);
                        break;
                }
            }
            else
            {
                conn = Init(conn);
            }

        }


        private static SqlConnection connToServer(string connectStr)
        {
            SqlConnection myConnection;
            try
            {
                myConnection = new SqlConnection(connectStr);
                myConnection.Open();
            }
            catch
            {
                return null;
            }
            isOk = true;
            return myConnection;
        }

        private static OleDbConnection connToServerOle(string connectStr)
        {
            OleDbConnection myConn;
            try
            {
                myConn = new OleDbConnection(connectStr);
                myConn.Open();
            }
            catch
            {
                return null;
            }
            isOk = true;
            return myConn;
        }  

        private static SqlConnection Init(SqlConnection myConnection)
        {
            //错误IP测试
            //strCon = @"server=10.105.223.14,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
            //正确IP
            strConn = @"server=10.105.223.19,56625;database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
            isOk = false;
            myConnection = connToServer(strConn);
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                Connect2SqlForm con2sql = new Connect2SqlForm();
                con2sql.IPChange += new Connect2SqlForm.DelegateIPChange(IPChanged);
                con2sql.ShowDialog();

                myConnection = connToServer(strConn);
            }
            //MessageBox.Show("连接数据库成功", "success");
            return myConnection;
        }

        private static OleDbConnection Init(string strConnect, OleDbConnection myConnection)
        {
            isOk = false;
            myConnection = connToServerOle(strConnect);
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return null;

            }
            //MessageBox.Show("连接数据库成功", "success");
            return myConnection;
        }
 

        public static void IPChanged(string IP, string port)
        {
            //获取新IP
            strConn = @"server=" + IP + "," + port + ";database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
        }



    }
}
