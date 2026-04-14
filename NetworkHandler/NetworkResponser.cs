using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHandler
{
    public class NetworkResponser
    {
        public static List<IPAddress> connected_clients = new List<IPAddress>();
        public static bool Is_ClientReciever = false;

        private static string appKey, nickName;

        public static void Initialize(string value_appkey, string value_nickname)
        {
            appKey = value_appkey;

            nickName = value_nickname;
        }

        public static void Echo(IPAddress remote, string msg)
        {
            /*
                Логика обращений с сообщениями:

                Пример сообщения {TYPE} {POSTFIX} {KEY} (CHECK WRLSSUPDCONNECT:KEY_123)

                TYPE - отвечает за тип сообщения "HELLO" - Привет, "Check" - Проверка и далее по аналогии
                POSTFIX - Отвечает за параметр сообщения, если не нужен - может не применяться
                KEY - Ключ для того чтобы отличить клиента от любого иного запроса на этом же порту (подразумевается измеенение с версией приложения)
            */

            if (msg == $"HELLO WRLSSUPDCONNECT:KEY_123")
            {
                if (Is_ClientReciever)
                {
                    NetworkParser.Send_message(remote, "ECHO WRLSSUPDCONNECT:KEY_123");
                    NetworkController.GetNew_Client(remote);

                    Is_ClientReciever = true;
                }
            }

            else if (msg == "ECHO WRLSSUPDCONNECT:KEY_123")
            {

                if (!NetworkController.clients.Contains(remote))
                {
                    NetworkController.GetNew_Client(remote);
                }

            }

            else if (msg == "CHECK WRLSSUPDCONNECT:KEY_123")
            {

                NetworkParser.Send_message(remote, "ECHO_CHECK WRLSSUPDCONNECT:KEY_123");
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
