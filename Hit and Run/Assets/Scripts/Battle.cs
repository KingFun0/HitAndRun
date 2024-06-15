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
    public CameraSwitch сameraSwitch;

    private Animator anim;
    private GameObject animOfPlayer;

    float last_action;
    int wait_time = 0;
    public void Awake()//тут я собираю ссылочки на нужные объекты
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
        сameraSwitch.SwitchPriority();//смена камер
        yield return new WaitForSeconds(1);//сколько длятся анимации перехода
        BattleCanvas.SetActive(true);
        MainCanvas.SetActive(false);
        EnemyHealthBar.SetActive(true);
        HealthBar.GetComponent<Slider>().value = 1;
        EnemyHealthBar.GetComponent<Slider>().value = 1;
        Player.Instance.enabled = false;
    }
    public void StartBattle(GameObject sender,int en_lvl,int player_level)//начало битвы
    {
        Debug.Log("Начало битвы с монстром");
        anim.SetBool("isTouched", true);
        Player.Instance.PlaySound(Player.Instance.fightWithMonster); // воспроизведение звука
        StartCoroutine(StartSwitchCameras());
        Sender = sender;
        enemy_level = en_lvl;
        max_enemy_health = 90+(int)Math.Pow(enemy_level,2)* 10;
        enemy_health = max_enemy_health;
        player_health = 100;
        player_attack = 9 + (int)Math.Pow(player_level,2);
        Debug.Log("Выключаю скрипт передвижения игрока");
        Player.Instance.enabled = false;//выключает скрипт передвижения игрока при начале битвы
        
    }
    public void AttackButton()//кнопка атаки
    {
        if (Time.time - last_action < wait_time)//если не прошёл кулдаун на нажатие кнопопк
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
    IEnumerator TakeDamage()//получение урона игроком
    {
        int damage;
        if(has_parried)//если игрок парировал атаку
        {
            damage = 0;
            has_parried = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Враг промахнулся!";
        }
        else if (has_defended)//если игрок защитился от атаки
        {
            damage = UnityEngine.Random.Range(4, 6);
            has_defended = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Враг атаковал вас!";
            Player.Instance.PlaySound(Player.Instance.damage); // воспроизведение звука
        }
        else//если он просто получает урон
        {
            damage = UnityEngine.Random.Range(8, 12);
            PopupText.GetComponent<TextMeshProUGUI>().text = "Враг атаковал вас!";
            Player.Instance.PlaySound(Player.Instance.damage); // воспроизведение звука
        }
        player_health -= damage;
        HealthBar.GetComponent<Slider>().value = (float)player_health / 100f;
        wait_time += 1;
        yield return new WaitForSeconds(1);
        if (player_health <= 0)//если игрок умер
        {
            StartCoroutine(EndBattle());
            Player.Instance.level -= 6;
        }
        PopupText.GetComponent<TextMeshProUGUI>().text = "";
    }
    public void DefendButton()//кнопка защиты
    {
        if (Time.time - last_action < wait_time)//если не прошёл кулдаун на нажатие кнопопк
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        StartCoroutine(Defend());
    }
    public void RunButton()//кнопка побега(кубика)
    {
        if (Time.time - last_action < wait_time)//если не прошёл кулдаун на нажатие кнопопк
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        StartCoroutine(Run());
    }
    IEnumerator Attack()//атака игрока
    {

        double damage = player_attack * UnityEngine.Random.Range((float)0.8, (float)1.2);//рандомит наносимый урон
        enemy_health -= (int)Math.Round(damage, 0);
        EnemyHealthBar.GetComponent<Slider>().value = (float)enemy_health / (float)max_enemy_health;

        PopupText.GetComponent<TextMeshProUGUI>().text = "Вы успешно атаковали врага";
        Player.Instance.PlaySound(Player.Instance.attack); // воспроизведение звука
        wait_time += 1;
        yield return new WaitForSeconds(1);
        PopupText.GetComponent<TextMeshProUGUI>().text = "";
        if (enemy_health <= 0)//если враг умер
        {
            StartCoroutine(EndBattle());
            Player.Instance.level += UnityEngine.Random.Range(2, 4);
        }
        StartCoroutine(TakeDamage());
    }
    IEnumerator Defend()//защита игрока
    {
        if (UnityEngine.Random.Range(1, 100) <= 45)//если он парировал атаку
        {
            player_health += 12;
            if (player_health > 100)
            {
                player_health = 100;
            }
            HealthBar.GetComponent<Slider>().value = (float)player_health / 100f;
            has_parried = true;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Вы успешно парировали атаку!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
        else//если просто защитился от неё
        {
            has_defended = true;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Вы смогли Защититьcя от атаки!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
    }
    IEnumerator Run()//побег из битвы
    {
        if (UnityEngine.Random.Range(1, 7) > 4)//выпадение кубика
        {
            PopupText.GetComponent<TextMeshProUGUI>().text = "Вы смогли сбежать!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(EndBattle());
        }
        else
        {
            PopupText.GetComponent<TextMeshProUGUI>().text = "У вас не получилось сбежать!";
            wait_time += 1;
            yield return new WaitForSeconds(1);
            PopupText.GetComponent<TextMeshProUGUI>().text = "";
            StartCoroutine(TakeDamage());
        }
    }
    IEnumerator EndBattle()//конец битвы
    {
        anim.SetBool("isTouched", false);
        сameraSwitch.SwitchPriority();//смена камер
        yield return new WaitForSeconds(1);//сколько длятся анимации перехода
        //включение нужнных объектов и выключение ненеужных
        Player.Instance.enabled = true;
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + UnityEngine.Random.Range(12, 21));
        BattleCamera.SetActive(false);
        MainCamera.SetActive(true);
        enabled = false;
        HealthBar.SetActive(false);
        EnemyHealthBar.SetActive(false);
        BattleCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        Destroy(Sender);//удаление врага
    }
    
}
