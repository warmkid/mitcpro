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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingMainForm));
            this.SettingPanelRight = new System.Windows.Forms.Panel();
            this.SettingPanelLeft = new System.Windows.Forms.Panel();
            this.Btn灭菌 = new System.Windows.Forms.Button();
            this.Btn清洁分切 = new System.Windows.Forms.Button();
            this.ExtruSetBtn = new System.Windows.Forms.Button();
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
            this.SettingPanelLeft.Controls.Add(this.Btn灭菌);
            this.SettingPanelLeft.Controls.Add(this.Btn清洁分切);
            this.SettingPanelLeft.Controls.Add(this.ExtruSetBtn);
            this.SettingPanelLeft.Controls.Add(this.PeopleSetBtn);
            this.SettingPanelLeft.Controls.Add(this.SystemSetBtn);
            this.SettingPanelLeft.Location = new System.Drawing.Point(4, 2);
            this.SettingPanelLeft.Name = "SettingPanelLeft";
            this.SettingPanelLeft.Size = new System.Drawing.Size(180, 615);
            this.SettingPanelLeft.TabIndex = 2;
            // 
            // Btn灭菌
            // 
            this.Btn灭菌.FlatAppearance.BorderSize = 0;
            this.Btn灭菌.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn灭菌.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn灭菌.Image = ((System.Drawing.Image)(resources.GetObject("Btn灭菌.Image")));
            this.Btn灭菌.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn灭菌.Location = new System.Drawing.Point(3, 90);
            this.Btn灭菌.Name = "Btn灭菌";
            this.Btn灭菌.Size = new System.Drawing.Size(172, 43);
            this.Btn灭菌.TabIndex = 6;
            this.Btn灭菌.Text = " 灭菌设置";
            this.Btn灭菌.UseVisualStyleBackColor = true;
            this.Btn灭菌.Click += new System.EventHandler(this.Btn灭菌_Click);
            // 
            // Btn清洁分切
            // 
            this.Btn清洁分切.FlatAppearance.BorderSize = 0;
            this.Btn清洁分切.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn清洁分切.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn清洁分切.Image = ((System.Drawing.Image)(resources.GetObject("Btn清洁分切.Image")));
            this.Btn清洁分切.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn清洁分切.Location = new System.Drawing.Point(3, 47);
            this.Btn清洁分切.Name = "Btn清洁分切";
            this.Btn清洁分切.Size = new System.Drawing.Size(172, 43);
            this.Btn清洁分切.TabIndex = 5;
            this.Btn清洁分切.Text = "  清洁分切设置";
            this.Btn清洁分切.UseVisualStyleBackColor = true;
            this.Btn清洁分切.Click += new System.EventHandler(this.Btn清洁分切_Click);
            // 
            // ExtruSetBtn
            // 
            this.ExtruSetBtn.FlatAppearance.BorderSize = 0;
            this.ExtruSetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExtruSetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExtruSetBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExtruSetBtn.Image")));
            this.ExtruSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExtruSetBtn.Location = new System.Drawing.Point(2, 3);
            this.ExtruSetBtn.Name = "ExtruSetBtn";
            this.ExtruSetBtn.Size = new System.Drawing.Size(172, 43);
            this.ExtruSetBtn.TabIndex = 4;
            this.ExtruSetBtn.Text = "  吹膜设置 ";
            this.ExtruSetBtn.UseVisualStyleBackColor = true;
            this.ExtruSetBtn.Click += new System.EventHandler(this.ExtruSetBtn_Click);
            // 
            // PeopleSetBtn
            // 
            this.PeopleSetBtn.FlatAppearance.BorderSize = 0;
            this.PeopleSetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PeopleSetBtn.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PeopleSetBtn.Image = ((System.Drawing.Image)(resources.GetObject("PeopleSetBtn.Image")));
            this.PeopleSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PeopleSetBtn.Location = new System.Drawing.Point(3, 176);
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
            this.SystemSetBtn.Image = ((System.Drawing.Image)(resources.GetObject("SystemSetBtn.Image")));
            this.SystemSetBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SystemSetBtn.Location = new System.Drawing.Point(3, 133);
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
        private System.Windows.Forms.Button ExtruSetBtn;
        private System.Windows.Forms.Button PeopleSetBtn;
        private System.Windows.Forms.Button Btn灭菌;
        private System.Windows.Forms.Button Btn清洁分切;
    }
}