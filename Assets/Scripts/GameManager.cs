using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Escenas")]
    public string gameOverSceneName = "GameOver";
    public string victorySceneName = "Victory";

    [Header("Nivel / Tiempo")]
    public float levelTimeSeconds = 60f; // duración del nivel en segundos

    [Header("Opciones")]
    public bool autoStartTimer = true; // si true comienza el timer al Start()

    public bool IsGameOver { get; private set; }
    public bool IsVictory { get; private set; }
    public float TimeRemaining { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetState();
        if (autoStartTimer) StartLevelTimer();
    }

    public void ResetState()
    {
        IsGameOver = false;
        IsVictory = false;
        TimeRemaining = levelTimeSeconds;
    }

    public void StartLevelTimer()
    {
        StopAllCoroutines();
        StartCoroutine(LevelTimerCoroutine());
    }

    IEnumerator LevelTimerCoroutine()
    {
        // simple loop decrementando el tiempo
        while (TimeRemaining > 0f)
        {
            yield return null;
            if (IsGameOver) yield break; // si se ha muerto, paramos
            TimeRemaining -= Time.deltaTime;
        }

        // tiempo acabado -> victory
        TimeRemaining = 0f;
        OnVictory();
    }

    public void GameOver()
    {
        if (IsGameOver || IsVictory) return;
        IsGameOver = true;
        StopAllCoroutines();
        // puedes hacer efectos, reproducir sonido, anim, etc.
        // Cargar escena de GameOver (si la tienes)
        if (!string.IsNullOrEmpty(gameOverSceneName))
            StartCoroutine(LoadSceneDelayed(gameOverSceneName, 0.8f));
    }

    public void OnVictory()
    {
        if (IsGameOver || IsVictory) return;
        IsVictory = true;
        StopAllCoroutines();
        if (!string.IsNullOrEmpty(victorySceneName))
            StartCoroutine(LoadSceneDelayed(victorySceneName, 0.8f));
    }

    IEnumerator LoadSceneDelayed(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
