using Microsoft.CodeAnalysis;

namespace GAE.Generators;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor MissingInterfaceRule = new(
        id: "GAE001",
        title: "Missing IArcadeGame implementation",
        messageFormat: "Class '{0}' has [ArcadeGame] but does not implement IArcadeGame",
        category: "GAE.Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Classes annotated with [ArcadeGame] must implement IArcadeGame.");

    public static readonly DiagnosticDescriptor MissingParameterlessConstructorRule = new(
        id: "GAE002",
        title: "Missing public parameterless constructor",
        messageFormat: "Class '{0}' implements IArcadeGame but has no public parameterless constructor",
        category: "GAE.Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Discovered game classes need a public parameterless constructor for generated activation.");
}
