using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scriipts.Bonuses
{
    public class SpeedBonus : IPlayerBonus //бонус скорости
    {
        public void ApplyBonus(BonusController player) //включение работы бонуса
        {
            player.IncreaseSpeed(1);
            player.StartCoroutine(RemoveBonus(player));
        }

        private IEnumerator RemoveBonus(BonusController player) //выключение работы бонуса
        {
            yield return new WaitForSeconds(5f); //задержка на 5 секунд
            player.DecreaseSpeed(1);
        }
    }
}
