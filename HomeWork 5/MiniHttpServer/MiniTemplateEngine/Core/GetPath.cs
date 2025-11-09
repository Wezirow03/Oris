using System;
using System.Collections;
using System.Reflection;

namespace MiniTemplateEngine.Core
{
    public class GetPath
    {
        public object? GetValueByPath(object obj, string path)
        {
            if (obj == null || string.IsNullOrEmpty(path))
                return null;

            if (obj is IDictionary dict && dict.Contains(path))
                return dict[path];

            var parts = path.Split('.');
            object? current = obj;

            foreach (var part in parts)
            {
                if (current == null)
                    return null;

                if (current is IDictionary d && d.Contains(part))
                {
                    current = d[part];
                    continue;
                }

                var prop = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null)
                    return null;

                current = prop.GetValue(current);
            }

            return current;
        }
    }
}
