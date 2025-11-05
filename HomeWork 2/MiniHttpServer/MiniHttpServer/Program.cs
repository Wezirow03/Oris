using MiniHttpServer.Settings;
using System.Text.Json;
using MiniHttpServer.EmailService;


namespace MiniHttpServer
{
    class Program
    {
        private static EmailService_ service = new EmailService_();
        public static async Task Main()
        {
            var settings = Singleton.GetInstance();
            var server = new HttpServer(settings);
            server.Start();
            //service.SendService("Vezirov0416@gmail.com", "Belirsiz", "Ilk denemem basarili");
            //service.SendService("Vezirov003@yandex.com", "Belirsiz", "Ilk denemem basarili");




            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd == "/stop")
                {
                    server.Stop();
                    break;
                }


                if (settings == null)
            {
                Console.WriteLine("Ошибка: Не удалось загрузить настройки.");
                return;
            }



            if (!System.IO.File.Exists("Public\\Gpt/gpt.html"))
            {
                Console.WriteLine("Ошибка: index.html не найден. Сервер не запущен.");
                return;
            }

            }
        }
    }
}