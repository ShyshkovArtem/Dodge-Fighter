using UnityEngine;
using UnityEngine.SceneManagement;   

public class MainMenu : MonoBehaviour
{

    public GameObject shopPopUp;
    public GameObject optionsPopUp;
    public GameObject Logo;
    public GameObject Buttons;
    public GameObject PauseGameSet;
    public AudioManager am;


    private void Awake()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    //MAIN MENU UI
    public void OnStartButtonPressed()
    {
        
        SceneManager.LoadScene("room1");
        am.PlaySFX(am.buttonClick);
    }
    public void OnOptionsButtonPressed()
    {
        // Show the options pop-up menu for future
        optionsPopUp.SetActive(true);
        Logo.SetActive(false);
        Buttons.SetActive(false);
        am.PlaySFX(am.buttonClick);
    }

    public void ClosePopup()
    {
        optionsPopUp.SetActive(false);
        Logo.SetActive(true);
        Buttons.SetActive(true);
        am.PlaySFX(am.buttonClick);
    }

    /* Method for the Shop Button
    public void OnShopButtonPressed()
    {
        // Show the shop pop-up menu for future
        shopPopUp.SetActive(true);
    }

    // Method for the Options Button
    

    
    */


    //IN GAME UI

    public void PauseGame()
    {
        PauseGameSet.SetActive(true);
        Time.timeScale = 0;
        am.PlaySFX(am.buttonClick);
    }

    public void CountinueGame() 
    { 
        PauseGameSet.SetActive(false);
        Time.timeScale = 1;
        am.PlaySFX(am.buttonClick);
    }
    public void HomeGame()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        am.PlaySFX(am.buttonClick);
    }
}
