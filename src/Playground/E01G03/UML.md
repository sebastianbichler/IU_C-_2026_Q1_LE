# GameOfBichler - UML Klassendiagramm

Dieses Diagramm stellt die Architektur der Windows/Avalonia-Desktop-Anwendung dar. Es zeigt die Trennung zwischen **Model** (Logik), **ViewModel** (Vermittler) und **View** (UI).

```mermaid
classDiagram
    %% --- CORE TYPES & ENUMS ---
    class Direction {
        <<enumeration>>
        Up
        Down
        Left
        Right
    }

    class Position {
        +int X
        +int Y
        +Add(Direction dir) Position
    }

    %% --- MODELS (LOGIK) ---
    class IGridObject {
        <<interface>>
        +bool IsWalkable
        +OnPlayerEnter(Player p, GameBoard b)
    }

    class Stone {
        +IsWalkable : false
    }
    class Sun {
        +IsWalkable : true
    }
    class Arrow {
        +Direction ForceDirection
        +IsWalkable : true
    }
    class EndGoal {
        +IsWalkable : true
    }
    class EmptyTile {
        +IsWalkable : true
    }

    %% Implementierung des Interfaces
    Stone ..|> IGridObject
    Sun ..|> IGridObject
    Arrow ..|> IGridObject
    EndGoal ..|> IGridObject
    EmptyTile ..|> IGridObject

    class Player {
        +Position Position
        +int Score
        +AddScore(int amount)
    }

    class Enemy {
        +Position Position
        +DateTime StunnedUntil
    }

    class GameBoard {
        -Dictionary~Position, IGridObject~ _grid
        +Player Player
        +Enemy? Enemy
        +int Width
        +int Height
        +bool IsGameOver
        -- Events --
        +event Action~string~ OnMessage
        +event Action OnBoardChanged
        +event Action~Position~ OnExplosion
        -- Methoden --
        +Initialize(Player p)
        +SpawnEnemy(int x, int y)
        +MovePlayer(Direction dir)
        +MoveEnemy()
        +Reset()
        +ClearTile(Position pos)
        +GetObjectAt(Position pos) IGridObject
    }

    %% Beziehungen im Model
    GameBoard "1" *-- "1" Player : Composition
    GameBoard "1" o-- "0..1" Enemy : Aggregation
    GameBoard "1" o-- "*" IGridObject : enthält
    Player --> Position
    Enemy --> Position

    %% --- VIEWMODELS (MVVM) ---
    class ViewModelBase {
        <<abstract>>
        +SetProperty()
    }

    class TileViewModel {
        +string Symbol
        +IBrush Color
        +Bitmap? Image
        +bool IsExploding
    }

    class MainWindowViewModel {
        -GameBoard _board
        -DispatcherTimer _enemyTimer
        -Bitmap _enemyImage
        -Bitmap _explosionImage
        +ObservableCollection~TileViewModel~ GridTiles
        +string StatusText
        -- Commands --
        +StartNewGame()
        +Move(Direction dir)
        -- Private Logik --
        -UpdateView()
        -GenerateRandomLevel()
        -TriggerExplosionEffect(Position pos)
        -HandleExplosion(Position pos)
    }

    %% Beziehungen im ViewModel
    ViewModelBase <|-- TileViewModel
    ViewModelBase <|-- MainWindowViewModel
    MainWindowViewModel --> GameBoard : steuert
    MainWindowViewModel "1" *-- "*" TileViewModel : Liste für UI

    %% --- VIEW (UI) ---
    class MainWindow {
        +InitializeComponent()
    }

    %% Binding Beziehung
    MainWindow ..> MainWindowViewModel : DataContext Binding
