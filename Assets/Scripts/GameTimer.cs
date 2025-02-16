using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;  // Texto de la UI para mostrar el tiempo
    public float tiempoRestante = 120f;  // 2 minutos en segundos
    private bool juegoEnCurso = false;

    void Start()
    {
        ActualizarTiempoUI();
        IniciarTemporizador();
    }

    void Update()
    {
        if (juegoEnCurso && tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarTiempoUI();
        }
        else if (tiempoRestante <= 1)
        {
            FinDelTiempo();
        }
    }

    void ActualizarTiempoUI()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void IniciarTemporizador()
    {
        juegoEnCurso = true;
    }

    public void AgregarTiempo(float segundos)
    {
        tiempoRestante += segundos;
        ActualizarTiempoUI();
    }

    private void FinDelTiempo()
    {
        juegoEnCurso = false;
        timerText.text = "TIME'S UP!";
        Debug.Log("¡Tiempo agotado! Fin del juego.");
        // Aquí puedes agregar lógica para finalizar la partida
    }
}
