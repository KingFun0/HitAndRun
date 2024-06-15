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
public class BossBattle : MonoBehaviour
{

    int boss_level;
    public int boss_health;
    int max_boss_health;

    public int player_health;
    int player_attack;

    bool has_defended = false;
    bool has_parried = false;

    private GameObject MainCamera;
    private GameObject BattleCamera;
    private GameObject HealthBar;
    private GameObject EnemyHealthBar;
    private GameObject BossHealthBar;
    private GameObject PopupText;
    private GameObject BattleCanvas;
    private GameObject BossCanvas;
    private GameObject MainCanvas;
    private GameObject Sender;
    private GameObject attackButton;
    private GameObject defendButton;
    private GameObject bossAttackButton;
    private GameObject bossDefendButton;
    private GameObject runButton;
    public CameraSwitch сameraSwitch;

    float last_action;
    int wait_time = 0;
    public void Awake()//тут я собираю ссылочки на нужные объекты
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BattleCamera = GameObject.FindGameObjectWithTag("BattleCamera");
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar");
        BossHealthBar = GameObject.FindGameObjectWithTag("BossHealthBar");
        EnemyHealthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
        PopupText = GameObject.FindGameObjectWithTag("Popup Text");
        BattleCanvas = GameObject.FindGameObjectWithTag("BattleCanvas");
        BossCanvas = GameObject.FindGameObjectWithTag("BossCanvas");
        MainCanvas = GameObject.FindGameObjectWithTag("Canvas");
        attackButton = GameObject.FindGameObjectWithTag("AttackButton");
        defendButton = GameObject.FindGameObjectWithTag("DefendButton");
        bossAttackButton = GameObject.FindGameObjectWithTag("BossAttackButton");
        bossDefendButton = GameObject.FindGameObjectWithTag("BossDefendButton");
        runButton = GameObject.FindGameObjectWithTag("RunButton");

    }
    public void Start()
    {
        BossHealthBar.SetActive(false);
        BossCanvas.SetActive(false);
    }
    IEnumerator StartSwitchCameras()
    {
        сameraSwitch.SwitchPriority();//смена камер
        yield return new WaitForSeconds(1);//сколько длятся анимации перехода
        //вот тут переназначаю ивенты при нажатии кнопок
        
        MainCanvas.SetActive(false);
        BossCanvas.SetActive(true);
        BattleCanvas.SetActive(true);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        runButton.SetActive(false);
        bossAttackButton.SetActive(true);
        bossDefendButton.SetActive(true);
        EnemyHealthBar.SetActive(false);
        BossHealthBar.SetActive(true);
        HealthBar.GetComponent<Slider>().value = 1;
        BossHealthBar.GetComponent<Slider>().value = 1;
    }
    public void StartBattle(GameObject sender, int en_lvl, int player_level)//начало битвы
    {
        Debug.Log("Начало битвы с боссом");
        Player.Instance.PlaySound(Player.Instance.fightWithBossMonster); // воспроизведение звука
        //attackButton.GetComponent<Button>().onClick.RemoveAllListeners();

        StartCoroutine(StartSwitchCameras());
        Sender = sender;
        boss_level = en_lvl;
        max_boss_health = (int)Math.Round(boss_level * 100 + 0.2*(double)player_level*100);
        boss_health = max_boss_health;
        player_health = 100;
        player_attack = 12 + (player_level - 1) * 7;
        Player.Instance.enabled = false;//выключает ненужные объекты при начале битвы
        
    }
    public void AttackButton()//кнопка атаки
    {
        if (Time.time - last_action < wait_time)//если не прошёл кулдаун на нажатие кнопопк
        {
            return;
        }
        wait_time = 0;
        last_action = Time.time;
        StartCoroutine(Attack());
    }
    IEnumerator TakeDamage()//получение урона игроком
    {
        int damage=0;
        if (has_parried)//если игрок парировал атаку
        {
            damage = 0;
            has_parried = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Враг промахнулся!";
        }
        else if (has_defended)//если игрок защитился от атаки
        {
            damage = UnityEngine.Random.Range(5, 8);
            has_defended = false;
            PopupText.GetComponent<TextMeshProUGUI>().text = "Враг атаковал вас!";
            Player.Instance.PlaySound(Player.Instance.damage); // воспроизведение звука
        }
        else//если он просто получает урон
        {
            damage = UnityEngine.Random.Range(10, 16);
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
            Player.Instance.level -= 99;
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
    IEnumerator Attack()//атака игрока
    {

        double damage = player_attack * UnityEngine.Random.Range((float)0.8, (float)1.2);//рандомит наносимый урон
        boss_health -= (int)Math.Round(damage, 0);
        BossHealthBar.GetComponent<Slider>().value = (float)boss_health / (float)max_boss_health;

        PopupText.GetComponent<TextMeshProUGUI>().text = "Вы вспешно атаковали врага";
        Player.Instance.PlaySound(Player.Instance.attack); // воспроизведение звука
        wait_time += 1;
        yield return new WaitForSeconds(1);
        PopupText.GetComponent<TextMeshProUGUI>().text = "";
        if (boss_health <= 0)//если враг умер
        {
            StartCoroutine(EndBattle());
        }
        StartCoroutine(TakeDamage());
    }
    IEnumerator Defend()//защита игрока
    {
        if (UnityEngine.Random.Range(1, 101) < 80)//если он парировал атаку
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
    private IEnumerator EndBattle()//конец битвы
    {
        сameraSwitch.SwitchPriority();
        yield return new WaitForSeconds(1);//сколько длятся анимации перехода
        //включение нужнных объектов и выключение ненеужных
        Player.Instance.enabled = true;
        PlayerPrefs.SetInt("coins", (PlayerPrefs.GetInt("coins") + (PlayerPrefs.GetInt("levels") * Player.currentLevel)));
        BattleCamera.SetActive(false);
        MainCamera.SetActive(true);
        enabled = false;
        HealthBar.SetActive(false);
        BossHealthBar.SetActive(false);
        BattleCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        Destroy(Sender);//удаление врага
    }
}

