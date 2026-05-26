using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string MainMenuSceneName = "MainMenu";
    private const string FirstRoomSceneName = "room1";

    [SerializeField] private GameObject shopPopUp;
    [SerializeField] private GameObject optionsPopUp;
    [SerializeField] private GameObject Logo;
    [SerializeField] private GameObject Buttons;
    [SerializeField] private GameObject PauseGameSet;

    private AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    public void OnStartButtonPressed()
    {
        audioManager?.PlayButtonClick();
        SceneManager.LoadScene(FirstRoomSceneName);
    }

    public void OnOptionsButtonPressed()
    {
        SetMainMenuVisible(false);
        SetActive(optionsPopUp, true);
        audioManager?.PlayButtonClick();
    }

    public void OnShopButtonPressed()
    {
        SetMainMenuVisible(false);
        SetActive(shopPopUp, true);
        audioManager?.PlayButtonClick();
    }

    public void ClosePopup()
    {
        SetActive(optionsPopUp, false);
        SetMainMenuVisible(true);
        audioManager?.PlayButtonClick();
    }

    public void CloseShopPopup()
    {
        SetActive(shopPopUp, false);
        SetMainMenuVisible(true);
        audioManager?.PlayButtonClick();
    }

    public void PauseGame()
    {
        SetActive(PauseGameSet, true);
        Time.timeScale = 0f;
        audioManager?.PlayButtonClick();
    }

    public void ContinueGame()
    {
        SetActive(PauseGameSet, false);
        Time.timeScale = 1f;
        audioManager?.PlayButtonClick();
    }

    public void CountinueGame() => ContinueGame();

    public void HomeGame()
    {
        Time.timeScale = 1f;
        audioManager?.PlayButtonClick();
        SceneManager.LoadScene(MainMenuSceneName);
    }

    private void SetMainMenuVisible(bool isVisible)
    {
        SetActive(Logo, isVisible);
        SetActive(Buttons, isVisible);
    }

    private static void SetActive(GameObject target, bool isActive)
    {
        if (target != null)
        {
            target.SetActive(isActive);
        }
    }
}
