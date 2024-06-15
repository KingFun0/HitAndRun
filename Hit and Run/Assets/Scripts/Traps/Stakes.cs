using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scriipts.Traps
{
    public class Stakes: ITrap //ловушка - колья
    {
        public void FallIntoTrap(TrapController trap) //попадание в ловушку
        {
            trap.GetDamage(2);
        }
    }
}
