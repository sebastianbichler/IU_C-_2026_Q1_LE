using System.Linq.Expressions;
using Shared.Core;

namespace GAE.Query;

public class AchievementRule<T> : IRule<T>
{
    public string Description { get; init; } = string.Empty;
    public Expression<Func<T, bool>> Criteria { get; init; } = null!;
}
