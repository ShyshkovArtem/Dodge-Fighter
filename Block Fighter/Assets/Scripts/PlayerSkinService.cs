using UnityEngine;

public static class PlayerSkinService
{
    private const string SelectedSkinKey = "selectedSkin";
    private const string OwnedSkinPrefix = "ownedSkin_";
    private const string DefaultSkinId = "virtual_guy";

    public static string SelectedSkinId => PlayerPrefs.GetString(SelectedSkinKey, DefaultSkinId);

    public static bool IsOwned(SkinDefinition skin)
    {
        if (skin == null)
        {
            return false;
        }

        return skin.StartsOwned || PlayerPrefs.GetInt(OwnedSkinPrefix + skin.Id, 0) == 1;
    }

    public static bool Buy(SkinDefinition skin)
    {
        if (skin == null || IsOwned(skin))
        {
            return false;
        }

        if (!CoinManager.TrySpendCoins(skin.Price))
        {
            return false;
        }

        PlayerPrefs.SetInt(OwnedSkinPrefix + skin.Id, 1);
        PlayerPrefs.Save();
        return true;
    }

    public static void Equip(SkinDefinition skin)
    {
        if (skin == null || !IsOwned(skin))
        {
            return;
        }

        PlayerPrefs.SetString(SelectedSkinKey, skin.Id);
        PlayerPrefs.Save();
    }
}
