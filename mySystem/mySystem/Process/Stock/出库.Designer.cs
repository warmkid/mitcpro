namespace 订单和库存管理
{
    partial class 出库
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
            this.btn出库 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb出库人 = new System.Windows.Forms.TextBox();
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.dtp出库时间 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tb出库单名称 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn出库
            // 
            this.btn出库.Location = new System.Drawing.Point(304, 411);
            this.btn出库.Name = "btn出库";
            this.btn出库.Size = new System.Drawing.Size(75, 23);
            this.btn出库.TabIndex = 21;
            this.btn出库.Text = "出库";
            this.btn出库.UseVisualStyleBackColor = true;
            this.btn出库.Click += new System.EventHandler(this.btn出库_Click);
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(162, 411);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 20;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(22, 411);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 19;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(22, 131);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(357, 177);
            this.dataGridView1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "出库时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "出库人";
            // 
            // tb出库人
            // 
            this.tb出库人.Location = new System.Drawing.Point(102, 57);
            this.tb出库人.Name = "tb出库人";
            this.tb出库人.Size = new System.Drawing.Size(150, 21);
            this.tb出库人.TabIndex = 15;
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(281, 20);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(98, 58);
            this.btn查询插入.TabIndex = 14;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // dtp出库时间
            // 
            this.dtp出库时间.Location = new System.Drawing.Point(102, 90);
            this.dtp出库时间.Name = "dtp出库时间";
            this.dtp出库时间.Size = new System.Drawing.Size(277, 21);
            this.dtp出库时间.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "出库单名称";
            // 
            // tb出库单名称
            // 
            this.tb出库单名称.Location = new System.Drawing.Point(102, 20);
            this.tb出库单名称.Name = "tb出库单名称";
            this.tb出库单名称.Size = new System.Drawing.Size(150, 21);
            this.tb出库单名称.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "备注";
            // 
            // tb备注
            // 
            this.tb备注.Location = new System.Drawing.Point(22, 339);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(357, 54);
            this.tb备注.TabIndex = 23;
            // 
            // 出库
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 478);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn出库);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb出库人);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.dtp出库时间);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb出库单名称);
            this.Name = "出库";
            this.Text = "出库";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn出库;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb出库人;
        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.DateTimePicker dtp出库时间;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb出库单名称;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb备注;
    }
}