using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace GameOfBichler.Gui.ViewModels
{
    public class TileViewModel : ObservableObject
    {
        private string _symbol = "";
        private IBrush _color = Brushes.Transparent;
        private Bitmap? _image = null;

        private bool _isExploding = false;

        public string Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value);
        }

        public IBrush Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public Bitmap? Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public bool IsExploding
        {
            get => _isExploding;
            set => SetProperty(ref _isExploding, value);
        }
    }
}
