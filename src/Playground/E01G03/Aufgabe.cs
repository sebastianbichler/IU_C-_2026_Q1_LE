using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.E01G03;
class Aufgabe
{
    public static void Parser()
    {
        string rawData = "ID:42;TYPE:PLAYER;POS:10,20";
        ReadOnlySpan<char> span = rawData.AsSpan();

        int colon1 = span.IndexOf(':');
        int semi1 = span.IndexOf(';');

        ReadOnlySpan<char> idSlice = span.Slice(colon1 + 1, semi1 - colon1 - 1);
        int id = int.Parse(idSlice);

        ReadOnlySpan<char> afterFirstSemi = span.Slice(semi1 + 1);

        int colon2 = afterFirstSemi.IndexOf(':');
        int semi2 = afterFirstSemi.IndexOf(';');

        ReadOnlySpan<char> typeSlice = afterFirstSemi.Slice(colon2 + 1, semi2 - colon2 - 1);

        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"TYPE: {typeSlice.ToString()}");

        Console.WriteLine("\nTaste drücken zum Beenden...");
        Console.ReadKey();
    }
}
