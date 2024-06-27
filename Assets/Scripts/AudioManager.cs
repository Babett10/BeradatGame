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
        float db;

        if (mixer.GetFloat("SFX_VOL", out db))
            SFXSlider.value = (db+80)/80;

        if (mixer.GetFloat("BGM_VOL", out db))
            SFXSlider.value = (db+80)/80;

    }

    //SFX slider
    public void SFXVolume(float value)
    {
       value = value*80 - 80;

       mixer.SetFloat("SFX_VOL",value);
    }

    


    //BGM slider
    public void BGMVolume(float value)
    {
       value = value*80 - 80;

       mixer.SetFloat("BGM_VOL",value);
    }

}

    
