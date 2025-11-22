using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float lifeTime = 4f;
    public bool homing = true; // si true ajusta dirección hacia target cada frame

    Transform target;
    Vector2 direction;
    float spawnTime;

    [Header("Sonido")]
    public AudioClip hitSfx;


    public void Init(Transform targetTransform, int damageAmount, float spd)
    {
        target = targetTransform;
        damage = damageAmount;
        speed = spd;
        spawnTime = Time.time;
        // dirección inicial hacia objetivo (si no hay objetivo, dispara en facing)
        if (target != null) direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
    }

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        // vida limitada
        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null)
        {
            // si se pierde target, avanzar en la última dirección
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            return;
        }

        if (homing)
        {
            // ajustar dirección hacia target y moverse
            Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
        }
        else
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var h = other.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(damage);
            }

            // SONIDO DE IMPACTO
            if (hitSfx != null)
            {
                AudioSource.PlayClipAtPoint(
                    hitSfx,
                    Camera.main != null ? Camera.main.transform.position : transform.position
                );
            }

            Destroy(gameObject);
        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                Destroy(gameObject);
            }
        }
    }

}

