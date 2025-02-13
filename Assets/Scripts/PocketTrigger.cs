using UnityEngine;

public class PocketTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto tiene el tag "Bolas"
        if (other.CompareTag("Bolas"))
        {
            Debug.Log($"✅ ¡La bola {other.gameObject.name} ha entrado en la buchaca!");

            BallIdentifier ball = other.GetComponent<BallIdentifier>(); 
            if (ball != null)
            {
                Debug.Log($"🎯 Bola ID: {ball.ballID} registrada.");
                FindObjectOfType<LifeManager>().RegistrarBolaMetida(ball.ballID);
            }
        }

        if (other.GetComponent<whiteBall>())
        {
            FindObjectOfType<LifeManager>().FinDeTurno(true, other.GetComponent<Rigidbody2D>());
        }

    }
}
