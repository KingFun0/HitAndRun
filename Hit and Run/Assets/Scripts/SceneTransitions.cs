using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public GameObject ?fadeInTransition;
    public GameObject ?fadeOutTransition;
    // Start is called before the first frame update
    public static SceneTransitions instance;
    private void Start()
    {
        instance = this;
        if (fadeInTransition != null)
            fadeInTransition.SetActive(false);
        if (fadeOutTransition != null)
            StartCoroutine(fadeOut());
    }
    public static IEnumerator fadeIn(int scene)
    {
        if (instance.fadeInTransition == null)
        {
            SceneManager.LoadScene(scene);//Если на сцене нет анимаций выхода
            yield break;
        }
        Player.isPaused = true;
        instance.fadeInTransition.SetActive(true);
        yield return new WaitForSeconds(1);
        Player.Instance.SetCompletedLevels((Player.Instance.GetCompletedLevels())+1);
        instance.fadeInTransition.SetActive(false);
        SceneManager.LoadScene(scene); // перемещение на сцену победы
    }
    public static IEnumerator fadeOut()
    {
        if (instance.fadeOutTransition == null)
        { 
            yield break; 
        }
        Player.isPaused = true;
        float player_speed = Player.Instance.GetSpeed();
        Player.Instance.SetSpeed(0);
        instance.fadeOutTransition.SetActive(true);
        yield return new WaitForSeconds(1);
        Player.isPaused = false;
        Player.Instance.SetSpeed(player_speed);
        instance.fadeOutTransition.SetActive(false);
    }
}
