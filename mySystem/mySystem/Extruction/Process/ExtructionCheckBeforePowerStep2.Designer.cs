namespace mySystem.Extruction.Process
{
    partial class ExtructionCheckBeforePowerStep2
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
            this.Title = new System.Windows.Forms.Label();
            this.CheckBeforePowerView = new System.Windows.Forms.DataGridView();
            this.StaticLabel2 = new System.Windows.Forms.Label();
            this.PSLabel = new System.Windows.Forms.Label();
            this.StaticLabel3 = new System.Windows.Forms.Label();
            this.StaticLabel4 = new System.Windows.Forms.Label();
            this.StaticLabel5 = new System.Windows.Forms.Label();
            this.Confirm = new System.Windows.Forms.Label();
            this.ConfirmDate = new System.Windows.Forms.Label();
            this.Check = new System.Windows.Forms.Label();
            this.CheckDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(502, 9);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(125, 12);
            this.Title.TabIndex = 0;
            this.Title.Text = "吹膜机组开机前确认表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CheckBeforePowerView
            // 
            this.CheckBeforePowerView.AllowUserToAddRows = false;
            this.CheckBeforePowerView.AllowUserToDeleteRows = false;
            this.CheckBeforePowerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheckBeforePowerView.Location = new System.Drawing.Point(27, 30);
            this.CheckBeforePowerView.Name = "CheckBeforePowerView";
            this.CheckBeforePowerView.RowTemplate.Height = 23;
            this.CheckBeforePowerView.Size = new System.Drawing.Size(1123, 435);
            this.CheckBeforePowerView.TabIndex = 1;
            // 
            // StaticLabel2
            // 
            this.StaticLabel2.AutoSize = true;
            this.StaticLabel2.Location = new System.Drawing.Point(24, 497);
            this.StaticLabel2.Name = "StaticLabel2";
            this.StaticLabel2.Size = new System.Drawing.Size(53, 12);
            this.StaticLabel2.TabIndex = 5;
            this.StaticLabel2.Text = "确认人：";
            this.StaticLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PSLabel
            // 
            this.PSLabel.Location = new System.Drawing.Point(25, 477);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(498, 15);
            this.PSLabel.TabIndex = 6;
            this.PSLabel.Text = "注：\t正常或符合打“√”，不正常或不符合打“×”。";
            // 
            // StaticLabel3
            // 
            this.StaticLabel3.AutoSize = true;
            this.StaticLabel3.Location = new System.Drawing.Point(395, 497);
            this.StaticLabel3.Name = "StaticLabel3";
            this.StaticLabel3.Size = new System.Drawing.Size(41, 12);
            this.StaticLabel3.TabIndex = 7;
            this.StaticLabel3.Text = "日期：";
            // 
            // StaticLabel4
            // 
            this.StaticLabel4.AutoSize = true;
            this.StaticLabel4.Location = new System.Drawing.Point(666, 496);
            this.StaticLabel4.Name = "StaticLabel4";
            this.StaticLabel4.Size = new System.Drawing.Size(53, 12);
            this.StaticLabel4.TabIndex = 8;
            this.StaticLabel4.Text = "复核人：";
            // 
            // StaticLabel5
            // 
            this.StaticLabel5.AutoSize = true;
            this.StaticLabel5.Location = new System.Drawing.Point(1042, 497);
            this.StaticLabel5.Name = "StaticLabel5";
            this.StaticLabel5.Size = new System.Drawing.Size(41, 12);
            this.StaticLabel5.TabIndex = 9;
            this.StaticLabel5.Text = "日期：";
            // 
            // Confirm
            // 
            this.Confirm.AutoSize = true;
            this.Confirm.Location = new System.Drawing.Point(83, 497);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(11, 12);
            this.Confirm.TabIndex = 10;
            this.Confirm.Text = "A";
            // 
            // ConfirmDate
            // 
            this.ConfirmDate.AutoSize = true;
            this.ConfirmDate.Location = new System.Drawing.Point(431, 498);
            this.ConfirmDate.Name = "ConfirmDate";
            this.ConfirmDate.Size = new System.Drawing.Size(59, 12);
            this.ConfirmDate.TabIndex = 11;
            this.ConfirmDate.Text = "X年X月X日";
            // 
            // Check
            // 
            this.Check.AutoSize = true;
            this.Check.Location = new System.Drawing.Point(714, 497);
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(11, 12);
            this.Check.TabIndex = 12;
            this.Check.Text = "B";
            // 
            // CheckDate
            // 
            this.CheckDate.AutoSize = true;
            this.CheckDate.Location = new System.Drawing.Point(1078, 496);
            this.CheckDate.Name = "CheckDate";
            this.CheckDate.Size = new System.Drawing.Size(59, 12);
            this.CheckDate.TabIndex = 13;
            this.CheckDate.Text = "X年X月X日";
            // 
            // ExtructionCheckBeforePowerStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 523);
            this.Controls.Add(this.CheckDate);
            this.Controls.Add(this.Check);
            this.Controls.Add(this.ConfirmDate);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.StaticLabel5);
            this.Controls.Add(this.StaticLabel4);
            this.Controls.Add(this.StaticLabel3);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.StaticLabel2);
            this.Controls.Add(this.CheckBeforePowerView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtructionCheckBeforePowerStep2";
            this.Text = "ExtructionCheckBeforePowerStep2";
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView CheckBeforePowerView;
        private System.Windows.Forms.Label StaticLabel2;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.Label StaticLabel3;
        private System.Windows.Forms.Label StaticLabel4;
        private System.Windows.Forms.Label StaticLabel5;
        private System.Windows.Forms.Label Confirm;
        private System.Windows.Forms.Label ConfirmDate;
        private System.Windows.Forms.Label Check;
        private System.Windows.Forms.Label CheckDate;
    }
}