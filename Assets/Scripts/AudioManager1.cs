using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager1 : MonoBehaviour
{
    [SerializeField] Image musicOnButton;
    [SerializeField] Image musicOffButton;
    [SerializeField] Image SFXOnButton;
    [SerializeField] Image SFXOffButton;
    public Slider SFXSlider;
    public Slider BGMSlider;
    public AudioMixer mixer;
    public AudioSource asMusic;
    public AudioSource asSFX;

    private static AudioManager1 backgroundMusic;

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
        UpdateMusicIcon();
        UpdateSFXIcon();
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

    public void ButtonMusicMute()
    {
        asMusic.mute = !asMusic.mute;
        UpdateMusicIcon();
    }

    private void UpdateMusicIcon()
    {
        if (asMusic.mute)
        {
            musicOnButton.enabled = false;
            musicOffButton.enabled = true;
        }
        else
        {
            musicOnButton.enabled = true;
            musicOffButton.enabled = false;
        }
    }

    public void ButtonSFX()
    {
        asSFX.Play();
    }

    public void ButtonMuteSFX()
    {
        asSFX.mute = !asSFX.mute;
        UpdateSFXIcon();
    }

    private void UpdateSFXIcon()
    {
        if (asSFX.mute)
        {
            SFXOnButton.enabled = false;
            SFXOffButton.enabled = true;
        }
        else
        {
            SFXOnButton.enabled = true;
            SFXOffButton.enabled = false;
        }
    }

}
