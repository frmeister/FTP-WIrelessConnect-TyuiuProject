using FilePacket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace NetworkHandler
{
    /// <summary>
    /// Обработчики отдельных команд из <see cref="NetworkResponser.Echo"/> — без изменения логики.
    /// </summary>
    internal static class NetworkResponserEchoHandlers
    {
        internal static void HandleHello(IPAddress remote)
        {
            if (!NetworkResponser._isClientReceiver)
            {
                Debug.WriteLine($"[NetworkResponser] Отправляю ответ на HELLO...");
                NetworkParser.Send_message(remote, $"ECHO_HELLO {NetworkResponser.appKey} {NetworkResponser.nickName}");
                NetworkController.GetNew_Client(remote, NetworkResponser.nickName_Recieved);

                NetworkResponser._isClientReceiver = true;
            }
        }

        internal static void HandleEchoHello(IPAddress remote)
        {
            if (!NetworkController.clients.Contains(remote))
            {
                NetworkController.GetNew_Client(remote, NetworkResponser.nickName_Recieved);
            }
        }

        internal static void HandleCheck(IPAddress remote)
        {
            NetworkParser.Send_message(remote, $"ECHO_CHECK {NetworkResponser.appKey} {NetworkResponser.nickName}");
            lock (NetworkResponser.connected_clients)
            {
                NetworkResponser.connected_clients.Add(remote);
            }
        }

        internal static void HandleEchoCheck(IPAddress remote)
        {
            lock (NetworkResponser.connected_clients)
            {
                NetworkResponser.connected_clients.Add(remote);
            }
        }

        internal static void HandleAskSend(IPAddress remote)
        {
            IPAddress targetIp;

            NetworkResponser.fs = new FileSender();

            Debug.WriteLine($"[ASK_SEND] ListOfFiles.Count = {NetworkResponser.ListOfFiles!.Count}");
            foreach (var f in NetworkResponser.ListOfFiles) Debug.WriteLine($"  - {f}");

            // Если файлов 0
            if (NetworkResponser.ListOfFiles.Count == 0)
            {
                if (NetworkController.dClients.TryGetValue(NetworkResponser.nickName_Recieved, out IPAddress value))
                {
                    targetIp = value;

                    NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {NetworkResponser.appKey} {NetworkResponser.nickName} CODE_1");

                    return;
                }
                else;
            }

            // Если в директории существует всего один файл
            else if (NetworkResponser.ListOfFiles.Count == 1)
            {
                if (NetworkController.dClients.TryGetValue(NetworkResponser.nickName_Recieved, out IPAddress value))
                {
                    targetIp = value;

                    string fullPath = Path.Combine(NetworkResponser.FileDir, NetworkResponser.ListOfFiles[0]);

                    Task.Run(() => NetworkResponser.fs.SendAskedFile(fullPath, targetIp, 8889));

                    return;
                }
            }

            // Если их много
            else
            {
                if (NetworkController.dClients.TryGetValue(NetworkResponser.nickName_Recieved, out IPAddress value))
                {
                    targetIp = value;

                    // int fileCount = ListOfFiles.Count, counter = 0;  - Счетчик файлов, пока нет применения

                    NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {NetworkResponser.appKey} {NetworkResponser.nickName} LIST_START");

                    foreach (var name in NetworkResponser.ListOfFiles)
                    {
                        NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {NetworkResponser.appKey} {NetworkResponser.nickName} {name}");
                    }

                    NetworkParser.Send_message(targetIp, $"ECHO_ASK_SEND {NetworkResponser.appKey} {NetworkResponser.nickName} LIST_END");

                    return;
                }
            }
        }

        internal static void HandleEchoAskSend(IPAddress remote, string msg)
        {
            //// Error Handler
            //if (content == "CODE_1") { SendAskedStatus = 1; break; }

            //if (content == "CODE_2") { SendAskedStatus = 1; break; }

            //if (content != "LIST_START" && content != "LIST_END") { ReceivedList.Add(content); break; }

            //if (content == "LIST_START") { ReceivedList = new List<string>(); break; }

            //if (content == "LIST_END") { SendAskedStatus = 2; FileListReceived?.Invoke(remote, ReceivedList ?? new List<string>()); break; }

            //else break;

            // Формат: ECHO_ASK_SEND {appKey} {nickName} [параметр]
            // Параметр может содержать пробелы (имя файла).
            if (!NetworkEchoMessageParser.TryExtractParam(msg, out string extractedParam))
            {
                Debug.WriteLine($"[ECHO_ASK_SEND] Не удалось извлечь параметр из: '{msg}'");
                return;
            }

            NetworkResponser.param = extractedParam;

            Debug.WriteLine($"[ECHO_ASK_SEND] param = '{NetworkResponser.param}'");

            if (NetworkResponser.param == "CODE_1" || NetworkResponser.param == "CODE_2")
            {
                NetworkResponser.SendAskedStatus = (NetworkResponser.param == "CODE_1") ? 1 : 2;
                return;
            }
            if (NetworkResponser.param == "LIST_START")
            {
                NetworkResponser.ReceivedList = new List<string>();
                Debug.WriteLine("[ECHO_ASK_SEND] LIST_START, список очищен");
                return;
            }
            if (NetworkResponser.param == "LIST_END")
            {
                NetworkResponser.SendAskedStatus = 2;
                Debug.WriteLine($"[ECHO_ASK_SEND] LIST_END, список содержит {NetworkResponser.ReceivedList.Count} файлов");
                NetworkResponser.RaiseFileListReceived(remote, NetworkResponser.ReceivedList ?? new List<string>());
                return;
            }
            // Иначе это имя файла
            NetworkResponser.ReceivedList.Add(NetworkResponser.param);
            Debug.WriteLine($"[ECHO_ASK_SEND] Добавлен файл: {NetworkResponser.param}, теперь в списке {NetworkResponser.ReceivedList.Count}");
        }

        internal static void HandleRequestFile(IPAddress remote, string msg)
        {
            NetworkResponser.fs = new FileSender();

            if (NetworkController.dClients.TryGetValue(NetworkResponser.nickName_Recieved, out IPAddress reqValue))
            {
                IPAddress targetIp = reqValue;

                if (!NetworkEchoMessageParser.TryExtractParam(msg, out string extractedParam))
                {
                    Debug.WriteLine($"[REQUEST_FILE] Не удалось извлечь имя файла из: '{msg}'");
                    return;
                }

                NetworkResponser.param = extractedParam;

                string fullPath = Path.Combine(NetworkResponser.FileDir, NetworkResponser.param);

                // Важно отправлять запрошенные файлы последовательно,
                // иначе пакеты разных файлов могут перемешаться по времени.
                NetworkResponser.fs.SendAskedFile(fullPath, targetIp, 8889).GetAwaiter().GetResult();
            }
        }

        internal static void HandleFileSent_Ok(IPAddress remote, string msg)
        {
            string[] tokens = NetworkEchoMessageParser.SplitMessageTokens(msg);
            // Ожидаемый формат: FILE_SENT_OK {appKey} {fileName} {fileHash}
            if (tokens.Length < 4)
            {
                Debug.WriteLine("[NetworkResponser] Некорректное сообщение FILE_SENT_OK");
                return;
            }

            // Ключ уже проверен в Echo, пропускаем tokens[1]
            string fileName = tokens[2];
            string expectedHash = tokens[3];

            string fullPath = Path.Combine(NetworkResponser.FileDir, fileName);
            if (!File.Exists(fullPath))
            {
                Debug.WriteLine($"[NetworkResponser] Файл {fileName} не найден для проверки хеша");
                NetworkParser.Send_message(remote, $"ECHO_FILE_SENT_ERROR {NetworkResponser.appKey} {fileName} FILE_NOT_FOUND");
                return;
            }

            string actualHash = ComputeFileHash(fullPath);
            if (actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine($"[NetworkResponser] Хеш файла {fileName} совпадает, целостность подтверждена");
                NetworkParser.Send_message(remote, $"ECHO_FILE_SENT_OK {NetworkResponser.appKey} {fileName}");
            }
            else
            {
                Debug.WriteLine($"[NetworkResponser] Хеш файла {fileName} НЕ совпадает! Ожидался {expectedHash}, получен {actualHash}");
                NetworkParser.Send_message(remote, $"ECHO_FILE_SENT_ERROR {NetworkResponser.appKey} {fileName} HASH_MISMATCH");
            }
        }

        private static string ComputeFileHash(string filePath)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hash = sha256.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        internal static void HandleEchoFileSentOk(IPAddress remote, string msg)
        {
            string[] tokens = NetworkEchoMessageParser.SplitMessageTokens(msg);
            // Ожидаем: ECHO_FILE_SENT_OK {key} {fileName}
            if (tokens.Length < 3)
            {
                Debug.WriteLine("[NetworkResponser] Некорректное сообщение ECHO_FILE_SENT_OK");
                return;
            }
            // Ключ уже проверен, tokens[1] игнорируем
            string fileName = tokens[2];

            Debug.WriteLine($"[NetworkResponser] Получено подтверждение целостности для {fileName}");
            NetworkResponser.RaiseFileConfirmationReceived(remote, fileName, true);
        }

        internal static void HandleEchoFileSentError(IPAddress remote, string msg)
        {
            string[] tokens = NetworkEchoMessageParser.SplitMessageTokens(msg);
            // Ожидаем: ECHO_FILE_SENT_ERROR {key} {fileName} [errorDetails]
            if (tokens.Length < 3)
            {
                Debug.WriteLine("[NetworkResponser] Некорректное сообщение ECHO_FILE_SENT_ERROR");
                return;
            }
            string fileName = tokens[2];
            string errorDetails = tokens.Length > 3 ? tokens[3] : "UNKNOWN_ERROR";

            Debug.WriteLine($"[NetworkResponser] Получена ошибка целостности для {fileName}: {errorDetails}");
            NetworkResponser.RaiseFileConfirmationReceived(remote, fileName, false);
        }

        internal static void HandleUnknown(IPAddress remote, string msg)
        {
            Debug.WriteLine($"[NetworkResponser] : Неизвестный пакет от {remote}: {msg}");
        }
    }
}
