namespace mySystem.Query
{
    partial class QueryExtruForm
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
            this.Chart6Btn = new System.Windows.Forms.Button();
            this.Chart1Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Chart6Btn
            // 
            this.Chart6Btn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Chart6Btn.Location = new System.Drawing.Point(260, 22);
            this.Chart6Btn.Name = "Chart6Btn";
            this.Chart6Btn.Size = new System.Drawing.Size(194, 41);
            this.Chart6Btn.TabIndex = 10;
            this.Chart6Btn.Text = "吹膜工序物料平衡记录";
            this.Chart6Btn.UseVisualStyleBackColor = true;
            this.Chart6Btn.Click += new System.EventHandler(this.Chart6Btn_Click);
            // 
            // Chart1Btn
            // 
            this.Chart1Btn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Chart1Btn.Location = new System.Drawing.Point(67, 22);
            this.Chart1Btn.Name = "Chart1Btn";
            this.Chart1Btn.Size = new System.Drawing.Size(148, 40);
            this.Chart1Btn.TabIndex = 9;
            this.Chart1Btn.Text = "吹膜生产日报表";
            this.Chart1Btn.UseVisualStyleBackColor = true;
            this.Chart1Btn.Click += new System.EventHandler(this.Chart1Btn_Click);
            // 
            // QueryExtruForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 615);
            this.Controls.Add(this.Chart6Btn);
            this.Controls.Add(this.Chart1Btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QueryExtruForm";
            this.Text = "QueryExtruForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Chart6Btn;
        private System.Windows.Forms.Button Chart1Btn;
    }
}