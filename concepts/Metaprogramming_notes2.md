---

# Metaprogrammierung & Introspektion (Notizen)

## 1. Intro: Die Philosophie der Selbsterkenntnis

**Hintergrund:**
Metaprogrammierung ist in C# kein „Add-on“, sondern das Fundament. Ohne Metaprogrammierung gäbe es kein Entity
Framework (Mapping), kein ASP.NET Core (Routing) und kein xUnit (Test-Discovery).

**Notizen:**

* „Stellen Sie sich vor, Ihr Code wäre nicht nur eine Liste von Befehlen, sondern ein Buch, das sich während des Lesens
  selbst umschreibt oder kommentiert.“
* „Wir unterscheiden zwischen **Introspektion** (Was bin ich?) und **Interzession** (Ich ändere, was ich bin). C# war
  lange Zeit primär auf Introspektion (Reflection) ausgelegt. Mit Source Generators haben wir nun ein Werkzeug zur
  statischen Interzession.“

---

## 2. Reflection: Anatomie des Typsystems

*Bezug zu Kotz/Wenz: Kapitel 14.1 – Das System.Type Objekt*

**Hintergrund:**
Jedes Objekt im .NET-Heap hat im Header einen Pointer auf seine **Method Table**. Diese Tabelle ist die
Laufzeit-Repräsentation des Typs. Reflection greift auf diese Metadaten zu, die in der Assembly (PE-Datei) im Manifest
gespeichert sind.

**Notizen:**

* „Reflection ist wie das Durchsuchen eines Dateisystems. Der `Type`-Knoten ist Ihr Root-Verzeichnis.“
* **Deep Dive Performance:** „Warum ist `Invoke()` langsam? Normaler Code wird vom JIT-Compiler in direkten
  Maschinencode übersetzt ( Sprung). Reflection muss zur Laufzeit die Methodensignatur prüfen, Parameter auf den Stack
  schieben (Boxing!) und Security-Checks durchführen. Das ist teuer.“
* **GAE-Bezug:** „In unserem Arcade-System nutzen wir Reflection, um DLLs zu laden, die wir beim Kompilieren noch gar
  nicht kannten. Das ist der Preis für absolute Flexibilität: Wir tauschen Geschwindigkeit gegen Dynamik.“

---

## 3. Attribute: Deklarative Architektur

*Bezug zu Kotz/Wenz: Kapitel 14.2 – Metadaten-Annotationen*

**Hintergrund:**
Attribute sind Instanzen von Klassen, die von `System.Attribute` erben. Das Besondere: Sie werden in den Metadaten der
Assembly „eingebacken“. Sie existieren dort als serialisierte Datenströme. Erst wenn man `GetCustomAttributes()`
aufruft, werden die Attribut-Objekte tatsächlich auf dem Heap konstruiert.

**Notizen:**

* „Attribute tun von sich aus absolut nichts. Sie sind wie Post-its an einer Akte. Erst wenn jemand (der
  Reflection-Code) die Notiz liest, passiert eine Aktion.“
* **Best Practice:** „Nutzen Sie `AttributeUsage`, um zu verhindern, dass Entwickler Ihr `[ArcadeGame]`-Attribut
  fälschlicherweise an eine lokale Variable hängen. Wir wollen Struktur erzwingen.“
* **Master-Wissen:** „Wussten Sie, dass Attribute auch für den Compiler wichtig sind? `[Obsolete]` löst
  Compiler-Warnungen aus, `[Conditional]` entfernt Methodenaufrufe komplett aus dem Release-Build. Das ist
  Metaprogrammierung auf Compiler-Ebene.“

---

## 4. Source Generators: Die Roslyn-Revolution

**Hintergrund (Deep Dive):**
Source Generators sind Teil des **Roslyn-Compiler-Workspace**. Früher war der Compiler eine Blackbox (Source in -> DLL
out). Heute ist er eine Pipeline. Source Generators erlauben es, während der **Syntax-Analyse** einzugreifen.

**Notizen:**

* „Warum verabschieden wir uns teilweise von Reflection? Wegen der Cloud und mobilen Endgeräten. **Native AOT** (
  Ahead-of-Time Compilation) hasst Reflection, weil der Linker nicht weiß, welche Typen zur Laufzeit dynamisch geladen
  werden könnten. Source Generators machen alles statisch und damit 'Linker-safe'.“
* **Das Caching-Geheimnis:** „Ein `IIncrementalGenerator` ist wie eine Excel-Tabelle. Wenn Sie eine Zelle (einen
  Code-Schnipsel) ändern, berechnet Roslyn nur die davon abhängigen Formeln (Generierungsschritte) neu. Das ist der
  Grund, warum Ihre IDE nicht einfriert, während Sie tippen.“
* **GAE-Bezug:** „In Gruppe 5 generieren wir das Hauptmenü. Wenn ein Student ein neues Spiel mit `[ArcadeGame]`
  markiert, erscheint es im Menü, noch bevor er das erste Mal auf 'Start' gedrückt hat. Das ist Magie durch Metadaten.“

---

## 5. Testing & Diagnostics (Der Qualitäts-Aspekt)

**Hintergrund:**
Metaprogrammierung ist schwer zu debuggen. Da der Code „unsichtbar“ generiert wird oder erst zur Laufzeit „entsteht“,
versagen klassische Unit-Tests oft.

**Notizen:**

* „Wie testet man Code, den man nicht sieht? Für Source Generators schreiben wir Tests, die den Output des Generators
  gegen einen Erwartungstext prüfen (Snapshot Testing).“
* „Für Reflection nutzen wir **Integrationstests**. Wir legen eine 'Dummy-DLL' in einen Ordner und schauen, ob unser
  `PluginLoader` sie korrekt erkennt. Metaprogrammierung erfordert defensive Programmierung: Prüfen Sie immer auf
  `null`, wenn Sie `GetMethod` aufrufen – ein Tippfehler im String und Ihr System stürzt ab.“

---

## Zusammenfassung für den Abschluss (The Big Picture)

1. **Reflection** ist für die **Offenheit** (Plugins, DLL-Loading).
2. **Attribute** sind für die **Beschreibung** (Metadaten, Regeln).
3. **Source Generators** sind für die **Effizienz** (Boilerplate-Eliminierung, AOT).

> Wenn jemand fragt: „Soll ich alles mit Source Generators machen?“, ist die Antwort: „Nur wenn
> die Informationen zur Kompilierzeit vorliegen. Wenn Sie ein Plugin-System bauen, bei dem User eigene DLLs hochladen,
> bleibt Reflection Ihr einziges Werkzeug.“
