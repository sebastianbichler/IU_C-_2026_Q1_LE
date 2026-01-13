using System;
using System.Collections.Generic;

namespace GameOfBichler.Gui.Models
{
    public class GameBoard
    {
        private readonly Dictionary<Position, IGridObject> _grid;

        public Player? Player { get; private set; }
        public Enemy? Enemy { get; private set; }
        public bool IsGameOver { get; set; } = false;

        public int Width { get; }
        public int Height { get; }

        public event Action<string>? OnMessage;
        public event Action? OnBoardChanged;
        public event Action<Position>? OnExplosion;

        public GameBoard(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new Dictionary<Position, IGridObject>();
        }

        public void Initialize(Player player)
        {
            Player = player;
        }

        public void SpawnEnemy(int x, int y)
        {
            Enemy = new Enemy(new Position(x, y));
        }

        public IGridObject GetObjectAt(Position pos)
        {
            if (_grid.TryGetValue(pos, out var obj)) return obj;
            return new EmptyTile();
        }

        public void AddObject(int x, int y, IGridObject obj)
        {
            _grid[new Position(x, y)] = obj;
        }

        public void ClearTile(Position pos)
        {
            if (_grid.ContainsKey(pos))
            {
                _grid[pos] = new EmptyTile();
                OnBoardChanged?.Invoke();
            }
        }

        public void SendMessage(string msg) => OnMessage?.Invoke(msg);

        public void MoveEnemy()
        {
            if (IsGameOver || Enemy == null || Player == null) return;

            if (DateTime.Now < Enemy.StunnedUntil)
            {
                return;
            }

            Position nextPos = CalculateNextEnemyStep();

            IGridObject targetObj = GetObjectAt(nextPos);

            if (targetObj is Stone)
            {
                ClearTile(nextPos);
                OnExplosion?.Invoke(nextPos);

                SendMessage("BOOM! Stein gesprengt!");
                Enemy.StunnedUntil = DateTime.Now.AddSeconds(3);

                OnBoardChanged?.Invoke();
                return;
            }

 
            if (targetObj.IsWalkable)
            {
                Enemy.Position = nextPos;
                CheckEnemyCatch();


                HandleEnemySlide();
            }

            OnBoardChanged?.Invoke();
        }

        private Position CalculateNextEnemyStep()
        {
            int dx = Player.Position.X - Enemy!.Position.X;
            int dy = Player.Position.Y - Enemy.Position.Y;

            int stepX = 0;
            int stepY = 0;

            if (Math.Abs(dx) > Math.Abs(dy)) stepX = dx > 0 ? 1 : -1;
            else stepY = dy > 0 ? 1 : -1;

            Position preferredPos = new Position(Enemy.Position.X + stepX, Enemy.Position.Y + stepY);


            IGridObject obj = GetObjectAt(preferredPos);
            if (!obj.IsWalkable && !(obj is Stone))
            {
                if (stepX != 0)
                {
                    stepY = dy > 0 ? 1 : -1;
                    if (dy == 0) stepY = 1; 
                    return new Position(Enemy.Position.X, Enemy.Position.Y + stepY);
                }
                else
                {
                    stepX = dx > 0 ? 1 : -1;
                    if (dx == 0) stepX = 1; 
                    return new Position(Enemy.Position.X + stepX, Enemy.Position.Y);
                }
            }

            return preferredPos;
        }

        private void HandleEnemySlide()
        {

            int safetyCounter = 0;

            while (safetyCounter < 20)
            {
                IGridObject currentObj = GetObjectAt(Enemy!.Position);

                if (currentObj is Arrow arrow)
                {
                    Position slideTarget = Enemy.Position.Add(arrow.ForceDirection);

                    IGridObject slideObj = GetObjectAt(slideTarget);

                    if (!slideObj.IsWalkable)
                    {
                        break;
                    }
                    Enemy.Position = slideTarget;

                    if (CheckEnemyCatch()) return;
                }
                else
                {
                    break;
                }
                safetyCounter++;
            }
        }

        private bool CheckEnemyCatch()
        {
            if (Enemy!.Position.X == Player.Position.X && Enemy.Position.Y == Player.Position.Y)
            {
                IsGameOver = true;
                SendMessage("GAME OVER! Du wurdest erwischt.");
                return true;
            }
            return false;
        }

        public void MovePlayer(Direction dir)
        {
            if (IsGameOver) return;
            if (Player == null) return;

            Position targetPos = Player.Position.Add(dir);

            if (targetPos.X < 0 || targetPos.X >= Width || targetPos.Y < 0 || targetPos.Y >= Height) return;

            IGridObject targetObject = GetObjectAt(targetPos);
            if (!targetObject.IsWalkable) return;

            Player.Position = targetPos;
            targetObject.OnPlayerEnter(Player, this);
            OnBoardChanged?.Invoke();
        }

        public void Reset()
        {
            _grid.Clear();
            IsGameOver = false;
            Player = new Player(new Position(1, 1));
            OnBoardChanged?.Invoke();
        }

        public bool IsTileEmpty(Position pos)
        {
            if (!_grid.ContainsKey(pos)) return true;
            return _grid[pos] is EmptyTile;
        }
    }
}
