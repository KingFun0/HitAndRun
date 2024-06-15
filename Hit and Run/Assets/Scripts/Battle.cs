using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TextToTMPNamespace;

using System.Security.Cryptography;
using UnityEngine.XR;
using TMPro;
using UnityEditor.VersionControl;
public class Battle : MonoBehaviour
{

    int enemy_level;
    public int enemy_health;
    int max_enemy_health;

    public int player_health;
    int player_attack;

    bool has_defended = false;
    bool has_parried = false;

    private GameObject MainCamera;
    private GameObject BattleCamera;
    private GameObject HealthBar;
    private GameObject EnemyHealthBar;
    private GameObject PopupText;
    private GameObject BattleCanvas;
    private GameObject MainCanvas;
    private GameObject Sender;
    public CameraSwitch �ameraSwitch;

    private Animator anim;
    private GameObject animOfPlayer;

    float last_action;
    int wait_time = 0;
    public void Awake()//��� � ������� �������� �� ������ �������
    {
        animOfPlayer = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(animOfPlayer.name);
        anim = animOfPlayer.GetComponent<Animator>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BattleCamera = GameObject.FindGameObjectWithTag("BattleCamera");
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar");
        EnemyHealthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
        PopupText = GameObject.FindGameObjectWithTag("Popup Text");
        BattleCanvas = GameObject.FindGameObjectWithTag("BattleCanvas");
        MainCanvas = GameObject.FindGameObjectWithTag("Canvas");

    }
    public void Start()
    {
        EnemyHealthBar.SetActive(false);
        BattleCanvas.SetActive(false);
    }
    IEnumerator StartSwitchCameras()
    {
        �ameraSwitch.SwitchPriority();//����� �����
        yield return new WaitForSeconds(1);//������� ������ �������� ��������
        BattleCanvas.SetActive(true);
        MainCanvas.SetActive(false);
        EnemyHealthBar.SetActive(true);
        HealthBar.GetComponent<Slider>().value = 1;
        EnemyHealthBar.GetComponent<Slider>().value = 1;
        Player.Instance.enabled = false;
    }
    public void StartBattle(GameObject sender,int en_lvl,int player_level)//������ �����
    {
        Debug.Log("������ ����� � ��������");
        anim.SetBool("isTouched", true);
        Player.Instance.PlaySound(Player.Instance.fightWithMonster); // ��������������� �����
        StartCoroutine(StartSwitchCameras());
        Sender = sender;
        enemy_level = en_lvl;
        max_enemy_health = 90+(int)Math.Pow(enemy_level,2)* 10;
        enemy_health = max_enemy_health;
        player_health = 100;
        player_attack = 9 + (int)Math.Pow(player_level,2);
        Debug.Log("�������� ������ ������������ ������");
        Player.Instance.enabled = false;//��������� ������ ������������ ������ ��� ������ �����
        
    }
    public void AttackButton()//������ �����
    {
        if (Time.time - last_action < wait_time)//���� �� ������ ������� �� ������� �������
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        //anim.SetTrigger("isAttack");
        //anim.SetBool("isInpact", true);
        StartCoroutine(Attack());
        //anim.SetBool("isInpact", false);

    }
    IEnumerator TakeDamage()//��������� ����� �������
    {
        int damage;
        if(has_parried)//���� ����� ��������� �����
        {
            damage = 0;
            has_parried = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "���� �����������!";
        }
        else if (has_defended)//���� ����� ��������� �� �����
        {
            damage = UnityEngine.Random.Range(4, 6);
            has_defended = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "���� �������� ���!";
            Player.Instance.PlaySound(Player.Instance.damage); // ��������������� �����
        }
        else//���� �� ������ �������� ����
        {
            damage = UnityEngine.Random.Range(8, 12);
            PopupText.GetComponent<TextMeshProUGUI>().text = "���� �������� ���!";
            Player.Instance.PlaySound(Player.Instance.damage); // ��������������� �����
        }
        player_health -= damage;
        HealthBar.GetComponent<Slider>().value = (float)player_health / 100f;
        wait_time += 1;
        yield return new WaitForSeconds(1);
        if (player_health <= 0)//���� ����� ����
        {
            StartCoroutine(EndBattle());
            Player.Instance.level -= 6;
        }
        PopupText.GetComponent<TextMeshProUGUI>().text = "";
    }
    public void DefendButton()//������ ������
    {
        if (Time.time - last_action < wait_time)//���� �� ������ ������� �� ������� �������
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        StartCoroutine(Defend());
    }
    public void RunButton()//������ ������(������)
    {
        if (Time.time - last_action < wait_time)//���� �� ������ ������� �� ������� �������
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        StartCoroutine(Run());
    }
    IEnumerator Attack()//����� ������
    {

        double damage = player_attack * UnityEngine.Random.Range((float)0.8, (float)1.2);//�������� ��������� ����
        enemy_health -= (int)Math.Round(damage, 0);
        EnemyHealthBar.GetComponent<Slider>().value = (float)enemy_health / (float)max_enemy_health;

        PopupText.GetComponent<TextMeshProUGUI>().text = "�� ������� ��������� �����";
        Player.Instance.PlaySound(Player.Instance.attack); // ��������������� �����
        wait_time += 1;
        yield return new WaitForSeconds(1);
        PopupText.GetComponent<TextMeshProUGUI>().text = "";
        if (enemy_health <= 0)//���� ���� ����
        {
            StartCoroutine(EndBattle());
            Player.Instance.level += UnityEngine.Random.Range(2, 4);
        }
        StartCoroutine(TakeDamage());
    }
    IEnumerator Defend()//������ ������
    {
        if (UnityEngine.Random.Range(1, 100) <= 45)//���� �� ��������� �����
        {
            player_health += 12;
            if (player_health > 100)
            {
                player_health = 100;
            }
            HealthBar.GetComponent<Slider>().value = (float)player_health / 100f;
            has_parried = true;
            PopupText.GetComponent<TextMeshProUGUI>().text = "�� ������� ���������� �����!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
        else//���� ������ ��������� �� ��
        {
            has_defended = true;
            PopupText.GetComponent<TextMeshProUGUI>().text = "�� ������ ��������c� �� �����!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
    }
    IEnumerator Run()//����� �� �����
    {
        if (UnityEngine.Random.Range(1, 7) > 4)//��������� ������
        {
            PopupText.GetComponent<TextMeshProUGUI>().text = "�� ������ �������!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(EndBattle());
        }
        else
        {
            PopupText.GetComponent<TextMeshProUGUI>().text = "� ��� �� ���������� �������!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
    }
    IEnumerator EndBattle()//����� �����
    {
        anim.SetBool("isTouched", false);
        �ameraSwitch.SwitchPriority();//����� �����
        yield return new WaitForSeconds(1);//������� ������ �������� ��������
        //��������� ������� �������� � ���������� ���������
        Player.Instance.enabled = true;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + UnityEngine.Random.Range(12, 21));
        BattleCamera.SetActive(false);
        MainCamera.SetActive(true);
        enabled = false;
        HealthBar.SetActive(false);
        EnemyHealthBar.SetActive(false);
        BattleCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        Destroy(Sender);//�������� �����
    }
    
}
