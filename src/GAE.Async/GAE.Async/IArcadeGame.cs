namespace GAE.Async;

public record PlayerUpdate(int PlayerId, float X, float Y);

public interface IArcadeGame : IDisposable
{
    string Name { get; }
    void Initialize();
    void Update(double deltaTime);
    void OnNetworkUpdateReceived(PlayerUpdate update);
    void Render();
    void Shutdown();
}
