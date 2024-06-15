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

public class EndOfLevel : MonoBehaviour // ������ �������� �� ������ ������
{
    public TextMeshProUGUI textAboutCoins;
    public GameObject buttonNextLevel;
    public static int levelNumber;



    public void FixedUpdate()
    {
        if (levelNumber != Player.currentLevel) // ��������� � ���������� ������� �����
            levelNumber = Player.currentLevel;
        textAboutCoins.text = PlayerPrefs.GetInt("coins").ToString();
        if (levelNumber >= 25)  // ���� ������� ������ ���� ����� 25, �� ��������� ������ "���� �������"
        {
            buttonNextLevel.SetActive(false);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("levels") * Player.currentLevel);    // ��������� �������, ���� ����� ������ ��������� �������
        }
        else
            buttonNextLevel.SetActive(true);
    }

    public void ToNextLevel() // ������� ������ "���� �������"
    {
        if (levelNumber < 25) 
        {
            ++levelNumber; 
            StartCoroutine(SceneTransitions.fadeIn(levelNumber));// ����������� �� ���� �������
            //SceneManager.LoadScene(levelNumber); 
            buttonNextLevel.SetActive(true); 
        }
        PlayerPrefs.SetInt("levels", levelNumber); //���������� �������
    }



    public void ToMainMenu() // ������� ������ "� ������� ����"
    {
        if (levelNumber < 25) 
            ++levelNumber;
        SceneManager.LoadScene(0); // ����������� � ������� ����
        PlayerPrefs.SetInt("levels", levelNumber);

    }
}
