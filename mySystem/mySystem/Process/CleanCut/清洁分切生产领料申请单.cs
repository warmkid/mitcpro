using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Other;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace mySystem.Process.CleanCut
{
    public partial class 清洁分切生产领料申请单 : BaseForm
    {
        private SqlConnection conn = null;
        private OleDbConnection connOle = null;
        private Boolean isSqlOk;
        private Int32 InstruID;
        private String Instruction;
        private DataTable dt物料代码数量, dt生产指令信息;

        //TODO: 产品代码批号 和 物料代码批号 区分不开

        public 清洁分切生产领料申请单(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();

            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;
            InstruID = Parameter.cleancutInstruID;
            Instruction = Parameter.cleancutInstruction;

            getOtherData();//读取该指令下的物料代码，数量

            生产领料申请单 my生产领料申请单 = new mySystem.Other.生产领料申请单(base.mainform, dt生产指令信息, dt物料代码数量, conn, connOle);
            my生产领料申请单.ShowDialog();
        }

        public 清洁分切生产领料申请单(mySystem.MainForm mainform, Int32 ID)
            : base(mainform)
        {
            InitializeComponent();
            conn = Parameter.conn;
            connOle = Parameter.connOle;
            isSqlOk = Parameter.isSqlOk;

            variableInit(ID);
            getOtherData();//读取该指令下的物料代码，数量

            生产领料申请单 my生产领料申请单 = new mySystem.Other.生产领料申请单(base.mainform, ID, dt生产指令信息, dt物料代码数量, conn, connOle);
            my生产领料申请单.ShowDialog();
        }

        void variableInit(int id)
        {
            OleDbDataAdapter da = new OleDbDataAdapter("select * from 生产领料申请单表 where ID=" + id, connOle);
            DataTable dt = new DataTable("temp");
            da.Fill(dt);
            InstruID = Convert.ToInt32(dt.Rows[0]["生产指令ID"]);
            Instruction = dt.Rows[0]["生产指令编号"].ToString();
        }
        
        //读取该指令下的物料代码，数量
        private void getOtherData()
        {
            Double outTemp;

            DataTable dt生产指令 = new DataTable("生产指令");
            DataTable dt生产指令制袋 = new DataTable("生产指令制袋");
            //内表：物料代码、批号、数量
            dt物料代码数量 = new DataTable("物料代码数量");
            dt物料代码数量.Columns.Add("物料代码", Type.GetType("System.String"));
            dt物料代码数量.Columns.Add("物料批号", Type.GetType("System.String"));
            dt物料代码数量.Columns.Add("数量", Type.GetType("System.Double"));
            //外表产品：生产指令ID、生产指令编号、属于工序、产品代码、产品批号、isSqlOk
            dt生产指令信息 = new DataTable("生产指令信息");
            dt生产指令信息.Columns.Add("生产指令ID", Type.GetType("System.Int32"));
            dt生产指令信息.Columns.Add("生产指令编号", Type.GetType("System.String"));
            dt生产指令信息.Columns.Add("属于工序", Type.GetType("System.String"));
            dt生产指令信息.Columns.Add("产品代码", Type.GetType("System.String"));
            dt生产指令信息.Columns.Add("产品批号", Type.GetType("System.String"));
            dt生产指令信息.Columns.Add("isSqlOk", Type.GetType("System.Boolean"));

            if (!isSqlOk)
            {
                OleDbCommand comm1 = new OleDbCommand();
                comm1.Connection = Parameter.connOle;
                comm1.CommandText = "select * from 清洁分切工序生产指令 where ID = " + InstruID.ToString();//这里应有生产指令编码
                OleDbDataAdapter datemp1 = new OleDbDataAdapter(comm1);
                datemp1.Fill(dt生产指令);

                //OleDbDataReader reader1 = comm1.ExecuteReader();
                //if (reader1.Read())
                if (dt生产指令.Rows.Count > 0)
                {
                    //读取生产指令物料
                    OleDbCommand comm2 = new OleDbCommand();
                    comm2.Connection = Parameter.connOle;
                    comm2.CommandText = "select * from 清洁分切工序生产指令详细信息 where T生产指令表ID = " + dt生产指令.Rows[0]["ID"].ToString();
                    OleDbDataAdapter datemp2 = new OleDbDataAdapter(comm2);
                    datemp2.Fill(dt生产指令制袋);
                    datemp2.Dispose();
                    //添加物料代码、数量
                    if (dt生产指令制袋.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt生产指令制袋.Rows.Count; i++)
                        { dt物料代码数量.Rows.Add(new object[] { dt生产指令制袋.Rows[i]["清洁前产品代码"], dt生产指令制袋.Rows[i]["清洁前批号"], (Double.TryParse(dt生产指令制袋.Rows[i]["数量米"].ToString(), out outTemp) == true ? outTemp : -1) }); }
                        dt生产指令信息.Rows.Add(new object[] { InstruID, Instruction, "清洁分切", dt生产指令制袋.Rows[0]["清洁前产品代码"], dt生产指令制袋.Rows[0]["清洁前批号"], isSqlOk });
                    }
                    else
                    {
                        dt物料代码数量.Rows.Add(new object[] { "暂无", -1 });
                        dt生产指令信息.Rows.Add(new object[] { InstruID, Instruction, "清洁分切", "暂无", "暂无", isSqlOk });
                    }
                }
                else
                {
                    //添加物料简称、批号、代码
                    dt物料代码数量.Rows.Add(new object[] { "暂无", -1 });
                }
                //reader1.Dispose();
                datemp1.Dispose();
            }
            else
            { }
        }
        
    }
}
