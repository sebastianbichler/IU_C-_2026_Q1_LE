using Shared.Data;

namespace Shared.Core;

public interface IHighscoreProvider
{
    IQueryable<Highscore> AllHighscores { get; }

    void AddHighscore(Highscore highscore);
}
