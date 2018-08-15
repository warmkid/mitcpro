namespace mySystem
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainProduceBtn = new System.Windows.Forms.Button();
            this.MainSettingBtn = new System.Windows.Forms.Button();
            this.MainQueryBtn = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn导入 = new System.Windows.Forms.Button();
            this.btn浏览 = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.userLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainProduceBtn
            // 
            this.MainProduceBtn.BackColor = System.Drawing.SystemColors.Control;
            this.MainProduceBtn.FlatAppearance.BorderSize = 0;
            this.MainProduceBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainProduceBtn.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainProduceBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainProduceBtn.Image")));
            this.MainProduceBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainProduceBtn.Location = new System.Drawing.Point(132, 9);
            this.MainProduceBtn.Name = "MainProduceBtn";
            this.MainProduceBtn.Size = new System.Drawing.Size(75, 63);
            this.MainProduceBtn.TabIndex = 0;
            this.MainProduceBtn.Text = "工序";
            this.MainProduceBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.MainProduceBtn.UseVisualStyleBackColor = false;
            this.MainProduceBtn.Click += new System.EventHandler(this.MainProduceBtn_Click);
            // 
            // MainSettingBtn
            // 
            this.MainSettingBtn.BackColor = System.Drawing.SystemColors.Control;
            this.MainSettingBtn.FlatAppearance.BorderSize = 0;
            this.MainSettingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainSettingBtn.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainSettingBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainSettingBtn.Image")));
            this.MainSettingBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainSettingBtn.Location = new System.Drawing.Point(207, 9);
            this.MainSettingBtn.Name = "MainSettingBtn";
            this.MainSettingBtn.Size = new System.Drawing.Size(75, 63);
            this.MainSettingBtn.TabIndex = 2;
            this.MainSettingBtn.Text = "设置";
            this.MainSettingBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.MainSettingBtn.UseVisualStyleBackColor = false;
            this.MainSettingBtn.Click += new System.EventHandler(this.MainSettingBtn_Click);
            // 
            // MainQueryBtn
            // 
            this.MainQueryBtn.FlatAppearance.BorderSize = 0;
            this.MainQueryBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainQueryBtn.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainQueryBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainQueryBtn.Image")));
            this.MainQueryBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainQueryBtn.Location = new System.Drawing.Point(282, 9);
            this.MainQueryBtn.Name = "MainQueryBtn";
            this.MainQueryBtn.Size = new System.Drawing.Size(114, 63);
            this.MainQueryBtn.TabIndex = 3;
            this.MainQueryBtn.Text = "查询/审核";
            this.MainQueryBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.MainQueryBtn.UseVisualStyleBackColor = true;
            this.MainQueryBtn.Click += new System.EventHandler(this.MainQueryBtn_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainPanel.Location = new System.Drawing.Point(9, 76);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1249, 592);
            this.MainPanel.TabIndex = 4;
            this.MainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainPanel_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.MainQueryBtn);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.MainSettingBtn);
            this.groupBox1.Controls.Add(this.btn导入);
            this.groupBox1.Controls.Add(this.MainProduceBtn);
            this.groupBox1.Controls.Add(this.btn浏览);
            this.groupBox1.Controls.Add(this.ExitBtn);
            this.groupBox1.Controls.Add(this.userLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1251, 74);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(128, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(4, 70);
            this.label2.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(4, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 63);
            this.button1.TabIndex = 11;
            this.button1.Text = "我的任务";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnMyTask_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(479, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(31, 23);
            this.textBox1.TabIndex = 9;
            this.textBox1.Visible = false;
            // 
            // btn导入
            // 
            this.btn导入.Location = new System.Drawing.Point(540, 31);
            this.btn导入.Name = "btn导入";
            this.btn导入.Size = new System.Drawing.Size(103, 23);
            this.btn导入.TabIndex = 8;
            this.btn导入.Text = "导入存货档案";
            this.btn导入.UseVisualStyleBackColor = true;
            this.btn导入.Visible = false;
            this.btn导入.Click += new System.EventHandler(this.btn导入_Click);
            // 
            // btn浏览
            // 
            this.btn浏览.Location = new System.Drawing.Point(516, 32);
            this.btn浏览.Name = "btn浏览";
            this.btn浏览.Size = new System.Drawing.Size(75, 23);
            this.btn浏览.TabIndex = 7;
            this.btn浏览.Text = "浏览";
            this.btn浏览.UseVisualStyleBackColor = true;
            this.btn浏览.Visible = false;
            this.btn浏览.Click += new System.EventHandler(this.btn浏览_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Font = new System.Drawing.Font("宋体", 15F);
            this.ExitBtn.Location = new System.Drawing.Point(1088, 26);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(99, 30);
            this.ExitBtn.TabIndex = 6;
            this.ExitBtn.Text = "退出登录";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("宋体", 15F);
            this.userLabel.Location = new System.Drawing.Point(978, 31);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(0, 20);
            this.userLabel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F);
            this.label1.Location = new System.Drawing.Point(895, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "登录人：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1276, 670);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎使用颇尔奥星管理系统    v0.9.17  201808151500";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MainProduceBtn;
        private System.Windows.Forms.Button MainSettingBtn;
        private System.Windows.Forms.Button MainQueryBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn导入;
        private System.Windows.Forms.Button btn浏览;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;


    }
}