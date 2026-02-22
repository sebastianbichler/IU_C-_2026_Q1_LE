using Shared.Data;

namespace Shared.Core;

public class LocalHighscoreService : IHighscoreProvider
{
    private readonly List<Highscore> _highscores = new();

    public IQueryable<Highscore> AllHighscores => _highscores.AsQueryable();

    public void AddHighscore(Highscore highscore) => _highscores.Add(highscore);
}
