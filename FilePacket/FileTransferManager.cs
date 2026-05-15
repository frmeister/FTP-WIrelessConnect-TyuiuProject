using FilePacket;
using System.Diagnostics;
using System.Net;

public class FileTransferManager
{
    private const int MAX_RETRIES = 3;
    private const int CONFIRMATION_TIMEOUT_MS = 5000; // 5 секунд ожидания подтверждения

    private readonly FileSender _fileSender;
    private readonly string _appKey;

    // Словарь для ожидания подтверждения по имени файла
    private readonly Dictionary<string, TaskCompletionSource<bool>> _pendingConfirmations = new();

    public event Action<string>? FileSentSuccessfully;
    public event Action<IPAddress, string>? SendMessageRequested;

    public FileTransferManager(string appKey)
    {
        _appKey = appKey ?? throw new ArgumentNullException(nameof(appKey));
        _fileSender = new FileSender();
    }

    /// <summary>
    /// Вызывается из NetworkResponser при получении ECHO_FILE_SENT_OK или ECHO_FILE_SENT_ERROR
    /// </summary>
    public void ConfirmFromReceiver(string fileName, bool success)
    {
        lock (_pendingConfirmations)
        {
            if (_pendingConfirmations.TryGetValue(fileName, out var tcs))
            {
                tcs.TrySetResult(success);
                _pendingConfirmations.Remove(fileName);
            }
        }
    }

    public async Task SendFileWithIntegrityCheck(string filePath, IPAddress targetIp, int port)
    {
        string fileName = Path.GetFileName(filePath);
        string expectedHash = ComputeFileHash(filePath);
        int retryCount = 0;
        bool confirmed = false;

        while (retryCount < MAX_RETRIES && !confirmed)
        {
            // Отправляем файл (без каких-либо служебных сообщений)
            bool sent = await Task.Run(() => _fileSender.SendFile(filePath, targetIp, port));

            if (!sent)
            {
                retryCount++;
                Debug.WriteLine($"[FileTransferManager] Ошибка отправки {fileName}, попытка {retryCount}/{MAX_RETRIES}");
                await Task.Delay(1000 * retryCount);
                continue;
            }

            Debug.WriteLine($"[FileTransferManager] Файл {fileName} отправлен, ожидание подтверждения...");

            // Создаём TaskCompletionSource для ожидания подтверждения
            var tcs = new TaskCompletionSource<bool>();
            lock (_pendingConfirmations)
            {
                _pendingConfirmations[fileName] = tcs;
            }

            // Ждём подтверждения или таймаута
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(CONFIRMATION_TIMEOUT_MS));
            if (completedTask == tcs.Task && tcs.Task.Result)
            {
                // Подтверждение успешно получено
                confirmed = true;
                Debug.WriteLine($"[FileTransferManager] Подтверждение для {fileName} получено успешно.");
                FileSentSuccessfully?.Invoke(filePath);
            }
            else
            {
                // Таймаут или явная ошибка
                lock (_pendingConfirmations)
                {
                    _pendingConfirmations.Remove(fileName);
                }
                retryCount++;
                if (completedTask == tcs.Task && !tcs.Task.Result)
                {
                    Debug.WriteLine($"[FileTransferManager] Получатель сообщил об ошибке целостности {fileName}, попытка {retryCount}/{MAX_RETRIES}");
                }
                else
                {
                    Debug.WriteLine($"[FileTransferManager] Таймаут подтверждения для {fileName}, попытка {retryCount}/{MAX_RETRIES}");
                }
                await Task.Delay(1000 * retryCount);
            }
        }

        if (!confirmed)
        {
            Debug.WriteLine($"[FileTransferManager] Не удалось подтвердить доставку {fileName} после {MAX_RETRIES} попыток");
            SendMessageRequested?.Invoke(targetIp, $"FILE_SENT_ERROR {_appKey} {fileName} NO_CONFIRMATION");
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