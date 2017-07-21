namespace BatchProductRecord
{
    partial class BatchProductRecord
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtp结束时间 = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp开始时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp批准时间 = new System.Windows.Forms.DateTimePicker();
            this.tb批准 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtp审核时间 = new System.Windows.Forms.DateTimePicker();
            this.tb审核 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtp汇总时间 = new System.Windows.Forms.DateTimePicker();
            this.tb汇总 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.tb使用物料 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb生产指令 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bt日志 = new System.Windows.Forms.Button();
            this.bt发送审核 = new System.Windows.Forms.Button();
            this.bt打印 = new System.Windows.Forms.Button();
            this.bt审核 = new System.Windows.Forms.Button();
            this.bt确认 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(433, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "批生产记录（吹膜）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(206, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "汇总审批";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(217, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "吹膜工序记录 目录";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.Location = new System.Drawing.Point(3, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(572, 330);
            this.dataGridView1.TabIndex = 1;
            // 
            // Column1
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.Width = 70;
            // 
            // Column2
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column2.HeaderText = "记录";
            this.Column2.Name = "Column2";
            this.Column2.Width = 400;
            // 
            // Column3
            // 
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column3.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column3.HeaderText = "页数";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column4.HeaderText = "备注";
            this.Column4.Name = "Column4";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 46);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tb备注);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dtp结束时间);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.dtp开始时间);
            this.splitContainer1.Panel2.Controls.Add(this.dtp批准时间);
            this.splitContainer1.Panel2.Controls.Add(this.tb批准);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.dtp审核时间);
            this.splitContainer1.Panel2.Controls.Add(this.tb审核);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.dtp汇总时间);
            this.splitContainer1.Panel2.Controls.Add(this.tb汇总);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.tb使用物料);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.tb生产指令);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitContainer1.Size = new System.Drawing.Size(1053, 519);
            this.splitContainer1.SplitterDistance = 576;
            this.splitContainer1.TabIndex = 2;
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb备注.Location = new System.Drawing.Point(44, 399);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(521, 105);
            this.tb备注.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(3, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 14);
            this.label10.TabIndex = 2;
            this.label10.Text = "备注";
            // 
            // dtp结束时间
            // 
            this.dtp结束时间.Location = new System.Drawing.Point(302, 121);
            this.dtp结束时间.Name = "dtp结束时间";
            this.dtp结束时间.Size = new System.Drawing.Size(153, 26);
            this.dtp结束时间.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(272, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 16);
            this.label11.TabIndex = 18;
            this.label11.Text = "到";
            // 
            // dtp开始时间
            // 
            this.dtp开始时间.Location = new System.Drawing.Point(113, 122);
            this.dtp开始时间.Name = "dtp开始时间";
            this.dtp开始时间.Size = new System.Drawing.Size(153, 26);
            this.dtp开始时间.TabIndex = 17;
            // 
            // dtp批准时间
            // 
            this.dtp批准时间.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp批准时间.Location = new System.Drawing.Point(260, 478);
            this.dtp批准时间.Name = "dtp批准时间";
            this.dtp批准时间.Size = new System.Drawing.Size(150, 23);
            this.dtp批准时间.TabIndex = 16;
            // 
            // tb批准
            // 
            this.tb批准.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb批准.Location = new System.Drawing.Point(136, 478);
            this.tb批准.Name = "tb批准";
            this.tb批准.Size = new System.Drawing.Size(103, 23);
            this.tb批准.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(74, 481);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 14);
            this.label9.TabIndex = 14;
            this.label9.Text = "批准";
            // 
            // dtp审核时间
            // 
            this.dtp审核时间.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp审核时间.Location = new System.Drawing.Point(260, 441);
            this.dtp审核时间.Name = "dtp审核时间";
            this.dtp审核时间.Size = new System.Drawing.Size(150, 23);
            this.dtp审核时间.TabIndex = 13;
            // 
            // tb审核
            // 
            this.tb审核.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb审核.Location = new System.Drawing.Point(136, 438);
            this.tb审核.Name = "tb审核";
            this.tb审核.Size = new System.Drawing.Size(103, 23);
            this.tb审核.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(74, 441);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 14);
            this.label8.TabIndex = 11;
            this.label8.Text = "审核";
            // 
            // dtp汇总时间
            // 
            this.dtp汇总时间.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp汇总时间.Location = new System.Drawing.Point(260, 399);
            this.dtp汇总时间.Name = "dtp汇总时间";
            this.dtp汇总时间.Size = new System.Drawing.Size(150, 23);
            this.dtp汇总时间.TabIndex = 10;
            // 
            // tb汇总
            // 
            this.tb汇总.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb汇总.Location = new System.Drawing.Point(136, 399);
            this.tb汇总.Name = "tb汇总";
            this.tb汇总.Size = new System.Drawing.Size(103, 23);
            this.tb汇总.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(74, 402);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 14);
            this.label7.TabIndex = 8;
            this.label7.Text = "汇总";
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6,
            this.Column7});
            this.dataGridView2.Location = new System.Drawing.Point(4, 165);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(466, 215);
            this.dataGridView2.TabIndex = 7;
            // 
            // Column5
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column5.DefaultCellStyle = dataGridViewCellStyle12;
            this.Column5.HeaderText = "产品编码";
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column6.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column6.HeaderText = "产品批号";
            this.Column6.Name = "Column6";
            this.Column6.Width = 150;
            // 
            // Column7
            // 
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column7.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column7.HeaderText = "生产数量（米）";
            this.Column7.Name = "Column7";
            this.Column7.Width = 150;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(23, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "生产时段";
            // 
            // tb使用物料
            // 
            this.tb使用物料.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb使用物料.Location = new System.Drawing.Point(112, 84);
            this.tb使用物料.Name = "tb使用物料";
            this.tb使用物料.ReadOnly = true;
            this.tb使用物料.Size = new System.Drawing.Size(170, 23);
            this.tb使用物料.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(23, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "使用物料";
            // 
            // tb生产指令
            // 
            this.tb生产指令.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb生产指令.Location = new System.Drawing.Point(112, 47);
            this.tb生产指令.Name = "tb生产指令";
            this.tb生产指令.ReadOnly = true;
            this.tb生产指令.Size = new System.Drawing.Size(170, 23);
            this.tb生产指令.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(23, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "生产指令";
            // 
            // bt日志
            // 
            this.bt日志.Location = new System.Drawing.Point(937, 587);
            this.bt日志.Name = "bt日志";
            this.bt日志.Size = new System.Drawing.Size(89, 23);
            this.bt日志.TabIndex = 48;
            this.bt日志.Text = "查看日志";
            this.bt日志.UseVisualStyleBackColor = true;
            // 
            // bt发送审核
            // 
            this.bt发送审核.Location = new System.Drawing.Point(827, 587);
            this.bt发送审核.Name = "bt发送审核";
            this.bt发送审核.Size = new System.Drawing.Size(89, 23);
            this.bt发送审核.TabIndex = 47;
            this.bt发送审核.Text = "发送审核";
            this.bt发送审核.UseVisualStyleBackColor = true;
            // 
            // bt打印
            // 
            this.bt打印.Location = new System.Drawing.Point(114, 587);
            this.bt打印.Name = "bt打印";
            this.bt打印.Size = new System.Drawing.Size(63, 23);
            this.bt打印.TabIndex = 46;
            this.bt打印.Text = "打印";
            this.bt打印.UseVisualStyleBackColor = true;
            // 
            // bt审核
            // 
            this.bt审核.Location = new System.Drawing.Point(25, 587);
            this.bt审核.Name = "bt审核";
            this.bt审核.Size = new System.Drawing.Size(63, 23);
            this.bt审核.TabIndex = 45;
            this.bt审核.Text = "审核";
            this.bt审核.UseVisualStyleBackColor = true;
            // 
            // bt确认
            // 
            this.bt确认.Location = new System.Drawing.Point(748, 587);
            this.bt确认.Name = "bt确认";
            this.bt确认.Size = new System.Drawing.Size(63, 23);
            this.bt确认.TabIndex = 44;
            this.bt确认.Text = "确认";
            this.bt确认.UseVisualStyleBackColor = true;
            this.bt确认.Click += new System.EventHandler(this.bt确认_Click);
            // 
            // BatchProductRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 622);
            this.Controls.Add(this.bt日志);
            this.Controls.Add(this.bt发送审核);
            this.Controls.Add(this.bt打印);
            this.Controls.Add(this.bt审核);
            this.Controls.Add(this.bt确认);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BatchProductRecord";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb使用物料;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb生产指令;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtp批准时间;
        private System.Windows.Forms.TextBox tb批准;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtp审核时间;
        private System.Windows.Forms.TextBox tb审核;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtp汇总时间;
        private System.Windows.Forms.TextBox tb汇总;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.Button bt日志;
        private System.Windows.Forms.Button bt发送审核;
        private System.Windows.Forms.Button bt打印;
        private System.Windows.Forms.Button bt审核;
        private System.Windows.Forms.Button bt确认;
        private System.Windows.Forms.DateTimePicker dtp结束时间;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp开始时间;

    }
}

