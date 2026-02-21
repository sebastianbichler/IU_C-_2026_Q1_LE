using GAE.Memory;
using Shared.Core; // Interface
using System.Buffers;
using System.Diagnostics;

// basePath holen
string basePath = AppContext.BaseDirectory;

// Unterordner, der von VS kopiert wurde
string bilderOrdner = Path.Combine(basePath, "AssetLoaderTest", "DemoPictures");

// DEBUG Check
// DEBUG Console.WriteLine($"Suche Bilder in: {bilderOrdner}");

// Falls keine Daten existieren, werden welche erstellt
if (Directory.GetFiles(bilderOrdner).Length == 0)
{
    Console.WriteLine("Generiere Test-Daten...");
    byte[] dummyData = new byte[1024 * 1024]; // 1 MB
    new Random().NextBytes(dummyData);
    for (int i = 0; i < 100; i++) File.WriteAllBytes(Path.Combine(bilderOrdner, $"asset_{i}.bin"), dummyData);
}


var dateien = Directory.GetFiles(bilderOrdner);
IAssetProvider loader = new PooledAssetProvider(bilderOrdner);

int loop = 1000;
// optionen Auswahl
Console.WriteLine("=== ASSET LOADER BENCHMARK ===");
Console.WriteLine($"Schleife läuft: {loop} durch");
Console.WriteLine("Drücke '1' für OPTIMIERT Durchlauf");
Console.WriteLine("Drücke '2' für UNOPTIMIERT Durchlauf");
Console.WriteLine("Drücke 'X' zum Beenden");


while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.X) break;

        if (key == ConsoleKey.D1)
        {
            await RunOptimizedLoop(loader, dateien);
        }
        else if (key == ConsoleKey.D2)
        {
            await RunBadLoop(dateien);
        }

            Console.WriteLine("\nDrücke '1' oder 'X'.");
    }
    await Task.Delay(100);
}

// --- optimierte Schleife ---
async Task RunOptimizedLoop(IAssetProvider loader, string[] files)
{
    GC.Collect();
    Console.WriteLine("\n>>> Starte OPTIMIERTEN Modus...");
    var sw = Stopwatch.StartNew();
    long startGarbage = GC.GetTotalAllocatedBytes(true);
    long totalBytes = 0;

    // durchlaufen
    for (int frame = 0; frame <= loop; frame++)
    {
        foreach (var file in files)
        {
            using (AssetData asset = await loader.RentAssetAsync(Path.GetFileName(file)))
            {
                totalBytes += VerarbeiteDatenSynchron(asset.Bytes);

            } // Dispose speicher freigeben
        }

        // Status-Update
        Console.Write($"\rRunde {frame}: {totalBytes / 1024 / 1024} MB verarbeitet.");
    }

    sw.Stop();
    //GC auf Müll untersuchen
    long endGarbage = GC.GetTotalAllocatedBytes(true);
    long garbageErzeugt = (endGarbage - startGarbage) / 1024 / 1024; // in MB umrechnen

    Console.WriteLine($"\nFertig! Zeit: {sw.ElapsedMilliseconds}ms.");
    Console.WriteLine($"NEUER MÜLL AUF DEM HEAP: {garbageErzeugt} MB");
}

// --- nicht optimierte Schleife ---
async Task RunBadLoop(string[] files)
{
    GC.Collect();
    Console.WriteLine("\n>>> Starte NAIVEN Modus (Standard C# - Heap Allokation)...");
    var sw = Stopwatch.StartNew();
    long startGarbage = GC.GetTotalAllocatedBytes(true);
    long totalBytes = 0;

    for (int frame = 0; frame <= loop; frame++)
    {
        foreach (var file in files)
        {
            // File.ReadAllBytes erzeugt bei jedem Aufruf ein brandneues byte[] Array auf dem Heap
            // Bei 205 MB Gesamtdaten erzeugen wir hier 205 MB puren Müll für den Garbage Collector.
            byte[] data = await File.ReadAllBytesAsync(file);

            totalBytes += data.Length;

            // Sobald die Schleife hier neu startet, ist 'data' nutzlos.
            // Der Garbage Collector muss es irgendwann mühsam wegräumen.
        }

        Console.Write($"\rRunde {frame}: {totalBytes / 1024 / 1024} MB verarbeitet.");
    }

    sw.Stop();
    //GC auf Müll untersuchen
    long endGarbage = GC.GetTotalAllocatedBytes(true);
    long garbageErzeugt = (endGarbage - startGarbage) / 1024 / 1024; // in MB umrechnen

    Console.WriteLine($"\nFertig! Zeit: {sw.ElapsedMilliseconds}ms.");
    Console.WriteLine($"NEUER MÜLL AUF DEM HEAP: {garbageErzeugt} MB");
}


// Diese Methode darf Span benutzen, weil sie NICHT 'async' ist.
// Sie läuft komplett auf dem Stack ab (Superschnell).
long VerarbeiteDatenSynchron(ReadOnlyMemory<byte> memory)
{
    // HIER DARFST DU SPAN BENUTZEN!
    ReadOnlySpan<byte> span = memory.Span;

    // Wir simulieren Arbeit: Einfach die Länge zurückgeben.
    // In einem echten Spiel würdest du hier Pixel parsen.
    return span.Length;
}
