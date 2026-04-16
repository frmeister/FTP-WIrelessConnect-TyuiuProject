using FilePacket;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
/*
    Логика обращений с сообщениями:

    Пример сообщения {TYPE} {KEY} {POSTFIX} (HELLO WRLSCONNECT_123 Vasya)

    TYPE - отвечает за тип сообщения "HELLO" - Привет, "Check" - Проверка и далее по аналогии
    POSTFIX - Отвечает за параметр сообщения, если не нужен - может не применяться
    KEY - Ключ для того чтобы отличить клиента от любого иного запроса на этом же порту (подразумевается измеенение с версией приложения)

    Текущие используемые комманды:
    TYPE: HELLO, ECHO_HELLO, CHECK, ECHO_CHECK

    ASK_SEND {appKey} {nickName}
*/

namespace NetworkHandler
{
    public class NetworkResponser
    {
        public static List<IPAddress> connected_clients = new List<IPAddress>();
        public static bool _isClientReciever = false;

        private static string appKey, nickName,FilePath;
        public static string nickName_Recieved;

        public static void Initialize(string value_appkey, string value_nickname, string value_FilePath)
        {
            appKey = value_appkey;

            nickName = value_nickname;

            FilePath = value_FilePath;

            Debug.WriteLine($"[NetworkResponser] ------------------------------------");
            Debug.WriteLine($"[NetworkResponser] Текущий ключ: {appKey}");
            Debug.WriteLine($"[NetworkResponser] Ник: {nickName}");
            Debug.WriteLine($"[NetworkResponser] ------------------------------------");
        }

        public static void Echo(IPAddress remote, string msg)
        {
            string[] lines = msg.Split(' ');

            bool _isMessageCorrect = true, _postfixExists = false;

            if (lines.Length < 2)
            {
                Debug.WriteLine($"[NetworkResponser] Сообщение слишком короткое: {msg}");
                return;
            }
            else if (lines.Length == 3) { string POSTFIX = lines[2]; nickName_Recieved = POSTFIX;  } // Додумать как впихнуть в вывод

            string TYPE = lines[0];
            string KEY = lines[1];

            if (KEY != appKey)
            {
                Debug.WriteLine($"[NetworkResponser] Неверный ключ: {msg}");
                return;
            }

            switch (TYPE)
            {
                case "HELLO":
                    if (!_isClientReciever)
                    {
                        Debug.WriteLine($"[NetworkResponser] Отправляю ответ на HELLO...");
                        NetworkParser.Send_message(remote, $"ECHO_HELLO WRLSCONNECT_123 {nickName}"); 
                        NetworkController.GetNew_Client(remote, nickName_Recieved);

                        _isClientReciever = true;
                    }
                    break;


                case "ECHO_HELLO":
                    if (!NetworkController.clients.Contains(remote))
                    {
                        NetworkController.GetNew_Client(remote, nickName_Recieved);
                    }
                    break;


                case "CHECK":
                    NetworkParser.Send_message(remote, "ECHO_CHECK WRLSCONNECT_123");
                    lock (connected_clients)
                    {
                        connected_clients.Add(remote);
                    }
                    break;


                case "ECHO_CHECK":
                    lock (connected_clients)
                    {
                        connected_clients.Add(remote);
                    }
                    break;


                case "ASK_SEND":

                    IPAddress targetIp;
                    
                    FileSender fs = new FileSender();

                    if (NetworkController.dClients.TryGetValue(nickName_Recieved, out IPAddress value))
                    {
                        targetIp = value;

                        Task.Run(() => fs.SendAskedFile(FilePath, targetIp, 8889));

                        break;
                    }
                    else break;
                    

                // Лог случайного пакета
                default:
                    Debug.WriteLine($"[NetworkResponser] : Неизвестный пакет от {remote}: {msg}");
                    break;
            }
        }
    }
}
