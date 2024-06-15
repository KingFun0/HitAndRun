using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsInTheGame : MonoBehaviour
{
    public AudioClip pickUpBonus;
    public AudioClip pickUpCoin;
    public AudioClip fallIntoATrap;
    public AudioClip fightWithMonster;
    public AudioClip fightWithBossMonster;
    public AudioClip attack;
    public AudioClip damage;
    public AudioClip lose;
    public AudioClip win;

    private AudioSource AudioSource => GetComponent<AudioSource>();

    public void PlaySound(AudioClip audioClip)
    {
        AudioSource.PlayOneShot(audioClip, volumeScale: PlayerPrefs.GetFloat("musicVolume") / 3); //разделила громкость эффектов
    }
}
