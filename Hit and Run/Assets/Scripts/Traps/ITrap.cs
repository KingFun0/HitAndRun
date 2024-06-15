using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scriipts.Traps
{
    internal interface ITrap //интерфейс ловушек
    {
        public abstract void FallIntoTrap(TrapController trapController);
    }
}
