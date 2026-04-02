using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_BroadcastHandler
{
    public class UDP_Parser
    {
        static int port = 8888;
        IPAddress broadcast = IPAddress.Broadcast;
        static UdpClient sender = new UdpClient();

        static string message = "HELLO";

        // Тут должен быть код 

        public static void Send_message(IPAddress ip, string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);

            Debug.WriteLine($"[DEBUG] : Отправляяю {msg} на адрес {ip.ToString()}");

            sender.Send(data, data.Length, ip.ToString(), port);
        }
    }
}
