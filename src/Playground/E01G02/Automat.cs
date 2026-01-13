using System;

namespace Playground.E01G02;

class Automat
{
    public static void ProcessPayment()
    {
        Console.Clear();
        Random rand = new();
        int price = rand.Next(5, 101); // Random price between 5 and 100
        Console.WriteLine($"The asking price is: {price}");

        int totalPaid = 0;
        while (totalPaid < price)
        {
            Console.Write("Insert bill (5, 10, or 20): ");
            if (!int.TryParse(Console.ReadLine(), out int bill))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            if (bill != 5 && bill != 10 && bill != 20)
            {
                Console.WriteLine("Invalid bill. Only 5, 10, or 20 accepted.");
                continue;
            }
            totalPaid += bill;
            Console.WriteLine($"Total paid so far: {totalPaid}");
        }

        int change = totalPaid - price;
        if (change > 0)
        {
            Console.WriteLine($"Change to return: {change}");
            // Calculate change in bills
            int twenties = change / 20;
            change %= 20;
            int tens = change / 10;
            change %= 10;
            int fives = change / 5;
            change %= 5; // Should be 0 since we only give in 5s

            if (twenties > 0) Console.WriteLine($"{twenties} x 20");
            if (tens > 0) Console.WriteLine($"{tens} x 10");
            if (fives > 0) Console.WriteLine($"{fives} x 5");
        }
        else
        {
            Console.WriteLine("Exact payment received. No change.");
        }
    }
}