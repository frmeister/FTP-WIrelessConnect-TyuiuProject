using System.Net;
using System.Net.Sockets;
using NetworkHandler;
using FilePacket;

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

        // DOP
        private string nickName = ConfigManager.GetValue("nickName");
        public string nickName_Recived = NetworkResponser.nickName_Recieved;

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

            this.Resize += FormMain_Resize;

            openFileDialog.Filter = "Все файлы (*.*)|*.*";
            saveFileDialog.Filter = "Все файлы (*.*)|*.*";
        }

        #region ELEMENTS

        private void main_buttonParse_Click(object sender, EventArgs e)
        {
            NetworkParser.Broadcast($"HELLO WRLSCONNECT_123 {nickName}");

            Task.Delay(500).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    List<IPAddress> IP_list = NetworkHandler.NetworkController.clients;

                    if (IP_list.Count == 0)
                    {
                        labelStatus.Text = "Пользователи не найдены. Проверьте подключение.";
                    }
                    else
                    {
                        comboBox_ListIPs.Items.Clear();

                        foreach (IPAddress ip in IP_list)
                        {
                            comboBox_ListIPs.Items.Add(ip.ToString());

                        }

                        comboBox_ListIPs.Enabled = true;
                        labelStatus.Text = $"Найдено {IP_list.Count} пользователей";
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
                    labelStatus.Text = $"Выбран файл: {Path.GetFileName(openFilePath)}";
                    buttonSend.Enabled = true;
                    buttonSaveFile.Enabled = true;
                    buttonStats.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            IPAddress targetIp = IPAddress.Parse(selectedIP);
            labelStatus.Text = "Отправка...";

            bool success = await fileSender.SendFile(openFilePath, targetIp, 8889);

            if (success)
            {
                labelStatus.Text = "Файл отправлен!"; MessageBox.Show("Файл успешно отправлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "Ошибка отправки"; MessageBox.Show("Ошибка отправки файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            if (!fileReceiver.IsComplete)
            {
                MessageBox.Show("Файл еще не полностью получен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    labelStatus.Text = $"Файл сохранен: {Path.GetFileName(path)}";
                    MessageBox.Show($"Файл успешно сохранен!\nПуть: {path}", "Сохранение завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonSaveFile.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении файла!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            textBoxReceivedContent.Clear();
            labelStatus.Text = "";
            fileReceiver.ResetReceiver();
            NetworkReciever.Is_ClientReciever = true;
            main_buttonParse.Enabled = true;
        }

        private void main_buttonSettings_Click(object sender, EventArgs e)
        {
            MainForm_Settings form_Settings = new MainForm_Settings();
            form_Settings.ShowDialog();
        }

        #endregion

        #region EVENTS

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!NetworkReciever.Is_ClientReciever)
            {
                main_buttonParse.Enabled = false;
            }
            if (fileReceiver.ReceivedContent.Length > 0)
            {
                textBoxReceivedContent.Text = fileReceiver.ReceivedContent.ToString();
                if (fileReceiver.IsComplete)
                {
                    labelStatus.Text = $"Файл {fileReceiver.CurrentFileName} получен! ({fileReceiver.CurrentPacket}/{fileReceiver.TotalPackets} пакетов)";
                    buttonSaveFile.Enabled = true;
                }
                else
                {
                    labelStatus.Text = $"Получение: {fileReceiver.CurrentPacket}/{fileReceiver.TotalPackets} пакетов";
                    buttonSaveFile.Enabled = false;
                }
            }
        }

        private void comboBox_ListIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = comboBox_ListIPs.SelectedItem.ToString();
            selectedIP = selectedState;


            buttonOpenFile.Enabled = true;
            buttonStop.Enabled = true;
            // buttonSaveFile.Enabled = true; Понять что делать после решения проблемы (1), пока стандарт = true
        }
        private void buttonStats_Click(object sender, EventArgs e)
        {
            string receivedData = textBoxReceivedContent.Text;
            FormStats statsForm = new FormStats();
            statsForm.ReceiveData(receivedData);
            statsForm.Show();
        }


        private void comboBox_ListIPs_onInit()
        {
            comboBox_ListIPs.Items.Clear();
            comboBox_ListIPs.Items.Add("Клинтов нет");
            comboBox_ListIPs.Enabled = false;

        }

        #endregion

        #region TRAY

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        private void InitializeTrayIcon()
        {
            // Создаем контекстное меню для трея
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Open", null, OnTrayRestore);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Exit", null, OnTrayExit);

            // Создаем иконку в трее
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application, // Можно заменить на свою иконку
                Text = "ProgramConnectionPrikol",
                ContextMenuStrip = trayMenu,
                Visible = false // Изначально скрыта
            };

            // Обработка кликов по иконке
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
            this.Activate(); // Активируем окно
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

            // !!! ДОБАВИТЬ ПРОВЕРКУ НА РАБОТУ ПРОЦЕССОВ ПЕРЕД ЗАКРЫТИЕМ !!!
            // !!! ИНАЧЕ ПИЗДА И ОШИБКА ВСЕМУ НАХУЙ !!!

            // Корректно закрываем приложение
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            // Сворачиваем в трей при нажатии на кнопку "минус"
            if (this.WindowState == FormWindowState.Minimized)
            {
                MinimizeToTray();
            }
        }

        #endregion



        
    }
}