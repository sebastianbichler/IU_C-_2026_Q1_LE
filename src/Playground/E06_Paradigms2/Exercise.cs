namespace Playground.E06_Paradigms2;

public class Exercise : Launcher
{
    private int id;
    private int level;
    private string description;
    private bool isActive;

    record HighScore(string PlayerName, int Points);

    public void Deconstruct(out int id, out int level)
    {
        id = 33;
        level = 5;
    }

    public override void Run()
    {
        Console.Clear();
        Console.WriteLine("Exercise 6: Paradigms 2");

        Main();
        playWithRecords();
        DeconstructMonster();
        PlayWithSpan();
    }

    public void changeYear(ref short year)
    {
        year += 1;
    }

    public void Main()
    {
        short year = 2026;
        changeYear(ref year);
        Console.WriteLine(year); // Ausgabe: without ref 2026, with ref 2027
    }

    public void playWithRecords()
    {
        var scores = new List<HighScore>
        {
            new HighScore("Alice", 1200),
            new HighScore("Bob", 850)
        };

        var originalScore = scores[0];
        var updatedScore = originalScore with { Points = 1350 };

        Console.WriteLine($"Original: {originalScore}");
        Console.WriteLine($"Update:   {updatedScore}");
        Console.WriteLine($"Ist es dasselbe Objekt? {ReferenceEquals(originalScore, updatedScore)}");
    }

    public void DeconstructMonster()
    {
        var myMonster = new Monster("Stein-Golem", 500, 15);

        // Hier passiert die Deconstruction
        var (n, h) = myMonster;

        Console.WriteLine($"Monster-Name: {n}, Lebenspunkte: {h}");
    }

    public void PlayWithSpan()
    {
        string input = "SCORE:1500;NAME:PLAYER1"; // SCORE:1500;NAME:PLAYER1
        ReadOnlySpan<char> span = input.AsSpan();

        int scoreKeyEnd = span.IndexOf(':');
        int scoreValueEnd = span.IndexOf(';');

        ReadOnlySpan<char> scoreValueSpan = span.Slice(scoreKeyEnd + 1, scoreValueEnd - scoreKeyEnd - 1);
        int id = int.Parse(scoreValueSpan);

        Console.WriteLine($"Ergebnis:");
        Console.WriteLine($"- SCORE (als int): {id}");
    }
}
