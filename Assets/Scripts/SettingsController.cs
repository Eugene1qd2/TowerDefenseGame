using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static float SoundsVolume=1f;
    public static float MusicVolume=1f;

    [SerializeField] GameObject SoundSlider;
    [SerializeField] GameObject MusicSlider;

    private void Start()
    {
        SoundSlider.GetComponent<Slider>().value = SoundsVolume;
        MusicSlider.GetComponent<Slider>().value = MusicVolume;
    }  
    
    public void AcceptSettings()
    {
        SoundsVolume = SoundSlider.GetComponent<Slider>().value;
        MusicVolume = MusicSlider.GetComponent<Slider>().value;
        Debug.Log(SoundsVolume);
        Debug.Log(MusicVolume);
        SceneManager.LoadScene(0);
    }

}
