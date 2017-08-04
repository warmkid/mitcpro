namespace mySystem.Process.Stock
{
    partial class 物资验收记录
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
            this.Title = new System.Windows.Forms.Label();
            this.cmb供应商代码 = new System.Windows.Forms.ComboBox();
            this.tb检验报告理由 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn保存 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp请验时间 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtp接收时间 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tb验收人 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb厂家随附检验报告 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb是否有样品 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb样品理由 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb请验人 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp审核时间 = new System.Windows.Forms.DateTimePicker();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.btn上移 = new System.Windows.Forms.Button();
            this.btn下移 = new System.Windows.Forms.Button();
            this.lbl验收记录编号 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl供应商名称 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(284, 31);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(129, 19);
            this.Title.TabIndex = 4;
            this.Title.Text = "物资验收记录";
            // 
            // cmb供应商代码
            // 
            this.cmb供应商代码.FormattingEnabled = true;
            this.cmb供应商代码.Location = new System.Drawing.Point(100, 81);
            this.cmb供应商代码.Name = "cmb供应商代码";
            this.cmb供应商代码.Size = new System.Drawing.Size(121, 20);
            this.cmb供应商代码.TabIndex = 5;
            // 
            // tb检验报告理由
            // 
            this.tb检验报告理由.Location = new System.Drawing.Point(256, 448);
            this.tb检验报告理由.Name = "tb检验报告理由";
            this.tb检验报告理由.Size = new System.Drawing.Size(100, 21);
            this.tb检验报告理由.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 452);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "理由：";
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(399, 546);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 8;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "供应商代码";
            // 
            // dtp请验时间
            // 
            this.dtp请验时间.Location = new System.Drawing.Point(229, 503);
            this.dtp请验时间.Name = "dtp请验时间";
            this.dtp请验时间.Size = new System.Drawing.Size(109, 21);
            this.dtp请验时间.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "接收时间";
            // 
            // dtp接收时间
            // 
            this.dtp接收时间.Location = new System.Drawing.Point(304, 78);
            this.dtp接收时间.Name = "dtp接收时间";
            this.dtp接收时间.Size = new System.Drawing.Size(200, 21);
            this.dtp接收时间.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(534, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "验收人";
            // 
            // tb验收人
            // 
            this.tb验收人.Location = new System.Drawing.Point(581, 78);
            this.tb验收人.Name = "tb验收人";
            this.tb验收人.Size = new System.Drawing.Size(100, 21);
            this.tb验收人.TabIndex = 13;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 171);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(662, 221);
            this.dataGridView1.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 452);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "厂家随附检验报告";
            // 
            // cmb厂家随附检验报告
            // 
            this.cmb厂家随附检验报告.FormattingEnabled = true;
            this.cmb厂家随附检验报告.Location = new System.Drawing.Point(127, 449);
            this.cmb厂家随附检验报告.Name = "cmb厂家随附检验报告";
            this.cmb厂家随附检验报告.Size = new System.Drawing.Size(52, 20);
            this.cmb厂家随附检验报告.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 452);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "是否有样品";
            // 
            // cmb是否有样品
            // 
            this.cmb是否有样品.FormattingEnabled = true;
            this.cmb是否有样品.Location = new System.Drawing.Point(471, 448);
            this.cmb是否有样品.Name = "cmb是否有样品";
            this.cmb是否有样品.Size = new System.Drawing.Size(52, 20);
            this.cmb是否有样品.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(544, 451);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "理由：";
            // 
            // tb样品理由
            // 
            this.tb样品理由.Location = new System.Drawing.Point(581, 447);
            this.tb样品理由.Name = "tb样品理由";
            this.tb样品理由.Size = new System.Drawing.Size(100, 21);
            this.tb样品理由.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 509);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "请验人";
            // 
            // tb请验人
            // 
            this.tb请验人.Location = new System.Drawing.Point(64, 504);
            this.tb请验人.Name = "tb请验人";
            this.tb请验人.Size = new System.Drawing.Size(100, 21);
            this.tb请验人.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(170, 509);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "请验时间";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(512, 509);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "审核时间";
            // 
            // tb审核员
            // 
            this.tb审核员.Location = new System.Drawing.Point(406, 504);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 21);
            this.tb审核员.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(359, 509);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "审核员";
            // 
            // dtp审核时间
            // 
            this.dtp审核时间.Location = new System.Drawing.Point(571, 503);
            this.dtp审核时间.Name = "dtp审核时间";
            this.dtp审核时间.Size = new System.Drawing.Size(109, 21);
            this.dtp审核时间.TabIndex = 25;
            // 
            // btn提交审核
            // 
            this.btn提交审核.Location = new System.Drawing.Point(504, 546);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(75, 23);
            this.btn提交审核.TabIndex = 29;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn查看日志
            // 
            this.btn查看日志.Location = new System.Drawing.Point(607, 546);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(75, 23);
            this.btn查看日志.TabIndex = 30;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(19, 546);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 31;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(19, 408);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 32;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // btn上移
            // 
            this.btn上移.Location = new System.Drawing.Point(127, 408);
            this.btn上移.Name = "btn上移";
            this.btn上移.Size = new System.Drawing.Size(75, 23);
            this.btn上移.TabIndex = 33;
            this.btn上移.Text = "上移";
            this.btn上移.UseVisualStyleBackColor = true;
            // 
            // btn下移
            // 
            this.btn下移.Location = new System.Drawing.Point(229, 408);
            this.btn下移.Name = "btn下移";
            this.btn下移.Size = new System.Drawing.Size(75, 23);
            this.btn下移.TabIndex = 34;
            this.btn下移.Text = "下移";
            this.btn下移.UseVisualStyleBackColor = true;
            // 
            // lbl验收记录编号
            // 
            this.lbl验收记录编号.AutoSize = true;
            this.lbl验收记录编号.Location = new System.Drawing.Point(344, 134);
            this.lbl验收记录编号.Name = "lbl验收记录编号";
            this.lbl验收记录编号.Size = new System.Drawing.Size(0, 12);
            this.lbl验收记录编号.TabIndex = 35;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(245, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 12);
            this.label12.TabIndex = 36;
            this.label12.Text = "验收记录编号：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(546, 31);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(49, 19);
            this.label角色.TabIndex = 37;
            this.label角色.Text = "角色";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 134);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 39;
            this.label13.Text = "供应商名称";
            // 
            // lbl供应商名称
            // 
            this.lbl供应商名称.AutoSize = true;
            this.lbl供应商名称.Location = new System.Drawing.Point(98, 134);
            this.lbl供应商名称.Name = "lbl供应商名称";
            this.lbl供应商名称.Size = new System.Drawing.Size(0, 12);
            this.lbl供应商名称.TabIndex = 40;
            // 
            // 物资验收记录
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 609);
            this.Controls.Add(this.lbl供应商名称);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lbl验收记录编号);
            this.Controls.Add(this.btn下移);
            this.Controls.Add(this.btn上移);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dtp审核时间);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb请验人);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb样品理由);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmb是否有样品);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmb厂家随附检验报告);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb验收人);
            this.Controls.Add(this.dtp接收时间);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtp请验时间);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb检验报告理由);
            this.Controls.Add(this.cmb供应商代码);
            this.Controls.Add(this.Title);
            this.Name = "物资验收记录";
            this.Text = "增加物资验收记录";
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.ComboBox cmb供应商代码;
        private System.Windows.Forms.TextBox tb检验报告理由;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp请验时间;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp接收时间;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb验收人;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb厂家随附检验报告;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb是否有样品;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb样品理由;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb请验人;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp审核时间;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn上移;
        private System.Windows.Forms.Button btn下移;
        private System.Windows.Forms.Label lbl验收记录编号;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbl供应商名称;
    }
}