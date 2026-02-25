# Entity Framework Core 8 Architektur

Entity Framework Core 8 (EF 8) löst den Impedance Mismatch auf der konzeptionellen Ebene auf. Hier dokumentiere ich die spezifische **Fluent API Modellierung**, die unsere Domäne schützt.

## Technisches Mapping-Modell

Dieses Diagramm zeigt, wie die komplexen C#-Typen über spezifische EF 8 Features auf die flachen SQLite-Tabellen projiziert werden.



```mermaid
classDiagram
    direction LR

    %% Domänen-Ebene
    class PlayerProfile {
        +PlayerId Id
        +PlayerName Name
        +Inventory Inventory
        +int Level
    }
    class Inventory {
        -List~InventoryItem~ _items
    }

    %% Infrastruktur / Mapping (Deine Leistung)
    class EF8_Fluent_API {
        <<Configuration>>
        +HasConversion()
        +OwnsOne()
        +UsePropertyAccessMode()
    }

    %% Datenbank-Ebene
    class SQLite_Tables {
        <<Database>>
        Table Players
        Table InventoryItems
    }

    %% Beziehungen mit bereinigten Labels
    PlayerProfile ..> EF8_Fluent_API : 1. Nutzt ValueConverter
    Inventory ..> EF8_Fluent_API : 2. Mappt als OwnedEntity
    EF8_Fluent_API ..> SQLite_Tables : 3. Generiert Schema

    %% Details als Notizen (stabiler als Labels)
    note for EF8_Fluent_API "Feature 1: GUID to String Conversion\nFeature 2: Inventory to Subtable Mapping\nFeature 3: Field Access for _items"
