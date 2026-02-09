namespace GAE.Shared.Core;

public interface IArcadeGame : IDisposable
{
    string Name { get; }
    void Initialize();
    void Update(double deltaTime);
    void Render();
    void Shutdown();
}

public record Highscore(string Player, int Value, DateTime Date, string GameName);
