using GAE.Shared.Core;

namespace Demo.Console.G5Integration;

[ArcadeGame(DisplayName = "G5 Snake", Description = "Generator integration demo game")]
public sealed class G5SnakeGame : IArcadeGame
{
    public string Name => "G5 Snake";

    public void Initialize() => System.Console.WriteLine("[G5Snake] Initialize");

    public void Update(double deltaTime)
    {
    }

    public void Render()
    {
    }

    public void Shutdown() => System.Console.WriteLine("[G5Snake] Shutdown");

    public void Dispose()
    {
    }
}
