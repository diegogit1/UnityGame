using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2.0f;
    public int damage = 1;

    private Rigidbody2D rb;
    private Transform player;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private readonly int hSpeed = Animator.StringToHash("Speed");
    private readonly int hMoveX = Animator.StringToHash("MoveX");
    private readonly int hMoveY = Animator.StringToHash("MoveY");

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
}



