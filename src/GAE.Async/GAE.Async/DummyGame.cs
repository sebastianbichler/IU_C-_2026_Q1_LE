using GAE.Shared.Core;

namespace GAE.Async;
internal class DummyGame : IArcadeGame
{
    public string Name { get; } = nameof(DummyGame);

    public void Initialize()
    {
        Console.WriteLine($"{nameof(DummyGame)}.{nameof(Initialize)}");
    }

    public void Update(double deltaTime)
    {
        Console.WriteLine($"{nameof(DummyGame)}.{nameof(Update)} - {deltaTime}");
    }

    public void Render()
    {
        Console.WriteLine($"{nameof(DummyGame)}.{nameof(Render)}");
    }

    public void Shutdown()
    {
        Console.WriteLine($"{nameof(DummyGame)}.{nameof(Shutdown)}");
    }

    public void Dispose()
    {
        Console.WriteLine($"{nameof(DummyGame)}.{nameof(Dispose)}");
    }
}
