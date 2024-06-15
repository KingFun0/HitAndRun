using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundOfClick : MonoBehaviour
{
    public AudioClip click; 

    private AudioSource AudioSource => GetComponent<AudioSource>();


    private void Awake()
    {
        StartCoroutine(ClearDontDestroyOnLoadObjects()); // ����� ��������
    }

    public void PlaySound()
    {
        AudioSource.PlayOneShot(click, volumeScale: PlayerPrefs.GetFloat("musicVolume")); // ��������������� �����
        DontDestroyOnLoad(AudioSource); // �� ���������� �����
        AudioSource.tag = "Persistent"; // ��������� ����� ���
    }
    IEnumerator ClearDontDestroyOnLoadObjects()
    {
        yield return new WaitForSeconds(click.length); // ���������
        if (GameObject.FindGameObjectWithTag("Persistent") != null) // ���� ���� ������� � ����� �����
        {
            GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent"); // ����� ��
            foreach (GameObject obj in persistentObjects) 
            {
                obj.tag = "Untagged"; // ��������� ���
                Destroy(obj); // ����������
            }
        }
    }
}
