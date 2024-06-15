using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scriipts.Traps
{
    public class WheelWithStakes: ITrap //ловушка - колесо с кольями
    {
        public void FallIntoTrap(TrapController trap) // попадание в ловушку
        {
            trap.GetDamage(5);
        }
    }
}
