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
    public class NetworkReciever
    {
        // Debug.WriteLine($"[DEBUG] : ///");
        private static readonly HashSet<IPAddress> _localAddresses = new();

        public static bool Is_ClientReciever = true;
        public static bool IsRunning = true;

        static NetworkReciever()
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

                    NetworkResponser.Echo(remote.Address, msg);
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
            return _localAddresses.Contains(address);
            // return false;
        }

    }
}
