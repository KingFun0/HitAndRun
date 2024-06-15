using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public static float musicVolume;
    public TextMeshProUGUI musicVolumeText;
    public GameObject buttonOn;
    public GameObject buttonOff;

    public static SliderController Instance;


    public void Start()
    {
        Instance = this;
        musicVolume = PlayerPrefs.GetFloat("musicVolume") * 100; //�������� ��������� ������
        slider.value = musicVolume / 100;
        musicVolumeText.text = $"{musicVolume}%";

        if (musicVolume == 0) // ��� ������ ���/����
        {
            buttonOff.SetActive(false);
            buttonOn.SetActive(true);
        }    
        else
        {
            buttonOff.SetActive(true);
            buttonOn.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        PlayerPrefs.SetFloat("musicVolume", slider.value); // ���������� ���������
    }

    public void ChangeVolume() // ��� ��������� ������� ��������
    {
        musicVolume = Convert.ToInt32(slider.value * 100);
        musicVolumeText.text = $"{musicVolume}%";
    }
    public void MusicOff() // ���� ���� ������ ������ ����
    {
        slider.value = 0;
        musicVolumeText.text = "0%";
        PlayerPrefs.SetFloat("musicVolume", slider.value);
    }

    public void MusicOn() //������ ���
    {
        if (musicVolume == 0)
            musicVolume = 50;

        slider.value = musicVolume / 100;
        musicVolumeText.text = $"{musicVolume}%";
        PlayerPrefs.SetFloat("musicVolume", slider.value);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    private void OnDestroy() // ���������� ��������� �� ������
    {
        PlayerPrefs.SetFloat("musicVolume", slider.value);
    }
}
