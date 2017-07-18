namespace 订单和库存管理
{
    partial class 添加订单
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
            this.tb订单名称 = new System.Windows.Forms.TextBox();
            this.cmb产品类型 = new System.Windows.Forms.ComboBox();
            this.tb客户名称 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb产品名称 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn添加 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.dtp交付时间 = new System.Windows.Forms.DateTimePicker();
            this.tb费用 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "订单名称";
            // 
            // tb订单名称
            // 
            this.tb订单名称.Location = new System.Drawing.Point(71, 27);
            this.tb订单名称.Name = "tb订单名称";
            this.tb订单名称.Size = new System.Drawing.Size(100, 21);
            this.tb订单名称.TabIndex = 1;
            // 
            // cmb产品类型
            // 
            this.cmb产品类型.FormattingEnabled = true;
            this.cmb产品类型.Location = new System.Drawing.Point(71, 72);
            this.cmb产品类型.Name = "cmb产品类型";
            this.cmb产品类型.Size = new System.Drawing.Size(121, 20);
            this.cmb产品类型.TabIndex = 2;
            // 
            // tb客户名称
            // 
            this.tb客户名称.Location = new System.Drawing.Point(430, 30);
            this.tb客户名称.Name = "tb客户名称";
            this.tb客户名称.Size = new System.Drawing.Size(100, 21);
            this.tb客户名称.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "客户名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "产品类型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(341, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "产品名称";
            // 
            // cmb产品名称
            // 
            this.cmb产品名称.FormattingEnabled = true;
            this.cmb产品名称.Location = new System.Drawing.Point(409, 75);
            this.cmb产品名称.Name = "cmb产品名称";
            this.cmb产品名称.Size = new System.Drawing.Size(121, 20);
            this.cmb产品名称.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 150);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(518, 150);
            this.dataGridView1.TabIndex = 8;
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(12, 423);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 9;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(242, 423);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 10;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(455, 423);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 11;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(213, 25);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(75, 23);
            this.btn查询插入.TabIndex = 12;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // dtp交付时间
            // 
            this.dtp交付时间.Location = new System.Drawing.Point(71, 115);
            this.dtp交付时间.Name = "dtp交付时间";
            this.dtp交付时间.Size = new System.Drawing.Size(121, 21);
            this.dtp交付时间.TabIndex = 13;
            // 
            // tb费用
            // 
            this.tb费用.Location = new System.Drawing.Point(409, 118);
            this.tb费用.Name = "tb费用";
            this.tb费用.Size = new System.Drawing.Size(121, 21);
            this.tb费用.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "交付时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(341, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "费用";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 312);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "备注";
            // 
            // tb备注
            // 
            this.tb备注.Location = new System.Drawing.Point(12, 330);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(518, 79);
            this.tb备注.TabIndex = 18;
            // 
            // 添加订单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 475);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb费用);
            this.Controls.Add(this.dtp交付时间);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmb产品名称);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb客户名称);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmb产品类型);
            this.Controls.Add(this.tb订单名称);
            this.Controls.Add(this.label1);
            this.Name = "添加订单";
            this.Text = "添加订单";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb订单名称;
        private System.Windows.Forms.ComboBox cmb产品类型;
        private System.Windows.Forms.TextBox tb客户名称;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb产品名称;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.DateTimePicker dtp交付时间;
        private System.Windows.Forms.TextBox tb费用;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb备注;
    }
}