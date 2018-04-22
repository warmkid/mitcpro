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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSBag_CheckBeforePower));
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.tb操作员备注 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp审核日期 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtp操作日期 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb操作员 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.tb生产指令编号 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtp生产日期 = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.cb夜班 = new System.Windows.Forms.CheckBox();
            this.cb白班 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
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
            this.Title.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
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
            this.dataGridView1.Location = new System.Drawing.Point(13, 106);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1113, 339);
            this.dataGridView1.TabIndex = 27;
            // 
            // btn查看日志
            // 
            this.btn查看日志.Font = new System.Drawing.Font("宋体", 12F);
            this.btn查看日志.Location = new System.Drawing.Point(1038, 580);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(80, 30);
            this.btn查看日志.TabIndex = 115;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.btn查看日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn提交审核.Location = new System.Drawing.Point(949, 580);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(80, 30);
            this.btn提交审核.TabIndex = 114;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("宋体", 12F);
            this.btn打印.Location = new System.Drawing.Point(97, 580);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(80, 30);
            this.btn打印.TabIndex = 113;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn审核.Location = new System.Drawing.Point(5, 580);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(80, 30);
            this.btn审核.TabIndex = 112;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("宋体", 12F);
            this.btn确认.Location = new System.Drawing.Point(860, 580);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(80, 30);
            this.btn确认.TabIndex = 111;
            this.btn确认.Text = "保存";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.btn确认_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 155;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(870, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 16);
            this.label6.TabIndex = 154;
            this.label6.Text = "登录角色：";
            // 
            // cb打印机
            // 
            this.cb打印机.Font = new System.Drawing.Font("宋体", 12F);
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(304, 582);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(279, 24);
            this.cb打印机.TabIndex = 152;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("宋体", 12F);
            this.label40.Location = new System.Drawing.Point(208, 587);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(104, 16);
            this.label40.TabIndex = 153;
            this.label40.Text = "选择打印机：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(975, 23);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 151;
            this.label角色.Text = "角色";
            // 
            // tb操作员备注
            // 
            this.tb操作员备注.Font = new System.Drawing.Font("宋体", 12F);
            this.tb操作员备注.Location = new System.Drawing.Point(309, 524);
            this.tb操作员备注.Name = "tb操作员备注";
            this.tb操作员备注.Size = new System.Drawing.Size(100, 26);
            this.tb操作员备注.TabIndex = 164;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F);
            this.label5.Location = new System.Drawing.Point(216, 529);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 165;
            this.label5.Text = "操作员备注：";
            // 
            // dtp审核日期
            // 
            this.dtp审核日期.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp审核日期.Location = new System.Drawing.Point(978, 526);
            this.dtp审核日期.Name = "dtp审核日期";
            this.dtp审核日期.Size = new System.Drawing.Size(145, 26);
            this.dtp审核日期.TabIndex = 163;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(897, 530);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 162;
            this.label4.Text = "审核日期：";
            // 
            // dtp操作日期
            // 
            this.dtp操作日期.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp操作日期.Location = new System.Drawing.Point(520, 525);
            this.dtp操作日期.Name = "dtp操作日期";
            this.dtp操作日期.Size = new System.Drawing.Size(152, 26);
            this.dtp操作日期.TabIndex = 161;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(439, 529);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 160;
            this.label3.Text = "操作日期：";
            // 
            // tb审核员
            // 
            this.tb审核员.Font = new System.Drawing.Font("宋体", 12F);
            this.tb审核员.Location = new System.Drawing.Point(788, 525);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 26);
            this.tb审核员.TabIndex = 159;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(720, 533);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 158;
            this.label2.Text = "审核员:";
            // 
            // tb操作员
            // 
            this.tb操作员.Font = new System.Drawing.Font("宋体", 12F);
            this.tb操作员.Location = new System.Drawing.Point(81, 525);
            this.tb操作员.Name = "tb操作员";
            this.tb操作员.Size = new System.Drawing.Size(100, 26);
            this.tb操作员.TabIndex = 157;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(13, 529);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 156;
            this.label1.Text = "操作员：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(13, 464);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 166;
            this.label7.Text = "备注：";
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("宋体", 12F);
            this.tb备注.Location = new System.Drawing.Point(75, 451);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(1051, 54);
            this.tb备注.TabIndex = 167;
            // 
            // tb生产指令编号
            // 
            this.tb生产指令编号.Font = new System.Drawing.Font("宋体", 12F);
            this.tb生产指令编号.Location = new System.Drawing.Point(123, 74);
            this.tb生产指令编号.Name = "tb生产指令编号";
            this.tb生产指令编号.Size = new System.Drawing.Size(236, 26);
            this.tb生产指令编号.TabIndex = 169;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.Location = new System.Drawing.Point(13, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 16);
            this.label8.TabIndex = 168;
            this.label8.Text = "生产指令编号";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(429, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 16);
            this.label9.TabIndex = 170;
            this.label9.Text = "生产日期";
            // 
            // dtp生产日期
            // 
            this.dtp生产日期.Font = new System.Drawing.Font("宋体", 12F);
            this.dtp生产日期.Location = new System.Drawing.Point(507, 70);
            this.dtp生产日期.Name = "dtp生产日期";
            this.dtp生产日期.Size = new System.Drawing.Size(137, 26);
            this.dtp生产日期.TabIndex = 171;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F);
            this.label10.Location = new System.Drawing.Point(816, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 172;
            this.label10.Text = "班次";
            // 
            // cb夜班
            // 
            this.cb夜班.AutoSize = true;
            this.cb夜班.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb夜班.Location = new System.Drawing.Point(927, 76);
            this.cb夜班.Name = "cb夜班";
            this.cb夜班.Size = new System.Drawing.Size(59, 20);
            this.cb夜班.TabIndex = 174;
            this.cb夜班.Text = "夜班";
            this.cb夜班.UseVisualStyleBackColor = true;
            // 
            // cb白班
            // 
            this.cb白班.AutoSize = true;
            this.cb白班.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb白班.Location = new System.Drawing.Point(862, 76);
            this.cb白班.Name = "cb白班";
            this.cb白班.Size = new System.Drawing.Size(59, 20);
            this.cb白班.TabIndex = 173;
            this.cb白班.Text = "白班";
            this.cb白班.UseVisualStyleBackColor = true;
            // 
            // CSBag_CheckBeforePower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 622);
            this.Controls.Add(this.cb夜班);
            this.Controls.Add(this.cb白班);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dtp生产日期);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb生产指令编号);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label7);
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
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Title);
            this.Name = "CSBag_CheckBeforePower";
            this.Text = "制袋机组开机前确认表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CSBag_CheckBeforePower_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn确认;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.TextBox tb操作员备注;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp审核日期;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtp操作日期;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb操作员;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.TextBox tb生产指令编号;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtp生产日期;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cb夜班;
        private System.Windows.Forms.CheckBox cb白班;

    }
}