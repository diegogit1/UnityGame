using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject instructionsPanel;
    public GameObject settingsPanel;

    [Header("Build")]
    public string gameSceneName = "GameScene"; // pon el nombre exacto de tu escena de juego

    void Start()
    {
        ShowMain();
    }

    // Botones públicos para conectar en el Inspector
    public void OnPlayPressed()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnInstructionsPressed()
    {
        ShowInstructions();
    }

    public void OnSettingsPressed()
    {
        ShowSettings();
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnBackToMenuPressed()
    {
        ShowMain();
    }

    // mostrar paneles (simple)
    void ShowMain()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    void ShowInstructions()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
    }

    void ShowSettings()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
}

