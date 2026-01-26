namespace Playground.E06_Paradigms2;

public class Monster(string name, int hp, int level)
{
    public string Name { get; set; } = name;
    public int HP { get; set; } = hp;
    public int Level { get; set; } = level;

    // Die Deconstruct-Methode ermöglicht das Tuple-ähnliche Auspacken
    public void Deconstruct(out string name, out int hp)
    {
        name = Name;
        hp = HP;
    }
}
