using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Pause: MonoBehaviour // скрипт отвечает за панель паузы
{

    public Behaviour player; // скрипт игрока


    public void PauseLevel()
    {
        Player.isPaused = true;
        Time.timeScale = 0; // отключение времени в игре
        //player.enabled = false; //€ вырубил эти строчки, т.к. они вроде не нужны особо, и из-за них был баг с тем что перс двигалс€ после начала битвы( ост€)
    }

    public void Return() // включение
    {
        Player.isPaused = false;
        Time.timeScale = 1;
        //player.enabled = true; //€ вырубил эти строчки, т.к. они вроде не нужны особо, и из-за них был баг с тем что перс двигалс€ после начала битвы( ост€)
    }

    // используетс€ когда игрок нажмет кнопки "в главное меню" или "заново"
    // во врем€ нажатой паузы. чтобы после перемещени€ на другую сцену врем€ в игре включилось
    private void OnDisable() 
    {
        Player.isPaused = false;
        Time.timeScale = 1;
        //if (player != null )//€ вырубил эти строчки, т.к. они вроде не нужны особо, и из-за них был баг с тем что перс двигалс€ после начала битвы( ост€)
        //    player.enabled = true;
    }
}
 