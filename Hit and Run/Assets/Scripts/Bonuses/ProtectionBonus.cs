using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;

namespace Assets.Scriipts.Bonuses
{
    public class ProtectionBonus : IPlayerBonus //бонус защиты
    {
        public void ApplyBonus(BonusController player) //включение работы бонуса
        {
            player.EnableProtection();
            player.StartCoroutine(RemoveBonus(player));
        }

        private IEnumerator RemoveBonus(BonusController player) //выключение работы бонуса
        {
            yield return new WaitForSeconds(5f); //задержка на 5 секунд
            player.DisableProtection();
        }
    }
}
