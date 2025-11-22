using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public int value = 10;
    public AudioClip pickupSfx;    // asigna en prefab

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null) GameManager.Instance.AddScore(value);

        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, Camera.main != null ? Camera.main.transform.position : transform.position);


        Destroy(gameObject);
    }
}


