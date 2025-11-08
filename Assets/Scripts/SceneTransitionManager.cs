using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade")]
    public Canvas fadeCanvas;
    public Image fadeImage; // imagen negra que cubrirá la pantalla
    public float fadeDuration = 0.6f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Si no está asignado, intentar buscar en hijos
        if (fadeCanvas == null)
            fadeCanvas = GetComponentInChildren<Canvas>();
        if (fadeImage == null && fadeCanvas != null)
            fadeImage = fadeCanvas.GetComponentInChildren<Image>();

        // asegurarse de estado inicial (visible en la primera carga)
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
            fadeImage.color = new Color(0f, 0f, 0f, 1f);
            StartCoroutine(Fade(1f, 0f)); // fade-in al arrancar
        }
    }

    /// <summary>Fade coroutine helper:  from alphaFrom to alphaTo</summary>
    IEnumerator Fade(float alphaFrom, float alphaTo)
    {
        if (fadeImage == null) yield break;
        float t = 0f;
        float dur = Mathf.Max(0.001f, fadeDuration);
        Color c = fadeImage.color;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(alphaFrom, alphaTo, t / dur);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }
        c.a = alphaTo;
        fadeImage.color = c;
    }

    /// <summary>Carga escena con fade out -> load async -> fade in</summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        // fade out
        if (fadeImage != null) yield return StartCoroutine(Fade(fadeImage.color.a, 1f));
        // cargar async
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;
        // fade in
        if (fadeImage != null) yield return StartCoroutine(Fade(1f, 0f));
    }

    // utilidades
    public void LoadSceneImmediate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

