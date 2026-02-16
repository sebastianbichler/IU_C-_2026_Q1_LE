using System.Threading.Channels;

namespace GAE.Async;

public class ChannelBasedGame : IArcadeGame
{
    public string Name => "Channel-Based Sync";

    private readonly Dictionary<int, PlayerUpdate> _players = new();

    private readonly Channel<PlayerUpdate> _updateChannel = Channel.CreateUnbounded<PlayerUpdate>(
        new UnboundedChannelOptions { SingleReader = true, SingleWriter = false });

    public void OnNetworkUpdateReceived(PlayerUpdate update)
    {
        _updateChannel.Writer.TryWrite(update);
    }

    public void Update(double deltaTime)
    {
        int processed = 0;
        // Wir verarbeiten nur so viel, wie in unseren Frame passt (Rate-Limiting)
        while (processed < 100 && _updateChannel.Reader.TryRead(out var update))
        {
            if (GameLogic.IsMovementValid(update))
            {
                _players[update.PlayerId] = update;
            }
            processed++;
        }
    }

    public async Task ProcessUpdatesAsync(CancellationToken ct)
    {
        await foreach (var update in _updateChannel.Reader.ReadAllAsync(ct))
        {
            _players[update.PlayerId] = update;
        }
    }

    public void Initialize() => Console.WriteLine($"{Name} mit System.Threading.Channels initialisiert.");
    public void Render() { /* Dummy */ }
    public void Shutdown() => _players.Clear();
    public void Dispose() => Shutdown();
}
