namespace mySystem.Other
{
    partial class BOMList
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.存货代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.存货名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn导入 = new System.Windows.Forms.Button();
            this.btn浏览 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.存货代码,
            this.存货名称,
            this.规格型号,
            this.数量});
            this.dataGridView1.Location = new System.Drawing.Point(12, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(555, 203);
            this.dataGridView1.TabIndex = 0;
            // 
            // 存货代码
            // 
            this.存货代码.HeaderText = "存货代码";
            this.存货代码.Name = "存货代码";
            this.存货代码.Width = 150;
            // 
            // 存货名称
            // 
            this.存货名称.HeaderText = "存货名称";
            this.存货名称.Name = "存货名称";
            this.存货名称.Width = 150;
            // 
            // 规格型号
            // 
            this.规格型号.HeaderText = "规格型号";
            this.规格型号.Name = "规格型号";
            this.规格型号.Width = 150;
            // 
            // 数量
            // 
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.Width = 60;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 238);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(492, 238);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 2;
            this.btnDone.Text = "完成";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(93, 238);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(186, 240);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 21);
            this.textBox1.TabIndex = 4;
            // 
            // btn导入
            // 
            this.btn导入.Location = new System.Drawing.Point(411, 238);
            this.btn导入.Name = "btn导入";
            this.btn导入.Size = new System.Drawing.Size(75, 23);
            this.btn导入.TabIndex = 6;
            this.btn导入.Text = "导入";
            this.btn导入.UseVisualStyleBackColor = true;
            this.btn导入.Click += new System.EventHandler(this.btn导入_Click);
            // 
            // btn浏览
            // 
            this.btn浏览.Location = new System.Drawing.Point(330, 238);
            this.btn浏览.Name = "btn浏览";
            this.btn浏览.Size = new System.Drawing.Size(75, 23);
            this.btn浏览.TabIndex = 5;
            this.btn浏览.Text = "浏览";
            this.btn浏览.UseVisualStyleBackColor = true;
            this.btn浏览.Click += new System.EventHandler(this.btn浏览_Click);
            // 
            // BOMList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 273);
            this.Controls.Add(this.btn导入);
            this.Controls.Add(this.btn浏览);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Name = "BOMList";
            this.Text = "BOMList";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn 存货代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 存货名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn导入;
        private System.Windows.Forms.Button btn浏览;
    }
}