using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniTemplateEngine.Core.Conditions
{
    public class Condition_Foreach
    {
        private readonly GetPath _getPath = new GetPath();
        private readonly HtmlTemplateRenderer _renderer;

        public Condition_Foreach(HtmlTemplateRenderer renderer)
        {
            _renderer = renderer;
        }

        public string RenderForEachBlocks(string template, object dataModel)
        {
            var foreachRegex = new Regex(@"\$foreach\s*\((.*?)\)(.*?)\$endfor", RegexOptions.Singleline);

            while (true)
            {
                var match = foreachRegex.Match(template);
                if (!match.Success) break;

                string condition = match.Groups[1].Value.Trim();
                string body = match.Groups[2].Value;

                var parts = condition.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4)
                    throw new InvalidOperationException($"Некорректная конструкция foreach: {condition}");

                string varName = parts[1];
                string collectionName = parts[3];

                var collection = _getPath.GetValueByPath(dataModel, collectionName) as IEnumerable
                                 ?? throw new InvalidOperationException($"Коллекция '{collectionName}' не найдена.");

                var sb = new StringBuilder();
                foreach (var item in collection)
                {
                    var localData = _renderer.NewDataModels(dataModel, varName, item);
                   
                    sb.Append(_renderer.RenderFromString(body, localData));
                }

                template = template.Substring(0, match.Index)
                           + sb.ToString()
                           + template.Substring(match.Index + match.Length);
            }

            return template;
        }
    }
}
    