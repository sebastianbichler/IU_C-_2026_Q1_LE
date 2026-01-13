using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollenspiel
{
    internal abstract class Entitity
    {
        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public int MaxDamage { get; set; }

        public Entitity (string name, int HealthPoints, int MaxDamage)
        {
            this.Name = name;
            this.HealthPoints = HealthPoints;
            this.MaxDamage = MaxDamage;
        }

        public void takeDamage(int Damage)
        {
            HealthPoints -= Damage;
            if(HealthPoints < 0) HealthPoints = 0;
            Console.WriteLine($"{Name} killed {Damage} damage! (Leftover-HP: {HealthPoints})");
        }
        public bool isDeath()
        {
            return HealthPoints <= 0;
        }

    }
}
