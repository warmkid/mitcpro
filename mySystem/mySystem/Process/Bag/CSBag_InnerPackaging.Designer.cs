namespace mySystem.Process.Bag
{
    partial class CSBag_InnerPackaging
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
            this.RecordView = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.生产日期时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内包序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.包装规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品数量包 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品数量只 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.热封线不合格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.黑点晶点 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.指示剂不良 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.其他 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.不良合计 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.包装袋热封线 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.内标签 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.内包装外观 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.制袋包装分检人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.复核人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NeightcheckBox = new System.Windows.Forms.CheckBox();
            this.DatecheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.CheckBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // RecordView
            // 
            this.RecordView.AllowUserToAddRows = false;
            this.RecordView.AllowUserToDeleteRows = false;
            this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.生产日期时间,
            this.内包序号,
            this.包装规格,
            this.产品数量包,
            this.产品数量只,
            this.热封线不合格,
            this.黑点晶点,
            this.指示剂不良,
            this.其他,
            this.不良合计,
            this.包装袋热封线,
            this.内标签,
            this.内包装外观,
            this.制袋包装分检人,
            this.复核人});
            this.RecordView.Location = new System.Drawing.Point(22, 87);
            this.RecordView.Name = "RecordView";
            this.RecordView.RowTemplate.Height = 23;
            this.RecordView.Size = new System.Drawing.Size(1291, 349);
            this.RecordView.TabIndex = 28;
            this.RecordView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellContentClick);
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            // 
            // 生产日期时间
            // 
            this.生产日期时间.HeaderText = "生产日期时间";
            this.生产日期时间.Name = "生产日期时间";
            // 
            // 内包序号
            // 
            this.内包序号.HeaderText = "内包序号";
            this.内包序号.Name = "内包序号";
            // 
            // 包装规格
            // 
            this.包装规格.HeaderText = "包装规格";
            this.包装规格.Name = "包装规格";
            // 
            // 产品数量包
            // 
            this.产品数量包.HeaderText = "产品数量包";
            this.产品数量包.Name = "产品数量包";
            // 
            // 产品数量只
            // 
            this.产品数量只.HeaderText = "产品数量只";
            this.产品数量只.Name = "产品数量只";
            // 
            // 热封线不合格
            // 
            this.热封线不合格.HeaderText = "热封线不合格";
            this.热封线不合格.Name = "热封线不合格";
            // 
            // 黑点晶点
            // 
            this.黑点晶点.HeaderText = "黑点晶点";
            this.黑点晶点.Name = "黑点晶点";
            // 
            // 指示剂不良
            // 
            this.指示剂不良.HeaderText = "指示剂不良";
            this.指示剂不良.Name = "指示剂不良";
            // 
            // 其他
            // 
            this.其他.HeaderText = "其他";
            this.其他.Name = "其他";
            // 
            // 不良合计
            // 
            this.不良合计.HeaderText = "不良合计";
            this.不良合计.Name = "不良合计";
            // 
            // 包装袋热封线
            // 
            this.包装袋热封线.HeaderText = "包装袋热封线";
            this.包装袋热封线.Name = "包装袋热封线";
            this.包装袋热封线.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.包装袋热封线.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 内标签
            // 
            this.内标签.HeaderText = "内标签";
            this.内标签.Name = "内标签";
            this.内标签.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.内标签.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 内包装外观
            // 
            this.内包装外观.HeaderText = "内包装外观";
            this.内包装外观.Name = "内包装外观";
            this.内包装外观.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.内包装外观.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 制袋包装分检人
            // 
            this.制袋包装分检人.HeaderText = "制袋包装分检人";
            this.制袋包装分检人.Name = "制袋包装分检人";
            // 
            // 复核人
            // 
            this.复核人.HeaderText = "复核人";
            this.复核人.Name = "复核人";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(573, 9);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(149, 19);
            this.Title.TabIndex = 29;
            this.Title.Text = "产品内包装记录";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox1.Location = new System.Drawing.Point(133, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 26);
            this.textBox1.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(19, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 16);
            this.label5.TabIndex = 39;
            this.label5.Text = "生产指令编号：";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox2.Location = new System.Drawing.Point(491, 48);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(150, 26);
            this.textBox2.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(406, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "产品代码：";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBox3.Location = new System.Drawing.Point(835, 48);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(150, 26);
            this.textBox3.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(751, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 43;
            this.label2.Text = "生产批号：";
            // 
            // NeightcheckBox
            // 
            this.NeightcheckBox.AutoSize = true;
            this.NeightcheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.NeightcheckBox.Location = new System.Drawing.Point(1258, 54);
            this.NeightcheckBox.Name = "NeightcheckBox";
            this.NeightcheckBox.Size = new System.Drawing.Size(59, 20);
            this.NeightcheckBox.TabIndex = 47;
            this.NeightcheckBox.Text = "英文";
            this.NeightcheckBox.UseVisualStyleBackColor = true;
            // 
            // DatecheckBox
            // 
            this.DatecheckBox.AutoSize = true;
            this.DatecheckBox.Checked = true;
            this.DatecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DatecheckBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.DatecheckBox.Location = new System.Drawing.Point(1195, 54);
            this.DatecheckBox.Name = "DatecheckBox";
            this.DatecheckBox.Size = new System.Drawing.Size(59, 20);
            this.DatecheckBox.TabIndex = 46;
            this.DatecheckBox.Text = "中文";
            this.DatecheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(1134, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 45;
            this.label3.Text = "标签：";
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(1233, 442);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 51;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // CheckBtn
            // 
            this.CheckBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.CheckBtn.Location = new System.Drawing.Point(1233, 481);
            this.CheckBtn.Name = "CheckBtn";
            this.CheckBtn.Size = new System.Drawing.Size(80, 30);
            this.CheckBtn.TabIndex = 50;
            this.CheckBtn.Text = "审核通过";
            this.CheckBtn.UseVisualStyleBackColor = true;
            this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.SaveBtn.Location = new System.Drawing.Point(1141, 481);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 30);
            this.SaveBtn.TabIndex = 49;
            this.SaveBtn.Text = "确认";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("SimSun", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(1141, 442);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 48;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(21, 449);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(456, 48);
            this.label4.TabIndex = 52;
            this.label4.Text = "注：1. 外观不良填写数量，不良品处理方式-报废，称重记录。\r\n    2. 包装项中符合填写“√”，不符合取消勾选\r\n    3. 每次填写间隔不超过2小时。";
            // 
            // CSBag_InnerPackaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 526);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.CheckBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.NeightcheckBox);
            this.Controls.Add(this.DatecheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.RecordView);
            this.Name = "CSBag_InnerPackaging";
            this.Text = "CSBag_InnerPackaging";
            ((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView RecordView;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox NeightcheckBox;
        private System.Windows.Forms.CheckBox DatecheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生产日期时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内包序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 包装规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品数量包;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品数量只;
        private System.Windows.Forms.DataGridViewTextBoxColumn 热封线不合格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 黑点晶点;
        private System.Windows.Forms.DataGridViewTextBoxColumn 指示剂不良;
        private System.Windows.Forms.DataGridViewTextBoxColumn 其他;
        private System.Windows.Forms.DataGridViewTextBoxColumn 不良合计;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 包装袋热封线;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 内标签;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 内包装外观;
        private System.Windows.Forms.DataGridViewTextBoxColumn 制袋包装分检人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 复核人;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button CheckBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Label label4;
    }
}