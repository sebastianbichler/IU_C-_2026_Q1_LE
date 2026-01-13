using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rollenspiel
{
    internal class Orc : Monster
    {
        public Orc() : base("Grok the Orc", 200, 30) { }

        public virtual void Attack(Entitity Target)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Name} spits fire on {Target.Name}!");

            Target.takeDamage(25);
            Console.ResetColor();
        }
    }
}
