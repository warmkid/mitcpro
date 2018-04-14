namespace mySystem.Setting
{
    partial class SetPeopleForm
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
            this.SetPeoplePanelTop = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pw3TextBox = new System.Windows.Forms.TextBox();
            this.pw1TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pw2TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SetPeoplePanelBottom = new System.Windows.Forms.Panel();
            this.SaveEditBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.SetPeoplePanelTop.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SetPeoplePanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.SuspendLayout();
            // 
            // SetPeoplePanelTop
            // 
            this.SetPeoplePanelTop.Controls.Add(this.CancelBtn);
            this.SetPeoplePanelTop.Controls.Add(this.SaveBtn);
            this.SetPeoplePanelTop.Controls.Add(this.label6);
            this.SetPeoplePanelTop.Controls.Add(this.groupBox2);
            this.SetPeoplePanelTop.Controls.Add(this.groupBox1);
            this.SetPeoplePanelTop.Location = new System.Drawing.Point(13, 13);
            this.SetPeoplePanelTop.Name = "SetPeoplePanelTop";
            this.SetPeoplePanelTop.Size = new System.Drawing.Size(1145, 184);
            this.SetPeoplePanelTop.TabIndex = 0;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(954, 139);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(96, 30);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(836, 139);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(96, 30);
            this.SaveBtn.TabIndex = 7;
            this.SaveBtn.Text = "确认修改";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(33, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "修改姓名及密码";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pw3TextBox);
            this.groupBox2.Controls.Add(this.pw1TextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.pw2TextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(352, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 136);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // pw3TextBox
            // 
            this.pw3TextBox.Location = new System.Drawing.Point(148, 98);
            this.pw3TextBox.Name = "pw3TextBox";
            this.pw3TextBox.PasswordChar = '*';
            this.pw3TextBox.Size = new System.Drawing.Size(189, 26);
            this.pw3TextBox.TabIndex = 6;
            // 
            // pw1TextBox
            // 
            this.pw1TextBox.Location = new System.Drawing.Point(148, 27);
            this.pw1TextBox.Name = "pw1TextBox";
            this.pw1TextBox.PasswordChar = '*';
            this.pw1TextBox.Size = new System.Drawing.Size(189, 26);
            this.pw1TextBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "再次输入新密码：";
            // 
            // pw2TextBox
            // 
            this.pw2TextBox.Location = new System.Drawing.Point(148, 63);
            this.pw2TextBox.Name = "pw2TextBox";
            this.pw2TextBox.PasswordChar = '*';
            this.pw2TextBox.Size = new System.Drawing.Size(189, 26);
            this.pw2TextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "输入原密码：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "输入新密码：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.idLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(37, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 136);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(109, 63);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(100, 26);
            this.nameTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "员工ID：";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(107, 30);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(32, 16);
            this.idLabel.TabIndex = 2;
            this.idLabel.Text = "123";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "员工姓名：";
            // 
            // SetPeoplePanelBottom
            // 
            this.SetPeoplePanelBottom.Controls.Add(this.SaveEditBtn);
            this.SetPeoplePanelBottom.Controls.Add(this.DeleteBtn);
            this.SetPeoplePanelBottom.Controls.Add(this.AddBtn);
            this.SetPeoplePanelBottom.Controls.Add(this.label7);
            this.SetPeoplePanelBottom.Controls.Add(this.dgvUser);
            this.SetPeoplePanelBottom.Location = new System.Drawing.Point(12, 203);
            this.SetPeoplePanelBottom.Name = "SetPeoplePanelBottom";
            this.SetPeoplePanelBottom.Size = new System.Drawing.Size(1145, 344);
            this.SetPeoplePanelBottom.TabIndex = 1;
            // 
            // SaveEditBtn
            // 
            this.SaveEditBtn.Location = new System.Drawing.Point(240, 64);
            this.SaveEditBtn.Name = "SaveEditBtn";
            this.SaveEditBtn.Size = new System.Drawing.Size(99, 30);
            this.SaveEditBtn.TabIndex = 12;
            this.SaveEditBtn.Text = "保存更改";
            this.SaveEditBtn.UseVisualStyleBackColor = true;
            this.SaveEditBtn.Click += new System.EventHandler(this.SaveEditBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(125, 64);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(75, 30);
            this.DeleteBtn.TabIndex = 11;
            this.DeleteBtn.Text = "删除";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(38, 64);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 30);
            this.AddBtn.TabIndex = 10;
            this.AddBtn.Text = "添加";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(34, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 20);
            this.label7.TabIndex = 9;
            this.label7.Text = "员工信息管理";
            // 
            // dgvUser
            // 
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUser.Location = new System.Drawing.Point(38, 102);
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.RowHeadersVisible = false;
            this.dgvUser.RowTemplate.Height = 23;
            this.dgvUser.Size = new System.Drawing.Size(999, 223);
            this.dgvUser.TabIndex = 0;
            this.dgvUser.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvUser_DataBindingComplete);
            // 
            // SetPeopleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.SetPeoplePanelBottom);
            this.Controls.Add(this.SetPeoplePanelTop);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SetPeopleForm";
            this.Text = "SetPeopleForm";
            this.SetPeoplePanelTop.ResumeLayout(false);
            this.SetPeoplePanelTop.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SetPeoplePanelBottom.ResumeLayout(false);
            this.SetPeoplePanelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SetPeoplePanelTop;
        private System.Windows.Forms.Panel SetPeoplePanelBottom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pw2TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.TextBox pw3TextBox;
        private System.Windows.Forms.TextBox pw1TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SaveEditBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Button AddBtn;
    }
}