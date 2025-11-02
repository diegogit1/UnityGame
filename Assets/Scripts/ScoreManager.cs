using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static void SaveScore(int score)
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public static int GetHighScore() => PlayerPrefs.GetInt("HighScore", 0);
}

