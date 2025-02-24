using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class WhiteBallAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float maxSpeed = 5f; // 🔹 Velocidad máxima para la animación
    public float minSpeedMultiplier = 0.2f; // 🔹 Velocidad mínima de animación

    private LifeManager lifeManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;
        float animationSpeed = Mathf.Lerp(minSpeedMultiplier, 1f, speed / maxSpeed);
        animator.speed = (speed < 0.1f) ? 0 : animationSpeed;

    // 🔹 Rotar la bola según su dirección de movimiento
        if (speed > 0.1f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}