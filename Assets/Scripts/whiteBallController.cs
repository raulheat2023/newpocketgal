using UnityEngine;
using UnityEngine.UI;

public class whiteBallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;

    public float fuerzaMultiplicadora = 5f;
    public float fuerzaMaxima = 10f;

    private Vector2 initialPosition; // Para almacenar la posición inicial de la bola

    public Image barraFuerza; // 🔹 Referencia a la barra de fuerza en la UI
    private float fuerzaActual = 0f;

    private LifeManager lifeManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        barraFuerza.fillAmount = 0; // Inicialmente vacía
        initialPosition = transform.position; // Guardamos la posición inicial de la bola
        lifeManager = FindObjectOfType<LifeManager>(); // Buscar el LifeManager en la escena
    }

    void Update()
    {
        // Detectar el click y empezar a arrastrar
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Usar la cámara principal directamente
            if (Vector2.Distance(mousePosition, transform.position) < 0.5f)
            {
                isDragging = true;
                startPoint = mousePosition;
                fuerzaActual = 0; // Reiniciar fuerza
            }
        }

        // Mientras el click esté presionado, aumentar la fuerza
        if (Input.GetMouseButton(0) && isDragging)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fuerzaActual += Time.deltaTime * 20f;
            fuerzaActual = Mathf.Clamp(fuerzaActual, 0, fuerzaMaxima);

            barraFuerza.fillAmount = fuerzaActual / fuerzaMaxima; // Actualiza la barra de fuerza
        }

        // Al soltar el click, aplicar la fuerza a la bola
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Vector2 direction = startPoint - endPoint;
            float fuerza = Mathf.Clamp(fuerzaActual * fuerzaMultiplicadora, 0, fuerzaMaxima);

            rb.velocity = direction.normalized * fuerza;
            barraFuerza.fillAmount = 0; // Reiniciar la barra después del disparo
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Buchaca")) // Asegúrate de que las buchacas tengan este tag
        {
            lifeManager.PerderVida(); // Quitar una vida
            ResetBallPosition();
        }
    }

     private void ResetBallPosition()
    {
        rb.velocity = Vector2.zero; // Detener la bola
        rb.angularVelocity = 0; // Detener la rotación
        transform.position = initialPosition; // Volver a la posición inicial
    }
}
