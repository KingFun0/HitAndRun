using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Pause: MonoBehaviour // ������ �������� �� ������ �����
{

    public Behaviour player; // ������ ������


    public void PauseLevel()
    {
        Player.isPaused = true;
        Time.timeScale = 0; // ���������� ������� � ����
        //player.enabled = false; //� ������� ��� �������, �.�. ��� ����� �� ����� �����, � ��-�� ��� ��� ��� � ��� ��� ���� �������� ����� ������ �����(�����)
    }

    public void Return() // ���������
    {
        Player.isPaused = false;
        Time.timeScale = 1;
        //player.enabled = true; //� ������� ��� �������, �.�. ��� ����� �� ����� �����, � ��-�� ��� ��� ��� � ��� ��� ���� �������� ����� ������ �����(�����)
    }

    // ������������ ����� ����� ������ ������ "� ������� ����" ��� "������"
    // �� ����� ������� �����. ����� ����� ����������� �� ������ ����� ����� � ���� ����������
    private void OnDisable() 
    {
        Player.isPaused = false;
        Time.timeScale = 1;
        //if (player != null )//� ������� ��� �������, �.�. ��� ����� �� ����� �����, � ��-�� ��� ��� ��� � ��� ��� ���� �������� ����� ������ �����(�����)
        //    player.enabled = true;
    }
}
 