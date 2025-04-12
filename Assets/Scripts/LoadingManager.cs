using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // Asigna el Slider desde el Inspector
    public float tiempoMinimo = 2f; // Tiempo mínimo de carga para simular un efecto de transición
    public TextMeshProUGUI progressText; 
    public Image progressImage; 
    private static string escenaDestino; 
    public Image fadeImage;
    public float fadeDuration = 1f;

    public static void CargarEscena(string nombreEscena)
    {
        escenaDestino = nombreEscena;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(CargarEscenaAsync());
    }

    IEnumerator CargarEscenaAsync()
    {
        yield return new WaitForSeconds(0.5f); // Pequeño delay inicial para efecto visual

        AsyncOperation operation = SceneManager.LoadSceneAsync(escenaDestino);
        operation.allowSceneActivation = false; // Evita que la escena se active automáticamente

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normaliza el progreso (0 - 1)

            // Actualiza el Slider
            if (progressBar != null)
                progressBar.value = progress;

            // Actualiza el Texto de porcentaje
            if (progressText != null)
                progressText.text = (progress * 100).ToString("0") + "%";

            // Actualiza el Sprite (Si tienes una barra de imagen que se llena)
            if (progressImage != null)
                progressImage.fillAmount = progress;

            // Cuando el progreso llega al 90%, espera a que el jugador presione una tecla para avanzar
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(FadeOut());
                operation.allowSceneActivation = true;
            }
        }
    }
    IEnumerator FadeOut()
{
    float elapsed = 0f;
    Color color = fadeImage.color;

    while (elapsed < fadeDuration)
    {
        elapsed += Time.deltaTime;
        color.a = Mathf.Clamp01(elapsed / fadeDuration);
        fadeImage.color = color;
        yield return null;
    }
}
}
