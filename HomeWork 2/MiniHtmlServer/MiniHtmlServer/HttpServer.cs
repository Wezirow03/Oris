using System.Net;
using System.Text;
using MiniHtmlServer.Shared;

namespace MiniHtmlServer
{
    public class HttpServer
    {
        private readonly SettingsModel _config;
        private HttpListener _listener;

        public HttpServer(SettingsModel config)
        {
            _config = config;
        }

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://{_config.Domain}:{_config.Port}/");
            _listener.Start();
            Console.WriteLine($"Сервер запущен по адресу http://{_config.Domain}:{_config.Port}/");

            _ = HandleRequestsAsync(); 
        }

        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("Сервер остановлен.");
        }

        private async Task HandleRequestsAsync()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => ProcessRequest(context));
                }
                catch (HttpListenerException)
                {
                    break;
                }
            }
        }

        private async Task ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Console.WriteLine($"Запрос: {request.Url}");

            string localPath = request.Url.AbsolutePath.TrimStart('/');
            if (string.IsNullOrEmpty(localPath))
                localPath = "index.html";

            string filePath = Path.Combine(_config.PublicDirectoryPath, localPath);


            if (Directory.Exists(filePath))
                filePath = Path.Combine(filePath, "index.html");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Ошибка: файл {filePath} не найден.");
                response.StatusCode = 404;
                byte[] notFound = Encoding.UTF8.GetBytes("<h1>404 Not Found</h1>");
                response.ContentType = "text/html";
                response.ContentLength64 = notFound.Length;
                await response.OutputStream.WriteAsync(notFound);
                response.Close();
                return;
            }

            try
            {
                string contentType = GetContentType(filePath);
                response.ContentType = contentType;

                using var fs = File.OpenRead(filePath);
                response.ContentLength64 = fs.Length;
                await fs.CopyToAsync(response.OutputStream);
                response.Close();

                Console.WriteLine($"Отправлен файл: {filePath} ({contentType})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
            }
        }

        private string GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".ico" => "image/x-icon",
                _ => "application/octet-stream"
            };
        }
    }
}
