using System.Collections;
using System.Text.RegularExpressions;

namespace MiniTemplateEngine.Core.Conditions
{
    public class EvaluateCond
    {
        private readonly GetPath _getPath = new GetPath();

        public bool EvaluateCondition(object dataModel, string condition)
        {
            condition = condition.Trim();

            if (condition.Contains("||"))
            {
                var parts = condition.Split(new[] { "||" }, StringSplitOptions.None);
                foreach (var part in parts)
                    if (EvaluateCondition(dataModel, part)) return true;
                return false;
            }

            if (condition.Contains("&&"))
            {
                var parts = condition.Split(new[] { "&&" }, StringSplitOptions.None);
                foreach (var part in parts)
                    if (!EvaluateCondition(dataModel, part)) return false;
                return true;
            }

            var equalMatch = Regex.Match(condition, @"(.+)==(.+)");
            if (equalMatch.Success)
                return Equals(GetValue(dataModel, equalMatch.Groups[1].Value), GetValue(dataModel, equalMatch.Groups[2].Value));

            var notEqualMatch = Regex.Match(condition, @"(.+)!=(.+)");
            if (notEqualMatch.Success)
                return !Equals(GetValue(dataModel, notEqualMatch.Groups[1].Value), GetValue(dataModel, notEqualMatch.Groups[2].Value));

            var greaterOrEqualMatch = Regex.Match(condition, @"(.+)>=(.+)");
            if (greaterOrEqualMatch.Success)
                return CompareValues(dataModel, greaterOrEqualMatch.Groups[1].Value, greaterOrEqualMatch.Groups[2].Value) >= 0;

            var lessOrEqualMatch = Regex.Match(condition, @"(.+)<=(.+)");
            if (lessOrEqualMatch.Success)
                return CompareValues(dataModel, lessOrEqualMatch.Groups[1].Value, lessOrEqualMatch.Groups[2].Value) <= 0;

            var greaterThanMatch = Regex.Match(condition, @"(.+)>(.+)");
            if (greaterThanMatch.Success)
                return CompareValues(dataModel, greaterThanMatch.Groups[1].Value, greaterThanMatch.Groups[2].Value) > 0;

            var lessThanMatch = Regex.Match(condition, @"(.+)<(.+)");
            if (lessThanMatch.Success)
                return CompareValues(dataModel, lessThanMatch.Groups[1].Value, lessThanMatch.Groups[2].Value) < 0;

            var value = GetValue(dataModel, condition);
            return value is bool b && b;
        }

        private object? GetValue(object dataModel, string expr)
        {
            expr = expr.Trim();

            if (expr.StartsWith("\"") && expr.EndsWith("\""))
                return expr.Substring(1, expr.Length - 2);

            if (double.TryParse(expr, out double num))
                return num;

            if (dataModel is IDictionary dict && dict.Contains(expr))
                return dict[expr];

            return _getPath.GetValueByPath(dataModel, expr);
        }

        private int CompareValues(object dataModel, string leftExpr, string rightExpr)
        {
            var left = GetValue(dataModel, leftExpr);
            var right = GetValue(dataModel, rightExpr);

            if (left == null || right == null)
                return 0;

            if (left is IConvertible && right is IConvertible)
                return Convert.ToDouble(left).CompareTo(Convert.ToDouble(right));

            if (left is IComparable cLeft)
                return cLeft.CompareTo(right);

            throw new InvalidOperationException($"Невозможно сравнить значения: {left} и {right}");
        }
    }
}
