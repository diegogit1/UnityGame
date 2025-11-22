using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI")]
    public Slider volumeSlider;
    public Button normalButton;
    public Button hardButton;
    public Image dificultadIcono;   // arrastra el Image del panel
    public Sprite iconoNormal;      // sprite para Normal
    public Sprite iconoDificil;     // sprite para Difícil


    private const string VOLUME_KEY = "GameVolume";
    private const string DIFFICULTY_KEY = "GameDifficulty"; // 0=Normal, 1=Difícil

    void Start()
    {
        // Cargar valores guardados
        float vol = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        int diff = PlayerPrefs.GetInt(DIFFICULTY_KEY, 0);

        if (volumeSlider != null) volumeSlider.value = vol;
        ApplyVolume(vol);

        ApplyDifficulty(diff);

        // Listeners
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener((v) => { ApplyVolume(v); SaveVolume(v); });

        if (normalButton != null)
            normalButton.onClick.AddListener(() => { ApplyDifficulty(0); SaveDifficulty(0); });

        if (hardButton != null)
            hardButton.onClick.AddListener(() => { ApplyDifficulty(1); SaveDifficulty(1); });
    }

    void ApplyVolume(float v)
    {
        AudioListener.volume = v; // simple y efectivo
    }

    void SaveVolume(float v)
    {
        PlayerPrefs.SetFloat(VOLUME_KEY, v);
        PlayerPrefs.Save();
    }

    void ApplyDifficulty(int diff)
    {
        // Actualiza el icono
        if (dificultadIcono != null)
        {
            dificultadIcono.sprite = (diff == 0) ? iconoNormal : iconoDificil;
        }

        // Ajusta spawners
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (var s in spawners)
        {
            s.SetDifficulty(diff == 1); // 0=Normal, 1=Difícil
        }
    }


    void SaveDifficulty(int diff)
    {
        PlayerPrefs.SetInt(DIFFICULTY_KEY, diff);
        PlayerPrefs.Save();
    }
}


