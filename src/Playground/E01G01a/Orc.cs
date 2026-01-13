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

        public virtual void Attack(Entitity target)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{name} spits fire on {target.name}!");

            target.takeDamage(25);
            Console.ResetColor();
        }
    }
}
