### Programmieren mit C# (DSPC016)

---

# Aspektorientierte Programmierung (AOP) in C#

---

**AOP** steht für **Aspect-Oriented Programming** (Aspektorientierte Programmierung).

Es ist ein Programmierparadigma, das dazu dient, sogenannte **querschnittliche Belange** (Cross-Cutting Concerns) aus
der eigentlichen Geschäftslogik auszulagern. In diesem Kurs ist dies ein wichtiges Konzept, um den Studierenden zu
zeigen, wie man "sauberen" Code schreibt, der nicht mit Infrastruktur-Code (wie Logging oder Security) "verschmutzt"
ist.

### 1. Das Problem: "Code Tangling" und "Code Scattering"

In der klassischen objektorientierten Programmierung (OOP) haben Sie oft Funktionen, die Sie in fast jeder Methode
benötigen, die aber nichts mit der eigentlichen Aufgabe der Methode zu tun haben.

- **Logging:** Jede Methode soll ihren Start und Ende loggen.

- **Validierung:** Jeder Parameter muss geprüft werden.

- **Transaktionsmanagement:** Datenbankoperationen müssen in eine Transaktion gekapselt werden.

- **Exception Handling:** Überall brauchen wir das gleiche Try-Catch-Muster.

Ohne AOP müssten Sie diesen Code in jede einzelne Funktion kopieren. Das Ergebnis ist unübersichtlicher Code, bei dem
die Geschäftslogik unter Bergen von Boilerplate-Code begraben liegt.

### 2. Die Lösung: Aspekte

AOP erlaubt es, diese Funktionen in separate Module – sogenannte **Aspekte** – zu packen. Diese Aspekte werden dann "
quer" über die bestehende Klassenstruktur gelegt.

### 3. Kernbegriffe der AOP

Wenn Sie AOP im Kurs erklären, sind diese vier Begriffe essenziell:

1. **Aspect (Aspekt):** Das Modul, das den querschnittlichen Code enthält (z. B. ein "Logging-Aspekt").

2. **Join Point (Verknüpfungspunkt):** Ein Punkt im Programmablauf, an dem ein Aspekt eingreifen könnte (z. B. der
   Aufruf einer Methode).

3. **Advice (Anweisung):** Der eigentliche Code, der ausgeführt wird (z. B. "Schreibe in die Log-Datei"). Es gibt
   *Before* (vor der Methode), *After* (nach der Methode) und *Around* (ersetzt/umschließt die Methode).

4. **Pointcut (Schnittmenge):** Ein Filter, der definiert, an welchen Join Points der Advice angewendet werden soll (z.
   B. "bei allen Methoden, die mit `Save...` beginnen").

### 4. Umsetzung in C#

Im Gegensatz zu Java (wo AspectJ sehr verbreitet ist) hat C# keine native, tief im Compiler integrierte
AOP-Unterstützung. Es gibt jedoch drei gängige Wege:

- Compile-Time Weaving (Source Generators / Metalama): Dies ist der modernste Weg (und das, was wir vorhin als "
  Lombok-Ersatz" besprochen haben). Ein Tool wie Metalama oder PostSharp analysiert den Code beim Kompilieren und
  schreibt den Aspekt-Code direkt in die Assembly.

  Beispiel: Sie markieren eine Methode mit [Log] und der Generator fügt automatisch die Console.WriteLine-Befehle ein.

- Dynamic Proxy (Interceptors):

  Bibliotheken wie Castle Windsor oder der Autofac Interceptor erstellen zur Laufzeit eine neue Klasse ("Proxy"), die
  Ihre Klasse umschließt und den Zusatzcode ausführt.

- Middleware (in ASP.NET Core):

  In der Webentwicklung ist das Middleware-Konzept eine Form von AOP. Jede Anfrage läuft durch eine Kette von
  Komponenten (Logging, Authentifizierung), bevor sie den Controller erreicht.

### 5. Beispiel für Ihren Kurs (Pseudocode / Metalama-Stil)

**Ohne AOP (OOP-Standard):**

C#

```
public void ProcessOrder(Order order)
{
    _logger.Log("Start Processing"); // Boilerplate
    if (order == null) throw new ArgumentNullException(); // Boilerplate

    // EIGENTLICHE LOGIK
    _db.Save(order);

    _logger.Log("End Processing"); // Boilerplate
}
```

**Mit AOP:**

C#

```
[Log] // Aspekt übernimmt Logging
[Validate] // Aspekt übernimmt Null-Check
public void ProcessOrder(Order order)
{
    _db.Save(order); // Nur noch die reine Logik!
}
```

### Fazit

AOP ist die konsequente Fortführung des **Single Responsibility Principle (SRP)**. Während OOP die Anwendung vertikal in
Module (Klassen) gliedert, erlaubt AOP eine horizontale Strukturierung für systemweite Aufgaben.
