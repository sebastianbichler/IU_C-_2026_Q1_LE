# Gruppe 5 - Meta-Programming (Source Generator)

Dieser Teil implementiert den **Auto-Discovery Orchestrator** für das GAE-Projekt.

## Forschungsfrage
Welche Vorteile haben **Source Generators** gegenüber **Runtime-Reflection** bei der dynamischen Modul-Entdeckung in Plug-and-Play-Architekturen?

## Was hier umgesetzt ist

- Ein Roslyn `IIncrementalGenerator` zur Compile-Time-Discovery von Spielen
- Expliziter Discovery-Vertrag über `[ArcadeGame]` + `IArcadeGame`
- Generierung von:
  - `GAE.Generated.GameRegistry.CreateAll()`
  - `GAE.Generated.GameTelemetry`
- Compiler-Diagnostics:
  - `GAE001` - `[ArcadeGame]` ohne `IArcadeGame`
  - `GAE002` - `IArcadeGame` ohne public parameterless constructor

## Pipeline (vereinfacht)

1. **Syntax-Filter**: finde Klassen mit `[ArcadeGame]`
2. **Semantic Check**: prüfe Interface + Konstruktor
3. **Code Emission**: generiere `GameRegistry.g.cs`
4. **Compilation**: generierter Code wird normal mitkompiliert

## Warum das relevant ist

- Discovery wird von Runtime auf Compile-Time verlagert
- Weniger Runtime-Scanning/Reflection
- Frühes Feedback durch Diagnostics
- Klarer, reproduzierbarer Integrationspfad im Build

## Wichtige Dateien

- `GAE.Generators/ArcadeDiscoveryGenerator.cs`
- `GAE.Generators/DiagnosticDescriptors.cs`
- `GAE.Generators/GameInfo.cs`
- `../Shared.Core/ArcadeGameAttribute.cs`

## Integration im Consumer

Der Generator wird als Analyzer referenziert (nicht als normale Referenz):

```xml
<ProjectReference Include="..\GAE.Generators\GAE.Generators\GAE.Generators.csproj"
                  OutputItemType="Analyzer"
                  ReferenceOutputAssembly="false" />
```

Im IU-Repo ist das aktuell in `src/Demo.Console/Demo.Console.csproj` eingebunden.

## Build / Nachweis

Aus dem Repo-Root:

```bash
dotnet build MainProject.sln -v minimal
```

Nachweis, dass der generierte Namespace im Consumer enthalten ist (macOS/Linux):

```bash
strings src/Demo.Console/bin/Debug/net8.0/Demo.Console.dll | grep -E "GAE.Generated|GameRegistry|GameTelemetry|G5DiscoveryShowcase"
```

## Demo-Empfehlung für den Vortrag

- **Live-Demo lokal** über `GAE_SourceGenerator_Code` (stabiler Ablauf)
- **IU-Repo nur als Integrationsnachweis** (Build + Symbol-Nachweis)

## Hinweis zum aktuellen Stand

Im IU-Stand sind `GAE001` und `GAE002` als **Warnings** konfiguriert.
