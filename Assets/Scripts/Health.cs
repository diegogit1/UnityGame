using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Eventos")]
    public UnityEvent OnDeath;
    public UnityEvent OnDamage;
    public UnityEvent OnHeal;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        OnDamage?.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHeal?.Invoke();
    }

    void Die()
    {
        // Si el objeto tiene componente Enemy, usamos su método Die()
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die();  // suma puntos, reproduce SFX y destruye el objeto de forma segura
        }
        else
        {
            // si no es un enemigo, destruimos normalmente
            Destroy(gameObject);
        }

        // Invocar eventos de muerte
        OnDeath?.Invoke();
    }

}


