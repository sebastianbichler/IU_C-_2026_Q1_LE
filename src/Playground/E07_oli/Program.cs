namespace Playground.E07_oli;

public class Program : Launcher
{
    public override void Run()
    {
        Console.Clear();

        OlisClass oli = new OlisClass();

        oli.IstSchaltjahr(2026);

        oli.Fahrkartenautomat();
    }
}
