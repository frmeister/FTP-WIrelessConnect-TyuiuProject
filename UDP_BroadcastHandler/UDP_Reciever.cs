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
        private static readonly HashSet<IPAddress> _localAddresses = new();

        public static bool Is_ClientReciever = true;
        public static bool IsRunning = true;

        static UDP_Reciever()
        {

            RefreshLocalAddresses();

        }

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

                    if (IsLocalAddress(remote.Address))
                    {
                        Debug.WriteLine($"[DEBUG] : Пропускаю свой пакет: {msg}");
                        continue;
                    }

                    Debug.WriteLine($"[DEBUG] : Получено от {remote.Address}: {msg}");

                    Echo(remote.Address, msg);
                }
                catch (SocketException)
                {
                    Debug.WriteLine($"[DEBUG] : Ошибка в прослушивании... ");
                }
            }
        }

        public static void RefreshLocalAddresses()
        {
            _localAddresses.Clear();

            // loopback всегда свой
            _localAddresses.Add(IPAddress.Loopback);

            try
            {
                // получаем ВСЕ локальные IP (может быть несколько сетевых интерфейсов)
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    _localAddresses.Add(ip);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DEBUG] : Ошибка получения локальных IP: {ex.Message}");
            }

            Debug.WriteLine($"[DEBUG] : Локальные адреса: {string.Join(", ", _localAddresses)}");
        }

        public static bool IsLocalAddress(IPAddress address)
        {
            //return _localAddresses.Contains(address);
            return false;
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

            else if (msg == "ECHO WRLSSUPDCONNECT:KEY_123")
            {

                if (!UDP_Controller.clients.Contains(remote))
                {
                    UDP_Controller.GetNew_Client(remote);
                }

            }

            else if (msg == "CHECK WRLSSUPDCONNECT:KEY_123")
            {

                UDP_Parser.Send_message(remote, "ECHO_CHECK WRLSSUPDCONNECT:KEY_123");
                lock (connected_clients)
                {
                    connected_clients.Add(remote);
                }
            }

            else if (msg == "ECHO_CHECK WRLSSUPDCONNECT:KEY_123")
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
