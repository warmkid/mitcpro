namespace 订单和库存管理
{
    partial class 添加采购单
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
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb采购人 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb采购单名称 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp采购时间 = new System.Windows.Forms.DateTimePicker();
            this.tb采购单总价 = new System.Windows.Forms.TextBox();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(518, 24);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(137, 23);
            this.btn查询插入.TabIndex = 25;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(580, 423);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 24;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(298, 423);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 23;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(22, 423);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 22;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(22, 120);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(633, 166);
            this.dataGridView1.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(473, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "采购单总价";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "采购时间";
            // 
            // tb采购人
            // 
            this.tb采购人.Location = new System.Drawing.Point(310, 75);
            this.tb采购人.Name = "tb采购人";
            this.tb采购人.Size = new System.Drawing.Size(100, 21);
            this.tb采购人.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "采购人";
            // 
            // tb采购单名称
            // 
            this.tb采购单名称.Location = new System.Drawing.Point(100, 26);
            this.tb采购单名称.Name = "tb采购单名称";
            this.tb采购单名称.Size = new System.Drawing.Size(391, 21);
            this.tb采购单名称.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "采购单名称";
            // 
            // dtp采购时间
            // 
            this.dtp采购时间.Location = new System.Drawing.Point(91, 72);
            this.dtp采购时间.Name = "dtp采购时间";
            this.dtp采购时间.Size = new System.Drawing.Size(121, 21);
            this.dtp采购时间.TabIndex = 26;
            // 
            // tb采购单总价
            // 
            this.tb采购单总价.Location = new System.Drawing.Point(555, 72);
            this.tb采购单总价.Name = "tb采购单总价";
            this.tb采购单总价.Size = new System.Drawing.Size(100, 21);
            this.tb采购单总价.TabIndex = 27;
            // 
            // tb备注
            // 
            this.tb备注.Location = new System.Drawing.Point(22, 325);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(633, 81);
            this.tb备注.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 297);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 29;
            this.label5.Text = "备注";
            // 
            // 添加采购单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 485);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.tb采购单总价);
            this.Controls.Add(this.dtp采购时间);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb采购人);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb采购单名称);
            this.Controls.Add(this.label1);
            this.Name = "添加采购单";
            this.Text = "添加采购单";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb采购人;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb采购单名称;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp采购时间;
        private System.Windows.Forms.TextBox tb采购单总价;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Label label5;
    }
}