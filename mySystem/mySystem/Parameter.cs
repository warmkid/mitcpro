using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem
{
    class Parameter
    {
        public bool isSqlOk = false; //sql or access
        public int userID; //登录人ID
        public string username; //登录用户名
        public int userRole; //用户角色（权限）
        
        public SqlConnection conn;
        public OleDbConnection connOle;               
        
        public string proInstruction; //吹膜生产指令
        public string csbagInstruction; //cs制袋生产指令
        public string cleancutInstruction; //清洁分切生产指令

        public string strCon;
        



    }
}
