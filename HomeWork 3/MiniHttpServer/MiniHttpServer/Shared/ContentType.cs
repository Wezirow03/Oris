namespace MiniHttpServer.Shared
{
    public  static class ContentType
    {
        public static  string GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".html" => "text/html",
                ".htm" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".ico" => "image/x-icon",
                ".svg" => "image/svg+xml",
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                _ => "application/octet-stream"
            };
        }
    }
}
