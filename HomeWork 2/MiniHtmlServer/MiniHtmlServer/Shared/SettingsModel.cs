using System.Text.Json;

namespace MiniHtmlServer.Shared
{
    public class SettingsModel
    {
        public string PublicDirectoryPath { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }

        private static SettingsModel _instance;
        private static readonly object _lock = new();

        // ❗️public olmalı, yoksa JSON deserializer hata verir
        public SettingsModel() { }

        public static SettingsModel GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        try
                        {
                            string json = File.ReadAllText("settings.json");
                            _instance = JsonSerializer.Deserialize<SettingsModel>(json);
                            Console.WriteLine("Настройки успешно загружены!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка загрузки файла настроек: " + ex.Message);
                            return null;
                        }
                    }
                }
            }
            return _instance;
        }
    }
}
