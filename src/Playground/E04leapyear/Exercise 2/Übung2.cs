using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// namespace Playground.E01G01a.Exercise_2;
//
// internal class Übung2
// {
//     bool IstSchaltjahr(int jahr)
//     {
//         //Jedes Jahr, in dem das westliche Kalenderjahr durch 4 teilbar ist,
//         //ist ein Schaltjahr. Ein Jahr, in dem das AD - Jahr durch 100 teilbar ist,
//         //ist üblicherweise ein normales Jahr. Allerdings ist jedes Jahr,
//         //in dem das westliche Kalenderjahr durch 400 teilbar ist, ein Schaltjahr.
//
//         if (jahr % 400 == 0)
//             return true;
//         else if (jahr % 100 == 0)
//             return false;
//         else if (jahr % 4 == 0)
//             return true;
//         else
//             return false;
//     }
//
//     public void Vergleichsausdruecke()
//     {
//         //(i - 4) > (j* j)
//         //(i > 0) || (i == j)
//         //(i > -j) && (i<j)
//         //(i >= j) && (i != 0)
//     }
//
//     public void Fahrkartenautomat()
//     {
//         int ticketPreis = 4;
//         do
//         {
//             Console.WriteLine("Der Fahrkartenpreis beträgt 4 Euro.");
//             Console.Write("Bitte werfen Sie einen 5, 10 oder 20 Euro Schein ein: ");
//
//             if (int.TryParse(Console.ReadLine(), out int eingabe))
//             {
//                 switch (eingabe)
//                 {
//                     case 5:
//                         Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
//                         break;
//                     case 10:
//                         Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
//                         break;
//                     case 20:
//                         Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
//                         break;
//                     default:
//                         Console.WriteLine("Ungültige Eingabe. Der Automat akzeptiert nur 5, 10 oder 20 Euro Scheine.");
//                         break;
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("Fehler: Bitte geben Sie eine gültige Zahl ein.");
//             }
//
//             Console.ReadKey();
//         } while (true);
//     }
// }
//
// }
