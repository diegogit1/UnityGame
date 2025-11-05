using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    Animator animator;
    readonly int hSpeed = Animator.StringToHash("Speed");
    readonly int hMoveX = Animator.StringToHash("MoveX");
    readonly int hMoveY = Animator.StringToHash("MoveY");

    // suavizado ligero para evitar saltos en Blend Trees
    public float directionSmooth = 12f;
    public float speedSmooth = 8f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator no encontrado.");
    }

    // Llamar cada frame: moveInput = Vector2(-1..1), baseSpeed = speed
    public void UpdateMovementParams(Vector2 moveInput, float baseSpeed)
    {
        float targetSpeed = moveInput.magnitude * baseSpeed;

        float curSpeed = animator.GetFloat(hSpeed);
        float smoothSpeed = Mathf.Lerp(curSpeed, targetSpeed, Time.deltaTime * speedSmooth);
        animator.SetFloat(hSpeed, smoothSpeed);

        float curX = animator.GetFloat(hMoveX);
        float curY = animator.GetFloat(hMoveY);
        float smoothX = Mathf.Lerp(curX, moveInput.x, Time.deltaTime * directionSmooth);
        float smoothY = Mathf.Lerp(curY, moveInput.y, Time.deltaTime * directionSmooth);
        animator.SetFloat(hMoveX, smoothX);
        animator.SetFloat(hMoveY, smoothY);
    }
}

