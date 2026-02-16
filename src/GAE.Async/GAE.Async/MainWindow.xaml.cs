using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GAE.Async;

public partial class MainWindow : Window
{
    private readonly BenchmarkRunner _runner = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnStart_Click(object sender, RoutedEventArgs e)
    {
        BtnStart.IsEnabled = false;
        if (!int.TryParse(TxtUsers.Text, out int users)) return;

        // --- LOCK TEST ---
        LblLock.Text = "Serie läuft (5x)...";
        var lockResult = await _runner.RunSeriesAsync(new LockBasedGame(), users, 1000);
        // HIER: LblLockHistory als dritten Parameter hinzufügen
        UpdateResultDisplay(LblLock, LblLockFps, LblLockHistory, lockResult);

        await Task.Delay(500);

        // --- CHANNEL TEST ---
        LblChannel.Text = "Serie läuft (5x)...";
        var channelResult = await _runner.RunSeriesAsync(new ChannelBasedGame(), users, 1000);
        // HIER: LblChannelHistory als dritten Parameter hinzufügen
        UpdateResultDisplay(LblChannel, LblChannelFps, LblChannelHistory, channelResult);

        BtnStart.IsEnabled = true;
    }

    private void UpdateResultDisplay(TextBlock label, TextBlock fpsLabel, TextBlock historyLabel, BenchmarkResult res)
    {
        label.Text = $"{res.ElapsedMs}ms Ø Lastdauer\n{res.AvgUpdateMs:F4}ms Ø Update";

        // Einzelwerte anzeigen (z.B. "Messungen: 22ms, 25ms, 21ms...")
        historyLabel.Text = "Einzelwerte: " + string.Join("ms, ", res.History) + "ms";

        double fps = res.AvgUpdateMs > 0 ? 1000 / res.AvgUpdateMs : 9999;
        fpsLabel.Text = $"{Math.Min(fps, 9999):F0} FPS";

        // Dynamische Farbe
        if (fps >= 60) fpsLabel.Foreground = System.Windows.Media.Brushes.SpringGreen;
        else if (fps >= 30) fpsLabel.Foreground = System.Windows.Media.Brushes.Orange;
        else fpsLabel.Foreground = System.Windows.Media.Brushes.Crimson;
    }
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }

    // Hilfsmethode: Fenster schließen
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
