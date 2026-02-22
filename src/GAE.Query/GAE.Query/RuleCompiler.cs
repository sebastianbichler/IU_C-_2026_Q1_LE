using Shared.Core;

namespace GAE.Query;

public class RuleCompiler
{
    public Func<T, bool> Compile<T>(IRule<T> rule) => rule.Criteria.Compile();
}
