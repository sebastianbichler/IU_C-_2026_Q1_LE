using Microsoft.CodeAnalysis;

namespace GAE.Generators;

internal sealed class GameInfo
{
    public GameInfo(
        string fullTypeName,
        string displayName,
        bool implementsArcadeGame,
        bool hasValidConstructor,
        Location? location)
    {
        FullTypeName = fullTypeName;
        DisplayName = displayName;
        ImplementsArcadeGame = implementsArcadeGame;
        HasValidConstructor = hasValidConstructor;
        Location = location;
    }

    public string FullTypeName { get; }

    public string DisplayName { get; }

    public bool ImplementsArcadeGame { get; }

    public bool HasValidConstructor { get; }

    public Location? Location { get; }
}
