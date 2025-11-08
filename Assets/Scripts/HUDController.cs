using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI livesText;      // "Vidas: X"
    public TextMeshProUGUI timeText;       // "Tiempo: mm:ss"
    public TextMeshProUGUI scoreText;      // opcional si quieres mostrar score

    [Header("Referencia jugador (opcional)")]
    public PlayerMovement player; // si lo dejas vacío buscará por tag "Player"

    void Start()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.GetComponent<PlayerMovement>();
        }

        // inicial refresh
        RefreshAll();
    }

    void Update()
    {
        RefreshAll();
    }

    void RefreshAll()
    {
        UpdateLives();
        UpdateTime();
        // UpdateScore() si lo implementas
    }

    void UpdateLives()
    {
        if (livesText == null) return;

        int cur = 0;
        int max = 0;
        if (player != null)
        {
            // usamos campos públicos en PlayerMovement (asegúrate de que currentHealth es accesible)
            cur = player.currentHealth;
            max = player.maxHealth;
        }
        livesText.text = $"HP: {cur}/{max}";
    }

    void UpdateTime()
    {
        if (timeText == null || GameManager.Instance == null) return;
        float t = GameManager.Instance.TimeRemaining;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

