namespace mySystem.Process.灭菌
{
    partial class 辐照灭菌台帐
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.b打印 = new System.Windows.Forms.Button();
            this.bt添加 = new System.Windows.Forms.Button();
            this.bt保存 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(296, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "辐照灭菌台帐";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(26, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(712, 253);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // b打印
            // 
            this.b打印.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.b打印.Location = new System.Drawing.Point(663, 322);
            this.b打印.Name = "b打印";
            this.b打印.Size = new System.Drawing.Size(75, 41);
            this.b打印.TabIndex = 2;
            this.b打印.Text = "打印";
            this.b打印.UseVisualStyleBackColor = true;
            // 
            // bt添加
            // 
            this.bt添加.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt添加.Location = new System.Drawing.Point(422, 326);
            this.bt添加.Name = "bt添加";
            this.bt添加.Size = new System.Drawing.Size(75, 37);
            this.bt添加.TabIndex = 3;
            this.bt添加.Text = "添加";
            this.bt添加.UseVisualStyleBackColor = true;
            // 
            // bt保存
            // 
            this.bt保存.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt保存.Location = new System.Drawing.Point(534, 326);
            this.bt保存.Name = "bt保存";
            this.bt保存.Size = new System.Drawing.Size(75, 37);
            this.bt保存.TabIndex = 4;
            this.bt保存.Text = "保存";
            this.bt保存.UseVisualStyleBackColor = true;
            // 
            // 辐照灭菌台帐
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 403);
            this.Controls.Add(this.bt保存);
            this.Controls.Add(this.bt添加);
            this.Controls.Add(this.b打印);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "辐照灭菌台帐";
            this.Text = "辐照灭菌台帐";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button b打印;
        private System.Windows.Forms.Button bt添加;
        private System.Windows.Forms.Button bt保存;
    }
}