using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))] // Or CharacterController if 3D
public class EnemyAnimatorScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        // Update Blend Tree parameter
        animator.SetFloat("Speed", speed);

        // Optional: Add logic to control other states like "morir" or "Disparar" here
    }
}
