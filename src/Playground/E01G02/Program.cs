namespace Playground.E01G02;

public class Program : Launcher
{
    public override void Run()
    {
        Console.Clear();

        string data = "ID:42;TYPE:PLAYER;POS:10,20";
        Console.WriteLine($"Data: {data}\n");

        ReadOnlySpan<char> span = data.AsSpan();

        var idValue = ExtractValue(span, "ID:");
        int id = int.Parse(idValue);
        Console.WriteLine($"ID: {id}");

        var typeValue = ExtractValue(span, "TYPE:");
        string type = typeValue.ToString();
        Console.WriteLine($"TYPE: {type}");

        var posValue = ExtractValue(span, "POS:");
        int commaIndex = posValue.IndexOf(',');
        int x = int.Parse(posValue.Slice(0, commaIndex));
        int y = int.Parse(posValue.Slice(commaIndex + 1));
        Console.WriteLine($"POS: X={x}, Y={y}");

    }

    private ReadOnlySpan<char> ExtractValue(ReadOnlySpan<char> span, string key)
    {
        int keyIndex = span.IndexOf(key.AsSpan());
        if (keyIndex == -1) return ReadOnlySpan<char>.Empty;

        int startIndex = keyIndex + key.Length;
        var reminder = span.Slice(startIndex);

        int semicolonIndex = reminder.IndexOf(';');

        return semicolonIndex == -1 ? reminder : reminder.Slice(0, semicolonIndex);
    }

}
