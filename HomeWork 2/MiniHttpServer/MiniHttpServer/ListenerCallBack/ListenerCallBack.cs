using System.Net;
using System.Text;

namespace MiniHttpServer.ListenerCallBack
{
    public class ListenerCallBack_
    {
        private FindFolder findfolder = new FindFolder();
        private ContenType _contentType = new ContenType();
        public async Task CallBack(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Console.WriteLine($"Запрос: {request.Url}");


            string localPath = request.Url.AbsolutePath.TrimStart('/');
            if (string.IsNullOrEmpty(localPath))
            { localPath = "index.html"; }

            string[] parts = localPath.Split('/');
            localPath = parts[^1];
            

            string filePath = findfolder.DicFind(localPath);
          
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
                string contentType = _contentType.GetContentType(filePath);
                response.ContentType = contentType;

                using var fs = File.OpenRead(filePath);
                response.ContentLength64 = fs.Length;
                await fs.CopyToAsync(response.OutputStream);
                response.Close();

                Console.WriteLine($"Отправлен файл: {filePath} ({contentType})");
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine($"Bos bir referans verildi {nre}");
            }
            catch (FileNotFoundException fex)
            {
                Console.WriteLine($"Dosya bulunamadi {fex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
            }

        }
    }
}