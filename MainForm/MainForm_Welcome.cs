using FilePacket;
using NetworkHandler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace MainForm
{
    public partial class MainForm_Welcome : Form
    {
        System.Windows.Forms.Timer uiTimer = new System.Windows.Forms.Timer();

        public static string openFilePath;
        private FileSender fileSender;
        private FileReceiver fileReceiver;
        private Task receiveTask;
        private string selectedIP;
        private string KEY;
        private string nickName = ConfigManager.GetValue("nickName");
        public string nickName_Recived = NetworkResponser.nickName_Recieved;

        private List<Tuple<string, string>> receivedFilesHistory = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> receivedFiles = new List<Tuple<string, string>>();
        private string lastReceivedContent = "";
        private string currentReceivingFile = "";

        public MainForm_Welcome()
        {
            InitializeComponent();
            InitializeTrayIcon();
            comboBox_ListIPs_onInit();

            fileSender = new FileSender();
            fileReceiver = new FileReceiver();
            receiveTask = Task.Run(() => fileReceiver.StartReceiving(8889));

            uiTimer.Interval = 500;
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            NetworkResponser.FileListReceived += NetworkResponser_FileListReceived;
            this.Resize += FormMain_Resize;

            openFileDialog.Filter = "˜˜˜ ˜˜˜˜˜ (*.*)|*.*";
            saveFileDialog.Filter = "˜˜˜ ˜˜˜˜˜ (*.*)|*.*";

            KEY = ConfigManager.GetValue("appKey");
        }

        

        #region ELEMENTS

        private void main_buttonParse_Click(object sender, EventArgs e)
        {
            NetworkParser.Broadcast($"HELLO {KEY} {nickName}");

            Task.Delay(500).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    List<IPAddress> IP_list = NetworkHandler.NetworkController.clients;

                    if (IP_list.Count == 0)
                    {
                        labelStatus.Text = "˜˜˜˜˜˜˜˜˜˜˜˜ ˜˜ ˜˜˜˜˜˜˜. ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜˜.";
                    }
                    else
                    {
                        comboBox_ListIPs.Items.Clear();

                        foreach (IPAddress ip in IP_list)
                        {
                            string ip_name = NetworkController.dClients.FirstOrDefault(x => x.Value == ip).Key;
                            comboBox_ListIPs.Items.Add(ip_name);
                        }

                        comboBox_ListIPs.Enabled = true;
                        labelStatus.Text = $"˜˜˜˜˜˜˜ {IP_list.Count} ˜˜˜˜˜˜˜˜˜˜˜˜˜";
                    }
                }));
            });
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                openFilePath = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(openFilePath))
                {
                    labelStatus.Text = $"˜˜˜˜˜˜ ˜˜˜˜: {Path.GetFileName(openFilePath)}";
                    buttonSend.Enabled = true;
                    buttonSaveFile.Enabled = true;
                    buttonStats.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("˜˜˜˜˜˜ ", "˜˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            IPAddress targetIp = IPAddress.Parse(selectedIP);
            labelStatus.Text = "˜˜˜˜˜˜˜˜...";

            bool success = await fileSender.SendFile(openFilePath, targetIp, 8889);

            if (success)
            {
                labelStatus.Text = "˜˜˜˜ ˜˜˜˜˜˜˜˜˜!"; MessageBox.Show("˜˜˜˜ ˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜!", "˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜"; MessageBox.Show("˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜ ˜˜˜˜˜", "˜˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            if (!fileReceiver.IsComplete)
            {
                MessageBox.Show("˜˜˜˜ ˜˜˜ ˜˜ ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜!", "˜˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            saveFileDialog.FileName = fileReceiver.CurrentFileName;
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;

                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    File.Delete(path);
                }

                bool success = fileReceiver.SaveFile(path);
                if (success)
                {
                    labelStatus.Text = $"˜˜˜˜ ˜˜˜˜˜˜˜˜: {Path.GetFileName(path)}";
                    MessageBox.Show($"˜˜˜˜ ˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜!\n˜˜˜˜: {path}", "˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonSaveFile.Enabled = false;
                }
                else
                {
                    MessageBox.Show("˜˜˜˜˜˜ ˜˜˜ ˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜!", "˜˜˜˜˜˜", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            textBoxReceivedContent.Clear();
            labelStatus.Text = "";
            fileReceiver.ResetReceiver();
            NetworkReceiver.Is_ClientReciever = true;
            main_buttonParse.Enabled = true;
            receivedFilesHistory.Clear();
            lastReceivedContent = "";
            currentReceivingFile = "";
            comboBox_ListIPs.Items.Clear();
            comboBox_ListIPs.Items.Add("˜˜˜˜˜˜˜ ˜˜˜");
            comboBox_ListIPs.Enabled = false;
            buttonSaveFile.Enabled = false;
            buttonStats.Enabled = false;
            labelStatus.Text = "˜˜˜ ˜˜˜˜˜˜ ˜˜˜˜˜˜˜";

        }

        private void main_buttonSettings_Click(object sender, EventArgs e)
        {
            MainForm_Settings form_Settings = new MainForm_Settings();
            form_Settings.ShowDialog();
        }

        private void buttonRequest_Click(object sender, EventArgs e)
        {
            NetworkParser.Send_message(IPAddress.Parse(selectedIP), $"ASK_SEND {KEY} {nickName}");

            buttonStats.Enabled = true;
        }

        #endregion

        #region EVENTS

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!NetworkReceiver.Is_ClientReciever)
            {
                main_buttonParse.Enabled = false;
            }
            if (fileReceiver.ReceivedContent.Length > 0)
            {
                string currentContent = fileReceiver.ReceivedContent.ToString();
                if (currentContent != lastReceivedContent)
                {
                    if (fileReceiver.IsComplete && textBoxReceivedContent.Text.Length > 0)
                    {
                        textBoxReceivedContent.AppendText(Environment.NewLine);
                        textBoxReceivedContent.AppendText(new string('=', 80));
                        textBoxReceivedContent.AppendText(Environment.NewLine);
                    }

                    textBoxReceivedContent.AppendText(currentContent);
                    textBoxReceivedContent.SelectionStart = textBoxReceivedContent.Text.Length;
                    textBoxReceivedContent.ScrollToCaret();
                    lastReceivedContent = currentContent;
                }

                var completedFiles = fileReceiver.DequeueCompletedFiles();
                foreach (var completed in completedFiles)
                {
                    bool alreadySaved = receivedFilesHistory.Any(f => f.Item1 == completed.Item1 && f.Item2 == completed.Item2);
                    if (!alreadySaved)
                    {
                        receivedFilesHistory.Add(completed);
                        Debug.WriteLine($"[UI] ˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜ ˜ ˜˜˜˜˜˜˜: {completed.Item1}");
                    }
                }

                labelStatus.Text = fileReceiver.IsComplete
                    ? $"˜˜˜˜ {fileReceiver.CurrentFileName} ˜˜˜˜˜˜˜! ({fileReceiver.CurrentPacket}/{fileReceiver.TotalPackets} ˜˜˜˜˜˜˜)"
                    : $"˜˜˜˜˜˜˜˜˜: {fileReceiver.CurrentPacket}/{fileReceiver.TotalPackets} ˜˜˜˜˜˜˜";

                buttonSaveFile.Enabled = fileReceiver.IsComplete;
            }
        }


        private void comboBox_ListIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = comboBox_ListIPs.SelectedItem.ToString();
            if (NetworkController.dClients.TryGetValue(selectedState, out IPAddress value))
            {
                selectedState = value.ToString();
                selectedIP = selectedState;
            }

            else return;

            buttonOpenFile.Enabled = true;
            buttonStop.Enabled = true;
            buttonRequest.Enabled = true;
            // buttonSaveFile.Enabled = true; ˜˜˜˜˜˜ ˜˜˜ ˜˜˜˜˜˜ ˜˜˜˜˜ ˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜ (1), ˜˜˜˜ ˜˜˜˜˜˜˜˜ = true
        }
        private void buttonStats_Click(object sender, EventArgs e)
        {
            FormStats statsForm = new FormStats();
            foreach (var file in receivedFilesHistory)
                statsForm.AddFileData(file.Item2, file.Item1);
            statsForm.Show();
        }


        private void comboBox_ListIPs_onInit()
        {
            comboBox_ListIPs.Items.Clear();
            comboBox_ListIPs.Items.Add("˜˜˜˜˜˜˜ ˜˜˜");
            comboBox_ListIPs.Enabled = false;

        }

        private void NetworkResponser_FileListReceived(IPAddress fromIp, List<string> availableFiles)
        {
            if (InvokeRequired)
            {
                Invoke(() => NetworkResponser_FileListReceived(fromIp, availableFiles));
                return;
            }

            // ˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜ ˜˜˜˜˜˜ ˜˜˜˜˜˜
            using (var selectForm = new MainForm_ListSelect(availableFiles))
            {
                DialogResult res = selectForm.ShowDialog();

                if (res == DialogResult.OK)
                {
                    var selected = selectForm.SelectedFiles;
                    if (selected.Count == 0) return;

                    foreach (string fileName in selected)
                    {
                        NetworkParser.Send_message(fromIp, $"REQUEST_FILE {KEY} {nickName} {fileName}");
                    }
                    Debug.WriteLine($"˜˜˜˜˜˜˜˜˜ {selected.Count} ˜˜˜˜˜˜ ˜ {fromIp}");
                }
                else if (res == DialogResult.Cancel) NetworkParser.Send_message(fromIp, $"ECHO_ASK_SEND {KEY} {nickName} CODE_2");
            }
        }

        #endregion

        #region TRAY

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        private void InitializeTrayIcon()
        {
            // ˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜ ˜˜˜ ˜˜˜˜
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Open", null, OnTrayRestore);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Exit", null, OnTrayExit);

            // ˜˜˜˜˜˜˜ ˜˜˜˜˜˜ ˜ ˜˜˜˜
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application, // ˜˜˜˜˜ ˜˜˜˜˜˜˜˜ ˜˜ ˜˜˜˜ ˜˜˜˜˜˜
                Text = "ProgramConnectionPrikol",
                ContextMenuStrip = trayMenu,
                Visible = false // ˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜
            };

            // ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜ ˜˜ ˜˜˜˜˜˜
            trayIcon.DoubleClick += (s, e) => RestoreFromTray();
            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    RestoreFromTray();
                }
            };
        }

        private void RestoreFromTray()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            trayIcon.Visible = false;
            this.Activate(); // ˜˜˜˜˜˜˜˜˜˜ ˜˜˜˜
        }

        private void MinimizeToTray()
        {
            this.Hide();
            this.ShowInTaskbar = false;
            trayIcon.Visible = true;
        }

        private void OnTrayRestore(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private bool isExitingFromTray = false;

        private async void OnTrayExit(object sender, EventArgs e)
        {
            isExitingFromTray = true;

            // !!! ˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜ ˜˜ ˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜ !!!
            // !!! ˜˜˜˜˜ ˜˜˜˜˜ ˜ ˜˜˜˜˜˜ ˜˜˜˜˜ ˜˜˜˜˜ !!!

            // ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            // ˜˜˜˜˜˜˜˜˜˜˜ ˜ ˜˜˜˜ ˜˜˜ ˜˜˜˜˜˜˜ ˜˜ ˜˜˜˜˜˜ "˜˜˜˜˜"
            if (this.WindowState == FormWindowState.Minimized)
            {
                MinimizeToTray();
            }
        }

        #endregion

    }
}