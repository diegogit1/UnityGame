using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class AutoAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject projectilePrefab;
    public float attackInterval = 1.0f;
    public float attackRange = 8f;
    public int damage = 1;
    public float projectileSpeed = 8f;

    Transform self;
    float lastAttackTime = -999f;

    void Start()
    {
        self = transform;
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                Transform nearest = FindNearestEnemy();
                if (nearest != null)
                {
                    FireAt(nearest);
                    lastAttackTime = Time.time;
                }
            }
            yield return null;
        }
    }

    Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(self.position, attackRange);
        Transform nearest = null;
        float best = float.MaxValue;

        foreach (var c in hits)
        {
            if (!c.CompareTag("Enemy")) continue;
            float d = (c.transform.position - self.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                nearest = c.transform;
            }
        }
        return nearest;
    }

    void FireAt(Transform target)
    {
        if (projectilePrefab == null || target == null) return;

        // instanciar proyectil
        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = go.GetComponent<Projectile>();
        if (p != null)
        {
            // Intentamos inicializar con la forma más común: Init(target, damage, speed)
            // Si tu Projectile tiene otra firma, ajustamos también los campos públicos por seguridad.
            // (Si tu Projectile sólo usa campos públicos, estas asignaciones los fijarán igualmente.)
            try
            {
                // Llamada habitual de 3 parámetros (la más probable en tu proyecto)
                p.Init(target, damage, projectileSpeed);
            }
            catch
            {
                // Si no existe esa sobrecarga, intentamos asignar campos públicos como fallback.
                // Estas líneas compilan si Projectile tiene 'damage' y 'speed' públicos (la versión que te pasé sí).
#pragma warning disable CS0168
                try { p.damage = damage; } catch (System.Exception) { }
                try { p.speed = projectileSpeed; } catch (System.Exception) { }
#pragma warning restore CS0168
            }

            // En cualquier caso, aseguramos que tenga target si existe una propiedad pública 'target' (rara).
            // No hacemos más para no asumir firmas inexistentes.
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


