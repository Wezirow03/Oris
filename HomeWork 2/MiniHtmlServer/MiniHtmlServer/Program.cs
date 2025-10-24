using MiniHtmlServer.Shared;
using System.Text.Json;

var settings = SettingsModel.GetInstance();
if (settings == null)
{
    Console.WriteLine("Ошибка: Не удалось загрузить настройки.");
    return;
}

// index.html var mı diye kontrol
string indexPath = Path.Combine(settings.PublicDirectoryPath, "index.html");
if (!File.Exists(indexPath))
{
    Console.WriteLine("Ошибка: index.html не найден. Сервер не запущен.");
    return;
}

var server = new MiniHtmlServer.HttpServer(settings);
server.Start();

while (true)
{
    string cmd = Console.ReadLine();
    if (cmd == "/stop")
    {
        server.Stop();
        break;
    }
}
