using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollenspiel
{
    internal interface IAttacker
    {
        public void Attack(Entitity Target);
    }
}
