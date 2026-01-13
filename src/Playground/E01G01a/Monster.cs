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
        public Monster(string Name, int hp, int schaden) : base(Name, hp, schaden) { }

        public void Attack(Entitity Target)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Name} hits {Target.Name}!");

            Random rnd = new Random();
            int Damage = rnd.Next(1, MaxDamage);

            Target.takeDamage(Damage);
            Console.ResetColor();
        }
    }
}
