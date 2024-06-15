using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour // ������ ������������ ��� �������� ��������� �������
{
    // ������� ������ � ��������
    public Button[] buttonsLevels;
    public Sprite[] newSprite;

    private void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("levels") > 25)
            PlayerPrefs.SetInt("levels", 25);
        else if (PlayerPrefs.GetInt("levels") == 0)
            PlayerPrefs.SetInt("levels", 1);
        for (int i = 0; i < PlayerPrefs.GetInt("levels"); i++) // ���� i ������ ��� ���������� ���-�� �������
        {
            if (buttonsLevels[i].image.sprite != newSprite[i]) // � ���� ������� �� �����
            {
                buttonsLevels[i].image.sprite = newSprite[i]; // �� ����� �������
                buttonsLevels[i].interactable = true;    // ���� ��������� ����������������� ������
            }
        }
    }

}
