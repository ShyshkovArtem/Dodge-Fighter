using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coinAll;
    public TextMeshProUGUI coinALL_Text;

    private void Update()
    {
        coinALL_Text.text = coinAll.ToString();

    }
}

