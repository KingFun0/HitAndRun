using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndOfLevel : MonoBehaviour // скрипт отвечает за панель победы
{
    public TextMeshProUGUI textAboutCoins;
    public GameObject buttonNextLevel;
    public static int levelNumber;



    public void FixedUpdate()
    {
        if (levelNumber != Player.currentLevel) // сохраняет в переменную текущий левел
            levelNumber = Player.currentLevel;
        textAboutCoins.text = PlayerPrefs.GetInt("coins").ToString();
        if (levelNumber >= 25)  // если уровень больше либо равен 25, то отключает кнопку "след уровень"
        {
            buttonNextLevel.SetActive(false);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("levels") * Player.currentLevel);    // добавляет монетки, если игрок прошел последний уровень
        }
        else
            buttonNextLevel.SetActive(true);
    }

    public void ToNextLevel() // функция кнопки "след уровень"
    {
        if (levelNumber < 25) 
        {
            ++levelNumber; 
            StartCoroutine(SceneTransitions.fadeIn(levelNumber));// перемещение на след уровень
            //SceneManager.LoadScene(levelNumber); 
            buttonNextLevel.SetActive(true); 
        }
        PlayerPrefs.SetInt("levels", levelNumber); //сохранение уровней
    }



    public void ToMainMenu() // функция кнопки "в главное меню"
    {
        if (levelNumber < 25) 
            ++levelNumber;
        SceneManager.LoadScene(0); // перемещение в главное меню
        PlayerPrefs.SetInt("levels", levelNumber);

    }
}
