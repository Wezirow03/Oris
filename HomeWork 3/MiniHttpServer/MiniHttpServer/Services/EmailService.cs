using System.Net;
using System.Net.Mail;
using MiniHttpServer.Settings;

namespace MiniHttpServer.Services
{
    public class EmailService_
    {
        public void SendService(string email, string subject, string message)
        {
            var gmailSettings = new EmailServiceSettings()
            {
                Name = "Google",
                Host = "smtp.gmail.com",
                Sender = "Vezirov0416@gmail.com",
                Port = 587,
                EnableSSL = true,
                UserName = "Vezirov",
                Password = "giripnameetjek"
            };
            var yandexSettings = new EmailServiceSettings()
            {
                Name = "Yandex",
                Host = "smtp.yandex.ru",
                Sender = "Vezirov003@yandex.com",
                Port = 587,
                EnableSSL = true,
                UserName = "Vezirov",
                Password = "giripnameetjek"
            };

            var googleSmtp = CreateSmtp(email, subject, message, gmailSettings);
            var yandexSmtp = CreateSmtp(email, subject, message, yandexSettings);
            Action[] clients = new[] { googleSmtp, yandexSmtp };

            foreach (var client in clients)
            {
                try
                {
                    client.Invoke();
                    Console.WriteLine("Сообщение отправлено");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка отправки");
                }
            }
        }
         
        public Action CreateSmtp(string email, string subject, string message_, EmailServiceSettings settings)
        {
            MailAddress from = new MailAddress(settings.Sender, settings.Name);
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from,to);
            message.Subject=subject;
            message.Body = $"<h2>Your login is : {email}</h2>" +
                           $"<h2>Your password: {message_}</h2>";
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment("file.zip"));
             
            SmtpClient smtp = new SmtpClient(settings.Host, settings.Port);
            smtp.Credentials = new NetworkCredential(settings.UserName, settings.Password);
            smtp.EnableSsl = settings.EnableSSL;
            return () =>
            {
                smtp.Send(message);
            };

        }

    }
}
