namespace GAE.Async;

public class LockBasedGame : IArcadeGame
{
    public string Name => "Lock-Based Sync";
    private readonly Dictionary<int, PlayerUpdate> _players = new();
    private readonly object _syncRoot = new();

    public void OnNetworkUpdateReceived(PlayerUpdate update)
    {
        lock (_syncRoot)
        {
            if (GameLogic.IsMovementValid(update))
            {
                _players[update.PlayerId] = update;
            }
        }
    }

    public void Update(double deltaTime)
    {
        lock (_syncRoot)
        {
            // Simuliere Logik: Wir iterieren über alle Spieler
            foreach (var player in _players.Values)
            {
                // Berechnungen...
            }
        }
    }

    public void Initialize() => Console.WriteLine($"{Name} gestartet.");
    public void Render() { /* Dummy */ }
    public void Shutdown() => _players.Clear();
    public void Dispose() => Shutdown();
}
