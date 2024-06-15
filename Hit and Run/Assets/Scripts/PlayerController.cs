using Assets.Scriipts.Bonuses;
using Assets.Scriipts.Traps;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : SoundsInTheGame
{
    public float speed;
    public int level;
    private int coins;
    private int completed_levels;
    public static bool isProtected;
    public static int currentLevel;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private float LeftLine;
    private float RightLine;
    public static bool isPaused;
     
    private bool isMovingLeft;
    private bool isMovingRight;
    public static Player Instance;

    public BonusController bonusController;
    private IPlayerBonus speedBonus;
    private IPlayerBonus protectionBonus;
    private IPlayerBonus levelBonus;
    
    public TrapController trapController;
    private ITrap stakesTrap;
    private ITrap wheelWithStakesTrap;
    private GameObject BattleCamera;

    
    private void Awake()
    {

        Instance = this; //реализация шаблона одиночки

        speedBonus = new SpeedBonus();
        protectionBonus = new ProtectionBonus();
        levelBonus = new AddingLevelsBonus();

        stakesTrap = new Stakes();
        wheelWithStakesTrap = new WheelWithStakes();
        BattleCamera = GameObject.FindGameObjectWithTag("BattleCamera");
    }
    void Start()
    {
        BattleCamera.SetActive(false);
        level = 1;
        isProtected = false;
        isPaused = false;
        dragDistance = Screen.height * 15 / 100;
        LeftLine = 1.25f;
        RightLine = -1.25f;
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if (activeScene != 0 && activeScene != 26 && activeScene != 27 && activeScene != 28 && activeScene != 29)
            currentLevel = activeScene;
    }

    void Update()
    {
        if (!isPaused)
        {
            Vector3 moveDirection = new Vector3(0, 0, speed);//вектор движенния игрока

            //проверка на свайп от игрока
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 15% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {
                        //It's a drag
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            isMovingLeft = false;
                            isMovingRight = true;
                        }
                        else
                        {   //Left swipe                       
                            isMovingLeft = true;
                            isMovingRight = false;
                        }
                    }
                }
            }
            //также сделаю на кнопки a и d чтобы с компа тестить
            if (Input.GetKey(KeyCode.A))
            {
                isMovingLeft = true;
                isMovingRight = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
            if (isMovingLeft)
            {
                if (IsAtLeftLine(transform.position.z))//если игрок уже дошёл до левой линии
                {
                    isMovingLeft = false;
                    moveDirection = new Vector3(0, 0, speed);
                    transform.position = new Vector3(transform.position.x, transform.position.y, LeftLine);//передвинуть игрока ровно на левую линию
                }
                else
                {
                    moveDirection = new Vector3(-15, 0, speed);
                }
            }
            else if (isMovingRight)
            {
                if (IsAtRightLine(transform.position.z))//если игрок уже дошёл до правой линии
                {
                    isMovingRight = false;
                    moveDirection = new Vector3(0, 0, speed);
                    transform.position = new Vector3(transform.position.x, transform.position.y, RightLine);//передвинуть игрока ровно на левую линию
                }
                else
                {
                    moveDirection = new Vector3(15, 0, speed);
                }
            }
            else
            {
                moveDirection = new Vector3(0, 0, speed);
            }
            transform.Translate(moveDirection * Time.deltaTime);
        }

        if (transform.position.x >= 14) //условие конца уровня
        {
            StartCoroutine(SceneTransitions.fadeIn(26));
        }

        if (level <= 0)
        {
            StartCoroutine(SceneTransitions.fadeIn(29));
        }
    }

    private bool IsAtLeftLine(float z)
    {
        if (z >= LeftLine)
            return true;
        else
            return false;
    }
    private bool IsAtRightLine(float z)
    {
        if (z <= RightLine)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider other) //включение бонуса или ловушки на триггер, проверка на монетки
    {
        if (other.CompareTag("speedBonus"))
        {
            speedBonus.ApplyBonus(bonusController);
            Destroy(other.gameObject);
            ShowMessage.message = "Вы получили бонус Скорости!";
            PlaySound(pickUpBonus);
        }
        else if (other.CompareTag("protectionBonus"))
        {
            protectionBonus.ApplyBonus(bonusController);
            Destroy(other.gameObject);
            ShowMessage.message = "Вы получили бонус Защиты!";
            PlaySound(pickUpBonus);
        }
        else if (other.CompareTag("addingLevelsBonus"))
        {
            levelBonus.ApplyBonus(bonusController);
            Destroy(other.gameObject);
            ShowMessage.message = "Вы получили бонус Добавления уровней!";
            PlaySound(pickUpBonus);
        }
        else if (other.CompareTag("stakesTrap"))
        {
            stakesTrap.FallIntoTrap(trapController);
            Destroy(other.gameObject);
            if (isProtected)
                ShowMessage.message = "Вы испольЗовали щит!";
            else
                ShowMessage.message = "Вы получили урон!";
            PlaySound(fallIntoATrap);
        }
        else if (other.CompareTag("wheelWithStakesTrap"))
        {
            wheelWithStakesTrap.FallIntoTrap(trapController);
            Destroy(other.gameObject);
            if (isProtected)
                ShowMessage.message = "Вы испольЗовали щит!";
            else
                ShowMessage.message = "Вы получили урон!";
            PlaySound(fallIntoATrap);
        }
        else if (other.CompareTag("coin"))
        {
            coins++;
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
            Destroy(other.gameObject);
            ShowMessage.message = "Вы получили монетку!";
            PlaySound(pickUpCoin);
        }
    }
    private void OnDisable()
    {
        bonusController.enabled = false;
        trapController.enabled = false;
    }
    private void OnEnable()
    {
        bonusController.enabled = true;
        trapController.enabled = true;
    }
    public float GetSpeed()
    {
        return speed;
    }

    public float SetSpeed(float speed)
    {
        this.speed = speed;
        return this.speed;
    }
    public int GetCoins()
    {
        return coins;
    }
    public void SetCoins(int coins)
    {
        this.coins = coins;
    }
    public int GetCompletedLevels()
    {
        return completed_levels;
    }
    public void SetCompletedLevels(int completed_levels)
    {
        this.completed_levels = completed_levels;
    }
}
