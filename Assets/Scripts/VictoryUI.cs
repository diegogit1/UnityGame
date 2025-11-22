using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalScoreText; // Puntaje de la partida actual
    public TextMeshProUGUI bestScoreText;  // Mejor puntaje histórico

    [Header("Escenas")]
    public string mainMenuSceneName = "InicialMenu";
    public string gameSceneName = "Game";

    void Start()
    {
        int last = PlayerPrefs.GetInt("LastScore", 0);
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (finalScoreText != null)
            finalScoreText.text = $"Score: {last}";

        if (bestScoreText != null)
            bestScoreText.text = $"Best: {best}";
    }

    // Botón REINICIAR / SIGUIENTE
    public void NextOrRestart()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Botón VOLVER AL MENÚ
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}


