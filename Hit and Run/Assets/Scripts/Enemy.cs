using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    public bool isDynamic;
    public bool isBoss;
    float previous_attempt;
    private GameObject MainCamera;
    private GameObject BattleCamera;
    private GameObject player;
    private GameObject HealthBar;
    private void Awake()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BattleCamera = GameObject.FindGameObjectWithTag("BattleCamera");
        HealthBar = GameObject.FindGameObjectWithTag("HealthBar");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        HealthBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDynamic)//если уровень динамичный
        {
            if(Time.time - previous_attempt >1)//прошла секунда. —делал чтобы сильной нагрузки не было, чтобы оно каждую секунду обновл€лось
            {
                previous_attempt = Time.time;//обновл€ем врем€ прошлой попытки обновить уровень
                level = Player.Instance.level;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag=="Player") && !Player.isProtected || isBoss) 
        {
            Player.Instance.PlaySound(Player.Instance.fightWithMonster); // воспроизведение звука
            Debug.Log("¬ы столкнулись с врагом!");
            //переключение на боЄвку
            BattleCamera.SetActive(true);
            MainCamera.SetActive(false);
            HealthBar.SetActive(true);
            if (isBoss)
            {
                player.GetComponent<BossBattle>().StartBattle(gameObject, level, Player.Instance.level);
                transform.position = new Vector3(Player.Instance.transform.position.x+4, transform.position.y, Player.Instance.transform.position.z);
            }
            else
            {
                player.GetComponent<Battle>().StartBattle(gameObject, level, Player.Instance.level);
                transform.position = transform.position + new Vector3(4, 0, 0);
            }
        
            
        }
        else if (Player.isProtected)
        {
            Debug.Log("щит");
            ShowMessage.message = "¬ы исполь«овали щит";
            Player.isProtected = false;
            Destroy(this.gameObject);
        }
    }
}
