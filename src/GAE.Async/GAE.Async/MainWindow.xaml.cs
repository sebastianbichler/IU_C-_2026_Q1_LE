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
        var lockResult = await _runner.RunSeriesAsync(new LockBasedGame(), users, 100);
        UpdateResultDisplay(LblLock, LblLockHistory, lockResult);

        await Task.Delay(500);

        // --- CHANNEL TEST ---
        LblChannel.Text = "Serie läuft (5x)...";
        var channelResult = await _runner.RunSeriesAsync(new ChannelBasedGame(), users, 100);
        UpdateResultDisplay(LblChannel, LblChannelHistory, channelResult);

        BtnStart.IsEnabled = true;
    }

    private void UpdateResultDisplay(TextBlock label, TextBlock historyLabel, BenchmarkResult res)
    {
        label.Text = $"{res.ElapsedMs}ms Ø Lastdauer\n{res.AvgUpdateMs:F4}ms Ø Update";
        historyLabel.Text = "Einzelwerte: " + string.Join("ms, ", res.History) + "ms";

        if (res.ElapsedMs <= 30) label.Foreground = System.Windows.Media.Brushes.SpringGreen;
        else if (res.ElapsedMs <= 100) label.Foreground = System.Windows.Media.Brushes.Orange;
        else label.Foreground = System.Windows.Media.Brushes.Crimson;
    }
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
