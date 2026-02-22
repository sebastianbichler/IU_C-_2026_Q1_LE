## Zustandsdiagramm - Lebenszyklus einer deklarativen Regel

```mermaid
stateDiagram-v2
    state isSafe <<choice>>
    state isCached <<choice>>

    [*] --> Defined : Regel (als AST¹) anlegen
    Defined --> Discovered : Regel über Regel-Provider abrufen

    Discovered --> isSafe : AST¹ rekursiv durchlaufen und Operatoren prüfen
    isSafe --> Analyzed : Regel ist sicher
    isSafe --> Rejected : Regel ist unsicher
    Rejected --> [*]

    Analyzed --> isCached
    isCached --> Cached : [Bekannt] Logik-Referenz abrufen
    isCached --> Compiled : [Neu] AST¹ kompilieren

    Compiled --> Cached : Ergebnis im Cache speichern
    Cached --> Executed : Funktion auf Objekt anwenden
    Executed --> Cached : Logik-Referenz beibehalten
    Executed --> [*]
```

## Aktivitätsdiagramm - Prozessmodell der Rule-Engine

```mermaid
flowchart TD
    Start([Programmstart]) --> Define[Regeln als AST¹ in Spiel-Modulen anlegen]
    Define --> Scan[Regel-Provider der Spiel-Module identifizieren]

    subgraph Discovery ["Dashboard Initialisierung"]
        Scan --> Select[Bereitgestellte Regeln abrufen]
    end

    subgraph Analysis ["Analyse & Vorbereitung"]
        Select --> Visit[AST¹ rekursiv durchlaufen & Operatoren überprüfen]

        Visit --> Security{Ist Regel sicher?}

        Security -- [Ja] --> CacheLookup{Bereits im Cache?}

        CacheLookup -- [Nein] --> JIT[JIT²-Kompilierung]
    end

    Security -- [Nein] --> Reject([Abbruch mit Exception])

    subgraph Optimization ["Bereitstellung der Logik"]
        CacheLookup -- [Ja] --> Fetch[Aus Cache abrufen]
        JIT --> Save[In Cache speichern]
    end

    subgraph Execution ["Datenverarbeitung"]
        Save --> Apply[Funktion auf Objekt anwenden]
        Fetch --> Apply
        Apply --> Return[Ergebnis zurückgeben]
    end

    Return --> Display([Gefilterte Daten im Dashboard anzeigen])
```

## Sequenzdiagramm - Engine-Plugin-Kommunikation

Zeigt, wie das Dashboard eine Regel von einem Spiel-Modul erhält und diese über die Engine gegen die Highscore-Daten prüft.

```mermaid
sequenceDiagram
    participant D as Dashboard
    participant Q as Rule Engine
    participant G as Game Module
    participant C as Cache

    G->>G: Regeln als AST¹ anlegen

    Note over D, G: Phase 1: Dashboard Initialisierung

    D->>G: Scan Assembly via Reflection
    G->>D: Liste der implementierten Regel-Provider zurückgeben

    D->>G: Dashboard fordert verfügbare Regeln der Provider an
    G->>D: Liste der Regel-Objekte zurückgeben

    Note over D, C: Phase 2: Analyse & Vorbereitung
    D->>Q: Regel analysieren
    Q->>Q: AST¹ rekursiv durchlaufen<br>& Operatoren prüfen

    alt Regel ist nicht sicher
        Q->>D: Abbruch mit Exception
    else Regel ist sicher

        alt Regel nicht im Cache
            Q->>Q: JIT²-Kompilierung der Expression

            Note over Q, C: Phase 3: Bereitstellung der Logik
            Q->>C: Logik-Referenz speichern
        else Regel im Cache
            C->>Q: Logik-Referenz abrufen
        end

        Note over D, Q: Phase 4: Anwendung der Logik
        Q->>Q: Funktion auf Objekt anwenden
        Q->>D: Ergebnisse zurückgeben
    end
```

---

¹AST: Abstract Syntax Tree (= baumartige Datenstruktur, die die logische Struktur von Programmcode als verarbeitbare Objekte darstellt)

²JIT: Just-in-Time (auch bedarfssynchron bezeichnet)
