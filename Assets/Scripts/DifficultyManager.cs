using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Normal, Hard }
    public Difficulty currentDifficulty = Difficulty.Normal;

    [Header("UI Sprites")]
    public Sprite normalSprite;
    public Sprite hardSprite;
    public UnityEngine.UI.Image difficultyDisplay;

    [Header("Spawn Controller")]
    public EnemySpawner spawnController; // tu sistema de spawn

    public void SetNormal()
    {
        currentDifficulty = Difficulty.Normal;
        UpdateUI();
        spawnController.SetDifficulty(false); // false = normal
    }

    public void SetHard()
    {
        currentDifficulty = Difficulty.Hard;
        UpdateUI();
        spawnController.SetDifficulty(true); // true = difícil
    }

    void UpdateUI()
    {
        if (difficultyDisplay != null)
        {
            difficultyDisplay.sprite = (currentDifficulty == Difficulty.Normal) ? normalSprite : hardSprite;
        }
    }
}

