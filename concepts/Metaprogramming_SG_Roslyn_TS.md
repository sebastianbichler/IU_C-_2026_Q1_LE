### Programmieren mit C# (DSPC016)

---

# Troubleshooting: Mein Source Generator wird nicht angezeigt

Wenn der generierte Code nicht auftaucht oder die IDE rote Wellenlinien anzeigt, obwohl der Code da sein müsste, arbeite
diese Punkte nacheinander ab:

### 1. Die Projekt-Konfiguration (Der häufigste Fehler)

Der Generator **muss** ein `netstandard2.0` Projekt sein. Wenn dort `net8.0` steht, kann der Compiler (der oft noch auf
dem alten Framework läuft) den Generator nicht laden.

**Checkliste `.csproj` des Generators:**

* ` <TargetFramework>netstandard2.0</TargetFramework>`
* ` <EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>`

**Checkliste `.csproj` des Hauptprojekts (Consumer):**
Die Referenz muss so aussehen:

```xml
<ItemGroup>
  <ProjectReference Include="..\MeinGeneratorProjekt.csproj"
                    OutputItemType="Analyzer"
                    ReferenceOutputAssembly="false" />
</ItemGroup>

```

* `OutputItemType="Analyzer"` ist zwingend!
* `ReferenceOutputAssembly="false"` verhindert, dass die Generator-DLL in deinem fertigen Programm landet (sie wird nur
  beim Kompilieren gebraucht).

---

### 2. Sichtbarkeit: Wo ist der Code eigentlich?

Source Generators schreiben Dateien standardmäßig nicht auf die Festplatte, sondern nur in den Arbeitsspeicher des
Compilers.

**So findest du sie trotzdem:**

1. Gehe im **Solution Explorer** des Hauptprojekts auf **Dependencies**.
2. Klappe **Analyzers** auf.
3. Suche dein Generator-Projekt und klappe es auf.
4. Dort findest du die Datei (z.B. `GameRegistry.g.cs`). Ein Doppelklick öffnet die „virtuelle“ Datei.

**Pro-Tipp:** Wenn du die Dateien physisch auf der Platte sehen willst, füge dies in die `.csproj` des Hauptprojekts
ein:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>Generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>

```

---

### 3. Der „Visual Studio Schluckauf“ (Caching)

Visual Studio hält den Prozess des Generators oft im Hintergrund offen. Wenn du den Code deines Generators änderst,
merkst du im Hauptprojekt oft keine Änderung.

**Lösungsschritte:**

1. **Solution Clean & Rebuild:** Hilft in 50% der Fälle.
2. **Visual Studio Neustart:** Hilft in 90% der Fälle.
3. **Roslyn-Prozess töten:** Wenn du nicht neustarten willst, öffne den Task-Manager und beende alle Prozesse namens
   `VBCSCompiler.exe`.

---

### 4. Das `[Generator]` Attribut vergessen?

Es klingt trivial, aber ohne das Attribut `[Generator]` über der Klasse, die `IIncrementalGenerator` implementiert, wird
die Klasse vom Compiler schlicht ignoriert. Es gibt keine Fehlermeldung, es passiert einfach nichts.

---

### 5. Debugging für Mutige

Wenn gar nichts mehr geht, füge diese Zeile ganz am Anfang deiner `Initialize`-Methode im Generator ein:

```csharp
public void Initialize(IncrementalGeneratorInitializationContext context)
{
    #if DEBUG
    if (!System.Diagnostics.Debugger.IsAttached)
    {
        System.Diagnostics.Debugger.Launch();
    }
    #endif
    // ... restlicher Code
}

```

Wenn du das Projekt nun baust, springt ein Fenster auf und fragt dich, mit welcher Visual Studio Instanz du den
Generator debuggen willst. So kannst du Zeile für Zeile durch deinen Generator-Code gehen.

---

### 💡 Fazit für die Vorlesung:

**„Geduld ist eine Tugend.“** Source Generators sind extrem mächtig, aber die Werkzeuge
drumherum fühlen sich manchmal noch wie Beta-Software an. Ein sauberer Rebuild ist euer bester Freund.
