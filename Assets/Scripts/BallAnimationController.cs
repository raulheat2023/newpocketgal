using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class BallAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float maxSpeed = 5f; // 🔹 Velocidad máxima para la animación
    public float minSpeedMultiplier = 0.2f; // 🔹 Velocidad mínima de animación
    public float shrinkDuration = 0.1f;
     public AudioClip soundBallCollision;  // Sonido de colisión con otras bolas
    public AudioClip soundWallCollision;  // Sonido de colisión con los bordes
    private AudioSource audioSource;

    public AudioMixer audioMixer;  // Asignar el Audio Mixer en el Inspector
    private static int activeCollisions = 0;  // Contador de colisiones activas
    private const float baseVolume = -10f; // Volumen base del sonido
    private const float maxVolume = 0f; // Volumen máximo permitido
    private const float volumeReductionPerHit = -5f; // Reducción por cada sonido activo

    private LifeManager lifeManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
    {
        Debug.LogError("No se encontró AudioSource en " + gameObject.name);
    }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Buchaca")) // Asegúrate de que la buchaca tenga el tag correcto
    {
        rb.velocity = Vector2.zero; // 🔹 Detener la bola
        rb.angularVelocity = 0f;    // 🔹 Detener la rotación

        StartCoroutine(ShrinkAndDisappear());
    }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Borde")) // Si choca con los bordes
        {
            PlaySound(soundWallCollision, collision.relativeVelocity.magnitude);
        }
        else if (collision.gameObject.CompareTag("Bolas")) // Si choca con otra bola
        {
            PlaySound(soundBallCollision, collision.relativeVelocity.magnitude);
        }
    }

    void PlaySound(AudioClip clip, float impactForce)
    {
        if (clip != null)
        {
            audioSource.volume = Mathf.Clamp(impactForce / 10f, 0.1f, 1f); // Ajusta el volumen según la fuerza del golpe
            audioSource.PlayOneShot(clip);
        }
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

public void OnCollisionSoundStart()
    {
        activeCollisions++;
        AdjustVolume();
    }

    public void OnCollisionSoundEnd()
    {
        activeCollisions = Mathf.Max(0, activeCollisions - 1);
        AdjustVolume();
    }

    private void AdjustVolume()
    {
        float newVolume = Mathf.Clamp(baseVolume + (activeCollisions * volumeReductionPerHit), -40f, maxVolume);
        audioMixer.SetFloat("CollisionVolume", newVolume);
    }

IEnumerator ShrinkAndDisappear()
{
    float elapsedTime = 0;
    Vector3 initialScale = transform.localScale;
    Color initialColor = spriteRenderer.color;

    while (elapsedTime < shrinkDuration)
    {
        float scaleAmount = Mathf.Lerp(1, 0, elapsedTime / shrinkDuration);
        transform.localScale = initialScale * scaleAmount;

        // Reducir la opacidad de la bola
        Color newColor = initialColor;
        newColor.a = Mathf.Lerp(1, 0, elapsedTime / shrinkDuration);
        spriteRenderer.color = newColor;

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    Destroy(gameObject);
}
}