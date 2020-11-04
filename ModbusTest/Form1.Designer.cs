namespace ModbusTest
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_ReadData = new System.Windows.Forms.Button();
            this.listBox_Display = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(100, 51);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(75, 23);
            this.btn_Open.TabIndex = 0;
            this.btn_Open.Text = "连接串口";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // btn_ReadData
            // 
            this.btn_ReadData.Location = new System.Drawing.Point(458, 51);
            this.btn_ReadData.Name = "btn_ReadData";
            this.btn_ReadData.Size = new System.Drawing.Size(75, 23);
            this.btn_ReadData.TabIndex = 1;
            this.btn_ReadData.Text = "读取数据";
            this.btn_ReadData.UseVisualStyleBackColor = true;
            this.btn_ReadData.Click += new System.EventHandler(this.btn_ReadData_Click);
            // 
            // listBox_Display
            // 
            this.listBox_Display.FormattingEnabled = true;
            this.listBox_Display.ItemHeight = 12;
            this.listBox_Display.Location = new System.Drawing.Point(100, 107);
            this.listBox_Display.Name = "listBox_Display";
            this.listBox_Display.Size = new System.Drawing.Size(433, 196);
            this.listBox_Display.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 345);
            this.Controls.Add(this.listBox_Display);
            this.Controls.Add(this.btn_ReadData);
            this.Controls.Add(this.btn_Open);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Button btn_ReadData;
        private System.Windows.Forms.ListBox listBox_Display;
    }
}

