using Shared.Core;
using Shared.Data;

namespace GAE.Query;

public static class HighscoreExtensions
{
    public static IEnumerable<Highscore> GetTopPlayers(IHighscoreProvider provider, int count)
    {
        return provider.AllHighscores
            .OrderByDescending(s => s.Score)
            .Take(count);
    }

    public static double GetAverageScore(IHighscoreProvider provider, string gameName)
    {
        var gameScores = provider.AllHighscores.Where(s => s.GameName == gameName);
        if (!gameScores.Any()) return 0;
        return gameScores.Average(s => s.Score);
    }
}
