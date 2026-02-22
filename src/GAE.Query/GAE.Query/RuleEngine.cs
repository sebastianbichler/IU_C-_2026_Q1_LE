using System.Collections.Concurrent;
using Shared.Core;

namespace GAE.Query;

public class RuleEngine : IQueryEngine
{
    private static readonly ConcurrentDictionary<string, object> _cache = new();

    private readonly RuleCompiler _compiler = new();
    private readonly RuleAnalyzer _analyzer = new();

    // nur zu Demonstrationszwecken
    public List<(int Level, string Type)> GetDetectedOperatorsTree() => _analyzer.DetectedOperatorsTree;

    public bool Evaluate<T>(T entity, IRule<T> rule)
    {
        _analyzer.Analyze(rule);

        string key = rule.Criteria.ToString();

        // nur zu Demonstrationszwecken
        if (_cache.ContainsKey(key))
        {
            Console.WriteLine($"[RuleEngine] Regel '{rule.Description}' bereits im Cache vorhanden");
        }

        var func = (Func<T, bool>)_cache.GetOrAdd(key, _ =>
        {
            Console.WriteLine($"[RuleEngine] Kompiliere Regel '{rule.Description}'");
            return _compiler.Compile(rule);
        });

        return func(entity);
    }

    public IQueryable<T> ApplyRule<T>(IQueryable<T> source, IRule<T> rule)
    {
        _analyzer.Analyze(rule);
        return source.Where(rule.Criteria);
    }
}
