using System;
using System.Net;
using MiniHtmlServer.Shared;

namespace MiniHtmlServer
{
    public class HttpServer
    {
        private HttpListener _listener;
        private SettingsModel _config;

        public HttpServer(SettingsModel config)
        {
            _config = config;
        }

        public void Start()
        {
            _listener = new HttpListener();
            // BURASI YANLIŞTI → "server" yok, _listener kullanmalısın
            _listener.Prefixes.Add($"http://{_config.Domain}:{_config.Port}/");
            _listener.Start();
            Receive();
            Console.WriteLine("Сервер запущен.");
        }

        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("Сервер остановлен.");
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private async void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                Console.WriteLine($"Запрос: {request.Url}");

                string responseText = "<h1>Hello from server!</h1>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);

                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();

                Receive(); 
            }
        }
    }
}
