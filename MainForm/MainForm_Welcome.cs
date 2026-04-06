using System.Net;
using System.Net.Sockets;
using UDP_BroadcastHandler;
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

        public MainForm_Welcome()
        {
            InitializeComponent();
            fileSender = new FileSender();
            fileReceiver = new FileReceiver();
            receiveTask = Task.Run(() => fileReceiver.StartReceiving(8889));

            uiTimer.Interval = 500;
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            openFileDialog.Filter = "Все файлы (*.*)|*.*";
            saveFileDialog.Filter = "Все файлы (*.*)|*.*";
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!UDP_Reciever.Is_ClientReciever)
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

        private void MainForm_Welcome_Load(object sender, EventArgs e) { }

        private void main_buttonParse_Click(object sender, EventArgs e)
        {
            main_textBoxListIPs.Clear();
            UDP_Parser.Broadcast("HELLO WRLSSUPDCONNECT:KEY_123");

            Task.Delay(500).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    List<IPAddress> IP_list = UDP_BroadcastHandler.UDP_Controller.clients;

                    if (IP_list.Count == 0)
                    {
                        main_textBoxListIPs.Text = "Клиенты не найдены";
                        labelStatus.Text = "Клиенты не найдены. Проверьте подключение.";
                    }
                    else
                    {
                        foreach (IPAddress ip in IP_list)
                        {
                            main_textBoxListIPs.AppendText(ip.ToString() + Environment.NewLine);
                        }
                        labelStatus.Text = $"Найдено {IP_list.Count} клиент(ов)";
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void main_textBoxListIPs_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            if (!fileReceiver.IsComplete)
            {
                MessageBox.Show("Файл еще не полностью получен!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show($"Файл успешно сохранен!\nПуть: {path}",
                        "Сохранение завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonSaveFile.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении файла!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(openFilePath))
            {
                MessageBox.Show("Выберите файл для отправки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(labelStatus.Text))
            {
                MessageBox.Show("Нет доступных IP. Нажмите 'Поиск'", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedIp = "";
            selectedIp = main_textBoxListIPs.SelectedText.Trim();
            
            IPAddress targetIp = IPAddress.Parse(selectedIp);
            labelStatus.Text = "Отправка...";

            bool success = await fileSender.SendFile(openFilePath, targetIp, 8889);

            if (success)
            {
                labelStatus.Text = "Файл отправлен!";
                MessageBox.Show("Файл успешно отправлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "Ошибка отправки";
                MessageBox.Show("Ошибка отправки файла", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            textBoxReceivedContent.Clear();
            labelStatus.Text = "";
            fileReceiver.ResetReceiver();
            main_textBoxListIPs.Clear();
            UDP_Reciever.Is_ClientReciever = true;
            main_buttonParse.Enabled = true;
        }
    }
}
    