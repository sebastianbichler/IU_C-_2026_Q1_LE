namespace E01G03;
class Fahrkarte
{
    public static void Fahrkartenautomat()
    {
        int ticketPreis = 4;
        do
        {
            Console.WriteLine("Der Fahrkartenpreis beträgt 4 Euro.");
            Console.Write("Bitte werfen Sie einen 5, 10 oder 20 Euro Schein ein: ");

            if (int.TryParse(Console.ReadLine(), out int eingabe))
            {
                switch (eingabe)
                {
                    case 5:
                        Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
                        break;
                    case 10:
                        Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
                        break;
                    case 20:
                        Console.WriteLine($"Ihr Rückgeld beträgt: {eingabe - ticketPreis} Euro.");
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe. Der Automat akzeptiert nur 5, 10 oder 20 Euro Scheine.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Fehler: Bitte geben Sie eine gültige Zahl ein.");
            }
            Console.ReadKey();
        }
        while (true);

    }
}
