namespace MidiRounder
{
    partial class Form1
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
            this.go_button = new System.Windows.Forms.Button();
            this.tick_amount_input = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tick_amount_input)).BeginInit();
            this.SuspendLayout();
            // 
            // go_button
            // 
            this.go_button.Location = new System.Drawing.Point(28, 87);
            this.go_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.go_button.Name = "go_button";
            this.go_button.Size = new System.Drawing.Size(374, 36);
            this.go_button.TabIndex = 0;
            this.go_button.Text = "Go!";
            this.go_button.UseVisualStyleBackColor = true;
            this.go_button.Click += new System.EventHandler(this.go_button_Click);
            // 
            // tick_amount_input
            // 
            this.tick_amount_input.Location = new System.Drawing.Point(282, 36);
            this.tick_amount_input.Maximum = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            this.tick_amount_input.Name = "tick_amount_input";
            this.tick_amount_input.Size = new System.Drawing.Size(120, 31);
            this.tick_amount_input.TabIndex = 1;
            this.tick_amount_input.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Round to this many ticks:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Note: 64th note = 30 ticks";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 231);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tick_amount_input);
            this.Controls.Add(this.go_button);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.tick_amount_input)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button go_button;
        private System.Windows.Forms.NumericUpDown tick_amount_input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

