using System.Net;
using System.Net.Sockets;

using UDP_BroadcastHandler;

namespace MainForm
{
    public partial class MainForm_Welcome : Form
    {
        System.Windows.Forms.Timer uiTimer = new System.Windows.Forms.Timer();

        public MainForm_Welcome()
        {
            InitializeComponent();

            uiTimer.Interval = 500; // каждые 0.5 сек
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!UDP_Reciever.Is_ClientReciever)
            {
                main_buttonParse.Enabled = false;
            }
        }

        private void MainForm_Welcome_Load(object sender, EventArgs e)
        {

        }

        private void main_buttonParse_Click(object sender, EventArgs e)
        {
            UDP_Parser.Broadcast("HELLO WRLSSUPDCONNECT:KEY_123");

            List<IPAddress> IP_list = UDP_BroadcastHandler.UDP_Controller.clients;

            foreach (IPAddress ip in IP_list)
            {
                main_textBoxListIPs.Text += Convert.ToString(ip) + "\n";
            }

            main_textBoxListIPs.Enabled = true;
        }
    }
}
