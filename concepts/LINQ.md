### Programmieren mit C# (DSPC016)

---

## LINQ: Wissenschaftliche Grundlagen und Vergleich mit anderen Sprachen

**LINQ (Language Integrated Query)** ist eines der herausragendsten Merkmale von C# und markiert historisch den Punkt (
ca. 2007 mit C# 3.0), an dem funktionale Paradigmen massiv Einzug in die Mainstream-Programmierung hielten.

Wissenschaftlich betrachtet ist LINQ eine Implementierung von **Monaden** (insbesondere der List-Monade), die es
erlaubt, deklarative Abfragen über unterschiedliche Datenquellen (Objekte, XML, SQL) hinweg mit einer einheitlichen
Syntax zu formulieren.

### 1. Das funktionale Konzept hinter LINQ

LINQ basiert auf drei Säulen der funktionalen Programmierung:

1. **Immutability (Unveränderlichkeit):** LINQ-Operationen verändern die ursprüngliche Datenquelle nicht, sondern geben
   eine neue Projektion zurück.

2. **Lazy Evaluation (Verzögerte Ausführung):** Eine LINQ-Abfrage wird nicht ausgeführt, wenn sie definiert wird,
   sondern erst, wenn das Ergebnis tatsächlich iteriert wird (z. B. durch `foreach`). Dies wird durch das
   `IEnumerable<T>`-Interface und den Typ `IQueryable<T>` realisiert.

3. **Higher-Order Functions:** Funktionen wie `Where`, `Select` und `Aggregate` nehmen andere Funktionen (Lambdas) als
   Argumente entgegen.

---

### 2. Vergleich mit Java, C++ und Python

Hier zeigt sich die unterschiedliche Philosophie der Sprachen bei der Verarbeitung von Sequenzen:

| **Feature**            | **C# (LINQ)**             | **Java (Streams)**     | **C++ (Ranges/STL)**     | **Python (List Compr./Generators)** |
|------------------------|---------------------------|------------------------|--------------------------|-------------------------------------|
| **Einführung**         | 2007 (C# 3.0)             | 2014 (Java 8)          | 2020 (C++20 Ranges)      | 2000 / 2001                         |
| **Syntax**             | Fluent API & Query Syntax | Fluent API (Streams)   | Pipes `\\|` & Operatoren | Comprehensions / Map-Filter         |
| **Typisierung**        | Statisch & Reified        | Statisch (Erasure)     | Statisch (Templates)     | Dynamisch (Duck Typing)             |
| **Deferred Execution** | Ja (Standard)             | Ja (Terminal Op nötig) | Ja (Views)               | Ja (Generators / Iterators)         |
| **Provider-Modell**    | Ja (LINQ to SQL/EF)       | Eingeschränkt          | Nein                     | Nein                                |

#### C# Beispiel (LINQ)

C#

```
var result = students
    .Where(s => s.Grade < 2.0)
    .OrderBy(s => s.Name)
    .Select(s => s.Email);
```

#### Java Vergleich (Streams)

Java Streams sind LINQ sehr ähnlich, aber oft etwas "geschwätziger", da man explizit einen Stream öffnen und am Ende
wieder schließen (collecten) muss:

Java

```
List<String> result = students.stream()
    .filter(s -> s.getGrade() < 2.0)
    .sorted(Comparator.comparing(Student::getName))
    .map(Student::getEmail)
    .collect(Collectors.toList());
```

#### C++ Vergleich (Ranges)

Seit C++20 gibt es die `std::ranges`, die funktional ähnlich wirken, aber durch die Template-Metaprogrammierung zur
Kompilierzeit extrem performant sind:

C++

```
auto result = students
    | views::filter([](auto& s) { return s.grade < 2.0; })
    | views::transform([](auto& s) { return s.email; });
```

#### Python Vergleich

Python nutzt oft List Comprehensions, was für einfache Filterungen sehr elegant ist, aber bei komplexen Ketten
unübersichtlich werden kann:

Python

```
result = [s.email for s in students if s.grade < 2.0] # Unsortiert
# Oder funktional:
result = map(lambda s: s.email, filter(lambda s: s.grade < 2.0, students))
```

---

### 3. Wissenschaftliche Besonderheit: Query Provider

Was C# von Java und Python abhebt, ist die Unterscheidung zwischen IEnumerable (In-Memory) und IQueryable (Remote).

Wenn Sie in C# eine LINQ-Abfrage gegen eine Datenbank (via Entity Framework) schreiben, wird der Lambda-Ausdruck nicht
als kompiliertes Delegate übergeben, sondern als Expression Tree (ein abstrakter Syntaxbaum des Codes).

- **Der Provider** (z. B. für SQL Server) analysiert diesen Baum zur Laufzeit.

- Er übersetzt das C#-`Where` direkt in ein SQL-`WHERE`.

- Dadurch findet die Filterung in der Datenbank statt, nicht im Arbeitsspeicher der Applikation. Java benötigt hierfür
  oft separate Frameworks wie *QueryDSL* oder *Criteria API*, die weniger elegant integriert sind.

### 4. Fazit

LINQ ist ein exzellentes Beispiel, wie **deklarative Programmierung** ("Was möchte ich?") die
**imperative Programmierung** ("Wie soll der Computer iterieren?") ersetzt.

Ein interessanter Punkt für das höhere Semester wäre die Untersuchung von `Deferred Execution`. Ein häufiger Fehler ist
das mehrfache Iterieren einer LINQ-Abfrage, was jedes Mal die Berechnung (oder Datenbankabfrage) neu auslöst, sofern
nicht `.ToList()` aufgerufen wurde.

---

Das Konzept der **Expression Trees** ist einer der intellektuell spannendsten Aspekte von C#, da es die Grenze zwischen
Code und Daten aufhebt (ähnlich wie bei Lisp-Makros, aber in einer statisch typisierten Welt).

---

## 1. Das Konzept: Code as Data

In Java oder Python ist ein Lambda-Ausdruck in der Regel eine anonyme Methode – ein ausführbarer Block. In C# kann
derselbe Lambda-Ausdruck in zwei verschiedene Typen kompiliert werden, je nachdem, was die linke Seite erwartet:

1. **`Func<T, bool>` (Delegate):** Kompilierter IL-Code, direkt ausführbar (In-Memory).

2. **`Expression<Func<T, bool>>` (Expression Tree):** Eine Datenstruktur, die den Code beschreibt.

### Beispiel:

C#

```
// 1. In-Memory (Funktional)
Func<Student, bool> isSeniorFunc = s => s.Semester > 5;
// Wird ausgeführt als: if (s.Semester > 5) ...

// 2. Als Metadaten (Expression Tree)
Expression<Func<Student, bool>> isSeniorExpr = s => s.Semester > 5;
// Wird nicht ausgeführt, sondern als Baumstruktur gespeichert:
// "Eigenschaft 'Semester' vom Parameter 's' muss größer sein als Konstante '5'"
```

---

## 2. Struktur eines Expression Trees

Ein Expression Tree zerlegt den Code in seine atomaren Bestandteile. Für das obige Beispiel `s => s.Semester > 5` sieht
der Baum vereinfacht so aus:

- **LambdaExpression** (Die Wurzel)

    - **Parameters**: `s` (Typ: Student)

    - **Body** (BinaryExpression: `>`)

        - **Left**: MemberAccess (`s.Semester`)

        - **Right**: Constant (`5`)

---

## 3. Der Clou: Der LINQ-Provider (z.B. Entity Framework)

Wenn Sie `db.Students.Where(s => s.Semester > 5)` aufrufen, passiert folgendes:

1. Der C#-Compiler erkennt, dass `Where` eine `Expression` erwartet.

2. Er generiert den **Expression Tree** zur Kompilierzeit.

3. Zur Laufzeit nimmt der SQL-Provider (z.B. für PostgreSQL) diesen Baum und "wandert" ihn ab (Visitor Pattern).

4. Er sieht: `BinaryExpression` mit `GreaterThan`. Er schreibt: `WHERE`.

5. Er sieht: `MemberAccess` auf `Semester`. Er schreibt: `"Semester"`.

6. Er sieht: `Constant` 5. Er schreibt: `> 5`.

**Resultat:** Ein valides `SELECT * FROM Students WHERE Semester > 5;`.

---

## 4. Wissenschaftlicher Vergleich (Java & Co)

- **Java:** Java hat keine native Unterstützung für Expression Trees. Frameworks wie **Querydsl** nutzen stattdessen
  Code-Generierung (APT), um "Q-Klassen" zu erstellen. Das ist weniger elegant, da man nicht den echten Java-Code
  transformiert, sondern mit Hilfsobjekten eine Abfrage "nachbaut".

- **Python:** Python ist so dynamisch, dass man den AST (Abstract Syntax Tree) zur Laufzeit inspizieren könnte (Modul
  `ast`), aber es gibt keine tief in die Sprache integrierte Brücke zwischen "nativem Code" und "Datenbankabfragen" wie
  LINQ.

- **C++:** Hier passiert fast alles zur Kompilierzeit (Template-Metaprogrammierung). Ein "Laufzeit-Umbau" von Code in
  SQL-Strings ist in C++ ohne massive externe Bibliotheken und Reflection kaum machbar.

---

## 5. Experiment für die Vorlesung

Schreiben Sie einen eigenen kleinen "Compiler", der einen Expression Tree analysiert:

C#

```
Expression<Func<int, bool>> expr = x => x > 10;
var binary = (BinaryExpression)expr.Body;

Console.WriteLine($"Operator: {binary.NodeType}"); // Output: GreaterThan
Console.WriteLine($"Linke Seite: {binary.Left}");   // Output: x
Console.WriteLine($"Rechte Seite: {binary.Right}"); // Output: 10
```

---
