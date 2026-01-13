using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollenspiel
{
    internal abstract class Entitity
    {
        protected string name { get; set; }
        protected int healthPoints { get; set; }
        protected int maxDamage { get; set; }

        public Entitity (string name, int healthPoints, int maxDamage)
        {
            this.name = name;
            this.healthPoints = healthPoints;
            this.maxDamage = maxDamage;
        }

        public void takeDamage(int damage)
        { 
            healthPoints -= damage;
            if(healthPoints < 0) healthPoints = 0;
            Console.WriteLine($"{name} killed {damage} damage! (Leftover-HP: {healthPoints})");
        }
        public bool isDeath()
        {
            return healthPoints <= 0;
        }

    }
}
