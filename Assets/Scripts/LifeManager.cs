using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Necesario para usar IEnumerator

public class LifeManager : MonoBehaviour
{
    public int vidas = 4; // Número inicial de vidas
    public Image[] corazones; // Arreglo de imágenes de corazones
    public Animator[] heartAnimators;
    private bool bolaMetida = false;
    private Rigidbody2D rb;

    void Start()
    {
        heartAnimators = new Animator[corazones.Length];
        for (int i = 0; i < corazones.Length; i++)
        {
            heartAnimators[i] = corazones[i].GetComponent<Animator>(); // Obtener el Animator de cada corazón
        }
        ActualizarVidasUI();
    }

    public void RegistrarBolaMetida(int bolaID)
    {
        bolaMetida = true;
        Debug.Log($"✅ Se metió la bola con ID: {bolaID}");
    }

    public void FinDeTurno(bool bolaBlancaMetida, Rigidbody2D whiteBall)
    {
        
    FrenarBolaBlanca(whiteBall); // Detener la bola blanca

    if (!bolaMetida || bolaBlancaMetida)
    {
        Debug.Log("❌ No se metió ninguna bola en este turno.");
        PerderVida();
    }

    if (bolaBlancaMetida){
        Debug.Log("Se metió la bola blanca!");
    }

    bolaMetida = false; // Reiniciar para el próximo turno
}

    public void PerderVida()
    {
        if (vidas > 0)
        {
            vidas--;
            Animator heartAnimator = heartAnimators[vidas];
        
        if (heartAnimator != null)
        {
            heartAnimator.Play("brokenHeart", 0, 0); // 🔹 Forzar la animación desde el inicio
            heartAnimator.SetBool("Romper", true); // 🔹 Desactivar cualquier posible loop
        }

        // Iniciar el Fade Out después de la animación
            StartCoroutine(FadeOutHeart(corazones[vidas]));
            
            if (vidas <= 0)
            {
                GameOver();
            }
        }
    }

    public void FrenarBolaBlanca(Rigidbody2D whiteBall)
    {
        whiteBall.velocity = Vector2.zero;
        whiteBall.angularVelocity = 0f;
    }


    IEnumerator FadeOutHeart(Image brokenHeart)
    {
        yield return new WaitForSeconds(0f); // Espera un poco después de la animación
        float duration = .3f;
        float elapsedTime = 0f;
        Color originalColor = brokenHeart.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            brokenHeart.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, elapsedTime / duration));
            yield return null;
        }

        brokenHeart.gameObject.SetActive(false); // Oculta completamente el corazón
    }

    void ActualizarVidasUI()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidas)
                corazones[i].enabled = true; // Mostrar corazón
            else
                corazones[i].enabled = false; // Ocultar corazón si la vida se ha perdido
        }
    }

    void GameOver()
    {
        Debug.Log("¡Game Over!");
        // Aquí puedes reiniciar el nivel, mostrar una pantalla de "Game Over", etc.
    }
}