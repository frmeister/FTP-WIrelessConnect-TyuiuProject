using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_BroadcastHandler
{
    public class NetworkParser
    {
        static int port = 8888;
        IPAddress broadcast = IPAddress.Broadcast;
        static UdpClient sender = new UdpClient(); // 255.255.255.255 <- IP.Sender

        public static void Broadcast(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);

            sender.EnableBroadcast = true;

            sender.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));
        }

        public static void Send_message(IPAddress ip, string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            sender.EnableBroadcast = false;

            Debug.WriteLine($"[DEBUG] : Отправляяю {msg} на адрес {ip.ToString()}");

            sender.Send(data, data.Length, ip.ToString(), port);
        }
    }
}
