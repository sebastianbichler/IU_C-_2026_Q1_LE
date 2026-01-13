using Rollenspiel;

namespace Playground.E01G01a;

public class Program : Launcher
{
    public override void Run()
    {
        Console.Clear();
        Console.WriteLine("E01G01a: Put your code for Exercise 1 here.");

        //TODO: Klassen etablieren Mage/Swordsman und den Spieler diese auswählen lassen,
        //die dann verschiedene Fähigkeiten haben
        Hero player = new Hero("Player1");

        //TODO: Verschiedene Listen mit Monstern implementieren für Schwierigkeit
        List<Monster> enemyList = new List<Monster>();
        enemyList.Add(new Monster("Orc1", 30, 10));
        enemyList.Add(new Orc());

        Console.WriteLine("--- Fight Starts ---");


        //Console.WriteLine($"[DEBUG] Monsers: {enemyList.Count}");
        //Console.WriteLine($"[DEBUG] Player HP: {player.healthPoints}");
        //Console.WriteLine($"[DEBUG] Player is dead? {player.isDeath()}");

        foreach (var enemy in enemyList)
        {
            if (player.IsDeath()) break;

            Console.WriteLine($"\n A Wild {enemy.Name} appeard!");

            while (!enemy.IsDeath() && !player.IsDeath())
            {
                Console.WriteLine("\n (A) Attack or (H) Healing");
                string action = Console.ReadLine();

                if (action.ToLower() == "a")
                {
                    player.Attack(enemy);
                }
                else
                {
                    player.Healing();
                }

                //enemy turn
                if (!enemy.IsDeath())
                {
                    enemy.Attack(player);
                }

                Console.WriteLine($"Player health: {player.HealthPoints}");
                Console.WriteLine($"Enemy health: {enemy.HealthPoints}");
            }

            if (player.IsDeath()) Console.WriteLine("You died...");
            else Console.WriteLine($"{enemy.Name} was slain.");

        }

        Console.WriteLine("Quit Game");
        Console.ReadKey();

    }
}
