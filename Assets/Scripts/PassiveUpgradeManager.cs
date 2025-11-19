using UnityEngine;
using TMPro; // Import necesario para TextMeshPro
using UnityEngine.UI;
using System.Collections;

public class PassiveUpgradeManager : MonoBehaviour
{
    [Header("Jugador")]
    public PlayerMovement player;
    public int puntosParaMejora = 10; // cada X puntos se da una mejora

    [Header("UI Mejora")]
    public GameObject panelMejora; // Panel en canvas
    public Image iconoMejora;
    public TextMeshProUGUI nombreMejora;
    public TextMeshProUGUI descripcionMejora;
    public float tiempoMostrar = 2f; // segundos que se muestra

    [Header("Iconos de mejoras")]
    public Sprite iconoDaño;
    public Sprite iconoVelocidad;
    public Sprite iconoVida;

    [Header("Sonido")]
    public AudioClip sonidoMejora; // clip que se reproducirá al mostrar la mejora

    private int ultimoUmbral = 0;

    void Update()
    {
        if (GameManager.Instance == null) return;

        int score = GameManager.Instance.GetCurrentScore();

        if (score / puntosParaMejora > ultimoUmbral)
        {
            ultimoUmbral = score / puntosParaMejora;
            AplicarMejoraAleatoria();
        }
    }

    void AplicarMejoraAleatoria()
    {
        int opcion = Random.Range(0, 3); // 0=daño, 1=velocidad, 2=vida

        string nombre = "";
        string descripcion = "";
        Sprite icono = null;

        switch (opcion)
        {
            case 0: // daño
                AutoAttack aa = player.GetComponent<AutoAttack>();
                if (aa != null) aa.damage += 1;

                nombre = "Más daño";
                descripcion = "Tu ataque hace +1 de daño";
                icono = iconoDaño;
                break;

            case 1: // velocidad
                player.moveSpeed += 0.5f;

                nombre = "Más velocidad";
                descripcion = "Tu velocidad aumenta";
                icono = iconoVelocidad;
                break;

            case 2: // vida
                player.currentHealth += 1;

                nombre = "Más vida";
                descripcion = "Sumas 1 punto de vida extra";
                icono = iconoVida;
                break;
        }

        MostrarUI(nombre, descripcion, icono);
    }

    void MostrarUI(string nombre, string descripcion, Sprite icono)
    {
        if (panelMejora == null) return;

        // Pausar el juego
        Time.timeScale = 0f;

        panelMejora.SetActive(true);
        if (iconoMejora != null) iconoMejora.sprite = icono;
        if (nombreMejora != null) nombreMejora.text = nombre;
        if (descripcionMejora != null) descripcionMejora.text = descripcion;

        // Reproducir sonido de aparición de mejora
        if (sonidoMejora != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMejora, Camera.main.transform.position);
        }

        StartCoroutine(EsconderPanel());
    }

    IEnumerator EsconderPanel()
    {
        // Nota: WaitForSeconds usa tiempo normal, así que para Time.timeScale=0 necesitamos usar WaitForSecondsRealtime
        yield return new WaitForSecondsRealtime(tiempoMostrar);
        panelMejora.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;
    }
}



