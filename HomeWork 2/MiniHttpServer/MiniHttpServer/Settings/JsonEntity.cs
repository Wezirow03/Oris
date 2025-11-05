using System;
using System.Collections.Generic;
    
namespace MiniHttpServer.Settings
{
    public class JsonEntity
    {
        public string PublicDirectoryPath { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }

        public  JsonEntity(string publicDirectoryPath, string domain, int port)
        {
            PublicDirectoryPath = publicDirectoryPath;
            Domain = domain;
            Port = port;
        }
        public JsonEntity() { }
    }
}
