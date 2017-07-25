namespace mySystem.Process.Bag
{
    partial class CSBag_CheckBeforePower
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PSLabel = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp操作日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb操作员 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb操作员备注 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "1";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column1.HeaderText = "国家";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "2";
            this.Column2.HeaderText = "城市";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "3";
            this.Column3.HeaderText = "男";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "4";
            this.Column4.HeaderText = "女";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(474, 20);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(219, 20);
            this.Title.TabIndex = 26;
            this.Title.Text = "制袋机组开机前确认表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 96);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1113, 349);
            this.dataGridView1.TabIndex = 27;
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("SimSun", 12F);
            this.PSLabel.Location = new System.Drawing.Point(10, 449);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(498, 15);
            this.PSLabel.TabIndex = 28;
            this.PSLabel.Text = "注：\t正常或符合打“√”，不正常或不符合打“×”。";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(960, 61);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(145, 26);
            this.dtp审核日期.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F);
            this.label4.Location = new System.Drawing.Point(879, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 37;
            this.label4.Text = "审核日期：";
            // 
            // tb审核员
            // 
            this.tb审核员.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb审核员.Location = new System.Drawing.Point(770, 60);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 26);
            this.tb审核员.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(702, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "审核员:";
            // 
            // dtp操作日期
            // 
            this.dtp操作日期.Font = new System.Drawing.Font("SimSun", 12F);
            this.dtp操作日期.Location = new System.Drawing.Point(513, 60);
            this.dtp操作日期.Name = "dtp操作日期";
            this.dtp操作日期.Size = new System.Drawing.Size(152, 26);
            this.dtp操作日期.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F);
            this.label3.Location = new System.Drawing.Point(432, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "操作日期：";
            // 
            // tb操作员
            // 
            this.tb操作员.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb操作员.Location = new System.Drawing.Point(81, 60);
            this.tb操作员.Name = "tb操作员";
            this.tb操作员.Size = new System.Drawing.Size(100, 26);
            this.tb操作员.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F);
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "操作员：";
            // 
            // tb操作员备注
            // 
            this.tb操作员备注.Font = new System.Drawing.Font("SimSun", 12F);
            this.tb操作员备注.Location = new System.Drawing.Point(309, 59);
            this.tb操作员备注.Name = "tb操作员备注";
            this.tb操作员备注.Size = new System.Drawing.Size(100, 26);
            this.tb操作员备注.TabIndex = 109;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F);
            this.label5.Location = new System.Drawing.Point(216, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 110;
            this.label5.Text = "操作员备注：";
            // 
            // btn查看日志
            // 
            this.btn查看日志.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn查看日志.Location = new System.Drawing.Point(1046, 474);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(80, 30);
            this.btn查看日志.TabIndex = 115;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn提交审核.Location = new System.Drawing.Point(957, 474);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(80, 30);
            this.btn提交审核.TabIndex = 114;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn打印.Location = new System.Drawing.Point(105, 474);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(80, 30);
            this.btn打印.TabIndex = 113;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn审核.Location = new System.Drawing.Point(13, 474);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(80, 30);
            this.btn审核.TabIndex = 112;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("SimSun", 12F);
            this.btn确认.Location = new System.Drawing.Point(868, 474);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(80, 30);
            this.btn确认.TabIndex = 111;
            this.btn确认.Text = "确认";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.btn确认_Click);
            // 
            // CSBag_CheckBeforePower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 515);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.tb操作员备注);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtp审核日期);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp操作日期);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb操作员);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Title);
            this.Name = "CSBag_CheckBeforePower";
            this.Text = "制袋机组开机前确认表";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp操作日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb操作员;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb操作员备注;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn确认;

    }
}