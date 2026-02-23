namespace GAE.Shared.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ArcadeGameAttribute : Attribute
{
    public string? DisplayName { get; init; }

    public string? Description { get; init; }
}
