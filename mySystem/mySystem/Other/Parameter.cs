using System;
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
        public static bool isSqlOk = false; //sql or access
        public static int userID; //登录人ID
        public static string userName; //登录用户名
        public static int userRole; //登录用户角色（权限）

        public static SqlConnection conn;
        public static OleDbConnection connOle;

        public static string proInstruction; //吹膜生产指令
        public static string csbagInstruction; //cs制袋生产指令
        public static string cleancutInstruction; //清洁分切生产指令

        public static int selectCon;
        static string strConn;
        static bool isOk = false;
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

        public static void IPChanged(string IP, string port)
        {
            //获取新IP
            strConn = @"server=" + IP + "," + port + ";database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
        }



    }
}
