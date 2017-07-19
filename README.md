

# 每个窗口要实现的函数



```C#

// Parameter中不需要为OleDB和SQL写两个连接变量，写一个:
DbConnection conn;
// 利用isSQL来判断是用SQLConnection还是OleDBConnection来new这个对象
// 带 ID 的构造函数
// 注意，如果当前登录人既不是操作员也不是审核员，则提示，然后不显示界面(管理员例外）
// 父类的变量
DbDataAdapter daOuter, daInner;
DbCommandBuilder cbOuter, cbInner;
BindingSource bsOuter, bsInner;
DataTable dtOuter, dtInner;

// 根据条件从数据库中读取一行外表的数据
void readOuterData(能唯一确定一行外表数据的参数，一般是生产指令ID或生产指令编号);
// 给外表的一行写入默认值，包括操作人，时间，班次等
DataRow writeOuterDefault(DataRow dr);
// 先清除绑定，再完成外表和控件的绑定
void outerBind();


// 根据条件从数据库中读取内表数据
void readInnerData(int 外表行ID);
// 给内表的一行写入默认值，包括操作人，时间，Y/N等
DataRow writeInnerDefault(DataRow dr);
// 内表和控件的绑定
void innerBind();

// 设置DataGridView中各列的格式，包括列类型，列名，是否可以排序
void setDataGridViewColumns();
// 刷新DataGridView中的列：序号
void setDataGridViewRowNums();

// 设置各控件的事件
	// 设置读取数据的事件，比如生产检验记录的 “产品代码”的SelectedIndexChanged
void addDateEventHandler();
	// 设置自动计算类事件
void addComputerEventHandler();
	// 其他事件，比如按钮的点击，数据有效性判断
void addOtherEvnetHandler();
// 打印函数
void print(bool);
// 获取其他需要的数据，比如产品代码，产生废品原因等
void getOtherData();
// 获取操作员和审核员
void getPeople();
// 计算，主要用于日报表、物料平衡记录中的计算
void computer();
// 获取当前窗体状态：
// 如果『审核人』为空，则为未保存
// 否则，如果『审核人』为『__待审核』，则为『待审核』
// 否则
//         如果审核结果为『通过』，则为『审核通过』
//         如果审核结果为『不通过』，则为『审核未通过』
// 这个函数可以放在父类中？
void setFormState();
// 设置用户状态，用户状态有3个：0--操作员，1--审核员，2--管理员
void setUserState();
// 设置控件可用性，根据状态设置，状态是每个窗体的变量，放在父类中
// 0：未保存；1：待审核；2：审核通过；3：审核未通过
void setEnableReadOnly();

  
// 窗口下的按钮：审核                   保存，发送审核，打印，查看记录
```



流程图：

1. 带ID的构造函数：getPeople()-->判断用户是否可以查看界面-->根据ID读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)--> addOtherEvnetHandler()
2. 不带ID的构造函数，直接读取数据类型：getPeople()--> setUserState()--> getOtherData()-->读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)--> addComputerEventHandler()--> setFormState()--> setEnableReadOnly() --> addOtherEvnetHandler()
3. 不带ID的构造函数，通过控件的值读取数据类型：getPeople()--> setUserState()--> getOtherData()--> addDataEventHandler()  {当combobox或datetimepicker取到值后：读取数据并显示(readOuterData(),outerBind(),readInnerData(),innerBind)-->addComputerEventHandler()-->setFormState()-->setEnableReadOnly() --> addOtherEvnetHandler()}
4. 不带ID的构造函数，纯计算类型：getPeople()--> setUserState()--> getOtherData()--> Computer() --> addOtherEvnetHandler()-->setFormState()-->setEnableReadOnly()



# 样例



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
  DataGridViewCheckBoxColumn ckbc;
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
        // 这一列不可排序
        tbc.SortMode = DataGridViewColumnSortMode.NotSortable;
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

