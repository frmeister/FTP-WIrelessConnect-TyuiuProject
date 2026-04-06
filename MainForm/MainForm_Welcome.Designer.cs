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
            groupBoxButtons.Name = "groupBoxButtons";
            groupBoxButtons.Size = new Size(176, 762);
            groupBoxButtons.TabIndex = 1;
            groupBoxButtons.TabStop = false;
            groupBoxButtons.Text = "groupBoxButtons";
            // 
            // buttonSend
            // 
            buttonSend.Image = (Image)resources.GetObject("buttonSend.Image");
            buttonSend.Location = new Point(6, 241);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(164, 95);
            buttonSend.TabIndex = 6;
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // buttonSaveFile
            // 
            buttonSaveFile.Image = (Image)resources.GetObject("buttonSaveFile.Image");
            buttonSaveFile.Location = new Point(6, 342);
            buttonSaveFile.Name = "buttonSaveFile";
            buttonSaveFile.Size = new Size(164, 95);
            buttonSaveFile.TabIndex = 5;
            buttonSaveFile.UseVisualStyleBackColor = true;
            buttonSaveFile.Click += buttonSaveFile_Click;
            // 
            // buttonStop
            // 
            buttonStop.Image = (Image)resources.GetObject("buttonStop.Image");
            buttonStop.Location = new Point(6, 655);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(164, 95);
            buttonStop.TabIndex = 3;
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Image = (Image)resources.GetObject("buttonOpenFile.Image");
            buttonOpenFile.Location = new Point(6, 140);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new Size(164, 95);
            buttonOpenFile.TabIndex = 2;
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // main_buttonParse
            // 
            main_buttonParse.Image = (Image)resources.GetObject("main_buttonParse.Image");
            main_buttonParse.Location = new Point(6, 38);
            main_buttonParse.Margin = new Padding(3, 4, 3, 4);
            main_buttonParse.Name = "main_buttonParse";
            main_buttonParse.Size = new Size(164, 95);
            main_buttonParse.TabIndex = 1;
            main_buttonParse.UseVisualStyleBackColor = true;
            main_buttonParse.Click += main_buttonParse_Click;
            // 
            // groupBoxStatus
            // 
            groupBoxStatus.Controls.Add(main_textBoxListIPs);
            groupBoxStatus.Controls.Add(labelStatus);
            groupBoxStatus.Dock = DockStyle.Top;
            groupBoxStatus.Location = new Point(176, 0);
            groupBoxStatus.Name = "groupBoxStatus";
            groupBoxStatus.Size = new Size(1195, 67);
            groupBoxStatus.TabIndex = 3;
            groupBoxStatus.TabStop = false;
            groupBoxStatus.Text = "groupBoxStatus";
            // 
            // main_textBoxListIPs
            // 
            main_textBoxListIPs.Dock = DockStyle.Right;
            main_textBoxListIPs.Location = new Point(969, 23);
            main_textBoxListIPs.Name = "main_textBoxListIPs";
            main_textBoxListIPs.Size = new Size(223, 27);
            main_textBoxListIPs.TabIndex = 7;
            // 
            // labelStatus
            // 
            labelStatus.Enabled = false;
            labelStatus.Location = new Point(3, 23);
            labelStatus.Margin = new Padding(3, 4, 3, 4);
            labelStatus.Multiline = true;
            labelStatus.Name = "labelStatus";
            labelStatus.ReadOnly = true;
            labelStatus.Size = new Size(1189, 41);
            labelStatus.TabIndex = 0;
            labelStatus.TextChanged += main_textBoxListIPs_TextChanged;
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(textBoxReceivedContent);
            groupBoxData.Dock = DockStyle.Fill;
            groupBoxData.Location = new Point(176, 67);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(1195, 695);
            groupBoxData.TabIndex = 2;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "groupBoxData";
            // 
            // textBoxReceivedContent
            // 
            textBoxReceivedContent.Dock = DockStyle.Fill;
            textBoxReceivedContent.Location = new Point(3, 23);
            textBoxReceivedContent.Multiline = true;
            textBoxReceivedContent.Name = "textBoxReceivedContent";
            textBoxReceivedContent.ScrollBars = ScrollBars.Both;
            textBoxReceivedContent.Size = new Size(1189, 669);
            textBoxReceivedContent.TabIndex = 0;
            textBoxReceivedContent.TextChanged += textBox1_TextChanged;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // MainForm_Welcome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 762);
            Controls.Add(groupBoxData);
            Controls.Add(groupBoxStatus);
            Controls.Add(groupBoxButtons);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm_Welcome";
            Text = "Welcome";
            Load += MainForm_Welcome_Load;
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
    }
}
