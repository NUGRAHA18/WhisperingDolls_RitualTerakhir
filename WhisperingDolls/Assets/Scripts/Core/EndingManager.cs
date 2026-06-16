using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    [SerializeField] string mainMenuSceneName = "MainMenu";

    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject goodEndingPanel;

    [Header("Ending Text")]
    [SerializeField] TMP_Text gameOverBodyText;
    [SerializeField] TMP_Text goodEndingBodyText;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ShowGameOver()
    {
        GameManager.Instance?.SetState(GameManager.GameState.GameOver);

        if (gameOverBodyText != null)
            gameOverBodyText.text =
                "Kamu tertangkap...\n\nKegelapan menyelimuti segalanya.\nBisikan-bisikan itu semakin keras.";

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void ShowGoodEnding()
    {
        GameManager.Instance?.SetState(GameManager.GameState.GameOver);

        bool hasDollGood = InventoryManager.Instance.CountOfType(ItemType.DollGood) > 0;
        string endingText = hasDollGood
            ? "Kutukan Kalimutu... akhirnya berakhir.\n\nBoneka-boneka baik itu ikut menemanimu pergi — mereka pun bebas.\n\n★ ENDING TERBAIK ★"
            : "Kutukan Kalimutu... akhirnya berakhir.\n\nKamu selamat, tapi tidak semua roh bisa diselamatkan.\n\n✦ ENDING NORMAL ✦";

        if (goodEndingBodyText != null)
            goodEndingBodyText.text = endingText;

        if (goodEndingPanel != null) goodEndingPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance?.SetState(GameManager.GameState.Playing);
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartGame()
    {
        GameManager.Instance?.SetState(GameManager.GameState.Playing);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
