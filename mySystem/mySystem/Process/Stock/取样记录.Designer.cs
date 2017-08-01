namespace mySystem.Process.Stock
{
    partial class 取样记录
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
            this.tb名称 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn删除 = new System.Windows.Forms.Button();
            this.btn添加 = new System.Windows.Forms.Button();
            this.btn查询插入 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb名称
            // 
            this.tb名称.Location = new System.Drawing.Point(86, 27);
            this.tb名称.Name = "tb名称";
            this.tb名称.Size = new System.Drawing.Size(100, 21);
            this.tb名称.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "名称";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(439, 340);
            this.dataGridView1.TabIndex = 2;
            // 
            // btn保存
            // 
            this.btn保存.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn保存.Location = new System.Drawing.Point(376, 435);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 42;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn删除
            // 
            this.btn删除.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn删除.Location = new System.Drawing.Point(188, 435);
            this.btn删除.Name = "btn删除";
            this.btn删除.Size = new System.Drawing.Size(75, 23);
            this.btn删除.TabIndex = 41;
            this.btn删除.Text = "删除";
            this.btn删除.UseVisualStyleBackColor = true;
            this.btn删除.Click += new System.EventHandler(this.btn删除_Click);
            // 
            // btn添加
            // 
            this.btn添加.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn添加.Location = new System.Drawing.Point(12, 435);
            this.btn添加.Name = "btn添加";
            this.btn添加.Size = new System.Drawing.Size(75, 23);
            this.btn添加.TabIndex = 40;
            this.btn添加.Text = "添加";
            this.btn添加.UseVisualStyleBackColor = true;
            this.btn添加.Click += new System.EventHandler(this.btn添加_Click);
            // 
            // btn查询插入
            // 
            this.btn查询插入.Location = new System.Drawing.Point(225, 24);
            this.btn查询插入.Name = "btn查询插入";
            this.btn查询插入.Size = new System.Drawing.Size(75, 23);
            this.btn查询插入.TabIndex = 43;
            this.btn查询插入.Text = "查询/插入";
            this.btn查询插入.UseVisualStyleBackColor = true;
            this.btn查询插入.Click += new System.EventHandler(this.btn查询插入_Click);
            // 
            // 取样记录
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 468);
            this.Controls.Add(this.btn查询插入);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.btn删除);
            this.Controls.Add(this.btn添加);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb名称);
            this.Name = "取样记录";
            this.Text = "取样记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb名称;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn删除;
        private System.Windows.Forms.Button btn添加;
        private System.Windows.Forms.Button btn查询插入;
    }
}