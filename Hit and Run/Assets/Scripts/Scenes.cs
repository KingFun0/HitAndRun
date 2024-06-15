using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void ChangeScenes(int sceneNumber) // для смены сцены на любую введенную
    {
        StartCoroutine(SceneTransitions.fadeIn(sceneNumber));
    }
    public void ChangeScenesWithoutAnimations(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void Exit() // для выхода из игры
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
