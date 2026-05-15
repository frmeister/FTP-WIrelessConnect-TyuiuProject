using FilePacket;
using FileHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

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

        public static FileTransferManager TransferManager { get; private set; }
        public static event Action<IPAddress, string, bool>? FileConfirmationReceived;

        internal static List<string>? ListOfFiles;
        internal static List<string> ReceivedList = new List<string>();
        public static bool _isClientReceiver = false;

        public static event Action<IPAddress, List<string>>? FileListReceived;

        public static int SendAskedStatus = 0; // 0 - Neutral, 1 - Error, 2 - OK

        private static string content;
        internal static string param;
        internal static string FileDir;
        public static string nickName_Recieved, appKey, nickName;

        static FileBrowser fileBrowser = new FileBrowser();
        internal static FileSender fs = new FileSender();

        public static void Initialize(string value_appkey, string value_nickname, string value_FileDir)
        {
            appKey = value_appkey;

            nickName = value_nickname;

            FileDir = value_FileDir;

            if (!string.IsNullOrEmpty(FileDir))
                ListOfFiles = fileBrowser.ListOfReqFiles(FileDir);
            else Debug.WriteLine("[NetworkResponser] : Невозможно получить адрес директории!");

            TransferManager = new FileTransferManager(appKey);

            // Подписываем TransferManager на событие подтверждения
            FileConfirmationReceived += (remote, fileName, success) =>
            {
                TransferManager.ConfirmFromReceiver(fileName, success);
            };

            TransferManager.SendMessageRequested += (remote, msg) =>
            {
                NetworkParser.Send_message(remote, msg);
            };
            TransferManager.FileSentSuccessfully += (filePath) =>
            {
                Debug.WriteLine($"[NetworkResponser] Файл успешно отправлен и подтверждён: {filePath}");
            };

            Debug.WriteLine($"[NetworkResponser] ------------------------------------");
            Debug.WriteLine($"[NetworkResponser] Текущий ключ: {appKey}");
            Debug.WriteLine($"[NetworkResponser] Ник: {nickName}");
            Debug.WriteLine($"[NetworkResponser] ------------------------------------");
        }

        internal static void RaiseFileListReceived(IPAddress remote, List<string> files) =>
            FileListReceived?.Invoke(remote, files);

        internal static void RaiseFileConfirmationReceived(IPAddress remote, string fileName, bool success)
        {
            FileConfirmationReceived?.Invoke(remote, fileName, success);
        }

        public static void Echo(IPAddress remote, string msg)
        {
            string[] lines = NetworkEchoMessageParser.SplitMessageTokens(msg);

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
                    NetworkResponserEchoHandlers.HandleHello(remote);
                    break;

                case "ECHO_HELLO":
                    NetworkResponserEchoHandlers.HandleEchoHello(remote);
                    break;

                case "CHECK":
                    NetworkResponserEchoHandlers.HandleCheck(remote);
                    break;

                case "ECHO_CHECK":
                    NetworkResponserEchoHandlers.HandleEchoCheck(remote);
                    break;

                case "ASK_SEND":
                    NetworkResponserEchoHandlers.HandleAskSend(remote);
                    break;

                case ("ECHO_ASK_SEND"):
                    NetworkResponserEchoHandlers.HandleEchoAskSend(remote, msg);
                    break;

                case ("REQUEST_FILE"):
                    NetworkResponserEchoHandlers.HandleRequestFile(remote, msg);
                    break;

                case ("FILE_SENT_OK"):
                    NetworkResponserEchoHandlers.HandleFileSent_Ok(remote, msg);
                    break;

                case ("FILE_SENT_ERROR"):

                    break;

                case ("ECHO_FILE_SENT_OK"):
                    NetworkResponserEchoHandlers.HandleEchoFileSentOk(remote, msg);
                    break;

                case ("ECHO_FILE_SENT_ERROR"):
                    NetworkResponserEchoHandlers.HandleEchoFileSentError(remote, msg);
                    break;

                default:
                    NetworkResponserEchoHandlers.HandleUnknown(remote, msg);
                    break;
            }
        }
    }
}
