using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MiniTemplateEngine.Core.Conditions;

namespace MiniTemplateEngine.Core
{
    public class HtmlTemplateRenderer
    {
        private readonly GetPath _getPath = new GetPath();
        private readonly Condition_If _condIf;
        private readonly Condition_Foreach _condForeach;

        public HtmlTemplateRenderer()
        {
            _condIf = new Condition_If(this);
            _condForeach = new Condition_Foreach(this);
        }

        public string RenderFromString(string template, object dataModel)
        {
            template = _condForeach.RenderForEachBlocks(template, dataModel);
            template = _condIf.RenderIfCond(template, dataModel);
            template = RenderVariables(template, dataModel);
            return template;
        }

        private string RenderVariables(string template, object dataModel)
        {
            var regex = new Regex(@"\$\{([A-Za-z_][A-Za-z0-9_\.]*)\}");
            return regex.Replace(template, m =>
            {
                var path = m.Groups[1].Value;
                var value = _getPath.GetValueByPath(dataModel, path);
                return value?.ToString() ?? "";
            });
        }

        public Dictionary<string, object?> NewDataModels(object dataModel, string localVarName, object localVarValue)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                [localVarName] = localVarValue
            };

            if (dataModel is IDictionary dictModel)
            {
                foreach (var key in dictModel.Keys)
                    dict[key.ToString()] = dictModel[key];
            }
            else
            {
                foreach (var prop in dataModel.GetType().GetProperties())
                    dict[prop.Name] = prop.GetValue(dataModel);
            }

            return dict;
        }
    }
}
