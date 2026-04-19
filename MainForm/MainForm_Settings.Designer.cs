namespace MainForm
{
    partial class MainForm_Settings
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
            settings_groupBoxInfo = new GroupBox();
            settings_buttonSourceData_ChangeDirectory = new Button();
            settings_textBoxSourceData_Info = new TextBox();
            settings_textBoxSourceData_Output = new TextBox();
            settings_textBoxNickname_Input = new TextBox();
            settings_textBoxNickname_Info = new TextBox();
            settings_buttonSave = new Button();
            settings_buttonCancel = new Button();
            settings_folderBrowserDialog = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            settings_groupBoxInfo.SuspendLayout();
            SuspendLayout();
            // 
            // settings_groupBoxInfo
            // 
            settings_groupBoxInfo.Controls.Add(settings_buttonSourceData_ChangeDirectory);
            settings_groupBoxInfo.Controls.Add(settings_textBoxSourceData_Info);
            settings_groupBoxInfo.Controls.Add(settings_textBoxSourceData_Output);
            settings_groupBoxInfo.Controls.Add(settings_textBoxNickname_Input);
            settings_groupBoxInfo.Controls.Add(settings_textBoxNickname_Info);
            settings_groupBoxInfo.Location = new Point(14, 16);
            settings_groupBoxInfo.Margin = new Padding(3, 4, 3, 4);
            settings_groupBoxInfo.Name = "settings_groupBoxInfo";
            settings_groupBoxInfo.Padding = new Padding(3, 4, 3, 4);
            settings_groupBoxInfo.Size = new Size(451, 608);
            settings_groupBoxInfo.TabIndex = 0;
            settings_groupBoxInfo.TabStop = false;
            settings_groupBoxInfo.Text = "Инфомация";
            // 
            // settings_buttonSourceData_ChangeDirectory
            // 
            settings_buttonSourceData_ChangeDirectory.Location = new Point(7, 147);
            settings_buttonSourceData_ChangeDirectory.Margin = new Padding(3, 4, 3, 4);
            settings_buttonSourceData_ChangeDirectory.Name = "settings_buttonSourceData_ChangeDirectory";
            settings_buttonSourceData_ChangeDirectory.Size = new Size(114, 69);
            settings_buttonSourceData_ChangeDirectory.TabIndex = 3;
            settings_buttonSourceData_ChangeDirectory.Text = "Change";
            settings_buttonSourceData_ChangeDirectory.UseVisualStyleBackColor = true;
            settings_buttonSourceData_ChangeDirectory.Click += settings_buttonSourceData_ChangeDirectory_Click;
            // 
            // settings_textBoxSourceData_Info
            // 
            settings_textBoxSourceData_Info.Enabled = false;
            settings_textBoxSourceData_Info.Location = new Point(7, 89);
            settings_textBoxSourceData_Info.Margin = new Padding(3, 4, 3, 4);
            settings_textBoxSourceData_Info.Name = "settings_textBoxSourceData_Info";
            settings_textBoxSourceData_Info.Size = new Size(114, 27);
            settings_textBoxSourceData_Info.TabIndex = 2;
            settings_textBoxSourceData_Info.Text = "Путь к файлам:";
            // 
            // settings_textBoxSourceData_Output
            // 
            settings_textBoxSourceData_Output.Enabled = false;
            settings_textBoxSourceData_Output.Location = new Point(155, 89);
            settings_textBoxSourceData_Output.Margin = new Padding(3, 4, 3, 4);
            settings_textBoxSourceData_Output.Name = "settings_textBoxSourceData_Output";
            settings_textBoxSourceData_Output.Size = new Size(289, 27);
            settings_textBoxSourceData_Output.TabIndex = 1;
            // 
            // settings_textBoxNickname_Input
            // 
            settings_textBoxNickname_Input.Location = new Point(155, 32);
            settings_textBoxNickname_Input.Margin = new Padding(3, 4, 3, 4);
            settings_textBoxNickname_Input.Name = "settings_textBoxNickname_Input";
            settings_textBoxNickname_Input.Size = new Size(289, 27);
            settings_textBoxNickname_Input.TabIndex = 1;
            // 
            // settings_textBoxNickname_Info
            // 
            settings_textBoxNickname_Info.Enabled = false;
            settings_textBoxNickname_Info.Location = new Point(7, 29);
            settings_textBoxNickname_Info.Margin = new Padding(3, 4, 3, 4);
            settings_textBoxNickname_Info.Multiline = true;
            settings_textBoxNickname_Info.Name = "settings_textBoxNickname_Info";
            settings_textBoxNickname_Info.Size = new Size(118, 32);
            settings_textBoxNickname_Info.TabIndex = 0;
            settings_textBoxNickname_Info.Text = "Ваш псевдоним:";
            // 
            // settings_buttonSave
            // 
            settings_buttonSave.Location = new Point(287, 632);
            settings_buttonSave.Margin = new Padding(3, 4, 3, 4);
            settings_buttonSave.Name = "settings_buttonSave";
            settings_buttonSave.Size = new Size(86, 51);
            settings_buttonSave.TabIndex = 1;
            settings_buttonSave.Text = "Save";
            settings_buttonSave.UseVisualStyleBackColor = true;
            settings_buttonSave.Click += settings_buttonSave_Click;
            // 
            // settings_buttonCancel
            // 
            settings_buttonCancel.Location = new Point(379, 631);
            settings_buttonCancel.Margin = new Padding(3, 4, 3, 4);
            settings_buttonCancel.Name = "settings_buttonCancel";
            settings_buttonCancel.Size = new Size(86, 51);
            settings_buttonCancel.TabIndex = 1;
            settings_buttonCancel.Text = "Cancel";
            settings_buttonCancel.UseVisualStyleBackColor = true;
            settings_buttonCancel.Click += settings_buttonCancel_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm_Settings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(477, 695);
            ControlBox = false;
            Controls.Add(settings_buttonCancel);
            Controls.Add(settings_buttonSave);
            Controls.Add(settings_groupBoxInfo);
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(495, 742);
            MinimumSize = new Size(495, 742);
            Name = "MainForm_Settings";
            Text = "Settings";
            settings_groupBoxInfo.ResumeLayout(false);
            settings_groupBoxInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox settings_groupBoxInfo;
        private Button settings_buttonSave;
        private Button settings_buttonCancel;
        private Button settings_buttonSourceData_ChangeDirectory;
        private TextBox settings_textBoxSourceData_Info;
        private TextBox settings_textBoxSourceData_Output;
        private TextBox settings_textBoxNickname_Input;
        private TextBox settings_textBoxNickname_Info;
        private FolderBrowserDialog settings_folderBrowserDialog;
        private OpenFileDialog openFileDialog1;
    }
}