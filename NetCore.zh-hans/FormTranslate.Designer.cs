namespace NetCore.zh_hans
{
    partial class FormTranslate
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_appId = new System.Windows.Forms.TextBox();
            this.textBox_secretKey = new System.Windows.Forms.TextBox();
            this.button_Import = new System.Windows.Forms.Button();
            this.progressBar_Translate = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "百度appId:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "secretKey:";
            // 
            // textBox_appId
            // 
            this.textBox_appId.Location = new System.Drawing.Point(117, 22);
            this.textBox_appId.Name = "textBox_appId";
            this.textBox_appId.PasswordChar = '*';
            this.textBox_appId.Size = new System.Drawing.Size(234, 23);
            this.textBox_appId.TabIndex = 1;
            this.textBox_appId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_secretKey
            // 
            this.textBox_secretKey.Location = new System.Drawing.Point(117, 65);
            this.textBox_secretKey.Name = "textBox_secretKey";
            this.textBox_secretKey.PasswordChar = '*';
            this.textBox_secretKey.Size = new System.Drawing.Size(234, 23);
            this.textBox_secretKey.TabIndex = 1;
            this.textBox_secretKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Import
            // 
            this.button_Import.Location = new System.Drawing.Point(45, 148);
            this.button_Import.Name = "button_Import";
            this.button_Import.Size = new System.Drawing.Size(310, 93);
            this.button_Import.TabIndex = 2;
            this.button_Import.Text = "批量导入Xml文件 | 开始翻译";
            this.button_Import.UseVisualStyleBackColor = true;
            this.button_Import.Click += new System.EventHandler(this.button_Import_Click);
            // 
            // progressBar_Translate
            // 
            this.progressBar_Translate.Location = new System.Drawing.Point(45, 110);
            this.progressBar_Translate.Name = "progressBar_Translate";
            this.progressBar_Translate.Size = new System.Drawing.Size(306, 23);
            this.progressBar_Translate.TabIndex = 3;
            // 
            // FormTranslate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 261);
            this.Controls.Add(this.progressBar_Translate);
            this.Controls.Add(this.button_Import);
            this.Controls.Add(this.textBox_secretKey);
            this.Controls.Add(this.textBox_appId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 300);
            this.MinimumSize = new System.Drawing.Size(420, 300);
            this.Name = "FormTranslate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VS.NetCore“注释摘要\"中文翻译 zh-hans";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTranslate_FormClosing);
            this.Load += new System.EventHandler(this.FormTranslate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_appId;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox_secretKey;
        private System.Windows.Forms.Button button_in;
        private System.Windows.Forms.Button button_ImportFile;
        private System.Windows.Forms.Button button_Import;
        private System.Windows.Forms.Button button_ImportXML;
        private System.Windows.Forms.ProgressBar progressBar_;
        private System.Windows.Forms.ProgressBar progressBar_t;
        private System.Windows.Forms.ProgressBar progressBar_Translate;
    }
}

