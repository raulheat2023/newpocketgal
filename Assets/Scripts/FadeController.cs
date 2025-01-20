using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para usar IEnumerator

public class FadeController : MonoBehaviour
{
    public Image fadeImage;  // Imagen para el efecto de fade
    public float fadeDuration = 1f;  // Duración del fade

    private void Start()
    {
        // Comienza con un fade in (desvanecer desde negro a transparente)
        StartCoroutine(FadeIn());
    }

    public void LoadNextScene(string sceneName)
    {
        // Inicia el fade out y cambia de escena
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Asegura que el fade termine completamente
        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Carga la siguiente escena después del fade
        SceneManager.LoadScene(sceneName);
    }
}
