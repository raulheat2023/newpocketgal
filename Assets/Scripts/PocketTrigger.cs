using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PocketTrigger : MonoBehaviour
{
    public RectTransform barraBolas;
    public GameObject bolaPrefab;
    public GameTimer gameTimer;

    public Animator Isumi_01;
    
    public AudioClip soundBolaNormal; 
    public AudioClip soundBolaBlanca;
    private AudioSource audioSource;

    private List<string> bolasInsertadas = new List<string>();  
    private ScoreManager scoreManager;

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();  // Buscar el temporizador en la escena
        scoreManager = FindObjectOfType<ScoreManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string bolaNombre = collision.gameObject.name;
        // Verifica si el objeto tiene el tag "Bolas"
        if (collision.CompareTag("Bolas"))
        {
            Debug.Log($"✅ ¡La bola {collision.gameObject.name} ha entrado en la buchaca!");
            Debug.Log("Bola ingresó en la buchaca: " + bolaNombre);
            Isumi_01.SetTrigger("Sonrie");

            BallIdentifier ball = collision.GetComponent<BallIdentifier>(); 
            if (ball != null)
            {
                Debug.Log($"🎯 Bola ID: {ball.ballID} registrada.");
                FindObjectOfType<LifeManager>().RegistrarBolaMetida(ball.ballID);
            }
             gameTimer.AgregarTiempo(10f);
             Debug.Log("Bola embocada: " + collision.gameObject.name + " (+10s)");

             RegistrarBolaIngresada(bolaNombre);

             scoreManager.SumarPuntos(100);
             scoreManager.RegistrarBolaIngresadaJson(collision.gameObject.name);
             Debug.Log("Puntaje actualizado: " + scoreManager.ObtenerPuntaje());
             
             audioSource.PlayOneShot(soundBolaNormal);
        }

        if (collision.CompareTag("BolaBlanca"))
        { 
            Debug.Log("¡Bola blanca embocada! No se suma tiempo.");
            audioSource.PlayOneShot(soundBolaBlanca);
        }
    }

    private void RegistrarBolaIngresada(string bolaNombre)
    {
        if (!bolasInsertadas.Contains(bolaNombre))
        {
            Debug.Log("Registrando bola ingresada: " + bolaNombre);
            bolasInsertadas.Add(bolaNombre);
            MostrarBolaEnBarra(bolaNombre);
        }
    }

    private void MostrarBolaEnBarra(string bolaNombre)
    {
        GameObject nuevaBola = Instantiate(bolaPrefab, barraBolas);
        Sprite bolaSprite = Resources.Load<Sprite>("Sprites/Bolas/" + bolaNombre);

        if (bolaSprite != null)
        {
            nuevaBola.GetComponent<Image>().sprite = bolaSprite;
            Debug.Log("Bola añadida a la barra: " + bolaNombre);
        }
        else
        {
            Debug.LogWarning("No se encontró el sprite para: " + bolaNombre);
        }
    }
}
