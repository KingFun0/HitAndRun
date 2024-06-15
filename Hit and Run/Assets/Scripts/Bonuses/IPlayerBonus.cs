using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scriipts.Bonuses
{
    internal interface IPlayerBonus //интерфейс бонусов
    {
        public abstract void ApplyBonus(BonusController playerController);
    }
}
