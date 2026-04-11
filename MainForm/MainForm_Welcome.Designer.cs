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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm_Welcome));
            groupBoxButtons = new GroupBox();
            buttonSend = new Button();
            buttonSaveFile = new Button();
            buttonStop = new Button();
            buttonOpenFile = new Button();
            main_buttonParse = new Button();
            groupBoxStatus = new GroupBox();
            comboBox_ListIPs = new ComboBox();
            main_textBoxListIPs = new TextBox();
            labelStatus = new TextBox();
            groupBoxData = new GroupBox();
            textBoxReceivedContent = new TextBox();
            toolTip = new ToolTip(components);
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            groupBoxButtons.SuspendLayout();
            groupBoxStatus.SuspendLayout();
            groupBoxData.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxButtons
            // 
            groupBoxButtons.Controls.Add(buttonSend);
            groupBoxButtons.Controls.Add(buttonSaveFile);
            groupBoxButtons.Controls.Add(buttonStop);
            groupBoxButtons.Controls.Add(buttonOpenFile);
            groupBoxButtons.Controls.Add(main_buttonParse);
            groupBoxButtons.Dock = DockStyle.Left;
            groupBoxButtons.Location = new Point(0, 0);
            groupBoxButtons.Margin = new Padding(3, 2, 3, 2);
            groupBoxButtons.Name = "groupBoxButtons";
            groupBoxButtons.Padding = new Padding(3, 2, 3, 2);
            groupBoxButtons.Size = new Size(154, 572);
            groupBoxButtons.TabIndex = 1;
            groupBoxButtons.TabStop = false;
            groupBoxButtons.Text = "groupBoxButtons";
            // 
            // buttonSend
            // 
            buttonSend.Enabled = false;
            buttonSend.Image = (Image)resources.GetObject("buttonSend.Image");
            buttonSend.Location = new Point(5, 181);
            buttonSend.Margin = new Padding(3, 2, 3, 2);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(144, 71);
            buttonSend.TabIndex = 6;
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // buttonSaveFile
            // 
            buttonSaveFile.Image = (Image)resources.GetObject("buttonSaveFile.Image");
            buttonSaveFile.Location = new Point(5, 256);
            buttonSaveFile.Margin = new Padding(3, 2, 3, 2);
            buttonSaveFile.Name = "buttonSaveFile";
            buttonSaveFile.Size = new Size(144, 71);
            buttonSaveFile.TabIndex = 5;
            buttonSaveFile.UseVisualStyleBackColor = true;
            buttonSaveFile.Click += buttonSaveFile_Click;
            // 
            // buttonStop
            // 
            buttonStop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonStop.Image = (Image)resources.GetObject("buttonStop.Image");
            buttonStop.Location = new Point(5, 491);
            buttonStop.Margin = new Padding(3, 2, 3, 2);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(144, 71);
            buttonStop.TabIndex = 3;
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Enabled = false;
            buttonOpenFile.Image = (Image)resources.GetObject("buttonOpenFile.Image");
            buttonOpenFile.Location = new Point(5, 105);
            buttonOpenFile.Margin = new Padding(3, 2, 3, 2);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new Size(144, 71);
            buttonOpenFile.TabIndex = 2;
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // main_buttonParse
            // 
            main_buttonParse.Image = (Image)resources.GetObject("main_buttonParse.Image");
            main_buttonParse.Location = new Point(5, 28);
            main_buttonParse.Name = "main_buttonParse";
            main_buttonParse.Size = new Size(144, 71);
            main_buttonParse.TabIndex = 1;
            main_buttonParse.UseVisualStyleBackColor = true;
            main_buttonParse.Click += main_buttonParse_Click;
            // 
            // groupBoxStatus
            // 
            groupBoxStatus.Controls.Add(comboBox_ListIPs);
            groupBoxStatus.Controls.Add(main_textBoxListIPs);
            groupBoxStatus.Controls.Add(labelStatus);
            groupBoxStatus.Dock = DockStyle.Top;
            groupBoxStatus.Location = new Point(154, 0);
            groupBoxStatus.Margin = new Padding(3, 2, 3, 2);
            groupBoxStatus.Name = "groupBoxStatus";
            groupBoxStatus.Padding = new Padding(3, 2, 3, 2);
            groupBoxStatus.Size = new Size(1046, 50);
            groupBoxStatus.TabIndex = 3;
            groupBoxStatus.TabStop = false;
            groupBoxStatus.Text = "groupBoxStatus";
            // 
            // comboBox_ListIPs
            // 
            comboBox_ListIPs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ListIPs.FormattingEnabled = true;
            comboBox_ListIPs.Location = new Point(847, 17);
            comboBox_ListIPs.Name = "comboBox_ListIPs";
            comboBox_ListIPs.Size = new Size(193, 23);
            comboBox_ListIPs.TabIndex = 4;
            comboBox_ListIPs.SelectedIndexChanged += comboBox_ListIPs_SelectedIndexChanged;
            // 
            // main_textBoxListIPs
            // 
            main_textBoxListIPs.Dock = DockStyle.Right;
            main_textBoxListIPs.Location = new Point(847, 18);
            main_textBoxListIPs.Margin = new Padding(3, 2, 3, 2);
            main_textBoxListIPs.Name = "main_textBoxListIPs";
            main_textBoxListIPs.Size = new Size(196, 23);
            main_textBoxListIPs.TabIndex = 7;
            main_textBoxListIPs.Visible = false;
            // 
            // labelStatus
            // 
            labelStatus.Enabled = false;
            labelStatus.Location = new Point(3, 17);
            labelStatus.Multiline = true;
            labelStatus.Name = "labelStatus";
            labelStatus.ReadOnly = true;
            labelStatus.Size = new Size(1041, 32);
            labelStatus.TabIndex = 0;
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(textBoxReceivedContent);
            groupBoxData.Dock = DockStyle.Fill;
            groupBoxData.Location = new Point(154, 50);
            groupBoxData.Margin = new Padding(3, 2, 3, 2);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Padding = new Padding(3, 2, 3, 2);
            groupBoxData.Size = new Size(1046, 522);
            groupBoxData.TabIndex = 2;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "groupBoxData";
            // 
            // textBoxReceivedContent
            // 
            textBoxReceivedContent.Dock = DockStyle.Fill;
            textBoxReceivedContent.Location = new Point(3, 18);
            textBoxReceivedContent.Margin = new Padding(3, 2, 3, 2);
            textBoxReceivedContent.Multiline = true;
            textBoxReceivedContent.Name = "textBoxReceivedContent";
            textBoxReceivedContent.ScrollBars = ScrollBars.Both;
            textBoxReceivedContent.Size = new Size(1040, 502);
            textBoxReceivedContent.TabIndex = 0;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // MainForm_Welcome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 572);
            Controls.Add(groupBoxData);
            Controls.Add(groupBoxStatus);
            Controls.Add(groupBoxButtons);
            MinimumSize = new Size(1216, 611);
            Name = "MainForm_Welcome";
            Text = "Welcome";
            groupBoxButtons.ResumeLayout(false);
            groupBoxStatus.ResumeLayout(false);
            groupBoxStatus.PerformLayout();
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBoxData;
        private Button main_buttonParse;
        private TextBox labelStatus;
        private GroupBox groupBoxButtons;
        private GroupBox groupBoxStatus;
        private Button buttonStop;
        private Button buttonOpenFile;
        private TextBox textBoxReceivedContent;
        private Button buttonSaveFile;
        private ToolTip toolTip;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Button buttonSend;
        private TextBox main_textBoxListIPs;
        private ComboBox comboBox_ListIPs;
    }
}
