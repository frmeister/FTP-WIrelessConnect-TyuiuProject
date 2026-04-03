using System.Net;
using System.Net.Sockets;

using UDP_BroadcastHandler;

namespace MainForm
{
    public partial class MainForm_Welcome : Form
    {
        public MainForm_Welcome()
        {
            InitializeComponent();

            onGoingHandler();
        }

        private void onGoingHandler()
        {
            while (true)
            {
                if (!UDP_BroadcastHandler.UDP_Reciever.Is_ClientReciever) main_buttonParse.Enabled = false;
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
