using UnityEngine;
using UnityEngine.SceneManagement;   

public class MainMenu : MonoBehaviour
{

    public GameObject shopPopUp;
    public GameObject optionsPopUp;
    public GameObject Logo;
    public GameObject Buttons;

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("room1");
    }
    public void OnOptionsButtonPressed()
    {
        // Show the options pop-up menu for future
        optionsPopUp.SetActive(true);
        Logo.SetActive(false);
        Buttons.SetActive(false);
    }

    public void ClosePopup()
    {
        optionsPopUp.SetActive(false);
        Logo.SetActive(true);
        Buttons.SetActive(true);
    }

    /* Method for the Shop Button
    public void OnShopButtonPressed()
    {
        // Show the shop pop-up menu for future
        shopPopUp.SetActive(true);
    }

    // Method for the Options Button
    

    
    */
}
