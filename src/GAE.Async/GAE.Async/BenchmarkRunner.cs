using System.Diagnostics;

namespace GAE.Async;

public record BenchmarkResult(
    long ElapsedMs,
    double AvgUpdateMs,
    int TotalFrames,
    List<long> History // Neu: Speichert die 5 Einzelmessungen
);
public class BenchmarkRunner
{
    public async Task<BenchmarkResult> RunSeriesAsync(IArcadeGame game, int userCount, int updatesPerUser, int iterations = 5)
    {
        await RunTestAsync(game, userCount, 10); // Warm-up
        var results = new List<BenchmarkResult>();

        for (int i = 0; i < iterations; i++)
        {
            results.Add(await RunTestAsync(game, userCount, updatesPerUser));
        }

        // Mittelwerte berechnen
        long avgDuration = (long)results.Average(r => r.ElapsedMs);
        double avgUpdate = results.Average(r => r.AvgUpdateMs);
        int avgFrames = (int)results.Average(r => r.TotalFrames);

        // Historie der Lastdauern extrahieren
        var history = results.Select(r => r.ElapsedMs).ToList();

        return new BenchmarkResult(avgDuration, avgUpdate, avgFrames, history);
    }

    private async Task<BenchmarkResult> RunTestAsync(IArcadeGame game, int userCount, int updatesPerUser)
    {
        var cts = new CancellationTokenSource();
        var sw = new Stopwatch();
        long totalUpdateTicks = 0;
        int frameCount = 0;

        var loop = Task.Run(async () => {
            game.Initialize();
            while (!cts.Token.IsCancellationRequested)
            {
                var fSw = Stopwatch.StartNew();
                game.Update(0.016);
                fSw.Stop();
                Interlocked.Add(ref totalUpdateTicks, fSw.ElapsedTicks);
                Interlocked.Increment(ref frameCount);
                await Task.Delay(16);
            }
        });

        sw.Start();
        await Task.WhenAll(Enumerable.Range(0, userCount).Select(id => Task.Run(() => {
            for (int i = 0; i < updatesPerUser; i++)
                game.OnNetworkUpdateReceived(new PlayerUpdate(id, i, i));
        })));
        sw.Stop();
        cts.Cancel();
        await loop;

        // Präzise Zeitberechnung über Hardware-Frequenz
        double totalMs = (double)totalUpdateTicks / Stopwatch.Frequency * 1000;
        double avgMs = totalMs / Math.Max(1, frameCount);

        return new BenchmarkResult(sw.ElapsedMilliseconds, avgMs, frameCount, new List<long> { sw.ElapsedMilliseconds });
    }
}
