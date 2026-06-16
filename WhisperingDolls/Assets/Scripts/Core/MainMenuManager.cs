using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string gameSceneName = "FloodedGroundsGameScene";

    [Header("Panels")]
    [SerializeField] GameObject lorePanel;
    [SerializeField] GameObject creditsPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowLore()
    {
        if (lorePanel != null) lorePanel.SetActive(true);
    }

    public void HideLore()
    {
        if (lorePanel != null) lorePanel.SetActive(false);
    }

    public void ShowCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
