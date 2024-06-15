using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : MonoBehaviour
{

    private float speed = 2f; //�������� ��� �������������
    public TextMeshProUGUI textCoins;
    public TextMeshProUGUI textProtected;
    public TextMeshProUGUI textLevel;

    public void IncreaseSpeed(float amount) //��������� �������� 
    {
        speed += amount;
        Player.Instance.SetSpeed(speed);
        Debug.Log("�����");
    }

    public void DecreaseSpeed(float amount) //��������� ��������
    {
        speed -= amount;
        Player.Instance.SetSpeed(speed);
    }

    public void EnableProtection() //�������� ���
    {
        Player.isProtected = true;
        Debug.Log("�����");
    }

    public void DisableProtection() //��������� ���
    {
        Player.isProtected = false;
    }

    public void AddLevels() //�������� ��������� ���������� ������� �� 1 �� 10
    {
        Player.Instance.level += Random.Range(1, 10);
        Debug.Log("�����");
    }

    private void FixedUpdate() //������� �� ����� 
    {
        Debug.Log("coins");
        //textCoins.text = $"�������: {PlayerPrefs.GetInt("coins")}";
        textLevel.text = $"�������: {Player.Instance.level}";

        if (Player.isProtected)
            textProtected.text = "������� �� ���: ��";
        else if (!Player.isProtected)
            textProtected.text = "������� �� ���: ���";
    }
}
