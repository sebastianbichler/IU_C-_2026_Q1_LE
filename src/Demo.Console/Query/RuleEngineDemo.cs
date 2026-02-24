using System.Reflection;
using GAE.Query;
using Shared.Core;
using Shared.Data;

namespace Demo.Console.Query;

public class RuleEngineDemo
{
    public class Game1RuleProvider : IRuleProvider<Highscore>
    {
        public string GameName => "Game1";

        public IEnumerable<IRule<Highscore>> GetRules()
        {
            yield return new AchievementRule<Highscore>
            {
                Description = "Gold-Abzeichen",
                Criteria = s => s.Score > 2000
            };

            yield return new AchievementRule<Highscore>
            {
                Description = "Silber-Abzeichen",
                Criteria = s => s.Score > 1250
            };

            yield return new AchievementRule<Highscore>
            {
                Description = "Bronze-Abzeichen",
                Criteria = s => s.Score > 500
            };
        }
    }

    public class Game2RuleProvider : IRuleProvider<Highscore>
    {
        public string GameName => "Game2";

        public IEnumerable<IRule<Highscore>> GetRules()
        {
            yield return new AchievementRule<Highscore>
            {
                Description = "Elfer-Wette",
                Criteria = s => s.Score % 11 == 0
            };

            yield return new AchievementRule<Highscore>
            {
                Description = "Game Over",
                Criteria = s => s.Score < 0
            };
        }
    }

    public static void Main()
    {
        var providerTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRuleProvider<>)) && !t.IsInterface);
        var providers = providerTypes.Select(t => Activator.CreateInstance(t)).ToList();

        var rulesByGame = new Dictionary<string, IEnumerable<IRule<Highscore>>>();
        foreach (IRuleProvider<Highscore> provider in providers)
        {
            if (provider != null)
            {
                System.Console.WriteLine($"\n>> {provider.GetType().Name}:");

                var provRules = provider.GetRules();
                rulesByGame.Add(provider.GameName, provRules);

                foreach (var provRule in provRules)
                {
                    System.Console.WriteLine($"    >> {provRule.Description}");
                }
            }
        }

        var ruleEngine = new RuleEngine();

        foreach (var entry in rulesByGame)
        {
            var mockData = GetMockData().AllHighscores
                .Where(s => s.GameName == entry.Key)
                .ToList();

            foreach (var rule in entry.Value)
            {
                try
                {
                    System.Console.WriteLine($"\n\n===== Regel: {rule.Description} =====");

                    List<string> msg = new List<string>();
                    foreach (var highscore in mockData)
                    {
                        if (ruleEngine.Evaluate(highscore, rule))
                        {
                            msg.Add($"Spieler {highscore.PlayerName} hat das Achievement '{rule.Description}' am {highscore.Date.ToShortDateString()} freigeschaltet.");
                        }
                    }

                    System.Console.WriteLine("\nStruktur-Analyse (Knotentypen):");
                    foreach (var (level, nodeType) in ruleEngine.GetDetectedOperatorsTree())
                    {
                        string indent = new string(' ', level * 2);
                        string prefix = level > 0 ? "└─ " : "";
                        System.Console.WriteLine($"{indent}{prefix}{nodeType}");
                    }

                    System.Console.WriteLine();
                    msg.ForEach(System.Console.WriteLine);
                }
                catch (InvalidOperationException)
                {
                    System.Console.WriteLine($"Die Regel '{rule.Description}' enthält unerlaubte Operatoren!: {rule.Criteria}");
                }
            }
        }
    }

    private static LocalHighscoreService GetMockData()
    {
        var storage = new LocalHighscoreService();
        storage.AddHighscore(new Highscore("Player01",   100, new DateTime(2025, 10, 1), "Game1"));
        storage.AddHighscore(new Highscore("Player02",   500, new DateTime(2025, 11, 1), "Game1"));
        storage.AddHighscore(new Highscore("Player03",  1200, new DateTime(2025, 12, 1), "Game1"));
        storage.AddHighscore(new Highscore("Player04",  1500, new DateTime(2026,  1, 1), "Game1"));
        storage.AddHighscore(new Highscore("Player05",  2100, new DateTime(2026,  2, 1), "Game1"));
        storage.AddHighscore(new Highscore("Player07", -   5, new DateTime(2025, 11, 1), "Game2"));
        storage.AddHighscore(new Highscore("Player08",     0, new DateTime(2025, 12, 1), "Game2"));
        storage.AddHighscore(new Highscore("Player09",     5, new DateTime(2026,  1, 1), "Game2"));
        return storage;
    }
}
