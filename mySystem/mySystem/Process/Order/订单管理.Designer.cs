namespace 订单和库存管理
{
    partial class 订单管理
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage销售订单 = new System.Windows.Forms.TabPage();
            this.tabPage采购需求单 = new System.Windows.Forms.TabPage();
            this.tabPage采购批准单 = new System.Windows.Forms.TabPage();
            this.tabPage采购订单 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage销售订单);
            this.tabControl1.Controls.Add(this.tabPage采购需求单);
            this.tabControl1.Controls.Add(this.tabPage采购批准单);
            this.tabControl1.Controls.Add(this.tabPage采购订单);
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(852, 398);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage销售订单
            // 
            this.tabPage销售订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage销售订单.Name = "tabPage销售订单";
            this.tabPage销售订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage销售订单.Size = new System.Drawing.Size(192, 67);
            this.tabPage销售订单.TabIndex = 1;
            this.tabPage销售订单.Text = "销售订单";
            this.tabPage销售订单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购需求单
            // 
            this.tabPage采购需求单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购需求单.Name = "tabPage采购需求单";
            this.tabPage采购需求单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购需求单.Size = new System.Drawing.Size(192, 67);
            this.tabPage采购需求单.TabIndex = 2;
            this.tabPage采购需求单.Text = "采购需求单";
            this.tabPage采购需求单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购批准单
            // 
            this.tabPage采购批准单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购批准单.Name = "tabPage采购批准单";
            this.tabPage采购批准单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购批准单.Size = new System.Drawing.Size(764, 67);
            this.tabPage采购批准单.TabIndex = 3;
            this.tabPage采购批准单.Text = "采购批准单";
            this.tabPage采购批准单.UseVisualStyleBackColor = true;
            // 
            // tabPage采购订单
            // 
            this.tabPage采购订单.Location = new System.Drawing.Point(4, 29);
            this.tabPage采购订单.Name = "tabPage采购订单";
            this.tabPage采购订单.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage采购订单.Size = new System.Drawing.Size(844, 365);
            this.tabPage采购订单.TabIndex = 4;
            this.tabPage采购订单.Text = "采购订单";
            this.tabPage采购订单.UseVisualStyleBackColor = true;
            // 
            // 订单管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 422);
            this.Controls.Add(this.tabControl1);
            this.Name = "订单管理";
            this.Text = "订单管理";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage销售订单;
        private System.Windows.Forms.TabPage tabPage采购需求单;
        private System.Windows.Forms.TabPage tabPage采购批准单;
        private System.Windows.Forms.TabPage tabPage采购订单;
    }
}