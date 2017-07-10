namespace mySystem.Process.Extruction.D
{
    partial class NetExchange
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
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn保存 = new System.Windows.Forms.Button();
            this.btn插入 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(449, 43);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 19);
            this.label7.TabIndex = 52;
            this.label7.Text = "吹膜机更换过滤网记录";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(44, 117);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(993, 360);
            this.dataGridView1.TabIndex = 53;
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(804, 69);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 54;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // btn插入
            // 
            this.btn插入.Location = new System.Drawing.Point(908, 69);
            this.btn插入.Name = "btn插入";
            this.btn插入.Size = new System.Drawing.Size(75, 23);
            this.btn插入.TabIndex = 55;
            this.btn插入.Text = "插入";
            this.btn插入.UseVisualStyleBackColor = true;
            this.btn插入.Click += new System.EventHandler(this.btn插入_Click);
            // 
            // NetExchange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 741);
            this.Controls.Add(this.btn插入);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NetExchange";
            this.Text = "NetExchange";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn插入;
    }
}