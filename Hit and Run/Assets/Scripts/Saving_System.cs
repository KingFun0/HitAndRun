using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving_System : MonoBehaviour
{
    public void Start()
    {
        Load_Data();
    }
    public void Save_Data()
    {
        PlayerPrefs.SetInt("coins", Player.Instance.GetCoins());
        PlayerPrefs.SetInt("levels", Player.Instance.GetCompletedLevels());
        PlayerPrefs.SetFloat("musicVolume", SliderController.Instance.GetMusicVolume());
    }
    public void Load_Data()
    {
        Player.Instance.SetCoins(PlayerPrefs.GetInt("coins"));
        Player.Instance.SetCompletedLevels(PlayerPrefs.GetInt("levels"));
        SliderController.Instance.SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
    }
}
