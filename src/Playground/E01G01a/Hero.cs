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
        public Hero(string name) : base(name, 100, 20) { }

        public void Attack(Entitity target)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{name} swings the sword {target.name}!");

            Random rnd = new Random();
            int damage = rnd.Next(5, maxDamage);

            target.takeDamage(damage);
            Console.ResetColor();
        }

        public void Healing()
        {
            healthPoints += 15;
            healthPoints += 15;
            Console.WriteLine($"{name} drinks a health potion. HP: {healthPoints}");
        }
    }
}
