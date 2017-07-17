namespace 订单和库存管理
{
    partial class 入库
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
            this.tb入库单名称 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp入库时间 = new System.Windows.Forms.DateTimePicker();
            this.btn查询插入 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb入库人 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn添加 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn入库 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb入库单名称
            // 
            this.tb入库单名称.Location = new System.Drawing.Point(100, 23);
            this.tb入库单名称.Name = "tb入库单名称";
            this.tb入库单名称.Size = new System.Drawing.Size(150, 21);
            this.tb入库单名称.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "入库单名称";
            // 
            // dtp入库时间
            // 
            this.dtp入库时间.Location = new System.Drawing.Point(100, 93);
            this.dtp入库时间.Name = "dtp入库时间";
            this.dtp入库时间.Size = new System.Drawing.Size(277, 21);
            this.dtp入库时间.TabIndex = 2;
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(279, 26);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(98, 58);
            this.btn查询插入.TabIndex = 3;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "入库人";
            // 
            // tb入库人
            // 
            this.tb入库人.Location = new System.Drawing.Point(100, 60);
            this.tb入库人.Name = "tb入库人";
            this.tb入库人.Size = new System.Drawing.Size(150, 21);
            this.tb入库人.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "入库时间";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 134);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(357, 177);
            this.dataGridView1.TabIndex = 7;
            // 
            // btn添加
            // 
            this.btn添加.Location = new System.Drawing.Point(20, 408);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 8;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // btn删除
            // 
            this.btn删除.Location = new System.Drawing.Point(160, 408);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 9;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn入库
            // 
            this.btn入库.Location = new System.Drawing.Point(302, 408);
            this.btn入库.Name = "btn入库";
            this.btn入库.Size = new System.Drawing.Size(75, 23);
            this.btn入库.TabIndex = 10;
            this.btn入库.Text = "入库";
            this.btn入库.UseVisualStyleBackColor = true;
            this.btn入库.Click += new System.EventHandler(this.btn入库_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 323);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "备注";
            // 
            // tb备注
            // 
            this.tb备注.Location = new System.Drawing.Point(20, 341);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(357, 61);
            this.tb备注.TabIndex = 12;
            // 
            // 入库
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 474);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn入库);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb入库人);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.dtp入库时间);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb入库单名称);
            this.Name = "入库";
            this.Text = "入库";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb入库单名称;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp入库时间;
        private System.Windows.Forms.Button btn查询插入;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb入库人;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn入库;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb备注;
    }
}