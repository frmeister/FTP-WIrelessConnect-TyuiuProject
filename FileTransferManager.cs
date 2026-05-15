public class FileTransferManager
{
    private const int MAX_RETRIES = 3;
    private readonly FileSender _fileSender;

    // Событие для уведомления NetworkResponser об успешной отправке
    public event Action<string>? FileSentSuccessfully;

    // Событие для отправки сообщения (будет связано с SendMessage)
    public event Action<string>? SendMessageRequested;

    public async Task SendFileWithIntegrityCheck(string filePath, IPAddress targetIp, int port)
    {
        int retryCount = 0;
        bool fileSent = false;

        while (retryCount < MAX_RETRIES && !fileSent)
        {
            // Вычисляем хеш файла до отправки
            string fileHash = ComputeFileHash(filePath);

            // Передаём хеш в FilePacket (потребуется модификация класса)
            bool sent = await _fileSender.SendFile(filePath, targetIp, port, fileHash);

            if (sent)
            {
                fileSent = true;
                Debug.WriteLine($"[FileTransferManager] Файл отправлен успешно, попытка {retryCount + 1}");
                FileSentSuccessfully?.Invoke(filePath);
                SendMessageRequested?.Invoke($"FILE_SENT_OK {fileHash}");
            }
            else
            {
                retryCount++;
                Debug.WriteLine($"[FileTransferManager] Ошибка отправки, попытка {retryCount}/{MAX_RETRIES}");
                await Task.Delay(1000 * retryCount); // Экспоненциальная задержка
            }
        }

        if (!fileSent)
        {
            Debug.WriteLine($"[FileTransferManager] Файл не удалось отправить после {MAX_RETRIES} попыток");
            SendMessageRequested?.Invoke("FILE_SENT_ERROR Максимальное количество попыток исчерпано");
        }
    }

    private string ComputeFileHash(string filePath)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hash = sha256.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}