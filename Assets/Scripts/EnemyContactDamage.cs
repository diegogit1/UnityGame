using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Daño por contacto")]
    public int contactDamage = 1;
    [Tooltip("Tiempo mínimo entre impactos al mismo objetivo (s).")]
    public float damageCooldown = 1.0f;

    // guarda el último tiempo que se dañó al jugador por este enemigo
    float lastDamageTime = -999f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // si el jugador se queda en contacto, permitir daño periódico
        TryDamage(collision.collider);
    }

    void TryDamage(Collider2D other)
    {
        if (other.CompareTag("Player") == false) return;

        if (Time.time - lastDamageTime < damageCooldown) return;

        Health h = other.GetComponent<Health>();
        if (h == null) return;

        h.TakeDamage(contactDamage);
        lastDamageTime = Time.time;
    }
}

