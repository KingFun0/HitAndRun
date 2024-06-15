using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void ChangeScenes(int sceneNumber) // ��� ����� ����� �� ����� ���������
    {
        StartCoroutine(SceneTransitions.fadeIn(sceneNumber));
    }
    public void ChangeScenesWithoutAnimations(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void Exit() // ��� ������ �� ����
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
