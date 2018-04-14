namespace mySystem
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.usernameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UserPWTextBox = new System.Windows.Forms.TextBox();
            this.用户密码 = new System.Windows.Forms.Label();
            this.UserIDTextBox = new System.Windows.Forms.TextBox();
            this.用户ID = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(118, 58);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(0, 12);
            this.usernameLabel.TabIndex = 26;
            this.usernameLabel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(136, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 21);
            this.label1.TabIndex = 25;
            this.label1.Text = "颇尔奥星管理系统";
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExitButton.Location = new System.Drawing.Point(188, 153);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(106, 30);
            this.ExitButton.TabIndex = 23;
            this.ExitButton.Text = "退出系统";
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoginButton.Image = ((System.Drawing.Image)(resources.GetObject("LoginButton.Image")));
            this.LoginButton.Location = new System.Drawing.Point(51, 153);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(106, 30);
            this.LoginButton.TabIndex = 22;
            this.LoginButton.Text = "登录系统";
            this.LoginButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LoginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UserPWTextBox
            // 
            this.UserPWTextBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserPWTextBox.Location = new System.Drawing.Point(120, 115);
            this.UserPWTextBox.Name = "UserPWTextBox";
            this.UserPWTextBox.PasswordChar = '*';
            this.UserPWTextBox.Size = new System.Drawing.Size(174, 26);
            this.UserPWTextBox.TabIndex = 2;
            this.UserPWTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserPWTextBox_KeyPress);
            // 
            // 用户密码
            // 
            this.用户密码.AutoSize = true;
            this.用户密码.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.用户密码.Location = new System.Drawing.Point(49, 118);
            this.用户密码.Name = "用户密码";
            this.用户密码.Size = new System.Drawing.Size(56, 16);
            this.用户密码.TabIndex = 20;
            this.用户密码.Text = "密码：";
            // 
            // UserIDTextBox
            // 
            this.UserIDTextBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserIDTextBox.Location = new System.Drawing.Point(120, 84);
            this.UserIDTextBox.Name = "UserIDTextBox";
            this.UserIDTextBox.Size = new System.Drawing.Size(174, 26);
            this.UserIDTextBox.TabIndex = 1;
            this.UserIDTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserIDTextBox_KeyPress);
            // 
            // 用户ID
            // 
            this.用户ID.AutoSize = true;
            this.用户ID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.用户ID.Location = new System.Drawing.Point(49, 87);
            this.用户ID.Name = "用户ID";
            this.用户ID.Size = new System.Drawing.Size(72, 16);
            this.用户ID.TabIndex = 18;
            this.用户ID.Text = "操作员：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 197);
            this.ControlBox = false;
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.UserPWTextBox);
            this.Controls.Add(this.用户密码);
            this.Controls.Add(this.UserIDTextBox);
            this.Controls.Add(this.用户ID);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label 用户密码;
        private System.Windows.Forms.Label 用户ID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox UserIDTextBox;
        private System.Windows.Forms.TextBox UserPWTextBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button ExitButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}