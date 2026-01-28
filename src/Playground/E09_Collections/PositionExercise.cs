using System.Diagnostics;

namespace Playground.E09_Collections;

public class PositionExercise : Launcher
{
    List<PositionClass> positionsClasses;
    List<PositionStruct> positionStructs;

    public override void Run()
    {
        CompareEfficiency();
    }

    private void CompareEfficiency()
    {
        // Erzeugt 100 PositionStruct-Instanzen (Index als Beispielwerte) und speichert sie in einer Liste.
        positionStructs = Enumerable.Range(0, 10000)
            .Select(i => new PositionStruct())
            .ToList();

        Console.WriteLine($"Created {positionStructs.Count} positions.");

        Random rnd = new Random();
        positionsClasses = Enumerable.Range(0, 10000)
            .Select(i => new PositionClass())
            .ToList();

        Console.WriteLine($"Created {positionsClasses.Count} positions.");

        int repeats = 1000;
        long checksum = 0;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // measuring with struct
        var sw = Stopwatch.StartNew();
        for (int r = 0; r < repeats; r++)
        {
            for (int i = 0; i < positionStructs.Count; i++)
            {
                var p = positionStructs[i];
                checksum += p.X + p.Y;
            }
        }

        sw.Stop();
        var structTime = sw.Elapsed;

        // GC.Collect();
        // GC.WaitForPendingFinalizers();
        // GC.Collect();

        // measuring with class
        sw.Restart();
        for (int r = 0; r < repeats; r++)
        {
            for (int i = 0; i < positionsClasses.Count; i++)
            {
                var p = positionsClasses[i];
                checksum += p.X + p.Y;
            }
        }

        sw.Stop();
        var classTime = sw.Elapsed;

        Console.WriteLine($"Struct iteration: {structTime.TotalMilliseconds} ms");
        Console.WriteLine($"Class iteration:  {classTime.TotalMilliseconds} ms");
        Console.WriteLine($"Checksum: {checksum}");
    }
}
