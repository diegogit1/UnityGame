using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Sin gravedad (estilo Vampire Survivors)
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Leer input de movimiento
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Actualizar parámetro "Speed" (se usa para pasar de Idle a Run)
        float speedValue = moveInput.sqrMagnitude;
        animator.SetFloat("Speed", speedValue);

        // Voltear sprite según dirección horizontal
        if (moveInput.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        // Movimiento sin aceleración ni gravedad
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}


