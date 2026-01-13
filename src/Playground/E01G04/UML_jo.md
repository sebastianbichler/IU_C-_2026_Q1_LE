## UML-Klassendiagramm

```mermaid
classDiagram
    class Parkautomat {
        -const int TICKET_PREIS = 4
        -int eingezahlterBetrag
        +BanknoteAnnehmen(int wert)
        +VorgangAbbrechen()
        -TicketErstellen()
        -WechselgeldBerechnen() int
        -Auszahlen(int betrag)
    }

    class Ticket {
        +int TicketID
        +DateTime Zeitstempel
        +int Preis
        +Drucken()
    }

    class Banknote {
        <<enumeration>>
        ZEHN_EURO = 10
        ZWANZIG_EURO = 20
    }

    class Display {
        +ZeigeText(string nachricht)
        +ZeigeBetrag(int betrag)
    }

    Parkautomat "1" -- "1" Display : nutzt
    Parkautomat "1" ..> Ticket : erzeugt
    Parkautomat "1" ..> Banknote : akzeptiert
