﻿namespace mySystem.Extruction.Process
{
    partial class HandoverRecordofExtrusionProcess
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpNight = new System.Windows.Forms.DateTimePicker();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbAbnormal = new System.Windows.Forms.Label();
            this.txbTakeinN = new System.Windows.Forms.TextBox();
            this.txbHandinN = new System.Windows.Forms.TextBox();
            this.txbTakeinD = new System.Windows.Forms.TextBox();
            this.txbAbnormalN = new System.Windows.Forms.TextBox();
            this.txbHandinD = new System.Windows.Forms.TextBox();
            this.txbAbnormalD = new System.Windows.Forms.TextBox();
            this.ibProductCode = new System.Windows.Forms.Label();
            this.lbAmounts = new System.Windows.Forms.Label();
            this.lbBatchNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbCodeN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbAmountsN = new System.Windows.Forms.TextBox();
            this.txbCodeD = new System.Windows.Forms.TextBox();
            this.txbBatchNoN = new System.Windows.Forms.TextBox();
            this.txbAmountsD = new System.Windows.Forms.TextBox();
            this.txbBatchNoD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbInstructionId = new System.Windows.Forms.TextBox();
            this.lbInstructionId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(36, 93);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(592, 400);
            this.dataGridView1.TabIndex = 52;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "确认项目";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 320;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "白班Y";
            this.Column2.Name = "Column2";
            this.Column2.Width = 60;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "白班N";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "夜班Y";
            this.Column4.Name = "Column4";
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "夜班N";
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(265, 519);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 29);
            this.button1.TabIndex = 51;
            this.button1.Text = "复核";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(228, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 19);
            this.label7.TabIndex = 50;
            this.label7.Text = "吹膜岗位交接班记录";
            // 
            // dtpDate
            // 
            this.dtpDate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(481, 56);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(265, 26);
            this.dtpDate.TabIndex = 49;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(362, 517);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 31);
            this.btnAdd.TabIndex = 48;
            this.btnAdd.Text = "保存";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDefault.Location = new System.Drawing.Point(36, 519);
            this.btnDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(100, 31);
            this.btnDefault.TabIndex = 47;
            this.btnDefault.Text = "设置";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Visible = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dtpNight);
            this.panel1.Controls.Add(this.dtpDay);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lbAbnormal);
            this.panel1.Controls.Add(this.txbTakeinN);
            this.panel1.Controls.Add(this.txbHandinN);
            this.panel1.Controls.Add(this.txbTakeinD);
            this.panel1.Controls.Add(this.txbAbnormalN);
            this.panel1.Controls.Add(this.txbHandinD);
            this.panel1.Controls.Add(this.txbAbnormalD);
            this.panel1.Controls.Add(this.ibProductCode);
            this.panel1.Controls.Add(this.lbAmounts);
            this.panel1.Controls.Add(this.lbBatchNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txbCodeN);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txbAmountsN);
            this.panel1.Controls.Add(this.txbCodeD);
            this.panel1.Controls.Add(this.txbBatchNoN);
            this.panel1.Controls.Add(this.txbAmountsD);
            this.panel1.Controls.Add(this.txbBatchNoD);
            this.panel1.Location = new System.Drawing.Point(663, 95);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 487);
            this.panel1.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(17, 444);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 58;
            this.label6.Text = "交接时间";
            // 
            // dtpNight
            // 
            this.dtpNight.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpNight.Location = new System.Drawing.Point(275, 444);
            this.dtpNight.Margin = new System.Windows.Forms.Padding(4);
            this.dtpNight.Name = "dtpNight";
            this.dtpNight.Size = new System.Drawing.Size(150, 26);
            this.dtpNight.TabIndex = 57;
            // 
            // dtpDay
            // 
            this.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpDay.Location = new System.Drawing.Point(121, 444);
            this.dtpDay.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.ShowUpDown = true;
            this.dtpDay.Size = new System.Drawing.Size(150, 26);
            this.dtpDay.TabIndex = 56;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(35, 412);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 55;
            this.label4.Text = "接班人";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(35, 376);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "交班人";
            // 
            // lbAbnormal
            // 
            this.lbAbnormal.AutoSize = true;
            this.lbAbnormal.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAbnormal.Location = new System.Drawing.Point(17, 161);
            this.lbAbnormal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAbnormal.Name = "lbAbnormal";
            this.lbAbnormal.Size = new System.Drawing.Size(72, 16);
            this.lbAbnormal.TabIndex = 53;
            this.lbAbnormal.Text = "异常处理";
            // 
            // txbTakeinN
            // 
            this.txbTakeinN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbTakeinN.Location = new System.Drawing.Point(275, 405);
            this.txbTakeinN.Margin = new System.Windows.Forms.Padding(4);
            this.txbTakeinN.Name = "txbTakeinN";
            this.txbTakeinN.Size = new System.Drawing.Size(150, 26);
            this.txbTakeinN.TabIndex = 52;
            // 
            // txbHandinN
            // 
            this.txbHandinN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbHandinN.Location = new System.Drawing.Point(275, 369);
            this.txbHandinN.Margin = new System.Windows.Forms.Padding(4);
            this.txbHandinN.Name = "txbHandinN";
            this.txbHandinN.Size = new System.Drawing.Size(150, 26);
            this.txbHandinN.TabIndex = 50;
            // 
            // txbTakeinD
            // 
            this.txbTakeinD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbTakeinD.Location = new System.Drawing.Point(117, 405);
            this.txbTakeinD.Margin = new System.Windows.Forms.Padding(4);
            this.txbTakeinD.Name = "txbTakeinD";
            this.txbTakeinD.Size = new System.Drawing.Size(150, 26);
            this.txbTakeinD.TabIndex = 51;
            // 
            // txbAbnormalN
            // 
            this.txbAbnormalN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbAbnormalN.Location = new System.Drawing.Point(275, 161);
            this.txbAbnormalN.Margin = new System.Windows.Forms.Padding(4);
            this.txbAbnormalN.Multiline = true;
            this.txbAbnormalN.Name = "txbAbnormalN";
            this.txbAbnormalN.Size = new System.Drawing.Size(150, 199);
            this.txbAbnormalN.TabIndex = 48;
            // 
            // txbHandinD
            // 
            this.txbHandinD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbHandinD.Location = new System.Drawing.Point(117, 369);
            this.txbHandinD.Margin = new System.Windows.Forms.Padding(4);
            this.txbHandinD.Name = "txbHandinD";
            this.txbHandinD.Size = new System.Drawing.Size(150, 26);
            this.txbHandinD.TabIndex = 49;
            // 
            // txbAbnormalD
            // 
            this.txbAbnormalD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbAbnormalD.Location = new System.Drawing.Point(117, 161);
            this.txbAbnormalD.Margin = new System.Windows.Forms.Padding(4);
            this.txbAbnormalD.Multiline = true;
            this.txbAbnormalD.Name = "txbAbnormalD";
            this.txbAbnormalD.Size = new System.Drawing.Size(150, 199);
            this.txbAbnormalD.TabIndex = 47;
            // 
            // ibProductCode
            // 
            this.ibProductCode.AutoSize = true;
            this.ibProductCode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ibProductCode.Location = new System.Drawing.Point(17, 133);
            this.ibProductCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ibProductCode.Name = "ibProductCode";
            this.ibProductCode.Size = new System.Drawing.Size(72, 16);
            this.ibProductCode.TabIndex = 46;
            this.ibProductCode.Text = "产品代码";
            // 
            // lbAmounts
            // 
            this.lbAmounts.AutoSize = true;
            this.lbAmounts.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAmounts.Location = new System.Drawing.Point(17, 97);
            this.lbAmounts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAmounts.Name = "lbAmounts";
            this.lbAmounts.Size = new System.Drawing.Size(72, 16);
            this.lbAmounts.TabIndex = 45;
            this.lbAmounts.Text = "生产数量";
            // 
            // lbBatchNo
            // 
            this.lbBatchNo.AutoSize = true;
            this.lbBatchNo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBatchNo.Location = new System.Drawing.Point(17, 63);
            this.lbBatchNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBatchNo.Name = "lbBatchNo";
            this.lbBatchNo.Size = new System.Drawing.Size(72, 16);
            this.lbBatchNo.TabIndex = 44;
            this.lbBatchNo.Text = "生产批号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(337, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 43;
            this.label2.Text = "夜班";
            // 
            // txbCodeN
            // 
            this.txbCodeN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbCodeN.Location = new System.Drawing.Point(275, 127);
            this.txbCodeN.Margin = new System.Windows.Forms.Padding(4);
            this.txbCodeN.Name = "txbCodeN";
            this.txbCodeN.Size = new System.Drawing.Size(150, 26);
            this.txbCodeN.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(181, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 42;
            this.label3.Text = "白班";
            // 
            // txbAmountsN
            // 
            this.txbAmountsN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbAmountsN.Location = new System.Drawing.Point(275, 91);
            this.txbAmountsN.Margin = new System.Windows.Forms.Padding(4);
            this.txbAmountsN.Name = "txbAmountsN";
            this.txbAmountsN.Size = new System.Drawing.Size(150, 26);
            this.txbAmountsN.TabIndex = 39;
            // 
            // txbCodeD
            // 
            this.txbCodeD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbCodeD.Location = new System.Drawing.Point(117, 127);
            this.txbCodeD.Margin = new System.Windows.Forms.Padding(4);
            this.txbCodeD.Name = "txbCodeD";
            this.txbCodeD.Size = new System.Drawing.Size(150, 26);
            this.txbCodeD.TabIndex = 40;
            // 
            // txbBatchNoN
            // 
            this.txbBatchNoN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbBatchNoN.Location = new System.Drawing.Point(275, 56);
            this.txbBatchNoN.Margin = new System.Windows.Forms.Padding(4);
            this.txbBatchNoN.Name = "txbBatchNoN";
            this.txbBatchNoN.Size = new System.Drawing.Size(150, 26);
            this.txbBatchNoN.TabIndex = 37;
            // 
            // txbAmountsD
            // 
            this.txbAmountsD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbAmountsD.Location = new System.Drawing.Point(117, 91);
            this.txbAmountsD.Margin = new System.Windows.Forms.Padding(4);
            this.txbAmountsD.Name = "txbAmountsD";
            this.txbAmountsD.Size = new System.Drawing.Size(150, 26);
            this.txbAmountsD.TabIndex = 38;
            // 
            // txbBatchNoD
            // 
            this.txbBatchNoD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbBatchNoD.Location = new System.Drawing.Point(117, 56);
            this.txbBatchNoD.Margin = new System.Windows.Forms.Padding(4);
            this.txbBatchNoD.Name = "txbBatchNoD";
            this.txbBatchNoD.Size = new System.Drawing.Size(150, 26);
            this.txbBatchNoD.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(347, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "生产日期";
            // 
            // txbInstructionId
            // 
            this.txbInstructionId.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbInstructionId.Location = new System.Drawing.Point(171, 60);
            this.txbInstructionId.Margin = new System.Windows.Forms.Padding(4);
            this.txbInstructionId.Name = "txbInstructionId";
            this.txbInstructionId.Size = new System.Drawing.Size(132, 26);
            this.txbInstructionId.TabIndex = 2;
            // 
            // lbInstructionId
            // 
            this.lbInstructionId.AutoSize = true;
            this.lbInstructionId.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInstructionId.Location = new System.Drawing.Point(32, 64);
            this.lbInstructionId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbInstructionId.Name = "lbInstructionId";
            this.lbInstructionId.Size = new System.Drawing.Size(104, 16);
            this.lbInstructionId.TabIndex = 1;
            this.lbInstructionId.Text = "生产指令编号";
            // 
            // HandoverRecordofExtrusionProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 595);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbInstructionId);
            this.Controls.Add(this.lbInstructionId);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HandoverRecordofExtrusionProcess";
            this.Text = "HandoverRecordofExtrusionProcess";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInstructionId;
        private System.Windows.Forms.TextBox txbInstructionId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbBatchNoD;
        private System.Windows.Forms.TextBox txbBatchNoN;
        private System.Windows.Forms.TextBox txbAmountsN;
        private System.Windows.Forms.TextBox txbAmountsD;
        private System.Windows.Forms.TextBox txbCodeN;
        private System.Windows.Forms.TextBox txbCodeD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpNight;
        private System.Windows.Forms.DateTimePicker dtpDay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbAbnormal;
        private System.Windows.Forms.TextBox txbTakeinN;
        private System.Windows.Forms.TextBox txbHandinN;
        private System.Windows.Forms.TextBox txbTakeinD;
        private System.Windows.Forms.TextBox txbAbnormalN;
        private System.Windows.Forms.TextBox txbHandinD;
        private System.Windows.Forms.TextBox txbAbnormalD;
        private System.Windows.Forms.Label ibProductCode;
        private System.Windows.Forms.Label lbAmounts;
        private System.Windows.Forms.Label lbBatchNo;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
    }
}