using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;

namespace mySystem
{
    class Parameter
    {
        public enum UserState
        {
            NoBody = 0,
            操作员 = 1, 
            审核员 = 2, 
            Both = 3,
            管理员 = 4,
        }
        public enum FormState
        {
            无数据 = -1,
            未保存 = 0,
            待审核 = 1,
            审核通过 = 2,
            审核未通过 = 3,
        }

        public static int i = 0;
        public static bool isSqlOk = false; //sql or access
        public static int userID; //登录人ID
        public static string userName; //登录用户名
        public static int userRole; //登录用户角色（权限）
        public static string userflight; // 登录人班次

        public static string IP_port="10.105.223.19,56625";//sql服务器IP和端口
        public static string sql_user = "";
        public static string sql_pwd = "";
        public static SqlConnection conn;
        public static OleDbConnection connOle;
        public static SqlConnection connUser;
        public static OleDbConnection connOleUser;
        public static DbConnection Conn;
        public static DbConnection ConnUser;

        public static string proInstruction; //吹膜生产指令
        public static int proInstruID; //吹膜生产指令编号
        public static string csbagInstruction; //cs制袋生产指令
        public static int csbagInstruID; //cs制袋生产指令编号
        public static string cleancutInstruction; //清洁分切生产指令
        public static int cleancutInstruID; //清洁分切生产指令编号
        public static string bpvbagInstruction; //BPV制袋生产指令
        public static int bpvbagInstruID; //BPV制袋生产指令编号
        public static string ldpebagInstruction; //LDPE制袋生产指令
        public static int ldpebagInstruID; //LDPE制袋生产指令编号
        public static string ptvbagInstruction; //PTV制袋生产指令
        public static int ptvbagInstruID; //PTV制袋生产指令编号
        public static string miejunInstruction; //灭菌委托单
        public static int miejunInstruID; //灭菌委托单编号

        public static int selectCon;
        static string strConn; //sql连接地址
        static bool isOk = false;

        public static ExtructionMainForm parentExtru; //吹膜mainform
        public static mySystem.Process.CleanCut.CleanCutMainForm parentClean; //清洁分切mainform
        public static mySystem.Process.Bag.CSBagMainForm parentCS; //cs制袋mainform
        public static mySystem.Process.Bag.LDPE.LDPEMainForm parentLDPE; //LDPE制袋mainform
        public static mySystem.Process.Bag.PTV.PTVMainForm parentPTV; //PTV制袋mainform
        public static mySystem.Process.Bag.BTV.BTVMainForm parentBPV; //BPV制袋mainform


        #region 公用函数
        //通过id查名字
        public static string IDtoName(int id)
        {
            string name = null;
            string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
            if (!isSqlOk)
            {
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = myConn;
                comm.CommandText = "select * from [users] where 用户ID= @ID";
                comm.Parameters.AddWithValue("@ID", id);

                OleDbDataReader myReader = comm.ExecuteReader();
                while (myReader.Read())
                {
                    name = myReader["姓名"].ToString();
                }

                myReader.Close();
                comm.Dispose();
                myConn.Dispose();
            }
            else
            {
                strCon = @"server=" + mySystem.Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection myConn = new SqlConnection(strCon);
                myConn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = myConn;
                comm.CommandText = "select * from [users] where 用户ID= @ID";
                comm.Parameters.AddWithValue("@ID", id);

                SqlDataReader myReader = comm.ExecuteReader();
                while (myReader.Read())
                {
                    name = myReader["姓名"].ToString();
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
                                Data Source=../../database/user.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "用户ID" });
                List<String> whereCols = new List<String>(new String[] { "姓名" });
                List<Object> whereVals = new List<Object>(new Object[] { name });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);

                if (res.Count > 1)
                {
                    MessageBox.Show("该姓名的员工不唯一，请在设置中更改", "警告");
                    id = Convert.ToInt32(res[0][0]);
                }
                else if (res.Count == 0)
                {
                    //MessageBox.Show("未找到结果", "错误");
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt32(res[0][0]);
                }
                myConn.Dispose();
            }
            else
            {
                string strCon = @"server=" + mySystem.Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection myConn = new SqlConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "用户ID" });
                List<String> whereCols = new List<String>(new String[] { "姓名" });
                List<Object> whereVals = new List<Object>(new Object[] { name });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);

                if (res.Count > 1)
                {
                    MessageBox.Show("该姓名的员工不唯一，请在设置中更改", "警告");
                    id = Convert.ToInt32(res[0][0]);
                }
                else if (res.Count == 0)
                {
                    //MessageBox.Show("未找到结果", "错误");
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
        public static String IDtoFlight(int id)
        {
            String flight = null;
            if (!isSqlOk)
            {
                string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "班次" });
                List<String> whereCols = new List<String>(new String[] { "用户ID" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                flight = res[0][0].ToString();
                myConn.Dispose();
            }
            else
            {
                string strCon = @"server=" + mySystem.Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection myConn = new SqlConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "班次" });
                List<String> whereCols = new List<String>(new String[] { "用户ID" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                flight = res[0][0].ToString();
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
                                Data Source=../../database/user.mdb;Persist Security Info=False";
                OleDbConnection myConn = new OleDbConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "角色ID" });
                List<String> whereCols = new List<String>(new String[] { "用户ID" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                role = Convert.ToInt32(res[0][0]);
                myConn.Dispose();
            }
            else
            {
                string strCon = @"server=" + mySystem.Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                SqlConnection myConn = new SqlConnection(strCon);
                myConn.Open();
                String tblName = "users";
                List<String> queryCols = new List<String>(new String[] { "角色ID" });
                List<String> whereCols = new List<String>(new String[] { "用户ID" });
                List<Object> whereVals = new List<Object>(new Object[] { id });
                List<List<Object>> res = Utility.selectAccess(myConn, tblName, queryCols, whereCols, whereVals, null, null, null, null, null);
                role = Convert.ToInt32(res[0][0]);
                myConn.Dispose();
            }
            return role;
        }
        #endregion

        #region DbConnection
        //初始化连接有用户表的数据库
        public static void ConnUserInit()
        {
            if (!isSqlOk)  //access
            {
                string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
                try
                {
                    ConnUser = new OleDbConnection(strCon);
                    ConnUser.Open();
                }
                catch
                {
                    ConnUser = null;
                    MessageBox.Show("连接数据库失败", "error");
                    return;
                }
            }
            else  //sql
            {
                //要改数据库！！！
                strConn = @"server=" + mySystem.Parameter.IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                isOk = false;
                ConnUser = connToServer(strConn);
                while (!isOk)
                {
                    MessageBox.Show("连接数据库失败", "error");
                    Connect2SqlForm con2sql = new Connect2SqlForm();
                    con2sql.IPChange += new Connect2SqlForm.DelegateIPChange(IPChanged);
                    con2sql.ShowDialog();

                    ConnUser = connToServer(strConn);
                }
                
            }
        }

        //通过按钮点击选择连接
        public static void ConInit()
        {
            if (!isSqlOk)
            {
                switch (selectCon)
                {
                    case 1:
                        String strCon1 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/extrusionnew.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon1);
                        break;
                    case 2:  //清洁分切
                        String strCon2 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/welding.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon2);
                        break;
                    case 3:  //CS制袋
                        String strCon3 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon3);
                        break;
                    case 4: //订单、库存
                        String strCon4 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon4);
                        break;
                    case 5: //灭菌
                        String strCon5 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon5);
                        break;
                    case 6: //BPV制袋
                        String strCon6 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/BPV.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon6);
                        break;
                    case 7: //LDPE制袋
                        String strCon7 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/LDPE.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon7);
                        break;
                    case 8: //PTV制袋
                        String strCon8 = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/PTV.mdb;Persist Security Info=False";
                        Conn = InitOle(strCon8);
                        break;
                }
            }
            else
            { }
        }

        #endregion


        //初始化连接有用户表的数据库
        public static void InitConnUser()
        {
            if (!isSqlOk)
            {
                string strsql = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/user.mdb;Persist Security Info=False";
                isOk = false;
                connOleUser = connToServerOle(strsql);
                while (!isOk)
                {
                    MessageBox.Show("连接数据库失败", "error");
                    return ;
                }

            }
            else
            {
                //connUser = Init(connUser);

                strConn = "server=" + IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                isOk = false;
                connUser = connToServer(strConn);
                if (!isOk)
                {
                    MessageBox.Show("连接数据库失败.");

                    Connect2SqlForm con2sql = new Connect2SqlForm();
                    con2sql.IPChange += new Connect2SqlForm.DelegateIPChange(IPChanged);
                    con2sql.ShowDialog();
                    //strConn = "server=" + IP_port + ";database=user;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                    //connUser = connToServer(strConn);
                    Application.Exit();
                }

            }
        }

        //通过按钮点击选择连接
        public static void InitCon()
        {
            if (!isSqlOk)
            {
                switch (selectCon)
                {
                    case 1:  //吹膜
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/extrusionnew.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 2:  //清洁分切
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/welding.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 3:  //CS制袋
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/csbag.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 4: //订单、库存
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/dingdan_kucun.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 5: //灭菌
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/miejun.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 6: //BPV制袋
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/BPV.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 7: //LDPE制袋
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/LDPE.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                    case 8: //PTV制袋
                        strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;
                                Data Source=../../database/PTV.mdb;Persist Security Info=False";
                        connOle = InitOle(strConn);
                        break;
                }
            }
            else
            {
                //conn = Init(conn);

                switch (selectCon)
                {
                    case 1:  //吹膜
                        strConn = "server=" + IP_port + ";database=extrusionnew;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 2:  //清洁分切
                        strConn = "server=" + IP_port + ";database=welding;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 3:  //CS制袋
                        strConn = "server=" + IP_port + ";database=csbag;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 4: //订单、库存
                        strConn = "server=" + IP_port + ";database=dingdan_kucun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 5: //灭菌
                        strConn = "server=" + IP_port + ";database=miejun;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 6: //BPV制袋
                        strConn = "server=" + IP_port + ";database=BPV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 7: //LDPE制袋
                        strConn = "server=" + IP_port + ";database=LDPE;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                    case 8: //PTV制袋
                        strConn = "server=" + IP_port + ";database=PTV;MultipleActiveResultSets=true;Uid=" + Parameter.sql_user + ";Pwd=" + Parameter.sql_pwd;
                        conn = Init(strConn);
                        break;
                }
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

        //private static SqlConnection Init(SqlConnection myConnection)
        //{
        //    //错误IP测试
        //    //strCon = @"server=10.105.223.14,56625;database=ProductionPlan;Uid=sa;Pwd=mitc";
        //    //正确IP
        //    strConn = @"server=10.105.223.19,56625;database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
        //    isOk = false;
        //    myConnection = connToServer(strConn);
        //    while (!isOk)
        //    {
        //        MessageBox.Show("连接数据库失败", "error");
        //        Connect2SqlForm con2sql = new Connect2SqlForm();
        //        con2sql.IPChange += new Connect2SqlForm.DelegateIPChange(IPChanged);
        //        con2sql.ShowDialog();

        //        myConnection = connToServer(strConn);
        //    }
        //    //MessageBox.Show("连接数据库成功", "success");
        //    return myConnection;
        //}      

        private static SqlConnection Init(string strConnect)
        {
            isOk = false;
            SqlConnection myConnection = connToServer(strConnect);
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return null;
            }
            return myConnection;
        }

        private static OleDbConnection InitOle(string strConnect)
        {
            isOk = false;
            OleDbConnection myConnection = connToServerOle(strConnect);
            while (!isOk)
            {
                MessageBox.Show("连接数据库失败", "error");
                return null;
            }
            return myConnection;
        }

        public static void IPChanged(string IP, string port)
        {
            //获取新IP
            strConn = @"server=" + IP + "," + port + ";database=ProductionPlan;MultipleActiveResultSets=true;Uid=sa;Pwd=mitc";
            IP_port = IP + "," + port;
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            config.AppSettings.Settings["ip"].Value = IP;
            config.AppSettings.Settings["port"].Value = port;
            config.Save(ConfigurationSaveMode.Modified);
        }



    }
}
