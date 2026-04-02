namespace MainForm
{
    partial class MainForm_Welcome
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
            main_textBoxListIPs = new TextBox();
            main_buttonParse = new Button();
            SuspendLayout();
            // 
            // main_textBoxListIPs
            // 
            main_textBoxListIPs.Enabled = false;
            main_textBoxListIPs.Location = new Point(12, 12);
            main_textBoxListIPs.Multiline = true;
            main_textBoxListIPs.Name = "main_textBoxListIPs";
            main_textBoxListIPs.Size = new Size(457, 412);
            main_textBoxListIPs.TabIndex = 0;
            // 
            // main_buttonParse
            // 
            main_buttonParse.Location = new Point(484, 12);
            main_buttonParse.Name = "main_buttonParse";
            main_buttonParse.Size = new Size(304, 100);
            main_buttonParse.TabIndex = 1;
            main_buttonParse.Text = "Parse";
            main_buttonParse.UseVisualStyleBackColor = true;
            main_buttonParse.Click += main_buttonParse_Click;
            // 
            // MainForm_Welcome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 437);
            Controls.Add(main_buttonParse);
            Controls.Add(main_textBoxListIPs);
            Name = "MainForm_Welcome";
            Text = "Welcome";
            Load += MainForm_Welcome_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox main_textBoxListIPs;
        private Button main_buttonParse;
    }
}
