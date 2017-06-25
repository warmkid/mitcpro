using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem
{
    public partial class ReplaceHeadForm : Form
    {
        public ReplaceHeadForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ///***********************表格数据初始化************************///
            //表格界面设置
            this.dgv.Font = new Font("宋体", 12, FontStyle.Regular);
            this.dgv.RowHeadersVisible = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ColumnHeadersHeight = 40;

            //添加行
            Object[] row1 = new Object[] { "模头", "吊中心线，检查模头应居中。前后误差±1mm。左右误差±2mm。", true };
            Object[] row2 = new Object[] { "模头", "校水平，检查模头应水平。上下误差±1mm。", true };
            Object[] row3 = new Object[] { "挤出机", "校水平，流道连接处水平度误差±1mm。（分别检查A、B、C三层）", true };
            Object[] row4 = new Object[] { "挤出机", "连接法兰连接紧固，水平度误差±1mm。", true };
            Object[] row5 = new Object[] { "加热片", "分别检查A、B、C层加热片位置安装正确，连接牢固。", true };
            Object[] row6 = new Object[] { "电热偶", "分别检查A、B、C层及模头电热偶位置安装正确，连接牢固。", true };
            Object[] row7 = new Object[] { "电源插头", "分别检查A、B、C层及模头电源插头位置安装正确，连接牢固。", true };
            Object[] row8 = new Object[] { "风环", "吊中心线，检查风环应居中。前后误差±1mm。左右误差±1mm。", true };
            Object[] row9 = new Object[] { "风环", "校水平，检查风环应水平。上下误差±1mm。", true };
            Object[] row10 = new Object[] { "风环", "检查内风环安装位置正确。（拧紧后回一圈）", true };
            Object[] row11 = new Object[] { "风管", "检查风环6根外冷风管安装正确，连接牢固。", true };
            Object[] row12 = new Object[] { "风管", "检查内冷进风（短的3根）及内排风6根不锈钢管安装正确，连接牢固。", true };
            Object[] row13 = new Object[] { "风管", "检查内冷进风及内排风6根风管安装正确，连接牢固。", true };
            Object[] row14 = new Object[] { "风管", "检查外冷、内冷、内排3根主风管安装正确，连接牢固。", true };
            Object[] row15 = new Object[] { "气管", "气管连接牢固，无漏气。", true };
            Object[] row16 = new Object[] { "盖板", "不锈钢盖板连接牢固。（盖板下安装耐高温密封垫）", true };

            Object[] rows = new Object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14, row15, row16 };

            foreach (Object[] rowArray in rows)
            {
                this.dgv.Rows.Add(rowArray);
            }



        }


    }
}
