using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coinAll;
    public TextMeshProUGUI coinALL_Text;

    private void Start()
    {
        if (PlayerPrefs.HasKey("allCoins"))
        {
            coinAll = PlayerPrefs.GetInt("allCoins");
        }
        else
        {
            PlayerPrefs.SetInt("allCoins", coinAll);
        }
    }
    private void Update()
    {
        coinAll = PlayerPrefs.GetInt("allCoins");
        coinALL_Text.text = coinAll.ToString();
    }

   
}

