using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour // скрипт предназначен для открытия закрытыхх уровней
{
    // массивы кнопок и спрайтов
    public Button[] buttonsLevels;
    public Sprite[] newSprite;

    private void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("levels") > 25)
            PlayerPrefs.SetInt("levels", 25);
        else if (PlayerPrefs.GetInt("levels") == 0)
            PlayerPrefs.SetInt("levels", 1);
        for (int i = 0; i < PlayerPrefs.GetInt("levels"); i++) // если i меньше чем пройденное кол-во уровней
        {
            if (buttonsLevels[i].image.sprite != newSprite[i]) // и если спрайты не равны
            {
                buttonsLevels[i].image.sprite = newSprite[i]; // то смена спрайта
                buttonsLevels[i].interactable = true;    // плюс включение работоспособности кнопки
            }
        }
    }

}
