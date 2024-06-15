using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scriipts.Bonuses
{
    public class AddingLevelsBonus : IPlayerBonus //бонус добавления уровней
    {
        public void ApplyBonus(BonusController player) // применение бонуса
        {
            player.AddLevels();
        }
    }
}
