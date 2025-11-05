using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorDebugTester : MonoBehaviour
{
    Animator animator;
    int hSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        hSpeed = Animator.StringToHash("Speed");

        Debug.Log("[DEBUG] AnimatorDebugTester started. Animator assigned: " + (animator != null));
        if (animator == null)
            Debug.LogError("[DEBUG] NO ANIMATOR encontrado en este GameObject.");
    }

    void Update()
    {
        // Muestra valor actual de Speed en consola (muy verboso; quítalo si spamea demasiado)
        float current = animator != null ? animator.GetFloat(hSpeed) : -999f;
        Debug.Log($"[DEBUG] Speed = {current:F3}");

        // Forzar Speed 0 / 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetFloat(hSpeed, 0f);
            Debug.Log("[DEBUG] Forzado Speed = 0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetFloat(hSpeed, 2f);
            Debug.Log("[DEBUG] Forzado Speed = 2");
        }

        // Forzar Play de estado Run / Idle
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Run");
            Debug.Log("[DEBUG] animator.Play(\"Run\") llamado");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.Play("Idle");
            Debug.Log("[DEBUG] animator.Play(\"Idle\") llamado");
        }
    }
}
