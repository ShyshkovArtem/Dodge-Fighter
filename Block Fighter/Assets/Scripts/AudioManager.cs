using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip background;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip steps;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lvlUp;

    private void Start()
    {
        if (musicSource == null || background == null)
        {
            return;
        }

        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayDeath() => PlaySFX(death);
    public void PlayCoin() => PlaySFX(coin);
    public void PlaySteps() => PlaySFX(steps);
    public void PlayDash() => PlaySFX(dash);
    public void PlayButtonClick() => PlaySFX(buttonClick);
    public void PlayWin() => PlaySFX(win);
    public void PlayLevelUp() => PlaySFX(lvlUp);

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}
