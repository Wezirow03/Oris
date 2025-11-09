

namespace MiniHttpServer.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPost : Attribute
    {
        public string? Rout { get; }

        public HttpPost() { }

        public HttpPost(string? rout)
        {
            Rout = rout;
        }

    }
}
