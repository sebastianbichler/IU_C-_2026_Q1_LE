namespace GAE.Domain.PlayerProfile;

public sealed class PlayerProfile
{
    public Inventory Inventory { get; } = new();
    public PlayerId Id { get; }
    public PlayerName Name { get; }

    public int Level { get; private set; }

    public PlayerProfile(PlayerId id, PlayerName name)
    {
        Id = id;
        Name = name;
        Level = 1;
    }

    public void LevelUp() => Level++;

}
