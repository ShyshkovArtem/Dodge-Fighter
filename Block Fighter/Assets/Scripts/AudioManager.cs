using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip coin;
    public AudioClip steps;
    public AudioClip dash;
    public AudioClip buttonClick;
    public AudioClip win;
    public AudioClip lvlUp;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX ( AudioClip clip )
    {
        sfxSource.PlayOneShot(clip);
    }
}
