using TMPro;
using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private const string AllCoinsKey = "allCoins";

    public static int coinAll;
    public static event Action CoinsChanged;

    [SerializeField] private TextMeshProUGUI coinALL_Text;

    private void OnEnable()
    {
        CoinsChanged += RefreshDisplay;
    }

    private void OnDisable()
    {
        CoinsChanged -= RefreshDisplay;
    }

    private void Start()
    {
        coinAll = PlayerPrefs.GetInt(AllCoinsKey, coinAll);
        PlayerPrefs.SetInt(AllCoinsKey, coinAll);
        RefreshDisplay();
    }

    public static void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        coinAll += amount;
        PlayerPrefs.SetInt(AllCoinsKey, coinAll);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke();
    }

    public static bool TrySpendCoins(int amount)
    {
        coinAll = PlayerPrefs.GetInt(AllCoinsKey, coinAll);

        if (amount < 0 || coinAll < amount)
        {
            return false;
        }

        coinAll -= amount;
        PlayerPrefs.SetInt(AllCoinsKey, coinAll);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke();
        return true;
    }

    public static int GetCoins()
    {
        coinAll = PlayerPrefs.GetInt(AllCoinsKey, coinAll);
        return coinAll;
    }

    private void RefreshDisplay()
    {
        if (coinALL_Text != null)
        {
            coinALL_Text.text = coinAll.ToString();
        }
    }
}
