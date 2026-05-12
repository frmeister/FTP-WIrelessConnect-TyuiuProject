namespace MainForm
{
    partial class MainForm_ListSelect
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
            selector_buttonOK = new Button();
            selector_buttonCancel = new Button();
            selector_checkedListBox = new CheckedListBox();
            SuspendLayout();
            // 
            // selector_buttonOK
            // 
            selector_buttonOK.Location = new Point(116, 326);
            selector_buttonOK.Name = "selector_buttonOK";
            selector_buttonOK.Size = new Size(75, 23);
            selector_buttonOK.TabIndex = 0;
            selector_buttonOK.Text = "OK";
            selector_buttonOK.UseVisualStyleBackColor = true;
            selector_buttonOK.Click += selector_buttonOK_Click;
            // 
            // selector_buttonCancel
            // 
            selector_buttonCancel.Location = new Point(197, 326);
            selector_buttonCancel.Name = "selector_buttonCancel";
            selector_buttonCancel.Size = new Size(75, 23);
            selector_buttonCancel.TabIndex = 0;
            selector_buttonCancel.Text = "Cancel";
            selector_buttonCancel.UseVisualStyleBackColor = true;
            selector_buttonCancel.Click += selector_buttonCancel_Click;
            // 
            // selector_checkedListBox
            // 
            selector_checkedListBox.CheckOnClick = true;
            selector_checkedListBox.FormattingEnabled = true;
            selector_checkedListBox.Location = new Point(12, 12);
            selector_checkedListBox.Name = "selector_checkedListBox";
            selector_checkedListBox.Size = new Size(260, 292);
            selector_checkedListBox.TabIndex = 1;
            // 
            // MainForm_ListSelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 361);
            ControlBox = false;
            Controls.Add(selector_checkedListBox);
            Controls.Add(selector_buttonCancel);
            Controls.Add(selector_buttonOK);
            MaximumSize = new Size(300, 400);
            MinimumSize = new Size(300, 400);
            Name = "MainForm_ListSelect";
            Text = "Selector";
            ResumeLayout(false);
        }

        #endregion

        private Button selector_buttonOK;
        private Button selector_buttonCancel;
        private CheckedListBox selector_checkedListBox;
    }
}