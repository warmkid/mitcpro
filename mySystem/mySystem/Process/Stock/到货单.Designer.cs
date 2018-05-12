namespace mySystem.Process.Stock
{
    partial class 到货单
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn新建 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.combox打印机选择 = new System.Windows.Forms.ComboBox();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn下移 = new System.Windows.Forms.Button();
            this.btn上移 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb税率 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tb运输方式 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb汇率 = new System.Windows.Forms.TextBox();
            this.cmb币种 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb业务员 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb部门 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb供应商 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb采购类型 = new System.Windows.Forms.TextBox();
            this.dtp日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb业务类型 = new System.Windows.Forms.ComboBox();
            this.tb单据号 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.btn日志 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn新建
            // 
            this.btn新建.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn新建.Location = new System.Drawing.Point(549, 73);
            this.btn新建.Name = "btn新建";
            this.btn新建.Size = new System.Drawing.Size(98, 23);
            this.btn新建.TabIndex = 280;
            this.btn新建.Text = "新建/查询";
            this.btn新建.UseVisualStyleBackColor = true;
            this.btn新建.Click += new System.EventHandler(this.btn新建_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn确认.Location = new System.Drawing.Point(744, 481);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(75, 23);
            this.btn确认.TabIndex = 279;
            this.btn确认.Text = "保存";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.btn确认_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn提交审核.Location = new System.Drawing.Point(863, 481);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(100, 23);
            this.btn提交审核.TabIndex = 278;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // combox打印机选择
            // 
            this.combox打印机选择.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.combox打印机选择.FormattingEnabled = true;
            this.combox打印机选择.Location = new System.Drawing.Point(228, 479);
            this.combox打印机选择.Name = "combox打印机选择";
            this.combox打印机选择.Size = new System.Drawing.Size(182, 24);
            this.combox打印机选择.TabIndex = 277;
            this.combox打印机选择.Visible = false;
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn打印.Location = new System.Drawing.Point(121, 479);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(75, 23);
            this.btn打印.TabIndex = 276;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Visible = false;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn审核.Location = new System.Drawing.Point(28, 480);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 275;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(864, 444);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 16);
            this.label13.TabIndex = 106;
            this.label13.Text = "审核日期";
            // 
            // tb审核员
            // 
            this.tb审核员.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb审核员.Location = new System.Drawing.Point(735, 438);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 26);
            this.tb审核员.TabIndex = 105;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(653, 444);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 16);
            this.label14.TabIndex = 104;
            this.label14.Text = "审核员";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp审核日期.Location = new System.Drawing.Point(958, 439);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(148, 26);
            this.dtp审核日期.TabIndex = 103;
            // 
            // btn删除
            // 
            this.btn删除.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn删除.Location = new System.Drawing.Point(130, 439);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 102;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn下移
            // 
            this.btn下移.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn下移.Location = new System.Drawing.Point(349, 439);
            this.btn下移.Name = "btn下移";
            this.btn下移.Size = new System.Drawing.Size(75, 23);
            this.btn下移.TabIndex = 101;
            this.btn下移.Text = "下移";
            this.btn下移.UseVisualStyleBackColor = true;
            this.btn下移.Click += new System.EventHandler(this.btn下移_Click);
            // 
            // btn上移
            // 
            this.btn上移.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn上移.Location = new System.Drawing.Point(247, 439);
            this.btn上移.Name = "btn上移";
            this.btn上移.Size = new System.Drawing.Size(75, 23);
            this.btn上移.TabIndex = 100;
            this.btn上移.Text = "上移";
            this.btn上移.UseVisualStyleBackColor = true;
            this.btn上移.Click += new System.EventHandler(this.btn上移_Click);
            // 
            // btn添加
            // 
            this.btn添加.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn添加.Location = new System.Drawing.Point(34, 439);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 99;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(34, 199);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1072, 234);
            this.dataGridView1.TabIndex = 98;
            // 
            // tb税率
            // 
            this.tb税率.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb税率.Location = new System.Drawing.Point(751, 167);
            this.tb税率.Name = "tb税率";
            this.tb税率.Size = new System.Drawing.Size(143, 26);
            this.tb税率.TabIndex = 97;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(923, 172);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 96;
            this.label10.Text = "备注";
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb备注.Location = new System.Drawing.Point(999, 164);
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(107, 26);
            this.tb备注.TabIndex = 95;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(682, 172);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 16);
            this.label11.TabIndex = 93;
            this.label11.Text = "税率";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(353, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 92;
            this.label12.Text = "运输方式";
            // 
            // tb运输方式
            // 
            this.tb运输方式.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb运输方式.Location = new System.Drawing.Point(446, 117);
            this.tb运输方式.Name = "tb运输方式";
            this.tb运输方式.Size = new System.Drawing.Size(100, 26);
            this.tb运输方式.TabIndex = 91;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(923, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 90;
            this.label9.Text = "汇率";
            // 
            // tb汇率
            // 
            this.tb汇率.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb汇率.Location = new System.Drawing.Point(999, 117);
            this.tb汇率.Name = "tb汇率";
            this.tb汇率.Size = new System.Drawing.Size(107, 26);
            this.tb汇率.TabIndex = 89;
            // 
            // cmb币种
            // 
            this.cmb币种.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb币种.FormattingEnabled = true;
            this.cmb币种.Location = new System.Drawing.Point(446, 164);
            this.cmb币种.Name = "cmb币种";
            this.cmb币种.Size = new System.Drawing.Size(100, 24);
            this.cmb币种.TabIndex = 88;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(384, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 87;
            this.label8.Text = "币种";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(923, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 86;
            this.label7.Text = "业务员";
            // 
            // tb业务员
            // 
            this.tb业务员.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb业务员.Location = new System.Drawing.Point(999, 75);
            this.tb业务员.Name = "tb业务员";
            this.tb业务员.Size = new System.Drawing.Size(107, 26);
            this.tb业务员.TabIndex = 85;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(682, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 84;
            this.label6.Text = "部门";
            // 
            // tb部门
            // 
            this.tb部门.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb部门.Location = new System.Drawing.Point(751, 112);
            this.tb部门.Name = "tb部门";
            this.tb部门.Size = new System.Drawing.Size(143, 26);
            this.tb部门.TabIndex = 83;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(37, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 82;
            this.label5.Text = "供应商";
            // 
            // tb供应商
            // 
            this.tb供应商.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb供应商.Location = new System.Drawing.Point(128, 167);
            this.tb供应商.Name = "tb供应商";
            this.tb供应商.Size = new System.Drawing.Size(192, 21);
            this.tb供应商.TabIndex = 81;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(37, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 80;
            this.label4.Text = "采购类型";
            // 
            // tb采购类型
            // 
            this.tb采购类型.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb采购类型.Location = new System.Drawing.Point(130, 122);
            this.tb采购类型.Name = "tb采购类型";
            this.tb采购类型.Size = new System.Drawing.Size(121, 26);
            this.tb采购类型.TabIndex = 79;
            // 
            // dtp日期
            // 
            this.dtp日期.CalendarFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp日期.Location = new System.Drawing.Point(751, 73);
            this.dtp日期.Name = "dtp日期";
            this.dtp日期.Size = new System.Drawing.Size(143, 26);
            this.dtp日期.TabIndex = 77;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(682, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 76;
            this.label3.Text = "日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(368, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 75;
            this.label1.Text = "单据号";
            // 
            // cmb业务类型
            // 
            this.cmb业务类型.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb业务类型.FormattingEnabled = true;
            this.cmb业务类型.Location = new System.Drawing.Point(130, 73);
            this.cmb业务类型.Name = "cmb业务类型";
            this.cmb业务类型.Size = new System.Drawing.Size(121, 24);
            this.cmb业务类型.TabIndex = 74;
            // 
            // tb单据号
            // 
            this.tb单据号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb单据号.Location = new System.Drawing.Point(446, 73);
            this.tb单据号.Name = "tb单据号";
            this.tb单据号.Size = new System.Drawing.Size(100, 26);
            this.tb单据号.TabIndex = 73;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(37, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 72;
            this.label2.Text = "业务类型";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold);
            this.label角色.Location = new System.Drawing.Point(592, 27);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(49, 19);
            this.label角色.TabIndex = 71;
            this.label角色.Text = "角色";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(300, 27);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(69, 19);
            this.Title.TabIndex = 70;
            this.Title.Text = "到货单";
            // 
            // btn日志
            // 
            this.btn日志.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn日志.Location = new System.Drawing.Point(1024, 481);
            this.btn日志.Name = "btn日志";
            this.btn日志.Size = new System.Drawing.Size(82, 23);
            this.btn日志.TabIndex = 333;
            this.btn日志.Text = "查看日志";
            this.btn日志.UseVisualStyleBackColor = true;
            this.btn日志.Click += new System.EventHandler(this.btn日志_Click);
            // 
            // 到货单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 516);
            this.Controls.Add(this.btn日志);
            this.Controls.Add(this.btn新建);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.combox打印机选择);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn下移);
            this.Controls.Add(this.btn上移);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tb税率);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tb运输方式);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb汇率);
            this.Controls.Add(this.cmb币种);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb业务员);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb部门);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb供应商);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb采购类型);
            this.Controls.Add(this.dtp日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb业务类型);
            this.Controls.Add(this.tb单据号);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.Title);
            this.Name = "到货单";
            this.Text = "到货单";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.到货单_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.TextBox tb单据号;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb业务类型;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb采购类型;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb供应商;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb部门;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb业务员;
        private System.Windows.Forms.ComboBox cmb币种;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb汇率;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb运输方式;
        private System.Windows.Forms.TextBox tb税率;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn下移;
        private System.Windows.Forms.Button btn上移;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Button btn确认;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.ComboBox combox打印机选择;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn新建;
        private System.Windows.Forms.Button btn日志;
    }
}