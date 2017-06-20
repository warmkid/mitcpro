namespace mySystem
{
    partial class Connect2SqlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connect2SqlForm));
            this.ExitButton = new System.Windows.Forms.Button();
            this.Connect2SqlButton = new System.Windows.Forms.Button();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.端口 = new System.Windows.Forms.Label();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.服务器IP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("SimSun", 12F);
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.Location = new System.Drawing.Point(201, 110);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(85, 30);
            this.ExitButton.TabIndex = 37;
            this.ExitButton.Text = "退出";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // Connect2SqlButton
            // 
            this.Connect2SqlButton.Font = new System.Drawing.Font("SimSun", 12F);
            this.Connect2SqlButton.Image = ((System.Drawing.Image)(resources.GetObject("Connect2SqlButton.Image")));
            this.Connect2SqlButton.Location = new System.Drawing.Point(62, 110);
            this.Connect2SqlButton.Name = "Connect2SqlButton";
            this.Connect2SqlButton.Size = new System.Drawing.Size(85, 30);
            this.Connect2SqlButton.TabIndex = 36;
            this.Connect2SqlButton.Text = "连接";
            this.Connect2SqlButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Connect2SqlButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Connect2SqlButton.UseVisualStyleBackColor = true;
            this.Connect2SqlButton.Click += new System.EventHandler(this.Connect2SqlButton_Click);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.PortTextBox.Location = new System.Drawing.Point(124, 64);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(163, 26);
            this.PortTextBox.TabIndex = 33;
            this.PortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortTextBox_KeyPress);
            // 
            // 端口
            // 
            this.端口.AutoSize = true;
            this.端口.Font = new System.Drawing.Font("SimSun", 12F);
            this.端口.Location = new System.Drawing.Point(43, 69);
            this.端口.Name = "端口";
            this.端口.Size = new System.Drawing.Size(56, 16);
            this.端口.TabIndex = 32;
            this.端口.Text = "端口：";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.IPTextBox.Location = new System.Drawing.Point(124, 31);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(163, 26);
            this.IPTextBox.TabIndex = 31;
            this.IPTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IPTextBox_KeyPress);
            // 
            // 服务器IP
            // 
            this.服务器IP.AutoSize = true;
            this.服务器IP.Font = new System.Drawing.Font("SimSun", 12F);
            this.服务器IP.Location = new System.Drawing.Point(43, 36);
            this.服务器IP.Name = "服务器IP";
            this.服务器IP.Size = new System.Drawing.Size(88, 16);
            this.服务器IP.TabIndex = 30;
            this.服务器IP.Text = "服务器IP：";
            // 
            // Connect2SqlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 166);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.Connect2SqlButton);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.端口);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.服务器IP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Connect2SqlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "连接到数据库";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button Connect2SqlButton;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label 端口;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Label 服务器IP;
    }
}