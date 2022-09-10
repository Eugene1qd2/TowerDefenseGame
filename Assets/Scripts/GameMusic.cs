using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{

    AudioSource music;
    [SerializeField] bool isMusic = false;

    void Start()
    {
        music = GetComponent<AudioSource>();
        if (isMusic)
            music.volume = SettingsController.MusicVolume;
        else
            music.volume = SettingsController.SoundsVolume;
    }


}
