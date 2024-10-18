using UnityEngine;
using UnityEngine.SceneManagement;   

public class MainMenu : MonoBehaviour
{

    public GameObject shopPopUp;
    public GameObject optionsPopUp;

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("room1");
    }

    /* Method for the Shop Button
    public void OnShopButtonPressed()
    {
        // Show the shop pop-up menu for future
        shopPopUp.SetActive(true);
    }

    // Method for the Options Button
    public void OnOptionsButtonPressed()
    {
        // Show the options pop-up menu for future
        optionsPopUp.SetActive(true);
    }

    // Method to hide the pop-up panels (if you want close buttons)
    public void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
    }*/
}
