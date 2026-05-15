using System;

namespace NetworkHandler
{
    /// <summary>
    /// Разбор входящих UDP-сообщений для <see cref="NetworkResponser.Echo"/>.
    /// </summary>
    /// <remarks>
    /// <para><b>Заголовок (как раньше).</b> Строка режется по пробелам через <c>Split(' ')</c> без лимита —
    /// первый токен — тип команды, второй — ключ приложения (может быть пустой строкой, тогда в сообщении
    /// будут двойные пробелы), третий — ник отправителя. Если ровно четыре токена, четвёртый по-прежнему
    /// попадает в поле <see cref="NetworkResponser"/> <c>content</c> (наследие старого кода).</para>
    /// <para><b>Длинный хвост (гибкая часть).</b> Для команд <c>ECHO_ASK_SEND</c> и <c>REQUEST_FILE</c>
    /// имя файла может содержать пробелы. Его нельзя надёжно взять из «токена 4», поэтому используется
    /// <see cref="TryExtractParam"/>: от полной строки отрезается всё после третьего пробела (TYPE, KEY, NICK)
    /// одним куском — это и есть параметр (имя файла, LIST_START, LIST_END, CODE_1 и т.д.).</para>
    /// </remarks>
    internal static class NetworkEchoMessageParser
    {
        internal static bool TryExtractParam(string msg, out string extractedParam)
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

        /// <summary>
        /// Делит сообщение на токены так же, как исходный <c>Echo</c>.
        /// </summary>
        internal static string[] SplitMessageTokens(string msg) => msg.Split(' ');
    }
}
