using GAE.Query;
using Shared.Core;
using Shared.Data;

namespace Demo.Console.Query;

public class FluentApiDemo
{
    public static void Main()
    {
        LocalHighscoreService storage = GetMockData();

        System.Console.WriteLine("--- GLOBALE TOP 3 (über alle Spiele) ---");
        var globalTop = HighscoreExtensions.GetTopPlayers(storage, 3);
        PrintHighscoreTable(globalTop);

        System.Console.WriteLine("\n--- DURCHSCHNITTSWERTE PRO SPIEL ---");
        string[] games = { "SpaceInvaders", "Pacman", "Tetris" };

        foreach (var game in games)
        {
            double average = HighscoreExtensions.GetAverageScore(storage, game);
            string avgDisplay = average > 0 ? $"{average:F2} Pkt." : "Keine Daten";
            System.Console.WriteLine($"{game,-15} : {avgDisplay}");
        }
    }

    private static void PrintHighscoreTable(IEnumerable<Highscore> scores)
    {
        System.Console.WriteLine($"{"Spieler",-12} | {"Score",-8} | {"Spiel",-15}");
        System.Console.WriteLine(new string('-', 40));
        foreach (var s in scores)
        {
            System.Console.WriteLine($"{s.PlayerName,-12} | {s.Score,-8} | {s.GameName,-15}");
        }
    }

    private static LocalHighscoreService GetMockData()
    {
        var service = new LocalHighscoreService();
        service.AddHighscore(new Highscore("Player1", 7500, DateTime.Now, "SpaceInvaders"));
        service.AddHighscore(new Highscore("Player2", 3000, DateTime.Now, "SpaceInvaders"));
        service.AddHighscore(new Highscore("Player3", 1200, DateTime.Now, "Pacman"));
        service.AddHighscore(new Highscore("Player4",  900, DateTime.Now, "Pacman"));
        service.AddHighscore(new Highscore("Player5", 5000, DateTime.Now, "Pacman"));
        return service;
    }
}
