using FilePacket;
using FileHandler;
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
    ECHO_ASK_SEND_CODE{0-2} {appKey} {nickName} CODE_1 - Error, CODE_2 - Forced Dial End
    ECHO_ASK_SEND_LIST_START
*/

namespace NetworkHandler
{
    public class NetworkResponser
    {
        public static List<IPAddress> connected_clients = new List<IPAddress>();

        static private List<string> ListOfFiles;
        private static List<string> ReceivedList = new List<string>();
        public static bool _isClientReceiver = false;

        public static event Action<IPAddress, List<string>>? FileListReceived;

        public static int SendAskedStatus = 0; // 0 - Neutral, 1 - Error, 2 - OK

        private static string content, param, FileDir;
        public static string nickName_Recieved, appKey, nickName;

        static FileBrowser fileBrowser = new FileBrowser();
        static FileSender fs = new FileSender();

        private static bool TryExtractParam(string msg, out string extractedParam)
        {
            extractedParam = string.Empty;
            if (string.IsNullOrWhiteSpace(msg))
                return false;

            // Команда имеет формат: TYPE KEY NICK [PARAM...]
            // KEY может быть пустым (в сообщении появятся двойные пробелы),
            // поэтому НЕ удаляем пустые элементы.
            // Берем все, что идет после третьего токена, включая пробелы в имени файла.
            string[] parts = msg.Split(new[] { ' ' }, 4, StringSplitOptions.None);
            if (parts.Length < 4)
                return false;

            extractedParam = parts[3].Trim();
            return !string.IsNullOrEmpty(extractedParam);
        }

        public static void Initialize(string value_appkey, string value_nickname, string value_FileDir)
        {
            appKey = value_appkey;

            nickName = value_nickname;

            FileDir = value_FileDir;

            if (!string.IsNullOrEmpty(FileDir))
                ListOfFiles = fileBrowser.ListOfReqFiles(FileDir);
            else Debug.WriteLine("[NetworkResponser] : Невозможно получить адрес директории!");

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

            if (lines.Length == 4) { content = lines[3]; }

            string TYPE = lines[0];
            string KEY = lines[1];
            string POSTFIX = lines[2];

            nickName_Recieved = POSTFIX;

            if (KEY != appKey)
            {
                Debug.WriteLine($"[NetworkResponser] Неверный ключ: {msg}");
                return;
            }

            switch (TYPE)
            {
                case "HELLO":
                    if (!_isClientReceiver)
                    {
                        Debug.WriteLine($"[NetworkResponser] Отправляю ответ на HELLO...");
                        NetworkParser.Send_message(remote, $"ECHO_HELLO {appKey} {nickName}"); 
                        NetworkController.GetNew_Client(remote, nickName_Recieved);

                        _isClientReceiver = true;
                    }
                    break;


                case "ECHO_HELLO":
                    if (!NetworkController.clients.Contains(remote))
                    {
                        NetworkController.GetNew_Client(remote, nickName_Recieved);
                    }
                    break;


                case "CHECK":
                    NetworkParser.Send_message(remote, $"ECHO_CHECK {appKey} {nickName}");
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
                    
                    fs = new FileSender();

                    Debug.WriteLine($"[ASK_SEND] ListOfFiles.Count = {ListOfFiles.Count}");
                    foreach (var f in ListOfFiles) Debug.WriteLine($"  - {f}");

                    // Если файлов 0
                    if (ListOfFiles.Count == 0) {
                        if (NetworkController.dClients.TryGetValue(nickName_Recieved, out IPAddress value))
                        {
                            targetIp = value;

                            NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {appKey} {nickName} CODE_1");

                            break;
                        }
                        else; }

                    // Если в директории существует всего один файл
                    else if (ListOfFiles.Count == 1)
                    {
                        if (NetworkController.dClients.TryGetValue(nickName_Recieved, out IPAddress value))
                        {
                            targetIp = value;

                            string fullPath = Path.Combine(FileDir, ListOfFiles[0]);

                            Task.Run(() => fs.SendAskedFile(fullPath, targetIp, 8889));

                            break;
                        }
                    }

                    // Если их много
                    else
                    {
                        if (NetworkController.dClients.TryGetValue(nickName_Recieved, out IPAddress value))
                        {
                            targetIp = value;

                            // int fileCount = ListOfFiles.Count, counter = 0;  - Счетчик файлов, пока нет применения

                            NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {appKey} {nickName} LIST_START");

                            foreach (var name in ListOfFiles)
                            {
                                NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {appKey} {nickName} {name}");
                            }

                            NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {appKey} {nickName} LIST_END");

                            break;
                        }
                    }

                    break;


                // Эхо запрашиваемого файла
                case ("ECHO_ASK_SEND"):

                    //// Error Handler
                    //if (content == "CODE_1") { SendAskedStatus = 1; break; }

                    //if (content == "CODE_2") { SendAskedStatus = 1; break; }

                    //if (content != "LIST_START" && content != "LIST_END") { ReceivedList.Add(content); break; }

                    //if (content == "LIST_START") { ReceivedList = new List<string>(); break; }

                    //if (content == "LIST_END") { SendAskedStatus = 2; FileListReceived?.Invoke(remote, ReceivedList ?? new List<string>()); break; }

                    //else break;

                    {
                        // Формат: ECHO_ASK_SEND {appKey} {nickName} [параметр]
                        // Параметр может содержать пробелы (имя файла).
                        if (!TryExtractParam(msg, out param))
                        {
                            Debug.WriteLine($"[ECHO_ASK_SEND] Не удалось извлечь параметр из: '{msg}'");
                            break;
                        }

                        Debug.WriteLine($"[ECHO_ASK_SEND] param = '{param}'");

                        if (param == "CODE_1" || param == "CODE_2")
                        {
                            SendAskedStatus = (param == "CODE_1") ? 1 : 2;
                            break;
                        }
                        if (param == "LIST_START")
                        {
                            ReceivedList = new List<string>();
                            Debug.WriteLine("[ECHO_ASK_SEND] LIST_START, список очищен");
                            break;
                        }
                        if (param == "LIST_END")
                        {
                            SendAskedStatus = 2;
                            Debug.WriteLine($"[ECHO_ASK_SEND] LIST_END, список содержит {ReceivedList.Count} файлов");
                            FileListReceived?.Invoke(remote, ReceivedList ?? new List<string>());
                            break;
                        }
                        // Иначе это имя файла
                        ReceivedList.Add(param);
                        Debug.WriteLine($"[ECHO_ASK_SEND] Добавлен файл: {param}, теперь в списке {ReceivedList.Count}");
                        break;
                    }


                case ("REQUEST_FILE"):

                    fs = new FileSender();

                    if (NetworkController.dClients.TryGetValue(nickName_Recieved, out IPAddress reqValue))
                    {
                        targetIp = reqValue;

                        if (!TryExtractParam(msg, out param))
                        {
                            Debug.WriteLine($"[REQUEST_FILE] Не удалось извлечь имя файла из: '{msg}'");
                            break;
                        }

                        string fullPath = Path.Combine(FileDir, param);

                        // Важно отправлять запрошенные файлы последовательно,
                        // иначе пакеты разных файлов могут перемешаться по времени.
                        fs.SendAskedFile(fullPath, targetIp, 8889).GetAwaiter().GetResult();

                        break;
                    }

                    break;

                // Лог случайного пакета
                default:
                    Debug.WriteLine($"[NetworkResponser] : Неизвестный пакет от {remote}: {msg}");
                    break;
            }
        }
    }
}
