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
            this.label角色 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn保存 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp审核时间 = new System.Windows.Forms.DateTimePicker();
            this.btn取样证 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.label角色.Location = new System.Drawing.Point(315, 20);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(49, 19);
            this.label角色.TabIndex = 46;
            this.label角色.Text = "角色";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(155, 20);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(89, 19);
            this.Title.TabIndex = 45;
            this.Title.Text = "取样记录";
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn审核.Location = new System.Drawing.Point(3, 469);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 44;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn提交审核.Location = new System.Drawing.Point(362, 469);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(89, 23);
            this.btn提交审核.TabIndex = 43;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn保存
            // 
            this.btn保存.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn保存.Location = new System.Drawing.Point(252, 469);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 42;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(217, 435);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 50;
            this.label10.Text = "审核时间";
            // 
            // tb审核员
            // 
            this.tb审核员.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb审核员.Location = new System.Drawing.Point(74, 430);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 26);
            this.tb审核员.TabIndex = 49;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(16, 435);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 16);
            this.label11.TabIndex = 48;
            this.label11.Text = "审核员";
            // 
            // dtp审核时间
            // 
            this.dtp审核时间.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtp审核时间.Location = new System.Drawing.Point(295, 430);
            this.dtp审核时间.Name = "dtp审核时间";
            this.dtp审核时间.Size = new System.Drawing.Size(156, 26);
            this.dtp审核时间.TabIndex = 47;
            // 
            // btn取样证
            // 
            this.btn取样证.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn取样证.Location = new System.Drawing.Point(119, 469);
            this.btn取样证.Name = "btn取样证";
            this.btn取样证.Size = new System.Drawing.Size(75, 23);
            this.btn取样证.TabIndex = 51;
            this.btn取样证.Text = "取样证";
            this.btn取样证.UseVisualStyleBackColor = true;
            this.btn取样证.Click += new System.EventHandler(this.btn取样证_Click);
            // 
            // 取样记录
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 528);
            this.Controls.Add(this.btn取样证);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dtp审核时间);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.dataGridView1);
            this.Name = "取样记录";
            this.Text = "取样记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp审核时间;
        private System.Windows.Forms.Button btn取样证;
    }
}