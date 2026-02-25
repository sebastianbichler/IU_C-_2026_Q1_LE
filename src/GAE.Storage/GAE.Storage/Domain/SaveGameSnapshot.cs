namespace GAE.Storage.Domain;

using GAE.Storage.Domain.PlayerProfile;
using GAE.Storage.Domain.PlayerProfile.ValueObjects;

public sealed record SaveGameSnapshot(
    PlayerId PlayerId,
    string PlayerName,
    int Level,
    IReadOnlyCollection<InventoryItem> Inventory,
    DateTimeOffset SavedAtUtc);
