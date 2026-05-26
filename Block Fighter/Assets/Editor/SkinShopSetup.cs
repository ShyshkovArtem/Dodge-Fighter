using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class SkinShopSetup
{
    private const string MainMenuScenePath = "Assets/Scenes/MainMenu.unity";
    private const string RoomScenePath = "Assets/Scenes/room1.unity";
    private const string ResourcesFolder = "Assets/Resources";
    private const string GeneratedFolder = "Assets/Generated";
    private const string SkinDatabasePath = ResourcesFolder + "/SkinDatabase.asset";
    private const string GeneratedAnimationsFolder = GeneratedFolder + "/SkinAnimations";

    [MenuItem("Tools/Block Fighter/Create Or Update Skin Shop")]
    public static void CreateOrUpdate()
    {
        EnsureFolder("Assets", "Resources");
        EnsureFolder("Assets", "Generated");
        EnsureFolder(GeneratedFolder, "SkinAnimations");

        SkinDatabase database = CreateOrUpdateSkinDatabase();
        CreateOrUpdateMainMenuPanel(database);
        CreateOrUpdateDeathPanel();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Skin shop setup complete.");
    }

    private static SkinDatabase CreateOrUpdateSkinDatabase()
    {
        SkinDatabase database = AssetDatabase.LoadAssetAtPath<SkinDatabase>(SkinDatabasePath);
        if (database == null)
        {
            database = ScriptableObject.CreateInstance<SkinDatabase>();
            AssetDatabase.CreateAsset(database, SkinDatabasePath);
        }

        SkinSource[] sources =
        {
            new SkinSource("virtual_guy", "Virtual Guy", 0, true, "Assets/Sprites/Idle (32x32).png", "Assets/Sprites/Run (32x32).png"),
            new SkinSource("mask_dude", "Mask Dude", 25, false, "Assets/Pixel Adventure 1/Assets/Main Characters/Mask Dude/Idle (32x32).png", "Assets/Pixel Adventure 1/Assets/Main Characters/Mask Dude/Run (32x32).png"),
            new SkinSource("ninja_frog", "Ninja Frog", 50, false, "Assets/Pixel Adventure 1/Assets/Main Characters/Ninja Frog/Idle (32x32).png", "Assets/Pixel Adventure 1/Assets/Main Characters/Ninja Frog/Run (32x32).png"),
            new SkinSource("pink_man", "Pink Man", 75, false, "Assets/Pixel Adventure 1/Assets/Main Characters/Pink Man/Idle (32x32).png", "Assets/Pixel Adventure 1/Assets/Main Characters/Pink Man/Run (32x32).png")
        };

        SerializedObject serializedDatabase = new SerializedObject(database);
        serializedDatabase.FindProperty("defaultSkinId").stringValue = "virtual_guy";

        SerializedProperty skinsProperty = serializedDatabase.FindProperty("skins");
        skinsProperty.arraySize = sources.Length;

        for (int i = 0; i < sources.Length; i++)
        {
            SkinSource source = sources[i];
            Sprite[] idleSprites = LoadSprites(source.IdlePath);
            Sprite[] runSprites = LoadSprites(source.RunPath);

            AnimationClip idleClip = CreateOrUpdateSpriteClip(source.Id + "_Idle", idleSprites);
            AnimationClip runClip = CreateOrUpdateSpriteClip(source.Id + "_Run", runSprites);

            SerializedProperty skinProperty = skinsProperty.GetArrayElementAtIndex(i);
            skinProperty.FindPropertyRelative("id").stringValue = source.Id;
            skinProperty.FindPropertyRelative("displayName").stringValue = source.DisplayName;
            skinProperty.FindPropertyRelative("price").intValue = source.Price;
            skinProperty.FindPropertyRelative("startsOwned").boolValue = source.StartsOwned;
            skinProperty.FindPropertyRelative("previewSprite").objectReferenceValue = idleSprites.FirstOrDefault();
            skinProperty.FindPropertyRelative("idleClip").objectReferenceValue = idleClip;
            skinProperty.FindPropertyRelative("runClip").objectReferenceValue = runClip;
        }

        serializedDatabase.ApplyModifiedProperties();
        EditorUtility.SetDirty(database);
        return database;
    }

    private static AnimationClip CreateOrUpdateSpriteClip(string clipName, Sprite[] sprites)
    {
        string path = GeneratedAnimationsFolder + "/" + clipName + ".anim";
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (clip == null)
        {
            clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, path);
        }

        clip.name = clipName;
        clip.frameRate = 12f;

        EditorCurveBinding binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = string.Empty,
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] frames = sprites
            .Select((sprite, index) => new ObjectReferenceKeyframe
            {
                time = index / clip.frameRate,
                value = sprite
            })
            .ToArray();

        AnimationUtility.SetObjectReferenceCurve(clip, binding, frames);

        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        EditorUtility.SetDirty(clip);
        return clip;
    }

    private static void CreateOrUpdateMainMenuPanel(SkinDatabase database)
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(MainMenuScenePath);
        Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        MainMenu mainMenu = UnityEngine.Object.FindObjectOfType<MainMenu>();

        if (canvas == null || mainMenu == null)
        {
            throw new InvalidOperationException("MainMenu scene must contain a Canvas and MainMenu component.");
        }

        GameObject panel = GameObject.Find("ShopPanel");
        if (panel == null)
        {
            panel = CreatePanel(canvas.transform);
        }

        SkinShopUI shopUi = panel.GetComponent<SkinShopUI>();
        if (shopUi == null)
        {
            shopUi = panel.AddComponent<SkinShopUI>();
        }

        TextMeshProUGUI coinsText = panel.transform.Find("CoinsText")?.GetComponent<TextMeshProUGUI>();
        Transform itemsRoot = panel.transform.Find("ItemsRoot");
        Button closeButton = panel.transform.Find("CloseButton")?.GetComponent<Button>();

        SerializedObject serializedShop = new SerializedObject(shopUi);
        serializedShop.FindProperty("skinDatabase").objectReferenceValue = database;
        serializedShop.FindProperty("coinsText").objectReferenceValue = coinsText;
        serializedShop.FindProperty("itemsRoot").objectReferenceValue = itemsRoot;
        serializedShop.FindProperty("closeButton").objectReferenceValue = closeButton;
        serializedShop.FindProperty("coinSprite").objectReferenceValue = LoadFirstSprite("Assets/Violet Theme Ui/GemsAndCoins/Coin Stack.png");
        serializedShop.FindProperty("itemSize").vector2Value = new Vector2(190f, 220f);
        serializedShop.FindProperty("previewSize").vector2Value = new Vector2(112f, 112f);
        serializedShop.FindProperty("priceIconSize").vector2Value = new Vector2(32f, 32f);
        serializedShop.FindProperty("nameFontSize").floatValue = 32f;
        serializedShop.FindProperty("actionFontSize").floatValue = 26f;
        serializedShop.FindProperty("textWidth").floatValue = 210f;
        serializedShop.FindProperty("priceRowWidth").floatValue = 210f;
        serializedShop.FindProperty("priceIconSpacing").floatValue = 4f;
        serializedShop.FindProperty("itemPadding").intValue = 12;
        serializedShop.FindProperty("itemSpacing").floatValue = 8f;
        serializedShop.FindProperty("normalItemColor").colorValue = new Color(0.15f, 0.12f, 0.22f, 0.92f);
        serializedShop.FindProperty("equippedItemColor").colorValue = new Color(0.42f, 0.34f, 0.12f, 0.96f);
        serializedShop.FindProperty("equippedBorderColor").colorValue = new Color(1f, 0.82f, 0.18f, 1f);
        serializedShop.FindProperty("equippedBorderSize").vector2Value = new Vector2(8f, 8f);
        serializedShop.ApplyModifiedProperties();

        if (closeButton != null)
        {
            SetButtonAction(closeButton, mainMenu.CloseShopPopup);
        }

        Button shopButton = GameObject.Find("btn_Shop")?.GetComponent<Button>();
        if (shopButton != null)
        {
            SetButtonAction(shopButton, mainMenu.OnShopButtonPressed);
        }

        SerializedObject serializedMainMenu = new SerializedObject(mainMenu);
        serializedMainMenu.FindProperty("shopPopUp").objectReferenceValue = panel;
        serializedMainMenu.ApplyModifiedProperties();

        panel.SetActive(false);
        EditorUtility.SetDirty(panel);
        EditorUtility.SetDirty(mainMenu);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static void CreateOrUpdateDeathPanel()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(RoomScenePath);
        Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        GameManager gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();

        if (canvas == null || gameManager == null)
        {
            throw new InvalidOperationException("room1 scene must contain a Canvas and GameManager component.");
        }

        GameObject panel = GameObject.Find("DeathPanel");
        if (panel == null)
        {
            panel = CreateDeathPanel(canvas.transform);
        }

        TextMeshProUGUI scoreText = panel.transform.Find("ScoreValue")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI earnedCoinsText = panel.transform.Find("CoinsValue")?.GetComponent<TextMeshProUGUI>();
        Button retryButton = panel.transform.Find("RetryButton")?.GetComponent<Button>();
        Button mainMenuButton = panel.transform.Find("MainMenuButton")?.GetComponent<Button>();

        if (retryButton != null)
        {
            SetButtonAction(retryButton, gameManager.RetryLevel);
        }

        if (mainMenuButton != null)
        {
            SetButtonAction(mainMenuButton, gameManager.ReturnToMainMenu);
        }

        SerializedObject serializedGameManager = new SerializedObject(gameManager);
        serializedGameManager.FindProperty("deathPanel").objectReferenceValue = panel;
        serializedGameManager.FindProperty("deathScoreText").objectReferenceValue = scoreText;
        serializedGameManager.FindProperty("deathEarnedCoinsText").objectReferenceValue = earnedCoinsText;
        serializedGameManager.ApplyModifiedProperties();

        panel.SetActive(false);
        EditorUtility.SetDirty(panel);
        EditorUtility.SetDirty(gameManager);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static GameObject CreatePanel(Transform parent)
    {
        GameObject panel = new GameObject("ShopPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(920f, 500f);
        rect.anchoredPosition = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.color = new Color(0.08f, 0.07f, 0.12f, 0.96f);

        CreateText(panel.transform, "Title", "Shop", 48f, new Vector2(0f, 205f));
        CreateText(panel.transform, "CoinsText", "0", 34f, new Vector2(0f, 150f));
        CreateItemsRoot(panel.transform);
        CreateCloseButton(panel.transform);
        return panel;
    }

    private static void CreateItemsRoot(Transform parent)
    {
        GameObject root = new GameObject("ItemsRoot", typeof(RectTransform), typeof(GridLayoutGroup));
        root.transform.SetParent(parent, false);

        RectTransform rect = root.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(850f, 230f);
        rect.anchoredPosition = new Vector2(0f, 0f);

        GridLayoutGroup grid = root.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(190f, 220f);
        grid.spacing = new Vector2(18f, 0f);
        grid.childAlignment = TextAnchor.MiddleCenter;
    }

    private static void CreateCloseButton(Transform parent)
    {
        GameObject buttonObject = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(160f, 52f);
        rect.anchoredPosition = new Vector2(0f, -215f);

        buttonObject.GetComponent<Image>().color = new Color(0.38f, 0.22f, 0.58f, 1f);
        CreateText(buttonObject.transform, "Text", "Close", 24f, Vector2.zero);
    }

    private static GameObject CreateDeathPanel(Transform parent)
    {
        GameObject panel = new GameObject("DeathPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(620f, 420f);
        rect.anchoredPosition = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.color = new Color(0.08f, 0.07f, 0.12f, 0.96f);

        CreateText(panel.transform, "Title", "Game Over", 48f, new Vector2(0f, 145f));
        CreateText(panel.transform, "ScoreValue", "Your score: 0", 34f, new Vector2(0f, 65f));
        CreateText(panel.transform, "CoinsValue", "You earned: 0", 34f, new Vector2(0f, 10f));
        CreateDeathButton(panel.transform, "RetryButton", "Try Again", new Vector2(-120f, -120f));
        CreateDeathButton(panel.transform, "MainMenuButton", "Main Menu", new Vector2(120f, -120f));

        return panel;
    }

    private static void CreateDeathButton(Transform parent, string name, string label, Vector2 position)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(190f, 58f);
        rect.anchoredPosition = position;

        buttonObject.GetComponent<Image>().color = new Color(0.38f, 0.22f, 0.58f, 1f);
        CreateText(buttonObject.transform, "Text", label, 24f, Vector2.zero);
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, string value, float size, Vector2 position)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(260f, 58f);
        rect.anchoredPosition = position;

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        return text;
    }

    private static void SetButtonAction(Button button, UnityAction action)
    {
        button.onClick = new Button.ButtonClickedEvent();
        UnityEventTools.AddPersistentListener(button.onClick, action);
        EditorUtility.SetDirty(button);
    }

    private static Sprite[] LoadSprites(string path)
    {
        return AssetDatabase.LoadAllAssetsAtPath(path)
            .OfType<Sprite>()
            .OrderBy(sprite => sprite.name, new SpriteNameComparer())
            .ToArray();
    }

    private static Sprite LoadFirstSprite(string path)
    {
        return LoadSprites(path).FirstOrDefault();
    }

    private static void EnsureFolder(string parent, string child)
    {
        string path = parent + "/" + child;
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private readonly struct SkinSource
    {
        public SkinSource(string id, string displayName, int price, bool startsOwned, string idlePath, string runPath)
        {
            Id = id;
            DisplayName = displayName;
            Price = price;
            StartsOwned = startsOwned;
            IdlePath = idlePath;
            RunPath = runPath;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public int Price { get; }
        public bool StartsOwned { get; }
        public string IdlePath { get; }
        public string RunPath { get; }
    }

    private sealed class SpriteNameComparer : System.Collections.Generic.IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return GetIndex(x).CompareTo(GetIndex(y));
        }

        private static int GetIndex(string name)
        {
            int underscore = name.LastIndexOf('_');
            return underscore >= 0 && int.TryParse(name.Substring(underscore + 1), out int index) ? index : 0;
        }
    }
}
