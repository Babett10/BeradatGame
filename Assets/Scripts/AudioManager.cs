using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider BGMSlider;
    public AudioMixer mixer;
    public AudioSource Bgm;
    public AudioSource Sfx;


    public void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

    }

    //BGM & SFX slider
    public void SetMusicVolume()
    {
        float volume = BGMSlider.value;
        mixer.SetFloat("BGM_VOL", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("SFX_VOL", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }

    public void ButtonSFX()
    {
        Sfx.Play();
    }

    //SFX slider
    public void SFXVolume(float value)
    {
        value = value * 80 - 80;

        mixer.SetFloat("SFX_VOL", value);
    }

    //BGM slider
    public void BGMVolume(float value)
    {
        value = value * 80 - 80;

        mixer.SetFloat("BGM_VOL", value);
    }

}


