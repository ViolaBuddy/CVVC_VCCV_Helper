namespace CVVC_VCCV_Helper
{
    partial class MainForm
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
            this.dictionary_combo = new System.Windows.Forms.ComboBox();
            this.go_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.about_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dictionary_combo
            // 
            this.dictionary_combo.FormattingEnabled = true;
            this.dictionary_combo.Location = new System.Drawing.Point(209, 41);
            this.dictionary_combo.Name = "dictionary_combo";
            this.dictionary_combo.Size = new System.Drawing.Size(541, 24);
            this.dictionary_combo.TabIndex = 0;
            // 
            // go_btn
            // 
            this.go_btn.Location = new System.Drawing.Point(544, 151);
            this.go_btn.Name = "go_btn";
            this.go_btn.Size = new System.Drawing.Size(100, 26);
            this.go_btn.TabIndex = 2;
            this.go_btn.Text = "Go";
            this.go_btn.UseVisualStyleBackColor = true;
            this.go_btn.Click += new System.EventHandler(this.go_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.Location = new System.Drawing.Point(650, 151);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(100, 26);
            this.cancel_btn.TabIndex = 3;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 100;
            this.label1.Text = "Choose reclist:";
            // 
            // about_btn
            // 
            this.about_btn.Location = new System.Drawing.Point(438, 151);
            this.about_btn.Name = "about_btn";
            this.about_btn.Size = new System.Drawing.Size(100, 26);
            this.about_btn.TabIndex = 1;
            this.about_btn.Text = "About";
            this.about_btn.UseVisualStyleBackColor = true;
            this.about_btn.Click += new System.EventHandler(this.about_btn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 237);
            this.Controls.Add(this.about_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.go_btn);
            this.Controls.Add(this.dictionary_combo);
            this.Name = "MainForm";
            this.Text = "CVVC/VCCV Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dictionary_combo;
        private System.Windows.Forms.Button go_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button about_btn;
    }
}

