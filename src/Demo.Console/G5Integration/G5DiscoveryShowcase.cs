namespace Demo.Console.G5Integration;

public static class G5DiscoveryShowcase
{
    public static void Run()
    {
        var games = GAE.Generated.GameRegistry.CreateAll();

        System.Console.WriteLine("--- G5 Source Generator Showcase ---");
        System.Console.WriteLine($"Discovered: {GAE.Generated.GameTelemetry.TotalGamesDiscovered}");
        System.Console.WriteLine($"Names: {string.Join(", ", GAE.Generated.GameTelemetry.GameNames)}");

        foreach (var game in games)
        {
            System.Console.WriteLine($"-> {game.Name}");
        }
    }
}
