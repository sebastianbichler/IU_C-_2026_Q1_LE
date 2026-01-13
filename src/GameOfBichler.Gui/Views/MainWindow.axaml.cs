using Avalonia.Controls;
using Avalonia.Input;
using GameOfBichler.Gui.Models;
using GameOfBichler.Gui.ViewModels;

namespace GameOfBichler.Gui.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandler(object? sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                switch (e.Key)
                {
                    case Key.Up: case Key.W: viewModel.Move(Direction.Up); break;
                    case Key.Down: case Key.S: viewModel.Move(Direction.Down); break;
                    case Key.Left: case Key.A: viewModel.Move(Direction.Left); break;
                    case Key.Right: case Key.D: viewModel.Move(Direction.Right); break;
                }
            }
        }
    }
}
