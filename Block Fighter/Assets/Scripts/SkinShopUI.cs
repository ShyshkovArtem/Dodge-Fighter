using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : MonoBehaviour
{
    [SerializeField] private SkinDatabase skinDatabase;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Transform itemsRoot;
    [SerializeField] private Button closeButton;
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Vector2 itemSize = new Vector2(190f, 220f);
    [SerializeField] private Vector2 previewSize = new Vector2(112f, 112f);
    [SerializeField] private Vector2 priceIconSize = new Vector2(32f, 32f);
    [SerializeField] private float nameFontSize = 32f;
    [SerializeField] private float actionFontSize = 26f;
    [SerializeField] private float textWidth = 210f;
    [SerializeField] private float priceRowWidth = 210f;
    [SerializeField] private float priceIconSpacing = 4f;
    [SerializeField] private int itemPadding = 12;
    [SerializeField] private float itemSpacing = 8f;
    [SerializeField] private Color normalItemColor = new Color(0.15f, 0.12f, 0.22f, 0.92f);
    [SerializeField] private Color equippedItemColor = new Color(0.42f, 0.34f, 0.12f, 0.96f);
    [SerializeField] private Color equippedBorderColor = new Color(1f, 0.82f, 0.18f, 1f);
    [SerializeField] private Vector2 equippedBorderSize = new Vector2(8f, 8f);

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
    }

    private void OnEnable()
    {
        BuildShop();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void BuildShop()
    {
        if (skinDatabase == null || itemsRoot == null)
        {
            return;
        }

        ClearItems();
        RefreshCoins();

        foreach (SkinDefinition skin in skinDatabase.Skins)
        {
            if (skin != null)
            {
                CreateSkinButton(skin);
            }
        }
    }

    private void CreateSkinButton(SkinDefinition skin)
    {
        GameObject item = new GameObject(skin.DisplayName, typeof(RectTransform), typeof(Image), typeof(Button));
        item.transform.SetParent(itemsRoot, false);

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.sizeDelta = itemSize;

        Image background = item.GetComponent<Image>();
        bool isEquipped = PlayerSkinService.SelectedSkinId == skin.Id;
        background.color = isEquipped ? equippedItemColor : normalItemColor;

        Button button = item.GetComponent<Button>();
        button.onClick.AddListener(() => BuyOrEquip(skin));

        if (isEquipped)
        {
            Outline outline = item.AddComponent<Outline>();
            outline.effectColor = equippedBorderColor;
            outline.effectDistance = equippedBorderSize;
        }

        VerticalLayoutGroup layout = item.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(itemPadding, itemPadding, itemPadding, itemPadding);
        layout.spacing = itemSpacing;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        CreatePreview(item.transform, skin);
        CreateText(item.transform, skin.DisplayName, nameFontSize, textWidth);

        if (PlayerSkinService.SelectedSkinId == skin.Id)
        {
            return;
        }

        if (PlayerSkinService.IsOwned(skin))
        {
            CreateText(item.transform, "Bought", actionFontSize, textWidth);
        }
        else
        {
            CreatePriceRow(item.transform, skin.Price);
        }
    }

    private void BuyOrEquip(SkinDefinition skin)
    {
        if (PlayerSkinService.IsOwned(skin))
        {
            PlayerSkinService.Equip(skin);
        }
        else if (PlayerSkinService.Buy(skin))
        {
            PlayerSkinService.Equip(skin);
        }

        BuildShop();
    }

    private void CreatePreview(Transform parent, SkinDefinition skin)
    {
        GameObject preview = new GameObject("Preview", typeof(RectTransform), typeof(Image));
        preview.transform.SetParent(parent, false);

        Image image = preview.GetComponent<Image>();
        image.sprite = skin.PreviewSprite;
        image.preserveAspect = true;

        RectTransform rectTransform = preview.GetComponent<RectTransform>();
        rectTransform.sizeDelta = previewSize;
    }

    private void CreatePriceRow(Transform parent, int price)
    {
        GameObject row = new GameObject("Price", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        row.transform.SetParent(parent, false);

        RectTransform rowTransform = row.GetComponent<RectTransform>();
        rowTransform.sizeDelta = new Vector2(priceRowWidth, actionFontSize + 14f);

        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = priceIconSpacing;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        CreateText(row.transform, price.ToString(), actionFontSize, textWidth * 0.35f);

        if (coinSprite != null)
        {
            GameObject iconObject = new GameObject("Coin", typeof(RectTransform), typeof(Image));
            iconObject.transform.SetParent(row.transform, false);

            RectTransform iconTransform = iconObject.GetComponent<RectTransform>();
            iconTransform.sizeDelta = priceIconSize;

            Image icon = iconObject.GetComponent<Image>();
            icon.sprite = coinSprite;
            icon.preserveAspect = true;
        }
    }

    private static void CreateText(Transform parent, string value, float size, float width)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(parent, false);

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, size + 12f);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
    }

    private void ClearItems()
    {
        for (int i = itemsRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(itemsRoot.GetChild(i).gameObject);
        }
    }

    private void RefreshCoins()
    {
        if (coinsText != null)
        {
            coinsText.text = CoinManager.GetCoins().ToString();
        }
    }
}
