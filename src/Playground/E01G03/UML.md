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
```

```mermaid
graph TD
    %% --- START ---
    Start((◉ Start)) --> Idle[Warten auf Ereignis]

    %% --- SWIMLANES ---
    subgraph Player_Event [Spieler Input]
        Idle -.-> |Taste gedrückt| CalcPlayer[Ziel-Position berechnen]
        CalcPlayer --> CheckWalkable{Ist Feld begehbar?}
        
        CheckWalkable -- Nein/Wand --> MergeUI[Zusammenführung]
        CheckWalkable -- Ja --> SetPPos[Spieler Position aktualisieren]
        SetPPos --> CheckWin{Ziel erreicht?}
        
        CheckWin -- Ja --> Win((GEWONNEN))
        CheckWin -- Nein --> MergeUI
    end

    subgraph Enemy_Tick [Timer Event alle 2s]
        Idle -.-> |Timer Tick| CheckStun{Ist Gegner betäubt?}
        
        %% Der "Kein Warten" Pfad: Einfach überspringen
        CheckStun -- Ja --> MergeUI
        
        CheckStun -- Nein --> Pathfind[Weg zum Spieler berechnen]
        Pathfind --> CheckObs{Was ist im Weg?}
        
        %% Fall: Weg frei
        CheckObs -- Frei/Pfeil --> MoveEnemy[Gegner bewegen]
        MoveEnemy --> CheckCatch{Spieler gefangen?}
        CheckCatch -- Ja --> GameOver((GAME OVER))
        CheckCatch -- Nein --> MergeUI

        %% Fall: Stein (Explosion)
        CheckObs -- Stein --> DestroyStone[Stein entfernen]
        DestroyStone --> TriggerAnim[Event: Explosion & Sound]
        TriggerAnim --> SetStun[Gegner: StunnedUntil setzen]
        SetStun --> MergeUI
        
        %% Fall: Wand (Blockiert)
        CheckObs -- Wand --> MergeUI
    end

    subgraph System_UI [Rendering & UI]
        MergeUI --> UpdateView[UpdateView: Kacheln neu zeichnen]
        UpdateView --> Idle
    end
    
    %% Styling
    style Start fill:#2ecc71,stroke:#27ae60,color:white
    style Win fill:#f1c40f,stroke:#f39c12,color:black
    style GameOver fill:#e74c3c,stroke:#c0392b,color:white
    style Idle fill:#ecf0f1,stroke:#bdc3c7,stroke-dasharray: 5 5
    style MergeUI fill:#3498db,stroke:#2980b9,color:white,shape:circle
```
