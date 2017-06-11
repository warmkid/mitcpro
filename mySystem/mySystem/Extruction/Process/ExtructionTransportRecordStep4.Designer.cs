namespace mySystem.Extruction.Process
{
    partial class ExtructionTransportRecordStep4
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
            this.TransportRecordView = new System.Windows.Forms.DataGridView();
            this.DeleteLineBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TransportRecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(204, 6);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(101, 12);
            this.Title.TabIndex = 3;
            this.Title.Text = "吹膜工序传料记录";
            // 
            // TransportRecordView
            // 
            this.TransportRecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TransportRecordView.Location = new System.Drawing.Point(12, 23);
            this.TransportRecordView.Name = "TransportRecordView";
            this.TransportRecordView.RowTemplate.Height = 23;
            this.TransportRecordView.Size = new System.Drawing.Size(498, 268);
            this.TransportRecordView.TabIndex = 4;
            // 
            // DeleteLineBtn
            // 
            this.DeleteLineBtn.Location = new System.Drawing.Point(435, 298);
            this.DeleteLineBtn.Name = "DeleteLineBtn";
            this.DeleteLineBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteLineBtn.TabIndex = 5;
            this.DeleteLineBtn.Text = "删除记录";
            this.DeleteLineBtn.UseVisualStyleBackColor = true;
            this.DeleteLineBtn.Click += new System.EventHandler(this.DeleteLineBtn_Click_1);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Location = new System.Drawing.Point(352, 298);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddLineBtn.TabIndex = 6;
            this.AddLineBtn.Text = "添加记录";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click_1);
            // 
            // ExtructionTransportRecordStep4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 326);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.DeleteLineBtn);
            this.Controls.Add(this.TransportRecordView);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtructionTransportRecordStep4";
            this.Text = "ExtructionTransportRecordStep4";
            ((System.ComponentModel.ISupportInitialize)(this.TransportRecordView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.DataGridView TransportRecordView;
        private System.Windows.Forms.Button DeleteLineBtn;
        private System.Windows.Forms.Button AddLineBtn;
    }
}