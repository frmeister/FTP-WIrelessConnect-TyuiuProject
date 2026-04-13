using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace UDP_BroadcastHandler
{
    public class NetworkController
    {
        static Timer timer = new Timer(10000);
        public static List<IPAddress> clients = new List<IPAddress>();
        public static List<IPAddress> old_clients = new List<IPAddress>();

        public static void GetNew_Client(IPAddress ip)
        {
            lock (clients)
            {
                if (!clients.Contains(ip))
                    clients.Add(ip);
            }
        }

        public static void Is_Online()
        {
            timer.Elapsed += Timer_Elapsed;

            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            lock (clients)
            {
                if (clients.Count == 0) return;

                Debug.WriteLine($"[DEBUG] : Прошел таймер. Проверяю все ли на месте...");

                NetworkResponser.connected_clients.Clear();

                foreach (IPAddress ip in clients)
                {

                    NetworkParser.Send_message(ip, "CHECK WRLSSUPDCONNECT:KEY_123");

                }
            }
            Thread.Sleep(1000); // дать время ответить

            Is_Connected();

            Debug.WriteLine($"[DEBUG] : Проверка прошла!");
        }

        public static void Is_Connected()
        {
            lock (clients)
            {
                // Все клиенты на месте
                if (NetworkResponser.connected_clients.SequenceEqual(clients))
                    return;

                else
                {
                    old_clients = clients;
                    clients = NetworkResponser.connected_clients;
                }
            }
        }

    }
}
