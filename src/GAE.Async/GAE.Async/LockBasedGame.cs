using System.Collections.Generic;

namespace GAE.Async;

public class LockBasedGame : IArcadeGame
{
    public string Name => "Lock-Based Sync";

    private readonly Dictionary<int, PlayerUpdate> _players = new();
    private readonly Queue<PlayerUpdate> _pendingUpdates = new();
    private readonly object _syncRoot = new();

    public void OnNetworkUpdateReceived(PlayerUpdate update)
    {
        lock (_syncRoot)
        {
            _pendingUpdates.Enqueue(update);
        }
    }

    public void Update(double deltaTime)
    {
        lock (_syncRoot)
        {
            while (_pendingUpdates.Count > 0)
            {
                var update = _pendingUpdates.Dequeue();

                if (GameLogic.IsMovementValid(update))
                {
                    _players[update.PlayerId] = update;
                }
            }
        }
    }

    public void Initialize() => Console.WriteLine($"{Name} gestartet.");
    public void Render() { }
    public void Shutdown() => _players.Clear();
    public void Dispose() => Shutdown();
}
