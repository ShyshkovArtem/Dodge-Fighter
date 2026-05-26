using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerVolume : MonoBehaviour
{
    private const string MusicVolumeKey = "musicVolume";
    private const string SfxVolumeKey = "sfxVolume";
    private const float MinVolume = 0.0001f;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        SetMixerVolume("music", musicSlider, MusicVolumeKey);
    }

    public void SetSFXVolume()
    {
        SetMixerVolume("sfx", sfxSlider, SfxVolumeKey);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        SetMusicVolume();

        sfxSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey);
        SetSFXVolume();
    }

    private void SetMixerVolume(string parameterName, Slider slider, string prefsKey)
    {
        if (myMixer == null || slider == null)
        {
            return;
        }

        float volume = Mathf.Max(slider.value, MinVolume);
        myMixer.SetFloat(parameterName, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(prefsKey, slider.value);
    }
}
