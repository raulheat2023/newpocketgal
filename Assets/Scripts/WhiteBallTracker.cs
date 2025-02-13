using UnityEngine;

public class WhiteBallTracker : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool esperandoTurno = false;
    private float umbralVelocidad = 0.05f; // Define qué tan lento debe ser el movimiento para considerarse "detenido"

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Si la bola blanca se está moviendo, activamos la espera del turno
        if (rb.velocity.magnitude > umbralVelocidad)
        {
            esperandoTurno = true;
        }
        // Si ya estaba en movimiento y ahora se ha detenido, se termina el turno
        else if (esperandoTurno && rb.velocity.magnitude <= umbralVelocidad)
        {
            esperandoTurno = false; // Resetear para el siguiente turno
            FindObjectOfType<LifeManager>().FinDeTurno(false, rb); // Llamar a FinDeTurno cuando la bola se detiene
        }
    }
}
