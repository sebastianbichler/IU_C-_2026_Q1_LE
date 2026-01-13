using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rollenspiel
{
    internal class Hero : Entitity, IAttacker
    {
        public Hero(string Name) : base(Name, 100, 20) { }

        public void Attack(Entitity target)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{Name} swings the sword {target.Name}!");

            Random rnd = new Random();
            int Damage = rnd.Next(5, MaxDamage);

            target.takeDamage(Damage);
            Console.ResetColor();
        }

        public void Healing()
        {
            HealthPoints += 15;
            HealthPoints += 15;
            Console.WriteLine($"{Name} drinks a health potion. HP: {HealthPoints}");
        }
    }
}
