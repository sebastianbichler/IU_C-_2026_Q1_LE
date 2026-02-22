using System.Linq.Expressions;

namespace Shared.Core;

public interface IRule<T>
{
    string Description { get; }

    Expression<Func<T, bool>> Criteria { get; }
}
