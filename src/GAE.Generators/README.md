# Gruppe 5 - Meta-Programming (Source Generator)

Dieser Teil implementiert den **Auto-Discovery Orchestrator** für das GAE-Projekt.

## Forschungsfrage
Welche Vorteile haben **Source Generators** gegenüber **Runtime-Reflection** bei der dynamischen Modul-Entdeckung in Plug-and-Play-Architekturen?

## Architektur der Compile-Time Modul-Entdeckung im GAE-System
```mermaid 
flowchart TB
  %% Gruppe 5 – Meta-Programming (Source Generator)
  %% Auto-Discovery Orchestrator für GAE (Compile-Time Discovery)

  subgraph Goal["Gruppe 5: Meta-Programming (Source Generator)"]
    RQ["Forschungsfrage:\nWelche Vorteile haben Source Generators gegenüber Runtime-Reflection\nbei dynamischer Modul-Entdeckung in Plug-and-Play-Architekturen?"]
  end

  subgraph Inputs["Discovery-Vertrag (Input im Consumer-Code)"]
    A1["[ArcadeGame] Attribute\n../Shared.Core/ArcadeGameAttribute.cs"]
    A2["IArcadeGame Interface\n(Implementierung in Game-Klassen)"]
    C1["Game-Klassen im Consumer\n(z. B. Demo.Console / Games)"]
    A1 --> C1
    A2 --> C1
  end

  subgraph Gen["Roslyn Generator: IIncrementalGenerator\nGAE.Generators/ArcadeDiscoveryGenerator.cs"]
    P1["1) Syntax-Filter\nFinde Klassen mit [ArcadeGame]"]
    P2["2) Semantic Check\n- Implementiert IArcadeGame?\n- public parameterless ctor?"]
    D1["Diagnostics\nGAE.Generators/DiagnosticDescriptors.cs"]
    D001["GAE001 (Warning):\n[ArcadeGame] aber kein IArcadeGame"]
    D002["GAE002 (Warning):\nIArcadeGame aber kein public parameterless ctor"]
    P3["3) Code Emission\nGeneriere *.g.cs Dateien"]
    M1["Modell/Metadata\nGAE.Generators/GameInfo.cs"]

    P1 --> P2 --> P3
    P2 --> D1
    D1 --> D001
    D1 --> D002
    P2 <--> M1
  end

  subgraph Outputs["Generierter Code (Compile-Time Output)"]
    O1["GAE.Generated.GameRegistry.CreateAll()\n(GameRegistry.g.cs)"]
    O2["GAE.Generated.GameTelemetry\n(GameTelemetry.g.cs)"]
  end

  subgraph Consumer["Integration im Consumer (Analyzer-Referenz)"]
    PR["ProjectReference als Analyzer<br/>(OutputItemType=Analyzer,<br/>ReferenceOutputAssembly=false)<br/><br/>ProjectReference Include csproj<br/>OutputItemType Analyzer<br/>ReferenceOutputAssembly false"]
    IU["IU-Repo: src/Demo.Console/Demo.Console.csproj\n(aktuell eingebunden)"]
    PR --> IU
  end

  subgraph Build["Build / Nachweis (macOS/Linux)"]
    B1["dotnet build MainProject.sln -v minimal"]
    B2["Symbol-Nachweis in Consumer DLL:<br/>strings .../Demo.Console.dll<br/>GameRegistry G5DiscoveryShowcase"]
  end

  %% Connections
  C1 --> P1
  P3 --> O1
  P3 --> O2
  O1 --> B1
  O2 --> B1
  IU --> B1 --> B2

  %% Relevanz (Vorteile)
  subgraph Why["Warum relevant (Vorteile ggü. Runtime-Reflection)"]
    W1["Discovery von Runtime → Compile-Time"]
    W2["Weniger Runtime-Scanning / Reflection"]
    W3["Frühes Feedback via Diagnostics (GAE001/GAE002)"]
    W4["Reproduzierbarer Integrationspfad im Build"]
  end

  Gen -.liefert.-> Why
  Outputs -.ermöglichen.-> Why
  ```

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
