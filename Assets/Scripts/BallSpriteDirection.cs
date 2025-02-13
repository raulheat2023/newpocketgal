using UnityEngine;

public class BallSpriteDirection : MonoBehaviour
{
    public Sprite[] sprites; // 🔹 Arrastra aquí los sprites en el Inspector
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0.05f) // 🔹 Solo cambia si hay movimiento real
        {
            UpdateSpriteBasedOnDirection();
        }
    }

    void UpdateSpriteBasedOnDirection()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg; // Convertir dirección a grados
        angle = (angle + 360) % 360; // Asegurar que el ángulo esté entre 0 y 360

        int index = Mathf.FloorToInt(angle / (360f / sprites.Length)) % sprites.Length; // Determinar índice del sprite
        spriteRenderer.sprite = sprites[index]; // Asignar sprite correspondiente
    }
}
