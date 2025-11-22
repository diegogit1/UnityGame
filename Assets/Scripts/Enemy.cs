using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2.0f;
    public int damage = 1;

    [Header("Puntos")]
    [Tooltip("Puntos que da este enemigo al morir")]
    public int pointValue = 10;
    [Tooltip("Si usas pooling (reusar objetos) pon false y maneja la suma en tu sistema de muerte")]
    public bool awardPointsOnDestroy = true;

    private Rigidbody2D rb;
    private Transform player;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private readonly int hSpeed = Animator.StringToHash("Speed");
    private readonly int hMoveX = Animator.StringToHash("MoveX");
    private readonly int hMoveY = Animator.StringToHash("MoveY");

    [Header("Sonido")]
    public AudioClip deathSfx;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = (p != null) ? p.transform : null;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * speed;

        // Flip del sprite
        if (spriteRenderer != null)
            spriteRenderer.flipX = dir.x < 0;

        // Animación
        if (animator != null)
        {
            animator.SetFloat(hSpeed, rb.velocity.magnitude);
            animator.SetFloat(hMoveX, dir.x);
            animator.SetFloat(hMoveY, dir.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
                playerScript.TakeDamage(damage);
        }
    }

    // Si el enemigo es destruido (Destroy), sumamos puntos aquí.
    // Filtramos para no sumar nada cuando se sale del Play mode en el Editor.
    /*void OnDestroy()
    {
        if (!awardPointsOnDestroy) return;
        if (!Application.isPlaying) return;

        // SONIDO AL MORIR
        if (deathSfx != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSfx,
                Camera.main != null ? Camera.main.transform.position : transform.position
            );
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointValue);
        }
    }*/


    public void Die()
    {
        // Reproducir sonido
        if (deathSfx != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSfx,
                Camera.main != null ? Camera.main.transform.position : transform.position
            );
        }

        // Sumar puntos
        if (awardPointsOnDestroy && GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointValue);
        }

        // Destruir enemigo
        Destroy(gameObject);
    }

}




