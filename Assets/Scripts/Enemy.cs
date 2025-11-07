using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2.0f;
    public float flipThreshold = 0.1f; // umbral para decidir flip por X

    Rigidbody2D rb;
    Transform player;

    // visual & anim
    SpriteRenderer spriteRenderer;
    Animator animator;

    // hashes para animación
    readonly int hSpeed = Animator.StringToHash("Speed");
    readonly int hMoveX = Animator.StringToHash("MoveX");
    readonly int hMoveY = Animator.StringToHash("MoveY");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // recomendamos Kinematic para movimiento controlado por código
        // rb.bodyType = RigidbodyType2D.Kinematic; // descomenta si quieres kinematic
        rb.freezeRotation = true;

        // buscar components en este GO o hijos (más robusto)
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        player = (p != null) ? p.transform : null;

        // asegurarnos de que la visual esté visible al spawnear
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

        Vector2 dir = ((Vector2)player.position - rb.position);
        float dist = dir.magnitude;
        if (dist > 0.001f)
        {
            Vector2 dirNorm = dir / dist;

            // mover
            Vector2 targetPos = rb.position + dirNorm * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            // flip visual por X (no modificar transform.localScale)
            if (spriteRenderer != null)
            {
                if (dirNorm.x > flipThreshold) spriteRenderer.flipX = false;
                else if (dirNorm.x < -flipThreshold) spriteRenderer.flipX = true;
            }

            // actualizar animator (si existe) - usamos velocidad real y direccionalidad
            if (animator != null)
            {
                float speedValue = dirNorm.magnitude * speed; // será = speed, pero sirve si cambias lógica
                animator.SetFloat(hSpeed, speedValue);
                animator.SetFloat(hMoveX, dirNorm.x);
                animator.SetFloat(hMoveY, dirNorm.y);
            }
        }
        else
        {
            // si estamos prácticamente encima, dejamos Speed a 0
            if (animator != null)
            {
                animator.SetFloat(hSpeed, 0f);
            }
        }
    }
}


