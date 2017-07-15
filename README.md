

# 所有窗体视实际需求考虑实现如下函数：



```C#
// 给外表的一行写入默认值，包括操作人，时间，班次等
DataRow writeOuterDefault(DataRow);
// 给内表的一行写入默认值，包括操作人，时间，Y/N等
DataRow writeInnerDefault(DataRow);
// 根据条件从数据库中读取一行外表的数据
void readOuterData(能唯一确定一行外表数据的参数，一般是生产指令ID或生产指令编号)；
// 根据条件从数据库中读取多行内表数据
void readInnerData(int 外表行ID);
// 移除外表和控件的绑定，建议使用Control.DataBinds.Clear()
void removeOuterBinding();
// 移除内表和控件的绑定，如果只是一个DataGridView可以不用实现
void removeInner(Binding);
// 外表和控件的绑定
void outerBind();
// 内表和控件的绑定
void innerBind();
// 设置DataGridView中各列的格式
void setDataGridViewColumns();
// 刷新DataGridView中的列：序号
void setDataGridViewRowNums();
```



# 以下是样例



```C#
        private void readOuterData(String name)
        {
            daOuter = new OleDbDataAdapter("select * from 订单信息 where 订单名称='" + name+"'", conn);
            cbOuter = new OleDbCommandBuilder(daOuter);
            dtOuter = new DataTable("订单信息");
            bsOuter = new BindingSource();
            daOuter.Fill(dtOuter);
        }

         private DataRow writeOuterDefault(DataRow dr)
        {
            dr["订单名称"] = tb订单名称.Text;
            return dr;
        }

        private void outerBind()
        {
            bsOuter.DataSource = dtOuter;
            tb订单名称.DataBindings.Add("Text", bsOuter.DataSource, "订单名称");
            tb客户名称.DataBindings.Add("Text", bsOuter.DataSource, "客户名称");
            cmb产品类型.DataBindings.Add("SelectedItem", bsOuter.DataSource, "产品类型");
            cmb产品名称.DataBindings.Add("SelectedItem", bsOuter.DataSource, "产品名称");
        }

        private void removeOuterBinding()
        {
            tb订单名称.DataBindings.RemoveAt(0);
            tb客户名称.DataBindings.RemoveAt(0);
            cmb产品类型.DataBindings.RemoveAt(0);
            cmb产品名称.DataBindings.RemoveAt(0);
        }

        private void readInnerData(int id)
        {
            daInner = new OleDbDataAdapter("select * from 订单BOM信息 where 订单信息ID=" + id, conn);
            cbInner = new OleDbCommandBuilder(daInner);
            dtInner = new DataTable("订单BOM信息");
            bsInner = new BindingSource();

            daInner.Fill(dtInner);
        }

        private void innerBind()
        {
            bsInner.DataSource = dtInner;
            dataGridView1.DataSource = bsInner.DataSource;
        }

        private void setDataGridViewColumns()
        {
            DataGridViewTextBoxColumn tbc;
            DataGridViewComboBoxColumn cbc;
            foreach (DataColumn dc in dtInner.Columns)
            {

                switch (dc.ColumnName)
                {

                    case "ID":
                    case "订单信息ID":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        tbc.Visible = false;
                        break;
                    case "商品名称":
                        cbc = new DataGridViewComboBoxColumn();
                        cbc.DataPropertyName = dc.ColumnName;
                        cbc.HeaderText = dc.ColumnName;
                        cbc.Name = dc.ColumnName;
                        cbc.ValueType = dc.DataType;
                        HashSet<String> items = new HashSet<string>();
                        foreach (String s in goodsAndPrice.Keys.OfType<String>().ToList<String>())
                        {
                            items.Add(s);
                        }
                        foreach (DataRow dr in dtInner.Rows)
                        {
                            items.Add(Convert.ToString(dr["商品名称"]));
                        }
                        foreach (String s in items)
                        {
                            cbc.Items.Add(s);
                        }
                        dataGridView1.Columns.Add(cbc);
                        break;
                    case "商品数量":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        dataGridView1.Columns.Add(tbc);
                        break;
                    case "商品单价":
                        tbc = new DataGridViewTextBoxColumn();
                        tbc.DataPropertyName = dc.ColumnName;
                        tbc.HeaderText = dc.ColumnName;
                        tbc.Name = dc.ColumnName;
                        tbc.ValueType = dc.DataType;
                        tbc.ReadOnly = true;
                        dataGridView1.Columns.Add(tbc);
                        break;
                }
            }
        }

        

        private DataRow writeInnerDefault(DataRow dr)
        {
            dr["订单信息ID"]=dtOuter.Rows[0]["ID"];
            return dr;
        }


        private void btn保存_Click(object sender, EventArgs e)
        {
          	// 保存数据的方法，每次保存之后重新读取数据，重新绑定控件
            daInner.Update((DataTable)bsInner.DataSource);
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            innerBind();


            bsOuter.EndEdit();
            daOuter.Update((DataTable)bsOuter.DataSource);
            readOuterData(tb订单名称.Text);
            removeOuterBinding();
            outerBind();

        }

		
		private void btn查询插入_Click(object sender, EventArgs e)
        {
          	// 先读外表，再读内表的代码
          
           	// 读外表，如果行数位0，则插入一行并保存
            readOuterData(tb订单名称.Text);
            outerBind();
            if (dtOuter.Rows.Count <= 0)
            {
                DataRow dr = dtOuter.NewRow();
                dr = writeOuterDefault(dr);
                dtOuter.Rows.Add(dr);
                daOuter.Update((DataTable)bsOuter.DataSource);
                readOuterData(tb订单名称.Text);
                removeOuterBinding();
                outerBind();
            }
 
          	// 读内表，因为我实现了一个“添加”按钮，所以这里可以不用插入空白行
            readInnerData(Convert.ToInt32(dtOuter.Rows[0]["ID"]));
            setDataGridViewColumns();
            innerBind();
          
            // 控件状态
            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }
            tb订单名称.Enabled = false;
            btn查询插入.Enabled = false;
        }


		private void btn添加_Click(object sender, EventArgs e)
        {
          	// 内表中添加一行
            DataRow dr = dtInner.NewRow();
            dr = writeInnerDefault(dr);
            dtInner.Rows.Add(dr);
        }

        private void setDataGridViewRowNums()
        {

            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
        }


```



# 读写、打印Excel  的代码



```c#
// 打开一个Excel进程
Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
// 利用这个进程打开一个Excel文件
Microsoft.Office.Interop.Excel._Workbook wb = oXL.Workbooks.Open(@"D:/Desktop/tt.xlsx");
// 选择一个Sheet，注意Sheet的序号是从1开始的
Microsoft.Office.Interop.Excel._Worksheet my = wb.Worksheets[wb.Worksheets.Count];
// 设置该进程是否可见
oXL.Visible = true;
// 修改Sheet中某行某列的值
my.Cells[1, 2].Value = "11";
// 让这个Sheet为被选中状态
my.Select();  // oXL.Visible=true 加上这一行  就相当于预览功能
// 直接用默认打印机打印该Sheet
my.PrintOut(); // oXL.Visible=false 就会直接打印该Sheet
// 关闭文件，false表示不保存
wb.Close(false);
// 关闭Excel进程
oXL.Quit();
// 释放COM资源
Marshal.ReleaseComObject(wb);
Marshal.ReleaseComObject(oXL);
```

