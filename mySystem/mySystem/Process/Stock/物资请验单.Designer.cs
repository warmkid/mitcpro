namespace mySystem.Process.Stock
{
    partial class 物资请验单
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
            this.components = new System.ComponentModel.Container();
            this.btn审核 = new System.Windows.Forms.Button();
            this.btn查看日志 = new System.Windows.Forms.Button();
            this.btn提交审核 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.tb审核员 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtp审核时间 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.tb请验人 = new System.Windows.Forms.TextBox();
            this.dtp请验时间 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn保存 = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.tb供应商代码 = new System.Windows.Forms.TextBox();
            this.tb供应商名称 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label角色 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.物资验收记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.物资请验单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检验记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取样记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn审核
            // 
            this.btn审核.Location = new System.Drawing.Point(28, 369);
            this.btn审核.Name = "btn审核";
            this.btn审核.Size = new System.Drawing.Size(75, 23);
            this.btn审核.TabIndex = 62;
            this.btn审核.Text = "审核";
            this.btn审核.UseVisualStyleBackColor = true;
            this.btn审核.Click += new System.EventHandler(this.btn审核_Click);
            // 
            // btn查看日志
            // 
            this.btn查看日志.Location = new System.Drawing.Point(616, 369);
            this.btn查看日志.Name = "btn查看日志";
            this.btn查看日志.Size = new System.Drawing.Size(75, 23);
            this.btn查看日志.TabIndex = 61;
            this.btn查看日志.Text = "查看日志";
            this.btn查看日志.UseVisualStyleBackColor = true;
            // 
            // btn提交审核
            // 
            this.btn提交审核.Location = new System.Drawing.Point(528, 369);
            this.btn提交审核.Name = "btn提交审核";
            this.btn提交审核.Size = new System.Drawing.Size(75, 23);
            this.btn提交审核.TabIndex = 60;
            this.btn提交审核.Text = "提交审核";
            this.btn提交审核.UseVisualStyleBackColor = true;
            this.btn提交审核.Click += new System.EventHandler(this.btn提交审核_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(257, 375);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 59;
            this.label10.Text = "审核时间";
            // 
            // tb审核员
            // 
            this.tb审核员.Location = new System.Drawing.Point(151, 370);
            this.tb审核员.Name = "tb审核员";
            this.tb审核员.Size = new System.Drawing.Size(100, 21);
            this.tb审核员.TabIndex = 58;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(104, 375);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 57;
            this.label11.Text = "审核员";
            // 
            // dtp审核时间
            // 
            this.dtp审核时间.Location = new System.Drawing.Point(316, 369);
            this.dtp审核时间.Name = "dtp审核时间";
            this.dtp审核时间.Size = new System.Drawing.Size(109, 21);
            this.dtp审核时间.TabIndex = 56;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(29, 129);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(662, 221);
            this.dataGridView1.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(544, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "请验人";
            // 
            // tb请验人
            // 
            this.tb请验人.Location = new System.Drawing.Point(591, 95);
            this.tb请验人.Name = "tb请验人";
            this.tb请验人.Size = new System.Drawing.Size(100, 21);
            this.tb请验人.TabIndex = 44;
            // 
            // dtp请验时间
            // 
            this.dtp请验时间.Location = new System.Drawing.Point(87, 95);
            this.dtp请验时间.Name = "dtp请验时间";
            this.dtp请验时间.Size = new System.Drawing.Size(200, 21);
            this.dtp请验时间.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 42;
            this.label3.Text = "请验时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 40;
            this.label2.Text = "供应商代码";
            // 
            // btn保存
            // 
            this.btn保存.Location = new System.Drawing.Point(444, 369);
            this.btn保存.Name = "btn保存";
            this.btn保存.Size = new System.Drawing.Size(75, 23);
            this.btn保存.TabIndex = 39;
            this.btn保存.Text = "保存";
            this.btn保存.UseVisualStyleBackColor = true;
            this.btn保存.Click += new System.EventHandler(this.btn保存_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(294, 16);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(109, 19);
            this.Title.TabIndex = 35;
            this.Title.Text = "物资请验单";
            // 
            // tb供应商代码
            // 
            this.tb供应商代码.Location = new System.Drawing.Point(112, 66);
            this.tb供应商代码.Name = "tb供应商代码";
            this.tb供应商代码.Size = new System.Drawing.Size(100, 21);
            this.tb供应商代码.TabIndex = 66;
            // 
            // tb供应商名称
            // 
            this.tb供应商名称.Location = new System.Drawing.Point(591, 66);
            this.tb供应商名称.Name = "tb供应商名称";
            this.tb供应商名称.Size = new System.Drawing.Size(100, 21);
            this.tb供应商名称.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(506, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "供应商名称";
            // 
            // label角色
            // 
            this.label角色.AutoSize = true;
            this.label角色.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold);
            this.label角色.Location = new System.Drawing.Point(494, 16);
            this.label角色.Name = "label角色";
            this.label角色.Size = new System.Drawing.Size(49, 19);
            this.label角色.TabIndex = 69;
            this.label角色.Text = "角色";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.物资验收记录ToolStripMenuItem,
            this.物资请验单ToolStripMenuItem,
            this.检验记录ToolStripMenuItem,
            this.取样记录ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 92);
            // 
            // 物资验收记录ToolStripMenuItem
            // 
            this.物资验收记录ToolStripMenuItem.Name = "物资验收记录ToolStripMenuItem";
            this.物资验收记录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.物资验收记录ToolStripMenuItem.Text = "物资验收记录";
            // 
            // 物资请验单ToolStripMenuItem
            // 
            this.物资请验单ToolStripMenuItem.Name = "物资请验单ToolStripMenuItem";
            this.物资请验单ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.物资请验单ToolStripMenuItem.Text = "物资请验单";
            // 
            // 检验记录ToolStripMenuItem
            // 
            this.检验记录ToolStripMenuItem.Name = "检验记录ToolStripMenuItem";
            this.检验记录ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.检验记录ToolStripMenuItem.Text = "检验记录";
            // 
            // 取样记录ToolStripMenuItem
            // 
            this.取样记录ToolStripMenuItem.Name = "取样记录ToolStripMenuItem";
            this.取样记录ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.取样记录ToolStripMenuItem.Text = "取样记录";
            // 
            // 物资请验单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 417);
            this.Controls.Add(this.label角色);
            this.Controls.Add(this.tb供应商名称);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb供应商代码);
            this.Controls.Add(this.btn审核);
            this.Controls.Add(this.btn查看日志);
            this.Controls.Add(this.btn提交审核);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb审核员);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dtp审核时间);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb请验人);
            this.Controls.Add(this.dtp请验时间);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn保存);
            this.Controls.Add(this.Title);
            this.Name = "物资请验单";
            this.Text = "物资请验单";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn审核;
        private System.Windows.Forms.Button btn查看日志;
        private System.Windows.Forms.Button btn提交审核;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb审核员;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtp审核时间;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb请验人;
        private System.Windows.Forms.DateTimePicker dtp请验时间;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn保存;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.TextBox tb供应商代码;
        private System.Windows.Forms.TextBox tb供应商名称;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label角色;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 物资验收记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 物资请验单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检验记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取样记录ToolStripMenuItem;
    }
}