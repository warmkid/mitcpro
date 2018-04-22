namespace mySystem.Process.CleanCut
{
    partial class CleanCut_RunRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanCut_RunRecord));
            this.Title = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb备注 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.btn打印 = new System.Windows.Forms.Button();
            this.btn确认 = new System.Windows.Forms.Button();
            this.tb生产指令编号 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn查询新建 = new System.Windows.Forms.Button();
            this.cb打印机 = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn提交数据审核 = new System.Windows.Forms.Button();
            this.btn数据审核 = new System.Windows.Forms.Button();
            this.btn审核 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(438, 19);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(169, 19);
            this.Title.TabIndex = 70;
            this.Title.Text = "清洁分切运行记录";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(29, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(923, 289);
            this.dataGridView1.TabIndex = 93;
            // 
            // tb备注
            // 
            this.tb备注.Font = new System.Drawing.Font("宋体", 12F);
            this.tb备注.Location = new System.Drawing.Point(81, 430);
            this.tb备注.Multiline = true;
            this.tb备注.Name = "tb备注";
            this.tb备注.Size = new System.Drawing.Size(871, 49);
            this.tb备注.TabIndex = 96;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(26, 430);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 97;
            this.label7.Text = "备注：";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.AddLineBtn.Location = new System.Drawing.Point(561, 394);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(80, 30);
            this.AddLineBtn.TabIndex = 94;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Font = new System.Drawing.Font("宋体", 12F);
            this.DelLineBtn.Location = new System.Drawing.Point(647, 394);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(80, 30);
            this.DelLineBtn.TabIndex = 95;
            this.DelLineBtn.Text = "删除记录";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // btn查看日志
            // 
            this.btn查看日志.Font = new System.Drawing.Font("宋体", 12F);
            this.btn查看日志.Location = new System.Drawing.Point(872, 501);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(80, 30);
            this.btn查看日志.TabIndex = 110;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            this.btn查看日志.Click += new System.EventHandler(this.bt日志_Click);
            // 
            // btn提交审核
            // 
            this.btn提交审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn提交审核.Location = new System.Drawing.Point(780, 501);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(80, 30);
            this.btn提交审核.TabIndex = 109;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            // 
            // btn打印
            // 
            this.btn打印.Font = new System.Drawing.Font("宋体", 12F);
            this.btn打印.Location = new System.Drawing.Point(484, 501);
            this.btn打印.Name = "btn打印";
            this.btn打印.Size = new System.Drawing.Size(80, 30);
            this.btn打印.TabIndex = 108;
            this.btn打印.Text = "打印";
            this.btn打印.UseVisualStyleBackColor = true;
            this.btn打印.Click += new System.EventHandler(this.btn打印_Click);
            // 
            // btn确认
            // 
            this.btn确认.Font = new System.Drawing.Font("宋体", 12F);
            this.btn确认.Location = new System.Drawing.Point(688, 501);
            this.btn确认.Name = "btn确认";
            this.btn确认.Size = new System.Drawing.Size(80, 30);
            this.btn确认.TabIndex = 106;
            this.btn确认.Text = "保存";
            this.btn确认.UseVisualStyleBackColor = true;
            this.btn确认.Click += new System.EventHandler(this.bt确认_Click);
            // 
            // tb生产指令编号
            // 
            this.tb生产指令编号.Font = new System.Drawing.Font("宋体", 12F);
            this.tb生产指令编号.Location = new System.Drawing.Point(142, 63);
            this.tb生产指令编号.Name = "tb生产指令编号";
            this.tb生产指令编号.Size = new System.Drawing.Size(250, 26);
            this.tb生产指令编号.TabIndex = 114;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.Location = new System.Drawing.Point(26, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 113;
            this.label8.Text = "生产指令编码：";
            // 
            // btn查询新建
            // 
            this.btn查询新建.Font = new System.Drawing.Font("宋体", 12F);
            this.btn查询新建.Location = new System.Drawing.Point(859, 56);
            this.btn查询新建.Name = "btn查询新建";
            this.btn查询新建.Size = new System.Drawing.Size(93, 30);
            this.btn查询新建.TabIndex = 115;
            this.btn查询新建.Text = "查询/新建";
            this.btn查询新建.UseVisualStyleBackColor = true;
            this.btn查询新建.Click += new System.EventHandler(this.bt查询新建_Click);
            // 
            // cb打印机
            // 
            this.cb打印机.Font = new System.Drawing.Font("宋体", 12F);
            this.cb打印机.FormattingEnabled = true;
            this.cb打印机.Location = new System.Drawing.Point(227, 505);
            this.cb打印机.Name = "cb打印机";
            this.cb打印机.Size = new System.Drawing.Size(251, 24);
            this.cb打印机.TabIndex = 178;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("宋体", 12F);
            this.label40.Location = new System.Drawing.Point(130, 508);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(104, 16);
            this.label40.TabIndex = 179;
            this.label40.Text = "选择打印机：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point(751, 19);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(93, 16);
            this.label27.TabIndex = 177;
            this.label27.Text = "登录角色：";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label角色.Location = new System.Drawing.Point(856, 19);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(42, 16);
            this.label角色.TabIndex = 176;
            this.label角色.Text = "角色";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(29, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 175;
            this.pictureBox1.TabStop = false;
            // 
            // btn提交数据审核
            // 
            this.btn提交数据审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn提交数据审核.Location = new System.Drawing.Point(836, 394);
            this.btn提交数据审核.Name = "btn提交数据审核";
            this.btn提交数据审核.Size = new System.Drawing.Size(116, 30);
            this.btn提交数据审核.TabIndex = 328;
            this.btn提交数据审核.Text = "提交数据审核";
            this.btn提交数据审核.UseVisualStyleBackColor = true;
            this.btn提交数据审核.Click += new System.EventHandler(this.btn提交数据审核_Click);
            // 
            // btn数据审核
            // 
            this.btn数据审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn数据审核.Location = new System.Drawing.Point(733, 394);
            this.btn数据审核.Name = "btn数据审核";
            this.btn数据审核.Size = new System.Drawing.Size(97, 30);
            this.btn数据审核.TabIndex = 327;
            this.btn数据审核.Text = "数据审核";
            this.btn数据审核.UseVisualStyleBackColor = true;
            this.btn数据审核.Click += new System.EventHandler(this.btn数据审核_Click);
            // 
            // btn审核
            // 
            this.btn审核.Font = new System.Drawing.Font("宋体", 12F);
            this.btn审核.Location = new System.Drawing.Point(29, 501);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(80, 30);
            this.btn审核.TabIndex = 329;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // CleanCut_RunRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 555);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn提交数据审核);
            this.Controls.Add(this.btn数据审核);
            this.Controls.Add(this.cb打印机);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn查询新建);
            this.Controls.Add(this.tb生产指令编号);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.btn打印);
            this.Controls.Add(this.btn确认);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tb备注);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.Title);
            this.Name = "CleanCut_RunRecord";
            this.Text = "清洁分切运行记录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CleanCut_RunRecord_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb备注;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Button btn打印;
        private System.Windows.Forms.Button btn确认;
        private System.Windows.Forms.TextBox tb生产指令编号;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn查询新建;
        private System.Windows.Forms.ComboBox cb打印机;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn提交数据审核;
        private System.Windows.Forms.Button btn数据审核;
        private System.Windows.Forms.Button btn审核;
    }
}