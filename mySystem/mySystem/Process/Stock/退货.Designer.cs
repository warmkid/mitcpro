namespace 订单和库存管理
{
    partial class 退货
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
            this.btn退货 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb退货人 = new System.Windows.Forms.TextBox();
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.dtp退货时间 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tb退货单名称 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb订单名称 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb退货原因 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn退货
            // 
            this.btn退货.Location = new System.Drawing.Point(314, 464);
            this.btn退货.Name = "btn退货";
            this.btn退货.Size = new System.Drawing.Size(75, 23);
            this.btn退货.TabIndex = 32;
            this.btn退货.Text = "退货";
            this.btn退货.UseVisualStyleBackColor = true;
            this.btn退货.Click += new System.EventHandler(this.btn退货_Click);
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(172, 464);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 31;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(32, 464);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 30;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 165);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(357, 177);
            this.dataGridView1.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "退货时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "退货人";
            // 
            // tb退货人
            // 
            this.tb退货人.Location = new System.Drawing.Point(112, 63);
            this.tb退货人.Name = "tb退货人";
            this.tb退货人.Size = new System.Drawing.Size(150, 21);
            this.tb退货人.TabIndex = 26;
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(291, 26);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(98, 58);
            this.btn查询插入.TabIndex = 25;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // dtp退货时间
            // 
            this.dtp退货时间.Location = new System.Drawing.Point(112, 96);
            this.dtp退货时间.Name = "dtp退货时间";
            this.dtp退货时间.Size = new System.Drawing.Size(277, 21);
            this.dtp退货时间.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "退货单名称";
            // 
            // tb退货单名称
            // 
            this.tb退货单名称.Location = new System.Drawing.Point(112, 26);
            this.tb退货单名称.Name = "tb退货单名称";
            this.tb退货单名称.Size = new System.Drawing.Size(150, 21);
            this.tb退货单名称.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "订单名称";
            // 
            // cmb订单名称
            // 
            this.cmb订单名称.FormattingEnabled = true;
            this.cmb订单名称.Location = new System.Drawing.Point(112, 131);
            this.cmb订单名称.Name = "cmb订单名称";
            this.cmb订单名称.Size = new System.Drawing.Size(277, 20);
            this.cmb订单名称.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 355);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 35;
            this.label5.Text = "退货原因";
            // 
            // tb退货原因
            // 
            this.tb退货原因.Location = new System.Drawing.Point(32, 379);
            this.tb退货原因.Multiline = true;
            this.tb退货原因.Name = "tb退货原因";
            this.tb退货原因.Size = new System.Drawing.Size(357, 79);
            this.tb退货原因.TabIndex = 36;
            // 
            // 退货
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 499);
            this.Controls.Add(this.tb退货原因);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmb订单名称);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn退货);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb退货人);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.dtp退货时间);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb退货单名称);
            this.Name = "退货";
            this.Text = "退货";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn退货;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb退货人;
        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.DateTimePicker dtp退货时间;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb退货单名称;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb订单名称;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb退货原因;
    }
}