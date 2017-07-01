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
            this.订单信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.myDataSet = new mySystem.myDataSet();
            this.库存信息BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.MainProduceBtn = new System.Windows.Forms.Button();
            this.MainSettingBtn = new System.Windows.Forms.Button();
            this.MainQueryBtn = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.userLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.订单信息BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 订单信息BindingSource
            // 
            this.订单信息BindingSource.DataMember = "订单信息";
            this.订单信息BindingSource.DataSource = this.myDataSet;
            // 
            // myDataSet
            // 
            this.myDataSet.DataSetName = "myDataSet";
            this.myDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 库存信息BindingSource
            // 
            this.库存信息BindingSource.DataMember = "库存信息";
            this.库存信息BindingSource.DataSource = this.myDataSet;
            // 
            // MainProduceBtn
            // 
            this.MainProduceBtn.BackColor = System.Drawing.SystemColors.Control;
            this.MainProduceBtn.FlatAppearance.BorderSize = 0;
            this.MainProduceBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainProduceBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainProduceBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainProduceBtn.Image")));
            this.MainProduceBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainProduceBtn.Location = new System.Drawing.Point(3, 9);
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
            this.MainSettingBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainSettingBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainSettingBtn.Image")));
            this.MainSettingBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainSettingBtn.Location = new System.Drawing.Point(78, 9);
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
            this.MainQueryBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainQueryBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainQueryBtn.Image")));
            this.MainQueryBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainQueryBtn.Location = new System.Drawing.Point(153, 9);
            this.MainQueryBtn.Name = "MainQueryBtn";
            this.MainQueryBtn.Size = new System.Drawing.Size(114, 63);
            this.MainQueryBtn.TabIndex = 3;
            this.MainQueryBtn.Text = "台帐/查询";
            this.MainQueryBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.MainQueryBtn.UseVisualStyleBackColor = true;
            this.MainQueryBtn.Click += new System.EventHandler(this.MainQueryBtn_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainPanel.Location = new System.Drawing.Point(0, 69);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1360, 620);
            this.MainPanel.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExitBtn);
            this.groupBox1.Controls.Add(this.userLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MainQueryBtn);
            this.groupBox1.Controls.Add(this.MainProduceBtn);
            this.groupBox1.Controls.Add(this.MainSettingBtn);
            this.groupBox1.Location = new System.Drawing.Point(4, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1360, 74);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // ExitBtn
            // 
            this.ExitBtn.Font = new System.Drawing.Font("SimSun", 15F);
            this.ExitBtn.Location = new System.Drawing.Point(1233, 31);
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
            this.userLabel.Font = new System.Drawing.Font("SimSun", 15F);
            this.userLabel.Location = new System.Drawing.Point(1151, 37);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(0, 20);
            this.userLabel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 15F);
            this.label1.Location = new System.Drawing.Point(1068, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "操作员：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1358, 699);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MainPanel);
            this.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎使用颇尔奥星管理系统";
            ((System.ComponentModel.ISupportInitialize)(this.订单信息BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.库存信息BindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource 订单信息BindingSource;
        private myDataSet myDataSet;
        private System.Windows.Forms.BindingSource 库存信息BindingSource;
        private System.Windows.Forms.Button MainProduceBtn;
        private System.Windows.Forms.Button MainSettingBtn;
        private System.Windows.Forms.Button MainQueryBtn;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label label1;


    }
}