using UnityEngine;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
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

    public void NextOrRestart()
    {
        SceneTransitionManager.Instance.LoadScene(gameSceneName);
    }

    public void MainMenu()
    {
        SceneTransitionManager.Instance.LoadScene(mainMenuSceneName);
    }
}
