using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_BroadcastHandler
{
    public class UDP_Reciever
    {
        // Debug.WriteLine($"[DEBUG] : ///);
        public static void Listener()
        {
            int port = 8888;

            UdpClient listener = new UdpClient(port);
            listener.EnableBroadcast = true;

            Debug.WriteLine($"[DEBUG] : Слушаю UDP порт {port}...");

            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = listener.Receive(ref remote);
                string msg = Encoding.UTF8.GetString(data);

                Debug.WriteLine($"[DEBUG] : Получено от {remote.Address}: {msg}");

                Echo(remote.Address, msg);
            }
        }

        public static void Echo(IPAddress remote, string msg)
        {


            if (msg == "HELLO WRLSSUPDCONNECT:KEY_123")
            {

                

            }

        }

    }
}
