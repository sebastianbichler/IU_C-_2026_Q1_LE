### Programmieren mit C# (DSPC016)

---

# Monaden Part I - Anwendung in C#

## Definition

**Monads** sind ein Konzept aus der funktionalen Programmierung, das es ermöglicht, Berechnungen zu strukturieren und
Nebenwirkungen zu handhaben. In C# werden Monads oft durch bestimmte Typen und Methoden implementiert, die es erlauben,
Werte in einem Kontext zu verarbeiten.

Mit Blick auf LINQ lässt sich eine Monade am besten als ein „Design Pattern für Pipelines“ verstehen. Viele Entwickler
nutzen Monaden in C# täglich, ohne den mathematischen Begriff dahinter zu kennen.

Eine Monade ist ein Behälter (Wrapper) für einen Wert, der zwei Dinge kann:

    Einpacken: Einen einfachen Wert in den Behälter stecken (z. B. aus einer Zahl 5 eine Liste [5] machen).

    Verketten (Chaining): Eine Funktion auf den Inhalt anwenden, wobei das Ergebnis automatisch wieder im Behälter landet, ohne dass man sich um die „Logik“ des Behälters (wie Fehlerprüfung oder Schleifen) kümmern muss.

---

## LINQ als „Monaden-Maschine“

In C# ist das bekannteste Beispiel für eine Monade das Interface IEnumerable<T>.

Stell dir vor, du hast eine Liste von Kunden und möchtest deren Bestellungen abrufen. Ohne Monaden-Logik müsstest du
verschachtelte foreach-Schleifen schreiben. Mit LINQ nutzt du die monadische Struktur:

````
C#
var bestellungen = kunden
.SelectMany(k => k.Orders); // Das ist die "Bind"-Operation der Monade
````

var bestellungen = kunden
.SelectMany(k => k.Orders); // Das ist die "Bind"-Operation der Monade

Die drei Bestandteile einer Monade in LINQ:

    Der Typ-Konstruktor: Ein generischer Typ wie IEnumerable<T>, Task<T> oder Nullable<T>. Er „umhüllt“ den eigentlichen Datentyp.

    Die Unit-Funktion (Einpacken): In C# gibt es dafür oft keinen einzelnen Namen, aber es ist der Moment, in dem du eine Sequenz erstellst, z. B. new List<int> { 5 } oder Task.FromResult(5).

    Die Bind-Funktion (Verketten): Das ist das Herzstück. In LINQ heißt diese Funktion SelectMany. Sie nimmt einen Wert aus dem Behälter, wendet eine Funktion an (die wieder einen Behälter zurückgibt) und „flacht“ das Ergebnis ab.

## Warum ist das nützlich?

Das monadische Prinzip von LINQ erlaubt es, Logik von der Infrastruktur zu trennen:
Monade Der "Behälter" kümmert sich um... Beispiel in LINQ
IEnumerable Das Iterieren über mehrere Elemente. SelectMany
Task Das Warten auf asynchrone Ergebnisse. await (ähnelt Bind)
Nullable / Option Den Check auf null (leerer Behälter). ?. oder spezielle LINQ-Extensions

4. Die LINQ-Query-Syntax

Ein cooler Beweis dafür, dass LINQ auf Monaden basiert, ist die Query-Syntax. Der Compiler übersetzt diese Zeilen blind
in monadische Aufrufe:
C#

var query = from kunde in kunden // Nimm Wert aus Behälter 1
from bestellung in kunde.Orders // Verkette mit Behälter 2
select bestellung; // Packe Ergebnis in neuen Behälter

Dies wird vom Compiler exakt in kunden.SelectMany(kunde => kunde.Orders, ...) übersetzt.

## Zusammenfassung

Eine Monade in LINQ ist ein Prinzip, das es dir erlaubt, Operationen nacheinander auszuführen, während der "Behälter" (
wie IEnumerable) im Hintergrund die schmutzige Arbeit erledigt (z. B. durch Listen gehen, auf Daten warten oder
Null-Checks machen).
