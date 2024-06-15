using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : MonoBehaviour
{

    private float speed = 2f; //изменить при необходимости
    public TextMeshProUGUI textCoins;
    public TextMeshProUGUI textProtected;
    public TextMeshProUGUI textLevel;

    public void IncreaseSpeed(float amount) //увеличить скорость 
    {
        speed += amount;
        Player.Instance.SetSpeed(speed);
        Debug.Log("бонус");
    }

    public void DecreaseSpeed(float amount) //уменьшить скорость
    {
        speed -= amount;
        Player.Instance.SetSpeed(speed);
    }

    public void EnableProtection() //включить щит
    {
        Player.isProtected = true;
        Debug.Log("бонус");
    }

    public void DisableProtection() //выключить щит
    {
        Player.isProtected = false;
    }

    public void AddLevels() //добавить рандомное количество уровней от 1 до 10
    {
        Player.Instance.level += Random.Range(1, 10);
        Debug.Log("бонус");
    }

    private void FixedUpdate() //вывести на экран 
    {
        Debug.Log("coins");
        //textCoins.text = $"Монетки: {PlayerPrefs.GetInt("coins")}";
        textLevel.text = $"Уровень: {Player.Instance.level}";

        if (Player.isProtected)
            textProtected.text = "Включен ли щит: да";
        else if (!Player.isProtected)
            textProtected.text = "Включен ли щит: нет";
    }
}
