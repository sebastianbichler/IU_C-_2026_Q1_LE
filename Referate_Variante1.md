### Programmieren mit C# (DSPC016)

---

# Referatsunterlagen Variante 1

Dies stellt die erste Variante für die Anforderungen an die Prüfung bzw. den Referaten.
Die zweite Variante ist eine Erweiterung der ersten und anspruchsvoller, da sie spezifischer wissenschaftlichen
Fragestellungen und
Anwendungsfällen vorsieht. Welche Variante zum Einsatz kommt, werden wir besprechen.

## 1. Qualifikationsziele

Nach Abschluss des Moduls sollen Studierende in der Lage sein:

- Typische Datenstrukturen zu kennen und abzugrenzen.
- Selbstständig Lösungen in der Programmiersprache C# im NET8-Ökosystem unter Verwendung dieser Strukturen zu erstellen.
- Objektvergleiche, Zeichenketten, Kalenderobjekte und Streams effektiv einzusetzen.

## 2. 🎓 Prüfungsleistung & Formalitäten

- **Prüfungsform:** „Referat“.
- **Inhalt:**
    - Entwicklung einer Anwendung bzw. eines Prototypen, Präsentation der Architektur/Funktionalität sowie kritische
      Reflexion des
      Vorgehens.
    - Vorstellung der wissenschaftlichen Thesis, Modellierung (UML), Entwurfsmuster, C#-Spezifika.
- **Vortragszeit:** Max. 15 Minuten pro Teammitglied.
- **Bearbeitungszeit:** Max. 6 Wochen.
- **Abgabebestandteile:**
    - Entwickelte Anwendung in diesem Git-Repo
    - Entwicklerdokumentation im Paket unter docs
    - Entsprechend dokumentierter Quellcode (Kommentare, Readme-Dateien etc.)
    - Testfälle im entsprechenden Testprojekt
- **Code Contribution:** Regelmäßige Commits im Gruppen-Branch, dokumentiert durch aussagekräftige Pull Requests, werden
  erwartet.

## 3. Basisanforderungen (für alle Projekte/Pakete)

Jedes Projekt muss zwingend folgende Kriterien erfüllen:

- **BANF0:** Entwicklung in C# und den möglichst nativen Möglichkeiten von NET8
- **BANF1:** Übersichtliche Benutzeroberfläche (GUI oder Konsole nach Designkriterien)
- **BANF2:** Umsetzung einer Vererbungshierarchie und Nutzung von Polymorphie
- **BANF3:** Einhaltung der **SOLID-Prinzipien**
- **BANF4:** Persistenzschicht (Datei oder Datenbank) zum Speichern und Wiederherstellen von Zuständen
- **BANF5:** Implementierung von Unit-Tests für kritische Komponenten
- **BANF6:** Nutzung von **Asynchronität** (async/await) für mindestens eine Funktionalität
- **BANF7:** Je nach Thema Einsatz von enstprechenden Entwurfsmustern

## 4. Themenübersicht

Die Teamstärke beträgt jeweils 1–4 Personen18.

| **Nr.** | **Themenbereich**                    | **Kurzbeschreibung**                                                                |
|---------|--------------------------------------|-------------------------------------------------------------------------------------|
| 1       | **Fuhrparkverwaltung**               | PKW/LKW-Management, Fahrtenbuch, Reparaturbuch, Restbuchwerte.                      |
| 2       | **Büromittelorganisation**           | Bestandsführung, Ausgabe an Nutzer/Teams, Abschreibungen (AfA), Nachbestell-Logik.  |
| 3       | **Literaturdatenbank**               | Quellenverwaltung, Literaturverzeichnis (APA-Standard), Zitate, Suchfunktion.       |
| 4       | **Terminmanagement**                 | Multi-User Kalender, Ressourcenbuchung, Vorschlagsassistent, Aufgabenverwaltung.    |
| 5       | **Projektmanagement**                | Vorgänge, Ressourcen, Vor-/Nachfolger (Kalkulation), GANTT-Chart.                   |
| 6       | **Nachrichtendienst**                | Lokal-Netzwerk Chat, Dateitransfer, Verschlüsselung (Cäsar/RSA).                    |
| 7       | **Schach**                           | Mensch vs. Mensch, Speichern/Laden, KI-Gegenspieler, Timed-Mode.                    |
| 8       | **Zooverwaltung**                    | Digitale Tierakte, Gehegeplanung (Anforderungen), Mitarbeiterverwaltung.            |
| 9       | **"Finde die Minen"**                | Spielfeld-Generator, Schwierigkeitsgrade, Highscore/Counter, Multiplayer.           |
| 10      | **Rezepte-Datenbank**                | Rezeptanlage, Vorratsschrank-Verwaltung, Verfallsdaten, ökonomischer Kochvorschlag. |
| 11      | **Lernquiz**                         | Themenbasierte Fragen, Top-10 Fehler-Analyse, Timed-Modus, Lernkarten.              |
| 12      | **Vier-Gewinnt**                     | Spielmechanik am PC, KI-Gegenspieler, dynamische Feldgröße, Gewinnchance-Analyse.   |
| 13      | **Fitness-Tracker**                  | Trainingspläne, Fortschrittsanalyse, Zielsetzung, Ernährungsprotokoll.              |
| 14      | **Budgetverwaltung**                 | Einnahmen/Ausgaben, Kategorien, Monatsberichte, Sparziele.                          |
| 15      | **Wetterstation**                    | Sensoranbindung (simuliert), Datenvisualisierung, Vorhersagemodell.                 |
| 16      | **Musikverwaltung**                  | Playlists, Metadaten-Editor, Wiedergabelisten-Shuffle, Musikbibliothek.             |
| 17      | **Reiseplaner**                      | Reiseziele, Unterkünfte, Aktivitäten, Budgetplanung, Kalenderintegration.           |
| 18      | **Schach-Turnierverwaltung**         | Spielerprofile, Turnierbaum, Spielpaarungen, Ergebnisverwaltung.                    |
| 19      | **Inventarverwaltung**               | Lagerbestände, Lieferantenmanagement, Bestellwesen, Inventur.                       |
| 20      | **Kundenbeziehungsmanagement (CRM)** | Kundenprofile, Interaktionshistorie, Verkaufschancen, Berichte.                     |
| 21      | **E-Commerce-Shop**                  | Produktkatalog, Warenkorb, Bestellabwicklung, Zahlungsintegration.                  |
| 22      | **Blog-Plattform**                   | Beitragserstellung, Kommentarsystem, Benutzerverwaltung, Tagging.                   |
| 23      | **Ticket-System**                    | Ticket-Erstellung, Priorisierung, Status-Tracking, Benutzerrollen.                  |
| 24      | **Social-Media-Analyzer**            | Datenimport, Trendanalyse, Visualisierung, Sentiment-Analyse.                       |
| 25      | **Online-Umfrage-Tool**              | Umfrageerstellung, Teilnehmerverwaltung, Ergebnisanalyse, Exportfunktionen.         |
| 26      | **Zeiterfassungssoftware**           | Projektzeiterfassung, Berichte, Stundenzettel, Abrechnungsfunktionen.               |
| 27      | **Online-Banking-Simulator**         | Kontoverwaltung, Transaktionen, Budgetplanung, Sicherheitsfunktionen.               |
| 28      | **Virtuelles Whiteboard**            | Zeichenwerkzeuge, Kollaboration in Echtzeit, Speichern/Laden von Boards.            |
| 29      | **E-Learning-Plattform**             | Kursverwaltung, Fortschrittsverfolgung, Quiz-Integration, Zertifikate.              |
| 30      | **Personalmanagement-System**        | Mitarbeiterprofile, Urlaubsverwaltung, Leistungsbeurteilungen, Berichte.            |

---
