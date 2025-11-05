using System.Net;
using System.Text;
using MiniHttpServer.ListenerCallBack;
using MiniHttpServer.Settings;

namespace MiniHttpServer
{
    public class HttpServer
    {
        private JsonEntity _config;
        private HttpListener _listener;
        private Singleton settings;
        private ListenerCallBack_ callBack = new ListenerCallBack_();
    public HttpServer(Singleton settings)
        {
            this.settings = settings;
            _config = settings.Settings;
        }


        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://{_config.Domain}:{_config.Port}/");
            _listener.Start();
            Console.WriteLine($"Сервер запущен по адресу http://{_config.Domain}:{_config.Port}/");
            
            _ = RequestsAsync();
        }

        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("Сервер остановлен.");
        }

        private async Task RequestsAsync()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => callBack.CallBack(context));
                }
                catch (HttpListenerException)
                {
                    break;
                }
            }
        }
    }

}
