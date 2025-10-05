using System.Net;
using System.Text;
using System.Text.Json;
using MiniHtmlServer.Shared;

var settingsJson = string.Empty;
try
{
    settingsJson = File.ReadAllText("settings.json");
}
catch (FileNotFoundException filex)
{
    Console.WriteLine("File not found " + filex.Message);
    return;
}

SettingsModel settings = null;
try
{
    settings = JsonSerializer.Deserialize<SettingsModel>(settingsJson);
}
catch (Exception)
{
    Console.WriteLine("file ne korrektno json");
    Environment.Exit(1);
}

string indexPath = Path.Combine(settings.PublicDirectoryPath, "index.html");
if (!File.Exists(indexPath))
{
    Console.WriteLine("index.html не найден. Сервер не запущен.");
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
