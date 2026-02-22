namespace GAE.Storage.Domain.PlayerProfile.ValueObjects;

public sealed record PlayerName
{
    public string Value { get; }

    public PlayerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Player name must not be empty.", nameof(value));

        if (value.Length > 32)
            throw new ArgumentException("Player name too long.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
