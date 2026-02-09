using GAE.Shared.Core;

namespace GAE.Async;
public class GameLoopManager
{
    // private readonly ILogger<GameLoopManager> _Logger;

    private readonly CancellationTokenSource _CancellationTokenSource = new();
    private readonly Dictionary<string, Task> _GameLoopTasks = new();

    public GameLoopManager()
        // ILogger<GameLoopManager> logger)
    {
       // _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void StartGameLoop(IArcadeGame game)
    {
        if (!_GameLoopTasks.TryGetValue(game.Name, out Task _))
        {
            //_Logger.LogError($"Game with Hash {} is already running");
            return;
        }

        CancellationToken token = _CancellationTokenSource.Token;

        Task gameLoop = Task.Run(async () =>
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(16));

            game.Initialize();

            while (await timer.WaitForNextTickAsync(token))
            {
                game.Update(0.016);
                game.Render();
            }

            game.Shutdown();
        }, token);

        _GameLoopTasks.Add(game.Name, gameLoop);
        //_Logger.LogInformation($"Started Game {}");
    }

    public async Task StopGameLoopAsync(IArcadeGame game, CancellationToken cancellationToken)
    {
        Task gameLoop;
        if (!_GameLoopTasks.TryGetValue(game.Name, out gameLoop))
        {
            //_Logger.LogError($"Game with Hash {} could not be found");
            return;
        }

        await gameLoop;
    }
}
