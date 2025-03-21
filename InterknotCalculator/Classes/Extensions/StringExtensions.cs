using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ILMath;

namespace InterknotCalculator.Classes.Extensions;

public static class StringExtensions {
    public static string ProcessVariables(this string expression, Dictionary<string, double> variables) {
        return Regex.Replace(expression, @"\{(.*?)\}", match => {
            string key = match.Groups[1].Value;
            return variables.TryGetValue(key, out double value) ? value.ToString(CultureInfo.InvariantCulture) : match.Value;
        });
    }

    public static EvaluationContext EvalContext { get; } = EvaluationContext.CreateDefault();

    public static SafeDictionary<string, double> Variables { get; set; } = new();
    
    public static double EvaluateExpression(this string expression) {
        var eval = MathEvaluation.CompileExpression("tmp", 
            expression.ProcessVariables(Variables), CompilationMethod.ExpressionTree);
        return eval.Invoke(EvalContext);
    }
}