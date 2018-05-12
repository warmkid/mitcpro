namespace mySystem.Process.Order
{
    partial class 出库单
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
            this.bt日志 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn出库 = new System.Windows.Forms.Button();
            this.combox打印机选择 = new System.Windows.Forms.ComboBox();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.tb审核人 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb出库类别 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb业务类型 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb业务员 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn查询 = new System.Windows.Forms.Button();
            this.tb客户名称 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtp出库日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb销售订单号 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl出库单号 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt日志
            // 
            this.bt日志.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt日志.Location = new System.Drawing.Point(858, 562);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(75, 23);
            this.bt日志.TabIndex = 214;
            this.bt日志.Text = "查看日志";
            this.bt日志.UseVisualStyleBackColor = true;
            this.bt日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // btn删除
            // 
            this.btn删除.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn删除.Location = new System.Drawing.Point(14, 463);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 213;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn出库
            // 
            this.btn出库.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn出库.Location = new System.Drawing.Point(629, 463);
            this.btn出库.Name = "btn出库";
            this.btn出库.Size = new System.Drawing.Size(75, 23);
            this.btn出库.TabIndex = 212;
            this.btn出库.Text = "出库";
            this.btn出库.UseVisualStyleBackColor = true;
            this.btn出库.Click += new System.EventHandler(this.btn出库_Click);
            // 
            // combox打印机选择
            // 
            this.combox打印机选择.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.combox打印机选择.FormattingEnabled = true;
            this.combox打印机选择.Location = new System.Drawing.Point(234, 562);
            this.combox打印机选择.Name = "combox打印机选择";
            this.combox打印机选择.Size = new System.Drawing.Size(182, 24);
            this.combox打印机选择.TabIndex = 211;
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn打印.Location = new System.Drawing.Point(127, 562);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(75, 23);
            this.btn打印.TabIndex = 210;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn确认.Location = new System.Drawing.Point(629, 563);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(75, 23);
            this.btn确认.TabIndex = 209;
            this.btn确认.Text = "保存";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.btn确认_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn提交审核.Location = new System.Drawing.Point(710, 563);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(100, 23);
            this.btn提交审核.TabIndex = 208;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn审核.Location = new System.Drawing.Point(18, 562);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 207;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(627, 303);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(472, 143);
            this.dataGridView3.TabIndex = 206;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(624, 269);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 205;
            this.label12.Text = "库存信息";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(624, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 16);
            this.label9.TabIndex = 204;
            this.label9.Text = "订单未发货信息";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(627, 95);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(472, 144);
            this.dataGridView2.TabIndex = 203;
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView2_DataBindingComplete);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(17, 174);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(577, 272);
            this.dataGridView1.TabIndex = 202;
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp审核日期.Location = new System.Drawing.Point(376, 517);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(200, 26);
            this.dtp审核日期.TabIndex = 201;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(294, 524);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 200;
            this.label11.Text = "审核日期";
            // 
            // tb审核人
            // 
            this.tb审核人.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb审核人.Location = new System.Drawing.Point(83, 521);
            this.tb审核人.Name = "tb审核人";
            this.tb审核人.Size = new System.Drawing.Size(141, 26);
            this.tb审核人.TabIndex = 197;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(21, 524);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 16);
            this.label10.TabIndex = 196;
            this.label10.Text = "审核人";
            // 
            // tb出库类别
            // 
            this.tb出库类别.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb出库类别.Location = new System.Drawing.Point(355, 131);
            this.tb出库类别.Name = "tb出库类别";
            this.tb出库类别.Size = new System.Drawing.Size(141, 26);
            this.tb出库类别.TabIndex = 195;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(277, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 194;
            this.label8.Text = "出库类别";
            // 
            // tb业务类型
            // 
            this.tb业务类型.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb业务类型.Location = new System.Drawing.Point(95, 128);
            this.tb业务类型.Name = "tb业务类型";
            this.tb业务类型.Size = new System.Drawing.Size(141, 26);
            this.tb业务类型.TabIndex = 193;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(17, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 192;
            this.label7.Text = "业务类型";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(-179, -179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 190;
            this.label6.Text = "业务员";
            // 
            // tb业务员
            // 
            this.tb业务员.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb业务员.Location = new System.Drawing.Point(355, 63);
            this.tb业务员.Name = "tb业务员";
            this.tb业务员.Size = new System.Drawing.Size(141, 26);
            this.tb业务员.TabIndex = 189;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(271, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 188;
            this.label5.Text = "业务员";
            // 
            // btn查询
            // 
            this.btn查询.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn查询.Location = new System.Drawing.Point(511, 99);
            this.btn查询.Name = "btn查询";
            this.btn查询.Size = new System.Drawing.Size(75, 23);
            this.btn查询.TabIndex = 187;
            this.btn查询.Text = "查询";
            this.btn查询.UseVisualStyleBackColor = true;
            this.btn查询.Click += new System.EventHandler(this.btn查询_Click);
            // 
            // tb客户名称
            // 
            this.tb客户名称.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb客户名称.Location = new System.Drawing.Point(95, 95);
            this.tb客户名称.Name = "tb客户名称";
            this.tb客户名称.Size = new System.Drawing.Size(141, 26);
            this.tb客户名称.TabIndex = 186;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(17, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 185;
            this.label4.Text = "客户名称";
            // 
            // dtp出库日期
            // 
            this.dtp出库日期.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp出库日期.Location = new System.Drawing.Point(96, 63);
            this.dtp出库日期.Name = "dtp出库日期";
            this.dtp出库日期.Size = new System.Drawing.Size(141, 26);
            this.dtp出库日期.TabIndex = 184;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(14, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 182;
            this.label3.Text = "出库日期";
            // 
            // tb销售订单号
            // 
            this.tb销售订单号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb销售订单号.Location = new System.Drawing.Point(355, 99);
            this.tb销售订单号.Name = "tb销售订单号";
            this.tb销售订单号.Size = new System.Drawing.Size(141, 26);
            this.tb销售订单号.TabIndex = 181;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(261, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 180;
            this.label1.Text = "销售订单号";
            // 
            // lbl出库单号
            // 
            this.lbl出库单号.AutoSize = true;
            this.lbl出库单号.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl出库单号.Location = new System.Drawing.Point(246, 12);
            this.lbl出库单号.Name = "lbl出库单号";
            this.lbl出库单号.Size = new System.Drawing.Size(120, 16);
            this.lbl出库单号.TabIndex = 179;
            this.lbl出库单号.Text = "XSCK2017070071";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(160, 12);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 16);
            this.label15.TabIndex = 178;
            this.label15.Text = "出库单号：";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label44.Location = new System.Drawing.Point(821, 11);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(93, 16);
            this.label44.TabIndex = 128;
            this.label44.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(936, 11);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 127;
            this.label角色.Text = "角色";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(525, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 19);
            this.label2.TabIndex = 126;
            this.label2.Text = "出库单";
            // 
            // 出库单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 614);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn出库);
            this.Controls.Add(this.combox打印机选择);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tb审核人);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb出库类别);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb业务类型);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb业务员);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn查询);
            this.Controls.Add(this.tb客户名称);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp出库日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb销售订单号);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl出库单号);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.label2);
            this.Name = "出库单";
            this.Text = "出库单";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.出库单_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl出库单号;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb销售订单号;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp出库日期;
        private System.Windows.Forms.TextBox tb客户名称;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn查询;
        private System.Windows.Forms.TextBox tb业务员;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb业务类型;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb出库类别;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb审核人;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.ComboBox combox打印机选择;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn确认;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn出库;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button bt日志;
    }
}