namespace MiniHttpServer.Settings
{
    public class EmailServiceSettings
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Sender { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
