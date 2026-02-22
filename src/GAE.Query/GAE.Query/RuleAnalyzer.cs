using System.Linq.Expressions;
using Shared.Core;

namespace GAE.Query;

public class RuleAnalyzer : ExpressionVisitor
{
    private readonly ExpressionType[] _allowedTypes = {
        ExpressionType.Lambda,
        ExpressionType.MemberAccess,
        ExpressionType.Parameter,
        ExpressionType.Constant,
        ExpressionType.Convert,

        ExpressionType.GreaterThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThan,
        ExpressionType.LessThanOrEqual,
        ExpressionType.Equal,
        ExpressionType.NotEqual,

        ExpressionType.And,
        ExpressionType.AndAlso,
        ExpressionType.Or,
        ExpressionType.OrElse,
        ExpressionType.Not,

        ExpressionType.Add,
        ExpressionType.Subtract
    };

    public void Analyze<T>(IRule<T> rule)
    {
        _currentLevel = 0;
        DetectedOperatorsTree.Clear();
        Visit(rule.Criteria);
    }

    // nur zu Demonstrationszwecken
    private int _currentLevel = 0;
    public List<(int Level, string Type)> DetectedOperatorsTree { get; } = new();

    public override Expression? Visit(Expression? node)
    {
        if (node == null) return null;

        if (!_allowedTypes.Contains(node.NodeType))
        {
            throw new InvalidOperationException($"Der Operator {node.NodeType} ist in Spielregeln nicht erlaubt!");
        }

        DetectedOperatorsTree.Add((_currentLevel, node.NodeType.ToString()));
        _currentLevel++;

        var expression = base.Visit(node);

        _currentLevel--;

        return expression;
    }
}
