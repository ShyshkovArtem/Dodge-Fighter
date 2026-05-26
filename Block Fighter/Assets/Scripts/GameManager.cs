using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string MainMenuSceneName = "MainMenu";

    [SerializeField] private GameObject block;
    [SerializeField] private GameObject coin;
    [SerializeField] private Transform spawnPoint;
    [FormerlySerializedAs("spawmRate")]
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float minSpawnX = -8.1f;
    [SerializeField] private float maxSpawnX = 2.7f;

    [SerializeField] private GameObject tapText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinLVL_Text;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TextMeshProUGUI deathScoreText;
    [SerializeField] private TextMeshProUGUI deathEarnedCoinsText;

    public int coinLvl;

    private bool gameStarted;
    private bool gameOver;
    private int score;

    private void Start()
    {
        Time.timeScale = 1f;
        deathPanel ??= CreateDefaultDeathPanel();
        ResolveDeathPanelReferences();
        UpdateCoinLevelText();
        UpdateScoreText();

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameStarted && !gameOver)
        {
            StartSpawning();
            gameStarted = true;

            if (tapText != null)
            {
                tapText.SetActive(false);
            }
        }
    }

    public void AddLevelCoin()
    {
        if (gameOver)
        {
            return;
        }

        coinLvl++;
        UpdateCoinLevelText();
    }

    public void PlayerDied()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
        gameStarted = false;
        CancelInvoke(nameof(SpawnBlock));

        CoinManager.AddCoins(coinLvl);

        if (deathPanel != null)
        {
            ShowDeathPanel();
            Time.timeScale = 0f;
            return;
        }

        ReturnToMainMenu();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    private void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnBlock), 0.5f, spawnRate);
    }

    private void SpawnBlock()
    {
        if (gameOver)
        {
            return;
        }

        SpawnAtRandomX(block);

        score++;
        UpdateScoreText();

        SpawnAtRandomX(coin);
    }

    private void SpawnAtRandomX(GameObject prefab)
    {
        if (prefab == null || spawnPoint == null)
        {
            return;
        }

        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.x = Random.Range(minSpawnX, maxSpawnX);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void UpdateCoinLevelText()
    {
        if (coinLVL_Text != null)
        {
            coinLVL_Text.text = coinLvl.ToString();
        }
    }

    private void ShowDeathPanel()
    {
        deathPanel.SetActive(true);

        if (deathScoreText != null)
        {
            deathScoreText.text = "Your score: " + score;
        }

        if (deathEarnedCoinsText != null)
        {
            deathEarnedCoinsText.text = "You earned: " + coinLvl;
        }
    }

    private void ResolveDeathPanelReferences()
    {
        if (deathPanel == null)
        {
            return;
        }

        if (deathScoreText == null)
        {
            deathScoreText = deathPanel.transform.Find("ScoreValue")?.GetComponent<TextMeshProUGUI>();
        }

        if (deathEarnedCoinsText == null)
        {
            deathEarnedCoinsText = deathPanel.transform.Find("CoinsValue")?.GetComponent<TextMeshProUGUI>();
        }
    }

    private GameObject CreateDefaultDeathPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            return null;
        }

        GameObject panel = new GameObject("DeathPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(620f, 420f);
        panelRect.anchoredPosition = Vector2.zero;

        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = new Color(0.08f, 0.07f, 0.12f, 0.96f);

        CreatePanelText(panel.transform, "Title", "Game Over", 48f, new Vector2(0f, 145f));
        deathScoreText = CreatePanelText(panel.transform, "ScoreValue", "Your score: 0", 34f, new Vector2(0f, 65f));
        deathEarnedCoinsText = CreatePanelText(panel.transform, "CoinsValue", "You earned: 0", 34f, new Vector2(0f, 10f));

        CreatePanelButton(panel.transform, "RetryButton", "Try Again", new Vector2(-120f, -120f), RetryLevel);
        CreatePanelButton(panel.transform, "MainMenuButton", "Main Menu", new Vector2(120f, -120f), ReturnToMainMenu);

        panel.SetActive(false);
        return panel;
    }

    private static TextMeshProUGUI CreatePanelText(Transform parent, string name, string value, float size, Vector2 position)
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

    private static void CreatePanelButton(Transform parent, string name, string label, Vector2 position, UnityEngine.Events.UnityAction action)
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
        buttonObject.GetComponent<Button>().onClick.AddListener(action);
        CreatePanelText(buttonObject.transform, "Text", label, 24f, Vector2.zero);
    }
}
