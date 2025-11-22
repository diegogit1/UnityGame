using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class HUDController : MonoBehaviour
{
    [Header("UI refs")]
    public TextMeshProUGUI livesText;    // arrastra TMP lives
    public TextMeshProUGUI timeText;     // arrastra TMP time
    public TextMeshProUGUI scoreText;    // arrastra TMP score (opcional)

    [Header("Player reference (opcional)")]
    public PlayerMovement player; // si no asignas, buscará por tag "Player" en Start()

    void Start()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        UpdateLives();
        UpdateTime();
        UpdateScore();
    }

    void UpdateLives()
    {
        if (livesText == null) return;
        if (player != null)
            livesText.text = $"HP: {player.currentHealth}";
        else
            livesText.text = "HP: ?";
    }

    void UpdateTime()
    {
        if (timeText == null) return;
        if (GameManager.Instance == null) { timeText.text = "--:--"; return; }
        float t = GameManager.Instance.TimeRemaining;
        int mm = Mathf.FloorToInt(t / 60f);
        int ss = Mathf.FloorToInt(t % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", mm, ss);
    }

    void UpdateScore()
    {
        if (scoreText == null) return;
        if (GameManager.Instance != null)
            scoreText.text = $"Score: {GameManager.Instance.GetCurrentScore()}";
    }
}


