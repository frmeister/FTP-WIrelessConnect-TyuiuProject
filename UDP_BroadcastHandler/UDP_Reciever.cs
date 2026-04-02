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

        public static List<IPAddress> connected_clients = new List<IPAddress>();
        public static bool Is_ClientReciever = false;

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

                UDP_Parser.Send_message(remote, "ECHO WRLSSUPDCONNECT:KEY_123");

                UDP_Controller.GetNew_Client(remote);

                Is_ClientReciever = true;

            }

            if (msg == "ECHO WRLSSUPDCONNECT:KEY_123")
            {

                UDP_Controller.GetNew_Client(remote);
                
            }

            if (msg == "CHECK WRLSSUPDCONNECT:KEY_123")
            {

                UDP_Parser.Send_message(remote, "ECHO_CHECK WRLSSUPDCONNECT:KEY_123");

                connected_clients.Add(remote);

            }

            if (msg == "ECHO_CHECK WRLSSUPDCONNECT:KEY_123")
            {

                connected_clients.Add(remote);

            }

        }

    }
}
