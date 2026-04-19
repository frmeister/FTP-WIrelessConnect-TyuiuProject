
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
            buttonStats = new Button();
            buttonSend = new Button();
            main_buttonSettitngs = new Button();
            buttonSaveFile = new Button();
            buttonStop = new Button();
            buttonOpenFile = new Button();
            main_buttonParse = new Button();
            groupBoxStatus = new GroupBox();
            buttonRequest = new Button();
            comboBox_ListIPs = new ComboBox();
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
            groupBoxButtons.Controls.Add(buttonStats);
            groupBoxButtons.Controls.Add(buttonSend);
            groupBoxButtons.Controls.Add(main_buttonSettitngs);
            groupBoxButtons.Controls.Add(buttonSaveFile);
            groupBoxButtons.Controls.Add(buttonStop);
            groupBoxButtons.Controls.Add(buttonOpenFile);
            groupBoxButtons.Controls.Add(main_buttonParse);
            groupBoxButtons.Dock = DockStyle.Left;
            groupBoxButtons.Location = new Point(0, 0);
            groupBoxButtons.Name = "groupBoxButtons";
            groupBoxButtons.Size = new Size(176, 763);
            groupBoxButtons.TabIndex = 1;
            groupBoxButtons.TabStop = false;
            groupBoxButtons.Text = "Элементы управления";
            // 
            // buttonStats
            // 
            buttonStats.Enabled = false;
            buttonStats.Image = (Image)resources.GetObject("buttonStats.Image");
            buttonStats.Location = new Point(5, 353);
            buttonStats.Name = "buttonStats";
            buttonStats.Size = new Size(165, 95);
            buttonStats.TabIndex = 7;
            buttonStats.UseVisualStyleBackColor = true;
            buttonStats.Click += buttonStats_Click;
            // 
            // buttonSend
            // 
            buttonSend.Enabled = false;
            buttonSend.Image = (Image)resources.GetObject("buttonSend.Image");
            buttonSend.Location = new Point(5, 252);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(165, 95);
            buttonSend.TabIndex = 6;
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // main_buttonSettitngs
            // 
            main_buttonSettitngs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            main_buttonSettitngs.Image = (Image)resources.GetObject("main_buttonSettitngs.Image");
            main_buttonSettitngs.Location = new Point(5, 656);
            main_buttonSettitngs.Name = "main_buttonSettitngs";
            main_buttonSettitngs.Size = new Size(165, 95);
            main_buttonSettitngs.TabIndex = 5;
            main_buttonSettitngs.UseVisualStyleBackColor = true;
            main_buttonSettitngs.Click += main_buttonSettings_Click;
            // 
            // buttonSaveFile
            // 
            buttonSaveFile.Enabled = false;
            buttonSaveFile.Image = (Image)resources.GetObject("buttonSaveFile.Image");
            buttonSaveFile.Location = new Point(5, 453);
            buttonSaveFile.Name = "buttonSaveFile";
            buttonSaveFile.Size = new Size(165, 95);
            buttonSaveFile.TabIndex = 5;
            buttonSaveFile.UseVisualStyleBackColor = true;
            buttonSaveFile.Click += buttonSaveFile_Click;
            // 
            // buttonStop
            // 
            buttonStop.Enabled = false;
            buttonStop.Image = (Image)resources.GetObject("buttonStop.Image");
            buttonStop.Location = new Point(5, 554);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(165, 95);
            buttonStop.TabIndex = 3;
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Enabled = false;
            buttonOpenFile.Image = (Image)resources.GetObject("buttonOpenFile.Image");
            buttonOpenFile.Location = new Point(5, 151);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new Size(165, 95);
            buttonOpenFile.TabIndex = 2;
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // main_buttonParse
            // 
            main_buttonParse.Image = (Image)resources.GetObject("main_buttonParse.Image");
            main_buttonParse.Location = new Point(5, 49);
            main_buttonParse.Margin = new Padding(3, 4, 3, 4);
            main_buttonParse.Name = "main_buttonParse";
            main_buttonParse.Size = new Size(165, 95);
            main_buttonParse.TabIndex = 1;
            main_buttonParse.UseVisualStyleBackColor = true;
            main_buttonParse.Click += main_buttonParse_Click;
            // 
            // groupBoxStatus
            // 
            groupBoxStatus.Controls.Add(buttonRequest);
            groupBoxStatus.Controls.Add(comboBox_ListIPs);
            groupBoxStatus.Controls.Add(labelStatus);
            groupBoxStatus.Dock = DockStyle.Top;
            groupBoxStatus.Location = new Point(176, 0);
            groupBoxStatus.Name = "groupBoxStatus";
            groupBoxStatus.Size = new Size(1195, 67);
            groupBoxStatus.TabIndex = 3;
            groupBoxStatus.TabStop = false;
            groupBoxStatus.Text = "Статус:";
            // 
            // buttonRequest
            // 
            buttonRequest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRequest.Enabled = false;
            buttonRequest.Location = new Point(875, 28);
            buttonRequest.Margin = new Padding(3, 4, 3, 4);
            buttonRequest.Name = "buttonRequest";
            buttonRequest.Size = new Size(86, 31);
            buttonRequest.TabIndex = 4;
            buttonRequest.Text = "Запрос";
            buttonRequest.UseVisualStyleBackColor = true;
            buttonRequest.Click += buttonRequest_Click;
            // 
            // comboBox_ListIPs
            // 
            comboBox_ListIPs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox_ListIPs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ListIPs.FormattingEnabled = true;
            comboBox_ListIPs.Location = new Point(968, 28);
            comboBox_ListIPs.Margin = new Padding(3, 4, 3, 4);
            comboBox_ListIPs.Name = "comboBox_ListIPs";
            comboBox_ListIPs.Size = new Size(220, 28);
            comboBox_ListIPs.TabIndex = 4;
            comboBox_ListIPs.SelectedIndexChanged += comboBox_ListIPs_SelectedIndexChanged;
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
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(textBoxReceivedContent);
            groupBoxData.Dock = DockStyle.Fill;
            groupBoxData.Location = new Point(176, 67);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(1195, 696);
            groupBoxData.TabIndex = 2;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Полученные данные:";
            // 
            // textBoxReceivedContent
            // 
            textBoxReceivedContent.Dock = DockStyle.Fill;
            textBoxReceivedContent.Location = new Point(3, 23);
            textBoxReceivedContent.Multiline = true;
            textBoxReceivedContent.Name = "textBoxReceivedContent";
            textBoxReceivedContent.ScrollBars = ScrollBars.Both;
            textBoxReceivedContent.Size = new Size(1189, 670);
            textBoxReceivedContent.TabIndex = 0;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // MainForm_Welcome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 763);
            Controls.Add(groupBoxData);
            Controls.Add(groupBoxStatus);
            Controls.Add(groupBoxButtons);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1387, 796);
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
        private ComboBox comboBox_ListIPs;
        private Button main_buttonSettitngs;
        private Button buttonStats;
        private Button buttonRequest;
    }
}
