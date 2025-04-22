using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Al inicio, se hace un fade in
        StartCoroutine(FadeIn());
    }

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        for (float t = 1f; t >= 0; t -= Time.deltaTime / fadeDuration)
        {
            color.a = t;
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        Color color = fadeImage.color;
        for (float t = 0f; t <= 1f; t += Time.deltaTime / fadeDuration)
        {
            color.a = t;
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAfterDelay(string targetScene, float delay)
    {
        StartCoroutine(WaitAndLoad(targetScene, delay));
    }

    private IEnumerator WaitAndLoad(string targetScene, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutAndLoad(targetScene));
    }
}
