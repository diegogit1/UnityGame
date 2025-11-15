using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // <-- necesario

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scenes")]
    public string gameSceneName = "GameScene";       // NOMBRE de la escena de juego
    public string gameOverSceneName = "GameOver";
    public string victorySceneName = "Victory";

    [Header("Level / Timer")]
    public float levelDurationSeconds = 120f; // duración del nivel
    public bool autoStartTimer = true;

    [Header("Score")]
    public int currentScore = 0;
    public string lastScoreKey = "LastScore";
    public string bestScoreKey = "BestScore";

    [Header("Behaviour")]
    public bool saveLastScoreOnEnd = true;
    public bool saveBestScoreOnEnd = true;

    // Estado
    public bool IsPlaying { get; private set; } = false;
    public bool IsGameOver { get; private set; } = false;
    public bool IsVictory { get; private set; } = false;
    public float TimeRemaining { get; private set; }

    // Eventos para UI o otros sistemas
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnVictory;
    public UnityEvent OnTimerTick;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Nos suscribimos al evento de escena cargada para reiniciar cuando la escena de juego se cargue
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ResetState();
        if (autoStartTimer && SceneManager.GetActiveScene().name == gameSceneName)
            StartLevel();
    }

    // Se ejecuta cuando cualquier escena se carga
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si la escena cargada es la de juego, reiniciar estado y arrancar timer
        if (scene.name == gameSceneName)
        {
            ResetState();
            StartLevel();
        }
    }

    public void ResetState()
    {
        StopAllCoroutines();
        IsPlaying = false;
        IsGameOver = false;
        IsVictory = false;
        currentScore = 0;
        TimeRemaining = levelDurationSeconds;
    }

    public void StartLevel()
    {
        if (IsPlaying) return;
        IsPlaying = true;
        IsGameOver = false;
        IsVictory = false;
        if (TimeRemaining <= 0f) TimeRemaining = levelDurationSeconds;
        StartCoroutine(LevelTimerRoutine());
        OnGameStart?.Invoke();
    }

    IEnumerator LevelTimerRoutine()
    {
        while (TimeRemaining > 0f && !IsGameOver && !IsVictory)
        {
            yield return null;
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining < 0f) TimeRemaining = 0f;
            OnTimerTick?.Invoke();
        }

        if (!IsGameOver && !IsVictory)
        {
            Victory();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        // DEBUG opcional:
        // Debug.Log("Score: " + currentScore);
    }

    public void SaveScores()
    {
        if (saveLastScoreOnEnd)
        {
            PlayerPrefs.SetInt(lastScoreKey, currentScore);
        }

        if (saveBestScoreOnEnd)
        {
            int best = PlayerPrefs.GetInt(bestScoreKey, 0);
            if (currentScore > best)
            {
                PlayerPrefs.SetInt(bestScoreKey, currentScore);
            }
        }

        PlayerPrefs.Save();
    }

    public void GameOver()
    {
        if (IsGameOver || IsVictory) return;
        IsGameOver = true;
        IsPlaying = false;
        StopAllCoroutines();

        SaveScores();
        OnGameOver?.Invoke();

        if (SceneTransitionManagerAvailable())
            SceneTransitionManager.Instance.LoadScene(gameOverSceneName);
        else
            SceneManager.LoadScene(gameOverSceneName);
    }

    public void Victory()
    {
        if (IsVictory || IsGameOver) return;
        IsVictory = true;
        IsPlaying = false;
        StopAllCoroutines();

        // OPCIONAL: añadir bonus por ganar
        AddScore(100); // <--- suma los +100 que pediste antes

        SaveScores();
        OnVictory?.Invoke();

        if (SceneTransitionManagerAvailable())
            SceneTransitionManager.Instance.LoadScene(victorySceneName);
        else
            SceneManager.LoadScene(victorySceneName);
    }

    bool SceneTransitionManagerAvailable()
    {
        return (SceneTransitionManager.Instance != null);
    }

    // Método público para reiniciar el nivel (llamar desde UI / botones)
    public void RestartLevel()
    {
        // Usamos la transición si existe, la reinicialización la hará OnSceneLoaded
        if (SceneTransitionManagerAvailable())
            SceneTransitionManager.Instance.LoadScene(gameSceneName);
        else
            SceneManager.LoadScene(gameSceneName);
    }

    // utilidades
    public int GetCurrentScore() => currentScore;
    public int GetLastScore() => PlayerPrefs.GetInt(lastScoreKey, 0);
    public int GetBestScore() => PlayerPrefs.GetInt(bestScoreKey, 0);
}

