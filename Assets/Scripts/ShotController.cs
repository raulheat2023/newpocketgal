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
    private float umbralMovimiento = 0.1f;
    private LifeManager lifeManager;
    private const float baseVolume = -10f;
    private const float maxVolume = 0f;
    private const float volumeReductionPerHit = -5f;
    private List<GameObject> trajectoryPoints = new List<GameObject>();

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





