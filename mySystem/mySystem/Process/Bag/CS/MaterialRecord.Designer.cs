namespace mySystem.Process.Bag
{
    partial class MaterialRecord
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            this.tb产品批号 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb生产指令编号 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb成品率 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb废品重量 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb产品代码 = new System.Windows.Forms.TextBox();
            this.tb操作员备注 = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb操作员 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtp操作日期 = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.btn数据审核 = new System.Windows.Forms.Button();
            this.btn提交数据审核 = new System.Windows.Forms.Button();
            this.btn删除记录 = new System.Windows.Forms.Button();
            this.btn添加记录 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(451, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "生产领料使用记录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 423);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(368, 16);
            this.label3.TabIndex = 23;
            this.label3.Text = "备注：物料数量单位---膜材用米，其余物料用个。";
            // 
            // cb打印机
            // 
            this.cb打印机.Font = new System.Drawing.Font("SimSun", 12F);
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(323, 559);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(279, 24);
            this.cb打印机.TabIndex = 159;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("SimSun", 12F);
            this.label40.Location = new System.Drawing.Point(227, 563);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(104, 16);
            this.label40.TabIndex = 160;
            this.label40.Text = "选择打印机：";
            // 
            // btn查看日志
            // 
            this.btn查看日志.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn查看日志.Location = new System.Drawing.Point(947, 556);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(80, 30);
            this.btn查看日志.TabIndex = 158;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn提交审核.Location = new System.Drawing.Point(858, 556);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(80, 30);
            this.btn提交审核.TabIndex = 157;
            this.btn提交审核.Text = "最后审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn打印.Location = new System.Drawing.Point(116, 556);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(80, 30);
            this.btn打印.TabIndex = 156;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn审核.Location = new System.Drawing.Point(24, 556);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(80, 30);
            this.btn审核.TabIndex = 155;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn确认.Location = new System.Drawing.Point(769, 556);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(80, 30);
            this.btn确认.TabIndex = 154;
            this.btn确认.Text = "确认";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.btn确认_Click);
            // 
            // tb产品批号
            // 
            this.tb产品批号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb产品批号.Location = new System.Drawing.Point(475, 73);
            this.tb产品批号.Name = "tb产品批号";
            this.tb产品批号.Size = new System.Drawing.Size(160, 26);
            this.tb产品批号.TabIndex = 163;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(399, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 162;
            this.label5.Text = "产品批号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(21, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 161;
            this.label2.Text = "产品代码：";
            // 
            // tb生产指令编号
            // 
            this.tb生产指令编号.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb生产指令编号.Location = new System.Drawing.Point(863, 73);
            this.tb生产指令编号.Name = "tb生产指令编号";
            this.tb生产指令编号.Size = new System.Drawing.Size(164, 26);
            this.tb生产指令编号.TabIndex = 178;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("SimSun", 12F);
            this.label14.Location = new System.Drawing.Point(749, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 16);
            this.label14.TabIndex = 177;
            this.label14.Text = "生产指令编号：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point(880, 32);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(93, 16);
            this.label27.TabIndex = 181;
            this.label27.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(985, 32);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 180;
            this.label角色.Text = "角色";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(24, 110);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1003, 303);
            this.dataGridView1.TabIndex = 182;
            // 
            // tb成品率
            // 
            this.tb成品率.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb成品率.Location = new System.Drawing.Point(91, 472);
            this.tb成品率.Name = "tb成品率";
            this.tb成品率.Size = new System.Drawing.Size(104, 26);
            this.tb成品率.TabIndex = 184;
            this.tb成品率.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(22, 477);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(656, 16);
            this.label4.TabIndex = 183;
            this.label4.Text = "成品率：             （人工计算：产品数量计算成面积/用上面膜材规格和长度计算面积)";
            this.label4.Visible = false;
            // 
            // tb废品重量
            // 
            this.tb废品重量.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb废品重量.Location = new System.Drawing.Point(888, 470);
            this.tb废品重量.Name = "tb废品重量";
            this.tb废品重量.Size = new System.Drawing.Size(104, 26);
            this.tb废品重量.TabIndex = 186;
            this.tb废品重量.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F);
            this.label6.Location = new System.Drawing.Point(803, 477);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(224, 16);
            this.label6.TabIndex = 185;
            this.label6.Text = "废品重量：               kg";
            this.label6.Visible = false;
            // 
            // tb产品代码
            // 
            this.tb产品代码.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb产品代码.Location = new System.Drawing.Point(103, 73);
            this.tb产品代码.Name = "tb产品代码";
            this.tb产品代码.Size = new System.Drawing.Size(160, 26);
            this.tb产品代码.TabIndex = 187;
            // 
            // tb操作员备注
            // 
            this.tb操作员备注.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb操作员备注.Location = new System.Drawing.Point(292, 521);
            this.tb操作员备注.Name = "tb操作员备注";
            this.tb操作员备注.Size = new System.Drawing.Size(100, 26);
            this.tb操作员备注.TabIndex = 202;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("SimSun", 12F);
            this.label50.Location = new System.Drawing.Point(197, 525);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(104, 16);
            this.label50.TabIndex = 201;
            this.label50.Text = "操作员备注：";
            // 
            // tb审核员
            // 
            this.tb审核员.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb审核员.Location = new System.Drawing.Point(710, 522);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 26);
            this.tb审核员.TabIndex = 200;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("SimSun", 12F);
            this.label12.Location = new System.Drawing.Point(648, 526);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 199;
            this.label12.Text = "审核员：";
            // 
            // tb操作员
            // 
            this.tb操作员.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb操作员.Location = new System.Drawing.Point(86, 521);
            this.tb操作员.Name = "tb操作员";
            this.tb操作员.Size = new System.Drawing.Size(100, 26);
            this.tb操作员.TabIndex = 198;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("SimSun", 12F);
            this.label13.Location = new System.Drawing.Point(21, 525);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 16);
            this.label13.TabIndex = 197;
            this.label13.Text = "操作员：";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(896, 521);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(131, 26);
            this.dtp审核日期.TabIndex = 196;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F);
            this.label7.Location = new System.Drawing.Point(814, 526);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 195;
            this.label7.Text = "审核日期：";
            // 
            // dtp操作日期
            // 
            this.dtp操作日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp操作日期.Location = new System.Drawing.Point(492, 521);
            this.dtp操作日期.Name = "dtp操作日期";
            this.dtp操作日期.Size = new System.Drawing.Size(131, 26);
            this.dtp操作日期.TabIndex = 194;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("SimSun", 12F);
            this.label15.Location = new System.Drawing.Point(415, 526);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 16);
            this.label15.TabIndex = 193;
            this.label15.Text = "操作日期：";
            // 
            // btn数据审核
            // 
            this.btn数据审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn数据审核.Location = new System.Drawing.Point(937, 423);
            this.btn数据审核.Name = "btn数据审核";
            this.btn数据审核.Size = new System.Drawing.Size(90, 30);
            this.btn数据审核.TabIndex = 206;
            this.btn数据审核.Text = "数据审核";
            this.btn数据审核.UseVisualStyleBackColor = true;
            this.btn数据审核.Click += new System.EventHandler(this.btn数据审核_Click);
            // 
            // btn提交数据审核
            // 
            this.btn提交数据审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn提交数据审核.Location = new System.Drawing.Point(811, 424);
            this.btn提交数据审核.Name = "btn提交数据审核";
            this.btn提交数据审核.Size = new System.Drawing.Size(120, 30);
            this.btn提交数据审核.TabIndex = 205;
            this.btn提交数据审核.Text = "提交数据审核";
            this.btn提交数据审核.UseVisualStyleBackColor = true;
            this.btn提交数据审核.Click += new System.EventHandler(this.btn提交数据审核_Click);
            // 
            // btn删除记录
            // 
            this.btn删除记录.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn删除记录.Location = new System.Drawing.Point(735, 424);
            this.btn删除记录.Name = "btn删除记录";
            this.btn删除记录.Size = new System.Drawing.Size(70, 30);
            this.btn删除记录.TabIndex = 204;
            this.btn删除记录.Text = "删除";
            this.btn删除记录.UseVisualStyleBackColor = true;
            this.btn删除记录.Click += new System.EventHandler(this.btn删除记录_Click);
            // 
            // btn添加记录
            // 
            this.btn添加记录.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn添加记录.Location = new System.Drawing.Point(659, 424);
            this.btn添加记录.Name = "btn添加记录";
            this.btn添加记录.Size = new System.Drawing.Size(70, 30);
            this.btn添加记录.TabIndex = 203;
            this.btn添加记录.Text = "添加";
            this.btn添加记录.UseVisualStyleBackColor = true;
            this.btn添加记录.Click += new System.EventHandler(this.btn添加记录_Click);
            // 
            // MaterialRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 601);
            this.Controls.Add(this.btn数据审核);
            this.Controls.Add(this.btn提交数据审核);
            this.Controls.Add(this.btn删除记录);
            this.Controls.Add(this.btn添加记录);
            this.Controls.Add(this.tb操作员备注);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tb操作员);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtp操作日期);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tb产品代码);
            this.Controls.Add(this.tb废品重量);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb成品率);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.tb生产指令编号);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tb产品批号);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MaterialRecord";
            this.Text = "生产领料使用记录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialRecord_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn确认;
        private System.Windows.Forms.TextBox tb产品批号;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb生产指令编号;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb成品率;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb废品重量;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb产品代码;
        private System.Windows.Forms.TextBox tb操作员备注;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb操作员;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtp操作日期;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn数据审核;
        private System.Windows.Forms.Button btn提交数据审核;
        private System.Windows.Forms.Button btn删除记录;
        private System.Windows.Forms.Button btn添加记录;
    }
}