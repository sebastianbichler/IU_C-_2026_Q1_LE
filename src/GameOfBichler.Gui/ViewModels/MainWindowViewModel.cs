using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using GameOfBichler.Gui.Models;
using System.Collections.ObjectModel;

namespace GameOfBichler.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly GameBoard _board;
        private string _statusText = "";

        private readonly DispatcherTimer _enemyTimer;

        private readonly Bitmap _enemyImage;
        private readonly Bitmap _explosionImage;

        public ObservableCollection<TileViewModel> GridTiles { get; }

        public int Columns => _board.Width;
        public int Rows => _board.Height;

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public MainWindowViewModel()
        {
            try
            {
                _enemyImage = new Bitmap(AssetLoader.Open(new Uri("avares://GameOfBichler.Gui/Assets/krickel.jpg")));
                _explosionImage = new Bitmap(AssetLoader.Open(new Uri("avares://GameOfBichler.Gui/Assets/explosion.gif")));
            }
            catch (Exception ex)
            {
                StatusText = "Asset-Fehler: " + ex.Message;
                if (_enemyImage == null) _enemyImage = null!;
                if (_explosionImage == null) _explosionImage = null!;
            }

            var player = new Player(new Position(1, 1));
            _board = new GameBoard(10, 10, player);


            _board.OnBoardChanged += UpdateView;
            _board.OnMessage += (msg) => StatusText = msg;

            _board.OnExplosion += TriggerExplosionEffect;

            GridTiles = new ObservableCollection<TileViewModel>();
            for (int i = 0; i < _board.Width * _board.Height; i++)
            {
                GridTiles.Add(new TileViewModel());
            }

            _enemyTimer = new DispatcherTimer();
            _enemyTimer.Interval = TimeSpan.FromSeconds(2);
            _enemyTimer.Tick += (s, e) =>
            {
                if (_board.IsGameOver)
                {
                    _enemyTimer.Stop();
                    return;
                }
                _board.MoveEnemy();
            };

            StartNewGame();
        }

        private async void TriggerExplosionEffect(Position pos)
        {
            int index = pos.Y * _board.Width + pos.X;
            if (index >= 0 && index < GridTiles.Count)
            {
                var tile = GridTiles[index];

                tile.IsExploding = true;

                tile.Image = _explosionImage;
                tile.Symbol = "";
                tile.Color = Brushes.OrangeRed;

                await Task.Delay(500);

                tile.IsExploding = false;
            }

            UpdateView();
        }

        [RelayCommand]
        public void StartNewGame()
        {
            _board.Reset();
            StatusText = "Lauf weg! Der rote Gegner kommt.";
            GenerateRandomLevel();

            _enemyTimer.Start();
            UpdateView();
        }

        private void GenerateRandomLevel()
        {
            var rand = new Random();
            int width = _board.Width;
            int height = _board.Height;

            int stoneCount = 15;
            for (int i = 0; i < stoneCount; i++) PlaceRandomly(new Stone(), rand);

            GenerateArrowChain(rand, chainLength: 6);
            GenerateArrowChain(rand, chainLength: 4);

            int sunCount = 5;
            for (int i = 0; i < sunCount; i++) PlaceRandomly(new Sun(), rand);

            PlaceRandomly(new EndGoal(), rand);

            int attempts = 0;
            while (attempts < 100)
            {
                int x = rand.Next(_board.Width);
                int y = rand.Next(_board.Height);
                var pos = new Position(x, y);

                if ((x != 1 || y != 1) && _board.IsTileEmpty(pos))
                {
                    _board.SpawnEnemy(x, y);
                    break;
                }
                attempts++;
            }
        }

        private void GenerateArrowChain(Random rand, int chainLength)
        {
            Position currentPos;
            int attempts = 0;

            do
            {
                int x = rand.Next(_board.Width);
                int y = rand.Next(_board.Height);
                currentPos = new Position(x, y);
                attempts++;
                if (attempts > 100) return;
            }
            while (!_board.IsTileEmpty(currentPos) || (currentPos.X == 1 && currentPos.Y == 1));

            for (int i = 0; i < chainLength; i++)
            {
                var validNeighbors = new List<(Position pos, Direction dir)>();
                CheckNeighbor(currentPos, Direction.Up, validNeighbors);
                CheckNeighbor(currentPos, Direction.Down, validNeighbors);
                CheckNeighbor(currentPos, Direction.Left, validNeighbors);
                CheckNeighbor(currentPos, Direction.Right, validNeighbors);

                if (validNeighbors.Count == 0) break;

                var nextStep = validNeighbors[rand.Next(validNeighbors.Count)];
                _board.AddObject(currentPos.X, currentPos.Y, new Arrow(nextStep.dir));
                currentPos = nextStep.pos;
            }
        }

        private void CheckNeighbor(Position current, Direction dir, List<(Position, Direction)> list)
        {
            Position next = current.Add(dir);
            if (next.X >= 0 && next.X < _board.Width && next.Y >= 0 && next.Y < _board.Height)
            {
                if (_board.IsTileEmpty(next)) list.Add((next, dir));
            }
        }

        private void PlaceRandomly(IGridObject obj, Random rand)
        {
            int attempts = 0;
            while (attempts < 100)
            {
                int x = rand.Next(_board.Width);
                int y = rand.Next(_board.Height);
                var pos = new Position(x, y);

                if ((x != 1 || y != 1) && _board.IsTileEmpty(pos))
                {
                    _board.AddObject(x, y, obj);
                    break;
                }
                attempts++;
            }
        }

        public void Move(Direction dir)
        {
            _board.MovePlayer(dir);
        }

        private void UpdateView()
        {
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    int index = y * _board.Width + x;
                    var tileVM = GridTiles[index];

                    if (tileVM.IsExploding) continue;

                    var pos = new Position(x, y);

                    tileVM.Image = null;
                    tileVM.Symbol = "";
                    tileVM.Color = Brushes.WhiteSmoke;

                    if (_board.Player.Position.X == x && _board.Player.Position.Y == y)
                    {
                        tileVM.Symbol = "P";
                        tileVM.Color = Brushes.CornflowerBlue;
                    }
                    else if (_board.Enemy != null && _board.Enemy.Position.X == x && _board.Enemy.Position.Y == y)
                    {
                        tileVM.Image = _enemyImage;
                        tileVM.Color = Brushes.Transparent;
                    }
                    else
                    {
                        var obj = _board.GetObjectAt(pos);
                        tileVM.Symbol = GetSymbolFor(obj);
                        tileVM.Color = GetColorFor(obj);
                    }
                }
            }
        }

        private string GetSymbolFor(IGridObject obj) => obj switch
        {
            Stone => "#",
            Sun => "☀",
            EndGoal => "⚑",
            Arrow a => GetArrowSymbol(a.ForceDirection),
            _ => ""
        };

        private IBrush GetColorFor(IGridObject obj) => obj switch
        {
            Stone => Brushes.DarkGray,
            Sun => Brushes.Gold,
            EndGoal => Brushes.ForestGreen,
            Arrow => Brushes.MediumTurquoise,
            _ => Brushes.WhiteSmoke
        };

        private string GetArrowSymbol(Direction dir) => dir switch
        {
            Direction.Up => "↑",
            Direction.Down => "↓",
            Direction.Left => "←",
            Direction.Right => "→",
            _ => "?"
        };
    }
}
