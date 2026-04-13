using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainForm
{
    static public class ConfigManager
    {
        /*

            КонфигМенеджер - Класс помогающий работе с файлом конфигурации

            Объяснение работы методов:

             - Initialize() - Инициализирует конфиг и используется при запуске программы, в случае ошибки загрузки конфига
            создает дефолтный конфиг с помощью CreateDefaultConfig()
            **ВАЖНО** ПРИ АВТО-ЗАПУСКЕ МОЖЕТ ПОНАДОБИТСЯ СПЛЭШ ФОРМА ДЛЯ РАННЕЙ ПОДГРУЗКИ КОНФИГА, ИНАЧЕ КАЖДАЯ ЗАГРУЗКА
            БУДЕТ СОПРОВАЖДАТЬСЯ ПОВТОРНОЙ ЕГО НАСТРОЙКОЙ ВРУЧНУЮ ПОЛЬЗОВАТЕЛЕМ

             - CreateDefaultConfig() - Создает стандартный конфиг при первом запуске и сохраняет его
            Туда можно вписать новые параметры которые могут понадобится при новом запуске
            
             - LoadConfigFromFile() - Загружает конфиг, если такой сужествует

             - SaveConfig() - Сохраняет конфиг вцелом

             - Reload() - Перезагружает конфиг при возникновении ошибок (если упрощать, то применяется когда при дебаге возникает
             ошибка с конфигом. Нужно вставлять вручную, но лучше сначала посоветоваться)

             !!! SetValue() - Устанавливает значение в конфиг. Сначала нужно вписать ключ (прим. appKey), затем значение (прим. Value123)
            таким образом получается SetValue("appKey", "Value123"). 100% понадобится при работе, так как туда можно вписать то, что вам нужно

             !!! GetValue() - Возвращает запрашиваемое значение. Сначала нужно вписать ключ (прим. appKey),
            Общий пример: допустим nickName = Vasya123, тогда GetValue("nickName") вернет строку "Vasya123", если же имя не назначено, то
            код вернет null, следовательно можно использовать конструкцию if (string.IsNullOrEmpty(value)), таким образом можно получить любое значение из конфига.
            


            Методы которыми можно пользоваться без опасений получить ошибку - SetValue(), GetValue().

        */

        static private string cfgDirectory;
        static bool _isInitialized = false;
        private static readonly object _lock = new object();

        static private Dictionary<string, string> _config;

        // Инициализруем конфиг если он еще не загружен
        static public void Initialize(string appDirectory = null)
        {
            if (_isInitialized) return;

            lock (_lock)
            {
                if (_isInitialized) return;

                // Устанавливаем абсолютный путь к приложению, а затем и к конфигу
                if (string.IsNullOrEmpty(appDirectory))
                {
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    appDirectory = Path.GetDirectoryName(exePath);
                }

                cfgDirectory = Path.Combine(appDirectory, "Config.cfg");
                Debug.WriteLine($"[ConfigManager] Путь к конфигу: {cfgDirectory}");

                _config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                try
                {
                    if (!File.Exists(cfgDirectory))
                    {
                        Debug.WriteLine("[ConfigManager] Стандартный конфиг не найде, создаем новый...");
                        CreateDefaultConfig();
                    }
                    else
                    {
                        Debug.WriteLine($"[ConfigManager] Загружаем конфиг из: {cfgDirectory}");
                        LoadConfigFromFile();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ConfigManager] Ошибка инициализации: {ex.Message}");
                    CreateDefaultConfig();
                }
            }
        }

        static private void CreateDefaultConfig()
        {
            _config = new Dictionary<string, string>
            {
                { "dataPath", "none" },
                { "nickName", "none" },
                { "appKey", "WRLSCONNECT_123"}
            };
            SaveConfig();
        }

        static private void LoadConfigFromFile()
        {
            var lines = File.ReadAllLines(cfgDirectory);
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                    continue;

                var parts = trimmed.Split('=', 2);
                if (parts.Length == 2)
                {
                    _config[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        static private void SaveConfig()
        {
            try
            {
                var lines = _config.Select(kvp => $"{kvp.Key} = {kvp.Value}");
                File.WriteAllLines(cfgDirectory, lines);
                Debug.WriteLine($"[ConfigManager] Конфиг сохранен в: {cfgDirectory}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ConfigManager] Ошибка сохранения: {ex.Message}");
            }
        }

        static public void Reload()
        {
            lock (_lock)
            {
                _isInitialized = false;
                Initialize();
            }
        }

        static public void SetValue(string key, string value)
        {
            if (!_isInitialized) Initialize();

            lock (_lock)
            {
                _config[key] = value;
                SaveConfig();
            }
        }

        static public string GetValue(string key, string defaultValue = "")
        {
            if (!_isInitialized)
            {
                Debug.WriteLine($"[ConfigManager] Конфиг не был иницилазирован при получении: {key}");
                Initialize();
            }

            lock (_lock)
            {
                return _config.TryGetValue(key, out var value) ? value : defaultValue;
            }
        }
    }
}
