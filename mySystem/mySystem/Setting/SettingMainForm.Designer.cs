namespace mySystem.Setting
{
    partial class SettingMainForm
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
            this.SettingPanelRight = new System.Windows.Forms.Panel();
            this.SettingPanelLeft = new System.Windows.Forms.Panel();
            this.PWSetBtn = new System.Windows.Forms.Button();
            this.AuthoritySetBtn = new System.Windows.Forms.Button();
            this.PeopleSetBtn = new System.Windows.Forms.Button();
            this.SystemSetBtn = new System.Windows.Forms.Button();
            this.SettingPanelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingPanelRight
            // 
            this.SettingPanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SettingPanelRight.Location = new System.Drawing.Point(185, 2);
            this.SettingPanelRight.Name = "SettingPanelRight";
            this.SettingPanelRight.Size = new System.Drawing.Size(1170, 615);
            this.SettingPanelRight.TabIndex = 3;
            // 
            // SettingPanelLeft
            // 
            this.SettingPanelLeft.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SettingPanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SettingPanelLeft.Controls.Add(this.PWSetBtn);
            this.SettingPanelLeft.Controls.Add(this.AuthoritySetBtn);
            this.SettingPanelLeft.Controls.Add(this.PeopleSetBtn);
            this.SettingPanelLeft.Controls.Add(this.SystemSetBtn);
            this.SettingPanelLeft.Location = new System.Drawing.Point(4, 2);
            this.SettingPanelLeft.Name = "SettingPanelLeft";
            this.SettingPanelLeft.Size = new System.Drawing.Size(180, 615);
            this.SettingPanelLeft.TabIndex = 2;
            // 
            // PWSetBtn
            // 
            this.PWSetBtn.FlatAppearance.BorderSize = 0;
            this.PWSetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PWSetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PWSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PWSetBtn.Location = new System.Drawing.Point(3, 133);
            this.PWSetBtn.Name = "PWSetBtn";
            this.PWSetBtn.Size = new System.Drawing.Size(172, 43);
            this.PWSetBtn.TabIndex = 5;
            this.PWSetBtn.Text = " 密码管理";
            this.PWSetBtn.UseVisualStyleBackColor = true;
            this.PWSetBtn.Click += new System.EventHandler(this.PWSetBtn_Click);
            // 
            // AuthoritySetBtn
            // 
            this.AuthoritySetBtn.FlatAppearance.BorderSize = 0;
            this.AuthoritySetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AuthoritySetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AuthoritySetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AuthoritySetBtn.Location = new System.Drawing.Point(3, 90);
            this.AuthoritySetBtn.Name = "AuthoritySetBtn";
            this.AuthoritySetBtn.Size = new System.Drawing.Size(172, 43);
            this.AuthoritySetBtn.TabIndex = 4;
            this.AuthoritySetBtn.Text = " 权限管理";
            this.AuthoritySetBtn.UseVisualStyleBackColor = true;
            this.AuthoritySetBtn.Click += new System.EventHandler(this.AuthoritySetBtn_Click);
            // 
            // PeopleSetBtn
            // 
            this.PeopleSetBtn.FlatAppearance.BorderSize = 0;
            this.PeopleSetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PeopleSetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PeopleSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PeopleSetBtn.Location = new System.Drawing.Point(3, 47);
            this.PeopleSetBtn.Name = "PeopleSetBtn";
            this.PeopleSetBtn.Size = new System.Drawing.Size(172, 43);
            this.PeopleSetBtn.TabIndex = 3;
            this.PeopleSetBtn.Text = " 人员管理";
            this.PeopleSetBtn.UseVisualStyleBackColor = true;
            this.PeopleSetBtn.Click += new System.EventHandler(this.PeopleSetBtn_Click);
            // 
            // SystemSetBtn
            // 
            this.SystemSetBtn.FlatAppearance.BorderSize = 0;
            this.SystemSetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SystemSetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SystemSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SystemSetBtn.Location = new System.Drawing.Point(3, 4);
            this.SystemSetBtn.Name = "SystemSetBtn";
            this.SystemSetBtn.Size = new System.Drawing.Size(172, 43);
            this.SystemSetBtn.TabIndex = 0;
            this.SystemSetBtn.Text = " 参数设置";
            this.SystemSetBtn.UseVisualStyleBackColor = true;
            this.SystemSetBtn.Click += new System.EventHandler(this.SystemSetBtn_Click);
            // 
            // SettingMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 620);
            this.Controls.Add(this.SettingPanelRight);
            this.Controls.Add(this.SettingPanelLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingMainForm";
            this.Text = "SettingMainForm";
            this.SettingPanelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SettingPanelRight;
        private System.Windows.Forms.Panel SettingPanelLeft;
        private System.Windows.Forms.Button SystemSetBtn;
        private System.Windows.Forms.Button PWSetBtn;
        private System.Windows.Forms.Button AuthoritySetBtn;
        private System.Windows.Forms.Button PeopleSetBtn;
    }
}