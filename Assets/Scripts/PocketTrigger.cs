using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PocketTrigger : MonoBehaviour
{
    public RectTransform barraBolas;
    public GameObject bolaPrefab;
    public GameTimer gameTimer;

    private List<string> bolasInsertadas = new List<string>();  

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();  // Buscar el temporizador en la escena
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto tiene el tag "Bolas"
        if (collision.CompareTag("Bolas"))
        {
            Debug.Log($"✅ ¡La bola {collision.gameObject.name} ha entrado en la buchaca!");
            string bolaNombre = collision.gameObject.name;
            Debug.Log("Bola ingresó en la buchaca: " + bolaNombre);

            BallIdentifier ball = collision.GetComponent<BallIdentifier>(); 
            if (ball != null)
            {
                Debug.Log($"🎯 Bola ID: {ball.ballID} registrada.");
                FindObjectOfType<LifeManager>().RegistrarBolaMetida(ball.ballID);
            }

             RegistrarBolaIngresada(bolaNombre);
        }


        if (collision.CompareTag("Bolas"))
        {
            gameTimer.AgregarTiempo(10f);
            Debug.Log("Bola embocada: " + collision.gameObject.name + " (+10s)");
        }

        if (collision.CompareTag("BolaBlanca"))
        {
            Debug.Log("¡Bola blanca embocada! No se suma tiempo.");
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
