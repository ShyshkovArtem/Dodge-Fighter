using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Block Fighter/Skin Database")]
public class SkinDatabase : ScriptableObject
{
    [SerializeField] private string defaultSkinId = "virtual_guy";
    [SerializeField] private SkinDefinition[] skins;

    public string DefaultSkinId => defaultSkinId;
    public SkinDefinition[] Skins => skins;

    public SkinDefinition GetSkin(string id)
    {
        if (skins == null || string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return Array.Find(skins, skin => skin != null && skin.Id == id);
    }
}

[Serializable]
public class SkinDefinition
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private int price;
    [SerializeField] private bool startsOwned;
    [SerializeField] private Sprite previewSprite;
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip runClip;

    public string Id => id;
    public string DisplayName => displayName;
    public int Price => price;
    public bool StartsOwned => startsOwned;
    public Sprite PreviewSprite => previewSprite;
    public AnimationClip IdleClip => idleClip;
    public AnimationClip RunClip => runClip;
}
