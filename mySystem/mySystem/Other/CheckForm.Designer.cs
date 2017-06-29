namespace mySystem
{
    partial class CheckForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckForm));
            this.label1 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CheckerPWTextBox = new System.Windows.Forms.TextBox();
            this.用户密码 = new System.Windows.Forms.Label();
            this.CheckerIDTextBox = new System.Windows.Forms.TextBox();
            this.用户ID = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.NotOKBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OpTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimHei", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(147, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 21);
            this.label1.TabIndex = 33;
            this.label1.Text = "颇尔奥星管理系统";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelBtn.Image = ((System.Drawing.Image)(resources.GetObject("CancelBtn.Image")));
            this.CancelBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CancelBtn.Location = new System.Drawing.Point(258, 259);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(83, 30);
            this.CancelBtn.TabIndex = 31;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OKBtn.Image = ((System.Drawing.Image)(resources.GetObject("OKBtn.Image")));
            this.OKBtn.Location = new System.Drawing.Point(17, 259);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(106, 30);
            this.OKBtn.TabIndex = 30;
            this.OKBtn.Text = "审核通过";
            this.OKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CheckerPWTextBox
            // 
            this.CheckerPWTextBox.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckerPWTextBox.Location = new System.Drawing.Point(94, 114);
            this.CheckerPWTextBox.Name = "CheckerPWTextBox";
            this.CheckerPWTextBox.PasswordChar = '*';
            this.CheckerPWTextBox.Size = new System.Drawing.Size(234, 26);
            this.CheckerPWTextBox.TabIndex = 27;
            this.CheckerPWTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckerPWTextBox_KeyPress);
            // 
            // 用户密码
            // 
            this.用户密码.AutoSize = true;
            this.用户密码.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.用户密码.Location = new System.Drawing.Point(14, 117);
            this.用户密码.Name = "用户密码";
            this.用户密码.Size = new System.Drawing.Size(56, 16);
            this.用户密码.TabIndex = 29;
            this.用户密码.Text = "密码：";
            // 
            // CheckerIDTextBox
            // 
            this.CheckerIDTextBox.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckerIDTextBox.Location = new System.Drawing.Point(94, 83);
            this.CheckerIDTextBox.Name = "CheckerIDTextBox";
            this.CheckerIDTextBox.Size = new System.Drawing.Size(234, 26);
            this.CheckerIDTextBox.TabIndex = 26;
            this.CheckerIDTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckerIDTextBox_KeyPress);
            // 
            // 用户ID
            // 
            this.用户ID.AutoSize = true;
            this.用户ID.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.用户ID.Location = new System.Drawing.Point(14, 86);
            this.用户ID.Name = "用户ID";
            this.用户ID.Size = new System.Drawing.Size(72, 16);
            this.用户ID.TabIndex = 28;
            this.用户ID.Text = "审核员：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // NotOKBtn
            // 
            this.NotOKBtn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NotOKBtn.Image = ((System.Drawing.Image)(resources.GetObject("NotOKBtn.Image")));
            this.NotOKBtn.Location = new System.Drawing.Point(144, 259);
            this.NotOKBtn.Name = "NotOKBtn";
            this.NotOKBtn.Size = new System.Drawing.Size(94, 30);
            this.NotOKBtn.TabIndex = 34;
            this.NotOKBtn.Text = "未通过";
            this.NotOKBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NotOKBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.NotOKBtn.UseVisualStyleBackColor = true;
            this.NotOKBtn.Click += new System.EventHandler(this.NotOKBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(14, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 35;
            this.label2.Text = "审核意见：";
            // 
            // OpTextBox
            // 
            this.OpTextBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.OpTextBox.Location = new System.Drawing.Point(94, 160);
            this.OpTextBox.Multiline = true;
            this.OpTextBox.Name = "OpTextBox";
            this.OpTextBox.Size = new System.Drawing.Size(234, 81);
            this.OpTextBox.TabIndex = 36;
            this.OpTextBox.Text = "合格。";
            // 
            // CheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 310);
            this.ControlBox = false;
            this.Controls.Add(this.OpTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NotOKBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CheckerPWTextBox);
            this.Controls.Add(this.用户密码);
            this.Controls.Add(this.CheckerIDTextBox);
            this.Controls.Add(this.用户ID);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label 用户密码;
        private System.Windows.Forms.Label 用户ID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OpTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox CheckerPWTextBox;
        private System.Windows.Forms.TextBox CheckerIDTextBox;
        private System.Windows.Forms.Button NotOKBtn;
        public System.Windows.Forms.Button CancelBtn;
    }
}