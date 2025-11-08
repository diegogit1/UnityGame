using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // arrastra aquí el Text TMP de puntuación
    public string mainMenuSceneName = "MainMenu";
    public string gameSceneName = "Game";

    void Start()
    {
        if (finalScoreText != null)
        {
            int last = PlayerPrefs.GetInt("LastScore", 0);
            finalScoreText.text = $"Score: {last}";
        }
    }

    public void Restart()
    {
        SceneTransitionManager.Instance.LoadScene(gameSceneName);
    }

    public void MainMenu()
    {
        SceneTransitionManager.Instance.LoadScene(mainMenuSceneName);
    }
}

