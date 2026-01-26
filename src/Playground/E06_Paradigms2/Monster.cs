namespace Playground.E06_Paradigms2;

public class Monter
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int Level { get; set; }

    public Monster(string name, int hp, int level)
    {
        Name = name;
        HP = hp;
        Level = level;
    }

    // Die Deconstruct-Methode ermöglicht das Tuple-ähnliche Auspacken
    public void Deconstruct(out string name, out int hp)
    {
        name = Name;
        hp = HP;
    }
}
