using MiniHttpServer.Core.Attributes;
using MiniHttpServer.Services;

namespace MiniHttpServer.Endpoints
{
    [Endpoint]
    internal class AuthEndpoint
    {

        private static EmailService_ service = new EmailService_();

        // Get /auth/
        [HttpGet]
        public string LoginPage()
        {

            return "index.html";
        }

        // Post /auth/
        [HttpPost]
        public string Login(string email, string password)
        {
            service.SendService(email, "Авторизация", $@"
                                    <html>
                                    <body>
                                        <h2>Уведомление об авторизации</h2>
                                        <p>Вы успешно авторизовались в системе.</p>
                                        
                                        <p><strong>Ваш логин:</strong> {email}</p>
                                        <p><strong>Ваш пароль:</strong> {password}</p>
                                        
                                        <p>Сохраните эти данные для будущих входов в систему.</p>
                                    </body>
                                    </html>");
            return "index.html";

        }


        // Post /auth/sendEmail
        [HttpPost("sendEmail")]
        public void SendEmail(string to, string title, string message)
        {
            service.SendService( to, title, message);
           



        }

    }
}
