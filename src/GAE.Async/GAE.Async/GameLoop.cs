using GAE.Shared.Core;

namespace GAE.Async;

public class GameLoop
{
    private bool _isRunning;

    public async Task StartLoopAsync(IArcadeGame game, CancellationToken ct)
    {
        _isRunning = true;
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(16));

        game.Initialize();

        while (await timer.WaitForNextTickAsync(ct) && _isRunning)
        {
            game.Update(0.016);
            game.Render();
        }

        game.Shutdown();
    }

    public void Stop() => _isRunning = false;
}
