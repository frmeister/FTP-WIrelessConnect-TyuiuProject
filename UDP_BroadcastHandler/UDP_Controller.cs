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
    public class UDP_Controller
    {
        static Timer timer = new Timer(10000);
        public static List<IPAddress> clients = new List<IPAddress>();
        public static List<IPAddress> old_clients = new List<IPAddress>();

        public static void GetNew_Client(IPAddress ip)
        {
            clients.Add(ip);
        }

        public static void Is_Online()
        {
            timer.Elapsed += Timer_Elapsed;

            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (clients.Count == 0) return;

            Debug.WriteLine($"[DEBUG] : Прошел таймер. Проверяю все ли на месте...");

            foreach (IPAddress ip in clients)
            {

                UDP_Parser.Send_message(ip, "CHECK WRLSSUPDCONNECT:KEY_123");

            }

            Is_Connected();

            Debug.WriteLine($"[DEBUG] : Проверка прошла!");
        }

        public static void Is_Connected()
        {

            // Все клиенты на месте
            if (UDP_Reciever.connected_clients == clients)
                return;

            else
            {
                old_clients = clients;
                clients = UDP_Reciever.connected_clients;
            }    
        }

    }
}
