using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrapController : MonoBehaviour
{

   public void GetDamage(int damage)
   {
        if (!Player.isProtected)
        {
            Player.Instance.level -= damage;
        }
        else
        {
            Player.isProtected = false;
        }
        Debug.Log("ловушка");
    }
}
