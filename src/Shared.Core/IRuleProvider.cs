namespace Shared.Core;

public interface IRuleProvider<T> where T : class
{
    string GameName { get; }
    IEnumerable<IRule<T>> GetRules();
}
