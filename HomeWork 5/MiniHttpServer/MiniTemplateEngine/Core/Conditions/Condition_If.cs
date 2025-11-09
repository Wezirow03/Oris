using System.Text.RegularExpressions;

namespace MiniTemplateEngine.Core.Conditions
{
    public class Condition_If
    {
        private readonly EvaluateCond _evaluator = new EvaluateCond();
        private readonly HtmlTemplateRenderer _renderer;

        public Condition_If(HtmlTemplateRenderer renderer)
        {
            _renderer = renderer;
        }

        public string RenderIfCond(string template, object dataModel)
        {
            var ifRegex = new Regex(@"\$if\s*\((.*?)\)(.*?)(?:\$else(.*?))?\$endif", RegexOptions.Singleline);

            while (true)
            {
                var match = ifRegex.Match(template);
                if (!match.Success) break;

                string condition = match.Groups[1].Value.Trim();
                string ifBody = match.Groups[2].Value;
                string elseBody = match.Groups[3].Success ? match.Groups[3].Value : "";

                bool condResult = _evaluator.EvaluateCondition(dataModel, condition);

               
                string bodyToRender = condResult ? ifBody : elseBody;
                bodyToRender = _renderer.RenderFromString(bodyToRender, dataModel);

                template = template.Substring(0, match.Index)
                           + bodyToRender
                           + template.Substring(match.Index + match.Length);
            }

            return template;
        }
    }
}
