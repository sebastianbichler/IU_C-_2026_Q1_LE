using GAE.Shared.Core;

namespace Demo.Console.G5Integration;

[ArcadeGame(DisplayName = "G5 Mines", Description = "Generator integration demo game")]
public sealed class G5MinesGame : IArcadeGame
{
    public string Name => "G5 Mines";

    public void Initialize() => System.Console.WriteLine("[G5Mines] Initialize");

    public void Update(double deltaTime)
    {
    }

    public void Render()
    {
    }

    public void Shutdown() => System.Console.WriteLine("[G5Mines] Shutdown");

    public void Dispose()
    {
    }
}
