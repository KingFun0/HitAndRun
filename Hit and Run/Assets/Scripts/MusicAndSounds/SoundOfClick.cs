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
        StartCoroutine(ClearDontDestroyOnLoadObjects()); // вызов корутины
    }

    public void PlaySound()
    {
        AudioSource.PlayOneShot(click, volumeScale: PlayerPrefs.GetFloat("musicVolume")); // воспроизведение звука
        DontDestroyOnLoad(AudioSource); // не уничтожать аудио
        AudioSource.tag = "Persistent"; // присвоить аудио тег
    }
    IEnumerator ClearDontDestroyOnLoadObjects()
    {
        yield return new WaitForSeconds(click.length); // подождать
        if (GameObject.FindGameObjectWithTag("Persistent") != null) // если есть объекты с таким тегом
        {
            GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent"); // найти их
            foreach (GameObject obj in persistentObjects) 
            {
                obj.tag = "Untagged"; // присвоить тег
                Destroy(obj); // уничтожить
            }
        }
    }
}
