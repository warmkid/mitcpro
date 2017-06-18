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
            this.PSLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBeforePowerView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(482, 7);
            this.Title.Name = "Title";
            this.Title.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Title.Size = new System.Drawing.Size(209, 19);
            this.Title.TabIndex = 0;
            this.Title.Text = "吹膜机组开机前确认表";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CheckBeforePowerView
            // 
            this.CheckBeforePowerView.AllowUserToAddRows = false;
            this.CheckBeforePowerView.AllowUserToDeleteRows = false;
            this.CheckBeforePowerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheckBeforePowerView.Location = new System.Drawing.Point(27, 33);
            this.CheckBeforePowerView.Name = "CheckBeforePowerView";
            this.CheckBeforePowerView.RowTemplate.Height = 23;
            this.CheckBeforePowerView.Size = new System.Drawing.Size(1113, 402);
            this.CheckBeforePowerView.TabIndex = 1;
            this.CheckBeforePowerView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CheckBeforePowerView_CellContentClick);
            // 
            // PSLabel
            // 
            this.PSLabel.Font = new System.Drawing.Font("宋体", 12F);
            this.PSLabel.Location = new System.Drawing.Point(25, 438);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(498, 15);
            this.PSLabel.TabIndex = 6;
            this.PSLabel.Text = "注：\t正常或符合打“√”，不正常或不符合打“×”。";
            // 
            // ExtructionCheckBeforePowerStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 464);
            this.Controls.Add(this.PSLabel);
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
        private System.Windows.Forms.Label PSLabel;
    }
}