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
        public static bool Is_ClientReciever = true;
        public static bool IsRunning = true;

        public static void Listener()
        {
            int port = 8888;

            UdpClient listener = new UdpClient(port);
            listener.EnableBroadcast = true;

            Debug.WriteLine($"[DEBUG] : Слушаю UDP порт {port}...");

            while (IsRunning)
            {
                try
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = listener.Receive(ref remote);
                    string msg = Encoding.UTF8.GetString(data);

                    Debug.WriteLine($"[DEBUG] : Получено от {remote.Address}: {msg}");

                    Echo(remote.Address, msg);
                }
                catch (SocketException)
                {
                    Debug.WriteLine($"[DEBUG] : Ошибка в прослушивании... ");
                }
            }
        }

        public static void Echo(IPAddress remote, string msg)
        {


            if (msg == "HELLO WRLSSUPDCONNECT:KEY_123")
            {

                if (Is_ClientReciever)
                {
                    UDP_Parser.Send_message(remote, "ECHO WRLSSUPDCONNECT:KEY_123");
                    UDP_Controller.GetNew_Client(remote);

                    // send message (otpravitel)

                    Is_ClientReciever = false;

                    // func poluchenia
                }
            }

            if (msg == "ECHO WRLSSUPDCONNECT:KEY_123")
            {

                if (!UDP_Controller.clients.Contains(remote))
                {
                    UDP_Controller.GetNew_Client(remote);
                }

            }

            if (msg == "CHECK WRLSSUPDCONNECT:KEY_123")
            {

                UDP_Parser.Send_message(remote, "ECHO_CHECK WRLSSUPDCONNECT:KEY_123");
                lock (connected_clients)
                {
                    connected_clients.Add(remote);
                }
            }

            if (msg == "ECHO_CHECK WRLSSUPDCONNECT:KEY_123")
            {

                lock (connected_clients)
                {
                    connected_clients.Add(remote);
                }

            }

            //

            // Лог случайного пакета
            else
            {
                Debug.WriteLine($"[DEBUG] : Неизвестный пакет от {remote}: {msg}");
            }

            // priem paket "PAKET :::::"

        }

    }
}
