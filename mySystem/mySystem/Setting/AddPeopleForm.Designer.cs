namespace mySystem.Setting
{
    partial class AddPeopleForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.tb岗位 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox部门 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.comboBox角色 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox班次 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PWtextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NametextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IDtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp班次开始时间 = new System.Windows.Forms.DateTimePicker();
            this.dtp班次结束时间 = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "班次开始时间：";
            // 
            // tb岗位
            // 
            this.tb岗位.Location = new System.Drawing.Point(352, 164);
            this.tb岗位.Name = "tb岗位";
            this.tb岗位.Size = new System.Drawing.Size(121, 26);
            this.tb岗位.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(290, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "岗位：";
            // 
            // comboBox部门
            // 
            this.comboBox部门.FormattingEnabled = true;
            this.comboBox部门.Items.AddRange(new object[] {
            "运营部",
            "生产部"});
            this.comboBox部门.Location = new System.Drawing.Point(352, 118);
            this.comboBox部门.Name = "comboBox部门";
            this.comboBox部门.Size = new System.Drawing.Size(121, 24);
            this.comboBox部门.TabIndex = 13;
            this.comboBox部门.SelectedIndexChanged += new System.EventHandler(this.comboBox部门_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(290, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "部门：";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(398, 219);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 30);
            this.CancelBtn.TabIndex = 11;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(308, 219);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 30);
            this.AddBtn.TabIndex = 10;
            this.AddBtn.Text = "添加";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // comboBox角色
            // 
            this.comboBox角色.FormattingEnabled = true;
            this.comboBox角色.Items.AddRange(new object[] {
            "操作员",
            "计划员",
            "管理员"});
            this.comboBox角色.Location = new System.Drawing.Point(352, 71);
            this.comboBox角色.Name = "comboBox角色";
            this.comboBox角色.Size = new System.Drawing.Size(121, 24);
            this.comboBox角色.TabIndex = 9;
            this.comboBox角色.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "角色：";
            // 
            // comboBox班次
            // 
            this.comboBox班次.FormattingEnabled = true;
            this.comboBox班次.Items.AddRange(new object[] {
            "白班",
            "夜班"});
            this.comboBox班次.Location = new System.Drawing.Point(352, 26);
            this.comboBox班次.Name = "comboBox班次";
            this.comboBox班次.Size = new System.Drawing.Size(121, 24);
            this.comboBox班次.TabIndex = 7;
            this.comboBox班次.SelectedIndexChanged += new System.EventHandler(this.comboBox班次_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(290, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "班次：";
            // 
            // PWtextBox
            // 
            this.PWtextBox.Location = new System.Drawing.Point(105, 115);
            this.PWtextBox.Name = "PWtextBox";
            this.PWtextBox.Size = new System.Drawing.Size(139, 26);
            this.PWtextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "登录密码：";
            // 
            // NametextBox
            // 
            this.NametextBox.Location = new System.Drawing.Point(105, 71);
            this.NametextBox.Name = "NametextBox";
            this.NametextBox.Size = new System.Drawing.Size(139, 26);
            this.NametextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "员工姓名：";
            // 
            // IDtextBox
            // 
            this.IDtextBox.Location = new System.Drawing.Point(105, 24);
            this.IDtextBox.Name = "IDtextBox";
            this.IDtextBox.Size = new System.Drawing.Size(139, 26);
            this.IDtextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "员工ID：";
            // 
            // dtp班次开始时间
            // 
            this.dtp班次开始时间.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtp班次开始时间.Location = new System.Drawing.Point(138, 158);
            this.dtp班次开始时间.Name = "dtp班次开始时间";
            this.dtp班次开始时间.Size = new System.Drawing.Size(106, 26);
            this.dtp班次开始时间.TabIndex = 17;
            this.dtp班次开始时间.Value = new System.DateTime(2017, 7, 14, 8, 0, 0, 0);
            // 
            // dtp班次结束时间
            // 
            this.dtp班次结束时间.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtp班次结束时间.Location = new System.Drawing.Point(138, 200);
            this.dtp班次结束时间.Name = "dtp班次结束时间";
            this.dtp班次结束时间.Size = new System.Drawing.Size(106, 26);
            this.dtp班次结束时间.TabIndex = 19;
            this.dtp班次结束时间.Value = new System.DateTime(2017, 7, 14, 17, 0, 0, 0);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 16);
            this.label9.TabIndex = 18;
            this.label9.Text = "班次结束时间：";
            // 
            // AddPeopleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 271);
            this.Controls.Add(this.dtp班次结束时间);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dtp班次开始时间);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb岗位);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox部门);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.comboBox角色);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox班次);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PWtextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NametextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IDtextBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddPeopleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加员工信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IDtextBox;
        private System.Windows.Forms.TextBox NametextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PWtextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox班次;
        private System.Windows.Forms.ComboBox comboBox角色;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ComboBox comboBox部门;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb岗位;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtp班次开始时间;
        private System.Windows.Forms.DateTimePicker dtp班次结束时间;
        private System.Windows.Forms.Label label9;
    }
}