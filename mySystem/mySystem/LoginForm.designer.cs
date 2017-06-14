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
            this.ExitButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UserPWTextBox = new System.Windows.Forms.TextBox();
            this.用户密码 = new System.Windows.Forms.Label();
            this.UserIDTextBox = new System.Windows.Forms.TextBox();
            this.用户ID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExitButton.Location = new System.Drawing.Point(188, 106);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 25);
            this.ExitButton.TabIndex = 23;
            this.ExitButton.Text = "退出系统";
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Image = ((System.Drawing.Image)(resources.GetObject("LoginButton.Image")));
            this.LoginButton.Location = new System.Drawing.Point(51, 106);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(100, 25);
            this.LoginButton.TabIndex = 22;
            this.LoginButton.Text = "登录系统";
            this.LoginButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LoginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UserPWTextBox
            // 
            this.UserPWTextBox.Location = new System.Drawing.Point(120, 66);
            this.UserPWTextBox.Name = "UserPWTextBox";
            this.UserPWTextBox.PasswordChar = '*';
            this.UserPWTextBox.Size = new System.Drawing.Size(174, 21);
            this.UserPWTextBox.TabIndex = 21;
            // 
            // 用户密码
            // 
            this.用户密码.AutoSize = true;
            this.用户密码.Location = new System.Drawing.Point(49, 69);
            this.用户密码.Name = "用户密码";
            this.用户密码.Size = new System.Drawing.Size(65, 12);
            this.用户密码.TabIndex = 20;
            this.用户密码.Text = "用户密码：";
            // 
            // UserIDTextBox
            // 
            this.UserIDTextBox.Location = new System.Drawing.Point(120, 35);
            this.UserIDTextBox.Name = "UserIDTextBox";
            this.UserIDTextBox.Size = new System.Drawing.Size(174, 21);
            this.UserIDTextBox.TabIndex = 19;
            // 
            // 用户ID
            // 
            this.用户ID.AutoSize = true;
            this.用户ID.Location = new System.Drawing.Point(49, 38);
            this.用户ID.Name = "用户ID";
            this.用户ID.Size = new System.Drawing.Size(53, 12);
            this.用户ID.TabIndex = 18;
            this.用户ID.Text = "用户ID：";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 166);
            this.ControlBox = false;
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.UserPWTextBox);
            this.Controls.Add(this.用户密码);
            this.Controls.Add(this.UserIDTextBox);
            this.Controls.Add(this.用户ID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录管理系统";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox UserPWTextBox;
        private System.Windows.Forms.Label 用户密码;
        public System.Windows.Forms.TextBox UserIDTextBox;
        private System.Windows.Forms.Label 用户ID;
    }
}