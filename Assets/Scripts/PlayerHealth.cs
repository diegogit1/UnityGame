using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;
    public int score = 0;
    public Text livesText;
    public Text scoreText;

    void Start()
    {
        UpdateHUD();
    }

    public void TakeDamage(int amount)
    {
        lives -= amount;
        UpdateHUD();

        if (lives <= 0)
            SceneManager.LoadScene("DeathScene");
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        livesText.text = $"Vidas: {lives}";
        scoreText.text = $"Puntos: {score}";
    }
}

