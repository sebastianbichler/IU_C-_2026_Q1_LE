using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rollenspiel
{
    internal class Monster : Entitity, IAttacker
    {
        public Monster(string name, int hp, int schaden) : base(name, hp, schaden) { }

        public void Attack(Entitity target)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{name} hits {target.name}!");

            Random rnd = new Random();
            int damage = rnd.Next(1, maxDamage);

            target.takeDamage(damage);
            Console.ResetColor();
        }
    }
}
