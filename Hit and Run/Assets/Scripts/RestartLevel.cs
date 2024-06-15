using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour // пройти уровень заново
{
    private void Start()
    {
        SceneManager.LoadScene(Player.currentLevel); // перемещение со сцены загрузки на текущий уровень
    }
}
