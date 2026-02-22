namespace Shared.Core;

public interface IQueryEngine
{
    IQueryable<T> ApplyRule<T>(IQueryable<T> source, IRule<T> rule);
}
