using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;

public class ShotController : MonoBehaviour
{
    [Header("UI & Animations")]
    public Slider barraFuerza;
    public Animator HandleAnimator;
    
    [Header("Fuerza del Disparo")]
    public float fuerzaMinima = 5f;
    public float fuerzaMaxima = 25f;
    public float velocidadOscilacion = 2f;
    
    [Header("Audio")]
    public AudioClip soundBallCollision;
    public AudioClip soundWallCollision;
    public AudioSource apuntadoAudio;
    private AudioSource audioSource;
    public AudioMixer audioMixer;
    public AudioClip soundShot;
    public AudioSource shotAudioSource;
    
    [Header("Configuración de Movimiento")]
    public int maxBounces = 3;
    public LayerMask collisionLayer;
    public float lineLength = .2f;
    public GameObject trajectoryPointPrefab;
    public int trajectoryPointCount = 10;
    
    private Rigidbody2D rb;
    private float fuerzaActual;
    private bool cargandoFuerza = false;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;
    private Vector2 initialPosition;
    private LifeManager lifeManager;
    private const float baseVolume = -10f;
    private const float maxVolume = 0f;
    private const float volumeReductionPerHit = -5f;
    private List<GameObject> trajectoryPoints = new List<GameObject>();

    [Header("Botones de efecto")]
    public Button botonRetroceso;
    public Button botonCorrido;
    public Button botonIzquierda;
    public Button botonDerecha;
    public Color colorSeleccionado = Color.green;
    public Color colorNormal = Color.white;

    private float velocidadRotacion;

    [Header("Animaciones de Efecto")]
    public Animator animator;
    public RuntimeAnimatorController retrocesoAnim;
    public RuntimeAnimatorController corridoAnim;
    public RuntimeAnimatorController izquierdaAnim;
    public RuntimeAnimatorController derechaAnim;
    public RuntimeAnimatorController normalAnim;

    // efectos
    private enum Efecto
    {
        Ninguno,
        Retroceso,
        Corrido,
        Izquierda,
        Derecha
    }
    private Efecto efectoSeleccionado = Efecto.Ninguno;

    public void SeleccionarEfecto(string efecto)
    {
        switch (efecto)
        {
            case "Retroceso":
                efectoSeleccionado = Efecto.Retroceso;
                animator.runtimeAnimatorController = retrocesoAnim;
                Debug.Log("efecto retroceso seleccionado");
                break;
            case "Corrido":
                efectoSeleccionado = Efecto.Corrido;
                animator.runtimeAnimatorController = corridoAnim;
                Debug.Log("efecto Corrido seleccionado");
                break;
            case "Izquierda":
                efectoSeleccionado = Efecto.Izquierda;
                animator.runtimeAnimatorController = izquierdaAnim;
                Debug.Log("efecto Izquierda seleccionado");
                break;
            case "Derecha":
                efectoSeleccionado = Efecto.Derecha;
                animator.runtimeAnimatorController = derechaAnim;
                Debug.Log("efecto Derecha seleccionado");
                break;
            default:
                efectoSeleccionado = Efecto.Ninguno;
                animator.runtimeAnimatorController = normalAnim;
                Debug.Log("efecto Centro seleccionado");
                break;
        }

        ActualizarColoresBotones();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        lifeManager = FindObjectOfType<LifeManager>();
        CreateTrajectoryPoints();
        audioSource = GetComponent<AudioSource>();
        MostrarNumeros();
    }

    void Update()
    {
        DetectarEntrada();
        ActualizarCargaFuerza();
        if (rb.velocity.magnitude > 0.001)
        {
            OcultarNumeros();
        }
        else
        {
            MostrarNumeros();
        }

        // Obtener la velocidad de rotación y de desplazamiento
        float velocidadRotacion = Mathf.Abs(rb.angularVelocity) / 100f; // Normalizamos la rotación
        float velocidadMovimiento = rb.velocity.magnitude / 5f; // Normalizamos la velocidad lineal

        // Combinamos ambas velocidades para la animación
        float velocidadAnimacion = velocidadRotacion + velocidadMovimiento;

        // Limitamos la velocidad de la animación para evitar valores extremos
        animator.speed = Mathf.Clamp(velocidadAnimacion, 0.1f, 3f);
    }

    private bool TodasLasBolasDetenidas()
    {
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bolas"); // Encuentra todas las bolas
        foreach (GameObject bola in bolas)
        {
            Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
            if (rbBola != null && rbBola.velocity.magnitude > 0.1f) // Si alguna bola aún se mueve, retorna false
            {
                return false;
            }
        }
        return true; // Todas las bolas están detenidas
    }

    private void DetectarEntrada()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0) && Vector2.Distance(mousePosition, transform.position) < 0.5f && TodasLasBolasDetenidas())
        {
            IniciarApuntado(mousePosition);
        }
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            endPoint = mousePosition;
            ActualizarTrayectoria();
        }
        
        if (Input.GetMouseButtonUp(0) && cargandoFuerza)
        {
            EjecutarDisparo();
        }
    }

    private void IniciarApuntado(Vector2 mousePosition)
    {
        if (rb.velocity.magnitude == 0 && TodasLasBolasDetenidas())
        {
            isDragging = true;
            startPoint = mousePosition;
            cargandoFuerza = true;
            HandleAnimator.SetBool("Cargando", true);
            if (!apuntadoAudio.isPlaying) apuntadoAudio.Play();     
            MostrarTrayectoria();
        }
    }

    private void ActualizarCargaFuerza()
    {
        if (cargandoFuerza)
        {
            barraFuerza.value = Mathf.PingPong(Time.time * velocidadOscilacion, 1);
        }
    }

    private void EjecutarDisparo()
    {
        cargandoFuerza = false;
        isDragging = false;
        HandleAnimator.SetBool("Cargando", false);
        apuntadoAudio.Stop();
        OcultarTrayectoria();
        switch (efectoSeleccionado)
        {
            case Efecto.Retroceso:
                StartCoroutine(AplicarEfectoRetroceso());
                break;
            case Efecto.Corrido:
                StartCoroutine(AplicarEfectoCorrido());
                break;
            case Efecto.Izquierda:
                StartCoroutine(AplicarEfectoIzquierda());
                break;
            case Efecto.Derecha:
                StartCoroutine(AplicarEfectoDerecha());
                break;
        }

        fuerzaActual = Mathf.Lerp(fuerzaMinima, fuerzaMaxima, barraFuerza.value);
        Vector2 direccionDisparo = (startPoint - endPoint).normalized;
        rb.AddForce(direccionDisparo * fuerzaActual, ForceMode2D.Impulse);

        barraFuerza.value = 0;
        if (soundShot != null && shotAudioSource != null)
        {
            shotAudioSource.PlayOneShot(soundShot);
        }    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Buchaca"))
        {
            lifeManager.PerderVida();
            ResetBallPosition();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Borde"))
        {
            PlaySound(soundWallCollision, collision.relativeVelocity.magnitude);
        }
        else if (collision.gameObject.CompareTag("Bolas"))
        {
            PlaySound(soundBallCollision, collision.relativeVelocity.magnitude);
        }
    }

    void PlaySound(AudioClip clip, float impactForce)
    {
        if (clip != null)
        {
            apuntadoAudio.volume = 1;
            apuntadoAudio.PlayOneShot(clip);
        }
    }

    private void ResetBallPosition()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = initialPosition;
    }

    private void CreateTrajectoryPoints()
    {
        for (int i = 0; i < trajectoryPointCount; i++)
        {
            GameObject point = Instantiate(trajectoryPointPrefab);
            point.SetActive(false);
            trajectoryPoints.Add(point);
        }
    }

    private void MostrarTrayectoria()
    {
        foreach (GameObject point in trajectoryPoints)
        {
            point.SetActive(true);
        }
    }

    private void OcultarTrayectoria()
    {
        foreach (GameObject point in trajectoryPoints)
        {
            point.SetActive(false);
        }
    }

    private void MostrarNumeros()
    {
        GameObject[] numeros = GameObject.FindGameObjectsWithTag("BolasNumber");
        foreach (GameObject numero in numeros)
        {
            CanvasGroup canvasGroup = numero.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0.9f;
            }
        }
    }

    private void OcultarNumeros()
    {
        GameObject[] numeros = GameObject.FindGameObjectsWithTag("BolasNumber");
        foreach (GameObject numero in numeros)
        {
            CanvasGroup canvasGroup = numero.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
            
        }

    }

    private IEnumerator AplicarEfectoRetroceso()
    {
        yield return new WaitForSeconds(0.001f); // Espera un poco antes de aplicar el efecto
        rb.AddForce(-rb.velocity.normalized * (fuerzaActual / 2), ForceMode2D.Impulse);
        Debug.Log("Efecto de retroceso aplicado");
    }

    private IEnumerator AplicarEfectoCorrido()
    {
        yield return new WaitForSeconds(0.001f);
        rb.AddForce(rb.velocity.normalized * (fuerzaActual / 2), ForceMode2D.Impulse);
        Debug.Log("Efecto de Corrido aplicado");
    }

    private IEnumerator AplicarEfectoDerecha()
    {
        yield return new WaitForSeconds(0.001f);
        rb.AddTorque(-2f, ForceMode2D.Impulse); // Simula giro hacia la derecha
        Debug.Log("efecto Derecha aplicado");
    }

    private IEnumerator AplicarEfectoIzquierda()
    {
        yield return new WaitForSeconds(0.001f);
        rb.AddTorque(2f, ForceMode2D.Impulse); // Simula giro hacia la izquierda
        Debug.Log("efecto Izquierda aplicado");
    }

    private void ActualizarColoresBotones()
    {
        botonRetroceso.image.color = (efectoSeleccionado == Efecto.Retroceso) ? colorSeleccionado : colorNormal;
        botonCorrido.image.color = (efectoSeleccionado == Efecto.Corrido) ? colorSeleccionado : colorNormal;
        botonIzquierda.image.color = (efectoSeleccionado == Efecto.Izquierda) ? colorSeleccionado : colorNormal;
        botonDerecha.image.color = (efectoSeleccionado == Efecto.Derecha) ? colorSeleccionado : colorNormal;
    }

    private void ActualizarTrayectoria()
    {
        Vector2 direccionDisparo = (startPoint - endPoint).normalized;
        float fuerzaSimulada = Mathf.Lerp(fuerzaMinima, fuerzaMaxima, barraFuerza.value);
        Vector2 posicionInicial = transform.position;
        Vector2 velocidad = direccionDisparo * fuerzaSimulada;

        for (int i = 0; i < trajectoryPoints.Count; i++)
        {
            float t = i * 0.005f;
            RaycastHit2D hit = Physics2D.Raycast(posicionInicial, velocidad.normalized, velocidad.magnitude * t, collisionLayer);
            
            if (hit.collider != null)
            {
                velocidad = Vector2.Reflect(velocidad, hit.normal);
                posicionInicial = hit.point + hit.normal * 0.001f; 
            }
            else
            {
                posicionInicial += velocidad * t;
            }
            
            trajectoryPoints[i].transform.position = posicionInicial;
            trajectoryPoints[i].SetActive(true);
        }
    }
}





