namespace AirSystem
{
    partial class ModBusTest
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
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_ReadKeep = new System.Windows.Forms.Button();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(130, 71);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(75, 23);
            this.btn_Open.TabIndex = 0;
            this.btn_Open.Text = "打开串口";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // btn_ReadKeep
            // 
            this.btn_ReadKeep.Location = new System.Drawing.Point(526, 71);
            this.btn_ReadKeep.Name = "btn_ReadKeep";
            this.btn_ReadKeep.Size = new System.Drawing.Size(75, 23);
            this.btn_ReadKeep.TabIndex = 0;
            this.btn_ReadKeep.Text = "读取寄存器";
            this.btn_ReadKeep.UseVisualStyleBackColor = true;
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.ItemHeight = 17;
            this.listBoxData.Location = new System.Drawing.Point(130, 146);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(471, 242);
            this.listBoxData.TabIndex = 1;
            // 
            // ModBusTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 433);
            this.Controls.Add(this.listBoxData);
            this.Controls.Add(this.btn_ReadKeep);
            this.Controls.Add(this.btn_Open);
            this.Name = "ModBusTest";
            this.Text = "ModBusTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Button btn_ReadKeep;
        private System.Windows.Forms.ListBox listBoxData;
    }
}