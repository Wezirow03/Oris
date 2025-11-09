namespace MiniTemplateEngine.Core
{
    public  interface IHtmlTemplateRenderer
    {
        public string RenderFromFile(string filePath, object DataModel);


        public string RenderFromString(string htmlTemplate, object DataModel);


        public string RenderToFile(string inputFilePath, string outputFilePath, object DataModel);
        
    }
}
